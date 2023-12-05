using Microsoft.OpenApi.Models;
using Rainfall.Api.Filters;
using Rainfall.Api.Services;
using Rainfall.Api.Swashbuckle;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => options.Filters.Add<ModelValidationFilter>());
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => 
{
    options.SwaggerDoc("v1",
    new OpenApiInfo
        {
        Title = "Rainfall Api",
        Version = "1.0",
        Description = "An API which provides rainfall reading data"
        });
    options.AddServer(new OpenApiServer { Url = builder.Configuration["Urls"], Description = "Rainfall Api" });
    options.DocumentFilter<TagDescriptionsDocumentFilter>();
    
     var filePath = Path.Combine(AppContext.BaseDirectory, "Rainfall.Api.xml");
     options.IncludeXmlComments(filePath);
});

builder.Services.AddHttpClient();
builder.Services.AddTransient<IRainfallService, RainfallService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rainfall Api");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseExceptionHandler("/error");
app.MapControllers();

app.Run();
