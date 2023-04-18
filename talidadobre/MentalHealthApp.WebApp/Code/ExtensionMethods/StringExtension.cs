namespace MentalHealthApp.WebApp.Code.ExtensionMethods
{
    public static class StringExtension
    {
        private const string DIACRITICE = "ăâîșț";
        private const string NORMALIZATE = "aaist";
        public static string NormalizeString(this string str) =>
            str
                .Select(chr => DIACRITICE.Contains(chr) ? NORMALIZATE.ElementAt(DIACRITICE.IndexOf(chr)) : chr)
                .Aggregate(string.Empty, (acc, next) => $"{acc}{next}");

    }
}
