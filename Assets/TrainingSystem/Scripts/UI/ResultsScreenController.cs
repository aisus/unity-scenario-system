﻿using System;
using System.Text;
using TrainingSystem.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace TrainingSystem.Scripts.UI
{
    public class ResultsScreenController : MonoBehaviour
    {
        public Action OnRestartPressed { get; set; }
        public Action OnExitPressed { get; set; }

        [SerializeField] private Text _resultsText;
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
            sb.Append($"{entity.TotalActionsCount} действий, {entity.FailedActionsCount} ошибочных\n");
            sb.Append($"Верных действий: {Mathf.CeilToInt(entity.SuccessRate * 100)}%\n");
            sb.Append($"Время выполнения: {Mathf.FloorToInt(entity.TimeInSeconds / 60)} минут {Mathf.FloorToInt(entity.TimeInSeconds % 60)} секунд");
            _resultsText.text = sb.ToString();
        }
    }
}