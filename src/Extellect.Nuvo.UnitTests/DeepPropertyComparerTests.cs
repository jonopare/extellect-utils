using System.Collections;

namespace Extellect.Nuvo;

public class DeepPropertyComparerTests
{
    private sealed class SimpleNode
    {
        public int Id { get; set; }
        public string Label { get; set; } = "";
        public List<int>? Scores { get; set; }
        public SimpleNode? Child { get; set; }
    }

    [Fact]
    public void Both_null_reference_type_is_similar()
    {
        var sut = new DeepPropertyComparer();
        Assert.True(sut.AreSimilar<SimpleNode?>(null, null));
        Assert.Empty(sut.Differences);
    }

    [Fact]
    public void Exactly_one_null_is_not_similar()
    {
        var sut = new DeepPropertyComparer();
        Assert.False(sut.AreSimilar(new SimpleNode(), null));
        Assert.Contains(sut.Differences, d => d.Contains("exactly one of X or Y is null", StringComparison.Ordinal));

        sut = new DeepPropertyComparer();
        Assert.False(sut.AreSimilar(null, new SimpleNode()));
        Assert.Contains(sut.Differences, d => d.Contains("exactly one of X or Y is null", StringComparison.Ordinal));
    }

    [Fact]
    public void Primitives_and_string_equal_and_unequal()
    {
        var sut = new DeepPropertyComparer(DeepPropertyComparisonOptions.Primitives);
        Assert.True(sut.AreSimilar(42, 42));
        Assert.False(sut.AreSimilar(1, 2));
        Assert.True(sut.AreSimilar("x", "x"));
        Assert.False(sut.AreSimilar("a", "b"));
        Assert.True(sut.AreSimilar(Guid.Empty, Guid.Empty));
        Assert.False(sut.AreSimilar(Guid.Empty, Guid.Parse("b7d9e0a1-3c5f-4a82-9d11-2e6f4a8b0c1d")));
    }

    [Fact]
    public void Deep_property_graph_matches_when_equal()
    {
        var a = new SimpleNode
        {
            Id = 1,
            Label = "root",
            Scores = [10, 20],
            Child = new SimpleNode { Id = 2, Label = "leaf", Scores = [] },
        };
        var b = new SimpleNode
        {
            Id = 1,
            Label = "root",
            Scores = [10, 20],
            Child = new SimpleNode { Id = 2, Label = "leaf", Scores = [] },
        };
        var sut = new DeepPropertyComparer(DeepPropertyComparisonOptions.All);
        Assert.True(sut.AreSimilar(a, b));
        Assert.Empty(sut.Differences);
    }

    [Fact]
    public void Deep_property_graph_is_not_similar_when_children_are_unequal()
    {
        var a = new SimpleNode
        {
            Id = 1,
            Label = "root",
            Scores = [10, 20],
            Child = new SimpleNode { Id = 2, Label = "leaf", Scores = [30] },
        };
        var b = new SimpleNode
        {
            Id = 1,
            Label = "root",
            Scores = [10, 20],
            Child = new SimpleNode { Id = 2, Label = "leaf", Scores = [7] },
        };
        var sut = new DeepPropertyComparer(DeepPropertyComparisonOptions.All);
        Assert.False(sut.AreSimilar(a, b));
        Assert.Contains(sut.Differences, d => d.Contains("Child.Scores.[0]: 30 != 7", StringComparison.Ordinal));
    }

    [Fact]
    public void Deep_property_graph_differs_when_scalar_property_differs()
    {
        var a = new SimpleNode { Id = 1, Label = "x" };
        var b = new SimpleNode { Id = 1, Label = "y" };
        var sut = new DeepPropertyComparer(DeepPropertyComparisonOptions.All);
        Assert.False(sut.AreSimilar(a, b));
        Assert.Contains(sut.Differences, d => d.Contains("Label", StringComparison.Ordinal));
    }

    [Fact]
    public void List_of_int_same_length_and_values_is_similar()
    {
        var sut = new DeepPropertyComparer(DeepPropertyComparisonOptions.All);
        Assert.True(sut.AreSimilar(new List<int> { 1, 2, 3 }, new List<int> { 1, 2, 3 }));
        Assert.Empty(sut.Differences);
    }

    [Fact]
    public void List_of_int_different_length_is_not_similar()
    {
        var sut = new DeepPropertyComparer(DeepPropertyComparisonOptions.All);
        Assert.False(sut.AreSimilar(new List<int> { 1 }, new List<int> { 1, 2 }));
        Assert.Contains(
            sut.Differences,
            d => d.Contains("sequences differ in length", StringComparison.Ordinal));
    }

    [Fact]
    public void List_of_int_same_length_different_item_is_not_similar()
    {
        var sut = new DeepPropertyComparer(DeepPropertyComparisonOptions.All);
        Assert.False(sut.AreSimilar(new List<int> { 1, 9 }, new List<int> { 1, 2 }));
        Assert.Contains(sut.Differences, d => d.Contains("[1]", StringComparison.Ordinal));
    }

    [Fact]
    public void Empty_lists_are_similar()
    {
        var sut = new DeepPropertyComparer(DeepPropertyComparisonOptions.All);
        Assert.True(sut.AreSimilar(new List<int>(), new List<int>()));
    }

    [Fact]
    public void Array_of_int_compared_elementwise()
    {
        var sut = new DeepPropertyComparer(DeepPropertyComparisonOptions.All);
        Assert.True(sut.AreSimilar(new[] { 1, 2 }, new[] { 1, 2 }));
        Assert.False(sut.AreSimilar(new[] { 1 }, new[] { 1, 2 }));
    }

    [Fact]
    public void List_of_nested_objects_uses_recursive_compare()
    {
        var a = new List<SimpleNode> { new() { Id = 1, Label = "a" } };
        var b = new List<SimpleNode> { new() { Id = 1, Label = "b" } };
        var sut = new DeepPropertyComparer(DeepPropertyComparisonOptions.All);
        Assert.False(sut.AreSimilar(a, b));
        Assert.Contains(sut.Differences, d => d.Contains("[0].Label", StringComparison.Ordinal));
    }

    [Fact]
    public void Registered_double_similarity_used_within_tolerance()
    {
        var sut = new DeepPropertyComparer(new DoubleSimilarity(0.05));
        Assert.True(sut.AreSimilar(1.0, 1.02));
        Assert.Empty(sut.Differences);
    }

    [Fact]
    public void Registered_double_similarity_fails_outside_tolerance()
    {
        var sut = new DeepPropertyComparer(new DoubleSimilarity(0.01));
        Assert.False(sut.AreSimilar(1.0, 1.5));
        Assert.NotEmpty(sut.Differences);
    }

    [Fact]
    public void Nested_collections_report_path_in_differences()
    {
        var a = new SimpleNode { Scores = [1, 2, 3] };
        var b = new SimpleNode { Scores = [1, 9, 3] };
        var sut = new DeepPropertyComparer(DeepPropertyComparisonOptions.All);
        Assert.False(sut.AreSimilar(a, b));
        Assert.Contains(sut.Differences, d => d.Contains("Scores.[1]", StringComparison.Ordinal));
    }
}
