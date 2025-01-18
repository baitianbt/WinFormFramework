using WinFormFramework.Common.Infrastructure;

namespace WinFormFramework.Infrastructure.FileStorage
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _basePath;

        public LocalFileStorageService(string basePath)
        {
            _basePath = basePath;
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        public async Task<string> SaveFileAsync(string fileName, byte[] content, string? subDirectory = null)
        {
            var directory = GetDirectory(subDirectory);
            var filePath = Path.Combine(directory, fileName);

            await File.WriteAllBytesAsync(filePath, content);
            return filePath;
        }

        public async Task<byte[]> GetFileAsync(string fileName, string? subDirectory = null)
        {
            var filePath = GetFilePath(fileName, subDirectory);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            return await File.ReadAllBytesAsync(filePath);
        }

        public Task DeleteFileAsync(string fileName, string? subDirectory = null)
        {
            var filePath = GetFilePath(fileName, subDirectory);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return Task.CompletedTask;
        }

        public bool FileExists(string fileName, string? subDirectory = null)
        {
            var filePath = GetFilePath(fileName, subDirectory);
            return File.Exists(filePath);
        }

        public string GetFilePath(string fileName, string? subDirectory = null)
        {
            var directory = GetDirectory(subDirectory);
            return Path.Combine(directory, fileName);
        }

        public IEnumerable<string> GetFiles(string? searchPattern = null, string? subDirectory = null)
        {
            var directory = GetDirectory(subDirectory);
            return Directory.GetFiles(directory, searchPattern ?? "*.*")
                .Select(f => Path.GetFileName(f)!);
        }

        private string GetDirectory(string? subDirectory)
        {
            if (string.IsNullOrEmpty(subDirectory))
            {
                return _basePath;
            }

            var directory = Path.Combine(_basePath, subDirectory);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }
    }
} 