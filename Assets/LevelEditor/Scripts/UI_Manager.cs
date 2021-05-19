using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public GameObject container;
    public Text assetsText;
    public Text oneSideText;
    public Text twoSidesText;


    public Text saveBottonText;
    public Text saveInputFieldText;
    public Text saveInputFieldPlaceHolder;
    public Text flipButtonText;
    public Text existingLevelText;

    float width;
    // Update is called once per frame
    void Update()
    {
        if(width != container.GetComponent<RectTransform>().rect.width)
        {
            width = container.GetComponent<RectTransform>().rect.width;
            Vector2 newSize = new Vector2 (width / 2.5f, width / 2.5f);
            Vector2 newSpacing = new Vector2 (newSize.x/8, newSize.x/8);
            float newPadding = newSize.x/8;

            container.GetComponent<GridLayoutGroup>().cellSize = newSize;

            container.GetComponent<GridLayoutGroup>().spacing = newSpacing;

            container.GetComponent<GridLayoutGroup>().padding.bottom = (int)newPadding;
            container.GetComponent<GridLayoutGroup>().padding.top = (int)newPadding;
            container.GetComponent<GridLayoutGroup>().padding.right = (int)newPadding;
            container.GetComponent<GridLayoutGroup>().padding.left = (int)newPadding;


            assetsText.fontSize = (int)width /6;
            oneSideText.fontSize = (int)width /12;
            twoSidesText.fontSize = (int)width /12;

            saveBottonText.fontSize = (int)saveBottonText.rectTransform.rect.width / 4;
            saveInputFieldText.fontSize = (int)saveInputFieldText.rectTransform.rect.width / 10;
            saveInputFieldPlaceHolder.fontSize = (int)saveInputFieldText.rectTransform.rect.width /10;
            flipButtonText.fontSize = (int)flipButtonText.rectTransform.rect.width / 4;
            existingLevelText.fontSize = (int)existingLevelText.rectTransform.rect.width / 6;
        }
    }
}
