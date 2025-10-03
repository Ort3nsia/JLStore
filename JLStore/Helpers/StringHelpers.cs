namespace JLStore.Helpers;

public static class StringHelpers
{
    public static string RemoveVowels(string input)
    {
        var replace = "aeiouAEIOU";
        foreach (char c in replace)
        {
            input = input.Replace(c.ToString(), "");
        }
        return input;
    }
}