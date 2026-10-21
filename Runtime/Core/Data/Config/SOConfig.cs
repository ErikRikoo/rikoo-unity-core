using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Rikoo.Core.Data
{
    public abstract class SOConfig<T> : ScriptableObject where T : SOConfig<T>
    {
        private static T instance;
        
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = SOConfigUtilities.GetOrCreate<T>();
                }

                return instance;
            }
        }
    }

    public static class SOConfigUtilities
    {
        public static bool ChecksExistence<T>() where T : SOConfig<T>
        {
            Type t = typeof(T);
            var (_, resourceBasedPath) = GetPath<T>();

            T config = Resources.Load<T>(resourceBasedPath);

            return config != null;
        }
        
        public static T Get<T>() where T : SOConfig<T>
        {
            Type t = typeof(T);
            var (_, resourceBasedPath) = GetPath<T>();
            Debug.Log(resourceBasedPath);

            T config = Resources.Load<T>(resourceBasedPath);
            return config;
        }
        
        #if UNITY_EDITOR
        public static T GetOrCreate<T>() where T : SOConfig<T>
        {
            Type t = typeof(T);
            var (projectBasedPath, resourceBasedPath) = GetPath<T>();

            T config = Resources.Load<T>(resourceBasedPath);

            if (config == null)
            {
                config = CreateConfig<T>(projectBasedPath);
            }

            return config;
        }
        #endif

        private static T CreateConfig<T>(string projectBasedPath) where T : SOConfig<T>
        {
#if UNITY_EDITOR
            T config = ScriptableObject.CreateInstance<T>();
            CreateParentDirectoryIfNeeded(projectBasedPath);
            AssetDatabase.CreateAsset(config, projectBasedPath);
            AssetDatabase.ImportAsset(projectBasedPath);
            return config;
#else
            return null;
#endif
        }

        private static void CreateParentDirectoryIfNeeded(string projectBasedPath)
        {
            
            var items = projectBasedPath.Split(Path.DirectorySeparatorChar);
            var accumulatedPath = items[0];
            for (var i = 1; i < items.Length; i++)
            {
                if (items[i].Contains('.'))
                {
                    break;
                }
                if (!AssetDatabase.IsValidFolder(Path.Join(accumulatedPath, items[i])))
                {
                    AssetDatabase.CreateFolder(accumulatedPath, items[i]);
                }
                accumulatedPath = Path.Combine(accumulatedPath, items[i]);
            }
        }

        public static (string projectBased, string resourceBased) GetPath<T>() where T : SOConfig<T>
        {
            Type t = typeof(T);
            string resourceBased = Path.Join("Config", $"{t.Name}");
            string projectBased = Path.Join("Assets", "Resources", resourceBased + ".asset");

            return (projectBased, resourceBased);
        }
    }
}