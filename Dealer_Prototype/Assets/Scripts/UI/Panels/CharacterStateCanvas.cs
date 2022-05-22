using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent(typeof(RectTransform))]
public class CharacterStateCanvas : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void SetText_State(string text)
    {
        if(_text != null)
        {
            _text.text = text.ToLower().Trim();
        }
    }
}
