using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCAIS.Core.Models.CoursePlan
{
    public class CoursePlanItem
    {
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int Credits { get; set; }

        public bool IsEligible { get; set; }
        public string Message { get; set; }
    }
}
