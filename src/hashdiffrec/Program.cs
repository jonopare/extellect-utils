using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace hashdiffrec
{
    class Program
    {
        class Counter
        {
            public int LeftOnlyFiles { get; set; }
            public int RightOnlyFiles { get; set; }
            public int BothFilesIdentical { get; set; }
            public int BothFilesDifferent { get; set; }

            public int LeftOnlyDirectories { get; set; }
            public int RightOnlyDirectories { get; set; }
            public int BothDirectories { get; set; }
        }

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: hashdiffrec.exe [leftpath] [rightpath]");
                return;
            }

            Counter counter = new Counter();

            Compare(
                new DirectoryInfo(args[0]),
                new DirectoryInfo(args[1]),
                counter
                );

            Console.WriteLine("======START_SUMMARY=====");
            Console.WriteLine("Left Only Files: {0}", counter.LeftOnlyFiles);
            Console.WriteLine("Right Only Files: {0}", counter.RightOnlyFiles);
            Console.WriteLine("Both Files Identical: {0}", counter.BothFilesIdentical);
            Console.WriteLine("Both Files Different: {0}", counter.BothFilesDifferent);
            Console.WriteLine("Left Only Directories: {0}", counter.LeftOnlyDirectories);
            Console.WriteLine("Right Only Directories: {0}", counter.RightOnlyDirectories);
            Console.WriteLine("Both Directories: {0}", counter.BothDirectories);
            Console.WriteLine("======END_SUMMARY=====");
        }

        static void Compare(DirectoryInfo left, DirectoryInfo right, Counter counter)
        {
            CompareDirectories(left, right, counter);
            CompareFiles(left, right, counter);

        }

        private static void CompareDirectories(DirectoryInfo left, DirectoryInfo right, Counter counter)
        {
            counter.BothDirectories++;

            IEnumerable<DirectoryInfo> leftDirectories = left.GetDirectories();
            IEnumerable<DirectoryInfo> rightDirectories = right.GetDirectories();

            IEnumerable<string> lefts = leftDirectories.Select(f => f.Name);
            IEnumerable<string> rights = rightDirectories.Select(f => f.Name);

            IEnumerable<string> both = lefts.Intersect(rights);
            foreach (string name in both)
            {
                Compare(
                    leftDirectories.Where(f => f.Name == name).First(),
                    rightDirectories.Where(f => f.Name == name).First(),
                    counter
                    );
            }

            if (lefts.Count() != rights.Count() || lefts.Count() != both.Count())
            {
                IEnumerable<string> leftOnly = lefts.Except(rights);
                if (leftOnly.Count() > 0)
                {
                    Console.WriteLine("------START-LEFT_ONLY-----");
                    foreach (string name in leftOnly)
                    {
                        Console.WriteLine(leftDirectories.Where(f => f.Name == name).First().FullName);
                        counter.LeftOnlyDirectories++;
                    }
                    Console.WriteLine("------END-LEFT_ONLY-----");
                }
                IEnumerable<string> rightOnly = rights.Except(lefts);
                if (rightOnly.Count() > 0)
                {
                    Console.WriteLine("------START-RIGHT_ONLY-----");
                    foreach (string name in rightOnly)
                    {
                        Console.WriteLine(rightDirectories.Where(f => f.Name == name).First().FullName);
                        counter.RightOnlyDirectories++;
                    }
                    Console.WriteLine("------END-RIGHT_ONLY-----");
                }
            }
        }

        private static void CompareFiles(DirectoryInfo left, DirectoryInfo right, Counter counter)
        {
            IEnumerable<FileInfo> leftFiles = left.GetFiles();
            IEnumerable<FileInfo> rightFiles = right.GetFiles();

            IEnumerable<string> lefts = leftFiles.Select(f => f.Name);
            IEnumerable<string> rights = rightFiles.Select(f => f.Name);

            IEnumerable<string> both = lefts.Intersect(rights);
            foreach (string name in both)
            {
                Compare(
                    leftFiles.Where(f => f.Name == name).First(),
                    rightFiles.Where(f => f.Name == name).First(),
                    counter
                    );
            }

            if (lefts.Count() != rights.Count() || lefts.Count() != both.Count())
            {
                IEnumerable<string> leftOnly = lefts.Except(rights);
                if (leftOnly.Count() > 0)
                {
                    Console.WriteLine("------START-LEFT_ONLY-----");
                    foreach (string name in leftOnly)
                    {
                        Console.WriteLine(leftFiles.Where(f => f.Name == name).First().FullName);
                        counter.LeftOnlyFiles++;
                    }
                    Console.WriteLine("------END-LEFT_ONLY-----");
                }
                IEnumerable<string> rightOnly = rights.Except(lefts);
                if (rightOnly.Count() > 0)
                {
                    Console.WriteLine("------START-RIGHT_ONLY-----");
                    foreach (string name in rightOnly)
                    {
                        Console.WriteLine(rightFiles.Where(f => f.Name == name).First().FullName);
                        counter.RightOnlyFiles++;
                    }
                    Console.WriteLine("------END-RIGHT_ONLY-----");
                }
            }
        }

        static void Compare(FileInfo left, FileInfo right, Counter counter)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            md5.Initialize();

            byte[] leftHash;
            using (FileStream stream = left.OpenRead())
            {
                leftHash = md5.ComputeHash(stream);
            }

            byte[] rightHash;
            using (FileStream stream = right.OpenRead())
            {
                rightHash = md5.ComputeHash(stream);
            }

            if (!leftHash.SequenceEqual(rightHash))
            {
                Console.WriteLine("------START-FILE_DIFF-----");
                Console.WriteLine(left.FullName);
                Console.WriteLine(BitConverter.ToString(leftHash));
                Console.WriteLine(right.FullName);
                Console.WriteLine(BitConverter.ToString(rightHash));
                Console.WriteLine("------END-FILE_DIFF-----");
                counter.BothFilesDifferent++;
            }
            else
            {
                counter.BothFilesIdentical++;
            }
        }
    }
}
