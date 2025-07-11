
using Microsoft.AspNetCore.SignalR;
using pos_system.Models;

namespace pos_system.Services
{
    public class OrderHub : Hub
    {
        public async Task SendOrder(string orderId)
        {
            await Clients.All.SendAsync("ReceiveOrder", orderId);
        }
    }
}
