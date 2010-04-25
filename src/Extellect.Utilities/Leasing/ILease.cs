using System;

namespace Extellect.Utilities.Leasing
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILease
    {
        bool IsExpired { get; }
        void Renew(TimeSpan amount);
    }
}
