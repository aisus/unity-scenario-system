using TrainingSystem.Scripts.Infrastructure.Services.DI;
using TrainingSystem.Scripts.Infrastructure.Services.Interaction;
using TrainingSystem.Scripts.Model;

namespace TrainingSystem.Scripts.Infrastructure.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        public StatisticsEntity Statistics { get; }

        private readonly IInteractionService _interactionService;

        public StatisticsService()
        {
            Statistics = new StatisticsEntity();
            _interactionService = ServiceLocator.Current.ResolveDependency<IInteractionService>();
            _interactionService.OnActionPerformed += ActionPerformedHandler;
            _interactionService.OnActionFailed += ActionFailedHandler;
            _interactionService.OnScenarioCompleted += ScenarioCompletedHandler;
        }

        private void ActionPerformedHandler(InteractiveObjectEntity entity) => Statistics.TotalActionsCount++;
        
        private void ActionFailedHandler(InteractiveObjectEntity entity) => Statistics.FailedActionsCount++;

        private void ScenarioCompletedHandler()
        {
        }
    }
}