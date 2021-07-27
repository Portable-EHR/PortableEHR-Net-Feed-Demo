using System;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
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
        private readonly ILogger<ClientController> _logger;
        private readonly State _state;

        private static string BASE_URL = "https://localhost:3004";
        private static WebClient _webClient;
        
        
        public ClientController(ILogger<ClientController> logger, State state)
        {
            _logger = logger;
            _state = state;
            
            _webClient = new WebClient();
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, errors) => true;
            ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) { return true; };
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
        }
        
        private string Call(string path, Object request)
        {
            if (_state.clientJWTAuthToken != null)
            {
                _webClient.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + _state.clientJWTAuthToken);
            }

            _webClient.Headers.Add(HttpRequestHeader.Accept,"application/json");
            _webClient.Headers.Add(HttpRequestHeader.ContentType,"application/json");
            _webClient.Headers.Add(HttpRequestHeader.UserAgent, "PEHR .NET Feed Demo");

            return _webClient.UploadString(BASE_URL + path, "POST", JsonSerializer.Serialize(request));
        }
        
        [HttpPost]
        [Route("login")]
        public LoginResponse Login(string json)
        {
            _logger.LogInformation("Call /client/login with json: " + json);
            
            LoginRequest request = (LoginRequest) JsonSerializer.Deserialize(json, typeof(LoginRequest));
            
            string responseString = Call("/login", request);
            LoginResponse loginResponse = (LoginResponse) JsonSerializer.Deserialize(responseString, typeof(LoginResponse));
            _state.clientJWTAuthToken = loginResponse.responseContent.token;

            _logger.LogInformation(JsonSerializer.Serialize(loginResponse));
            return loginResponse;
        }

        [HttpPost]
        [Route("backend/patient/reachability")]
        public PatientReachabilityResponse CallPatientReachability(string json)
        {
            _logger.LogInformation("Call client/backend/patient/reachability with json: " + json);
            
            PatientReachabilityRequest request = (PatientReachabilityRequest) JsonSerializer.Deserialize(json, typeof(PatientReachabilityRequest));

            string responseString = Call("/backend/patient/reachability", request);
            PatientReachabilityResponse response = (PatientReachabilityResponse) JsonSerializer.Deserialize(responseString, typeof(PatientReachabilityResponse));

            _logger.LogInformation(JsonSerializer.Serialize(response));
            return response;
        }
        
        [HttpPost]
        [Route("backend/privateMessage/notification")]
        public FeedHubApiResponse CallPrivateMessageNotifications(string json)
        {
            _logger.LogInformation("Call /client/backend/privateMessage/notification with json: " + json);
            
            PrivateMessageNotificationRequest request = (PrivateMessageNotificationRequest) JsonSerializer.Deserialize(json, typeof(PrivateMessageNotificationRequest));

            string responseString = Call("/backend/privateMessage/notification", request);
            FeedHubApiResponse response = (FeedHubApiResponse) JsonSerializer.Deserialize(responseString, typeof(FeedHubApiResponse));

            _logger.LogInformation(JsonSerializer.Serialize(response));
            return response;
        }
        
        [HttpPost]
        [Route("backend/idissuers")]
        public IdIssuersResponse CallIdIssuers(string json)
        {
            _logger.LogInformation("Call /client/backend/idissuers with json: " + json);
            
            FeedBackendRequest request = (FeedBackendRequest) JsonSerializer.Deserialize(json, typeof(FeedBackendRequest));

            string responseString = Call("/backend/idissuers", request);
            IdIssuersResponse response = (IdIssuersResponse) JsonSerializer.Deserialize(responseString, typeof(IdIssuersResponse));

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