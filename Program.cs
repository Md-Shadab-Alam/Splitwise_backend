using Microsoft.EntityFrameworkCore;
using Splitwise.Data;
using Splitwise.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SplitwiseDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("SplitwiseConnectionString")));

//Adding Services
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IBalanceService, BalanceService>();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
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

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();