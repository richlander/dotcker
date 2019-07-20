using System;
using System.CommandLine;

namespace dotcker
{
    class Program
    {
        static void Main(string[] args)
        {
            //using var reader = Docker.DockerImagePull();
            //var images = Image.GetImagesForString(reader);


            var pullString = "--pull";

            var rootCommand = new RootCommand()
            {
                new Option(pullString){Argument = new Argument(){Arity = ArgumentArity.ZeroOrOne}}
            };
                     
            args = new string[] {pullString, "rich"};
            var result = rootCommand.Parse(args);

            var pullCommand = result.GetCommandObject(pullString);

            if (pullCommand !is object)
            {
                Console.WriteLine("bad input");
                return;
            }

            var pullArgument = pullCommand.Tokens[0]?.Value;

            if (pullArgument is object)
            {
                
            }

        }
    }
}
