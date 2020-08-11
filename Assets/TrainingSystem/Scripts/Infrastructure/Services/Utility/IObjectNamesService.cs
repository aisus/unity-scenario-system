using TrainingSystem.Scripts.Infrastructure.Services.DI;

namespace TrainingSystem.Scripts.Infrastructure.Services.Utility
{
    /// <summary>
    /// Service to map system object keys to displayed names
    /// </summary>
    public interface IObjectNamesService : ISceneService
    {
        string GetNameByKey(string key);
        string GetKeyByName(string name);
    }
}