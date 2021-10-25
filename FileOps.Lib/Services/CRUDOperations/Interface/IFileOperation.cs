using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOps.Lib.Services.CRUDOperations.Interface
{
    public interface  IFileOperation
    {
        List<string> ReadFileIntoMemory(string path, char[] separators, bool ascendingFlag = false, int length = 0);
    }
}
