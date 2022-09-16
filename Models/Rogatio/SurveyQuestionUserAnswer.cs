using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sophia.Models.Rogatio
{
    // Entity Db, zurzeit nicht verwendet
    public class SurveyQuestionUserAnswer
    {
        [Key]
        public string SurveyQeustionUserAnswerId { get; set; }
        public string SurveyQuestionId { get; set; }       
        public string UserAnswerId { get; set; }
        //[ForeignKey("SurveyQuestionId")]
        public SurveyQuestion SurveyQuestion { get; set; }
        //[ForeignKey("UserAnswerId")]
        public UserAnswer UserAnswer { get; set; }
        
    }
}
