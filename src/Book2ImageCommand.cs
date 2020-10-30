using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace jdp.command
{
    public class Book2ImageCommand : Command
    {
        private readonly string PdfImages;
        private readonly string DDjVu;

        public Book2ImageCommand(IConfiguration configuration)
           : base("book2image", "Extract images from PDF/DjVu scanned books.")
        {
            var inputFile = new Option<FileInfo>(new string[] {"--input-file", "--input", "-i"})
            {
                Name = "inputFile",
                Description = "PDF or DjVu file with scanned book.",
                IsRequired = true
            };

            var outputDirectory = new Option<DirectoryInfo>(new string[] {"--output-directory", "--output", "-o"})
            {
                Name = "outputDirectory",
                Description = "Directory to save image files."
            };

            var verbose = new Option<bool>(new string[] {"--verbose", "-v"})
            {
                Name = "verbose",
                Description = "Print verbose messages."
            };

            this.AddOption(inputFile.ExistingOnly());
            this.AddOption(outputDirectory);
            this.AddOption(verbose);

            this.Handler = CommandHandler.Create<FileInfo, DirectoryInfo, bool>(this.HandleCommand);

            this.PdfImages = configuration
                .GetSection("Tools")
                .GetSection("PdfImages")
                .Get<string>();

            this.DDjVu = configuration
                .GetSection("Tools")
                .GetSection("DDjVu")
                .Get<string>();
        }

        private int HandleCommand(FileInfo inputFile, DirectoryInfo? outputDirectory, bool verbose = false)
        {
            if(verbose) Console.WriteLine($"Input file: {inputFile?.FullName}");

            var bookFormat = BookFormat.Unknown;
            ProcessStartInfo startInfo = new ProcessStartInfo();

            FileInfo pdfImages;
            FileInfo dDjVu;

            switch (inputFile?.Extension)
            {
                case ".pdf":
                    bookFormat = BookFormat.Pdf;
                    break;
                case ".djvu":
                    bookFormat = BookFormat.DjVu;
                    break;
                default:
                    Console.WriteLine($"Unsupported book extension: {inputFile?.Extension}");
                    return (int) ReturnCode.InvalidInput;
            }

            if (bookFormat == BookFormat.Pdf) {
                if (string.IsNullOrEmpty(this.PdfImages)) {
                    Console.WriteLine("Missing tool in configuration: PdfImages");
                    return (int) ReturnCode.MissingTool;
                }

                pdfImages = new FileInfo(this.PdfImages);

                if (!pdfImages.Exists) {
                    Console.WriteLine($"Missing PdfImages: {pdfImages.FullName}");
                    return (int) ReturnCode.MissingTool;
                }

                if(verbose) Console.WriteLine($"Found PdfImages: {pdfImages.FullName}");
                startInfo.FileName = pdfImages.FullName;
            }
            
            if (bookFormat == BookFormat.DjVu) {
                if (string.IsNullOrEmpty(this.DDjVu)) {
                    Console.WriteLine("Missing tool in configuration: DDjVu");
                    return (int) ReturnCode.MissingTool;
                }

                dDjVu = new FileInfo(this.DDjVu);

                if (!dDjVu.Exists) {
                    Console.WriteLine($"Missing DDjVu: {dDjVu.FullName}");
                    return (int) ReturnCode.MissingTool;
                }

                if(verbose) Console.WriteLine($"Found DDjVu: {dDjVu.FullName}");
                startInfo.FileName = dDjVu.FullName;
            }
            

            if (outputDirectory == null) {
                if (verbose) Console.WriteLine("Output directory: generating default...");
                outputDirectory = new DirectoryInfo(
                    Path.Join(
                        inputFile?.Directory?.FullName,
                        Path.GetFileNameWithoutExtension(inputFile?.Name)));
            }

            
            if (outputDirectory.Exists) {
                if (verbose) Console.WriteLine($"Output directory exists: {outputDirectory.FullName}");
            } else {
                if (verbose) Console.WriteLine($"Creating output directory: {outputDirectory.FullName}");
                try {
                    outputDirectory.Create();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return (int) ReturnCode.ProcessingError;
                }
            }

            if (bookFormat == BookFormat.Pdf) {
                startInfo.Arguments = $"-png \"{inputFile.FullName}\" \"{Path.Join(outputDirectory.FullName, "page")}\"";
            }

            if (bookFormat == BookFormat.DjVu) {
                startInfo.Arguments = $"-format=tiff -eachpage \"{inputFile.FullName}\" \"{Path.Join(outputDirectory.FullName, "page")}-%04d.tiff\"";
            }

            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;

            using(System.Diagnostics.Process pProcess = new System.Diagnostics.Process()) {
                pProcess.StartInfo = startInfo;
                pProcess.Start();
                string output = pProcess.StandardOutput.ReadToEnd();
                pProcess.WaitForExit();
            }

            return (int) ReturnCode.Success;
        }
    }
}