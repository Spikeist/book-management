namespace Book_Management
{
    public partial class EditAddressForm : Form
    {
        public string AddressType { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string AddressLine2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZIP { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        private ComboBox addressTypeComboBox;
        private TextBox addressLine1TextBox;
        private TextBox addressLine2TextBox;
        private TextBox cityTextBox;
        private TextBox stateTextBox;
        private TextBox zipTextBox;
        private TextBox countryTextBox;
        private Button saveButton;

#nullable disable
        public EditAddressForm(Address address, List<string> existingAddressTypes)
        {
            InitializeComponent();
            AddressType = address.AddressType;
            AddressLine1 = address.AddressLine1;
            AddressLine2 = address.AddressLine2;
            City = address.City;
            State = address.State;
            ZIP = address.ZIP;
            Country = address.Country;

            addressTypeComboBox.Text = AddressType;
            addressLine1TextBox.Text = AddressLine1;
            addressLine2TextBox.Text = AddressLine2;
            cityTextBox.Text = City;
            stateTextBox.Text = State;
            zipTextBox.Text = ZIP;
            countryTextBox.Text = Country;

            addressTypeComboBox.Items.Clear();
            addressTypeComboBox.Items.AddRange(new string[] { "Home", "Business", "Billing", "Shipping" });

            foreach (string addressType in existingAddressTypes)
            {
                if (addressType != address.AddressType)
                {
                    addressTypeComboBox.Items.Remove(addressType);
                }
            }

            addressTypeComboBox.SelectedItem = AddressType;
        }

        private void InitializeComponent()
        {
            this.addressTypeComboBox = new System.Windows.Forms.ComboBox();
            this.addressLine1TextBox = new System.Windows.Forms.TextBox();
            this.addressLine2TextBox = new System.Windows.Forms.TextBox();
            this.cityTextBox = new System.Windows.Forms.TextBox();
            this.stateTextBox = new System.Windows.Forms.TextBox();
            this.zipTextBox = new System.Windows.Forms.TextBox();
            this.countryTextBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();

            Label addressTypeLabel = new Label();
            Label addressLine1Label = new Label();
            Label addressLine2Label = new Label();
            Label cityLabel = new Label();
            Label stateLabel = new Label();
            Label zipLabel = new Label();
            Label countryLabel = new Label();

            addressTypeLabel.AutoSize = true;
            addressTypeLabel.Location = new System.Drawing.Point(12, 15);
            addressTypeLabel.Name = "addressTypeLabel";
            addressTypeLabel.Size = new System.Drawing.Size(74, 13);
            addressTypeLabel.TabIndex = 0;
            addressTypeLabel.Text = "Address Type:";
            this.Controls.Add(addressTypeLabel);

            this.addressTypeComboBox.FormattingEnabled = true;
            this.addressTypeComboBox.Items.AddRange(new object[] {
    "Home",
    "Business",
    "Billing",
    "Shipping"});
            this.addressTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.addressTypeComboBox.Location = new System.Drawing.Point(110, 12);
            this.addressTypeComboBox.Name = "addressTypeComboBox";
            this.addressTypeComboBox.Size = new System.Drawing.Size(162, 21);
            this.addressTypeComboBox.TabIndex = 1;

            addressLine1Label.AutoSize = true;
            addressLine1Label.Location = new System.Drawing.Point(12, 42);
            addressLine1Label.Name = "addressLine1Label";
            addressLine1Label.Size = new System.Drawing.Size(77, 13);
            addressLine1Label.TabIndex = 2;
            addressLine1Label.Text = "Address Line 1:";
            this.Controls.Add(addressLine1Label);

            this.addressLine1TextBox.Location = new System.Drawing.Point(110, 42);
            this.addressLine1TextBox.Name = "addressLine1TextBox";
            this.addressLine1TextBox.Size = new System.Drawing.Size(162, 20);
            this.addressLine1TextBox.TabIndex = 3;

            addressLine2Label.AutoSize = true;
            addressLine2Label.Location = new System.Drawing.Point(12, 68);
            addressLine2Label.Name = "addressLine2Label";
            addressLine2Label.Size = new System.Drawing.Size(77, 13);
            addressLine2Label.TabIndex = 4;
            addressLine2Label.Text = "Address Line 2:";
            this.Controls.Add(addressLine2Label);

            this.addressLine2TextBox.Location = new System.Drawing.Point(110, 72);
            this.addressLine2TextBox.Name = "addressLine2TextBox";
            this.addressLine2TextBox.Size = new System.Drawing.Size(162, 20);
            this.addressLine2TextBox.TabIndex = 5;

            cityLabel.AutoSize = true;
            cityLabel.Location = new System.Drawing.Point(12, 94);
            cityLabel.Name = "cityLabel";
            cityLabel.Size = new System.Drawing.Size(27, 13);
            cityLabel.TabIndex = 6;
            cityLabel.Text = "City:";
            this.Controls.Add(cityLabel);

            this.cityTextBox.Location = new System.Drawing.Point(110, 102);
            this.cityTextBox.Name = "cityTextBox";
            this.cityTextBox.Size = new System.Drawing.Size(162, 20);
            this.cityTextBox.TabIndex = 7;

            stateLabel.AutoSize = true;
            stateLabel.Location = new System.Drawing.Point(12, 120);
            stateLabel.Name = "stateLabel";
            stateLabel.Size = new System.Drawing.Size(35, 13);
            stateLabel.TabIndex = 8;
            stateLabel.Text = "State:";
            this.Controls.Add(stateLabel);

            this.stateTextBox.Location = new System.Drawing.Point(110, 132);
            this.stateTextBox.Name = "stateTextBox";
            this.stateTextBox.Size = new System.Drawing.Size(162, 20);
            this.stateTextBox.TabIndex = 9;

            zipLabel.AutoSize = true;
            zipLabel.Location = new System.Drawing.Point(12, 146);
            zipLabel.Name = "zipLabel";
            zipLabel.Size = new System.Drawing.Size(25, 13);
            zipLabel.TabIndex = 10;
            zipLabel.Text = "ZIP:";
            this.Controls.Add(zipLabel);

            this.zipTextBox.Location = new System.Drawing.Point(110, 162);
            this.zipTextBox.Name = "zipTextBox";
            this.zipTextBox.Size = new System.Drawing.Size(162, 20);
            this.zipTextBox.TabIndex = 11;

            countryLabel.AutoSize = true;
            countryLabel.Location = new System.Drawing.Point(12, 172);
            countryLabel.Name = "countryLabel";
            countryLabel.Size = new System.Drawing.Size(46, 13);
            countryLabel.TabIndex = 12;
            countryLabel.Text = "Country:";
            this.Controls.Add(countryLabel);

            this.countryTextBox.Location = new System.Drawing.Point(110, 192);
            this.countryTextBox.Name = "countryTextBox";
            this.countryTextBox.Size = new System.Drawing.Size(162, 20);
            this.countryTextBox.TabIndex = 13;

            this.saveButton.Location = new System.Drawing.Point(197, 222);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 14;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 250);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.countryTextBox);
            this.Controls.Add(this.zipTextBox);
            this.Controls.Add(this.stateTextBox);
            this.Controls.Add(this.cityTextBox);
            this.Controls.Add(this.addressLine2TextBox);
            this.Controls.Add(this.addressLine1TextBox);
            this.Controls.Add(this.addressTypeComboBox);
            this.Name = "AddAddressForm";
            this.Text = "Add Address";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
#nullable enable

        private void SaveButton_Click(object sender, EventArgs e)
        {
            AddressType = addressTypeComboBox.SelectedItem?.ToString() ?? string.Empty;
            AddressLine1 = addressLine1TextBox.Text;
            AddressLine2 = addressLine2TextBox.Text;
            City = cityTextBox.Text;
            State = stateTextBox.Text;
            ZIP = zipTextBox.Text;
            Country = countryTextBox.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}