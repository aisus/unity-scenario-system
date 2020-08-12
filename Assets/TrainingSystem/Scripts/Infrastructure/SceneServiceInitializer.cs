using TrainingSystem.Scripts.Configuration;
using TrainingSystem.Scripts.Infrastructure.Services.DI;
using TrainingSystem.Scripts.Infrastructure.Services.Interaction;
using TrainingSystem.Scripts.Infrastructure.Services.Preferences;
using TrainingSystem.Scripts.Infrastructure.Services.Scenarios;
using TrainingSystem.Scripts.Infrastructure.Services.Statistics;
using TrainingSystem.Scripts.Infrastructure.Services.Utility.ObjectNames;
using UnityEngine;

namespace TrainingSystem.Scripts.Infrastructure
{
    /// <summary>
    /// Registers scene-specific services when scene is loaded
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ObjectNamesService))]
    [RequireComponent(typeof(ScenarioService))]
    public class SceneServiceInitializer : MonoBehaviour
    {
        [Header("Default preferences")]
        [SerializeField] private TrainingScenario _defaultTrainingScenario;
        [SerializeField] private DisplayedObjectNames _defaultDisplayedObjectNames;
        
        private void Awake()
        {
            // Register default preferences, if not present
            if (GlobalPreferences.SelectedScenario == null)
                GlobalPreferences.SelectedScenario = _defaultTrainingScenario;
            if (GlobalPreferences.DisplayedObjectNames == null)
                GlobalPreferences.DisplayedObjectNames = _defaultDisplayedObjectNames;

            // Register MonoBehaviour objects as services
            ServiceLocator.Current.RegisterService<IObjectNamesService>(GetComponent<ObjectNamesService>());
            ServiceLocator.Current.RegisterService<IScenarioService>(GetComponent<ScenarioService>());
            
            // Register plain C# objects as services
            ServiceLocator.Current.RegisterService<IInteractionService>(new InteractionService());
            ServiceLocator.Current.RegisterService<IStatisticsService>(new StatisticsService());
        }

        private void OnDestroy()
        {
            // When scene exits, finalize and remove scene-specific services
            ServiceLocator.Current.FinalizeSceneServices();
        }
    }
}