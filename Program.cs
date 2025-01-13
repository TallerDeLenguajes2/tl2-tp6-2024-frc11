var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IClientesRepository,ClientesRepository>();
builder.Services.AddSingleton<IPresupuestosRepository, PresupuestosRepository>();
builder.Services.AddSingleton<IProductosRepository, ProductosRepository>();
builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true; 
});
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Presupuestos}/{action=Index}/{id?}");

app.Run();