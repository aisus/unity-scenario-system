using TrainingSystem.Scripts.Infrastructure.Services.DI;
using TrainingSystem.Scripts.Infrastructure.Services.Interaction;
using TrainingSystem.Scripts.Infrastructure.Services.Statistics;
using TrainingSystem.Scripts.Infrastructure.Services.Utility.ObjectNames;
using TrainingSystem.Scripts.Model;
using TrainingSystem.Scripts.SceneInteraction;
using UnityEngine;
using UnityEngine.UI;

namespace TrainingSystem.Scripts.UI
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private Selector _selector;
        [SerializeField] private Text _text;

        private IStatisticsService _statisticsService;
        private IInteractionService _interactionService;
        private IObjectNamesService _objectNamesService;

        private void Start()
        {
            _statisticsService = ServiceLocator.Current.ResolveDependency<IStatisticsService>();
            _interactionService = ServiceLocator.Current.ResolveDependency<IInteractionService>();
            _objectNamesService = ServiceLocator.Current.ResolveDependency<IObjectNamesService>();
            _interactionService.OnScenarioCompleted += ScenarioCompletedHandler;
            _interactionService.OnActionFailed += ActionFailedHandler;
        }

        private void Update()
        {
            _statisticsService.Statistics.TimeInSeconds = Time.realtimeSinceStartup;
            var currentSelectedObject = _selector.CurrentSelectedObject;
            _text.gameObject.SetActive(currentSelectedObject);
            if (currentSelectedObject)
            {
                _text.text = _objectNamesService.GetNameByKey(currentSelectedObject.Entity.Key);
            }
        }

        private void ScenarioCompletedHandler() => ShowScenarioStatistics();

        private void ActionFailedHandler(InteractiveObjectEntity entity)
        {
            if (_statisticsService.Statistics.FailedActionsCount <= 1) ShowRestartScreen();
            ShowFailureMessage();
        }

        private void ShowRestartScreen()
        {
            
        }
        
        private void ShowFailureMessage()
        {
            
        }
        
        private void ShowScenarioStatistics()
        {
            Debug.Log($"Completed in {_statisticsService.Statistics.TimeInSeconds}, success rate {_statisticsService.Statistics.SuccessRate}");
        }
    }
}