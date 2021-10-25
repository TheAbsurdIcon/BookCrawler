using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManipulation.Lib.Services.Interface
{
    public interface IDataManipulation
    {
        void Execute(string path, char[] separators, bool flag, int length, int numberOfWords);
    }
}
