﻿using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace Wdbs.edit
{
    class SupportZMFeatureClass
    {
        /// <summary>
        /// 修改FeatureClass的Z、M值
        /// </summary>
        /// <param name="featureClass"></param>
        /// <param name="modifiedGeo"></param>
        /// <returns></returns>
        public static IGeometry ModifyGeomtryZMValue(IObjectClass featureClass, IGeometry modifiedGeo)
        {
            IFeatureClass trgFtCls = featureClass as IFeatureClass;
            if (trgFtCls == null) return null;
            string shapeFieldName = trgFtCls.ShapeFieldName;
            IFields fields = trgFtCls.Fields;
            int geometryIndex = fields.FindField(shapeFieldName);
            IField field = fields.get_Field(geometryIndex);
            IGeometryDef pGeometryDef = field.GeometryDef;
            IPointCollection pPointCollection = modifiedGeo as IPointCollection;
            if (pGeometryDef.HasZ)
            {
                IZAware pZAware = modifiedGeo as IZAware;
                pZAware.ZAware = true;
                IZ iz1 = modifiedGeo as IZ;
                //将Z值设置为0
                iz1.SetConstantZ(0);
            }
            else
            {
                IZAware pZAware = modifiedGeo as IZAware;
                pZAware.ZAware = false;
            }
            if (pGeometryDef.HasM)
            {
                IMAware pMAware = modifiedGeo as IMAware;
                pMAware.MAware = true;
            }
            else
            {
                IMAware pMAware = modifiedGeo as IMAware;
                pMAware.MAware = false;
            }
            return modifiedGeo;
        }

    }
}
