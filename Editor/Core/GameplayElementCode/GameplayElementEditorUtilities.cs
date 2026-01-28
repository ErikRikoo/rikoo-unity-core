using UnityEditor;
using UnityEngine;

namespace Rikoo.Framework.Core.GameplayElementCode
{
    public static class GameplayElementEditorUtilities
    {
        [MenuItem("GameObject/Rikoo/GPE/Create GPE")]
        public static void CreateSimpleGPE()
        {
            var createdGameObject = new GameObject("GPE");
            var gpe = createdGameObject.AddComponent<GameplayElement>();
            gpe.EasySetup();
        }
    }
}