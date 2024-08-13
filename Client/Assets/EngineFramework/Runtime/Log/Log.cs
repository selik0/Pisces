/****************
 *@class name:		Log
 *@description:		日志
 *@author:			selik0
*************************************************************************/
using UnityEngine;
namespace PiscesEngine
{
    public static class Log
    {
        public static void Info(string info)
        {
            Debug.Log(info);
        }
        
        public static void Warning(string info)
        {
            Debug.LogWarning(info);
        }

        public static void Error(string info)
        {
            Debug.LogError(info);
        }
    }
}