using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using workshop.webapi.DataModels;

namespace exercise.wwwapi.DataModels
{
    [Table("posts")]
    public class Post
    {
        [Key]
        public int id;
        [ForeignKey(nameof(Blog))]
        public int fk_blog;

        Blog Blog { get; set; }
    }
}
