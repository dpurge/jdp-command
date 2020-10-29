using System;
using System.IO;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace jdp
{
    public static class MainCommand
    {
        public static async Task<int> Main(string[] args)
        {
            var Verbose = new Option(
                aliases: new[] { "--verbose", "-v" },
                description: "Print verbose messages");

            var cmd = new RootCommand
            {
                PdfHandler.GetCommand(Verbose: Verbose),
                DjvuHandler.GetCommand(Verbose: Verbose),

            };

            return await cmd.InvokeAsync(args);
        }

    }
}
