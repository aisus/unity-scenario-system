using TrainingSystem.Scripts.Infrastructure.Services.Preferences;
using UnityEngine.SceneManagement;

namespace TrainingSystem.Scripts.Infrastructure.Services.Utility.SceneManagement
{
    public static class SceneLoader
    {
        public static void LoadMainMenu()
        {
            SceneManager.LoadScene(GlobalPreferences.MainMenuSceneName);
        }

        public static void LoadTrainingScene()
        {
            SceneManager.LoadScene(GlobalPreferences.TrainingSceneName);
        }

        public static void ReloadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}