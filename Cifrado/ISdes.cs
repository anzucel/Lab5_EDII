using System;
using System.Collections.Generic;
using System.Text;

namespace Cifrado
{
    public interface ISdes
    {
        public byte[] Cifrar(byte[] texto, int llave);
        public byte[] Descifrar(byte[] texto, int llave);
    }
}
