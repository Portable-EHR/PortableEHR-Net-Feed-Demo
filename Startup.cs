// Copyright © Portable EHR inc, 2021
// https://portableehr.com/

using System.IO;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortableEHRNetFeedDemo.Models;

namespace PortableEHRNetFeedDemo
{
    public class Startup
    {
        //SERVER
        public static string SERVER_LOGIN_RESPONSE_ROOT = "mocks" + Path.DirectorySeparatorChar + "feedResponses" +
                                                          Path.DirectorySeparatorChar + "login" +
                                                          Path.DirectorySeparatorChar + "post";

        public static string SERVER_PATIENT_RESPONSE_ROOT = "mocks" + Path.DirectorySeparatorChar + "feedResponses" +
                                                            Path.DirectorySeparatorChar + "patient" +
                                                            Path.DirectorySeparatorChar + "post";

        public static string SERVER_REACHABILITY_RESPONSE_ROOT =
            "mocks" + Path.DirectorySeparatorChar + "feedResponses" +
            Path.DirectorySeparatorChar + "patient" +
            Path.DirectorySeparatorChar + "pehrReachability" +
            Path.DirectorySeparatorChar + "post";

        public static string SERVER_PRATITIONER_RESPONSE_ROOT =
            "mocks" + Path.DirectorySeparatorChar + "feedResponses" +
            Path.DirectorySeparatorChar + "practitioner" +
            Path.DirectorySeparatorChar + "post";

        public static string SERVER_PM_CONTENT_RESPONSE_ROOT = "mocks" + Path.DirectorySeparatorChar + "feedResponses" +
                                                               Path.DirectorySeparatorChar + "privateMessage" +
                                                               Path.DirectorySeparatorChar + "content" +
                                                               Path.DirectorySeparatorChar + "post";

        public static string SERVER_PM_STATUS_RESPONSE_ROOT = "mocks" + Path.DirectorySeparatorChar + "feedResponses" +
                                                              Path.DirectorySeparatorChar + "privateMessage" +
                                                              Path.DirectorySeparatorChar + "status" +
                                                              Path.DirectorySeparatorChar + "post";

        public static string SERVER_APPOINTMENT_RESPONSE_ROOT =
            "mocks" + Path.DirectorySeparatorChar + "feedResponses" +
            Path.DirectorySeparatorChar + "appointment" +
            Path.DirectorySeparatorChar + "post";

        public static string SERVER_APPOINTMENT_DISPOSITION_RESPONSE_ROOT = "mocks" + Path.DirectorySeparatorChar +
                                                                            "feedResponses" +
                                                                            Path.DirectorySeparatorChar +
                                                                            "appointment" +
                                                                            Path.DirectorySeparatorChar +
                                                                            "disposition" +
                                                                            Path.DirectorySeparatorChar + "post";

        private readonly string CLIENT_IDISSUERS_REQUEST_JSON = "mocks" + Path.DirectorySeparatorChar + "feedRequests" +
                                                                Path.DirectorySeparatorChar + "backend" +
                                                                Path.DirectorySeparatorChar + "idIssuers" +
                                                                Path.DirectorySeparatorChar + "post" +
                                                                Path.DirectorySeparatorChar + "default.json";

        // CLIENT
        private readonly string CLIENT_LOGIN_REQUEST_JSON = "mocks" + Path.DirectorySeparatorChar + "feedRequests" +
                                                            Path.DirectorySeparatorChar + "login" +
                                                            Path.DirectorySeparatorChar +
                                                            "post" + Path.DirectorySeparatorChar + "default.json";

        private readonly string CLIENT_PRIVATE_MESSAGE_REQUEST_JSON =
            "mocks" + Path.DirectorySeparatorChar + "feedRequests" +
            Path.DirectorySeparatorChar + "backend" +
            Path.DirectorySeparatorChar + "privateMessage" +
            Path.DirectorySeparatorChar + "notifications" +
            Path.DirectorySeparatorChar + "post" +
            Path.DirectorySeparatorChar + "default.json";

        private readonly string CLIENT_REACHABILITY_REQUEST_JSON =
            "mocks" + Path.DirectorySeparatorChar + "feedRequests" +
            Path.DirectorySeparatorChar + "backend" +
            Path.DirectorySeparatorChar + "patient" +
            Path.DirectorySeparatorChar + "reachability" +
            Path.DirectorySeparatorChar + "post" +
            Path.DirectorySeparatorChar + "default.json";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(LoadState());
            services.AddControllersWithViews();
            services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
                .AddCertificate()
                .AddCertificateCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private State LoadState()
        {
            var state = new State();

            // server
            state.serverLoginSelected = "default.json";
            state.serverLoginOptions.Clear();
            foreach (var file in Directory.EnumerateFiles(SERVER_LOGIN_RESPONSE_ROOT))
                state.serverLoginOptions.Add(new FileInfo(file).Name);

            state.serverPatientSingleSelected = "single.json";
            state.serverPatientBundleSelected = "bundle_empty.json";
            state.serverPatientOptions.Clear();
            foreach (var file in Directory.EnumerateFiles(SERVER_PATIENT_RESPONSE_ROOT))
                state.serverPatientOptions.Add(new FileInfo(file).Name);

            state.serverPatientPehrReachabilitySelected = "default.json";
            state.serverPatientPehrReachabilityOptions.Clear();
            foreach (var file in Directory.EnumerateFiles(SERVER_REACHABILITY_RESPONSE_ROOT))
                state.serverPatientPehrReachabilityOptions.Add(new FileInfo(file).Name);

            state.serverPractitionerSingleSelected = "single.json";
            state.serverPractitionerBundleSelected = "bundle_empty.json";
            state.serverPractitionerOptions.Clear();
            foreach (var file in Directory.EnumerateFiles(SERVER_PRATITIONER_RESPONSE_ROOT))
                state.serverPractitionerOptions.Add(new FileInfo(file).Name);

            state.serverPrivateMessageContentSelected = "default.json";
            state.serverPrivateMessageContentOptions.Clear();
            foreach (var file in Directory.EnumerateFiles(SERVER_PM_CONTENT_RESPONSE_ROOT))
                state.serverPrivateMessageContentOptions.Add(new FileInfo(file).Name);

            state.serverPrivateMessageStatusSelected = "default.json";
            state.serverPrivateMessageStatusOptions.Clear();
            foreach (var file in Directory.EnumerateFiles(SERVER_PM_STATUS_RESPONSE_ROOT))
                state.serverPrivateMessageStatusOptions.Add(new FileInfo(file).Name);

            state.serverAppointmentSingleSelected = "single_confirmed.json";
            state.serverAppointmentBundleSelected = "bundle_empty.json";
            state.serverAppointmentOptions.Clear();
            foreach (var file in Directory.EnumerateFiles(SERVER_APPOINTMENT_RESPONSE_ROOT))
                state.serverAppointmentOptions.Add(new FileInfo(file).Name);

            state.serverAppointmentDispositionSelected = "default.json";
            state.serverAppointmentDispositionsOptions.Clear();
            foreach (var file in Directory.EnumerateFiles(SERVER_APPOINTMENT_DISPOSITION_RESPONSE_ROOT))
                state.serverAppointmentDispositionsOptions.Add(new FileInfo(file).Name);

            // client
            state.clientLoginRequestJson = File.ReadAllText(CLIENT_LOGIN_REQUEST_JSON);
            state.clientReachabilityRequestJson = File.ReadAllText(CLIENT_REACHABILITY_REQUEST_JSON);
            state.clientPrivateMessateNotificationRequestJson = File.ReadAllText(CLIENT_PRIVATE_MESSAGE_REQUEST_JSON);
            state.clientIDIssuersRequestJson = File.ReadAllText(CLIENT_IDISSUERS_REQUEST_JSON);

            return state;
        }
    }
}