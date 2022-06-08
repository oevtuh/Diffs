using System;
using System.Threading.Tasks;
using DiffingAPITask.BusinessLogic.DTO;
using DiffingAPITask.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiffingAPITask.API.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/diff")]
    [ApiVersion("1.0")]
    public class DiffController : ControllerBase
    {
        private readonly IDiffService _diffService;
        
        public DiffController(IDiffService diffService)
        {
            _diffService = diffService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DiffResultDTO>> GetDiffResult(int id)
        {
            var result = await _diffService.GetDiffResult(id);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        
        [HttpPut("{id:int}/right")]
        public async Task<IActionResult> SetRight(int id, DataPartDTO rightData)
        {
            if (rightData.Data == null)
            {
                return BadRequest();
            }

            try
            {
                await _diffService.SetRight(id, rightData.Data);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            return StatusCode(201);
        }
        
        [HttpPut("{id:int}/left")]
        public async Task<IActionResult> SetLeft(int id, DataPartDTO leftData)
        {
            if (leftData.Data == null)
            {
                return BadRequest();
            }

            try
            {
                await _diffService.SetLeft(id, leftData.Data);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            return StatusCode(201);
        }
    }
}