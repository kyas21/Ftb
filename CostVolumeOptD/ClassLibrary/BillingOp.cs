using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.Windows.Forms;

namespace ClassLibrary
{
    public class BillingOp :DbAccess
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        private string tableName;
        const int subjCnt = 6;
        const string seliDSql = ";SELECT CAST(SCOPE_IDENTITY() AS int)";
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public BillingOp()
        {
        }

        public BillingOp( string tableName )
        {
            this.tableName = tableName;
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public string TableName
        {
            get { return this.tableName; }
            set { this.tableName = value; }
        }
        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public DataTable SelectUsingTaskEntryID( int taskEntryID )
        {
            if( taskEntryID == 0 ) return null;
            string sqlStr = "SELECT * FROM " + tableName + "WHERE TaskEntryID = " + Convert.ToString( taskEntryID );
            return UsingTryExReader( sqlStr );
        }

        // 外注精算データ、外注精算内訳データ
        public DataTable SelectOsResultsCont( OsResultsData ord )
        {
            string sqlStr = "SELECT * FROM D_OsResultsCont OsResultsID = " + Convert.ToString( ord.OsResultsID ) + " ORDER BY LNo";
            return UsingTryExReader( sqlStr );
        }

        public int InsertOsResults( OsResultsData ord )
        {
            if( ord == null ) return -1;

            string sqlStr = "INSERT INTO D_OsResults "
                + "(TaskCode, VersionNo, OrderNo, PartnerCode, PayRoule, Amount, "
                + "PublishDate, StartDate, EndDate, InspectDate, ReceiptDate, Place, Note, OfficeCode, Department, ContractForm, RecordedDate, TaskEntryID) "
                + "VALUES (@tCod, @vNo, @oNo, @pCod, @payR, @amt, @pDat, @sDat, @eDat, @iDat, @rDat, @plac, @note, @oCod, @dept, @crtF, @recD, @teID)"
                + seliDSql;
            Int32 newProdID = -1;

            using( TransactionScope tran = new TransactionScope() )
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand( sqlStr, conn );
                    cmd = osResults_cmd( cmd, ord );
                    
                    newProdID = TryExScalar( conn, cmd );
                }
                catch( SqlException sqle )
                {
                    MessageBox.Show( "SQLエラー errorno " + Convert.ToString( sqle.Number ) + " " + sqle.Message );
                    conn.Close();
                    return -1;
                }
                conn.Close();
                tran.Complete();
            }
            return ( int )newProdID;
        }


        public bool UpdateOsResults( OsResultsData ord )
        {
            if( ord == null ) return false;

            string sqlStr = "UPDATE D_OsResults SET TaskCode = @tCod, VersionNo = @vNo, OrderNo = @oNo, PartnerCode = @pCod, PayRoule = @payR, Amount = @amt,"
                + " PublishDate = @pDat, StartDate = @sDat, EndDate = @eDat, InspectDate = @iDat, ReceiptDate = @rDat,"
                + " Place = @plac, Note = @note, OfficeCode = @oCod, Department = @dept, RecordedDate = @recD WHERE OsResultsID = @oRID";

            using( TransactionScope tran = new TransactionScope() )
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand( sqlStr, conn );
                    cmd = osResults_cmd( cmd, ord );
                    cmd.Parameters.Add( "@oRID", SqlDbType.Int );
                    cmd.Parameters["@oRID"].Value = ord.OsResultsID;
                    if( TryExecute( conn, cmd ) < 0 ) return false;
                }
                catch( SqlException sqle )
                {
                    MessageBox.Show( "SQLエラー errorno " + Convert.ToString( sqle.Number ) + " " + sqle.Message );
                    conn.Close();
                    return false;
                }
                conn.Close();
                tran.Complete();
            }
            return true;
        }


        public bool DeleteOsResults( int osResultsID )
        {
            string sqlStr = "DELETE FROM D_OsResults WHERE OsResultsID = " + Convert.ToString( osResultsID );
            return UsingTryExecute( sqlStr );
        }


        public bool DeleteOsResultsCont( int osResultsID )
        {
            string sqlStr = "DELETE FROM D_OsResultsCont WHERE OsResultsID = " + Convert.ToString( osResultsID );
            return UsingTryExecute( sqlStr );
        }


        public bool UpdateOsResultsCont( DataGridView dgvL, DataGridView dgvR, OsResultsData ord )
        {
            if( ord == null ) return false;

            string sqlStr = "UPDATE D_OsResultsCont SET Item = @item, Quantity = @qty"
                          + " WHERE OsResultsID = @oRID AND Subject = @subj";

            using( TransactionScope tran = new TransactionScope() )
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand( sqlStr, conn );
                    cmd.Parameters.Add( "@item", SqlDbType.NVarChar );
                    cmd.Parameters.Add( "@qty", SqlDbType.Decimal );

                    cmd.Parameters.Add( "@oRID", SqlDbType.Int );
                    cmd.Parameters.Add( "@pDat", SqlDbType.Date );
                    cmd.Parameters.Add( "@subj", SqlDbType.Int );

                    cmd.Parameters["@oRID"].Value = ord.OsResultsID;

                    for( int i = 0; i < dgvL.Rows.Count; i++ )
                    {
                        if( string.IsNullOrEmpty( Convert.ToString( dgvL.Rows[i].Cells["ItemL"].Value ) ) ) continue;
                        cmd.Parameters["@item"].Value = Convert.ToString( dgvL.Rows[i].Cells["ItemL"].Value );
                        cmd.Parameters["@pDat"].Value = Convert.ToDateTime( dgvL.Rows[i].Cells["PublishDateL"].Value );
                        for( int j = 0; j < 6; j++ )
                        {
                            cmd.Parameters["@qty"].Value = DHandling.ToRegDecimal( Convert.ToString( dgvL.Rows[i].Cells["QuantityL" + j.ToString()].Value ) );
                            cmd.Parameters["@subj"].Value = j;
                            if( TryExecute( conn, cmd ) < 0 ) return false;
                        }
                    }

                    for( int i = 0; i < dgvR.Rows.Count; i++ )
                    {
                        if( string.IsNullOrEmpty( Convert.ToString( dgvR.Rows[i].Cells["ItemR"].Value ) ) ) continue;
                        cmd.Parameters["@item"].Value = Convert.ToString( dgvR.Rows[i].Cells["ItemR"].Value );
                        cmd.Parameters["@pDat"].Value = Convert.ToDateTime( dgvR.Rows[i].Cells["PublishDateR"].Value );
                        for( int j = 0; j < 6; j++ )
                        {
                            cmd.Parameters["@qty"].Value = DHandling.ToRegDecimal( Convert.ToString( dgvR.Rows[i].Cells["QuantityR" + j.ToString()].Value ) );
                            cmd.Parameters["@subj"].Value = j;
                            if( TryExecute( conn, cmd ) < 0 ) return false;
                        }
                    }
                }
                catch( SqlException sqle )
                {
                    MessageBox.Show( "SQLエラー errorno " + Convert.ToString( sqle.Number ) + " " + sqle.Message );
                    conn.Close();
                    return false;
                }
                conn.Close();
                tran.Complete();
            }
            return true;
        }


        public bool UpdateOsResultsSum( DataGridView dgv, OsResultsData ord )
        {
            if( ord == null ) return false;

            string sqlStr = "UPDATE D_OsResultsSum SET ItemDetail = @iDtl, Quantity = @qty, Cost = @cost, Note = @note"
                          + " WHERE OsResultsID = @oRID";

            using( TransactionScope tran = new TransactionScope() )
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand( sqlStr, conn );
                    cmd.Parameters.Add( "@iDtl", SqlDbType.NVarChar );
                    cmd.Parameters.Add( "@qty", SqlDbType.Decimal );
                    cmd.Parameters.Add( "@cost", SqlDbType.Decimal );
                    cmd.Parameters.Add( "@note", SqlDbType.NVarChar );

                    cmd.Parameters.Add( "@oRID", SqlDbType.Int );
                    cmd.Parameters.Add( "@rDat", SqlDbType.Date );
                    cmd.Parameters.Add( "@subj", SqlDbType.Int );

                    cmd.Parameters["@oRID"].Value = ord.OsResultsID;
                    cmd.Parameters["@rDat"].Value = ord.ReceiptDate.StripTime();       // 計上年月、日付は月末日。念のため時間を除く
                    for( int i = 0; i < dgv.Rows.Count - 1; i++ )                          // 合計行は除く
                    {
                        cmd.Parameters["@subj"].Value = i;
                        cmd.Parameters["@qty"].Value = DHandling.ToRegDecimal(Convert.ToString( dgv.Rows[i].Cells["Quantity"].Value ));
                        cmd.Parameters["@cost"].Value = DHandling.ToRegDecimal(Convert.ToString( dgv.Rows[i].Cells["Cost"].Value ));
                        cmd.Parameters["@iDtl"].Value = Convert.ToString( dgv.Rows[i].Cells["ItemDetail"].Value );
                        cmd.Parameters["@note"].Value = Convert.ToString( dgv.Rows[i].Cells["Note"].Value );
                        if( TryExecute( conn, cmd ) < 0 ) return false;
                    }
                }
                catch( SqlException sqle )
                {
                    MessageBox.Show( "SQLエラー errorno " + Convert.ToString( sqle.Number ) + " " + sqle.Message );
                    conn.Close();
                    return false;
                }
                conn.Close();
                tran.Complete();
            }
            return true;
        }


        public bool InsertOsResultsCont( DataGridView dgv, OsResultsData ord )
        {
            string sqlStr = "INSERT INTO D_OsResultsCont (OsResultsID, RecordedDate, LNo, ItemCode, Item, PQuantity, Unit, Cost, Quantity, PublishDate, Subject, "
                          + "TaskEntryID, PartnerCode) "
                          + "VALUES (@oRID, @rDat, @lNo, @iCod, @item, @pQty, @unit, @cost, @qty, @pDat, @subj, @teID, @pCod)";
            int line = 0;

            using( TransactionScope tran = new TransactionScope() )
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand( sqlStr, conn );
                    cmd = osResultsContParaAdd( cmd );
                    cmd.Parameters["@oRID"].Value = ord.OsResultsID;
                    cmd.Parameters["@rDat"].Value = ord.RecordedDate;
                    cmd.Parameters["@teID"].Value = ord.TaskEntryID;
                    cmd.Parameters["@pCod"].Value = ord.PartnerCode;
                    for( int i = 0; i < dgv.Rows.Count; i++ )
                    {
                        if( ord.ContractForm == 0 )              // 請負
                        {
                            cmd.Parameters["@pDat"].Value = ord.PublishDate;
                            if( Convert.ToString( dgv.Rows[i].Cells["Item"].Value ) == Sign.SumTotal ||
                                                        dgv.Rows[i].Cells["Item"].Value == null )
                            {
                                // 何もしない
                            }
                            else
                            {
                                cmd = osResultsContPara_C_Value( cmd, dgv.Rows[i] );
                                line++;
                                cmd.Parameters["@lNo"].Value = line;
                                if( TryExecute( conn, cmd ) < 0 ) return false;
                            }
                        }
                        else
                        {
                            cmd = osResultsContPara_R_Value( cmd, dgv.Rows[i], null );
                            cmd.Parameters["@pDat"].Value = new DateTime( ord.RecordedDate.Year, ord.RecordedDate.Month, i + 1 );

                            for( int j = 0; j < subjCnt; j++ )
                            {
                                if( dgv.Rows[i].Cells["Quantity" + j.ToString()].Value == null )
                                {
                                    cmd.Parameters["@qty"].Value = 0;
                                }
                                else
                                {
                                    cmd.Parameters["@qty"].Value = DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["Quantity" + j.ToString()].Value ) );
                                }
                                cmd.Parameters["@subj"].Value = j;
                                line++;
                                cmd.Parameters["@lNo"].Value = line;
                                if( TryExecute( conn, cmd ) < 0 ) return false;
                            }
                        }


                    }
                }
                catch( SqlException sqle )
                {
                    MessageBox.Show( "SQLエラー errorno " + Convert.ToString( sqle.Number ) + " " + sqle.Message );
                    conn.Close();
                    return false;
                }
                conn.Close();
                tran.Complete();
            }
            return true;
        }

        /// <summary>
        /// 下記と交換で使う
        /// </summary>
        /// <param name="dgvL"></param>
        /// <param name="dgvR"></param>
        /// <param name="ord"></param>
        /// <returns></returns>
        //public bool InsertOsResultsCont( DataGridView dgvL, DataGridView dgvR, OsResultsData ord )
        //{
        //    string sqlStr = "INSERT INTO D_OsResultsCont (OsResultsID, RecordedDate, LNo, ItemCode, Item, PQuantity, Unit, Cost, Quantity, PublishDate, Subject, "
        //                  + "TaskEntryID, PartnerCode) "
        //                  + "VALUES (@oRID, @rDat, @lNo, @iCod, @item, @pQty, @unit, @cost, @qty, @pDat, @subj, @teID, @pCod)";
        //    int line = 0;

        //    using( TransactionScope tran = new TransactionScope() )
        //    using( SqlConnection conn = new SqlConnection( ConnectionString ) )
        //    {
        //        try
        //        {
        //            conn.Open();
        //            SqlCommand cmd = new SqlCommand( sqlStr, conn );
        //            cmd = osResultsContParaAdd( cmd );
        //            cmd.Parameters["@oRID"].Value = ord.OsResultsID;
        //            cmd.Parameters["@rDat"].Value = ord.RecordedDate;
        //            cmd.Parameters["@teID"].Value = ord.TaskEntryID;
        //            cmd.Parameters["@pCod"].Value = ord.PartnerCode;
        //            for( int i = 0; i < dgvL.Rows.Count; i++ )
        //            {
        //                cmd = osResultsContPara_R_Value( cmd, dgvL.Rows[i], "L" );
        //                cmd.Parameters["@pDat"].Value = new DateTime( ord.RecordedDate.Year, ord.RecordedDate.Month, i + 1 );

        //                for( int j = 0; j < subjCnt; j++ )
        //                {
        //                    //if( dgvL.Rows[i].Cells["QuantityL" + j.ToString()].Value == null )
        //                    //{
        //                    //    cmd.Parameters["@qty"].Value = 0;
        //                    //}
        //                    //else
        //                    //{
        //                    //    cmd.Parameters["@qty"].Value = DHandling.ToRegDecimal( Convert.ToString( dgvL.Rows[i].Cells["QuantityL" + j.ToString()].Value ) );
        //                    //}
        //                    cmd.Parameters["@qty"].Value = ( dgvL.Rows[i].Cells["QuantityL" + j.ToString()].Value == null ) ? 0
        //                        : DHandling.ToRegDecimal( Convert.ToString( dgvL.Rows[i].Cells["QuantityL" + j.ToString()].Value ) );
        //                    cmd.Parameters["@subj"].Value = j;
        //                    line++;
        //                    cmd.Parameters["@lNo"].Value = line;
        //                    if( TryExecute( conn, cmd ) < 0 ) return false;
        //                }
        //            }

        //            for( int i = 0; i < dgvR.Rows.Count; i++ )
        //            {
        //                cmd = osResultsContPara_R_Value( cmd, dgvR.Rows[i], "R" );
        //                cmd.Parameters["@pDat"].Value = new DateTime( ord.RecordedDate.Year, ord.RecordedDate.Month, i + 17 );

        //                for( int j = 0; j < subjCnt; j++ )
        //                {
        //                    //if( dgvR.Rows[i].Cells["QuantityR" + j.ToString()].Value == null )
        //                    //{
        //                    //    cmd.Parameters["@qty"].Value = 0;
        //                    //}
        //                    //else
        //                    //{
        //                    //    cmd.Parameters["@qty"].Value = DHandling.ToRegDecimal( Convert.ToString( dgvR.Rows[i].Cells["QuantityR" + j.ToString()].Value ) );
        //                    //}

        //                    cmd.Parameters["@qty"].Value = ( dgvR.Rows[i].Cells["QuantityR" + j.ToString()].Value == null ) ? 0
        //                        : DHandling.ToRegDecimal( Convert.ToString( dgvR.Rows[i].Cells["QuantityR" + j.ToString()].Value ) );
        //                    cmd.Parameters["@subj"].Value = j;
        //                    line++;
        //                    cmd.Parameters["@lNo"].Value = line;
        //                    if( TryExecute( conn, cmd ) < 0 ) return false;
        //                }
        //            }
        //        }
        //        catch( SqlException sqle )
        //        {
        //            MessageBox.Show( "SQLエラー errorno " + Convert.ToString( sqle.Number ) + " " + sqle.Message );
        //            conn.Close();
        //            return false;
        //        }
        //        conn.Close();
        //        tran.Complete();
        //    }
        //    return true;
        //}


        /// <summary>
        /// 上のと交換で使う
        /// </summary>
        /// <param name="dgvL"></param>
        /// <param name="dgvR"></param>
        /// <param name="ord"></param>
        /// <returns></returns>
        public bool InsertOsResultsCont( DataGridView dgvL, DataGridView dgvR, OsResultsData ord )
        {
            string sqlStr = "INSERT INTO D_OsResultsCont (OsResultsID, RecordedDate, LNo, ItemCode, Item, PQuantity, Unit, Cost, Quantity, PublishDate, Subject, "
                          + "TaskEntryID, PartnerCode) "
                          + "VALUES (@oRID, @rDat, @lNo, @iCod, @item, @pQty, @unit, @cost, @qty, @pDat, @subj, @teID, @pCod)";
            int line = 0;

            using( TransactionScope tran = new TransactionScope() )
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand( sqlStr, conn );
                    cmd = osResultsContParaAdd( cmd );
                    cmd.Parameters["@oRID"].Value = ord.OsResultsID;
                    cmd.Parameters["@rDat"].Value = ord.RecordedDate;
                    cmd.Parameters["@teID"].Value = ord.TaskEntryID;
                    cmd.Parameters["@pCod"].Value = ord.PartnerCode;
                    for( int i = 0; i < dgvL.Rows.Count; i++ )
                    {
                        if( string.IsNullOrEmpty( Convert.ToString(dgvL.Rows[i].Cells["ItemL"].Value ) ) ) continue;

                        cmd = osResultsContPara_R_Value( cmd, dgvL.Rows[i], "L" );
                        cmd.Parameters["@pDat"].Value = new DateTime( ord.RecordedDate.Year, ord.RecordedDate.Month, i + 1 );

                        for( int j = 0; j < subjCnt; j++ )
                        {
                            cmd.Parameters["@qty"].Value = ( dgvL.Rows[i].Cells["QuantityL" + j.ToString()].Value == null ) ? 0
                                : DHandling.ToRegDecimal( Convert.ToString( dgvL.Rows[i].Cells["QuantityL" + j.ToString()].Value ) );
                            cmd.Parameters["@subj"].Value = j;
                            line++;
                            cmd.Parameters["@lNo"].Value = line;
                            if( TryExecute( conn, cmd ) < 0 ) return false;
                        }
                    }

                    for( int i = 0; i < dgvR.Rows.Count; i++ )
                    {
                        if( string.IsNullOrEmpty( Convert.ToString(dgvR.Rows[i].Cells["ItemR"].Value ) ) ) continue;

                        cmd = osResultsContPara_R_Value( cmd, dgvR.Rows[i], "R" );
                        cmd.Parameters["@pDat"].Value = new DateTime( ord.RecordedDate.Year, ord.RecordedDate.Month, i + 17 );

                        for( int j = 0; j < subjCnt; j++ )
                        {
                            cmd.Parameters["@qty"].Value = ( dgvR.Rows[i].Cells["QuantityR" + j.ToString()].Value == null ) ? 0
                                : DHandling.ToRegDecimal( Convert.ToString( dgvR.Rows[i].Cells["QuantityR" + j.ToString()].Value ) );
                            cmd.Parameters["@subj"].Value = j;
                            line++;
                            cmd.Parameters["@lNo"].Value = line;
                            if( TryExecute( conn, cmd ) < 0 ) return false;
                        }
                    }
                }
                catch( SqlException sqle )
                {
                    MessageBox.Show( "SQLエラー errorno " + Convert.ToString( sqle.Number ) + " " + sqle.Message );
                    conn.Close();
                    return false;
                }
                conn.Close();
                tran.Complete();
            }
            return true;
        }

        public bool InsertOsResultsSum( DataGridView dgv, OsResultsData ord )
        {
            string sqlStr = "INSERT INTO D_OsResultsSum (OsResultsID, RecordedDate, Subject, ItemDetail, Quantity, Unit, Cost, Note, TaskEntryID, PartnerCode) "
                          + "VALUES (@oRID, @rDat, @subj, @iDtl, @qty, @unit, @cost, @note, @teID, @pCod)";

            using( TransactionScope tran = new TransactionScope() )
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand( sqlStr, conn );
                    cmd = osResultsSumParaAdd( cmd );

                    cmd.Parameters["@oRID"].Value = ord.OsResultsID;
                    cmd.Parameters["@rDat"].Value = ord.RecordedDate;
                    cmd.Parameters["@teID"].Value = ord.TaskEntryID;
                    cmd.Parameters["@pCod"].Value = ord.PartnerCode;

                    for( int i = 0; i < subjCnt; i++ )
                    {
                        cmd.Parameters["@subj"].Value = i;
                        cmd.Parameters["@iDtl"].Value = Convert.ToString( dgv.Rows[i].Cells["ItemDetail"].Value );
                        cmd.Parameters["@qty"].Value = DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["Quantity"].Value ) );
                        cmd.Parameters["@unit"].Value = Convert.ToString( dgv.Rows[i].Cells["Unit"].Value );
                        cmd.Parameters["@cost"].Value = DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["Cost"].Value ) );
                        cmd.Parameters["@note"].Value = Convert.ToString( dgv.Rows[i].Cells["Note"].Value );

                        if( TryExecute( conn, cmd ) < 0 ) return false;
                    }
                }
                catch( SqlException sqle )
                {
                    MessageBox.Show( "SQLエラー errorno " + Convert.ToString( sqle.Number ) + " " + sqle.Message );
                    conn.Close();
                    return false;
                }
                conn.Close();
                tran.Complete();
            }
            return true;
        }


        public bool DeleteAndInsertOsResultsCont( DataGridView dgv, OsResultsData ord )
        {
            DeleteOsResultsCont( ord.OsResultsID );
            InsertOsResultsCont( dgv, ord );
            return true;
        }


        private SqlCommand osResults_cmd( SqlCommand cmd, OsResultsData ord )
        {
            cmd.Parameters.Add( "@tCod", SqlDbType.Char );
            cmd.Parameters.Add( "@vNo", SqlDbType.Int );
            cmd.Parameters.Add( "@oNo", SqlDbType.Char );
            cmd.Parameters.Add( "@pCod", SqlDbType.Char );
            cmd.Parameters.Add( "@payR", SqlDbType.Int );
            cmd.Parameters.Add( "@amt", SqlDbType.Decimal );
            cmd.Parameters.Add( "@pDat", SqlDbType.Date );
            cmd.Parameters.Add( "@sDat", SqlDbType.Date );
            cmd.Parameters.Add( "@eDat", SqlDbType.Date );
            cmd.Parameters.Add( "@iDat", SqlDbType.Date );
            cmd.Parameters.Add( "@rDat", SqlDbType.Date );
            cmd.Parameters.Add( "@plac", SqlDbType.NVarChar );
            cmd.Parameters.Add( "@note", SqlDbType.NVarChar );
            cmd.Parameters.Add( "@oCod", SqlDbType.Char );
            cmd.Parameters.Add( "@dept", SqlDbType.Char );
            cmd.Parameters.Add( "@crtF", SqlDbType.Int );
            cmd.Parameters.Add( "@recD", SqlDbType.Date );
            cmd.Parameters.Add( "@teID", SqlDbType.Int );

            cmd.Parameters["@tCod"].Value = ord.TaskCode;
            cmd.Parameters["@vNo"].Value = ord.VersionNo;
            cmd.Parameters["@oNo"].Value = ord.OrderNo;
            cmd.Parameters["@pCod"].Value = ord.PartnerCode;
            cmd.Parameters["@payR"].Value = ord.PayRoule;
            cmd.Parameters["@amt"].Value = ord.Amount;
            cmd.Parameters["@pDat"].Value = ord.PublishDate;
            cmd.Parameters["@sDat"].Value = ord.StartDate;
            cmd.Parameters["@eDat"].Value = ord.EndDate;
            cmd.Parameters["@iDat"].Value = ord.InspectDate;
            cmd.Parameters["@rDat"].Value = ord.ReceiptDate;
            cmd.Parameters["@plac"].Value = ord.Place;
            cmd.Parameters["@note"].Value = ord.Note;
            cmd.Parameters["@oCod"].Value = ord.OfficeCode;
            cmd.Parameters["@dept"].Value = ord.Department;
            cmd.Parameters["@crtF"].Value = ord.ContractForm;
            cmd.Parameters["@recD"].Value = ord.RecordedDate;
            cmd.Parameters["@teID"].Value = ord.TaskEntryID;

            return cmd;
        }


        private SqlCommand osResultsContParaAdd( SqlCommand cmd )
        {
            cmd.Parameters.Add( "@oRID", SqlDbType.Int );
            cmd.Parameters.Add( "@rDat", SqlDbType.Date );
            cmd.Parameters.Add( "@lNo", SqlDbType.Int );
            cmd.Parameters.Add( "@iCod", SqlDbType.VarChar );
            cmd.Parameters.Add( "@item", SqlDbType.NVarChar );
            cmd.Parameters.Add( "@pQty", SqlDbType.Decimal );
            cmd.Parameters.Add( "@unit", SqlDbType.NVarChar );
            cmd.Parameters.Add( "@cost", SqlDbType.Decimal );
            cmd.Parameters.Add( "@qty", SqlDbType.Decimal );
            cmd.Parameters.Add( "@pDat", SqlDbType.Date );
            cmd.Parameters.Add( "@subj", SqlDbType.Int );
            cmd.Parameters.Add( "@teID", SqlDbType.Int );
            cmd.Parameters.Add( "@pCod", SqlDbType.Char );
            return cmd;
        }


        private SqlCommand osResultsContPara_C_Value( SqlCommand cmd, DataGridViewRow dgvr )
        {
            cmd.Parameters["@iCod"].Value = Convert.ToString( dgvr.Cells["ItemCode"].Value );
            cmd.Parameters["@item"].Value = Convert.ToString( dgvr.Cells["Item"].Value );
            cmd.Parameters["@pQty"].Value = Convert.ToDecimal( dgvr.Cells["PQuantity"].Value );
            cmd.Parameters["@unit"].Value = Convert.ToString( dgvr.Cells["Unit"].Value );
            cmd.Parameters["@cost"].Value = DHandling.ToRegDecimal( Convert.ToString( dgvr.Cells["Cost"].Value ) );
            cmd.Parameters["@qty"].Value = Convert.ToDecimal( dgvr.Cells["Quantity"].Value );
            cmd.Parameters["@subj"].Value = -1;
            return cmd;
        }


        private SqlCommand osResultsContPara_R_Value( SqlCommand cmd, DataGridViewRow dgvr, string pos )
        {
            cmd.Parameters["@iCod"].Value = "";
            cmd.Parameters["@item"].Value = string.IsNullOrEmpty( pos ) ? Convert.ToString( dgvr.Cells["Item"].Value )
                                                                      : Convert.ToString( dgvr.Cells["Item" + pos].Value );
            cmd.Parameters["@pQty"].Value = 0;
            cmd.Parameters["@unit"].Value = "";
            cmd.Parameters["@cost"].Value = 0;
            return cmd;
        }


        private SqlCommand osResultsSumParaAdd( SqlCommand cmd )
        {
            cmd.Parameters.Add( "@oRID", SqlDbType.Int );
            cmd.Parameters.Add( "@rDat", SqlDbType.Date );
            cmd.Parameters.Add( "@subj", SqlDbType.Int );
            cmd.Parameters.Add( "@iDtl", SqlDbType.NVarChar );
            cmd.Parameters.Add( "@qty", SqlDbType.Decimal );
            cmd.Parameters.Add( "@unit", SqlDbType.NVarChar );
            cmd.Parameters.Add( "@cost", SqlDbType.Decimal );
            cmd.Parameters.Add( "@note", SqlDbType.NVarChar );
            cmd.Parameters.Add( "@teID", SqlDbType.Int );
            cmd.Parameters.Add( "@pCod", SqlDbType.Char );
            return cmd;
        }


        // 業務データTaskData読込　業務個別データ→業務データ
        public DataTable SelectTaskData( string taskCode )
        {
            string sqlStr = "SELECT *  FROM D_TaskInd LEFT JOIN D_Task ON D_TaksInd.TaskID = D_Task.TaskID WHERE D_TaskInd.TaskCode = '" + taskCode + "'";

            return SelectFull_Core( sqlStr ); ;
        }


        // 請求書
        public int Account_Insert( AccountData acnd )
        {
            if( acnd == null ) return -1;

            string sqlStr = "INSERT INTO D_Account "
                + "(PartnerCode, TaskCode, CAmount, RecordedDate, Amount, SAmount, InvoiceType, TaskEntryID) "
                + "VALUES (@pCod, @tCod, @cAmt, @rDat, @amt, @sAmt, @iType, @tedI) "
                + seliDSql;
            Int32 newProdID = -1;

            using( TransactionScope tran = new TransactionScope() )
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand( sqlStr, conn );
                    cmd = account_cmd( cmd, acnd );
                    newProdID =  TryExScalar( conn, cmd );
                    if( newProdID < 1 ) return -1;
                }
                catch( SqlException sqle )
                {
                    MessageBox.Show( "SQLエラー errorno " + Convert.ToString( sqle.Number ) + " " + sqle.Message );
                    conn.Close();
                    return -1;
                }
                conn.Close();
                tran.Complete();
            }
            return (int)newProdID;
        }


        public bool Account_Update( AccountData acnd )
        {
            if( acnd == null ) return false;

            string sqlStr = "UPDATE D_Account SET PartnerCode = @pCod, TaskCode = @tCod, CAmount = @cAmt,"
                + " RecordedDate = @rDat, Amount = @amt, SAmount = @sAmt, InvoiceType = @iType, TaskEntryID = @tedI"
                + " WHERE AccountID = @acnID";

            using( TransactionScope tran = new TransactionScope() )
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand( sqlStr, conn );
                    cmd = account_cmd( cmd, acnd );
                    cmd.Parameters.Add( "@acnID", SqlDbType.Int );
                    cmd.Parameters["@acnID"].Value = acnd.AccountID;
                    if( TryExecute( conn, cmd ) < 0 ) return false;
                }
                catch( SqlException sqle )
                {
                    MessageBox.Show( "SQLエラー errorno " + Convert.ToString( sqle.Number ) + " " + sqle.Message );
                    conn.Close();
                    return false;
                }
                conn.Close();
                tran.Complete();
            }
            return true;
        }


        public bool Account_Delete( int accountID )
        {
            string sqlStr = "DELETE FROM D_Account WHERE AccountID = " + Convert.ToString( accountID );
            return UsingTryExecute( sqlStr );
        }


        public bool AccountCont_Delete( int accountID )
        {
            string sqlStr = "DELETE FROM D_AccountCont WHERE AccountID = " + Convert.ToString( accountID );
            return UsingTryExecute( sqlStr );
        }


        public bool AccountCont_Delete( AccountData acnd )
        {
            string sqlStr = "DELETE FROM D_AccountCont WHERE AccountID = " + Convert.ToString( acnd.AccountID )
                + " AND RecordedDate = '" + acnd.RecordedDate + "'";
            return UsingTryExecute( sqlStr );
        }


        public bool AccountCont_Update( DataGridView dgv, AccountData acnd )
        {
            if( acnd == null ) return false;

            string sqlStr = "UPDATE D_AccountCont SET TaskCode = @tCod, RecordedDate = @rDat, WorkDate = @wDat,"
                + " ItemCode = @iCod, Item = @item, ItemDetail = @iDtl, Quantity = @qty, Unit = @unit, "
                + " OfficeCode = @oCod, Department = @dept, HContract = @hCont, HAmount = @hAmt"
                + " WHERE AccountID = @acnID AND WorkDate = @wDat AND Item = @item AND ItemDetail = @iDtl";

            using( TransactionScope tran = new TransactionScope() )
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand( sqlStr, conn );

                    cmd.Parameters.Add( "@tCod", SqlDbType.Char );
                    cmd.Parameters.Add( "@rDat", SqlDbType.Date );
                    cmd.Parameters.Add( "@wDat", SqlDbType.Date );
                    cmd.Parameters.Add( "@iCod", SqlDbType.VarChar );
                    cmd.Parameters.Add( "@item", SqlDbType.NVarChar );
                    cmd.Parameters.Add( "@iDtl", SqlDbType.NVarChar );
                    cmd.Parameters.Add( "@qty", SqlDbType.Decimal );
                    cmd.Parameters.Add( "@unit", SqlDbType.NVarChar );
                    cmd.Parameters.Add( "@oCod", SqlDbType.Char );
                    cmd.Parameters.Add( "@dept", SqlDbType.Char );
                    cmd.Parameters.Add( "@hCont", SqlDbType.Decimal );
                    cmd.Parameters.Add( "@hAmt", SqlDbType.Decimal );
                    cmd.Parameters.Add( "@acnID", SqlDbType.Int );

                    cmd.Parameters["@acnID"].Value = acnd.AccountID;
                    cmd.Parameters["@tCod"].Value = acnd.TaskCode;
                    cmd.Parameters["@rDat"].Value = acnd.RecordedDate;
                    cmd.Parameters["@oCod"].Value = acnd.OfficeCode;
                    cmd.Parameters["@dept"].Value = acnd.Department;

                    for( int i = 0; i < dgv.Rows.Count; i++ )
                    {
                        if(string.IsNullOrEmpty(Convert.ToString(dgv.Rows[i].Cells["Month"].Value))
                                    || string.IsNullOrEmpty( Convert.ToString( dgv.Rows[i].Cells["Day"].Value ) ) )
                        {
                            cmd.Parameters["@wDat"].Value = DateTime.MinValue;
                        }
                        else
                        {
                            cmd.Parameters["@wDat"].Value = editWorkDate( Convert.ToInt32( dgv.Rows[i].Cells["Month"].Value ),
                                                                           Convert.ToInt32( dgv.Rows[i].Cells["Day"].Value ), acnd.RecordedDate );
                        }
                        cmd.Parameters["@iCod"].Value = Convert.ToString( dgv.Rows[i].Cells["ItemCode"].Value );
                        cmd.Parameters["@item"].Value = Convert.ToString( dgv.Rows[i].Cells["Item"].Value );
                        cmd.Parameters["@iDtl"].Value = Convert.ToString( dgv.Rows[i].Cells["ItemDetail"].Value );
                        cmd.Parameters["@qty"].Value = DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["Quantity"].Value ) );
                        cmd.Parameters["@unit"].Value = Convert.ToString( dgv.Rows[i].Cells["Unit"].Value );
                        //cmd.Parameters["@cost"].Value = DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["CCost"].Value ) );
                        cmd.Parameters["@hCont"].Value = DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["HContract"].Value ) );
                        cmd.Parameters["@hAmt"].Value = DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["HAmount"].Value ) );
                        if( TryExecute( conn, cmd ) < 0 ) return false;
                    }
                }
                catch( SqlException sqle )
                {
                    MessageBox.Show( "SQLエラー errorno " + Convert.ToString( sqle.Number ) + " " + sqle.Message );
                    conn.Close();
                    return false;
                }
                conn.Close();
                tran.Complete();
            }
            return true;
        }


        //public bool AccountCont_Insert( DataGridView dgv, AccountData acnd )
        //{
        //    string sqlStr = "INSERT INTO D_AccountCont (AccountID, TaskCode, RecordedDate, WorkDate, ItemCode, Item, ItemDetail,"
        //        + " CQuantity, CCost, Unit, Quantity, Amount, OfficeCode, Department, HContract, HAmount, LNo) "
        //        + "VALUES (@acnID, @tCod, @rDat, @wDat, @iCod, @item, @iDtl, @cQty, @cost, @unit, @qty, @amt, @oCod, @dept, @hCont, @hAmt, @lNo)";

        //    using( TransactionScope tran = new TransactionScope() )
        //    using( SqlConnection conn = new SqlConnection( ConnectionString ) )
        //    {
        //        try
        //        {
        //            conn.Open();
        //            SqlCommand cmd = new SqlCommand( sqlStr, conn );
        //            cmd = accountContParaAdd( cmd );

        //            cmd.Parameters["@acnID"].Value = acnd.AccountID;
        //            cmd.Parameters["@tCod"].Value = string.IsNullOrEmpty( acnd.TaskCode ) ? "" : acnd.TaskCode;
        //            cmd.Parameters["@rDat"].Value = acnd.RecordedDate;
        //            cmd.Parameters["@oCod"].Value = acnd.OfficeCode;
        //            cmd.Parameters["@dept"].Value = acnd.Department;


        //            for( int i = 0; i < dgv.Rows.Count; i++ )
        //            {
        //                if( string.IsNullOrEmpty( Convert.ToString( dgv.Rows[i].Cells["Item"].Value ) ) &&
        //                    string.IsNullOrEmpty( Convert.ToString( dgv.Rows[i].Cells["ItemDetail"].Value ) ) ) continue;

        //                cmd = accountContParaValue( cmd, dgv.Rows[i], acnd.RecordedDate );

        //                if( acnd.InvoiceType == 0 )              // 出来高
        //                {
        //                    if( Convert.ToString( dgv.Rows[i].Cells["HContract"].Value ) != "" )
        //                    {
        //                        cmd.Parameters["@hCont"].Value = DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["HContract"].Value ) );
        //                    }
        //                }
        //                cmd.Parameters["@lNo"].Value = i;
        //                if( TryExecute( conn, cmd ) < 0 ) return false;
        //            }

        //        }
        //        catch( SqlException sqle )
        //        {
        //            MessageBox.Show( "SQLエラー errorno " + Convert.ToString( sqle.Number ) + " " + sqle.Message );
        //            conn.Close();
        //            return false;
        //        }
        //        conn.Close();
        //        tran.Complete();
        //    }
        //    return true;
        //}


        public bool AccountCont_Insert( DataGridView dgv, AccountData acnd )
        {
            string sqlStr = "INSERT INTO D_AccountCont (AccountID, TaskCode, RecordedDate, WorkDate, ItemCode, Item, ItemDetail,"
                + " CQuantity, CCost, Unit, SQuantity, SAmount, Quantity, Amount, OfficeCode, Department, HContract, HAmount, LNo) "
                + "VALUES (@acnID, @tCod, @rDat, @wDat, @iCod, @item, @iDtl, @cQty, @cost, @unit, @sQty, @sAmt, @qty, @amt, @oCod, @dept, @hCont, @hAmt, @lNo)";

            using( TransactionScope tran = new TransactionScope() )
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand( sqlStr, conn );
                    cmd = accountContParaAdd( cmd );

                    cmd.Parameters["@acnID"].Value = acnd.AccountID;
                    cmd.Parameters["@tCod"].Value = string.IsNullOrEmpty( acnd.TaskCode ) ? "" : acnd.TaskCode;
                    cmd.Parameters["@rDat"].Value = acnd.RecordedDate;
                    cmd.Parameters["@oCod"].Value = acnd.OfficeCode;
                    cmd.Parameters["@dept"].Value = acnd.Department;


                    for( int i = 0; i < dgv.Rows.Count; i++ )
                    {
                        if( string.IsNullOrEmpty( Convert.ToString( dgv.Rows[i].Cells["Item"].Value ) ) &&
                            string.IsNullOrEmpty( Convert.ToString( dgv.Rows[i].Cells["ItemDetail"].Value ) ) ) continue;

                        cmd = accountContParaValue( cmd, dgv.Rows[i], acnd.RecordedDate );

                        if( acnd.InvoiceType == 0 )              // 出来高
                        {
                            if( Convert.ToString( dgv.Rows[i].Cells["HContract"].Value ) != "" )
                            {
                                cmd.Parameters["@hCont"].Value = DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["HContract"].Value ) );
                            }
                        }
                        cmd.Parameters["@lNo"].Value = i;
                        if( TryExecute( conn, cmd ) < 0 ) return false;
                    }

                }
                catch( SqlException sqle )
                {
                    MessageBox.Show( "SQLエラー errorno " + Convert.ToString( sqle.Number ) + " " + sqle.Message );
                    conn.Close();
                    return false;
                }
                conn.Close();
                tran.Complete();
            }
            return true;
        }


        private SqlCommand account_cmd( SqlCommand cmd, AccountData acnd )
        {
            cmd.Parameters.Add( "@pCod", SqlDbType.Char );
            cmd.Parameters.Add( "@tCod", SqlDbType.Char );
            cmd.Parameters.Add( "@cAmt", SqlDbType.Decimal );
            cmd.Parameters.Add( "@rDat", SqlDbType.Date );
            cmd.Parameters.Add( "@amt", SqlDbType.Decimal );
            cmd.Parameters.Add( "@sAmt", SqlDbType.Decimal );
            cmd.Parameters.Add( "@iType", SqlDbType.Int );
            cmd.Parameters.Add( "@tedI", SqlDbType.Int );

            cmd.Parameters["@pCod"].Value = acnd.PartnerCode;
            cmd.Parameters["@tCod"].Value = acnd.TaskCode;
            cmd.Parameters["@cAmt"].Value = acnd.CAmount;
            cmd.Parameters["@rDat"].Value = acnd.RecordedDate;
            cmd.Parameters["@amt"].Value = acnd.Amount;
            cmd.Parameters["@sAmt"].Value = acnd.SAmount;
            cmd.Parameters["@iType"].Value = acnd.InvoiceType;
            cmd.Parameters["@tedI"].Value = acnd.TaskEntryID;

            return cmd;
        }


        private SqlCommand accountContParaAdd( SqlCommand cmd, AccountData acnd )
        {
            return accountContParaAdd(cmd);
        }


        private SqlCommand accountContParaAdd( SqlCommand cmd )
        {
            cmd.Parameters.Add( "@acnID", SqlDbType.Int );
            cmd.Parameters.Add( "@tCod", SqlDbType.Char );
            cmd.Parameters.Add( "@rDat", SqlDbType.Date );
            cmd.Parameters.Add( "@wDat", SqlDbType.Date );
            cmd.Parameters.Add( "@iCod", SqlDbType.VarChar );
            cmd.Parameters.Add( "@item", SqlDbType.NVarChar );
            cmd.Parameters.Add( "@iDtl", SqlDbType.NVarChar );
            cmd.Parameters.Add( "@cQty", SqlDbType.Decimal );
            cmd.Parameters.Add( "@cost", SqlDbType.Decimal );
            cmd.Parameters.Add( "@unit", SqlDbType.NVarChar );
            cmd.Parameters.Add( "@sQty", SqlDbType.Decimal );
            cmd.Parameters.Add( "@sAmt", SqlDbType.Decimal );
            cmd.Parameters.Add( "@qty", SqlDbType.Decimal );
            cmd.Parameters.Add( "@amt", SqlDbType.Decimal );
            cmd.Parameters.Add( "@oCod", SqlDbType.Char );
            cmd.Parameters.Add( "@dept", SqlDbType.Char );
            cmd.Parameters.Add( "@hCont", SqlDbType.Decimal );
            cmd.Parameters.Add( "@hAmt", SqlDbType.Decimal );
            cmd.Parameters.Add( "@lNo", SqlDbType.Int );
            return cmd;
        }


        private SqlCommand accountContParaValue( SqlCommand cmd, DataGridViewRow dgvRow, DateTime recordedDate )
        {
            if( string.IsNullOrEmpty( Convert.ToString( dgvRow.Cells["Month"].Value ) ) ||
               string.IsNullOrEmpty( Convert.ToString( dgvRow.Cells["Day"].Value ) ) )
            {
                cmd.Parameters["@wDat"].Value = DateTime.MinValue; 
            }
            else
            {
                cmd.Parameters["@wDat"].Value = editWorkDate( Convert.ToInt32( dgvRow.Cells["Month"].Value ),
                                                              Convert.ToInt32( dgvRow.Cells["Day"].Value ), recordedDate );

            }
            cmd.Parameters["@iCod"].Value = Convert.ToString( dgvRow.Cells["ItemCode"].Value );
            cmd.Parameters["@item"].Value = Convert.ToString( dgvRow.Cells["Item"].Value );
            cmd.Parameters["@iDtl"].Value = Convert.ToString( dgvRow.Cells["ItemDetail"].Value );
            cmd.Parameters["@cQty"].Value = DHandling.ToRegDecimal( Convert.ToString( dgvRow.Cells["CQuantity"].Value) );
            cmd.Parameters["@cost"].Value = DHandling.ToRegDecimal( Convert.ToString( dgvRow.Cells["CCost"].Value ) );
            cmd.Parameters["@unit"].Value = Convert.ToString( dgvRow.Cells["Unit"].Value );
            cmd.Parameters["@sQty"].Value = DHandling.ToRegDecimal( Convert.ToString( dgvRow.Cells["SQuantity"].Value ) );
            cmd.Parameters["@sAmt"].Value = DHandling.ToRegDecimal( Convert.ToString( dgvRow.Cells["SAmount"].Value ) );
            cmd.Parameters["@qty"].Value = DHandling.ToRegDecimal( Convert.ToString( dgvRow.Cells["Quantity"].Value ) );
            cmd.Parameters["@amt"].Value = DHandling.ToRegDecimal( Convert.ToString( dgvRow.Cells["Amount"].Value ) );
            cmd.Parameters["@hAmt"].Value = DHandling.ToRegDecimal( Convert.ToString( dgvRow.Cells["HAmount"].Value ) );
            cmd.Parameters["@hCont"].Value = DHandling.ToRegDecimal( Convert.ToString( dgvRow.Cells["HContract"].Value ) );
            return cmd;
        }


        //----------------------------------------------------------------------------//
        //     SubRoutine                                                             //
        //----------------------------------------------------------------------------//
        private DateTime editWorkDate( int month, int day, DateTime bDate )
        {
            if( month > bDate.Month ) return ( new DateTime( bDate.Year - 1, month, day ) );
            return ( new DateTime( bDate.Year, month, day ) );
        }

        private void storeItemsToDgv( DataGridViewRow dgvR, DataRow dr )
        {
            dgvR.Cells["ItemCode"].Value = Convert.ToString( dr["ItemCode"] );
            storeItemsToDgvExcItemCode( dgvR, dr );
        }

        private void storeItemsToDgvExcItemCode( DataGridViewRow dgvR, DataRow dr )
        {
            dgvR.Cells["Item"].Value = Convert.ToString( dr["Item"] );
            dgvR.Cells["ItemDetail"].Value = Convert.ToString( dr["ItemDetail"] );
        }

        private void storeCostsToDgv( DataGridViewRow dgvR, DataRow dr )
        {
            storeCostsToDgvExcCost( dgvR, dr );
            dgvR.Cells["Cost"].Value = Convert.ToDecimal( dr["Cost"] );
        }

        private void storeCostsToDgvExcCost( DataGridViewRow dgvR, DataRow dr )
        {
            dgvR.Cells["Quantity"].Value = Convert.ToDecimal( dr["Quantity"] );
            dgvR.Cells["Unit"].Value = Convert.ToString( dr["Unit"] );
        }

    }
}