using System;

namespace Extellect.Utilities.Leasing
{
    public interface ILease
    {
        bool IsExpired { get; }
        void Renew(TimeSpan amount);
    }
}
