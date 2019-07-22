using System;

public class Image
{
    public string Repository {get;set;}
    public string Tag { get; set; }
    public string ImageID { get; set; }
    public string Created { get; set; }
    public string Size { get; set; }
    public string LongTag => $"{Repository}:{Tag}";

    public string GetImageInfo()
    {
        return $"{LongTag}; Created: {Created}; Size: {Size}";
    }
}