using System.Text;
using Google.Protobuf.WellKnownTypes;

namespace RestAPI.Hypermedia
{
    public class HyperMediaLink
    {
        public string Rel { get; set; }
        public string href;
        public string Href
        {
            get
            {
                lock (this)
                {
                    StringBuilder sb = new StringBuilder(href);
                    return sb.Replace("%2F", "/").ToString(); //! o .NET muda as barras dos links para %2F, assim para apresentar, mudamos a string para conter a barra
                }

            }

            set { href = value; }
        }
        public string Type { get; set; }
        public string Action { get; set; }
    }
}