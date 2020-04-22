using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider uiScaleSlider;
    private float scale = 0;

    public delegate void OnVariableChangeDelegate(float newVal);
    public event OnVariableChangeDelegate OnVariableChange;

    private void Start()
    {
        OnVariableChange += Scale;
    }
    private void Update()
    {
        if (scale != uiScaleSlider.value && OnVariableChange != null)
        {
            scale = uiScaleSlider.value;
            OnVariableChange(0.5f + uiScaleSlider.value);
        }
    }

void Scale(float scale)
    {
        foreach(Transform t in transform)
        {
            t.localScale = new Vector2(scale, scale);
        }
    }
}
