using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Book_Management
{
    public partial class Form1 : Form
    {
        private string jsonFilePath = "data.json";
        private List<Customer> customers;
        private DataGridView customerGrid;
        private DataGridView addressGrid;
        private ComboBox sortOrderComboBox;
        
        #nullable disable
        public Form1()
        {
            InitializeComponent();
            SetupForm();
            LoadCustomers();
        }
        #nullable enable

        private void SetupForm()
        {
            // Set up the form
            this.Text = "Address Book";
            this.Size = new System.Drawing.Size(800, 600);

            #nullable disable
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
            #nullable enable

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

            #nullable disable
            // Set up the buttons
            Button addCustomerButton = new Button();
            addCustomerButton.Text = "Add Customer";
            addCustomerButton.Location = new System.Drawing.Point(440, 60);
            addCustomerButton.Size = new System.Drawing.Size(100, 30);
            addCustomerButton.Click += AddCustomerButton_Click;
            this.Controls.Add(addCustomerButton);

            Button updateCustomerButton = new Button();
            updateCustomerButton.Text = "Update Customer";
            updateCustomerButton.Location = new System.Drawing.Point(560, 60);
            updateCustomerButton.Size = new System.Drawing.Size(100, 30);
            updateCustomerButton.Click += UpdateCustomerButton_Click;
            this.Controls.Add(updateCustomerButton);

            Button deleteCustomerButton = new Button();
            deleteCustomerButton.Text = "Delete Customer";
            deleteCustomerButton.Location = new System.Drawing.Point(680, 60);
            deleteCustomerButton.Size = new System.Drawing.Size(100, 30);
            deleteCustomerButton.Click += DeleteCustomerButton_Click;
            this.Controls.Add(deleteCustomerButton);

            Button addAddressButton = new Button();
            addAddressButton.Text = "Add Address";
            addAddressButton.Location = new System.Drawing.Point(440, 100);
            addAddressButton.Size = new System.Drawing.Size(100, 30);
            addAddressButton.Click += AddAddressButton_Click;
            this.Controls.Add(addAddressButton);

            Button updateAddressButton = new Button();
            updateAddressButton.Text = "Update Address";
            updateAddressButton.Location = new System.Drawing.Point(560, 100);
            updateAddressButton.Size = new System.Drawing.Size(100, 30);
            updateAddressButton.Click += UpdateAddressButton_Click;
            this.Controls.Add(updateAddressButton);

            Button deleteAddressButton = new Button();
            deleteAddressButton.Text = "Delete Address";
            deleteAddressButton.Location = new System.Drawing.Point(680, 100);
            deleteAddressButton.Size = new System.Drawing.Size(100, 30);
            deleteAddressButton.Click += DeleteAddressButton_Click;
            this.Controls.Add(deleteAddressButton);
            #nullable enable
        }

        #nullable disable
        private void LoadCustomers()
        {
            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                customers = JsonConvert.DeserializeObject<List<Customer>>(json);
            }
            else
            {
                customers = new List<Customer>();
            }

            customerGrid.DataSource = customers;
        }
        #nullable enable

        private void SaveCustomers()
        {
            string json = JsonConvert.SerializeObject(customers, Formatting.Indented);
            File.WriteAllText(jsonFilePath, json);
        }

        #nullable disable
        private void LoadAddresses(int customerNumber)
        {
            Customer selectedCustomer = customers.FirstOrDefault(c => c.CustomerNumber == customerNumber);
            if (selectedCustomer != null)
            {
                addressGrid.DataSource = selectedCustomer.Addresses;
            }
            else
            {
                addressGrid.DataSource = null;
            }
        }
        
        private void UpdateCustomer(int customerNumber)
        {
            Customer selectedCustomer = customers.FirstOrDefault(c => c.CustomerNumber == customerNumber);
            if (selectedCustomer != null)
            {
                // Update the customer properties
                selectedCustomer.FirstName = "Updated First Name";
                selectedCustomer.LastName = "Updated Last Name";

                SaveCustomers();
                LoadCustomers();
            }
        }
        
        private void DeleteCustomer(int customerNumber)
        {
            Customer selectedCustomer = customers.FirstOrDefault(c => c.CustomerNumber == customerNumber);
            if (selectedCustomer != null)
            {
                customers.Remove(selectedCustomer);
                SaveCustomers();
                LoadCustomers();
            }
        }
        #nullable enable

        private void AddCustomer()
        {
            var newCustomer = new Customer
            {
                CustomerNumber = customers.Any() ? customers.Max(c => c.CustomerNumber) + 1 : 1,
                FirstName = "New",
                LastName = "Customer",
                Addresses = new List<Address>()
            };

            customers.Add(newCustomer);
            SaveCustomers();
            LoadCustomers();
        }

        #nullable disable
        private void UpdateAddress(int customerNumber, int addressId)
        {
            Customer selectedCustomer = customers.FirstOrDefault(c => c.CustomerNumber == customerNumber);
            if (selectedCustomer != null)
            {
                Address selectedAddress = selectedCustomer.Addresses.FirstOrDefault(a => a.AddressId == addressId);
                if (selectedAddress != null)
                {
                    // Update the address properties
                    selectedAddress.AddressLine1 = "Updated Address Line 1";
                    selectedAddress.AddressLine2 = "Updated Address Line 2";
                    selectedAddress.City = "Updated City";
                    selectedAddress.State = "Updated State";
                    selectedAddress.ZIP = "Updated ZIP";
                    selectedAddress.Country = "Updated Country";

                    SaveCustomers();
                    LoadAddresses(customerNumber);
                }
            }
        }

        private void DeleteAddress(int customerNumber, int addressId)
        {
            Customer selectedCustomer = customers.FirstOrDefault(c => c.CustomerNumber == customerNumber);
            if (selectedCustomer != null)
            {
                Address selectedAddress = selectedCustomer.Addresses.FirstOrDefault(a => a.AddressId == addressId);
                if (selectedAddress != null)
                {
                    selectedCustomer.Addresses.Remove(selectedAddress);
                    SaveCustomers();
                    LoadAddresses(customerNumber);
                }
            }
        }
        

        private void AddAddress(int customerNumber)
        {
            Customer selectedCustomer = customers.FirstOrDefault(c => c.CustomerNumber == customerNumber);
            if (selectedCustomer != null)
            {
                var newAddress = new Address
                {
                    AddressId = selectedCustomer.Addresses.Any() ? selectedCustomer.Addresses.Max(a => a.AddressId) + 1 : 1,
                    AddressType = "Home",
                    AddressLine1 = "New Address Line 1",
                    AddressLine2 = "New Address Line 2",
                    City = "New City",
                    State = "New State",
                    ZIP = "New ZIP",
                    Country = "New Country"
                };

                selectedCustomer.Addresses.Add(newAddress);
                SaveCustomers();
                LoadAddresses(customerNumber);
            }
        }
        #nullable enable

        private void CustomerGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (customerGrid.SelectedRows.Count > 0)
            {
                int customerNumber = Convert.ToInt32(customerGrid.SelectedRows[0].Cells["CustomerNumber"].Value);
                LoadAddresses(customerNumber);
            }
        }

        #nullable disable
        private void SortOrderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sortOrder = sortOrderComboBox.SelectedItem.ToString();
            switch (sortOrder)
            {
                case "CustomerNumber":
                    customers = customers.OrderBy(c => c.CustomerNumber).ToList();
                    break;
                case "FirstName":
                    customers = customers.OrderBy(c => c.FirstName).ToList();
                    break;
                case "LastName":
                    customers = customers.OrderBy(c => c.LastName).ToList();
                    break;
            }
            customerGrid.DataSource = null;
            customerGrid.DataSource = customers;
        }
        #nullable enable

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
            if (customerGrid.SelectedRows.Count > 0 && addressGrid.SelectedRows.Count > 0)
            {
                int customerNumber = Convert.ToInt32(customerGrid.SelectedRows[0].Cells["CustomerNumber"].Value);
                int addressId = Convert.ToInt32(addressGrid.SelectedRows[0].Cells["AddressId"].Value);
                DeleteAddress(customerNumber, addressId);
            }
            else
            {
                MessageBox.Show("Please select a customer and an address to delete.");
            }
        }
    }

    public class Customer
    {
        public int CustomerNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>();
    }

    public class Address
    {
        public int AddressId { get; set; }
        public string? AddressType { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZIP { get; set; }
        public string? Country { get; set; }
    }
}