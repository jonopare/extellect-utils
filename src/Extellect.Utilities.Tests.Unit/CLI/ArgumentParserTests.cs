using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extellect.Utilities.CLI
{
    [TestClass]
    public class ArgumentParserTests
    {
        private class IgnoreUnmarkedArguments : Arguments
        {
            public string PropertyWithoutAttribute { get; set; }

            public override bool IgnoreUnmarked
            {
                get
                {
                    return false;
                }
            }
        }

        private class TestArguments : Arguments
        {
            [Argument(Required = true)]
            public int Int32 { get; set; }

            [Argument]
            public List<int> ListOfInt32 { get; set; }

            public string PropertyWithoutAttribute { get; set; }
        }

        [TestMethod]
        public void ParseInternal_HyphenEquals_SplitsCorrectly()
        {
            var validationErrors = new List<string>();

            var pair = ArgumentParser<Arguments>.Parse("-foo=bar", e => validationErrors.Add(e));

            Assert.AreEqual(0, validationErrors.Count);
            Assert.AreEqual("foo", pair.Key);
            Assert.AreEqual("bar", pair.Value);
        }

        [TestMethod]
        public void ParseInternal_HyphenColon_SplitsCorrectly()
        {
            var validationErrors = new List<string>();

            var pair = ArgumentParser<Arguments>.Parse("-foo:bar", e => validationErrors.Add(e));

            Assert.AreEqual(0, validationErrors.Count);
            Assert.AreEqual("foo", pair.Key);
            Assert.AreEqual("bar", pair.Value);
        }

        [TestMethod]
        public void ParseInternal_SlashEquals_SplitsCorrectly()
        {
            var validationErrors = new List<string>();

            var pair = ArgumentParser<Arguments>.Parse("/foo=bar", e => validationErrors.Add(e));

            Assert.AreEqual(0, validationErrors.Count);
            Assert.AreEqual("foo", pair.Key);
            Assert.AreEqual("bar", pair.Value);
        }

        [TestMethod]
        public void ParseInternal_SlashColon_SplitsCorrectly()
        {
            var validationErrors = new List<string>();

            var pair = ArgumentParser<Arguments>.Parse("/foo:bar", e => validationErrors.Add(e));

            Assert.AreEqual(0, validationErrors.Count);
            Assert.AreEqual("foo", pair.Key);
            Assert.AreEqual("bar", pair.Value);
        }

        [TestMethod]
        public void ParseInternal_BlankEquals_SplitsCorrectly()
        {
            var validationErrors = new List<string>();

            var pair = ArgumentParser<Arguments>.Parse("foo=bar", e => validationErrors.Add(e));

            Assert.AreEqual(0, validationErrors.Count);
            Assert.AreEqual("foo", pair.Key);
            Assert.AreEqual("bar", pair.Value);
        }

        [TestMethod]
        public void ParseInternal_BlankColon_SplitsCorrectly()
        {
            var validationErrors = new List<string>();

            var pair = ArgumentParser<Arguments>.Parse("foo:bar", e => validationErrors.Add(e));

            Assert.AreEqual(0, validationErrors.Count);
            Assert.AreEqual("foo", pair.Key);
            Assert.AreEqual("bar", pair.Value);
        }

        [TestMethod]
        public void ParseInternal_NameOnly_FailsValidation()
        {
            var validationErrors = new List<string>();

            var pair = ArgumentParser<Arguments>.Parse("foo", e => validationErrors.Add(e));

            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual("No separator found: foo", validationErrors[0]);
        }

        [TestMethod]
        public void ParseInternal_SlashMissingNameAndSeparator_FailsValidation()
        {
            var validationErrors = new List<string>();

            var pair = ArgumentParser<Arguments>.Parse("/", e => validationErrors.Add(e));

            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual("No name or separator found: /", validationErrors[0]);
        }

        [TestMethod]
        public void ParseInternal_SlashMissingName_FailsValidation()
        {
            var validationErrors = new List<string>();

            var pair = ArgumentParser<Arguments>.Parse("/:", e => validationErrors.Add(e));

            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual("No name found: /:", validationErrors[0]);
        }

        [TestMethod]
        public void ParseInternal_BlankMissingName_FailsValidation()
        {
            var validationErrors = new List<string>();

            var pair = ArgumentParser<Arguments>.Parse(":", e => validationErrors.Add(e));

            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual("No name found: :", validationErrors[0]);
        }

        [TestMethod]
        public void ConvertAndSetValue_Int32_ConvertsCorrectly()
        {
            var args = new TestArguments();
            var property = typeof(TestArguments).GetProperty("Int32");
            ArgumentParser<TestArguments>.ConvertAndSetValue(args, property, "1");
            Assert.AreEqual(1, args.Int32);
        }

        [TestMethod]
        public void ConvertAndSetValue_ListOfInt32_ConvertsCorrectly()
        {
            var args = new TestArguments();
            var property = typeof(TestArguments).GetProperty("ListOfInt32");
            ArgumentParser<TestArguments>.ConvertAndSetValue(args, property, "2");
            Assert.AreEqual(1, args.ListOfInt32.Count);
            Assert.AreEqual(2, args.ListOfInt32[0]);
        }

        [TestMethod]
        public void ConvertAndSetValue_MultipleValueListOfInt32_ConvertsCorrectly()
        {
            var args = new TestArguments();
            var property = typeof(TestArguments).GetProperty("ListOfInt32");
            ArgumentParser<TestArguments>.ConvertAndSetValue(args, property, "3");
            ArgumentParser<TestArguments>.ConvertAndSetValue(args, property, "4");
            Assert.AreEqual(2, args.ListOfInt32.Count);
            Assert.AreEqual(3, args.ListOfInt32[0]);
            Assert.AreEqual(4, args.ListOfInt32[1]);
        }

        [TestMethod]
        public void Parse_Various_SplitsCorrectly()
        {
            var args = ArgumentParser<TestArguments>.Parse(new[] { "/Int32:1", "/ListOfInt32:2", "/ListOfInt32:3" });
            Assert.AreEqual(1, args.Int32);
            Assert.AreEqual(2, args.ListOfInt32.Count);
            Assert.AreEqual(2, args.ListOfInt32[0]);
            Assert.AreEqual(3, args.ListOfInt32[1]);
        }

        [ExpectedException(typeof(ArgumentValidationException))]
        [TestMethod]
        public void Parse_UnknownArgumentFound_ThrowsException()
        {
            ArgumentParser<TestArguments>.Parse(new[] { "/BadSpelling:1" });
        }

        [ExpectedException(typeof(ArgumentValidationException))]
        [TestMethod]
        public void Parse_MissingRequiredArgument_ThrowsException()
        {
            ArgumentParser<TestArguments>.Parse(new[] { "/ListOfInt32:1" });
        }

        [ExpectedException(typeof(ArgumentValidationException))]
        [TestMethod]
        public void Parse_PropertyWithoutAttribute_ThrowsException()
        {
            var args = ArgumentParser<TestArguments>.Parse(new[] { "/Int32:41", "/PropertyWithoutAttribute:ignore" });
            Assert.IsTrue(args.IgnoreUnmarked);
            Assert.IsNull(args.PropertyWithoutAttribute);
        }

        [TestMethod]
        public void Parse_CannotIgnoreUnmarked_SetsValue()
        {
            var args = ArgumentParser<IgnoreUnmarkedArguments>.Parse(new[] { "/PropertyWithoutAttribute:unmarked" });
            Assert.IsFalse(args.IgnoreUnmarked);
            Assert.AreEqual("unmarked", args.PropertyWithoutAttribute);
        }
    }
}
