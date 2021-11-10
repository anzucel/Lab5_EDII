using System;
using System.Collections.Generic;
using System.Text;

namespace Cifrado
{
    public interface ISdes
    {
        public byte[] Cifrar(byte[] texto, int llave, int n=0);
        public byte[] Descifrar(byte[] texto, int llave, int n=0);
    }
}
