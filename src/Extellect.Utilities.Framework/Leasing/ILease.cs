#pragma warning disable 1591
using System;

namespace Extellect.Leasing
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
