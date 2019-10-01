﻿using FilterPolishUtil.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolishUtil.Model
{
    public class LoggingFacade : ICleanable
    {
        private static LoggingFacade instance;

        public ObservableCollection<LogEntryModel> Logs { get; set; } = new ObservableCollection<LogEntryModel>();

        public Action<string> CustomLoggingAction { get; set; }

        private LoggingFacade()
        {

        }

        public object _itemsLock;

        public void SetLock(object localLock)
        {
            _itemsLock = localLock;
        }

        public static LoggingFacade GetInstance()
        {
            if (instance == null)
            {
                instance = new LoggingFacade();
                instance._itemsLock = new object();
            }

            return instance;
        }

        public static void LogWarning(string info)
        {
            GetInstance().CustomLoggingAction(info);
            GetInstance().Log(info, LoggingLevel.Warning);
        }

        public static void LogError(string info)
        {
            GetInstance().CustomLoggingAction(info);
            GetInstance().Log(info, LoggingLevel.Errors);
        }

        public static void LogItem(string info)
        {
            GetInstance().Log(info, LoggingLevel.Item);
        }

        public static void LogInfo(string info, bool showMessage = false)
        {
            if (showMessage)
            {
                GetInstance().CustomLoggingAction(info);
            }

            GetInstance().Log(info, LoggingLevel.Info);
        }

        public static void LogDebug(string info) => GetInstance().Log(info, LoggingLevel.Debug);

        public void SetCustomLoggingAction(Action<string> action)
        {
            this.CustomLoggingAction = action;
        }

        public void Log(string info, LoggingLevel level)
        {
            var mth = new StackTrace().GetFrame(2).GetMethod();
            var cls = mth?.ReflectedType?.Name ?? "unknown name";
            var methodName = mth.Name;

            lock(_itemsLock)
            {
                this.Logs.Add(new LogEntryModel()
                {
                    Level = level,
                    Time = DateTime.Now,
                    Information = info,
                    Source = cls,
                    MethodName = methodName
                });
            }
        }

        public void ClearLogs()
        {
            Logs.Clear();
        }

        public void Clean()
        {
            this.ClearLogs();
            instance = null;
        }
    }
}
