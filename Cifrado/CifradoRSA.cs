using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Cifrado
{
    public class CifradoRSA : ISdes
    {
        //nuevos
        public List<string> generadorLlaves()
        {
            int MAX_RANGE = 100000;
            int p = 0;
            int q = 0;
            uint n = 0;
            uint sn;
            double d = 0;

            bool sonPrimos = false;
            Random generadorRandom = new Random();
            while (!sonPrimos)
            {
                p = generadorRandom.Next(6, MAX_RANGE);

                q = generadorRandom.Next(6, MAX_RANGE);
                if (p != q)
                {
                    if (esPrimo(p) && esPrimo(q)) sonPrimos = true;
                }
            }
            n = (uint)(p * q);
            sn = (uint)((p - 1) * (q - 1));

            bool esMCD = false;
            uint e = 0;
            while (!esMCD || d == -1)
            {
                esMCD = false;
                e = (uint)generadorRandom.Next(2, (int)sn - 1);
                if (mcd(sn, e) == 1) esMCD = true;
                d = inversa(sn, e);
            }
            Console.WriteLine(p + " : " + q + " : " + e + " : " + d + "  " + sn);
            string publicKey = e + "," + n;
            string privateKey = d + "," + n;

            List<string> keys = new List<string>();
            keys.Add(publicKey);
            keys.Add(privateKey);

            return keys;
        }

        public List<string> generadorLlaves(int p, int q)
        {
           
            uint n = 0;
            uint sn;
            if (p == q) throw new Exception("P y Q sin iguales");
            if (!(esPrimo(p) && esPrimo(q))) throw new Exception("P o Q no es primo");
            n = (uint)(p * q);
            if (n < 256) throw new Exception("P * Q menor a 256");
            sn = (uint)((p - 1) * (q - 1));

            bool esMCD = false;
            uint e = 0;
            long d = 0;

            while (!esMCD || d == -1)
            {
                Random generadorRandom = new Random();
                e = (uint)generadorRandom.Next(2, ((int)sn) - 1);
                esMCD = false;
                if (mcd(sn, e) == 1) esMCD = true;
                d = inversa(sn, e);
                if (d == e) d += sn;
            }
            //System.Diagnostics.Debug.WriteLine("AAA: " + BigInteger.Multiply(d,e) % sn);
            string publicKey = e + "," + n;
            string privateKey = d + "," + n;

            List<string> keys = new List<string>();
            keys.Add(publicKey);
            keys.Add(privateKey);

            return keys;
        }

        private long inversa(uint sn, uint e)
        {
            long d = -1;
            for (uint i = 1; i <= sn; i++)
            {

                long x = 1 + (long)BigInteger.Multiply(i, sn);

                if (x % e == 0)
                {
                    d = x / e;
                    i = sn + 1;
                }
            }
            return d;
        }

        private Boolean esPrimo(int numero)
        {
            int i;
            for (i = 2; i <= Math.Sqrt(numero); i++)
            {
                if (numero % i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        private uint mcd(uint mayor, uint menor)
        {
            uint resultado = 0;
            do
            {
                resultado = menor;
                menor = mayor % menor;
                mayor = resultado;
            }
            while
            (menor != 0);
            return resultado;
        }

        public byte[] Cifrar(byte[] data, int e, int n)
        {
            string dataEncryted = "";
            dataEncryted += "15|45|65|";
            foreach (byte by in data)
            {
                //C = M ^ e mod n
                int xx = (int)BigInteger.ModPow(by, e, n);
                dataEncryted += xx + "|";
            }
            return Encoding.ASCII.GetBytes(dataEncryted);
        }

        public byte[] Descifrar(byte[] data, int d, int n)
        {
            //M = C ^ d mod n
            byte[] dataE = new byte[(data.Length - 8) / 8];
            int i = 0;
            for (int j = 8; j < data.Length; j += 8)
            {
                //if (j > 799990) System.Diagnostics.Debug.WriteLine(j);
                List<byte> bytes = new List<byte>();
                for (int k = 0; k < 8; k++)
                {
                    bytes.Add(data[j + k]);
                }
                BigInteger byteParaDes = new BigInteger(bytes.ToArray());
                int xx = (int)BigInteger.ModPow(byteParaDes, d, n);
                dataE[i] = (byte)xx;
                i++;

            }
            return dataE;
        }

        public byte[] Descifrar(byte[] texto, int llave)
        {
            {
                throw new NotImplementedException();
            }
        }

        public byte[] Cifrar(byte[] texto, int llave)
        {
            {
                throw new NotImplementedException();
            }
        }
    }

}

