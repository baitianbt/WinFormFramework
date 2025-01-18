namespace WinFormFramework.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RequirePermissionAttribute : Attribute
    {
        public string PermissionCode { get; }
        public bool RequireAll { get; set; }

        public RequirePermissionAttribute(string permissionCode)
        {
            PermissionCode = permissionCode;
        }

        public RequirePermissionAttribute(params string[] permissionCodes)
        {
            PermissionCode = string.Join(",", permissionCodes);
        }
    }
} 