using TMPro;
using UnityEngine;

public class SampleReorderableListComponentView : ReorderableListComponentView<SampleReorderableListComponentData>
{
    [SerializeField] private TMP_Text _text;

    public override void Initialize(SampleReorderableListComponentData data)
    {
        base.Initialize(data);
        _text.text = data.Text;
    }
}
