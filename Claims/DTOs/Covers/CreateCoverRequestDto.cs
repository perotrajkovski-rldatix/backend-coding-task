namespace Claims.DTOs.Covers;

public sealed class CreateCoverRequestDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public CoverType Type { get; set; }
}
