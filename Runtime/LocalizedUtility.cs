using System;

namespace NooboPackage.NooboLocalize.Runtime
{
    public static class LocalizedUtility
    {
        internal static string Localized(this int num, string format = "")
        {
            if (!NooboLocalizeSettings.CurrentLocale().overrideNumberCharacterSet)
                return num.ToString(format);

            var offset = NooboLocalizeSettings.CurrentLocale().zeroCharacterCode;
            var s = "";
            foreach (var t in num.ToString(format))
                if(t >= 48 && t <= 58)
                    s += Convert.ToChar(int.Parse(t + "") + offset);
                else
                    s += t;
            
            return s;
        }
    }
}
