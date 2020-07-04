using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace eFood.Common.Serilog
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(ILogger<RequestResponseLoggingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                //  log when we start processing
                DateTime start = DateTime.Now;

                //First, get the incoming request
                var request = await FormatRequest(context.Request);
                _logger.LogDebug(request.LogTemplate, request.Details);

                //Copy a pointer to the original response body stream
                var originalBodyStream = context.Response.Body;

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;
                    await _next.Invoke(context);

                    var response = await FormatResponse(context.Response, DateTime.Now - start);
                    _logger.LogDebug(response.str, response.details);

                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<(string LogTemplate, object[] Details)> FormatRequest(HttpRequest request)
        {
            var method = request.Method;
            var schema = request.Scheme;
            var host = request.Host;
            var path = request.Path;
            var query = request.Query.ToDictionary(pair => pair.Key, pair => pair.Value);
            var headers = request.Headers.ToDictionary(pair => pair.Key, pair => pair.Value);
            var cookies = request.Cookies.ToDictionary(pair => pair.Key, pair => pair.Value);
            string bodyAsText;

            using (var reader = new StreamReader(request.Body, Encoding.UTF8, false, 8192, true))
            {
                var body = request.Body;
                request.EnableBuffering();
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                await request.Body.ReadAsync(buffer, 0, buffer.Length);
                bodyAsText = Encoding.UTF8.GetString(buffer);
                request.Body = body;
            }


            return ("HttpRequest: {@schema}, {@method}, {@host}, {@path}, {@query}, {@headers}, {@cookies}, {@Body}",
                new object[]
                {
                    schema,
                    method,
                    host,
                    path,
                    query,
                    headers,
                    cookies,
                    bodyAsText
                });
        }

        private async Task<(string str, object[] details)> FormatResponse(HttpResponse response, TimeSpan duration)
        {
            int statusCode = response.StatusCode;

            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            // return $"{response.StatusCode}: {text}";

            var headers = response.Headers.ToDictionary(pair => pair.Key, pair => pair.Value);

            string str = "HttpResponse: {@HttpStatusCode}, {@Duration} {@ResponseHeaders}, {@ResponseBody}";

            var details = new object[]
            {
                statusCode,
                duration.TotalMilliseconds,
                headers,
                text
            };

            return (str, details);
        }
    }
}
