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
    [Route("server")]
    public class ServerController : Controller
    {
        private readonly ILogger<ServerController> _logger;
        private readonly State _state;
        
        public ServerController(ILogger<ServerController> logger, State state)
        {
            _logger = logger;
            _state = state;
        }

        public IActionResult Index()
        {
            dynamic context = new ExpandoObject();
            
            context.state = _state;
            
            return View(context);
        }

        [HttpPut]
        [Route("login")]
        public ActionResult Login(string option)
        {
            _logger.LogInformation("Call /server/login with option: " + option);
            _state.serverLoginSelected = option;
            return new EmptyResult();
        }
        
        [HttpPut]
        [Route("feed/patientSingle")]
        public void SelectPatientSingle(string option)
        {
            _logger.LogInformation("Call patientSingle with option: " + option);
        }
        
        [HttpPut]
        [Route("feed/patientBundle")]
        public void SelectPatientBundle(string option)
        {
            _logger.LogInformation("Call patientBundle with option: " + option);
        }
        
        [HttpPut]
        [Route("feed/patient/pehrReachability")]
        public void SelectPatientPEHRReachability(string option)
        {
            _logger.LogInformation("Call pehrReachability with option: " + option);
        }
        
        [HttpPut]
        [Route("feed/practitionerSingle")]
        public void SelectPractitionerSingle(string option)
        {
            _logger.LogInformation("Call practitionerSingle with option: " + option);
        }
        
        [HttpPut]
        [Route("feed/practitionerBundle")]
        public void SelectPractitionerBundle(string option)
        {
            _logger.LogInformation("Call practitionerBundle with option: " + option);
        }
        
        [HttpPut]
        [Route("feed/privateMessage/content")]
        public void SelectPrivateMessageContent(string option)
        {
            _logger.LogInformation("Call privateMessage/content with option: " + option);
        }
        
        [HttpPut]
        [Route("feed/privateMessage/status")]
        public void SelectStatus(string option)
        {
            _logger.LogInformation("Call privateMessage/status with option: " + option);
        }
        
        [HttpPut]
        [Route("feed/appointmentSingle")]
        public void SelectAppointmentSingle(string option)
        {
            _logger.LogInformation("Call appointmentSingle with option: " + option);
        }
        
        [HttpPut]
        [Route("feed/appointmentBundle")]
        public void SelectAppointmentBundle(string option)
        {
            _logger.LogInformation("Call appointmentBundle with option: " + option);
        }
        
        [HttpPut]
        [Route("feed/appointment/disposition")]
        public void SelectAppointmentDisposition(string option)
        {
            _logger.LogInformation("Call appointment/disposition with option: " + option);
        }
        
        [HttpGet]
        [Route("logs")]
        public String GetServerLogs()
        {
            return _state.serverLogs.ToString();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}