using System;

namespace GameTracker
{
    internal class CSVSerializer : ISerializer
    {
        //TODO: cambiar a Tracker event
        string ISerializer.serialize(Event e)
        {
            //TODO: pedirselo a la persitencia + ponerlo en csv
            return e.toJson();
        }
    }
}
