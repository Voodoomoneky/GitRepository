using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using MvcApplication3.Models;

namespace MvcApplication3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string path,string previousPath)
        {
            ViewData["ErrorOccured"] = "hidden";
            List<LinkData> AllDirectories = new List<LinkData>();
            Dictionary<string,decimal> AllFiles = new Dictionary<string,decimal>();
            if (path == null)
            {
                ViewData["Path"] = "Please choose any drive";
                DriveInfo[] AllDrives = DriveInfo.GetDrives();
                foreach (var x in AllDrives)
                    AllDirectories.Add(new LinkData(){Name = x.Name,URL = x.Name.Split('\\')[0]});
            }
            else
            {

                try
                {
                    DirectoryInfo ChosenDirectory;
                    if (path.Length < 3)// check if previous page was "Drives"
                    {
                        AllDirectories.Add(new LinkData() { Name = "..", URL = "" });//Add dots to drives selection
                        ChosenDirectory = new DirectoryInfo(path + "\\");
                    }
                    else
                    {
                        AllDirectories.Add(new LinkData() { Name = "..", URL = path.Substring(0, path.Length - path.Split('\\')[path.Split('\\').Length - 1].Length - 1) });// Add dots to previous directory
                        ChosenDirectory = new DirectoryInfo(path);// Get the directory that was chosen
                    }
                    CountAmountOfFiles(ChosenDirectory);
                    path += '\\';
                    ViewData["Path"] = "You are here:" + path;// Path label
                    foreach (var x in ChosenDirectory.GetDirectories())
                        AllDirectories.Add(new LinkData() { Name = x.Name, URL = path + x.Name });// Add all subdirectories chosen directory

                    foreach (var x in ChosenDirectory.GetFiles())
                        AllFiles.Add(x.Name, x.Length / 1000);// Add all files from the chosen directory
                }
                catch
                {
                    ViewData["ErrorOccured"] = "shown";
                }
            }
            ViewData["AllFiles"] = AllFiles;
            return View(AllDirectories);
        }
        public void CountAmountOfFiles(DirectoryInfo ChosenDirectory)
        {
          
            int SmallFiles = 0; // <=10 mb
            int MiddleFiles = 0; // >10 mb <=50mb
            int BigFiles = 0;// =>100mb
            FileInfo[] Files = ChosenDirectory.GetFiles();
            foreach (FileInfo f in Files)
            {
                decimal FileSize = f.Length / 1000000;
                if (FileSize <= 10)
                    SmallFiles++;
                else if (FileSize <= 50 && FileSize > 10)
                    MiddleFiles++;
                else if (FileSize >= 100)
                    BigFiles++;
            }

            DirectoryInfo[] SubDirectories = ChosenDirectory.GetDirectories();
            foreach(var x in SubDirectories)
            {
                try
                {
                    Files = x.GetFiles();
                    foreach (FileInfo f in Files)
                    {
                        decimal FileSize = f.Length / 1000000;
                        if (FileSize <= 10)
                            SmallFiles++;
                        else if (FileSize <= 50 && FileSize > 10)
                            MiddleFiles++;
                        else if (FileSize >= 100)
                            BigFiles++;
                    }
                }
                catch {}
            }
            
            ViewData["SmallFiles"] = SmallFiles;
            ViewData["MiddleFiles"] = MiddleFiles;
            ViewData["BigFiles"] = BigFiles;
        }
    }
}
