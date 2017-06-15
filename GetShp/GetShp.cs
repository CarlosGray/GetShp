using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gdal = OSGeo.GDAL.Gdal;
using Ogr = OSGeo.OGR.Ogr;
using OSGeo.OGR;
using System.Runtime.InteropServices;

namespace GetShp
{
    class CGetShp
    {
        enum LAYER_TYPE
        {
            POINT,
            LINE
        }
        private string mSrcFile;
        private string mOutPath;
        public CGetShp(string srcFile, string outPath)
        {
            mSrcFile = srcFile;
            mOutPath = outPath;
        }
        [DllImport("gdal111.dll", EntryPoint = "OGR_F_GetFieldAsString", CallingConvention = CallingConvention.Cdecl)]
        public extern static System.IntPtr OGR_F_GetFieldAsString(HandleRef handle, int i);
        public void readLine(ref Layer oLayer, string pointTableName)
        {
            // 对图层进行初始化，如果对图层进行了过滤操作，执行这句后，之前的过滤全部清空  
            oLayer.ResetReading();

            //打开数据  
            DataSource newDs = Ogr.Open(mSrcFile, 0);
            if (newDs == null)
            {
                Console.WriteLine(string.Format("打开文件【{0}】失败！\n", mSrcFile));
                return;
            }

            //根据图层名称获取相应的图层
            Layer matchLayer = newDs.GetLayerByName(pointTableName);

            Driver oDriver = null;
            DataSource oDS = null;
            Layer newLayer = null;
            initShp(ref oLayer, ref oDriver, ref oDS, ref newLayer, LAYER_TYPE.LINE);

            // 获取图层中的属性表表头并输出  
            string strInfo = "属性表结构信息：\n";
            FeatureDefn oDefn = oLayer.GetLayerDefn();
            int iFieldCount = oDefn.GetFieldCount();
            for (int iAttr = 0; iAttr < iFieldCount; iAttr++)
            {
                FieldDefn oField = oDefn.GetFieldDefn(iAttr);
                FieldType type = oField.GetFieldType();

                //为新图层创建属性
                FieldDefn newField = new FieldDefn(oField.GetNameRef(), type);
                if (type == FieldType.OFTString)
                {
                    newField.SetWidth(oField.GetWidth());
                }
                newLayer.CreateField(newField, 1);

                strInfo += string.Format("{0}:{1} ({2}.{3})\n", oField.GetNameRef(),
                         oField.GetFieldTypeName(oField.GetFieldType()),
                         oField.GetWidth(), oField.GetPrecision());
            }
            FeatureDefn newDefn = newLayer.GetLayerDefn();
            // 输出图层中的要素个数  
            strInfo += string.Format("要素个数 = {0}\n", oLayer.GetFeatureCount(0));
            Feature oFeature = null;
            // 下面开始遍历图层中的要素  
            double S_X = 0.0;
            double S_Y = 0.0;
            double E_X = 0.0;
            double E_Y = 0.0;
            while ((oFeature = oLayer.GetNextFeature()) != null)
            {
                strInfo += string.Format("\n当前处理第{0}个: \n属性值：", oFeature.GetFID());

                //为新图层创建要素
                Feature oFeatureLineString = new Feature(newDefn);

                string sql = string.Empty;
                // 获取要素中的属性表内容  
                for (int iField = 0; iField < iFieldCount; iField++)
                {
                    FieldDefn oFieldDefn = oDefn.GetFieldDefn(iField);
                    string name = oFieldDefn.GetNameRef();
                    FieldType type = oFieldDefn.GetFieldType();
                    switch (type)
                    {
                        case FieldType.OFTString:
                            IntPtr pchar = OGR_F_GetFieldAsString(Feature.getCPtr(oFeature), iField);
                            string val = Marshal.PtrToStringAnsi(pchar);
                            oFeatureLineString.SetField(iField, val);
                            switch (name)
                            {
                                case "S_Point":
                                    sql = oFeature.GetFieldAsString(iField);
                                    getCoordinate(ref matchLayer, ref sql, ref S_X, ref S_Y);
                                    break;
                                case "E_Point":
                                    sql = oFeature.GetFieldAsString(iField);
                                    getCoordinate(ref matchLayer, ref sql, ref E_X, ref E_Y);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case FieldType.OFTReal:
                            oFeatureLineString.SetField(name, oFeature.GetFieldAsDouble(iField));
                            break;
                        case FieldType.OFTInteger:
                            oFeatureLineString.SetField(iField, oFeature.GetFieldAsInteger(iField));
                            break;
                        default:
                            oFeatureLineString.SetField(iField, oFeature.GetFieldAsString(iField));
                            break;
                    }

                }
                Geometry oGeometry = Geometry.CreateFromWkt(string.Format("LINESTRING({0} {1},{2} {3})", S_X, S_Y, E_X, E_Y));
                oFeatureLineString.SetGeometryDirectly(oGeometry);
                newLayer.CreateFeature(oFeatureLineString);
                oGeometry.Dispose();
            }
            strInfo += "\n数据集关闭！";

            oDS.Dispose();
        }
        //-----------------------------------------------------------------------------------
        private void getCoordinate(ref Layer matchLayer, ref string sql, ref double X, ref double Y)
        {
            matchLayer.ResetReading();
            matchLayer.SetAttributeFilter(string.Format("Exp_No = '{0}'", sql));
            Feature matchFeature = matchLayer.GetNextFeature();
            X = matchFeature.GetFieldAsDouble("X");
            Y = matchFeature.GetFieldAsDouble("Y");
        }
        //-----------------------------------------------------------------------------------
        public void readPoint(ref Layer oLayer)
        {
            // 对图层进行初始化，如果对图层进行了过滤操作，执行这句后，之前的过滤全部清空  
            oLayer.ResetReading();
            Driver oDriver = null;
            DataSource oDS = null;
            Layer newLayer = null;
            initShp(ref oLayer, ref oDriver, ref oDS, ref newLayer, LAYER_TYPE.POINT);

            // 获取图层中的属性表表头并输出  
            string strInfo = "属性表结构信息：\n";
            FeatureDefn oDefn = oLayer.GetLayerDefn();

            int iFieldCount = oDefn.GetFieldCount();
            for (int iAttr = 0; iAttr < iFieldCount; iAttr++)
            {
                FieldDefn oField = oDefn.GetFieldDefn(iAttr);
                FieldType type = oField.GetFieldType();
                string fieldName = oField.GetName();
                //为新图层创建属性
                if (!fieldName.Equals("X坐标") && !fieldName.Equals("Y坐标"))
                {
                    FieldDefn newField = new FieldDefn(fieldName, type);
                    if (type == FieldType.OFTString)
                    {
                        newField.SetWidth(oField.GetWidth());
                    }
                    newLayer.CreateField(newField, 1);
                }
                //获取图层属性信息
                strInfo += string.Format("{0}:{1} ({2}.{3})\n", oField.GetNameRef(),
                         oField.GetFieldTypeName(oField.GetFieldType()),
                         oField.GetWidth(), oField.GetPrecision());
            }
            FeatureDefn newDefn = newLayer.GetLayerDefn();
            // 输出图层中的要素个数  
            strInfo += string.Format("要素个数 = {0}\n", oLayer.GetFeatureCount(0));
            Feature oFeature = null;
            // 下面开始遍历图层中的要素  
            while ((oFeature = oLayer.GetNextFeature()) != null)
            {
                strInfo += string.Format("\n当前处理第{0}个: \n属性值：", oFeature.GetFID());
                //为新图层创建要素
                Feature oFeaturePoint = new Feature(newDefn);
                double pointX = 0.0;
                double pointY = 0.0;
                // 获取要素中的属性表内容  
                for (int iField = 0; iField < iFieldCount; iField++)
                {
                    FieldDefn oFieldDefn = oDefn.GetFieldDefn(iField);
                    FieldType type = oFieldDefn.GetFieldType();
                    string name = oFieldDefn.GetNameRef();
                    switch (type)
                    {
                        case FieldType.OFTString:
                            IntPtr pchar = OGR_F_GetFieldAsString(Feature.getCPtr(oFeature), iField);
                            string val = Marshal.PtrToStringAnsi(pchar);
                            oFeaturePoint.SetField(name, val);
                            break;
                        case FieldType.OFTReal:
                            if (!name.Equals("X坐标") && !name.Equals("Y坐标"))
                            {
                                oFeaturePoint.SetField(name, oFeature.GetFieldAsDouble(iField));
                            }
                            switch (name)
                            {
                                case "X":
                                    pointX = oFeature.GetFieldAsDouble(iField);
                                    break;
                                case "Y":
                                    pointY = oFeature.GetFieldAsDouble(iField);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case FieldType.OFTInteger:
                            oFeaturePoint.SetField(iField, oFeature.GetFieldAsInteger(iField));
                            break;
                        default:
                            oFeaturePoint.SetField(iField, oFeature.GetFieldAsString(iField));
                            break;
                    }
                }
                //填充要素几何信息  
                Geometry oGeometry = Geometry.CreateFromWkt(string.Format("POINT({0} {1})", pointX, pointY));
                oFeaturePoint.SetGeometry(oGeometry);
                newLayer.CreateFeature(oFeaturePoint);
            }
            strInfo += "\n数据集关闭！";
            oDS.Dispose();
        }
        //---------------------------------------------------------------------------------------
        private void initShp(ref Layer oLayer, ref Driver oDriver, ref DataSource oDS, ref Layer newLayer, LAYER_TYPE type)
        {
            //创建数据
            string shpFileName = string.Format("{0}\\{1}.shp", mOutPath,oLayer.GetName());
            oDriver = Ogr.GetDriverByName("ESRI Shapefile");
            if (oDriver == null)
            {
                Console.WriteLine("{0} 驱动不可用！\n", shpFileName);
                return;
            }
            // 创建数据源
            oDS = oDriver.CreateDataSource(shpFileName, null);
            if (oDS == null)
            {
                Console.WriteLine("创建矢量文件【%s】失败！\n", shpFileName);
                return;
            }
            // 创建图层，创建一个点图层，这里没有指定空间参考，如果需要的话，需要在这里进行指定
            switch (type)
            {
                case LAYER_TYPE.LINE:
                    newLayer = oDS.CreateLayer(oLayer.GetName(), null, wkbGeometryType.wkbLineString, null);
                    break;
                case LAYER_TYPE.POINT:
                    newLayer = oDS.CreateLayer(oLayer.GetName(), null, wkbGeometryType.wkbPoint, null);
                    break;
                default:
                    break;
            }
            if (oLayer == null)
            {
                Console.WriteLine("图层创建失败！\n");
                return;
            }
        }
        public void test(ref Layer oLayer)
        {
            // 对图层进行初始化，如果对图层进行了过滤操作，执行这句后，之前的过滤全部清空  
            oLayer.ResetReading();

            // 获取图层中的属性表表头并输出  
            string strInfo = "属性表结构信息：\n";
            FeatureDefn oDefn = oLayer.GetLayerDefn();
            int iFieldCount = oDefn.GetFieldCount();
            for (int iAttr = 0; iAttr < iFieldCount; iAttr++)
            {
                FieldDefn oField = oDefn.GetFieldDefn(iAttr);

                strInfo += string.Format("{0}:{1} ({2}.{3})\n", oField.GetNameRef(),
                         oField.GetFieldTypeName(oField.GetFieldType()),
                         oField.GetWidth(), oField.GetPrecision());
            }
            // 输出图层中的要素个数  
            strInfo += string.Format("要素个数 = {0}\n", oLayer.GetFeatureCount(0));
            Feature oFeature = null;
            // 下面开始遍历图层中的要素  
            while ((oFeature = oLayer.GetNextFeature()) != null)
            {

                strInfo += string.Format("\n当前处理第{0}个: \n属性值：", oFeature.GetFID());

                // 获取要素中的属性表内容  
                for (int iField = 0; iField < iFieldCount; iField++)
                {
                    FieldDefn oFieldDefn = oDefn.GetFieldDefn(iField);
                    FieldType type = oFieldDefn.GetFieldType();
                    switch (type)
                    {
                        case FieldType.OFTString:
                            IntPtr pchar = OGR_F_GetFieldAsString(Feature.getCPtr(oFeature), iField);
                            string val = Marshal.PtrToStringAnsi(pchar);
                            strInfo += string.Format("{0}\t", val);
                            break;
                        case FieldType.OFTReal:
                            strInfo += string.Format("{0}\t", oFeature.GetFieldAsDouble(iField));
                            break;
                        case FieldType.OFTInteger:
                            strInfo += string.Format("{0}\t", oFeature.GetFieldAsInteger(iField));
                            break;
                        default:
                            strInfo += string.Format("{0}\t", oFeature.GetFieldAsString(iField));
                            break;
                    }
                }
                // 获取要素中的几何体  
                Geometry oGeometry = oFeature.GetGeometryRef();
                // 为了演示，只输出一个要素信息  
                break;
            }
            strInfo += "\n数据集关闭！";
        }
    }
}
