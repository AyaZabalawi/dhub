using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QuestionType QuestionType { get; set; }

        public List <string> Options { get; set; }  = new List<string>();

        [JsonIgnore]
        public virtual Survey? Survey { get; set; }
    }
}
