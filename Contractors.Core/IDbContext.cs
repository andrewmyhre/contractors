using System;

namespace Contractors.Core
{
    public interface IDbContext : IDisposable
    {
        IDbSession OpenSession();
    }
}