using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PViewMarker : MonoBehaviour
{
    public enum ViewType { PView, PComponent}
    [Header("要创建的类名")]
    public string className;
    public ViewType viewType = ViewType.PView;
    [HideInInspector]
    public List<string> fieldNameList = new List<string>();
    [HideInInspector]
    public List<UnityEngine.Object> fieldValueList = new List<UnityEngine.Object>();
}
