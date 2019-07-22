using System;
using System.CommandLine;
using System.IO;

namespace dotcker
{
    class Program
    {
        static void Main(string[] args)
        {
            args = new string[] {"--pull","mcr.microsoft.com/dotnet/core/runtime"};
            var startup = new Startup();
            (var success, var argument) = startup.Parse(args);

            if (!success)
            {
                Console.WriteLine(argument);
                return;
            }

            switch(startup.GetCommand())
            {
                case AppCommands.PullImage when argument is string tag:
                    PullAndPrintImageSingle(tag);
                    break;
                case AppCommands.PullImage when argument is TextReader reader:
                    PullAndPrintImageMultiple(reader);
                    break;
                default:
                    Console.WriteLine("bad things");
                    break;
            }
        }

        private static void PullAndPrintImageMultiple(TextReader reader)
        {
            var images = Docker.GetImages(reader);

            foreach(var image in images.Keys)
            {
                Docker.Pull(image);
            }

        }

        private static void PullAndPrintImageSingle(string tag)
        {
            var imageInfo = string.Empty;
            var updatedImageInfo = string.Empty;

            if (!tag.Contains(':'))
            {
                tag = $"{tag}:latest";
            }
            var images = Docker.GetImages(Docker.Images());
            if (images.ContainsKey(tag))
            {
                imageInfo = images[tag].GetImageInfo();
            }


            Console.WriteLine(imageInfo);

            if (imageInfo != updatedImageInfo)
            {
                Console.WriteLine("Updated:");
                Console.WriteLine(updatedImageInfo);
            }
        }

        private static string PullAndPrintImage(string tag)
        {
            var stream = Docker.Pull(tag);
            var images = Docker.GetImages(Docker.Images());
            var imageInfo = string.Empty;
            if (images.ContainsKey(tag))
            {
                imageInfo = images[tag].GetImageInfo();
            }
            return imageInfo;
        }
    }
}
