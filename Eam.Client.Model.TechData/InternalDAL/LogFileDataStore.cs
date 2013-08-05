using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Eam.Client.Model.TechData.InternalDAL {
    /// <summary>
    /// This class is implement a logging with using a log file.
    /// One log file per day (option per minute and per hour)
    /// </summary>
    internal class LogFileDataStore: Interfaces.IDataStore {
        /// <summary>
        /// Possible time intervals for recreate log file
        /// </summary>
        internal enum LogTimeInterval { minute, hour, day }; 

        private string LOG_BASE_PATH = ".\\Log\\";

        private FileStream _fileStream;
        private StreamWriter _streamWriter;

        /// <summary>
        /// Log file create date (for check obsolete of log file)
        /// </summary>
        private DateTime _logFileCreateDate;
        /// <summary>
        /// Current uses update log file interval
        /// </summary>
        private LogTimeInterval _currentLogTimeInterval;

        internal LogFileDataStore() {
            _currentLogTimeInterval = LogTimeInterval.minute;
            LOG_BASE_PATH = Properties.Settings.Default.Log_file_base_path;
            CreateNewLogFile();
        }

        /// <summary>
        /// Create name of log file by current date
        /// </summary>
        /// <returns>file name</returns>
        private string CreateCurrentLogFileName() {
            string result;
            DateTime now = DateTime.Now;
            result = String.Format("{0}_{1}_{2}_{3}_{4}.log", now.Day, now.Month, now.Year, now.Hour, now.Minute);
            return result;
        }

        /// <summary>
        /// Creatint a new log file and its streams
        /// </summary>
        private void CreateNewLogFile() {
            string newFileName = CreateCurrentLogFileName();
            if (_streamWriter != null)
                _streamWriter.Close();
            if (_fileStream != null)
                _fileStream.Close();
            _fileStream = new FileStream(LOG_BASE_PATH + newFileName, FileMode.Append, FileAccess.Write);
            _streamWriter = new StreamWriter(_fileStream);
            _logFileCreateDate = DateTime.Now;
        }

        /// <summary>
        /// Is Log file obsolete? check. 
        /// 1-true (need to recreate file)
        /// 0-false
        /// </summary>
        /// <param name="currentLogTimeInterval"></param>
        /// <returns>
        /// 1-true (need to recreate file)
        /// 0-false
        /// </returns>
        private bool IsLogFileObsolete(LogTimeInterval currentLogTimeInterval) {
            switch (currentLogTimeInterval) {
                case LogTimeInterval.minute:
                    return _logFileCreateDate.Minute != DateTime.Now.Minute;
                case LogTimeInterval.hour:
                    return _logFileCreateDate.Hour != DateTime.Now.Hour;
                case LogTimeInterval.day:
                    return _logFileCreateDate.Day != DateTime.Now.Day;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Save new values (Implementation of public interface)
        /// </summary>
        /// <param name="pollItemValues"></param>
        public void SaveValues(CommonDataContract.PollItemValue[] pollItemValues) {
            if (IsLogFileObsolete(_currentLogTimeInterval)) {
                CreateNewLogFile();
            }

            if (_streamWriter != null) {
                string writeString;
                foreach (var currentPollItemValue in pollItemValues) {
                    try {
                        writeString = String.Format("{0}\t{1} {2}\t{3}",
                            currentPollItemValue.ItemID,
                            currentPollItemValue.Timestamp.ToShortDateString(),
                            currentPollItemValue.Timestamp.ToLongTimeString(),
                            currentPollItemValue.Value.ToString());
                        _streamWriter.WriteLine(writeString);
                    }
                    catch (Exception e) {
                        //_streamWriter.WriteLine(e.Message);
                    }
                }
            }

        }

        
    }
}
