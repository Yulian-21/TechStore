using System;
using System.Collections.Generic;

namespace TechStore.Business.Exceptions;

public class InvalidModelException : Exception
{
    public InvalidModelException(string error)
    {
        Errors = new Dictionary<string, string[]>
        {
            ["error"] = new[] { error }
        };
    }
    
    public InvalidModelException(IDictionary<string, string[]> errors)
    {
        Errors = errors;
    }

    public IDictionary<string, string[]> Errors { get; private set; }
}