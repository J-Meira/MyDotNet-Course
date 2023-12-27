var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapControllers();

app.Run();
