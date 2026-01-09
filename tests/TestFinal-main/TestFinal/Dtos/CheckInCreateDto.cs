namespace TestFinal.Dtos;

public class CheckInCreateDto
{
    public int ExamId { get; set; }
    public int StudentId { get; set; }

    public string ObservedSeatCode { get; set; } = default!;
    public string? CapturedImageRef { get; set; }
    public string? Notes { get; set; }
}
