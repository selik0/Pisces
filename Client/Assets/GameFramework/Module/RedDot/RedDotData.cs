/****************
 *@class name:		RedDotData
 *@description:		红点数据层，树型结构
 *@author:			selik0
*************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
namespace PiscesGame
{
    public class RedDotData
    {
        /// <summary>
        /// 自己的红点计数
        /// </summary>
        private int m_selfCount = 0;
        /// <summary>
        /// 自己+子节点的红点计数
        /// </summary>
        private int m_totalCount = 0;
        private readonly RedDotData m_parent;
        private readonly Dictionary<object, RedDotData> m_childDict = new Dictionary<object, RedDotData>();
        public event Action OnRedStateChanged;
        /// <summary>
        /// 获取的是自身的红点计数和所有子节点红点计数之和
        /// 设置的只是自身的红点计数不会影响子节点的红点计数
        /// </summary>
        public int Count
        {
            get { return m_totalCount; }
            set
            {
                m_selfCount = value;
                UpdateTotalCount();
            }
        }

        /// <summary>
        /// 方便使用的变量， 设置为true时自身红点计数为1， 设置为false时自身红点计数为0， 如果子节点的红点计数不为0则该变量依然返回true
        /// </summary>
        public bool IsShow
        {
            get { return m_totalCount > 0; }
            set
            {
                Count = value ? 1 : 0;
            }
        }

        private RedDotData() { }
        public RedDotData(RedDotData parent)
        {
            m_parent = parent;
        }

        /// <summary>
        /// 获取子红点，没有则创建
        /// </summary>
        /// <param name="key">key为值类型或string，不允许自定义类型</param>
        /// <returns></returns>
        public RedDotData GetChildData(object key)
        {
            if (!m_childDict.TryGetValue(key, out RedDotData childRed))
            {
                childRed = new RedDotData(this);
                m_childDict.Add(key, childRed);
            }
            return childRed;
        }

        private void UpdateTotalCount()
        {
            m_totalCount = m_selfCount;
            foreach (var child in m_childDict)
            {
                m_totalCount += child.Value.m_totalCount;
            }
            m_parent?.UpdateTotalCount();
            OnRedStateChanged.Invoke();
        }
    }
}