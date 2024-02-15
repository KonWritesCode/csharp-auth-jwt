using exercise.wwwapi.DataModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace workshop.webapi.DataModels
{
    [Table("blogs")]
    public class Blog
    {
        [Column("id")]
        public int Id { get; set; }
        public string BlogTitle { get; set; }
        

        List<Post> Posts { get; set; }
    }
}
