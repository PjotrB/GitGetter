namespace GitGetter.GitExe.Helpers
{
    internal static class Extensions
    {
        internal static bool IsNullOrWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        internal static bool HasValue(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }
    }
}