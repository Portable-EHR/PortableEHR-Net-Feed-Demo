using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PortableEHRNetFeedDemo.Models;
using PortableEHRNetSDK.Model.Server;
using PortableEHRNetSDK.Network.Server.Request.Appointment;
using PortableEHRNetSDK.Network.Server.Request.Login;
using PortableEHRNetSDK.Network.Server.Request.Patient;
using PortableEHRNetSDK.Network.Server.Request.Practitioner;
using PortableEHRNetSDK.Network.Server.Request.Privatemessage;
using PortableEHRNetSDK.Network.Server.Response;
using PortableEHRNetSDK.Network.Server.Response.Appointment;
using PortableEHRNetSDK.Network.Server.Response.Login;
using PortableEHRNetSDK.Network.Server.Response.Patient;
using PortableEHRNetSDK.Network.Server.Response.Practitioner;
using PortableEHRNetSDK.Network.Server.Response.Privatemessage;

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
        
        [HttpPost]
        [Route("privateMessage/content")]
        public PrivateMessageContentResponse PrivateMethodContent([FromBody] PrivateMessageContentRequest request)
        {
            _logger.LogInformation("/feed/privateMessage/content called");

            string selected = Startup.SERVER_PM_CONTENT_RESPONSE_ROOT + Path.DirectorySeparatorChar +
                              _state.serverPrivateMessageContentSelected;
            PrivateMessageContentResponse response = JsonSerializer.Deserialize(System.IO.File.ReadAllText(selected),
                typeof(PrivateMessageContentResponse)) as PrivateMessageContentResponse;

            _state.addLogLine("/feed/privateMessage/content", selected, "OK");
            return response;
        }
        
        [HttpPost]
        [Route("privateMessage/status")]
        public PrivateMessageContentResponse PrivateMethodStatus([FromBody] PrivateMessageContentRequest request)
        {
            _logger.LogInformation("/feed/privateMessage/status called");

            string selected = Startup.SERVER_PM_STATUS_RESPONSE_ROOT + Path.DirectorySeparatorChar +
                              _state.serverPrivateMessageStatusSelected;
            PrivateMessageContentResponse response = JsonSerializer.Deserialize(System.IO.File.ReadAllText(selected),
                typeof(PrivateMessageContentResponse)) as PrivateMessageContentResponse;

            _state.addLogLine("/feed/privateMessage/status", selected, "OK");
            return response;
        }
        
        [HttpPost]
        [Route("appointment")]
        public AppointmentPullResponse Appointment([FromBody] AppointmentPullRequest request)
        {
            _logger.LogInformation("/feed/appointment called");

            string selected = null;
            AppointmentPullResponse response = null;
            if (request.command.Equals("pullSingle"))
            {
                selected = Startup.SERVER_APPOINTMENT_RESPONSE_ROOT + Path.DirectorySeparatorChar +
                           _state.serverAppointmentSingleSelected;
                response = JsonSerializer.Deserialize(System.IO.File.ReadAllText(selected),
                    typeof(AppointmentPullResponse)) as AppointmentPullResponse;
                
                AppointmentPullSingleResponseContent appointment = ((AppointmentPullSingleResponseContent) response.responseContent);
                appointment.feedItemId = Guid.NewGuid();
                appointment.id = appointment.feedItemId.ToString();
                appointment.lastUpdated = DateTime.Now;
                appointment.startTime = DateTime.Now.Add(TimeSpan.FromHours(48));
                appointment.endTime = DateTime.Now.Add(TimeSpan.FromHours(49));
            }
            else if (request.command.Equals("pullBundle"))
            {
                selected = Startup.SERVER_APPOINTMENT_RESPONSE_ROOT + Path.DirectorySeparatorChar +
                           _state.serverAppointmentBundleSelected;
                response = JsonSerializer.Deserialize(System.IO.File.ReadAllText(selected),
                    typeof(AppointmentPullResponse)) as AppointmentPullResponse;
                foreach (var appointment in ((AppointmentPullBundleResponseContent)response.responseContent).results)
                {
                    appointment.feedItemId = Guid.NewGuid();
                    appointment.id = appointment.feedItemId.ToString();
                    appointment.lastUpdated = DateTime.Now;
                    appointment.startTime = DateTime.Now.Add(TimeSpan.FromHours(48));
                    appointment.endTime = DateTime.Now.Add(TimeSpan.FromHours(49));
                }
            }

            _state.addLogLine("/feed/appointment", selected, "OK");
            return response;
        }
        
        [HttpPost]
        [Route("appointment/disposition")]
        public AppointmentDispositionResponse AppointmentDisposition([FromBody] AppointmentDispositionRequest request)
        {
            _logger.LogInformation("/feed/appointment/disposition called");

            string selected = Startup.SERVER_APPOINTMENT_DISPOSITION_RESPONSE_ROOT + Path.DirectorySeparatorChar +
                              _state.serverAppointmentDispositionSelected;
            AppointmentDispositionResponse response = JsonSerializer.Deserialize(System.IO.File.ReadAllText(selected),
                typeof(AppointmentDispositionResponse)) as AppointmentDispositionResponse;

            _state.addLogLine("/feed/appointment/disposition", selected, "OK");
            return response;
        }
    }
}