/****************
 *@class name:		MonoSingleton
 *@description:		
 *@author:			selik0
*************************************************************************/
using UnityEngine;
namespace PiscesEngine
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T m_Instance;
        public static T Instance
        {
            get
            {
                if (!Application.isPlaying)
                {
                    return null;
                }
                m_Instance = FindObjectOfType(typeof(T)) as T;
                if (m_Instance == null)
                {
                    GameObject parent = GameObject.Find("ManagerGo");
                    if (parent == null)
                    {
                        parent = new GameObject("ManagerGo");
                        DontDestroyOnLoad(parent);
                    }
                    GameObject go = new GameObject(typeof(T).Name);
                    m_Instance = go.AddComponent<T>();
                    if (parent != null && go != null)
                    {
                        go.transform.parent = parent.transform;
                    }
                }
                return m_Instance;
            }
        }
        protected void Awake()
        {
            Init();
        }
        protected abstract void Init();
        public abstract void ReLogin();
    }
}