using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sophia.Models.DataBase;
using Sophia.Models.Rogatio;
using Sophia.Models.ViewModels;
using Sophia.Data;
using Microsoft.EntityFrameworkCore;

namespace Sophia.Controllers
{
    public class SurveyController : Controller
    {
        readonly SophiaDB _sophiaDb;
        public SurveyController(SophiaDB db)
        {
            _sophiaDb = db;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(new UserSurveyOverview() { Surveys = _sophiaDb.Surveys.Include(x => x.Questions).Where(survey => survey.Active == true).ToList() });
        }


        [HttpGet]
        public IActionResult TakeSurvey(int id)
        {
            TakeSurveyViewModel survey = HttpContext.Session.GetObject<TakeSurveyViewModel>("CurrentTake");
            if (survey != null)
            {
                // Search last page
                return View("NextQuestion", survey);
            }
            else
            {
                TakeSurveyViewModel startingSurvey = new TakeSurveyViewModel() { QuestionAnswers = new List<QuestionAnswer>() };
                _sophiaDb.Questions.Where(x => x.SurveyId == id).ToList().ForEach(c => { startingSurvey.QuestionAnswers.Add(new QuestionAnswer { Question = c, Answer = new Answer() {QuestionType = c.Type } }); });
                startingSurvey.CurrentQuestionAnswer = startingSurvey.QuestionAnswers.ElementAt(0);
                startingSurvey.QuestionsNo = startingSurvey.QuestionAnswers.Count;
                HttpContext.Session.SetObject("CurrentTake", startingSurvey);
                return View("NextQuestion", startingSurvey);
            }
        }
        [HttpGet]
        public IActionResult NextQuestion()
        {
            return View("NextQuestion", HttpContext.Session.GetObject<TakeSurveyViewModel>("CurrentTake"));
        }
        [HttpPost]
        public IActionResult NextQuestion(TakeSurveyViewModel model)
        {
            // Save model result
            var currentSurvey = HttpContext.Session.GetObject<TakeSurveyViewModel>("CurrentTake");
            if (currentSurvey != null)
            {
                QuestionAnswer currentQuestionAnswer = currentSurvey.QuestionAnswers.Where(x => x.Question.QuestionId == model.CurrentQuestionAnswer.Question.QuestionId).FirstOrDefault();
                currentQuestionAnswer.Answer.QuestionType = model.CurrentQuestionAnswer.Answer.QuestionType;
                currentQuestionAnswer.Answer.RatingAnswer = model.CurrentQuestionAnswer.Answer.RatingAnswer;
                currentQuestionAnswer.Answer.OpenEndedAnswer = model.CurrentQuestionAnswer.Answer.OpenEndedAnswer;
                currentQuestionAnswer.Answer.ClosedEndedAnswer = model.CurrentQuestionAnswer.Answer.ClosedEndedAnswer;
                currentQuestionAnswer.Answer.MultipleChoiceAnswer = model.CurrentQuestionAnswer.Answer.MultipleChoiceAnswer;

                int currentIndex = currentSurvey.QuestionAnswers.FindIndex(x => x.Question.QuestionId == currentQuestionAnswer.Question.QuestionId);

                if (currentIndex + 1 < currentSurvey.QuestionAnswers.Count)
                {
                    currentSurvey.CurrentQuestionAnswer = currentSurvey.QuestionAnswers.ElementAt(currentIndex + 1);
                    currentSurvey.CurrentNo = currentIndex + 1;
                }
                else
                {
                    currentSurvey.Finished = true;
                }
                
                
                HttpContext.Session.SetObject("CurrentTake", currentSurvey);

                return Redirect("NextQuestion");
            }

            return View("Index");
        }
        [HttpPost]
        public IActionResult PreviousQuestion(TakeSurveyViewModel model)
        {
            // Save model result
            var currentSurvey = HttpContext.Session.GetObject<TakeSurveyViewModel>("CurrentTake");
            if (currentSurvey != null)
            {
                QuestionAnswer answer = currentSurvey.QuestionAnswers.Where(x => x.Question.QuestionId == model.CurrentQuestionAnswer.Question.QuestionId).FirstOrDefault();
                answer.Answer.QuestionType = model.CurrentQuestionAnswer.Answer.QuestionType;
                answer.Answer.RatingAnswer = model.CurrentQuestionAnswer.Answer.RatingAnswer;
                answer.Answer.OpenEndedAnswer = model.CurrentQuestionAnswer.Answer.OpenEndedAnswer;
                answer.Answer.ClosedEndedAnswer = model.CurrentQuestionAnswer.Answer.ClosedEndedAnswer;
                answer.Answer.MultipleChoiceAnswer = model.CurrentQuestionAnswer.Answer.MultipleChoiceAnswer;

                int currentIndex = currentSurvey.QuestionAnswers.FindIndex(x => x.Question.QuestionId == answer.Question.QuestionId);

                if (currentIndex - 1 > -1)
                {
                    currentSurvey.CurrentQuestionAnswer = currentSurvey.QuestionAnswers.ElementAt(currentIndex - 1);
                    currentSurvey.CurrentNo = currentIndex - 1;
                }
                    

                HttpContext.Session.SetObject("CurrentTake", currentSurvey);

                return Redirect("NextQuestion");
            }

            return View("Index");
        }
        public IActionResult JumpToQuestion(int no)
        {
            var currentSurvey = HttpContext.Session.GetObject<TakeSurveyViewModel>("CurrentTake");
            if (currentSurvey == null)
                return NotFound();
            QuestionAnswer questionAnswer = currentSurvey.QuestionAnswers.ElementAt(no-1);
            currentSurvey.CurrentNo = no-1;
            currentSurvey.CurrentQuestionAnswer = questionAnswer;
            HttpContext.Session.SetObject("CurrentTake", currentSurvey);
            return View("NextQuestion", currentSurvey);
        }
        [HttpGet]
        public IActionResult AbortTake()
        {
            HttpContext.Session.Clear();
            return View("Index", new UserSurveyOverview() { Surveys = _sophiaDb.Surveys.Include(q=>q.Questions).ToList() });
        }

        [HttpPost]
        public IActionResult SubmitSurvey(TakeSurveyViewModel model)
        {
            // Save model result
            var currentSurvey = HttpContext.Session.GetObject<TakeSurveyViewModel>("CurrentTake");
            if (currentSurvey != null)
            {
                QuestionAnswer answer = currentSurvey.QuestionAnswers.Where(x => x.Question.QuestionId == model.CurrentQuestionAnswer.Question.QuestionId).FirstOrDefault();
                answer.Answer.QuestionType = model.CurrentQuestionAnswer.Answer.QuestionType;
                answer.Answer.RatingAnswer = model.CurrentQuestionAnswer.Answer.RatingAnswer;
                answer.Answer.OpenEndedAnswer = model.CurrentQuestionAnswer.Answer.OpenEndedAnswer;
                answer.Answer.ClosedEndedAnswer = model.CurrentQuestionAnswer.Answer.ClosedEndedAnswer;
                answer.Answer.MultipleChoiceAnswer = model.CurrentQuestionAnswer.Answer.MultipleChoiceAnswer;

                int currentIndex = currentSurvey.QuestionAnswers.FindIndex(x => x.Question.QuestionId == answer.Question.QuestionId);             

                saveTakeSurvey(currentSurvey);

                return Redirect("Index");
            }
            
            return View("Index");
        }
        [HttpGet]
        public IActionResult SubmitSurvey()
        {
            var currentSurvey = HttpContext.Session.GetObject<TakeSurveyViewModel>("CurrentTake");
            saveTakeSurvey(currentSurvey);
            return Redirect("Index");
        }
        void saveTakeSurvey(TakeSurveyViewModel model)
        {
            foreach (var question in model.QuestionAnswers)
            {
                question.Answer.SessionId = HttpContext.Session.Id;
                var dbquestion = _sophiaDb.Questions.Include(y=>y.Answers).Where(c => c.QuestionId == question.Question.QuestionId).FirstOrDefault();
                dbquestion.Answers.Add(question.Answer);
            }
            _sophiaDb.SaveChanges();
            HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Session");
        }
    }
}
