
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.DTOs;
using TaskManager.Api.Services;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _svc;
    private readonly IAuthService _auth;
    public TasksController(ITaskService svc, IAuthService auth) { _svc = svc; _auth = auth; }

    private async Task<Guid> CurrentUserId() => await _auth.GetUserIdFromClaims(User);

    [HttpGet]
    public async Task<IActionResult> List([FromQuery]int page=1, [FromQuery]int pageSize=20, [FromQuery]string? status=null)
    {
        var res = await _svc.GetAsync(await CurrentUserId(), page, pageSize, status);
        return Ok(res);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var res = await _svc.GetByIdAsync(await CurrentUserId(), id);
        return res is null ? NotFound() : Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskCreateDto dto)
    {
        var res = await _svc.CreateAsync(await CurrentUserId(), dto);
        return CreatedAtAction(nameof(Get), new { id = res.Id }, res);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, TaskUpdateDto dto)
    {
        var res = await _svc.UpdateAsync(await CurrentUserId(), id, dto);
        return Ok(res);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _svc.DeleteAsync(await CurrentUserId(), id);
        return NoContent();
    }
}
