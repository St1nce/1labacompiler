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

namespace _1labacompiler
{
    public partial class Compiler : Form
    {
        private string filePath;
        private bool isTextChanged = false;

        //private bool isTextChanged = false;
        public Compiler()
        {
            InitializeComponent();

            // Создаем элемент меню "Файл"
            ToolStripMenuItem fileItem = new ToolStripMenuItem("Файл");

            // Добавляем подменю "Создать" к элементу "Файл"
            fileItem.DropDownItems.Add("Создать");

            // Добавляем элемент "Файл" к главному меню
            //menuStrip4_ItemClicked.Items.Add(fileItem);

            // Подписываемся на событие изменения текста в richTextBox1
            richTextBox1.TextChanged += richTextBox1_TextChanged;

            // Подписываемся на событие закрытия формы
            this.FormClosing += Compiler_FormClosing;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // Устанавливаем флаг isTextChanged в true при изменении текста
            isTextChanged = true;
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Compiler_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Проверяем, были ли несохраненные изменения
            if (isTextChanged)
            {
                // Предлагаем пользователю сохранить изменения
                DialogResult result = MessageBox.Show("Сохранить изменения?", "Предупреждение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Сохраняем файл, если пользователь выбрал "Да"
                    SaveFile();
                }
                else if (result == DialogResult.Cancel)
                {
                    // Отменяем событие закрытия формы, если пользователь выбрал "Отмена"
                    e.Cancel = true;
                }
            }
        }

        private void Open_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.IO.FileStream file;
            OpenFileDialog a = new OpenFileDialog();
            a.InitialDirectory = "c:\\Users\\Lenovo\\Desktop";
            a.Filter = "txt files (*.txt)|*txt|All files (*.*)|*.*";
            if (a.ShowDialog() == DialogResult.OK)
            {
                this.filePath = a.FileName;
                FileStream fs = File.OpenRead(this.filePath);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                richTextBox1.Text = Encoding.Default.GetString(buffer);
                fs.Close();
                isTextChanged = false;
            }
        }

        private void Save_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }
        
        private void SaveAs_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.IO.FileStream file;
            SaveFileDialog a = new SaveFileDialog();
            a.InitialDirectory = "c:\\Users\\Lenovo\\Desktop";
            if (a.ShowDialog() == DialogResult.OK)
            {
                this.filePath = a.FileName;
                FileStream fs = File.Create(this.filePath);
                byte[] buffer = new UTF8Encoding(true).GetBytes(richTextBox1.Text);
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();
            }
        }

        private void Exit_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Cut_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void Copy_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }
        private void Insert_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void Delete_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void SelectAll_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        private void Create_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.IO.FileStream file;
            SaveFileDialog a = new SaveFileDialog();
            a.InitialDirectory = "c:\\Users\\Lenovo\\Desktop";
            if (a.ShowDialog() == DialogResult.OK)
            {
                this.filePath = a.FileName;
                FileStream fs = File.Create(this.filePath);
                fs.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Undo_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void Redo_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Redo();
        }
        private void Undo()
        {
            richTextBox1.Undo();
        }

        private void Redo()
        {
            richTextBox1.Redo();
        }

        /*private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!isTextChanged)
            {
                undoStack.Add(richTextBox1.Text);
                redoStack.Clear();
            }
            isTextChanged = false;
        }*/
        
        private void Helping_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helping form = new Helping();
            form.Show();
        }

        private void About_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
        }
        private void SaveFile()
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    // Если путь к файлу не указан, вызываем SaveFileDialog
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.InitialDirectory = "c:\\Users\\Lenovo\\Desktop";
                    saveFileDialog.Filter = "txt files (*.txt)|*txt|All files (*.*)|*.*";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        filePath = saveFileDialog.FileName;
                    }
                    else
                    {
                        return; // Если пользователь отменил операцию сохранения, выходим из метода
                    }
                }

                // Сохраняем текст в файл
                File.WriteAllText(filePath, richTextBox1.Text);

                // Сбрасываем флаг isTextChanged после успешного сохранения файла
                isTextChanged = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
    } }
