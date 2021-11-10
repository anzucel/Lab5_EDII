using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Cifrado;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProyectoAPI.Controllers
{
    [Route("api/rsa")]
    [ApiController]
    public class RSAController : ControllerBase
    {
        [Route("keys/{p}/{q}")]
        [HttpGet]
        public IActionResult obtenerLlaves([FromRoute] int p, [FromRoute] int q)
        {
            try
            {
                CifradoRSA cifrado = new CifradoRSA();
                List<string> keys = cifrado.generadorLlaves(p, q);

                var archivoComprimidoStream = new MemoryStream();
                using (var archivoZip = new ZipArchive(archivoComprimidoStream, ZipArchiveMode.Create,
                    leaveOpen: true))
                {
                    var publicEntry = archivoZip.CreateEntry("public.key");
                    using (var originalFileStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(keys[0])))
                    using (var zipPublicEntryStream = publicEntry.Open())
                    {
                        originalFileStream.CopyTo(zipPublicEntryStream);
                    }
                    var privateEntry = archivoZip.CreateEntry("private.key");
                    using (var originalFileStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(keys[1])))
                    using (var zipPrivateEntryStream = privateEntry.Open())
                    {
                        originalFileStream.CopyTo(zipPrivateEntryStream);
                    }
                }
                archivoComprimidoStream.Position = 0;
                FileContentResult ArchivoZip = File(archivoComprimidoStream.ToArray(), "application/zip");
                ArchivoZip.FileDownloadName = "keys.zip";
                return ArchivoZip;
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [Route("{nombre}")]
        [HttpPost]
        public IActionResult normalGet([FromForm] List<IFormFile> files, [FromRoute] string nombre)
        {
            try
            {
                IFormFile informacion = files[0];
                IFormFile key = files[1];
                byte[] informacionBytes = null;
                using (var ms = new MemoryStream())
                {
                    informacion.CopyTo(ms);
                    informacionBytes = ms.ToArray();
                }

                byte[] keyBytes = null;
                using (var ms = new MemoryStream())
                {
                    key.CopyTo(ms);
                    keyBytes = ms.ToArray();
                }

                CifradoRSA cifrado = new CifradoRSA();
                string contenidoKey = Encoding.ASCII.GetString(keyBytes, 0, keyBytes.Length);
                byte[] informacionProcesada;
                System.Diagnostics.Debug.WriteLine(informacionBytes[0] + " " + informacionBytes[1]);
                if (!(informacionBytes[0] == 49))
                {
                    System.Diagnostics.Debug.WriteLine("encrypt");
                    string es = contenidoKey.Substring(0, contenidoKey.IndexOf(','));
                    string ns = contenidoKey.Substring(contenidoKey.IndexOf(',') + 1, contenidoKey.Length - contenidoKey.IndexOf(',') - 1);
                    informacionProcesada = cifrado.Cifrar(informacionBytes, Convert.ToInt32(es), Convert.ToInt32(ns));

                }
                else
                {
                    string informacionString = System.Text.Encoding.Default.GetString(informacionBytes);
                    //System.Diagnostics.Debug.WriteLine(informacionString);
                    string[] infoString = informacionString.Split('|');
                    System.Diagnostics.Debug.WriteLine(infoString.Length);
                    string ds = contenidoKey.Substring(0, contenidoKey.IndexOf(','));
                    string ns = contenidoKey.Substring(contenidoKey.IndexOf(',') + 1, contenidoKey.Length - contenidoKey.IndexOf(',') - 1);
                    informacionProcesada = cifrado.Descifrar(informacionBytes, Convert.ToInt32(ds), Convert.ToInt32(ns));
                }

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = nombre + ".txt",
                    Inline = true,
                };


                Response.Headers.Add("Content-Disposition", cd.ToString());
                return File(informacionProcesada, "application/text");

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return StatusCode(500);
            }
        }
    }
}