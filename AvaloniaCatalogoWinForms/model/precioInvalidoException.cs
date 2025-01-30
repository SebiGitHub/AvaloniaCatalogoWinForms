using System;

namespace AvaloniaApplication1.model;

public class precioInvalidoException : Exception
{
    public precioInvalidoException()
    {
    }

    public precioInvalidoException(string? message) : base(message)
    {
    }

    public precioInvalidoException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}