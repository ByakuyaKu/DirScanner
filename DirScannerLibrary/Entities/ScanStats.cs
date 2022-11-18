namespace DirScannerLibrary.Entities
{
    public class ScanStats
    {
        public string Name { get; set; }

        public string QuantitativeRatio { get; set; }

        public double Percentage { get; set; }

        public double AvgSize { get; set; }

        public ScanStats(string _name, string _quantitativeRatio, double _percentage, double _avgSize)
        {
            Name = _name;
            QuantitativeRatio = _quantitativeRatio;
            Percentage = _percentage;
            AvgSize = _avgSize;
        }
    }
}
