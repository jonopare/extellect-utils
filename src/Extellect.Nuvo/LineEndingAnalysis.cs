using System.Text;

namespace Extellect.Nuvo
{
    public class LineEndingAnalysis
    {
        private readonly string _text;
        private readonly List<int> _crs;
        private readonly List<int> _lfs;
        private readonly List<int> _crLfs;

        public LineEndingAnalysis(string text, List<int> crs, List<int> lfs, List<int> crLfs)
        {
            _text = text;
            _crs = crs;
            _lfs = lfs;
            _crLfs = crLfs;
        }

        public bool HasMacintosh => _crs.Count > 0;
        public bool HasUnix => _lfs.Count > 0;
        public bool HasWindows => _crLfs.Count > 0;
        public bool HasMixed => (HasMacintosh ? 1 : 0) + (HasUnix ? 1 : 0) + (HasWindows ? 1 : 0) > 1;

        public string ToUnix()
        {
            if (HasMacintosh)
                throw new NotSupportedException();

            if (HasUnix && !HasWindows)
                return _text;

            var unix = new StringBuilder(_text);
            foreach (var crLf in _crLfs)
            {
                unix.Remove(crLf, 1);
            }
            return unix.ToString();
        }

        public string ToWindows()
        {
            if (HasMacintosh)
                throw new NotSupportedException();

            if (HasWindows && !HasUnix)
                return _text;

            var windows = new StringBuilder(_text);
            foreach (var ir in _crs.Select(r => new { Index = r + 1, Replacement = '\n' })
                .Concat(_lfs.Select(n => new { Index = n, Replacement = '\r' }))
                .OrderByDescending(x => x.Index))
            {
                windows.Insert(ir.Index, ir.Replacement);
            }
            return windows.ToString();
        }

        public string ToMajorityRules(out LineEndingNormalizations normalization)
        {
            if (_crLfs.Count >= _lfs.Count)
            {
                normalization = LineEndingNormalizations.Windows;
                return ToWindows();
            }
            else
            {
                normalization = LineEndingNormalizations.Unix;
                return ToUnix();
            }
        }
    }
}
