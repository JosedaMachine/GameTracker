using System;
using UnityEngine;

namespace GameTracker
{
    interface IPersistence
    {
        void send(TrackerEvent e);
        void flush();

    }
}
