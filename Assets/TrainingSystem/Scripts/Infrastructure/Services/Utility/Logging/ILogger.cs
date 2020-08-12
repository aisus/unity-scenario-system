using System;
using TrainingSystem.Scripts.Infrastructure.Services.DI;
using UnityEngine;

namespace TrainingSystem.Scripts.Infrastructure.Services.Utility.Logging
{
    public interface ILogger : IService
    {
        void Log(string message, LogType type);
        void Log(Exception exception);
    }
}