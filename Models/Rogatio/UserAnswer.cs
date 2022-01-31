using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Sophia.Models.DataBase;

namespace Sophia.Models.Rogatio
{
    public class UserAnswer
    {        
        //[Key]
        public string UserAnswerId { get; set; }             
        
        //[Key]
        //[Column(Order =1)]
        public string AnswerId { get; set; }
        //[Key]
        //[Column(Order = 2)]
        public string SophiaUserId {get; set; }
        //[ForeignKey("AspNetUser")]
        public SophiaUser SophiaUser { get; set; }
        //[ForeignKey("AnswerId")]
        public Answer Answer { get; set; }

        public ICollection<SurveyQuestionUserAnswer> QuestionnaireQuestionUserAnswer { get; set; }

    }
}
