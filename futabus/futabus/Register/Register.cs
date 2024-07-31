﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using futabus.models;
using futabus.Login;
using futabus.models;
using DevExpress.Utils;

namespace futabus.Register
{
    public partial class Register : Form
    {
        public static string connectionString = "mongodb://localhost:27017";
        // Đối tượng MongoClient để kết nối đến MongoDB
        private MongoClient _client;

        // Collection để thao tác với dữ liệu người dùng trong MongoDB
        private IMongoCollection<User> _userCollection;


        public Register()
        {


            FormBorderStyle = FormBorderStyle.FixedSingle; // Chỉ cho phép cố định kích thước
            MaximizeBox = false; // Ẩn nút phóng to
            MinimizeBox = false; // Ẩn nút thu nhỏ
            InitializeComponent();
            // Khởi tạo kết nối đến MongoDB
            _client = new MongoClient(connectionString);

            // Lấy reference đến database và collection "User"
            var database = _client.GetDatabase("MDM");
            _userCollection = database.GetCollection<User>("User");
        }

        private void Register_Load(object sender, EventArgs e)
        {

        }

        private void usernameTxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void passwordTxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void registerBtn_Click(object sender, EventArgs e)
        {
            if (passwordTxt.Text == reEnterPasswordtxt.Text)
            {
                int new_userID = GetNextUserID();

                User newUser = new User
                {
                    userID = new_userID,
                    username = usernameTxt.Text.Trim(),
                    password = passwordTxt.Text.Trim(),
                    email = emailTxt.Text.Trim()
                };

                bool registerSuccess = RegisterUser(newUser);

                if (registerSuccess)
                {
                    MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Đóng form đăng ký sau khi đăng ký thành công
                    futabus.Login.Login form = new Login.Login();
                    form.Show();
                    this.Hide();

                }
                else
                {
                    MessageBox.Show("Đăng ký thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Mật khẩu không khớp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }


        private int GetNextUserID()
        {
            try
            {
                // Tìm userID lớn nhất hiện tại trong collection
                var sort = Builders<User>.Sort.Descending("userID");
                var lastUser = _userCollection.Find(FilterDefinition<User>.Empty).Sort(sort).FirstOrDefault();

                if (lastUser != null)
                {
                    return lastUser.userID + 1;
                }
                else
                {
                    // Nếu collection trống, bắt đầu từ 1
                    return 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy userID: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1; // Trả về 1 nếu có lỗi
            }
        }

        private bool RegisterUser(User user)
        {
            try
            {
                // Insert đối tượng User vào MongoDB
                _userCollection.InsertOne(user);
                return true; // Đăng ký thành công
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đăng ký: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Đăng ký thất bại
            }
        }
        private void label4_Click(object sender, EventArgs e)
        {

        }



        private void registerBtn_MouseEnter(object sender, EventArgs e)
        {
            registerBtn.BackColor = Color.OrangeRed;
            registerBtn.ForeColor = Color.White;


        }

        private void registerBtn_MouseLeave(object sender, EventArgs e)
        {
            registerBtn.BackColor = SystemColors.Control;
            registerBtn.ForeColor = SystemColors.ControlText;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
