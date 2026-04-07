using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
namespace Presentation.Controllers
{


    public class ProcessingController : Controller
    {
        public IActionResult CreateDemoFiles()
        {
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "temp-files");
            Directory.CreateDirectory(folder);

            System.IO.File.WriteAllText(Path.Combine(folder, "file1.txt"), "demo 1");
            System.IO.File.WriteAllText(Path.Combine(folder, "file2.txt"), "demo 2");
            System.IO.File.WriteAllText(Path.Combine(folder, "file3.txt"), "demo 3");

            return Content("Demo files created in: " + folder);
        }

        public IActionResult Convert(string file)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            var convertedFolder = Path.Combine(Directory.GetCurrentDirectory(), "converted");

            Directory.CreateDirectory(uploadsFolder);
            Directory.CreateDirectory(convertedFolder);

            // INTENTIONALLY VULNERABLE
            // Pretends to "convert" by copying the chosen uploaded file into the converted folder
            string command =
                $"copy /Y \"{uploadsFolder}\\{file}\" \"{convertedFolder}\\output.jpg\"";

            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/c " + command,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process();
            process.StartInfo = psi;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            if (!process.WaitForExit(5000))
            {
                process.Kill();
                return Content("Command timed out.\r\n\r\nCOMMAND:\r\n" + command);
            }

            return Content(
                "COMMAND RUN:\r\n" + command +
                "\r\n\r\nOUTPUT:\r\n" + output +
                "\r\nERROR:\r\n" + error
            );
        }
    }
}
