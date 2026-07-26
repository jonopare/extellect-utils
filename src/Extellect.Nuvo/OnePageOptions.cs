using System.Runtime.CompilerServices;

namespace Extellect.Nuvo
{
    public class OnePageOptions
    {
        public int PageIndex { get; }
        public int PageSize { get; }
        public bool IncludeItemCount { get; }

        public OnePageOptions(int pageIndex, int pageSize, bool includeItemCount = true)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            IncludeItemCount = includeItemCount;
        }

        public int SkipCount => IsUnlimited() ? 0 : checked(PageSize * NonNegativePageIndex());

        public int TakeCount => IsUnlimited() ? int.MaxValue : PageSize;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsUnlimited() => PageSize < 0 || PageSize == int.MaxValue;

        /// <summary>
        /// Yeah, sometimes it might be nice to use negative paging to work backwards
        /// from the end but it's not possible without first knowing the number of items
        /// and that changes this from a nice simple object
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int NonNegativePageIndex() => PageIndex < 0 ? 0 : PageIndex;

        /// <summary>
        /// Returns the page options to fetch one page with all items.
        /// </summary>
        public static OnePageOptions Unlimited { get; } = new OnePageOptions(0, int.MaxValue, includeItemCount: false);

        /// <summary>
        /// Returns the page options to fetch a page with a single item.
        /// </summary>
        public static OnePageOptions Single { get; } = new OnePageOptions(0, 1, includeItemCount: false);

        /// <summary>
        /// Counts the number of pages required to cover the specified number of items.
        /// </summary>
        public int PageCount(int itemCount)
        {
            if (itemCount < -1)
            {
                throw new ArgumentException($"{nameof(itemCount)} cannot be negative.");
            }
            else if (itemCount == -1 || itemCount == 0)
            {
                return itemCount; // itemCount of -1 is used to indicate it's unknown
            }
            else
            {
                var pageSize = IsUnlimited() ? int.MaxValue : PageSize;
                return itemCount / pageSize + (itemCount % pageSize == 0 ? 0 : 1);
            }
        }
    }
}
