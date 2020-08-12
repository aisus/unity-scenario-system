using TrainingSystem.Scripts.Configuration;

namespace TrainingSystem.Scripts.Infrastructure.Services.Preferences
{
    public static class GlobalPreferences
    {
        public static TrainingScenario SelectedScenario { get; set; }
        public static string MainMenuSceneName { get; set; }
        public static string TrainingSceneName { get; set; }
    }
}