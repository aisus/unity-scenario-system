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
        string CurrentStageName { get; }

        /// <summary>
        /// Check conditions and try to switch to the next stage
        /// when actions is performed on interactive object 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        ScenarioActionResult TryExecuteScenarioAction(InteractiveObjectEntity entity);
    }
}