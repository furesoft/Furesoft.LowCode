using NiL.JS.Core.Interop;

namespace Furesoft.LowCode;

public class Result(bool isSuccess, object value)
{
    public bool IsSuccess { get; set; } = isSuccess;
    public object Value { get; set; } = value;

    [JavaScriptName("ok")]
    public static Result Ok(object result)
    {
        return new(true, result);
    }

    [JavaScriptName("fail")]
    public static Result Failure(object error)
    {
        return new(false, error);
    }

    public static Result Failure<T>(string message)
        where T : Exception
    {
        return Failure(Activator.CreateInstance(typeof(T), message));
    }
}
