using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Book_Management
{
    public partial class Form1 : Form
    {
        private string connectionString = "Server=localhost;Database=address_book;User Id=root;Password=Password1!;Port=3306";
        private DataGridView customerGrid;
        private DataGridView addressGrid;
        private ComboBox sortOrderComboBox;
        private ComboBox updateMethodComboBox;

        public Form1()
        {
            InitializeComponent();
            SetupForm();
            LoadCustomers();
        }

        private void SetupForm()
        {
            // Set up the form
            this.Text = "Address Book";
            this.Size = new System.Drawing.Size(800, 600);

            // Set up the customer grid
            customerGrid = new DataGridView();
            customerGrid.Location = new System.Drawing.Point(20, 20);
            customerGrid.Size = new System.Drawing.Size(400, 200);
            customerGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            customerGrid.MultiSelect = false;
            customerGrid.ReadOnly = true;
            customerGrid.ColumnCount = 3;
            customerGrid.Columns[0].Name = "CustomerNumber";
            customerGrid.Columns[1].Name = "FirstName";
            customerGrid.Columns[2].Name = "LastName";
            customerGrid.SelectionChanged += CustomerGrid_SelectionChanged;
            this.Controls.Add(customerGrid);

            // Set up the address grid
            addressGrid = new DataGridView();
            addressGrid.Location = new System.Drawing.Point(20, 240);
            addressGrid.Size = new System.Drawing.Size(600, 200);
            addressGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            addressGrid.MultiSelect = false;
            addressGrid.ReadOnly = true;
            addressGrid.ColumnCount = 8;
            addressGrid.Columns[0].Name = "AddressId";
            addressGrid.Columns[1].Name = "AddressType";
            addressGrid.Columns[2].Name = "AddressLine1";
            addressGrid.Columns[3].Name = "AddressLine2";
            addressGrid.Columns[4].Name = "City";
            addressGrid.Columns[5].Name = "State";
            addressGrid.Columns[6].Name = "ZIP";
            addressGrid.Columns[7].Name = "Country";
            this.Controls.Add(addressGrid);

            // Set up the sort order combo box
            sortOrderComboBox = new ComboBox();
            sortOrderComboBox.Location = new System.Drawing.Point(440, 20);
            sortOrderComboBox.Size = new System.Drawing.Size(200, 20);
            sortOrderComboBox.Items.Add("CustomerNumber");
            sortOrderComboBox.Items.Add("FirstName");
            sortOrderComboBox.Items.Add("LastName");
            sortOrderComboBox.SelectedIndex = 0;
            sortOrderComboBox.SelectedIndexChanged += SortOrderComboBox_SelectedIndexChanged;
            this.Controls.Add(sortOrderComboBox);

            // Set up the update method combo box
            updateMethodComboBox = new ComboBox();
            updateMethodComboBox.Location = new System.Drawing.Point(440, 60);
            updateMethodComboBox.Size = new System.Drawing.Size(200, 20);
            updateMethodComboBox.Items.Add("Dynamic SQL");
            updateMethodComboBox.Items.Add("Stored Procedures");
            updateMethodComboBox.Items.Add("REST API");
            updateMethodComboBox.SelectedIndex = 0;
            this.Controls.Add(updateMethodComboBox);

            // Set up the buttons
            Button addCustomerButton = new Button();
            addCustomerButton.Text = "Add Customer";
            addCustomerButton.Location = new System.Drawing.Point(440, 100);
            addCustomerButton.Size = new System.Drawing.Size(100, 30);
            addCustomerButton.Click += AddCustomerButton_Click;
            this.Controls.Add(addCustomerButton);

            Button updateCustomerButton = new Button();
            updateCustomerButton.Text = "Update Customer";
            updateCustomerButton.Location = new System.Drawing.Point(560, 100);
            updateCustomerButton.Size = new System.Drawing.Size(100, 30);
            updateCustomerButton.Click += UpdateCustomerButton_Click;
            this.Controls.Add(updateCustomerButton);

            Button deleteCustomerButton = new Button();
            deleteCustomerButton.Text = "Delete Customer";
            deleteCustomerButton.Location = new System.Drawing.Point(680, 100);
            deleteCustomerButton.Size = new System.Drawing.Size(100, 30);
            deleteCustomerButton.Click += DeleteCustomerButton_Click;
            this.Controls.Add(deleteCustomerButton);

            Button addAddressButton = new Button();
            addAddressButton.Text = "Add Address";
            addAddressButton.Location = new System.Drawing.Point(440, 140);
            addAddressButton.Size = new System.Drawing.Size(100, 30);
            addAddressButton.Click += AddAddressButton_Click;
            this.Controls.Add(addAddressButton);

            Button updateAddressButton = new Button();
            updateAddressButton.Text = "Update Address";
            updateAddressButton.Location = new System.Drawing.Point(560, 140);
            updateAddressButton.Size = new System.Drawing.Size(100, 30);
            updateAddressButton.Click += UpdateAddressButton_Click;
            this.Controls.Add(updateAddressButton);

            Button deleteAddressButton = new Button();
            deleteAddressButton.Text = "Delete Address";
            deleteAddressButton.Location = new System.Drawing.Point(680, 140);
            deleteAddressButton.Size = new System.Drawing.Size(100, 30);
            deleteAddressButton.Click += DeleteAddressButton_Click;
            this.Controls.Add(deleteAddressButton);
        }

        private void LoadCustomers()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT CustomerNumber, FirstName, LastName FROM Customer";
                    query += " ORDER BY " + sortOrderComboBox.SelectedItem.ToString();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            customerGrid.Rows.Clear();
                            while (reader.Read())
                            {
                                customerGrid.Rows.Add(reader["CustomerNumber"], reader["FirstName"], reader["LastName"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LoadAddresses(int customerNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT a.AddressId, at.Description AS AddressType, a.AddressLine1, a.AddressLine2, a.City, a.State, a.ZIP, a.Country";
                    query += " FROM Address a";
                    query += " JOIN AddressType at ON a.AddressTypeId = at.AddressTypeId";
                    query += " WHERE a.CustomerNumber = @CustomerNumber";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerNumber", customerNumber);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            addressGrid.Rows.Clear();
                            while (reader.Read())
                            {
                                addressGrid.Rows.Add(reader["AddressId"], reader["AddressType"], reader["AddressLine1"], reader["AddressLine2"],
                                    reader["City"], reader["State"], reader["ZIP"], reader["Country"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void UpdateCustomer(int customerNumber)
        {
            try
            {
                string updateMethod = updateMethodComboBox.SelectedItem.ToString();

                switch (updateMethod)
                {
                    case "Dynamic SQL":
                        // Build and execute the SQL update statement
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            string query = "UPDATE Customer SET FirstName = @FirstName, LastName = @LastName";
                            query += " WHERE CustomerNumber = @CustomerNumber";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@FirstName", "Updated First Name");
                                command.Parameters.AddWithValue("@LastName", "Updated Last Name");
                                command.Parameters.AddWithValue("@CustomerNumber", customerNumber);

                                command.ExecuteNonQuery();
                            }
                        }
                        break;
                    case "Stored Procedures":
                        // Call the stored procedure to update the customer
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            using (SqlCommand command = new SqlCommand("UpdateCustomer", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@p_CustomerNumber", customerNumber);
                                command.Parameters.AddWithValue("@p_FirstName", "Updated First Name");
                                command.Parameters.AddWithValue("@p_LastName", "Updated Last Name");

                                command.ExecuteNonQuery();
                            }
                        }
                        break;
                    case "REST API":
                        // Placeholder for REST API implementation
                        MessageBox.Show("REST API implementation is not available.");
                        break;
                    default:
                        throw new InvalidOperationException("Invalid update method selected.");
                }

                // Refresh the customer grid
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void DeleteCustomer(int customerNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM Customer WHERE CustomerNumber = @CustomerNumber";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerNumber", customerNumber);

                        command.ExecuteNonQuery();
                    }
                }

                // Refresh the customer grid
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void AddCustomer()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Customer (FirstName, LastName) VALUES (@FirstName, @LastName)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", "New First Name");
                        command.Parameters.AddWithValue("@LastName", "New Last Name");

                        command.ExecuteNonQuery();
                    }
                }

                // Refresh the customer grid
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void UpdateAddress(int customerNumber, int addressId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE Address SET AddressLine1 = @AddressLine1, AddressLine2 = @AddressLine2, City = @City, State = @State, ZIP = @ZIP, Country = @Country";
                    query += " WHERE AddressId = @AddressId AND CustomerNumber = @CustomerNumber";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AddressLine1", "Updated Address Line 1");
                        command.Parameters.AddWithValue("@AddressLine2", "Updated Address Line 2");
                        command.Parameters.AddWithValue("@City", "Updated City");
                        command.Parameters.AddWithValue("@State", "Updated State");
                        command.Parameters.AddWithValue("@ZIP", "Updated ZIP");
                        command.Parameters.AddWithValue("@Country", "Updated Country");
                        command.Parameters.AddWithValue("@AddressId", addressId);
                        command.Parameters.AddWithValue("@CustomerNumber", customerNumber);

                        command.ExecuteNonQuery();
                    }
                }

                // Refresh the address grid
                LoadAddresses(customerNumber);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void DeleteAddress(int addressId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM Address WHERE AddressId = @AddressId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AddressId", addressId);

                        command.ExecuteNonQuery();
                    }
                }

                // Refresh the address grid
                int customerNumber = Convert.ToInt32(customerGrid.SelectedRows[0].Cells["CustomerNumber"].Value);
                LoadAddresses(customerNumber);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void AddAddress(int customerNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Address (CustomerNumber, AddressTypeId, AddressLine1, AddressLine2, City, State, ZIP, Country)";
                    query += " VALUES (@CustomerNumber, @AddressTypeId, @AddressLine1, @AddressLine2, @City, @State, @ZIP, @Country)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerNumber", customerNumber);
                        command.Parameters.AddWithValue("@AddressTypeId", 1);
                        command.Parameters.AddWithValue("@AddressLine1", "New Address Line 1");
                        command.Parameters.AddWithValue("@AddressLine2", "New Address Line 2");
                        command.Parameters.AddWithValue("@City", "New City");
                        command.Parameters.AddWithValue("@State", "New State");
                        command.Parameters.AddWithValue("@ZIP", "New ZIP");
                        command.Parameters.AddWithValue("@Country", "New Country");

                        command.ExecuteNonQuery();
                    }
                }

                // Refresh the address grid
                LoadAddresses(customerNumber);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void CustomerGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (customerGrid.SelectedRows.Count > 0)
            {
                int customerNumber = Convert.ToInt32(customerGrid.SelectedRows[0].Cells["CustomerNumber"].Value);
                LoadAddresses(customerNumber);
            }
        }

        private void SortOrderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void AddCustomerButton_Click(object sender, EventArgs e)
        {
            AddCustomer();
        }

        private void UpdateCustomerButton_Click(object sender, EventArgs e)
        {
            if (customerGrid.SelectedRows.Count > 0)
            {
                int customerNumber = Convert.ToInt32(customerGrid.SelectedRows[0].Cells["CustomerNumber"].Value);
                UpdateCustomer(customerNumber);
            }
            else
            {
                MessageBox.Show("Please select a customer to update.");
            }
        }

        private void DeleteCustomerButton_Click(object sender, EventArgs e)
        {
            if (customerGrid.SelectedRows.Count > 0)
            {
                int customerNumber = Convert.ToInt32(customerGrid.SelectedRows[0].Cells["CustomerNumber"].Value);
                DeleteCustomer(customerNumber);
            }
            else
            {
                MessageBox.Show("Please select a customer to delete.");
            }
        }

        private void AddAddressButton_Click(object sender, EventArgs e)
        {
            if (customerGrid.SelectedRows.Count > 0)
            {
                int customerNumber = Convert.ToInt32(customerGrid.SelectedRows[0].Cells["CustomerNumber"].Value);
                AddAddress(customerNumber);
            }
            else
            {
                MessageBox.Show("Please select a customer to add an address for.");
            }
        }

        private void UpdateAddressButton_Click(object sender, EventArgs e)
        {
            if (customerGrid.SelectedRows.Count > 0 && addressGrid.SelectedRows.Count > 0)
            {
                int customerNumber = Convert.ToInt32(customerGrid.SelectedRows[0].Cells["CustomerNumber"].Value);
                int addressId = Convert.ToInt32(addressGrid.SelectedRows[0].Cells["AddressId"].Value);
                UpdateAddress(customerNumber, addressId);
            }
            else
            {
                MessageBox.Show("Please select a customer and an address to update.");
            }
        }

        private void DeleteAddressButton_Click(object sender, EventArgs e)
        {
            if (addressGrid.SelectedRows.Count > 0)
            {
                int addressId = Convert.ToInt32(addressGrid.SelectedRows[0].Cells["AddressId"].Value);
                DeleteAddress(addressId);
            }
            else
            {
                MessageBox.Show("Please select an address to delete.");
            }
        }
    }
}