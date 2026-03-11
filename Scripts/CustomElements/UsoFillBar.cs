using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements.CustomElements
{
    [UxmlElement]
    public partial class UsoFillBar : VisualElement
    {
        [UxmlAttribute]
        public int minBarHeight
        {
            get => _minBarHeight;
            set
            {
                _minBarHeight = value;
                _fillBackground.style.minHeight = new StyleLength(Length.Pixels(_minBarHeight));
                _fill.style.minHeight = new StyleLength(Length.Pixels(_minBarHeight));
            }
        }
        private int _minBarHeight = 1;

        [UxmlAttribute]
        public Color FillColor
        {
            get => _fillColor;
            set
            {
                _fillColor = value;
                _fill.style.backgroundColor = value;
            }
        }
        private Color _fillColor = Color.white;

        [UxmlAttribute]
        public Color FillColorBackground
        {
            get => _fillColorBackground;
            set
            {
                _fillColorBackground = value;
                _fillBackground.style.backgroundColor = value;
            }
        }
        private Color _fillColorBackground = Color.gray;

        [UxmlAttribute]
        public float MaxAmount
        {
            get => _maxAmount;
            set
            {
                _maxAmount = value;
                UpdateFillBarMath(value);
            }
        }
        private float _maxAmount;

        [UxmlAttribute]
        public float MinAmount
        {
            get => _minAmount;
            set
            {
                _minAmount = value;
                UpdateFillBarMath(value);
            }
        }
        private float _minAmount;

        [UxmlAttribute]
        public float CurrentAmount
        {
            get
            {
                return _currentAmount;
            }
            set
            {
                var clampedValue = Mathf.Clamp(value, MinAmount, MaxAmount);
                _currentAmount = clampedValue;
                UpdateFillBarMath(value);
            }
        }

        private float _currentAmount;
        private VisualElement _fill;
        private VisualElement _fillBackground;
        private float _tolerance = 0.0001f;


        public UsoFillBar()
        {
            AddToClassList("uso-player-stat-bar");
            this.style.flexGrow = 1;
            _fillBackground = new VisualElement();
            _fillBackground.AddToClassList("uso-player-stat-bar--fill-background");

            //_fillBackground.style.flexGrow = 1;
            _fillBackground.style.flexShrink = 1;
            _fillBackground.style.minHeight = minBarHeight;
            _fillBackground.style.width = new StyleLength(Length.Percent(100));
            _fillBackground.style.backgroundColor = FillColorBackground;
            _fillBackground.style.alignContent = Align.Center;

            _fill = new VisualElement();
            _fill.AddToClassList("uso-player-stat-bar--fill");
            _fill.style.backgroundColor = FillColor;
            _fill.style.flexGrow = 1;
            _fill.style.flexShrink = 1;
            _fill.style.minHeight = new StyleLength(Length.Percent(100));
            _fill.style.minWidth = new StyleLength(Length.Percent(CurrentAmount));
            _fill.style.maxWidth = new StyleLength(Length.Percent(CurrentAmount));
            _fillBackground.Add(_fill);
            Add(_fillBackground);
        }

        private void UpdateFillBarMath(float value)
        {
            _currentAmount = Mathf.Clamp(value, MinAmount, MaxAmount);
            if (Mathf.Abs(MaxAmount - MinAmount) > _tolerance)
            {
                _currentAmount = (_currentAmount - MinAmount) / (MaxAmount - MinAmount) * 100;
            }
            else
            {
                _currentAmount = 0;
            }
            _fill.style.minWidth = new StyleLength(Length.Percent(_currentAmount));
            _fill.style.maxWidth = new StyleLength(Length.Percent(_currentAmount));
        }
    }
}