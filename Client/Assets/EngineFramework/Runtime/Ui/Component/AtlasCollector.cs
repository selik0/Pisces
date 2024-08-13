/****************
 *@class name:		AtlasCollector
 *@description:		
 *@author:			selik0
*************************************************************************/
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
namespace PiscesEngine
{
    public class AtlasCollector : MonoBehaviour
    {
        [Header("图集的名称")]
        public string atlasName;
        [Header("同时收集子对象内容")]
        public bool includeChildren = true;
        // 记录image用到的图片
        [Header("记录image组件")]
        public List<Image> images = new List<Image>();
        // 记录image用到的图片
        public List<string> imageSpriteNames = new List<string>();
        //  记录button， toggle在spriteSwap模式下用到的图片
        [Header("记录imageSelectable")]
        public List<Selectable> selectables = new List<Selectable>();
        //  记录button， toggle在spriteSwap模式下用到的图片，长度*4
        public List<string> selectableSpriteNames = new List<string>();

        [Header("需要动态加载的Texture, 不放入图集")]
        public List<Texture> dynamicTextures = new List<Texture>();

        public void RecoverSprites(SpriteAtlas atlas)
        {
            if (atlas != null)
            {
                Sprite targetSprite;
                for (int i = 0, lenI = images.Count; i < lenI; i++)
                {
                    targetSprite = atlas.GetSprite(imageSpriteNames[i]);
                    if (targetSprite != null)
                        images[i].sprite = targetSprite;
                }
                SpriteState tempState;
                //恢复selectable的Sprite//
                for (int i = 0, lenI = selectables.Count; i < lenI; i++)
                {
                    tempState = new SpriteState();
                    tempState.highlightedSprite = null;
                    if (!string.IsNullOrEmpty(selectableSpriteNames[i * 4]))
                        tempState.highlightedSprite = atlas.GetSprite(selectableSpriteNames[i * 4]);
                    if (!string.IsNullOrEmpty(selectableSpriteNames[i * 4 + 1]))
                        tempState.pressedSprite = atlas.GetSprite(selectableSpriteNames[i * 4 + 1]);
                    if (!string.IsNullOrEmpty(selectableSpriteNames[i * 4 + 2]))
                        tempState.selectedSprite = atlas.GetSprite(selectableSpriteNames[i * 4 + 2]);
                    if (!string.IsNullOrEmpty(selectableSpriteNames[i * 4 + 3]))
                        tempState.disabledSprite = atlas.GetSprite(selectableSpriteNames[i * 4 + 3]);
                    selectables[i].spriteState = tempState;
                }
            }
        }

#if UNITY_EDITOR
        List<AtlasCollector> collectors;
        [Header("需要动态加载的图片")]
        public List<Sprite> dynamicSprites = new List<Sprite>();
        [HideInInspector]
        public List<string> spritePaths = new List<string>();
        [ContextMenu("收集")]
        public void Collect()
        {
            images.Clear();
            imageSpriteNames.Clear();
            selectables.Clear();
            selectableSpriteNames.Clear();
            spritePaths.Clear();

            string spritePath;
            Image[] imgs = this.GetComponentsInChildren<Image>(true);
            foreach (var img in imgs)
            {

                if (img.sprite != null && ComponentCanBeCollected(img.transform))
                {
                    spritePath = AssetDatabase.GetAssetPath(img.sprite);
                    Debug.Log(spritePath);
                    if (TextureCanBeCollected(spritePath))
                    {
                        images.Add(img);
                        imageSpriteNames.Add(img.sprite.name);
                        if (!spritePaths.Contains(spritePath))
                            spritePaths.Add(spritePath);
                    }
                }
            }
            Selectable[] selectableComs = GetComponentsInChildren<Selectable>(true);
            foreach (var selectable in selectableComs)
            {
                if (selectable.transition == Selectable.Transition.SpriteSwap && ComponentCanBeCollected(selectable.transform))
                {
                    selectables.Add(selectable);
                    //高亮状态//
                    if (selectable.spriteState.highlightedSprite != null)
                    {
                        spritePath = AssetDatabase.GetAssetPath(selectable.spriteState.highlightedSprite);
                        if (TextureCanBeCollected(spritePath))
                        {
                            selectableSpriteNames.Add(selectable.spriteState.highlightedSprite.name);
                            if (!spritePaths.Contains(spritePath))
                                spritePaths.Add(spritePath);
                        }
                        else
                            selectableSpriteNames.Add(string.Empty);
                    }
                    else
                    {
                        selectableSpriteNames.Add(string.Empty);
                    }

                    //按下状态//
                    if (selectable.spriteState.pressedSprite != null)
                    {
                        spritePath = AssetDatabase.GetAssetPath(selectable.spriteState.pressedSprite);
                        if (TextureCanBeCollected(spritePath))
                        {
                            selectableSpriteNames.Add(selectable.spriteState.pressedSprite.name);
                            if (!spritePaths.Contains(spritePath))
                                spritePaths.Add(spritePath);
                        }
                        else
                            selectableSpriteNames.Add(string.Empty);
                    }
                    else
                    {
                        selectableSpriteNames.Add(string.Empty);
                    }

                    //选中状态//
                    if (selectable.spriteState.selectedSprite != null)
                    {
                        spritePath = AssetDatabase.GetAssetPath(selectable.spriteState.selectedSprite);
                        if (TextureCanBeCollected(spritePath))
                        {
                            selectableSpriteNames.Add(selectable.spriteState.selectedSprite.name);
                            if (!spritePaths.Contains(spritePath))
                                spritePaths.Add(spritePath);
                        }
                        else
                            selectableSpriteNames.Add(string.Empty);
                    }
                    else
                    {
                        selectableSpriteNames.Add(string.Empty);
                    }

                    //Disable状态//
                    if (selectable.spriteState.disabledSprite != null)
                    {
                        spritePath = AssetDatabase.GetAssetPath(selectable.spriteState.disabledSprite);
                        if (TextureCanBeCollected(spritePath))
                        {
                            selectableSpriteNames.Add(selectable.spriteState.disabledSprite.name);
                            if (!spritePaths.Contains(spritePath))
                                spritePaths.Add(spritePath);
                        }
                        else
                            selectableSpriteNames.Add(string.Empty);
                    }
                    else
                    {
                        selectableSpriteNames.Add(string.Empty);
                    }
                }
            }
        }

        bool ComponentCanBeCollected(Transform trans)
        {
            if (includeChildren)
                return true;
            if (null == collectors)
                collectors = new List<AtlasCollector>(GetComponentsInChildren<AtlasCollector>(true));

            collectors.Remove(this);
            if (collectors.Count >= 1)
            {
                foreach (var item in collectors)
                {
                    if (trans.IsChildOf(item.transform))
                        return false;
                }
            }
            return true;
        }

        bool TextureCanBeCollected(string spritePath)
        {
            // foreach (var path in MyFrameConfig.UISpriteAtlasCollectorIncludePath)
            // {
            //     if (spritePath.Contains(path))
            //         return true;
            // }
            return false;
        }
#endif
    }
}