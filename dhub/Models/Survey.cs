using System.ComponentModel.DataAnnotations;

namespace dhub.Models
{
    public class Survey
    {
        [Key]
        public Guid SurveyID { get; set; }


        [Required]
        public string Name { get; set; }

        public ICollection<Question> Questions { get; set; }

        
    }
}
