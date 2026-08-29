using System.Collections.Generic;
using UnityEditor;

namespace Rikoo.Framework.Utilities.UI
{
    public static class SerializedPropertyExtension
    {
        public static string ToValueString(this SerializedProperty prop)
        {
            if (prop == null) return "null";

            return prop.propertyType switch
            {
                SerializedPropertyType.Integer => $"int: {prop.intValue}",
                SerializedPropertyType.Boolean => $"bool: {prop.boolValue}",
                SerializedPropertyType.Float => $"float: {prop.floatValue}",
                SerializedPropertyType.String => $"string: \"{prop.stringValue}\"",
                SerializedPropertyType.ObjectReference => $"object: {prop.objectReferenceValue}",
                SerializedPropertyType.Enum => $"enum: {prop.enumNames[prop.enumValueIndex]} ({prop.enumValueIndex})",
                SerializedPropertyType.Vector2 => $"Vector2: {prop.vector2Value}",
                SerializedPropertyType.Vector3 => $"Vector3: {prop.vector3Value}",
                SerializedPropertyType.Vector4 => $"Vector4: {prop.vector4Value}",
                SerializedPropertyType.Quaternion => $"Quaternion: {prop.quaternionValue}",
                SerializedPropertyType.Color => $"Color: {prop.colorValue}",
                SerializedPropertyType.Rect => $"Rect: {prop.rectValue}",
                SerializedPropertyType.Bounds => $"Bounds: {prop.boundsValue}",
                SerializedPropertyType.AnimationCurve => $"AnimationCurve: {prop.animationCurveValue}",
                SerializedPropertyType.LayerMask => $"LayerMask: {prop.intValue}",
                _ => $"unsupported type: {prop.propertyType}"
            };
        }
        
        public static bool HasChildren(this SerializedProperty property)
        {
            if (property == null)
                return false;

            var iterator = property.Copy();

            if (!iterator.NextVisible(true))
                return false;

            return iterator.depth > property.depth;
        }
        
        public static bool HasVisibleProperties(this SerializedObject so)
        {
            if (so == null)
                return false;

            var it = so.GetIterator();

            // Descend dans les enfants visibles
            if (!it.NextVisible(true))
                return false;

            return true;
        }
        
        public static bool HasVisibleProperties(this SerializedObject so, params string[] ignoredPaths)
        {
            if (so == null)
                return false;

            var ignored = new HashSet<string>(ignoredPaths);

            var it = so.GetIterator();

            if (!it.NextVisible(true))
                return false;

            // Parcours de toutes les propriétés visibles
            while (it.NextVisible(false))
            {
                if (ignored.Contains(it.propertyPath) || ignored.Contains(it.name))
                    continue;

                return true; // au moins une propriété valide trouvée
            }

            return false;
        }
    }
}