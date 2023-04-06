namespace GameTracker
{
    interface ISerializer
    {
        //TODO: cambiar a TrackerEvent
        string serialize(TrackerEvent e);


        string getName();
    }
}
