using System;
using System.Linq;
using TrainingSystem.Scripts.Configuration;
using UnityEngine;

namespace TrainingSystem.Scripts.Infrastructure
{
    /// <summary>
    /// Initialize global preferences with values from inspector
    /// </summary>
    public class TrainingPreferencesInitializer : MonoBehaviour
    {
        public ScenarioPreferences[] Data => _data;

        [SerializeField] private ScenarioPreferences[] _data;
        [SerializeField] private string                _trainingSceneName;
        [SerializeField] private string                _mainMenuSceneName;

        public void SetupPreferences(ScenarioPreferences scenario)
        {
            TrainingPreferences.Name                 = scenario.Name;
            TrainingPreferences.TrainingScenario     = scenario.TrainingScenario;
            TrainingPreferences.DisplayedObjectNames = scenario.DisplayedObjectNames;
            TrainingPreferences.TrainingSetupPrefab  = scenario.TrainingSetupPrefab;
            TrainingPreferences.TrainingSceneName    = _trainingSceneName;
            TrainingPreferences.MainMenuSceneName    = _mainMenuSceneName;
        }
    }
}