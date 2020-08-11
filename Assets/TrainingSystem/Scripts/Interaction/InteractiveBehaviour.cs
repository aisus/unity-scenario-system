using System;
using TrainingSystem.Scripts.Enums;
using TrainingSystem.Scripts.Infrastructure;
using TrainingSystem.Scripts.Infrastructure.Services;
using TrainingSystem.Scripts.Infrastructure.Services.Interfaces;
using TrainingSystem.Scripts.Infrastructure.Services.ServiceLocator;
using TrainingSystem.Scripts.Model;
using UnityEngine;

namespace TrainingSystem.Scripts.Interaction
{
    /// <summary>
    /// Interactive object's Behaviour component
    /// </summary>
    public class InteractiveBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Entity with interactive object's data
        /// </summary>
        public InteractiveObjectEntity Entity => _entity;
        public Action<InteractiveBehaviour> OnSelected { get; set; }
        public Action<InteractiveBehaviour> OnDeselected { get; set; }
        public Action<InteractiveBehaviour> OnActionPerformed { get; set; }
        
        [SerializeField] private InteractiveObjectEntity _entity;

        [Header("Materials")] [SerializeField] private Material _outlineMaterial;

        private Renderer _renderer;
        private Material _materialBackup;
        private Animator _animator;
        
        private static readonly int ActivatePropIndex = Animator.StringToHash("Activate");
        private static readonly int DeactivatePropIndex = Animator.StringToHash("Deactivate");

        #region EVENT_FUNCTIONS

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _renderer = GetComponent<Renderer>();
            _materialBackup = _renderer.material;
        }

        private void Start()
        {
            ServiceLocator.Current.ResolveDependency<IInteractionService>().AddInteractiveEntity(this);
        }

        #endregion

        /// <summary>
        /// Select interactive entity (when looking at it or pointing with mouse)
        /// </summary>
        /// <param name="param"></param>
        public void Select(bool param)
        {
            if (param)
            {
                _materialBackup = _renderer.material;
                _renderer.material = _outlineMaterial;
                _renderer.material.mainTexture = _materialBackup.mainTexture;
                OnSelected?.Invoke(this);
            }
            else
            {
                _renderer.material = _materialBackup;
                OnDeselected?.Invoke(this);
            }
        }

        /// <summary>
        /// Perform action on interactive entity
        /// </summary>
        public void PerformAction()
        {
            OnActionPerformed?.Invoke(this);
        }

        /// <summary>
        /// Visualise action - play animations, sounds, etc.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void VisualizeAction()
        {
            switch (_entity.State)
            {
                case InteractiveObjectState.Disabled:
                    break;
                case InteractiveObjectState.Inactive:
                    _animator.SetTrigger(ActivatePropIndex);
                    _entity.State = InteractiveObjectState.Active;
                    break;
                case InteractiveObjectState.Active:
                    _animator.SetTrigger(DeactivatePropIndex);
                    _entity.State = InteractiveObjectState.Inactive;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}