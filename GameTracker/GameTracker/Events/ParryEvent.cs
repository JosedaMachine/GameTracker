using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace GameTracker
{
    internal class ParryEvent : TrackerEvent
    {
        public ParryEvent(CommonContent common) : base(common){
            eventType_ = "Parry";
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

            //Add data
            data["Level"] = "Satanic Alberto";

            // Serialize collection with new data
            string newCollection = JsonConvert.SerializeObject(data, new JsonSerializerSettings { Formatting = Formatting.Indented });

            //Close file
            newCollection += "\n]";

            return newCollection;
        }

    }
}
