using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ESRI.ArcGIS.ADF.BaseClasses;
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
    class New_layout : BaseCommand
    {
        private IMapControl3 m_mapControl;
        public New_layout()
        {
            base.m_caption = "new Layout";
        }
        public override void OnClick()
        {
            IFeatureLayer pFeatureLayer;
            pFeatureLayer = new FeatureLayer();
            pFeatureLayer.Name = "new layout";
            m_mapControl.AddLayer(pFeatureLayer);
        }

        public override void OnCreate(object hook)
        {
            m_mapControl = (IMapControl3)hook;
        }
    }
}
