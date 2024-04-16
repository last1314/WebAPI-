using Microsoft.AspNetCore.Http.Features;
using System.Net.Http.Headers;
using System.Net;
using System.Text;

namespace WebAPI加密
{
    public class SwaggerAuthMiddleware 
    {
        private readonly RequestDelegate next;

        //定义用户名和密码
        private readonly string UserName = "admin";
        private readonly string Password = "1";
        public SwaggerAuthMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        bool Flag = true;
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/SignOut") && Flag == true)
            {
                context.Request.Headers.Remove("Authorization");
                context.Response.Headers["www-authenticate"] = "Basic";
                context.Response.Headers["Hello"] = "World";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                Flag = false;
                return;
            }
            Flag = true;
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                // Get the credentials from request header
                var header = AuthenticationHeaderValue.Parse(authHeader);
                var base64 = Convert.FromBase64String(header.Parameter);
                var credentials = Encoding.UTF8.GetString(base64).Split(':');
                var username = credentials[0];
                var password = credentials[1];
                // validate credentials
                if (username.Equals(UserName) && password.Equals(Password))
                {
                    await next.Invoke(context).ConfigureAwait(false);
                    return;
                }
            }

            //通知后端进行Basic认证,弹出登录小窗口，输入以上定义的用户名密码才能访问Swagger文档与接口资源之后的页面
            context.Response.Headers["WWW-Authenticate"] = "Basic";
            context.Response.Headers["Hello"] = "World";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;




        }
    }
}
