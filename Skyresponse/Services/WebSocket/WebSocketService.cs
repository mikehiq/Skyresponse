using Skyresponse.Persistence;
using Skyresponse.Services.User;

namespace Skyresponse.Services.WebSocket
{
    public class WebSocketService : IWebSocketService
    {
        private readonly IPersistenceManager _persistenceManager;
        private readonly IUserService _userService;

        public WebSocketService(IPersistenceManager persistenceManager,  IUserService userService)
        {
            _persistenceManager = persistenceManager;
            _userService = userService;
        }
    }
}
