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
            customerGrid.AllowUserToDeleteRows = false;
            customerGrid.AllowUserToAddRows = false;
            customerGrid.AutoGenerateColumns = true;
            customerGrid.ReadOnly = true;
            customerGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            customerGrid.AllowUserToOrderColumns = true;
            customerGrid.SelectionChanged += CustomerGrid_SelectionChanged;
            customerGrid.UserDeletingRow += CustomerGrid_UserDeletingRow;
            customerGrid.ColumnHeaderMouseClick += CustomerGrid_ColumnHeaderMouseClick;
            customerGrid.DataBindingComplete += CustomerGrid_DataBindingComplete;
            this.Controls.Add(customerGrid);

            // Set up address grid
            addressGrid = new DataGridView();
            addressGrid.Location = new System.Drawing.Point(20, 240);
            addressGrid.Size = new System.Drawing.Size(850, 200);
            addressGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            addressGrid.MultiSelect = false;
            addressGrid.AllowUserToDeleteRows = false;
            addressGrid.AllowUserToOrderColumns = true;
            addressGrid.AllowUserToAddRows = false;
            addressGrid.AutoGenerateColumns = false;
            addressGrid.ReadOnly = true;
            addressGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            DataGridViewTextBoxColumn addressIdColumn = new DataGridViewTextBoxColumn();
            addressIdColumn.DataPropertyName = "AddressId";
            addressIdColumn.HeaderText = "AddressId";
            addressIdColumn.Name = "AddressId";
            addressIdColumn.ReadOnly = true;
            addressGrid.Columns.Add(addressIdColumn);

            DataGridViewComboBoxColumn addressTypeColumn = new DataGridViewComboBoxColumn();
            addressTypeColumn.DataPropertyName = "AddressType";
            addressTypeColumn.HeaderText = "AddressType";
            addressTypeColumn.Name = "AddressType";
            addressTypeColumn.Items.AddRange(new string[] { "Home", "Business", "Billing", "Shipping" });
            addressTypeColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            addressGrid.Columns.Add(addressTypeColumn);

            DataGridViewTextBoxColumn addressLine1Column = new DataGridViewTextBoxColumn();
            addressLine1Column.DataPropertyName = "AddressLine1";
            addressLine1Column.HeaderText = "AddressLine1";
            addressLine1Column.Name = "AddressLine1";
            addressGrid.Columns.Add(addressLine1Column);

            DataGridViewTextBoxColumn addressLine2Column = new DataGridViewTextBoxColumn();
            addressLine2Column.DataPropertyName = "AddressLine2";
            addressLine2Column.HeaderText = "AddressLine2";
            addressLine2Column.Name = "AddressLine2";
            addressGrid.Columns.Add(addressLine2Column);

            DataGridViewTextBoxColumn cityColumn = new DataGridViewTextBoxColumn();
            cityColumn.DataPropertyName = "City";
            cityColumn.HeaderText = "City";
            cityColumn.Name = "City";
            addressGrid.Columns.Add(cityColumn);

            DataGridViewTextBoxColumn stateColumn = new DataGridViewTextBoxColumn();
            stateColumn.DataPropertyName = "State";
            stateColumn.HeaderText = "State";
            stateColumn.Name = "State";
            addressGrid.Columns.Add(stateColumn);

            DataGridViewTextBoxColumn zipColumn = new DataGridViewTextBoxColumn();
            zipColumn.DataPropertyName = "ZIP";
            zipColumn.HeaderText = "ZIP";
            zipColumn.Name = "ZIP";
            addressGrid.Columns.Add(zipColumn);

            DataGridViewTextBoxColumn countryColumn = new DataGridViewTextBoxColumn();
            countryColumn.DataPropertyName = "Country";
            countryColumn.HeaderText = "Country";
            countryColumn.Name = "Country";
            addressGrid.Columns.Add(countryColumn);

            addressGrid.UserDeletingRow += AddressGrid_UserDeletingRow;
            addressGrid.ColumnHeaderMouseClick += AddressGrid_ColumnHeaderMouseClick;

            this.Controls.Add(addressGrid);

            // Set up buttons
            Button addCustomerButton = new Button();
            addCustomerButton.Text = "Add Customer";
            addCustomerButton.Location = new System.Drawing.Point(440, 60);
            addCustomerButton.Size = new System.Drawing.Size(100, 30);
            addCustomerButton.Click += AddCustomerButton_Click;
            this.Controls.Add(addCustomerButton);

            Button editCustomerButton = new Button();
            editCustomerButton.Text = "Edit Customer";
            editCustomerButton.Location = new System.Drawing.Point(560, 60);
            editCustomerButton.Size = new System.Drawing.Size(100, 30);
            editCustomerButton.Click += EditCustomerButton_Click;
            this.Controls.Add(editCustomerButton);

            Button deleteCustomerButton = new Button();
            deleteCustomerButton.Text = "Delete Customer";
            deleteCustomerButton.Location = new System.Drawing.Point(680, 60);
            deleteCustomerButton.Size = new System.Drawing.Size(120, 30);
            deleteCustomerButton.Click += DeleteCustomerButton_Click;
            this.Controls.Add(deleteCustomerButton);

            Button addAddressButton = new Button();
            addAddressButton.Text = "Add Address";
            addAddressButton.Location = new System.Drawing.Point(440, 100);
            addAddressButton.Size = new System.Drawing.Size(100, 30);
            addAddressButton.Click += AddAddressButton_Click;
            this.Controls.Add(addAddressButton);

            Button editAddressButton = new Button();
            editAddressButton.Text = "Edit Address";
            editAddressButton.Location = new System.Drawing.Point(560, 100);
            editAddressButton.Size = new System.Drawing.Size(100, 30);
            editAddressButton.Click += EditAddressButton_Click;
            this.Controls.Add(editAddressButton);

            Button deleteAddressButton = new Button();
            deleteAddressButton.Text = "Delete Address";
            deleteAddressButton.Location = new System.Drawing.Point(680, 100);
            deleteAddressButton.Size = new System.Drawing.Size(120, 30);
            deleteAddressButton.Click += DeleteAddressButton_Click;
            this.Controls.Add(deleteAddressButton);
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
                    addressGrid.DataSource = null;
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

        private void CustomerGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (customerGrid.Columns.Contains("CustomerNumber"))
            {
                customerGrid.Columns["CustomerNumber"].ReadOnly = true;
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

#nullable disable
        private Dictionary<string, ListSortDirection> addressSortOrders = new Dictionary<string, ListSortDirection>();

        private void SortAddresses(string columnName)
        {
            if (customerGrid.SelectedRows.Count > 0)
            {
                int customerNumber = Convert.ToInt32(customerGrid.SelectedRows[0].Cells["CustomerNumber"].Value);
                Customer selectedCustomer = customers.FirstOrDefault(c => c.CustomerNumber == customerNumber);

                if (selectedCustomer != null)
                {
                    if (!addressSortOrders.ContainsKey(columnName))
                    {
                        addressSortOrders[columnName] = ListSortDirection.Ascending;
                    }
                    else
                    {
                        addressSortOrders[columnName] = addressSortOrders[columnName] == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
                    }

                    ListSortDirection sortDirection = addressSortOrders[columnName];

                    Func<Address, object> keySelector = null;

                    switch (columnName)
                    {
                        case "AddressId":
                            keySelector = a => a.AddressId;
                            break;
                        case "AddressType":
                            keySelector = a => a.AddressType;
                            break;
                        case "AddressLine1":
                            keySelector = a => a.AddressLine1;
                            break;
                        case "AddressLine2":
                            keySelector = a => a.AddressLine2;
                            break;
                        case "City":
                            keySelector = a => a.City;
                            break;
                        case "State":
                            keySelector = a => a.State;
                            break;
                        case "ZIP":
                            keySelector = a => a.ZIP;
                            break;
                        case "Country":
                            keySelector = a => a.Country;
                            break;
                    }

                    if (keySelector != null)
                    {
                        bool allValuesNull = selectedCustomer.Addresses.All(a => keySelector(a) == null || string.IsNullOrEmpty(keySelector(a).ToString()));

                        if (allValuesNull)
                        {
                            MessageBox.Show("Cannot sort when all values are null or empty.", "Sorting Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            if (sortDirection == ListSortDirection.Ascending)
                            {
                                selectedCustomer.Addresses = new SortableBindingList<Address>(selectedCustomer.Addresses.OrderBy(keySelector).ToList());
                            }
                            else
                            {
                                selectedCustomer.Addresses = new SortableBindingList<Address>(selectedCustomer.Addresses.OrderByDescending(keySelector).ToList());
                            }

                            addressGrid.DataSource = null;
                            addressGrid.DataSource = selectedCustomer.Addresses;
                        }
                    }
                }
            }
        }
#nullable enable

        private void AddCustomerButton_Click(object sender, EventArgs e)
        {
            AddCustomer();
        }

#nullable disable
        private void EditCustomerButton_Click(object sender, EventArgs e)
        {
            if (customerGrid.SelectedRows.Count > 0)
            {
                int customerNumber = Convert.ToInt32(customerGrid.SelectedRows[0].Cells["CustomerNumber"].Value);
                Customer selectedCustomer = customers.FirstOrDefault(c => c.CustomerNumber == customerNumber);
                if (selectedCustomer != null)
                {
                    EditCustomerForm editCustomerForm = new EditCustomerForm(selectedCustomer);
                    if (editCustomerForm.ShowDialog() == DialogResult.OK)
                    {
                        selectedCustomer.FirstName = editCustomerForm.FirstName;
                        selectedCustomer.LastName = editCustomerForm.LastName;
                        SaveCustomers();
                        LoadCustomers();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a customer to edit.");
            }
        }
#nullable enable

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

#nullable disable
        private void EditAddressButton_Click(object sender, EventArgs e)
        {
            if (customerGrid.SelectedRows.Count > 0 && addressGrid.SelectedRows.Count > 0)
            {
                int customerNumber = Convert.ToInt32(customerGrid.SelectedRows[0].Cells["CustomerNumber"].Value);
                Address selectedAddress = (Address)addressGrid.SelectedRows[0].DataBoundItem;
                if (selectedAddress != null)
                {
                    Customer selectedCustomer = customers.FirstOrDefault(c => c.CustomerNumber == customerNumber);
                    List<string> existingAddressTypes = selectedCustomer.Addresses
                        .Where(a => a.AddressId != selectedAddress.AddressId)
                        .Select(a => a.AddressType)
                        .ToList();

                    EditAddressForm editAddressForm = new EditAddressForm(selectedAddress, existingAddressTypes);
                    if (editAddressForm.ShowDialog() == DialogResult.OK)
                    {
                        selectedAddress.AddressType = editAddressForm.AddressType;
                        selectedAddress.AddressLine1 = editAddressForm.AddressLine1;
                        selectedAddress.AddressLine2 = editAddressForm.AddressLine2;
                        selectedAddress.City = editAddressForm.City;
                        selectedAddress.State = editAddressForm.State;
                        selectedAddress.ZIP = editAddressForm.ZIP;
                        selectedAddress.Country = editAddressForm.Country;
                        SaveCustomers();
                        LoadAddresses(customerNumber);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a customer and an address to edit.");
            }
        }
#nullable enable

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