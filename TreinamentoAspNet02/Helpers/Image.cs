using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TreinamentoAspNet02.Models;

namespace TreinamentoAspNet02.Helpers
{
    public static class Image
    {
        //Return the name of file to save in database
        public static string Save(HttpPostedFileBase foto)
        {
            string nomeFoto = null;
            if (foto != null)
            {
                nomeFoto = Guid.NewGuid().ToString() + System.IO.Path.GetFileName(foto.FileName).Replace(" ", "-");
                string path = System.IO.Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Images/Perfil"), nomeFoto);            
                foto.SaveAs(path);

            }
            return nomeFoto;
        }

        public static string Update(string fotoAntiga, HttpPostedFileBase fotoNova)
        {
            string nomeFoto = null;
            if (fotoNova != null)
            {
                // Excluir img se houver
                Delete(fotoAntiga);
                // Salva a foto nova
                nomeFoto = Save(fotoNova);
            }
            else
            {
                nomeFoto = fotoAntiga;
            }
            return nomeFoto;
        }

        public static void Delete(string nomeFoto)
        {
            FileInfo fotoPerfil = new FileInfo(System.Web.Hosting.HostingEnvironment.MapPath("~/Images/Perfil/") + nomeFoto);
            if (fotoPerfil.Exists)
            {
                fotoPerfil.Delete();
            }
        }
    }
}