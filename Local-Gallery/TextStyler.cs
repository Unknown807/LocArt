using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using System.Windows.Media;

namespace Utilities
{
    static class TextStyler
    {
        /// <summary>
        /// Gets all keywords via regex
        /// </summary>
        /// <param name="document">The document object from the RichTextBox gallery item description</param>
        /// <returns>A enumerable of all the keywords found via regex</returns>
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

                    // If a keyword is found
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

        /// <param name="words">An enumerable of TextRanges (usually keywords from descriptions)</param>
        /// <param name="color">The color to apply to the passed in words</param>
        public static void applyKeyWordStyling(IEnumerable<TextRange> words, SolidColorBrush color)
        {
            foreach(TextRange word in words)
            {
                word.ApplyPropertyValue(TextElement.ForegroundProperty, color);
            }
        }
    }
}
