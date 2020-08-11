using TrainingSystem.Scripts.Infrastructure.Services.Impl;
using TrainingSystem.Scripts.Infrastructure.Services.Interfaces;
using TrainingSystem.Scripts.Infrastructure.Services.ServiceLocator;
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