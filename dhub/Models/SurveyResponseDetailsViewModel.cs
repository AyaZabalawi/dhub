using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dhub.Models
{
    public class SurveyResponseDetailsViewModel
    {
        [Key]
        public Guid SurveyResponseDetailsViewModelId { get; set; }

        [ForeignKey("QuestionResponse")]
        public Guid ResponseID { get; set; }

        [ForeignKey("Survey")]
        public Guid SurveyID { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string SurveyName { get; set; }
        public string SubmittedBy { get; set; }
        public List<QuestionResponseDetails> QuestionResponses { get; set; } = new List<QuestionResponseDetails>();
    }
}

