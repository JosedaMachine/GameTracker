using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTracker
{
    class JsonSerializer : ISerializer
    {
        //TODO: cambiar a Tracker event
        string ISerializer.serialize(Event e)
        {
            //TODO: pedirselo a la persitencia

            return e.toJson();
        }
    }
}
