using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MonoBehaviour scriptToToggle; // drag the script you want to toggle here
    Vector3 currentScale;

    private void Start()
    {
        if (scriptToToggle != null)
            scriptToToggle.enabled = false;

        currentScale = transform.localScale; 
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (scriptToToggle != null)
            scriptToToggle.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (scriptToToggle != null)
            scriptToToggle.enabled = false;
        transform.localScale = currentScale; 
    }
}
