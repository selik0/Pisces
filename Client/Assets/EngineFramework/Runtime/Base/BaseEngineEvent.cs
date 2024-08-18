/****************
 *@class name:		BaseEngineEvent
 *@description:		
 *@author:			selik0
*************************************************************************/
namespace PiscesEngine
{
    public delegate void BaseEngineDelegate<T>(T obj);
    public class BaseEngineEvent<T>
    {
        private event BaseEngineDelegate<T> m_event;

        public void AddListener(BaseEngineDelegate<T> listener)
        {
            m_event += listener;
        }
        public void RemoveListener(BaseEngineDelegate<T> listener)
        {
            m_event -= listener;
        }
        public void Invoke(T obj)
        {
            m_event?.Invoke(obj);
        }
    }
}