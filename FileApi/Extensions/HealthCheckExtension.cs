using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace FileApi.Extensions
{
    public static class HealthCheckExtension
    {

        public static IApplicationBuilder CustomHealthCheck(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/api/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
            {
                ResponseWriter = async (context, report) =>
                {
                    await context.Response.WriteAsync("OK");
                }
            });
            return app;
        }

    }
}
