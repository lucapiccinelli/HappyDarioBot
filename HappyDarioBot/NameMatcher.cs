using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace HappyDarioBot
{
    public class NameMatcher
    {
        private readonly string[] _namesList;

        public NameMatcher(string[] namesList)
        {
            _namesList = namesList;
        }

        public String Match(string name)
        {
            string normalizedName = Normalize(name);
            return _namesList
                       .Select(nameInList => new {OriginalName = nameInList, normalizedName = Normalize(nameInList)})
                       .FirstOrDefault(nameInList => nameInList.normalizedName.Equals(normalizedName, StringComparison.InvariantCultureIgnoreCase))
                       ?.OriginalName
                   ?? "";
        }

        private string Normalize(string nameInList) => 
            Path.GetFileNameWithoutExtension(RemoveDiacritics(nameInList));

        string RemoveDiacritics(string text) => 
            string.Concat(
                text.Normalize(NormalizationForm.FormD)
                    .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
            ).Normalize(NormalizationForm.FormC);
    }
}