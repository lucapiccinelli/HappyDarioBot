using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HappyDarioBot
{
    public class StringNormalizer
    {
        public string Normalize(string input) => 
            string.Concat(
                    Regex.Replace(input, "[^a-zA-Z\\u00C0-\\u017F]", "")
                        .ToLower()
                        .Replace('y', 'i')
                        .Normalize(NormalizationForm.FormD)
                        .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                )
                .Normalize(NormalizationForm.FormC);
    }
}