using System;

using Android.App;
using Android.Content;
using Android.Preferences;
using System.Text;
using System.Linq;
using Java.Util;
using Android.Content.Res;
using System.Collections.Generic;

namespace Ayadi.Droid
{
    public class XmlDb
    {
        Context _Ctx;
        ISharedPreferences prefs;

        public XmlDb(Context ctx)
        {
            _Ctx = ctx;
            prefs = PreferenceManager.GetDefaultSharedPreferences(_Ctx);
        }

        public bool SaveLangId(string LangId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(LangId))
                {
                    ISharedPreferencesEditor editor = prefs.Edit();
                    editor.PutString("LangId", LangId);
                    editor.Apply();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public string getSavedLangId()
        {
            try
            {
                return prefs.GetString("LangId", "0");
            }
            catch
            {
                return "0";
            }
        }

        public bool SaveProductsStyle(string styleName)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(styleName))
                {
                    ISharedPreferencesEditor editor = prefs.Edit();
                    editor.PutString("styleName", styleName);
                    editor.Apply();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public string GetProductsStyle()
        {
            try
            {
                return prefs.GetString("styleName", "Grid");
            }
            catch
            {
                return "Grid";
            }
        }
    }
}