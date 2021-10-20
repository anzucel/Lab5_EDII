using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections;
using Cifrado;
using ProyectoAPI.Extra;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProyectoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class sdesController : ControllerBase
    {
        // GET: api/<sdesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // GET api/<sdesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }


        // POST api/<sdesController>
        [HttpPost]
        [Route("cipher/{nombre}")]
        public IActionResult PostFileCompress([FromForm] IFormFile file, [FromRoute] string nombre, [FromForm] IFormFile key)
        {
            byte[] buffer = new byte[0];
            byte[] bufferfinal = new byte[0];
            var mahByteArray = new List<byte[]>();
            int llave = Convert.ToInt32(key);
            String name = nombre;
            ISdes cifrar = new Cifrado.Sdes();

            string nombrearchiv = file.FileName;
            string nombrearchivofinal = nombrearchiv.Split(".").First();

            Singleton.Instance.name_cipher.Add(nombre);
            Singleton.Instance.name_Original.Add(nombrearchivofinal);


            using (MemoryStream archivotexto = new MemoryStream())

                try
                {
                    var bytesarray = archivotexto.ToArray();
                    file.CopyToAsync(archivotexto);

                    using var leer = new BinaryReader(archivotexto);
                    archivotexto.Position = 0;

                    while (archivotexto.Position < archivotexto.Length)
                    {
                        buffer = leer.ReadBytes(100000);
                        bufferfinal = cifrar.Cifrar(buffer, llave);
                        mahByteArray.Add(bufferfinal);
                    }

                    generarArchivo_txt(mahByteArray, name);

                    return Ok();
                }
                catch (Exception)
                {
                    return StatusCode(500);
                }
        }



        // POST api/<sdesController>
        [HttpPost]
        [Route("decipher")]
        public IActionResult DecipherPostFileCompress([FromForm] IFormFile file, [FromForm] IFormFile key)
        {
            byte[] buffer = new byte[0];
            byte[] bufferfinal = new byte[0];
            var mahByteArray = new List<byte[]>();
            int llave = Convert.ToInt32(key);
            ISdes Descifrar = new Cifrado.Sdes();
            string nameOriginal = "";
            string nombrearchiv = file.FileName;
            string nombrearchivofinal = nombrearchiv.Split(".").First();



            for (int i=0; i< Singleton.Instance.name_cipher.Count;i++)
            {
               
                if (nombrearchivofinal==Singleton.Instance.name_cipher[i])
                {
                    nameOriginal = Singleton.Instance.name_Original[i];
                }
            }
             

            using (MemoryStream archivotexto = new MemoryStream())

                try
                {

                    var bytesarray = archivotexto.ToArray();
                    file.CopyToAsync(archivotexto);

                    using var leer = new BinaryReader(archivotexto);
                    archivotexto.Position = 0;


                    while (archivotexto.Position < archivotexto.Length)
                    {
                        buffer = leer.ReadBytes(100000);                     
                        bufferfinal = Descifrar.Cifrar(buffer, llave);
                        mahByteArray.Add(bufferfinal);

                    }
                  
                    generarArchivo_txt(mahByteArray, nameOriginal);

                    return Ok();
                }
                catch (Exception)
                {
                    return StatusCode(500);
                }
        }

        public void generarArchivo_txt(List<byte[]> datos, string name)
        {
             string fileName ="../Archivos/"+ name + ".txt";


            List<byte[]> dataArray = datos;

            using (FileStream
                fileStream = new FileStream(fileName, FileMode.Create))
            {
                //escribe byte por byte
                for(int j=0; j<dataArray.Count;j++)
                {
                    byte[] imprimir = dataArray[j];
                    for (int i = 0; i < imprimir.Length; i++)
                    {
                        fileStream.WriteByte(imprimir[i]);
                    }
                }
                

                // Set the stream position to the beginning of the file.
                fileStream.Seek(0, SeekOrigin.Begin);

            }

        }

    }
}

 

