using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Skyresponse.Persistence;
using Skyresponse.Services.User;
using Skyresponse.Wrappers.DialogWrappers;
using Skyresponse.Wrappers.HttpWrappers;

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
