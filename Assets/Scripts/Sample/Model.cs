using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class Entity
{
    public Guid Id;
    public int OrderIndex;
    public string Name;
}

public class Model
{

    private readonly ReactiveProperty<List<Entity>> _data = new(new List<Entity>());
    public IReadOnlyReactiveProperty<List<Entity>> Data => _data;
    
    private readonly ReactiveProperty<Entity> _selected = new(null);
    public IReadOnlyReactiveProperty<Entity> Selected => _selected;

    public Model()
    {
        _data.Value = CreateMockData().ToList();
    }

    private IEnumerable<Entity> CreateMockData()
    {
        var list = new List<Entity>();
        
        for (var i = 0; i < 20; i++)
        {
            var guid = Guid.NewGuid();
            list.Add(new Entity
            {
                Id = guid,
                OrderIndex = i,
                Name = guid.ToString()
            });
        }

        return list;
    }
    
    public void UpdateSelection(Guid? id)
    {
        var selectedTarget = _data.Value.FirstOrDefault(x => x.Id == id);
        Debug.Log($"Update Selection : {id}");
        _selected.SetValueAndForceNotify(selectedTarget);
    }

    public void UpdateSelectedData(string name)
    {
        if (_selected.Value == null) return;

        _selected.Value.Name = name;
        _selected.SetValueAndForceNotify(_selected.Value);
    }

    public void UpdateOrder(IEnumerable<Entity> list)
    {
        foreach (var target in list)
        {
            var t = _data.Value.FirstOrDefault(entity => target.Id == entity.Id);
            if(t == null) continue;
            t.OrderIndex = target.OrderIndex;
        }
        
        _data.SetValueAndForceNotify(_data.Value.OrderBy(x => x.OrderIndex).ToList());
    }

    public void AddData()
    {
        var guid = Guid.NewGuid();
        
        var data = new Entity
        {
            Id = guid,
            OrderIndex = _data.Value.Count,
            Name = guid.ToString()
        };
        
        _data.Value.Add(data);
        _data.SetValueAndForceNotify(_data.Value);
    }

    public void RemoveData()
    {
        if(_selected.Value == null) return;

        var target = _selected.Value;

        _data.Value.Remove(target);
        _data.SetValueAndForceNotify(_data.Value);

        _selected.Value = null;
    }

    public void ClearData()
    {
        _data.Value.Clear();
        _data.SetValueAndForceNotify(_data.Value);

        _selected.Value = null;
    }
    
}