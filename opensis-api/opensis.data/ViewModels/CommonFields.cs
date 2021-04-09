using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels
{
    public class CommonFields
    {
        public string _userName { get; set; }
        public string _tenantName { get; set; }

        public string _token { get; set; }
        public DateTimeOffset _tokenExpiry { get; set; }
        public bool _failure { get; set; }
        public string _message { get; set; }
    }
}
