using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sophia.Models.Rogatio
{
    // Entity Db
    public class Question : IQuestion
    {
        public int QuestionId { get; set; }
        public int SurveyId { get; set; }
        public Survey Survey { get; set; }
        [InverseProperty("Question")]
        public List<Answer> Answers { get; set; }

        public QuestionType Type { get; set; }
        [Display(Name = "Question text")]
        public string QuestionText { get; set; }
        [Display(Name = "Proposed answer")]
        public string OpenEndedAnswer { get; set; }
        //[Display(Name = "Right choice")]
        //public bool ClosedEndedAnswer { get; set; }
        [Display(Name = "Closed Question")]
        public string ClosedEndedAnswerDesign { get; set; }
        [Display(Name = "Rating answer")]
        public int RatingAnswer { get; set; }
        [Display(Name = "Rating from - to keywords")]
        public string RatingDesign { get; set; }
        [Display(Name = "Right answer")]
        public string MultipleChoiceAnswer { get; set; }

        [Display(Name = "Answer selection")]
        public string MultipleChoiceAnswerDesign { get; set; }
        [Display(Name = "Category")]
        public Category Category { get; set; }
        [Display(Name = "SubCategory")]
        public SubCategory SubCategory { get; set; }

    }
}
