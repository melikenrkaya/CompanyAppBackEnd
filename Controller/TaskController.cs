using companyappbasic.Data.Context;
using companyappbasic.Data.Models;
using companyappbasic.Services.TaskServices;
using Microsoft.AspNetCore.Mvc;
using companyappbasic.Common.Extensions;


namespace companyappbasic.Controller
{
    [Route("Api/TaskClass")]
    [ApiController]

    public class TaskController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly ITask _taskrepo;

        public TaskController(ApplicationDBContext context, ITask taskrepo)
        {
            _context = context;
            _taskrepo = taskrepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetALL()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var taskss = await _taskrepo.GetAllAsync();
            var tasksDto = taskss.Select(s => s.ToTaskDto());
            return Ok(taskss);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var taskss = await _taskrepo.GetByIdAsync(id);
            if (taskss == null)
            {
                return NotFound();
            }
            return Ok(taskss.ToTaskDto());
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequestDto createtaskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tasksModel = createtaskDto.ToTasksFromCreateDTO();
            await _taskrepo.CreateAsync(tasksModel);
            return CreatedAtAction(nameof(GetById), new { id = tasksModel.Id }, tasksModel.ToTaskDto());
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTaskRequestDto updateTaskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tasksModel = await _taskrepo.UpdateAsync(id, updateTaskDto);
            if (tasksModel == null)
            {
                return NotFound();
            }
            return Ok(tasksModel.ToTaskDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tasksModel = await _taskrepo.DeleteAsync(id);
            if (tasksModel == null)
            {
                return NotFound();
            }
            return NoContent();
        }


    }

}
