using System;
using System.Linq;
using TrainingSystem.Scripts.Enums;
using UnityEngine;

namespace TrainingSystem.Scripts.Configuration
{
    [CreateAssetMenu(fileName = "New TrainingScenario", menuName = "TrainingScenario", order = 0)]
    public class TrainingScenario : ScriptableObject
    {
        [Serializable]
        public class Condition
        {
            public string ObjectKey => _objectKey;

            public InteractiveObjectState RequiredState => _requiredState;
            
            public bool IsSatisfied { get; set; }

            [SerializeField] private string _objectKey;
            [SerializeField] private InteractiveObjectState _requiredState;
        }

        [Serializable]
        public class Stage
        {
            public bool isCompleted => _completionConditions.All(x => x.IsSatisfied);
            
            public string[] EnableObjectsWhenEntered => _enableObjectsWhenEntered;

            public string[] DisableObjectsWhenEntered => _disableObjectsWhenEntered;

            public Condition[] CompletionConditions => _completionConditions;
            
            [SerializeField] private string[] _enableObjectsWhenEntered;
            [SerializeField] private string[] _disableObjectsWhenEntered;
            [SerializeField] private Condition[] _completionConditions;
        }

        public Stage[] Stages => _stages;

        [SerializeField] private Stage[] _stages;
    }
}