using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Test : MonoBehaviour
{

    [SerializeField] private TestPresenter _presenter;

    private List<IDisposable> _disposable = new();
    
    private void Start()
    {
        var model = new Model();
        
        _disposable.AddRange(_presenter.Bind(model));

        _disposable.AddRange(Setup(model));
    }
    
    private IEnumerable<IDisposable> Setup(Model model)
    {

        yield return _presenter.OnAddButtonClicked.Subscribe(_ =>
        {
            model.AddData();
        });

        yield return _presenter.OnSelected.Subscribe(guid =>
        {
            model.UpdateSelection(guid);
        });

        yield return _presenter.OnRemoveButtonClicked.Subscribe(_ =>
        {
            model.RemoveData();
        });

        yield return _presenter.OnClearButtonClicked.Subscribe(_ =>
        {
            model.ClearData();
        });
        
        yield return _presenter.OnOrderChanged.Subscribe(list =>
        {
            model.UpdateOrder(list);
        });

    }

    private void OnDestroy()
    {
        _disposable.ForEach(d =>
        {
            d.Dispose();
        });
        _disposable.Clear();
    }
}