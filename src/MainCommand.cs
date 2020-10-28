using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace jdp
{
    class MainCommand
    {
        public static async Task<int> Main(string[] args)
        {
            var command = new RootCommand
            {
                new Option(new [] {"--verbose", "-v"}),
                new Option("--name", "-n") { Argument = new Argument<string>() }
            };

            command.Handler = CommandHandler.Create(
                (bool verbose, string name) =>
                {
                    if (verbose)
                        Console.WriteLine("Preparing for greeting...");

                    Console.WriteLine("Hello World!");

                    if (verbose)
                        Console.WriteLine("Greeting completed.");
                }
            );
            
            await command.InvokeAsync(args);
            return 0;
        }
    }
}
