using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileActivitySimulator
{
    public class FileSimulationObject
    {
        public string name;
        public string path;
        public string fileType;
        public bool isSensitive;

        public FileSystemOperation FileSystemOperation = new FileSystemOperation();



    }
}
