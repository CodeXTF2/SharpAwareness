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

            //Get Windows Version
            Console.WriteLine("[*] Windows Version:\n" + GetVersionInfo() + "\n");

            //Get users that have logged in to this box
            var localusers = Directory.GetDirectories("C:\\Users");
            Console.WriteLine("[*] Local users:");
            for (int i = 0; i < localusers.Length; i++)
            {
                if(localusers[i] != "C:\\Users\\Public" && localusers[i] != "C:\\Users\\All Users"){
                    Console.WriteLine(localusers[i]);
                }
            }

            //Installed programs
            var programfiles = Directory.GetDirectories("C:\\Program Files");
            Console.WriteLine("[*] Installed programs:");
            for (int i = 0; i < programfiles.Length; i++)
            {
 
                Console.WriteLine(programfiles[i]);
                
            }
            var programfiles2 = Directory.GetDirectories("C:\\Program Files (x86)");
            for (int i = 0; i < programfiles2.Length; i++)
            {
 
                Console.WriteLine(programfiles2[i]);
                
            }

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

            //Foreground window


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
            //Drives
            Console.WriteLine("\n[*] Logical drives");
            string[] drives = System.IO.Directory.GetLogicalDrives();

            foreach (string str in drives) 
            {
                System.Console.WriteLine(str);
            }


            }catch{
                Console.WriteLine("SharpAwareness broke :( At least we didn't kill your beacon!");
            }
        
        }

    }


}