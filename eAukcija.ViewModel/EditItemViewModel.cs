using eAukcija.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace eAukcija.ViewModel
{
    public class EditItemViewModel : INotifyPropertyChanged
    {
        #region Konstruktori
        public EditItemViewModel()
        {
            InsertImageCommand = new RelayCommand(InsertImageExecute, CanInsertImage);
            UpdateItemCommand = new RelayCommand(UpdateItemExecute, CanUpdateItem);

            CurrentItem = new Item();
        }

        public EditItemViewModel(Mediator mediator)
        {
            this._mediator = mediator;

            InsertImageCommand = new RelayCommand(InsertImageExecute, CanInsertImage);
            UpdateItemCommand = new RelayCommand(UpdateItemExecute, CanUpdateItem);

            CurrentItem = new Item();
        }

        public EditItemViewModel(Item item, Mediator mediator)
        {
            this._mediator = mediator;

            InsertImageCommand = new RelayCommand(InsertImageExecute, CanInsertImage);
            UpdateItemCommand = new RelayCommand(UpdateItemExecute, CanUpdateItem);

            CurrentItem = item;
        }

        #endregion

        #region Fields
        private ICommand _insertImageCommand;
        private ICommand _updateItemCommand;
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

        public ICommand UpdateItemCommand 
        {
            get { return _updateItemCommand; }
            set
            {
                if (_updateItemCommand == value)
                    return;

                _updateItemCommand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateItemCommand"));
            }
        }
        #endregion

        #region ICommands
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

        void UpdateItemExecute(object obj)
        {

        }

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

        bool CanInsertImage(object obj) => true;
        bool CanUpdateItem(object obj) => true;
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
        public void OnDone(DoneEventArgs e) => Done?.Invoke(this, e);
    }
}
