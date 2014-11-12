using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTS
{
  internal class IndentedStringBuilder
  {
    private readonly StringBuilder _sb;
    private int _indentation;

    public IndentedStringBuilder(int capacity)
    {
      _sb = new StringBuilder(capacity);
      _indentation = 0;
    }

    public void IncreaseIndentation()
    {
      _indentation++;
    }

    public void DecreaseIndentation()
    {
      _indentation = Math.Max(0, _indentation - 1);
    }

    private void AppendWithIndentation(string template, params object[] args)
    {
      _sb.AppendLine(new string('\t', _indentation) + string.Format(template, args));
    }

    public IndentedStringBuilder AppendLine(string template, params object[] args)
    {
      AppendWithIndentation(template, args);
      return this;
    }

    public override string ToString()
    {
      return _sb.ToString();
    }
  }
}
