using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sophia.Models.Rogatio;
using Sophia.Models.DataBase;


namespace Sophia.Models.ViewModels
{
    public class TakeSurveyViewModel
    {
        public bool Finished { get; set; } = false;
        public int CurrentNo { get; set; }
        public int QuestionsNo { get; set; }
        public List<QuestionAnswer> QuestionAnswers { get; set; }
        public QuestionAnswer CurrentQuestionAnswer { get; set; }        
    }
    public class QuestionAnswer
    {
        public Question Question { get; set; }
        public Answer Answer { get; set; }
    }
}
