// Copyright © Portable EHR inc, 2021
// https://portableehr.com/

using System.Diagnostics;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PortableEHRNetFeedDemo.Models;

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