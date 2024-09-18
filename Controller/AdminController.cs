using companyappbasic.Common.Extensions;
using companyappbasic.Data.Context;
using companyappbasic.Data.Models;
using companyappbasic.Services.AdminServices;
using companyappbasic.Services.AppUserServices;
using companyappbasic.Services.EmployeeServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace companyappbasic.Controller
{
    [Route("Api/Admin")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : ControllerBase
    {

        private readonly ApplicationDBContext _context;
        private readonly IAdmin _adminRepo;

        public AdminController(ApplicationDBContext context,IAdmin adminRepo)
        {
            _context = context;
            _adminRepo = adminRepo;
            
        }

        [HttpGet]
        public async Task<IActionResult> GetALL()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var admin = await _adminRepo.GetAllAsync();
            var adminDto = admin.Select(s => s.ToAdminDto());
            return Ok(admin);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var admin = await _adminRepo.GetByIdAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            return Ok(admin.ToAdminDto());
        }

        [HttpPost("{id:int}")]
        public async Task<IActionResult> Createid([FromRoute] int id, [FromBody] CreateAdminRequestDto createAdminDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var adminModel = createAdminDto.ToAdminFromCreatedDTO(id);
            await _adminRepo.CreateAsync(adminModel);
            return CreatedAtAction(nameof(GetById), new { id = adminModel.Id }, adminModel.ToAdminDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateAdminRequestDto updateAdminDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var adminModel = await _adminRepo.UpdateAsync(id, updateAdminDto);
            if (adminModel == null)
            {
                return NotFound("Admin Bulunamadı");
            }
            return Ok(adminModel.ToAdminDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var adminModel = await _adminRepo.DeleteAsync(id);
            if (adminModel == null)
            {
                return NotFound("Yorum Bulunamadı");
            }
            return NoContent();
        }
    }
}
