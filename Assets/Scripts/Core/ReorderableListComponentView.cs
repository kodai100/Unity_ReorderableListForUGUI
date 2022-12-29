using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public abstract class ReorderableListComponentView<T> : MonoBehaviour where T : ReorderableListComponentData
{
    [SerializeField] private DragController _dragController;
    [SerializeField] private Button _selectButton;
    [SerializeField] private Image _backgroundImage;

    [SerializeField] private Color _selectedColor = Color.black;
    [SerializeField] private Color _unselectedColor = Color.grey;
    
    public IObservable<Guid> OnSelect => _selectButton.OnClickAsObservable().Select(_ => _data.Guid);
    public DragController DragController => _dragController;

    private T _data;
    public T Data => _data;

    public virtual void Initialize(T data)
    {
        _data = data;
        name = data.Guid.ToString();
        UnSelect();
    }

    public void Select()
    {
        _backgroundImage.color = _selectedColor;
    }

    public void UnSelect()
    {
        _backgroundImage.color = _unselectedColor;
    }
    
    public void RecalculateIndex()
    {
        _data.Index = transform.GetSiblingIndex();
    }
}
