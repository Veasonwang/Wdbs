using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
///////////////////////////////////////////////////////
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
using System.Collections;


using System.Diagnostics;
using System.IO;
using stdole;

namespace Wdbs
{
    public partial class mainwindows : Form
    {
        #region class members
        private IMapControl3 m_mapControl = null;
        private IPageLayoutControl2 m_pageLayoutControl = null;  
        private ITool m_mapActiveTool = null;
        private ITool m_pageLayoutActiveTool = null;
        private bool m_IsMapCtrlactive = true;
        private ControlsSynchronizer m_controlsSynchronizer;
        private ArrayList m_frameworkControls = null;
        #endregion  
        #region//定义右键菜单
        private IToolbarMenu m_TocMenuMap = null;
        private IToolbarMenu m_TocMenuLayer = null;
        private IToolbarMenu m_MapPopMenu = null;
        #endregion
        bool m_bIsMapCtrActive = true;
        public mainwindows()
        {

            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            InitializeComponent();
            this.axTOCControl1.SetBuddyControl(this.axMapControl1);
        }

        private void main_Load(object sender, EventArgs e)
        {
            //ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            //InitializeComponent();
            //this.axTOCControl1.SetBuddyControl(this.axMapControl1);
            #region
            //添加TOC右键菜单项
            AddTocMapPopMenu();
            AddTocLayerPopMenu();

            //添加MapControl右键菜单项
            AddMapControlPopMenu();

            #endregion

            m_mapControl = (IMapControl3)this.axMapControl1.Object;
            m_pageLayoutControl = (IPageLayoutControl2)this.axPageLayoutControl1.Object;

            //初始化controls synchronization calss  
            m_controlsSynchronizer = new
            ControlsSynchronizer(m_mapControl, m_pageLayoutControl);
            //把MapControl和PageLayoutControl绑定起来(两个都指向同一个Map),然后设置MapControl为活动的Control  
            m_controlsSynchronizer.BindControls(true);
            //为了在切换MapControl和PageLayoutControl视图同步，要添加Framework Control  
            m_controlsSynchronizer.AddFrameworkControl(axToolbarControl1.Object);
            m_controlsSynchronizer.AddFrameworkControl(this.axTOCControl1.Object);  
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #region  //添加TOC右键菜单项
        //添加TOC右键菜单项
        private void AddTocMapPopMenu()
        {
            m_TocMenuMap = new ToolbarMenu();
            //Get a toolbar item
            //Add pre-defined control commands to the ToolbarControl
            m_TocMenuMap.AddItem(new New_layout(), -1, 0, false, esriCommandStyles.esriCommandStyleTextOnly);
            m_TocMenuMap.SetHook(this.axMapControl1);
        }

        //添加TOCLayer右键菜单项
        private void AddTocLayerPopMenu()
        {
            //Add custom commands to the map menu
            m_TocMenuLayer = new ToolbarMenu();
            m_TocMenuLayer.AddItem(new remover(), -1, 0, false, esriCommandStyles.esriCommandStyleTextOnly);
            m_TocMenuLayer.AddItem(new newfeature(), 1, 1, true, esriCommandStyles.esriCommandStyleTextOnly);
            //m_TocMenuLayer.AddItem(new ScaleThresholds(), 2, 2, false, esriCommandStyles.esriCommandStyleTextOnly);
            //m_TocMenuLayer.AddItem(new ScaleThresholds(), 3, 3, false, esriCommandStyles.esriCommandStyleTextOnly);
            //m_TocMenuLayer.AddItem(new LayerSelectable(), 1, 4, true, esriCommandStyles.esriCommandStyleTextOnly);
            //m_TocMenuLayer.AddItem(new LayerSelectable(), 2, 5, false, esriCommandStyles.esriCommandStyleTextOnly);
            //m_TocMenuLayer.AddItem(new ZoomToLayer(), -1, 6, true, esriCommandStyles.esriCommandStyleTextOnly);
            m_TocMenuLayer.SetHook(this.axMapControl1);

        }
        #endregion

        private void AddMapControlPopMenu()
        {
            m_MapPopMenu = new ToolbarMenu();
            //this.axToolbarControl1.AddItem("esriControls.ControlsMapPanTool", -1, 0, false, 0, esriCommandStyles.esriCommandStyleIconOnly);
            //m_MapPopMenu.AddItem(new MapPan(), -1, -1, false, esriCommandStyles.esriCommandStyleIconAndText);
            m_MapPopMenu.AddItem(new newfeature(), -1, 0, false, esriCommandStyles.esriCommandStyleIconAndText);
            m_MapPopMenu.SetHook(this.axMapControl1); //axMapMain右键菜单
        }

        //Add popmenu



        private void axTOCControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.ITOCControlEvents_OnMouseDownEvent e)
        {
            #region //Add Popmenu on TOCControl
            
            try
            {

                if (e.button == 2) //右键
                {
                    esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
                    IBasicMap map = null; ILayer layer = null;
                    object other = null; object index = null;
                    //Determine what kind of item is selected
                    this.axTOCControl1.HitTest(e.x, e.y, ref item, ref map, ref layer, ref other, ref index);
                    //Ensure the item gets selected 
                    if (item == esriTOCControlItem.esriTOCControlItemMap)
                        this.axTOCControl1.SelectItem(map, null);
                    else
                        this.axTOCControl1.SelectItem(layer, null);

                    //Set the layer into the CustomProperty (this is used by the custom layer commands)      
                    this.axMapControl1.CustomProperty = layer;
                    //Popup the correct context menu
                    if (item == esriTOCControlItem.esriTOCControlItemMap) m_TocMenuMap.PopupMenu(e.x, e.y, this.axTOCControl1.hWnd);
                    if (item == esriTOCControlItem.esriTOCControlItemLayer) m_TocMenuLayer.PopupMenu(e.x, e.y, this.axTOCControl1.hWnd);

                }
            }
            catch
            {
                
            }


        }


            #endregion
        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (e.button == 2)
            {
                m_MapPopMenu.PopupMenu(e.x, e.y, this.axMapControl1.hWnd);
            }
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
                m_controlsSynchronizer.ReplaceMap(this.axMapControl1.Map); 
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

        private void 空间数据查询与统计ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 属性查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //新创建属性查询窗体
            FormQueryByAttribute formQueryByAttribute = new FormQueryByAttribute();
            //将当前主窗体中MapControl控件中的Map对象赋值给FormQueryByAttribute窗体的CurrentMap属性
            formQueryByAttribute.CurrentMap = this.axMapControl1.Map;
            //显示属性查询窗体
            formQueryByAttribute.Show();
        }

        private void 空间查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormQueryBySpace FQBS1 = new FormQueryBySpace();
            FQBS1.CurrentMap = this.axMapControl1.Map;
            FQBS1.Show();
        }

        private void 统计查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormQueryByStatistics FQsta1 = new FormQueryByStatistics();
            FQsta1.CurrentMap = this.axMapControl1.Map;
            FQsta1.Show();
        }
        #region 属性页切换事件
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            //if (tabControl1.SelectedTab == this.tabPage1)
            //{
            //    //当MapControl的Map对象被替换后，需要重新激活MapControl  
            //    ActiveMapControl();
            //}
            //else
            //{
            //    //当PageLayoutControl的PageLayout对象被替换后，需要重新激活PageLayoutControl  
            //    ActivePageLayoutControl();
            //}
            if (tabControl1.SelectedTab == this.tabPage1)
            {
                //数据视图
                //使排版视图功能无效
                //this.MapFeatureToolStripMenuItem1.Enabled = false;
                //this.MapGridToolStripMenuItem.Enabled = false;
                //this.MapTamplateToolStripMenuItem.Enabled = false;
                //this.ax
                ////axMapControl1.Map = axPageLayoutControl1.ActiveView.FocusMap;
                //this.axTOCControl1.SetBuddyControl(this.axMapControl1);
                //this.axTOCControl1.Update();
                //ActiveMapControl();
                //m_bIsMapCtrActive = true;
                ActivateMap();
            }
            else
            {
                //排版视图
                //使数据视图相关功能无效
                //this.MapFeatureToolStripMenuItem1.Enabled = true;
                //this.MapGridToolStripMenuItem.Enabled = true;
                //this.MapTamplateToolStripMenuItem.Enabled = true;
                //axMapControl1.Map = axPageLayoutControl1.ActiveView.FocusMap;
                //this.axTOCControl1.SetBuddyControl(this.axPageLayoutControl1);
                //this.axTOCControl1.Update();
                //ActivePageLayoutControl();
                
                //m_bIsMapCtrActive = false;
                ActivatePageLayout();
            }

        }
        #endregion

        #region 同步属性页切换
     
        public void ActiveMapControl()
        {
            try
            {
                axPageLayoutControl1.ActiveView.Deactivate();
                //if (!m_MapControl.ActiveView.IsActive())    //如果在激活状态下重复激活，程序会崩溃  
                axMapControl1.ActiveView.Activate(axMapControl1.hWnd);    //会触发ActiveView的ContentsChanged事件  

                m_bIsMapCtrActive = true;
            }
            catch (System.Exception)
            {

            }
        }

        public void ActivePageLayoutControl()
        {
            try
            {
                axMapControl1.ActiveView.Deactivate();
                axPageLayoutControl1.ActiveView.Activate(axPageLayoutControl1.hWnd);
                m_bIsMapCtrActive = false;
            }
            catch (System.Exception)
            {

            }
        }  
        #endregion



        #region 同步处理函数
        #region Methods
        /// <summary>  
        /// 激活MapControl并解除the PagleLayoutControl  
        /// </summary>  
        public void ActivateMap()
        {
            try
            {
                if (m_pageLayoutControl == null || axMapControl1 == null)
                    throw new Exception("ControlsSynchronizer::ActivateMap:\r\nEither MapControl or PageLayoutControl are not initialized!");

                //缓存当前PageLayout的CurrentTool  
                if (m_pageLayoutControl.CurrentTool != null) m_pageLayoutActiveTool = m_pageLayoutControl.CurrentTool;

                //解除PagleLayout  
                m_pageLayoutControl.ActiveView.Deactivate();

                //激活MapControl  
                axMapControl1.ActiveView.Activate(axMapControl1.hWnd);

                //将之前MapControl最后使用的tool，作为活动的tool，赋给MapControl的CurrentTool  
                if (m_mapActiveTool != null) axMapControl1.CurrentTool = m_mapActiveTool;

                m_IsMapCtrlactive = true;

                //为每一个的framework controls,设置Buddy control为MapControl  
                this.SetBuddies(axMapControl1.Object);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ControlsSynchronizer::ActivateMap:\r\n{0}", ex.Message));
            }
        }

        /// <summary>  
        /// 激活PagleLayoutControl并解除MapCotrol  
        /// </summary>  
        public void ActivatePageLayout()
        {
            try
            {
                if (m_pageLayoutControl == null || axMapControl1 == null)
                    throw new Exception("ControlsSynchronizer::ActivatePageLayout:\r\nEither MapControl or PageLayoutControl are not initialized!");

                //缓存当前MapControl的CurrentTool  
                if (axMapControl1.CurrentTool != null) m_mapActiveTool = axMapControl1.CurrentTool;

                //解除MapControl  
                axMapControl1.ActiveView.Deactivate();

                //激活PageLayoutControl  
                m_pageLayoutControl.ActiveView.Activate(m_pageLayoutControl.hWnd);

                //将之前PageLayoutControl最后使用的tool，作为活动的tool，赋给PageLayoutControl的CurrentTool  
                if (m_pageLayoutActiveTool != null) m_pageLayoutControl.CurrentTool = m_pageLayoutActiveTool;

                m_IsMapCtrlactive = false;

                //为每一个的framework controls,设置Buddy control为PageLayoutControl  
                this.SetBuddies(m_pageLayoutControl.Object);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ControlsSynchronizer::ActivatePageLayout:\r\n{0}", ex.Message));
            }
        }

        /// <summary>  
        /// 给予一个地图, 置换PageLayoutControl和MapControl的focus map  
        /// </summary>  
        /// <param name="newMap"></param>  
        public void ReplaceMap(IMap newMap)
        {
            if (newMap == null)
                throw new Exception("ControlsSynchronizer::ReplaceMap:\r\nNew map for replacement is not initialized!");

            if (m_pageLayoutControl == null || axMapControl1 == null)
                throw new Exception("ControlsSynchronizer::ReplaceMap:\r\nEither MapControl or PageLayoutControl are not initialized!");

            //create a new instance of IMaps collection which is needed by the PageLayout  
            //创建一个PageLayout需要用到的,新的IMaps collection的实例  
            IMaps maps = new Maps();
            //add the new map to the Maps collection  
            //把新的地图加到Maps collection里头去  
            maps.Add(newMap);

            bool bIsMapActive = m_IsMapCtrlactive;

            //call replace map on the PageLayout in order to replace the focus map  
            //we must call ActivatePageLayout, since it is the control we call 'ReplaceMaps'  
            //调用PageLayout的replace map来置换focus map  
            //我们必须调用ActivatePageLayout,因为它是那个我们可以调用"ReplaceMaps"的Control  
            this.ActivatePageLayout();
            m_pageLayoutControl.PageLayout.ReplaceMaps(maps);

            //assign the new map to the MapControl  
            //把新的地图赋给MapControl  
            axMapControl1.Map = newMap;

            //reset the active tools  
            //重设active tools  
            m_pageLayoutActiveTool = null;
            m_mapActiveTool = null;

            //make sure that the last active control is activated  
            //确认之前活动的control被激活  
            if (bIsMapActive)
            {
                this.ActivateMap();
                axMapControl1.ActiveView.Refresh();
            }
            else
            {
                this.ActivatePageLayout();
                m_pageLayoutControl.ActiveView.Refresh();
            }
        }

        /// <summary>  
        /// bind the MapControl and PageLayoutControl together by assigning a new joint focus map  
        /// 指定共同的Map来把MapControl和PageLayoutControl绑在一起  
        /// </summary>  
        /// <param name="mapControl"></param>  
        /// <param name="pageLayoutControl"></param>  
        /// <param name="activateMapFirst">true if the MapControl supposed to be activated first,如果MapControl被首先激活,则为true</param>  
        public void BindControls(IMapControl3 mapControl, IPageLayoutControl2 pageLayoutControl, bool activateMapFirst)
        {
            if (mapControl == null || pageLayoutControl == null)
                throw new Exception("ControlsSynchronizer::BindControls:\r\nEither MapControl or PageLayoutControl are not initialized!");

            //axMapControl1 = mapControl;
            m_pageLayoutControl = pageLayoutControl;

            this.BindControls(activateMapFirst);
        }

        /// <summary>  
        /// bind the MapControl and PageLayoutControl together by assigning a new joint focus map  
        /// 指定共同的Map来把MapControl和PageLayoutControl绑在一起  
        /// </summary>  
        /// <param name="activateMapFirst">true if the MapControl supposed to be activated first,如果MapControl被首先激活,则为true</param>  
        public void BindControls(bool activateMapFirst)
        {
            if (m_pageLayoutControl == null || axMapControl1 == null)
                throw new Exception("ControlsSynchronizer::BindControls:\r\nEither MapControl or PageLayoutControl are not initialized!");

            //create a new instance of IMap  
            //创造IMap的一个实例  
            IMap newMap = new MapClass();
            newMap.Name = "Map";

            //create a new instance of IMaps collection which is needed by the PageLayout  
            //创造一个新的IMaps collection的实例,这是PageLayout所需要的  
            IMaps maps = new Maps();
            //add the new Map instance to the Maps collection  
            //把新的Map实例赋给Maps collection  
            maps.Add(newMap);

            //call replace map on the PageLayout in order to replace the focus map  
            //调用PageLayout的replace map来置换focus map  
            m_pageLayoutControl.PageLayout.ReplaceMaps(maps);
            //assign the new map to the MapControl  
            //把新的map赋给MapControl  
            axMapControl1.Map = newMap;

            //reset the active tools  
            //重设active tools  
            m_pageLayoutActiveTool = null;
            m_mapActiveTool = null;

            //make sure that the last active control is activated  
            //确定最后活动的control被激活  
            if (activateMapFirst)
                this.ActivateMap();
            else
                this.ActivatePageLayout();
        }

        /// <summary>  
        ///by passing the application's toolbars and TOC to the synchronization class, it saves you the  
        ///management of the buddy control each time the active control changes. This method ads the framework  
        ///control to an array; once the active control changes, the class iterates through the array and   
        ///calles SetBuddyControl on each of the stored framework control.  
        /// </summary>  
        /// <param name="control"></param>  
        public void AddFrameworkControl(object control)
        {
            if (control == null)
                throw new Exception("ControlsSynchronizer::AddFrameworkControl:\r\nAdded control is not initialized!");

            m_frameworkControls.Add(control);
        }

        /// <summary>  
        /// Remove a framework control from the managed list of controls  
        /// </summary>  
        /// <param name="control"></param>  
        public void RemoveFrameworkControl(object control)
        {
            if (control == null)
                throw new Exception("ControlsSynchronizer::RemoveFrameworkControl:\r\nControl to be removed is not initialized!");

            m_frameworkControls.Remove(control);
        }

        /// <summary>  
        /// Remove a framework control from the managed list of controls by specifying its index in the list  
        /// </summary>  
        /// <param name="index"></param>  
        public void RemoveFrameworkControlAt(int index)
        {
            if (m_frameworkControls.Count < index)
                throw new Exception("ControlsSynchronizer::RemoveFrameworkControlAt:\r\nIndex is out of range!");

            m_frameworkControls.RemoveAt(index);
        }

        /// <summary>  
        /// when the active control changes, the class iterates through the array of the framework controls  
        ///  and calles SetBuddyControl on each of the controls.  
        /// </summary>  
        /// <param name="buddy">the active control</param>  
        private void SetBuddies(object buddy)
        {
            try
            {
                if (buddy == null)
                    throw new Exception("ControlsSynchronizer::SetBuddies:\r\nTarget Buddy Control is not initialized!");

                foreach (object obj in m_frameworkControls)
                {
                    if (obj is IToolbarControl)
                    {
                        ((IToolbarControl)obj).SetBuddyControl(buddy);
                    }
                    else if (obj is ITOCControl)
                    {
                        ((ITOCControl)obj).SetBuddyControl(buddy);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ControlsSynchronizer::SetBuddies:\r\n{0}", ex.Message));
            }
        }
        #endregion  
        #endregion
    }
}
