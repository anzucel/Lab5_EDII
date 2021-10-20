using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyectoAPI;
using ProyectoAPI.Extra;

namespace ProyectoAPI.Extra
{
    public sealed class Singleton
    {
        private readonly static Singleton instance = new Singleton();
        
        public List<string> name_Original = new List<string>();
        public List<string> name_cipher = new List<string>();

        private Singleton()
        {
           
        }
      
        public static Singleton Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
