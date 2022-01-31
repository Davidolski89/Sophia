using Sophia.Models.Rogatio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sophia.Models.ViewModels
{
    public class SurveyCreationViewModel
    {
        public Survey CurrentSurvey { get; set; }
        public Question CurrentQuestion { get; set; }
    }
}
