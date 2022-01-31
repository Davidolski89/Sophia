using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sophia.Models.Rogatio
{
    public interface IQuestion
    {
        public int QuestionId { get; set; }
        public QuestionType Type { get; set; }
        public string QuestionText { get; set; }
        public string OpenEndedAnswer { get; set; }
        //public bool ClosedEndedAnswer { get; set; }
        public int RatingAnswer { get; set; }
        public string MultipleChoiceAnswer { get; set; }
        public Category Category { get; set; }
        public SubCategory SubCategory { get; set; }

    }

    public enum QuestionType
    {
        OpenEnded,
        //ClosedEnded,
        Rating,
        MultipleChoice        
    }
    public enum Category
    {
        Category1,
        Category2,
        Category3,
        Category4,
        Category5
    }
    public enum SubCategory
    {
        Category1,
        Category2,
        Category3,
        Category4,
        Category5,
        Category6,
        Category7,
        Category8,
        Category9,
        Category10,
        Category11,
        Category12,
        Category13,
        Category14,
        Category15,
        Category16,
        Category17,
        Category18,
        Category19,
        Category20
    }
}
