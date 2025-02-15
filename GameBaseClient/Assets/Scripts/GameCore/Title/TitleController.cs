using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    [SerializeField] TitleBackGroundView TitleBackGroundView;
    public void Awake()
    {
        TitleBackGroundView.Initialize();
    }
}
