using UnityEngine;

namespace TrainingSystem.Scripts.Configuration
{
    /// <summary>
    /// Training scenario preferences 
    /// </summary>
    [CreateAssetMenu(fileName = "New ScenarioPreferences", menuName = "Scenario Preferences", order = 0)]
    public class ScenarioPreferences : ScriptableObject
    {
        public string               Name                 => _name;
        public TrainingScenario     TrainingScenario     => _trainingScenario;
        public DisplayedObjectNames DisplayedObjectNames => _displayedObjectNames;

        /// <summary>
        /// Prefab with scenario items, instantiated when scene loaded
        /// </summary>
        public GameObject TrainingSetupPrefab => _trainingSetupPrefab;

        [SerializeField] private string               _name;
        [SerializeField] private TrainingScenario     _trainingScenario;
        [SerializeField] private DisplayedObjectNames _displayedObjectNames;
        [SerializeField] private GameObject           _trainingSetupPrefab;
    }
}