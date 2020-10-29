using System.CommandLine;
using System.CommandLine.IO;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace jdp
{
    public static class PdfHandler
    {
        // public static Command GetCommand()
        // {

        // }
        public static async Task<int> Split (
            FileInfo input,
            DirectoryInfo output,
            string filename,
            bool verbose,
            IConsole console,
            CancellationToken cancellationToken)
        {
            // int delay = 0;
            // while (!cancellationToken.IsCancellationRequested)
            // {
            //     await Task.Delay(delay, cancellationToken);
            // }
            if (verbose) {
                console.Out.WriteLine($"Input file: {input.FullName}");
                console.Out.WriteLine($"Output directory: {output.FullName}");
                console.Out.WriteLine($"Filename pattern: {filename}");
            } 
            
            console.Out.WriteLine($"I am pdf split method!");
            return 0;
        }

        public static async Task<int> Join (
            IConsole console,
            CancellationToken cancellationToken)
        {
            console.Out.WriteLine($"I am pdf join method!");
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
    }
}