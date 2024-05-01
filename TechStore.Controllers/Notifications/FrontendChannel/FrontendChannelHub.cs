using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TechStore.Controllers.Notifications.FrontendChannel;

[AllowAnonymous]
public class FrontendChannelHub : Hub<IFrontendChannel>
{
}