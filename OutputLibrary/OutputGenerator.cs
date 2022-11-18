using DirScannerLibrary.Entities;
using System.Text;
using FileInfo = DirScannerLibrary.Entities.FileInfo;

namespace OutputLibrary
{
    public class OutputGenerator : IOutputGenerator
    {
        //private readonly string _MIMETypesListTemlatePath = Directory.GetCurrentDirectory() + "\\ResLibrary\\MIMETypesListTemlate.html";
        //private readonly string _outTemplate = Directory.GetCurrentDirectory() + "\\ResLibrary\\OutTemplate.html";
        //private readonly string _fileStatsTemplatePath = Directory.GetCurrentDirectory() + "\\ResLibrary\\FileStatsTemplate.html";
        //private readonly string _dirStatsTemplatePath = Directory.GetCurrentDirectory() + "\\ResLibrary\\DirStatsTemplate.html";


        private readonly string _MIMETypesListTemlatePath = ResLibrary.Properties.Resources.MIMETypesListTemlate;
        private readonly string _outTemplate = ResLibrary.Properties.Resources.OutTemplate;
        private readonly string _fileStatsTemplatePath = ResLibrary.Properties.Resources.FileStatsTemplate;
        private readonly string _dirStatsTemplatePath = ResLibrary.Properties.Resources.DirStatsTemplate;

        public async Task GenerateHtmlAsync(List<ScanStats> scanStats, List<DirInfo> dirInfos, List<FileInfo> fileInfos)
        {
            string html;
            try
            {
                html = await File.ReadAllTextAsync(_outTemplate);
            }
            catch
            {
                Console.WriteLine("No OutTemplate.html template");
                return;
            }

            StringBuilder typeListStats;
            if (scanStats != null)
            {
                try
                {
                    typeListStats = await GenerateMIMEStatList(scanStats);

                    html = ReplaceGeneratedList(html, typeListStats.ToString(), "{{TypeList}}");
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("No MIMETypesListTemlate.html template");
                    return;
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("No resourses");
                    return;
                }
            }
            else
            {
                html = ReplaceGeneratedList(html, "No info", "{{TypeList}}");
            }

            StringBuilder dirListStats;
            if (dirInfos != null)
            {
                try
                {
                    dirListStats = await GenerateDirsStatList(dirInfos);

                    html = ReplaceGeneratedList(html, dirListStats.ToString(), "{{DirsList}}");
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("No DirStatsTemplate.html template");
                    return;
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("No resourses");
                    return;
                }

            }
            else
            {
                html = ReplaceGeneratedList(html, "No info", "{{DirsList}}");
            }

            StringBuilder fileListStats;
            if (fileInfos != null)
            {
                try
                {
                    fileListStats = await GenerateFilesStatList(fileInfos);

                    html = ReplaceGeneratedList(html, fileListStats.ToString(), "{{FilesList}}");
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("No FileStatsTemplate.html template");
                    return;
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("No resourses");
                    return;
                }
            }
            else
            {
                html = ReplaceGeneratedList(html, "No info", "{{FilesList}}");
            }

            await CreateHTMLOut(html);
        }

        private async Task<StringBuilder> GenerateMIMEStatList(List<ScanStats> scanStats)
        {
            if (scanStats.Count == 0)
                return new StringBuilder("No elements");

            var html = await File.ReadAllTextAsync(_MIMETypesListTemlatePath);


            var res = new StringBuilder();
            foreach (var stat in scanStats)
            {
                var li = html.Replace("{{stat.Name}}", stat.Name)
                    .Replace("{{stat.AvgSize}}", stat.AvgSize.ToString())
                    .Replace("{{stat.QuantitativeRatio}}", stat.QuantitativeRatio)
                    .Replace("{{stat.Percentage}}", stat.Percentage.ToString());

                res.Append(li);
            }

            return res;
        }

        private async Task<StringBuilder> GenerateDirsStatList(List<DirInfo> scanStats)
        {
            if (scanStats.Count == 0)
                return new StringBuilder("No elements");

            var html = await File.ReadAllTextAsync(_dirStatsTemplatePath);

            var res = new StringBuilder();
            foreach (var stat in scanStats)
            {
                var li = html.Replace("{{stat.Name}}", stat.Name)
                    .Replace("{{stat.Size}}", stat.Size.ToString());

                res.Append(li);
            }

            return res;
        }

        private async Task<StringBuilder> GenerateFilesStatList(List<FileInfo> scanStats)
        {
            if (scanStats.Count == 0)
                return new StringBuilder("No elements");

            var html = await File.ReadAllTextAsync(_fileStatsTemplatePath);

            var res = new StringBuilder();
            foreach (var stat in scanStats)
            {
                var li = html.Replace("{{stat.Name}}", stat.Name)
                    .Replace("{{stat.Size}}", stat.Size.ToString())
                    .Replace("{{stat.MIMEType}}", stat.MIMEType);
                res.Append(li);
            }

            return res;
        }

        private string ReplaceGeneratedList(string src, string generatedList, string replaceVariable)
        {
            var res = src.Replace(replaceVariable, generatedList);

            return res;
        }

        private async Task CreateHTMLOut(string html) => await File.WriteAllTextAsync("D:\\Projects\\DirScanner\\ResLibrary\\" + "out.html", html);

    }
}
