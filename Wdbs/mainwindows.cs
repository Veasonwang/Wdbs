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
using Wdbs.edit;

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
            InitObject();              //编辑功能初始化
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


        //双击TOCControl控件时触发事件
        private void mainTOCControl_OnDoubleClick(object sender, ITOCControlEvents_OnDoubleClickEvent e)
        {
            esriTOCControlItem itemType = esriTOCControlItem.esriTOCControlItemNone;
            IBasicMap basicMap = null;
            ILayer layer = null;
            object unk = null;
            object data = null;
            axTOCControl1.HitTest(e.x, e.y, ref itemType, ref basicMap, ref layer, ref unk, ref data);
            if (e.button == 1)
            {
                if (itemType == esriTOCControlItem.esriTOCControlItemLegendClass)
                {
                    //取得图例
                    ILegendClass pLegendClass = ((ILegendGroup)unk).get_Class((int)data);
                    //创建符号选择器SymbolSelector实例
                    frmSymbolSelector SymbolSelectorFrm = new frmSymbolSelector(pLegendClass, layer);
                    if (SymbolSelectorFrm.ShowDialog() == DialogResult.OK)
                    {
                        //局部更新主Map控件
                        axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                        //设置新的符号
                        pLegendClass.Symbol = SymbolSelectorFrm.pSymbol;
                        //更新主Map控件和图层控件
                        this.axMapControl1.ActiveView.Refresh();
                        this.axTOCControl1.Refresh();
                    }
                }
            }
        }
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
                m_controlsSynchronizer.ActivateMap();
                
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
                m_controlsSynchronizer.ActivatePageLayout();
                //m_bIsMapCtrActive = false;
               
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

        private void openmxdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Add OpenNewMapDocument.OnClick implementation  
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Map Documents (*.mxd)|*.mxd";
            dlg.Multiselect = false;
            dlg.Title = "Open Map Document";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string docName = dlg.FileName;
                IMapDocument mapDoc = new MapDocumentClass();
                if (mapDoc.get_IsPresent(docName) && !mapDoc.get_IsPasswordProtected(docName))
                {
                    mapDoc.Open(docName, string.Empty);
                    IMap map = mapDoc.get_Map(0);
                    m_controlsSynchronizer.ReplaceMap(map);

                    mapDoc.Close();
                }
                sMxdPath = docName;
            }
            
            if (axMapControl1.CheckMxFile(sMxdPath))
            {
                axMapControl1.LoadMxFile(sMxdPath);
            }
            pMap = axMapControl1.Map;
            plstLayers = MapManager.GetLayers(pMap);
            pActiveView = pMap as IActiveView;
        }
        #region 编辑功能响应函数与变量

        #region 变量定义        
        private string sMxdPath = Application.StartupPath ;
        private IMap pMap = null;
        private IActiveView pActiveView = null;
        private List<ILayer> plstLayers = null;
        private IFeatureLayer pCurrentLyr = null;      
        private IEngineEditor pEngineEditor = null;
        private IEngineEditTask pEngineEditTask = null;
        private IEngineEditLayers pEngineEditLayers = null;

        #endregion
        #endregion
        private void InitObject()
        {
            try
            {
                ChangeButtonState(false);
                pEngineEditor = new EngineEditorClass();
                MapManager.EngineEditor = pEngineEditor;
                pEngineEditTask = pEngineEditor as IEngineEditTask;
                pEngineEditLayers = pEngineEditor as IEngineEditLayers;
                sMxdPath = "C:\\Users\\v\\Desktop\\ArcGIS Engine 地理信息系统开发教程---基于C#.NET\\chp06\\空间数据编辑\\data\\edit.mxd";
                //if (axMapControl1.CheckMxFile(sMxdPath))
                //{
                //    axMapControl1.LoadMxFile(sMxdPath);
                //}
                //pMap = axMapControl1.Map;
                //plstLayers = MapManager.GetLayers(pMap);
                //pActiveView = pMap as IActiveView;
            }
            catch (Exception ex)
            {

            }
        }
        #region 编辑操作

        /// <summary>
        /// 开始编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (plstLayers == null || plstLayers.Count == 0)
                {
                    MessageBox.Show("请加载编辑图层！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                pMap.ClearSelection();
                pActiveView.Refresh();
                InitComboBox(plstLayers);
                ChangeButtonState(true);
                //如果编辑已经开始，则直接退出
                if (pEngineEditor.EditState != esriEngineEditState.esriEngineStateNotEditing)
                    return;
                if (pCurrentLyr == null) return;
                //获取当前编辑图层工作空间
                IDataset pDataSet = pCurrentLyr.FeatureClass as IDataset;
                IWorkspace pWs = pDataSet.Workspace;
                //设置编辑模式，如果是ArcSDE采用版本模式
                if (pWs.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
                {
                    pEngineEditor.EditSessionMode = esriEngineEditSessionMode.esriEngineEditSessionModeVersioned;
                }
                else
                {
                    pEngineEditor.EditSessionMode = esriEngineEditSessionMode.esriEngineEditSessionModeNonVersioned;
                }
                //设置编辑任务
                pEngineEditTask = pEngineEditor.GetTaskByUniqueName("ControlToolsEditing_CreateNewFeatureTask");
                pEngineEditor.CurrentTask = pEngineEditTask;// 设置编辑任务
                pEngineEditor.EnableUndoRedo(true); //是否可以进行撤销、恢复操作
                pEngineEditor.StartEditing(pWs, pMap); //开始编辑操作
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 保存编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ICommand m_saveEditCom = new SaveEditCommandClass();
                m_saveEditCom.OnCreate(axMapControl1.Object);
                m_saveEditCom.OnClick();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 结束编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEndEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ICommand m_stopEditCom = new StopEditCommandClass();
                m_stopEditCom.OnCreate(axMapControl1.Object);
                m_stopEditCom.OnClick();
                ChangeButtonState(false);
                axMapControl1.CurrentTool = null;
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 添加选择框中内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSelLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string sLyrName = cmbSelLayer.SelectedItem.ToString();
                pCurrentLyr = MapManager.GetLayerByName(pMap, sLyrName) as IFeatureLayer;
                //设置编辑目标图层
                pEngineEditLayers.SetTargetLayer(pCurrentLyr, 0);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 选择要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelFeat_Click(object sender, EventArgs e)
        {
            try
            {
                ICommand m_SelTool = new SelectFeatureToolClass();
                m_SelTool.OnCreate(axMapControl1.Object);
                m_SelTool.OnClick();
                axMapControl1.CurrentTool = m_SelTool as ITool;
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerArrow;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUndo_Click(object sender, EventArgs e)
        {
            try
            {
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerArrow;
                ICommand m_undoCommand = new UndoCommandClass();
                m_undoCommand.OnCreate(axMapControl1.Object);
                m_undoCommand.OnClick();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRedo_Click(object sender, EventArgs e)
        {
            try
            {
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerArrow;
                ICommand m_redoCommand = new RedoCommandClass();
                m_redoCommand.OnCreate(axMapControl1.Object);
                m_redoCommand.OnClick();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 移动要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelMove_Click(object sender, EventArgs e)
        {
            try
            {
                ICommand m_moveTool = new MoveFeatureToolClass();
                m_moveTool.OnCreate(axMapControl1.Object);
                m_moveTool.OnClick();
                axMapControl1.CurrentTool = m_moveTool as ITool;
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerArrow;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 添加要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddFeature_Click(object sender, EventArgs e)
        {
            try
            {
                ICommand m_CreateFeatTool = new CreateFeatureToolClass();
                m_CreateFeatTool.OnCreate(axMapControl1.Object);
                m_CreateFeatTool.OnClick();
                axMapControl1.CurrentTool = m_CreateFeatTool as ITool;
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 删除要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelFeature_Click(object sender, EventArgs e)
        {
            try
            {
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerArrow;
                ICommand m_delFeatCom = new DelFeatureCommandClass();
                m_delFeatCom.OnCreate(axMapControl1.Object);
                m_delFeatCom.OnClick();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 属性编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAttributeEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ICommand m_AtrributeCom = new EditAtrributeToolClass();
                m_AtrributeCom.OnCreate(axMapControl1.Object);
                m_AtrributeCom.OnClick();
                axMapControl1.CurrentTool = m_AtrributeCom as ITool;
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerArrow;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 移动节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMoveVertex_Click(object sender, EventArgs e)
        {
            try
            {
                ICommand m_MoveVertexTool = new MoveVertexToolClass();
                m_MoveVertexTool.OnCreate(axMapControl1.Object);
                axMapControl1.CurrentTool = m_MoveVertexTool as ITool;
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddVertex_Click(object sender, EventArgs e)
        {
            try
            {
                ICommand m_AddVertexTool = new AddVertexToolClass();
                m_AddVertexTool.OnCreate(axMapControl1.Object);
                axMapControl1.CurrentTool = m_AddVertexTool as ITool;
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelVertex_Click(object sender, EventArgs e)
        {
            try
            {
                ICommand m_DelVertexTool = new DelVertexToolClass();
                m_DelVertexTool.OnCreate(axMapControl1.Object);
                axMapControl1.CurrentTool = m_DelVertexTool as ITool;
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region 操作函数

        private void ChangeButtonState(bool bEnable)
        {
            btnStartEdit.Enabled = !bEnable;
            btnSaveEdit.Enabled = bEnable;
            btnEndEdit.Enabled = bEnable;

            cmbSelLayer.Enabled = bEnable;

            btnSelFeat.Enabled = bEnable;
            btnSelMove.Enabled = bEnable;
            btnAddFeature.Enabled = bEnable;
            btnDelFeature.Enabled = bEnable;
            btnAttributeEdit.Enabled = bEnable;
            btnUndo.Enabled = bEnable;
            btnRedo.Enabled = bEnable;
            btnMoveVertex.Enabled = bEnable;
            btnAddVertex.Enabled = bEnable;
            btnDelVertex.Enabled = bEnable;
        }

        private void InitComboBox(List<ILayer> plstLyr)
        {
            cmbSelLayer.Items.Clear();
            for (int i = 0; i < plstLyr.Count; i++)
            {
                if (!cmbSelLayer.Items.Contains(plstLyr[i].Name))
                {
                    cmbSelLayer.Items.Add(plstLyr[i].Name);
                }
            }
            if (cmbSelLayer.Items.Count != 0) cmbSelLayer.SelectedIndex = 0;
        }

        #endregion

       


    }
}
