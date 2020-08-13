using UnityEngine;

namespace TrainingSystem.Scripts.Configuration
{
    /// <summary>
    /// Static container to persist preferences between scenes
    /// </summary>
    public static class TrainingPreferences
    {
        public static string               Name                 { get; set; }
        public static string               MainMenuSceneName    { get; set; }
        public static string               TrainingSceneName    { get; set; }
        public static TrainingScenario     TrainingScenario     { get; set; }
        public static DisplayedObjectNames DisplayedObjectNames { get; set; }
        public static GameObject           TrainingSetupPrefab  { get; set; }
    }
}