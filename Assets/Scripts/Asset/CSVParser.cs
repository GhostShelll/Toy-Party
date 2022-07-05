using System.IO;
using System.Text;

using ReadWriteCsv;

using com.jbg.core;

namespace com.jbg.asset
{
    public static class CSVParser
    {
        public class Info
        {
            public enum TYPE
            {
                Plain,
                String,
                ArrayOpen,
                ArrayClose,
                ObjectOpen,
                ObjectClose,
            }

            public string key;
            public TYPE type;

            public Info(string key, TYPE type)
            {
                this.key = key;
                this.type = type;
            }
        }

        public static string CsvToJson(this string thiz, Info[] headers, bool useIndent = false, int skipRows = 0, int skipColumns = 0)
        {
            if (thiz == null)
                return null;

            StringBuilder result = new(thiz.Length);
            result.Append('[');

            int indent = 0;
            if (useIndent)
            {
                result.Append('\n');
                ++indent;
            }

            using (MemoryStream stream = new())
            {
                using (StreamWriter writer = new(stream))
                {
                    writer.Write(thiz);
                    writer.Flush();
                    stream.Position = 0;

                    using (CsvFileReader reader = new(stream))
                    {
                        CsvRow row = new();

                        for (int n = 0; n < skipRows; ++n)
                            reader.ReadRow(row);

                        if (null == headers)
                        {
                            reader.ReadRow(row);
                            string[] stringHeaders = row.ToArray();
                            headers = new Info[headers.Length];
                            for (int n = 0; n < stringHeaders.Length; ++n)
                                headers[n] = new Info(stringHeaders[n], Info.TYPE.String);
                        }

                        while (reader.ReadRow(row))
                        {
                            if (useIndent)
                            {
                                for (int i = 0; i < indent; i++)
                                    result.Append("    ");
                            }

                            result.Append('{');

                            if (useIndent)
                            {
                                result.Append('\n');
                                ++indent;
                            }


                            int dataColumn = skipColumns;
                            for (int n = 0; n < headers.Length; ++n)
                            {
                                switch (headers[n].type)
                                {
                                    case Info.TYPE.Plain:
                                    case Info.TYPE.String:
                                    case Info.TYPE.ArrayOpen:
                                    case Info.TYPE.ObjectOpen:
                                        {
                                            if (Info.TYPE.Plain == headers[n].type || Info.TYPE.String == headers[n].type)
                                            {
                                                if (string.IsNullOrEmpty(row[dataColumn]))
                                                {
                                                    ++dataColumn;
                                                    continue;
                                                }
                                            }

                                            if (useIndent)
                                            {
                                                for (int i = 0; i < indent; i++)
                                                    result.Append("    ");
                                            }

                                            if (!string.IsNullOrEmpty(headers[n].key))
                                                result.Append('\"').Append(headers[n].key).Append('\"').Append(':').Append(' ');

                                            switch (headers[n].type)
                                            {
                                                case Info.TYPE.Plain:
                                                    if (row.Count <= dataColumn)
                                                        throw new System.Exception("Field count must match header count. row.Count:" + row.Count + ", dataColumn:" + dataColumn);
                                                    result.Append(row[dataColumn++]).Append(',');
                                                    break;

                                                case Info.TYPE.String:
                                                    if (row.Count <= dataColumn)
                                                        throw new System.Exception("Field count must match header count. row.Count:" + row.Count + ", dataColumn:" + dataColumn);
                                                    result.Append('\"').Append(row[dataColumn++]).Append('\"').Append(',');
                                                    break;

                                                case Info.TYPE.ArrayOpen:
                                                    if (useIndent)
                                                    {
                                                        result.Append('\n');
                                                        for (int i = 0; i < indent; i++)
                                                            result.Append("    ");
                                                    }
                                                    result.Append('[');
                                                    break;

                                                case Info.TYPE.ObjectOpen:
                                                    if (useIndent)
                                                    {
                                                        result.Append('\n');
                                                        for (int i = 0; i < indent; i++)
                                                            result.Append("    ");
                                                    }
                                                    result.Append('{');
                                                    break;
                                            }

                                            if (useIndent)
                                            {
                                                result.Append('\n');

                                                switch (headers[n].type)
                                                {
                                                    case Info.TYPE.ArrayOpen:
                                                    case Info.TYPE.ObjectOpen:
                                                        ++indent;
                                                        break;
                                                }
                                            }

                                            break;
                                        }

                                    case Info.TYPE.ArrayClose:
                                    case Info.TYPE.ObjectClose:
                                        {
                                            if (useIndent)
                                            {
                                                --indent;
                                                for (int i = 0; i < indent; i++)
                                                    result.Append("    ");
                                            }

                                            switch (headers[n].type)
                                            {
                                                case Info.TYPE.ArrayClose:
                                                    result.Append(']').Append(',');
                                                    break;

                                                case Info.TYPE.ObjectClose:
                                                    result.Append('}').Append(',');
                                                    break;
                                            }

                                            if (useIndent)
                                            {
                                                result.Append('\n');
                                            }

                                            break;
                                        }
                                }
                            }

                            if (useIndent)
                            {
                                --indent;
                                for (int i = 0; i < indent; i++)
                                    result.Append("    ");
                            }

                            result.Append('}').Append(',');

                            if (useIndent)
                            {
                                result.Append('\n');
                            }
                        }
                    }
                }
            }

            result.Append(']');

            if (useIndent)
            {
                result.Append('\n');
            }


            return result.ToString();
        }
    }
}
