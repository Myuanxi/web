using Microsoft.Extensions.Configuration;
using dms.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// ��ӷ�������
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DmsContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                     new MySqlServerVersion(new Version(8, 0, 28))));

// ʹ�ù�������ע�� ChatWebSocketHandler
builder.Services.AddScoped<ChatWebSocketHandler>(provider =>
{
    var context = provider.GetRequiredService<DmsContext>();
    return new ChatWebSocketHandler(context);
});

// ��������֤�� Cookie ��֤
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
    });

var app = builder.Build();

// ����HTTP����ܵ�
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // �����һ�������������֤
app.UseAuthorization(); // ȷ��������Ȩ

// ��Ĭ�Ͽ���������Ϊ AuthController
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
