using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public GameObject container;
    float width;
    // Update is called once per frame
    void Update()
    {
        if(width != container.GetComponent<RectTransform>().rect.width)
        {
            width = container.GetComponent<RectTransform>().rect.width;
            Debug.Log(width);
            Vector2 newSize = new Vector2 (width / 2.5f, width / 2.5f);
            Vector2 newSpacing = new Vector2 (newSize.x/8, newSize.x/8);
            float newPadding = newSize.x/8;

            container.GetComponent<GridLayoutGroup>().cellSize = newSize;

            container.GetComponent<GridLayoutGroup>().spacing = newSpacing;

            container.GetComponent<GridLayoutGroup>().padding.bottom = (int)newPadding;
            container.GetComponent<GridLayoutGroup>().padding.top = (int)newPadding;
            container.GetComponent<GridLayoutGroup>().padding.right = (int)newPadding;
            container.GetComponent<GridLayoutGroup>().padding.left = (int)newPadding;
        }
    }
}
