using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class TestPresenter : MonoBehaviour
{

    [SerializeField] private SampleReorderableListView _reorderableListView;

    [SerializeField] private Button _addButton;
    [SerializeField] private Button _removeButton;
    [SerializeField] private Button _clearButton;

    public IObservable<Guid> OnSelected => _reorderableListView.OnComponentSelected;

    public IObservable<IEnumerable<Entity>> OnOrderChanged =>
        _reorderableListView.OnListOrderChanged.Select(d => 
            d.Select(e 
                => new Entity { Id = e.Guid, OrderIndex = e.Index, Name = e.Text }));
    public IObservable<Unit> OnAddButtonClicked => _addButton.OnClickAsObservable();
    public IObservable<Unit> OnRemoveButtonClicked => _removeButton.OnClickAsObservable();
    public IObservable<Unit> OnClearButtonClicked => _clearButton.OnClickAsObservable();

    public IEnumerable<IDisposable> Bind(Model model)
    {

        yield return model.Data.Subscribe(dataList =>
        {
            _reorderableListView.Initialize(dataList.Select(x => new SampleReorderableListComponentData{Guid = x.Id, Index = x.OrderIndex, Text = x.Name}));
        });

        yield return model.Selected.Subscribe(selectedData =>
        {
            _reorderableListView.MarkAsSelected(selectedData?.Id);
        });

    }
    
    
}
