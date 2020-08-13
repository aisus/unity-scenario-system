using System;
using System.Text;
using TrainingSystem.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace TrainingSystem.Scripts.UI.Scene
{
    /// <summary>
    /// Results screen controller, displays when scenario finished
    /// </summary>
    public class ResultsScreenController : MonoBehaviour
    {
        public Action OnRestartPressed { get; set; }
        public Action OnExitPressed    { get; set; }

        [SerializeField] private Text   _resultsText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;

        private void Awake()
        {
            _restartButton.onClick.AddListener(() => OnRestartPressed?.Invoke());
            _exitButton.onClick.AddListener(() => OnExitPressed?.Invoke());
        }

        public void DisplayResults(StatisticsEntity entity)
        {
            var sb = new StringBuilder();

            sb.Append($"Сценарий выполнен {(entity.IsSuccessful ? "успешно" : "с ошибками")}\n");

            sb.Append($"Действий: {entity.TotalActionsCount}, ошибочных: {entity.FailedActionsCount}\n");

            sb.Append($"Верных действий: {Mathf.CeilToInt(entity.SuccessRate * 100)}%\n");

            var minutes = Mathf.FloorToInt(entity.TimeInSeconds / 60);
            var seconds = Mathf.FloorToInt(entity.TimeInSeconds % 60);
            sb.Append($"Время выполнения: {minutes} мин. {seconds} сек.");

            _resultsText.text = sb.ToString();
        }
    }
}