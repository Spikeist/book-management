namespace Book_Management
{
    public partial class EditCustomerForm : Form
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        private Label FirstNameLabel;
        private Label LastNameLabel;
        private TextBox FirstNameTextBox;
        private TextBox LastNameTextBox;
        private Button SaveButton;

#nullable disable
        public EditCustomerForm(Customer customer)
        {
            InitializeComponent();
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            FirstNameTextBox.Text = FirstName;
            LastNameTextBox.Text = LastName;
        }

        private void InitializeComponent()
        {
            this.FirstNameLabel = new System.Windows.Forms.Label();
            this.LastNameLabel = new System.Windows.Forms.Label();
            this.FirstNameTextBox = new System.Windows.Forms.TextBox();
            this.LastNameTextBox = new System.Windows.Forms.TextBox();
            this.SaveButton = new System.Windows.Forms.Button();

            this.FirstNameLabel.AutoSize = true;
            this.FirstNameLabel.Location = new System.Drawing.Point(12, 15);
            this.FirstNameLabel.Name = "FirstNameLabel";
            this.FirstNameLabel.Size = new System.Drawing.Size(60, 13);
            this.FirstNameLabel.TabIndex = 0;
            this.FirstNameLabel.Text = "First Name:";

            this.LastNameLabel.AutoSize = true;
            this.LastNameLabel.Location = new System.Drawing.Point(12, 41);
            this.LastNameLabel.Name = "LastNameLabel";
            this.LastNameLabel.Size = new System.Drawing.Size(61, 13);
            this.LastNameLabel.TabIndex = 1;
            this.LastNameLabel.Text = "Last Name:";

            this.FirstNameTextBox.Location = new System.Drawing.Point(93, 12);
            this.FirstNameTextBox.Name = "FirstNameTextBox";
            this.FirstNameTextBox.Size = new System.Drawing.Size(179, 20);
            this.FirstNameTextBox.TabIndex = 2;

            this.LastNameTextBox.Location = new System.Drawing.Point(93, 38);
            this.LastNameTextBox.Name = "LastNameTextBox";
            this.LastNameTextBox.Size = new System.Drawing.Size(179, 20);
            this.LastNameTextBox.TabIndex = 3;

            this.SaveButton.Location = new System.Drawing.Point(197, 64);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 4;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 99);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.LastNameTextBox);
            this.Controls.Add(this.FirstNameTextBox);
            this.Controls.Add(this.LastNameLabel);
            this.Controls.Add(this.FirstNameLabel);
            this.Name = "AddCustomerForm";
            this.Text = "Add Customer";
        }
#nullable enable

        private void SaveButton_Click(object sender, EventArgs e)
        {
            FirstName = FirstNameTextBox.Text;
            LastName = LastNameTextBox.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}