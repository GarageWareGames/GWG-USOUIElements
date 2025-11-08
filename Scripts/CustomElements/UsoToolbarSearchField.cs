using System.Collections;
using System.Collections.Generic;
using GWG.UsoUIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class UsoToolbarSearchField : BindableElement, INotifyValueChanged<string>
{
    private string _value;
    // Start is called before the first frame update

    public UsoTextField textfield
    {
        get;
        set;
    }

    public string value
    {
        get
        {
            return _value;
        }
        set
        {
            textfield.SetValueWithoutNotify(value);
        }
    }

    public void SetValueWithoutNotify(string newValue)
    {
        _value = newValue;
        textfield.value = _value;
    }
    string INotifyValueChanged<string>.value
    {
        get
        {
            return _value;
        }
        set
        {
            textfield.SetValueWithoutNotify(value);
        }
    }

    public UsoToolbarSearchField()
    {
        style.flexDirection = FlexDirection.Row;
        style.flexGrow = 1;
        style.flexShrink = 1;
        style.alignItems = Align.Center;
        textfield = new UsoTextField();
        textfield.style.flexGrow = 1;
        textfield.style.flexShrink = 1;
        textfield.style.marginRight = 0;

        Add(textfield);
        textfield.RegisterValueChangedCallback(evt =>
        {
            _value = evt.newValue;
            this.value = _value;
        });

        // add a clear button
        UsoToolbarButton clearButton = new UsoToolbarButton
        {
            style =
            {
                maxWidth = 20,
                maxHeight = 20,
                flexShrink = 0,
                flexGrow = 0,
                marginLeft = 0
            },
            text = "X"
        };
        clearButton.AddToClassList("clear-button");
        clearButton.clickable.clicked += () =>
        {
            value = "";
        };
        Add(clearButton);
    }

}
