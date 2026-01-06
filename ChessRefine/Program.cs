using ChessRefine.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
// Servicees

builder.Services.AddSingleton(
    new StockfishService("C:\\Users\\pawqy\\stockfish\\stockfish-windows-x86-64-avx2.exe") // sciezka do stockfisha
);

builder.Services.AddScoped<GameAnalysisService>();

var app = builder.Build();

// Wylaczenie redirection to httpS, na podstawie testow
//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
