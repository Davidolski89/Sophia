using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sophia.Models.DataBase;
using Sophia.Models.Rogatio;
using Sophia.Data;
using Microsoft.EntityFrameworkCore;
using Sophia.Models.ViewModels;
using static Sophia.Data.StaticHelpers;

namespace Sophia.Controllers
{
    public class PytaniaController : Controller
    {
        readonly SophiaDB _sophiaDB;
        public PytaniaController(SophiaDB dB)
        {
            _sophiaDB = dB;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(_sophiaDB.Surveys.Include(x => x.Questions).ToList());
        }

        [HttpGet]
        public IActionResult CreateSurvey()
        {
            TempData["QuestionAction"] = "create";
            Survey survey = HttpContext.Session.GetObject<Survey>("CurrentSurvey");
            if (survey is null)
                return View("CreateSurvey", new SurveyCreationViewModel() { CurrentSurvey = new Survey() { Questions = new List<Question>() } });
            else
                return View("CreateQuestion", new SurveyCreationViewModel() { CurrentSurvey = survey });
        }

        [HttpPost]
        public IActionResult CreateSurvey(SurveyCreationViewModel model)
        {
            if (String.IsNullOrEmpty(model.CurrentSurvey.Description) || String.IsNullOrEmpty(model.CurrentSurvey.Title))
            {
                ModelState.AddModelError("TitleDescriptionEmpty", "Title and Description must be filled");
                return View(model);
            }
            else
            {
                if (model.CurrentSurvey.Questions is null)
                    model.CurrentSurvey.Questions = new List<Question>();
                HttpContext.Session.SetObject("CurrentSurvey", model.CurrentSurvey);
                TempData["QuestionAction"] = "create";
                return View("CreateQuestion", model);
            }
        }
        [HttpGet]
        public IActionResult AddQuestion(int id)
        {
            Survey survey = _sophiaDB.Surveys.Include(c => c.Questions).Where(x => x.SurveyId == id).FirstOrDefault();
            if (survey != null)
            {
                if (HttpContext.Session.GetObject<Survey>("CurrentSurvey") != null)
                    HttpContext.Session.Clear();
                HttpContext.Session.SetObject("CurrentSurvey", survey);
                return RedirectToAction("CreateQuestion");
            }
            return View("Index");
        }
        [HttpGet]
        public IActionResult CreateQuestion()
        {
            TempData["QuestionAction"] = "create";
            var survey = HttpContext.Session.GetObject<Survey>("CurrentSurvey");
            if (survey != null)
                return View(new SurveyCreationViewModel { CurrentSurvey = survey, CurrentQuestion = new Question() });
            else
                return View("CreateSurvey", new SurveyCreationViewModel() { CurrentSurvey = new Survey() });
        }
        [HttpPost]
        public IActionResult CreateQuestion(SurveyCreationViewModel model)
        {
            TempData["QuestionAction"] = "create";
            var survey = HttpContext.Session.GetObject<Survey>("CurrentSurvey");

            ParseQuestionResult result = model.CurrentQuestion.ParseQuestion();
            if (result.Success)
            {
                survey.Questions.Add(model.CurrentQuestion);
                HttpContext.Session.SetObject("CurrentSurvey", survey);
                return Redirect("CreateQuestion");
            }
            else
            {
                ModelState.AddModelError("error", result.ErrorMessage);
                return View(new SurveyCreationViewModel { CurrentSurvey = survey, CurrentQuestion = model.CurrentQuestion });
            }
        }

        public async Task<IActionResult> SaveSurvey()
        {
            try
            {
                var survey = HttpContext.Session.GetObject<Survey>("CurrentSurvey");
                if (survey.SurveyId == 0)
                {
                    _sophiaDB.Surveys.Add(survey);
                    await _sophiaDB.SaveChangesAsync();
                }
                else
                {
                    var dbSurvey = _sophiaDB.Surveys.Include(q => q.Questions).Where(c=>c.SurveyId == survey.SurveyId).FirstOrDefault();
                    dbSurvey.Questions.AddRange(survey.Questions.Where(x => x.QuestionId == 0));
                    _sophiaDB.SaveChanges();
                }
            }
            catch { }

            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult EditSurvey(int id)
        {
            Survey survey = _sophiaDB.Surveys.Where(x => x.SurveyId == id).Include(x => x.Questions).FirstOrDefault();
            return View(new EditSurveyViewModel { Survey = survey });
        }
        public IActionResult EditSurvey(EditSurveyViewModel model)
        {
            Survey survey = _sophiaDB.Surveys.Where(x => x.SurveyId == model.Survey.SurveyId).Include(c => c.Questions).FirstOrDefault();
            survey.Title = model.Survey.Title;
            survey.Description = model.Survey.Description;
            survey.Active = model.Survey.Active;
            _sophiaDB.SaveChanges();

            return View("Index", _sophiaDB.Surveys.Include(q => q.Questions).ToList());
        }
        [HttpGet]
        public IActionResult DiscardSurvey()
        {
            HttpContext.Session.Remove("CurrentSurvey");
            return View("Index", _sophiaDB.Surveys.Include(x => x.Questions).ToList());
        }
        [HttpGet]
        public IActionResult DeleteSurvey(int id)
        {
            Survey survey = _sophiaDB.Surveys.Where(x => x.SurveyId == id).FirstOrDefault();
            _sophiaDB.Surveys.Remove(survey);
            _sophiaDB.SaveChanges();
            return Redirect("Index");
        }
        [HttpGet]
        public IActionResult RearrangeSurvey(int id)
        {
            return View(_sophiaDB.Surveys.Include(x => x.Questions).Where(c => c.SurveyId == id).FirstOrDefault());
        }
        [HttpGet]
        public IActionResult EditQuestion(int id)
        {
            Question question = _sophiaDB.Questions.Where(x => x.QuestionId == id).FirstOrDefault();
            TempData["QuestionAction"] = "edit";
            return View("CreateQuestion",new SurveyCreationViewModel(){CurrentQuestion = question });
        }
       
        [HttpPost]
        public async Task<IActionResult> EditQuestion(SurveyCreationViewModel model)
        {
            Question dbquestion = _sophiaDB.Questions.Where(q => q.QuestionId == model.CurrentQuestion.QuestionId).FirstOrDefault();

            ParseQuestionResult result = model.CurrentQuestion.ParseQuestion();
            if (result.Success)
            {
                dbquestion.QuestionText = model.CurrentQuestion.QuestionText;
                dbquestion.OpenEndedAnswer = model.CurrentQuestion.OpenEndedAnswer;
                //dbquestion.ClosedEndedAnswer = question.ClosedEndedAnswer;
                dbquestion.ClosedEndedAnswerDesign = model.CurrentQuestion.ClosedEndedAnswerDesign;
                dbquestion.MultipleChoiceAnswer = model.CurrentQuestion.MultipleChoiceAnswer;
                dbquestion.MultipleChoiceAnswerDesign = model.CurrentQuestion.MultipleChoiceAnswerDesign;
                dbquestion.RatingAnswer = model.CurrentQuestion.RatingAnswer;
                dbquestion.RatingDesign = model.CurrentQuestion.RatingDesign;
                dbquestion.Type = model.CurrentQuestion.Type;
                dbquestion.SubCategory = model.CurrentQuestion.SubCategory;

                await _sophiaDB.SaveChangesAsync();

                return RedirectToAction("EditSurvey", "Pytania", new { id = dbquestion.SurveyId });
            }
            else
            {
                ModelState.AddModelError("someerror", result.ErrorMessage);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult DeleteQuestion(int id)
        {
            Question question = _sophiaDB.Questions.Where(y => y.QuestionId == id).FirstOrDefault();
            _sophiaDB.Questions.Remove(question);
            _sophiaDB.SaveChanges();
            return RedirectToAction("EditSurvey", new { id = question.SurveyId });
        }

        public IActionResult SaveChanges()
        {
            return View("Index", _sophiaDB.Surveys);
        }
    }
}
