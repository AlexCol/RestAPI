using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Configurations;

public class TokenConfiguration
{
    public string Audience { get; private set; }
    public string Issuer { get; private set; }
    public string Secret { get; private set; }
    public int Minutes { get; private set; }
    public int DaysToExpire { get; private set; }
}
