using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Verse;

namespace BadPeople.Settings
{
    class BPSettings
    {
        private static BadPeoplePerfsData data;

        private static readonly string prefsFilePath =
            Path.Combine(GenFilePaths.ConfigFolderPath, "BadPeople_Perfs.xml");


        public static bool DebugTabVisible {
            get
            {
                return data.displayDebugPawnTab;
            }
            set
            {
                data.displayDebugPawnTab = value;
            }
         }

        public static void Init()
        {
            var flag = !new FileInfo(prefsFilePath).Exists;
            data = new BadPeoplePerfsData();
            data = DirectXmlLoader.ItemFromXmlFile<BadPeoplePerfsData>(prefsFilePath, true);
        }

        public static void Save()
        {
            
            try
            {
                var xDocument = new XDocument();
                var content = DirectXmlSaver.XElementFromObject(data, typeof(BadPeoplePerfsData));
                xDocument.Add(content);
                xDocument.Save(prefsFilePath);
            }
            catch (Exception ex)
            {
                GenUI.ErrorDialog("ProblemSavingFile".Translate(prefsFilePath, ex.ToString()));
                Log.Error("Exception saving prefs: " + ex);
            }
        }
    }
}
