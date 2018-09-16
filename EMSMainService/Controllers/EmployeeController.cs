using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EMSMainService.BusinessEntities;
using System.Web.Http.Cors;
using System.Data.SqlClient;

namespace EMSMainService.Controllers
{
    [EnableCors("*", "*", "*")]
    [Authorize]
    public class EmployeeController : ApiController
    {

        //GET Employees
        [Route("api/employees")]
        [HttpGet]
        public HttpResponseMessage GetEmployees()
        {
            try
            {
                EmployeeDBEntities1 context = new EmployeeDBEntities1();
                var employees = context.Employees.ToList();
                List<EmployeeModel> list = new List<EmployeeModel>();
                foreach (var item in employees)
                {
                    EmployeeModel emp = new EmployeeModel();
                    emp.Id = item.Id;
                    emp.FirstName = item.FirstName;
                    emp.LastName = item.LastName;
                    emp.Email = item.Email;
                    emp.Gender = item.Gender;
                    emp.Country = item.Country.Name;
                    emp.State = item.State.Name;
                    emp.City = item.City.Name;
                    emp.Department = item.Department.Name;
                    emp.DateOfBirth = item.DateOfBirth;
                    emp.DateOfJoining = item.DateOfJoining;
                    emp.IsActive = item.IsActive;
                    emp.Role = item.RoleMaster.Name;
                    emp.Phone = item.Phone;
                    list.Add(emp);
                }
                return Request.CreateResponse((employees != null && employees.Count > 0) ? HttpStatusCode.OK : HttpStatusCode.NoContent, list.OrderByDescending(x => x.Id));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("api/employee/create")]
        [HttpPost]
        public HttpResponseMessage CreateEmployee(Employee employee)
        {
            try
            {
                var connection = new SqlConnection("Data Source=JIGAR-PC;Initial Catalog=EmployeeDb;Integrated Security=True");
                connection.Open();
                var command = new SqlCommand("InsertUpdateEmployee", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", employee.Id);
                command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                command.Parameters.AddWithValue("@LastName", employee.LastName);
                command.Parameters.AddWithValue("@Email", employee.Email);
                command.Parameters.AddWithValue("@Gender", employee.Gender);
                command.Parameters.AddWithValue("@Phone", employee.Phone);
                command.Parameters.AddWithValue("@CountryId", employee.CountryId);
                command.Parameters.AddWithValue("@StateId", employee.StateId);
                command.Parameters.AddWithValue("@CityId", employee.CityId);
                command.Parameters.AddWithValue("@DepartmentId", employee.DepartmentId);
                command.Parameters.AddWithValue("@DateOfBirth", Convert.ToDateTime(employee.DateOfBirth.ToString("yyyy-MM-dd")));
                command.Parameters.AddWithValue("@DateofJoining", Convert.ToDateTime(employee.DateOfJoining.ToString("yyyy-MM-dd")));
                command.Parameters.AddWithValue("@IsActive", employee.IsActive);
                command.Parameters.AddWithValue("@RoleType", employee.RoleType);
                command.ExecuteNonQuery();
                connection.Close();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("api/employee/{id}")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeById(int id)
        {
            try
            {
                EmployeeDBEntities1 context = new EmployeeDBEntities1();
                var employees = context.Employees.Where(emp => emp.Id == id).FirstOrDefault();
                if (employees == null)
                {

                    return Request.CreateResponse(HttpStatusCode.NoContent, string.Format("No Employee with Id = {0} found", id));
                }
                else
                {

                    EmployeeModel employee = new EmployeeModel()
                    {
                        Id = employees.Id,
                        FirstName = employees.FirstName,
                        LastName = employees.LastName,
                        Email = employees.Email,
                        Gender = employees.Gender,
                        Country = employees.Country.Name,
                        CountryId = employees.CountryId,
                        StateId = employees.StateId,
                        CityId = employees.CityId,
                        DepartmentId = employees.DepartmentId,
                        RoleType = employees.RoleType,
                        State = employees.State.Name,
                        City = employees.City.Name,
                        Department = employees.Department.Name,
                        DateOfBirth = employees.DateOfBirth,
                        DateOfJoining = employees.DateOfJoining,
                        IsActive = employees.IsActive,
                        Role = employees.RoleMaster.Name,
                        Phone = employees.Phone
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, employee);
                    //   return Request.CreateResponse(HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [Route("api/employee/countries")]
        [HttpGet]
        public HttpResponseMessage GetCountries()
        {
            try
            {
                EmployeeDBEntities1 context = new EmployeeDBEntities1();
                var countries = context.Countries.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, countries);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("api/employee/departments")]
        [HttpGet]
        public HttpResponseMessage GetDepartments()
        {
            try
            {
                EmployeeDBEntities1 context = new EmployeeDBEntities1();
                var countries = context.Departments.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, countries);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("api/employee/roles")]
        [HttpGet]
        public HttpResponseMessage GetRoles()
        {
            try
            {
                EmployeeDBEntities1 context = new EmployeeDBEntities1();
                var countries = context.RoleMasters.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, countries);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("api/employee/states/{countryId}")]
        [HttpGet]
        public HttpResponseMessage GetStates(int countryId)
        {
            try
            {
                EmployeeDBEntities1 context = new EmployeeDBEntities1();
                var states = context.States.Where(x => x.CountryId == countryId).ToList();
                List<StateModel> lst = new List<StateModel>();
                if (states != null && states.Count > 0)
                {
                    foreach (var item in states)
                    {
                        StateModel sm = new StateModel() { Id = item.Id, Name = item.Name };
                        lst.Add(sm);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, lst);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("api/employee/cities/{stateId}")]
        [HttpGet]
        public HttpResponseMessage GetCities(int stateId)
        {
            try
            {
                EmployeeDBEntities1 context = new EmployeeDBEntities1();
                var cities = context.Cities.Where(x => x.StateId == stateId).ToList();
                List<CityModel> lst = new List<CityModel>();
                if (cities != null && cities.Count > 0)
                {
                    foreach (var item in cities)
                    {
                        CityModel sm = new CityModel() { Id = item.Id, Name = item.Name };
                        lst.Add(sm);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, lst);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }

    public class EmployeeModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int Phone { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Department { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public System.DateTime DateOfJoining { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public int DepartmentId { get; set; }
        public int RoleType { get; set; }
    }

    public class StateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CityModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
