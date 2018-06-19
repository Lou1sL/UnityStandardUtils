
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityStandardUtils;
using UnityStandardUtilsEditor;

namespace UnityStandardUtilsEditor
{
    public class PrefabCacherWindow : EditorWindow
    {
        static string AssetSave = "Assets/Resources/PrefabLocationCache.asset";

        static PrefabCacher gmp = null;

        Vector2 scrollPos;

        [MenuItem("UnityStandardUtils/PrefebCacher")]
        static void Init()
        {
            PrefabCacherWindow window = (PrefabCacherWindow)EditorWindow.GetWindow(typeof(PrefabCacherWindow));
            window.Show();

            window.titleContent = new GUIContent("PrefebCacher");

            gmp = ScriptableObjectUtility.ReadAsset<PrefabCacher>(AssetSave);
        }

        
        void OnGUI()
        {
            if (Application.isPlaying)
            {
                GUILayout.Label("Please exit Play Mode then reopen this window.", GUILayout.Width(position.width));
                return;
            }


            if (gmp != null)
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height - 2 * EditorGUIUtility.singleLineHeight));

                GUILayout.Label("Cached Prefabs:" + gmp.GetAllPrefab().Count, GUILayout.Width(EditorGUIUtility.labelWidth));

                foreach (PrefabCacher.PrefabCache pc in gmp.GetAllPrefab())
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(pc.gameObject.name, GUILayout.Width(EditorGUIUtility.labelWidth));
                    GUILayout.Label(pc.path, GUILayout.Width(position.width * 0.97f - EditorGUIUtility.labelWidth));
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndScrollView();
            }
            

            
            if (GUILayout.Button("CREATE CACHE"))
            {
                gmp = new PrefabCacher();
                ScriptableObjectUtility.DeleteAsset(AssetSave);

                gmp.UpdateCache(LoadAllPrefab());
                ScriptableObjectUtility.WriteAsset(gmp, AssetSave);
            }
        }

        private static List<PrefabCacher.PrefabCache> LoadAllPrefab()
        {
            List<PrefabCacher.PrefabCache> prefab = new List<PrefabCacher.PrefabCache>();

            var absolutePaths = System.IO.Directory.GetFiles(Application.dataPath, "*.prefab", System.IO.SearchOption.AllDirectories);

            foreach (var absolutePath in absolutePaths)
            {
                var path = "Assets/" + absolutePath.Replace(Application.dataPath + System.IO.Path.DirectorySeparatorChar, "");
                path = path.Substring(0, path.Length - 7);
                path = path.Replace("\\", "/");
                path += ".prefab";


                GameObject go = EditorExtension.LoadPrefabPath(path);
                if (go)
                {
                    PrefabIdentity pi = go.GetComponent<PrefabIdentity>();
                    if (!pi) pi = go.AddComponent<PrefabIdentity>();

                    pi.MyPath = path;
                    prefab.Add(new PrefabCacher.PrefabCache() { path = path, gameObject = go });
                }
            }
            return prefab;
        }

    }


}
