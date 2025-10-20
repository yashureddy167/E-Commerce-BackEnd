using AuthService.Infrastructure;
using AuthService.Application;

var builder = WebApplication.CreateBuilder(args);

// Service configuration
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    // Optional: Add JWT Bearer support
    options.SwaggerDoc("v1", new() { Title = "AuthService API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token."
    });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("*")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Global error handling
app.UseExceptionHandler("/error");
app.UseStatusCodePages();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Enable CORS
app.UseCors();

// Security headers
app.UseHsts();

// Add authentication before authorization
app.UseAuthentication();
app.UseAuthorization();

// OpenAPI configuration - Consider if you want this in non-Development environments too
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthService API v1");
        c.RoutePrefix = string.Empty; // Swagger at root: http://localhost:5000/
    });
    app.MapOpenApi();
}

app.MapControllers();

app.Run();
