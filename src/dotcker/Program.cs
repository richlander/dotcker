using System;

namespace dotcker
{
    class Program
    {
        static void Main(string[] args)
        {
            using var reader = Docker.DockerImagePull();
            var images = Image.GetImagesForString(reader);
            

        }
    }
}
