using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dhub.Models
{
    public class SurveyResponseViewModel
    {
        [Required]
        public Guid SurveyID { get; set; }

        public List<QuestionResponse> Responses { get; set; } = new List<QuestionResponse>();

        public string SubmittedBy { get; set; }

        public class QuestionResponse
        {
            [ForeignKey("Question")]
            public Guid QuestionID { get; set; }

            public string QuestionText { get; set; }

            public List<string> Response { get; set; }

            public QuestionType QuestionType { get; set; }
            public List<string> Options { get; set; } = new List<string>();
        }
    }
}
