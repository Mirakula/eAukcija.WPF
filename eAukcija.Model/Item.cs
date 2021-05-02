using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;
using System.Collections;
using System.Text.RegularExpressions;

namespace eAukcija.Model
{
    public class Item : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region Properties
        public int ItemId
        {
            get { return _itemId; }
            set
            {
                if (_itemId == value)
                    return;

                _itemId = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ItemId"));
            }
        }
        public string ItemName
        {
            get { return _itemName; }
            set
            {
                if (_itemName == value)
                    return;

                _itemName = value;

                List<string> errors = new List<string>();
                bool valid = true;

                if (value == null || value == "")
                {
                    errors.Add("Ime artikla ne može biti prazno !");
                    SetError("ItemName", errors);
                    valid = false;
                }

                if (Regex.Match(value, @"^[a - zA - Z] + $").Success)
                {
                    errors.Add("Ime artikla može samo da sadrži slova !");
                    SetError("ItemName", errors);
                    valid = false;
                }

                if (valid)
                    ClearErrors("ItemName");

                OnPropertyChanged(new PropertyChangedEventArgs("ItemName"));
            }
        }

        public byte[] ItemImage
        {
            get { return _itemImage; }
            set
            {
                if (_itemImage == value)
                    return;

                _itemImage = value;

                List<string> errors = new List<string>();
                bool valid = true;

                if (value == null)
                {
                    errors.Add("Dodajte fotografiju artikla");
                    SetError("ItemImage", errors);
                    valid = false;
                }

                if (valid)
                    ClearErrors("ItemImage");

                OnPropertyChanged(new PropertyChangedEventArgs("ItemImage"));
            }
        }
        public double ItemPrice
        {
            get { return _itemPrice; }
            set
            {
                if (_itemPrice == value)
                    return;

                _itemPrice = value;

                List<string> errors = new List<string>();
                bool valid = true;

                if (value == 0.0f || value == 0)
                {
                    errors.Add("Unesite cijenu artikla");
                    SetError("ItemPrice", errors);
                    valid = false;
                }

                if (valid)
                    ClearErrors("ItemPrice");

                OnPropertyChanged(new PropertyChangedEventArgs("ItemPrice"));
            }
        }
        public string ItemLocation
        {
            get { return _itemLocation; }
            set
            {
                if (_itemLocation == value)
                    return;

                _itemLocation = value;

                List<string> errors = new List<string>();
                bool valid = true;

                if (value == null || value == "")
                {
                    errors.Add("Unesite lokaciju artikla");
                    SetError("ItemLocation", errors);
                    valid = false;
                }

                if (valid)
                    ClearErrors("ItemLocation");

                OnPropertyChanged(new PropertyChangedEventArgs("ItemLocation"));
            }
        }

        public string ItemImagePath 
        {
            get { return _itemImagePath; }
            set 
            {
                if (_itemImagePath == value)
                    return;

                _itemImagePath = value;

                List<string> errors = new List<string>();
                bool valid = true;

                if (value == null)
                {
                    errors.Add("Dodajte fotografiju artikla");
                    SetError("ItemImagePath", errors);
                    valid = false;
                }

                if (valid)
                    ClearErrors("ItemImagePath");

                OnPropertyChanged(new PropertyChangedEventArgs("ItemImagePath"));
            }
        }
        public BitmapImage ImageToShow { get; set; }

        public bool IsDeleted
        {
            get { return _isDeleted; }
            set
            {
                if (_isDeleted == value)
                    return;

                _isDeleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleted"));
            }
        }

        public bool IsSold 
        {
            get { return _isSold; }
            set
            {
                if (_isSold == value)
                    return;

                _isSold = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSold"));
            }
        }

        #endregion

        #region Fields
        private int _itemId;
        private string _itemName;
        private byte[] _itemImage;
        private double _itemPrice;
        private string _itemLocation;
        private string _itemImagePath;
        private BitmapImage _imageToShow;
        private bool _isDeleted;
        private bool _isSold;
        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #region Konstruktori
        public Item() { }
        public Item(int itemId, string itemName, byte[] itemImage, string itemImagePath, BitmapImage ImageToShow, double itemPrice, string itemLocation, bool isDeleted, bool isSold)
        {
            _itemId = itemId;
            _itemName = itemName;
            _itemImage = itemImage;
            _itemPrice = itemPrice;
            _itemLocation = itemLocation;
            _itemImagePath = itemImagePath;
            _imageToShow = ImageToShow;
            _isDeleted = isDeleted;
            _isSold = isSold;
        }
        #endregion

        #region Data Access
        

        public void InsertItem()
        {
            using (SqlConnection sqlConnection = new SqlConnection())
            {
                sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ToString();

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Connection.Open();

                    sqlCommand.CommandText = "INSERT INTO Items (ItemName, ItemImage, ItemPrice, ItemLocation, IsDeleted, IsSold)" +
                                             "VALUES (@ItemName, @ItemImage, @ItemPrice, @ItemLocation, @IsDeleted, @IsSold)";

                    sqlCommand.Parameters.Add("@ItemName", SqlDbType.NVarChar).Value = this.ItemName;
                    sqlCommand.Parameters.Add("@ItemImage", SqlDbType.VarBinary).Value = this.ItemImage;
                    sqlCommand.Parameters.Add("@ItemPrice", SqlDbType.Money).Value = this.ItemPrice;
                    sqlCommand.Parameters.Add("@ItemLocation", SqlDbType.NVarChar).Value = this.ItemLocation;
                    sqlCommand.Parameters.Add("@IsDeleted", SqlDbType.Int).Value = this.IsDeleted;
                    sqlCommand.Parameters.Add("@IsSold", SqlDbType.Int).Value = this.IsSold;
                        
                    int rows = sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public void UpdateItem()
        {
            using (SqlConnection sqlConnection = new SqlConnection())
            {
                sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ToString();
                sqlConnection.Open();

                string sqlCommand = "UPDATE Items SET ItemName = @ItemName, ItemImage = @ItemImage, ItemPrice = @ItemPrice, ItemLocation = @ItemLocation, IsDeleted = @IsDeleted, IsSold = @IsSold WHERE ItemId = @ItemId";
                SqlCommand command = new SqlCommand(sqlCommand, sqlConnection);

                command.Parameters.Add("@ItemName", SqlDbType.NVarChar).Value = this.ItemName;
                command.Parameters.Add("@ItemImage", SqlDbType.VarBinary).Value = this.ItemImage;
                command.Parameters.Add("@ItemPrice", SqlDbType.Money).Value = this.ItemPrice;
                command.Parameters.Add("@ItemLocation", SqlDbType.NVarChar).Value = this.ItemLocation;
                command.Parameters.Add("@IsDeleted", SqlDbType.NVarChar).Value = this.IsDeleted;
                command.Parameters.Add("@IsSold", SqlDbType.NVarChar).Value = this.IsSold;
                command.Parameters.Add("@ItemId", SqlDbType.Int).Value = this.ItemId;

                int rows = command.ExecuteNonQuery();
            }
        }

        public void DeleteItem()
        {
            using (SqlConnection sqlConnection = new SqlConnection())
            {
                sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ToString();
                sqlConnection.Open();

                string sqlCommand = "UPDATE Items SET IsDeleted = 1 WHERE ItemId = @ItemId";
                SqlCommand command = new SqlCommand(sqlCommand, sqlConnection);

                command.Parameters.Add("@ItemId", SqlDbType.Int).Value = this.ItemId;

                int rows = command.ExecuteNonQuery();
            }
        }
        #endregion

        

        #region DataSetReader
        public static Item GetItemFromReader(SqlDataReader reader)
        {
            Item Item = new Item(
                                Convert.ToInt32(reader["ItemId"]),
                                Convert.ToString(reader["ItemName"]),
                                (byte[])reader["ItemImage"],
                                null,
                                null,
                                Convert.ToDouble(reader["ItemPrice"]),
                                Convert.ToString(reader["ItemLocation"]),
                                Convert.ToBoolean(reader["IsDeleted"]),
                                Convert.ToBoolean((bool)reader["IsSold"])
                            );

            return Item;
        }
        #endregion

        #region Metode kolekcije gresaka
        private void SetError(string propertyName, List<string> propertyErrors)
        {
            // Obrisi sve greske koje postoje za property
            errors.Remove(propertyName);

            // Dodaj kolekciju gresaka za taj property
            errors.Add(propertyName, propertyErrors);

            // Podigni event
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void ClearErrors(string propertyName)
        {
            // Otkloni listu gresaka propertija
            errors.Remove(propertyName);

            // Podigni event
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return errors.Values;
            else
            {
                if (errors.ContainsKey(propertyName))
                    return errors[propertyName];
                else
                    return null;
            }
        }

        public bool HasErrors
        {
            get { return errors.Count > 0; }
        }
        #endregion

        public Item Clone()
        {
            Item clonedItem = new Item();
            clonedItem.ItemName = this.ItemName;
            clonedItem.ItemImage = this.ItemImage;
            clonedItem.ItemLocation = this.ItemLocation;
            clonedItem.ItemPrice = this.ItemPrice;
            clonedItem.ItemId = this.ItemId;

            return clonedItem;
        }

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                    return false;
            
            Item objItem = (Item)obj;
            if (objItem.ItemId == this.ItemId)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return ItemId.GetHashCode();
        }
    }
}
