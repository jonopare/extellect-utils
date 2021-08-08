using Extellect.Logging;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Extellect
{
    public class CellStyleBuilder
    {
        internal readonly static Regex _rgbColorRegex = new Regex(@"^rgb\((?<red>\d+),(?<green>\d+),(?<blue>\d+)\)$");
        internal readonly static Regex _colorRegex = new Regex(@"(?<name>black|silver|gray|white|maroon|red|purple|fuchsia|green|lime|olive|yellow|navy|blue|teal|aqua)|(?<hex>#(?<hexRed>[0-9A-Fa-f]{2})(?<hexGreen>[0-9A-Fa-f]{2})(?<hexBlue>[0-9A-Fa-f]{2}))|(?<rgb>rgb\((?<red>\d+)\s*,\s*(?<green>\d+)\s*,\s*(?<blue>\d+)\))");

        private readonly XSSFWorkbook _workbook;
        private readonly IBasicLog _basicLog;
        private readonly Dictionary<string, Action<XSSFCellStyle>> _cellStyleInitializerByRule;
        private readonly Dictionary<string, Action<XSSFFont>> _fontInitializerByRule;
        private readonly Dictionary<string, string> _dataFormatByRule;
        private readonly Dictionary<string, string[]> _ruleSets;
        private readonly Dictionary<string, XSSFCellStyle> _cellStyleCache;
        private readonly Dictionary<string, XSSFFont> _fontCache;
        private readonly Dictionary<string, XSSFColor> _colorCache;
        private readonly Dictionary<string, short> _dataFormatCache;
        private IDataFormat _dataFormat;

        public CellStyleBuilder(XSSFWorkbook workbook, IBasicLog basicLog)
        {
            _workbook = workbook;
            _basicLog = basicLog;
            _dataFormatByRule = new Dictionary<string, string>
            {
                { "data-format: number", "_ * #,##0_ ; * -#,##0_ ;_ * \"-\"_ ;_ @_ " } ,
                { "data-format: money", "#,##0.00" },
                { "data-format: date", "yyyy-MM-dd" },
                { "data-format: datetime", "yyyy-MM-dd HH:mm:ss" },
                { "data-format: percent", "0.00%" },
            };
            _fontInitializerByRule = new Dictionary<string, Action<XSSFFont>>
            {
                { "font-weight: bold", font => font.IsBold = true },
                { "font-style: italic", font => font.IsItalic = true },
                { "color: red", font => font.SetColor(Color(NormalizeColor("red"))) },
            };
            _cellStyleInitializerByRule = new Dictionary<string, Action<XSSFCellStyle>>
            {
                { "border-bottom: double red", cs => { cs.BorderBottom = BorderStyle.Double; cs.SetBottomBorderColor(Color(NormalizeColor("red"))); } },
            };
            _ruleSets = new Dictionary<string, string[]>
            {
                { "italic", new[] { "font-style: italic" } },
                { "bold", new[] { "font-weight: bold" } },
                { "red", new[] { "color: red" } },
                { "money", new [] { "data-format: money" } },
            };
            _cellStyleCache = new Dictionary<string, XSSFCellStyle>();
            _fontCache = new Dictionary<string, XSSFFont>();
            _colorCache = new Dictionary<string, XSSFColor>();
            _dataFormatCache = new Dictionary<string, short>();
        }

        public XSSFCellStyle CellStyle(params string[] ruleSetNames)
        {
            return CellStyle((IEnumerable<string>)ruleSetNames);
        }

        public XSSFCellStyle CellStyle(IEnumerable<string> ruleSetNames)
        {
            // Cell style key is made up of all names
            // Font key is made up of names relevant only to fonts
            var cellStyleKey = string.Join("|", ruleSetNames);
            if (!_cellStyleCache.TryGetValue(cellStyleKey, out var cellStyle))
            {
                _basicLog?.Debug($"Cell style not found in cache. Creating: {cellStyleKey}");
                cellStyle = (XSSFCellStyle)_workbook.CreateCellStyle();

                var cellStyleRules = new List<string>();
                var fontRules = new List<string>();
                var dataFormatRules = new List<string>();

                ExtractRules(ruleSetNames, cellStyleRules, fontRules, dataFormatRules);

                ApplyCellStyleRules(cellStyle, cellStyleRules);
                ApplyFontRules(cellStyle, fontRules);
                ApplyDataFormatRules(cellStyle, dataFormatRules);

                _cellStyleCache.Add(cellStyleKey, cellStyle);
            }
            return cellStyle;
        }

        private void ExtractRules(IEnumerable<string> ruleSetNames, List<string> cellStyleRules, List<string> fontRules, List<string> dataFormatRules)
        {
            foreach (var ruleSetName in ruleSetNames)
            {
                if (!_ruleSets.TryGetValue(ruleSetName, out var rules))
                {
                    _basicLog?.Debug($"Rule set not found: {ruleSetName}");
                    continue;
                }
                foreach (var rule in rules)
                {
                    if (_cellStyleInitializerByRule.ContainsKey(rule))
                    {
                        cellStyleRules.Add(rule);
                    }
                    else if (_fontInitializerByRule.ContainsKey(rule))
                    {
                        fontRules.Add(rule);
                    }
                    else if (_dataFormatByRule.ContainsKey(rule))
                    {
                        dataFormatRules.Add(rule);
                    }
                    else
                    {
                        _basicLog?.Debug($"Rule handler not found");
                    }
                }
            }
        }

        private void ApplyCellStyleRules(XSSFCellStyle cellStyle, List<string> cellStyleRules)
        {
            var hasBorderHack = false;

            foreach (var rule in cellStyleRules)
            {
                // HACK: workaround for NPOI issue
                if (rule.StartsWith("border-") && !hasBorderHack)
                {
                    _cellStyleInitializerByRule["border-diagonal: thin"](cellStyle);
                    hasBorderHack = true;
                }

                _basicLog?.Debug($"Initializing cell style: {rule}");
                _cellStyleInitializerByRule[rule](cellStyle);
            }

            if (hasBorderHack)
            {
                _cellStyleInitializerByRule["border-diagonal: none"](cellStyle);
            }
        }

        private void ApplyDataFormatRules(XSSFCellStyle cellStyle, List<string> dataFormatRules)
        {
            foreach (var rule in dataFormatRules)
            {
                _basicLog?.Debug($"Setting data format: {rule}");
                cellStyle.SetDataFormat(DataFormat(_dataFormatByRule[rule]));
            }
        }

        private void ApplyFontRules(XSSFCellStyle cellStyle, List<string> fontRules)
        {
            if (fontRules.Any())
            {
                var fontKey = string.Join("|", fontRules);

                if (!_fontCache.TryGetValue(fontKey, out var font))
                {
                    _basicLog?.Debug($"Font not found in cache. Creating: {fontKey}");
                    font = (XSSFFont)_workbook.CreateFont();
                    font.FontHeightInPoints = 11;
                    foreach (var rule in fontRules)
                    {
                        _basicLog?.Debug($"Initializing font: {rule}");
                        _fontInitializerByRule[rule](font);
                    }
                    _fontCache.Add(fontKey, font);
                }

                cellStyle.SetFont(font);
            }
        }

        private short DataFormat(string format)
        {
            if (!_dataFormatCache.TryGetValue(format, out var result))
            {
                if (_dataFormat == null)
                {
                    _dataFormat = _workbook.CreateDataFormat();
                }
                result = _dataFormat.GetFormat(format);
                _dataFormatCache.Add(format, result);
            }
            return result;
        }

        private XSSFColor Color(string color)
        {
            if (!_colorCache.TryGetValue(color, out var result))
            {
                result = new XSSFColor(ParseRgb(color));
                _colorCache.Add(color, result);
            }
            return result;
        }

        internal IEnumerable<string> Atomize(string declaration)
        {
            yield break;
        }

        internal static string NormalizeColor(string color)
        {
            var match = _colorRegex.Match(color);
            if (!match.Success)
            {
                throw new NotSupportedException();
            }
            return NormalizeColor(match);
        }

        internal static string NormalizeColor(Match colorMatch)
        {
            if (colorMatch.Groups["name"].Success)
            {
                switch (colorMatch.Groups["name"].Value)
                {
                    case "black": return "rgb(0,0,0)";
                    case "silver": return "rgb(192,192,192)";
                    case "gray": return "rgb(128,128,128)";
                    case "white": return "rgb(255,255,255)";
                    case "maroon": return "rgb(128,0,0)";
                    case "red": return "rgb(255,0,0)";
                    case "purple": return "rgb(128,0,128)";
                    case "fuchsia": return "rgb(255,0,255)";
                    case "green": return "rgb(0,128,0)";
                    case "lime": return "rgb(0,255,0)";
                    case "olive": return "rgb(128,128,0)";
                    case "yellow": return "rgb(255,255,0)";
                    case "navy": return "rgb(0,0,128)";
                    case "blue": return "rgb(0,0,255)";
                    case "teal": return "rgb(0,128,128)";
                    case "aqua": return "rgb(0,255,255)";
                }
            }
            else if (colorMatch.Groups["hex"].Success)
            {
                var hexBytes = ParseHex(colorMatch.Groups["hexRed"].Value, colorMatch.Groups["hexGreen"].Value, colorMatch.Groups["hexBlue"].Value);
                return $"rgb({hexBytes[0]},{hexBytes[1]},{hexBytes[2]})";;
            }
            else if (colorMatch.Groups["rgb"].Success)
            {
                return $"rgb({colorMatch.Groups["red"].Value},{colorMatch.Groups["green"].Value},{colorMatch.Groups["blue"].Value})";
            }
            throw new NotSupportedException();
        }

        internal static byte[] ParseHex(string hexRed, string hexGreen, string hexBlue)
        {
            return new[]
            {
                byte.Parse(hexRed, NumberStyles.HexNumber),
                byte.Parse(hexGreen, NumberStyles.HexNumber),
                byte.Parse(hexBlue, NumberStyles.HexNumber),
            };
        }

        internal byte[] ParseRgb(string rgbColor)
        {
            var match = _rgbColorRegex.Match(rgbColor);
            if (!match.Success)
            {
                throw new NotSupportedException();
            }
            return new[]
            {
                byte.Parse(match.Groups["red"].Value),
                byte.Parse(match.Groups["green"].Value),
                byte.Parse(match.Groups["blue"].Value),
            };
        }
    }
}
