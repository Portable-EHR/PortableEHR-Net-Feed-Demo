using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PortableEHRNetFeedDemo.Models;
using PortableEHRNetSDK.Network.Server.Request.Login;
using PortableEHRNetSDK.Network.Server.Response.Login;

namespace PortableEHRNetFeedDemo.Controllers
{
    public class LoginApiController : Controller
    {
        private readonly ILogger<LoginApiController> _logger;
        private readonly State _state;
        
        public LoginApiController(ILogger<LoginApiController> logger, State state)
        {
            _logger = logger;
            _state = state;
        }
        
        
        [HttpPost]
        [Route("login")]
        public LoginResponse Login(LoginRequest request)
        {
            _logger.LogInformation("/login called");

            string selected = Startup.SERVER_LOGIN_RESPONSE_ROOT + Path.DirectorySeparatorChar +
                              _state.serverLoginSelected;
            var loginResponse =
                JsonSerializer.Deserialize(System.IO.File.ReadAllText(selected),
                    typeof(LoginResponse)) as LoginResponse;

            _state.addLogLine("/login", selected, "OK");
            return loginResponse;
        }
        
    }
}