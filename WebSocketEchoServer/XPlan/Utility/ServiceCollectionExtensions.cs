using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using MongoDB.Entities;

using System.Text;
using XPlan.Utility.Caches;
using XPlan.Utility.Databases;
using XPlan.Utility.Exceptions;

namespace XPlan.Utility
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 加入全域例外過濾器
        /// </summary>
        public static IServiceCollection AddExceptionHandling<T>(this IServiceCollection services) where T : GlobalExceptionFilter
        {
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add<T>();
            });

            return services;
        }

        /// <summary>
        /// 加入快取設定及記憶體快取
        /// </summary>
        public static IServiceCollection AddCacheSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));
            services.AddMemoryCache();
            return services;
        }

        /// <summary>
        /// 初始化 MongoDB 連線與註冊相關服務
        /// </summary>
        public static IServiceCollection InitialMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDBSettings>(configuration.GetSection("MongoDBSetting"));

            services.AddSingleton<IMongoClient>((sp) =>
            {
                var settings    = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
                var client      = new MongoClient(settings.ConnectionString);
                return client;
            });

            services.AddSingleton<IMongoDbContext, MongoDBContext>();

            return services;
        }

        /// <summary>
        /// 初始化 MongoDB.Entities 的 DB 連線
        /// </summary>
        public async static Task InitialMongoDBEntity(this IServiceCollection services, IConfiguration configuration)
        {
            var section                 = configuration.GetSection("MongoDBSetting");
            MongoDBSettings? dbSetting  = section.Get<MongoDBSettings>();

            if (dbSetting is null)
            {
                throw new InvalidOperationException("Missing or invalid MongoDBSetting section in configuration.");
            }

            await DB.InitAsync(dbSetting.DatabaseName, MongoClientSettings.FromConnectionString(dbSetting.ConnectionString));
        }

        /// <summary>
        /// 註冊 AutoMapper 映射設定並加入 DI
        /// </summary>
        public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services, ILoggerFactory loggerFactory)
        {
            var configExpression    = new MapperConfigurationExpression();
            configExpression.AddMaps(AppDomain.CurrentDomain.GetAssemblies());

            var mapperConfig        = new MapperConfiguration(configExpression, loggerFactory);
            var mapper              = mapperConfig.CreateMapper();

            services.AddSingleton<IMapper>(mapper);
            return services;
        }
    }
}
