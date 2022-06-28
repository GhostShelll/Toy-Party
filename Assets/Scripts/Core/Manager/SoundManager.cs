using System.Collections.Generic;

using UnityEngine;

namespace com.jbg.core.manager
{
    public class SoundManager : MonoBehaviour
    {
        private const string SOUND_BASE_PATH = "Sounds/";
        private const float DEFAULT_VOLUME = 1.0f;

        public static SoundManager Inst { get; private set; }

        [SerializeField]
        AudioSource audioSource;
        [SerializeField]
        Dictionary<string, AudioClip> audioClipList = new();

        private void Awake()
        {
            SoundManager.Inst = this;

            this.audioSource = this.GetComponent<AudioSource>();
        }

        private void OnDestroy()
        {
            SoundManager.Inst = null;
        }

        public void Play(string soundName, float vol = -1f)
        {
            if (this.audioClipList.ContainsKey(soundName) == false)
            {
                DebugEx.Log("SOUND_LOAD:" + soundName);

                if (this == null)
                {
                    DebugEx.Log("SOUND_LOAD_FAILED:" + soundName + ", MANAGER_IS_NULL");
                    return;
                }

                string soundPath = SoundManager.SOUND_BASE_PATH + soundName;

                AudioClip newAudioClip = Resources.Load<AudioClip>(soundPath);
                if (newAudioClip == null)
                {
                    DebugEx.Log("SOUND_LOAD_FAILED:" + soundName + ", PATH:" + soundPath + ", RESOURCES_LOAD_FAILED");
                    return;
                }

                this.audioClipList.Add(soundName, newAudioClip);
            }

            AudioClip audioClip = this.audioClipList[soundName];
            float volume = vol == -1f ? SoundManager.DEFAULT_VOLUME : vol;

            DebugEx.LogColor(string.Format("Name: {0}, Vol: {1}", soundName, volume), "lime");

            this.audioSource.volume = volume;
            this.audioSource.PlayOneShot(audioClip);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////// DEFINES
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static readonly string SOUND_YES = "yes";
        public static readonly string SOUND_NO = "no";
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
