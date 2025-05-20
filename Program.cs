using Microsoft.EntityFrameworkCore;
using codegen.Helpers;
using codegen.Middleware;
using BlazorApp1.Components;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastractureService(builder.Configuration);
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetService<AppHelper>();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.MapControllers();
app.UseNodeModules(app.Environment.ContentRootPath);
app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();