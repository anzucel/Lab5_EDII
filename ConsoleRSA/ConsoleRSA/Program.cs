using System;
using System.Collections.Generic;
using System.IO;
using Cifrado;

namespace ConsoleRSA
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Escribe el path y el nombre del archivo a comprimir");
            string filePath = Console.ReadLine();

            byte[] data = File.ReadAllBytes(filePath);

            CifradoRSA cifradoRsa = new CifradoRSA();
            List<string> keys = cifradoRsa.generadorLlaves();

            Console.WriteLine("Public Key: " + keys[0]);
            Console.WriteLine("Private Key: " + keys[1]);

            string publicKey = keys[0];
            string es = publicKey.Substring(0, publicKey.IndexOf(','));
            string ns = publicKey.Substring(publicKey.IndexOf(',') + 1, publicKey.Length - publicKey.IndexOf(',') - 1);
            byte[] dataEncriptada = cifradoRsa.Cifrar(data, Convert.ToInt32(es), Convert.ToInt32(ns));
            string fileName = filePath.Substring(0, filePath.IndexOf('.'));
            createFileE(dataEncriptada, fileName);


            string privareKey = keys[1];
            string ds = privareKey.Substring(0, privareKey.IndexOf(','));

            //Console.WriteLine("e: " + es + "  d: " + ds + "  n: " + ns  );
            byte[] dataDesencriptada = cifradoRsa.Descifrar(dataEncriptada, Convert.ToInt32(ds), Convert.ToInt32(ns));
            createFileD(dataDesencriptada, fileName);

            Console.WriteLine("Proceso completado");
            Console.ReadKey();

        }

        static void createFileE(byte[] data, string pathName)
        {
            using (FileStream fs = File.Create(pathName + "-cifrado.txt"))
            {
                // Add some text to file    
                fs.Write(data, 0, data.Length);

            }
        }

        static void createFileD(byte[] data, string pathName)
        {
            using (FileStream fs = File.Create(pathName + "-descifrado.txt"))
            {
                // Add some text to file    
                fs.Write(data, 0, data.Length);

            }
        }

    }
}
