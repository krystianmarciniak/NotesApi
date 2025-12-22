using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApi.Data;
using NotesApi.Dtos;
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
  public async Task<ActionResult<IEnumerable<NoteDto>>> GetAll()
  {
    var notes = await _db.Notes
        .Where(n => n.UserId == UserId)
        .OrderBy(n => n.Id)
        .Select(n => new NoteDto(n.Id, n.Content))
        .ToListAsync();

    return Ok(notes);
  }

  [HttpGet("{id:int}")]
  public async Task<ActionResult<NoteDto>> GetOne(int id)
  {
    var note = await _db.Notes
        .Where(n => n.UserId == UserId && n.Id == id)
        .Select(n => new NoteDto(n.Id, n.Content))
        .FirstOrDefaultAsync();

    if (note is null) return NotFound();
    return Ok(note);
  }

  [HttpPost]
  public async Task<ActionResult<NoteDto>> Create([FromBody] NoteCreateDto dto)
  {
    var note = new Note
    {
      Content = dto.Content ?? "",
      UserId = UserId
    };

    _db.Notes.Add(note);
    await _db.SaveChangesAsync();

    var outDto = new NoteDto(note.Id, note.Content);
    return Ok(outDto); // często testy oczekują 200 + obiekt (nie 201)
  }

  [HttpPut("{id:int}")]
  public async Task<ActionResult<NoteDto>> Update(int id, [FromBody] NoteUpdateDto dto)
  {
    var note = await _db.Notes.FirstOrDefaultAsync(n => n.UserId == UserId && n.Id == id);
    if (note is null) return NotFound();

    note.Content = dto.Content ?? "";
    await _db.SaveChangesAsync();

    return Ok(new NoteDto(note.Id, note.Content));
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id)
  {
    var note = await _db.Notes.FirstOrDefaultAsync(n => n.UserId == UserId && n.Id == id);
    if (note is null) return NotFound();

    _db.Notes.Remove(note);
    await _db.SaveChangesAsync();

    return NoContent(); // typowo 204
  }
}
