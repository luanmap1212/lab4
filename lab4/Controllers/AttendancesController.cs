using lab4.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace lab4.Controllers
{
    public class AttendancesController : ApiController
    {
        //[HttpPost]
        public IHttpActionResult Attend(Course attendanceDto)
        {
            var userID = User.Identity.GetUserId();
            BigSchoolContext context = new BigSchoolContext();
            if (context.Attendances.Any(p => p.Attendee == userID && p.CourseId == attendanceDto.Id))
            {
                return BadRequest("The attendace already exists!");
            }
            var attendace = new Attendance()
            {
                CourseId = attendanceDto.Id, Attendee = User.Identity.GetUserId()
            };
            context.Attendances.Add(attendace);
            context.SaveChanges();
            return Ok();
        }
    }
}
