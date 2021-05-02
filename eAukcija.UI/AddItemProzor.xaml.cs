using eAukcija.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace eAukcija.UI
{
    /// <summary>
    /// Interaction logic for AddItemProzor.xaml
    /// </summary>
    public partial class AddItemProzor : Window
    {
        public AddItemProzor()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddItemViewModel vm = (AddItemViewModel)DataContext;
            vm.Done += Vm_Done;
        }

        private void Vm_Done(object sender, AddItemViewModel.DoneEventArgs e)
        {
            MessageBox.Show(e.Message);
        }
    }
}
