// Copyright © Portable EHR inc, 2021
// https://portableehr.com/

namespace PortableEHRNetFeedDemo.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}