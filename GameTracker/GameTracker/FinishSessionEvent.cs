using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;

namespace GameTracker
{
    internal class FinishSessionEvent : TrackerEvent
    {
        public FinishSessionEvent(CommonContent common) : base(common){
            eventType_ = "Finish Session";
        }

        public override string toCSV()
        {
            //Base information
            string format = base.toCSV();

            return format + "\n";
        }

        public override string toJSON()
        {
            //Base information
            string format = base.toJSON();

            //extract collection data
            var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(format);

            // Serialize collection with new data
            string newCollection = System.Text.Json.JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });

            //Close file
            newCollection += "\n]";

            return newCollection;
        }

    }
}
