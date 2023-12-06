using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrubHubClone.Common.Authentication.Configuration;

public class AuthenticationConfiguration
{
    public string? Authority { get; set; }
    public string? Audience { get; set; }
}
