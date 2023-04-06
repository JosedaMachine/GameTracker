using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTracker
{
    class JsonSerializer : ISerializer
    {
        string ISerializer.serialize(TrackerEvent e)
        {
            return e.toJson();
        }

        string ISerializer.getName() { return "data.json"; }
    }
}
