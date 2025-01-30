using System;

namespace AvaloniaApplication1.model;

public class categoriaInvalidaException : Exception
{
    public categoriaInvalidaException()
    {
    }

    public categoriaInvalidaException(string? message) : base(message)
    {
    }

    public categoriaInvalidaException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}