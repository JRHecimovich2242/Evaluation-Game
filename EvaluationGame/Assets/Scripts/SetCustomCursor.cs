using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCustomCursor : MonoBehaviour
{
    [SerializeField] Texture2D _crosshair;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 cursorOffset = new Vector2(_crosshair.width / 2, _crosshair.height / 2);
        Cursor.SetCursor(_crosshair, cursorOffset, CursorMode.Auto);
    }
}
