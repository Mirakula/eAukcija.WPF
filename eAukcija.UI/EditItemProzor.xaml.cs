using eAukcija.ViewModel;
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
    /// Interaction logic for EditItemProzor.xaml
    /// </summary>
    public partial class EditItemProzor : Window
    {
        public EditItemProzor()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            EditItemViewModel vm = (EditItemViewModel)DataContext;
            vm.Done += Vm_Done;
        }

        private void Vm_Done(object sender, EditItemViewModel.DoneEventArgs e)
        {
            MessageBox.Show(e.Message);
        }
    }
}
