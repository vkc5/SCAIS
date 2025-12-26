using System;
using System.Collections.Generic;

namespace SCAIS.Model
{
    public class Course
    {
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string SpecializationName { get; set; }
        public string Prerequisite { get; set; }
        public string Corequisite { get; set; } 
    }
}
