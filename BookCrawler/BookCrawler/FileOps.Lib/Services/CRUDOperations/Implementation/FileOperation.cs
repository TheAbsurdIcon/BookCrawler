using FileOps.Lib.Services.CRUDOperations.Interface;
using FileOps.Lib.Services.Logging.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOps.Lib.Services.CRUDOperations.Implementation
{
    public class FileOperation  : IFileOperation
    {
        private readonly IInformationLogger _informationLogger;

        public FileOperation(IInformationLogger informationLogger)
        {
            _informationLogger = informationLogger;
        }
        public List<string> ReadFileIntoMemory(string path, char [] separators, bool ascendingFlag = false, int length = 0)
        {
            List<string> fileContents = new List<string>();
            try
            {
                if (ascendingFlag)
                {
                    fileContents = File.ReadAllText(path, Encoding.UTF8).Split(separators, StringSplitOptions.TrimEntries).ToList().Distinct().Where(a => a.Length > length).OrderBy(x => x.Length).ToList();
                }
                else
                {
                    fileContents = File.ReadAllText(path, Encoding.UTF8).Split(separators, StringSplitOptions.TrimEntries).ToList().Distinct().Where(a => a.Length > length).OrderByDescending(x => x.Length).ToList();
                }
                return fileContents;
            }
            catch (Exception ex)
            {
                _informationLogger.LogException(ex);
                throw;

            }
        }
    }
}
