using System;

namespace AvaloniaApplication1.model;

public class nombreInvalidoException : Exception
{
    public nombreInvalidoException()
    {
    }

    public nombreInvalidoException(string? message) : base(message)
    {
    }

    public nombreInvalidoException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}