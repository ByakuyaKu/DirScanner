using DirScannerLibrary.Entities;
using Microsoft.AspNetCore.StaticFiles;
using FileInfo = DirScannerLibrary.Entities.FileInfo;

namespace DirScannerLibrary
{
    public class DirScanner : IDirScanner
    {
        public List<ScanStats> ScanDir(out List<DirInfo> dirsInfo, out List<FileInfo> filesInfo)
        {
            string path;
            try
            {
                path = Directory.GetCurrentDirectory();
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("You do not have permission to access one or more folders in this directory tree.");
                dirsInfo = null;
                filesInfo = null;
                return null;
            }

            var files = GetAllFiles(path);

            var dirs = GetAllDirs(path);

            List<ScanStats> stats = null;
            filesInfo = null;
            if (files != null)
            {
                filesInfo = GetFilesInfoParallel(files);
                stats = GetFilesStatsParallel(filesInfo);
            }

            dirsInfo = null;
            if (dirs != null)
                dirsInfo = GetDirsInfoParallel(dirs);

            return stats;
        }

        private List<ScanStats> GetFilesStatsParallel(List<FileInfo> filesInfo)
        {
            var types = filesInfo.DistinctBy(f => f.MIMEType).Select(f => f.MIMEType);

            var scanStats = new List<ScanStats>();

            Parallel.ForEach(types, type =>
            {
                var typeCollection = filesInfo.Where(f => f.MIMEType.Equals(type));

                var avgSize = Math.Round(typeCollection.Average(f => f.Size), 3);
                var typeCount = typeCollection.Count();

                scanStats.Add(new ScanStats(type,
                    $"{typeCount} : {filesInfo.Count}",
                    Math.Round((double)typeCount / filesInfo.Count, 3),
                    avgSize));
            });

            return scanStats;
        }

        private string[] GetAllFiles(string path)
        {
            try
            {
                var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                return files;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("You do not have permission to access one or more folders in this directory tree.");
                return null;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"The specified directory {path} was not found.");
                return null;
            }
        }

        private string[] GetAllDirs(string path)
        {
            try
            {
                var dirs = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
                return dirs;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("You do not have permission to access one or more folders in this directory tree.");
                return null;
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"The specified directory {path} was not found.");
                return null;
            }
        }

        private DirInfo GetDirInfo(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);

            var dirSize = dirInfo.EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length);
            var dirName = dirInfo.Name;

            return new DirInfo(dirSize, dirName);
        }

        private List<DirInfo> GetDirsInfoParallel(string[] dirs)
        {
            var dirInfo = new List<DirInfo>();

            Parallel.ForEach(dirs, dir =>
            {
                var info = GetDirInfo(dir);

                dirInfo.Add(info);
            });

            return dirInfo;
        }

        private FileInfo GetFileInfo(string path)
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(path);

            var MIME = GetMIMETypeOfFile(path);

            return new FileInfo(fileInfo.Name, MIME, fileInfo.Length);
        }

        private List<FileInfo> GetFilesInfoParallel(string[] files)
        {
            var filesInfo = new List<FileInfo>();

            Parallel.ForEach(files, file =>
            {
                var info = GetFileInfo(file);

                filesInfo.Add(info);
            });

            return filesInfo;
        }

        private string GetMIMETypeOfFile(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;

            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "unknown";
            }

            return contentType;
        }
    }
}
