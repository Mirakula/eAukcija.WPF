using eAukcija.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace eAukcija.ViewModel
{
    public class GlavniProzorViewModel : INotifyPropertyChanged
    {
        #region Polja
        private BitmapImage _imageToShow;
        private Item currentItem;
        private ItemCollection itemList;
        private IEnumerable<Item> itemListLinq;
        private User currentUser;
        private List<User> userList;
        private IEnumerable<User> userListLinq;
        private Mediator _mediator;
        private ListCollectionView _itemCollectionView;
        private ICommand _deleteCommand;
        #endregion

        #region Properties

        public ListCollectionView ItemCollectionView 
        { 
            get { return _itemCollectionView; } 
            set
            {
                if (_itemCollectionView == value)
                    return;

                _itemCollectionView = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ItemCollectionView"));
            }
        }

        public Item CurrentItem 
        {
            get { return currentItem; }
            set 
            {
                if (currentItem == value)
                    return;

                currentItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentItem"));
            }
        }
        public ItemCollection ItemList 
        {
            get { return itemList; }
            set
            {
                if (itemList == value)
                    return;

                itemList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ItemList"));
            }
        }

        public IEnumerable<Item> ItemListLinq 
        {
            get { return itemListLinq; }
            set
            {
                if (itemListLinq == value)
                    return;

                itemListLinq = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ItemListLinq"));
            }
        }

        public IEnumerable<User> UserListLinq
        {
            get { return userListLinq; }
            set
            {
                if (userListLinq == value)
                    return;

                userListLinq = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserListLinq"));
            }
        }

        public User CurrentUser
        {
            get { return currentUser; }
            set
            {
                if (currentUser == value)
                    return;

                currentUser = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentUser"));
            }
        }

        public List<User> UserList 
        {
            get { return userList; }
            set
            {
                if (userList == value)
                    return;

                userList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserList"));
            }
        }

        public BitmapImage ImageToShow 
        { 
            get { return _imageToShow; }
            set
            {
                if (_imageToShow == value)
                    return;

                _imageToShow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImageToShow"));
            }
        }
        #endregion

        #region Konstruktor
        public GlavniProzorViewModel(Mediator mediator)
        {

            this._mediator = mediator;

            DeleteCommand = new RelayCommand(DeleteExecute, CanDelete);

            this.PropertyChanged += GlavniProzorViewModel_PropertyChanged;

            ItemList = ItemCollection.GetAllItems();

            foreach (var item in ItemList)
            {
                item.ImageToShow = LoadImage(item.ItemImage);
            }

            ItemCollectionView = new ListCollectionView(ItemList);

            CurrentItem = new Item();
            mediator.Register("ItemChange", ItemChanged);
        }

        private void ItemChanged(object obj)
        {
            Item item = (Item)obj;

            int index = ItemList.IndexOf(item);

            if (index != -1)
            {
                ItemList.RemoveAt(index);
                ItemList.Insert(index, item);
            }
            else
            {
                ItemList.Add(item);
            }
        }

        private void GlavniProzorViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("FilteringText"))
                ItemCollectionView.Refresh();
        }
        #endregion

        #region Image Convertver Logic
        private BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            var image = new BitmapImage();

            using (var memoryStream = new MemoryStream(imageData))
            {
                memoryStream.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = memoryStream;
                image.EndInit();
            }
            return image;
        }

        #endregion

        #region Commands
        public ICommand DeleteCommand
        {
            get { return _deleteCommand; }
            set
            {
                if (_deleteCommand == value)
                    return;

                _deleteCommand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeleteCommand"));
            }
        }

        void DeleteExecute(object obj)
        {
            CurrentItem.DeleteItem();
            ItemList.Remove(CurrentItem);
        }

        bool CanDelete(object obj)
        {
            if (CurrentItem == null)
                return false;

            return true;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }
}
