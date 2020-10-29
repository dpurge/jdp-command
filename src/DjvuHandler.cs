using System.CommandLine;
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
            string filename,
            bool verbose,
            IConsole console,
            CancellationToken cancellationToken)
        {
            console.Out.WriteLine($"I am djvu split method!");
        }

        public static void Join(IConsole console)
        {
            console.Out.WriteLine($"I am djvu join method!");
        }
    }
}