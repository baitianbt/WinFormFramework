namespace WinFormFramework.Common.Infrastructure
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(string fileName, byte[] content, string? subDirectory = null);
        Task<byte[]> GetFileAsync(string fileName, string? subDirectory = null);
        Task DeleteFileAsync(string fileName, string? subDirectory = null);
        bool FileExists(string fileName, string? subDirectory = null);
        string GetFilePath(string fileName, string? subDirectory = null);
        IEnumerable<string> GetFiles(string? searchPattern = null, string? subDirectory = null);
    }
} 