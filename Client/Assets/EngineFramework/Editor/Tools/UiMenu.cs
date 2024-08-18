using UnityEditor;
using UnityEngine;
namespace PiscesEditor
{
    public static class UiMenu
    {
        [MenuItem("GameObject/UI/Slider", true, 1033)]
        static public bool HideSlider()
        {
            return false;
        }
        [MenuItem("GameObject/UI/Slider222", true)]
        static public bool HideSlider222()
        {
            return false;
        }
        [MenuItem("GameObject/UI/Slider222", false)]
        static public void CreateSlider()
        {
            Debug.Log("333");
        }
    }
}