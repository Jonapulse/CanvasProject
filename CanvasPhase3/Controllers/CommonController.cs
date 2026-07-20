using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using CanvasPhase3.Context;
using CanvasPhase3.Context;
using CanvasPhase3.Entities;
using CanvasPhase3.Utilities;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CanvasPhase3.Controllers
{
    //TODO: add your controller as a "primary constructor" param:
    //eg: public class CommonController(MyContextType myContext) 
    public class CommonController(LMSContext myDbContext) : Controller
    {

        /*******Begin code to modify********/

        /// <summary>
        /// Retreive a JSON array of all departments from the database.
        /// Each object in the array should have a field called "name" and "subject",
        /// where "name" is the department name and "subject" is the subject abbreviation.
        /// </summary>
        /// <returns>The JSON array</returns>
        public IActionResult GetDepartments()
        {
            var depts = myDbContext.Departments.Select(d => new
            {
                name = d.Name,
                subject = d.Subjabbrv
            }).ToList();
            
            return Json(depts);
        }



        /// <summary>
        /// Returns a JSON array representing the course catalog.
        /// Each object in the array should have the following fields:
        /// "subject": The subject abbreviation, (e.g. "CS")
        /// "dname": The department name, as in "Computer Science"
        /// "courses": An array of JSON objects representing the courses in the department.
        ///            Each field in this inner-array should have the following fields:
        ///            "number": The course number (e.g. 5530)
        ///            "cname": The course name (e.g. "Database Systems")
        /// </summary>
        /// <returns>The JSON array</returns>
        public IActionResult GetCatalog()
        {            
            var courses = myDbContext.Departments.Select(d => new
            {
                subject = d.Subjabbrv,
                dname = d.Name,
                courses = d.Courses.Select(c => new 
                {
                    number = c.Number,
                    cname = c.Name,
                })
            }).ToList();
            
            return Json(courses);
        }

        /// <summary>
        /// Returns a JSON array of all class offerings of a specific course.
        /// Each object in the array should have the following fields:
        /// "season": the season part of the semester, such as "Fall"
        /// "year": the year part of the semester
        /// "location": the location of the class
        /// "start": the start time in format "hh:mm:ss"
        /// "end": the end time in format "hh:mm:ss"
        /// "fname": the first name of the professor
        /// "lname": the last name of the professor
        /// </summary>
        /// <param name="subject">The subject abbreviation, as in "CS"</param>
        /// <param name="number">The course number, as in 5530</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetClassOfferings(string subject, int number)
        {            
            var classOfferings = myDbContext.Classes.Where(c =>
                c.Catalog.Number == number && c.Catalog.Dep.Subjabbrv == subject).Select(c => new
            {
                season = c.Semesterterm,
                year = c.Semesteryear,
                location = c.Location,
                start = c.Starttime,
                end = c.Endtime,
                fname = c.Prof.UidNavigation.Firstname,
                lname = c.Prof.UidNavigation.Lastname
            });
            return Json(classOfferings);
        }

        /// <summary>
        /// This method does NOT return JSON. It returns plain text (containing html).
        /// Use "return Content(...)" to return plain text.
        /// Returns the contents of an assignment.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment in the category</param>
        /// <returns>The assignment contents</returns>
        public IActionResult GetAssignmentContents(string subject, int num, string season, int year, string category, string asgname)
        {
            string content = myDbContext.Assignments.Where(a =>
                a.Category.Class.Catalog.Dep.Subjabbrv == subject && a.Category.Class.Catalog.Number == num &&
                a.Category.Class.Semesterterm == season && a.Category.Class.Semesteryear == year &&
                a.Category.Name == category && a.Name == asgname).Select(a => a.Content).First().ToString();
            return Content(content);
        }


        /// <summary>
        /// This method does NOT return JSON. It returns plain text (containing html).
        /// Use "return Content(...)" to return plain text.
        /// Returns the contents of an assignment submission.
        /// Returns the empty string ("") if there is no submission.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment in the category</param>
        /// <param name="uid">The uid of the student who submitted it</param>
        /// <returns>The submission text</returns>
        public IActionResult GetSubmissionText(string subject, int num, string season, int year, string category, string asgname, string uid)
        {            
            string submission = myDbContext.Assignmentsubmissions.Where(a =>
                a.Studentid == uid.FromDisplayId() &&
                a.Assignment.Category.Class.Catalog.Dep.Subjabbrv == subject && a.Assignment.Category.Class.Catalog.Number == num &&
                a.Assignment.Category.Class.Semesterterm == season && a.Assignment.Category.Class.Semesteryear == year &&
                a.Assignment.Category.Name == category && a.Assignment.Name == asgname).Select(a => a.Content).First().ToString();
            return Content(submission);
        }


        /// <summary>
        /// Gets information about a user as a single JSON object.
        /// The object should have the following fields:
        /// "fname": the user's first name
        /// "lname": the user's last name
        /// "uid": the user's uid
        /// "department": (professors and students only) the name (such as "Computer Science") of the department for the user. 
        ///               If the user is a Professor, this is the department they work in.
        ///               If the user is a Student, this is the department they major in.    
        ///               If the user is an Administrator, this field is not present in the returned JSON
        /// </summary>
        /// <param name="uid">The ID of the user</param>
        /// <returns>
        /// The user JSON object 
        /// or an object containing {success: false} if the user doesn't exist
        /// </returns>
        public IActionResult GetUser(string uid)
        {
            var user = myDbContext.Users.FirstOrDefault(u => u.Uid == uid.FromDisplayId());
            if (user == null)
            {
                return Json(new { success = false });
            }

            string departmentName = null;
            if (user.Student != null)
            {
                departmentName = user.Student.MajordepNavigation.Name;
            }
            else if (user.Professor != null)
            {
                departmentName = user.Professor.EmployerdepNavigation.Name; 
            }

            if (departmentName != null)
            {
                return Json(new
                {
                    fname = user.Firstname,
                    lname = user.Lastname,
                    uid = user.Uid.ToDisplayId(),
                    department = departmentName
                });
            }
            else
            {
                return Json(new
                {
                    fname = user.Firstname,
                    lname = user.Lastname,
                    uid = user.Uid.ToDisplayId()
                });
            }
        }


        /*******End code to modify********/
    }
}
