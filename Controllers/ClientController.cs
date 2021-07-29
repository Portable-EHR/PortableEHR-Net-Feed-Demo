// Copyright © Portable EHR inc, 2021
// https://portableehr.com/

using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PortableEHRNetFeedDemo.Models;
using PortableEHRNetSDK.Network.Client.Request;
using PortableEHRNetSDK.Network.Client.Request.Login;
using PortableEHRNetSDK.Network.Client.Request.Patient;
using PortableEHRNetSDK.Network.Client.Request.Privatemessage;
using PortableEHRNetSDK.Network.Client.Response;
using PortableEHRNetSDK.Network.Client.Response.Idissuers;
using PortableEHRNetSDK.Network.Client.Response.Login;
using PortableEHRNetSDK.Network.Client.Response.Patient;

namespace PortableEHRNetFeedDemo.Controllers
{
    [Route("client")]
    public class ClientController : Controller
    {
        private static readonly string BASE_URL = "https://localhost:3004";
        private static WebClient _webClient;
        private readonly ILogger<ClientController> _logger;
        private readonly State _state;


        public ClientController(ILogger<ClientController> logger, State state)
        {
            _logger = logger;
            _state = state;

            _webClient = new WebClient();
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, errors) => true;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
        }

        private string Call(string path, object request)
        {
            if (_state.clientJWTAuthToken != null)
                _webClient.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + _state.clientJWTAuthToken);

            _webClient.Headers.Add(HttpRequestHeader.Accept, "application/json");
            _webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            _webClient.Headers.Add(HttpRequestHeader.UserAgent, "PEHR .NET Feed Demo");

            return _webClient.UploadString(BASE_URL + path, "POST", JsonSerializer.Serialize(request));
        }

        [HttpPost]
        [Route("login")]
        public LoginResponse Login(string json)
        {
            _logger.LogInformation("Call /client/login with json: " + json);

            var request = (LoginRequest) JsonSerializer.Deserialize(json, typeof(LoginRequest));

            var responseString = Call("/login", request);
            var loginResponse = (LoginResponse) JsonSerializer.Deserialize(responseString, typeof(LoginResponse));
            _state.clientJWTAuthToken = loginResponse.responseContent.token;

            _logger.LogInformation(JsonSerializer.Serialize(loginResponse));
            return loginResponse;
        }

        [HttpPost]
        [Route("backend/patient/reachability")]
        public PatientReachabilityResponse CallPatientReachability(string json)
        {
            _logger.LogInformation("Call client/backend/patient/reachability with json: " + json);

            var request =
                (PatientReachabilityRequest) JsonSerializer.Deserialize(json, typeof(PatientReachabilityRequest));

            var responseString = Call("/backend/patient/reachability", request);
            var response =
                (PatientReachabilityResponse) JsonSerializer.Deserialize(responseString,
                    typeof(PatientReachabilityResponse));

            _logger.LogInformation(JsonSerializer.Serialize(response));
            return response;
        }

        [HttpPost]
        [Route("backend/privateMessage/notification")]
        public FeedHubApiResponse CallPrivateMessageNotifications(string json)
        {
            _logger.LogInformation("Call /client/backend/privateMessage/notification with json: " + json);

            var request =
                (PrivateMessageNotificationRequest) JsonSerializer.Deserialize(json,
                    typeof(PrivateMessageNotificationRequest));

            var responseString = Call("/backend/privateMessage/notification", request);
            var response = (FeedHubApiResponse) JsonSerializer.Deserialize(responseString, typeof(FeedHubApiResponse));

            _logger.LogInformation(JsonSerializer.Serialize(response));
            return response;
        }

        [HttpPost]
        [Route("backend/idissuers")]
        public IdIssuersResponse CallIdIssuers(string json)
        {
            _logger.LogInformation("Call /client/backend/idissuers with json: " + json);

            var request = (FeedBackendRequest) JsonSerializer.Deserialize(json, typeof(FeedBackendRequest));

            var responseString = Call("/backend/idissuers", request);
            var response = (IdIssuersResponse) JsonSerializer.Deserialize(responseString, typeof(IdIssuersResponse));

            _logger.LogInformation(JsonSerializer.Serialize(response));
            return response;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}