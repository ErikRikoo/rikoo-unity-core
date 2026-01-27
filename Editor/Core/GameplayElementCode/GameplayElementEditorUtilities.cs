using UnityEditor;
using UnityEngine;

namespace Core.GameplayElementCode
{
    public static class GameplayElementEditorUtilities
    {
        [MenuItem("GameObject/Foxxy/GPE/Create GPE")]
        public static void CreateSimpleGPE()
        {
            var createdGameObject = new GameObject("GPE");
            var gpe = createdGameObject.AddComponent<GameplayElement>();
            gpe.EasySetup();
        }
    }
}