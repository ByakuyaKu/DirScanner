// See https://aka.ms/new-console-template for more information
using DirScannerLibrary;
using DirScannerLibrary.Entities;
using OutputLibrary;
using FileInfo = DirScannerLibrary.Entities.FileInfo;

IDirScanner sc = new DirScanner();

var dirInfos = new List<DirInfo>();
var fileInfos = new List<FileInfo>();
var stats = sc.ScanDir(out dirInfos, out fileInfos);

IOutputGenerator output = new OutputGenerator();

await output.GenerateHtmlAsync(stats, dirInfos, fileInfos);
