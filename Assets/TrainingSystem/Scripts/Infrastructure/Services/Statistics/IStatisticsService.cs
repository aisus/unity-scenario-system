using TrainingSystem.Scripts.Infrastructure.Services.DI;
using TrainingSystem.Scripts.Model;

namespace TrainingSystem.Scripts.Infrastructure.Services.Statistics
{
    /// <summary>
    /// Statistics service
    /// </summary>
    public interface IStatisticsService : IService
    {
        StatisticsEntity Statistics { get; }
    }
}