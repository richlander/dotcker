using System.Diagnostics;
using System.IO;

public class Docker
{
    public static StreamReader DockerImagePull()
    {
        var psi = GetPsi();
        psi.Arguments = "images";
        var foo = Process.Start(psi);
        return foo.StandardOutput;
    }

    private static ProcessStartInfo GetPsi()
    {
        var psi = new ProcessStartInfo();
        psi.RedirectStandardOutput = true;
        psi.FileName = "docker.exe";
        return psi;   
    }
}