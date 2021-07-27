using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PortableEHRNetFeedDemo.Models;
using PortableEHRNetSDK.Network.Client.Request.Login;

namespace PortableEHRNetFeedDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly State _state;
        
        public HomeController(ILogger<HomeController> logger, State state)
        {
            _logger = logger;
            _state = state;
        }

        [Route("")]
        public IActionResult Index()
        {
            dynamic context = new ExpandoObject();
            
            context.state = _state;
            
            return View(context);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}