﻿using System.Xml.Schema;

namespace System.Collections.Generic
{
    using System;
    using Runtime.Serialization;
    using Xml;
    using Xml.Serialization;

    [Serializable]
    public class SerializableDictionary<TKey, TVal> : Dictionary<TKey, TVal>, IXmlSerializable, ISerializable
    {
        #region Constants
/*
        private const string DictionaryNodeName = "Dictionary";
*/
        private const string ItemNodeName = "Item";
        private const string KeyNodeName = "Key";
        private const string ValueNodeName = "Value";
        #endregion
        #region Constructors
        public SerializableDictionary()
        {
        }

        public SerializableDictionary(Dictionary<TKey, TVal> dictionary)
            : base(dictionary)
        {
        }

        public SerializableDictionary(IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
        }

        public SerializableDictionary(int capacity)
            : base(capacity)
        {
        }

        public SerializableDictionary(Dictionary<TKey, TVal> dictionary, IEqualityComparer<TKey> comparer)
            : base(dictionary, comparer)
        {
        }

        public SerializableDictionary(int capacity, IEqualityComparer<TKey> comparer)
            : base(capacity, comparer)
        {
        }

        #endregion
        #region ISerializable Members

        protected SerializableDictionary(SerializationInfo info, StreamingContext context)
        {
            var itemCount = info.GetInt32("ItemCount");
            for (var i = 0; i < itemCount; i++)
            {
                var kvp = (KeyValuePair<TKey, TVal>)info.GetValue(String.Format("Item{0}", i), typeof(KeyValuePair<TKey, TVal>));
                Add(kvp.Key, kvp.Value);
            }
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ItemCount", Count);
            var itemIdx = 0;
            foreach (var kvp in this)
            {
                info.AddValue(String.Format("Item{0}", itemIdx), kvp, typeof(KeyValuePair<TKey, TVal>));
                itemIdx++;
            }
        }

        #endregion
        #region IXmlSerializable Members

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            //writer.WriteStartElement(DictionaryNodeName);
            foreach (var kvp in this)
            {
                writer.WriteStartElement(ItemNodeName);
                writer.WriteStartElement(KeyNodeName);
                KeySerializer.Serialize(writer, kvp.Key);
                writer.WriteEndElement();
                writer.WriteStartElement(ValueNodeName);
                ValueSerializer.Serialize(writer, kvp.Value);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            //writer.WriteEndElement();
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement)
            {
                return;
            }

            // Move past container
            if (!reader.Read())
            {
                throw new XmlException("Error in Deserialization of Dictionary");
            }

            //reader.ReadStartElement(DictionaryNodeName);
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement(ItemNodeName);
                reader.ReadStartElement(KeyNodeName);
                var key = (TKey)KeySerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement(ValueNodeName);
                var value = (TVal)ValueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadEndElement();
                Add(key, value);
                reader.MoveToContent();
            }
            //reader.ReadEndElement();

            reader.ReadEndElement(); // Read End Element to close Read of containing node
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        #endregion
        #region Private Properties
// ReSharper disable once MemberCanBePrivate.Global
        protected XmlSerializer ValueSerializer
        {
            get { return _valueSerializer ?? (_valueSerializer = new XmlSerializer(typeof (TVal))); }
        }

        private XmlSerializer KeySerializer
        {
            get { return _keySerializer ?? (_keySerializer = new XmlSerializer(typeof (TKey))); }
        }
        #endregion
        #region Private Members
        private XmlSerializer _keySerializer;
        private XmlSerializer _valueSerializer;
        #endregion
    }
}