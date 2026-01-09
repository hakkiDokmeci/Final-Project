namespace TestFinal.Dtos;

public class ExamCreateDto
{
    public string ExamName { get; set; } = default!;
    public string Room { get; set; } = default!;
    public DateOnly ExamDate { get; set; }
    public TimeOnly ExamTime { get; set; }
    public int SeatRows { get; set; }
    public int SeatCols { get; set; }
}
