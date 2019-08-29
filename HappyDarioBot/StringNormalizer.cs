using System.Globalization;
using System.Linq;
using System.Text;

namespace HappyDarioBot
{
    public class StringNormalizer
    {
        public string Normalize(string input) => 
            string.Concat(
                    input
                        .ToLower()
                        .Normalize(NormalizationForm.FormD)
                        .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                )
                .Normalize(NormalizationForm.FormC);
    }
}