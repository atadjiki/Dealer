public interface IEventReceiver
{
    public void HandleEvent(Constants.Enumerations.EventID eventID);
}
