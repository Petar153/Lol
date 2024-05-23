using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Baze_LV7_predlozak
{


    public partial class Form1 : Form
    {

        //Ovdje deklariraj SQL komande koristeci SQLCommand parametarsku sintaksu za zadatke 1a, 1b, 2 i 3

        private static string SQLInsert = "INSERT INTO Osoba (Ime, Prezime, OIB, DatumRodjenja, Spol, Visina, BrojCipela) VALUES (@Ime, @Prezime, @OIB, @DatumRodjenja, @Spol, @Visina, @BrojCipela)"; 

        private static string SQLUpdate = "UPDATE Osoba SET Ime = @Ime, Prezime = @Prezime, DatumRodjenja = @DatumRodjenja, Spol = @Spol, Visina = @Visina, BrojCipela = @BrojCipela WHERE OIB = @OIB";

        private static string SQLDelete = "DELETE FROM Persons WHERE PersonID = @PersonID";

        private static string SQLSelect = "SELECT * FROM osobe ORDER BY Prezime";


        public Form1()
        {
            InitializeComponent();
            btnDelete.Enabled = false;
        }
        private DBStudent Dbs;

        private void btnSve_Click(object sender, EventArgs e)
        {
            // NE MIJENJAJ

            //Funkcija koja traži od korisnika da unese zaporku
            if (Dbs == null)
            {
                using (FormLogin wl = new FormLogin())  //otvara login prozor
                {
                    wl.ShowDialog();
                    wl.Focus();
                    Dbs = new DBStudent(wl.Pwd);        //kreira klasu za sigurno
                                                        //korištenje zaporke
                    if (string.IsNullOrWhiteSpace(wl.Pwd))
                        return;
                }
            }

            using (SqlConnection conn = Dbs.GetConnection())
            {
                // Kodiraj 1a zadatak u funkciju LOadOsobe
                LoadOsobe(conn);

                if (dgvPodaci.Rows.Count > 0)
                    dgvPodaci.Rows[0].Selected = false;
            }

        }

        private void btnSpremi_Click(object sender, EventArgs e)
        {
            if (Dbs == null)
                return;

            string ime = txtIme.Text;
            string prezime = txtPrezime.Text;
            string oib = txtOIB.Text;
            DateTime datumRodjenja = txtDatum.Value;
            char spol = rbM.Checked ? 'M' : 'Z';
            decimal visina = decimal.Parse(txtVisina.Text);
            int brojCipela = int.Parse(txtBrCip.Text);

            using (SqlConnection conn = Dbs.GetConnection())
            {
                // OVDJE PIŠETE KOD ZA ZADATAK 1. b) i ZADATAK 2.:

                if (dgvPodaci.SelectedRows.Count > 0)
                {

                    using (SqlCommand command = new SqlCommand(SQLUpdate, connection))
                    {
                        
                        command.Parameters.AddWithValue("@Ime", ime);
                        command.Parameters.AddWithValue("@Prezime", prezime);
                        command.Parameters.AddWithValue("@OIB", oib);
                        command.Parameters.AddWithValue("@DatumRodjenja", datumRodjenja);
                        command.Parameters.AddWithValue("@Spol", spol);
                        command.Parameters.AddWithValue("@Visina", visina);
                        command.Parameters.AddWithValue("@BrojCipela", brojCipela);


                        conn.Open();
                            command.ExecuteNonQuery();
                        conn.Close();
                    }

                }
                else
                {


                    using (SqlCommand command = new SqlCommand(SQLInsert, connection))
                    {
                        command.Parameters.AddWithValue("@Ime", ime);
                        command.Parameters.AddWithValue("@Prezime", prezime);
                        command.Parameters.AddWithValue("@OIB", oib);
                        command.Parameters.AddWithValue("@DatumRodjenja", datumRodjenja);
                        command.Parameters.AddWithValue("@Spol", spol);
                        command.Parameters.AddWithValue("@Visina", visina);
                        command.Parameters.AddWithValue("@BrojCipela", brojCipela);


                        conn.Open();
                        command.ExecuteNonQuery();
                        conn.Close();

                    }
                }

                // NE MIJENJAJ ispod ove linije ******************
                LoadOsobe(conn);
                SelectCurrentRow();

            }
        }

        //izaberi aktivan red ako ga ima
        private void SelectCurrentRow()
        {
            // NE MIJENJAJ

            int selectedIndex = -1;

            dgvPodaci.ClearSelection();
            if (string.IsNullOrEmpty(txtOIB.Text) && dgvPodaci.Rows.Count > 0)
                selectedIndex = 0;
            else
            {
                foreach (DataGridViewRow row in dgvPodaci.Rows)
                {
                    if (row.Cells[0].Value.ToString().Trim().Equals(txtOIB.Text.Trim()))
                    {
                        selectedIndex = row.Index;
                        break;
                    }
                }
            }
            if (selectedIndex > -1)
            {
                dgvPodaci.Rows[selectedIndex].Selected = true;
                txtOIB.ReadOnly = true;
                btnDelete.Enabled = true;

            }
        }

        public void obrisiSve()
        {
            txtOIB.Text = "";
            txtIme.Text = "";
            txtPrezime.Text = "";
            txtDatum.Text = "";
            txtBrCip.Text = "";
            txtVisina.Text = "";
            dgvPodaci.ClearSelection();
            txtOIB.ReadOnly = false;
            btnDelete.Enabled = false;
        }

        private void btnObrisi_Click(object sender, EventArgs e)
        {
            obrisiSve();
        }

        private void dgvPodaci_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //OVDJE JE DODATAK POTREBAN ZA 2. ZADATAK
            txtIme.Text = dgvPodaci.SelectedRows[0].Cells[1].Value.ToString();
            txtPrezime.Text = dgvPodaci.SelectedRows[0].Cells[2].Value.ToString();
            txtOIB.Text = dgvPodaci.SelectedRows[0].Cells[0].Value.ToString();
            txtDatum.Text = dgvPodaci.SelectedRows[0].Cells[4].Value.ToString();
            if (dgvPodaci.SelectedRows[0].Cells[3].Value.ToString() == "M") 
                rbM.Checked = true;
            else 
                rbZ.Checked = true;
            txtVisina.Text = dgvPodaci.SelectedRows[0].Cells[5].Value.ToString();
            txtBrCip.Text = dgvPodaci.SelectedRows[0].Cells[6].Value.ToString();
            txtOIB.ReadOnly = true;
            btnDelete.Enabled = true;
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            using (SqlConnection conn = Dbs.GetConnection())
            {
                // OVDJE PIŠETE KOD ZA 3. ZADATAK:


                
                int personID = (int)dgvPodaci.SelectedRows[0].Cells["PersonID"].Value;

                
                using (SqlCommand command = new SqlCommand(SQLDelete, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personID);
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                }






                // NE MIJENJAJ ispod ove linije
                LoadOsobe(conn);
                dgvPodaci.Rows[0].Selected = false;

            }
            btnDelete.Enabled = false;
            obrisiSve(); 

        }

        private void LoadOsobe(SqlConnection conn)
        {
            // 1a - OVDJE KORISTITE DATA ADAPTER 

            //      Koristite SQLSelect komandu dekariranu na početku ove datoteke

            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(SQLSelect, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
        }

    }


}
