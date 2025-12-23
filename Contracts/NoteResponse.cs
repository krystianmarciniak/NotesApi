namespace NotesApi.Dtos;

public record NoteDto(int Id, string Content);
public record CreateNoteDto(string Content);
public record UpdateNoteDto(string Content);
