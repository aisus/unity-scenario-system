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
        /// <inheritdoc />
        public string CurrentStageName => _currentStage?.Name;

        private readonly Queue<TrainingScenario.Stage> _stagesQueue;
        private readonly IInteractionService           _interactionService;
        private          TrainingScenario.Stage        _currentStage;

        public ScenarioService(IInteractionService interactionService)
        {
            _interactionService = interactionService;
            _stagesQueue        = new Queue<TrainingScenario.Stage>(TrainingPreferences.TrainingScenario.Stages);
            _currentStage       = _stagesQueue.Dequeue();
            UpdateObjectsAvailability();
        }

        /// <inheritdoc />
        public ScenarioActionResult TryExecuteScenarioAction(InteractiveObjectEntity entity)
        {
            if (!IsActionAllowed(entity) || !IsActionSatisfiedConditions(entity))
                return ScenarioActionResult.ActionNotAllowed;
            if (!UpdateStageConditions()) return ScenarioActionResult.Ok;
            if (IsScenarioCompleted()) return ScenarioActionResult.ScenarioCompleted;
            do
            {
                _currentStage = _stagesQueue.Dequeue();
            } while (UpdateStageConditions());

            UpdateObjectsAvailability();

            return ScenarioActionResult.OkAndNextStage;
        }

        /// <summary>
        /// Check if entity exists in any of the current stage's exit conditions
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private bool IsActionAllowed(InteractiveObjectEntity entity) =>
            _currentStage.CompletionConditions.Any(x => x.ObjectKey.Equals(entity.Key));

        /// <summary>
        /// Check if entity state satisfies any conditions of the current stage
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private bool IsActionSatisfiedConditions(InteractiveObjectEntity entity)
        {
            var condition = _currentStage.CompletionConditions.FirstOrDefault(x => x.ObjectKey.Equals(entity.Key));
            if (condition == null) return false;
            return entity.State == condition.RequiredState;
        }

        private bool IsScenarioCompleted() => !_stagesQueue.Any() && _currentStage.isCompleted;

        /// <summary>
        /// Update current stage's conditions with current object states and return completion state
        /// </summary>
        /// <returns></returns>
        private bool UpdateStageConditions()
        {
            var interactiveEntities = _interactionService.InteractiveBehaviours.Select(x => x.Entity).ToList();

            foreach (var condition in _currentStage.CompletionConditions)
            {
                condition.IsSatisfied =
                    interactiveEntities.FirstOrDefault(x => x.Key.Equals(condition.ObjectKey))?.State ==
                    condition.RequiredState;
            }

            return _currentStage.isCompleted;
        }

        /// <summary>
        /// Update interactive objects availability for interaction according to current stage
        /// </summary>
        private void UpdateObjectsAvailability()
        {
            if (_currentStage.DisableObjectsWhenEntered.Any())
                _interactionService.InteractiveBehaviours.Where(x =>
                                       _currentStage.DisableObjectsWhenEntered.Contains(x.Entity.Key)).ToList()
                                   .ForEach(x => x.Entity.InteractionEnabled = false);

            if (_currentStage.EnableObjectsWhenEntered.Any())
                _interactionService.InteractiveBehaviours.Where(x =>
                                       _currentStage.EnableObjectsWhenEntered.Contains(x.Entity.Key)).ToList()
                                   .ForEach(x => x.Entity.InteractionEnabled = true);
        }
    }
}