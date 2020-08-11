using System;
using TrainingSystem.Scripts.Enums;
using UnityEngine;

namespace TrainingSystem.Scripts.Model
{
    /// <summary>
    /// Model of an interactive object
    /// </summary>
    [Serializable]
    public class InteractiveObjectEntity
    {
        /// <summary>
        /// Current state
        /// </summary>
        public InteractiveObjectState State
        {
            get => _state;
            set => _state = value;
        }

        /// <summary>
        /// String key of an object
        /// </summary>
        public string Key
        {
            get => _key;
            set => _key = value;
        }

        /// <summary>
        /// Current state
        /// </summary>
        [SerializeField] private InteractiveObjectState _state;
        
        /// <summary>
        /// String key of an object
        /// </summary>
        [SerializeField] private string _key;
    }
}