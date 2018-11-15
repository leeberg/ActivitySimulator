using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileActivitySimulator
{
    public class FileSystemOperation
    {
        
        public List<string> FileSystemOperationTypes = new List<string>{"Read","Write","Delete","Rename","Create"};
        public string OperationType;
       


        public string GetRandomOperationType(SimulationManager simManager)
        {
            List<string> AvailibleFileSystemOperationTypes = FileSystemOperationTypes;

            if (simManager.DoFileReads == false)
            {
                AvailibleFileSystemOperationTypes.Remove("Read");

            }

            if (simManager.DoFileWrites == false)
            {
                AvailibleFileSystemOperationTypes.Remove("Write");
            }

            if (simManager.DoFileDeletes == false)
            {
                AvailibleFileSystemOperationTypes.Remove("Delete");
            }

            if (simManager.DoFileRenames == false)
            {
                AvailibleFileSystemOperationTypes.Remove("Rename");
            }

            if (simManager.DoFileCreates == false)
            {
                AvailibleFileSystemOperationTypes.Remove("Create");
            }

            if(AvailibleFileSystemOperationTypes.Count > 0)
            {
                var random = new Random();
                int index = random.Next(AvailibleFileSystemOperationTypes.Count);
                return (AvailibleFileSystemOperationTypes[index]);
            }
            else
            {
                return "NONE";
            }
            

        }

        public void UpdateFile(string path)
        {
            using (StreamWriter streamWriter = new StreamWriter(path, false))
            {
                streamWriter.WriteLine(DateTime.Now.ToString());
            }
        }

        public void RenameFile(string path)
        {
            using (StreamWriter streamWriter = new StreamWriter(path, false))
            {
                streamWriter.WriteLine(DateTime.Now.ToString());
            }
        }


        public void ReadFile(string path)
        {
            System.IO.File.OpenRead(path);
        }

    }
}
