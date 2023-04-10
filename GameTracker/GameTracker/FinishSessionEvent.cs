using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

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
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(format);

            // Serialize collection with new data
            string newCollection = JsonConvert.SerializeObject(data, new JsonSerializerSettings { Formatting = Formatting.Indented });

            //Close file
            newCollection += "\n]";

            return newCollection;
        }

    }
}
