using UnityEngine;
using System.Collections;

public enum TextAlignment
{
    Left,
    Center,
    Right
}

[AddComponentMenu("Object Notes/Object Note"), ExecuteInEditMode]
public class ObjectNote : MonoBehaviour
{
    public bool IsNew = true;

    [SerializeField, Multiline]
    protected string Text;
    public string NoteText {
        get { return Text; }
        set {
            Text = value;
            SetStyle(false);
        }
    }

    public Texture2D Image;
    public GUIStyle ImgStyle;
    public Vector2 ImageScale = Vector2.one;
    public Vector2 ImageOffset = Vector2.zero;

    public bool ShowWhenSelected = true;
    public bool ShowWhenUnselected = false;
    public bool ShowInGameEditor = false;
    protected bool editorStyleChanged;

    [SerializeField]
    protected float Offset = 0f;
    public Vector3 AnchorOffset = new Vector3(0f, 0.6f, 0f);
    public Vector2 PixelOffset = new Vector2(0f, 0f);

    public bool Open = false;
    public Color Color = Color.white;
    protected Color textColor = Color.black;
    public Color TextColor { get { return textColor; } }

    [Range(4, 72)]
    public int FontSize = 9;
    public bool Bold = false;
    public TextAlignment Alignment;

    public Vector2 Size, GameSize;

    Texture2D colorTex;
    public Texture2D ColorTex
    {
        get {
            if (colorTex == null)
            {
                colorTex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                ColorTex.SetPixel(0, 0, Color);
                ColorTex.Apply();
            }
            return colorTex;
        }
    }


    public GUIStyle Style = null, GameStyle = null;
    public void SetStyle(bool inGame)
    {
        ColorTex.SetPixel(0, 0, Color);
        ColorTex.Apply();

        GUIStyle style = new GUIStyle();
        style.normal.background = null;
        style.normal.textColor = (Color.r + Color.g + Color.b < 1.5f) ? Color.white : Color.black;;
        style.fontSize = FontSize;
        style.fontStyle = Bold ? FontStyle.Bold : FontStyle.Normal;
        style.fixedWidth = 0;
        style.fixedHeight = 0;
        style.padding = new RectOffset(6, 6, 5, 5);
        style.wordWrap = true;
        style.alignment = Alignment == TextAlignment.Left ? TextAnchor.MiddleLeft : Alignment == TextAlignment.Center ? TextAnchor.MiddleCenter : TextAnchor.MiddleRight;

        ImgStyle = new GUIStyle();
        ImgStyle.normal.background = Image;
        ImgStyle.fixedWidth = 0;
        ImgStyle.fixedHeight = 0;
        ImgStyle.alignment = TextAnchor.MiddleCenter;
        ImgStyle.stretchWidth = true;
        ImgStyle.stretchHeight = true;

        if (inGame)
        {
            GameStyle = style;
            GameSize = style.CalcSize(new GUIContent(Text));
        }
        else
        {
            Style = style;
            Size = style.CalcSize(new GUIContent(Text));
            editorStyleChanged = true;
        }
    }

    void Awake()
    {
        if(Offset != 0f) { // Backward compatibility
            AnchorOffset.y = Offset;
            Offset = 0f;
        }
    }

#if UNITY_EDITOR
    void OnGUI()
    {
        if(ShowInGameEditor) {
            if (editorStyleChanged || GameStyle == null || GameSize.x == 0f)
                SetStyle(true);

            Vector2 guiPosition = Camera.main.WorldToScreenPoint(transform.position - AnchorOffset);
            guiPosition.y = Screen.height - guiPosition.y - GameSize.y / 2f;
            guiPosition.x -= GameSize.x / 2f;
            guiPosition += PixelOffset;
            GUI.backgroundColor = Color;
            if (!string.IsNullOrEmpty(Text))
            {
                GUI.DrawTexture(new Rect(guiPosition.x, guiPosition.y, GameSize.x, GameSize.y), ColorTex, ScaleMode.StretchToFill);
                GUI.Label(new Rect(guiPosition.x, guiPosition.y, GameSize.x, GameSize.y), Text, GameStyle);
            }
            guiPosition = Camera.main.WorldToScreenPoint(transform.position);
            guiPosition.y = Screen.height - guiPosition.y;
            GUI.backgroundColor = Color.white;
            if (Image != null) GUI.Label(new Rect(guiPosition.x - Image.width * ImageScale.x / 2 + ImageOffset.x, guiPosition.y - Image.height * ImageScale.y / 2 + ImageOffset.y, Image.width * ImageScale.x, Image.height * ImageScale.y), "", ImgStyle);
        }
    }
#endif
}
