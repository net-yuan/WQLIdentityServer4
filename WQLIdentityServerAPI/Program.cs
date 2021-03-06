﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WQLIdentityServerAPI.SeedData;
using Autofac.Extensions.DependencyInjection;

namespace WQLIdentityServerAPI
{
    public class Program
    {
        public static  void Main(string[] args)
        {

            Console.Title = "IdentityServer";

            var seed = args.Contains("seed");
            if (seed)
            {
                args = args.Except(new[] { "seed" }).ToArray();
            }

            var host = CreateWebHostBuilder(args).Build();


            EnsureSeedData seedData = new EnsureSeedData();
            //seedData.EnsureSeedDataAsync(host.Services).Wait();
            if (seed)
            {
                seedData.EnsureSeedDataAsync(host.Services).Wait();
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder().AddJsonFile("host.json", optional: true).Build();//发布地址配置文件
            return WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                    .ConfigureLogging(opt=>opt.AddDebug())
                    .ConfigureServices(service => service.AddAutofac()).UseConfiguration(config)
            ;
        }
    }
}
