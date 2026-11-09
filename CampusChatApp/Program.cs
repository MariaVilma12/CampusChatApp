using CampusChatApp.Components;
using CampusChatApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<ThreadService>();
builder.Services.AddSingleton<ChatService>();
builder.Services.AddSingleton<CampusChatStateService>();
builder.Services.AddScoped<SessionService>();

var app = builder.Build();

AppStateBootstrap.Initialize();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
