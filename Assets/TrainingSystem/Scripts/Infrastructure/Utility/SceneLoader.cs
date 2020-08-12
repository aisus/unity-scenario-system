using TrainingSystem.Scripts.Configuration;
using UnityEngine.SceneManagement;

namespace TrainingSystem.Scripts.Infrastructure.Utility
{
    public static class SceneLoader
    {
        public static void LoadMainMenu()
        {
            SceneManager.LoadScene(TrainingPreferences.MainMenuSceneName);
        }

        public static void LoadTrainingScene()
        {
            SceneManager.LoadScene(TrainingPreferences.TrainingSceneName);
        }

        public static void ReloadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}