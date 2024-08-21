using System.ComponentModel.DataAnnotations;

namespace dhub.Models
{
    public class AnswerOption
    {
        [Key]
        public Guid AnswerOptionID { get; set; }

        [Required]
        public Guid QuestionResponseID { get; set; }
        public QuestionResponse QuestionResponse { get; set; }

        public string OptionText { get; set; }
    }
}
