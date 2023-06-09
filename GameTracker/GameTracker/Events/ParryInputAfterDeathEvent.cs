﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GameTracker
{
    internal class ParryInputAfterDeath : TrackerEvent
    {
        short level;
        float timeAfterDeath;
        public ParryInputAfterDeath(CommonContent common) : base(common)
        {
            eventType_ = "ParryInputAfterDeath";
        }

        public override string toCSV()
        {
            //Base information
            string format = base.toCSV();

            return format+","+ level + ","+ timeAfterDeath + "\n";
        }

        public override string toJSON()
        {
            //Base information
            string format = base.toJSON();

            //collection data
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(format);

            //Add data
            data["Level"] = level;
            data["TimeAfterDeath"] = timeAfterDeath;

            // Serialize collection with new data
            string newCollection = JsonConvert.SerializeObject(data, new JsonSerializerSettings { Formatting = Formatting.Indented });

            newCollection += ",\n";

            return newCollection;
        }

        public void setLevel(short level_)
        {
            level = level_;
        }

        public void setTimeAfterDeath(float time)
        {
            timeAfterDeath = time;
        }
    }
}
