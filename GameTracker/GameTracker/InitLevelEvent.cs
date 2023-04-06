using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;

namespace GameTracker
{
    internal class InitlLevelEvent : TrackerEvent
    {
        public InitlLevelEvent(CommonContent common) : base(common){
            eventType_ = "Init Level";
        }

        public override string toCSV()
        {
            string format = base.toCSV();

            //formatear los datos
            format += ",Satanic Alberto";

            return  format + "\n";
        }

        public override string toJSON()
        {
            //Base information
            string format = base.toJSON();

            //collection data
            var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(format);

            //Add data
            data["Level"] = "Satanic Alberto";

            // Serialize collection with new data
            string newColleciton = System.Text.Json.JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });

            newColleciton += ",\n";

            return newColleciton;
        }

    }
}
