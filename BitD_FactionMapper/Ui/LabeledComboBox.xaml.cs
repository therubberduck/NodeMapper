using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
    }
}