using TrainingSystem.Scripts.Infrastructure.Preferences;
using TrainingSystem.Scripts.Infrastructure.Services.DI;
using TrainingSystem.Scripts.Infrastructure.Services.Interaction;
using TrainingSystem.Scripts.Infrastructure.Services.Statistics;
using TrainingSystem.Scripts.Infrastructure.Utility.SceneManagement;
using TrainingSystem.Scripts.Model;
using TrainingSystem.Scripts.SceneInteraction;
using UnityEngine;
using UnityEngine.UI;
using Logger = TrainingSystem.Scripts.Infrastructure.Utility.Logging.Logger;

namespace TrainingSystem.Scripts.UI
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private Selector _selector;
        [SerializeField] private Text _objectNameText;

        [Header("Screens")] [SerializeField] private FailureScreenController _failureScreen;
        [SerializeField] private ResultsScreenController _resultsScreen;

        private IStatisticsService _statisticsService;
        private IInteractionService _interactionService;

        private void Start()
        {
            _statisticsService = ServiceLocator.Current.ResolveDependency<IStatisticsService>();
            _interactionService = ServiceLocator.Current.ResolveDependency<IInteractionService>();

            _interactionService.OnScenarioCompleted += ScenarioCompletedHandler;
            _interactionService.OnActionFailed += ActionFailedHandler;

            _failureScreen.OnExitPressed += SceneLoader.LoadMainMenu;
            _failureScreen.OnContinuePressed += () => _failureScreen.gameObject.SetActive(false);
            _failureScreen.OnRestartPressed += SceneLoader.ReloadCurrentScene;
            _resultsScreen.OnExitPressed += SceneLoader.LoadMainMenu;
            _failureScreen.OnRestartPressed += SceneLoader.ReloadCurrentScene;
        }

        private void Update()
        {
            var currentSelectedObject = _selector.CurrentSelectedObject;
            _objectNameText.gameObject.SetActive(currentSelectedObject);
            if (currentSelectedObject)
            {
                _objectNameText.text =
                    GlobalPreferences.DisplayedObjectNames.GetNameByKey(currentSelectedObject.Entity.Key);
            }
        }

        private void ScenarioCompletedHandler() => ShowResultsScreen();

        private void ActionFailedHandler(InteractiveObjectEntity entity)
        {
            if (_statisticsService.Statistics.FailedActionsCount <= 1) ShowFirstFailureScreen();
            ShowFailureMessage();
        }

        private void ShowFirstFailureScreen()
        {
            _failureScreen.gameObject.SetActive(true);
        }

        private void ShowFailureMessage()
        {
        }

        private void ShowResultsScreen()
        {
            _statisticsService.Statistics.TimeInSeconds = Time.realtimeSinceStartup;
            _resultsScreen.gameObject.SetActive(true);
            _resultsScreen.DisplayResults(_statisticsService.Statistics);
            Logger.Log(
                $"Completed in {_statisticsService.Statistics.TimeInSeconds}, success rate {_statisticsService.Statistics.SuccessRate}",
                LogType.Log);
        }
    }
}