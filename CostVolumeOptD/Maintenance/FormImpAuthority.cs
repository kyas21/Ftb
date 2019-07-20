using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
//using System.Transactions;
using System.Windows.Forms;
using ClassLibrary;

namespace Maintenance
{
    public partial class FormImpAuthority : Form
    {
        //---------------------------------------------------------------------
        //      Field
        //---------------------------------------------------------------------
        private bool iniPro = true;                 // 初期化終了フラグ
        private int[] fintCheckNum = new int[0];    // ID確認用
        private int fintMember = 0;                 // 対象社員数格納用

        //---------------------------------------------------------------------
        //      Constructor
        //---------------------------------------------------------------------
        /// <summary>
        /// コンストラクター
        /// </summary>
        public FormImpAuthority()
        {
            InitializeComponent();
        }

        //---------------------------------------------------------------------
        //      Property
        //---------------------------------------------------------------------
        //---------------------------------------------------------------------
        //      Method
        //---------------------------------------------------------------------
        /// <summary>
        /// フォームロード時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormImpAuthority_Load(object sender, EventArgs e)
        {
            // M_Membersから現職のメンバーを取得する
            MembersScData msd = new MembersScData();
            DataTable dt = msd.SelectMembersData(); 
            if (dt == null) return;
            fintMember = dt.Rows.Count;

            MembersScData[] msda = new MembersScData[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                msda[i] = new MembersScData(dt.Rows[i]);
                // データグリッドビューに社員名列を追加
                this.dgvSetting.Columns.Add("Member" + msda[i].MemberCode, msda[i].Name + $"\n({msda[i].MemberCode})");
                this.dgvSetting.Columns[i + 3].ReadOnly = true;
                this.dgvSetting.Columns[i + 3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // M_Authorizationからプログラムを取得
            AuthorizationData ad = new AuthorizationData();
            dt = ad.SelectAuthMaster();
            if (dt == null)
            {
                // 行の追加
                this.dgvSetting.Rows.Add(1);
            }
            else
            {
                // 行の追加
                this.dgvSetting.Rows.Add(dt.Rows.Count);
            }

            UiHandling uih = new UiHandling(this.dgvSetting);
            // ヘッダ背景色設定
            uih.DgvReadyNoRHeader();

            // M_Authorizationのレコードを表示
            string[] strMemberCode;             // 社員番号格納用
            if (dt != null)
            {
                Array.Resize(ref fintCheckNum, Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["AuthID"]) + 1);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // ID
                    this.dgvSetting.Rows[i].Cells[0].Value = Convert.ToString(dt.Rows[i]["AuthID"]);
                    // 使用済みID設定
                    fintCheckNum[Convert.ToInt32(dt.Rows[i]["AuthID"])] = 1;
                    // ラベル
                    this.dgvSetting.Rows[i].Cells[1].Value = Convert.ToString(dt.Rows[i]["ModuleLabel"]);
                    // プログラム
                    this.dgvSetting.Rows[i].Cells[2].Value = Convert.ToString(dt.Rows[i]["ModuleName"]);
                    // 各社員番号設定
                    strMemberCode = Convert.ToString(dt.Rows[i]["AuthMember"]).Split(',');

                    for (int j = 0; j < strMemberCode.Length; j++)
                    {
                        // 登録済み社員番号分繰り返す
                        for (int k = 3; k < fintMember + 3; k++)
                        {
                            // 全社員番号分繰り返す
                            if (this.dgvSetting.Columns[k].Name == "Member" + strMemberCode[j])
                            {
                                this.dgvSetting.Rows[i].Cells[k].Value = "YES";
                            }
                            else if (Convert.ToString(this.dgvSetting.Rows[i].Cells[k].Value) != "YES")
                            {
                                this.dgvSetting.Rows[i].Cells[k].Value = "NO";
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 3; i < fintMember + 3; i++)
                {
                    // 社員番号の全設定をNOとする
                    this.dgvSetting.Rows[0].Cells[i].Value = "NO";
                }
            }
        }

       

        /// <summary>
        /// フォーム表示後
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormImpAuthority_Shown(object sender, EventArgs e)
        {
            iniPro = false;       // 初期化処理終了
            this.dgvSetting.CurrentCell = null;
        }

        /// <summary>
        /// データグリッドビューキーダウン時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSetting_KeyDown(object sender, KeyEventArgs e)
        {
            if (iniPro) return;
            if (!this.dgvSetting.Focused) return;

            if ((e.Modifiers & Keys.Control) != Keys.Control) return;   // Ctrlキーが押下された時のみ以下処理

            DataGridView dgvObject = (DataGridView)sender;

            switch (e.KeyCode)
            {
                case Keys.I:            // 追加
                    if (dgvObject.CurrentCellAddress.Y > 0)
                    {
                        for (int i = 3; i < fintMember + 3; i++)
                        {
                            // 社員番号の全設定をNOとする
                            this.dgvSetting.Rows[dgvObject.CurrentCellAddress.Y - 1].Cells[i].Value = "NO";
                        }
                    }
                    break;
                case Keys.D:            // 削除
                    if (this.dgvSetting.Rows.Count == 0)
                    {
                        // 全削除された場合1行追加する
                        this.dgvSetting.Rows.Add(1);
                        for (int i = 3; i < fintMember + 3; i++)
                        {
                            // 社員番号の全設定をNOとする
                            this.dgvSetting.Rows[0].Cells[i].Value = "NO";
                        }
                    }
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// データグリッドビューセルクリック時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSetting_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (iniPro) return;
            if (!this.dgvSetting.Focused) return;

            // ヘッダークリック時は処理を行わない
            if (e.RowIndex < 0) return;
            // ID列からプログラム列クリック時は処理を行わない
            if (e.ColumnIndex < 3) return;

            if (Convert.ToString(this.dgvSetting.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) == "YES")
            {
                // YES → NO
                this.dgvSetting.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "NO";
            }
            else
            {
                // NO → YES
                this.dgvSetting.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "YES";
            }
        }

        /// <summary>
        /// 保存ボタン押下時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            DbAccess clsDbAccess = new DbAccess();              // DbAccessインスタンス化
            string strSQL = "";                                 // SQL格納用

            // ModuleName確認
            if (this.dgvSetting.Rows.Count != 1)
            {
                for (int i = 0; i < this.dgvSetting.Rows.Count; i++)
                {
                    if (Convert.ToString(this.dgvSetting.Rows[i].Cells[2].Value) == "")
                    {
                        // 空欄の場合はメッセージを表示して処理を終了
                        MessageBox.Show($"{i + 1}行目[{this.dgvSetting.Columns[2].HeaderText}]が空欄です。");
                        return;
                    }
                }
            }

            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(clsDbAccess.ConnectionString))
            {
                try
                {
                    conn.Open();

                    // M_Authorizationの全削除
                    strSQL = "";
                    strSQL = strSQL + "DELETE FROM M_Authorization";

                    if (TryExecute(conn, strSQL) == false) return;

                    if (this.dgvSetting.Rows.Count != 1 ||
                            Convert.ToString(this.dgvSetting.Rows[0].Cells[2].Value) != "")
                    {
                        // データグリッドビューが1行かつプログラムが空欄の場合は処理を行わない
                        // M_AuthorizationのIDENTITY_INSERTをONとする
                        strSQL = "";
                        strSQL = strSQL + "SET IDENTITY_INSERT M_Authorization ON";

                        if (TryExecute(conn, strSQL) == false) return;
                        int intID = 0;                                      // 最大ID格納用 
                        bool bolLoopFlag = true;                            // 繰り返し処理フラグ

                        // データグリッドビューの行数分繰り返す
                        for (int i = 0; i < this.dgvSetting.Rows.Count; i++)
                        {
                            // 現在のデータグリッドビューをM_Authorizationに格納
                            strSQL = "";
                            strSQL = strSQL + "INSERT INTO M_Authorization ";
                            strSQL = strSQL + "(AuthID, ModuleLabel, ModuleName, AuthMember) ";
                            strSQL = strSQL + "VALUES (";
                            // AuthID
                            if (Convert.ToString(this.dgvSetting.Rows[i].Cells[0].Value) == "")
                            {
                                // IDが設定されていない場合
                                if (intID < fintCheckNum.Length)
                                {
                                    int j = 1;          // 繰り返し処理カウント用
                                    bolLoopFlag = true;

                                    do
                                    {
                                        // 空き番号を探す
                                        if (fintCheckNum[j] == 0)
                                        {
                                            intID = j;
                                            fintCheckNum[j] = 1;
                                            bolLoopFlag = false;
                                        }

                                        j++;

                                        if (j >= fintCheckNum.Length)
                                        {
                                            intID = j;
                                            bolLoopFlag = false;
                                        }
                                    } while (bolLoopFlag == true);
                                }
                                else
                                {
                                    intID++;
                                }
                                strSQL = strSQL + intID + ", ";
                            }
                            else
                            {
                                // IDが設定されていた場合
                                strSQL = strSQL + Convert.ToInt32(this.dgvSetting.Rows[i].Cells[0].Value) + ", ";
                            }

                            // ModulLabel
                            if (Convert.ToString(this.dgvSetting.Rows[i].Cells[1].Value) == "")
                            {
                                // 空欄の場合
                                strSQL = strSQL + "NULL, ";
                            }
                            else
                            {
                                // 空欄でない場合
                                strSQL = strSQL + "'" + Convert.ToString(this.dgvSetting.Rows[i].Cells[1].Value) + "', ";
                            }

                            // ModulName
                            strSQL = strSQL + "'" + Convert.ToString(this.dgvSetting.Rows[i].Cells[2].Value) + "', ";

                            // AuthMember
                            strSQL = strSQL + "'";
                            for (int k = 3; k < fintMember + 3; k++)
                            {
                                if (Convert.ToString(this.dgvSetting.Rows[i].Cells[k].Value) == "YES")
                                {
                                    // YESの場合
                                    strSQL = strSQL + this.dgvSetting.Columns[k].Name.Replace("Member", "") + ",";
                                }
                            }

                            if (strSQL.Substring(strSQL.Length - 1, 1) == ",")
                            {
                                strSQL = strSQL.Substring(0, strSQL.Length - 1);
                            }
                            strSQL = strSQL + "')";

                            if (TryExecute(conn, strSQL) == false) return;
                        }

                        // M_AuthorizationのIDENTITY_INSERTをOFFとする
                        strSQL = "";
                        strSQL = strSQL + "SET IDENTITY_INSERT M_Authorization OFF";

                        if (TryExecute(conn, strSQL) == false) return;
                    }
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                }
                conn.Close();
                tran.Complete();
                MessageBox.Show("保存が完了しました。");
            }
        }

        /// <summary>
        /// SQL実行
        /// </summary>
        /// <param name="conn">コネクション</param>
        /// <param name="strSQL">実行SQL</param>
        /// <returns></returns>
        private bool TryExecute(SqlConnection conn, string strSQL)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException sqle)
            {
                MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                return false;
            }
            return true;
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
