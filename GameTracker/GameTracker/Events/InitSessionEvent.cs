using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace GameTracker
{
    internal class InitSessionEvent : TrackerEvent
    {
        public InitSessionEvent(CommonContent common) : base(common){
            eventType_ = "Init Session";
        }

        public override string toCSV()
        {
            //Init de CSV
            string legend = "GameID,SessionID,UserID,TimeStamp,EventType,Params\n";

            //Base information
            string format = base.toCSV();


            return legend + format + "\n";
        }

        public override string toJSON()
        {
            //Init de JSON
            string open = "[\n";

            //Base information
            string format = base.toJSON();

            //collection data
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(format);

            // Serialize collection with new data
            string newCollection = JsonConvert.SerializeObject(data, new JsonSerializerSettings { Formatting = Formatting.Indented });

            newCollection = open + newCollection;

            newCollection += ",\n";

            return newCollection;
        }

    }
}
