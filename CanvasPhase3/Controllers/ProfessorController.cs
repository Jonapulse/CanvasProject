using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CanvasPhase3.Context;
using CanvasPhase3.Context;
using CanvasPhase3.Entities;
using CanvasPhase3.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CanvasPhase3.Controllers
{
    [Authorize(Roles = "Professor")]
    public class ProfessorController(LMSContext myDbContext): Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Students(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
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

        public IActionResult Categories(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult CatAssignments(string subject, string num, string season, string year, string cat)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
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

        public IActionResult Submissions(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }

        public IActionResult Grade(string subject, string num, string season, string year, string cat, string aname, string uid)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            ViewData["uid"] = uid;
            return View();
        }

        /*******Begin code to modify********/


        /// <summary>
        /// Returns a JSON array of all the students in a class.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "dob" - date of birth
        /// "grade" - the student's grade in this class
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetStudentsInClass(string subject, int num, string season, int year)
        {
            var students = myDbContext.Enrollments.Where(e =>
                e.Class.Catalog.Number == num && e.Class.Catalog.Dep != null && e.Class.Catalog.Dep.Subjabbrv == subject &&
                e.Class.Semesterterm == season && e.Class.Semesteryear == year).Select(e => new
            {
                fname = e.UidNavigation.UidNavigation.Lastname,
                lname = e.UidNavigation.UidNavigation.Firstname,
                uid = e.Uid,
                dob = e.UidNavigation.UidNavigation.Dob,
                grade = e.Grade
            }).ToList();
            
            return Json(students);
        }



        /// <summary>
        /// Returns a JSON array with all the assignments in an assignment category for a class.
        /// If the "category" parameter is null, return all assignments in the class.
        /// Each object in the array should have the following fields:
        /// "aname" - The assignment name
        /// "cname" - The assignment category name.
        /// "due" - The due DateTime
        /// "submissions" - The number of submissions to the assignment
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class, 
        /// or null to return assignments from all categories</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentsInCategory(string subject, int num, string season, int year, string category)
        {
            var assignments = myDbContext.Assignments.Where(a =>
                a.Category.Name == category && a.Category.Class.Semesterterm == season && 
                a.Category.Class.Semesteryear == year && a.Category.Class.Catalog.Number == num && 
                a.Category.Class.Catalog.Dep != null && a.Category.Class.Catalog.Dep.Subjabbrv == subject).Select(a => new
            {
                aname = a.Name,
                cname = a.Category.Name,
                due = a.Duedate,
                Submissions = a.Assignmentsubmissions.Count
            }).ToList();
            
            return Json(assignments);
        }


        /// <summary>
        /// Returns a JSON array of the assignment categories for a certain class.
        /// Each object in the array should have the folling fields:
        /// "name" - The category name
        /// "weight" - The category weight
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentCategories(string subject, int num, string season, int year)
        {
            var categories = myDbContext.Assignmentcategories.Where(c =>
                c.Class.Semesterterm == season && c.Class.Semesteryear == year && c.Class.Catalog.Number == num &&
                c.Class.Catalog.Dep != null && c.Class.Catalog.Dep.Subjabbrv == subject).Select(c => new
            {
                name = c.Name,
                weight = c.Gradingweight
            }).ToList();
            
            return Json(categories);
        }

        /// <summary>
        /// Creates a new assignment category for the specified class.
        /// If a category of the given class with the given name already exists, return success = false.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The new category name</param>
        /// <param name="catweight">The new category weight</param>
        /// <returns>A JSON object containing {success = true/false} </returns>
        public IActionResult CreateAssignmentCategory(string subject, int num, string season, int year, string category, int catweight)
        {
            //Lookup the class
            var classForCategory = myDbContext.Classes.FirstOrDefault(c =>
                c.Semesterterm == season && c.Semesteryear == year && c.Catalog.Number == num &&
                c.Catalog.Dep != null && c.Catalog.Dep.Subjabbrv == subject);
            
            //If it doesn't exist or that category name already exists for this class, abort
            if (classForCategory == null || myDbContext.Assignmentcategories.Any(c => 
                    c.Classid == classForCategory.Classid && c.Name == category))
            {
                return Json(new { success = false });
            }

            Assignmentcategory newCat = new Assignmentcategory()
            {
                Name = category,
                Gradingweight = (short)catweight,
                Classid = classForCategory.Classid
            };
            myDbContext.Assignmentcategories.Add(newCat);
            int entriesWritten = myDbContext.SaveChanges();
            return Json(new { success = entriesWritten > 0 });
        }

        /// <summary>
        /// Creates a new assignment for the given class and category.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The new assignment name</param>
        /// <param name="asgpoints">The max point value for the new assignment</param>
        /// <param name="asgdue">The due DateTime for the new assignment</param>
        /// <param name="asgcontents">The contents of the new assignment</param>
        /// <returns>A JSON object containing success = true/false</returns>
        public IActionResult CreateAssignment(string subject, int num, string season, int year, string category, string asgname, int asgpoints, DateTime asgdue, string asgcontents)
        {
            
            //Find the category
            var categoryForAssignment = myDbContext.Assignmentcategories.FirstOrDefault(c =>
                c.Name == category && c.Class.Semesterterm == season && c.Class.Semesteryear == year
                && c.Class.Catalog.Number == num && c.Class.Catalog.Dep != null &&
                c.Class.Catalog.Dep.Subjabbrv == subject);

            if (categoryForAssignment == null)
            {
                return Json(new { success = false });
            }
            
            Assignment assignment = new Assignment()
            {
                Categoryid =  categoryForAssignment.Categoryid,
                Name = asgname,
                Maxscore = asgpoints,
                Duedate = asgdue,
                Content = asgcontents
            };
            
            myDbContext.Assignments.Add(assignment);
            int entriesWritten = myDbContext.SaveChanges();
            
            // Recalculate grades for every enrolled student
            var studentIds = myDbContext.Enrollments
                .Where(e => e.Classid == categoryForAssignment.Classid)
                .Select(e => e.Uid)
                .ToList();

            foreach (var studentId in studentIds)
            {
                CalcLetterGrade(categoryForAssignment.Classid, studentId);
            }
            
            return Json(new { success = entriesWritten > 0 });
        }


        /// <summary>
        /// Gets a JSON array of all the submissions to a certain assignment.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "time" - DateTime of the submission
        /// "score" - The score given to the submission
        /// 
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetSubmissionsToAssignment(string subject, int num, string season, int year, string category, string asgname)
        {
            //Find the category
            var categoryForAssignment = myDbContext.Assignmentcategories.FirstOrDefault(c =>
                c.Name == category && c.Class.Semesterterm == season && c.Class.Semesteryear == year
                && c.Class.Catalog.Number == num && c.Class.Catalog.Dep != null &&
                c.Class.Catalog.Dep.Subjabbrv == subject);

            if (categoryForAssignment == null)
            {
                return Json(new { success = false });
            }
            
            //Find the assignment
            var assignment = myDbContext.Assignments.FirstOrDefault(c => c.Categoryid == categoryForAssignment.Classid && c.Name == asgname);
            if (assignment == null)
            {
                return Json(new { success = false });
            }

            var submissions = myDbContext.Assignmentsubmissions.Where(c => c.Assignmentid == assignment.Assignmentid)
                .Select(a => new
                {
                    fname = a.Student.UidNavigation.Firstname,
                    lname = a.Student.UidNavigation.Lastname,
                    uid = a.Studentid,
                    time = a.Submissiontime,
                    score = a.Score
                }).ToList();
                
            return Json(submissions);
        }


        /// <summary>
        /// Set the score of an assignment submission
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <param name="uid">The uid of the student whose submission is being graded</param>
        /// <param name="score">The new score for the submission</param>
        /// <returns>A JSON object containing success = true/false</returns>
        public IActionResult GradeSubmission(string subject, int num, string season, int year, string category, string asgname, string uid, int score)
        {
            //Find the assignment
            var assignment = myDbContext.Assignments.FirstOrDefault(c =>
                c.Category.Class.Catalog.Dep.Subjabbrv == subject && c.Category.Class.Catalog.Number == num && 
                c.Category.Class.Semesterterm == season && c.Category.Class.Semesteryear == year &&
                c.Name == asgname && c.Category.Name == category);

            if (assignment == null)
            {
                return Json(new { success = false });
            }
            
            var submission = myDbContext.Assignmentsubmissions.Where(c =>
                c.Studentid == uid.FromDisplayId() && c.Assignmentid == assignment.Assignmentid)
                .OrderByDescending(c => c.Submissiontime).FirstOrDefault();
            
            if (submission == null)
            {
                return Json(new { success = false });
            }
            else
            {
                submission.Score = score;
                myDbContext.Assignmentsubmissions.Update(submission);
                int entriesWritten = myDbContext.SaveChanges();
                
                // Calculate the student's letter grade
                CalcLetterGrade(assignment.Category.Classid, submission.Studentid);
                
                return Json(new { success = entriesWritten > 0 });
            }
        }


        /// <summary>
        /// Returns a JSON array of the classes taught by the specified professor
        /// Each object in the array should have the following fields:
        /// "subject" - The subject abbreviation of the class (such as "CS")
        /// "number" - The course number (such as 5530)
        /// "name" - The course name
        /// "season" - The season part of the semester in which the class is taught
        /// "year" - The year part of the semester in which the class is taught
        /// </summary>
        /// <param name="uid">The professor's uid</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetMyClasses(string uid)
        {          
            var myClasses = myDbContext.Classes.Where(c => c.Profid == uid.FromDisplayId()).Select(c => new
            {
                subject = c.Catalog.Dep.Subjabbrv,
                number = c.Catalog.Number,
                name = c.Catalog.Name,
                season = c.Semesterterm,
                year = c.Semesteryear
            }).ToList();
            
            return Json(myClasses);
        }
        /*******End code to modify********/

        private void CalcLetterGrade(int classId, int studentId)
        {
            // Find the enrollment
            var enrollment = myDbContext.Enrollments.FirstOrDefault(e =>
                e.Classid == classId &&
                e.Uid == studentId);

            if (enrollment == null)
                return;
            
            // Get all categories for the class
            var categories = myDbContext.Assignmentcategories.Where(c => c.Classid == classId).ToList();

            double weightedTotal = 0.0;
            double totalWeights = 0.0;

            foreach (var c in categories)
            {
                // Get assignments in the category
                var assignments = myDbContext.Assignments.Where(a => a.Categoryid == c.Categoryid).ToList();
                
                // Ignore empty categories
                if (assignments.Count == 0)
                    continue;

                double earnedPoints = 0.0;
                double possiblePoints = 0.0;

                foreach (var a in assignments)
                {
                    possiblePoints += a.Maxscore ?? 0;
                    
                    var submission = myDbContext.Assignmentsubmissions.FirstOrDefault(s => s.Assignmentid == a.Assignmentid
                    && s.Studentid == studentId);
                    
                    earnedPoints += submission.Score ?? 0;
                }

                if (possiblePoints == 0)
                    continue;
                
                double percent = earnedPoints / possiblePoints;
                
                weightedTotal += percent * (c.Gradingweight ?? 0);
                totalWeights += c.Gradingweight ?? 0;
            }
            
            // No graded categories
            if (totalWeights == 0)
            {
                enrollment.Grade = "--";
                myDbContext.SaveChanges();
                return;
            }
            
            // Rescale to percentage
            double finalPercent = weightedTotal *  100.0 / totalWeights;

            string letter;
            
            if(finalPercent >= 92) letter = "A";
            else if (finalPercent >= 90) letter = "A-";
            else if (finalPercent >= 87) letter = "B+";
            else if (finalPercent >= 82) letter = "B";
            else if (finalPercent >= 80) letter = "B-";
            else if (finalPercent >= 77) letter = "C+";
            else if (finalPercent >= 72) letter = "C";
            else if (finalPercent >= 70) letter = "C-";
            else if (finalPercent >= 67) letter = "D+";
            else if (finalPercent >= 62) letter = "D";
            else if (finalPercent >= 60) letter = "D-";
            else letter = "E";
            
            enrollment.Grade = letter;
            myDbContext.SaveChanges();
        }
    }
}
