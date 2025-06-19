using UnityEngine;
using UnityEngine.EventSystems;
public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MonoBehaviour scriptToToggle; // drag the script you want to toggle here
    Vector3 startScale;

    public float pulseSpeed = 6f;
    public float pulseAmount = 0.03f;

    bool isPulse;

    private void Start()
    {
        isPulse = false;

        startScale = transform.localScale;
    }

    void Update()
    {
        if (!isPulse) return;

        float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = startScale * scale;
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isPulse = true;

        if (scriptToToggle != null)
            scriptToToggle.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPulse = false;

        transform.localScale = startScale;
    }
}

