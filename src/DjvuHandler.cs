using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace jdp
{
    public static class DjvuHandler
    {        
        public static void Split(
            FileInfo input,
            DirectoryInfo output,
            string PageName,
            bool verbose,
            IConsole console,
            CancellationToken cancellationToken)
        {
            console.Out.WriteLine($"I am djvu split method!");
        }

        public static void Join (
            DirectoryInfo input,
            FileInfo output,
            bool verbose,
            IConsole console,
            CancellationToken cancellationToken)
        {
            console.Out.WriteLine($"I am djvu join method!");
        }

        public static Command GetCommand(Option Verbose)
        {
            var InputFile = new Option<FileInfo>(
                aliases: new[] {  "--input-file", "--input", "-i" },
                description: "Input file");
            InputFile.IsRequired = true;

            var InputDirectory = new Option<DirectoryInfo>(
                aliases: new[] { "--input-directory", "--input", "-i" },
                description: "Input directory");
            InputDirectory.IsRequired = true;

            var OutputFile = new Option<FileInfo>(
                aliases: new[] { "--output-file", "--output", "-o" },
                description: "Output file");

            var OutputDirectory = new Option<DirectoryInfo>(
                aliases: new[] { "--output-directory", "--output", "-o" },
                description: "Output directory");

            var PageName = new Option<string>(
                aliases: new[] { "--page-name", "--page", "-p" },
                description: "Page name",
                getDefaultValue: () => "page-000");

            var splitCmd = new Command("split", "Split DJVU document into pages")
            {
                InputFile.ExistingOnly(),
                OutputDirectory,
                PageName,
                Verbose
            };

            splitCmd.Handler = CommandHandler
                .Create<FileInfo, DirectoryInfo, string, bool, IConsole, CancellationToken>(Split);

            var joinCmd = new Command("join", "Join pages into DJVU document")
            {
                InputDirectory.ExistingOnly(),
                OutputFile,
                Verbose
            };

            joinCmd.Handler = CommandHandler
                .Create<DirectoryInfo, FileInfo, bool, IConsole, CancellationToken>(Join);

            var cmd = new Command("djvu", "Work with DJVU files")
            {
                splitCmd,
                joinCmd
            };
            return cmd;
        }
    }
}