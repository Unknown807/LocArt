using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace Utilities
{
    static class TextStyler
    {
        public static void applyKeyWordStyling(FlowDocument document)
        {
            string pattern = @"#([A-Za-z0-9])+";
            string textRun;

            TextPointer pointer, start, end;
            MatchCollection matches;

            int startIndex, length;
           
            pointer = document.ContentStart;

            while (pointer != null)
            {
                if (pointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    textRun = pointer.GetTextInRun(LogicalDirection.Forward);
                    matches = Regex.Matches(textRun, pattern);

                    foreach (Match match in matches)
                    {
                        startIndex = match.Index;
                        length = match.Length;
                        start = pointer.GetPositionAtOffset(startIndex);
                        end = start.GetPositionAtOffset(length);

                        new TextRange(start, end).ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.DeepSkyBlue);
                    }
                }

                pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
            }
        }
    }
}
