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

                while (true)
                {
                    char[] separators = new char[] { ' ', '.', ',', '-', '"', '!', '?', '(', ')', '/', '\\', ':', '[', ']', '—', ' ', '\r', '\n', '\'', '’', ';', '`', '”', '“' };
                    
                    bool ascendingFlag = (_configuration.GetSection("defaultSortOrder").Value == "true") ? true : false;
                    int length = int.Parse(_configuration.GetSection("defaultMinimumLengthOfWord").Value);
                    int numberOfWords = int.Parse(_configuration.GetSection("defaultNumberOfWords").Value);

                    System.Console.WriteLine("Execute program with default values?  Y/N \n\n" +
                        "1. Sort leghth in descending order.\n" +
                        "2. Pick top 50 words lenghtwise\n" +
                        "3. Count number of words with 6+ letters\n");
                    var responce = System.Console.ReadLine();

                    if (responce == "Y" || responce == "y")
                    {
                        Run(_defaultPath, separators, ascendingFlag, length, numberOfWords);
                    }
                    else
                    {
                        System.Console.WriteLine("Order words in Descending order? Y/N");
                        var flag = System.Console.ReadLine();

                        while ((flag != "Y") && (flag != "Y") && (flag != "N") && (flag != "n"))
                        {
                            System.Console.WriteLine("Please enter valid letter.");
                            flag = System.Console.ReadLine();
                        }

                        ascendingFlag = (flag == "Y" || flag == "y") ? true : false;

                        System.Console.WriteLine("Enter minimum length of each word");
                        while (!int.TryParse(System.Console.ReadLine(), out length))
                        {
                            System.Console.WriteLine("That was invalid. Enter a valid number.");
                        }

                        System.Console.WriteLine("Enter number of words to be selected");
                        while (!int.TryParse(System.Console.ReadLine(), out numberOfWords))
                        {
                            System.Console.WriteLine("That was invalid. Enter a valid number.");
                        }

                        Run(_defaultPath, separators, ascendingFlag, length, numberOfWords);
                    }
                }
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
                var watch = new Stopwatch();
                watch.Start();

                _dataManipulation.Execute(path, separators, flag, length, numberOfWords);

                watch.Stop();
                System.Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms\n\n\n\n");
            }
            catch (Exception ex)
            {
                _informationLogger.LogException(ex);
                throw;
            }

        }

 }
}
