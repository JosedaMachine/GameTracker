using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTracker
{
    interface ISerializer
    {
        //TODO: cambiar a TrackerEvent
        string serialize(Event e);
    }
}
