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
    /// Interaction logic for GlavniProzor.xaml
    /// </summary>
    public partial class GlavniProzor : Window
    {
        public GlavniProzor()
        {
            InitializeComponent();

            GlavniProzorViewModel vm = new GlavniProzorViewModel(Mediator.Instance);
            this.DataContext = vm;
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            AddItemProzor addItemWindow = new AddItemProzor();
            addItemWindow.DataContext = new AddItemViewModel(Mediator.Instance);
            addItemWindow.ShowDialog();
        }

        private void btnEditItem_Click(object sender, RoutedEventArgs e)
        {
            GlavniProzorViewModel viewModel = (GlavniProzorViewModel)DataContext;
            EditItemProzor editItemViewModel = new EditItemProzor();
            editItemViewModel.DataContext = new EditItemViewModel(viewModel.CurrentItem.Clone(), Mediator.Instance);
            editItemViewModel.ShowDialog();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
