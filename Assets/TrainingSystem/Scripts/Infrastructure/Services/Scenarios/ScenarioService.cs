using System.Collections.Generic;
using System.Linq;
using TrainingSystem.Scripts.Configuration;
using TrainingSystem.Scripts.Enums;
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
        private IInteractionService _interactionService;

        public ScenarioService(IInteractionService interactionService)
        {
            _interactionService = interactionService;
            _scenario = TrainingPreferences.TrainingScenario;
            _stagesQueue = new Queue<TrainingScenario.Stage>(_scenario.Stages);
            _currentStage = _stagesQueue.Dequeue();
            UpdateObjectStates();
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
            UpdateObjectStates();

            return ScenarioActionResult.OkAndNextStage;
        }

        private void UpdateObjectStates()
        {
            if (_currentStage.DisableObjectsWhenEntered.Any())
            {
                _interactionService.InteractiveBehaviours.Where(x =>
                        _currentStage.DisableObjectsWhenEntered.Contains(x.Entity.Key)).ToList()
                    .ForEach(x => x.Entity.InteractionEnabled = false);
            }
            
            if (_currentStage.EnableObjectsWhenEntered.Any())
            {
                _interactionService.InteractiveBehaviours.Where(x =>
                        _currentStage.EnableObjectsWhenEntered.Contains(x.Entity.Key)).ToList()
                    .ForEach(x => x.Entity.InteractionEnabled = true);
            }
        }

        private bool IsActionAllowed(InteractiveObjectEntity entity) =>
            _currentStage.CompletionConditions.Any(x => x.ObjectKey.Equals(entity.Key));

        private bool AreAllStageConditionsSatisfied()
        {
            var interactiveEntities = _interactionService.InteractiveBehaviours.Select(x => x.Entity).ToList();

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
    }
}