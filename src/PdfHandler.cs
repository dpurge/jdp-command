using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace jdp
{
    public static class PdfHandler
    {
        public static async Task<int> Split (
            FileInfo input,
            DirectoryInfo output,
            string pageFilename,
            bool verbose,
            IConsole console,
            CancellationToken cancellationToken)
        {
            if (output == null) {
                output = new DirectoryInfo(
                    Path.GetFileNameWithoutExtension(input.Name));
            }

            const int delay = 0;
            // while (!cancellationToken.IsCancellationRequested)
            // {
                await Task.Delay(delay, cancellationToken);
            // }
            if (verbose) {
                console.Out.WriteLine($"Input file: {input.FullName}");
                console.Out.WriteLine($"Output directory: {output.FullName}");
                console.Out.WriteLine($"Filename pattern: {pageFilename}");
            } 
            
            console.Out.WriteLine($"I am pdf split method!");
            return 0;
        }

        public static async Task<int> Join (
            DirectoryInfo input,
            FileInfo output,
            bool verbose,
            IConsole console,
            CancellationToken cancellationToken)
        {
            console.Out.WriteLine($"I am pdf join method!");
            await Task.Delay(0, cancellationToken);
            return 0;
        }
        
        public static void Convert(IConsole console, CancellationToken cancellationToken)
        {
            console.Out.WriteLine($"I am pdf convert method!");
        }

        public static void SplitPrint(IConsole console)
        {
            console.Out.WriteLine($"I am pdf split-print method!");
        }

        public static Command GetCommand(Option Verbose)
        {
            var InputFile = new Option<FileInfo>(
                aliases: new[] { "--input", "-i" },
                description: "Input file");
            InputFile.IsRequired = true;

            var InputDirectory = new Option<DirectoryInfo>(
                aliases: new[] { "--input", "-i" },
                description: "Input directory");
            InputDirectory.IsRequired = true;

            var OutputFile = new Option<FileInfo>(
                aliases: new[] { "--output", "-o" },
                description: "Output file");

            var OutputDirectory = new Option<DirectoryInfo>(
                aliases: new[] { "--output", "-o" },
                description: "Output directory");

            var OutputPageFilenameTemplate = new Option<string>(
                aliases: new[] { "--page-filename", "-p" },
                description: "Output page file name template",
                getDefaultValue: () => "page-000.pdf");

            var splitCmd = new Command("split", "Split PDF document into pages")
            {
                InputFile.ExistingOnly(),
                OutputDirectory,
                OutputPageFilenameTemplate,
                Verbose
            };

            splitCmd.Handler = CommandHandler
                .Create<FileInfo, DirectoryInfo, string, bool, IConsole, CancellationToken>(Split);

            var joinCmd = new Command("join", "Join pages into PDF document")
            {
                InputDirectory.ExistingOnly(),
                OutputFile,
                Verbose
            };

            joinCmd.Handler = CommandHandler
                .Create<DirectoryInfo, FileInfo, bool, IConsole, CancellationToken>(Join);

            var cmd = new Command("pdf", "Work with PDF files")
            {
                splitCmd,
                joinCmd
            };
            return cmd;
        }
    }
}