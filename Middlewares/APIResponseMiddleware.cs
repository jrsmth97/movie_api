using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using movie_api.Contexts;

namespace movie_api.Middlewares
{
    public class APIResponseSingle 
    {
        public bool success;
        public string message;
        public Object data;
    }

    public class APIResponseList
    {
        public bool success;
        public string message;
        public List<Object> data;
    }

    public class APIResponseError
    {
        public bool success;
        public Object errors;
        public string message;
    }

    public class APIResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly DBContext _db;
        
        public APIResponseMiddleware(RequestDelegate next, IConfiguration configuration, DBContext db)
        {
            _next           = next;
            _configuration  = configuration;
            _db             = db;
        }

        public async Task Invoke(HttpContext context) 
        {   
            string pathUrl = context.Request.Path.ToString();
            using (var buffer = new MemoryStream()) {
                var stream = context.Response.Body;

                context.Response.Body = buffer;
                await _next.Invoke(context);
                buffer.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(buffer);
                using (var bufferReader = new StreamReader(buffer)) { 
                    string body = await bufferReader.ReadToEndAsync();
                    string jsonString;
                    string method = getStatusWithMethod(context.Request.Method);
                    if (body.Contains("errors") || body.Contains("Unauthorized"))
                    {
                        var errResponse = new APIResponseError();
                        var errData = JsonConvert.DeserializeObject(body);
                        errResponse.errors = errData;
                        errResponse.message = "error has occurred";
                        errResponse.success = false;
                        jsonString = JsonConvert.SerializeObject(errResponse);
                        await context.Response.WriteAsync(jsonString);
                        context.Response.Body.Seek(0, SeekOrigin.Begin);
                        byte[] byteError = Encoding.ASCII.GetBytes(jsonString);
                        var dataError = new MemoryStream(byteError);
                        context.Response.Body = dataError;
                    } else if (pathUrl.Contains("auth") || ( !body.Contains("{") && !body.Contains("[") )) {
                        await context.Response.WriteAsync(body);
                        context.Response.Body.Seek(0, SeekOrigin.Begin);
                        byte[] byteSingle = Encoding.ASCII.GetBytes(body);
                        var dataSingle = new MemoryStream(byteSingle);
                        context.Response.Body = dataSingle;
                    } else {
                        if (body.StartsWith("["))
                            {
                                var responseList = new APIResponseList();
                                responseList.data = JsonConvert.DeserializeObject<List<Object>>(body);
                                responseList.message = $"success {method} data";
                                responseList.success = true;
                                jsonString = JsonConvert.SerializeObject(responseList);
                                await context.Response.WriteAsync(jsonString);
                                context.Response.Body.Seek(0, SeekOrigin.Begin);
                                byte[] byteList = Encoding.ASCII.GetBytes(jsonString);
                                var dataList = new MemoryStream(byteList);
                                context.Response.Body = dataList;
                            } else {
                                var response = new APIResponseSingle();
                                response.data = JsonConvert.DeserializeObject(body);
                                response.message = $"success {method} data";
                                response.success = true;
                                jsonString = JsonConvert.SerializeObject(response);
                                await context.Response.WriteAsync(jsonString);
                                context.Response.Body.Seek(0, SeekOrigin.Begin);
                                byte[] byteSingle = Encoding.ASCII.GetBytes(jsonString);
                                var dataSingle = new MemoryStream(byteSingle);
                                context.Response.Body = dataSingle;
                            }
                    }
 
                    await context.Response.Body.CopyToAsync(stream); 
                    context.Response.Body = stream;
                }
            }
        }

        private static string getStatusWithMethod(string method)
        {
            var status = "";
            switch(method)
            {
                case "POST":
                    status = "create";
                    break;
                case "PATCH":
                    status = "update";
                    break;
                case "DELETE":
                    status = "delete";
                    break;
                default:
                    status = "get";
                    break;
            }

            return status;
        }

    }
}