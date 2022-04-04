using Serilog;

namespace WebAPI.Installers
{
    public static class DependencyInjection
    {
        public static IHostBuilder AppSerilog(this IHostBuilder host)
        {
            host.UseSerilog((ctx, lc) => lc
                .WriteTo.Console());

            return host;
        }
    }
}
