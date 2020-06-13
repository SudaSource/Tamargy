using System;
using Core.Data.Types;

namespace Core.Data.Models
{
    public class Reply
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual Comment Comment{ get; set; }
        public State State { get; set; }
    }
}