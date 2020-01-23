using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TreinamentoAspNet02.Models;

namespace TreinamentoAspNet02.Helpers
{
    public static class FileHelper
    {
        //Return the name of file to save in database
        public static string Save(HttpPostedFileBase file, string nameFolder)
        {
            string nomeFile = null;
            if (file != null)
            {
                nomeFile = Guid.NewGuid().ToString() + System.IO.Path.GetFileName(file.FileName).Replace(" ", "-");

                string folder = System.Web.Hosting.HostingEnvironment.MapPath(nameFolder);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string path = System.IO.Path.Combine(folder, nomeFile);            
                file.SaveAs(path);

            }
            return nomeFile;
        }

        public static string Update(string fotoAntiga, HttpPostedFileBase fotoNova)
        {
            string nomeFoto = null;
            if (fotoNova != null)
            {
                // Excluir img se houver
                Delete(fotoAntiga);
                // Salva a foto nova
                nomeFoto = Save(fotoNova, "~/Images/Perfil");
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