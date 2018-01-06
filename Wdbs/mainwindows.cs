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
using ESRI.ArcGIS.Output;
namespace Wdbs
{
    public partial class mainwindows : Form
    {
        #region class members
        private IMapControl3 m_mapControl = null;
        private IPageLayoutControl2 m_pageLayoutControl = null;  
        //private ITool m_mapActiveTool = null;
        //private ITool m_pageLayoutActiveTool = null;
        //private bool m_IsMapCtrlactive = true;
        private ControlsSynchronizer m_controlsSynchronizer;
        //private ArrayList m_frameworkControls = null;
        #region 编辑变量定义
        private IMap pMap = null;
        private IActiveView pActiveView = null;
        private List<ILayer> plstLayers = null;
        private IFeatureLayer pCurrentLyr = null;
        private IEngineEditor pEngineEditor = null;
        private IEngineEditTask pEngineEditTask = null;
        private IEngineEditLayers pEngineEditLayers = null;
        #endregion
        #region 制图变量定义
        //地图到处窗体                 
        private frmSymbol frmSym = null;
        //对地图的基本操作类
        private OperatePageLayout m_OperatePageLayout = null;
        private INewEnvelopeFeedback pNewEnvelopeFeedback;
        private EnumMapSurroundType _enumMapSurType = EnumMapSurroundType.None;
        private IStyleGalleryItem pStyleGalleryItem;
        private IPoint m_MovePt = null;
        private IPoint m_PointPt = null;
        string filepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        #endregion
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
            InitObject();              //编辑功能初始化
            m_OperatePageLayout = new OperatePageLayout();   //地图制图
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
        #region 菜单相关
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
        #endregion
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
                pMap =  axMapControl1.Map;
            }
            catch (Exception ex)
            {
                MessageBox.Show("图层加载失败！" + ex.Message);
            }
        }
        private void openmxdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Add OpenNewMapDocument.OnClick implementation  
            string sMxdPath1 = null;
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
                    

                    mapDoc.Close();
                }
                sMxdPath1 = docName;
            }

            if (axMapControl1.CheckMxFile(sMxdPath1))
            {
                axMapControl1.LoadMxFile(sMxdPath1);
            }
            
            m_controlsSynchronizer.ReplaceMap(this.axMapControl1.Map);
            plstLayers = MapManager.GetLayers(axMapControl1.Map);
            pActiveView = axMapControl1.Map as IActiveView;
            pMap = axMapControl1.Map;

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
                
                m_controlsSynchronizer.ActivateMap();
                axToolbarControl1.Enabled = true;
            }
            else
            {
                m_controlsSynchronizer.ActivatePageLayout();
                axToolbarControl1.Enabled = false;
               
            }

        }
        #endregion

        #region 编辑功能响应函数与变量

        
        
        private void InitObject()
        {
            try
            {
                ChangeButtonState(false);
                pEngineEditor = new EngineEditorClass();
                MapManager.EngineEditor = pEngineEditor;
                pEngineEditTask = pEngineEditor as IEngineEditTask;
                pEngineEditLayers = pEngineEditor as IEngineEditLayers;
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
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

        #region 地图制图相关
        #region  添加制图要素1
        private void AddLegend_Click(object sender, EventArgs e)
        {
            try
            {
                _enumMapSurType = EnumMapSurroundType.Legend;
            }
            catch (Exception ex)
            {

            }
        }
        private void frmSym_GetSelSymbolItem(ref IStyleGalleryItem pStyleItem)
        {
            pStyleGalleryItem = pStyleItem;
        }

        private void AddNorthArrows_Click(object sender, EventArgs e)
        {
            try
            {
                _enumMapSurType = EnumMapSurroundType.NorthArrow;
                if (frmSym == null || frmSym.IsDisposed)
                {
                    frmSym = new frmSymbol();
                    frmSym.GetSelSymbolItem += new frmSymbol.GetSelSymbolItemEventHandler(frmSym_GetSelSymbolItem);
                }
                frmSym.EnumMapSurType = _enumMapSurType;
                frmSym.InitUI();
                frmSym.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void AddScaleBar_Click(object sender, EventArgs e)
        {
            try
            {
                _enumMapSurType = EnumMapSurroundType.ScaleBar;
                if (frmSym == null || frmSym.IsDisposed)
                {
                    frmSym = new frmSymbol();
                    frmSym.GetSelSymbolItem += new frmSymbol.GetSelSymbolItemEventHandler(frmSym_GetSelSymbolItem);
                }
                frmSym.EnumMapSurType = _enumMapSurType;
                frmSym.InitUI();
                frmSym.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
        #region 添加制图要素2
        //通过OnMouseDown事件，产生矩形框的第一个点
        private void axPageLayoutControl1_OnMouseDown(object sender, IPageLayoutControlEvents_OnMouseDownEvent e)
        {
            try
            {
                if (_enumMapSurType != EnumMapSurroundType.None)
                {
                    IActiveView pActiveView = null;
                    pActiveView = axPageLayoutControl1.PageLayout as IActiveView;
                    m_PointPt = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
                    if (pNewEnvelopeFeedback == null)
                    {
                        pNewEnvelopeFeedback = new NewEnvelopeFeedbackClass();
                        pNewEnvelopeFeedback.Display = pActiveView.ScreenDisplay;
                        pNewEnvelopeFeedback.Start(m_PointPt);
                    }
                    else
                    {
                        pNewEnvelopeFeedback.MoveTo(m_PointPt);
                    }

                }
            }
            catch
            {
            }
        }
        private void axPageLayoutControl1_OnMouseMove(object sender, IPageLayoutControlEvents_OnMouseMoveEvent e)
        {
            try
            {
                if (_enumMapSurType != EnumMapSurroundType.None)
                {
                    if (pNewEnvelopeFeedback != null)
                    {
                        m_MovePt = (axPageLayoutControl1.PageLayout as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
                        pNewEnvelopeFeedback.MoveTo(m_MovePt);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        //通过OnMouseUp事件，产生矩形框的第一个点的对焦点，返回一个矩形，并将制图要素添加到该矩形中
        private void axPageLayoutControl1_OnMouseUp(object sender, IPageLayoutControlEvents_OnMouseUpEvent e)
        {
            if (_enumMapSurType != EnumMapSurroundType.None)
            {
                if (pNewEnvelopeFeedback != null)
                {
                    IActiveView pActiveView = null;
                    pActiveView = axPageLayoutControl1.PageLayout as IActiveView;
                    IEnvelope pEnvelope = pNewEnvelopeFeedback.Stop();
                    AddMapSurround(pActiveView, _enumMapSurType, pEnvelope);
                    pNewEnvelopeFeedback = null;
                    _enumMapSurType = EnumMapSurroundType.None;
                }
            }
        }
        /// <summary>
        /// 添加地图整饰要素
        /// </summary>
        /// <param name="pAV"></param>
        /// <param name="_enumMapSurroundType"></param>
        /// <param name="pEnvelope"></param>
        private void AddMapSurround(IActiveView pAV, EnumMapSurroundType _enumMapSurroundType, IEnvelope pEnvelope)
        {
            try
            {
                switch (_enumMapSurroundType)
                {
                    case EnumMapSurroundType.NorthArrow:
                        addNorthArrow(axPageLayoutControl1.PageLayout, pEnvelope, pAV);
                        break;
                    case EnumMapSurroundType.ScaleBar:
                        makeScaleBar(pAV, axPageLayoutControl1.PageLayout, pEnvelope);
                        break;
                    case EnumMapSurroundType.Legend:
                        MakeLegend(pAV, axPageLayoutControl1.PageLayout, pEnvelope);
                        break;
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
        #region 添加制图要素3
        #region Graticule格网
        private void AddGraticuleh_Click(object sender, EventArgs e)
        {
            try
            {
                IActiveView pActiveView = axPageLayoutControl1.ActiveView;
                IPageLayout pPageLayout = axPageLayoutControl1.PageLayout;
                DeleteMapGrid(pActiveView, pPageLayout);
                CreateGraticuleMapGrid(pActiveView, pPageLayout);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void CreateGraticuleMapGrid(IActiveView pActiveView, IPageLayout pPageLayout)
        {
            IMap pMap = pActiveView.FocusMap;
            IGraticule pGraticule = new GraticuleClass();//看这个改动是否争取
            pGraticule.Name = "Map Grid";
            //设置网格线的符号样式
            ICartographicLineSymbol pLineSymbol;
            pLineSymbol = new CartographicLineSymbolClass();
            pLineSymbol.Cap = esriLineCapStyle.esriLCSButt;
            pLineSymbol.Width = 1;
            pLineSymbol.Color = m_OperatePageLayout.GetRgbColor(166, 187, 208);
            pGraticule.LineSymbol = pLineSymbol;
            //设置网格的边框样式           
            ISimpleMapGridBorder simpleMapGridBorder = new SimpleMapGridBorderClass();
            ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbolClass();
            simpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            simpleLineSymbol.Color = m_OperatePageLayout.GetRgbColor(100, 255, 0);
            simpleLineSymbol.Width = 2;
            simpleMapGridBorder.LineSymbol = simpleLineSymbol as ILineSymbol;
            pGraticule.Border = simpleMapGridBorder as IMapGridBorder;
            pGraticule.SetTickVisibility(true, true, true, true);
            //设置网格的主刻度的样式和可见性
            pGraticule.TickLength = 15;
            pLineSymbol = new CartographicLineSymbolClass();
            pLineSymbol.Cap = esriLineCapStyle.esriLCSButt;
            pLineSymbol.Width = 1;
            pLineSymbol.Color = m_OperatePageLayout.GetRgbColor(255, 187, 208);
            pGraticule.TickMarkSymbol = null;
            pGraticule.TickLineSymbol = pLineSymbol;
            pGraticule.SetTickVisibility(true, true, true, true);
            //设置网格的次级刻度的样式和可见性
            pGraticule.SubTickCount = 5;
            pGraticule.SubTickLength = 10;
            pLineSymbol = new CartographicLineSymbolClass();
            pLineSymbol.Cap = esriLineCapStyle.esriLCSButt;
            pLineSymbol.Width = 0.1;
            pLineSymbol.Color = m_OperatePageLayout.GetRgbColor(166, 187, 208);
            pGraticule.SubTickLineSymbol = pLineSymbol;
            pGraticule.SetSubTickVisibility(true, true, true, true);
            //设置网格的标签的样式和可见性
            IGridLabel pGridLabel;
            pGridLabel = pGraticule.LabelFormat;
            pGridLabel.LabelOffset = 15;
            stdole.StdFont pFont = new stdole.StdFont();
            pFont.Name = "Arial";
            pFont.Size = 16;
            pGraticule.LabelFormat.Font = pFont as stdole.IFontDisp;
            pGraticule.Visible = true;
            //创建IMeasuredGrid对象
            IMeasuredGrid pMeasuredGrid = new MeasuredGridClass();
            IProjectedGrid pProjectedGrid = pMeasuredGrid as IProjectedGrid;
            pProjectedGrid.SpatialReference = pMap.SpatialReference;
            pMeasuredGrid = pGraticule as IMeasuredGrid;
            //获取坐标范围，设置网格的起始点和间隔
            double MaxX, MaxY, MinX, MinY;
            pProjectedGrid.SpatialReference.GetDomain(out MinX, out MaxX, out MinY, out MaxY);
            pMeasuredGrid.FixedOrigin = true;
            pMeasuredGrid.Units = pMap.MapUnits;
            pMeasuredGrid.XIntervalSize = (MaxX - MinX) / 200;//纬度间隔
            pMeasuredGrid.XOrigin = MinX;
            pMeasuredGrid.YIntervalSize = (MaxY - MinY) / 200;//经度间隔.
            pMeasuredGrid.YOrigin = MinY;
            //将网格对象添加到地图控件中                              
            IGraphicsContainer pGraphicsContainer = pActiveView as IGraphicsContainer;
            IMapFrame pMapFrame = pGraphicsContainer.FindFrame(pMap) as IMapFrame;
            IMapGrids pMapGrids = pMapFrame as IMapGrids;
            pMapGrids.AddMapGrid(pGraticule);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
        }
        #endregion
        #region MeasuredGrid格网
        private void AddMeasuredGrid_Click(object sender, EventArgs e)
        {
            IActiveView pActiveView = axPageLayoutControl1.ActiveView;
            IPageLayout pPageLayout = axPageLayoutControl1.PageLayout;
            DeleteMapGrid(pActiveView, pPageLayout);//删除已存在格网
            CreateMeasuredGrid(pActiveView, pPageLayout);
        }
        public void CreateMeasuredGrid(IActiveView pActiveView, IPageLayout pPageLayout)
        {
            IMap map = pActiveView.FocusMap;
            IMeasuredGrid pMeasuredGrid = new MeasuredGridClass();
            //设置格网基本属性           
            pMeasuredGrid.FixedOrigin = false;
            pMeasuredGrid.Units = map.MapUnits;
            pMeasuredGrid.XIntervalSize = 5;//纬度间隔           
            pMeasuredGrid.YIntervalSize = 5;//经度间隔.             
            //设置GridLabel格式
            IGridLabel pGridLabel = new FormattedGridLabelClass();
            IFormattedGridLabel pFormattedGridLabel = new FormattedGridLabelClass();
            INumericFormat pNumericFormat = new NumericFormatClass();
            pNumericFormat.AlignmentOption = esriNumericAlignmentEnum.esriAlignLeft;
            pNumericFormat.RoundingOption = esriRoundingOptionEnum.esriRoundNumberOfDecimals;
            pNumericFormat.RoundingValue = 0;
            pNumericFormat.ZeroPad = true;
            pFormattedGridLabel.Format = pNumericFormat as INumberFormat;
            pGridLabel = pFormattedGridLabel as IGridLabel;
            StdFont myFont = new stdole.StdFontClass();
            myFont.Name = "宋体";
            myFont.Size = 25;
            pGridLabel.Font = myFont as IFontDisp;
            IMapGrid pMapGrid = new MeasuredGridClass();
            pMapGrid = pMeasuredGrid as IMapGrid;
            pMapGrid.LabelFormat = pGridLabel;
            //将格网添加到地图上           
            IGraphicsContainer graphicsContainer = pPageLayout as IGraphicsContainer;
            IFrameElement frameElement = graphicsContainer.FindFrame(map);
            IMapFrame mapFrame = frameElement as IMapFrame;
            IMapGrids mapGrids = null;
            mapGrids = mapFrame as IMapGrids;
            mapGrids.AddMapGrid(pMapGrid);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
        }
        #endregion
        #region IndexGrid格网
        private void AddIndexGrid_Click(object sender, EventArgs e)
        {
            IActiveView pActiveView = axPageLayoutControl1.ActiveView;
            IPageLayout pPageLayout = axPageLayoutControl1.PageLayout;
            DeleteMapGrid(pActiveView, pPageLayout);
            CreateIndexGrid(pActiveView, pPageLayout);
        }
        public void CreateIndexGrid(IActiveView pActiveView, IPageLayout pPageLayout)
        {
            IIndexGrid pIndexGrid = new IndexGridClass();
            //设置Index属性
            pIndexGrid.ColumnCount = 5;
            pIndexGrid.RowCount = 5;
            String[] indexnum = { "A", "B", "C", "D", "E" };
            //设置IndexLabel
            int i = 0;
            for (i = 0; i <= (pIndexGrid.ColumnCount - 1); i++)
            {
                pIndexGrid.set_XLabel(i, indexnum[i]);
            }
            for (i = 0; i <= (pIndexGrid.RowCount - 1); i++)
            {
                pIndexGrid.set_YLabel(i, i.ToString());
            }
            //设置GridLabel格式
            IGridLabel pGridLabel = new RoundedTabStyleClass();
            StdFont myFont = new stdole.StdFontClass();
            myFont.Name = "宋体";
            myFont.Size = 18;
            pGridLabel.Font = myFont as IFontDisp;
            IMapGrid pmapGrid = new IndexGridClass();
            pmapGrid = pIndexGrid as IMapGrid;
            pmapGrid.LabelFormat = pGridLabel;
            //添加IndexGrid         
            IMapGrid pMapGrid = pIndexGrid;
            IMap pMap = pActiveView.FocusMap;
            IGraphicsContainer graphicsContainer = pPageLayout as IGraphicsContainer;
            IFrameElement frameElement = graphicsContainer.FindFrame(pMap);
            IMapFrame mapFrame = frameElement as IMapFrame;
            IMapGrids mapGrids = null;
            mapGrids = mapFrame as IMapGrids;
            mapGrids.AddMapGrid(pMapGrid);
            axPageLayoutControl1.Refresh();
        }
        #endregion
        #region 删除已存在格网
        public void DeleteMapGrid(IActiveView pActiveView, IPageLayout pPageLayout)
        {
            IMap pMap = pActiveView.FocusMap;
            IGraphicsContainer graphicsContainer = pPageLayout as IGraphicsContainer;
            IFrameElement frameElement = graphicsContainer.FindFrame(pMap);
            IMapFrame mapFrame = frameElement as IMapFrame;
            IMapGrids mapGrids = null;
            mapGrids = mapFrame as IMapGrids;
            if (mapGrids.MapGridCount > 0)
            {
                IMapGrid pMapGrid = mapGrids.get_MapGrid(0);
                mapGrids.DeleteMapGrid(pMapGrid);
            }
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
        }
        #endregion
        #endregion
        #region 添加图例
        /// <summary>
        /// 添加图例
        /// </summary>
        /// <param name="activeView"></活动窗口>
        /// <param name="pageLayout"></布局窗口>
        /// <param name="pEnv"></包络线>
        private void MakeLegend(IActiveView pActiveView, IPageLayout pPageLayout, IEnvelope pEnv)
        {
            UID pID = new UID();
            pID.Value = "esriCarto.Legend";
            IGraphicsContainer pGraphicsContainer = pPageLayout as IGraphicsContainer;
            IMapFrame pMapFrame = pGraphicsContainer.FindFrame(pActiveView.FocusMap) as IMapFrame;
            IMapSurroundFrame pMapSurroundFrame = pMapFrame.CreateSurroundFrame(pID, null);//根据唯一标示符，创建与之对应MapSurroundFrame
            IElement pDeletElement = axPageLayoutControl1.FindElementByName("Legend");//获取PageLayout中的图例元素
            if (pDeletElement != null)
            {
                pGraphicsContainer.DeleteElement(pDeletElement);  //如果已经存在图例，删除已经存在的图例
            }
            //设置MapSurroundFrame背景
            ISymbolBackground pSymbolBackground = new SymbolBackgroundClass();
            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            ILineSymbol pLineSymbol = new SimpleLineSymbolClass();
            pLineSymbol.Color = m_OperatePageLayout.GetRgbColor(0, 0, 0);
            pFillSymbol.Color = m_OperatePageLayout.GetRgbColor(240, 240, 240);
            pFillSymbol.Outline = pLineSymbol;
            pSymbolBackground.FillSymbol = pFillSymbol;
            pMapSurroundFrame.Background = pSymbolBackground;
            //添加图例
            IElement pElement = pMapSurroundFrame as IElement;
            pElement.Geometry = pEnv as IGeometry;
            IMapSurround pMapSurround = pMapSurroundFrame.MapSurround;
            ILegend pLegend = pMapSurround as ILegend;
            pLegend.ClearItems();
            pLegend.Title = "图例";
            for (int i = 0; i < pActiveView.FocusMap.LayerCount; i++)
            {
                ILegendItem pLegendItem = new HorizontalLegendItemClass();
                pLegendItem.Layer = pActiveView.FocusMap.get_Layer(i);//获取添加图例关联图层             
                pLegendItem.ShowDescriptions = false;
                pLegendItem.Columns = 1;
                pLegendItem.ShowHeading = true;
                pLegendItem.ShowLabels = true;
                pLegend.AddItem(pLegendItem);//添加图例内容
            }
            pGraphicsContainer.AddElement(pElement, 0);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }
        #endregion
        #region 指北针
        /// <summary>
        /// 插入指北针
        /// </summary>
        /// <param name="pPageLayout"></param>
        /// <param name="pEnv"></param>
        /// <param name="pActiveView"></param>
        void addNorthArrow(IPageLayout pPageLayout, IEnvelope pEnv, IActiveView pActiveView)
        {
            IMap pMap = pActiveView.FocusMap;
            IGraphicsContainer pGraphicsContainer = pPageLayout as IGraphicsContainer;
            IMapFrame pMapFrame = pGraphicsContainer.FindFrame(pMap) as IMapFrame;
            if (pStyleGalleryItem == null) return;
            IMapSurroundFrame pMapSurroundFrame = new MapSurroundFrameClass();
            pMapSurroundFrame.MapFrame = pMapFrame;
            INorthArrow pNorthArrow = new MarkerNorthArrowClass();
            pNorthArrow = pStyleGalleryItem.Item as INorthArrow;
            pNorthArrow.Size = pEnv.Width * 50;
            pMapSurroundFrame.MapSurround = (IMapSurround)pNorthArrow;//根据用户的选取，获取相应的MapSurround            
            IElement pElement = axPageLayoutControl1.FindElementByName("NorthArrows");//获取PageLayout中的指北针元素
            if (pElement != null)
            {
                pGraphicsContainer.DeleteElement(pElement);  //如果存在指北针，删除已经存在的指北针
            }
            IElementProperties pElePro = null;
            pElement = (IElement)pMapSurroundFrame;
            pElement.Geometry = (IGeometry)pEnv;
            pElePro = pElement as IElementProperties;
            pElePro.Name = "NorthArrows";
            pGraphicsContainer.AddElement(pElement, 0);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }
        #endregion
        #region  比例尺
        /// <summary>
        /// 比例尺
        /// </summary>
        /// <param name="pActiveView"></param>
        /// <param name="pPageLayout"></param>
        /// <param name="pEnv"></param>
        public void makeScaleBar(IActiveView pActiveView, IPageLayout pPageLayout, IEnvelope pEnv)
        {
            IMap pMap = pActiveView.FocusMap;
            IGraphicsContainer pGraphicsContainer = pPageLayout as IGraphicsContainer;
            IMapFrame pMapFrame = pGraphicsContainer.FindFrame(pMap) as IMapFrame;
            if (pStyleGalleryItem == null) return;
            IMapSurroundFrame pMapSurroundFrame = new MapSurroundFrameClass();
            pMapSurroundFrame.MapFrame = pMapFrame;
            pMapSurroundFrame.MapSurround = (IMapSurround)pStyleGalleryItem.Item;
            IElement pElement = axPageLayoutControl1.FindElementByName("ScaleBar");
            if (pElement != null)
            {
                pGraphicsContainer.DeleteElement(pElement);  //删除已经存在的比例尺
            }
            IElementProperties pElePro = null;
            pElement = (IElement)pMapSurroundFrame;
            pElement.Geometry = (IGeometry)pEnv;
            pElePro = pElement as IElementProperties;
            pElePro.Name = "ScaleBar";
            pGraphicsContainer.AddElement(pElement, 0);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }
        #endregion
        #region  输出
        private void ExportMap_Click(object sender, EventArgs e)
        {
            ExportMapToImage();
        }
        private void ExportMapToImage()
        {
            try
            {
                SaveFileDialog pSaveDialog = new SaveFileDialog();
                pSaveDialog.FileName = "";
                pSaveDialog.Filter = "JPG图片(*.JPG)|*.jpg|tif图片(*.tif)|*.tif|PDF文档(*.PDF)|*.pdf";
                if (pSaveDialog.ShowDialog() == DialogResult.OK)
                {
                    double iScreenDispalyResolution = axPageLayoutControl1.ActiveView.ScreenDisplay.DisplayTransformation.Resolution;// 获取屏幕分辨率的值
                    IExporter pExporter = null;
                    if (pSaveDialog.FilterIndex == 1)
                    {
                        pExporter = new JpegExporterClass();
                    }
                    else if (pSaveDialog.FilterIndex == 2)
                    {
                        pExporter = new TiffExporterClass();
                    }
                    else if (pSaveDialog.FilterIndex == 3)
                    {
                        pExporter = new PDFExporterClass();
                    }
                    pExporter.ExportFileName = pSaveDialog.FileName;
                    pExporter.Resolution = (short)iScreenDispalyResolution; //分辨率
                    tagRECT deviceRect = axPageLayoutControl1.ActiveView.ScreenDisplay.DisplayTransformation.get_DeviceFrame();
                    IEnvelope pDeviceEnvelope = new EnvelopeClass();
                    pDeviceEnvelope.PutCoords(deviceRect.left, deviceRect.bottom, deviceRect.right, deviceRect.top);
                    pExporter.PixelBounds = pDeviceEnvelope; // 输出图片的范围
                    ITrackCancel pCancle = new CancelTrackerClass();//可用ESC键取消操作
                    axPageLayoutControl1.ActiveView.Output(pExporter.StartExporting(), pExporter.Resolution, ref deviceRect, axPageLayoutControl1.ActiveView.Extent, pCancle);
                    Application.DoEvents();
                    pExporter.FinishExporting();
                }

            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message, "输出图片", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }      
        #endregion 
        #endregion


    }
}
