using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyAPIV2.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
  // Include 'SecurityScheme' to use JWT Authentication
  var jwtSecurityScheme = new OpenApiSecurityScheme
  {
    BearerFormat = "JWT",
    Name = "JWT Authentication",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.Http,
    Scheme = JwtBearerDefaults.AuthenticationScheme,
    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

    Reference = new OpenApiReference
    {
      Id = JwtBearerDefaults.AuthenticationScheme,
      Type = ReferenceType.SecurityScheme
    }
  };

  setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

  setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

});

builder.Services.AddCors((options)=>{
    options.AddPolicy("Development", (corsBuilder) =>
    {
        corsBuilder.WithOrigins("https://dev.jm.app.br:3002", "https://dev.jm.app.br:3003", "https://dev.jm.app.br:3004")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
    options.AddPolicy("Production", (corsBuilder) =>
    {
        corsBuilder.WithOrigins("https://jm.app.br")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();

string? tokenKeyString = builder.Configuration.GetSection("AppSettings:TokenKey").Value;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters()
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                  tokenKeyString != null ? tokenKeyString : ""
              )),
        ValidateIssuer = false,
        ValidateAudience = false
      };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("Development");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("Production");
    app.UseHttpsRedirection();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
