namespace TestFinal.Services;

public class FakeFaceMatchService : IFaceMatchService
{
    public FaceMatchResult Evaluate(string? faceTemplateRef, string? capturedImageRef)
    {
        if (string.IsNullOrWhiteSpace(faceTemplateRef) || string.IsNullOrWhiteSpace(capturedImageRef))
            return new FaceMatchResult(false, 0);

        bool match = string.Equals(faceTemplateRef.Trim(), capturedImageRef.Trim(), StringComparison.OrdinalIgnoreCase);
        return match ? new FaceMatchResult(true, 98) : new FaceMatchResult(false, 30);
    }
}
