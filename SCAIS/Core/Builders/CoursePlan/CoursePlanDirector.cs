using SCAIS.Core.Models.CoursePlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCAIS.Core.Builders.CoursePlan
{
    public class CoursePlanDirector
    {
        private readonly ICoursePlanBuilder _builder;

        public CoursePlanDirector(ICoursePlanBuilder builder)
        {
            _builder = builder;
        }

        public CoursePlanProduct CreatePlan(
            string planId,
            string studentId,
            int semesterId,
            string status)
        {
            _builder.StartNewPlan(planId, studentId, semesterId);
            _builder.SetInitialStatus(status);
            _builder.LoadSelectedCourses();
            _builder.LoadEligibleCoursesAutoValidated();
            _builder.CalculateTotals();

            return _builder.GetResult();
        }
    }
}
