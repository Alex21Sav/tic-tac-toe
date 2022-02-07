using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public int Index;
    public Mark mark;
    public bool IsMarked;

    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();

        Index = transform.GetSiblingIndex();
        mark = Mark.None;
        IsMarked = false;
    }
    public void SetAsMarked(Sprite sprite, Mark mark, Color color)
    {
        IsMarked = true;
        this.mark = mark;
        _renderer.color = color;
        _renderer.sprite = sprite;
        GetComponent<CircleCollider2D>().enabled = false;
    }
}
