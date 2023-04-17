using System.Web;
using helpack.Data;
using helpack.Misc;
using MailKit;
using Microsoft.EntityFrameworkCore;
using IMailService = helpack.Services.IMailService;
using MailService = helpack.Services.MailService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<HelpackDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("HelpackDbConnection"));
});

builder.Services.AddAutoMapper(typeof(HelpackProfile), typeof(DonationProfile));

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.AddTransient<IMailService, MailService>();

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(
        options => options.WithOrigins("localhost:63342").AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
    );
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();