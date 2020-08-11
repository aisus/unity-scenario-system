using System;
using TrainingSystem.Scripts.Enums;
using TrainingSystem.Scripts.Infrastructure;
using TrainingSystem.Scripts.Infrastructure.Services;
using TrainingSystem.Scripts.Infrastructure.Services.DI;
using TrainingSystem.Scripts.Infrastructure.Services.Interaction;
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
                if (Entity.State == InteractiveObjectState.Disabled) return;
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
        /// Update state and visualize action
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void UpdateState()
        {
            switch (_entity.State)
            {
                case InteractiveObjectState.Disabled:
                    break;
                case InteractiveObjectState.Inactive:
                    // UseOnce disables itself when activated
                    if (Entity.Type == InteractiveObjectType.UseOnce)
                    {
                        _entity.State = InteractiveObjectState.Disabled;
                        Select(false);
                    }
                    // Button changes to active state when activated
                    // Switch changes to active state when activated
                    else
                    {
                        _entity.State = InteractiveObjectState.Active;
                    }

                    _animator.SetTrigger(ActivatePropIndex);
                    break;
                case InteractiveObjectState.Active:
                    // Switch changes to inactive state when deactivated
                    if (Entity.Type == InteractiveObjectType.Switch) _entity.State = InteractiveObjectState.Inactive;
                    // Button can't be deactivated
                    // UseOnce can't be deactivated
                    _animator.SetTrigger(DeactivatePropIndex);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}