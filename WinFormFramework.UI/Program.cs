using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WinFormFramework.BLL.Interfaces;
using WinFormFramework.BLL.Services;
using WinFormFramework.BLL.Mappings;
using WinFormFramework.Infrastructure;
using WinFormFramework.DAL;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace WinFormFramework.UI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = ConfigureServices();
            
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var loginForm = serviceProvider.GetRequiredService<LoginForm>();
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    var mainForm = serviceProvider.GetRequiredService<MainForm>();
                    Application.Run(mainForm);
                }
            }
        }

        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            // 配置
            IConfiguration configuration = DependencyInjection.BuildConfiguration();
            services.AddSingleton(configuration);

            // 数据库上下文
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            // 仓储
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // 业务服务
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISystemLogService, SystemLogService>();

            // 基础设施服务
            services.AddInfrastructure(
                configuration,
                Path.Combine(AppContext.BaseDirectory, "files"));

            // 窗体
            services.AddTransient<LoginForm>();
            services.AddTransient<MainForm>();

            return services;
        }
    }
} 