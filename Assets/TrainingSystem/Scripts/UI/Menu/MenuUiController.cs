using System.Linq;
using TrainingSystem.Scripts.Infrastructure;
using TrainingSystem.Scripts.Infrastructure.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace TrainingSystem.Scripts.UI.Menu
{
    public class MenuUiController : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private GameObject _scenarioStartButtonPrefab;
        [SerializeField] private TrainingPreferencesInitializer _initializer;

        private void Awake()
        {
            var scenarioNames = _initializer.Data.Select(x => x.Name).ToList();
            scenarioNames.ForEach(InitScenarioStartButton);
            _exitButton.onClick.AddListener(Application.Quit);
        }

        private void InitScenarioStartButton(string scenarioName)
        {
            var button = Instantiate(_scenarioStartButtonPrefab, _exitButton.transform.parent).GetComponent<Button>();
            button.GetComponentInChildren<Text>().text = scenarioName;
            button.onClick.AddListener(() =>
            {
                _initializer.SetupPreferences(scenarioName);
                SceneLoader.LoadTrainingScene();
            });
        }
    }
}