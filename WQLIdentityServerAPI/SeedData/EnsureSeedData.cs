﻿using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WQLIdentity.Infra.Data;
using WQLIdentity.Infra.Data.Entities;

namespace WQLIdentityServerAPI.SeedData
{
    public class EnsureSeedData
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationRole> _roleManager;
        private  ILogger _logger;
        //private readonly IServiceProvider _services;

        public async Task EnsureSeedDataAsync(IServiceProvider services)
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                _logger = scope.ServiceProvider.GetRequiredService<ILogger<EnsureSeedData>>();  
                _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                await EnsureIdentitySeedAsync();
                await EnsureIdentityServerSeedAsync(scope.ServiceProvider);
            }

        }
        private async Task EnsureIdentitySeedAsync()
        {
            if (!await _roleManager.RoleExistsAsync(AuthorizationConsts.AdministrationRole))
            {
                var role = new ApplicationRole() { Name = AuthorizationConsts.AdministrationRole, NormalizedName = "Administrator" };
                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    throw new Exception("初始化默认角色失败:" + result.Errors.SelectMany(e => e.Description));
                }
            }

            if (await _userManager.FindByNameAsync(AuthorizationConsts.AdministrationUser)==null)
            {
          

                var defaultUser = new ApplicationUser
                {
                    UserName = AuthorizationConsts.AdministrationUser,
                    Name = "IT测试",
                    SecurityStamp = "admin",
                };

                var result = await _userManager.CreateAsync(defaultUser, "123456");

                if (!result.Succeeded)

                {
                    throw new Exception("初始默认用户失败");
                }
                await _userManager.AddToRoleAsync(defaultUser, AuthorizationConsts.AdministrationRole);
            }
            _logger.LogInformation("初始化identity信息成功");
        }
        private async Task EnsureIdentityServerSeedAsync(IServiceProvider services)
        {
            //using (var scope = _services.GetRequiredService< IServiceScopeFactory>().CreateScope())
            //{
                services.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                var configurationDbContext = services.GetRequiredService<ConfigurationDbContext>();

                if (!configurationDbContext.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        await configurationDbContext.Clients.AddAsync(client.ToEntity());
                    }
                    await configurationDbContext.SaveChangesAsync();
                }

                if (!configurationDbContext.ApiResources.Any())
                {
                    foreach (var api in Config.GetApiResources())
                    {
                        await configurationDbContext.ApiResources.AddAsync(api.ToEntity());
                    }
                    await configurationDbContext.SaveChangesAsync();
                }

                if (!configurationDbContext.IdentityResources.Any())
                {
                    foreach (var identity in Config.GetIdentityResources())
                    {
                       await  configurationDbContext.IdentityResources.AddAsync(identity.ToEntity());
                    }
                    await configurationDbContext.SaveChangesAsync();
                }
            //}
            _logger.LogInformation("初始化identityserver信息成功");
        }
    }
}
