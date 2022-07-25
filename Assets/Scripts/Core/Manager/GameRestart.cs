using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.jbg.core.manager
{
    public class GameRestart : MonoBehaviour
    {
        private GameObject gameSceneObj = null;
        private int frameCount = 0;

        private void Awake()
        {
            GameObject.DontDestroyOnLoad(this.gameObject);

            if (Game.Instance != null)
                this.gameSceneObj = Game.Instance.gameObject;

            this.frameCount = 0;
        }

        private void Start()
        {
            if (this.gameSceneObj != null)
                GameObject.Destroy(this.gameSceneObj);
        }

        private void FixedUpdate()
        {
            this.frameCount++;

            if (this.frameCount == 10)
            {
                SceneManager.LoadScene("Game", LoadSceneMode.Single);

                Resources.UnloadUnusedAssets();
                System.GC.Collect();
            }
            else if (this.frameCount == 20)
            {
                Resources.UnloadUnusedAssets();
                System.GC.Collect();
            }
            else if (this.frameCount == 30)
            {
                GameObject.Destroy(this.gameObject);

                Resources.UnloadUnusedAssets();
                System.GC.Collect();
            }
        }

        public static void StartScene()
        {
            SceneManager.LoadScene("GameRestart", LoadSceneMode.Single);
        }
    }
}
