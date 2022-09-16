using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sophia.Models.Rogatio
{
    // Entity Db
    public class Survey
    {
        public int SurveyId { get; set; }
        [InverseProperty("Survey")]
        public List<Question> Questions { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        
    }
}
