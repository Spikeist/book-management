using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Book_Management
{
    public partial class Form1 : Form
    {
        private string jsonFilePath = "data.json";
        private SortableBindingList<Customer> customers;
        private DataGridView customerGrid;
        private DataGridView addressGrid;

#nullable disable
        public Form1()
        {
            InitializeComponent();
            customerGrid = new DataGridView();
            addressGrid = new DataGridView();
            SetupForm();
            LoadCustomers();
        }
#nullable enable

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
            customerGrid.ReadOnly = false;
            customerGrid.AllowUserToDeleteRows = true;
            customerGrid.AllowUserToAddRows = false;
            customerGrid.AutoGenerateColumns = true;
            customerGrid.AllowUserToOrderColumns = true;
            customerGrid.SelectionChanged += CustomerGrid_SelectionChanged!;
            customerGrid.CellValueChanged += CustomerGrid_CellValueChanged!;
            customerGrid.UserDeletingRow += CustomerGrid_UserDeletingRow!;
            customerGrid.ColumnHeaderMouseClick += CustomerGrid_ColumnHeaderMouseClick!;
            customerGrid.CellValidating += CustomerGrid_CellValidating!;
            this.Controls.Add(customerGrid);

            // Set up the address grid
            addressGrid = new DataGridView();
            addressGrid.Location = new System.Drawing.Point(20, 240);
            addressGrid.Size = new System.Drawing.Size(600, 200);
            addressGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            addressGrid.MultiSelect = false;
            addressGrid.ReadOnly = false;
            addressGrid.AllowUserToDeleteRows = true;
            addressGrid.AutoGenerateColumns = true;
            addressGrid.AllowUserToOrderColumns = true;
            addressGrid.AllowUserToAddRows = false;
            addressGrid.CellValueChanged += AddressGrid_CellValueChanged!;
            addressGrid.UserDeletingRow += AddressGrid_UserDeletingRow!;
            addressGrid.ColumnHeaderMouseClick += AddressGrid_ColumnHeaderMouseClick!;
            addressGrid.RowValidating += AddressGrid_RowValidating!;
            this.Controls.Add(addressGrid);

            // Set up the buttons
            Button addCustomerButton = new Button();
            addCustomerButton.Text = "Add Customer";
            addCustomerButton.Location = new System.Drawing.Point(440, 60);
            addCustomerButton.Size = new System.Drawing.Size(100, 30);
            addCustomerButton.Click += AddCustomerButton_Click!;
            this.Controls.Add(addCustomerButton);

            Button deleteCustomerButton = new Button();
            deleteCustomerButton.Text = "Delete Customer";
            deleteCustomerButton.Location = new System.Drawing.Point(560, 60);
            deleteCustomerButton.Size = new System.Drawing.Size(100, 30);
            deleteCustomerButton.Click += DeleteCustomerButton_Click!;
            this.Controls.Add(deleteCustomerButton);

            Button addAddressButton = new Button();
            addAddressButton.Text = "Add Address";
            addAddressButton.Location = new System.Drawing.Point(440, 100);
            addAddressButton.Size = new System.Drawing.Size(100, 30);
            addAddressButton.Click += AddAddressButton_Click!;
            this.Controls.Add(addAddressButton);

            Button deleteAddressButton = new Button();
            deleteAddressButton.Text = "Delete Address";
            deleteAddressButton.Location = new System.Drawing.Point(560, 100);
            deleteAddressButton.Size = new System.Drawing.Size(100, 30);
            deleteAddressButton.Click += DeleteAddressButton_Click!;
            this.Controls.Add(deleteAddressButton);
        }

        private void LoadCustomers()
        {
            if (File.Exists(jsonFilePath))
            {
                try
                {
                    string json = File.ReadAllText(jsonFilePath);
                    customers = new SortableBindingList<Customer>(JsonConvert.DeserializeObject<List<Customer>>(json) ?? new List<Customer>());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading customers: {ex.Message}");
                    customers = new SortableBindingList<Customer>();
                }
            }
            else
            {
                customers = new SortableBindingList<Customer>();
            }

            customerGrid.DataSource = customers;
        }

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
                Addresses = new SortableBindingList<Address>()
            };

            customers.Add(newCustomer);
            SaveCustomers();
            LoadCustomers();
        }

#nullable disable
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
                int nextAddressTypeNumber = selectedCustomer.Addresses.Count + 1;
                var newAddress = new Address
                {
                    AddressId = selectedCustomer.Addresses.Any() ? selectedCustomer.Addresses.Max(a => a.AddressId) + 1 : 1,
                    AddressType = $"Address {nextAddressTypeNumber}",
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

        private void CustomerGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                Customer customer = (Customer)customerGrid.Rows[e.RowIndex].DataBoundItem;
                SaveCustomers();
            }
        }

#nullable disable
        private void CustomerGrid_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            Customer customer = (Customer)e.Row.DataBoundItem;
            customer.Addresses.Clear();
            customers.Remove(customer);
            SaveCustomers();
        }
#nullable enable

        private void CustomerGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn column = customerGrid.Columns[e.ColumnIndex];
            SortCustomers(column.Name);
        }

        private Dictionary<string, ListSortDirection> customerSortOrders = new Dictionary<string, ListSortDirection>();

        private void SortCustomers(string columnName)
        {
            if (!customerSortOrders.ContainsKey(columnName))
            {
                customerSortOrders[columnName] = ListSortDirection.Ascending;
            }
            else
            {
                customerSortOrders[columnName] = customerSortOrders[columnName] == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
            }

            customerGrid.Sort(customerGrid.Columns[columnName], customerSortOrders[columnName]);
        }

#nullable disable
        private void CustomerGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (customerGrid.Columns[e.ColumnIndex].Name == "CustomerNumber")
            {
                if (!int.TryParse(e.FormattedValue.ToString(), out _))
                {
                    e.Cancel = true;
                    MessageBox.Show("Customer number must be numeric.");
                }
            }
        }
#nullable enable

        private void AddressGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                Address address = (Address)addressGrid.Rows[e.RowIndex].DataBoundItem;
                SaveCustomers();
            }
        }

#nullable disable
        private void AddressGrid_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            Address address = (Address)e.Row.DataBoundItem;
            Customer customer = (Customer)customerGrid.SelectedRows[0].DataBoundItem;
            customer.Addresses.Remove(address);
            SaveCustomers();
        }
#nullable enable

        private void AddressGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn column = addressGrid.Columns[e.ColumnIndex];
            SortAddresses(column.Name);
        }

        private Dictionary<string, ListSortDirection> addressSortOrders = new Dictionary<string, ListSortDirection>();

        private void SortAddresses(string columnName)
        {
            if (!addressSortOrders.ContainsKey(columnName))
            {
                addressSortOrders[columnName] = ListSortDirection.Ascending;
            }
            else
            {
                addressSortOrders[columnName] = addressSortOrders[columnName] == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
            }

            addressGrid.Sort(addressGrid.Columns[columnName], addressSortOrders[columnName]);
        }

#nullable disable
        private void AddressGrid_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= 0 && !addressGrid.Rows[e.RowIndex].IsNewRow)
            {
                DataGridViewRow row = addressGrid.Rows[e.RowIndex];
                string addressType = row.Cells["AddressType"].Value?.ToString();

                if (!string.IsNullOrEmpty(addressType))
                {
                    var customer = (Customer)customerGrid.SelectedRows[0].DataBoundItem;
                    if (customer.Addresses.Count(a => a.AddressType == addressType) > 1)
                    {
                        e.Cancel = true;
                        MessageBox.Show($"A customer can only have one address of type '{addressType}'.");
                    }
                }
            }
        }
#nullable enable

        private void AddCustomerButton_Click(object sender, EventArgs e)
        {
            AddCustomer();
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

    public class CustomerData
    {
        public List<Customer>? Customers { get; set; }
    }

    public class Customer
    {
        public int CustomerNumber { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public SortableBindingList<Address> Addresses { get; set; } = new SortableBindingList<Address>();
    }

    public class Address
    {
        public int AddressId { get; set; }
        public string AddressType { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZIP { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }

    public class SortableBindingList<T> : BindingList<T>
    {
        public SortableBindingList()
        {
        }

        public SortableBindingList(IList<T> list)
        {
            foreach (var item in list)
            {
                Add(item);
            }
        }

        protected override bool SupportsSortingCore => true;

        protected override bool IsSortedCore => true;

#nullable disable
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            if (prop.PropertyType.GetInterface("IComparable") != null)
            {
                List<T> itemsList = Items as List<T>;
                itemsList.Sort((x, y) =>
                {
                    object xValue = prop.GetValue(x);
                    object yValue = prop.GetValue(y);

                    int result = ((IComparable)xValue).CompareTo(yValue);

                    if (direction == ListSortDirection.Descending)
                        result = -result;

                    return result;
                });
            }
            else
            {
                throw new NotSupportedException($"Cannot sort by {prop.Name}. The property type does not implement IComparable.");
            }
        }
#nullable enable
    }
}


// [
//   {
//     "customerNumber": 1,
//     "firstName": "John",
//     "lastName": "Doe",
//     "addresses": [
//       {
//         "addressId": 1,
//         "addressType": "Home",
//         "addressLine1": "123 Main St",
//         "addressLine2": "",
//         "city": "New York",
//         "state": "NY",
//         "zip": "10001",
//         "country": "USA"
//       }
//     ]
//   },
//   {
//     "customerNumber": 2,
//     "firstName": "Jill",
//     "lastName": "Doe",
//     "addresses": [
//       {
//         "addressId": 1,
//         "addressType": "Home",
//         "addressLine1": "123 Main St",
//         "addressLine2": "",
//         "city": "New York",
//         "state": "NY",
//         "zip": "10001",
//         "country": "USA"
//       }
//     ]
//   }
// ]