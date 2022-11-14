using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class GalleryModel
    {
        public int ID { get; set; }
        public int F_SubCategroy_Id { get; set; }
        public string ImgPath { get; set; }
        public string Title { get; set; }
        public string CreateDateUtcJalali { get; set; }
    }
}