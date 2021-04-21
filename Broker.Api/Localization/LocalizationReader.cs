using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoBank.Broker.Api.Localization
{
    public class LocalizationReader
    {
        public Dictionary<string, string> GetLocalizationFileContent(string language)
        {
            var localeFilePath = String.Format(@"Localization\{0}.txt", language);
            var localeFileLines = File.ReadAllLines(localeFilePath);
            var locale = new Dictionary<string, string>();
            foreach (var localeFileLine in localeFileLines)
            {
                if (localeFileLine.Length == 0) continue;
                var spaceIndex = localeFileLine.IndexOf(' ');
                var key = localeFileLine.Substring(0, spaceIndex);
                var value = localeFileLine.Substring(spaceIndex + 1);
                locale.Add(key, value);
            }

            return locale;
        }
    }
}
