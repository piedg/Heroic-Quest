using HeroQuest.Saving;
using System.Collections;
using UnityEngine;

namespace RPG.SavingSystem
{
    public class SavingManager : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        JsonSavingSystem savingSystem;

        private void Awake()
        {
            savingSystem = GetComponent<JsonSavingSystem>();

            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            yield return savingSystem.LoadLastScene(defaultSaveFile);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.F3))
            {
                Delete();
            }
        }

        public void Save()
        {
            Debug.Log("Saved!");
            savingSystem.Save(defaultSaveFile);
        }

        public void Load()
        {
            Debug.Log("Loading...");
            savingSystem.Load(defaultSaveFile);
        }

        public void Delete()
        {
            Debug.Log("Saves file deleted!");
            savingSystem.Delete(defaultSaveFile);
        }
    }
}
