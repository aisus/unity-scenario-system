using System;
using System.Linq;
using TrainingSystem.Scripts.Configuration;
using UnityEngine;

namespace TrainingSystem.Scripts.Infrastructure
{
    public class TrainingPreferencesInitializer : MonoBehaviour
    {
        public string TrainingSceneName => _trainingSceneName;

        public string MainMenuSceneName => _mainMenuSceneName;

        public PreferencesSetup[] Data => _data;

        [SerializeField] private string _trainingSceneName;
        [SerializeField] private string _mainMenuSceneName;
        [SerializeField] private PreferencesSetup[] _data;

        public void SetupPreferences(string name)
        {
            var prefs = _data.FirstOrDefault(x => x.Name == name);
            if(prefs == null) throw new ArgumentException();

            TrainingPreferences.Name = prefs.Name;
            TrainingPreferences.TrainingScenario = prefs.TrainingScenario;
            TrainingPreferences.DisplayedObjectNames = prefs.DisplayedObjectNames;
            TrainingPreferences.TrainingSetupPrefab = prefs.TrainingSetupPrefab;
            TrainingPreferences.TrainingSceneName = _trainingSceneName;
            TrainingPreferences.MainMenuSceneName = _mainMenuSceneName;
        }
    }
}