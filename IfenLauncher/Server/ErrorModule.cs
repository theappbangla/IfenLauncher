using Nancy;
using Nancy.ErrorHandling;

namespace IfenLauncher.Server
{
    public class ErrorModule : IStatusCodeHandler
    {
        public bool HandlesStatusCode(HttpStatusCode statusCode,
                                      NancyContext context)
        {
            return statusCode == HttpStatusCode.NotFound;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            /*var response = new GenericFileResponse("Program.cs", "text/html", context);
            response.StatusCode = statusCode;
            context.Response = response;*/
        }
    }
}
