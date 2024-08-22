using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static dhub.Models.SurveyResponseViewModel;

namespace dhub.Models
{
    public class SurveyResponse
    {
        [Key]
        public Guid ResponseID { get; set; }

        [Required]
        public Guid SurveyID { get; set; }
        public virtual Survey Survey { get; set; }

        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;

        public string SubmittedBy { get; set; }

        public ICollection<QuestionResponse> QuestionResponses { get; set; } = new List<QuestionResponse>();
    }
}

