using SimpleImpersonation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FileActivitySimulator
{
    public class FileSystemOperation
    {
        
        public List<string> FileSystemOperationTypes = new List<string>{"Read","Write","Delete","Rename","Create"};
        public string OperationType;
        public NetworkCredential OperationCredential;


        // uses SimpleImpersonation https://github.com/mj1856/SimpleImpersonation
        //For local computer users, you can either pass the computer's machine name or . to the domain parameter, or omit the domain parameter and just pass the username by itself.

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
                Console.WriteLine("Picked a type of: " + AvailibleFileSystemOperationTypes[index]);
                return (AvailibleFileSystemOperationTypes[index]);
                
            }
            else
            {
                return "NONE";
            }
            

        }




        public void DeleteFile(string path)
        {

            bool fileExists = File.Exists(path);

            if (OperationCredential != null)
            {

                var credentials = new UserCredentials(OperationCredential.Domain, OperationCredential.UserName, OperationCredential.Password);

                var result = Impersonation.RunAsUser(credentials, LogonType.Interactive, () =>
                {

                    if (fileExists)
                    {
                        // do whatever you want as this user.
                        System.IO.File.Delete(path);

                      
                    }

                    return "OK";


                });
            }
            else
            {

                if (fileExists)
                {
                    // do whatever you want as this user.
                        try
                        {
                            System.IO.File.Delete(path);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    
                }


            }



        }






        public void UpdateFile(string path)
        {
           
           
            if(OperationCredential != null)
            {
                var credentials = new UserCredentials(OperationCredential.Domain, OperationCredential.UserName, OperationCredential.Password);


                var result = Impersonation.RunAsUser(credentials, LogonType.Interactive, () =>
                {
                    // do whatever you want as this user.
                    using (StreamWriter streamWriter = new StreamWriter(path, false))
                    {
                        streamWriter.WriteLine(DateTime.Now.ToString());
                    }

                    return "OK";

                });
            }
            else
            {
                using (StreamWriter streamWriter = new StreamWriter(path, false))
                {
                    streamWriter.WriteLine(DateTime.Now.ToString());
                }
            }
            


        }

        public void RenameFile(string path)
        {

            if (OperationCredential != null)
            {
                var credentials = new UserCredentials(OperationCredential.Domain, OperationCredential.UserName, OperationCredential.Password);

                var result = Impersonation.RunAsUser(credentials, LogonType.Interactive, () =>
                {
                // do whatever you want as this user.
                using (StreamWriter streamWriter = new StreamWriter(path, false))
                {
                        // Get Current Extension...
                        string currentFileExtension = path.Substring(path.Length - 4);

                        // Get Random Extension Type
                        var random = new Random();
                        var extensions = new List<string>{".docx",".txt",".xlsx",".pptx"};
                        int index = random.Next(extensions.Count);

                        // Rename
                        string newPath = (path).Replace(currentFileExtension, extensions[index]);
                        try
                        {
                            System.IO.File.Move(path, newPath);
                        }
                        catch
                        {
                            //can't touch this
                        }
                }

                    return "OK";

                });

            }
            else
            {
                // No User Specified
                using (StreamWriter streamWriter = new StreamWriter(path, false))
                {
                    // Get Current Extension...
                    string currentFileExtension = path.Substring(path.Length - 4);

                    // Get Random Extension Type
                    var random = new Random();
                    var extensions = new List<string> { ".docx", ".txt", ".xlsx", ".pptx" };
                    int index = random.Next(extensions.Count);
                                 

                    // Rename
                    string newPath = (path).Replace(currentFileExtension, extensions[index]);

                    Console.WriteLine("Trying Rename: " + path +  " to: " + newPath);

                    try
                    {
                        System.IO.File.Move(path, newPath);
                    }
                    catch
                    {
                        //can't touch this
                    }
                }



            };
        }

           
           
        public void ReadFile(string path)
        {

            if (OperationCredential != null)
            {

                var credentials = new UserCredentials(OperationCredential.Domain, OperationCredential.UserName, OperationCredential.Password);

                Console.WriteLine("domain:" + OperationCredential.Domain);
                Console.WriteLine("user:" + OperationCredential.UserName);
                Console.WriteLine("pass:" + OperationCredential.Password);


                try
                {

                
                    
                    var result = Impersonation.RunAsUser(credentials, LogonType.Interactive, () =>
                    {
                            // do whatever you want as this user.
                            System.IO.File.OpenRead(path);

                        return "OK";

                    });
                }
                catch (SimpleImpersonation.ImpersonationException IE)
                {
                    Console.WriteLine(IE.Data);
                    Console.WriteLine(IE.InnerException);
                    Console.WriteLine(IE.ErrorCode);
                    Console.WriteLine(IE.Message);
                    Console.WriteLine(IE.NativeErrorCode);

                }
            }
            else
            {
                // do whatever you want as this user.

                try
                {
                    System.IO.File.OpenRead(path);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                

                
            }
        }


        public void createFile(string path)
        {

            bool fileExists = File.Exists(path);

            //If File Exists add some numbers to the extension

            if(fileExists)
            {
                // Get Current Extension...
                string currentFileExtension = path.Substring(path.Length - 4);

                // Get New End Of File
                string fileTimeStamp = DateTime.Now.ToString("yyyyMMddTHHmmss");
                
                // Set New Path for File Create
                path = (path).Replace(currentFileExtension, (fileTimeStamp + currentFileExtension));

            }

            Console.WriteLine("Attempting to Create File: " + path);

            if (OperationCredential != null)
            {

                var credentials = new UserCredentials(OperationCredential.Domain, OperationCredential.UserName, OperationCredential.Password);


                var result = Impersonation.RunAsUser(credentials, LogonType.Interactive, () =>
                    {
                        // do whatever you want as this user.
                        System.IO.File.Create(path);

                        return "OK";

                    });
            }
            else
            {
                // do whatever you want as this user.
                if (fileExists)
                {
                    try
                    {
                        System.IO.File.Create(path);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                


            }
        }


        public void simulateRansomware(string path)
        {

            bool fileExists = File.Exists(path);

            //If File Exists add some numbers to the extension

            if (fileExists)
            {
                // Get Current Extension...
                string currentFileExtension = path.Substring(path.Length - 4);

                // Get New End Of File
                string fileTimeStamp = DateTime.Now.ToString("yyyyMMddTHHmmss");

                // Set New Path for File Create
                path = (path).Replace(currentFileExtension, (fileTimeStamp + currentFileExtension));

            }

            Console.WriteLine("Attempting to Create File: " + path);

            if (OperationCredential != null)
            {

                var credentials = new UserCredentials(OperationCredential.Domain, OperationCredential.UserName, OperationCredential.Password);


                var result = Impersonation.RunAsUser(credentials, LogonType.Interactive, () =>
                {
                    // do whatever you want as this user.
                    System.IO.File.Create(path);

                    return "OK";

                });
            }
            else
            {
                // do whatever you want as this user.
                if (fileExists)
                {
                    try
                    {
                        System.IO.File.Create(path);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }



            }
        }


    }
}
