namespace NotesApi.Models;

public class Note
{
  public int Id { get; set; }
  public string Content { get; set; } = string.Empty;

  // stałe powiązanie z użytkownikiem
  public string UserId { get; set; } = string.Empty;
}
