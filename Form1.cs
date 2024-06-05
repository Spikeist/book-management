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

        private void SetupForm()
        {
            // Set up form
            this.Text = "Address Book";
            this.Size = new System.Drawing.Size(910, 500);

            // Set up customer grid
            customerGrid = new DataGridView();
            customerGrid.Location = new System.Drawing.Point(20, 20);
            customerGrid.Size = new System.Drawing.Size(400, 200);
            customerGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            customerGrid.MultiSelect = false;
            customerGrid.ReadOnly = false;
            customerGrid.AllowUserToDeleteRows = true;
            customerGrid.AllowUserToAddRows = false;
            customerGrid.AutoGenerateColumns = true;
            customerGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            customerGrid.AllowUserToOrderColumns = true;
            customerGrid.SelectionChanged += CustomerGrid_SelectionChanged;
            customerGrid.CellValueChanged += CustomerGrid_CellValueChanged;
            customerGrid.UserDeletingRow += CustomerGrid_UserDeletingRow;
            customerGrid.ColumnHeaderMouseClick += CustomerGrid_ColumnHeaderMouseClick;
            customerGrid.CellValidating += CustomerGrid_CellValidating;
            customerGrid.DataBindingComplete += CustomerGrid_DataBindingComplete;
            this.Controls.Add(customerGrid);

            // Set up address grid
            addressGrid = new DataGridView();
            addressGrid.Location = new System.Drawing.Point(20, 240);
            addressGrid.Size = new System.Drawing.Size(850, 200);
            addressGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            addressGrid.MultiSelect = false;
            addressGrid.ReadOnly = false;
            addressGrid.AllowUserToDeleteRows = true;
            addressGrid.AllowUserToOrderColumns = true;
            addressGrid.AllowUserToAddRows = false;
            addressGrid.AutoGenerateColumns = false;
            addressGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            DataGridViewTextBoxColumn addressIdColumn = new DataGridViewTextBoxColumn();
            addressIdColumn.DataPropertyName = "AddressId";
            addressIdColumn.HeaderText = "AddressId";
            addressIdColumn.ReadOnly = true;
            addressGrid.Columns.Add(addressIdColumn);

            DataGridViewComboBoxColumn addressTypeColumn = new DataGridViewComboBoxColumn();
            addressTypeColumn.DataPropertyName = "AddressType";
            addressTypeColumn.HeaderText = "AddressType";
            addressTypeColumn.Name = "AddressType";
            addressTypeColumn.Items.AddRange(new string[] { "Home", "Business", "Billing", "Shipping" });
            addressGrid.Columns.Add(addressTypeColumn);

            DataGridViewTextBoxColumn addressLine1Column = new DataGridViewTextBoxColumn();
            addressLine1Column.DataPropertyName = "AddressLine1";
            addressLine1Column.HeaderText = "AddressLine1";
            addressGrid.Columns.Add(addressLine1Column);

            DataGridViewTextBoxColumn addressLine2Column = new DataGridViewTextBoxColumn();
            addressLine2Column.DataPropertyName = "AddressLine2";
            addressLine2Column.HeaderText = "AddressLine2";
            addressGrid.Columns.Add(addressLine2Column);

            DataGridViewTextBoxColumn cityColumn = new DataGridViewTextBoxColumn();
            cityColumn.DataPropertyName = "City";
            cityColumn.HeaderText = "City";
            addressGrid.Columns.Add(cityColumn);

            DataGridViewTextBoxColumn stateColumn = new DataGridViewTextBoxColumn();
            stateColumn.DataPropertyName = "State";
            stateColumn.HeaderText = "State";
            addressGrid.Columns.Add(stateColumn);

            DataGridViewTextBoxColumn zipColumn = new DataGridViewTextBoxColumn();
            zipColumn.DataPropertyName = "ZIP";
            zipColumn.HeaderText = "ZIP";
            addressGrid.Columns.Add(zipColumn);

            DataGridViewTextBoxColumn countryColumn = new DataGridViewTextBoxColumn();
            countryColumn.DataPropertyName = "Country";
            countryColumn.HeaderText = "Country";
            addressGrid.Columns.Add(countryColumn);

            addressGrid.CellValueChanged += AddressGrid_CellValueChanged;
            addressGrid.UserDeletingRow += AddressGrid_UserDeletingRow;
            addressGrid.ColumnHeaderMouseClick += AddressGrid_ColumnHeaderMouseClick;
            addressGrid.RowValidating += AddressGrid_RowValidating;
            addressGrid.CellBeginEdit += AddressGrid_CellBeginEdit;
            addressGrid.CellEndEdit += AddressGrid_CellEndEdit;
            addressGrid.CurrentCellDirtyStateChanged += AddressGrid_CurrentCellDirtyStateChanged;

            this.Controls.Add(addressGrid);

            // Set up buttons
            Button addCustomerButton = new Button();
            addCustomerButton.Text = "Add Customer";
            addCustomerButton.Location = new System.Drawing.Point(440, 60);
            addCustomerButton.Size = new System.Drawing.Size(100, 30);
            addCustomerButton.Click += AddCustomerButton_Click;
            this.Controls.Add(addCustomerButton);

            Button deleteCustomerButton = new Button();
            deleteCustomerButton.Text = "Delete Customer";
            deleteCustomerButton.Location = new System.Drawing.Point(560, 60);
            deleteCustomerButton.Size = new System.Drawing.Size(120, 30);
            deleteCustomerButton.Click += DeleteCustomerButton_Click;
            this.Controls.Add(deleteCustomerButton);

            Button addAddressButton = new Button();
            addAddressButton.Text = "Add Address";
            addAddressButton.Location = new System.Drawing.Point(440, 100);
            addAddressButton.Size = new System.Drawing.Size(100, 30);
            addAddressButton.Click += AddAddressButton_Click;
            this.Controls.Add(addAddressButton);

            Button deleteAddressButton = new Button();
            deleteAddressButton.Text = "Delete Address";
            deleteAddressButton.Location = new System.Drawing.Point(560, 100);
            deleteAddressButton.Size = new System.Drawing.Size(120, 30);
            deleteAddressButton.Click += DeleteAddressButton_Click;
            this.Controls.Add(deleteAddressButton);
        }

        private void AddressGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (addressGrid.IsCurrentCellDirty)
            {
                addressGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void AddressGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            SaveCustomers();
        }
#nullable enable

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
                addressGrid.DataSource = null;

                DataGridViewComboBoxColumn addressTypeColumn = (DataGridViewComboBoxColumn)addressGrid.Columns["AddressType"];
                List<string> availableAddressTypes = new List<string> { "Home", "Business", "Billing", "Shipping" };

                addressTypeColumn.DataSource = availableAddressTypes;

                foreach (DataGridViewRow row in addressGrid.Rows)
                {
                    Address address = (Address)row.DataBoundItem;
                    if (address != null && !availableAddressTypes.Contains(address.AddressType))
                    {
                        availableAddressTypes.Add(address.AddressType);
                    }
                }

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

                if (customers.Count == 0)
                {
                }
            }
        }
#nullable enable

        private void AddCustomer()
        {
            AddCustomerForm addCustomerForm = new AddCustomerForm();
            if (addCustomerForm.ShowDialog() == DialogResult.OK)
            {
                var newCustomer = new Customer
                {
                    CustomerNumber = customers.Any() ? customers.Max(c => c.CustomerNumber) + 1 : 1,
                    FirstName = addCustomerForm.FirstName,
                    LastName = addCustomerForm.LastName,
                    Addresses = new SortableBindingList<Address>()
                };

                customers.Add(newCustomer);
                SaveCustomers();
                LoadCustomers();
            }
        }

#nullable disable
        private void DeleteAddress(int customerNumber, Address address)
        {
            Customer selectedCustomer = customers.FirstOrDefault(c => c.CustomerNumber == customerNumber);
            if (selectedCustomer != null)
            {
                selectedCustomer.Addresses.Remove(address);
                SaveCustomers();
                addressGrid.DataSource = null;
                LoadAddresses(customerNumber);
            }
        }

        private void AddAddress(int customerNumber)
        {
            Customer selectedCustomer = customers.FirstOrDefault(c => c.CustomerNumber == customerNumber);
            if (selectedCustomer != null)
            {
                if (selectedCustomer.Addresses.Count >= 4)
                {
                    MessageBox.Show("A customer can have a maximum of 4 addresses.");
                    return;
                }

                AddAddressForm addAddressForm = new AddAddressForm();
                List<string> existingAddressTypes = selectedCustomer.Addresses.Select(a => a.AddressType).ToList();
                addAddressForm.UpdateAddressTypeComboBox(existingAddressTypes);

                while (true)
                {
                    if (addAddressForm.ShowDialog() == DialogResult.OK)
                    {
                        string selectedAddressType = addAddressForm.AddressType;
                        if (string.IsNullOrEmpty(selectedAddressType))
                        {
                            MessageBox.Show("Please select a valid address type.");
                            continue;
                        }

                        var newAddress = new Address
                        {
                            AddressId = selectedCustomer.Addresses.Any() ? selectedCustomer.Addresses.Max(a => a.AddressId) + 1 : 1,
                            AddressType = selectedAddressType,
                            AddressLine1 = addAddressForm.AddressLine1,
                            AddressLine2 = addAddressForm.AddressLine2,
                            City = addAddressForm.City,
                            State = addAddressForm.State,
                            ZIP = addAddressForm.ZIP,
                            Country = addAddressForm.Country
                        };

                        selectedCustomer.Addresses.Add(newAddress);
                        SaveCustomers();
                        addressGrid.DataSource = null;
                        LoadAddresses(customerNumber);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
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

        private void CustomerGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (customerGrid.Columns.Contains("CustomerNumber"))
            {
                customerGrid.Columns["CustomerNumber"].ReadOnly = true;
            }
        }

#nullable disable
        private void AddressGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                Address address = (Address)addressGrid.Rows[e.RowIndex].DataBoundItem;
                SaveCustomers();

                if (customerGrid.SelectedRows.Count > 0)
                {
                    int customerNumber = Convert.ToInt32(customerGrid.SelectedRows[0].Cells["CustomerNumber"].Value);
                    LoadAddresses(customerNumber);
                }
            }
        }

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

#nullable disable
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

            DataGridViewColumn column = addressGrid.Columns[columnName];
            ListSortDirection sortDirection = addressSortOrders[columnName];
        }

        private void AddressGrid_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (addressGrid.IsCurrentRowDirty && e.RowIndex >= 0 && !addressGrid.Rows[e.RowIndex].IsNewRow)
            {
                DataGridViewRow row = addressGrid.Rows[e.RowIndex];
                DataGridViewCell addressTypeCell = row.Cells["AddressType"];
                if (addressTypeCell.Value != null)
                {
                    string addressType = addressTypeCell.Value.ToString();
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

        private void AddressGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == addressGrid.Columns["AddressType"].Index)
            {
                Customer customer = (Customer)customerGrid.SelectedRows[0].DataBoundItem;
                DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)addressGrid.Rows[e.RowIndex].Cells["AddressType"];
                Address currentAddress = (Address)addressGrid.Rows[e.RowIndex].DataBoundItem;

                List<string> availableAddressTypes = new List<string> { "Home", "Business", "Billing", "Shipping" };
                List<string> takenAddressTypes = customer.Addresses
                    .Where(a => a != currentAddress)
                    .Select(a => a.AddressType)
                    .ToList();

                string currentAddressType = currentAddress.AddressType;
                if (!availableAddressTypes.Contains(currentAddressType))
                {
                    availableAddressTypes.Add(currentAddressType);
                }

                comboBoxCell.DataSource = availableAddressTypes.Except(takenAddressTypes).ToList();

                if (!comboBoxCell.Items.Contains(currentAddressType))
                {
                    comboBoxCell.Items.Add(currentAddressType);
                }

                comboBoxCell.Value = currentAddressType;
            }
        }

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
                Address selectedAddress = (Address)addressGrid.SelectedRows[0].DataBoundItem;
                DeleteAddress(customerNumber, selectedAddress);
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
//     "CustomerNumber": 1,
//     "FirstName": "John",
//     "LastName": "Doe",
//     "Addresses": [
//       {
//         "AddressId": 1,
//         "AddressType": "Home",
//         "AddressLine1": "123 Main St",
//         "AddressLine2": "",
//         "City": "New York",
//         "State": "NY",
//         "ZIP": "10001",
//         "Country": "USA"
//       }
//     ]
//   },
//   {
//     "CustomerNumber": 2,
//     "FirstName": "Jill",
//     "LastName": "Doe",
//     "Addresses": [
//       {
//         "AddressId": 1,
//         "AddressType": "Home",
//         "AddressLine1": "124 Main St",
//         "AddressLine2": "",
//         "City": "New York",
//         "State": "NY",
//         "ZIP": "10001",
//         "Country": "USA"
//       }
//     ]
//   }
// ]