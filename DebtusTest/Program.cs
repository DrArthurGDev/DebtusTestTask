using DebtusTest.Data;
using DebtusTest.Services;
using DebtusTest.Services.IServices;
using Microsoft.EntityFrameworkCore;
using StaffService.Middlewares;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddDbContext<TestDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IShiftService, ShiftService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<TestDbContext>();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.Run();
