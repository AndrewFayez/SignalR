using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SendMessageForOneUser.Models;
using SendMessageForOneUser.Services;
using SendMessageForOneUser.wwwroot.Hubs;

namespace SendMessageForOneUser.Controllers
{
    public class AdminController : Controller
    {
        //private readonly IHubContext<NotificationHub> _notificationHubContext;
        private readonly IHubContext<NotificationUserHub> _notificationUserHubContext;
        private readonly IUserConnectionManager _userConnectionManager;
        public AdminController( IHubContext<NotificationUserHub> notificationUserHubContext, IUserConnectionManager userConnectionManager)
        {
          
            _notificationUserHubContext = notificationUserHubContext;
            _userConnectionManager = userConnectionManager;
            //_notificationHubContext = notificationHubContext;
        }

        public IActionResult SendToSpecificUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SendToSpecificUser(Article model)
        {
            var connections = _userConnectionManager.GetUserConnections(model.userId);
            if (connections != null && connections.Count > 0)
            {
                foreach (var connectionId in connections)
                {
                    await _notificationUserHubContext.Clients.Client(connectionId).SendAsync("sendToUser", model.articleHeading, model.articleContent);
                }
            }
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(Article model)
        {
            await _notificationUserHubContext.Clients.All.SendAsync("sendToUser", model.articleHeading, model.articleContent);
            return View();
        }



    }
}
