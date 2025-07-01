using Microsoft.EntityFrameworkCore;
using StudentReservations.Date;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add db context option
builder.Services
    .AddDbContext<AppDbContext>(op =>
            op.UseSqlServer("Server=db21757.databaseasp.net; Database=db21757; User Id=db21757; Password=8m-XZ_7i6Wr?; Encrypt=False; MultipleActiveResultSets=True; "));

// ✅ Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()   // Allow all domains (for testing)
            .AllowAnyMethod()   // Allow GET, POST, etc.
            .AllowAnyHeader();  // Allow all headers
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();


//Server=db21757.databaseasp.net; Database=db21757; User Id=db21757; Password=8m-XZ_7i6Wr?; Encrypt=False; MultipleActiveResultSets=True; 


//"Server=db22400.public.databaseasp.net; Database=db22400; User Id=db22400; Password=8d@JzH4!_iN6; Encrypt=True; TrustServerCertificate=True;"));
               // "Server=db22402.public.databaseasp.net; Database=db22402; User Id=db22402; Password=o?7M+Pq3X4@r; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;"