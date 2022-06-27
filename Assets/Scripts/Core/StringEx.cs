using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.jbg.core
{
    public static class StringEx
    {
        public static string FormatJson(this string str)
        {
            int indent = 0;
            bool quoted = false;
            StringBuilder sb = new();
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        if (quoted == false)
                        {
                            sb.AppendLine();
                            IEnumerable<int> tabCount = Enumerable.Range(0, ++indent);
                            foreach (int item in tabCount)
                                sb.Append("    ");
                        }
                        break;
                    case '}':
                    case ']':
                        if (quoted == false)
                        {
                            sb.AppendLine();
                            IEnumerable<int> tabCount = Enumerable.Range(0, --indent);
                            foreach (int item in tabCount)
                                sb.Append("    ");
                        }
                        sb.Append(ch);
                        break;
                    case '"':
                        sb.Append(ch);
                        bool escaped = false;
                        int index = i;
                        while (index > 0 && str[--index] == '\\')
                            escaped = !escaped;
                        if (escaped == false)
                            quoted = !quoted;
                        break;
                    case ',':
                        sb.Append(ch);
                        if (quoted == false)
                        {
                            sb.AppendLine();
                            IEnumerable<int> tabCount = Enumerable.Range(0, indent);
                            foreach (int item in tabCount)
                                sb.Append("    ");
                        }
                        break;
                    case ':':
                        sb.Append(ch);
                        if (quoted == false)
                            sb.Append(" ");
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }
    }
}
