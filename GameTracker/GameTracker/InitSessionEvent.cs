﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;

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
            var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(format);

            // Serialize collection with new data
            string newColleciton = System.Text.Json.JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });

            newColleciton = open + newColleciton;

            newColleciton += ",\n";

            return newColleciton;
        }

    }
}
