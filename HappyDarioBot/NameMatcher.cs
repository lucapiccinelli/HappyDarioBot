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
        private readonly StringNormalizer _stringNormalizer;

        public NameMatcher(string[] namesList)
        {
            _namesList = namesList;
            _stringNormalizer = new StringNormalizer();
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
            Path.GetFileNameWithoutExtension(_stringNormalizer.Normalize(nameInList));
    }
}