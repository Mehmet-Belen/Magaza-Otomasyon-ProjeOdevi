using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace odev_proje
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        // Kullanıcı adını parametre olarak alan bir yapıcı metod
        public Dashboard(string user)
        {
            InitializeComponent();

            // Kullanıcı "Guest" ise bazı düğmeleri gizle
            if (user == "Guest")
            {
                btnAddItems.Hide();
                btnUpdate.Hide();
                btnRemove.Hide();
            }
            // Kullanıcı "Admin" ise bazı düğmeleri göster
            else if (user == "Admin")
            {
                btnAddItems.Show();
                btnUpdate.Show();
                btnRemove.Show();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogOut_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Giriş formunu oluştur ve bu formu gizle
            Form1 fm = new Form1();
            this.Hide();
            fm.Show();
        }

        private void uC_Welcome1_Load(object sender, EventArgs e)
        {

        }

        private void btnAddItems_Click(object sender, EventArgs e)
        {
            // "AddItems" kullanıcı kontrolünü görünür yap ve en öne getir
            uC_AddItems1.Visible = true;
            uC_AddItems1.BringToFront();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            // Form yüklendiğinde bazı kullanıcı kontrollerini gizle
            uC_AddItems1.Visible = false;
            uC_PlaceOrder1.Visible = false;
            uC_UpdateItems1.Visible = false;
        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            // "PlaceOrder" kullanıcı kontrolünü görünür yap ve en öne getir
            uC_PlaceOrder1.Visible = true;
            uC_PlaceOrder1.BringToFront();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // "UpdateItems" kullanıcı kontrolünü görünür yap ve en öne getir
            uC_UpdateItems1.Visible = true;
            uC_UpdateItems1.BringToFront();
        }
    }
}
