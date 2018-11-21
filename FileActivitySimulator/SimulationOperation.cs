using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileActivitySimulator
{
    class SimulationOperation
    {
        public string name;
        public string operationType;
        public bool isSensitive;

        // Cred Object
        public CredentialManager OperationCredential = new CredentialManager();

        // AD Object
        public ADSimulationObject ADSimulationObject = new ADSimulationObject();

        // File Object
        public FileSimulationObject FileSimulationObject = new FileSimulationObject();

        
    }
}
