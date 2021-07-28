using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PortableEHRNetFeedDemo.Models;
using PortableEHRNetSDK.Model.Server;
using PortableEHRNetSDK.Network.Server.Request.Login;
using PortableEHRNetSDK.Network.Server.Request.Patient;
using PortableEHRNetSDK.Network.Server.Response.Login;
using PortableEHRNetSDK.Network.Server.Response.Patient;

namespace PortableEHRNetFeedDemo.Controllers
{
    [Route("feed")]
    public class FeedApiController : Controller
    {
        private readonly ILogger<FeedApiController> _logger;
        private readonly State _state;
        
        public FeedApiController(ILogger<FeedApiController> logger, State state)
        {
            _logger = logger;
            _state = state;
        }
        
        
        [HttpPost]
        [Route("patient")]
        public PatientPullResponse PullPatient([FromBody] PatientPullRequest request)
        {
            _logger.LogInformation("/feed/patient called");

            string selected = null;
            PatientPullResponse response = null;
            if (request.command.Equals("pullSingle"))
            {
                selected = Startup.SERVER_PATIENT_RESPONSE_ROOT + Path.DirectorySeparatorChar +
                              _state.serverPatientSingleSelected;
                response = JsonSerializer.Deserialize(System.IO.File.ReadAllText(selected),
                    typeof(PatientPullResponse)) as PatientPullResponse;
                ((PatientPullSingleResponseContent) response.responseContent).lastUpdated = DateTime.Now;
            }
            else if (request.command.Equals("pullBundle"))
            {
                selected = Startup.SERVER_PATIENT_RESPONSE_ROOT + Path.DirectorySeparatorChar +
                           _state.serverPatientBundleSelected;
                response = JsonSerializer.Deserialize(System.IO.File.ReadAllText(selected),
                    typeof(PatientPullResponse)) as PatientPullResponse;
                foreach (var patient in ((PatientPullBundleResponseContent)response.responseContent).results)
                {
                    patient.lastUpdated = DateTime.Now;
                }
            }

            _state.addLogLine("/feed/patient", selected, "OK");
            return response;
        }
        
    }
}