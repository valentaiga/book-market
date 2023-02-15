#nullable enable
using System.Data.Common;

namespace BookMarket.Tests.Abstractions;

internal class InternalDbException : DbException
{
    public InternalDbException(string? message) : base(message)
    {
    }
}