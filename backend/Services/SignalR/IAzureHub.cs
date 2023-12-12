using Microsoft.AspNetCore.SignalR;

public interface IAzureHub
{
    public Task Send();
    Task OnConnectedAsync();
}