namespace WebAPI加密
{
    public static class MiddlerwareExtention
    {
        public static IApplicationBuilder UserSwaggerAuth(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SwaggerAuthMiddleware>();
        }
    }
}
