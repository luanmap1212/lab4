using lab4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace lab4.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Courses
        public ActionResult Create()
        {
            BigSchoolContext context = new BigSchoolContext();
            Course objCourse = new Course();
            objCourse.ListCategory = context.Categories.ToList();

            return View(objCourse);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course objCourse)
        {
            BigSchoolContext context = new BigSchoolContext();

            ModelState.Remove("LecturerId");
            if (!ModelState.IsValid)
            {
                objCourse.ListCategory = context.Categories.ToList();
                return View("Create", objCourse);
            }

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;

            context.Courses.Add(objCourse);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        
        public ActionResult Attending()
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendances = context.Attendances.Where(p => p.Attendee == currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach (Attendance temp in listAttendances)
            {
                Course objCourse = temp.Course;
                objCourse.LectureName = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                    .FindById(objCourse.LecturerId).Name;
                courses.Add(objCourse);
            }
            return View(courses);
        }

        public ActionResult Mine()
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            var courses = context.Courses.Where(c => c.LecturerId == currentUser.Id && c.Datetime > DateTime.Now).ToList();
            foreach (Course i in courses)
            {
                i.LectureName = currentUser.Name;
            }

            return View(courses);
        }

        public ActionResult DeleteMine(int id)
        {
            BigSchoolContext context = new BigSchoolContext();

            Course deletedCourse = context.Courses.FirstOrDefault(p => p.Id == id);

            return View(deletedCourse);
        }
        [HttpPost]
        public ActionResult DeleteMine(Course x)
        {
            BigSchoolContext context = new BigSchoolContext();

            Attendance deletedAttendance = context.Attendances.FirstOrDefault(p => p.CourseId == x.Id);
            if (deletedAttendance != null)
            {
                context.Attendances.Remove(deletedAttendance);
                context.SaveChanges();
            }

            Course deletedCourse = context.Courses.FirstOrDefault(p => p.Id == x.Id);
            if (deletedCourse != null)
            {
                context.Courses.Remove(deletedCourse);
                context.SaveChanges();
            }

            return RedirectToAction("Mine", "Courses");
        }

        public ActionResult EditMine(int id)
        {

            BigSchoolContext context = new BigSchoolContext();
            Course editCourse = context.Courses.FirstOrDefault(p => p.Id == id);

            if (editCourse != null)
            {
                editCourse.ListCategory = context.Categories.ToList();
            }
            return View(editCourse);
        }
        [HttpPost]
        public ActionResult EditMine([Bind(Include = "Id, Place, Datetime, CategoryId")] Course x)
        {
            BigSchoolContext context = new BigSchoolContext();
            Course editCourse = context.Courses.FirstOrDefault(p => p.Id == x.Id);
            if (editCourse != null)
            {
                editCourse.Place = x.Place;
                editCourse.Datetime = x.Datetime;
                editCourse.CategoryId = x.CategoryId;
                context.SaveChanges();
            }
            return RedirectToAction("Mine", "Courses");
        }

        public ActionResult LectureIamGoing()
        {
            ApplicationUser currentUser =
           System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            //danh sách giảng viên được theo dõi bởi người dùng (đăng nhập) hiện tại
            var listFollwee = context.Followings.Where(p => p.FollowerID ==
            currentUser.Id).ToList();
            //danh sách các khóa học mà người dùng đã đăng ký
            var listAttendances = context.Attendances.Where(p => p.Attendee ==
            currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach (var course in listAttendances)
            {
                foreach (var item in listFollwee)
                {
                    if (item.FolloweeID == course.Course.LecturerId)
                    {
                        Course objCourse = course.Course;
                        objCourse.LectureName =
                       System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objCourse.LecturerId).Name;
                        courses.Add(objCourse);
                    }
                }

            }
            return View(courses);
        }
    }
}