using TrainingSystem.Scripts.Configuration;
using TrainingSystem.Scripts.Infrastructure.Services.DI;
using TrainingSystem.Scripts.Infrastructure.Services.Interaction;
using TrainingSystem.Scripts.Infrastructure.Services.Statistics;
using TrainingSystem.Scripts.Infrastructure.Utility;
using TrainingSystem.Scripts.Model;
using TrainingSystem.Scripts.SceneInteraction;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using Logger = TrainingSystem.Scripts.Infrastructure.Utility.Logger;

namespace TrainingSystem.Scripts.UI.Scene
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private Selector _selector;
        [SerializeField] private Text _objectNameText;
        [SerializeField] private Text _currentStageText;

        [Header("Screens")] [SerializeField] private FailureScreenController _failureScreen;
        [SerializeField] private ResultsScreenController _resultsScreen;

        private IStatisticsService _statisticsService;
        private IInteractionService _interactionService;

        private MouseLook _mouseLook;

        private void Start()
        {
            _statisticsService = ServiceLocator.Current.ResolveDependency<IStatisticsService>();
            _interactionService = ServiceLocator.Current.ResolveDependency<IInteractionService>();

            _interactionService.OnScenarioCompleted += ScenarioCompletedHandler;
            _interactionService.OnActionFailed += ActionFailedHandler;

            _failureScreen.OnExitPressed += SceneLoader.LoadMainMenu;
            _failureScreen.OnContinuePressed += () => ShowFirstFailureScreen(false);
            _failureScreen.OnRestartPressed += SceneLoader.ReloadCurrentScene;
            _resultsScreen.OnExitPressed += SceneLoader.LoadMainMenu;
            _resultsScreen.OnRestartPressed += SceneLoader.ReloadCurrentScene;

            _mouseLook = FindObjectOfType<FirstPersonController>().MouseLook;
        }

        private void Update()
        {
            var currentSelectedObject = _selector.CurrentSelectedObject;
            _objectNameText.gameObject.SetActive(currentSelectedObject);
            if (currentSelectedObject)
            {
                _objectNameText.text =
                    TrainingPreferences.DisplayedObjectNames.GetNameByKey(currentSelectedObject.Entity.Key);
            }

            _currentStageText.text = _interactionService.ActiveStageName;
        }

        private void ScenarioCompletedHandler() => ShowResultsScreen();

        private void ActionFailedHandler(InteractiveObjectEntity entity)
        {
            if (_statisticsService.Statistics.FailedActionsCount <= 1) ShowFirstFailureScreen(true);
            ShowFailureMessage();
        }

        private void ShowFirstFailureScreen(bool param)
        {
            _mouseLook.SetCursorLock(!param);
            _failureScreen.gameObject.SetActive(param);
        }

        private void ShowFailureMessage()
        {
        }

        private void ShowResultsScreen()
        {
            _mouseLook.SetCursorLock(false);
            _statisticsService.Statistics.TimeInSeconds = Time.timeSinceLevelLoad;
            _resultsScreen.gameObject.SetActive(true);
            _resultsScreen.DisplayResults(_statisticsService.Statistics);
            Logger.Log(
                $"Completed in {_statisticsService.Statistics.TimeInSeconds}, success rate {_statisticsService.Statistics.SuccessRate}",
                LogType.Log);
        }
    }
}