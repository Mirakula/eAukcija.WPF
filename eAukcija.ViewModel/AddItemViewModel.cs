using eAukcija.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace eAukcija.ViewModel
{
    public class AddItemViewModel : INotifyPropertyChanged
    {
        #region Konstruktori
        public AddItemViewModel()
        {
            InsertImageCommand = new RelayCommand(InsertImageExecute, CanInsertImage);
            InsertCommand = new RelayCommand(InsertExecute, CanInsert);
            UpdateCommand = new RelayCommand(UpdateExecute, CanUpdate);
            DeleteCommand = new RelayCommand(DeleteExecute, CanDelete);

            CurrentItem = new Item();
        }

        public AddItemViewModel(Item Item, Mediator mediator)
        {
            this._mediator = mediator;

            InsertImageCommand = new RelayCommand(InsertImageExecute, CanInsertImage);
            InsertCommand = new RelayCommand(InsertExecute, CanInsert);
            UpdateCommand = new RelayCommand(UpdateExecute, CanUpdate);
            DeleteCommand = new RelayCommand(DeleteExecute, CanDelete);

            CurrentItem = Item;
        }

        public AddItemViewModel(Mediator mediator)
        {
            this._mediator = mediator;

            InsertImageCommand = new RelayCommand(InsertImageExecute, CanInsertImage);
            InsertCommand = new RelayCommand(InsertExecute, CanInsert);
            UpdateCommand = new RelayCommand(UpdateExecute, CanUpdate);
            DeleteCommand = new RelayCommand(DeleteExecute, CanDelete);

            CurrentItem = new Item();
        }

        #endregion

        #region Fields
        private ICommand _deleteCommand;
        private ICommand _updateCommand;
        private ICommand _insertCommand;
        private ICommand _insertImageCommand;
        private Mediator _mediator;
        private Item _currentItem;
        private string _imageToShow;
        #endregion

        #region Properties
        public string ImageToShow 
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

        public Item CurrentItem 
        {
            get { return _currentItem; } 
            set
            {
                if (_currentItem == value)
                    return;

                _currentItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentItem"));
            }
        }

        public ICommand InsertImageCommand
        {
            get { return _insertImageCommand; }
            set
            {
                if (_insertImageCommand == value)
                    return;

                _insertImageCommand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InsertImageCommand"));
            }
        }

        public ICommand UpdateCommand
        {
            get { return _updateCommand; } 
            set
            {
                if (_updateCommand == value)
                    return;

                _updateCommand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateCommand"));
            }
        }

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

        public  ICommand InsertCommand
        { 
            get { return _insertCommand; } 
            set
            {
                if (_insertCommand == value)
                    return;

                _insertCommand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InsertCommand"));
            }
        }

        #endregion

        #region ICommand Commands
        void UpdateExecute(object obj)
        {
            if (CurrentItem != null && !CurrentItem.HasErrors)
            {
                CurrentItem.UpdateItem();
                OnDone(new DoneEventArgs("Izmjene su spremljene !"));

                _mediator.Notify("ItemChange", CurrentItem);
            }
        }

        void InsertExecute(object obj)
        {
            if (CurrentItem != null && !CurrentItem.HasErrors)
            {
                CurrentItem.InsertItem();
                OnDone(new DoneEventArgs("Arikal je spremljen !"));

                _mediator.Notify("ItemChange", CurrentItem);
            }
            else
                OnDone(new DoneEventArgs("Provjerite podatke za artikal !"));
        }

        void DeleteExecute(object obj)
        {
            if (CurrentItem != null && !CurrentItem.HasErrors)
            {
                CurrentItem.UpdateItem();
                OnDone(new DoneEventArgs("Artikal je obrisan !"));

                _mediator.Notify("ItemChange", CurrentItem);
            }
        }

        void InsertImageExecute(object obj)
        {
            string destinationFile = string.Empty;

            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
            dialog.Filter = "Image Files (*.bmp;*.png;*.jpg)|*.bmp;*.png;*.jpg";
            destinationFile = dialog.FileName;

            BitmapImage image = new BitmapImage();
            try
            {
                image.BeginInit();
                image.UriSource = new Uri(destinationFile);
                image.EndInit();
                if (CurrentItem != null && !CurrentItem.HasErrors)
                    CurrentItem.ItemImage = ImageToBase64(destinationFile);

                ImageToShow = image.UriSource.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Molimo Vas da dodate fotografiju artikla !", "Greška !", MessageBoxButton.OKCancel);
            }
        }

        bool CanInsertImage(object obj) => true;

        bool CanInsert(object obj) => true;

        bool CanUpdate(object obj) => true;

        bool CanDelete(object obj) => true;
        #endregion
        
        #region Image Converter Logic
        public byte[] ImageToBase64(string imagePath)
        {
            using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                byte[] imageData = new byte[fs.Length];
                fs.Read(imageData, 0, Convert.ToInt32(fs.Length));
                fs.Close();

                return imageData;
            }
        }
        #endregion


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        public delegate void DoneEventHandler(object sender, DoneEventArgs e);

        public class DoneEventArgs : EventArgs
        {
            private string _message;

            public string Message
            { 
                get { return _message; } 
                set 
                {
                    if (_message == value)
                        return;

                    _message = value;
                }
            }

            public DoneEventArgs(string message)
            {
                this._message = message;
            }
        }

        public event DoneEventHandler Done;

        public void OnDone(DoneEventArgs e)
        {
            Done?.Invoke(this, e);
        }
    }
}
