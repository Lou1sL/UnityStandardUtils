using UnityEngine;

namespace UnityStandardUtils
{
    [System.Serializable]
    public class SceneField
    {
        [SerializeField] private UnityEngine.Object sceneAsset;
        [SerializeField] private string sceneName = "";

        public string SceneName
        {
            get { return sceneName; }
        }

        // makes it work with the existing Unity methods (LoadLevel/LoadScene)
        public static implicit operator string(SceneField sceneField)
        {
            return sceneField.SceneName;
        }
    }
}
