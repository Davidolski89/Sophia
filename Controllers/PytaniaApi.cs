using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sophia.Models.Rogatio;
using Sophia.Models.DataBase;
using Sophia.Data;
using Microsoft.EntityFrameworkCore;


namespace Sophia.Controllers
{
    [ApiController]
    [Route("/Sophia/{controller}/{action}")]
    public class PytaniaApi : ControllerBase
    {
        SophiaDB _sophiaDB;
        public PytaniaApi(SophiaDB db)
        {
            _sophiaDB = db;
        }

        [HttpPost]
        public IActionResult RearrangeSurvey(Range range)
        {
            try
            {
                Survey survey = _sophiaDB.Surveys.Include(c => c.Questions).Where(x => x.SurveyId == range.sid).FirstOrDefault();
                List<Question> originalCloneOrder = survey.Questions.CloneQuestions();

                for (int i = 0; i < range.qids.Count; i++)
                {
                    var QuestionToChange = survey.Questions.ElementAt(i);
                    var question = originalCloneOrder.Where(c => c.QuestionId == range.qids[i]).FirstOrDefault();
                    QuestionToChange.QuestionText = question.QuestionText;
                    QuestionToChange.OpenEndedAnswer = question.OpenEndedAnswer;
                    //QuestionToChange.ClosedEndedAnswer = question.ClosedEndedAnswer;
                    QuestionToChange.ClosedEndedAnswerDesign = question.ClosedEndedAnswerDesign;
                    QuestionToChange.RatingDesign = question.RatingDesign;
                    QuestionToChange.RatingAnswer = question.RatingAnswer;
                    QuestionToChange.MultipleChoiceAnswer = question.MultipleChoiceAnswer;
                    QuestionToChange.MultipleChoiceAnswerDesign = question.MultipleChoiceAnswerDesign;
                    QuestionToChange.Type = question.Type;
                    QuestionToChange.Answers = question.Answers;
                }
                _sophiaDB.SaveChanges();
                //_sophiaDB.Surveys
                return new JsonResult("Changed successfully");
            }
            catch
            {
                return new JsonResult("An error accured");
            }
            
        }

        public class Range
        {
            public int sid { get; set;  }
            public List<int> qids { get; set; }
        }

        
    }
}
