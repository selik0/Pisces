/****************
 *@class name:		ResolutionAdapter
 *@description:		分辨率适配
 *@author:			selik0
*************************************************************************/
using System;
using UnityEngine;
namespace PiscesEngine
{
    public class ResolutionAdapter
    {
        public bool IsPortrait { get; private set; }
        public Vector2Int DesignSize { get; private set; }
        private readonly Action<ResolutionAdapter> updateAction;
        private ResolutionAdapter() { }
        public ResolutionAdapter(bool isPortrait, Vector2Int designSize, Action<ResolutionAdapter> action)
        {
            IsPortrait = isPortrait;
            DesignSize = designSize;
            updateAction = action;
        }

        public void UpdateResolution()
        {

            updateAction?.Invoke(this);
        }
    }
}