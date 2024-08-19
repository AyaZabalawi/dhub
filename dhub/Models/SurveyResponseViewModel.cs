using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dhub.Models
{
    public class SurveyResponseViewModel
    {
        [Required]
        public Guid SurveyID { get; set; }

        public List<QuestionResponse> Responses { get; set; } = new List<QuestionResponse>();

        public class QuestionResponse
        {
            public int QuestionID { get; set; }
            public string Response { get; set; }
        }
    }

}
