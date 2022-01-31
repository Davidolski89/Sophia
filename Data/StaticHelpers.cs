using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sophia.Models.Rogatio;

namespace Sophia.Data
{
    public static class StaticHelpers
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static Question CloneQuestion(this Question question)
        {
            string stringObject = JsonConvert.SerializeObject(question);
            return JsonConvert.DeserializeObject<Question>(stringObject);
        }
        public static List<Question> CloneQuestions(this List<Question> questions)
        {
            List<Question> clondesObjects = new List<Question>();
            foreach (Question item in questions)
            {
                clondesObjects.Add(item.CloneQuestion());
            }
            return clondesObjects;
        }

        public static IEnumerable<string> GetMultipleChoices(this string multipleChoices){
            List<string> choices = multipleChoices.Split(',').ToList();            
            return choices.Where(x=>!string.IsNullOrEmpty(x));
        }

        public static RatingResult GetRatingResult(this string ratingString)
        {
            RatingResult result = new RatingResult();
            try
            {
                string[] items = ratingString.Split(',');
                result.FromToWords = new string[] { items[0], items[1] };
                result.RatingSteps = int.Parse(items[2]);
                result.Success = true;
                return result;
            }
            catch
            {
                result.Success = false;
                return result;
            }            
        }
        public static bool AnswerAnswered(this Answer answer)
        {
            switch (answer.QuestionType)
            {
                case QuestionType.OpenEnded:
                    if (!string.IsNullOrEmpty(answer.OpenEndedAnswer))
                        return true;
                    else
                        return false;                    
                //case QuestionType.ClosedEnded:
                //    if (answer.ClosedEndedAnswer.HasValue)
                //        return true;
                //    else
                //        return false;                   
                case QuestionType.Rating:
                    if (answer.RatingAnswer.HasValue)
                        return true;
                    else
                        return false;                    
                case QuestionType.MultipleChoice:
                    if (!string.IsNullOrEmpty(answer.MultipleChoiceAnswer))
                        return true;
                    else
                        return false;                    
                default:
                    return false;                    
            }
        }

        public class RatingResult
        {
            public string[] FromToWords { get; set; }
            public int RatingSteps { get; set; }
            public bool Success { get; set; }
        }

        public static void CleanQuestion(this Question question)
        {
            switch (question.Type)
            {
                case QuestionType.OpenEnded:
                    //question.ClosedEndedAnswer = false;
                    question.ClosedEndedAnswerDesign = null;
                    question.MultipleChoiceAnswerDesign = null;
                    question.MultipleChoiceAnswer = null;
                    question.RatingDesign = null;
                    question.RatingAnswer = 0;
                    break;
                //case QuestionType.ClosedEnded:
                //    question.OpenEndedAnswer = "";                   
                //    question.MultipleChoiceAnswerDesign = null;
                //    question.MultipleChoiceAnswer = null;
                //    question.RatingDesign = null;
                //    question.RatingAnswer = 0;
                //    break;
                case QuestionType.Rating:
                    question.OpenEndedAnswer = "";
                    //question.ClosedEndedAnswer = false;
                    question.ClosedEndedAnswerDesign = null;
                    question.MultipleChoiceAnswerDesign = null;
                    question.MultipleChoiceAnswer = null;                    
                    break;
                case QuestionType.MultipleChoice:
                    question.OpenEndedAnswer = null;
                    //question.ClosedEndedAnswer = false;
                    question.ClosedEndedAnswerDesign = null;                    
                    question.RatingDesign = null;
                    question.RatingAnswer = 0;
                    break;
                default:
                    break;
            }
        }
        public static ParseQuestionResult ParseQuestion(this Question question)
        {
            question.CleanQuestion();

            ParseQuestionResult result = new ParseQuestionResult() { Question = question , ErrorMessage=""};

            //if(question.Type != QuestionType.ClosedEnded)
            //{
            //    if (string.IsNullOrEmpty(question.QuestionText))
            //    {
            //        result.ErrorMessage = "Question text can't be empty";
            //        result.Success = false;
            //        return result;
            //    }
            //}
            

            switch (question.Type)
            {
                case QuestionType.OpenEnded:
                    result.Success = true;
                    break;
                //case QuestionType.ClosedEnded:
                //    if (string.IsNullOrEmpty(question.ClosedEndedAnswerDesign))
                //    {
                //        result.ErrorMessage = "Closedended question text can't be empty";
                //        result.Success = false;                       
                //    }
                //    else
                //    {
                //        result.Success = true;                        
                //    }
                //    break;
                case QuestionType.Rating:
                    try                    
                    { 
                        if(question.RatingDesign.GetRatingResult().Success)
                            result.Success = true;
                    }
                    catch
                    {
                        result.Success = false;
                        result.ErrorMessage = "Rating format is wrong";
                    }
                    break;
                case QuestionType.MultipleChoice:
                    if(question.MultipleChoiceAnswerDesign.GetMultipleChoices().Count() < 2)
                    {
                        result.Success = false;
                        result.ErrorMessage = "There have to be at least two choices";
                    }
                    else
                    {
                        result.Success = true;
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
        public class ParseQuestionResult
        {
            public Question Question { get; set; }
            public string ErrorMessage { get; set; }
            public bool Success { get; set; }
        }
    }
    
}
