using System;

namespace JoergIsAGeek.Workshop.IdentityServer4.ConsumerWebApp.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}