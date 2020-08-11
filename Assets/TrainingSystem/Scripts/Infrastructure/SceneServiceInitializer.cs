using TrainingSystem.Scripts.Infrastructure.Services.DI;
using TrainingSystem.Scripts.Infrastructure.Services.Interaction;
using TrainingSystem.Scripts.Infrastructure.Services.Scenarios;
using TrainingSystem.Scripts.Infrastructure.Services.Statistics;
using TrainingSystem.Scripts.Infrastructure.Services.Utility;
using UnityEngine;

namespace TrainingSystem.Scripts.Infrastructure
{
    /// <summary>
    /// Registers scene-specific services when scene is loaded
    /// </summary>
    public class SceneServiceInitializer : MonoBehaviour
    {
        private void Awake()
        {
            // Register MonoBehaviour objects as services
            ServiceLocator.Current.RegisterService<IObjectNamesService>(FindObjectOfType<ObjectNamesService>());
            ServiceLocator.Current.RegisterService<IScenarioService>(FindObjectOfType<ScenarioService>());
            
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