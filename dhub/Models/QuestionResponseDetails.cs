using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dhub.Models
{
    public class QuestionResponseDetails
    {
        [Key]
        public Guid QuestionResponseDetailId { get; set; }

        [ForeignKey("QuestionResponse")]
        public Guid QuestionResponseID { get; set; }
        public virtual QuestionResponse QuestionResponse { get; set; }

        [ForeignKey("Question")]
        public Guid QuestionID { get; set; }
        public virtual Question Question { get; set; }
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }
    }
}
