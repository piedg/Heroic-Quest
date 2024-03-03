using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HeroQuest.Saving
{
    public class JsonSavingSystem : MonoBehaviour
    {
        private const string saveFileExtension = ".json";

        public IEnumerator LoadLastScene(string saveFile)
        {
            // Get the state
            JObject state = LoadJsonFromFile(saveFile);

            IDictionary<string, JToken> stateDict = state;

            // Load last scene
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            if (stateDict.ContainsKey("lastSceneBuildIndex"))
            {
                buildIndex = (int)stateDict["lastSceneBuildIndex"];
            }
            yield return SceneManager.LoadSceneAsync(buildIndex);

            // Restore state
            RestoreFromToken(state);
        }

        public void Save(string saveFile)
        {
            JObject state = LoadJsonFromFile(saveFile);
            CaptureAsToken(state);
            SaveFileAsJson(saveFile, state);
        }

        public void Load(string saveFile)
        {
            RestoreFromToken(LoadJsonFromFile(saveFile));
        }

        public void Delete(string saveFile)
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }

        public IEnumerable<string> SavesList()
        {
            foreach (string path in Directory.EnumerateFiles(Application.persistentDataPath))
            {
                if (Path.GetExtension(path) == saveFileExtension)
                {
                    yield return Path.GetFileNameWithoutExtension(path);
                }
            }
        }

        private JObject LoadJsonFromFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new JObject();
            }

            using (var textReader = File.OpenText(path))
            {
                using (var reader = new JsonTextReader(textReader))
                {
                    reader.FloatParseHandling = FloatParseHandling.Double;

                    return JObject.Load(reader);
                }
            }
        }

        private void SaveFileAsJson(string saveFile, JObject state)
        {
            string path = GetPathFromSaveFile(saveFile);
            Debug.Log("Saving to " + path);

            using (var textWriter = File.CreateText(path))
            {
                using (var writer = new JsonTextWriter(textWriter))
                {
                    writer.Formatting = Formatting.Indented;
                    state.WriteTo(writer);
                }
            }
        }

        private void CaptureAsToken(JObject state)
        {
            IDictionary<string, JToken> stateDict = state;
            foreach (JsonSaveableEntity saveable in FindObjectsOfType<JsonSaveableEntity>())
            {
                stateDict[saveable.UniqueIdentifier] = saveable.CaptureAsJToken();
            }

            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreFromToken(JObject state)
        {
            JObject stateDict = state;
            foreach (JsonSaveableEntity saveable in FindObjectsOfType<JsonSaveableEntity>())
            {
                string id = saveable.UniqueIdentifier;
                if (state.ContainsKey(id))
                {
                    saveable.RestoreAsJToken(stateDict[id]);
                }
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + saveFileExtension);
        }
    }
}
