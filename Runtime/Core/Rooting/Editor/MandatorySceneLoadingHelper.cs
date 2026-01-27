using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Script.Runtime.Core.Rooting.Editor
{
    public class MandatorySceneLoadingHelper
    {
        private static List<int> MandatorySceneIndexes = new() {1};
		
        [InitializeOnLoadMethod]
        private static void OnLoad()
        {
            EditorSceneManager.activeSceneChangedInEditMode += OnActiveSceneChanged;
        }

        private static void OnActiveSceneChanged(Scene previousScene, Scene newScene)
        {
            foreach (var sceneIndex in MandatorySceneIndexes)
            {
                var scenePath = EditorBuildSettings.scenes[sceneIndex].path;

                var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
                EditorSceneManager.MoveSceneBefore(scene, EditorSceneManager.GetActiveScene());
            }
        }
    }
}