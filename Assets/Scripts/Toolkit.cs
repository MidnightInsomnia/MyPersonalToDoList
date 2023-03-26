using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Toolkit : MonoBehaviour
{
    //Метод для исправления графического бага Unity
    //Проявляется в runtime во время добавления элемента в VLG
    //Исправляется изменением параметра spacing
    public static IEnumerator UpdateLayoutSpacing(VerticalLayoutGroup v, float spacing)
    {
        v.spacing = spacing + 0.00001f;
        yield return new WaitForSeconds(0.00001f);
        v.spacing = spacing + 0.00002f;
        yield return new WaitForSeconds(0.00001f);
        v.spacing = spacing + 0.00003f;
        yield return new WaitForSeconds(0.00001f);
        v.spacing = spacing + 0.00004f;
        v.spacing = spacing;
    }
}

