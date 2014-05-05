using System;
using System.IO;
using System.Xml.Serialization;
using ImLazy.Contracts;
using ImLazy.Data;
using log4net;
using LogManager = ImLazy.RunTime.LogManager;

namespace ImLazy.Util
{
    /// <summary>
    /// 对象的序列化和反序列化
    /// </summary>
    public static class DataCreationUtil
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(DataCreationUtil));
        /// <summary>
        /// 从Xml序列化文件创建实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns>如果文件不存在，将返回null</returns>
        public static T TryCreateFromFile<T>(string filePath)
        {
            var result = default(T);
            try
            {
                Log.DebugFormat("Creating instance of {0} from path: {1}", typeof(T), filePath);
                if (File.Exists(filePath))
                {
                    using (var stream = new StreamReader(filePath))
                    {
                        var xs = new XmlSerializer(typeof(T));
                        result = (T)xs.Deserialize(stream);
                    }
                    Log.Debug("Creating instance finished.");
                }
                else
                {
                    Log.Debug("File not exist. Return null instead.");
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed in creating instance! Return null instead.", ex);
            }
            
            return result;
        }

        /// <summary>
        /// 保存到文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="filePath"></param>
        public static void TrySaveToFile<T>(T data, String filePath)
        {
            try
            {
                Log.DebugFormat("Saving instance of {0} to path: {1}", typeof(T), filePath);
                using (var stream = new StreamWriter(filePath, false))
                {
                    var xs = new XmlSerializer(typeof(T));
                    xs.Serialize(stream, data);
                }
                Log.Debug("Saving instance finished.");
            }
            catch (Exception ex)
            {
                Log.Error("Failed in saving file.", ex);
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TWrapper"></typeparam>
        /// <typeparam name="TIAddin"></typeparam>
        /// <typeparam name="TIMetadata"></typeparam>
        /// <param name="lazy"></param>
        /// <returns></returns>
        public static TWrapper FromLazy<TWrapper, TIAddin, TIMetadata>(Lazy<TIAddin, TIMetadata> lazy)
            where TWrapper : IAddinInfo,new()
            where TIAddin : IAddin
            where TIMetadata : IAddinMetadata
        {
            var c = AddinInfoFactory.Create<TWrapper>(lazy.Value);
            return c;
        }
    }
}
