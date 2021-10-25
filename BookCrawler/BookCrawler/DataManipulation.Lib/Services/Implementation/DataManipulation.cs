using DataManipulation.Lib.Services.Interface;
using FileOps.Lib.Services.CRUDOperations.Interface;
using FileOps.Lib.Services.Logging.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManipulation.Lib.Services.Implementation
{
    public class DataManipulation : IDataManipulation
    {
        private readonly IFileOperation _fileOperation;
        private readonly IInformationLogger _informationLogger;

        public DataManipulation(IInformationLogger informationLogger, IFileOperation fileOperation)
        {
            _fileOperation = fileOperation;
            _informationLogger = informationLogger;
        }

        public void Execute(string path, char[] separators, bool flag, int length, int numberOfWords)
        {
            try
            {
                List<string> contents = _fileOperation.ReadFileIntoMemory(path, separators, flag, length);
                System.Console.WriteLine($"Count \t\tWord\t\tLength");
                System.Console.WriteLine("===============================================");
                int count = 1;

                foreach (var item in contents)
                {
                    System.Console.WriteLine($"{item}");
                    count++;

                    if (count > numberOfWords)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _informationLogger.LogException(ex);
                throw;
            }
        }

    }
}
