using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

public class Docker
{
    public static StreamReader Pull(string image)
    {
        var psi = GetPsi();
        psi.Arguments = $"pull {image}";
        var process = Process.Start(psi);
        return process.StandardOutput;
    }

    public static StreamReader Images()
    {
        var psi = GetPsi();
        psi.Arguments = "images";
        var process = Process.Start(psi);
        return process.StandardOutput;
    }

    public static StreamReader Inspect(string image)
    {
        var psi = GetPsi();
        psi.Arguments = $"inspect {image}";
        var process = Process.Start(psi);
        return process.StandardOutput;
    }

    public static ulong GetImageSize(string image)
    {
        var reader = Inspect(image);
        var line = string.Empty;
        while ((line = reader.ReadLine())!=null)
        {
            var index = 0;
            var sizeText = "\"Size\": ";
            if ((index = line.IndexOf(sizeText))!=0)
            {
                var end = line.IndexOf(',');
                var size = line.Substring(sizeText.Length,end-sizeText.Length);
                return uint.Parse(size);
            }
        }
        return 0;
    }

    private static ProcessStartInfo GetPsi()
    {
        var psi = new ProcessStartInfo();
        psi.RedirectStandardOutput = true;
        psi.FileName = "docker";
        return psi;   
    }

    public static Dictionary<string,Image> GetImages(TextReader dockerImagesOutput)
    {
        var images = new Dictionary<string,Image>();
        string line = dockerImagesOutput.ReadLine();
        var offsets = GetOffsetsFromHeader(line);
        while ((line = dockerImagesOutput.ReadLine())!=null)
        {
            var image = new Image()
            {
                Repository = TrimString(line, 0,offsets[1]),
                Tag = TrimString(line, offsets[1], offsets[2]),
                ImageID = TrimString(line, offsets[2], offsets[3]),
                Created = TrimString(line, offsets[3], offsets[4]),
                Size = TrimString(line, offsets[4], line.Length-1),
            };
            images.Add(image.LongTag,image);
        }
        return images;
    }

    private static string TrimString(string line, int offset1, int offset2)
    {
        return line.Substring(offset1, offset2 - offset1).TrimEnd();
    }

    private static int[] GetOffsetsFromHeader(string header)
    {
        var headerStrings = new string[]{
            "REPOSITORY",
            "TAG",
            "IMAGE ID",
            "CREATED",
            "SIZE"
        };
        var offsets = new int[5];

        for (int i = 0; i < headerStrings.Length; i++)
        {
            offsets[i] = header.IndexOf(headerStrings[i]);
        }

        return offsets;
    }
}