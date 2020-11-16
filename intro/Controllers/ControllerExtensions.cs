using System.Net.Http;
using Aron.Http.Tools.HttpMsg;
using Aron.Web.Api.Interfaces.Message;
using Aron.Web.Api.Models.Message;
using Microsoft.AspNetCore.Mvc;

namespace intro.Controllers
{
    public static class ControllerExtensions
    {
        private static IBodyConverter<IResponseMessage> _bodyConverter = new JsonBodyConverter<IResponseMessage>();
        public static HttpResponseMessage CreateResponse<T>(this ControllerBase controller, T data)
        {
            var temp = new StdMsg<IResponseMessage>(_bodyConverter)
            {
                ContentType = ContentType.JSON,
            };
            var t = new ResponseMessage<T>();
            t.data = data;
            temp.Data = t;
            return temp.CreateResponseMsg(System.Net.HttpStatusCode.OK);
        }

        public static HttpResponseMessage CreateErrorResponse(this ControllerBase controller, string code, string msg)
        {
            var temp = new StdMsg<IResponseMessage>(_bodyConverter)
            {
                ContentType = ContentType.JSON,
            };
            var t = new ErrorResponseMessage();
            t.error_code = code;
            t.error_message = msg;
            temp.Data = t;
            return temp.CreateResponseMsg(System.Net.HttpStatusCode.InternalServerError);
        }

        public static HttpResponseMessage CreateErrorResponse<T>(this ControllerBase controller, T data, string code, string msg)
        {
            var temp = new StdMsg<IResponseMessage>(_bodyConverter)
            {
                ContentType = ContentType.JSON,
            };
            var t = new ErrorResponseMessage<T>();
            t.error_code = code;
            t.error_message = msg;
            t.data = data;
            temp.Data = t;
            return temp.CreateResponseMsg(System.Net.HttpStatusCode.InternalServerError);
        }
    }
}