using System.Text;
using NUglify;

namespace Furesoft.LowCode.Compilation;

public class CodeWriter
{
    private readonly StringBuilder _builder = new();
    private readonly bool _minify;
    private int indentLevel;

    public CodeWriter(bool minify = false)
    {
        _minify = minify;
    }

    public CodeWriter BeginBlock()
    {
        AppendLine("{");
        indentLevel++;
        return this;
    }

    public CodeWriter EndBlock()
    {
        indentLevel--;
        AppendLine("\n}");
        return this;
    }

    public CodeWriter Append(string src, bool indent = true)
    {
        var indentation = new string('\t', indentLevel);

        if (indent)
        {
            _builder.Append($"{indentation}{src}");
        }
        else
        {
            _builder.Append(src);
        }

        return this;
    }

    public CodeWriter Append(char src)
    {
        var indentation = new string('\t', indentLevel);

        _builder.Append($"{indentation}{src}");

        return this;
    }

    public CodeWriter AppendSymbol(char c)
    {
        _builder.Append(c);

        return this;
    }

    public CodeWriter AppendLine(string src)
    {
        return Append($"{src}{Environment.NewLine}");
    }

    public CodeWriter AppendIdentifier(string name)
    {
        if (char.IsDigit(name[0]))
        {
            Append("_");
        }

        foreach (var c in name.Where(
                     c => c is >= '0' and <= '9' or >= 'A' and <= 'Z' or >= 'a' and <= 'z' or '.' or '_'))
        {
            Append(c);
        }

        return this;
    }

    public override string ToString()
    {
        var result = _builder.ToString();

        return _minify ? Uglify.Js(result).Code : result;
    }

    public CodeWriter AppendKeyword(string keyword)
    {
        return Append($"{keyword} ");
    }

    public CodeWriter BeginFunctionDecl(string name)
    {
        return AppendKeyword("function")
            .AppendIdentifier(name)
            .Append("()");
    }

    public CodeWriter AppendCall(string func, params object[] args)
    {
        return AppendIdentifier(func)
            .AppendSymbol('(')
            .Append(string.Join(", ", args).Trim(), false)
            .AppendSymbol(')');
    }

    public CodeWriter Throw(string message)
    {
        return Append(message, false).AppendSymbol(';');
    }
}
