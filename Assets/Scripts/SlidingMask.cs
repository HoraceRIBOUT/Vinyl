using UnityEngine;

[ExecuteAlways]
public class SlidingMask : MonoBehaviour
{
    private RectTransform rectTransform;
    public  RectTransform fatherRectForm;
    private Vector3 startPosition;

    protected void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (fatherRectForm.anchorMax.x != 0)
        {
            rectTransform.anchorMax = Vector2.up + (Vector2.right * (1f / fatherRectForm.anchorMax.x));

            //Debug.Log("SizeDelta = " + (Vector2.up + (Vector2.right * (1f / fatherRectForm.anchorMax.x))));

            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
        else
        {
            //Bro, i don't care
        }
       

    }
}