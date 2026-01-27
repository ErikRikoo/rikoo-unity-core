using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Runtime.Core.Rooting
{
    public static class SceneRooter
    {
        private const int MainMenuSceneIndex = 0;
        private const int AutoloadSceneIndex = 1;
        private const int GameSceneIndex = 2;

        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            LoadAutoloadIfNeeded();
        }
		
        [RuntimeInitializeOnLoadMethod]
        private static void OnLoad() { }

        private static void LoadAutoloadIfNeeded()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).buildIndex == AutoloadSceneIndex)
                {
                    return;
                }
            }
            if (!SceneManager.GetSceneByBuildIndex(AutoloadSceneIndex).isLoaded)
            {
                SceneManager.LoadSceneAsync(AutoloadSceneIndex, LoadSceneMode.Additive);
            }
        }

        public static void GoToMainMenu()
        {
            SceneManager.UnloadSceneAsync(GameSceneIndex, UnloadSceneOptions.None);
            SceneManager.LoadSceneAsync(MainMenuSceneIndex, LoadSceneMode.Additive);
        }
        
        public static void GoToGame()
        {
            SceneManager.UnloadSceneAsync(MainMenuSceneIndex, UnloadSceneOptions.None);
            SceneManager.LoadSceneAsync(GameSceneIndex, LoadSceneMode.Additive);
        }
    }
}