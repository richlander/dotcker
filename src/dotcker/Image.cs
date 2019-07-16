using System.Collections.Generic;
using System.IO;

public class Image
{

    public string Repository {get; private set;}
    public string Tag { get; private set; }
    public string ImageID { get; private set; }
    public string Created { get; private set; }
    public string Size { get; private set; }

    public static List<Image> GetImagesForString(StreamReader dockerImagesOutput)
    {
        var images = new List<Image>();
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

            images.Add(image);
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