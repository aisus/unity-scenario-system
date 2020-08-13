using TrainingSystem.Scripts.Enums;
using TrainingSystem.Scripts.Model;

namespace TrainingSystem.Scripts.Infrastructure.Services.Scenarios
{
    /// <summary>
    /// Service to keep current training scenario, check conditions
    /// when actions executed, switch stages
    /// </summary>
    public interface IScenarioService
    {
        string ActiveStageName { get; }
        string[] GetObjectsToEnableOnCurrentStage();
        string[] GetObjectsToDisableOnCurrentStage();
        bool IsScenarioCompleted();
        ScenarioActionResult TryExecuteScenarioAction(InteractiveObjectEntity entity);
    }
}