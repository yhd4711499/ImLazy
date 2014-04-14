using ImLazy.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ImLazy.Addins
{
    public class FileOperationAction : IActionAddin
    {
        static readonly List<ActionOpeartion> Opeartions = new List<ActionOpeartion>
        {
            GetOperation<string>("copy",File.Copy),
            GetOperation<string>("delete",(a,b)=>File.Delete(a)),
            GetOperation<string>("move",File.Move),
            GetOperation<string>("rename",File.Move),
        };

        #region Utils
        /// <summary>
        /// Convinience for creating a new ActionOpeartion
        /// </summary>
        /// <typeparam name="T">Param type</typeparam>
        /// <param name="symbol"></param>
        /// <param name="action">ActionAddinInfo to perform</param>
        /// <returns></returns>
        static ActionOpeartion GetOperation<T>(string symbol, Action<T, T> action)
        {
            return new ActionOpeartion(typeof(T), symbol, (a, b) => action((T)a,(T)b));
        }

        internal static IEnumerable<ActionOpeartion> GetAvailSymbols(Type type)
        {
            return Opeartions.Where(_ => _.TargetType.Equals(type));
        }

        #endregion

        public void DoAction(string filePath, SerializableDictionary<string, object> config)
        {
            throw new NotImplementedException();
        }

        public IEditView CreateMainView(SerializableDictionary<string, object> config)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// An extension for Symbol
        /// </summary>
        internal class ActionOpeartion
        {
            internal Type TargetType { get; private set; }
            private readonly string _name;
            private Action<object, object> _action;

            public ActionOpeartion(Type type, string name, Action<object, object> action)
            {
                TargetType = type;
                _name = name;
                _action = action;
            }

            public override string ToString()
            {
                return _name;
            }
        }
    }
}
