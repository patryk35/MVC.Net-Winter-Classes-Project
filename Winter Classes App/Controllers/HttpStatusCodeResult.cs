using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Winter_Classes_App.Controllers
{
    public class HttpStatusCodeResult : ActionResult
    {
        private HttpStatusCode notFound;

        public HttpStatusCodeResult(HttpStatusCode notFound)
        {
            this.notFound = notFound;
        }
    }
}
