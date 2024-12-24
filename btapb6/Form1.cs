using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using btapb6.Models;

namespace btapb6
{
    public partial class Form1 : Form
    {
        private studentDBContext context;
        private Student SelectedStudent;

        public Form1()
        {
            InitializeComponent();
            dtgSinhVien.CellClick += dtgSinhVien_CellClick;
        }
        private void loadData()
        {
            context = new studentDBContext();
            List<Student> lstStudent = context.Students.ToList();
            List<Faculty> lstFaculty = context.Faculties.ToList();
            fillFacultyComboBox(lstFaculty);
            BlindGrid(lstStudent);

        }
        private void ClearForm()
        {
            txt_mssv.Clear();
            txt_hoten.Clear();
            txt_dtb.Clear();
            cbb_khoa.SelectedIndex = -1;
        }
        private void BlindGrid(List<Student> lstStudent)
        {
            dtgSinhVien.Rows.Clear();
            foreach (var item in lstStudent)
            {
                int index = dtgSinhVien.Rows.Add();
                dtgSinhVien.Rows[index].Cells[0].Value = item.StudentID;
                dtgSinhVien.Rows[index].Cells[1].Value = item.FullName;
                dtgSinhVien.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dtgSinhVien.Rows[index].Cells[3].Value = item.AverageScore;

            }

        }
        private void fillFacultyComboBox(List<Faculty> lstfaculty)
        {
            cbb_khoa.DataSource = lstfaculty;
            cbb_khoa.ValueMember = "FacultyID";
            cbb_khoa.DisplayMember = "FacultyName";
        }
        private void Form1_Load(object sender, EventArgs e)
        {
          loadData();
        }
        private void dtgSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Kiểm tra rằng hàng được nhấp vào là hợp lệ
            {
                DataGridViewRow row = dtgSinhVien.Rows[e.RowIndex];
                string studentId = row.Cells[0].Value.ToString(); // Lấy StudentID từ ô đầu tiên

                // Tìm sinh viên dựa trên StudentID
                SelectedStudent = context.Students.FirstOrDefault(s => s.StudentID == studentId);

                if (SelectedStudent != null) // Kiểm tra nếu sinh viên tồn tại
                {
                    txt_mssv.Text = SelectedStudent.StudentID;
                    txt_hoten.Text = SelectedStudent.FullName;
                    cbb_khoa.SelectedValue = SelectedStudent.FacultyID; // Đặt giá trị cho ComboBox
                    txt_dtb.Text = SelectedStudent.AverageScore.ToString();
                }
            }
        }
        private void btn_Them_Click(object sender, EventArgs e)
        {
            try
            {
                Student student = new Student()
                {
                    StudentID = txt_mssv.Text,
                    FullName = txt_hoten .Text,
                    FacultyID = Convert.ToInt32(cbb_khoa.SelectedValue),
                    AverageScore = float.Parse(txt_dtb.Text)
                };
                context.Students.Add(student);
                context.SaveChanges();
                ClearForm();
                loadData();
                MessageBox.Show("Them thanh cong");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
     private void btn_Sua_Click(object sender, EventArgs e)
        {
            if (SelectedStudent != null) // Kiểm tra sinh viên đã được chọn
            {
                try
                {
                    // Không sửa đổi StudentID
                    // Chỉ cần sửa đổi các thuộc tính khác
                    SelectedStudent.FullName = txt_hoten.Text;
                    SelectedStudent.AverageScore = float.Parse(txt_dtb.Text);
                    SelectedStudent.FacultyID = Convert.ToInt32(cbb_khoa.SelectedValue);

                    // Lưu thay đổi
                    context.SaveChanges();
                    ClearForm();
                    loadData(); // Làm mới dữ liệu
                    MessageBox.Show("Thông tin sinh viên đã được cập nhật thành công!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên trước khi thực hiện sửa đổi.");
            }
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            if (SelectedStudent != null)
            {
                try
                {
                    DialogResult result = MessageBox.Show("Ban có muon xoa svien " + SelectedStudent.FullName + " ?", "Thông báo", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        context.Students.Remove(SelectedStudent);
                        context.SaveChanges();

                        MessageBox.Show("Xoa svien thanh cong.");
                        ClearForm();
                        loadData();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Chon 1 svien de xoa.");
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Ban co muon thoat !", "Xac Nhan", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (rs == DialogResult.Yes)
            {
                this.Close();
            }

        }
    }
}
