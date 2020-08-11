using TrainingSystem.Scripts.Configuration;
using TrainingSystem.Scripts.Infrastructure.Services.DI;
using TrainingSystem.Scripts.Model;

namespace TrainingSystem.Scripts.Infrastructure.Services.Scenarios
{
    public interface IScenarioService : ISceneService
    {
        string[] GetObjectsToEnableOnCurrentStage();
        string[] GetObjectsToDisableOnCurrentStage();
        bool IsScenarioCompleted();
        ScenarioActionResult TryExecuteScenarioAction(InteractiveObjectEntity entity);
    }
}