using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SIAWeb2.Models;

namespace SIAWeb2.Controllers
{
    [RoutePrefix("Employee")]
    public class EmployeesController : ApiController
    {
      
        private UACEntities db = new UACEntities();

        // GET: api/Employees
        [Route("GetEmployees")]
        public IQueryable<Employee> GetEmployee()
        {
            return db.Employee;
        }

        [Route("Find/{keyword}")]
        public IHttpActionResult GetEmployee(string keyword)
        {
            var employees = db.Employee.Where(m => m.LastName.Contains(keyword) || m.FirstName.Contains(keyword));
            return Ok(employees);
        }

        [Route("Find/{keyword}/LastName")]
        public IHttpActionResult GetEmployeeByLastName(string keyword)
        {
            var employees = db.Employee.Where(m => m.LastName.Contains(keyword));
            return Ok(employees);
        }

        [Route("Find/{mm:int:min(1):max(12)}/{dd:int:min(1):max(31)}/{yyyy:int:min(1):max(9999)}/Birthday")]
        public IHttpActionResult GetEmployeeByBirthday(int mm, int dd, int yyyy)
        {
            DateTime birthday = new DateTime(yyyy, mm, dd);
            var employees = db.Employee.Where(m => m.Birthday == birthday);
            return Ok(employees);
        }

        // GET: api/Employees/5
        [Route("GetEmployee/{id:int}")]
        [Route("{id}")]
        public IHttpActionResult GetEmployee(int id)
        {
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // PUT: api/Employees/5
        [Route("Edit/{id}")]
        [Route("{id}")]
        public IHttpActionResult PutEmployee(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.Id)
            {
                return BadRequest();
            }

            db.Entry(employee).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Employees
        [Route("")]
        [Route("Add")]
        public IHttpActionResult PostEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Employee.Add(employee);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [Route("{id}")]
        [Route("Delete/{id}")]
        public IHttpActionResult DeleteEmployee(int id)
        {
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            db.Employee.Remove(employee);
            db.SaveChanges();

            return Ok(employee);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeExists(int id)
        {
            return db.Employee.Count(e => e.Id == id) > 0;
        }
    }
}