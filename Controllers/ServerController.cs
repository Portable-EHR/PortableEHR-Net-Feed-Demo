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
        public ActionResult SelectPatientSingle(string option)
        {
            _logger.LogInformation("Call patientSingle with option: " + option);
            _state.serverPatientSingleSelected = option;
            return new EmptyResult();
        }
        
        [HttpPut]
        [Route("feed/patientBundle")]
        public ActionResult SelectPatientBundle(string option)
        {
            _logger.LogInformation("Call patientBundle with option: " + option);
            _state.serverPatientBundleSelected = option;
            return new EmptyResult();
        }
        
        [HttpPut]
        [Route("feed/patient/pehrReachability")]
        public ActionResult SelectPatientPEHRReachability(string option)
        {
            _logger.LogInformation("Call pehrReachability with option: " + option);
            _state.serverPatientPehrReachabilitySelected = option;
            return new EmptyResult();
        }
        
        [HttpPut]
        [Route("feed/practitionerSingle")]
        public ActionResult SelectPractitionerSingle(string option)
        {
            _logger.LogInformation("Call practitionerSingle with option: " + option);
            _state.serverPatientSingleSelected = option;
            return new EmptyResult();
        }
        
        [HttpPut]
        [Route("feed/practitionerBundle")]
        public ActionResult SelectPractitionerBundle(string option)
        {
            _logger.LogInformation("Call practitionerBundle with option: " + option);
            _state.serverPractitionerBundleSelected = option;
            return new EmptyResult();
        }
        
        [HttpPut]
        [Route("feed/privateMessage/content")]
        public ActionResult SelectPrivateMessageContent(string option)
        {
            _logger.LogInformation("Call privateMessage/content with option: " + option);
            _state.serverPrivateMessageContentSelected = option;
            return new EmptyResult();
        }
        
        [HttpPut]
        [Route("feed/privateMessage/status")]
        public ActionResult SelectStatus(string option)
        {
            _logger.LogInformation("Call privateMessage/status with option: " + option);
            _state.serverPrivateMessageStatusSelected = option;
            return new EmptyResult();
        }
        
        [HttpPut]
        [Route("feed/appointmentSingle")]
        public ActionResult SelectAppointmentSingle(string option)
        {
            _logger.LogInformation("Call appointmentSingle with option: " + option);
            _state.serverAppointmentSingleSelected = option;
            return new EmptyResult();
        }
        
        [HttpPut]
        [Route("feed/appointmentBundle")]
        public ActionResult SelectAppointmentBundle(string option)
        {
            _logger.LogInformation("Call appointmentBundle with option: " + option);
            _state.serverAppointmentBundleSelected = option;
            return new EmptyResult();
        }
        
        [HttpPut]
        [Route("feed/appointment/disposition")]
        public ActionResult SelectAppointmentDisposition(string option)
        {
            _logger.LogInformation("Call appointment/disposition with option: " + option);
            _state.serverAppointmentDispositionSelected = option;
            return new EmptyResult();
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