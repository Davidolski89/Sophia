using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sophia.Models.Rogatio
{ 
    // zurzeit nicht verwendet
    public class SurveyQuestion
    {        
        [Key]
        public string SurveyQuestionId { get; set; }
        //[Key]
        //[Column(Order = 1)]
        public string QuestionId { get; set; }
        //[Key]
        //[Column(Order = 2)]
        public string SurveyId { get; set; }
        //[ForeignKey("SurveyId")]
        public Survey Survey { get; set; }
        //[ForeignKey("QuestionId")]
        public Question Question { get; set; }
        public ICollection<SurveyQuestionUserAnswer> QuestionnaireQuestionUserAnswer { get; set; }
    }
}
