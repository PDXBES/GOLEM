using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GOLEMv0_1
{
    class GolemQueries
    {
        //Get a list of junctions (nodes) in this trace.
        public const string GetJunctions = "SELECT Node_id, MIN(IE) AS IE, CASE WHEN (MAX(ground_elevation) - MIN(IE) ) < 2 THEN 2 ELSE  MAX(ground_elevation) - MIN(IE) END AS GE " +
                               "FROM( " +
                               "  SELECT Node_id, ISNULL(ds_invert,0) AS IE, ISNULL(ground_elevation,0) AS ground_elevation " +
                               "  FROM   [GOLEM].[GIS].[GOLEM_TracedNODES_BASE] " +
                               "         INNER JOIN [GOLEM].[GIS].[GOLEM_TracedLinks_BASE] " +
                               "         ON Node_id = DS_Node_id " +
                               "  UNION  " +
                               "  SELECT Node_id, ISNULL(us_invert,0) AS IE, ISNULL(ground_elevation,0) AS ground_elevation " +
                               "  FROM   [GOLEM].[GIS].[GOLEM_TracedNODES_BASE] " +
                               "         INNER JOIN [GOLEM].[GIS].[GOLEM_TracedLinks_BASE] " +
                               "         ON Node_id = US_Node_id " +
                               "  ) AS X " +
                               "WHERE Node_id NOT IN (SELECT Node_id FROM [GOLEM].[GIS].GOLEM_TracedOutfalls_BASE) " +
                               "      AND Node_id NOT IN (SELECT Node_id FROM [GOLEM].[GIS].GOLEM_TracedPumps_BASE) " +
                               "GROUP BY NODE_id ";
        public const string GetOptions = "SELECT	Tag, TagValue " +
                               "FROM	[GOLEM].[GIS].[GOLEM_SWMM5_OPTIONS] " +
                               "WHERE	ModelID = 1";
        public const string GetSpecialLinkJunctions = "SELECT	-to_link_id AS node_id, IE, GE " +
                                "FROM	( " +
			                                "SELECT	DISTINCT to_link_id, node_id " +
			                                "FROM	[GOLEM].[GIS].[GOLEM_TracedSpecialLinks_BASE] " +
		                                ") AS T " +
		                                "INNER JOIN  " +
		                                "( " +
			                                "SELECT	Node_id,  " +
					                                "MIN(IE) AS IE,  " +
					                                "MAX(ground_elevation) - MIN(IE) AS GE  " +
			                                "FROM " +
					                                "( " +
						                                "SELECT	Node_id,  " +
								                                "ds_invert AS IE,  " +
								                                "ground_elevation  " +
						                                "FROM	[GOLEM].[GIS].[GOLEM_TracedNODES_BASE]  " +
								                                "INNER JOIN  " +
								                                "[GOLEM].[GIS].[GOLEM_TracedLinks_BASE]  " +
								                                "ON	Node_id = DS_Node_id " +
						                                "UNION   " +
						                                "SELECT	Node_id,  " +
								                                "us_invert AS IE,  " +
								                                "ground_elevation  " +
						                                "FROM	[GOLEM].[GIS].[GOLEM_TracedNODES_BASE]  " +
								                                "INNER JOIN  " +
								                                "[GOLEM].[GIS].[GOLEM_TracedLinks_BASE]  " +
								                                "ON Node_id = US_Node_id  " +
					                                ") AS X  " +
			                                "WHERE	Node_id  " +
					                                "NOT IN  " +
					                                "( " +
						                                "SELECT	Node_id  " +
						                                "FROM	[GOLEM].[GIS].GOLEM_TracedOutfalls_BASE " +
					                                ")  " +
                                                    " AND Node_id  " +
					                                "NOT IN  " +
					                                "( " +
						                                "SELECT	Node_id  " +
                                                        "FROM	[GOLEM].[GIS].GOLEM_TracedPumps_BASE " +
					                                ")  " +
			                                "GROUP BY NODE_id  " +
		                                ") AS A " +
		                                "ON A.node_id = T.Node_id";
        public const string GetStorage = "SELECT	node_id, ww_el, bypass_el - ww_el as max_Depth, volume " +
                                 "FROM	[GOLEM].[GIS].[GOLEM_TracedStorage_BASE] ";
        public const string GetOutfalls = "SELECT Node_id, MIN(IE) AS IE " +
                               "FROM( " +
                               "  SELECT Node_id, ds_invert AS IE " +
                               "  FROM   [GOLEM].[GIS].[GOLEM_TracedOutfalls_BASE] " +
                               "         INNER JOIN [GOLEM].[GIS].[GOLEM_TracedLinks_BASE] " +
                               "         ON Node_id = DS_Node_id " +
                               "  ) AS X " +
                               "GROUP BY NODE_ID ";
        public const string GetSubcatchmentsAU = "SELECT [ssc_id] " +
                               "     ,[node_id_ex] " +
                               "     ,[theArea] " +
                               "     ,ImpPct " +
                               "     ,[Flow_Length] " +
                               "     ,[Slope] " +
                               " FROM [GOLEM].[GIS].[GOLEM_TracedImpAU_BASE] ";
        public const string GetSubcatchmentsRX = "SELECT [ssc_id] " +
                               "     ,[node_id_ex] " +
                               "     ,[theArea] " +
                               "     ,ImpPct " +
                               "     ,[Flow_Length] " +
                               "     ,[Slope] " +
                               " FROM [GOLEM].[GIS].[GOLEM_TracedImpRX_BASE] ";
        public const string GetSubcatchmentsPX = "SELECT [ssc_id] " +
                               "     ,[node_id_ex] " +
                               "     ,[theArea] " +
                               "     ,ImpPct " +
                               "     ,[Flow_Length] " +
                               "     ,[Slope] " +
                               " FROM [GOLEM].[GIS].[GOLEM_TracedImpPX_BASE] ";
        public const string GetSubcatchmentsTU = "SELECT [ssc_id] " +
                               "     ,[node_id_ex] " +
                               "     ,[theArea] " +
                               "     ,ImpPct " +
                               "     ,[Flow_Length] " +
                               "     ,[Slope] " +
                               " FROM [GOLEM].[GIS].[GOLEM_TracedImpTU_BASE] ";
        public const string GetSubareas = "SELECT	ssc_id, " +
                              "         impervious_roughness, " +
                              "         pervious_roughness, " +
                              "         impervious_storage_depth_in, " +
                              "         pervious_storage_depth_in, " +
                              "         Shape.ToString() " +
                              " FROM	[GOLEM].[GIS].[GOLEM_TracedSSCS_BASE] ";
        public const string GetInfiltration = "SELECT	ssc_id, " +
                                      "average_capillary_suction_in, " +
                                      "satur_hydraulic_cond_inhr, " +
                                      "initial_moisture_deficit " +
                               "FROM	[GOLEM].[GIS].[GOLEM_TracedSSCS_BASE] ";
        public const string GetConduits = "SELECT	Link_ID, " +
                               "         CASE WHEN Link_ID IN (SELECT To_Link_ID FROM [GOLEM].[GIS].GOLEM_SPECIAL_LINKS_BASE) " +
                               "              THEN -Link_id " +
                               "              ELSE us_node_id " +
                               "         END AS us_Node_id, " +
                               "         DS_Node_id, " +
                               "         ISNULL([Length],0) as Length, " +
                               "         ISNULL(N, 0.013) AS N, " +
                               "         ISNULL(Z1,0) AS Z1, " +
                               "         ISNULL(DS_invert - IE, 0) AS Z2 " +
                               " FROM " +
                               " ( " +
                               "     SELECT	Link_ID, " +
                               "             US_Node_id, " +
                               "             DS_Node_id, " +
                               "             [Length], " +
                               "             CASE " +
                               "                 WHEN	Roughness <= 0.0000 " +
                               "                 THEN	0.013 " +
                               "                 ELSE	Roughness " +
                               "             END AS N, " +
                               "             US_invert - IE AS Z1, " +
                               "             DS_invert " +
                               "     FROM	[GOLEM].[GIS].[GOLEM_TracedLinks_BASE] " +
                               "     INNER JOIN " +
                               "     (	SELECT Node_id, MIN(IE) AS IE  " +
                               "         FROM " +
                               "         (  " +
                               "           SELECT Node_id, DS_invert AS IE  " +
                               "           FROM   [GOLEM].[GIS].[GOLEM_TracedNodes_BASE]  " +
                               "                  INNER JOIN [GOLEM].[GIS].[GOLEM_TracedLinks_BASE]  " +
                               "                  ON Node_id = DS_Node_id " +
                               "           UNION   " +
                               "           SELECT Node_id, US_invert AS IE  " +
                               "           FROM   [GOLEM].[GIS].[GOLEM_TracedNodes_BASE]  " +
                               "                  INNER JOIN [GOLEM].[GIS].[GOLEM_TracedLinks_BASE]  " +
                               "                  ON Node_id = US_Node_id  " +
                               "         ) AS X  " +
                               "         GROUP BY NODE_id  " +
                               "     )  " +
                               "     AS A ON A.Node_id = [GOLEM].[GIS].[GOLEM_TracedLinks_BASE].US_Node_id " +
                               " ) AS B INNER JOIN " +
                               " (	SELECT Node_id, MIN(IE) AS IE  " +
                               "         FROM " +
                               "         (  " +
                               "           SELECT Node_id, DS_invert AS IE  " +
                               "           FROM   [GOLEM].[GIS].[GOLEM_TracedNodes_BASE]  " +
                               "                  INNER JOIN [GOLEM].[GIS].[GOLEM_TracedLinks_BASE]  " +
                               "                  ON Node_id = DS_Node_id " +
                               "           UNION   " +
                               "           SELECT Node_id, US_invert AS IE  " +
                               "           FROM   [GOLEM].[GIS].[GOLEM_TracedNodes_BASE]  " +
                               "                  INNER JOIN [GOLEM].[GIS].[GOLEM_TracedLinks_BASE]  " +
                               "                  ON Node_id = US_Node_id  " +
                               "         ) AS Z  " +
                               "         GROUP BY NODE_id " +
                               "         ) AS C ON B.DS_Node_id = C.Node_id ";
        public const string GetXSections = "SELECT [link_id] " +
                               "      ,[diameter_or_width_in] " +
                               "      ,[Height_in] " +
                               "      ,Shape_name " +
                               "  FROM [GOLEM].[GIS].[GOLEM_TracedLinks_BASE] AS Links " +
                               "  INNER JOIN GOLEM.GIS.GOLEM_Pipe_Shapes_BASE AS Shapes " +
                               "  ON Links.pipe_shape = Shapes.shape_ID";
        public const string GetWeirs = " SELECT -[To_Link_ID] AS NewNode" +
                        "     ,[to_link_id] " +
                        "     ,[to_link_id_index] " +
                        "     ,[WEIR1] " +
                        "     ,[Coeff] " +
                        "     ,[KWeir] " +
                        "     ,[WeirName] " +
                        "     ,[WLen] " +
                        "     ,[YCrest] " +
                        "     ,[YTop] " +
                        "     ,Link_ID " +
                        "     ,US_Node_id " +
                        "     ,DS_Node_id " +
                        " FROM [GOLEM].[GIS].[GOLEM_TracedWeirs_BASE] " +
                        " INNER JOIN [GOLEM].[GIS].[GOLEM_TracedLinks_BASE] " +
                        " ON to_link_id = link_id ";
        public const string GetOrifices = "SELECT -[To_Link_ID] AS NewNode" +
                               "               ,[To_Link_ID] " +
                               "               ,[To_Link_ID_index] " +
                               "               ,[ORIF1] " +
                               "               ,[ZP] " +
                               "               ,[OrifName] " +
                               "               ,[ISqRnd] " +
                               "               ,[COrif] " +
                               "               ,[AOrif] " +
                               "               ,[ONKLASS] " +
                               "               ,US_Node_id " +
                               "         FROM	[GOLEM].[GIS].[GOLEM_TracedOrifices_BASE] " +
                               "                 INNER JOIN  " +
                               "                 [GOLEM].[GIS].[GOLEM_TracedLinks_BASE] " +
                               "                 ON	To_Link_ID = Link_ID ";
        public const string GetPumps = "SELECT -A.[To_Link_ID] AS NewNode " +
                               "               ,A.[Node_id] " +
                               "               ,A.[To_Link_ID] " +
                               "               ,A.[To_Link_ID_index] " +
                               "               ,A.[PON] - B.ww_el AS PON" +
                               "               ,A.[POFF] - B.ww_el AS POFF" +
                               "               ,A.[DHI_PCurve] " +
                               "               ,A.[AStore] " +
                               "         FROM    [GOLEM].[GIS].[GOLEM_TracedPumps_BASE] as A " +
                               "                 INNER JOIN  " +
                               "                 [GOLEM].[GIS].[GOLEM_TracedStorage_BASE] AS B " +
                               "                 ON  A.node_id = B.node_id ";
        public const string GetTables = "SELECT [PumpType], [Index], [Rating], [Flow] " +
                              "         FROM	[GOLEM].[GIS].[GOLEM_TracedCurves_BASE] ";
        public const string GetDailyPatterns = "SELECT	[Name] " +
                              "         ,[PatternType] " +
                              "         ,[Hour1] " +
                              "         ,[Hour2] " +
                              "         ,[Hour3] " +
                              "         ,[Hour4] " +
                              "         ,[Hour5] " +
                              "         ,[Hour6] " +
                              "         ,[Hour7] " +
                              "         ,[Hour8] " +
                              "         ,[Hour9] " +
                              "         ,[Hour10] " +
                              "         ,[Hour11] " +
                              "         ,[Hour12] " +
                              "         ,[Hour13] " +
                              "         ,[Hour14] " +
                              "         ,[Hour15] " +
                              "         ,[Hour16] " +
                              "         ,[Hour17] " +
                              "         ,[Hour18] " +
                              "         ,[Hour19] " +
                              "         ,[Hour20] " +
                              "         ,[Hour21] " +
                              "         ,[Hour22] " +
                              "         ,[Hour23] " +
                              "         ,[Hour24] " +
                              " FROM	[GOLEM].[GIS].[GOLEM_PATTERNSHourly] " +
                              " INNER JOIN " +
                              "         ( " +
                              "             SELECT	[ModelID] " +
                              "                     ,[GOLEM].[GIS].[GOLEM_PATTERNSTypes].[PatternType] " +
                              "                     ,[PatternGOLEMID] " +
                              "             FROM	[GOLEM].[GIS].[GOLEM_PATTERNSModels] " +
                              "                     INNER JOIN " +
                              "                     [GOLEM].[GIS].[GOLEM_PATTERNSTypes] " +
                              "                     ON	[GOLEM].[GIS].[GOLEM_PATTERNSModels].PatternType = [GOLEM].[GIS].[GOLEM_PATTERNSTypes].GOLEMID " +
                              "                         AND " +
                              "                         [GOLEM].[GIS].[GOLEM_PATTERNSTypes].PatternType = 'HOURLY' " +
                              "         ) AS X " +
                              "         ON	[GOLEM].[GIS].[GOLEM_PATTERNSHourly].GOLEMID = X.PatternGOLEMID " +
                              " WHERE ModelID = 1";
        public const string GetWeekendPatterns = "SELECT	[Name] " +
                               "         ,[PatternType] " +
                               "         ,[Hour1] " +
                               "         ,[Hour2] " +
                               "         ,[Hour3] " +
                               "         ,[Hour4] " +
                               "         ,[Hour5] " +
                               "         ,[Hour6] " +
                               "         ,[Hour7] " +
                               "         ,[Hour8] " +
                               "         ,[Hour9] " +
                               "         ,[Hour10] " +
                               "         ,[Hour11] " +
                               "         ,[Hour12] " +
                               "         ,[Hour13] " +
                               "         ,[Hour14] " +
                               "         ,[Hour15] " +
                               "         ,[Hour16] " +
                               "         ,[Hour17] " +
                               "         ,[Hour18] " +
                               "         ,[Hour19] " +
                               "         ,[Hour20] " +
                               "         ,[Hour21] " +
                               "         ,[Hour22] " +
                               "         ,[Hour23] " +
                               "         ,[Hour24] " +
                               "         ,ModelID " +
                               " FROM	[GOLEM].[GIS].[GOLEM_PATTERNSHourly] " +
                               " INNER JOIN " +
                               "         ( " +
                               "             SELECT	[ModelID] " +
                               "                     ,[GOLEM].[GIS].[GOLEM_PATTERNSTypes].[PatternType] " +
                               "                     ,[PatternGOLEMID] " +
                               "             FROM	[GOLEM].[GIS].[GOLEM_PATTERNSModels] " +
                               "                     INNER JOIN " +
                               "                     [GOLEM].[GIS].[GOLEM_PATTERNSTypes] " +
                               "                     ON	[GOLEM].[GIS].[GOLEM_PATTERNSModels].PatternType = [GOLEM].[GIS].[GOLEM_PATTERNSTypes].GOLEMID " +
                               "                         AND " +
                               "                         [GOLEM].[GIS].[GOLEM_PATTERNSTypes].PatternType = 'WEEKEND' " +
                               "         ) AS X " +
                               "         ON	[GOLEM].[GIS].[GOLEM_PATTERNSHourly].GOLEMID = X.PatternGOLEMID " +
                               " WHERE ModelID = 1";
        public const string GetWeeklyPatterns = "SELECT	[Name] " +
                               "         ,[PatternType] " +
                               "         ,[Day1] " +
                               "         ,[Day2] " +
                               "         ,[Day3] " +
                               "         ,[Day4] " +
                               "         ,[Day5] " +
                               "         ,[Day6] " +
                               "         ,[Day7] " +
                               "         ,ModelID " +
                               " FROM	[GOLEM].[GIS].[GOLEM_PATTERNSDaily] " +
                               " INNER JOIN " +
                               "         ( " +
                               "             SELECT	[ModelID] " +
                               "                     ,[GOLEM].[GIS].[GOLEM_PATTERNSTypes].[PatternType] " +
                               "                     ,[PatternGOLEMID] " +
                               "             FROM	[GOLEM].[GIS].[GOLEM_PATTERNSModels] " +
                               "                     INNER JOIN " +
                               "                     [GOLEM].[GIS].[GOLEM_PATTERNSTypes] " +
                               "                     ON	[GOLEM].[GIS].[GOLEM_PATTERNSModels].PatternType = [GOLEM].[GIS].[GOLEM_PATTERNSTypes].GOLEMID " +
                               "                         AND " +
                               "                         [GOLEM].[GIS].[GOLEM_PATTERNSTypes].PatternType = 'DAILY' " +
                               "         ) AS X " +
                               "         ON	[GOLEM].[GIS].[GOLEM_PATTERNSDaily].GOLEMID = X.PatternGOLEMID " +
                               " WHERE ModelID = 1";
        public const string GetYearlyPatterns = "SELECT	[Name] " +
                               "         ,[PatternType] " +
                               "         ,[Month1] " +
                               "         ,[Month2] " +
                               "         ,[Month3] " +
                               "         ,[Month4] " +
                               "         ,[Month5] " +
                               "         ,[Month6] " +
                               "         ,[Month7] " +
                               "         ,[Month8] " +
                               "         ,[Month9] " +
                               "         ,[Month10] " +
                               "         ,[Month11] " +
                               "         ,[Month12] " +
                               "         ,ModelID " +
                               " FROM	[GOLEM].[GIS].[GOLEM_PATTERNSMonthly] " +
                               " INNER JOIN " +
                               "         ( " +
                               "             SELECT	[ModelID] " +
                               "                     ,[GOLEM].[GIS].[GOLEM_PATTERNSTypes].[PatternType] " +
                               "                     ,[PatternGOLEMID] " +
                               "             FROM	[GOLEM].[GIS].[GOLEM_PATTERNSModels] " +
                               "                     INNER JOIN " +
                               "                     [GOLEM].[GIS].[GOLEM_PATTERNSTypes] " +
                               "                     ON	[GOLEM].[GIS].[GOLEM_PATTERNSModels].PatternType = [GOLEM].[GIS].[GOLEM_PATTERNSTypes].GOLEMID " +
                               "                         AND " +
                               "                         [GOLEM].[GIS].[GOLEM_PATTERNSTypes].PatternType = 'MONTHLY' " +
                               "         ) AS X " +
                               "         ON	[GOLEM].[GIS].[GOLEM_PATTERNSMonthly].GOLEMID = X.PatternGOLEMID " +
                               " WHERE ModelID = 1";
        public const string GetDWF = "SELECT US_Node_id, SUM(BaseFlow_ex) AS AveFlow " +
                              " FROM [GOLEM].[GIS].[GOLEM_TracedDSCS_BASE] " +
                              "      INNER JOIN " +
                              "      [GOLEM].[GIS].[GOLEM_TracedLinks_BASE] " +
                              "      ON GOLEM_TracedDSCS_BASE.san_to_link_id = GOLEM_TracedLinks_BASE.link_id " +
                              " GROUP BY US_Node_id";
        public const string GetCurves = "SELECT	SHAPES.shape_name + '_' + CAST(SHAPES.width AS nvarchar(4)) + '_' + CAST(SHAPES.height AS nvarchar(4)) as shape_name, " +
                               "          DATA.[depth]*12.0/DATA.[height_in] as dfdR,  " +
                               "          DATA.[surface_width]*12.0/DATA.width_in as wfdR   " +
                               "  FROM	(  " +
                               "              SELECT	SHAPES1.Shape_name as pipe_shape, " +
                               "                      Links.diameter_or_width_in, " +
                               "                      Links.height_in " +
                               "              FROM	[GOLEM].[GIS].[GOLEM_TracedLINKS_BASE] AS Links  " +
                               "                      INNER JOIN " +
                               "                      [GOLEM].[GIS].[GOLEM_PIPE_SHAPES_BASE] AS SHAPES1 " +
                               "                      ON " +
                               "                      Links.pipe_shape = SHAPES1.shape_id " +
                               "              GROUP BY SHAPES1.Shape_name , " +
                               "                      Links.diameter_or_width_in, " +
                               "                      Links.height_in " +
                               "          ) AS A  " +
                               "          INNER JOIN  " +
                               "          [GOLEM].[GIS].[GOLEM_PIPE_SHAPES_BASE] AS SHAPES  " +
                               "          ON	A.pipe_shape = SHAPES.Shape_name " +
                               "              AND " +
                               "              A.diameter_or_width_in = SHAPES.width " +
                               "              AND " +
                               "              A.height_in = SHAPES.height " +
                               "          INNER JOIN   " +
                               "          [GOLEM].[GIS].[GOLEM_PIPE_SHAPES_DATA_BASE] AS DATA   " +
                               "          ON	SHAPES.Shape_id = Data.Shape_id  " +
                               "  ORDER BY SHAPES.shape_name + '_' + CAST(SHAPES.width AS nvarchar(4)) + '_' + CAST(SHAPES.height AS nvarchar(4)), DATA.[depth]*12.0/DATA.[height_in]";
        public static string GetTimeSeries(int StormID, int HoursPriming)
        {
            return "SELECT [StormID] " +
                               "    ,([TimePoint] * Interval) / 60.0 + " + HoursPriming.ToString() + " AS theTime " +
                               "    ,[Intensity] " +
                               "FROM [GOLEM].[GIS].[GOLEM_STORMSDATA_BASE] AS Data " +
                               "      INNER JOIN  " +
                               "      [GOLEM].[GIS].[GOLEM_STORMS_BASE] AS Storms " +
                               "      ON Storms.GOLEM_ID = Data.StormID " +
                               "WHERE StormID = " + StormID.ToString();
        }
        public const string GetCoordinates = "SELECT node_id " +
                               "    ,[x_coord] " +
                               "    ,[y_coord] " +
                               "FROM [GOLEM].[GIS].[GOLEM_TracedNodes_BASE] AS Coords ";
        public const string GetPolygonsAU = "SELECT [ssc_id] " +
                               "     ,[x_coord] " +
                               "     ,[y_coord] " +
                               " FROM [GOLEM].[GIS].[GOLEM_TracedImpAU_BASE] AS AU " +
                               "        INNER JOIN " +
                               "        [GOLEM].[GIS].[GOLEM_TracedNodes_BASE] AS Nodes" +
                               "        ON AU.[node_id_ex] = Nodes.Node_id ";
        public const string GetPolygonsRX = "SELECT [ssc_id] " +
                               "     ,[x_coord] " +
                               "     ,[y_coord] " +
                               " FROM [GOLEM].[GIS].[GOLEM_TracedImpRX_BASE] AS RX " +
                               "        INNER JOIN " +
                               "        [GOLEM].[GIS].[GOLEM_TracedNodes_BASE] AS Nodes" +
                               "        ON RX.[node_id_ex] = Nodes.Node_id ";
        public const string GetPolygonsPX = "SELECT [ssc_id] " +
                               "     ,[x_coord] " +
                               "     ,[y_coord] " +
                               " FROM [GOLEM].[GIS].[GOLEM_TracedImpPX_BASE] AS PX " +
                               "        INNER JOIN " +
                               "        [GOLEM].[GIS].[GOLEM_TracedNodes_BASE] AS Nodes" +
                               "        ON PX.[node_id_ex] = Nodes.Node_id ";
        public const string GetPolygonsTU = "SELECT [ssc_id] " +
                               "     ,[x_coord] " +
                               "     ,[y_coord] " +
                               " FROM [GOLEM].[GIS].[GOLEM_TracedImpTU_BASE] AS TU " +
                               "        INNER JOIN " +
                               "        [GOLEM].[GIS].[GOLEM_TracedNodes_BASE] AS Nodes" +
                               "        ON TU.[node_id_ex] = Nodes.Node_id ";
    }
}
