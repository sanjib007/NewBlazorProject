namespace L3T.OAuth2DotNet7.Middleware
{
    public static class GlobalCutomMiddleware
    {
        public static void UseGlobalCustomMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomMiddleware>();
        }
    }
}
