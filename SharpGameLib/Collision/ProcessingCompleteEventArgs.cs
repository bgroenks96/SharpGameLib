using System;

namespace SharpGameLib.Collision
{
    public class ProcessingCompleteEventArgs
    {
        public ProcessingCompleteEventArgs(TimeSpan totalProcessingTime, int processedEntityCount, int cellHitCount, int totalCellCount)
        {
            this.TotalProcessingTime = totalProcessingTime;
            this.ProcessedEntityCount = processedEntityCount;
            this.CellHitCount = cellHitCount;
            this.TotalCellCount = totalCellCount;
        }

        public TimeSpan TotalProcessingTime { get; }

        public int ProcessedEntityCount { get; }

        public int CellHitCount { get; }

        public int TotalCellCount { get; }
    }
}

