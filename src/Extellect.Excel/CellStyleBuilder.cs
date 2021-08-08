using Extellect.Logging;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Extellect
{
    public class CellStyleBuilder
    {
        private class Declaration : Tuple<string, string>
        {
            private readonly static Regex _declarationRegex = new Regex(@"^(?<property>[^:]+):\s*(?<value>.+)$");

            public static Declaration Parse(string declaration)
            {
                var match = _declarationRegex.Match(declaration);
                if (!match.Success)
                {
                    throw new NotSupportedException();
                }
                return new Declaration(match.Groups["property"].Value, match.Groups["value"].Value);
            }

            public Declaration(string property, string value)
                : base(property, value)
            {
            }

            public string Property => Item1;
            public string Value => Item2;
        }


        internal readonly static Regex _rgbColorRegex = new Regex(@"^rgb\((?<red>\d+),(?<green>\d+),(?<blue>\d+)\)$");
        internal readonly static Regex _colorRegex = new Regex(@"(?<name>black|silver|gray|white|maroon|red|purple|fuchsia|green|lime|olive|yellow|navy|blue|teal|aqua)|(?<hex>#(?<hexRed>[0-9A-Fa-f]{2})(?<hexGreen>[0-9A-Fa-f]{2})(?<hexBlue>[0-9A-Fa-f]{2}))|(?<rgb>rgb\((?<red>\d+)\s*,\s*(?<green>\d+)\s*,\s*(?<blue>\d+)\))");

        private readonly XSSFWorkbook _workbook;
        private readonly IBasicLog _basicLog;
        private readonly Dictionary<string, Action<XSSFCellStyle, string>> _cellStyleInitializerByProperty;
        private readonly Dictionary<string, Action<XSSFFont, string>> _fontInitializerByProperty;
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
            _fontInitializerByProperty = new Dictionary<string, Action<XSSFFont, string>>
            {
                { "font-weight", (font, arg) => font.IsBold = arg == "bold" },
                { "font-style", (font, arg) => font.IsItalic = arg == "italic" },
                { "color", (font, arg) => font.SetColor(Color(NormalizeColor(arg))) },
            };
            _cellStyleInitializerByProperty = new Dictionary<string, Action<XSSFCellStyle, string>>
            {
                { "data-format", (cellStyle, arg) => cellStyle.SetDataFormat(DataFormat(DataFormatString(arg))) },
                { "border-top-color", (cellStyle, arg) => cellStyle.SetBottomBorderColor(Color(NormalizeColor(arg))) },
                { "border-right-color", (cellStyle, arg) => cellStyle.SetBottomBorderColor(Color(NormalizeColor(arg))) },
                { "border-bottom-color", (cellStyle, arg) => cellStyle.SetBottomBorderColor(Color(NormalizeColor(arg))) },
                { "border-left-color", (cellStyle, arg) => cellStyle.SetBottomBorderColor(Color(NormalizeColor(arg))) },
                { "border-top-style", (cellStyle, arg) => cellStyle.BorderBottom = BorderStyle(arg) },
                { "border-right-style", (cellStyle, arg) => cellStyle.BorderBottom = BorderStyle(arg) },
                { "border-bottom-style", (cellStyle, arg) => cellStyle.BorderBottom = BorderStyle(arg) },
                { "border-left-style", (cellStyle, arg) => cellStyle.BorderBottom = BorderStyle(arg) },
                { "fill-foreground-color", (cellStyle, arg) => cellStyle.SetFillForegroundColor(Color(NormalizeColor(arg))) },
                { "fill-background-color", (cellStyle, arg) => cellStyle.SetFillBackgroundColor(Color(NormalizeColor(arg))) },
                { "fill-pattern", (cellStyle, arg) => cellStyle.FillPattern = FillPattern(arg) },
                { "text-indent", (cellStyle, arg) => cellStyle.Indention = short.Parse(arg) },
                { "text-align", (cellStyle, arg) => cellStyle.Alignment = HorizontalAlignment(arg) },
                { "vertical-align", (cellStyle, arg) => cellStyle.VerticalAlignment = VerticalAlignment(arg) },
            };
            _ruleSets = new Dictionary<string, string[]>();
            _cellStyleCache = new Dictionary<string, XSSFCellStyle>();
            _fontCache = new Dictionary<string, XSSFFont>();
            _colorCache = new Dictionary<string, XSSFColor>();
            _dataFormatCache = new Dictionary<string, short>();
        }

        public void AddRuleSet(string ruleSetName, params string[] declarations)
        {
            _ruleSets.Add(ruleSetName, declarations);
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

                var cellStyleDeclarations = new List<Declaration>();
                var fontDeclarations = new List<Declaration>();

                ExtractRules(ruleSetNames, cellStyleDeclarations, fontDeclarations);

                ApplyCellStyleRules(cellStyle, cellStyleDeclarations);
                ApplyFontRules(cellStyle, fontDeclarations);

                _cellStyleCache.Add(cellStyleKey, cellStyle);
            }
            return cellStyle;
        }

        private void ExtractRules(IEnumerable<string> ruleSetNames, List<Declaration> cellStyleDeclarations, List<Declaration> fontDeclarations)
        {
            foreach (var ruleSetName in ruleSetNames)
            {
                if (!_ruleSets.TryGetValue(ruleSetName, out var declarations))
                {
                    _basicLog?.Debug($"Rule set not found: {ruleSetName}");
                    continue;
                }
                foreach (var declaration in declarations.Select(Declaration.Parse))
                {
                    if (_cellStyleInitializerByProperty.ContainsKey(declaration.Property))
                    {
                        cellStyleDeclarations.Add(declaration);
                    }
                    else if (_fontInitializerByProperty.ContainsKey(declaration.Property))
                    {
                        fontDeclarations.Add(declaration);
                    }
                    else
                    {
                        _basicLog?.Debug($"Declaration property handler not found");
                    }
                }
            }
        }

        private void ApplyCellStyleRules(XSSFCellStyle cellStyle, List<Declaration> cellStyleDeclarations)
        {
            var hasBorderHack = false;

            foreach (var declaration in cellStyleDeclarations)
            {
                // HACK: workaround for NPOI issue
                if (declaration.Property.StartsWith("border-") && !hasBorderHack)
                {
                    _cellStyleInitializerByProperty["border-diagonal"](cellStyle, "thin");
                    hasBorderHack = true;
                }

                _basicLog?.Debug($"Initializing cell style: {declaration}");
                _cellStyleInitializerByProperty[declaration.Property](cellStyle, declaration.Value);
            }

            if (hasBorderHack)
            {
                _cellStyleInitializerByProperty["border-diagonal"](cellStyle, "none");
            }
        }

        private void ApplyFontRules(XSSFCellStyle cellStyle, List<Declaration> fontRules)
        {
            if (fontRules.Any())
            {
                var fontKey = string.Join("|", fontRules);

                if (!_fontCache.TryGetValue(fontKey, out var font))
                {
                    _basicLog?.Debug($"Font not found in cache. Creating: {fontKey}");
                    font = (XSSFFont)_workbook.CreateFont();
                    font.FontHeightInPoints = 11;
                    foreach (var declaration in fontRules)
                    {
                        _basicLog?.Debug($"Initializing font: {declaration}");
                        _fontInitializerByProperty[declaration.Property](font, declaration.Value);
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

        private static BorderStyle BorderStyle(string borderStyle)
        {
            return Enum.TryParse(borderStyle, true, out BorderStyle result) ? result : throw new NotSupportedException();
        }

        private static FillPattern FillPattern(string fillPattern)
        {
            return Enum.TryParse(fillPattern, true, out FillPattern result) ? result : throw new NotSupportedException();
        }

        private static HorizontalAlignment HorizontalAlignment(string horizontalAlignment)
        {
            return Enum.TryParse(horizontalAlignment, true, out HorizontalAlignment result) ? result : throw new NotSupportedException();
        }

        private static VerticalAlignment VerticalAlignment(string verticalAlignment)
        {
            return Enum.TryParse(verticalAlignment, true, out VerticalAlignment result) ? result : throw new NotSupportedException();
        }

        private static string DataFormatString(string name)
        {
            switch (name)
            {
                case "number":
                    return "_ * #,##0_ ; * -#,##0_ ;_ * \"-\"_ ;_ @_ ";
                case "money":
                    return "#,##0.00";
                case "date":
                    return "yyyy-MM-dd";
                case "datetime":
                    return "yyyy-MM-dd HH:mm:ss";
                case "percent":
                    return "0.00%";
                default:
                    return name;
            }
        }

        internal static IEnumerable<string> Atomize(string declaration)
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
                return $"rgb({hexBytes[0]},{hexBytes[1]},{hexBytes[2]})"; ;
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
