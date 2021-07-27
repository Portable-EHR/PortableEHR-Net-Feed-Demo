// Copyright © Portable EHR inc, 2021
// https://portableehr.com/

using System;
using System.Collections.Generic;
using System.Text;

namespace PortableEHRNetFeedDemo.Models
{
    public class State
    {
            //region server
            public string serverLoginSelected { get; set; }
            public List<string> serverLoginOptions { get; set; } = new List<string>();
            public string serverPatientSingleSelected { get; set; }
            public string serverPatientBundleSelected { get; set; }
            public List<string> serverPatientOptions { get; set; } = new List<string>();
            public string serverPatientPehrReachabilitySelected { get; set; }
            public List<string> serverPatientPehrReachabilityOptions { get; set; } = new List<string>();
            public string serverPractitionerSingleSelected { get; set; }
            public string serverPractitionerBundleSelected { get; set; }
            public List<string> serverPractitionerOptions { get; set; } = new List<string>();
            public string serverPrivateMessageContentSelected { get; set; }
            public List<string> serverPrivateMessageContentOptions { get; set; } = new List<string>();
            public string serverPrivateMessageStatusSelected { get; set; }
            public List<string> serverPrivateMessageStatusOptions { get; set; } = new List<string>();
            public string serverAppointmentSingleSelected { get; set; }
            public string serverAppointmentBundleSelected { get; set; }
            public List<string> serverAppointmentOptions { get; set; } = new List<string>();
            public string serverAppointmentDispositionSelected { get; set; }
            public List<string> serverAppointmentDispositionsOptions { get; set; } = new List<string>();

            public StringBuilder serverLogs { get; set; } = new StringBuilder();

            public void addLogLine(string path, string answer, string msg)
            {
                DateTime date1 = DateTime.Now;
                serverLogs.Insert(0, date1.ToLongTimeString() + " " + path + " -> " + answer + " " + msg + Environment.NewLine);
            }
            
            //endregion
            
            //region client
            public string clientJWTAuthToken { get; set; }
            public string clientLoginRequestJson { get; set; }
            public string clientReachabilityRequestJson { get; set; }
            public string clientPrivateMessateNotificationRequestJson { get; set; }
            public string clientIDIssuersRequestJson { get; set; }
            //endregion
    }
}