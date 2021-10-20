
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
        public IActionResult PostFileCompress([FromForm] IFormFile file, [FromRoute] string name)
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
        public IActionResult DecipherPostFileCompress([FromForm] IFormFile file, [FromRoute] string name)
        {

            //if (Tipos.Key == null || !(int.TryParse(Tipos.Key, out int Key)))
            //{
            //    return BadRequest(new string[] { "El valor -Key- es inválido" });
            //}
            //else
            //{ }

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



        // PUT api/<sdesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<sdesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
