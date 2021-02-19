using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace WitchMage
{
    public  class FileSystemOperations
    {

    
        public static StepSequence LoadConfigFromFile()
        {

            StepSequence loadedStepSequence;


            if (File.Exists(SetupFiles.ConfigFile))
            {
                string fileContents = File.ReadAllText(SetupFiles.ConfigFile);

                loadedStepSequence = Newtonsoft.Json.JsonConvert.DeserializeObject<StepSequence>(fileContents);

                return loadedStepSequence;

            }
            else
            {
                return null;
            }


        }





            public static void ExecuteProcess(string processpath, string parameters, bool asAdmin)
        {
            ProcessStartInfo psi = new ProcessStartInfo(processpath);

            string path = Directory.GetCurrentDirectory();

            psi.RedirectStandardOutput = false;
            psi.RedirectStandardError = false;
            psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            psi.UseShellExecute = true;
            if(asAdmin==true)
            {
                psi.Verb = "runas";
            }
            
            try
            { 
            Process newprocess = System.Diagnostics.Process.Start(psi);
            }
            catch
            {

            }

        }
    }
}
