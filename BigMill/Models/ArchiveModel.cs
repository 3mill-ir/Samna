using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class Archive
    {
        public Archive()
        {
            Archives = new List<ArchiveModel>();
        }
        public List<ArchiveModel> Archives { get; set; }
        public int PostSubCategoryId { get; set; }
    }
    public class ArchiveModel
    {
        public string ShowingDate { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Year { get; set; }
    }
}