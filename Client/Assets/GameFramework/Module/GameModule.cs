/****************
 *@class name:		GameModule
 *@description:		单列抽象类
 *@author:			selik0
*************************************************************************/
using System;
namespace PiscesGame
{
    public abstract class GameModule<T> where T : class, new()
    {
        private static T m_instance;
        public static T Instance
        {
            get
            {
                m_instance ??= Activator.CreateInstance<T>();
                return m_instance;
            }
        }

        public virtual int Priority => 0;

        public GameModule()
        {
            ReLogin();
        }
        public abstract void ReLogin();
    }
}