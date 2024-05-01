using System;

namespace TechStore.Business.Exceptions;

public class UnknownModelException : Exception
{
    public UnknownModelException(string model, string key, object value)
        : base($"{model} with {key} \"{value}\" not found.")
    {
    }
}