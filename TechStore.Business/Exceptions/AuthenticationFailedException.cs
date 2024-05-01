using System;

namespace TechStore.Business.Exceptions;

public class AuthenticationFailedException : Exception
{
    public AuthenticationFailedException(string email) : base(
        $"Login failed for user {email}")
    {
    }
}