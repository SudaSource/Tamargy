using System;

namespace Core.Data.Models
{
    public class ErrorLog
    {
        public long Id { get; set; }
        public string Mesg { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public DateTime CreateDate { get; set; }
    }
}