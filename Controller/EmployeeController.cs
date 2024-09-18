using companyappbasic.Common.Extensions;
using companyappbasic.Data.Context;
using companyappbasic.Data.Models;
using companyappbasic.Services.EmployeeServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace companyappbasic.Controller
{
    [Route("Api/Employees")]
    [ApiController]
    [Authorize(Policy = "EmployeeOnly")]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IEmployee _employeesServices;

        public EmployeeController(ApplicationDBContext context, IEmployee employeesServices)
        {
            _employeesServices = employeesServices;
            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult> GetALL()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employeess = await _employeesServices.GetAllAsync();
            var employeesDto = employeess.Select(s => s.ToEmployeesDto());
            return Ok(employeess);
        }


        [HttpGet("{id:int}")]
     
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employeess = await _employeesServices.GetByIdAsync(id);
            if (employeess == null)
            {
                return NotFound();
            }
            return Ok(employeess.ToEmployeesDto());
        }

        [HttpPost]
   
        public async Task<IActionResult> Create([FromBody] CreateEmployeesRequestDto employessDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employeesModel = employessDto.ToEmployeesFromCreateDTO();
            await _employeesServices.CreateAsync(employeesModel);
            return CreatedAtAction(nameof(GetById), new { id = employeesModel.Id }, employeesModel.ToEmployeesDto());
        }

        [HttpPut]
        [Route("{id:int}")]

        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateEmployeesRequestDto updateEmployeesDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employeesModel = await _employeesServices.UpdateAsync(id, updateEmployeesDto);
            if (employeesModel == null)
            {
                return NotFound();
            }
            return Ok(employeesModel.ToEmployeesDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
   
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employeesModel = await _employeesServices.DeleteAsync(id);
            if (employeesModel == null)
            {
                return NotFound();
            }
            return NoContent();
        }


    }
}
