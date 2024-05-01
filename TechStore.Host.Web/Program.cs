using System.Reflection;

namespace TechStore.Host.Web;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var path = Path.GetDirectoryName(assembly.Location) ?? string.Empty; 

        return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder =>
            {
                Directory.SetCurrentDirectory(path);
                builder.UseStartup<Startup>();
            });
    }
}