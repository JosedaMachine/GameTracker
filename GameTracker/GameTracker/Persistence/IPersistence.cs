using System;

namespace GameTracker
{
    interface IPersistence
    {
        void send(TrackerEvent e);
        void flush();

    }
}
