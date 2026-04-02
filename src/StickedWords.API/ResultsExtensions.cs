namespace StickedWords.API;

internal static class ResultsExtensions
{
    private const string Base64Prefix = "data:";

    extension(Results)
    {
        public static IResult FileFromBase64(string base64)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(base64);
            if (!base64.StartsWith(Base64Prefix))
            {
                throw new ArgumentException("Format is not valid", nameof(base64));
            }

            var mimeType = GetMimeType(base64);
            var base64Data = base64.Substring(base64.IndexOf(',') + 1);
            var imageBytes = Convert.FromBase64String(base64Data);

            return Results.File(imageBytes, mimeType);
        }

        private static string GetMimeType(string base64) =>
            base64.Substring(Base64Prefix.Length, base64.IndexOf(';') - Base64Prefix.Length);
    }
}
