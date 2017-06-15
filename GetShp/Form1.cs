using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gdal = OSGeo.GDAL.Gdal;
using Ogr = OSGeo.OGR.Ogr;
using OSGeo.OGR;

namespace GetShp
{
    public partial class GetShp : Form
    {
        private string mFileName;
        private string mOutPath;

        public GetShp()
        {
            InitializeComponent();
            mFileName = string.Empty;
            mOutPath = string.Empty;
            //Control.CheckForIllegalCrossThreadCalls = false;
            Tips.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog(); 
            openFileDialog1.Filter = "Access文件(*.mdb)|*.mdb|所有文件(*.*)|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                mFileName = openFileDialog1.FileName;
                InPutPath.Clear();
                InPutPath.AppendText(mFileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                mOutPath = dlg.SelectedPath.ToString();
                OutPutPath.Clear();
                OutPutPath.AppendText(mOutPath);
            }
        }
        

        private void OpenBT_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Explorer.exe", mOutPath);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Explorer.exe", mFileName);
        }

        private void runBT_Click(object sender, EventArgs e)
        {
            if (mFileName.Length == 0 || mOutPath.Length == 0)
            {
                MessageBox.Show("请选择有效的文件和路径", "提示");
            }
            else 
            {
                Tips.Show();
                button1.Enabled = false;
                button2.Enabled = false;
                button4.Enabled = false;
                OpenBT.Enabled = false;
                runBT.Enabled = false;
                new System.Threading.Thread(new System.Threading.ThreadStart(runFunc)).Start();
            }
        }
        private void runFunc()
        {
            Ogr.RegisterAll();
            Gdal.SetConfigOption("SHAPE_ENCODING", "CP936");

            string strInfo = string.Empty;
            //打开数据  
            DataSource ds = Ogr.Open(mFileName, 0);
            if (ds == null)
            {
                strInfo += string.Format("打开文件【{0}】失败！\n", mFileName);
                return;
            }
            strInfo += string.Format("打开文件【{0}】成功！\n", mFileName);

            CGetShp mdbToShp = new CGetShp(mFileName, mOutPath);
            // 获取该数据源中的图层个数，一般shp数据图层只有一个，如果是mdb、dxf等图层就会有多个  
            int iLayerCount = ds.GetLayerCount();
            strInfo += string.Format("个人地理数据库共包含{0}个图层！\n", iLayerCount);
            for (int i = 0; i < iLayerCount; i++)
            {
                // 获取第一个图层  
                Layer oLayer = ds.GetLayerByIndex(i);

                if (oLayer == null)
                {
                    strInfo += string.Format("获取第{0}个图层失败！\n", i);
                    return;
                }
                strInfo += string.Format("第{0}个图层名称：{1}\n", i, oLayer.GetName());
                //若当前图层为向量数据
                if (oLayer.GetName().Contains("LINE"))
                {
                    //调用方法获取向量信息，并传入想应的点图层名以方便查询
                    mdbToShp.readLine(ref oLayer, oLayer.GetName().Replace("LINE", "POINT"));
                    //break;
                }
                else
                {
                    mdbToShp.readPoint(ref oLayer);
                    //break;
                    //mdbToShp.test(ref oLayer);
                }

                SetTextMessage(100 * (i + 1) / iLayerCount);
            }
            
        }
        private delegate void SetPos(int ipos);
        private void SetTextMessage(int ipos)
        {
            if (this.InvokeRequired)
            {
                SetPos setpos = new SetPos(SetTextMessage);
                this.Invoke(setpos, new object[] { ipos });
            }
            else
            {
                this.progressBar1.Value = Convert.ToInt32(ipos);
                if (ipos == 100)
                {
                    Tips.Text = ("解析完成");
                    button4.Enabled = true;
                    OpenBT.Enabled = true;
                }
            }
        }

        private void InPutPath_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            mFileName = s[0];
            InPutPath.Clear();
            InPutPath.AppendText(mFileName);
        }

        private void InPutPath_DragEnter_1(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
    }
}
