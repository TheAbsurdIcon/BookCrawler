using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOps.Lib.Services.Logging.Interface
{
    public interface  IInformationLogger
    {
        void LogException(Exception ex, string path = "default");
        void LogData(List<string> data, string path);
    }
}
