using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

//Section 6 - Gateway Service : Start
    builder.Services.AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy")); //Mention config file

//Start - Section 5 Identity Service
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServiceUrl"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.NameClaimType = "username";
    });
//End - Section 5 Identity Service

//Section 6 - Gateway Service : End

var app = builder.Build();

app.MapReverseProxy();

//Section 6 - Gateway Service : Start
app.UseAuthentication();
app.UseAuthorization();
//Section 6 - Gateway Service : End

app.Run();
