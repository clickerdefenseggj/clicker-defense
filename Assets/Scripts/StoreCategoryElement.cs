using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class StoreCategoryElement : MonoBehaviour
{
    [FormerlySerializedAs("TitleText")]
    public Text titleText;
    [FormerlySerializedAs("ContentTransform")]
    public RectTransform content;

    public void Initialize(string name)
    {
        titleText.text = name;
    }
}
