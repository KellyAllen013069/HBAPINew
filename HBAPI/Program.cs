using HBAPI.Data;
using HBAPI.Converters;
using HBAPI.Configuration;  // Import the configuration classes
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddDatabase(builder.Configuration);  
builder.Services.AddControllersWithOptions();         
builder.Services.AddAuthenticationWithJwt(builder.Configuration);  
builder.Services.AddSwaggerDocumentation();           
builder.Services.AddCustomCors();
builder.Services.Configure<OpenAIConfiguration>(builder.Configuration.GetSection("OpenAI"));


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerDocumentation(); 
}
else
{
    app.UseProductionErrorHandling(); 
}

// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();