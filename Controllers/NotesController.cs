using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApi.Contracts.Notes;
using NotesApi.Data;
using NotesApi.Models;

namespace NotesApi.Controllers;

[ApiController]
[Route("notes")]
[Authorize]
public class NotesController : ControllerBase
{
  private readonly AppDbContext _db;
  public NotesController(AppDbContext db) => _db = db;

  private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

  [HttpGet]
  public async Task<ActionResult<IEnumerable<NoteResponse>>> List()
  {
    var notes = await _db.Notes
      .Where(n => n.UserId == UserId)
      .OrderBy(n => n.Id)
      .Select(n => new NoteResponse(n.Id, n.Content))
      .ToListAsync();

    return Ok(notes);
  }

  [HttpGet("{id:int}")]
  public async Task<ActionResult<NoteResponse>> GetById(int id)
  {
    var entity = await _db.Notes
      .Where(n => n.UserId == UserId && n.Id == id)
      .Select(n => new NoteResponse(n.Id, n.Content))
      .FirstOrDefaultAsync();

    if (entity is null) return NotFound();
    return Ok(entity);
  }

  [HttpPost]
  public async Task<ActionResult<NoteResponse>> Create([FromBody] CreateNoteRequest dto)
  {
    var entity = new Note
    {
      Content = dto.Content ?? "",
      UserId = UserId
    };

    _db.Notes.Add(entity);
    await _db.SaveChangesAsync();

    return Ok(new NoteResponse(entity.Id, entity.Content));
  }

  [HttpPut("{id:int}")]
  public async Task<ActionResult<NoteResponse>> Update(int id, [FromBody] UpdateNoteRequest dto)
  {
    var entity = await _db.Notes.FirstOrDefaultAsync(n => n.UserId == UserId && n.Id == id);
    if (entity is null) return NotFound();

    entity.Content = dto.Content ?? "";
    await _db.SaveChangesAsync();

    return Ok(new NoteResponse(entity.Id, entity.Content));
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id)
  {
    var entity = await _db.Notes.FirstOrDefaultAsync(n => n.UserId == UserId && n.Id == id);
    if (entity is null) return NotFound();

    _db.Notes.Remove(entity);
    await _db.SaveChangesAsync();

    return NoContent();
  }
}
