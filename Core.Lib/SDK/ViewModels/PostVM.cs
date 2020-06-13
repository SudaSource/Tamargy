using System;
using System.Collections.Generic;
using Core.Data.Models;
using Core.Data.Types;

namespace Core.Lib.SDK.ViewModels
{
    public class PostVM
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string MedicineNameAr { get; set; }
        public string MedicineNameEng { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual User CreatedBy { get; set; }
        public State State { get; set; }
        public List<Comment> Comments { get; set; }
        public int Likes { get; set; }
        public List<Like> PostLikes { get; set; }
        public bool AllowComment { get; set; }
        public PostType PostType { get; set; }
    }

    public class PostPage
    {

    }
}