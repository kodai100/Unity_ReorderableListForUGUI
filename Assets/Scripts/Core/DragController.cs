using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;


public class DragController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _currentTransform;
    
    private Transform _mainContent;
    private Vector3 _currentPosition;
    private int _totalChild;

    private Subject<Unit> _onOrderChanged = new();
    public IObservable<Unit> OnOrderChanged => _onOrderChanged;

    public void OnPointerDown(PointerEventData eventData)
    {
        _currentPosition = _currentTransform.position;
        _mainContent = _currentTransform.parent.transform;
        _totalChild = _mainContent.transform.childCount;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _currentTransform.position = new Vector3(_currentTransform.position.x, eventData.position.y, _currentTransform.position.z);

        for (var i = 0; i < _totalChild; i++)
        {
            if (i == _currentTransform.GetSiblingIndex()) continue;
            
            var otherTransform = _mainContent.GetChild(i);
            var distance = (int) Vector3.Distance(_currentTransform.position,otherTransform.position);
            
            if (distance <= 10)
            {
                var otherTransformOldPosition = otherTransform.position;
                otherTransform.position = new Vector3(otherTransform.position.x, _currentPosition.y, otherTransform.position.z);
                _currentTransform.position = new Vector3(_currentTransform.position.x, otherTransformOldPosition.y, _currentTransform.position.z);
                _currentTransform.SetSiblingIndex(otherTransform.GetSiblingIndex());
                _currentPosition = _currentTransform.position;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _currentTransform.position = _currentPosition;
        _onOrderChanged.OnNext(Unit.Default);
    }
}