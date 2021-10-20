
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
        public IActionResult PostFileCompress([FromForm] IFormFile file, [FromRoute] string name, [FromForm] IFormFile key)
        {
            byte[] buffer = new byte[0];

            using (MemoryStream archivotexto = new MemoryStream())

                try
                {
                    var bytesarray = archivotexto.ToArray();
                    file.CopyToAsync(archivotexto);

                    using var leer = new BinaryReader(archivotexto);
                    archivotexto.Position = 0;

                    while (archivotexto.Position < archivotexto.Length)
                    {
                        buffer = leer.ReadBytes(1);
                    }
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
            byte[] buffer = new byte[3];
            using (MemoryStream archivotexto = new MemoryStream())

                try
                {

                    var bytesarray = archivotexto.ToArray();
                    file.CopyToAsync(archivotexto);

                    using var leer = new BinaryReader(archivotexto);
                    archivotexto.Position = 0;


                    while (archivotexto.Position < archivotexto.Length)
                    {
                        buffer = leer.ReadBytes(3);
                    }
                    generarArchivo_txt(buffer, "sifunciona");

                    return Ok();
                }
                catch (Exception)
                {
                    return StatusCode(500);
                }
        }

        public void generarArchivo_txt(byte[] datos, string name)
        {
             string fileName = name + ".txt";


            byte[] dataArray = datos;

            using (FileStream
                fileStream = new FileStream(fileName, FileMode.Create))
            {
                //escribe byte por byte
                for (int i = 0; i < dataArray.Length; i++)
                {
                    fileStream.WriteByte(dataArray[i]);
                }

                // Set the stream position to the beginning of the file.
                fileStream.Seek(0, SeekOrigin.Begin);

            }

        }
    }
}

 

