using System.Text;

namespace TestProject.P3DFile.Helpers;

public static class StringHelpers
{
    public static string EscapeString(this string input)
    {
        StringBuilder sb = new StringBuilder();

        foreach (char c in input)
        {
            switch (c)
            {
                case '\0':
                    sb.Append("\\0");
                    break;
                case '\n':
                    sb.Append("\\n");
                    break;
                case '\r':
                    sb.Append("\\r");
                    break;
                case '\t':
                    sb.Append("\\t");
                    break;
                case '\\':
                    sb.Append("\\\\");
                    break;
                default:
                    sb.Append(c);
                    break;
            }
        }

        return sb.ToString();
    }
}
