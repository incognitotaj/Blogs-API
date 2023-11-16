using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models;

public class EmailSettings
{
    public EmailSettings(string apiKey, string fromEmail, string fromAddress)
    {
        ApiKey = apiKey;
        FromEmail = fromEmail;
        FromAddress = fromAddress;
    }

    public string ApiKey { get; set; }
    public string FromEmail { get; set; }
    public string FromAddress { get; set; }
}
