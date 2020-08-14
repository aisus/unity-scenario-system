using System;
using UnityEngine;
using UnityEngine.UI;

namespace TrainingSystem.Scripts.UI.Scene
{
    /// <summary>
    /// Failure screen controller, displays on first incorrect action
    /// </summary>
    public class FailureScreenController : MonoBehaviour
    {
        public Action OnRestartPressed { get; set; }
        public Action OnContinuePressed { get; set; }
        public Action OnExitPressed { get; set; }

        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _exitButton;

        private void Awake()
        {
            _restartButton.onClick.AddListener(() => OnRestartPressed?.Invoke());
            _exitButton.onClick.AddListener(() => OnExitPressed?.Invoke());
            _continueButton.onClick.AddListener(() => OnContinuePressed?.Invoke());
        }
    }
}