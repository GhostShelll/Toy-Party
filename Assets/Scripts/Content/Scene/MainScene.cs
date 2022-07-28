using UnityEngine;

using com.jbg.asset;
using com.jbg.asset.control;
using com.jbg.asset.data;
using com.jbg.content.popup;
using com.jbg.content.scene.view;
using com.jbg.core;
using com.jbg.core.manager;
using com.jbg.core.scene;

namespace com.jbg.content.scene
{
    public class MainScene : SceneEx
    {
        private MainView sceneView;

        public enum STATE
        {
            CheckAsset,
            DownloadAsset,
            WaitDone,
        }

        protected override void OnOpen()
        {
            base.OnOpen();

            this.sceneView = (MainView)this.SceneView;

            this.sceneView.BindEvent(MainView.Event.LottoSelect, this.OnClickLottoSelect);
            this.sceneView.BindEvent(MainView.Event.RefreshAsset, this.OnClickRefreshAsset);
            this.sceneView.BindEvent(MainView.Event.ChangeLanguage, this.OnClickChangeLanguage);

            MainView.Params p = new();
            p.lottoBtnTxt = LocaleControl.GetString(LocaleCodes.MAIN_SCENE_LOTTO_BTN_TEXT);
            p.checkAssetTxt = LocaleControl.GetString(LocaleCodes.MAIN_SCENE_ASSET_CHECKING_TEXT);
            p.downloadAssetTxt = LocaleControl.GetString(LocaleCodes.MAIN_SCENE_ASSET_LOADING_TEXT);
            p.refreshBtnTxt = LocaleControl.GetString(LocaleCodes.MAIN_SCENE_ASSET_LOADING_BTN_TEXT);
            p.languagesList = new() { "한국어", "English" };

            this.sceneView.OnOpen(p);
            this.sceneView.SetLanguageState((int)LocaleControl.LanguageCode);

            this.SetStateCheckAsset();
        }

        protected override void OnClose()
        {
            base.OnClose();

            this.sceneView.RemoveEvent(MainView.Event.LottoSelect);
            this.sceneView.RemoveEvent(MainView.Event.RefreshAsset);
            this.sceneView.RemoveEvent(MainView.Event.ChangeLanguage);
        }

        private void SetStateCheckAsset()
        {
            this.SetState((int)STATE.CheckAsset);

            this.sceneView.SetStateCheckAsset();

            // 에셋 체크 시작
            Coroutine task = CoroutineManager.AddTask(TableVersionControl.CheckAsset());

            this.AddUpdateFunc(() =>
            {
                if (TableVersionControl.CheckDone)
                {
                    // 에셋 체크 완료함
                    if (task != null)
                        CoroutineManager.RemoveTask(task);

                    this.SetStateDownloadAsset();
                }
                else
                {
                    // 에셋 체크 중
                    float currentProgress = TableVersionControl.GetRequestProgress();

                    this.sceneView.UpdateCheckAsset(currentProgress);
                }
            });
        }

        private void SetStateDownloadAsset()
        {
            this.SetState((int)STATE.DownloadAsset);

            // 에셋 로드 시작
            Coroutine task = CoroutineManager.AddTask(AssetManager.DownloadAsset());

            this.AddUpdateFunc(() =>
            {
                if (AssetManager.DownloadDone)
                {
                    // 에셋 로드 완료함
                    if (task != null)
                        CoroutineManager.RemoveTask(task);

                    this.SetStateWaitDone();
                }
                else
                {
                    // 에셋 로드중
                    string currentAsset = AssetManager.CurrentAsset;
                    float currentProgress = AssetManager.GetRequestProgress();

                    this.sceneView.UpdateDownloadAsset(currentAsset, currentProgress);
                }
            });
        }

        private void SetStateWaitDone()
        {
            this.SetState((int)STATE.WaitDone);

            this.sceneView.SetStateWaitDone();
        }

        private void OnClickLottoSelect(int eventNum, object obj)
        {
            SoundManager.Inst.Play(SoundManager.SOUND_YES);

            LottoPopupAssist.Open(() =>
            {

            });
        }

        private void OnClickRefreshAsset(int eventNum, object obj)
        {
            SoundManager.Inst.Play(SoundManager.SOUND_YES);

            // 에셋 갱신 시작
            this.SetStateCheckAsset();
        }

        private void OnClickChangeLanguage(int eventNum, object obj)
        {
            SoundManager.Inst.Play(SoundManager.SOUND_YES);

            bool parse = int.TryParse(obj.ToString(), out int option);
            if (parse == false)
            {
                DebugEx.LogColor("MAIN_SCENE PARSE ERROR", "red");
                return;
            }

            bool isDefined = System.Enum.IsDefined(typeof(LocaleControl.Language), option);
            if (isDefined == false)
            {
                DebugEx.LogColor("MAIN_SCENE new option is not defined. Option : " + option, "red");
                return;
            }

            LocaleControl.Language newLanguageCode = (LocaleControl.Language)option;
            DebugEx.Log("MAIN_SCENE new option is " + newLanguageCode.ToString());
            LocaleControl.LanguageCode = newLanguageCode;

            MainView.Params p = this.sceneView.ParamBuffer;
            p.lottoBtnTxt = LocaleControl.GetString(LocaleCodes.MAIN_SCENE_LOTTO_BTN_TEXT);
            p.checkAssetTxt = LocaleControl.GetString(LocaleCodes.MAIN_SCENE_ASSET_CHECKING_TEXT);
            p.downloadAssetTxt = LocaleControl.GetString(LocaleCodes.MAIN_SCENE_ASSET_LOADING_TEXT);
            p.refreshBtnTxt = LocaleControl.GetString(LocaleCodes.MAIN_SCENE_ASSET_LOADING_BTN_TEXT);

            this.sceneView.UpdateTextUI();
        }
    }
}
