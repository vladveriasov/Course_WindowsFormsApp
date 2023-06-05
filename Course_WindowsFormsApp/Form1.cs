using Excel;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 

namespace Course_WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataSet ds;

        Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
        Microsoft.Office.Interop.Excel._Workbook workbook;
        Microsoft.Office.Interop.Excel._Worksheet worksheet;
       
        string filePath = @"C:\Users\VladVer\source\repos\Course_WindowsFormsApp\DataBase";
        int indexRow;
        Bitmap bitmap;

        private void buttonExit_Click(object sender, EventArgs e)
        {
           Exit();
        }
        private void Exit()
        {
            DialogResult iExit;

            iExit = MessageBox.Show("Confirm if you want to exit", "Save?", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (iExit == DialogResult.Yes)
            {
                workbook.Close();
                app.Quit();
                ReleaseObject(worksheet);
                ReleaseObject(workbook);
                ReleaseObject(app);
                System.Windows.Forms.Application.Exit();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void buttonAddNew_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(txtName.Text, txtProduct.Text, txtPrice.Text, txtLotSize.Text,
             txtPayment.Text, txtShipment.Text, txtNote.Text, txtAddress.Text, txtMobileNo.Text);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            Delete();
        }
        public void Delete()
        {
            foreach(DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.RemoveAt(item.Index);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
        
        public void Reset()
        {
            //==================Clears All Data in TextBox============

            foreach (var c in this.Controls)
            {
                if(c is System.Windows.Forms.TextBox)
                {
                    ((System.Windows.Forms.TextBox)c).Text = String.Empty;
                }
            }

            //==================Clears the DataGridView=======================

            int numRows = dataGridView1.Rows.Count;
            for(int i = 0; i < numRows; i++)
            {
                try
                {
                    int max = dataGridView1.Rows.Count - 1;
                    dataGridView1.Rows.Remove(dataGridView1.Rows[max]);
                }
                catch(Exception exe)
                {
                    MessageBox.Show("All rows are to be deleted" + exe, "DataGridView Delete",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                } 
            }
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reset();
        }
        
        private void buttonPrint_Click(object sender, EventArgs e)
        {
            Print();
        }
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void Print()
        {
            int height = dataGridView1.Height;
            dataGridView1.Height = dataGridView1.RowCount * dataGridView1.RowTemplate.Height * 2;
            bitmap = new Bitmap(dataGridView1.Width, dataGridView1.Height);
            dataGridView1.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0, 0, dataGridView1.Width, dataGridView1.Height));
            printPreviewDialog1.PrintPreviewControl.Zoom = 1;
            printPreviewDialog1.ShowDialog();
            dataGridView1.Height = height;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, 0, 0);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            //SaveToExcel(@"C:\Users\VladVer\source\repos\Course_WindowsFormsApp\DataBase");
            Save(filePath);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SaveToExcel(filePath);
            Save(filePath);
        }

        //private void Save()
        //{
        //    // Очистка данных в листе Excel
        //    worksheet.Cells.ClearContents();

        //    // Сохранение данных из DataGridView в лист Excel
        //    for (int i = 0; i < dataGridView1.Rows.Count; i++)
        //    {
        //        for (int j = 0; j < dataGridView1.Columns.Count; j++)
        //        {
        //            worksheet.Cells[i + 1, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
        //        }
        //    }

        //    // Сохранение изменений в файл Excel
        //    workbook.Save();

        //    // Закрытие рабочей книги
        //    workbook.Close();

        //    // Закрытие приложения Excel
        //    app.Quit();

        //    // Освобождение ресурсов
        //    ReleaseObject(worksheet);
        //    ReleaseObject(workbook);
        //    ReleaseObject(app);
        //}

        private void Save(string filePath)
        {
            // Очистка данных в листе Excel
            worksheet.Cells.ClearContents();

            // Получение диапазона данных для записи
            Microsoft.Office.Interop.Excel.Range range = worksheet.Cells[1, 1].Resize[dataGridView1.Rows.Count, dataGridView1.Columns.Count];

            // Запись данных из DataGridView в ячейки листа Excel
            object[,] data = new object[dataGridView1.Rows.Count, dataGridView1.Columns.Count];
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    data[i, j] = dataGridView1.Rows[i].Cells[j].Value;
                }
            }
            range.Value = data;

            // Сохранение изменений в файл Excel
            workbook.SaveAs(filePath);

            // Закрытие рабочей книги
            workbook.Close();

            // Закрытие приложения Excel
            app.Quit();

            // Освобождение ресурсов
            ReleaseObject(range);
            ReleaseObject(worksheet);
            ReleaseObject(workbook);
            ReleaseObject(app);
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexRow = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[indexRow];

            txtName.Text = row.Cells[0].Value.ToString();
            txtProduct.Text = row.Cells[1].Value.ToString();
            txtPrice.Text = row.Cells[2].Value.ToString();
            txtLotSize.Text = row.Cells[3].Value.ToString();
            txtPayment.Text = row.Cells[4].Value.ToString();
            txtShipment .Text = row.Cells[5].Value.ToString();
            txtMobileNo.Text = row.Cells[6].Value.ToString();
            txtAddress.Text = row.Cells[7].Value.ToString();
            txtNote.Text = row.Cells[8].Value.ToString();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            DataGridViewRow newDataRow = dataGridView1.Rows[indexRow];

            newDataRow.Cells[0].Value = txtName.Text;
            newDataRow.Cells[1].Value = txtProduct.Text;
            newDataRow.Cells[2].Value = txtPrice.Text;
            newDataRow.Cells[3].Value = txtLotSize.Text;
            newDataRow.Cells[4].Value = txtPayment.Text;
            newDataRow.Cells[5].Value = txtShipment.Text;
            newDataRow.Cells[6].Value = txtMobileNo.Text;
            newDataRow.Cells[7].Value = txtAddress.Text;
            newDataRow.Cells[8].Value = txtNote.Text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            workbook = app.Workbooks.Open(filePath);
            worksheet = workbook.Sheets["Sheet1"];

            LoadDataFromExcel();

            //using (OpenFileDialog dialog = new OpenFileDialog() 
            //{ Filter = "Excel Workbook|*.xlsx", ValidateNames = true })
            //{
            //    if(dialog.ShowDialog() == DialogResult.OK)
            //    {
            //        FileStream fileStream = File.Open(dialog.FileName, FileMode.Open, FileAccess.Read);
            //        IExcelDataReader reader = ExcelReaderFactory.CreateBinaryReader(fileStream);
            //        reader.IsFirstRowAsColumnNames = true;
            //        ds = reader.AsDataSet();
            //        dataGridView1.DataSource = ds.Tables[0];
            //    }
            //}
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Закрытие и освобождение ресурсов приложения Excel при закрытии формы
            Exit();
        }

        private void LoadDataFromExcel()
        {
            // Получение диапазона данных из листа Excel
            Microsoft.Office.Interop.Excel.Range range = worksheet.UsedRange;

            // Очистка данных в DataGridView
            dataGridView1.Rows.Clear();

            // Чтение данных из листа Excel и заполнение DataGridView
            for (int row = 1; row <= range.Rows.Count; row++)
            {
                List<string> rowData = new List<string>();
                for (int column = 1; column <= range.Columns.Count; column++)
                {
                    if (range.Cells[row, column] != null && range.Cells[row, column].Value2 != null)
                    {
                        string cellValue = range.Cells[row, column].Value2.ToString();
                        rowData.Add(cellValue);
                    }
                    else
                    {
                        rowData.Add("");
                    }
                }
                dataGridView1.Rows.Add(rowData.ToArray());
            }
        }

        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Error releasing object: " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
        
    }
}
