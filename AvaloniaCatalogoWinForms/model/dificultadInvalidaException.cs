using System;

namespace AvaloniaApplication1.model;

public class dificultadInvalidaException : Exception
{
    public dificultadInvalidaException()
    {
    }

    public dificultadInvalidaException(string? message) : base(message)
    {
    }

    public dificultadInvalidaException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}