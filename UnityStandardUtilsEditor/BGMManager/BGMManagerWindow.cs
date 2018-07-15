using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityStandardUtils;
using UnityStandardUtilsEditor.Extension;

namespace UnityStandardUtilsEditor
{

    public class BGMManagerWindow : EditorWindow
    {
        private const string CachePath = "Assets/Resources/BGMCache.asset";
        private static BGMListCacher Cacher = new BGMListCacher();

        private SerializedObject so;
        private ReorderableList BGMListView;

        public static BGMManagerWindow Instance { get; set; }
        public static bool IsOpened { get { return Instance != null; } }

        private void OnEnable()
        {
            Cacher = ScriptableObjectExtension.ReadAsset<BGMListCacher>(CachePath);
            if (!Cacher) ScriptableObjectExtension.WriteAsset(Cacher, CachePath);


            so = new SerializedObject(Cacher);
            BGMListView = new ReorderableList(so, so.FindProperty("BGM"));


            Instance = this;


            BGMListView.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = BGMListView.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;

                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, position.width / 2f, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("Audio"), GUIContent.none);
                EditorGUI.PropertyField(
                    new Rect(rect.x + position.width / 2f, rect.y, position.width / 3f, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("Tag"), GUIContent.none);

            };

            BGMListView.onChangedCallback = (list) =>
            {
                
            };
        }

        [MenuItem("UnityStandardUtils/BGM Manager")]
        static void Init()
        {
            BGMManagerWindow window = (BGMManagerWindow)EditorWindow.GetWindow(typeof(BGMManagerWindow));
            window.titleContent = new GUIContent("BGM Manager");
            window.Show();
        }

        private void OnGUI()
        {

            if (Application.isPlaying)
            {
                GUILayout.Label("Please exit Play Mode then reopen this window.", GUILayout.Width(position.width));
                return;
            }
            so.Update();
            BGMListView.DoLayoutList();
            so.ApplyModifiedProperties();
            
        }

        private void OnDisable()
        {
            ScriptableObjectExtension.SaveAsset();
        }
    }
}