using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BitD_FactionMapper.Ui
{
    public partial class LabeledComboBox
    {
        public event SelectionChangedEventHandler SelectionChanged;

        public object SelectedItem
        {
            get => comboBox.SelectedItem;
            set => comboBox.SelectedItem = value;
        }

        public LabeledComboBox()
        {
            InitializeComponent();
            comboBox.SelectionChanged += delegate(object sender, SelectionChangedEventArgs args)
            {
                SelectionChanged?.Invoke(sender, args);
            };
        }

        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register("LabelText", typeof(string), typeof(LabeledComboBox), new PropertyMetadata(
                default(string),
                (d, e) => ((LabeledComboBox)d)?.SetLabelText()));

        private void SetLabelText()
        {
            label.Content = LabelText;
        }

        public void AddItems(params object[] values)
        {
            foreach (var value in values)
            {
                comboBox.Items.Add(value);
            }
        }
        
        public void AddItem(object newItem)
        {
            comboBox.Items.Add(newItem);
        }

        public void SetItems(IEnumerable<object> values)
        {
            comboBox.Items.Clear();
            foreach (var value in values)
            {
                comboBox.Items.Add(value);
            }
        }

        public void SetItems(params object[] values)
        {
            comboBox.Items.Clear();
            foreach (var value in values)
            {
                comboBox.Items.Add(value);
            }
        }

        public void ClearItems()
        {
            comboBox.Items.Clear();
        }

        public void SelectItem<T>(Func<T, bool> selector) where T : class
        {
            foreach (T item in comboBox.Items)
            {
                if (selector.Invoke(item))
                {
                    comboBox.SelectedItem = item;
                    break;
                }
            }
        }
    }
}