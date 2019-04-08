using System;
using AuthServer.Areas.Identity.Data;
using AuthServer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(AuthServer.Areas.Identity.IdentityHostingStartup))]
namespace AuthServer.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<AuthServerContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("AuthServerContextConnection")));

                services.AddIdentity<AuthServerUser, IdentityRole>()
                    .AddEntityFrameworkStores<AuthServerContext>().AddDefaultTokenProviders();
            });
        }
    }
}