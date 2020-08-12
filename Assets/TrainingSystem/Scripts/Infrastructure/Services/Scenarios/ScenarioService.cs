using System.Collections.Generic;
using System.Linq;
using TrainingSystem.Scripts.Configuration;
using TrainingSystem.Scripts.Enums;
using TrainingSystem.Scripts.Infrastructure.Services.Preferences;
using TrainingSystem.Scripts.Model;
using UnityEngine;

namespace TrainingSystem.Scripts.Infrastructure.Services.Scenarios
{
    /// <inheritdoc cref="IScenarioService" />
    [DisallowMultipleComponent]
    public class ScenarioService : MonoBehaviour, IScenarioService
    {
        private TrainingScenario _scenario;
        private Queue<TrainingScenario.Stage> _stagesQueue;
        private TrainingScenario.Stage _currentStage;

        #region EVENT_FUNCTIONS

        private void Start()
        {
            _scenario = GlobalPreferences.SelectedScenario;
            _stagesQueue = new Queue<TrainingScenario.Stage>(_scenario.Stages);
            _currentStage = _stagesQueue.Dequeue();
        }

        #endregion

        public string[] GetObjectsToEnableOnCurrentStage() => _currentStage.EnableObjectsWhenEntered;

        public string[] GetObjectsToDisableOnCurrentStage() => _currentStage.DisableObjectsWhenEntered;

        public bool IsScenarioCompleted() => !_stagesQueue.Any() && _currentStage.isCompleted;

        public ScenarioActionResult TryExecuteScenarioAction(InteractiveObjectEntity entity)
        {
            if (!IsActionAllowed(entity)) return ScenarioActionResult.ActionNotAllowed;
            var isSatisfied = TrySatisfyConditions(entity);
            var isStageCompleted = AreAllStageConditionsSatisfied();
            if (!isSatisfied) return ScenarioActionResult.ConditionsNotMatch;
            if (!isStageCompleted) return ScenarioActionResult.Ok;
            if (IsScenarioCompleted()) return ScenarioActionResult.ScenarioCompleted;
            _currentStage = _stagesQueue.Dequeue();
            return ScenarioActionResult.OkAndNextStage;
        }

        /// <inheritdoc />
        public void OnSceneExit()
        {
        }

        private bool IsActionAllowed(InteractiveObjectEntity entity) =>
            _currentStage.CompletionConditions.Any(x => x.ObjectKey.Equals(entity.Key));

        private bool AreAllStageConditionsSatisfied() =>
            _currentStage.CompletionConditions.All(x => x.IsSatisfied);

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