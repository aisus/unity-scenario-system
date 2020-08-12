using TrainingSystem.Scripts.Configuration;

namespace TrainingSystem.Scripts.Infrastructure.Preferences
{
    public static class GlobalPreferences
    {
        public static TrainingScenario SelectedScenario { get; set; }
        
        public static DisplayedObjectNames DisplayedObjectNames { get; set; }
        public static string MainMenuSceneName { get; set; }
        public static string TrainingSceneName { get; set; }
    }
}