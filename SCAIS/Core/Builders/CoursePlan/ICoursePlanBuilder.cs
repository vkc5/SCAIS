using SCAIS.Core.Models.CoursePlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCAIS.Core.Builders.CoursePlan
{
    public interface ICoursePlanBuilder
    {
        void StartNewPlan(string planId, string studentId, int semesterId);
        void LoadSelectedCourses();
        void LoadEligibleCoursesAutoValidated();
        void CalculateTotals();
        void SetInitialStatus(string status);

        CoursePlanProduct GetResult();
    }
}
