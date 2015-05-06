using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace GOLEMv0_1
{
    class GolemMethods
    {
        //for most of the BASE models, ModelID should be 0, but we will leave it as a parameter here.
        public static void CreateSWMM5ModelBASE_DWF(int ModelID, int XBJECTIDtoDownsize, decimal XBJECTIDFraction, string fileName,
            float inputMultiplier, string CONNECTION_STR, int createMassiveDamage, int massiveDamageProtection, string decPlaces, int StormID, int DWFModel)
        {
            //Using the SWMMModelKey and the table GOLEM_SWMMModel, get the ModelName, ParentModel, and BaseModel
            //ModelName should be the ModelName from GOLEM_SWMMModel + _ParentModel + _BaseModel
            String ModelName = "GOLEM_BASE_DWF";
            String Options = "";
            String Junctions = "";
            String Storage = "";
            String Subcatchments = "";
            String SubAreas = "";
            String Infiltration = "";
            String Conduits = "";
            String XSections = "";
            String Weirs = "";
            String Orifices = "";
            String Tables = "";
            String Pumps = "";
            String Controls = "";
            String DWF = "";
            String CURVES = "";
            String Patterns = "";
            String PatternHourly = "";
            String PatternWeekend = "";
            String PatternDaily = "";
            String PatternMonthly = "";
            String PatternHourlyName = "";
            String PatternWeekendName = "";
            String PatternDailyName = "";
            String PatternMonthlyName = "";
            String Outfalls = "";
            String TimeSeries = "";
            String Coordinates = "";
            String Polygons = "";
            SqlDataReader sqlDR;
            GolemQueries theQueries = new GolemQueries();

            //[OPTIONS]
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetOptions;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    Options = Options + sqlDR["Tag"] + "\t" + sqlDR["TagValue"] + "\n";
                }
            }

            //[JUNCTIONS]
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetJunctions;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    Junctions = Junctions + sqlDR["Node_id"] + "\t" +
                        (sqlDR.GetDecimal(1)/*["IE"]*/).ToString(decPlaces) + "\t" +
                        (sqlDR.GetDecimal(2)/*["GE"]*/).ToString(decPlaces) + "\t" +
                        "0\t" +
                        "0\n";
                }
            }

            //Special Link [JUNCTIONS]
            //Create a new node for every node in special links that has a to_link_id in the list
            //of traced links.  Name it the negative of the to_link_id.  Give it the properties
            //of the "node" from the same record in special links.
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetSpecialLinkJunctions;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    Junctions = Junctions + sqlDR["Node_id"] + "\t" +
                        (sqlDR.GetDecimal(1)/*["IE"]*/).ToString(decPlaces) + "\t" +
                        (sqlDR.GetDecimal(2)/*["GE"]*/).ToString(decPlaces) + "\t" +
                        "0\t" +
                        "0\n";
                }
            }

            //Pumpstation [STORAGE]
            //Create a new node for every node in special links that has a to_link_id in the list
            //of traced links and is a pumpstation.  Name it the negative of the to_link_id.  Give it the properties
            //of the "node" from the same record in special links.
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetStorage;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    Storage = Storage + sqlDR["node_id"] + "\t" +
                        (sqlDR["ww_el"]).ToString() + "\t" +
                        (sqlDR["max_Depth"]).ToString() + "\t" +
                        "0\t" +
                        "FUNCTIONAL\t" +
                        (sqlDR["volume"]).ToString() + "\t" +
                        "0\n";
                }
            }

            //[OUTFALLS]
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetOutfalls;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    Outfalls = Outfalls + sqlDR["Node_id"] + "\t"
                                       + (sqlDR.GetDecimal(1)/*["IE"]*/).ToString(decPlaces) + "\t"
                                       + "NORMAL" + "\t"
                                       + "NO" + "\n";
                }
            }

            //[SUBCATCHMENTS_AU]
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetSubcatchmentsAU;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    Subcatchments = Subcatchments + sqlDR["ssc_id"] + "AU\t" +
                                                       "GAGE1" + "\t" +
                                                       sqlDR.GetInt32(1).ToString() + "\t" +
                                                       (sqlDR.GetDouble(2)/*["theArea"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(3)/*["ImpPct"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(4)/*["Flow_Length"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDecimal(5)/*["Slope"]*/* (decimal)100.00).ToString(decPlaces) + "\t" +
                                                       "0" + "\n";
                }
            }

            //[SUBCATCHMENTS_RX]
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetSubcatchmentsRX;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    Subcatchments = Subcatchments + sqlDR["ssc_id"] + "\t" +
                                                       "GAGE1" + "\t" +
                                                       sqlDR.GetInt32(1).ToString() + "\t" +
                                                       (sqlDR.GetDouble(2)/*["theArea"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetInt32(3)/*["ImpPct"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(4)/*["Flow_Length"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDecimal(5)/*["Slope"]*/* (decimal)100.00).ToString(decPlaces) + "\t" +
                                                       "0" + "\n";
                    SubAreas = SubAreas + sqlDR["ssc_id"] + "\t" +
                                                       "0.013\t" +
                                                       "0.026\t" +
                                                       "0.030\t" +
                                                       "0.017\t" +
                                                       "20.0" + "\t" +
                                                       "OUTLET" + "\n";
                }
            }

            //[SUBCATCHMENTS_PX]
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetSubcatchmentsPX;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    Subcatchments = Subcatchments + sqlDR["ssc_id"] + "\t" +
                                                       "GAGE1" + "\t" +
                                                       sqlDR.GetInt32(1).ToString() + "\t" +
                                                       (sqlDR.GetDouble(2)/*["theArea"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetInt32(3)/*["ImpPct"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(4)/*["Flow_Length"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDecimal(5)/*["Slope"]*/* (decimal)100.00).ToString(decPlaces) + "\t" +
                                                       "0" + "\n";
                    SubAreas = SubAreas + sqlDR["ssc_id"] + "\t" +
                                                       "0.013\t" +
                                                       "0.026\t" +
                                                       "0.030\t" +
                                                       "0.017\t" +
                                                       "20.0" + "\t" +
                                                       "OUTLET" + "\n";
                }
            }

            //[SUBCATCHMENTS_TU]
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                String PolygonString = "";
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetSubcatchmentsTU;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    Subcatchments = Subcatchments + sqlDR["ssc_id"] + "\t" +
                                                       "GAGE1" + "\t" +
                                                       sqlDR.GetInt32(1).ToString() + "\t" +
                                                       (sqlDR.GetDecimal(2)/*["theArea"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetInt32(3)/*["ImpPct"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(4)/*["Flow_Length"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDecimal(5)/*["Slope"]*/* (decimal)100.00).ToString(decPlaces) + "\t" +
                                                       "0" + "\n";
                    SubAreas = SubAreas + sqlDR["ssc_id"] + "\t" +
                                                       "0.013\t" +
                                                       "0.026\t" +
                                                       "0.030\t" +
                                                       "0.017\t" +
                                                       "20.0" + "\t" +
                                                       "OUTLET" + "\n";
                }
            }

            //[SUBAREAS]
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetSubareas;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    SubAreas = SubAreas + sqlDR["ssc_id"] + "AU\t" +
                                                       (sqlDR.GetDecimal(1)/*["impervious_roughness"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDecimal(2)/*["impervious_roughness"]*/).ToString(decPlaces) + "\t" +
                        //(sqlDR.GetDecimal(2)/*["pervious_roughness"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDecimal(3)/*["impervious_storage_depth_in"]*/).ToString(decPlaces) + "\t" +
                                                        (sqlDR.GetDecimal(4)/*["impervious_storage_depth_in"]*/).ToString(decPlaces) + "\t" +
                        //(sqlDR.GetDecimal(4)/*["pervious_storage_depth_in"]*/).ToString(decPlaces) + "\t" +
                                                       "20.0" + "\t" +
                                                       "OUTLET" + "\n";
                    //Parse the entire polygon string.  it looks like this:
                    //POLYGON ((7661589.00 681479.99, 7661589.00 681465.99, 7661591.00 681283.99))
                    //Polygons = Polygons + sqlDR["ssc_id"] + "AU\t" +
                }
            }

            //[INFILTRATION] assumes Green-Ampt
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetInfiltration;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    if (sqlDR.GetDecimal(2) != 0 && sqlDR.GetDecimal(3) != 0)
                    {
                        Infiltration = Infiltration + sqlDR["ssc_id"] + "AU\t" +
                                                        (sqlDR.IsDBNull(1) ? (decimal)4.25 : sqlDR.GetDecimal(1)/*["average_capillary_suction_in"]*/).ToString(decPlaces) + "\t" +
                                                        (sqlDR.GetDecimal(2)/*["satur_hydraulic_cond_inhr"]*/).ToString(decPlaces) + "\t" +
                                                        (sqlDR.GetDecimal(3)/*["initial_moisture_deficit"]*/).ToString(decPlaces) + "\n";
                    }
                }
            }

            //[CONDUITS]
            //If a link is included in the to_link_id column of special links, the upstream node of that link
            //is now the negative of the linkID
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetConduits;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    Conduits = Conduits + sqlDR["link_id"] + "\t" +
                                           sqlDR["US_Node_id"] + "\t" +
                                           sqlDR["DS_Node_id"] + "\t" +
                                           (sqlDR.GetDecimal(3)/*["Length"]*/).ToString(decPlaces) + "\t" +
                                           (sqlDR.GetDecimal(4)/*["N"]*/).ToString(decPlaces) + "\t" +
                                           (sqlDR.GetDecimal(5)/*["Z1"]*/).ToString(decPlaces) + "\t" +
                                           (sqlDR.GetDecimal(6)/*["Z2"]*/).ToString(decPlaces) + "\t" +
                                           "0\n";
                }
            }

            //[XSECTIONS]
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                int PipeCounter = 0;
                String thisShape = "";
                String thisDescriptiveString = "";
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetXSections;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    XSections = XSections + sqlDR["link_id"] + "\t";

                    switch ((string)sqlDR["Shape_name"])
                    {
                        case "CIRC":
                            thisShape = "CIRCULAR";
                            thisDescriptiveString = (sqlDR.GetDecimal(2) / (decimal)12.0/*["Height_in"]*/).ToString(decPlaces);
                            break;
                        case "RCTP":
                            thisShape = "RECT_CLOSED";
                            thisDescriptiveString = (sqlDR.GetDecimal(2) / (decimal)12.0/*["Height_in"]*/).ToString(decPlaces);
                            break;
                        case "EGG":
                            thisShape = "EGG";
                            thisDescriptiveString = (sqlDR.GetDecimal(2) / (decimal)12.0/*["Height_in"]*/).ToString(decPlaces);
                            break;
                        default:
                            thisShape = "CUSTOM";
                            thisDescriptiveString = (string)sqlDR["Shape_name"] + "_" + ((int)sqlDR.GetDecimal(1)).ToString() + "_" + ((int)sqlDR.GetDecimal(2)).ToString();
                            break;
                    }

                    //If we are damaging pipes or massive damaging pipes
                    if ((System.Int32)sqlDR["link_id"] == XBJECTIDtoDownsize || (XBJECTIDtoDownsize == -1 && createMassiveDamage == 1 && PipeCounter % massiveDamageProtection == 0))
                    {
                        XSections = XSections + "\t" +
                                    thisShape + "\t" +
                                    ((XBJECTIDFraction * sqlDR.GetDecimal(1)) / (decimal)12.0/*["diameter_or_width_in"]*/).ToString(decPlaces) + "\t" +
                                    thisDescriptiveString + "\t" +
                                    "0" + "\t" +
                                    "0" + "\t" +
                                    "1" + "\n";
                    }
                    else
                    {
                        XSections = XSections + "\t" +
                                    thisShape + "\t" +
                                    (sqlDR.GetDecimal(1) / (decimal)12.0/*["diameter_or_width_in"]*/).ToString(decPlaces) + "\t" +
                                    thisDescriptiveString + "\t" +
                                    "0" + "\t" +
                                    "0" + "\t" +
                                    "1" + "\n";
                    }
                    PipeCounter++;

                }
            }

            //[WEIRS]
            //Weirs (all special links, really) have a downstream node that is equal
            //to the negative of the to_link_id
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetWeirs;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    Weirs = Weirs + sqlDR["WeirName"] + "\t" +
                                           sqlDR["US_Node_id"] + "\t" +
                                           sqlDR["NewNode"] + "\t" +
                                           "TRANSVERSE" + "\t" +
                                           (sqlDR/*.GetDecimal(8)*/["YCrest"]) + "\t" +
                                           (sqlDR/*.GetDecimal(4)*/["Coeff"]) + "\t" +
                                           "NO" + "\t" +
                                           "0" + "\t" +
                                           "0" + "\n";
                    XSections = XSections + sqlDR["WeirName"] + "\t" +
                                           "RECT_OPEN" + "\t" +
                                           (sqlDR/*.GetDecimal(9)*/["YTop"]) + "\t" +
                                           (sqlDR/*.GetDecimal(7)*/["WLen"]) + "\t" +
                                           "0" + "\t" +
                                           "0" + "\n";
                }
            }

            //[ORIFICES]
            //Orifices (all special links, really) have a downstream node that is equal
            //to the negative of the to_link_id
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetOrifices;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    Orifices = Orifices + sqlDR["OrifName"] + "\t" +
                                           sqlDR["US_Node_id"] + "\t" +
                                           sqlDR["NewNode"] + "\t" +
                                           "BOTTOM" + "\t" +
                                           (sqlDR/*.GetDecimal(4)*/["ZP"]) + "\t" +
                                           (sqlDR/*.GetDecimal(7)*/["COrif"]) + "\t" +
                                           "NO" + "\t" +
                                           "0" + "\n";
                    XSections = XSections + sqlDR["OrifName"] + "\t" +
                                           checkOrifShape(Int32.Parse((string)sqlDR["ISqRnd"])) + "\t" +
                                           getOrifGeom1(Int32.Parse((string)sqlDR["ISqRnd"]), Double.Parse((string)sqlDR["AOrif"])) + "\t" +
                                           getOrifGeom2(Int32.Parse((string)sqlDR["ISqRnd"]), Double.Parse((string)sqlDR["AOrif"])) + "\t" +
                                           "0" + "\t" +
                                           "0" + "\n";
                }
            }

            //[PUMPS]
            //Pumps (all special links, really) have a downstream node that is equal
            //to the negative of the to_link_id
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetPumps;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();
                String thisNode = "";
                String lastNode = "";

                while (sqlDR.Read())
                {
                    Pumps = Pumps + sqlDR["DHI_PCurve"] + "\t" +
                                           sqlDR["Node_id"] + "\t" +
                                           sqlDR["NewNode"] + "\t" +
                                           sqlDR["DHI_PCurve"] + "\t" +
                                           "OFF" + "\t" +
                                           sqlDR["PON"] + "\t" +
                                           sqlDR["POFF"] + "\n";

                    thisNode = (string)sqlDR["NewNode"].ToString();
                    if (thisNode.CompareTo(lastNode) != 0)
                    {
                        Junctions = Junctions + sqlDR["NewNode"].ToString() + "\t" +
                        sqlDR["PON"] + "\n";
                    }
                    lastNode = thisNode;
                }
            }

            //[TABLES]
            //Pumps (all special links, really) have a downstream node that is equal
            //to the negative of the to_link_id
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetTables;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                String thisPumpType = "";
                String lastPumpType = "";
                while (sqlDR.Read())
                {
                    thisPumpType = (string)sqlDR["PumpType"];
                    if (thisPumpType.CompareTo(lastPumpType) != 0)
                    {
                        Tables = Tables + "\n" + sqlDR["PumpType"] + "\t" +
                            "PUMP4\t";
                    }

                    lastPumpType = (string)sqlDR["PumpType"];
                    Tables = Tables + sqlDR["Rating"] + "\t" +
                        sqlDR["Flow"] + "\t";
                }
            }


            //[PATTERNS]
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetDailyPatterns;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    PatternHourlyName = (string)sqlDR["Name"];
                    PatternHourly = PatternHourly + sqlDR["Name"] + "\t" +
                                                       "HOURLY" + "\t" +
                                                       (sqlDR.GetDouble(2)/*["Hour1"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(3)/*["Hour2"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(4)/*["Hour3"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(5)/*["Hour4"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(6)/*["Hour5"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(7)/*["Hour6"]*/).ToString(decPlaces) + "\n" +
                                                       sqlDR["Name"] + "\t" +
                                                       (sqlDR.GetDouble(8)/*["Hour7"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(9)/*["Hour8"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(10)/*["Hour9"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(11)/*["Hour10"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(12)/*["Hour11"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(13)/*["Hour12"]*/).ToString(decPlaces) + "\n" +
                                                       sqlDR["Name"] + "\t" +
                                                       (sqlDR.GetDouble(14)/*["Hour13"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(15)/*["Hour14"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(16)/*["Hour15"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(17)/*["Hour16"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(18)/*["Hour17"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(19)/*["Hour18"]*/).ToString(decPlaces) + "\n" +
                                                       sqlDR["Name"] + "\t" +
                                                       (sqlDR.GetDouble(20)/*["Hour19"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(21)/*["Hour20"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(22)/*["Hour21"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(23)/*["Hour22"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(24)/*["Hour23"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(25)/*["Hour24"]*/).ToString(decPlaces) + "\n";
                }
            }

            //Weekend [PATTERNS]
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetWeekendPatterns;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    PatternWeekendName = (string)sqlDR["Name"];
                    PatternWeekend = PatternWeekend + sqlDR["Name"] + "\t" +
                                                       "WEEKEND" + "\t" +
                                                       (sqlDR.GetDouble(2)/*["Hour1"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(3)/*["Hour2"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(4)/*["Hour3"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(5)/*["Hour4"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(6)/*["Hour5"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(7)/*["Hour6"]*/).ToString(decPlaces) + "\n" +
                                                       sqlDR["Name"] + "\t" +
                                                       (sqlDR.GetDouble(8)/*["Hour7"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(9)/*["Hour8"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(10)/*["Hour9"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(11)/*["Hour10"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(12)/*["Hour11"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(13)/*["Hour12"]*/).ToString(decPlaces) + "\n" +
                                                       sqlDR["Name"] + "\t" +
                                                       (sqlDR.GetDouble(14)/*["Hour13"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(15)/*["Hour14"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(16)/*["Hour15"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(17)/*["Hour16"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(18)/*["Hour17"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(19)/*["Hour18"]*/).ToString(decPlaces) + "\n" +
                                                       sqlDR["Name"] + "\t" +
                                                       (sqlDR.GetDouble(20)/*["Hour19"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(21)/*["Hour20"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(22)/*["Hour21"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(23)/*["Hour22"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(24)/*["Hour23"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(25)/*["Hour24"]*/).ToString(decPlaces) + "\n";
                }
            }

            //Week [PATTERNS]
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetWeeklyPatterns;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    PatternDailyName = (string)sqlDR["Name"];
                    PatternDaily = PatternDaily + sqlDR["Name"] + "\t" +
                                                       "DAILY" + "\t" +
                                                        (sqlDR.GetDouble(2)/*["Day1"]*/).ToString(decPlaces) + "\t" +
                                                        (sqlDR.GetDouble(3)/*["Day2"]*/).ToString(decPlaces) + "\t" +
                                                        (sqlDR.GetDouble(4)/*["Day3"]*/).ToString(decPlaces) + "\t" +
                                                        (sqlDR.GetDouble(5)/*["Day4"]*/).ToString(decPlaces) + "\t" +
                                                        (sqlDR.GetDouble(6)/*["Day5"]*/).ToString(decPlaces) + "\t" +
                                                        (sqlDR.GetDouble(7)/*["Day6"]*/).ToString(decPlaces) + "\t" +
                                                        (sqlDR.GetDouble(8)/*["Day7"]*/).ToString(decPlaces) + "\n";

                }
            }

            //Monthly [PATTERNS]
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetYearlyPatterns;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    PatternMonthlyName = (string)sqlDR["Name"];
                    PatternMonthly = PatternMonthly + sqlDR["Name"] + "\t" +
                                                       "MONTHLY" + "\t" +
                                                       (sqlDR.GetDouble(2)/*["Month1"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(3)/*["Month2"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(4)/*["Month3"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(5)/*["Month4"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(6)/*["Month5"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(7)/*["Month6"]*/).ToString(decPlaces) + "\n" +
                                                       sqlDR["Name"] + "\t" +
                                                       (sqlDR.GetDouble(8)/*["Month7"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(9)/*["Month8"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(10)/*["Month9"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(11)/*["Month10"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(12)/*["Month11"]*/).ToString(decPlaces) + "\t" +
                                                       (sqlDR.GetDouble(13)/*["Month12"]*/).ToString(decPlaces) + "\n";
                }
            }

            //[DWF]
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetDWF;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    DWF = DWF + sqlDR["US_Node_id"] + "\t"
                              + "FLOW" + "\t"
                              + (
                                    sqlDR.GetDecimal(1)
                                  * (decimal)inputMultiplier/*["AveFlow"]*/
                                ).ToString(decPlaces)
                              + "\t"
                              + "\""
                              + PatternMonthlyName
                              + "\""
                              + "\t"
                              + "\""
                              + PatternDailyName
                              + "\""
                              + "\t"
                              + "\""
                              + PatternHourlyName
                              + "\""
                              + "\t"
                              + "\""
                              + PatternWeekendName
                              + "\""
                              + "\n";
                }
            }

            //[CURVES]
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetCurves;

                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();


                String thisShapeType = "";
                String lastShapeType = "";
                while (sqlDR.Read())
                {
                    thisShapeType = (string)sqlDR["shape_name"];

                    CURVES = CURVES + sqlDR["shape_name"] + "\t";

                    if (thisShapeType.CompareTo(lastShapeType) != 0)
                    {
                        CURVES = CURVES + "Shape\t";
                    }
                    else
                    {
                        CURVES = CURVES + "\t";
                    }

                    lastShapeType = (string)sqlDR["shape_name"];
                    CURVES = CURVES + (sqlDR.GetDecimal(1)).ToString(decPlaces) + "\t" +
                                        (sqlDR.GetDecimal(2)).ToString(decPlaces) + "\n";

                }
            }

            //[TIMESERIES]
            /*  + "SERIES1 0:00	0	0:05	0	0:10	0 \n"
                            + "SERIES1 0:15	0	0:20	0	0:25	0 \n"
                            + "SERIES1 0:30	0	0:35	0	0:40	0 \n"*/
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetTimeSeries(StormID, 6);
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();
                int thisCount = 0;

                while (sqlDR.Read())
                {
                    if (thisCount == 0)
                    {
                        TimeSeries = TimeSeries + "SERIES1";
                    }
                    TimeSeries = TimeSeries + "\t" + (sqlDR.GetDouble(1)).ToString(decPlaces) + "\t" +
                               (sqlDR.GetDouble(2)).ToString(decPlaces);
                    if (thisCount == 2)
                    {
                        TimeSeries = TimeSeries + "\n";
                    }
                    thisCount = (thisCount + 1) % 3;
                }
            }

            //[COORDINATES]
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetCoordinates;
                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    Coordinates = Coordinates + sqlDR["node_id"] + "\t" +
                               sqlDR.GetDecimal(1).ToString(decPlaces) + "\t" +
                               sqlDR.GetDecimal(2).ToString(decPlaces) + "\n";
                }
            }

            //[PolygonsAU]
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetPolygonsAU;

                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    Polygons = Polygons + sqlDR["ssc_id"] + "AU\t" +
                                    (sqlDR.GetDecimal(1) + 100).ToString(decPlaces) + "\t" +
                                    (sqlDR.GetDecimal(2) + 200).ToString(decPlaces) + "\n" +
                                    sqlDR["ssc_id"] + "AU\t" +
                                    (sqlDR.GetDecimal(1) + 150).ToString(decPlaces) + "\t" +
                                    (sqlDR.GetDecimal(2) + 150).ToString(decPlaces) + "\n" +
                                    sqlDR["ssc_id"] + "AU\t" +
                                    (sqlDR.GetDecimal(1) + 100).ToString(decPlaces) + "\t" +
                                    (sqlDR.GetDecimal(2) + 100).ToString(decPlaces) + "\n" +
                                    sqlDR["ssc_id"] + "AU\t" +
                                    (sqlDR.GetDecimal(1) + 50).ToString(decPlaces) + "\t" +
                                    (sqlDR.GetDecimal(2) + 150).ToString(decPlaces) + "\n";
                }
            }

            //[PolygonsRX]
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetPolygonsRX;

                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    Polygons = Polygons + sqlDR["ssc_id"] + "\t" +
                                    (sqlDR.GetDecimal(1) + 50).ToString(decPlaces) + "\t" +
                                    (sqlDR.GetDecimal(2) + 150).ToString(decPlaces) + "\n" +
                                    sqlDR["ssc_id"] + "\t" +
                                    (sqlDR.GetDecimal(1) + 100).ToString(decPlaces) + "\t" +
                                    (sqlDR.GetDecimal(2) + 100).ToString(decPlaces) + "\n" +
                                    sqlDR["ssc_id"] + "\t" +
                                    (sqlDR.GetDecimal(1) + 50).ToString(decPlaces) + "\t" +
                                    (sqlDR.GetDecimal(2) + 50).ToString(decPlaces) + "\n" +
                                    sqlDR["ssc_id"] + "\t" +
                                    (sqlDR.GetDecimal(1) + 0).ToString(decPlaces) + "\t" +
                                    (sqlDR.GetDecimal(2) + 100).ToString(decPlaces) + "\n";
                }
            }

            //[PolygonsPX]
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetPolygonsPX;

                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    Polygons = Polygons + sqlDR["ssc_id"] + "\t" +
                                    (sqlDR.GetDecimal(1) + 150).ToString(decPlaces) + "\t" +
                                    (sqlDR.GetDecimal(2) + 150).ToString(decPlaces) + "\n" +
                                    sqlDR["ssc_id"] + "\t" +
                                    (sqlDR.GetDecimal(1) + 200).ToString(decPlaces) + "\t" +
                                    (sqlDR.GetDecimal(2) + 100).ToString(decPlaces) + "\n" +
                                    sqlDR["ssc_id"] + "\t" +
                                    (sqlDR.GetDecimal(1) + 150).ToString(decPlaces) + "\t" +
                                    (sqlDR.GetDecimal(2) + 50).ToString(decPlaces) + "\n" +
                                    sqlDR["ssc_id"] + "\t" +
                                    (sqlDR.GetDecimal(1) + 100).ToString(decPlaces) + "\t" +
                                    (sqlDR.GetDecimal(2) + 100).ToString(decPlaces) + "\n";
                }
            }

            //[PolygonsTU]
            using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = GolemQueries.GetPolygonsTU;

                conn.Open();
                cmd.CommandTimeout = 0;
                sqlDR = cmd.ExecuteReader();

                while (sqlDR.Read())
                {
                    Polygons = Polygons + sqlDR["ssc_id"] + "\t" +
                                    (sqlDR.GetDecimal(1) + 100).ToString(decPlaces) + "\t" +
                                    (sqlDR.GetDecimal(2) + 100).ToString(decPlaces) + "\n" +
                                    sqlDR["ssc_id"] + "\t" +
                                    (sqlDR.GetDecimal(1) + 150).ToString(decPlaces) + "\t" +
                                    (sqlDR.GetDecimal(2) + 50).ToString(decPlaces) + "\n" +
                                    sqlDR["ssc_id"] + "\t" +
                                    (sqlDR.GetDecimal(1) + 100).ToString(decPlaces) + "\t" +
                                    (sqlDR.GetDecimal(2) + 0).ToString(decPlaces) + "\n" +
                                    sqlDR["ssc_id"] + "\t" +
                                    (sqlDR.GetDecimal(1) + 50).ToString(decPlaces) + "\t" +
                                    (sqlDR.GetDecimal(2) + 50).ToString(decPlaces) + "\n";
                }
            }

            //place a copy of the xpx file into a designated folder
            if (fileName != "")
            {
                StreamWriter sw = System.IO.File.CreateText(fileName);

                sw.WriteLine("[TITLE]");
                sw.WriteLine(ModelName);
                sw.WriteLine();
                sw.WriteLine("[OPTIONS]");
                sw.WriteLine(Options);
                sw.WriteLine();
                sw.WriteLine("[RAINGAGES] \n" +
                              ";Name Format Interval SCF DataSource SourceName \n" +
                              ";=================================================== \n"
                              + "GAGE1 INTENSITY 0:05 1.0 TIMESERIES SERIES1\n");
                sw.WriteLine("[EVAPORATION] \n" +
                             "CONSTANT 0.02 \n");
                sw.WriteLine("[SUBCATCHMENTS] \n" +
                               ";Name Raingage Outlet Area %Imperv Width Slope \n" +
                               ";====================================================");
                sw.WriteLine(Subcatchments);
                sw.WriteLine("[SUBAREAS] \n" +
                               ";Subcatch N_Imp N_Perv S_Imp S_Perv %ZER RouteTo \n" +
                               ";=====================================================");
                sw.WriteLine(SubAreas);
                sw.WriteLine("[INFILTRATION] \n" +
                               ";Subcatch Suction Conduct InitDef \n" +
                               ";======================================");
                sw.WriteLine(Infiltration);
                sw.WriteLine("[JUNCTIONS] \n" +
                               ";Name Elev \n" +
                               ";============\n");
                sw.WriteLine(Junctions);
                sw.WriteLine("[OUTFALLS] \n" +
                              ";;               Invert     Outfall    Stage/Table      Tide\n" +
                               ";;Name           Elev.      Type       Time Series      Gate\n" +
                               ";;-------------- ---------- ---------- ---------------- ----\n");
                sw.WriteLine(Outfalls);
                sw.WriteLine("[Storage] \n" +
                               ";;Name InvertEL MaxDepth InitDepth Acoeff Aexp \n" +
                               ";==================================================\n");
                sw.WriteLine(Storage);
                sw.WriteLine("[CONDUITS] \n" +
                               ";Name Node1 Node2 Length N Z1 Z2 Q0  \n" +
                               ";===========================================================");
                sw.WriteLine(Conduits);
                sw.WriteLine("[ORIFICES] \n" +
                               ";;               Inlet            Outlet           Orifice      Crest      Disch.     Flap Open/Close \n" +
                               ";;Name           Node             Node             Type         Height     Coeff.     Gate Time      \n" +
                               ";;-------------- ---------------- ---------------- ------------ ---------- ---------- ---- ----------");
                sw.WriteLine(Orifices);
                sw.WriteLine("[PUMPS] \n" +
                               ";;               Inlet            Outlet           Pump         Curve      Init    \n" +
                               ";;Name           Node             Node             Type         Name       Status  \n" +
                               ";;-------------- ---------------- ---------------- ------------ ---------- -------- ");
                sw.WriteLine(Pumps);
                /*sw.WriteLine("[TABLES] \n" +
                               ";;Name           Type             X-Value          Y-Value   \n" +
                               ";;-------------- ---------------- ---------------- ----------  ");*/

                sw.WriteLine("[WEIRS] \n" +
                               ";;               Inlet            Outlet           Weir         Crest      Disch.     Flap End      End \n" +
                               ";;Name           Node             Node             Type         Height     Coeff.     Gate Con.     Coeff. \n" +
                               ";;-------------- ---------------- ---------------- ------------ ---------- ---------- ---- -------- ----------");
                sw.WriteLine(Weirs);
                sw.WriteLine("[XSECTIONS] \n" +
                               ";Link Type G1 G2 G3 G4 \n" +
                               ";=================================================== ");
                sw.WriteLine(XSections);

                sw.WriteLine("[DWF] \n" +
                               ";;                                Average    Time \n" +
                               ";;Node           Parameter        Value      Patterns \n" +
                               ";;-------------- ---------------- ---------- ---------- ");
                sw.WriteLine(DWF);
                sw.WriteLine("[CURVES] \n" +
                               ";;Name           Type       X-Value    Y-Value   \n" +
                                ";;-------------- ---------- ---------- ----------");
                sw.WriteLine(CURVES);
                sw.WriteLine(Tables);
                //If we are doing dry weather flow, don't do a timeseries
                if (DWFModel == 0)
                {
                    sw.WriteLine("[TIMESERIES] \n" +
                                 ";Rainfall time series");
                    sw.WriteLine(TimeSeries);
                }
                else
                {
                    sw.WriteLine("[TIMESERIES] \n" +
                                 ";Rainfall time series\nSERIES1	0.0000	0.0000");
                }
                sw.WriteLine("[PATTERNS] \n" +
                               ";;Name           Type       Multipliers \n" +
                               ";;-------------- ---------- -----------");
                sw.WriteLine(PatternHourly);
                sw.WriteLine(PatternWeekend);
                sw.WriteLine(PatternDaily);
                sw.WriteLine(PatternMonthly);
                sw.WriteLine("[REPORT]  \n" +
                               "INPUT YES  \n" +
                               "SUBCATCHMENTS ALL  \n" +
                               "NODES ALL  \n" +
                               "LINKS ALL");
                sw.WriteLine("[COORDINATES]  \n" +
                                ";;Node           X-Coord            Y-Coord    \n" +
                                ";;-------------- ------------------ ------------------");
                sw.WriteLine(Coordinates);
                sw.WriteLine("[Polygons]  \n" +
                                ";;Subcatchment   X-Coord            Y-Coord    \n" +
                                ";;-------------- ------------------ ------------------");
                sw.WriteLine(Polygons);
                sw.Close();
            }

        }

        public static int ResizePipe(string oldFileName, string newFileName, string pipeName, float resizeRatio, string decPlaces)
        {
            using (var input = File.OpenText(oldFileName))
            {
                int inXsections = 0;
                int beyondXsections = 0;
                using (var output = new StreamWriter(newFileName))
                {
                    string line;
                    while (null != (line = input.ReadLine()))
                    {
                        if (line.Contains("[") && inXsections == 1)
                        {
                            beyondXsections = 1;
                        }
                        if (line.Contains("[XSECTIONS]"))
                        {
                            inXsections = 1;
                        }
                        if (beyondXsections == 0 && inXsections == 1 && line.Length > pipeName.Length + 1)
                        {
                            if (String.Compare(line.Substring(0, pipeName.Length + 1), pipeName + '\t') == 0)
                            {
                                //parse out the size column
                                //pipeName//Shape//Size1//Size2//Zero1//Zero2//One1
                                line = line.Trim();
                                Regex r = new Regex("\t+");
                                string[] items = r.Split(line);
                                decimal size = decimal.Parse(items[2]);
                                size = size * (decimal)resizeRatio;
                                line = items[0] + '\t' + items[1] + '\t' + size.ToString(decPlaces) + '\t' + items[3] + '\t' + items[4] + '\t' + items[5] + '\t' + items[6];
                                output.WriteLine(line);
                            }
                            else
                            {
                                output.WriteLine(line);
                            }
                        }
                        else
                        {
                            output.WriteLine(line);
                        }
                    }
                }
            }

            return 0;
        }

        public static int ModifyMannings(string oldFileName, string newFileName, string pipeName, float newMannings, string decPlaces)
        {
            using (var input = File.OpenText(oldFileName))
            {
                int inConduits = 0;
                int beyondConduits = 0;
                using (var output = new StreamWriter(newFileName))
                {
                    string line;
                    while (null != (line = input.ReadLine()))
                    {
                        if (line.Contains("[") && inConduits == 1)
                        {
                            beyondConduits = 1;
                        }
                        if (line.Contains("[CONDUITS]"))
                        {
                            inConduits = 1;
                            output.WriteLine(line);
                            continue;
                        }
                        if (beyondConduits == 0 && inConduits == 1 && line.Length > pipeName.Length + 1)
                        {
                            if (String.Compare(line.Substring(0, pipeName.Length + 1), pipeName + '\t') == 0 || (pipeName == "0" && line[0] != ';'))
                            {
                                //parse out the Manning N column
                                //Name//InletNode//OutletNode//Length//ManningN//InletOffset//OutletOffset//Init.Flow//Max.Flow (Not In current models)
                                line = line.Trim();
                                Regex r = new Regex("\t+");
                                string[] items = r.Split(line);
                                line = items[0] + '\t' + items[1] + '\t' + items[2] + '\t' + items[3] + '\t' + newMannings.ToString(decPlaces) + '\t' + items[5] + '\t' + items[6] + '\t' + items[7];
                                output.WriteLine(line);
                            }
                            else
                            {
                                output.WriteLine(line);
                            }
                        }
                        else
                        {
                            output.WriteLine(line);
                        }
                    }
                }
            }

            return 0;
        }

        public static void RunModelsAndProcessReports(string CONNECTION_STR, int resultsID)
        {
            resultsID = 0;
            //get the folder
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

            folderBrowserDialog1.ShowDialog();

            if (folderBrowserDialog1.SelectedPath != "")
            {
                //get a list of all the .inp files in the folder
                string[] filePaths = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.inp");
                //for every .inp file in the folder:

                foreach (string filename in filePaths)
                {
                    string filenameWithoutPath = System.IO.Path.GetFileName(filename);
                    Process p = new Process();
                    // Redirect the output stream of the child process.
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.WorkingDirectory = folderBrowserDialog1.SelectedPath;
                    p.StartInfo.FileName = "swmm5";
                    p.StartInfo.Arguments =
                                filenameWithoutPath + " " +
                                filenameWithoutPath.Substring(0, filenameWithoutPath.Length - 4) + "_.rpt " +
                                filenameWithoutPath.Substring(0, filenameWithoutPath.Length - 4) + "_.out ";
                    p.Start();
                    // Read the output stream first and then wait.
                    p.WaitForExit();
                    //run SWMM5 on all the inp files.  First make sure SWMM5 is accessible to the folder
                }

                //get a list of all the .rpt files in the folder
                filePaths = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.rpt");
                //for every .rpt file in the folder:
                foreach (string filename in filePaths)
                {
                    string shortFile = System.IO.Path.GetFileName(filename);

                    //parse the modelID from the filename
                    Regex reg = new Regex("_+");
                    string[] IDs = reg.Split(shortFile);
                    int modelID = int.Parse(IDs[1]);
                    int linkID = int.Parse(IDs[2]);
                    int constriction = int.Parse(IDs[3]);
                    //parse the report file for the 'Node Depth Summary'
                    //Parse again for the ------------
                    //Parse again for the ------------
                    //Following will be a list of nodes and Max HGL infos, like so (WS = WhiteSpace, I = Ignore)
                    //Node WS I WS I WS I WS MaxHGL WS I WS I
                    StreamReader reader = new StreamReader(folderBrowserDialog1.SelectedPath + "\\" + shortFile);
                    string line;
                    //Add a new GOLEM_RESULTS line, and get the key ID for that new line
                    using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
                    {
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "INSERT INTO [GOLEM].[GIS].GOLEM_RESULTS ([ModelID], [Variable1], [Variable2], [Description]) " +
                                            " OUTPUT INSERTED.GOLEMKEY " +
                                            " VALUES (" + IDs[1] + "," + IDs[2] + "," + IDs[3] + "," + "'" + shortFile + "') ";
                        conn.Open();
                        cmd.CommandTimeout = 0;
                        resultsID = (int)cmd.ExecuteScalar();
                    }

                    while ((line = reader.ReadLine()) != null)
                    {
                        /************
                        Link Summary
                        ************/
                        if (line.Contains("Link Summary"))
                        {
                            for (int loop = 0; loop < 4; loop++)
                            {
                                line = reader.ReadLine();
                            }
                            while (line.Count() > 4)
                            {
                                StringBuilder sb = new StringBuilder();

                                //get the variables
                                line = line.Trim();
                                //string[] items = line.Split();
                                Regex r = new Regex(" +");
                                string[] items = r.Split(line);
                                sb.Append(resultsID.ToString());
                                if (items[3] == "CONDUIT")
                                {
                                    for (int loop = 0; loop <= 6; loop++)
                                    {
                                        sb.Append(',');
                                        if (loop == 3)
                                        {
                                            sb.Append("'");
                                            sb.Append(items[loop]);
                                            sb.Append("'");
                                        }
                                        else
                                        {
                                            sb.Append(items[loop]);
                                        }
                                    }
                                    using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
                                    {
                                        SqlCommand cmd = conn.CreateCommand();
                                        cmd.CommandType = System.Data.CommandType.Text;
                                        cmd.CommandText = "INSERT INTO GIS.GOLEM_RESULTS_LINKS (ResultsID, Name, FromNode, ToNode, Type, Length, Slope, Roughness) " +
                                                            "VALUES (" + sb + "); ";
                                        conn.Open();
                                        cmd.CommandTimeout = 0;
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                line = reader.ReadLine();
                            }
                        }

                        /*********************
                         Cross Section Summary
                         *********************/
                        if (line.Contains("Cross Section Summary"))
                        {
                            for (int loop = 0; loop < 5; loop++)
                            {
                                line = reader.ReadLine();
                            }
                            while (line.Count() > 4)
                            {
                                StringBuilder sb = new StringBuilder();

                                //get the variables
                                line = line.Trim();
                                //string[] items = line.Split();
                                Regex r = new Regex(" +");
                                string[] items = r.Split(line);
                                sb.Append(resultsID.ToString());
                                for (int loop = 0; loop <= 7; loop++)
                                {
                                    sb.Append(',');
                                    if (loop == 1)
                                    {
                                        sb.Append("'");
                                        sb.Append(items[loop]);
                                        sb.Append("'");
                                    }
                                    else
                                    {
                                        sb.Append(items[loop]);
                                    }
                                }

                                using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
                                {
                                    SqlCommand cmd = conn.CreateCommand();
                                    cmd.CommandType = System.Data.CommandType.Text;
                                    cmd.CommandText = "INSERT INTO GIS.GOLEM_RESULTS_CROSSSECTIONS (ResultsID, Conduit, Shape, FullDepth, FullArea, HydRad, MaxWidth, NoOfBarrels, FullFlow) " +
                                                        "VALUES (" + sb + "); ";
                                    conn.Open();
                                    cmd.CommandTimeout = 0;
                                    cmd.ExecuteNonQuery();
                                }

                                line = reader.ReadLine();
                            }
                        }

                        /******************
                         Node Depth Summary
                         ******************/

                        if (line.Contains("Node Depth Summary"))
                        {
                            for (int loop = 0; loop < 8; loop++)
                            {
                                line = reader.ReadLine();
                            }
                            while (line.Count() > 4)
                            {
                                StringBuilder sb = new StringBuilder();

                                //get the variables
                                line = line.Trim();
                                Regex r = new Regex(" +");
                                string[] items = r.Split(line);
                                sb.Append(resultsID.ToString());
                                for (int loop = 0; loop <= 6; loop++)
                                {
                                    sb.Append(',');
                                    if (loop == 1 || loop == 6)
                                    {
                                        sb.Append("'");
                                        sb.Append(items[loop]);
                                        sb.Append("'");
                                    }
                                    else
                                    {
                                        sb.Append(items[loop]);
                                    }
                                }

                                using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
                                {
                                    SqlCommand cmd = conn.CreateCommand();
                                    cmd.CommandType = System.Data.CommandType.Text;
                                    cmd.CommandText = "INSERT INTO GIS.GOLEM_RESULTS_NODEDEPTH (ResultsID, [Node],[Type],[AverageDepthFeet],[MaximumDepthFeet],[MaximumHGLFeet],[TimeOfMaxOccurenceDays],[TimeOfMaxOccurenceHrMin]) " +
                                                        "VALUES (" + sb + "); ";
                                    conn.Open();
                                    cmd.CommandTimeout = 0;
                                    cmd.ExecuteNonQuery();
                                }

                                line = reader.ReadLine();
                            }
                        }

                        /*********************
                        Node Flooding Summary
                        *********************/
                        if (line.Contains("Node Flooding Summary"))
                        {
                            int NodeFlooding = 1;

                            for (int loop = 0; loop < 10; loop++)
                            {
                                line = reader.ReadLine();
                                if (line.Contains("No nodes were flooded."))
                                {
                                    NodeFlooding = 0;
                                }
                            }
                            while (line.Count() > 4 && NodeFlooding == 1)
                            {
                                StringBuilder sb = new StringBuilder();

                                //get the variables
                                line = line.Trim();
                                Regex r = new Regex(" +");
                                string[] items = r.Split(line);
                                sb.Append(resultsID.ToString());
                                for (int loop = 0; loop <= 6; loop++)
                                {
                                    sb.Append(',');
                                    if (loop == 4)
                                    {
                                        sb.Append("'");
                                        sb.Append(items[loop]);
                                        sb.Append("'");
                                    }
                                    else
                                    {
                                        sb.Append(items[loop]);
                                    }
                                }

                                using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
                                {
                                    SqlCommand cmd = conn.CreateCommand();
                                    cmd.CommandType = System.Data.CommandType.Text;
                                    cmd.CommandText = "INSERT INTO GIS.GOLEM_RESULTS_NODEFLOODING (ResultsID, [Node],[HoursFlooded],[MaxRateCFS],[TimeOfMaxOccurenceDays],[TimeOfMaxOccurenceHrMin], TotalFloodVolMegaGallons, MaxPondedDepthFeet) " +
                                                        "VALUES (" + sb + "); ";
                                    conn.Open();
                                    cmd.CommandTimeout = 0;
                                    cmd.ExecuteNonQuery();
                                }

                                line = reader.ReadLine();
                            }
                        }

                        /********************
                         Link Flow Summary
                         ********************/
                        if (line.Contains("Link Flow Summary"))
                        {
                            for (int loop = 0; loop < 8; loop++)
                            {
                                line = reader.ReadLine();
                            }
                            while (line.Count() > 4)
                            {
                                StringBuilder sb = new StringBuilder();

                                //get the variables
                                line = line.Trim();
                                //string[] items = line.Split();
                                Regex r = new Regex(" +");
                                string[] items = r.Split(line);
                                sb.Append(resultsID.ToString());
                                if (items[1] == "CONDUIT")
                                {
                                    for (int loop = 0; loop <= 7; loop++)
                                    {
                                        sb.Append(',');
                                        if (loop == 1 || loop == 4)
                                        {
                                            sb.Append("'");
                                            sb.Append(items[loop]);
                                            sb.Append("'");
                                        }
                                        else
                                        {
                                            float theValue;

                                            if (float.TryParse(items[loop], out theValue))
                                            {
                                                sb.Append(theValue.ToString());
                                            }
                                            else
                                            {
                                                sb.Append("NULL");
                                            }
                                        }
                                    }

                                    using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
                                    {
                                        SqlCommand cmd = conn.CreateCommand();
                                        cmd.CommandType = System.Data.CommandType.Text;
                                        cmd.CommandText = "INSERT INTO GIS.GOLEM_RESULTS_LINKFLOW (ResultsID, Link, Type, MaximumFlowCFS, TimeOfMaxOccurenceDays, TimeOfMaxOccurenceHrMin, MaximumVelocFtSec, MaxFullFlowRatio, MaxFullDepthRatio) " +
                                                            "VALUES (" + sb + "); ";
                                        conn.Open();
                                        cmd.CommandTimeout = 0;
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                line = reader.ReadLine();
                            }
                        }
                    }
                }
            }
        }

     static string checkOrifShape(int theShape)
     {
         if (theShape == 0)
         {
             return "CIRCULAR";
         }
         else
         {
             return "RECT_CLOSED";
         }
     }

     static string getOrifGeom1(int theShape, double theArea)
     {
         if (theShape == 0)
         {
             return Math.Sqrt(theArea*4.0/Math.PI).ToString();
         }
         else
         {
             return Math.Sqrt(theArea).ToString();
         }
     }

     static string getOrifGeom2(int theShape, double theArea)
     {
         if (theShape == 0)
         {
             return "0";
         }
         else
         {
             return Math.Sqrt(theArea).ToString();
         }
     }

     public static void RunModelsAndProcessReportsAuto(string CONNECTION_STR, int resultsID, string filename)
     {
         //resultsID = 0;

         if (filename != "")
         {
            string filenameWithoutPath = System.IO.Path.GetFileName(filename);
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(filename);// folderBrowserDialog1.SelectedPath;
            p.StartInfo.FileName = "swmm5";
            p.StartInfo.Arguments =
                        filenameWithoutPath + " " +
                        filenameWithoutPath.Substring(0, filenameWithoutPath.Length - 4) + "_.rpt " +
                        filenameWithoutPath.Substring(0, filenameWithoutPath.Length - 4) + "_.out ";
            p.Start();
            // Read the output stream first and then wait.
            p.WaitForExit();

                //parse the report file for the 'Node Depth Summary'
                //Parse again for the ------------
                //Parse again for the ------------
                //Following will be a list of nodes and Max HGL infos, like so (WS = WhiteSpace, I = Ignore)
                //Node WS I WS I WS I WS MaxHGL WS I WS I
            StreamReader reader = new StreamReader(filename.Substring(0, filename.Length - 4) + "_.rpt ");
                string line;
                //Add a new GOLEM_RESULTS line, and get the key ID for that new line
                /*using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "INSERT INTO [GOLEM].[GIS].GOLEM_RESULTS ([ModelID], [Variable1], [Variable2], [Description]) " +
                                        " OUTPUT INSERTED.GOLEMKEY " +
                                        " VALUES (" + IDs[1] + "," + IDs[2] + "," + IDs[3] + "," + "'" + shortFile + "') ";
                    conn.Open();
                    cmd.CommandTimeout = 0;
                    resultsID = (int)cmd.ExecuteScalar();
                }*/
            
                 while ((line = reader.ReadLine()) != null)
                 {
                     /************
                     Link Summary
                     ************/
                     if (line.Contains("Link Summary"))
                     {
                         for (int loop = 0; loop < 4; loop++)
                         {
                             line = reader.ReadLine();
                         }
                         while (line.Count() > 4)
                         {
                             StringBuilder sb = new StringBuilder();

                             //get the variables
                             line = line.Trim();
                             //string[] items = line.Split();
                             Regex r = new Regex(" +");
                             string[] items = r.Split(line);
                             sb.Append(resultsID.ToString());
                             if (items[3] == "CONDUIT")
                             {
                                 for (int loop = 0; loop <= 6; loop++)
                                 {
                                     sb.Append(',');
                                     if (loop == 3)
                                     {
                                         sb.Append("'");
                                         sb.Append(items[loop]);
                                         sb.Append("'");
                                     }
                                     else
                                     {
                                         sb.Append(items[loop]);
                                     }
                                 }
                                 using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
                                 {
                                     SqlCommand cmd = conn.CreateCommand();
                                     cmd.CommandType = System.Data.CommandType.Text;
                                     cmd.CommandText = "INSERT INTO GIS.GOLEM_RESULTS_LINKS (ResultsID, Name, FromNode, ToNode, Type, Length, Slope, Roughness) " +
                                                         "VALUES (" + sb + "); ";
                                     conn.Open();
                                     cmd.CommandTimeout = 0;
                                     cmd.ExecuteNonQuery();
                                 }
                             }

                             line = reader.ReadLine();
                         }
                     }

                     /*********************
                      Cross Section Summary
                      *********************/
                     if (line.Contains("Cross Section Summary"))
                     {
                         for (int loop = 0; loop < 5; loop++)
                         {
                             line = reader.ReadLine();
                         }
                         while (line.Count() > 4)
                         {
                             StringBuilder sb = new StringBuilder();

                             //get the variables
                             line = line.Trim();
                             //string[] items = line.Split();
                             Regex r = new Regex(" +");
                             string[] items = r.Split(line);
                             sb.Append(resultsID.ToString());
                             for (int loop = 0; loop <= 7; loop++)
                             {
                                 sb.Append(',');
                                 if (loop == 1)
                                 {
                                     sb.Append("'");
                                     sb.Append(items[loop]);
                                     sb.Append("'");
                                 }
                                 else
                                 {
                                     sb.Append(items[loop]);
                                 }
                             }

                             using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
                             {
                                 SqlCommand cmd = conn.CreateCommand();
                                 cmd.CommandType = System.Data.CommandType.Text;
                                 cmd.CommandText = "INSERT INTO GIS.GOLEM_RESULTS_CROSSSECTIONS (ResultsID, Conduit, Shape, FullDepth, FullArea, HydRad, MaxWidth, NoOfBarrels, FullFlow) " +
                                                     "VALUES (" + sb + "); ";
                                 conn.Open();
                                 cmd.CommandTimeout = 0;
                                 cmd.ExecuteNonQuery();
                             }

                             line = reader.ReadLine();
                         }
                     }

                     /******************
                      Node Depth Summary
                      ******************/

                     if (line.Contains("Node Depth Summary"))
                     {
                         for (int loop = 0; loop < 8; loop++)
                         {
                             line = reader.ReadLine();
                         }
                         while (line.Count() > 4)
                         {
                             StringBuilder sb = new StringBuilder();

                             //get the variables
                             line = line.Trim();
                             Regex r = new Regex(" +");
                             string[] items = r.Split(line);
                             sb.Append(resultsID.ToString());
                             for (int loop = 0; loop <= 6; loop++)
                             {
                                 sb.Append(',');
                                 if (loop == 1 || loop == 6)
                                 {
                                     sb.Append("'");
                                     sb.Append(items[loop]);
                                     sb.Append("'");
                                 }
                                 else
                                 {
                                     sb.Append(items[loop]);
                                 }
                             }

                             using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
                             {
                                 SqlCommand cmd = conn.CreateCommand();
                                 cmd.CommandType = System.Data.CommandType.Text;
                                 cmd.CommandText = "INSERT INTO GIS.GOLEM_RESULTS_NODEDEPTH (ResultsID, [Node],[Type],[AverageDepthFeet],[MaximumDepthFeet],[MaximumHGLFeet],[TimeOfMaxOccurenceDays],[TimeOfMaxOccurenceHrMin]) " +
                                                     "VALUES (" + sb + "); ";
                                 conn.Open();
                                 cmd.CommandTimeout = 0;
                                 cmd.ExecuteNonQuery();
                             }

                             line = reader.ReadLine();
                         }
                     }

                     /*********************
                     Node Flooding Summary
                     *********************/
                     if (line.Contains("Node Flooding Summary"))
                     {
                         int NodeFlooding = 1;

                         for (int loop = 0; loop < 10; loop++)
                         {
                             line = reader.ReadLine();
                             if (line.Contains("No nodes were flooded."))
                             {
                                 NodeFlooding = 0;
                             }
                         }
                         while (line.Count() > 4 && NodeFlooding == 1)
                         {
                             StringBuilder sb = new StringBuilder();

                             //get the variables
                             line = line.Trim();
                             Regex r = new Regex(" +");
                             string[] items = r.Split(line);
                             sb.Append(resultsID.ToString());
                             for (int loop = 0; loop <= 6; loop++)
                             {
                                 sb.Append(',');
                                 if (loop == 4)
                                 {
                                     sb.Append("'");
                                     sb.Append(items[loop]);
                                     sb.Append("'");
                                 }
                                 else
                                 {
                                     sb.Append(items[loop]);
                                 }
                             }

                             using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
                             {
                                 SqlCommand cmd = conn.CreateCommand();
                                 cmd.CommandType = System.Data.CommandType.Text;
                                 cmd.CommandText = "INSERT INTO GIS.GOLEM_RESULTS_NODEFLOODING (ResultsID, [Node],[HoursFlooded],[MaxRateCFS],[TimeOfMaxOccurenceDays],[TimeOfMaxOccurenceHrMin], TotalFloodVolMegaGallons, MaxPondedDepthFeet) " +
                                                     "VALUES (" + sb + "); ";
                                 conn.Open();
                                 cmd.CommandTimeout = 0;
                                 cmd.ExecuteNonQuery();
                             }

                             line = reader.ReadLine();
                         }
                     }

                     /********************
                      Link Flow Summary
                      ********************/
                     if (line.Contains("Link Flow Summary"))
                     {
                         for (int loop = 0; loop < 8; loop++)
                         {
                             line = reader.ReadLine();
                         }
                         while (line.Count() > 4)
                         {
                             StringBuilder sb = new StringBuilder();

                             //get the variables
                             line = line.Trim();
                             //string[] items = line.Split();
                             Regex r = new Regex(" +");
                             string[] items = r.Split(line);
                             sb.Append(resultsID.ToString());
                             if (items[1] == "CONDUIT")
                             {
                                 for (int loop = 0; loop <= 7; loop++)
                                 {
                                     sb.Append(',');
                                     if (loop == 1 || loop == 4)
                                     {
                                         sb.Append("'");
                                         sb.Append(items[loop]);
                                         sb.Append("'");
                                     }
                                     else
                                     {
                                         float theValue;

                                         if (float.TryParse(items[loop], out theValue))
                                         {
                                             sb.Append(theValue.ToString());
                                         }
                                         else
                                         {
                                             sb.Append("NULL");
                                         }
                                     }
                                 }

                                 using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
                                 {
                                     SqlCommand cmd = conn.CreateCommand();
                                     cmd.CommandType = System.Data.CommandType.Text;
                                     cmd.CommandText = "INSERT INTO GIS.GOLEM_RESULTS_LINKFLOW (ResultsID, Link, Type, MaximumFlowCFS, TimeOfMaxOccurenceDays, TimeOfMaxOccurenceHrMin, MaximumVelocFtSec, MaxFullFlowRatio, MaxFullDepthRatio) " +
                                                         "VALUES (" + sb + "); ";
                                     conn.Open();
                                     cmd.CommandTimeout = 0;
                                     cmd.ExecuteNonQuery();
                                 }
                             }

                             line = reader.ReadLine();
                         }
                     }
                 }
             }
         }
     }
}
