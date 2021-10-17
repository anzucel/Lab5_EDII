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
            string txt = "Ó";
            //char ch = 'Ó';
            //byte[] charByte = BitConverter.GetBytes(ch);
            byte[] bytes = { 211 };//Encoding.ASCII.GetBytes(txt);
            sdes.Cifrar(bytes, 364);
        }
    }
}
