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
    public partial class UC_AddItems : UserControl
    {
        function fn = new function();

        public UC_AddItems()
        {
            InitializeComponent();
        }

        // "Add Item" düğmesine tıklandığında çalışacak olan olay işleyicisi
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            // Girdilerin doğruluğunu kontrol etmek için değişkenler tanımlanır
            string itemName = txtItemName.Text.Trim();
            string category = txtCategory.Text.Trim();
            decimal price;

            // Eğer gerekli alanlar boşsa veya fiyat geçerli bir sayıya dönüştürülemezse, kullanıcıya uyarı gösterilir
            if (string.IsNullOrEmpty(itemName) || string.IsNullOrEmpty(category) || !decimal.TryParse(txtPrice.Text, out price))
            {
                MessageBox.Show("Please enter valid input.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Veritabanına ekleme sorgusu oluşturulur
            string query = "INSERT INTO iitems (name, category, price) VALUES (@itemName, @category, @price)";

            // Veritabanı bağlantısı oluşturulur
            using (SqlConnection con = fn.getConnection())
            {
                // Sorgu için SqlCommand nesnesi oluşturulur
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@itemName", itemName);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@price", price);

                    try
                    {
                        // Veritabanı bağlantısı açılır
                        con.Open();
                        // Komut çalıştırılır ve item başarıyla eklenirse kullanıcıya bilgi mesajı gösterilir
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Item added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clearAll();
                    }
                    catch (SqlException ex)
                    {
                        // Hata durumunda kullanıcıya hata mesajı gösterilir
                        MessageBox.Show("An error occurred while adding the item: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Tüm girdileri temizleme işlemi
        public void clearAll()
        {
            txtCategory.SelectedIndex = -1;
            txtItemName.Clear();
            txtPrice.Clear();
        }

        // UserControl'den ayrıldığında tüm girdileri temizleme işlemi
        private void UC_AddItems_Leave(object sender, EventArgs e)
        {
            clearAll();
        }
    }
}
