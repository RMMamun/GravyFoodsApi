using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidRepository.TaskManager;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs.TaskManager;
using GravyFoodsApi.Models.TaskManager;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers.TaskManager
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskInfoController : Controller
    {
        private readonly ITaskInfoRepository _repo;

        public TaskInfoController(ITaskInfoRepository repository)
        {
            _repo = repository;
        }

        //implement all methods
        //Task<List<TaskInfo>> GetAll();
        //Task<TaskInfo> GetById(int id);
        //Task<bool> Create(TaskInfo taskInfo);
        //Task<bool> Update(TaskInfo taskInfo);
        //Task<bool> Delete(int id);

        [HttpGet("GetAllTasksAsync")]
        public async Task<ActionResult<TaskInfoDto>> GetAll()
        {
            var product = await _repo.GetAll();
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskInfoDto>> Get(int id)
        {
            var product = await _repo.GetById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TaskInfoDto _dto)
        {
            if (_dto == null)
                return BadRequest("Task log data is required.");

            var created = await _repo.Create(_dto);
            if (created) return Ok();
            return BadRequest();
        }

        [HttpPost("TaskLog")]
        public async Task<ActionResult> CreateTaskLog([FromBody] TasksLogDto _dto)
        {
            if (_dto == null)
                return BadRequest("Task log data is required.");

            var created = await _repo.CreateTaskLogAsync(_dto);
            if (created) return Ok();
            return BadRequest();
        }


        [HttpPost("copyTask")]
        public async Task<ActionResult> CopyTask([FromBody] TaskInfoDto _dto)
        {
            var created = await _repo.CopyTask(_dto);
            if (created) return Ok();
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] TaskInfoDto _dto)
        {
            if (id != _dto.Id) return BadRequest();
            var updated = await _repo.Update(_dto);
            if (updated) return Ok();
            return BadRequest();
        }


    }
}
