
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<HttpClient>(s =>
{
	// Creating the URI helper needs to wait until the JS Runtime is initialized, so defer it.
	var uriHelper = s.GetRequiredService<NavigationManager>();
	return new HttpClient
	{
		BaseAddress = new Uri(uriHelper.BaseUri)
	};
});
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddTransient<eKnowledgebase.Services.eKnowledgebaseService>();
builder.Services.AddMudServices();

builder.Services.AddBlazoredLocalStorage();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
