using System;
using UnityEngine;

namespace GameTracker
{
    interface IPersistence
    {
        //Serializer;

        // TODO: Cambiar a TrackerEvent
        void send(Event e);
        void flush();

    }
}
