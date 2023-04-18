using Microsoft.AspNetCore.SignalR;

namespace MentalHealthApp.WebApp.WebRTCHub
{
    public class WebRTCHub : Hub
    {
        public async Task Join(int uuid)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, uuid.ToString());
            await Clients.Group(uuid.ToString()).SendAsync("joinedRoom", uuid);
        }

        public async Task LeaveRoom(int roomId)
        {
            await Clients.Group(roomId.ToString()).SendAsync("leftRoom");
        }

        public async Task SendMessage(int roomId, object message)
        {
            await Clients.OthersInGroup(roomId.ToString()).SendAsync("message", message);
        }
    }
}
