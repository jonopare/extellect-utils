namespace Extellect.Nuvo
{
    public class DependencyGraphTests
    {
        [Theory]
        [InlineData("A<-B;B<-C;C<-D;D<-A", true)]
        [InlineData("B->A;C->B;D->C;A->D", true)] // Same ring as above
        [InlineData("A<-B;B<-C;D<-C;E<-D", false)]
        [InlineData("HY2<-HY32;HY3<-HY32;FY2<-FY32;FY3<-FY32;HY32<-FY32;HY2<-FY2;HY3<-FY3;HY32<-FY32", false)]
        public void ContainsCycles(string dependencies, bool expected)
        {
            var graph = DependencyGraphHelper.ToDependencyGraph(dependencies);

            Assert.Equal(expected, graph.ContainsCycles);
        }

        [Fact]
        public void ContainsCycles_Cyclical_IsTrue()
        {
            var graph = new DependencyGraph<string>();
            graph.Add("A", "B");
            graph.Add("B", "C");
            graph.Add("C", "D");
            graph.Add("D", "A");

            Assert.True(graph.ContainsCycles);
        }

        [Fact]
        public void ContainsCycles_NonCyclical_IsFalse()
        {
            var graph = new DependencyGraph<string>();
            graph.Add("A", "B");
            graph.Add("B", "C");
            graph.Add("D", "C");
            graph.Add("E", "D");
            // the graph will implicitly contain "C" as an independent

            Assert.False(graph.ContainsCycles);
        }

        [Theory]
        [InlineData("HY2<-HY32;HY3<-HY32;FY2<-FY32;FY3<-FY32;HY32<-FY32;HY2<-FY2;HY3<-FY3;HY32<-FY32")]
        [InlineData("A<-AA;A<-AAA;AA<-AAA;B<-BB;B<-BBB;BB<-BBB")]
        [InlineData("(HY2019 VALUATION, TEST GROUP 2)<-(HY2019 VALUATION, TEST GROUP 32);(HY2019 VALUATION, TEST GROUP 2)<-(FY2019 VALUATION, TEST GROUP 2);(HY2019 VALUATION, TEST GROUP 32)<-(FY2019 VALUATION, TEST GROUP 32);(HY2019 VALUATION, TEST GROUP 3)<-(HY2019 VALUATION, TEST GROUP 32);(HY2019 VALUATION, TEST GROUP 3)<-(FY2019 VALUATION, TEST GROUP 3);(HY2019 VALUATION, TEST GROUP 4)<-(HY2019 VALUATION, TEST GROUP 33);(HY2019 VALUATION, TEST GROUP 4)<-(FY2019 VALUATION, TEST GROUP 4);(HY2019 VALUATION, TEST GROUP 33)<-(FY2019 VALUATION, TEST GROUP 33);(HY2019 VALUATION, TEST GROUP 24)<-(HY2019 VALUATION, TEST GROUP 38);(HY2019 VALUATION, TEST GROUP 24)<-(FY2019 VALUATION, TEST GROUP 24);(HY2019 VALUATION, TEST GROUP 38)<-(FY2019 VALUATION, TEST GROUP 38);(FY2019 VALUATION, TEST GROUP 2)<-(FY2019 VALUATION, TEST GROUP 32);(FY2019 VALUATION, TEST GROUP 3)<-(FY2019 VALUATION, TEST GROUP 32);(FY2019 VALUATION, TEST GROUP 4)<-(FY2019 VALUATION, TEST GROUP 33);(FY2019 VALUATION, TEST GROUP 24)<-(FY2019 VALUATION, TEST GROUP 38);(HY2019 VALUATION, TEST GROUP 1)<-(FY2019 VALUATION, TEST GROUP 1);(HY2019 VALUATION, TEST GROUP 5)<-(FY2019 VALUATION, TEST GROUP 5);(HY2019 VALUATION, TEST GROUP 6)<-(FY2019 VALUATION, TEST GROUP 6);(HY2019 VALUATION, TEST GROUP 17)<-(FY2019 VALUATION, TEST GROUP 17);(HY2019 VALUATION, TEST GROUP 18)<-(FY2019 VALUATION, TEST GROUP 18);(HY2019 VALUATION, TEST GROUP 19)<-(FY2019 VALUATION, TEST GROUP 19);(HY2019 VALUATION, TEST GROUP 20)<-(FY2019 VALUATION, TEST GROUP 20);(HY2019 VALUATION, TEST GROUP 21)<-(FY2019 VALUATION, TEST GROUP 21);(HY2019 VALUATION, TEST GROUP 22)<-(FY2019 VALUATION, TEST GROUP 22);(HY2019 VALUATION, TEST GROUP 23)<-(FY2019 VALUATION, TEST GROUP 23);(HY2019 VALUATION, TEST GROUP 25)<-(FY2019 VALUATION, TEST GROUP 25);(HY2019 VALUATION, TEST GROUP 26)<-(FY2019 VALUATION, TEST GROUP 26);(HY2019 VALUATION, TEST GROUP 27)<-(FY2019 VALUATION, TEST GROUP 27);(HY2019 VALUATION, TEST GROUP 28)<-(FY2019 VALUATION, TEST GROUP 28)")]
        public void Linearize_DoesNotReturnDuplicates(string dependenciesText)
        {
            var graph = DependencyGraphHelper.ToDependencyGraph(dependenciesText);

            var duplicates = graph.Linearize()
                .SelectMany(x => x)
                .GroupBy(x => x)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key);

            Assert.Empty(duplicates);
        }


        [Fact]
        public void Linearize_NonCyclical_EvaluatesInOrder_1()
        {
            // Remember: it's *not* A depends on B (it's A is depended on *by* B)

            var graph = new DependencyGraph<string>();
            graph.Add("HY 2", "HY 32");
            graph.Add("HY 3", "HY 32");
            graph.Add("FY 2", "FY 32");
            graph.Add("FY 3", "FY 32");
            graph.Add("HY 2", "FY 2");
            graph.Add("HY 3", "FY 3");
            graph.Add("HY 32", "FY 32");

            var expecteds = new Queue<HashSet<string>>();
            expecteds.Enqueue(new HashSet<string> { "HY 2", "HY 3", });
            expecteds.Enqueue(new HashSet<string> { "FY 2", "FY 3", "HY 32" });
            expecteds.Enqueue(new HashSet<string> { "FY 32", });

            foreach (var batch in graph.Linearize())
            {
                var expected = expecteds.Dequeue();
                Assert.Equal(expected, batch.ToHashSet());
            }

            Assert.Empty(expecteds);
        }

        [Fact]
        public void Linearize_NonCyclical_EvaluatesInOrder_2()
        {
            // You can't put on shoes before socks and you can't put on shoes before trousers.
            // Therefore you should put on your left sock, right sock and trousers first,
            // Then you can put on your left and right shoes second. 

            var graph = new DependencyGraph<string>();
            graph.Add("Trousers", "Left Shoe");
            graph.Add("Trousers", "Right Shoe");
            graph.Add("Left Sock", "Left Shoe");
            graph.Add("Right Sock", "Right Shoe");

            var expecteds = new Queue<HashSet<string>>();
            expecteds.Enqueue(new HashSet<string> { "Left Sock", "Right Sock", "Trousers" });
            expecteds.Enqueue(new HashSet<string> { "Left Shoe", "Right Shoe" });

            foreach (var batch in graph.Linearize())
            {
                var expected = expecteds.Dequeue();
                Assert.Equal(expected, batch.ToHashSet());
            }

            Assert.Empty(expecteds);
        }

        [Theory]
        [InlineData("A<-B;B<-C;C<-D;D<-A", "")]
        [InlineData("B->A;C->B;D->C;A->D", "")] // Same ring as above
        [InlineData("A<-B;B<-C;C<-D;D<-A;E<-D", "E")] // Contains a cycle but there's also a global independent
        [InlineData("A<-B;B<-C;D<-C;E<-D", "A;E")]
        [InlineData("HY2<-HY32;HY3<-HY32;FY2<-FY32;FY3<-FY32;HY32<-FY32;HY2<-FY2;HY3<-FY3;HY32<-FY32", "HY2;HY3")]
        public void GlobalIndependents(string dependencies, string expected)
        {
            var graph = DependencyGraphHelper.ToDependencyGraph(dependencies);

            var actual = string.Join(";", graph.GlobalIndependents.OrderBy(x => x));

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("A<-B;B<-C;D<-C;E<-D", "A", "A;B;C")]
        [InlineData("A<-B;B<-C;D<-C;E<-D", "B", "B;C")]
        [InlineData("A<-B;B<-C;D<-C;E<-D", "C", "C")]
        [InlineData("A<-B;B<-C;D<-C;E<-D", "D", "C;D")]
        [InlineData("A<-B;B<-C;D<-C;E<-D", "E", "C;D;E")]
        [InlineData("A<-B;B<-C;C<-D;D<-A", "A", "A;B;C;D")] // Ring
        public void GlobalDependents(string dependencies, string independent, string expected)
        {
            var graph = DependencyGraphHelper.ToDependencyGraph(dependencies);

            var actual = string.Join(";", graph.GlobalDependents(independent).OrderBy(x => x));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_IncludesAllNodesAndAllEdges()
        {
            var graph = DependencyGraphHelper.ToDependencyGraph("A<-B1;A<-B2;B1<-C1;B1<-C2;B2<-C3;B2<-C4;D");

            var actual = graph.ToString()
                .Split(new[] { ",\r\n" }, StringSplitOptions.None)
                .ToHashSet();

            string dependency(string independent, string dependent) => $"(:{independent})<-[:DEPENDS_ON]-(:{dependent})";

            var expected = new HashSet<string>
            {
                dependency("A", "B1"),
                dependency("A", "B2"),
                dependency("B1", "C1"),
                dependency("B1", "C2"),
                dependency("B2", "C3"),
                dependency("B2", "C4"),
                "(:A)",
                "(:D)",
            };

            //Assert.Subset(expected, actual);
            //Assert.Superset(expected, actual);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("A<-B;B<-C;C<-D", "AD", "A<-D")]
        [InlineData("A<-B;B<-C;C<-D", "ABD", "A<-B;B<-D")]
        [InlineData("A<-B;B<-C;C<-D", "ABCD", "A<-B;B<-C;C<-D")]
        [InlineData("A<-B;B<-C;C<-D", "AB", "A<-B")]
        [InlineData("A<-B;B<-C;C<-D", "CD", "C<-D")]
        [InlineData("A<-B;B<-C;C<-D", "BD", "B<-D")]
        [InlineData("A<-B;B<-C1;B<-C2;C2<-D;C1<-D", "BD", "B<-D")]
        [InlineData("A<-B;B<-C;C<-D", "B", "B")]
        [InlineData("A<-B;B<-C;C<-D;B", "B", "B")]
        public void Trim(string input, string keep, string output)
        {
            var graph = DependencyGraphHelper.ToDependencyGraph(input);
            var actual = graph.Trim(x => keep.IndexOf(x) >= 0);
            var expected = DependencyGraphHelper.ToDependencyGraph(output);
            Assert.Equal(expected.ToString(), actual.ToString());
        }
    }
}