using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HeroQuest.Saving
{
    [ExecuteAlways]
    public class JsonSaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";
        public string UniqueIdentifier { get { return uniqueIdentifier; } }

        // Cached state
        static Dictionary<string, JsonSaveableEntity> globalState = new Dictionary<string, JsonSaveableEntity>();

        public JToken CaptureAsJToken()
        {
            JObject state = new JObject();
            IDictionary<string, JToken> stateDict = state;
            foreach (IJsonSaveable jsonSaveable in GetComponents<IJsonSaveable>())
            {
                JToken token = jsonSaveable.CaptureAsJToken();
                string component = jsonSaveable.GetType().ToString();
                Debug.Log($"{name} Capture {component} = {token.ToString()}");
                stateDict[jsonSaveable.GetType().ToString()] = token;
            }
            return state;
        }

        public void RestoreAsJToken(JToken s)
        {
            JObject state = s.ToObject<JObject>();
            IDictionary<string, JToken> stateDict = state;
            foreach (IJsonSaveable jsonSaveable in GetComponents<IJsonSaveable>())
            {
                string component = jsonSaveable.GetType().ToString();
                if (stateDict.ContainsKey(component))
                {
                    Debug.Log($"{name} Restore {component} => {stateDict[component].ToString()}");
                    jsonSaveable.RestoreFromJToken(stateDict[component]);
                }
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalState[property.stringValue] = this;
        }
#endif

        private bool IsUnique(string candidate)
        {
            if (!globalState.ContainsKey(candidate)) return true;
            if (globalState[candidate] == this) return true;
            if (globalState[candidate] == null)
            {
                globalState.Remove(candidate);
                return true;
            }

            if (globalState[candidate].UniqueIdentifier != candidate)
            {
                globalState.Remove(candidate);
                return true;
            }

            return false;
        }
    }
}
