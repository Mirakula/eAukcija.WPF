using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace eAukcija.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedUICommand CustomCommand = new RoutedUICommand("CustomCommand", "CustomCommand",
            typeof(MainWindow),
            new InputGestureCollection()
            {
                        new KeyGesture(Key.B, ModifierKeys.Alt)
            });



        public int Custom
        {
            get { return (int)GetValue(CustomProperty); }
            set { SetValue(CustomProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Custom.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomProperty =
            DependencyProperty.Register("Custom", typeof(int), typeof(MainWindow), new PropertyMetadata(0));

        public MainWindow()
        {
            InitializeComponent();

            CommandBinding binding = new CommandBinding(ApplicationCommands.Save);
            binding.Executed += SaveCommand_Executed;
            this.CommandBindings.Add(binding);
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Save Command executed !");
        }

        // private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        // {
        //     if (cmdEnableChkBox != null)
        //         e.CanExecute = cmdEnableChkBox.IsChecked == true ? true : false;
        // }
        private void TxtBox_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (selectionStartLbl != null)
            {
                selectionStartLbl.Content = txtBox.SelectionStart;
            }
            if (selectionLengthLbl != null)
            {
                selectionLengthLbl.Content = txtBox.SelectionLength;
            }
            if (selectionTextLbl != null)
            {
                selectionTextLbl.Text = txtBox.SelectedText;
            }
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedItems == null) return;

            StringBuilder message = new StringBuilder();

            message.AppendLine("Selected:");

            foreach (var item in listBox.SelectedItems)
            {
                message.AppendLine(item.ToString());
            }

            MessageBox.Show(message.ToString());
        }
    }
}
