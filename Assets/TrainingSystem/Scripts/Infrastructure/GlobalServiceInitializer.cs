using TrainingSystem.Scripts.Infrastructure.Services.DI;
using TrainingSystem.Scripts.Infrastructure.Services.Utility.Logging;

namespace TrainingSystem.Scripts.Infrastructure
{
    public static class GlobalServiceInitializer
    {
        public static void RegisterGlobalServices(ServiceLocator locator)
        {
            locator.RegisterService<ILogger>(new UnityDebugLogger());
        }
    }
}