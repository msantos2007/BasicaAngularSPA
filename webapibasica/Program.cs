using MediatR;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using webapibasica.Data;
using webapibasica.Infrastructure;
using webapibasica.Mappings;
using Westwind.AspNetCore.LiveReload;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//***
builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson(options => options.SerializerSettings
                                                     .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddLiveReload(config =>
{
    // optional - use config instead
    //config.LiveReloadEnabled = true;
    //config.FolderToMonitor = Path.GetFullname(Path.Combine(Env.ContentRootPath,"..")) ;
    //config.FolderToMonitor = Path.GetFullPath(Environment.CurrentDirectory) + "\\wwwroot\\";
});

builder.Services.AddMvc().AddRazorRuntimeCompilation();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//***
builder.Services.AddMediatR(typeof(Program));


//*** THIS WILL MUST BE USED host.docker.internal
//*** Docker wont work with localhost
// string ip_output = "";
// using (System.Net.Sockets.Socket socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Dgram, 0))
// {
//     socket.Connect("8.8.8.8", 65530);
//     var endPoint = socket.LocalEndPoint as System.Net.IPEndPoint;
//     ip_output = endPoint?.Address.ToString() ?? "localhost";
// }



builder.Services.AddDbContext<BasicaContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddSingleton<MongoContext>();

// //*** 
// builder.Services.AddScoped<IDbFactory, DbFactory>();
// builder.Services.AddScoped<IDbFactory, DbFactory>();
// builder.Services.AddScoped(typeof(IEntityBaseRepository<>), typeof(EntityBaseRepository<>));

builder.Services.AddAutoMapper(typeof(CommonMappingProfile));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

//***
using (StreamReader iisUrlRewriteStreamReader = File.OpenText("UrlRewrite.xml"))
{
    var options = new RewriteOptions().AddRedirect("redirect-rule/(.*)", "redirected/$1")
                                      .AddRewrite(@"^rewrite-rule/(\d+)/(\d+)", "rewritten?var1=$1&var2=$2", skipRemainingRules: true)
                                      .AddIISUrlRewrite(iisUrlRewriteStreamReader);

    app.UseRewriter(options);
}

//***
app.UseLiveReload();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

//***
app.UseStaticFiles();

//***
app.UseRouting();

app.UseAuthorization();

//***
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.MapControllers();


// ***
// https://stackoverflow.com/questions/37780136/asp-core-migrate-ef-core-sql-db-on-startup

//// Libere este bloco
// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;

//     var context = services.GetRequiredService<BasicaContext>();
//     if (context.Database.GetPendingMigrations().Any())
//     {
//         context.Database.Migrate();
//     }
// }

app.Run();
