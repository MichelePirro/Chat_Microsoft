using ChatMicrosoft.Data;
using ChatMicrosoft.DataBase;
using MySql.Data.MySqlClient;
using System.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSignalR();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<ChatGPTConnectionService>();
builder.Services.AddScoped<ChatGPTChatService>();
builder.Services.AddScoped<DatabaseService>();
builder.Services.AddScoped<ChatGPTEmbeddingService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
