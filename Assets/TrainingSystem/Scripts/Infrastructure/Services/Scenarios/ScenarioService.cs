using System;
using System.Collections.Generic;
using System.Linq;
using TrainingSystem.Scripts.Configuration;
using TrainingSystem.Scripts.Enums;
using TrainingSystem.Scripts.Infrastructure.Services.DI;
using TrainingSystem.Scripts.Infrastructure.Services.Interaction;
using TrainingSystem.Scripts.Model;

namespace TrainingSystem.Scripts.Infrastructure.Services.Scenarios
{
    /// <inheritdoc cref="IScenarioService" />
    public class ScenarioService : IScenarioService
    {
        public string ActiveStageName => _currentStage?.Name;

        private TrainingScenario _scenario;
        private Queue<TrainingScenario.Stage> _stagesQueue;
        private TrainingScenario.Stage _currentStage;
        private Lazy<IInteractionService> _interactionService;

        public ScenarioService()
        {
            _interactionService =
                new Lazy<IInteractionService>(() => ServiceLocator.Current.ResolveDependency<IInteractionService>());
            _scenario = TrainingPreferences.TrainingScenario;
            _stagesQueue = new Queue<TrainingScenario.Stage>(_scenario.Stages);
            _currentStage = _stagesQueue.Dequeue();
        }

        public string[] GetObjectsToEnableOnCurrentStage() => _currentStage.EnableObjectsWhenEntered;

        public string[] GetObjectsToDisableOnCurrentStage() => _currentStage.DisableObjectsWhenEntered;

        public bool IsScenarioCompleted() => !_stagesQueue.Any() && _currentStage.isCompleted;

        public ScenarioActionResult TryExecuteScenarioAction(InteractiveObjectEntity entity)
        {
            if (!IsActionAllowed(entity)) return ScenarioActionResult.ActionNotAllowed;
            var isSatisfied = TrySatisfyConditions(entity);
            if (!isSatisfied) return ScenarioActionResult.ConditionsNotMatch;
            if (!AreAllStageConditionsSatisfied()) return ScenarioActionResult.Ok;
            if (IsScenarioCompleted()) return ScenarioActionResult.ScenarioCompleted;
            do
            {
                _currentStage = _stagesQueue.Dequeue();
            } while (AreAllStageConditionsSatisfied());

            return ScenarioActionResult.OkAndNextStage;
        }

        private bool IsActionAllowed(InteractiveObjectEntity entity) =>
            _currentStage.CompletionConditions.Any(x => x.ObjectKey.Equals(entity.Key));

        private bool AreAllStageConditionsSatisfied()
        {
            var interactiveEntities = _interactionService.Value.InteractiveBehaviours.Select(x => x.Entity).ToList();

            foreach (var condition in _currentStage.CompletionConditions)
            {
                condition.IsSatisfied =
                    interactiveEntities.FirstOrDefault(x => x.Key.Equals(condition.ObjectKey))?.State ==
                    condition.RequiredState;
            }

            return _currentStage.CompletionConditions.All(x => x.IsSatisfied);
        }

        private bool TrySatisfyConditions(InteractiveObjectEntity entity)
        {
            var condition = _currentStage.CompletionConditions.FirstOrDefault(x => x.ObjectKey.Equals(entity.Key));
            if (condition == null) return false;
            var isSatisfied = entity.State == condition.RequiredState;
            condition.IsSatisfied = isSatisfied;
            return isSatisfied;
        }

        /// <inheritdoc />
        public void OnSceneExit()
        {
        }
    }
}