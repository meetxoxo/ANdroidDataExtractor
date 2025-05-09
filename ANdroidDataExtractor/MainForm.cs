using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AndroidDataExtractor.Models;
using AndroidDataExtractor.Services;

namespace AndroidDataExtractor.Forms
{
    public partial class MainForm : Form
    {
        private ADBDataExtractor _adbDataExtractor;
        private DatabaseManager _databaseManager;
        private ReportGenerator _reportGenerator;
        private object dgvContacts;

        public object dgvMessages { get; private set; }
        public object txtDeviceInfo { get; private set; }
        public object dgvCallLogs { get; private set; }

        public MainForm()
        {
            InitializeComponent();
            _adbDataExtractor = new ADBDataExtractor();
            _databaseManager = new DatabaseManager();
            _reportGenerator = new ReportGenerator();
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Initialization code if needed
        }

        private void btnLoadData_Click(object sender, EventArgs e, object dgvCallLogs)
        {
            try
            {
                // Extract data using ADB
                var contacts = _adbDataExtractor.GetContacts();
                var messages = _adbDataExtractor.GetMessages();
                var callLogs = _adbDataExtractor.GetCallLogs();
                var deviceInfo = _adbDataExtractor.GetDeviceInfo();

                // Display data in grid/list
                dgvContacts.DataSource = contacts;
                dgvMessages.DataSource = messages;
                dgvCallLogs.DataSource = callLogs;
                txtDeviceInfo.Text = $"CPU Info: {deviceInfo.CPUInfo}\nMemory Info: {deviceInfo.MemoryInfo}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveToDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                // Save data to database
                var contacts = dgvContacts.DataSource as List<Contact>;
                var messages = dgvMessages.DataSource as List<System.Windows.Forms.Message>;
                var callLogs = dgvCallLogs.DataSource as List<CallLog>;
                var deviceInfo = new DeviceInfo
                {
                    CPUInfo = txtDeviceInfo.Lines[0],
                    MemoryInfo = txtDeviceInfo.Lines[1]
                };

                _databaseManager.InsertContacts(contacts);
                _databaseManager.InsertMessages(messages);
                _databaseManager.InsertCallLogs(callLogs);
                _databaseManager.InsertDeviceInfo(deviceInfo);

                MessageBox.Show("Data saved to database successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data to database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewReport_Click(object sender, EventArgs e)
        {
            try
            {
                // Generate PDF and Word report
                var contacts = dgvContacts.DataSource as List<Contact>;
                var messages = dgvMessages.DataSource as List<System.Windows.Forms.Message>;
                var callLogs = dgvCallLogs.DataSource as List<CallLog>;
                var deviceInfo = new DeviceInfo
                {
                    CPUInfo = txtDeviceInfo.Lines[0],
                    MemoryInfo = txtDeviceInfo.Lines[1]
                };

                string pdfPath = "report.pdf";
                string wordPath = "report.docx";

                _reportGenerator.GeneratePdfReport(contacts, messages, callLogs, deviceInfo, pdfPath);
                _reportGenerator.GenerateWordReport(contacts, messages, callLogs, deviceInfo, wordPath);

                MessageBox.Show("Reports generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
