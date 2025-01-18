using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WinFormFramework.DAL
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(DatabaseContext context, ILogger logger)
        {
            try
            {
                // 检查是否需要迁移
                if ((await context.Database.GetPendingMigrationsAsync()).Any())
                {
                    logger.LogInformation("正在应用数据库迁移...");
                    await context.Database.MigrateAsync();
                    logger.LogInformation("数据库迁移完成。");
                }

                // 检查是否需要初始化数据
                if (!await context.Users.AnyAsync())
                {
                    logger.LogInformation("正在初始化数据库...");
                    await InitializeDataAsync(context);
                    logger.LogInformation("数据库初始化完成。");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "初始化数据库时发生错误");
                throw;
            }
        }

        private static async Task InitializeDataAsync(DatabaseContext context)
        {
            // 如果没有通过迁移创建种子数据，这里可以添加初始化数据的逻辑
            await context.SaveChangesAsync();
        }
    }
} 