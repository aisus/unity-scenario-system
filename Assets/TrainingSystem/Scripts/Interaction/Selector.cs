using System;
using TrainingSystem.Scripts.Enums;
using TrainingSystem.Scripts.Infrastructure;
using TrainingSystem.Scripts.Infrastructure.Services;
using TrainingSystem.Scripts.Infrastructure.Services.Interaction;
using TrainingSystem.Scripts.Infrastructure.Services.ServiceLocator;
using TrainingSystem.Scripts.Infrastructure.Services.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace TrainingSystem.Scripts.Interaction
{
    public class Selector : MonoBehaviour
    {
        [Header("Selector settings")] [SerializeField] [Range(0.1f, 10f)]
        private float _maxSelectionDistance;

        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private Text _text;

        private IInteractionService _interactionService;
        private IObjectNamesService _objectNamesService;
        private InteractiveBehaviour _currentSelected;

        #region EVENT_FUNCTIONS

        private void Start()
        {
            _interactionService = ServiceLocator.Current.ResolveDependency<IInteractionService>();
            _objectNamesService = ServiceLocator.Current.ResolveDependency<IObjectNamesService>();
        }

        private void Update()
        {
            CheckSelection();
            CheckActionOnSelectedEntity();
            _text.gameObject.SetActive(_currentSelected);
            if (_currentSelected)
            {
                _text.text = _objectNamesService.GetNameByKey(_currentSelected.Entity.Key);
            }
        }

        #endregion

        private void CheckSelection()
        {
            var ray = new Ray(transform.position, transform.forward);
            if (!Physics.Raycast(ray, out var hit, _maxSelectionDistance, _interactionLayer))
            {
                if (!_currentSelected) return;
                _currentSelected.Select(false);
                _currentSelected = null;
                return;
            }

            var foundObject = hit.transform.GetComponent<InteractiveBehaviour>();
            if (foundObject)
            {
                if (foundObject == _currentSelected) return;
                if (_currentSelected) _currentSelected.Select(false);
                _currentSelected = foundObject;
                _currentSelected.Select(true);
            }
            else
            {
                if (!_currentSelected) return;
                _currentSelected.Select(false);
                _currentSelected = null;
            }
        }

        private void CheckActionOnSelectedEntity()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (_currentSelected) _currentSelected.PerformAction();
            }
        }
    }
}