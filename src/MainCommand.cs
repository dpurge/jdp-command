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
            var cmd = new RootCommand
            {
                new Command("pdf", "Work with PDF files")
                {
                    new Command("split", "Split document into pages")
                    {
                        new Option<FileInfo>(
                            aliases: new[] { "--input", "-i" },
                            description: "Input file"
                            ).ExistingOnly(),
                        new Option<DirectoryInfo>(
                            aliases: new[] { "--output", "-o" },
                            description: "Output directory",
                            getDefaultValue: () => new DirectoryInfo(".")),
                        new Option<string>(
                            aliases: new[] { "--filename", "-f" },
                            description: "Output file name pattern",
                            getDefaultValue: () => "page-000"),
                        new Option(new[] { "--verbose", "-v" },
                            description: "Print verbose messages"),
                    }.WithHandler(
                        className: nameof(PdfHandler),
                        methodName: nameof(PdfHandler.Split)),

                    new Command("join", "Join pages into document")
                    {
                    }.WithHandler(
                        className: nameof(PdfHandler),
                        methodName: nameof(PdfHandler.Join)),

                    new Command("convert", "Convert document into another format")
                    {
                    }.WithHandler(
                        className: nameof(PdfHandler),
                        methodName: nameof(PdfHandler.Join)),

                    new Command("print-split", "Split document into printable gatherings")
                    {
                    }.WithHandler(
                        className: nameof(PdfHandler),
                        methodName: nameof(PdfHandler.SplitPrint)),
                },

                new Command("djvu", "Work with DJVU files")
                {
                    new Command("split", "Split document into pages")
                    {
                        new Option<FileInfo>(new[] { "--input", "-i" },
                            description: "Input file")
                                .ExistingOnly(),
                        new Option<DirectoryInfo>(new[] { "--output", "-o" },
                            description: "Output directory",
                            getDefaultValue: () => new DirectoryInfo("."))
                                .ExistingOnly(),
                        new Option<string>(new[] { "--filename", "-f" },
                            description: "Output file name pattern",
                            getDefaultValue: () => "page-000"),
                        new Option(new[] { "--verbose", "-v" },
                            description: "Print verbose messages"),
                    }.WithHandler(
                        className: nameof(DjvuHandler),
                        methodName: nameof(DjvuHandler.Split)),

                    new Command("join", "Join pages into document")
                    {
                    }.WithHandler(
                        className: nameof(DjvuHandler),
                        methodName: nameof(DjvuHandler.Join)),
                },

            };

            return await cmd.InvokeAsync(args);
        }

        private static Command WithHandler(this Command command, string className, string methodName)
        {
            string currentNamespace = typeof(MainCommand).Namespace;
            Type handlerType = Type.GetType($"{currentNamespace}.{className}");
            var handlerMethod = handlerType?.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);
            var handler = CommandHandler.Create(handlerMethod!);
            command.Handler = handler;
            return command;
        }
    }
}