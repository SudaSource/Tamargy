using System;
using Core.Data.Types;

namespace Core.Data.Models
{
    public class Post
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string MedicineNameAr { get; set; }
        public string MedicineNameEng { get; set; }
        public DateTime CreateDate { get; set; }
        public long  CreatedBy { get; set; }
        public State State { get; set; }
        public PostType PostType { get; set; }
    }
}