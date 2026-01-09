namespace TestFinal.Services;

public record FaceMatchResult(bool Match, double Score);

public interface IFaceMatchService
{
    FaceMatchResult Evaluate(string? faceTemplateRef, string? capturedImageRef);
}
