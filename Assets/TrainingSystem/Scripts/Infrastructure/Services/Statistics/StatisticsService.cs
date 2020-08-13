using TrainingSystem.Scripts.Infrastructure.Services.DI;
using TrainingSystem.Scripts.Infrastructure.Services.Interaction;
using TrainingSystem.Scripts.Model;
using UnityEngine;

namespace TrainingSystem.Scripts.Infrastructure.Services.Statistics
{
    /// <inheritdoc />
    public class StatisticsService : IStatisticsService
    {
        public StatisticsEntity Statistics { get; }

        private readonly IInteractionService _interactionService;

        public StatisticsService()
        {
            Statistics                              =  new StatisticsEntity();
            _interactionService                     =  ServiceLocator.Current.ResolveDependency<IInteractionService>();
            _interactionService.OnActionPerformed   += ActionPerformedHandler;
            _interactionService.OnActionFailed      += ActionFailedHandler;
            _interactionService.OnScenarioCompleted += ScenarioCompletedHandler;
        }

        /// <summary>
        /// Executes each time when action performed
        /// </summary>
        /// <param name="entity"></param>
        private void ActionPerformedHandler(InteractiveObjectEntity entity) => Statistics.TotalActionsCount++;

        /// <summary>
        /// Executes each time when action failed
        /// </summary>
        /// <param name="entity"></param>
        private void ActionFailedHandler(InteractiveObjectEntity entity) => Statistics.FailedActionsCount++;

        /// <summary>
        /// Executes once when scenario is completed
        /// </summary>
        private void ScenarioCompletedHandler()
        {
            Statistics.TimeInSeconds = Time.timeSinceLevelLoad;
        }
    }
}