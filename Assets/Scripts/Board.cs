using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Board : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private LayerMask _boxesLayerMask;
    [SerializeField] private float _toucherRadius;

    [Header("Mark Sprite")]
    [SerializeField] private Sprite _spriteX;
    [SerializeField] private Sprite _spriteO;

    [Header("Mark Colors")]
    [SerializeField] private Color _colorX;
    [SerializeField] private Color _colorO;

    public Mark[] Marks;
    public UnityAction<Mark, Color> OnWinAction;

    private Camera _camera;
    private Mark _currentMark;
    private bool _canPlay;
    private LineRenderer _lineRenderer;

    private int _marksCount = 0;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
        _camera = Camera.main;
        _currentMark = Mark.X;

        Marks = new Mark[9];

        _canPlay = true;
    }

    private void Update()
    {
        if (_canPlay && Input.GetMouseButtonUp(0))
        {
            Vector2 touchPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            Collider2D hit = Physics2D.OverlapCircle(touchPosition, _toucherRadius, _boxesLayerMask);

            if (hit)
            {
                HitBox(hit.GetComponent<Box>());
            }
        }
    }
    private void HitBox(Box box)
    {
        if (!box.IsMarked)
        {
            Marks[box.Index] = _currentMark;
            Debug.Log(Marks[box.Index]);
            
            box.SetAsMarked(GetSprite(), _currentMark, GetColor());
            _marksCount++;

            bool won = CheckIfWin();
            if (won)
            {
                if(OnWinAction != null)
                {
                    OnWinAction.Invoke(_currentMark, GetColor());
                }
                Debug.Log(_currentMark.ToString() + "Wins");
                _canPlay = false;
            }
            if(_marksCount == 9)
            {                
                    if (OnWinAction != null)
                    {
                        OnWinAction.Invoke(Mark.None, Color.white);
                    }
                    Debug.Log("Nobody Wins");
                    _canPlay = false;               
            }            
            SwitchPlayer();
        }
    }
    private bool CheckIfWin()
    {
        return
        AreBoxesMatched(0, 1, 2) || AreBoxesMatched(3, 4, 5) || AreBoxesMatched(6, 7, 8) ||
        AreBoxesMatched(0, 3, 6) || AreBoxesMatched(1, 4, 7) || AreBoxesMatched(2, 5, 8) ||
        AreBoxesMatched(0, 4, 8) || AreBoxesMatched(2, 4, 6);
    }
    private bool AreBoxesMatched(int v1, int v2, int v3)
    {
        Mark m = _currentMark;
        bool matched = (Marks[v1] == m && Marks[v2] == m && Marks[v3] == m);

        if (matched)
        {
            DrawLine(v1, v3);
        }
        return matched;
    }
    private void DrawLine(int i, int k)
    {
        _lineRenderer.SetPosition(0, transform.GetChild(i).position);
        _lineRenderer.SetPosition(1, transform.GetChild(k).position);
        Color color = GetColor();
        color.a = .3f;
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;

        _lineRenderer.enabled = true;
    }
    private void SwitchPlayer()
    {
        _currentMark = (_currentMark == Mark.X) ? Mark.O : Mark.X;
    }
    private Color GetColor()
    {
        return (_currentMark == Mark.X) ? _colorX : _colorO;
    }
    private Sprite GetSprite()
    {
        return (_currentMark == Mark.X) ? _spriteX : _spriteO;
    }
}
