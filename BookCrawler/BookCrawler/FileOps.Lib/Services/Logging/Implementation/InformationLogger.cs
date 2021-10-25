using FileOps.Lib.Services.Logging.Interface;
using FileOps.Lib.Services.StreamHandler.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOps.Lib.Services.Logging.Implementation
{
    public class InformationLogger : IInformationLogger
    {
        private readonly IStreamProducer _streamProducer;
        private static string ExceptionFile = $"{Directory.GetCurrentDirectory()}\\Exception_{DateTime.UtcNow.ToString("yyyyMMdd")}.txt";

        public InformationLogger(IStreamProducer streamProducer)
        {
            _streamProducer = streamProducer;
        }

        public void LogException(Exception ex, string path = "default")
        {
            try
            {
                if(path == "default")
                {
                    Console.WriteLine(Encoding.ASCII.GetBytes($"Time when error occured : {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}\n"));
                    Console.WriteLine(Encoding.ASCII.GetBytes($"Exception: {ex.Message}\n"));
                    Console.WriteLine(Encoding.ASCII.GetBytes($"StackTrace: {ex.StackTrace ?? ""}\n"));
                }
                else
                {
                    using (Stream stream = _streamProducer.RetrieveStream())
                    {
                        stream.Write(Encoding.ASCII.GetBytes($"Time when error occured : {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}\n"));
                        stream.Write(Encoding.ASCII.GetBytes($"Exception: {ex.Message}\n"));
                        stream.Write(Encoding.ASCII.GetBytes($"StackTrace: {ex.StackTrace ?? ""}\n"));
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public void LogData(List<string> data, string path)
        {
            try
            {
                using (var file = File.Exists(path) ? File.Open(path, FileMode.Append) : File.Open(path, FileMode.CreateNew))
                using (Stream stream = _streamProducer.RetrieveStream())
                {
                    foreach (var item in data)
                    {
                        stream.Write(Encoding.ASCII.GetBytes(item));
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                throw;
            }
        }
    }
}
