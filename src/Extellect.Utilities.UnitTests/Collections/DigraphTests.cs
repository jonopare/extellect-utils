using Extellect.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Extellect.Utilities.Collections
{
    public class DigraphTests
    {
        [Theory]
        [InlineData("A>B", "B")]
        [InlineData("A>B;B", "B")]
        [InlineData("A>B;A>C", "B;C")]
        [InlineData("A>B;A>C;B>C", "C")]
        [InlineData("A>B;A>C;B>D;C>D", "D")]
        [InlineData("A>B;A>C;B>D;C>D;D>A", null)]
        public void Roots(string input, string expected)
        {
            var sut = new Digraph<string>();
            foreach (var pair in Edges(input))
            {
                if (pair.Item2 == null)
                {
                    sut.Add(pair.Item1);
                }
                else
                {
                    sut.Add(pair.Item1, pair.Item2);
                }
            }
            Assert.Equal(Groups(expected), sut.Roots.OrderBy(x => x));
        }

        [Theory]
        [InlineData("A>B", "A", "B")]
        [InlineData("A>B;A>C", "A", "B;C")]
        [InlineData("A>B;A>C;B>C", "A", "B;C")]
        [InlineData("A>B;A>C;B>D;C>D", "A", "B;C;D")]
        [InlineData("A>B;A>C;B>D;C>D;D>A", "A", "A;B;C;D")]
        public void Precedents(string graphText, string dependent, string expected)
        {
            var sut = new Digraph<string>();
            foreach (var pair in Edges(graphText))
            {
                sut.Add(pair.Item1, pair.Item2);
            }
            Assert.Equal(Groups(expected), sut.Precedents(dependent).OrderBy(x => x));
        }

        [Theory]
        [InlineData("A>B", "B", "A")]
        [InlineData("A>B;A>C", "B", "A")]
        [InlineData("A>B;A>C", "C", "A")]
        [InlineData("A>B;A>C;B>C", "C", "A;B")]
        [InlineData("A>B;A>C;B>D;C>D", "D", "A;B;C")]
        [InlineData("A>B;A>C;B>D;C>D;D>A", "A", "A;B;C;D")]
        public void Dependents(string graphText, string dependent, string expected)
        {
            var sut = new Digraph<string>();
            foreach (var pair in Edges(graphText))
            {
                sut.Add(pair.Item1, pair.Item2);
            }
            Assert.Equal(Groups(expected), sut.Dependents(dependent).OrderBy(x => x));
        }

        [Theory]
        [InlineData("A>B", false)]
        [InlineData("A>B;A>C", false)]
        [InlineData("A>B;A>C;B>C", false)]
        [InlineData("A>B;A>C;B>D;C>D", false)]
        [InlineData("A>B;A>C;B>D;C>D;D>A", true)]
        [InlineData("A>B;A>C;B>D;C>D;C>A", true)]
        public void ContainsCycles(string graphText, bool expected)
        {
            var sut = new Digraph<string>();
            foreach (var pair in Edges(graphText))
            {
                sut.Add(pair.Item1, pair.Item2);
            }
            Assert.Equal(expected, sut.ContainsCycles);
        }

        [Fact]
        public void Integration()
        {
            var sut = new Digraph<string>();
            sut.Add("Initial database table design [4h]", "Create basic project plan [4h]");
            sut.Add("Choose Source Control [0h]", "Create basic project plan [4h]");
            sut.Add("Setup new AWS account [2h]", "Create basic project plan [4h]");
            sut.Add("Setup new GitHub account [2h]", "Choose Source Control [0h]");
            sut.Add("Review existing C# code [1d 4h]", "Initial database table design [4h]");
            sut.Add("Review initial database table design [4h]", "Initial database table design [4h]");
            sut.Add("Create auth tables and classes [4h]", "Create basic project plan [4h]");
            sut.Add("Database table design [2d]", "Review existing C# code [1d 4h]");
            sut.Add("Database table design [2d]", "Review initial database table design [4h]");
            sut.Add("Setup ASP.NET in AWS [4h]", "Setup new AWS account [2h]");
            sut.Add("Setup ASP.NET service account [1h]", "Setup ASP.NET in AWS [4h]");
            sut.Add("ASP.NET Look and Feel Prototype [6h]", "Create basic project plan [4h]");
            sut.Add("ASP.NET master page [1d]", "ASP.NET Look and Feel Prototype [6h]");
            sut.Add("ASP.NET Excel upload page [4h]", "ASP.NET master page [1d]");
            sut.Add("Develop ASP.NET input pages with validation [5d]", "ASP.NET master page [1d]");
            sut.Add("Deliver ASP.NET application to non-production [4h]", "Setup ASP.NET service account [1h]");
            sut.Add("ASP.NET user management page [4h]", "ASP.NET master page [1d]");
            sut.Add("ASP.NET login page [2h]", "ASP.NET master page [1d]");
            sut.Add("Persist uploaded Excel data [1d]", "ASP.NET Excel upload page [4h]");
            sut.Add("Setup SQL Server in AWS [4h]", "Setup new AWS account [2h]");
            sut.Add("Deliver database to non-production [2h]", "Setup SQL Server in AWS [4h]");
            sut.Add("Deliver database to production [4h]", "Deliver database to non-production [2h]");
            sut.Add("Implement SQL tables and indexes [1d 4h]", "Database table design [2d]");
            sut.Add("Setup SQL Server database principals [2h]", "Database table design [2d]");
            sut.Add("Populate SQL tables with test data [1d]", "Implement SQL tables and indexes [1d 4h]");
            sut.Add("Develop C# database entity classes [1d]", "Database table design [2d]");
            sut.Add("Integrate entity classes with C# demo [1d]", "Implement SQL tables and indexes [1d 4h]");
            sut.Add("Test run [1d]", "Deliver database to non-production [2h]");
            sut.Add("Test run [1d]", "Deliver ASP.NET application to non-production [4h]");
            Assert.Equal(new[] { "Create basic project plan [4h]" }, sut.Roots);
        }

        private IEnumerable<(string, string)> Edges(string text)
        {
            return Groups(text).Select(x => x.Split('>')).Select(x => (x[0], x.Length > 1 ? x[1] : null));
        }

        private IEnumerable<string> Groups(string text)
        {
            return text == null ? Enumerable.Empty<string>() : text.Split(';');
        }
    }
}
