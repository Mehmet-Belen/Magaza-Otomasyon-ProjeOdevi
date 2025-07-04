using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace odev_proje.AllUserControl
{
    public partial class UC_UpdateItems : UserControl
    {
        function fn = new function();
        String query;

        public UC_UpdateItems()
        {
            InitializeComponent();
        }

        // UserControl yüklendiğinde çalışacak olan olay işleyicisi
        private void UC_UpdateItems_Load(object sender, EventArgs e)
        {
            // Verileri yükleme işlemi
            loadData();
        }

        // Veritabanından verileri yükleme işlemi
        public void loadData()
        {
            query = "select * from iitems";
            DataSet ds = fn.getData(query, null);
            dataGridView1.DataSource = ds.Tables[0];
        }

        // Arama metni değiştikçe çalışacak olan olay işleyicisi
        private void txtSearchItem_TextChanged(object sender, EventArgs e)
        {
            // Arama sorgusu oluşturulur
            query = "SELECT * FROM iitems WHERE name LIKE @Name";
            SqlParameter[] parameters =
            {
                new SqlParameter("@Name", txtSearchItem.Text + "%")
            };
            DataSet ds = fn.getData(query, parameters);
            dataGridView1.DataSource = ds.Tables[0];
        }

        int id;
        // DataGridView hücresine tıklandığında çalışacak olan olay işleyicisi
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Seçilen satırın verileri alınır
            id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            String category = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            String name = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            int price = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());

            // TextBox kontrollerine veriler aktarılır
            txtCategory.Text = category;
            txtName.Text = name;
            txtPrice.Text = price.ToString();
        }

        // "Update" düğmesine tıklandığında çalışacak olan olay işleyicisi
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Güncelleme sorgusu oluşturulur
            query = "UPDATE iitems SET name = @Name, category = @Category, price = @Price WHERE iid = @Id";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Name", txtName.Text),
                new SqlParameter("@Category", txtCategory.Text),
                new SqlParameter("@Price", txtPrice.Text),
                new SqlParameter("@Id", id)
            };

            // Veritabanı işlevi kullanılarak veri güncelleme işlemi yapılır
            fn.setData(query, parameters);
            // Veriler yeniden yüklenir
            loadData();

            // TextBox kontrolleri temizlenir
            txtName.Clear();
            txtCategory.Clear();
            txtPrice.Clear();
        }
    }
}
