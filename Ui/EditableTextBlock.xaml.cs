using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NodeMapper.Ui
{
    public partial class EditableTextBlock
    {
        public EditableTextBlock()
        {
            InitializeComponent();
        }
        
        public string LabelText {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }
        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register("LabelText", typeof(string), typeof(EditableTextBlock), new PropertyMetadata(default(string),
                (d,e)=> ((EditableTextBlock)d)?.SetLabelText()));

        private void SetLabelText()
        {
            label.Content = LabelText;
        }
        
        public string Text {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(EditableTextBlock), new PropertyMetadata(default(string),
                (d,e)=> ((EditableTextBlock)d)?.SetText()));

        private void SetText()
        {
            textBlock.Text = Text;
            if (textBox.Text != Text)
            {
                textBox.Text = Text;                
            }
        }

        private void TextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            ChangeFocus(true);
        }

        private void TextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            ChangeFocus(false);
        }

        private void EditableTextBlock_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangeFocus(true);
        }

        private void ChangeFocus(bool textBoxFocused)
        {
            if (textBoxFocused)
            {
                textBlock.Visibility = Visibility.Collapsed;
                textBox.Visibility = Visibility.Visible;
            }
            else
            {
                textBlock.Visibility = Visibility.Visible;
                textBox.Visibility = Visibility.Collapsed;
            }
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Text = textBox.Text;
        }
    }
}