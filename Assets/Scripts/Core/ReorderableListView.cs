using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public abstract class ReorderableListView<T> : MonoBehaviour where T : ReorderableListComponentData
{

    [SerializeField] private ReorderableListComponentView<T> _componentViewPrefab;
    [SerializeField] private ScrollRect _scrollRect;

    private List<ReorderableListComponentView<T>> _components = new();
    
    private Subject<Guid> _onComponentSelected = new();
    public IObservable<Guid> OnComponentSelected => _onComponentSelected;

    public IObservable<IEnumerable<T>> OnListOrderChanged => _onListOrderChanged;
    private Subject<IEnumerable<T>> _onListOrderChanged = new();

    private List<IDisposable> _disposables = new();

    public void Initialize(IEnumerable<T> dataList)
    {
        ClearAll();

        foreach (var data in dataList)
        {
            var component = Instantiate(_componentViewPrefab, _scrollRect.content);
            component.Initialize(data);
            
            component.DragController.OnOrderChanged.Subscribe(_ =>
            {
                _components.ForEach(c =>
                {
                    c.RecalculateIndex();
                });
        
                _onListOrderChanged.OnNext(_components.Select(c => c.Data));
                
            }).AddTo(_disposables);
            
            component.OnSelect.Subscribe(guid =>
            {
                _onComponentSelected.OnNext(guid);
            }).AddTo(_disposables);
            
            _components.Add(component);
        }
    }

    public void MarkAsSelected(Guid? guid)
    {
        _components.ForEach(c => c.UnSelect());
        
        var target = _components.FirstOrDefault(c => c.Data.Guid == guid);
        target?.Select();
    }

    private void ClearAll()
    {
        _components.ForEach(component =>
        {
            Destroy(component.gameObject);
        });
        _components.Clear();
        
        _disposables.ForEach(disposable =>
        {
            disposable.Dispose();
        });
        _disposables.Clear();
    }

}
