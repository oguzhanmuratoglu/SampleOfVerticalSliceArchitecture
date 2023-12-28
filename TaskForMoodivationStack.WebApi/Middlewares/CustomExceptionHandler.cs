using Corex.Model.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using TaskForMoodivationStack.WebApi.Shared;

namespace TaskForMoodivationStack.WebApi.Middlewares;


//En son loglama işlemi yaptığım için try-cach içerisinde globalExeptionHandler işlemine gerek kalmadı.Fakat silmeye kıyamadım


//public static class CustomExceptionHandler
//{
//    public static void UseCustomException(this IApplicationBuilder app)
//    {
//        app.UseExceptionHandler(config =>
//        {
//            config.Run(async context =>
//            {
//                context.Response.ContentType = "application/json";

//                var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

//                var statusCode = exceptionFeature.Error switch
//                {
//                    ClientSideException => 400,
//                    NotFoundException => 404,
//                    _ => 500
//                };

//                context.Response.StatusCode = statusCode;

//                var errorMessage = exceptionFeature.Error.Message;

//                var response = ResultModel.Error(new MessageItem
//                {
//                    Code = statusCode.ToString(),
//                    Message = errorMessage
//                });

//                await context.Response.WriteAsync(JsonSerializer.Serialize(response));

//            });
//        });
//    }
//}

//public class ClientSideException : Exception
//{
//    public ClientSideException(string message) : base(message)
//    {

//    }
//}

//public class NotFoundException : Exception
//{
//    public NotFoundException(string message) : base(message)
//    {

//    }
//}
