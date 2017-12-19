using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.GeoAnalyst;

namespace Wdbs
{
    public partial class main : Form
    {
        public main()
        {

            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            InitializeComponent();
            this.axTOCControl1.SetBuddyControl(this.axMapControl1);
        }

        private void main_Load(object sender, EventArgs e)
        {
            //ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            //InitializeComponent();
            this.axTOCControl1.SetBuddyControl(this.axMapControl1);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog pOpenFileDialog = new OpenFileDialog();
                pOpenFileDialog.CheckFileExists = true;
                pOpenFileDialog.Title = "打开Shape文件";
                pOpenFileDialog.Filter = "Shape文件（*.shp）|*.shp";
                pOpenFileDialog.ShowDialog();

                IWorkspaceFactory pWorkspaceFactory;
                IFeatureWorkspace pFeatureWorkspace;
                IFeatureLayer pFeatureLayer;

                string pFullPath = pOpenFileDialog.FileName;
                if (pFullPath == "") return;

                int pIndex = pFullPath.LastIndexOf("\\");
                string pFilePath = pFullPath.Substring(0, pIndex); //文件路径
                string pFileName = pFullPath.Substring(pIndex + 1); //文件名

                //实例化ShapefileWorkspaceFactory工作空间，打开Shape文件
                pWorkspaceFactory = new ShapefileWorkspaceFactory();
                pFeatureWorkspace = (IFeatureWorkspace)pWorkspaceFactory.OpenFromFile(pFilePath, 0);
                //创建并实例化要素集
                IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(pFileName);
                pFeatureLayer = new FeatureLayer();
                pFeatureLayer.FeatureClass = pFeatureClass;
                pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;

                //ClearAllData();    //新增删除数据

                this.axMapControl1.Map.AddLayer(pFeatureLayer);
                this.axMapControl1.ActiveView.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("图层加载失败！" + ex.Message);
            }
        }

        private void newDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sF1 = new SaveFileDialog();

            sF1.Filter = "gdb files   (*.gdb)|*.gdb";
            sF1.FilterIndex = 2;
            sF1.RestoreDirectory = true;
            String path, name;
            if (sF1.ShowDialog() == DialogResult.OK)
            {
                path = sF1.FileName.Substring(0, sF1.FileName.LastIndexOf("\\"));
                name = sF1.FileName.Substring(sF1.FileName.LastIndexOf("\\") + 1);
                IWorkspace workspace = CreateFileGdbWorkspace(path, name);
                MessageBox.Show("OK,In " + sF1.FileName);
            }
        }
        private IWorkspace CreateFileGdbWorkspace(String path, String name)
        {
            // Instantiate a file geodatabase workspace factory and create a file geodatabase.
            // The Create method returns a workspace name object.
            Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
            IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);
            IWorkspaceName workspaceName = workspaceFactory.Create(path, name, null, 0);

            // Cast the workspace name object to the IName interface and open the workspace.
            IName wname = (IName)workspaceName;
            IWorkspace workspace = (IWorkspace)wname.Open();
            return workspace;
        }
        private void openDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String name;
            FolderBrowserDialog oF1 = new FolderBrowserDialog();
            if (oF1.ShowDialog() == DialogResult.OK)
            {
                name = oF1.SelectedPath.Substring(oF1.SelectedPath.LastIndexOf("\\") + 1);
                listBox1.Items.Add(name);


            }

        }
    }
}
