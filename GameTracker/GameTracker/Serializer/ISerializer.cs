namespace GameTracker
{
    interface ISerializer
    {
        string serialize(TrackerEvent e);

        string getName();
    }
}
