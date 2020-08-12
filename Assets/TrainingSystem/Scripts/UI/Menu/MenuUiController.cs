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
            var names = _initializer.Data.Select(x => x.Name).ToList();
            names.ForEach(InitScenarioStartButton);
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