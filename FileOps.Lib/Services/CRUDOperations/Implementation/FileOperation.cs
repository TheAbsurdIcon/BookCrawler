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
                if (!ascendingFlag)
                {

                    fileContents = File.ReadAllText(path, Encoding.UTF8)
                                        .ToLower()
                                        .Split(     separators, StringSplitOptions.TrimEntries)
                                        .GroupBy(   word => word, (key) => new { Word = key })
                                        .Select(    word => new { word, count = word.Count() })
                                        .OrderByDescending(word => word.count)
                                        .Where(     word => !string.IsNullOrWhiteSpace(word.word.Key))
                                        .Where(word=> word.word.Key.Length > length)
                                        .Select(    word => $"{word.word.Key} \t:\t {word.count.ToString()}")
                                        .ToList();                    
                }
                else
                {
                    fileContents = File.ReadAllText(path, Encoding.UTF8)
                                        .ToLower()
                                        .Split(     separators, StringSplitOptions.TrimEntries)
                                        .GroupBy(   word => word, (key) => new { Word = key })
                                        .Select(    word => new { word, count = word.Count() })
                                        .OrderBy(   word => word.count)
                                        .Where(     word => !string.IsNullOrWhiteSpace(word.word.Key))
                                        .Where(     word => word.word.Key.Length > length)
                                        .Select(    word => $"{word.word.Key} \t:\t {word.count.ToString()}")
                                        .ToList();
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
