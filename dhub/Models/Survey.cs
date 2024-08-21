using dhub.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dhub.Models
{
    public class Survey
    {
        [Key]
        public Guid SurveyID { get; set; }


        [Required]
        public string Name { get; set; }

        public List<Question> Questions { get; set; } = new List<Question>();

        [ForeignKey("ApplicationUser")]
        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        
    }
}
