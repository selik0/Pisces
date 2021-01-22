namespace Pisces
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        /// <summary>
        /// 初期化フラグ
        /// </summary>
        private static T initInstance;

        private static bool isDestroy;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    //注意FindObjectOfType会相应增加负荷！
                    //注意不能发现非活动对象！
                    instance = FindObjectOfType<T>();

                    if (instance == null && Application.isPlaying)
                    {
                        var obj = new GameObject(typeof(T).Name);
                        instance = obj.AddComponent<T>();
                    }
                }

                // 初期化
                InitInstance();

                return instance;
            }
        }

        private static void InitInstance()
        {
            if (initInstance == null && instance != null && Application.isPlaying)
            {
                //设置GameObject不销毁
                DontDestroyOnLoad(instance.gameObject);

                //初始化
                var s = instance as MonoSingleton<T>;
                s.InitSingleton();

                initInstance = instance;
            }
        }

        /// <summary>
        /// 判断是否实例化
        /// </summary>
        /// <returns></returns>
        public static bool IsInstance()
        {
            return instance != null && isDestroy == false;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                InitInstance();
            }
            else
            {
                // 发送有2个单例的通知
                var s = instance as MonoSingleton<T>;
                s.DuplicateDetection(this as T);

                //2个单例时销毁一个
                Destroy(this.gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            //创建了单例才能销毁
            if (instance == this)
            {
                isDestroy = true;
            }
        }

        /// <summary>
        /// 重复创建单例通知
        /// </summary>
        /// <param name="duplicate"></param>
        protected virtual void DuplicateDetection(T duplicate) { }

        /// <summary>
        /// 内部初期化
        /// </summary>
        protected abstract void InitSingleton();
    }
}
