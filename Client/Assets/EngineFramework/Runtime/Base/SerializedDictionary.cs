/****************
 *@class name:		SerializedDictionary
 *@description:		序列化字典
 *@author:			selik0
*************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
namespace PiscesEngine
{
    public interface IDictionaryDrawable { }

    [Serializable]
    public class SerializedDictionary<K, V> : Dictionary<K, V>, IDictionaryDrawable, ISerializationCallbackReceiver
    {
        [SerializeField] List<K> m_Keys = new List<K>();

        [SerializeField] List<V> m_Values = new List<V>();

        /// <summary>
        /// OnBeforeSerialize implementation.
        /// </summary>
        public void OnBeforeSerialize()
        {
            m_Keys.Clear();
            m_Values.Clear();

            foreach (var kvp in this)
            {
                m_Keys.Add(kvp.Key);
                m_Values.Add(kvp.Value);
            }
        }

        /// <summary>
        /// OnAfterDeserialize implementation.
        /// </summary>
        public void OnAfterDeserialize()
        {
            Clear();
            for (int i = 0; i < m_Keys.Count; i++)
                Add(m_Keys[i], m_Values[i]);

            m_Keys.Clear();
            m_Values.Clear();
        }
    }
}