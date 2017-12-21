using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
namespace Wdbs
{
    class newfeature : BaseCommand
    {
        private IMapControl3 m_mapControl;
        public newfeature()
        {
            base.m_caption = "new Feature";
        }
        public override void OnClick()
        {

        }

        public override void OnCreate(object hook)
        {
            m_mapControl = (IMapControl3)hook;
        }

    }
}
