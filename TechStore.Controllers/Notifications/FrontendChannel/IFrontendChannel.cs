namespace TechStore.Controllers.Notifications.FrontendChannel;

public interface IFrontendChannel
{
    public Task Send(FrontendChannelNotification notification);
}