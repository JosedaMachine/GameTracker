using System;

namespace GameTracker
{
    internal class CSVSerializer : ISerializer
    {
        private string name_ = "data.csv";

        string ISerializer.serialize(TrackerEvent e)
        {
            return e.toCSV();
        }

        public void setName(string name)
        {
            name_ = name;
        }

        string ISerializer.getName() { return name_; }
    }
}
