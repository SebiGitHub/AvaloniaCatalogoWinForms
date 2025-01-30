using System;

namespace AvaloniaApplication1.model;

public class descInvalidaException : Exception
{
    public descInvalidaException()
    {
    }

    public descInvalidaException(string? message) : base(message)
    {
    }

    public descInvalidaException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}