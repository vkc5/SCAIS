using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCAIS.Core.Models.CoursePlan
{
    public class CoursePlanProduct
    {
        public string CoursePlanId { get; set; }
        public string StudentId { get; set; }
        public int SemesterId { get; set; }
        public string Status { get; set; }

        public List<CoursePlanItem> SelectedCourses { get; set; } = new List<CoursePlanItem>();
        public List<CoursePlanItem> EligibleCourses { get; set; } = new List<CoursePlanItem>();

        public int TotalCredits => SelectedCourses.Sum(c => c.Credits);
    }
}
