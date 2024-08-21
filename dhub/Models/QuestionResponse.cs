using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dhub.Models
{
    public class QuestionResponse
    {
        [Key]
        public Guid QuestionResponseID { get; set; }

        [Required]
        public Guid SurveyResponseID { get; set; }
        public SurveyResponse SurveyResponse { get; set; }

        [Required]
        public Guid QuestionID { get; set; }
        public Question Question { get; set; }

        public string QuestionText { get; set; }

        public string AnswerText { get; set; }


        public ICollection<AnswerOption> SelectedOptions { get; set; } = new List<AnswerOption>();
    }

}
