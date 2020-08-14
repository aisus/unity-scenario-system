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
                PlayAnimations();
        }

        /// <summary>
        /// Update state and visualize action
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void PlayAnimations()
        {
            switch (_entity.State)
            {
                case InteractiveObjectState.Inactive:
                    _animator.SetTrigger(DeactivatePropIndex);
                    break;
                case InteractiveObjectState.Active:
                    _animator.SetTrigger(ActivatePropIndex);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}