using Microsoft.EntityFrameworkCore;
using Application.Booking.Ports;
using Application.Guest.Ports;
using Application.Room.Ports;
using Application;
using Data;
using Data.Guest;
using Data.Room;
using Data.Booking;
using Domain.Ports;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//IoC
builder.Services.AddScoped<IGuestRepository, GuestRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

builder.Services.AddScoped<IGuestManager, GuestManager>();
builder.Services.AddScoped<IRoomManager, RoomManager>();
builder.Services.AddScoped<IBookingManager, BookingManager>();

//DbConnection
var connectionString = builder.Configuration.GetConnectionString("DefautConnection");
builder.Services.AddDbContext<HotelDbContext>(options => options.UseSqlServer(connectionString));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
