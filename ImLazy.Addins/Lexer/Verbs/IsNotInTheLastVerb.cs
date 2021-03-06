﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.IsNotInTheLastVerb")]
    class IsNotInTheLastVerb : IVerb
    {
        private static readonly LexerType[] SupportedSubjectTypes = { LexerTypes.Date };

        internal enum Units
        {
            Hours,
            Days,
            Weeks,
            Months,
            Years,
            Decades,
            Centries
        }

        public virtual string Name {get { return "IsNotInTheLastVerb"; }}

        public LexerType GetObjectType(LexerType verbType)
        {
            return LexerTypes.TimeSpan;
        }

        public IEnumerable<LexerType> GetSupportedSubjectTypes()
        {
            return SupportedSubjectTypes;
        }

        public static string ToConfigString(string value, string unit)
        {
            return value + ' ' + unit;
        }

        public static string[] GetConfigArguments(string config)
        {
            return config == null ? null : config.Split(' ');
        }

        public static DateTime FromConfigString(string config, DateTime baseDateTime)
        {
            var args = GetConfigArguments(config);
            var valueStr = args[0];
            var unitStr = args[1];
            var targetDate = baseDateTime;

            Units unitEnum;
            int intValue;
            if (!Enum.TryParse(unitStr, out unitEnum) || !Int32.TryParse(valueStr, out intValue))
            {

            }
            else
            {
                switch (unitEnum)
                {
                    case Units.Hours:
                        targetDate = targetDate.AddHours(intValue);
                        break;
                    case Units.Days:
                        targetDate = targetDate.AddDays(intValue);
                        break;
                    case Units.Weeks:
                        targetDate = targetDate.AddDays(intValue * 7);
                        break;
                    case Units.Months:
                        targetDate = targetDate.AddMonths(intValue);
                        break;
                    case Units.Years:
                        targetDate = targetDate.AddYears(intValue);
                        break;
                    case Units.Decades:
                        targetDate = targetDate.AddYears(intValue * 10);
                        break;
                    case Units.Centries:
                        targetDate = targetDate.AddYears(intValue * 100);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return targetDate;
        }

        public virtual bool IsMatch(object subject, object value)
        {
            var baseDateTime = (DateTime) subject;
            var targetTime = FromConfigString((string) value, baseDateTime);
            return DateTime.Now > targetTime;
        }
    }
}
