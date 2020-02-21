using DLL_Core;
using System;
using System.Diagnostics;

namespace T_CV
{
    class Program
    {
        public static readonly string AppName = "MinicapIO.Core.T_CV";
        private static Serilog.Core.Logger _logger = Logger.GetInstance();

        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                _logger.Information(string.Format("Starting Application {0} ...", AppName));

                CVFirstTest cvTest = new CVFirstTest();
                cvTest.Run();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, string.Format("An Error Occurred! Message: {0}", ex.Message));
            }
            finally
            {
                stopwatch.Stop();
                _logger.Information(string.Format("Application {0} completed in {1}", AppName, Utils.ElapsedTime(stopwatch.Elapsed)));
            }
        }
    }
}
