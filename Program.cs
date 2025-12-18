using siapv_backend.Services;
using siapv_backend.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using siapv_backend.Models;
using System.Globalization;
using System.Net.Mail;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using siapv_backend.Models.Internal;
using JsonPatchSample;
using siapv_backend.Helpers;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

builder.Services.AddCors(
//     options =>
// {
//     options.AddPolicy("AllowAllOrigins",
//         policy =>
//         {
//             policy.AllowAnyOrigin()
//                 .AllowAnyMethod()
//                 .AllowAnyHeader();
//         });
// }
);
builder.Services.AddSingleton<LdapService>();
builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<DBUsuariosContext>(options =>
        options.UseNpgsql(Configuration.GetConnectionString("DBusuarios")));
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
var smtpSettings = (SmtpSettings)builder.Configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFinanciamientoService, FinanciamientoService>();
builder.Services.AddScoped<IProyectoService, ProyectoService>();
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();
builder.Services.AddScoped<ISolicitudViajeService, SolicitudViajeService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ICorrelativoService, CorrelativoService> ();
builder.Services.AddScoped<IPresupuestoService, PresupuestoService>();
builder.Services.AddScoped<ICertificacionPOAService, CertificacionPOAService>();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "JWT Token Authentication API",
        Description = ".NET 8 Web API"
    });
    // To Enable authorization using Swagger (JWT)
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
});
var smtpClient = new SmtpClient(smtpSettings?.Host)
    {
        Port = smtpSettings.Port,
        Credentials = new System.Net.NetworkCredential(smtpSettings.UserName,
        smtpSettings.Password),
        EnableSsl= smtpSettings.EnableSsl
    };
builder.Services.AddFluentEmail(smtpSettings?.FromEmail, smtpSettings?.FromName)
    .AddRazorRenderer()
    .AddSmtpSender(smtpClient);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
var supportedCultures = new[] { "en-US", "es-ES" };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("es-ES"),
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
};

app.UseRequestLocalization(localizationOptions);
app.UseMiddleware<JwtMiddleware>();
app.UseHttpsRedirection();
//app.UseCors("AllowAllOrigins");
// global cors policy
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials
app.UseAuthorization();

app.MapControllers();

app.Run();