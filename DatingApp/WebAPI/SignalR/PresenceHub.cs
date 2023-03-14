using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Extensions;

namespace WebAPI.SignalR;

[Authorize]
public class PresenceHub : Hub
{
    private readonly PresenceTracker _presenceTracker;

    public PresenceHub(PresenceTracker presenceTracker)
    {
        _presenceTracker = presenceTracker;
    }
    
    public override async Task OnConnectedAsync()
    {
        var isOnline = await _presenceTracker.UserConnected(Context.User.GetUserName(), Context.ConnectionId);
        if (isOnline)
            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUserName());

        var currentUsers = await _presenceTracker.GetOnlineUsers();
        await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
        
        // await base.OnConnectedAsync();
    }
    
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var isOffline = await _presenceTracker.UserDisconnected(Context.User.GetUserName(), Context.ConnectionId);
        if (isOffline)
            await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUserName());
        
        // var currentUsers = await _presenceTracker.GetOnlineUsers();
        // await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
        
        await base.OnDisconnectedAsync(exception);
    }
}