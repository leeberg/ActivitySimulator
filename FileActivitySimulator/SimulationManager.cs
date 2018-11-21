using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileActivitySimulator
{
    public class SimulationManager
    {
        public bool DoFileReads = false;
        public bool DoFileWrites = false;

        //Todo Implement
        public bool DoFileCreates = false;
        public bool DoFileDeletes = false;
        public bool DoFileRenames = false;

        public List<string> FileSystemOperationTypes = new List<string> { "Read", "Write", "Delete", "Rename", "Create" };



    }
}
