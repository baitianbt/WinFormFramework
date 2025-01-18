using Microsoft.EntityFrameworkCore.Migrations;
using WinFormFramework.Common.Utils;

namespace WinFormFramework.DAL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 创建管理员角色
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleName", "Description", "IsSystem", "CreateTime", "CreateBy", "IsDeleted" },
                values: new object[] { 1, "Administrator", "系统管理员", true, DateTime.Now, "System", false }
            );

            // 创建管理员用户
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "UserName", "Password", "RealName", "Email", "IsEnabled", "CreateTime", "CreateBy", "IsDeleted" },
                values: new object[] { 1, "admin", PasswordHelper.HashPassword("admin123"), "系统管理员", "admin@example.com", true, DateTime.Now, "System", false }
            );

            // 分配管理员角色给管理员用户
            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "UserId", "RoleId", "CreateTime", "CreateBy", "IsDeleted" },
                values: new object[] { 1, 1, 1, DateTime.Now, "System", false }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("UserRoles", "Id", 1);
            migrationBuilder.DeleteData("Users", "Id", 1);
            migrationBuilder.DeleteData("Roles", "Id", 1);
        }
    }
} 