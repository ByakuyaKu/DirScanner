using DirScannerLibrary.Entities;
using System.Text;
using FileInfo = DirScannerLibrary.Entities.FileInfo;

namespace OutputLibrary
{
    public class OutputGenerator : IOutputGenerator
    {
        private readonly string _MIMETypesListTemlate = ResLibrary.Properties.Resources.MIMETypesListTemlate;
        private readonly string _outTemplate = ResLibrary.Properties.Resources.OutTemplate;
        private readonly string _fileStatsTemplate = ResLibrary.Properties.Resources.FileStatsTemplate;
        private readonly string _dirStatsTemplate = ResLibrary.Properties.Resources.DirStatsTemplate;

        public async Task GenerateHtmlAsync(List<ScanStats> scanStats, List<DirInfo> dirInfos, List<FileInfo> fileInfos)
        {
            if (_MIMETypesListTemlate == null || _outTemplate == null || _fileStatsTemplate == null || _dirStatsTemplate == null)
                return;

            var html = _outTemplate;

            StringBuilder typeListStats;
            if (scanStats != null)
            {

                typeListStats = await GenerateMIMEStatList(scanStats);

                html = ReplaceGeneratedList(html, typeListStats.ToString(), "{{TypeList}}");

            }
            else
            {
                html = ReplaceGeneratedList(html, "No info", "{{TypeList}}");
            }

            StringBuilder dirListStats;
            if (dirInfos != null)
            {

                dirListStats = await GenerateDirsStatList(dirInfos);

                html = ReplaceGeneratedList(html, dirListStats.ToString(), "{{DirsList}}");


            }
            else
            {
                html = ReplaceGeneratedList(html, "No info", "{{DirsList}}");
            }

            StringBuilder fileListStats;
            if (fileInfos != null)
            {

                fileListStats = await GenerateFilesStatList(fileInfos);

                html = ReplaceGeneratedList(html, fileListStats.ToString(), "{{FilesList}}");
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

            var html = _MIMETypesListTemlate;


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

            var html = _dirStatsTemplate;

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

            var html = _fileStatsTemplate;

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
