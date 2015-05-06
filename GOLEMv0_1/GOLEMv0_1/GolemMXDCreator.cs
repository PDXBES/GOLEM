using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace GOLEMv0_1
{
    class GolemMXDCreator
    {
        public static string CreateJPEGFromActiveView(string folder, string theImagePath)
        {
            System.String pathFileName = "\\\\Oberon\\GRP117\\IssacG\\GOLEM\\MXD_DWF.mxd";
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Desktop);
            IAoInitialize ao_ = new AoInitialize();
            IMapDocument pMapDoc = new MapDocument();
            String theMap = pathFileName;
            String thePath;
            String theFileName;
            double theWidth;
            double theHeight;
            
            IExportJPEG pExportJPEG_ = (IExportJPEG)new ExportJPEG();
            IExport pExport = (IExport)pExportJPEG_;
            IEnvelope pPixEnv = (IEnvelope)new Envelope();
            tagRECT pRect;
            int hDc;
            IActiveView pLayoutView;
            StreamWriter Swriter_;

            if (theMap == "")
            {
                return "";
            }

            //Get output path and output file name
            thePath = System.IO.Path.GetDirectoryName(theMap);
            theFileName = System.IO.Path.GetFileNameWithoutExtension(theMap);

            if (File.Exists(thePath + "\\tilecache_error.log"))
            {
                Swriter_ = File.AppendText(thePath + "\\tilecache_error.log");
            }
            else
            {
                Swriter_ = new StreamWriter(thePath + "\\tilecache_error.log");
            }

            try
            {
                if (ao_.IsProductCodeAvailable(esriLicenseProductCode.esriLicenseProductCodeStandard) == esriLicenseStatus.esriLicenseAvailable)
                {
                    ao_.Initialize(esriLicenseProductCode.esriLicenseProductCodeStandard);

                    if (pMapDoc.get_IsMapDocument(theMap))
                    {
                        //Open map and obtain layout
                        pMapDoc.Open(theMap);
                        pLayoutView = (IActiveView)pMapDoc.PageLayout;
                        //Get page width and height
                        IFeatureSelection features = (IFeatureSelection)pMapDoc.get_Layer(0, 0);

                        IQueryFilter qf = new QueryFilter();
                        qf.WhereClause = "";
                        features.SelectFeatures(qf, esriSelectionResultEnum.esriSelectionResultNew, false);
                        ISelectionSet theSelectionSet = features.SelectionSet;
                        ICursor cursor;
                        theSelectionSet.Search(null, true, out cursor);
                        // build an envelope containing a union of the selections
                        IFeatureCursor featureCursor = cursor as IFeatureCursor;
                        IFeature feature;
                        IEnvelope envelope = new EnvelopeClass();
                        while ((feature = featureCursor.NextFeature()) != null)
                        {
                            if (feature.Shape != null)
                            {
                                IGeometry geometry = feature.Shape;
                                IEnvelope featureExtent = geometry.Envelope;
                                envelope.Union(featureExtent);
                            }
                        }

                        features.Clear();


                        pMapDoc.ActiveView.Extent = envelope;
                        IEnvelope thisEnvelope = pMapDoc.ActiveView.Extent;
                        thisEnvelope.Expand(1.2, 1.2, true);
                        pMapDoc.ActiveView.Extent = thisEnvelope;
                        pMapDoc.ActiveView.Refresh();
                        pMapDoc.PageLayout.Page.QuerySize(out theWidth, out theHeight);

                        //Set up export parameters
                        theImagePath = folder + "\\" + theFileName + ".jpeg";
                        pExport.ExportFileName = theImagePath;
                        pExport.Resolution = 100;
                        //pExportVectorOptionEx = (IExportVectorOptionsEx)pExportJPEG_;
                        //pExportVectorOptionEx.ExportPictureSymbolOptions = esriPictureSymbolOptions.esriPSOVectorize;

                        pRect.left = 0;
                        pRect.top = 0;
                        pRect.right = (int)(theWidth * pExport.Resolution);
                        pRect.bottom = (int)(theHeight * pExport.Resolution);

                        pPixEnv.PutCoords(pRect.left, pRect.top, pRect.right, pRect.bottom);
                        pExport.PixelBounds = pPixEnv;

                        //Start export of layout
                        hDc = pExport.StartExporting();
                        //pLayoutView.Output(hDc, pExport.Resolution, pRect, Type.Missing, Type.Missing);
                        pLayoutView.Output(hDc, (System.Int16)96, ref pRect, null, null);
                        pExport.FinishExporting();
                    }
                    else
                    {
                        Swriter_.WriteLine("Export PDF (" + DateTime.Now.ToString() + ") - " + theFileName + " - Input map parameter is not an ArcGIS map document.");
                        Swriter_.Close();
                    }
                }
                else
                {
                    Swriter_.WriteLine("Export PDF (" + DateTime.Now.ToString() + ") - " + theFileName + " - Can not obtain an ArcInfo license.");
                    Swriter_.Close();
                    return "";
                }
                ao_.Shutdown();
            }
            catch (Exception ex)
            {
                ao_.Shutdown();
                Swriter_.WriteLine("Export PDF (" + DateTime.Now.ToString() + ") - " + theFileName + " - " + ex.Message);
            }
            return theImagePath;
            //parameter check
            /*if (activeView == null || !(pathFileName.EndsWith(".jpg")))
            {
                return false;
            }
            ESRI.ArcGIS.Output.IExport export = new ESRI.ArcGIS.Output.ExportJPEGClass();
            export.ExportFileName = pathFileName;

            // Microsoft Windows default DPI resolution
            export.Resolution = 96;
            ESRI.ArcGIS.esriSystem.tagRECT exportRECT = activeView.ExportFrame;
            ESRI.ArcGIS.Geometry.IEnvelope envelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();
            envelope.PutCoords(exportRECT.left, exportRECT.top, exportRECT.right, exportRECT.bottom);
            export.PixelBounds = envelope;
            System.Int32 hDC = export.StartExporting();
            activeView.Output(hDC, (System.Int16)export.Resolution, ref exportRECT, null, null);

            // Finish writing the export file and cleanup any intermediate files
            export.FinishExporting();
            export.Cleanup();*/

            //return true;
        }
    }
}
