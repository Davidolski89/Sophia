using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Sophia.Models.DataBase;

namespace Sophia.Models.Rogatio
{
    // Entity Db
    public class Answer
    {        
        public int AnswerId { get; set; }
        public int QuestionId { get; set;}
        public Question Question { get; set; }
        public string SessionId { get; set; }        
        public string OpenEndedAnswer { get; set; }
        public bool? ClosedEndedAnswer { get; set; }
        public int? RatingAnswer { get; set; }
        public string MultipleChoiceAnswer { get; set; }
        public QuestionType QuestionType { get; set; }
    }
}
