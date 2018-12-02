using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileActivitySimulator
{
    public class SimulationManager
    {

        //Do File Stuff
        public bool DoFileReads = false;
        public bool DoFileWrites = false;
        public bool DoFileCreates = false;
        public bool DoFileDeletes = false;
        public bool DoFileRenames = false;

        public List<string> FileSystemOperationTypes = new List<string> { "Read", "Write", "Delete", "Rename", "Create" };


        //Do AD Stuff
        public bool DoADAuth = false;
        public bool DoADCreate = false;
        public bool DoADUpdate = false;



        // File Threats
        public bool DoRansomwareThreat = false;


        // AD Threats
        public bool DoAdminSDHolderThreat = false;
        public bool DoOtherTtypeofThreat = false;
        public bool DoGoldenTicketThreat = false;
        public bool DoDCSyncThreat = false;





    }
}
