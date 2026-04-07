using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Presentation.Controllers
{
    public class VulnerableController : Controller
    {
        public IActionResult Download(string file, IWebHostEnvironment host)
        {
            string absolutePath = host.ContentRootPath + "/temp-files/" + Path.GetFileName(file);
            //C:/user/name/appdata/source/code/Webapplication1/temp-files/../appsettings.json

            string regexPattern = "^[a-zA-Z0-9.]$";
            if(Regex.IsMatch(file, regexPattern) == true)
            {
                return Content("filename contains invalid characters");
            }

            //attacker: "../appsettings.json" or  "../Data/mydatabase.mdf"
            string relativePath = "temp-files/" + Path.GetFileName(file);
            if(System.IO.File.Exists(relativePath) == false)
            {
                return Content("file doesn't exist");
            }
            
            byte[] myFileBytes = System.IO.File.ReadAllBytes(absolutePath);
            return File(myFileBytes, "application/octet-stream", file);
        }

        public IActionResult Resize(string file)
        {
            string command = $"/c echo Starting conversion of {file} && echo Conversion finished";

            string[] whiteList = { "echo"};
            foreach(string s in file.Split(new char[] {' '}))
            {
                if (whiteList.Contains(s))
                {
                    continue;
                }
                else
                {
                    return Content("invalid injected commands");
                }
            }


            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = command,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process? process = Process.Start(processStartInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                return Content("Output: " + output + "\nError: " + error);
            }
        }
    }
}
