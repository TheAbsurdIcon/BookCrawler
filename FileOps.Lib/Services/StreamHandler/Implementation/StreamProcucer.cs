using FileOps.Lib.Services.StreamHandler.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOps.Lib.Services.StreamHandler.Implementation
{
    public class StreamProcucer : IStreamProducer
    {
        #region Constructor, Injectables & Properties
        private readonly Stream _streamWriter;

        public StreamProcucer(Stream streamWriter)
        {
            _streamWriter = streamWriter;
        }

        #endregion
        public Stream RetrieveStream()
        {
            return _streamWriter;
        }
    }
}
