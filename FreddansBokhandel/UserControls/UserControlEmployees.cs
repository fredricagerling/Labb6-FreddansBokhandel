﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FreddansBokhandel
{
    public partial class UserControlEmployees : UserControl
    {
        List<Employee> employees;
        Employee selectedEmployee;
        public UserControlEmployees()
        {
            InitializeComponent();
        }

        private void LoadEmployeesFromDatabase()
        {
            using (var db = new FreddansBokhandelContext())
            {
                try
                {
                    if (db.Database.CanConnect())
                    {
                        employees = db.Anställda.Include(b => b.Stores).ToList();
                    }

                    else
                    {
                        MessageBox.Show("Kunde inte koppla upp mot databasen.");
                    }
                }

                catch
                {
                    return;
                }
            }
        }

        private void PopulateDataGridEmployees()
        {
            dataGridViewEmployees.Rows.Clear();

            foreach (var employee in employees)
            {
                int rowIndex = dataGridViewEmployees.Rows.Add();
                dataGridViewEmployees.Rows[rowIndex].Cells["ID"].Value = employee.Id;
                dataGridViewEmployees.Rows[rowIndex].Cells["Namn"].Value = employee.FirstName + " " + employee.LastName;
                dataGridViewEmployees.Rows[rowIndex].Cells["Butik"].Value = employee.Stores.Name;
                dataGridViewEmployees.Rows[rowIndex].Cells["Anställningsdatum"].Value = employee.HireDate.ToString("yyyy-MM-dd");
                dataGridViewEmployees.Rows[rowIndex].Cells["Roll"].Value = employee.Role;
                dataGridViewEmployees.Rows[rowIndex].Cells["Email"].Value = employee.Email;
                dataGridViewEmployees.Rows[rowIndex].Cells["Telefon"].Value = employee.Telephone;
                dataGridViewEmployees.Rows[rowIndex].Cells["Adress"].Value = employee.Address;
                dataGridViewEmployees.Rows[rowIndex].Cells["Postnr"].Value = employee.ZipCode;
                dataGridViewEmployees.Rows[rowIndex].Cells["Postadress"].Value = employee.PostalAdress;
                dataGridViewEmployees.Rows[rowIndex].Tag = employee;
            }
        }

        private void CreateNewEmployee()
        {
            selectedEmployee = null;
            FormAddorEditEmployee newEmployee = new FormAddorEditEmployee(selectedEmployee);
            newEmployee.ShowDialog();
        }

        private void buttonAddEmployee_Click(object sender, EventArgs e)
        {
            CreateNewEmployee();
            LoadEmployeesFromDatabase();
            PopulateDataGridEmployees();
        }

        private void UserControlEmployees_Load(object sender, EventArgs e)
        {
            LoadEmployeesFromDatabase();
            PopulateDataGridEmployees();
        }

        private void buttonEditEmployee_Click(object sender, EventArgs e)
        {
            EditEmployee();
        }

        private void EditEmployee()
        {
            FormAddorEditEmployee newEmployee = new FormAddorEditEmployee(selectedEmployee);
            newEmployee.ShowDialog();
        }

        private void SelectingARow()
        {
            if (dataGridViewEmployees.CurrentCell == null) { return; }
            if (dataGridViewEmployees.SelectedRows.Count < 1) { return; }

            buttonEditEmployee.Enabled = true;
            int selectedIndex = dataGridViewEmployees.SelectedRows[0].Index;
            selectedEmployee = dataGridViewEmployees.Rows[selectedIndex].Tag as Employee;
        }

        private void dataGridViewEmployees_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            SelectingARow();
        }
    }
}