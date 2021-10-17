using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Cifrado
{
    public class Sdes : ISdes
    {
        private string[] Permutaciones;
        private string llave, P10, k1, k2, IP, EP, P4;
        private string[,] sb0 = { { "01", "00", "11", "10" }, { "11", "10", "01", "00" }, { "00", "10", "01", "11" }, { "11", "01", "11", "10" } };
        private string[,] sb1 = { { "00", "01", "10", "11" }, { "10", "00", "01", "11" }, { "11", "00", "01", "00" }, { "10", "01", "00", "11" } };

        public Sdes()
        {
            var Direccion = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Direccion + @"\Permutations.txt";
            Permutaciones = File.ReadAllLines(path);
        }

        private string Permutar(string clave, int numPermutacion)
        {
            char[] array = clave.ToCharArray();
            string res = "";
            string[] aux = Permutaciones[numPermutacion].Split(',');
            for (int i = 0; i < aux.Length; i++)
            {
                int pos = Convert.ToInt32(aux[i]);
                res += array[pos-1];
            }
            return res;
        }

        private string CorrerIzq(string clave)
        {
            string res = "";
            char aux = ' ';
            for (int i = 0; i < clave.Length; i++)
            {
                if (i == 0) {
                    aux = clave[0];
                }                    
                else {
                    res += clave[i];
                }
            }
            return res + aux;
        }

        private string xor(string a, string b)
        {
            string res = "";
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == b[i])
                {
                    res += "0";
                }
                else
                {
                    res += "1";
                }
            }
            return res;
        }

        private void GenerarClaves(string llave)
        {
            P10 = Permutar(llave, 0);
            string LS1 = P10.Substring(0, 5);
            LS1 = CorrerIzq(LS1);
            string LS2 = P10.Substring(5, 5);
            LS2 = CorrerIzq(LS2);
            k1 = Permutar(LS1+LS2, 1);
            string LS3 = CorrerIzq(CorrerIzq(LS1));
            string LS4 = CorrerIzq(CorrerIzq(LS2));
            k2 = Permutar(LS3 + LS4, 1);
        }

        public byte [] Cifrar(byte[] texto, int dllave)
        {
            string bits, auxIP1, auxIP2, comb, s0, s1, f1, f2, c1, c2, swap;
            llave = DecimalBinario(dllave, 10);
            GenerarClaves(llave); //k1, k2

            for (int i = 0; i < texto.Length; i++)
            {
                bits = DecimalBinario(texto[i], 8);
                IP = Permutar(bits, 4);
                auxIP1 = IP.Substring(0, 4); // se utiliza en XOR
                auxIP2 = IP.Substring(4, 4); // se utiliza nuevamente al hacer swap
                EP = Permutar(auxIP2, 3);
                //XOR EP y K1
                comb = xor(EP, k1);
                char[] aux = comb.ToCharArray();
                f1 = aux[0].ToString() + aux[3].ToString();
                c1 = aux[1].ToString() + aux[2].ToString();
                f2 = aux[4].ToString() + aux[7].ToString();
                c2 = aux[5].ToString() + aux[6].ToString();
                s0 = sb0[BinarioDecimal(Convert.ToInt32(f1)), BinarioDecimal(Convert.ToInt32(c1))];
                s1 = sb1[BinarioDecimal(Convert.ToInt32(f2)), BinarioDecimal(Convert.ToInt32(c2))];
                P4 = Permutar(s0 + s1, 2);
                //XOR P4 y auxIP1
                comb = xor(P4, auxIP1);
                //swap
                swap = auxIP2 + comb;
            }


            throw new NotImplementedException();
        }

        public byte [] Descifrar(byte[] texto, int dllave)
        {
            throw new NotImplementedException();
        }


        //Binario → Decimal
        int BinarioDecimal(long binario)
        {

            int numero = 0;
            int digito = 0;
            const int DIVISOR = 10;

            for (long i = binario, j = 0; i > 0; i /= DIVISOR, j++)
            {
                digito = (int)i % DIVISOR;
                if (digito != 1 && digito != 0)
                {
                    return -1;
                }
                numero += digito * (int)Math.Pow(2, j);
            }

            return numero;
        }

        //Decimal → Binario
        string DecimalBinario(int numero, int longitud)
        {
            long binario = 0;

            const int DIVISOR = 2;
            long digito = 0;

            for (int i = numero % DIVISOR, j = 0; numero > 0; numero /= DIVISOR, i = numero % DIVISOR, j++)
            {
                digito = i % DIVISOR;
                binario += digito * (long)Math.Pow(10, j);
            }
            string Binario = binario.ToString();

            if (Binario.Length < longitud) //autorelleno de los 10 bits
            {
                int ceros = longitud - Binario.Length;
                for (int i = 0; i < ceros; i++)
                {
                    Binario = "0" + Binario;
                }
            }

            binario = Convert.ToInt64(Binario);
            return Binario;
        }
    }
}
