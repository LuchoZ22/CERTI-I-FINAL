using Microsoft.AspNetCore.Mvc;
using UPB.BussinessLogic.Managers;
using UPB.BussinessLogic.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TercerParcial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientCodeController : ControllerBase
    {

        private StudentCodeManager _studentCodeManager;


        public PatientCodeController(StudentCodeManager s)
        {
            _studentCodeManager = s;
        }


        // GET: api/<PatientCodeController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _studentCodeManager.GetPatientsCodesAsync().Result;
        }

        // GET api/<PatientCodeController>/5
        [HttpGet("{ci}")]
        public string Get(string ci)
        {
            return _studentCodeManager.GetPatientCodeAsync(ci).Result;
        }

        
    }
}
