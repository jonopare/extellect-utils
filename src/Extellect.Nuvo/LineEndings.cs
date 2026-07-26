namespace Extellect.Nuvo
{
    public static class LineEndings
    {
        public static LineEndingAnalysis Analyze(string text)
        {
            var crs = new List<int>();
            var lfs = new List<int>();
            for (var c = 0; c < text.Length; c++)
            {
                switch (text[c])
                {
                    case '\r':
                        crs.Add(c);
                        break;
                    case '\n':
                        lfs.Add(c);
                        break;
                    default:
                        break;
                }
            }

            var crLfs = new List<int>();
            for (var i = crs.Count - 1; i >= 0; i--)
            {
                var r = crs[i];
                var j = lfs.IndexOf(r + 1);
                if (j >= 0)
                {
                    crs.RemoveAt(i);
                    lfs.RemoveAt(j);
                    crLfs.Add(r);
                }
            }

            return new LineEndingAnalysis(text, crs, lfs, crLfs);
        }

        public static void Normalize(string directory, Func<string, bool> onDirectory, Func<string, LineEndingNormalizations> onFile, Action<string, LineEndingNormalizations> normalized, bool whatIf = false)
        {
            foreach (var sub in Directory.GetDirectories(directory))
            {
                if (onDirectory(sub))
                {
                    Normalize(sub, onDirectory, onFile, normalized);
                }
            }
            foreach (var path in Directory.GetFiles(directory))
            {
                var normalization = onFile(path);
                if (normalization != LineEndingNormalizations.None)
                {
                    var text = File.ReadAllText(path);
                    var analysis = Analyze(text);
                    if (analysis.HasMixed)
                    {
                        switch (normalization)
                        {
                            case LineEndingNormalizations.MajorityRules:
                                text = analysis.ToMajorityRules(out normalization);
                                break;
                            case LineEndingNormalizations.Windows:
                                text = analysis.ToWindows();
                                break;
                            case LineEndingNormalizations.Unix:
                                text = analysis.ToUnix();
                                break;
                        }
                        if (!whatIf)
                        {
                            File.WriteAllText(path, text);
                        }
                    }
                    else
                    {
                        normalization = LineEndingNormalizations.None;
                    }
                }
                normalized(path, normalization);
            }
        }
    }
}
