using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using StackExchange.Profiling.Storage;
using StudySystem.Application.Service;
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.EF;
using StudySystem.Data.EF.Seed_Data;
using StudySystem.Data.Models.Response;
using StudySystem.Infrastructure.Configuration;
using StudySystem.Infrastructure.Extensions;
using StudySystem.Infrastructure.Resources;
using StudySystem.Middlewares;
using System.IO.Compression;
using System.Net;
using System.Text;

#region log info
System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// logger
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Init main");
#endregion
var builder = WebApplication.CreateBuilder(args);
#region Nlog: setup Nlog dependence injector
builder.Logging.ClearProviders();
builder.Host.UseNLog();
#endregion
#region add miniprofiler
// Add services to the container.
builder.Services.AddMemoryCache(); // add the memory cache to the service collection
// Add miniprofilter : write speed time query sql
// More more about config miniprofiler at https://ilovedotnet.org/blogs/profiling-webapi-with-mini-profiler/
builder.Services.AddMiniProfiler(options =>
{
    options.RouteBasePath = "/profiler"; // /profiler/results-index
}).AddEntityFramework();
#endregion

// Add services to the container.
#region config automapper
builder.Services.AddAutoMapper(typeof(DomainToViewModelMappingProfile));
builder.Services.AddAutoMapper(typeof(ViewModelToDomainMappingProfile));
#endregion

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DictionaryKeyPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});


#region config jwt, AppDbContext
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<UserResolverSerive>();
builder.Services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = AppSetting.Issuer,
        ValidAudience = AppSetting.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSetting.SecretKey))
    };
});
#endregion

#region config allows web permission, response compression
builder.Services.AddCors(cors => cors.AddPolicy(name: "StudySystemPolicy", policy =>
{
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));


//Optimize data traffic transmitted between server and client
// Learn more about configuring Response Compression at https://learn.microsoft.com/en-us/aspnet/core/performance/response-compression?view=aspnetcore-7.0
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true; // enables https is a secure risk
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.SmallestSize;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.SmallestSize;
});

#endregion

#region register service Add Transient
builder.Services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));
builder.Services.AddTransient<DbInit>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ISendMailService, SendMailService>();
builder.Services.AddTransient<IUserTokenService, UserTokenService>();
builder.Services.AddTransient<ILocationService, LocationService>();
builder.Services.AddTransient<ISupplierService, SupplierService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<IExtensionsService, ExtensionsService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IRatingService, RatingService>();
builder.Services.AddTransient<INewService, NewService>();
builder.Services.AddTransient<IChartService, ChartService>();
builder.Services.AddTransient<IBannerService, BannerService>();
#endregion

#region configure connect to db
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(AppSetting.ConnectionString, cfg =>
    {
        cfg.MigrationsAssembly("StudySystem.Data.EF");
        cfg.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
    });
});
#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "JadeWebAPI", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT token must be provided",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.OperationFilter<AuthorizeCheckOperationFilter>();
});
builder.Services.AddControllersWithViews();

var app = builder.Build();


#region seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    //var context = services.GetRequiredService<AppDbContext>();
    //await context.Database.MigrateAsync();
    //var dbInit = services.GetRequiredService<DbInit>();
    //dbInit.Seed().Wait();
}
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Local"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseMiniProfiler(); // add miniprofiler
    app.UseHsts();
}

#region config allows web permission
app.UseCors("StudySystemPolicy");
#endregion
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
#region custom middleware
app.UseMiddleware<AuthTokenMiddleware>();
app.UseMiddleware<AuthPermissionMiddleware>();
app.ConfigureExceptionHandler();
#endregion
#region config status codes error response
app.Use(async (context, next) =>
{
    await next();
    dynamic responseError = new System.Dynamic.ExpandoObject();
    //if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest) // 400
    //{
    //    logger.Error(responseError);
    //    await context.Response.WriteAsJsonAsync(new StudySystemAPIResponse<StudySystemErrorResponseModel>
    //    {
    //        Code = (int)HttpStatusCode.BadRequest,
    //        Response = new Response<StudySystemErrorResponseModel>(false, new StudySystemErrorResponseModel(StatusCodes.Status400BadRequest, Message._400))
    //    });
    //}

    if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized) // 401
    {
        logger.Error(responseError);
        await context.Response.WriteAsJsonAsync(new StudySystemAPIResponse<StudySystemErrorResponseModel>
        {
            Code = (int)HttpStatusCode.Unauthorized,
            Response = new Response<StudySystemErrorResponseModel>(false, new StudySystemErrorResponseModel(StatusCodes.Status401Unauthorized, Message.Unauthorize))
        });
    }

    if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden) // 403
    {
        await context.Response.WriteAsJsonAsync(new StudySystemAPIResponse<StudySystemErrorResponseModel>
        {
            Code = (int)HttpStatusCode.Forbidden,
            Response = new Response<StudySystemErrorResponseModel>(false, new StudySystemErrorResponseModel(StatusCodes.Status403Forbidden, Message._403))
        });
    }

    if (context.Response.StatusCode == (int)HttpStatusCode.NotFound) // 404
    {
        logger.Error(responseError);
        await context.Response.WriteAsJsonAsync(new StudySystemAPIResponse<StudySystemErrorResponseModel>
        {
            Code = (int)HttpStatusCode.NotFound,
            Response = new Response<StudySystemErrorResponseModel>(false, new StudySystemErrorResponseModel(StatusCodes.Status404NotFound, Message._404))
        });
    }

    if (context.Response.StatusCode == (int)HttpStatusCode.MethodNotAllowed) // 405
    {
        await context.Response.WriteAsJsonAsync(new StudySystemAPIResponse<StudySystemErrorResponseModel>
        {
            Code = (int)HttpStatusCode.MethodNotAllowed,
            Response = new Response<StudySystemErrorResponseModel>(false, new StudySystemErrorResponseModel(StatusCodes.Status405MethodNotAllowed, Message._405))
        });
    }

    if (context.Response.StatusCode == (int)HttpStatusCode.InternalServerError) // 405
    {
        await context.Response.WriteAsJsonAsync(new StudySystemAPIResponse<StudySystemErrorResponseModel>
        {
            Code = (int)HttpStatusCode.InternalServerError,
            Response = new Response<StudySystemErrorResponseModel>(false, new StudySystemErrorResponseModel(StatusCodes.Status500InternalServerError, Message._500))
        });
    }
});
#endregion

// use Response compression
app.UseResponseCompression();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}"
    );
app.Run();
