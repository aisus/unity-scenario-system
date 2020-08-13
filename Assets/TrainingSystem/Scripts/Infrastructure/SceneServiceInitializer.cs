using TrainingSystem.Scripts.Configuration;
using TrainingSystem.Scripts.Infrastructure.Services.DI;
using TrainingSystem.Scripts.Infrastructure.Services.Interaction;
using TrainingSystem.Scripts.Infrastructure.Services.Scenarios;
using TrainingSystem.Scripts.Infrastructure.Services.Statistics;
using UnityEngine;

namespace TrainingSystem.Scripts.Infrastructure
{
    /// <summary>
    /// Registers scene-specific services when scene is loaded
    /// </summary>
    [DisallowMultipleComponent]
    public class SceneServiceInitializer : MonoBehaviour
    {
        [Header("Default in-editor preferences")]
        [SerializeField] private TrainingScenario _defaultTrainingScenario;
        [SerializeField] private DisplayedObjectNames _defaultDisplayedObjectNames;
        [SerializeField] private GameObject _defaultTrainingSetupPrefab;
        
        private void Awake()
        {
            // Register default preferences, if scene is played in editor
            if (Application.isEditor)
            {
                if (!TrainingPreferences.TrainingScenario)
                    TrainingPreferences.TrainingScenario = _defaultTrainingScenario;
                if (!TrainingPreferences.DisplayedObjectNames)
                    TrainingPreferences.DisplayedObjectNames = _defaultDisplayedObjectNames;
                if (!TrainingPreferences.TrainingSetupPrefab)
                    TrainingPreferences.TrainingSetupPrefab = _defaultTrainingSetupPrefab;
            }

            // Register MonoBehaviour objects as services, if needed
            
            // Register plain C# objects as services
            ServiceLocator.Current.RegisterService<IInteractionService>(new InteractionService());
            ServiceLocator.Current.RegisterService<IStatisticsService>(new StatisticsService());

            Instantiate(TrainingPreferences.TrainingSetupPrefab);
        }

        private void OnDestroy()
        {
            // When scene exits, finalize and remove scene-specific services
            ServiceLocator.Current.FinalizeSceneServices();
        }
    }
}