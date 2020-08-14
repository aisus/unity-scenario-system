using System.Linq;
using TMPro;
using TrainingSystem.Scripts.Configuration;
using TrainingSystem.Scripts.Infrastructure;
using TrainingSystem.Scripts.Infrastructure.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace TrainingSystem.Scripts.UI.Menu
{
    /// <summary>
    /// Main menu UI controller
    /// </summary>
    public class MenuUiController : MonoBehaviour
    {
        [SerializeField] private Button                         _exitButton;
        [SerializeField] private GameObject                     _scenarioStartButtonPrefab;
        [SerializeField] private TrainingPreferencesInitializer _initializer;

        private void Awake()
        {
            _initializer.Data.ToList().ForEach(InitScenarioStartButton);
            _exitButton.onClick.AddListener(Application.Quit);
            _exitButton.transform.SetAsLastSibling();
        }

        /// <summary>
        /// Init new scenario start button
        /// </summary>
        /// <param name="scenario"></param>
        private void InitScenarioStartButton(ScenarioPreferences scenario)
        {
            var button = Instantiate(_scenarioStartButtonPrefab, _exitButton.transform.parent).GetComponent<Button>();
            button.GetComponentInChildren<TextMeshProUGUI>().text = scenario.Name;
            button.onClick.AddListener(() =>
            {
                _initializer.SetupPreferences(scenario);
                SceneLoader.LoadTrainingScene();
            });
        }
    }
}