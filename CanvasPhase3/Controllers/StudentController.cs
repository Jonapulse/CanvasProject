using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CanvasPhase3.Context;
using CanvasPhase3.Context;
using CanvasPhase3.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CanvasPhase3.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController(LMSContext myDbContext) : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Catalog()
        {
            return View();
        }

        public IActionResult Class(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult Assignment(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }


        public IActionResult ClassListings(string subject, string num)
        {
            Console.WriteLine(subject + num);
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            return View();
        }


        /*******Begin code to modify********/

        /// <summary>
        /// Returns a JSON array of the classes the given student is enrolled in.
        /// Each object in the array should have the following fields:
        /// "subject" - The subject abbreviation of the class (such as "CS")
        /// "number" - The course number (such as 5530)
        /// "name" - The course name
        /// "season" - The season part of the semester
        /// "year" - The year part of the semester
        /// "grade" - The grade earned in the class, or "--" if one hasn't been assigned
        /// </summary>
        /// <param name="uid">The uid of the student</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetMyClasses(string uid)
        {           
            int studentUid = int.Parse(uid);
            
            var classes = myDbContext.Enrollments.Where(e => e.Uid == studentUid ).Select(e => new
            {
                subject = e.Class.Catalog.Dep.Subjabbrv,
                number = e.Class.Catalog.Number,
                name = e.Class.Catalog.Name,
                season = e.Class.Semesterterm,
                year = e.Class.Semesteryear,
                grade = e.Grade ?? "--"
            }).ToList();
            
            return Json(classes);
        }

        /// <summary>
        /// Returns a JSON array of all the assignments in the given class that the given student is enrolled in.
        /// Each object in the array should have the following fields:
        /// "aname" - The assignment name
        /// "cname" - The category name that the assignment belongs to
        /// "due" - The due Date/Time
        /// "score" - The score earned by the student, or null if the student has not submitted to this assignment.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="uid"></param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentsInClass(string subject, int num, string season, int year, string uid)
        {            
            int studentUid = int.Parse(uid);

            var assignments = myDbContext.Assignments.Where(a 
                => a.Category.Class.Catalog.Dep.Subjabbrv == subject && a.Category.Class.Catalog.Number == num &&
                   a.Category.Class.Semesterterm == season && a.Category.Class.Semesteryear == year).Select(a => new
            {
                aname = a.Name,
                cname = a.Category.Name,
                due = a.Duedate,
                score = a.Assignmentsubmissions.Where(s => s.Student.Uid == studentUid)
                    .Select(s => s.Score).FirstOrDefault()
            }).ToList();
            
            return Json(assignments);
        }



        /// <summary>
        /// Adds a submission to the given assignment for the given student
        /// The submission should use the current time as its DateTime
        /// You can get the current time with DateTime.Now
        /// The score of the submission should start as 0 until a Professor grades it
        /// If a Student submits to an assignment again, it should replace the submission contents
        /// and the submission time (the score should remain the same).
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The new assignment name</param>
        /// <param name="uid">The student submitting the assignment</param>
        /// <param name="contents">The text contents of the student's submission</param>
        /// <returns>A JSON object containing {success = true/false}</returns>
        public IActionResult SubmitAssignmentText(string subject, int num, string season, int year,
          string category, string asgname, string uid, string contents)
        {           
            int studentUid = int.Parse(uid);
            
            // Find the assignment
            var assignment = myDbContext.Assignments.FirstOrDefault(a => 
                a.Name == asgname && a.Category.Class.Catalog.Dep.Subjabbrv == subject && a.Category.Class.Catalog.Number == num &&
                a.Category.Class.Semesterterm == season && a.Category.Class.Semesteryear == year &&
                a.Category.Name == category);

            if (assignment == null)
            {
                return Json(new { success = false });
            }
            
            // Check if it's already been submitted
            var submission = myDbContext.Assignmentsubmissions.FirstOrDefault(s => s.Student.Uid == studentUid &&
                s.Assignmentid == assignment.Assignmentid);

            if (submission == null)
            {
                // Create new submission
                submission = new Assignmentsubmission
                {
                    Assignmentid = assignment.Assignmentid,
                    Studentid = studentUid,
                    Content = contents,
                    Submissiontime = DateTime.Now,
                    Score = 0
                };
                
                myDbContext.Assignmentsubmissions.Add(submission);
            }
            else
            {
                // Replace contents and submission time
                submission.Content = contents;
                submission.Submissiontime = DateTime.Now;
            }
            
            int entriesWritten = myDbContext.SaveChanges();
            return Json(new { success = entriesWritten > 0 });
        }


        /// <summary>
        /// Enrolls a student in a class.
        /// </summary>
        /// <param name="subject">The department subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester</param>
        /// <param name="year">The year part of the semester</param>
        /// <param name="uid">The uid of the student</param>
        /// <returns>A JSON object containing {success = {true/false}. 
        /// false if the student is already enrolled in the class, true otherwise.</returns>
        public IActionResult Enroll(string subject, int num, string season, int year, string uid)
        {          
            int  studentUid = int.Parse(uid);
            
            // Get class to enroll in
            var classToEnroll = myDbContext.Classes.FirstOrDefault(c => c.Catalog.Dep.Subjabbrv == subject && 
                c.Catalog.Number == num && c.Semesterterm == season && c.Semesteryear == year);

            if (classToEnroll == null)
            {
                return Json(new { success = false });
            }
            
            // Check if student is already enrolled
            bool alreadyEnrolled = myDbContext.Enrollments.Any(e => e.Classid == classToEnroll.Classid && e.Uid == studentUid);

            if (alreadyEnrolled)
            {
                return Json(new { success = false });
            }
            
            // Enroll student
            myDbContext.Enrollments.Add(new Enrollment
            {
                Classid = classToEnroll.Classid,
                Uid = studentUid,
                Grade = null
            });
            
            int entriesWritten = myDbContext.SaveChanges();
            return Json(new { success = entriesWritten > 0 });
        }



        /// <summary>
        /// Calculates a student's GPA
        /// A student's GPA is determined by the grade-point representation of the average grade in all their classes.
        /// Assume all classes are 4 credit hours.
        /// If a student does not have a grade in a class ("--"), that class is not counted in the average.
        /// If a student is not enrolled in any classes, they have a GPA of 0.0.
        /// Otherwise, the point-value of a letter grade is determined by the table on this page:
        /// https://advising.utah.edu/academic-standards/gpa-calculator-new.php
        /// </summary>
        /// <param name="uid">The uid of the student</param>
        /// <returns>A JSON object containing a single field called "gpa" with the number value</returns>
        public IActionResult GetGPA(string uid)
        {            
            int studentUid = int.Parse(uid);

            // Get list of grades
            var grades = myDbContext.Enrollments.Where(e => e.Uid == studentUid && e.Grade != null && e.Grade != "--")
                .Select(e => e.Grade).ToList();

            if (grades.Count == 0)
            {
                return Json(new { gpa = 0.0 });
            }
            
            var gradePoints = new Dictionary<string, double>()
            {
                ["A"] = 4.0,
                ["A-"] = 3.7,
                ["B+"] = 3.3,
                ["B"] = 3.0,
                ["B-"] = 2.7,
                ["C+"] = 2.3,
                ["C"] = 2.0,
                ["C-"] = 1.7,
                ["D+"] = 1.3,
                ["D"] = 1.0,
                ["D-"] = 0.7,
                ["E"] = 0.0
            };
            
            double total = grades.Sum(g => gradePoints[g]);
            double gpa = total / grades.Count;
            
            return Json(new {gpa});
        }
                
        /*******End code to modify********/

    }
}
