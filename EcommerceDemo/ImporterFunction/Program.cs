using EntityFramework;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(Environment.GetEnvironmentVariable("SqlConnection")));
    })
    .Build();

host.Run();
