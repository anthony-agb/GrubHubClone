using System.Text;
using System.Text.RegularExpressions;

namespace GrubHubClone.Common.Utilities;

public static class StringTransformer
{
    /// <summary>
    /// Formats the class to a queue name and removes the last word. Example, "TestClassConsumer" will turn into "test-class".
    /// </summary>
    /// <param name="type">The class type.</param>
    /// <param name="removeLastWord">Wether or not to remove the last word from the string.</param>
    /// <returns>Returns a kebab case formated string.</returns>
    public static string FormatClassToQueueName(Type type, bool removeLastWord)
    {
        var regex = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

        string[] ss = regex
            .Replace(type.Name, " ")
            .Split(" ");


        if (ss.Length == 1)
        {
            return ss[0].ToLower();
        }

        var sb = new StringBuilder();

        int count = removeLastWord ? ss.Length - 1 : ss.Length;

        for (int i = 0; i < count; i++)
        {
            sb.Append(ss[i].ToLower());
            if (i < count - 1)
            {
                sb.Append("-");
            }
        }

        return sb.ToString();
    }
}
