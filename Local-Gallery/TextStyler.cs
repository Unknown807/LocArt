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
        public static IEnumerable<TextRange> getAllKeyWords(FlowDocument document)
        {

            string pattern = @"#[A-Za-z0-9]+";
            string textRun;

            TextPointer pointer, start, end;
            Match match;

            int startIndex, length;

            pointer = document.ContentStart;

            while (pointer != null)
            {
                if (pointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    textRun = pointer.GetTextInRun(LogicalDirection.Forward);
                    match = Regex.Match(textRun, pattern);

                    if (match.Success)
                    {
                        startIndex = match.Index;
                        length = match.Length;
                        start = pointer.GetPositionAtOffset(startIndex);
                        end = start.GetPositionAtOffset(length);

                        yield return new TextRange(start, end);
                    }

                }

                pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
            }
        }

        public static void applyKeyWordStyling(IEnumerable<TextRange> words, SolidColorBrush color)
        {
            foreach(TextRange word in words)
            {
                word.ApplyPropertyValue(TextElement.ForegroundProperty, color);
            }
        }
    }
}
