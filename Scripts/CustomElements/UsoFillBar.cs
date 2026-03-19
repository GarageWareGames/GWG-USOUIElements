using System;
using System.Collections.Generic;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements.CustomElements
{
    [UxmlElement]
    public partial class UsoFillBar : VisualElement, IBindable, INotifyValueChanged<float>, IUsoUiElement
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
                UpdateFillBarMath();
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
                UpdateFillBarMath();
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
                _currentAmount = value;
                UpdateFillBarMath();
            }
        }

        private float _currentAmount;
        private VisualElement _fill;
        private VisualElement _fillBackground;
        private float _tolerance = 0.0001f;
        private IBinding _binding;
        private string _bindingPath;


        public float value
        {
            get => CurrentAmount;
            set
            {
                if (!EqualityComparer<float>.Default.Equals(CurrentAmount, value))
                {
                    float previousValue = CurrentAmount;
                    SetValueWithoutNotify(value);

                    // Send a change event to notify the binding system
                    using (ChangeEvent<float> evt = ChangeEvent<float>.GetPooled(previousValue, value))
                    {
                        evt.target = this;
                        SendEvent(evt);
                    }
                }
            }
        }

        public void SetValueWithoutNotify(float newValue)
        {
            CurrentAmount = newValue;
            // Update the UI representation here (e.g., a Label within this custom control)
            // labelElement.text = m_Value.ToString();
        }

        public UsoFillBar()
        {
            InitializeFillBar();
        }

        public UsoFillBar(string fieldName) : base()
        {
            name = fieldName;
            InitializeFillBar();
        }

        public UsoFillBar(string fieldName, out UsoFillBar newField) : base()
        {
            name = fieldName;
            InitializeFillBar();
            newField = this;
        }

        public UsoFillBar(string fieldName, float minAmount, float maxAmount, float currentAmount) : base()
        {
            name = fieldName;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
            CurrentAmount = currentAmount;
            InitializeFillBar();
        }

        public UsoFillBar(string fieldName, float minAmount, float maxAmount, float currentAmount, out UsoFillBar newField) : base()
        {
            name = fieldName;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
            CurrentAmount = currentAmount;
            InitializeFillBar();
            newField = this;
        }

        private void InitializeFillBar()
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



        private void UpdateFillBarMath()
        {
            float newAmount = 0;
            newAmount = Mathf.Clamp(CurrentAmount, MinAmount, MaxAmount);
            if (Mathf.Abs(MaxAmount - MinAmount) > _tolerance)
            {
                newAmount = (newAmount - MinAmount) / (MaxAmount - MinAmount) * 100;
            }
            else
            {
                newAmount = 0;
            }
            _fill.style.minWidth = new StyleLength(Length.Percent(newAmount));
            _fill.style.maxWidth = new StyleLength(Length.Percent(newAmount));
        }


        public IBinding binding
        {
            get
            {
                return _binding;
            }
            set
            {
                _binding = value;
            }
        }
        public string bindingPath
        {
            get
            {
                return _bindingPath;
            }
            set
            {
                _bindingPath = value;
            }
        }

        #region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation

        /// <summary>
        /// CSS class name applied to all UsoSlideToggle instances for styling purposes.
        /// </summary>
        private const string ElementClass = "uso-slide-toggle";

        /// <summary>
        /// CSS class name applied when field validation/status functionality is enabled.
        /// </summary>
        private const string ElementValidationClass = "uso-field-validation";

        /// <summary>
        /// Default binding property used when applying data bindings to this field.
        /// Binds to the 'value' property which controls the toggle's boolean state.
        /// </summary>
        private const string DefaultBindProp = "value";

        /// <summary>
        /// Gets the current field status type, which determines the visual state and validation feedback.
        /// This property is automatically reflected in the UI through CSS class modifications.
        /// </summary>
        /// <value>The current FieldStatusTypes value indicating the field's validation state.</value>
        [UxmlAttribute]
        public FieldStatusTypes FieldStatus
        {
            get
            {
                return _fieldStatus;
            }
            private set
            {
                _fieldStatus = value;
                UsoUiHelper.SetFieldStatus(this, value);
            }
        }
        private FieldStatusTypes _fieldStatus;

        /// <summary>
        /// Gets or sets whether field status/validation functionality is enabled for this control.
        /// When enabled, adds validation CSS class for styling. When disabled, removes validation styling.
        /// </summary>
        /// <value>True if field status functionality is enabled; otherwise, false. Default is true.</value>
        [UxmlAttribute]
        public bool FieldStatusEnabled
        {
            get
            {
                return _fieldStatusEnabled;
            }

            private set
            {
                _fieldStatusEnabled = value;
                if (value)
                {
                    AddToClassList(ElementValidationClass);
                }
                else
                {
                    RemoveFromClassList(ElementValidationClass);
                }
            }
        }
        private bool _fieldStatusEnabled = false;

        /// <summary>
        /// Applies data binding to update the fill value of this control using Unity's data binding system. Will create a read only link to the CurrentAmount property.
        /// </summary>
        /// <param name="fieldBindingPath">The path to the data source property to bind from.</param>
        /// <exception cref="Exception">Thrown when binding setup fails. Original exception is preserved and re-thrown.</exception>
        public void ApplyBinding(string fieldBindingPath)
        {
            try
            {
                SetBinding(DefaultBindProp, new DataBinding()
                {
                    dataSourcePath = new PropertyPath(fieldBindingPath),
                    bindingMode = BindingMode.ToTarget
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Updates the field's status type, which affects its visual appearance and validation state.
        /// The status change is automatically reflected in the UI through the FieldStatus property.
        /// </summary>
        /// <param name="fieldStatus">The new field status type to apply.</param>
        public void SetFieldStatus(FieldStatusTypes fieldStatus)
        {
            FieldStatus = fieldStatus;
        }

        /// <summary>
        /// Controls the visibility and functionality of the field status/validation system.
        /// When disabled, removes validation-related styling from the control.
        /// </summary>
        /// <param name="status">True to enable field status functionality; false to disable it.</param>
        public void ShowFieldStatus(bool status)
        {
            FieldStatusEnabled = status;
        }

        /// <summary>
        /// Retrieves the first ancestor UsoLineItem control in the visual tree hierarchy.
        /// This is useful for accessing parent container functionality and maintaining proper UI structure.
        /// </summary>
        /// <returns>The parent UsoLineItem if found; otherwise, null.</returns>
        public UsoLineItem GetParentLineItem()
        {
            return GetFirstAncestorOfType<UsoLineItem>();
        }

        public void ClearField()
        {
            SetFieldStatus(FieldStatusTypes.Default);
        }
        // End IUsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
#endregion
    }
}