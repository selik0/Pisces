/****************
 *@class name:		LogicBehaviour
 *@description:		基础的组件，
 *@author:			selik0
*************************************************************************/
using System;
using UnityEngine;
namespace PiscesEngine
{
    [DisallowMultipleComponent]
    public class LogicBehaviour : MonoBehaviour
    {
        public int Key;
        public event Action<int> DestroyEvent;

        protected void OnDestroy()
        {
            DestroyEvent?.Invoke(Key);
        }
    }
}