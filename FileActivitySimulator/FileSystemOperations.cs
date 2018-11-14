using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileActivitySimulator
{
    class FileSystemOperations
    {
        
        public List<string> FileSystemOperationTypes = new List<string>{"Read","Write","Delete","Rename","Create"};
        public string OperationType;


        public string GetRandomOperationType()
        {
            var random = new Random();
            int index = random.Next(FileSystemOperationTypes.Count);
            return (FileSystemOperationTypes[index]);
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

    }
}
