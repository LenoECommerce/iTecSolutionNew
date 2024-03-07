using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EigenbelegToolAlpha
{
    public static class EnvironmentExtensions
    {
        public static string GetExecutingDirectoryName()

        {

            var assembly = Assembly.GetEntryAssembly();

            if (assembly == null)

                throw new Exception("Could not get entry assembly!");



            var codeBase = assembly.GetName().CodeBase;



            if (string.IsNullOrEmpty(codeBase))

                throw new Exception("Could not get codebase from entry assembly!");



            var location = new Uri(codeBase);



            var fi = new FileInfo(location.AbsolutePath);



            return fi == null

                ? throw new Exception($"Could not get file info on {location.AbsolutePath}")

                : fi.Directory == null

                    ? throw new Exception($"Could not get directory on {location.AbsolutePath}")

                    : fi.Directory.FullName.Replace("%20"," ");


        }
    }
}
