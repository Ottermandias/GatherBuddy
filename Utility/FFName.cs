using Dalamud.Plugin;
using Dalamud;
using System;

namespace Otter
{
    public class FFName
    {
        public FFName()
        { }

        public string this[ClientLanguage lang]
        {
            get { return Name(lang); }
            set { SetName(lang, value); }
        }

        public bool AnyEmpty()
        {
            foreach (ClientLanguage lang in Enum.GetValues(typeof(ClientLanguage)))
                if (Name(lang) == null || Name(lang).Length == 0)
                    return true;
            return false;
        }

        public override string ToString()
        {
            return $"{Name(ClientLanguage.English)}|{Name(ClientLanguage.German)}|{Name(ClientLanguage.French)}|{Name(ClientLanguage.Japanese)}";
        }

        static public FFName FromPlaceName(DalamudPluginInterface pi, uint id)
        {
            var name = new FFName();
            foreach (ClientLanguage lang in Enum.GetValues(typeof(ClientLanguage)))
            {
                var row = pi.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.PlaceName>(lang).GetRow(id);
                name[lang] = row?.Name ?? "";
            }
            return name;
        }

        #region private
        private string Name(ClientLanguage lang)
        {
            return nameList[(int) lang];
        }

        private void SetName(ClientLanguage lang, string name)
        {
            if (lang == ClientLanguage.German || lang == ClientLanguage.French)
                name = Util.RemoveSplitMarkers(name);
            nameList[(int) lang] = Util.RemoveItalics(name);
        }

        private readonly string[] nameList = new string[4] { "", "", "", "" };
        #endregion
    }
}