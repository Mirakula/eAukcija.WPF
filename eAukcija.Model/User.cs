using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAukcija.Model
{
    public class User : INotifyPropertyChanged
    {
        #region Fields
        private int _userId;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _password;
        private double _userBalance;
        private bool _isAdmin;
        private bool _isLogged;
        private bool _isDeleted;

        #endregion

        #region Properties
        public int UserId
        {
            get { return _userId; }
            set
            {
                if (_userId == value)
                    return;

                _userId = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserId"));
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName == value)
                    return;

                _firstName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FirstName"));
            }
        }


        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName == value)
                    return;

                _lastName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LastName"));
            }
        }


        public string Email
        {
            get { return _email; }
            set
            {
                if (_email == value)
                    return;

                _lastName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Email"));
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value)
                    return;

                _password = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Password"));
            }
        }

        public double UserBalance
        {
            get { return _userBalance; }
            set
            {
                if (_userBalance == value)
                    return;

                _userBalance = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserBalance"));
            }
        }

        public bool IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                if (_isAdmin == value)
                    return;

                _isAdmin = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAdmin"));
            }
        }

        public bool IsLogged
        {
            get { return _isLogged; }
            set
            {
                if (_isLogged == value)
                    return;

                _isLogged = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLogged"));
            }
        }

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
        #endregion

        #region Konstruktori
        public User() { }

        public User(int userId, 
                    string firstName, 
                    string lastName, 
                    string email, 
                    string password, 
                    double userBalance, 
                    bool isAdmin, 
                    bool isLogged,
                    bool isDeleted)
        {
            _userId = userId;
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            _password = password;
            _userBalance = userBalance;
            _isAdmin = isAdmin;
            _isLogged = isLogged;
            _isDeleted = isDeleted;
        }

        #endregion

        #region DbCommunication

        #region DataSetReader
        public static User GetUserFromReader(SqlDataReader reader)
        {
            User User = new User(
                    Convert.ToInt32(reader["UserId"]),
                    Convert.ToString(reader["FirstName"]),
                    Convert.ToString(reader["LastName"]),
                    Convert.ToString(reader["Email"]),
                    Convert.ToString(reader["Password"]),
                    Convert.ToDouble(reader["UserBalance"]),
                    Convert.ToBoolean(reader["IsAdmin"]),
                    Convert.ToBoolean(reader["IsLogged"]),
                    Convert.ToBoolean(reader["IsDeleted"])
                );

            return User;
        }
        #endregion

        public static List<User> GetAllUsers()
        {
            List<User> userList = new List<User>();
            User user = null;

            using (SqlConnection sqlConnection = new SqlConnection())
            {
                sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ToString();
                sqlConnection.Open();

                string sqlCommand = "SELECT UserId, FirstName, LastName, Email, Password, UserBalance, IsAdmin, IsLogged, IsDeleted FROM Users WHERE IsDeleted = 0";
                SqlCommand command = new SqlCommand(sqlCommand, sqlConnection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = new User(
                                Convert.ToInt32(reader["UserId"]),
                                Convert.ToString(reader["FirstName"]),
                                Convert.ToString(reader["LastName"]),
                                Convert.ToString(reader["Email"]),
                                Convert.ToString(reader["Password"]),
                                Convert.ToDouble(reader["UserBalance"]),
                                Convert.ToBoolean(reader["IsAdmin"]),
                                Convert.ToBoolean(reader["IsLogged"]),
                                Convert.ToBoolean(reader["IsDeleted"]));

                        userList.Add(user);
                    }
                }
            }
            return userList;
        }
        public void InsertUser()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ToString();
                conn.Open();

                string sqlCommand = "INSERT INTO Users(FirstName, LastName, Email, Password, UserBalance, IsAdmin, IsLogged, IsDeleted) VALUES(@FirstName, @LastName, @Email, @Password, @UserBalance, @IsAdmin, @IsLogged, @IsDeleted );SELECT IDENT_CURRENT('Users')";
                SqlCommand command = new SqlCommand(sqlCommand, conn);

                command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = this.FirstName;
                command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = this.LastName;
                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = this.Email;
                command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = this.Password;
                command.Parameters.Add("@UserBalance", SqlDbType.Money).Value = this.UserBalance;
                command.Parameters.Add("@IsAdmin", SqlDbType.Bit).Value = this.IsAdmin;
                command.Parameters.Add("@IsLogged", SqlDbType.Bit).Value = this.IsLogged;
                command.Parameters.Add("@IsDeleted", SqlDbType.Bit).Value = this.IsDeleted;

                var id = command.ExecuteNonQuery();

                if (id != null)
                    this.UserId = Convert.ToInt32(id);
            }
        }

        public void UpdateUser()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ToString();
                conn.Open();

                string sqlCommand = "UPDATE Users SET FirstName = @FirstName, LastName= @LastName, Email = @Email, Password = @Password, UserBalance = @UserBalance, IsAdmin = @IsAdmin, IsLogged = @IsLogged, IsDeleted = @IsDeleted WHERE UserId = @UserId";
                SqlCommand command = new SqlCommand(sqlCommand, conn);

                command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = this.FirstName;
                command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = this.LastName;
                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = this.Email;
                command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = this.Password;
                command.Parameters.Add("@UserBalance", SqlDbType.Money).Value = this.UserBalance;
                command.Parameters.Add("@IsAdmin", SqlDbType.Bit).Value = this.IsAdmin;
                command.Parameters.Add("@IsLogged", SqlDbType.Bit).Value = this.IsLogged;
                command.Parameters.Add("@IsDeleted", SqlDbType.Bit).Value = this.IsDeleted;

                int rows = command.ExecuteNonQuery();
            }
        }

        public void DeleteUser()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ToString();
                conn.Open();

                string sqlCommand = "UPDATE Users SET IsDeleted = 1 WHERE id = @Id";
                SqlCommand command = new SqlCommand(sqlCommand, conn);

                command.Parameters.Add("@UserId", SqlDbType.Int).Value = this.UserId;

                int rows = command.ExecuteNonQuery();
            }
        }
        #endregion

        public User Clone()
        {
            User clonedUser = new User();
            clonedUser.UserId = this.UserId;
            clonedUser.FirstName = this.FirstName;
            clonedUser.LastName = this.LastName;
            clonedUser.Email = this.Email;
            clonedUser.UserBalance = this.UserBalance;
            clonedUser.Password = this.Password;
            clonedUser.IsAdmin = this.IsAdmin;
            clonedUser.IsLogged = this.IsLogged;
            clonedUser.IsDeleted = this.IsDeleted;

            return clonedUser;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        // TODO - Override methods, GetHashCode, Equals
    }
}
