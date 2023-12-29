using System.Text;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Hypermedia.Constants;

namespace RestAPI.Hypermedia.Extensions
{
    public static class PrepareLink
    {
        public static string getLink(this string path, long id, IUrlHelper urlHelper, string format)
        {

            var url = new { controller = path, id };
            string completePath = new StringBuilder(urlHelper.Link("DefaultApi", url)).Replace("%2F", "/").ToString();
            if (format == HttpActionVerb.PUT || format == HttpActionVerb.POST)
            {
                completePath = completePath.Substring(0, completePath.LastIndexOf('/'));
            }
            return completePath;
        }
    }
}