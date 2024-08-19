using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace dhub.Models
{
    public class Question
    {
        [Key]
        public Guid QuestionID { get; set; }

        [ForeignKey("Survey")]
        public Guid SurveyID { get; set; }

        [Required]
        public string QuestionText { get; set; }

        [Required]
        public bool IsRequired { get; set; }

        [Required]
        public bool IsHidden { get; set; }

        [Required]
        public QuestionType QuestionType { get; set; }

        public string[] Options { get; set; } 

        public Survey Survey { get; set; }
    }
}
