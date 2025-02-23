using AuctionService.Consumers;
using AuctionService.Data;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));

});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//Strart - Section 4 RabbitMQ
builder.Services.AddMassTransit(x => 
{
    x.AddEntityFrameworkOutbox<AuctionDbContext>( o =>
    {
        o.QueryDelay = TimeSpan.FromSeconds(10);

        o.UsePostgres();
        o.UseBusOutbox();
    });

    x.AddConsumersFromNamespaceContaining<AuctionCreatedFaultConsumer>();

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction", false));

    x.UsingRabbitMq((context, cfg) => 
    {
        //Start - Section 7 Dockerizing
            cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host => 
            {
                host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
                host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
            }); 
        //End - Section 7 Dockerizing
        cfg.ConfigureEndpoints(context);
    });
});
//End - Section 4 RabbitMQ

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

var app = builder.Build();

// Configure the HTTP request pipeline.

//Start - Section 5 Identity Service
app.UseAuthentication();
//End - Section 5 Identity Service

app.UseAuthorization(); // Use for Identity Service

app.MapControllers();

try
{
    DbInitializer.InitDb(app);
}
catch (Exception ex)
{
    Console.WriteLine($"Error seeding the database: {ex.Message}");
}

app.Run();

