using MvvmCross.Localization;
using System.Globalization;
using System.Resources;
using System;

namespace Ayadi.Core.Utility
{
    public class ResxTextProvider : IMvxTextProvider
    {
        private readonly ResourceManager _resourceManager;

        public ResxTextProvider(ResourceManager resourceManager , string CultureInfoString)
        {
            _resourceManager = resourceManager;
            CurrentLanguage = new CultureInfo(CultureInfoString);
        }

        public CultureInfo CurrentLanguage { get; set; }

        public string GetText(string namespaceKey,
            string typeKey, string name)
        {
            string resolvedKey = name;

            if (!string.IsNullOrEmpty(typeKey))
            {
                resolvedKey = $"{typeKey}.{resolvedKey}";
            }

            if (!string.IsNullOrEmpty(namespaceKey))
            {
                resolvedKey = $"{namespaceKey}.{resolvedKey}";
            }

            if (name.EndsWith("_"))
            {
                // it means its public word not belong to any viewmodel
                resolvedKey = name;
            }
            return _resourceManager.GetString(resolvedKey, CurrentLanguage);
        }

        public string GetText(string namespaceKey, string typeKey, string name, params object[] formatArgs)
        {
            string baseText = GetText(namespaceKey, typeKey, name);

            if (string.IsNullOrEmpty(baseText))
            {
                return baseText;
            }

            return string.Format(baseText, formatArgs);
        }

        public bool TryGetText(out string textValue, string namespaceKey, string typeKey, string name)
        {
            throw new NotImplementedException();
        }

        public bool TryGetText(out string textValue, string namespaceKey, string typeKey, string name, params object[] formatArgs)
        {
            throw new NotImplementedException();
        }
    }
}
