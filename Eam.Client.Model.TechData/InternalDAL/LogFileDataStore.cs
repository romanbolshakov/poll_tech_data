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
        private string INIT_LOG_TEXT = "<?xml version=\"1.0\" encoding=\"WINDOWS-1251\"?><root>";
        private string END_OF_FILE = "</root>";

        System.Xml.XmlDocument _xmlDocument;

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
            _xmlDocument = new System.Xml.XmlDocument();
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
            if (_streamWriter != null) {
                ClosePrevFile();
            }
            string newFileName = CreateCurrentLogFileName();
            _fileStream = new FileStream(LOG_BASE_PATH + newFileName, FileMode.Append, FileAccess.Write);
            _streamWriter = new StreamWriter(_fileStream);
            _streamWriter.WriteLine(INIT_LOG_TEXT);
            _logFileCreateDate = DateTime.Now;
        }

        private void ClosePrevFile() {
            _streamWriter.WriteLine(END_OF_FILE);
            if (_streamWriter != null)
                _streamWriter.Close();
            if (_fileStream != null)
                _fileStream.Close();
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
                        writeString = CreateLogString(currentPollItemValue);
                        _streamWriter.WriteLine(writeString);
                        _streamWriter.Flush();
                    }
                    catch (Exception e) {
                        TDInternalLogger.GetLogger().LogException(e);
                    }
                }
            }

        }
        /*
        public void SaveValues(CommonDataContract.PollItemValue[] pollItemValues) {
            if (IsLogFileObsolete(_currentLogTimeInterval)) {
                CreateNewLogFile();
            }

            if (_streamWriter != null) {
                System.Xml.XmlNode newNode;
                System.Xml.XmlAttribute newAttribure;
                foreach (var currentPollItemValue in pollItemValues) {
                    try {
                        newNode = _xmlDocument.CreateElement("item-value");
                        newAttribure = _xmlDocument.CreateAttribute("id");
                        newAttribure.Value = currentPollItemValue.ItemID;
                        newNode.Attributes.Append(newAttribure);
                        _xmlDocument.AppendChild(newNode);

                        newAttribure = _xmlDocument.CreateAttribute("timestamp");
                        newAttribure.Value = currentPollItemValue.Timestamp.ToShortDateString() + " " +currentPollItemValue.Timestamp.ToLongTimeString();
                        newNode.Attributes.Append(newAttribure);
                        _xmlDocument.AppendChild(newNode);


                        newAttribure = _xmlDocument.CreateAttribute("value");
                        newAttribure.Value = currentPollItemValue.Value.ToString();
                        newNode.Attributes.Append(newAttribure);
                        _xmlDocument.AppendChild(newNode);
                    }
                    catch (Exception e) {
                        TDInternalLogger.GetLogger().LogException(e);
                    }
                }
                _xmlDocument.Save(_streamWriter);
            }

        }
        */
        private string CreateLogString(CommonDataContract.PollItemValue currentPollItemValue) {
            //return String.Format("{0}\t{1} {2}\t{3}",
            //               currentPollItemValue.ItemID,
            //               currentPollItemValue.Timestamp.ToShortDateString(),
            //               currentPollItemValue.Timestamp.ToLongTimeString(),
            //               currentPollItemValue.Value.ToString());
            return String.Format("<item-value id=\"{0}\" timestamp=\"{1} {2}\"\t value=\"{3}\"/>",
                currentPollItemValue.ItemID,
                currentPollItemValue.Timestamp.ToShortDateString(),
                currentPollItemValue.Timestamp.ToLongTimeString(),
                currentPollItemValue.Value.ToString());
        }

        
    }
}
