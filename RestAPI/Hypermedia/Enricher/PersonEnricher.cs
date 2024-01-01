using Microsoft.AspNetCore.Mvc;
using RestAPI.Hypermedia.Constants;
using RestAPI.Hypermedia.Extensions;

namespace RestAPI.Hypermedia.Enricher
{
    public class PersonEnricher : ContentResponseEnricher<PersonVO>
    {
        protected override Task EnrichModel(PersonVO content, IUrlHelper urlHelper)
        {
            var path = "Person";
            //! v1*/ string link = getLink(content.Id, urlHelper, path);
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.GET,
                //! v1*/ Href = link,
                Href = path.getLink(content.Id, urlHelper, HttpActionVerb.GET),
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultGet
            });

            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.POST,
                //! v1*/ Href = link.Substring(0, link.LastIndexOf('/')),
                Href = path.getLink(content.Id, urlHelper, HttpActionVerb.POST),
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultPost
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.PUT,
                //! v1*/ Href = link.Substring(0, link.LastIndexOf('/')),
                Href = path.getLink(content.Id, urlHelper, HttpActionVerb.PUT),
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultPut
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.PATCH,
                //! v1*/ Href = link,
                Href = path.getLink(content.Id, urlHelper, HttpActionVerb.PATCH),
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultPatch
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.DELETE,
                //! v1*/ Href = link,
                Href = path.getLink(content.Id, urlHelper, HttpActionVerb.DELETE),
                Rel = RelationType.self,
                Type = "int"
            });
            return Task.CompletedTask;
        }


        /*V1
                private string getLink(long id, IUrlHelper urlHelper, string path)
                {
                    lock (this)
                    {
                        var url = new { controller = path, id };
                        return new StringBuilder(urlHelper.Link("DefaultApi", url)).Replace("%2F", "/").ToString();

                    }
                }
        */
    }
}