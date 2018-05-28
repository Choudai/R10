namespace Rewrite
{
    class ISAAC
    {
        public const int SIZEL = 9;
        public const int SIZE = 1 << SIZEL;
        public const int MASK = (SIZE - 1) << 2;
        public int count;
        public int[] rsl;
        public int[] mem;
        private int a;
        private int b;
        private int c;

        public ISAAC()
        {
            mem = new int[SIZE];
            rsl = new int[SIZE];
            Init(false);
        }

        public ISAAC(int[] seed)
        {
            mem = new int[SIZE];
            rsl = new int[SIZE];
            for (int i = 0; i < seed.Length; ++i)
            {
                rsl[i] = seed[i];
            }
            Init(true);
        }

        public void Isaac()
        {
            int i, j, x, y;

            b += ++c;
            for (i = 0, j = SIZE / 2; i < SIZE / 2;)
            {
                x = mem[i];
                a ^= a << 13;
                a += mem[j++];
                mem[i] = y = mem[(x & MASK) >> 2] + a + b;
                rsl[i++] = b = mem[((y >> SIZEL) & MASK) >> 2] + x;

                x = mem[i];
                a ^= (int)((uint)a >> 6);
                a += mem[j++];
                mem[i] = y = mem[(x & MASK) >> 2] + a + b;
                rsl[i++] = b = mem[((y >> SIZEL) & MASK) >> 2] + x;

                x = mem[i];
                a ^= a << 2;
                a += mem[j++];
                mem[i] = y = mem[(x & MASK) >> 2] + a + b;
                rsl[i++] = b = mem[((y >> SIZEL) & MASK) >> 2] + x;

                x = mem[i];
                a ^= (int)((uint)a >> 16);
                a += mem[j++];
                mem[i] = y = mem[(x & MASK) >> 2] + a + b;
                rsl[i++] = b = mem[((y >> SIZEL) & MASK) >> 2] + x;
            }

            for (j = 0; j < SIZE / 2;)
            {
                x = mem[i];
                a ^= a << 13;
                a += mem[j++];
                mem[i] = y = mem[(x & MASK) >> 2] + a + b;
                rsl[i++] = b = mem[((y >> SIZEL) & MASK) >> 2] + x;

                x = mem[i];
                a ^= (int)((uint)a >> 6);
                a += mem[j++];
                mem[i] = y = mem[(x & MASK) >> 2] + a + b;
                rsl[i++] = b = mem[((y >> SIZEL) & MASK) >> 2] + x;

                x = mem[i];
                a ^= a << 2;
                a += mem[j++];
                mem[i] = y = mem[(x & MASK) >> 2] + a + b;
                rsl[i++] = b = mem[((y >> SIZEL) & MASK) >> 2] + x;

                x = mem[i];
                a ^= (int)((uint)a >> 16);
                a += mem[j++];
                mem[i] = y = mem[(x & MASK) >> 2] + a + b;
                rsl[i++] = b = mem[((y >> SIZEL) & MASK) >> 2] + x;
            }
        }

        public void Init(bool flag)
        {
            int i;
            int a, b, c, d, e, f, g, h;
            a = b = c = d = e = f = g = h = unchecked((int)0x9e3779b9);

            for (i = 0; i < 4; ++i)
            {
                a ^= b << 11; d += a; b += c;
                b ^= (int)((uint)c >> 2); e += b; c += d;
                c ^= d << 8; f += c; d += e;
                d ^= (int)((uint)e >> 16); g += d; e += f;
                e ^= f << 10; h += e; f += g;
                f ^= (int)((uint)g >> 4); a += f; g += h;
                g ^= h << 8; b += g; h += a;
                h ^= (int)((uint)a >> 9); c += h; a += b;
            }

            for (i = 0; i < SIZE; i += 8)
            {
                if (flag)
                {
                    a += rsl[i]; b += rsl[i + 1]; c += rsl[i + 2]; d += rsl[i + 3];
                    e += rsl[i + 4]; f += rsl[i + 5]; g += rsl[i + 6]; h += rsl[i + 7];
                }
                a ^= b << 11; d += a; b += c;
                b ^= (int)((uint)c >> 2); e += b; c += d;
                c ^= d << 8; f += c; d += e;
                d ^= (int)((uint)e >> 16); g += d; e += f;
                e ^= f << 10; h += e; f += g;
                f ^= (int)((uint)g >> 4); a += f; g += h;
                g ^= h << 8; b += g; h += a;
                h ^= (int)((uint)a >> 9); c += h; a += b;
                mem[i] = a; mem[i + 1] = b; mem[i + 2] = c; mem[i + 3] = d;
                mem[i + 4] = e; mem[i + 5] = f; mem[i + 6] = g; mem[i + 7] = h;
            }

            if (flag)
            {
                for (i = 0; i < SIZE; i += 8)
                {
                    a += mem[i]; b += mem[i + 1]; c += mem[i + 2]; d += mem[i + 3];
                    e += mem[i + 4]; f += mem[i + 5]; g += mem[i + 6]; h += mem[i + 7];
                    a ^= b << 11; d += a; b += c;
                    b ^= (int)((uint)c >> 2); e += b; c += d;
                    c ^= d << 8; f += c; d += e;
                    d ^= (int)((uint)e >> 16); g += d; e += f;
                    e ^= f << 10; h += e; f += g;
                    f ^= (int)((uint)g >> 4); a += f; g += h;
                    g ^= h << 8; b += g; h += a;
                    h ^= (int)((uint)a >> 9); c += h; a += b;
                    mem[i] = a; mem[i + 1] = b; mem[i + 2] = c; mem[i + 3] = d;
                    mem[i + 4] = e; mem[i + 5] = f; mem[i + 6] = g; mem[i + 7] = h;
                }
            }

            Isaac();
            count = SIZE;
        }

        public int val()
        {
            if (0 == count--)
            {
                Isaac();
                count = SIZE - 1;
            }
            return rsl[count];
        }
    }
}
