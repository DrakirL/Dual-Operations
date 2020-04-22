using UnityEngine.UI;
using UnityEngine;

public class TextFadeIn : MonoBehaviour
{
    TMPro.TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    private void Update()
    {
        text.alpha += Time.deltaTime * 0.3f;
    }
}
