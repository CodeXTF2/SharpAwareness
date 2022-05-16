using System;
using Microsoft.Win32;
using System.Security.Principal;
using System.Management;
using System.Diagnostics;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Net;
using System.IO;
using System.Windows;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.ComponentModel;
/*
SharpAwareness - A light and more OPSEC friendly way for red teamers to gain quick situational awareness of both the host and the user.
https://github.com/CodeXTF2/SharpAwareness
*/
namespace sad{
internal class Win32
{
    public const int ErrorSuccess = 0;

    [DllImport("Netapi32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
    public static extern int NetGetJoinInformation(string server, out IntPtr domain, out NetJoinStatus status);

    [DllImport("Netapi32.dll")]
    public static extern int NetApiBufferFree(IntPtr Buffer);

    public enum NetJoinStatus
    {
        NetSetupUnknownStatus = 0,
        NetSetupUnjoined,
        NetSetupWorkgroupName,
        NetSetupDomainName
    }

}
    class Program{
    public static bool IsInDomain()
    {
        Win32.NetJoinStatus status = Win32.NetJoinStatus.NetSetupUnknownStatus;
        IntPtr pDomain = IntPtr.Zero;
        int result = Win32.NetGetJoinInformation(null, out pDomain, out status);
        if (pDomain != IntPtr.Zero)
        {
            Win32.NetApiBufferFree(pDomain);
        }
        if (result == Win32.ErrorSuccess)
        {
            return status == Win32.NetJoinStatus.NetSetupDomainName;
        }
        else
        {
            throw new Exception("Domain Info Get Failed", new Win32Exception());
        }
    }
        public static string[] edrlist =
        {
                "activeconsole",
                "amsi.dll",
                "anti malware",
                "anti-malware",
                "antimalware",
                "anti virus",
                "anti-virus",
                "antivirus",
                "appsense",
                "authtap",
                "avast",
                "avecto",
                "canary",
                "carbonblack",
                "carbon black",
                "cb.exe",
                "ciscoamp",
                "cisco amp",
                "countercept",
                "countertack",
                "cramtray",
                "crssvc",
                "crowdstrike",
                "csagent",
                "csfalcon",
                "csshell",
                "cybereason",
                "cyclorama",
                "cylance",
                "cyoptics",
                "cyupdate",
                "cyvera",
                "cyserver",
                "cytray",
                "darktrace",
                "defendpoint",
                "defender",
                "eectrl",
                "elastic",
                "endgame",
                "f-secure",
                "forcepoint",
                "fireeye",
                "groundling",
                "GRRservic",
                "inspector",
                "ivanti",
                "kaspersky",
                "lacuna",
                "logrhythm",
                "malware",
                "mandiant",
                "mcafee",
                "morphisec",
                "msascuil",
                "msmpeng",
                "nissrv",
                "omni",
                "omniagent",
                "osquery",
                "Palo Alto Networks",
                "pgeposervice",
                "pgsystemtray",
                "privilegeguard",
                "procwall",
                "protectorservic",
                "qradar",
                "redcloak",
                "secureworks",
                "securityhealthservice",
                "semlaunchsv",
                "sentinel",
                "sepliveupdat",
                "sisidsservice",
                "sisipsservice",
                "sisipsutil",
                "smc.exe",
                "smcgui",
                "snac64",
                "sophos",
                "splunk",
                "srtsp",
                "symantec",
                "symcorpu",
                "symefasi",
                "sysinternal",
                "sysmon",
                "tanium",
                "tda.exe",
                "tdawork",
                "tpython",
                "vectra",
                "wincollect",
                "windowssensor",
                "wireshark",
                "threat",
                "xagt.exe",
                "xagtnotif.exe"
        };
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
            string edrstrings = "";
            try{
                Console.WriteLine(" >> SharpAwareness << - by CodeX\n");
                Console.WriteLine("\n[*] Starting host recon");
                try{
                //Get Windows Version
                    Console.WriteLine("\n[*] Windows Version:\n" + GetVersionInfo() + "\n");
                }catch{
                    Console.WriteLine("\n[!] Failed to get Windows version");
                }

                try{
                    //Get users that have logged in to this box
                    var localusers = Directory.GetDirectories("C:\\Users");
                    Console.WriteLine("\n[*] Local user folders:");
                    for (int i = 0; i < localusers.Length; i++)
                    {
                        if(localusers[i] != "C:\\Users\\Public" && localusers[i] != "C:\\Users\\All Users"){
                            Console.WriteLine(localusers[i]);
                        }
                    }
                }catch{
                    Console.WriteLine("\n[!] Failed to enumerate local user folders");
                }

                try{
                    //Installed programs
                    var programfiles = Directory.GetDirectories("C:\\Program Files");
                    Console.WriteLine("\n[*] Installed programs:");
                    for (int i = 0; i < programfiles.Length; i++)
                    {
                        edrstrings += programfiles[i];
                        Console.WriteLine(programfiles[i]);
                        
                    }
                    var programfiles2 = Directory.GetDirectories("C:\\Program Files (x86)");
                    for (int i = 0; i < programfiles2.Length; i++)
                    {
                        edrstrings += programfiles[i];
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
                            edrstrings += p.ProcessName;
                            edrstrings += p.MainModule.FileVersionInfo.FileDescription;
                        }catch{
                            Console.WriteLine(p.Id + " " + p.ProcessName); 
                            edrstrings += p.ProcessName;
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

                //Proxy settings
                try{
                    Console.WriteLine("\n[!] Enumerating IE proxy settings");
                    var uri = WebRequest.DefaultWebProxy.GetProxy(new Uri("http://www.google.com"));
                    if(uri.ToString() != "http://www.google.com/"){
                        Console.WriteLine("Proxy address: " + uri.ToString());
                    }else{
                        Console.WriteLine("No proxy configured");
                    }
                }catch{
                    Console.WriteLine("\n[!] Failed to enumerate proxy settings");
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

                //Logon times
                Console.WriteLine("\n[*] Enumerating logon logoff times");
                try{
                    DirectoryEntry dirs = new DirectoryEntry("WinNT://" + Environment.MachineName);
                    foreach (DirectoryEntry de in dirs.Children)
                    {
                        if (de.SchemaClassName == "User")
                        {
                            Console.WriteLine("Name: " + de.Name);
                            if (de.Properties["lastlogin"].Value != null)
                            {
                                Console.WriteLine("Last login: " + de.Properties["lastlogin"].Value.ToString());
                            }
                            if (de.Properties["lastlogoff"].Value != null)
                            {
                                Console.WriteLine("Last logoff: " + de.Properties["lastlogoff"].Value.ToString());
                            }
                            Console.WriteLine();
                        }
                    }
                }catch{
                    Console.WriteLine("\n[!] Failed to enumerate last logon times");
                }

                //EDRs installed?
                try{
                    Console.WriteLine("\n[*] Identifying security products");
                    
                    foreach(string s in edrlist){
                        if (edrstrings.ToLower().Contains(s)){
                            Console.WriteLine("- " + s);
                        }
                    }
                }catch{
                    Console.WriteLine("\n[!] Failed to locate EDR strings");
                }
                Console.WriteLine("\n[*] Checking for Active Directory");
                if(IsInDomain()){
                    Console.WriteLine("Host IS domain joined.");
                }else{
                    Console.WriteLine("Host is NOT domain joined.");
                }

            //RIP my code broke :(
            }catch{
                Console.WriteLine("\n[!] SharpAwareness crashed :(");
            }
        
        }

    }


}