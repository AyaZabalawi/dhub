using dhub.Users;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dhub.Models
{
    public class Survey
    {
        [Key]
        public Guid SurveyID { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Question> Questions { get; set; } = new List<Question>();

        [ForeignKey("IdentityUser")]
        public Guid OwnerId { get; set; }
        
        public string FullName { get; set; } = string.Empty;
        public virtual ApplicationUser Owner { get; set; }
        
    }
}
