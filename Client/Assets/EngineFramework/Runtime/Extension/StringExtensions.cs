/****************
 *@class name:		StringExtensions
 *@description:		
 *@author:			selik0
*************************************************************************/
using UnityEngine;
namespace PiscesEngine
{
    public static class StringExtensions
    {
        public static string FirstUpper(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }
            return char.ToUpper(source[0]) + source.Substring(1);
        }
    }
}