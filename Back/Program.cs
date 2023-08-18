using Back.Middleware.SpaIndex;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

try
{
    var builder = WebApplication.CreateBuilder(args);

    //web
    builder.Services.AddControllers(o =>
    {
        o.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
        o.Filters.Add(
            new ResponseCacheAttribute { NoStore = true, Location = ResponseCacheLocation.None }
        );
    });
    builder.Services.AddResponseCompression();
    //builder.Services.AddEndpointsApiExplorer();


    var app = builder.Build();

    app.UseSpaIndex();
    app.UseStaticFiles();

    //app.MapGet("", () => "Hello World!");
    app.UseRouting();
    app.MapControllers();

    app.Run();
}
catch(Exception e)
{
    Console.WriteLine(e);
    throw;
}

