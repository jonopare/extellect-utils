using System.Collections.Generic;

namespace Extellect.Collections
{
    /// <summary>
    /// Represents an immutable binary tree.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ImmutableBinaryTree<T>
    {
        /// <summary>
        /// Gets the value of the current node.
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Gets the left branch of the tree.
        /// </summary>
        public ImmutableBinaryTree<T> Left { get; private set; }

        /// <summary>
        /// Gets the right branch of the tree
        /// </summary>
        public ImmutableBinaryTree<T> Right { get; private set; }

        /// <summary>
        /// Constructs a new binary tree with the specified value, and the specified
        /// left and right branches.
        /// </summary>
        public ImmutableBinaryTree(T value, ImmutableBinaryTree<T> left, ImmutableBinaryTree<T> right)
        {
            Value = value;
            Left = left;
            Right = right;
        }

        /// <summary>
        /// Gets the number of items in the tree.
        /// </summary>
        public int Count
        {
            get
            {
                return 1 
                    + (Left != null ? Left.Count : 0) 
                    + (Right != null ? Right.Count : 0);
            }
        }

        /// <summary>
        /// Iterates over all items in the tree in order from left to right.
        /// </summary>
        public IEnumerable<T> Items
        {
            get
            {
                if (Left != null)
                {
                    foreach (T item in Left.Items)
                    {
                        yield return item;
                    }
                }
                yield return Value;
                if (Right != null)
                {
                    foreach (T item in Right.Items)
                    {
                        yield return item;
                    }
                }
            }
        }

        /// <summary>
        /// Creates a new balanced tree from a list of items.
        /// </summary>
        public static ImmutableBinaryTree<T> CreateBalancedFrom(List<T> items)
        {
            return CreateBalancedFrom(items, 0, items.Count - 1);
        }

        /// <summary>
        /// Creates a new balanced subtree from a list of items.
        /// </summary>
        private static ImmutableBinaryTree<T> CreateBalancedFrom(List<T> items, int left, int right)
        {
            if (left > right)
            {
                return null;
            }
            if (left == right)
            {
                return new ImmutableBinaryTree<T>(items[left], null, null);
            }
            else
            {
                int mid = left + (right - left) / 2;
                return new ImmutableBinaryTree<T>(items[mid], CreateBalancedFrom(items, left, mid - 1), CreateBalancedFrom(items, mid + 1, right));
            }
        }
    }
}