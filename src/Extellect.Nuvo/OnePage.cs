using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections;

namespace Extellect.Nuvo
{
    /// <summary>
    /// Use AsPage when your items represent the subset of a collection that
    /// are ready to be placed single page. Use the overload with OnePageOptions
    /// to control the PageIndex and PageSize of the result. Note that any 
    /// pages created with this overload can contain inconsistent property 
    /// values (e.g. a collection of 5 items can be returned as a page with a 
    /// pagesize of 4). Furthermore, the item count should always represent 
    /// the total number of items and not the number of items on the page 
    /// (which can readily be found by examining the count of items).
    /// 
    /// Use TakePage when your items represent a full collection and need to
    /// be reduced to a single page by skipping and taking.
    /// </summary>
    public static class OnePage
    {
        public static OnePage<T> Create<T>(List<T> items)
        {
            return new OnePage<T>
            (
                0,
                items.Count,
                items,
                items.Count
            );
        }

        public static OnePage<T> Create<T>(IEnumerable<T> items)
        {
            return Create(items.ToList());
        }

        public static OnePage<T> Empty<T>()
        {
            return Create(new List<T>(0));
        }

        public static OnePage<T> AsPage<T>(this List<T> items)
        {
            return Create(items);
        }

        public static OnePage<T> AsPage<T>(this IEnumerable<T> items)
        {
            return Create(items);
        }

        /// <summary>
        /// Warning: don't use this method unless you really don't know how many items there are.
        /// </summary>
        public static OnePage<T> AsPage<T>(this IEnumerable<T> items, OnePageOptions options)
        {
            return options == null ? items.AsPage() : items.AsPage(options, -1);
        }

        /// <summary>
        /// Warning: don't use this method unless you really don't know how many items there are.
        /// </summary>
        public static OnePage<T> AsPage<T>(this List<T> items, OnePageOptions options)
        {
            return options == null ? items.AsPage() : items.AsPage(options, -1);
        }

        public static OnePage<T> AsPage<T>(this IEnumerable<T> items, OnePageOptions options, int itemCount)
        {
            return items.ToList().AsPage(options, itemCount);
        }

        public static OnePage<T> AsPage<T>(this List<T> items, OnePageOptions options, int itemCount)
        {
            return new OnePage<T>
            (
                options.PageIndex,
                options.PageSize,
                items,
                itemCount
            );
        }

        public static OnePage<T> TakePage<T>(this IEnumerable<T> items, OnePageOptions options)
        {
            if (options == null)
                options = OnePageOptions.Unlimited;
            return new OnePage<T>
            (
                options.PageIndex,
                options.PageSize,
                items.Skip(options.SkipCount).Take(options.TakeCount).ToList(),
                items.Count()
            );
        }

        public static OnePage<T> TakePage<T>(this IEnumerable<T> items, int pageIndex, int pageSize)
        {
            return items.TakePage(new OnePageOptions(pageIndex, pageSize));
        }
    }

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class OnePage<T> : IEnumerable<T>, IEquatable<OnePage<T>>
    {
        [JsonProperty]
        public int PageIndex { get; }
        [JsonProperty]
        public int PageSize { get; }
        [JsonProperty]
        public List<T> Items { get; }
        [JsonProperty]
        public int ItemCount { get; }

        public OnePage(int pageIndex, int pageSize, List<T> items, int itemCount)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            PageIndex = pageIndex;
            PageSize = pageSize;
            Items = items;
            ItemCount = itemCount;
        }

        public override int GetHashCode()
        {
            var hashCode = 1093083894 ^ PageIndex ^ PageSize ^ ItemCount;
            if (Items != null)
                hashCode ^= Items.GetHashCode();
            return hashCode;
        }

        public override bool Equals(object? obj)
        {
            return obj is OnePage<T> other 
                && Equals(other);
        }

        public bool Equals(OnePage<T>? other)
        {
            return other != null
                && PageIndex == other.PageIndex
                && PageSize == other.PageSize
                && ItemCount == other.ItemCount
                && ((Items == null && other.Items == null) || (Items != null && other.Items != null && Items.SequenceEqual(other.Items)));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items?.GetEnumerator() ?? Enumerable.Empty<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public OnePage<TResult> SelectPage<TResult>(Func<T, TResult> getResult)
        {
            return new OnePage<TResult>
            (
                PageIndex,
                PageSize,
                Items.Select(getResult).ToList(),
                ItemCount
            );
        }

        public OnePage<TResult> SelectPage<TResult>(Func<T, int, TResult> getResult)
        {
            return new OnePage<TResult>
            (
                PageIndex,
                PageSize,
                Items.Select(getResult).ToList(),
                ItemCount
            );
        }
    }
}
