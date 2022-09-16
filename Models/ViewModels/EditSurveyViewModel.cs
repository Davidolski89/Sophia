using Sophia.Models.Rogatio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sophia.Models.ViewModels
{
    // Das ist einfach nur schlecht xD.
    // Man hätte gleich die Entity verwenden können wenn man die sowieso schon komplett mitgibt.
    // Die Klasse scheint unnötig
    public class EditSurveyViewModel
    {
        public Survey Survey { get; set; }
    }
}
