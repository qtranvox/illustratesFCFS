using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FCFS
{
    public partial class Form1 : Form
    {
        public int[] mau;
        public int n, time, tot, step, next;
        public float avgrt, avgtat, avgwt, tottat, totwt, totrt;

        public string[] stp;

        public struct Task
        {
            public int id;
            public int arr;
            public int bur;
            public int tat;
            public int rt;
            public int wt;
            public int star;
            public int finish;
            public Color clor;
        }

        private void btnAddProcess_Click(object sender, EventArgs e)
        {
            if (txtArr.Text == "" || txtBru.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập thời gian vào hoặc thời gian CPU của một tiến trình");
            }
            else
            {
                StreamWriter writer = new StreamWriter("FCFS.txt", true);
                writer.WriteLine(txtArr.Text.ToString() + " " + txtBru.Text.ToString());
                writer.Close();
                txtArr.Clear(); txtBru.Clear();

                init();
                FileStream fi = new FileStream("FCFS.txt", FileMode.Open, FileAccess.Read);
                StreamReader rd = new StreamReader(fi);
                int red = 255, green = 0, blue = 0;
                Random rad = new Random();
                string line = null;
                string[] tg = null;
                n = 0;
                while ((line = rd.ReadLine()) != null)
                {
                    tg = new Regex(" ").Split(line);
                    task[n].id = n;
                    task[n].arr = int.Parse(tg[0].ToString());
                    task[n].bur = int.Parse(tg[1].ToString());
                    task[n].tat = 0; task[n].wt = 0; task[n].rt = 0;
                    task[n].clor = Color.FromArgb(red, green, blue);
                    red += 40; red %= 255;
                    blue += 70; blue %= 255;
                    green += 100; green %= 255;
                    n++;
                }
                fi.Close();
                process.Items.Clear();
                process.Items.Add(string.Format("Thời gian:0"));
                int i;
                for (i = 0; i < n; i++)
                {
                    process.Items.Add(String.Format("Tiến trình {0}: Thời điểm vào RL: {1}. Thời gian CPU: {2}", task[i].id + 1, task[i].arr, task[i].bur));
                }
                Array.Resize(ref task, n);
                Array.Sort(task, (a, b) => a.arr.CompareTo(b.arr));

            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            init();
            step = 0;
            process.Items.Clear();
            Graphics g = CreateGraphics();
            g.Clear(Color.CadetBlue);
            art.Text = "";
            awt.Text = "";
            atat.Text = "";
            File.WriteAllText("FCFS.txt", string.Empty);
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            init();
            FileStream fi = new FileStream("TextFile.txt", FileMode.Open, FileAccess.Read);
            StreamReader rd = new StreamReader(fi);
            int red = 255, green = 0, blue = 0;
            Random rad = new Random();
            string line = null;
            string[] tg = null;
            n = 0;
            while ((line = rd.ReadLine()) != null)
            {
                tg = new Regex(" ").Split(line);
                task[n].id = n;
                task[n].arr = int.Parse(tg[0].ToString());
                task[n].bur = int.Parse(tg[1].ToString());
                task[n].tat = 0; task[n].wt = 0; task[n].rt = 0;
                task[n].clor = Color.FromArgb(red, green, blue);
                red += 40; red %= 255;
                blue += 70; blue %= 255;
                green += 100; green %= 255;
                n++;
            }
            fi.Close();
            process.Items.Clear();
            process.Items.Add(string.Format("Thời gian:0"));
            int i;
            for (i = 0; i < n; i++)
            {
                process.Items.Add(String.Format("Tiến trình {0}: Thời điểm vào RL: {1}. Thời gian CPU: {2}", task[i].id + 1, task[i].arr, task[i].bur));
            }
            Array.Resize(ref task, n);
            Array.Sort(task, (a, b) => a.arr.CompareTo(b.arr));
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            init1();
            FCFS();
            Ghi();
            avgrt = totrt / n;
            avgwt = totwt / n;
            avgtat = tottat / n;
            art.Text = avgrt.ToString();
            awt.Text = avgwt.ToString();
            atat.Text = avgtat.ToString();
        }

        


        public Task[] task;
        public Form1()
        {
            InitializeComponent();
        }

        //FCFS
        void FCFS()
        {
            int i;
            for (i = 0; i < n; i++)
            {
                if (time < task[i].arr)
                    time = task[i].arr;

                task[i].star = time;
                task[i].rt = task[i].star - task[i].arr;
                task[i].wt = task[i].star - task[i].arr;
                task[i].finish = task[i].star + task[i].bur;
                task[i].tat = task[i].finish - task[i].arr;
                time += task[i].bur;
                while (step < time)
                {
                    mau[step] = i;
                    step++;
                }
            }
            for (i = 0; i < n; i++)
            {
                tottat += task[i].tat;
                totwt += task[i].wt;
                totrt += task[i].rt;
            }
        }
        
        
        void init1()
        {
            time = 0;
            tot = 0; tottat = 0; totwt = 0; totrt = 0; next = n;
        }

        void init()
        {
            mau = new int[1000];
            task = new Task[1000];
            step = 0;
            n = 0;
            init1();
        }
        void Ghi()
        {
            int i;
            Graphics g = CreateGraphics();
            int squareWidth = 30;
            Point squareStartLocation = new Point(gc.Location.X + gc.Width + 10, gc.Location.Y - squareWidth / 3);
            for (i = 0; i < step; i++)
            {
                Rectangle rect = new Rectangle(squareStartLocation.X + i * squareWidth, squareStartLocation.Y, squareWidth, squareWidth);
                SolidBrush brush = new SolidBrush(task[mau[i]].clor);
                g.DrawRectangle(new Pen(brush), rect);
                g.DrawString(i.ToString(), Font, Brushes.Black, squareStartLocation.X + i * squareWidth - 4, tu.Location.Y);
            }
            g.DrawString(step.ToString(), Font, Brushes.Black, squareStartLocation.X + i * squareWidth - 4, tu.Location.Y);
            process.Items.Clear();
            process.Items.Add(String.Format("Thời gian: {0}", time));
            for (i = 0; i < n; i++)
                process.Items.Add(String.Format("Tiến trình {0}: Thời gian đáp ứng: {1}. Thời gian chờ: {2}. Thời gian xoay vòng: {3}", task[i].id + 1, task[i].rt, task[i].wt, task[i].tat));

            int t = 0;
            for (i = 0; i < n; i++)
            {
                for (int j = 0; j < task[i].bur; j++)
                {
                    g.DrawString("P" + (task[i].id + 1), Font, Brushes.Black, squareStartLocation.X + t * squareWidth + 10, squareStartLocation.Y + 10);
                    t++;
                }
            }
        }
    }
}
