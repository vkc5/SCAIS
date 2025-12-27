using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using SCAIS.Core.Database;
using SCAIS.Model;

namespace SCAIS.Admin.Pages
{
    public partial class AdminManageCoursesPage : UserControl
    {
        private List<Course> allCourses = new List<Course>();
        private List<Course> courses = new List<Course>();
        private int pageSize = 20;
        private int currentPage = 1;
        private int totalPages = 1;

        public AdminManageCoursesPage()
        {
            InitializeComponent();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            // Your filtering code here
        }
        private void AdminManageCoursesPage_Load(object sender, EventArgs e)
        {
            LoadCourses();
        }

        private void LoadCourses()
        {
            string sql = @"
                SELECT 
                    c.CourseCode,
                    c.CourseName,
                    sp.SpecializationName,
                    ISNULL(pr.PrerequisiteCourseCode, '') AS Prerequisite,
                    ISNULL(co.CorequisiteCourseCode, '') AS Corequisite
                FROM dbo.Courses c
                LEFT JOIN dbo.Specializations sp ON c.SpecializationID = sp.SpecializationID
                LEFT JOIN dbo.Prerequisites pr ON c.CourseCode = pr.CourseCode
                LEFT JOIN dbo.Corequisites co ON c.CourseCode = co.CourseCode
                ORDER BY c.CourseCode;
            ";

            DataTable dt = Db.Query(sql);

            allCourses.Clear();
            courses.Clear();

            foreach (DataRow r in dt.Rows)
            {
                Course course = new Course
                {
                    CourseCode = r["CourseCode"].ToString(),
                    CourseName = r["CourseName"].ToString(),
                    SpecializationName = r["SpecializationName"].ToString(),
                    Prerequisite = r["Prerequisite"].ToString(),
                    Corequisite = r["Corequisite"].ToString()
                };

                allCourses.Add(course);
            }

            courses = new List<Course>(allCourses);
            FillGrid(courses);
        }

        private void FillGrid(List<Course> list)
        {
            courseGridView.Rows.Clear();

            if (list == null || list.Count == 0)
            {
                courseNumLab.Text = "0 of 0 courses";
                return;
            }

            totalPages = (int)Math.Ceiling(list.Count / (double)pageSize);


            //pagination
            if (currentPage > totalPages) currentPage = totalPages;
            if (currentPage < 1) currentPage = 1;

            var pagedCourses = list
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            foreach (var c in pagedCourses)
            {
                courseGridView.Rows.Add(
                    c.CourseCode,
                    c.CourseName,
                    c.SpecializationName,
                    c.Prerequisite,
                    c.Corequisite,
                    "Edit",    // Button column
                    "Delete"   // Button column
                );
            }

            int start = (currentPage - 1) * pageSize + 1;
            int end = start + pagedCourses.Count - 1;
            courseNumLab.Text = $"{start}-{end} of {list.Count} courses";

            backBtn.Enabled = currentPage > 1;
            NextBtn.Enabled = currentPage < totalPages;
        }

    

        private void NextBtn_Click_1(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                FillGrid(courses);
            }
        }

        private void backBtn_Click_1(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                FillGrid(courses);
            }
        }
    }
}
