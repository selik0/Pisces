/****************
 *@class name:		SlicedSprite
 *@description:		图片的切九宫格工具
 *@author:			selik0
*************************************************************************/
using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace PiscesEditor
{
    static class SlicedSprite
    {
        private const int SAVE_PIXEL_COUNT = 2;
        private const int MIN_SAME_PIXEL_COUNT = 10;
        private const float MIN_SAME_PIXEL_RATIO = .2f;
        private const float SAME_COLOR_OFFSET = .15f;
        private const float TOTAL_COLOR_OFFSET = .4f;
        private readonly static Vector2Int MIN_SIZE = new Vector2Int(32, 32);
        private readonly static Vector2Int BEGIN_RESIZE = new Vector2Int(64, 64);


        struct SlicedSpriteData
        {
            public Vector2Int originSize;
            public Vector2Int newSize;
            public int beginRow;
            public int beginCol;
            public int sameRowNum;
            public int sameColNum;
            public Vector4 spriteBoard;
            public bool canSlicedRow;
            public bool canSlicedCol;

            /// <summary>
            /// 加工数据
            /// </summary>
            public void Process(bool isResize)
            {
                int minColSamePixel = (int)Mathf.Max(MIN_SAME_PIXEL_COUNT, originSize.x * MIN_SAME_PIXEL_RATIO);
                int minRowSamePixel = (int)Mathf.Max(MIN_SAME_PIXEL_COUNT, originSize.y * MIN_SAME_PIXEL_RATIO);
                canSlicedRow = sameRowNum >= minRowSamePixel;
                canSlicedCol = sameColNum >= minColSamePixel;
                newSize.x = originSize.x;
                newSize.y = originSize.y;
                if (canSlicedCol)
                {
                    if (isResize)
                    {
                        if (originSize.x > BEGIN_RESIZE.x)
                        {
                            newSize.x = Mathf.Min(originSize.x, Mathf.Max(originSize.x - sameColNum + SAVE_PIXEL_COUNT, MIN_SIZE.x));
                        }
                        int leftWidth = originSize.x - sameColNum + SAVE_PIXEL_COUNT;
                        // 去除相同像素后剩下的像素比最小的贴图还小就从新设置beginRow和sameRowNum使得最后的像素等于MinSize
                        if (leftWidth < MIN_SIZE.x)
                        {
                            int halfWidth = (MIN_SIZE.x - SAVE_PIXEL_COUNT) / 2;
                            int oldBeginCol = beginCol;
                            int oldEndColNum = originSize.x - sameColNum - oldBeginCol;
                            int endColOffset = Mathf.Max(oldEndColNum, halfWidth) - oldEndColNum;
                            beginCol = Mathf.Max(beginCol, halfWidth);

                            int beginColOffset = beginCol - oldBeginCol;
                            sameColNum -= beginColOffset + endColOffset;
                        }
                    }
                    spriteBoard.x = beginCol;
                    spriteBoard.z = originSize.x - sameColNum - beginCol;
                }
                else
                {
                    beginCol = 0;
                    sameColNum = 0;
                    spriteBoard.x = 0;
                    spriteBoard.z = 0;
                }

                if (canSlicedRow)
                {
                    if (isResize)
                    {
                        if (originSize.y > BEGIN_RESIZE.y)
                        {
                            newSize.y = Mathf.Min(originSize.y, Mathf.Max(originSize.y - sameRowNum + SAVE_PIXEL_COUNT, MIN_SIZE.y));
                        }
                        int leftHeight = originSize.y - sameRowNum + SAVE_PIXEL_COUNT;
                        if (leftHeight < MIN_SIZE.y)
                        {
                            int halfHeight = (MIN_SIZE.y - SAVE_PIXEL_COUNT) / 2;
                            int oldBeginRow = beginRow;
                            int oldEndRowNum = originSize.y - sameRowNum - oldBeginRow;
                            int endRowOffset = Mathf.Max(oldEndRowNum, halfHeight) - oldEndRowNum;
                            beginRow = Mathf.Max(beginRow, halfHeight);

                            int beginRowOffset = beginRow - oldBeginRow;
                            sameRowNum -= beginRowOffset + endRowOffset;
                        }
                    }
                    spriteBoard.y = beginRow;
                    spriteBoard.w = originSize.y - sameRowNum - beginRow;
                }
                else
                {
                    beginRow = 0;
                    sameRowNum = 0;
                    spriteBoard.y = 0;
                    spriteBoard.w = 0;
                }
            }
        }

        [MenuItem("Assets/工具/设置九宫格数据&重置大小", true)]
        internal static bool SlicedMultipleSpriteAndReSizeCheck()
        {
            return CheckCanSliced();
        }
        [MenuItem("Assets/工具/设置图片九宫格数据", true)]
        internal static bool SlicedMultipleSpriteCheck()
        {
            return CheckCanSliced();
        }

        [MenuItem("Assets/工具/设置九宫格数据&重置大小")]
        internal static void SlicedMultipleSpriteAndReSize()
        {
            var objects = Selection.objects;
            if (objects == null || objects.Length == 0)
            {
                return;
            }
            for (int i = 0; i < objects.Length; i++)
            {
                SlicedOneSprite(objects[i], true);
            }
        }

        [MenuItem("Assets/工具/设置图片九宫格数据")]
        internal static void SlicedMultipleSprite()
        {
            var objects = Selection.objects;
            if (objects == null || objects.Length == 0)
            {
                return;
            }
            for (int i = 0; i < objects.Length; i++)
            {
                SlicedOneSprite(objects[i], false);
            }
        }

        static bool CheckCanSliced()
        {
            var objects = Selection.objects;
            if (objects == null || objects.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < objects.Length; i++)
            {
                if (AssetDatabase.IsMainAsset(objects[i]) && objects[i] is Texture2D)
                {
                    return true;
                }
            }
            return false;
        }

        static void SlicedOneSprite(UnityEngine.Object obj, bool isResize)
        {
            if (!AssetDatabase.IsMainAsset(obj))
            {
                UnityEngine.Debug.Log($"不是MainAsset {obj.name}");
                return;
            }
            var path = AssetDatabase.GetAssetPath(obj);
            var src = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (src == null)
            {
                UnityEngine.Debug.Log($"不是图片 {path}");
                return;
            }

            var tex = new Texture2D(src.width, src.height, src.format, false);
            Graphics.CopyTexture(src, tex);
            var slicedData = GetSlicedData(tex, isResize);

            if (!slicedData.canSlicedCol && !slicedData.canSlicedRow)
            {
                return;
            }
            if (isResize)
            {
                // 左下角
                var leftWidth = slicedData.beginCol + SAVE_PIXEL_COUNT;
                var leftHeight = slicedData.beginRow + SAVE_PIXEL_COUNT;
                // 右上角
                //slicedData.beginCol + slicedData.sameColNum为0时，右上角的最小点就是左下角的最大点
                var rightBeginX = slicedData.beginCol + slicedData.sameColNum;
                var rightBeginY = slicedData.beginRow + slicedData.sameRowNum;
                var rightWidth = tex.width - slicedData.beginCol - slicedData.sameColNum;
                var rightHeight = tex.height - slicedData.beginRow - slicedData.sameRowNum;
                if (rightBeginX == 0)
                {
                    rightBeginX = leftWidth;
                    rightWidth -= SAVE_PIXEL_COUNT;
                }
                if (rightBeginY == 0)
                {
                    rightBeginY = leftHeight;
                    rightHeight -= SAVE_PIXEL_COUNT;
                }
                var newTex = new Texture2D(slicedData.newSize.x, slicedData.newSize.y, tex.format, false);

                // 设置左下角像素
                var lbPixels = tex.GetPixels(0, 0, leftWidth, leftHeight);
                newTex.SetPixels(0, 0, leftWidth, leftHeight, lbPixels);

                // 设置左上角像素
                var ltPixels = tex.GetPixels(0, rightBeginY, leftWidth, rightHeight);
                newTex.SetPixels(0, leftHeight, leftWidth, rightHeight, ltPixels);

                // 设置右下角像素
                var rbPixels = tex.GetPixels(rightBeginX, 0, rightWidth, leftHeight);
                newTex.SetPixels(leftWidth, 0, rightWidth, leftHeight, rbPixels);

                // 设置右上角像素
                var rtPixels = tex.GetPixels(rightBeginX, rightBeginY, rightWidth, rightHeight);
                newTex.SetPixels(leftWidth, leftHeight, rightWidth, rightHeight, rtPixels);

                if (path.EndsWith(".png"))
                {
                    var bytes = newTex.EncodeToPNG();
                    File.WriteAllBytes(path, bytes);
                }
                else if (path.EndsWith(".jpg"))
                {
                    var bytes = newTex.EncodeToJPG();
                    File.WriteAllBytes(path, bytes);
                }
            }

            TextureImporter srcImporter = (TextureImporter)AssetImporter.GetAtPath(path);
            var setting = new TextureImporterSettings();
            srcImporter.ReadTextureSettings(setting);
            setting.spriteBorder = slicedData.spriteBoard;
            srcImporter.SetTextureSettings(setting);
            srcImporter.SaveAndReimport();
            AssetDatabase.Refresh();
        }

        static SlicedSpriteData GetSlicedData(Texture2D tex, bool isResize)
        {
            SlicedSpriteData data = new SlicedSpriteData
            {
                originSize = new Vector2Int(tex.width, tex.height)
            };
            //计算相同行
            int beginRow = 0;
            int sameRowNum = 1;
            Color[] firstRow = tex.GetPixels(0, 0, tex.width, 1);
            Color[] secondRow;
            for (int i = 1; i < tex.height; i++)
            {
                secondRow = tex.GetPixels(0, i, tex.width, 1);
                if (IsSame(firstRow, secondRow))
                {
                    sameRowNum++;
                    if (sameRowNum > data.sameRowNum)
                    {
                        data.beginRow = beginRow;
                        data.sameRowNum = sameRowNum;
                    }
                }
                else
                {
                    firstRow = secondRow;
                    beginRow = i;
                    sameRowNum = 1;
                }
            }

            // 计算相同列
            int beginCol = 0;
            int sameColNum = 1;
            Color[] firstCol = tex.GetPixels(0, 0, 1, tex.height);
            Color[] secondCol;
            for (int i = 1; i < tex.width; i++)
            {
                secondCol = tex.GetPixels(i, 0, 1, tex.height);
                if (IsSame(firstCol, secondCol))
                {
                    sameColNum++;
                    if (sameColNum > data.sameColNum)
                    {
                        data.beginCol = beginCol;
                        data.sameColNum = sameColNum;
                    }
                }
                else
                {
                    firstCol = secondCol;
                    beginCol = i;
                    sameColNum = 1;
                }
            }
            data.Process(isResize);
            return data;
        }

        static bool IsSame(Color[] a, Color[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }
            for (int i = 0; i < a.Length; i++)
            {
                if (!ColorIsSame(a[i], b[i]))
                {
                    return false;
                }
            }
            return true;
        }

        static bool ColorIsSame(Color a, Color b)
        {
            float q = Mathf.Abs(a.r - b.r);
            float w = Mathf.Abs(a.g - b.g);
            float e = Mathf.Abs(a.b - b.b);
            float r = Mathf.Abs(a.a - b.a);
            var isSame = q <= SAME_COLOR_OFFSET && w <= SAME_COLOR_OFFSET
                        && e <= SAME_COLOR_OFFSET && r <= SAME_COLOR_OFFSET;
            var isTotalSmae = q + w + e + r <= TOTAL_COLOR_OFFSET;
            return isSame && isTotalSmae;
        }
    }
}