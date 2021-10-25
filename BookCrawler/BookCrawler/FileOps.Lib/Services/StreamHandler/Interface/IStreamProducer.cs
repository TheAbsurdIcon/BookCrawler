using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOps.Lib.Services.StreamHandler.Interface
{
    public interface  IStreamProducer
    {
        Stream RetrieveStream();
    }
}
