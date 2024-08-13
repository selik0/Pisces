/****************
 *@class name:		TextureExtensions
 *@description:		
 *@author:			selik0
*************************************************************************/
using UnityEngine;
namespace PiscesEngine
{
    public struct TextureBlockData
    {
        public int width;
        public int height;
        public int blockByte;
    }
    public static class TextureExtensions
    {
        public static bool GetTextureBlockData(this Texture2D texture, out TextureBlockData blockData)
        {
            blockData = new TextureBlockData()
            {
                width = 16,
                height = 8,
                blockByte = 512
            };
            bool isSupport = true;
            switch (texture.format)
            {
                case TextureFormat.RGBA32:
                    break;
                case TextureFormat.ASTC_4x4:
                    blockData.width = 4;
                    blockData.height = 4;
                    blockData.blockByte = 16;
                    break;
                case TextureFormat.ASTC_5x5:
                    blockData.width = 5;
                    blockData.height = 5;
                    blockData.blockByte = 16;
                    break;
                case TextureFormat.ASTC_6x6:
                    blockData.width = 6;
                    blockData.height = 6;
                    blockData.blockByte = 16;
                    break;
                default:
                    isSupport = false;
                    break;
            }
            return isSupport;
        }
    }
}