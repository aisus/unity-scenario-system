using System;
using UnityEngine;

namespace TrainingSystem.Scripts.Infrastructure.Services.Utility.Logging
{
    public class UnityDebugLogger : ILogger
    {
        public void Log(string message, LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                    Debug.LogError(message);
                    break;
                case LogType.Assert:
                    Debug.LogAssertion(message);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(message);
                    break;
                case LogType.Log:
                    Debug.Log(message);
                    break;
                case LogType.Exception:
                    Debug.LogError(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void Log(Exception exception)
        {
            Debug.LogException(exception);
        }
    }
}