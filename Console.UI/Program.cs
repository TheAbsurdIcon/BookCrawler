using FileOps.Lib.Services.CRUDOperations.Implementation;
using FileOps.Lib.Services.Logging.Implementation;
using FileOps.Lib.Services.Logging.Interface;
using FileOps.Lib.Services.StreamHandler.Implementation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DataManipulation.Lib.Services.Interface;

namespace Console.UI
{
    class Program
    {
        #region Private members
        private static InformationLogger _informationLogger;
        private static StreamProcucer _streamProcucer;
        private static FileOperation _fileOperation;
        private static DataManipulation.Lib.Services.Implementation.DataManipulation _dataManipulation;
        private static string _exceptionFile;
        private static string _defaultPath;

        public static IConfiguration _configuration;

        #endregion

        static void Main(string[] args)
        {
            try
            {
                InitialiseMembers();

                char[] separators = new char[] { ' ', '.', ',', '-', '"', '!', '?', '(', ')', '/', '\\', ':', '[', ']', '—', ' ', '\r', '\n', '\'', '’', '‘', ';', '`', '”', '“', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '*' };
                bool ascendingFlag = (_configuration.GetSection("defaultSortOrder").Value == "true") ? true : false;
                int length = int.Parse(_configuration.GetSection("defaultMinimumLengthOfWord").Value);
                int numberOfWords = int.Parse(_configuration.GetSection("defaultNumberOfWords").Value);

                while (true)
                {
                    System.Console.WriteLine("Execute Program? Y/N");
                    var flag = System.Console.ReadLine();

                    while ((flag != "Y") && (flag != "y") && (flag != "N") && (flag != "n"))
                    {
                        System.Console.WriteLine("Please enter valid letter.");
                        flag = System.Console.ReadLine();
                    }

                    if (flag == "N" || flag == "n") break;
                    var watch = new Stopwatch();
                    watch.Start();

                    Run(_defaultPath, separators, ascendingFlag, length, numberOfWords);

                    watch.Stop();
                    System.Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms\n\n\n\n");
                }
                System.Console.WriteLine("Exiting");

            }
            catch (Exception ex)
            {
                _informationLogger.LogException(ex);
                System.Console.WriteLine($"Exception: { ex.Message}  StackTrace: {ex.StackTrace ?? ""}\n");
            }

        }

        static void InitialiseMembers()
        {

            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();

                _configuration = builder.Build();

                string exceptionFile = _configuration.GetSection("ExceptionFile").Value;
                string path = _configuration.GetSection("BookToRead").Value;
                _exceptionFile = (exceptionFile == "default") ? $"{Directory.GetCurrentDirectory()}\\Exception_{DateTime.UtcNow.ToString("yyyyMMdd")}.txt" : path;
                _defaultPath = (path == "default") ? $"{Directory.GetCurrentDirectory()}\\WarAndPeace.txt" : path;

                FileStream file = File.Exists(_exceptionFile) ? File.Open(_exceptionFile, FileMode.Append) : File.Open(_exceptionFile, FileMode.CreateNew);

                _streamProcucer = new StreamProcucer(file);
                _informationLogger = new InformationLogger(_streamProcucer);
                _fileOperation = new FileOperation(_informationLogger);
                _dataManipulation = new DataManipulation.Lib.Services.Implementation.DataManipulation(_informationLogger, _fileOperation);

            }
            catch (Exception ex)
            {
                _informationLogger.LogException(ex);
                throw;
            }
        }

        static void Run(string path, char[] separators, bool flag, int length, int numberOfWords)
        {
            try
            {
                _dataManipulation.Execute(path, separators, flag, length, numberOfWords);
            }
            catch (Exception ex)
            {
                _informationLogger.LogException(ex);
                throw;
            }

        }

 }
}
