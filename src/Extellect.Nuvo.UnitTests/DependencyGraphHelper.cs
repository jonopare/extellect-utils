namespace Extellect.Nuvo
{
    public static class DependencyGraphHelper
    {
        public static DependencyGraph<T> ToDependencyGraph<T>(string text, Func<string, T> create) where T : notnull
        {
            var result = new DependencyGraph<T>();
            foreach (var dependency in ToDependencies(text, create))
            {
                if (dependency.Item1.Equals(dependency.Item2))
                {
                    result.Add(dependency.Item1);
                }
                else
                {
                    result.Add(dependency.Item1, dependency.Item2);
                }
            }
            return result;
        }

        public static DependencyGraph<string> ToDependencyGraph(string text)
        {
            return ToDependencyGraph(text, x => x);
        }

        private static IEnumerable<Tuple<T, T>> ToDependencies<T>(string text, Func<string, T> create)
        {
            return text.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => ToDependency(x, create));
        }

        private static Tuple<T, T> ToDependency<T>(string text, Func<string, T> create)
        {
            var array = text.Split(new[] { "<-" }, StringSplitOptions.RemoveEmptyEntries);
            if (array.Length == 2)
            {
                return Tuple.Create(create(array[0]), create(array[1]));
            }
            else if (array.Length == 1)
            {
                array = text.Split(new[] { "->" }, StringSplitOptions.RemoveEmptyEntries);
                if (array.Length == 2)
                {
                    return Tuple.Create(create(array[1]), create(array[0]));
                }
                else if (array.Length == 1)
                {
                    return Tuple.Create(create(array[0]), create(array[0]));
                }
            }
            throw new NotSupportedException();
        }
    }
}