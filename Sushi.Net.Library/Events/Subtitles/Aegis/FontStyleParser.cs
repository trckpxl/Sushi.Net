using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using Sushi.Net.Library.Common;

namespace Sushi.Net.Library.Events.Subtitles.Aegis
{
    public class FontStyleParser
    {

        List<string> fields=new List<string>();

        public FontStyleParser(string format_line)
        {
            fields = format_line.Split(new char[] { ',',':'}, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        }

        public override string ToString()
        {
            return $"{fields[0]}: {string.Join(", ", fields.Skip(1))}";
        }

        public FontStyle CreateFontStyle(string format_line)
        {
            FontStyle style = new FontStyle(this);
            
            // Split the line by commas and colons, but we need to handle the first separator differently
            int firstSeparatorIdx = format_line.IndexOf(':');
            if (firstSeparatorIdx < 0)
                throw new FormatException("Invalid style line format: missing colon separator");
            
            // Create a list to hold all values
            List<string> values = new List<string>();
            
            // Get the first value (Kind/Format)
            string firstValue = format_line.Substring(0, firstSeparatorIdx).Trim();
            values.Add(firstValue);
            
            // Process the rest of the line by splitting on commas
            string restOfLine = format_line.Substring(firstSeparatorIdx + 1);
            string[] remainingValues = restOfLine.Split(',');
            
            foreach (string val in remainingValues)
            {
                values.Add(val.Trim());
            }
            
            // Now process each value according to its field name
            for (int x = 0; x < fields.Count && x < values.Count; x++)
            {
                string par = values[x];
                
                switch (fields[x].ToUpperInvariant())
                {
                    case "KIND":
                    case "FORMAT":
                        style.Kind = par;
                        break;
                    case "NAME":
                        style.Name = par;
                        break;
                    case "FONTNAME":
                        style.FontName = par;
                        break;
                    case "FONTSIZE":
                        if (!string.IsNullOrEmpty(par))
                            style.FontSize = float.Parse(par, NumberStyles.Any, CultureInfo.InvariantCulture);
                        break;
                    case "PRIMARYCOLOUR":
                    case "PRIMARYCOLOR":
                        style.PrimaryColor = par;
                        break;
                    case "SECONDARYCOLOUR":
                    case "SECONDARYCOLOR":
                        style.SecondaryColor = par;
                        break;
                    case "TERTIARYCOLOUR":
                    case "TERTIARYCOLOR":
                    case "OUTLINECOLOUR":
                    case "OUTLINECOLOR":
                        style.OutlineColor = par;
                        break;
                    case "BACKCOLOUR":
                    case "BACKCOLOR":
                        style.BackColor = par;
                        break;
                    case "BOLD":
                        if (!string.IsNullOrEmpty(par))
                            style.Bold = int.Parse(par) != 0;
                        break;
                    case "ITALIC":
                        if (!string.IsNullOrEmpty(par))
                            style.Italic = int.Parse(par) != 0;
                        break;
                    case "UNDERLINE":
                        if (!string.IsNullOrEmpty(par))
                            style.Underline = int.Parse(par) != 0;
                        break;
                    case "STRIKEOUT":
                        if (!string.IsNullOrEmpty(par))
                            style.Strikeout = int.Parse(par) != 0;
                        break;
                    case "BORDERSTYLE":
                        if (!string.IsNullOrEmpty(par))
                            style.BorderStyle = int.Parse(par);
                        break;
                    case "SCALEX":
                        if (!string.IsNullOrEmpty(par))
                            style.ScaleX = float.Parse(par, NumberStyles.Any, CultureInfo.InvariantCulture);
                        break;
                    case "SCALEY":
                        if (!string.IsNullOrEmpty(par))
                            style.ScaleY = float.Parse(par, NumberStyles.Any, CultureInfo.InvariantCulture);
                        break;
                    case "ANGLE":
                        if (!string.IsNullOrEmpty(par))
                            style.Angle = float.Parse(par, NumberStyles.Any, CultureInfo.InvariantCulture);
                        break;
                    case "OUTLINE":
                        if (!string.IsNullOrEmpty(par))
                            style.Outline = float.Parse(par, NumberStyles.Any, CultureInfo.InvariantCulture);
                        break;
                    case "SHADOW":
                        if (!string.IsNullOrEmpty(par))
                            style.Shadow = float.Parse(par, NumberStyles.Any, CultureInfo.InvariantCulture);
                        break;
                    case "ALIGNMENT":
                        if (!string.IsNullOrEmpty(par))
                            style.Alignment = int.Parse(par, NumberStyles.Any, CultureInfo.InvariantCulture);
                        break;
                    case "SPACING":
                        if (!string.IsNullOrEmpty(par))
                            style.Spacing = float.Parse(par, NumberStyles.Any, CultureInfo.InvariantCulture);
                        break;
                    case "MARGINL":
                        if (!string.IsNullOrEmpty(par))
                            style.MarginLeft = int.Parse(par, NumberStyles.Any, CultureInfo.InvariantCulture);
                        break;
                    case "MARGINR":
                        if (!string.IsNullOrEmpty(par))
                            style.MarginRight = int.Parse(par, NumberStyles.Any, CultureInfo.InvariantCulture);
                        break;
                    case "MARGINV":
                        if (!string.IsNullOrEmpty(par))
                            style.MarginVertical = int.Parse(par, NumberStyles.Any, CultureInfo.InvariantCulture);
                        break;
                    case "ALPHALEVEL":
                        if (!string.IsNullOrEmpty(par))
                            style.AlphaLevel = float.Parse(par, NumberStyles.Any, CultureInfo.InvariantCulture);
                        break;
                    case "ENCODING":
                        if (!string.IsNullOrEmpty(par))
                            style.Encoding = int.Parse(par, NumberStyles.Any, CultureInfo.InvariantCulture);
                        break;
                }
            }
            
            return style;
        }


        public string CreateLine(FontStyle style)
        {
            StringBuilder bld = new StringBuilder();

            for (int x = 0; x < fields.Count; x++)
            {
                if (x  == 1)
                    bld.Append(": ");
                if (x > 1)
                    bld.Append(",");
                switch (fields[x].ToUpperInvariant())
                {
                    case "KIND":
                    case "FORMAT":
                        bld.Append(style.Kind);
                        break;
                    case "NAME":
                        bld.Append(style.Name);
                        break;
                    case "FONTNAME":
                        bld.Append(style.FontName);
                        break;
                    case "FONTSIZE":
                        bld.Append(style.FontSize.ToString(CultureInfo.InvariantCulture));
                        break;
                    case "PRIMARYCOLOUR":
                    case "PRIMARYCOLOR":
                        bld.Append(string.IsNullOrEmpty(style.PrimaryColor) ? "" : style.PrimaryColor);
                        break;
                    case "SECONDARYCOLOUR":
                    case "SECONDARYCOLOR":
                        bld.Append(string.IsNullOrEmpty(style.SecondaryColor) ? "" : style.SecondaryColor);
                        break;
                    case "TERTIARYCOLOUR":
                    case "TERTIARYCOLOR":
                    case "OUTLINECOLOUR":
                    case "OUTLINECOLOR":
                        bld.Append(string.IsNullOrEmpty(style.OutlineColor) ? "" : style.OutlineColor);
                        break;
                    case "BACKCOLOUR":
                    case "BACKCOLOR":
                        bld.Append(string.IsNullOrEmpty(style.BackColor) ? "" : style.BackColor);
                        break;
                    case "BOLD":
                        bld.Append(style.Bold ? "-1" : "0");
                        break;
                    case "ITALIC":
                        bld.Append(style.Italic ? "-1" : "0");
                        break;
                    case "UNDERLINE":
                        bld.Append(style.Underline ? "-1" : "0");
                        break;
                    case "STRIKEOUT":
                        bld.Append(style.Strikeout ? "-1" : "0");
                        break;
                    case "BORDERSTYLE":
                        bld.Append(style.BorderStyle);
                        break;
                    case "SCALEX":
                        bld.Append(style.ScaleX.ToString(CultureInfo.InvariantCulture));
                        break;
                    case "SCALEY":
                        bld.Append(style.ScaleY.ToString(CultureInfo.InvariantCulture));
                        break;
                    case "OUTLINE":
                        bld.Append(style.Outline.ToString(CultureInfo.InvariantCulture));                        
                        break;
                    case "SHADOW":
                        bld.Append(style.Shadow.ToString(CultureInfo.InvariantCulture));
                        break;
                    case "ANGLE":
                        bld.Append(style.Angle.ToString(CultureInfo.InvariantCulture));
                        break;
                    case "ALIGNMENT":
                        bld.Append(style.Alignment.ToString(CultureInfo.InvariantCulture));
                        break;
                    case "SPACING":
                        // Don't output spacing if it's the default value of 0
                        if (style.Spacing != 0)
                            bld.Append(style.Spacing.ToString(CultureInfo.InvariantCulture));
                        break;
                    case "MARGINL":
                        bld.Append(style.MarginLeft.ToString("D4"));
                        break;
                    case "MARGINR":
                        bld.Append(style.MarginRight.ToString("D4"));
                        break;
                    case "MARGINV":
                        bld.Append(style.MarginVertical.ToString("D4"));
                        break;
                    case "ALPHALEVEL":
                        bld.Append(style.AlphaLevel.ToString(CultureInfo.InvariantCulture));
                        break;
                    case "ENCODING":
                        bld.Append(style.Encoding.ToString(CultureInfo.InvariantCulture));
                        break;

   
                }
            }
            return bld.ToString();
        }
    }
}
