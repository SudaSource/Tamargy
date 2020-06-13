namespace Core.Data.Models
{
    public class Like
    {
        public long Id { get; set; }
        public long Post { get; set; }
        public long User { get; set; }
    }
}