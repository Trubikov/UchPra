﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ProjectCRUDLoginFormApp
{
    public partial class AdminAddUsers : Form, IValidators
    {
        // string connectionString = @"Data Source = DESKTOP-3P9I28U\MATEUSZSQL; Initial Catalog = ProjectCRUDLoginForm; Integrated Security =True";
        string connectionString = @"Data Source = ADCLG1; Initial Catalog = 419/1_Trubikov; Integrated Security =True";
        LoginForm loginForm = new LoginForm();

        public AdminAddUsers()
        {
            InitializeComponent();
        }

        #region Button Methods
        private void bClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            AddNewRecord();
            loginForm.Hide();
        }
        #endregion

        #region Validators
        public bool CheckIfLoginExist()
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand("spDetectIfLoginExist", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@Login", txtLoginAdminPanel.Text);

                int UserExist = (int)sqlCmd.ExecuteScalar();

                if (UserExist > 0)
                    return false;
                else
                    return true;
            }
        }

        public bool CheckIfEmailExist()
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand("spDetectIfEmailExist", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@Email", txtEmailAdminPanel.Text);

                int EmailExist = (int)sqlCmd.ExecuteScalar();

                if (EmailExist > 0)
                    return false;
                else
                    return true;
            }
        }

        public bool EmailValidator()
        {
            string emailChecker = txtEmailAdminPanel.Text;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(emailChecker);
            if (!(match.Success))
                return false;
            else
                return true;
        }

        public bool PasswordValidation()
        {
            string PasswordChecker = txtPasswordAdminPanel.Text;
            Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$");
            Match match = regex.Match(PasswordChecker);
            if (!(match.Success))
                return false;
            else
                return true;
        }

        public void AddNewRecord()
        {
            if (txtFirstNameAdminPanel.Text == "" ||
                txtLastNameAdminPanel.Text == "" ||
                txtEmailAdminPanel.Text == "" ||
                txtLoginAdminPanel.Text == "" ||
                txtPasswordAdminPanel.Text == "" ||
                comboRoleAdminPanel.Text == "")
            {
                MessageBox.Show("Fill up all!");
            }
            else if (CheckIfLoginExist() == false)
            {
                MessageBox.Show("This Login already exist, choose different!", "Login exist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (CheckIfEmailExist() == false)
            {
                MessageBox.Show("This EMAIL already exist, choose different!", "Email exist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (EmailValidator() == false)
            {
                MessageBox.Show("Wrong email");
            }
            else if (PasswordValidation() == false)
            {
                MessageBox.Show("Password MUST contain:" + "\n"
                    + "- Min. 8 characters" + "\n" + "- Max. 15 characters" + "\n"
                    + "- Min. One uppercase and lowercase letter" + "\n"
                    + "- Min. One number.", "Password requirements", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                AddNewRecordByAdmin();
            }
        }
        #endregion

        #region Other Methods
        private void AddNewRecordByAdmin()
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand("spUserRegistration", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@FirstName", txtFirstNameAdminPanel.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@LastName", txtLastNameAdminPanel.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Email", txtEmailAdminPanel.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Login", txtLoginAdminPanel.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Password", txtPasswordAdminPanel.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Role", comboRoleAdminPanel.Text.Trim());
                sqlCmd.ExecuteNonQuery();
                this.Hide();
                MessageBox.Show("ACCOUNT CREATED SUCCESSFULLY!", "New Account", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion
    }
}
