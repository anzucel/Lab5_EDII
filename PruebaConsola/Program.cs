using System;
using Cifrado;
using System.Text;
using System.Text.Encodings;

namespace PruebaConsola
{
    class Program
    {
        static void Main(string[] args)
        {
            ISdes sdes = new Cifrado.Sdes();
            string txt = "ÓÓ";
            //char ch = 'Ó';
            //byte[] charByte = BitConverter.GetBytes(ch);

           // byte[] texto = Convert.ToByte( txt.ToCharArray);
            byte[] bytes = { 211, 211 };//Encoding.ASCII.GetBytes(txt);
            byte[] cifrado= sdes.Cifrar(bytes, 364);
            byte[] descifrado = sdes.Descifrar(cifrado, 364);
        }
    }
}
