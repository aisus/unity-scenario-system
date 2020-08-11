﻿using System;
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
        /// Object type
        /// </summary>
        public InteractiveObjectType Type
        {
            get => _type;
            set => _type = value;
        }

        /// <summary>
        /// String key of an object
        /// </summary>
        [SerializeField] private string _key;

        /// <summary>
        /// Object type
        /// </summary>
        [SerializeField] private InteractiveObjectType _type;

        /// <summary>
        /// Current state
        /// </summary>
        [SerializeField] private InteractiveObjectState _state;
    }
}