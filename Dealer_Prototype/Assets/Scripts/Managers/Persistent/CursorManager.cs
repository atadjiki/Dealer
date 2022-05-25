using UnityEngine;

public class CursorManager : MonoBehaviour
{
    //cursor materials
    [Header("Cursor Textures")]
    [SerializeField] private Texture2D Cursor_default;
    [SerializeField] private Texture2D Cursor_cancel;
    [SerializeField] private Texture2D Cursor_interaction;

    private static CursorManager _instance;

    public static CursorManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void ToDefault()
    {
        Cursor.SetCursor(Cursor_default, new Vector2(Cursor_default.width / 2, Cursor_default.height / 2), CursorMode.Auto);
    }

    public void ToCancel()
    {
        Cursor.SetCursor(Cursor_cancel, new Vector2(Cursor_cancel.width / 2, Cursor_default.height / 2), CursorMode.Auto);
    }

    public void ToInteract()
    {
        Cursor.SetCursor(Cursor_interaction, new Vector2(Cursor_interaction.width / 2, Cursor_interaction.height / 2), CursorMode.Auto);
    }

    public void ToMove()
    {
        Cursor.SetCursor(Cursor_interaction, new Vector2(Cursor_interaction.width / 2, Cursor_interaction.height / 2), CursorMode.Auto);
    }
}
