using MyPhoneBookManager.Domain.MappingProfile;
using MyPhoneBookManager.WebAPI.PhoneBookManager;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddPhoneBookManagerServices();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapPhoneBookManagerEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
