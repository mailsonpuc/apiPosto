using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using puc.Context;
using System.Text;
using System.Text.Json.Serialization;
using apiPosto.Filters;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// e usando exceçao global e ignorar ciclos de referencia
builder.Services.AddControllers(options =>
    {
        options.Filters.Add(typeof(ApiExceptionFilter));
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });



// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApiDocument(c =>
{
    c.Title = "Api PucMinas";
    c.AddSecurity("JWT", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
    {
        Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = NSwag.OpenApiSecurityApiKeyLocation.Header,
        Description = "Digite 'Bearer {token}' no cabeçalho Authorization."
    });
    c.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});



//swager




var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

// Registra o DbContext do Entity Framework com SQL Server.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));





//jwt
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    // Adding Jwt Bearer
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Ry74cBQva5dThwbwchR9jhbtRFnJxWSZ"))
        };
    });



var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(settings =>
    {
        settings.Path = "/swagger";
        settings.DocumentPath = "/swagger/v1/swagger.json";
    });
}



app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
