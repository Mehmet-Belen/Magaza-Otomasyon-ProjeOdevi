using System.Data.SqlClient;
using DGVPrinterHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace odev_proje.AllUserControl
{
    public partial class UC_PlaceOrder : UserControl
    {
        function fn = new function();
        String query;

        public UC_PlaceOrder()
        {
            InitializeComponent();
        }

        // Kategori seçildiğinde çalışacak olan olay işleyicisi
        private void comboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            String category = comboCategory.Text;
            string query = "SELECT name FROM iitems WHERE category = @Category";
            SqlParameter[] parameters =
            {
                new SqlParameter("@Category", category)
            };
            showItemList(query, parameters);
        }

        // DataGridView hücresi içeriği değiştiğinde çalışacak olan olay işleyicisi
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Bu kısmı kullanmadık.
        }

        // Arama metni değiştikçe çalışacak olan olay işleyicisi
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            String category = comboCategory.Text;
            string searchText = txtSearch.Text;
            string query = "SELECT name FROM iitems WHERE category = @Category AND name LIKE @SearchText";
            SqlParameter[] parameters =
            {
                new SqlParameter("@Category", category),
                new SqlParameter("@SearchText", searchText + "%")
            };
            showItemList(query, parameters);
        }

        // Ürün listesini gösteren yardımcı bir işlev
        private void showItemList(String query, SqlParameter[] parameters)
        {
            listBox1.Items.Clear();

            DataSet ds = fn.getData(query, parameters);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                listBox1.Items.Add(ds.Tables[0].Rows[i][0].ToString());
            }
        }

        // Ürün listesinde seçim yapıldığında çalışacak olan olay işleyicisi
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtQuantityUpDown.ResetText();
            txtTotal.Clear();

            String text = listBox1.GetItemText(listBox1.SelectedItem);
            txtItemName.Text = text;
            string query = "SELECT price FROM iitems WHERE name = @Name";
            SqlParameter[] parameters =
            {
                new SqlParameter("@Name", text)
            };
            DataSet ds = fn.getData(query, parameters);

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtPrice.Text = ds.Tables[0].Rows[0]["price"].ToString();
                }
            }
            catch { }
        }

        // Miktar değiştiğinde çalışacak olan olay işleyicisi
        private void txtQuantityUpDown_ValueChanged(object sender, EventArgs e)
        {
            Int64 quan = Int64.Parse(txtQuantityUpDown.Value.ToString());
            Int64 price = Int64.Parse(txtPrice.Text);
            txtTotal.Text = (quan * price).ToString();
        }

        protected int n, total = 0;

        // "Sepete Ekle" düğmesine tıklandığında çalışacak olan olay işleyicisi
        private void btnAddtoCart_Click(object sender, EventArgs e)
        {
            if (txtTotal.Text != "0" && txtTotal.Text != "")
            {
                n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = txtItemName.Text;
                dataGridView1.Rows[n].Cells[1].Value = txtPrice.Text;
                dataGridView1.Rows[n].Cells[2].Value = txtQuantityUpDown.Value;
                dataGridView1.Rows[n].Cells[3].Value = txtTotal.Text;

                total += int.Parse(txtTotal.Text);
                labelTotalAmount.Text = "Rs. " + total;

                txtQuantityUpDown.Value = 0;
                txtTotal.Clear();
            }
            else
            {
                MessageBox.Show("Minimum Quantity need to be 1", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        int amount;

        // DataGridView hücresine tıklandığında çalışacak olan olay işleyicisi
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                amount = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
            }
            catch { }
        }

        // "Kaldır" düğmesine tıklandığında çalışacak olan olay işleyicisi
        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows.RemoveAt(this.dataGridView1.SelectedRows[0].Index);
            }
            catch { }
            total -= amount;
            if(total < 0)
                total = 0;
            labelTotalAmount.Text = "Rs. " + total;
        }

        // "Yazdır" düğmesine tıklandığında çalışacak olan olay işleyicisi
        private void btnPrint_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Customer Bill";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "Total Payable Amount : " + labelTotalAmount.Text;
            printer.FooterSpacing = 15;
            printer.PrintDataGridView(dataGridView1);

            total = 0;
            dataGridView1.Rows.Clear();
            labelTotalAmount.Text = "Rs. " + total;
        }
    }
}
