using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExemploDeConsumoDaRestAPIviaSSharp.Model;

public class Link
{
    public string Rel { get; set; }
    public string Href { get; set; }
    public string Type { get; set; }
    public string Action { get; set; }

}
