using System;
using System.Linq;
using TrainingSystem.Scripts.Enums;
using UnityEngine;

namespace TrainingSystem.Scripts.Configuration
{
    /// <summary>
    /// Training scenario with a sequence of stages to complete
    /// </summary>
    [CreateAssetMenu(fileName = "New TrainingScenario", menuName = "Training Scenario", order = 1)]
    public class TrainingScenario : ScriptableObject
    {
        /// <summary>
        /// Stage completion condition 
        /// </summary>
        [Serializable]
        public class Condition
        {
            /// <summary>
            /// Key of an object
            /// </summary>
            public string ObjectKey => _objectKey;

            /// <summary>
            /// Required state at the end of the stage
            /// </summary>
            public InteractiveObjectState RequiredState => _requiredState;

            public bool IsSatisfied { get; set; }

            [SerializeField] private string                 _objectKey;
            [SerializeField] private InteractiveObjectState _requiredState;
        }

        /// <summary>
        /// Scenario stage
        /// </summary>
        [Serializable]
        public class Stage
        {
            public string Name        => _name;
            public bool   isCompleted => _completionConditions.All(x => x.IsSatisfied);

            /// <summary>
            /// List of objects to enable for interaction on this stage
            /// </summary>
            public string[] EnableObjectsWhenEntered => _enableObjectsWhenEntered;

            /// <summary>
            /// List of objects to disable for interaction on this stage
            /// </summary>
            public string[] DisableObjectsWhenEntered => _disableObjectsWhenEntered;

            /// <summary>
            /// List of conditions to finish this stage
            /// </summary>
            public Condition[] CompletionConditions => _completionConditions;

            [SerializeField] private string      _name;
            [SerializeField] private string[]    _enableObjectsWhenEntered;
            [SerializeField] private string[]    _disableObjectsWhenEntered;
            [SerializeField] private Condition[] _completionConditions;
        }

        public Stage[] Stages => _stages;

        [SerializeField] private Stage[] _stages;
    }
}