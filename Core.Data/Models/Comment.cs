using System;
using Core.Data.Types;

namespace Core.Data.Models
{
    public class Comment
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual User CreatedBy { get; set; }
        public long Post { get; set; }
        public State State { get; set; }
    }
}