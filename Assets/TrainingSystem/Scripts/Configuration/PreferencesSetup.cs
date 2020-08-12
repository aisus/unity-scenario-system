using UnityEngine;

namespace TrainingSystem.Scripts.Configuration
{
    [CreateAssetMenu(fileName = "New PreferencesSetup", menuName = "Preferences Setup", order = 2)]
    public class PreferencesSetup : ScriptableObject
    {
        public string Name => _name;
        
        public TrainingScenario TrainingScenario => _trainingScenario;
        
        public DisplayedObjectNames DisplayedObjectNames => _displayedObjectNames;
        
        public GameObject TrainingSetupPrefab => _trainingSetupPrefab;
        
        [SerializeField] private string _name;
        [SerializeField] private TrainingScenario _trainingScenario;
        [SerializeField] private DisplayedObjectNames _displayedObjectNames;
        [SerializeField] private GameObject _trainingSetupPrefab;
    }
}