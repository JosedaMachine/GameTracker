using System;

namespace GameTracker
{
    internal class CSVSerializer : ISerializer
    {
    
        string ISerializer.serialize(TrackerEvent e)
        {
            return e.toCSV();
        }

        string ISerializer.getName() { return "data.csv"; }
    }
}
