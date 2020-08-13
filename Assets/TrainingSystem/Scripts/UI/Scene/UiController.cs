using TMPro;
using TrainingSystem.Scripts.Configuration;
using TrainingSystem.Scripts.Infrastructure.Services.DI;
using TrainingSystem.Scripts.Infrastructure.Services.Interaction;
using TrainingSystem.Scripts.Infrastructure.Services.Statistics;
using TrainingSystem.Scripts.Infrastructure.Utility;
using TrainingSystem.Scripts.Model;
using TrainingSystem.Scripts.SceneInteraction;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using Logger = TrainingSystem.Scripts.Infrastructure.Utility.Logger;

namespace TrainingSystem.Scripts.UI.Scene
{
    /// <summary>
    /// In-scene UI controller
    /// </summary>
    public class UiController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _objectNameText;
        [SerializeField] private TextMeshProUGUI _currentStageText;

        [Header("Screens")] [SerializeField] private FailureScreenController _failureScreen;
        [SerializeField]                     private ResultsScreenController _resultsScreen;

        private IStatisticsService  _statisticsService;
        private IInteractionService _interactionService;

        /// <summary>
        /// To enable|disable cursor for UI elements interaction
        /// </summary>
        private MouseLook _mouseLook;

        /// <summary>
        /// To display selected object name
        /// </summary>
        private InteractiveObjectsSelector _interactiveObjectsSelector;

        private void Start()
        {
            _statisticsService  = ServiceLocator.Current.ResolveDependency<IStatisticsService>();
            _interactionService = ServiceLocator.Current.ResolveDependency<IInteractionService>();

            _interactionService.OnScenarioCompleted += ScenarioCompletedHandler;
            _interactionService.OnActionFailed      += ActionFailedHandler;

            _failureScreen.OnExitPressed     += SceneLoader.LoadMainMenu;
            _failureScreen.OnContinuePressed += () => ShowFirstFailureScreen(false);
            _failureScreen.OnRestartPressed  += SceneLoader.ReloadCurrentScene;
            _resultsScreen.OnExitPressed     += SceneLoader.LoadMainMenu;
            _resultsScreen.OnRestartPressed  += SceneLoader.ReloadCurrentScene;

            _mouseLook                  = FindObjectOfType<FirstPersonController>().MouseLook;
            _interactiveObjectsSelector = FindObjectOfType<InteractiveObjectsSelector>();
        }

        private void Update()
        {
            UpdateSelectedObjectText();
            UpdateCurrentStageName();
        }

        private void UpdateSelectedObjectText()
        {
            var currentSelectedObject = _interactiveObjectsSelector.CurrentSelectedObject;
            _objectNameText.gameObject.SetActive(currentSelectedObject);
            if (currentSelectedObject)
            {
                _objectNameText.text =
                    TrainingPreferences.DisplayedObjectNames.GetNameByKey(currentSelectedObject.Entity.Key);
            }
        }

        private void UpdateCurrentStageName()
        {
            _currentStageText.text = _interactionService.CurrentScenarioStageName;
        }

        private void ScenarioCompletedHandler() => ShowResultsScreen();

        private void ActionFailedHandler(InteractiveObjectEntity entity)
        {
            if (_statisticsService.Statistics.FailedActionsCount <= 1) ShowFirstFailureScreen(true);
        }

        private void ShowFirstFailureScreen(bool param)
        {
            _mouseLook.SetCursorLock(!param);
            _failureScreen.gameObject.SetActive(param);
        }

        private void ShowResultsScreen()
        {
            _mouseLook.SetCursorLock(false);
            _interactionService.EnableInteraction(false);
            _statisticsService.Statistics.TimeInSeconds = Time.timeSinceLevelLoad;
            _resultsScreen.gameObject.SetActive(true);
            _resultsScreen.DisplayResults(_statisticsService.Statistics);
            Logger.Log(
                $"Completed in {_statisticsService.Statistics.TimeInSeconds}, success rate {_statisticsService.Statistics.SuccessRate}",
                LogType.Log);
        }
    }
}