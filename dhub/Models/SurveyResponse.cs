using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using dhub.Models;

namespace dhub.Models
{

    public class SurveyResponse
    {
        [Key]
        public Guid ResponseID { get; set; }

        [Required]
        public Guid SurveyID { get; set; }
        public Survey Survey { get; set; }

        public DateTime SubmissionDate { get; set; }

        
        public string ResponsesJson { get; set; }
    }

}

