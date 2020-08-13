using System;
using TrainingSystem.Scripts.Enums;
using TrainingSystem.Scripts.Infrastructure.Services.DI;
using TrainingSystem.Scripts.Infrastructure.Services.Interaction;
using TrainingSystem.Scripts.Model;
using UnityEngine;

namespace TrainingSystem.Scripts.SceneInteraction
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

        public Action<InteractiveBehaviour> OnSelected        { get; set; }
        public Action<InteractiveBehaviour> OnDeselected      { get; set; }
        public Action<InteractiveBehaviour> OnActionPerformed { get; set; }

        [SerializeField] private InteractiveObjectEntity _entity;

        private IInteractionService _interactionService;
        private Renderer            _renderer;
        private Material            _materialBackup;
        private Animator            _animator;

        private static readonly int ActivatePropIndex   = Animator.StringToHash("Activate");
        private static readonly int DeactivatePropIndex = Animator.StringToHash("Deactivate");
        
        private void Awake()
        {
            _animator       = GetComponent<Animator>();
            _renderer       = GetComponent<Renderer>();
            _materialBackup = _renderer.material;
        }

        private void Start()
        {
            _interactionService = ServiceLocator.Current.ResolveDependency<IInteractionService>();
            _interactionService.AddInteractiveEntity(this);
        }
        
        /// <summary>
        /// Select interactive entity (when looking at it or pointing with mouse)
        /// </summary>
        /// <param name="param"></param>
        /// <param name="outlineMaterial"></param>
        public void Select(bool param, Material outlineMaterial = null)
        {
            if (param)
            {
                if (!_entity.InteractionEnabled) return;
                if (outlineMaterial)
                {
                    _materialBackup                = _renderer.material;
                    _renderer.material             = outlineMaterial;
                    _renderer.material.mainTexture = _materialBackup.mainTexture;
                }

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
            if (_interactionService.TryPerformAction(this))
                UpdateState();
        }

        /// <summary>
        /// Update state and visualize action
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void UpdateState()
        {
            switch (_entity.State)
            {
                case InteractiveObjectState.Inactive:
                    // UseOnce type disables itself when activated
                    if (Entity.Type == InteractiveObjectType.UseOnce)
                    {
                        _entity.State              = InteractiveObjectState.Active;
                        _entity.InteractionEnabled = false;
                        Select(false);
                    }
                    // Trigger type changes to active state when activated
                    // Switch type changes to active state when activated
                    else
                    {
                        _entity.State = InteractiveObjectState.Active;
                    }

                    _animator.SetTrigger(ActivatePropIndex);
                    break;
                case InteractiveObjectState.Active:
                    // Switch type changes to inactive state when deactivated
                    if (Entity.Type == InteractiveObjectType.Switch) _entity.State = InteractiveObjectState.Inactive;
                    // Trigger type can't be deactivated
                    // UseOnce type can't be deactivated
                    _animator.SetTrigger(DeactivatePropIndex);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}