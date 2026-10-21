using System.Buffers;
using Rikoo.Core.Data;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

namespace Rikoo.Framework.Core.Data.Config
{
    [CustomPropertyDrawer(typeof(ListConfigItemReference<,>))]
    public class ListConfigItemReferenceDrawer : PropertyDrawer
    {
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var casted = property.boxedValue as IListConfigItemRef;
            if (casted.HasConfig)
            {
                EditorGUI.LabelField(position.SetWidth(EditorGUIUtility.labelWidth), label);
                casted.GetPossibilities(out string[] guid, out string[] names);
                var guidProperty = property.FindPropertyRelative("guid");
                var setGuid = guidProperty.stringValue;
                int selected = -1;
                for (var i = 0; i < guid.Length; i++)
                {
                    if (guid[i] == setGuid)
                    {
                        selected = i;
                        break;
                    } 
                }
                int newSelected = EditorGUI.Popup(position.AddX(EditorGUIUtility.labelWidth), selected, names);
                if (newSelected != selected)
                {
                    guidProperty.stringValue = guid[newSelected];
                }
            }
            else
            {
                EditorGUI.LabelField(position.Split(0, 2), label);
                if (GUI.Button(position.Split(1, 2), "Create Config"))
                {
                    casted.CreateConfig();
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}