using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Localization
{
    public class LocalizedStrings
    {
        public LocalizedStrings() { }

        private static readonly Strings LocalizedStringsResources
            = new Strings();

        public Strings Strings => LocalizedStringsResources;
    }
}
