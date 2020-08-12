namespace TrainingSystem.Scripts.Model
{
    public class StatisticsEntity
    {
        public int TotalActionsCount { get; set; }

        public int FailedActionsCount { get; set; }

        public int SuccessfulActionsCount => TotalActionsCount - FailedActionsCount;

        public float TimeInSeconds { get; set; }

        public bool IsSuccessful => FailedActionsCount == 0;

        public float SuccessRate => (float) SuccessfulActionsCount / TotalActionsCount;
    }
}