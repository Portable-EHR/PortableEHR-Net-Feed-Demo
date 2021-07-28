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
using PortableEHRNetSDK.Network.Server.Request.Practitioner;
using PortableEHRNetSDK.Network.Server.Response;
using PortableEHRNetSDK.Network.Server.Response.Login;
using PortableEHRNetSDK.Network.Server.Response.Patient;
using PortableEHRNetSDK.Network.Server.Response.Practitioner;

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
        
        [HttpPost]
        [Route("patient/pehrReachability")]
        public FeedApiResponse PehrReachability([FromBody] PatientReachabilityRequest request)
        {
            _logger.LogInformation("/feed/patient/pehrReachability called");

            string selected = Startup.SERVER_REACHABILITY_RESPONSE_ROOT + Path.DirectorySeparatorChar +
                                          _state.serverPatientPehrReachabilitySelected;
            FeedApiResponse response = JsonSerializer.Deserialize(System.IO.File.ReadAllText(selected),
                    typeof(FeedApiResponse)) as FeedApiResponse;

            _state.addLogLine("/feed/patient/pehrReachability", selected, "OK");
            return response;
        }
        
        [HttpPost]
        [Route("practitioner")]
        public PractitionerPullResponse PullPractitioner([FromBody] PractitionerPullRequest request)
        {
            _logger.LogInformation("/feed/practitioner called");

            string selected = null;
            PractitionerPullResponse response = null;
            if (request.command.Equals("pullSingle"))
            {
                selected = Startup.SERVER_PRATITIONER_RESPONSE_ROOT + Path.DirectorySeparatorChar +
                           _state.serverPractitionerSingleSelected;
                response = JsonSerializer.Deserialize(System.IO.File.ReadAllText(selected),
                    typeof(PractitionerPullResponse)) as PractitionerPullResponse;
                ((PractitionerPullSingleResponseContent) response.responseContent).lastUpdated = DateTime.Now;
            }
            else if (request.command.Equals("pullBundle"))
            {
                selected = Startup.SERVER_PRATITIONER_RESPONSE_ROOT + Path.DirectorySeparatorChar +
                           _state.serverPractitionerBundleSelected;
                response = JsonSerializer.Deserialize(System.IO.File.ReadAllText(selected),
                    typeof(PractitionerPullResponse)) as PractitionerPullResponse;
                foreach (var practitioner in ((PractitionerPullBundleResponseContent)response.responseContent).results)
                {
                    practitioner.lastUpdated = DateTime.Now;
                }
            }

            _state.addLogLine("/feed/practitioner", selected, "OK");
            return response;
        }
    }
}