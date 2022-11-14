using BigMill.Areas.Admin3mill.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace BigMill.Models
{
    public class FileManagement
    {
        public FileManagement()
        {
            Path = Tools.ReturnPathPhysicalMode("FilesPath");
        }
        string Path { get; set; }

        public void EditFile(int ID, HttpPostedFileBase MyFile = null)
        {
            List<FileModel> list = new List<FileModel>();
            list.AddRange(LoadFiles());
            var FoundedObejct = list.FirstOrDefault(u => u.ID == ID);
            if (FoundedObejct != null)
            {
                if (File.Exists(Path + FoundedObejct.File))
                    File.Delete(Path + FoundedObejct.File);
                if (MyFile != null)
                    FoundedObejct.File = Tools.SaveFile(MyFile, Path);
                SaveChangesFiles(list);
            }
            else
                AddFile(ID, MyFile);
        }

        public void AddFile(int ID, HttpPostedFileBase File = null)
        {
            List<FileModel> list = new List<FileModel>();
            list.AddRange(LoadFiles());
            var FoundedObejct = list.FirstOrDefault(u => u.ID == ID);
            if (File != null)
            {
                if (FoundedObejct != null)
                    FoundedObejct.File = Tools.SaveFile(File, Path);
                else
                {
                    var f = new FileModel();
                    f.File = Tools.SaveFile(File, Path);
                    f.ID = ID;
                    list.Add(f);
                }
            }
            SaveChangesFiles(list);
        }
        public List<FileModel> LoadFiles()
        {
            List<FileModel> OBj = new List<FileModel>();
            var serializer = new XmlSerializer(typeof(List<FileModel>));
            if (System.IO.File.Exists(Path + "/Files.xml"))
            {
                using (var reader = XmlReader.Create(Path + "/Files.xml"))
                {
                    OBj = (List<FileModel>)serializer.Deserialize(reader);
                    return OBj;
                }
            }
            else
                return OBj;
        }

        private void SaveChangesFiles(List<FileModel> model)
        {
            System.IO.Directory.CreateDirectory(Path);
            var serializer = new XmlSerializer(model.GetType());
            using (var writer = XmlWriter.Create(Path + "/Files.xml"))
            {
                serializer.Serialize(writer, model);
            }
        }


    }
    public class FileModel
    {
        public int ID { get; set; }
        public string File { get; set; }
    }
}