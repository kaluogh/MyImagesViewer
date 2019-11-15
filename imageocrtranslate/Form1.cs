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

namespace imageocrtranslate
{
    public partial class Form1 : Form
    {
        //private string path;
        //private string currentRootPath;
        //private string currentFilePath;
        //private TreeNode currentRootNode;
        private TreeNode currentSelectNode;

        public Form1()
        {
            
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(textBox1.Text))
            {
                DirectoryInfo di = new DirectoryInfo(textBox1.Text);
                TreeNode tn = new TreeNode(textBox1.Text);
                tn.Tag = "Root";
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(tn);
                currentSelectNode = tn;
            }
            else {
                textBox1.Text = @"C:\Users\admin\Desktop\工作";
                DirectoryInfo di = new DirectoryInfo(textBox1.Text);
                TreeNode tn = new TreeNode(textBox1.Text);
                tn.Tag = "Root";
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(tn);
                currentSelectNode = tn;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            createDrivers();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
        }

        private void createDrivers()
        {
            DirectoryInfo di;
            TreeNode tn;
            //string s;
            foreach (string ss in Environment.GetLogicalDrives())
            {
                /**
                 * ss的值形式如下：
                 * C:\
                 * D:\
                 * E:\
                 * F:\
                 * I:\
                 */
                di = new DirectoryInfo(ss); //生成路径
                if (di.Exists) //如果当前路径存在
                {
                    //s = ss.Substring(0, ss.IndexOf("\\")); //去掉路径的 \ 得到如：C:
                    tn = new TreeNode(ss); //设置结点的值
                    tn.Tag = "Root";
                    treeView1.Nodes.Add(tn); //把结点加入到TreeView中
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode tn = e.Node; //得到发生选择事件的结点
            string currentPath = tn.FullPath;
            currentSelectNode = tn;
            //textBox1.Text = currentPath; //在左上角的文本框显示完整的路径
            if (tn.Tag.ToString() == "File") //如果当前结点是文件
            {
                string ext = currentPath.Substring(currentPath.LastIndexOf(".") + 1); //得到文件的后缀
                ext = ext.ToLower();
                if (ext == "ico" || ext == "gif" || ext == "jpg" || ext == "png" || ext == "bmp") {
                    //如果是图片，设置到右边的pictureBox
                    try
                    {
                        pictureBox1.Image = Image.FromFile(currentPath);
                        Size tempSize = pictureBox1.Image.Size;
                        pictureBox1.Width = panel1.Width * 95 / 100;
                        pictureBox1.Height = pictureBox1.Width * tempSize.Height / tempSize.Width;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else {
                    //否则使用系统默认的工具打开该文件
                    //System.Diagnostics.Process.Start(path);
                }
            }
            else
            {
                //反之当前结点是文件夹
                getSubContents(tn);//得到当前结点的全部内容
            }
        }

        private void getSubContents(TreeNode parent)
        {
            DirectoryInfo di;
            TreeNode tn;
            di = new DirectoryInfo(parent.FullPath + "\\"); //得到路径
            if (!di.Exists) return;             //路径不存在，直接返回
            foreach (DirectoryInfo d in di.GetDirectories()) //该路径下面的所有 文件夹
            {
                tn = new TreeNode(d.Name);
                tn.Tag = "Directory";
                parent.Nodes.Add(tn); //将该节点添加到父节点
            }
            FileInfo[] fileList = di.GetFiles();
            Array.Sort<FileInfo>(fileList, (s1, s2) => s1.Name.CompareTo(s2.Name));
            foreach (FileInfo f in fileList)
            {
                
                string temp = f.Extension;
                if (!String.IsNullOrWhiteSpace(temp)) {
                    if (temp == ".ico" || temp == ".gif" || temp == ".jpg" || temp == ".png" || temp == ".bmp")
                    {
                        tn = new TreeNode(f.Name); //该路径下的所有 文件
                        tn.Tag = "File";
                        parent.Nodes.Add(tn);
                    }
                }
                
            }
        }

        //prev button
        private void button2_Click(object sender, EventArgs e)
        {
            if (currentSelectNode.PrevNode != null) {
                currentSelectNode = currentSelectNode.PrevNode;
                treeView1.SelectedNode = currentSelectNode;
                if (currentSelectNode.Tag.ToString() == "File") //如果当前结点是文件
                {
                    try
                    {
                        pictureBox1.Image = Image.FromFile(currentSelectNode.FullPath);
                        Size tempSize = pictureBox1.Image.Size;
                        pictureBox1.Width = panel1.Width * 95 / 100;
                        pictureBox1.Height = pictureBox1.Width * tempSize.Height / tempSize.Width;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else //反之当前结点是文件夹
                {
                    getSubContents(currentSelectNode);//得到当前结点的全部内容
                }

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (currentSelectNode.NextNode != null)
            {
                currentSelectNode = currentSelectNode.NextNode;
                treeView1.SelectedNode = currentSelectNode;
                if (currentSelectNode.Tag.ToString() == "File") //如果当前结点是文件
                {
                    try
                    {
                        pictureBox1.Image = Image.FromFile(currentSelectNode.FullPath);
                        Size tempSize = pictureBox1.Image.Size;
                        pictureBox1.Width = panel1.Width * 95 / 100;
                        pictureBox1.Height = pictureBox1.Width * tempSize.Height / tempSize.Width;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else //反之当前结点是文件夹
                {
                    getSubContents(currentSelectNode);//得到当前结点的全部内容
                }

            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int tempWidth = int.Parse(textBox2.Text);
                if (tempWidth > 600 && tempWidth < 1800) {
                    panel1.Width = tempWidth;
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "abc\n123";
        }
    }
}
