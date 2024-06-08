using Microsoft.Extensions.Configuration;
using dms.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// 添加服务到容器
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DmsContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                     new MySqlServerVersion(new Version(8, 0, 28))));

// 使用工厂方法注册 ChatWebSocketHandler
builder.Services.AddScoped<ChatWebSocketHandler>(provider =>
{
    var context = provider.GetRequiredService<DmsContext>();
    return new ChatWebSocketHandler(context);
});

// 添加身份验证和 Cookie 认证
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
    });

var app = builder.Build();

// 配置HTTP请求管道
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // 添加这一行来启用身份验证
app.UseAuthorization(); // 确保启用授权

// 将默认控制器设置为 AuthController
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
