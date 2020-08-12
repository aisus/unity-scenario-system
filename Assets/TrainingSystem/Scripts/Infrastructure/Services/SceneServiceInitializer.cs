using TrainingSystem.Scripts.Configuration;
using TrainingSystem.Scripts.Infrastructure.Preferences;
using TrainingSystem.Scripts.Infrastructure.Services.DI;
using TrainingSystem.Scripts.Infrastructure.Services.Interaction;
using TrainingSystem.Scripts.Infrastructure.Services.Scenarios;
using TrainingSystem.Scripts.Infrastructure.Services.Statistics;
using UnityEngine;

namespace TrainingSystem.Scripts.Infrastructure.Services
{
    /// <summary>
    /// Registers scene-specific services when scene is loaded
    /// </summary>
    [DisallowMultipleComponent]
    public class SceneServiceInitializer : MonoBehaviour
    {
        [Header("Default preferences")]
        [SerializeField] private TrainingScenario _defaultTrainingScenario;
        [SerializeField] private DisplayedObjectNames _defaultDisplayedObjectNames;
        
        private void Awake()
        {
            // Register default preferences, if scene is played in editor
            if (Application.isEditor)
            {
                if (GlobalPreferences.SelectedScenario == null)
                    GlobalPreferences.SelectedScenario = _defaultTrainingScenario;
                if (GlobalPreferences.DisplayedObjectNames == null)
                    GlobalPreferences.DisplayedObjectNames = _defaultDisplayedObjectNames;
            }

            // Register MonoBehaviour objects as services
            
            // Register plain C# objects as services
            ServiceLocator.Current.RegisterService<IScenarioService>(new ScenarioService());
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