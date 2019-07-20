using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary;
using ListForm;

namespace Maintenance
{
    public partial class FormMCostMnt : Form
    {
        //---------------------------------------------------------------------
        //      Field
        //---------------------------------------------------------------------
        /// <summary>
        /// 最大原価コード管理構造体
        /// </summary>
        private struct COSTINFO
        {
            public string CostCodeH;
            public int CostCodeNumL;
            public int CostCodeNumU;
        }

        /// <summary>
        /// 原価データ管理用構造体
        /// </summary>
        private struct COSTKEY
        {
            public string CostID;
            public string CostCodeH;
            public string CostCodeNum;
            public string CostName;
            public string CostDetail;
            public string CostUnit;
            public string SetCost;
            public string MemberCode;
        }

        private bool iniPro = true;
        private COSTINFO[] CostInf;                         // 初期M_Cost情報
        private COSTKEY CostKey = new COSTKEY();            // 選択レコード格納用
        private COSTKEY CostKeyReg = new COSTKEY();         // 選択レコード格納用
        private string DelCost = "";                        // 削除レコード

        private const string CODEB = "(残)";                 // 原価コード"B"付与文字
        private const string CODEK = "▲";                   // 原価コード"K"付与文字
        private const string CODEL = "★";                   // 原価コード"L"付与文字列

        //---------------------------------------------------------------------
        //      Constructor
        //---------------------------------------------------------------------
        public FormMCostMnt()
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
        private void FormMCostMentL_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;   // マウスカーソルを砂時計(Wait)

            UiHandling uih = new UiHandling(this.dataGridViewList);
            uih.DgvReadyNoRHeader();
            // 並び替えができないようにする
            uih.NoSortable();

            // 部署コンボボックス設定
            CreateCbOffice();

            // 原価マスタ表示
            CreateDataGridView();
            CostKeyControl(0);

            Cursor.Current = Cursors.Default;  // マウスカーソルを戻す
        }

        /// <summary>
        /// フォーム表示後
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMCostMentL_Shown(object sender, EventArgs e)
        {
            iniPro = false;
        }

        /// <summary>
        /// 部署コンボボックス選択変更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxOfficeCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (iniPro) return;
            if (this.labelOfficeCode.Text != this.comboBoxOfficeCode.Text)
            {
                Func<DialogResult> dialogOverLoad = DMessage.DialogOverLoad;
                if (dialogOverLoad() == DialogResult.No)
                {
                    this.comboBoxOfficeCode.Text = this.labelOfficeCode.Text;
                    return;
                }
                // 原価マスタ表示
                CreateDataGridView();
            }
        }

        /// <summary>
        /// データグリッドビューキーダウン時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewList_KeyDown(object sender, KeyEventArgs e)
        {
            if (iniPro) return;
            if (!this.dataGridViewList.Focused) return;

            DataGridView dgvObject = (DataGridView)sender;

            // Deleteキーが押されたとき
            // Wakamatsu 20170303
            //if (e.KeyData == Keys.Delete)
            if (e.KeyData == Keys.Delete || e.KeyData == Keys.Back)
            {
                // データグリッドビューデータ変更後
                DataGridViewChange(dgvObject.CurrentCellAddress);
                CostKeyControl(dgvObject.CurrentCellAddress.Y);
                return;
            }

            if ((e.Modifiers & Keys.Control) != Keys.Control) return;   // Ctrlキーが押下された時のみ以下処理

            switch (e.KeyCode)
            {
                case Keys.V:            // 貼り付け
                    // データグリッドビューデータ変更後
                    DataGridViewChange(dgvObject.CurrentCellAddress);
                    CostKeyControl(dgvObject.CurrentCellAddress.Y);
                    break;
                case Keys.I:            // 追加
                    // 原価コードを設定する
                    DataDridVewAdd(dgvObject.CurrentCellAddress.Y - 1);
                    break;
                case Keys.D:            // 削除
                    if (CostKeyReg.CostID != "")
                        DelCost += CostKeyReg.CostID + ",";
                    if (CostKeyReg.CostCodeH == "A" || CostKeyReg.CostCodeH == "B")
                    {
                        int TargetRow = DataGridViewCheck(ref CostKeyReg);
                        if (TargetRow >= 0)
                        {
                            // 原価コードと社員コードが一致した場合
                            if (Convert.ToString(this.dataGridViewList.Rows[TargetRow].Cells[0].Value) != "")
                                // 削除フラグ設定
                                DelCost += Convert.ToString(this.dataGridViewList.Rows[TargetRow].Cells[0].Value) + ",";
                            this.dataGridViewList.Rows.RemoveAt(TargetRow);
                        }
                    }
                    if (dgvObject.Rows.Count == 0)
                    {
                        // 全削除された場合1行追加する
                        this.dataGridViewList.Rows.Add(1);
                        this.dataGridViewList.Rows[0].Cells[1].Value = "A";
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// データグリッドビュー選択行変更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewList_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (iniPro) return;
            CostKeyControl(e.RowIndex);
        }

        /// <summary>
        /// データグリッドビューデータ変更後
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (iniPro) return;
            DataGridViewChange(new Point(e.ColumnIndex, e.RowIndex));
        }

        /// <summary>
        /// ボタン押下時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, EventArgs e)
        {
            if (iniPro) return;

            Button btn = (Button)sender;

            switch (btn.Name)
            {
                case "buttonSave":          // 保存ボタン    
                    MessageBox.Show(DataRegistration());
                    break;
                case "buttonEnd":           // 終了ボタン
                    this.Close();
                    break;
                default:
                    break;
            }
        }

        /*+--------------------------------------------------------------------------+*/
        /*     SubRoutine                                                            */
        /*+--------------------------------------------------------------------------+*/
        /// <summary>
        /// 部署コンボボックス設定
        /// </summary>
        private void CreateCbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit(this.comboBoxOfficeCode);
            cbe.TableData("M_Office", "OfficeCode", "OfficeName");
        }

        /// <summary>
        /// データグリッドビュー表示
        /// </summary>
        private void CreateDataGridView()
        {
            int RowCount = this.dataGridViewList.Rows.Count;
            if (RowCount > 0)
                // データグリッドビューすべての行を削除
                for (int i = 1; i <= RowCount; i++)
                    this.dataGridViewList.Rows.RemoveAt(0);

            SqlHandling sh = new SqlHandling();                         // SQL実行クラス

            string SetSQL = "";

            SetSQL += "CostID, CostCode, Item, ItemDetail, ";
            SetSQL += "Unit, Cost, MemberCode, UpdateDate ";
            SetSQL += "FROM M_Cost ";
            SetSQL += "WHERE OfficeCode = '" + Convert.ToString(this.comboBoxOfficeCode.SelectedValue) + "' ";
            SetSQL += "ORDER BY CostCode, Cost";

            DataTable dt = sh.SelectFullDescription(SetSQL);                // SQL実行
            DataRow dr = null;                                              // レコード格納用
            if (dt == null)
            {
                this.dataGridViewList.Rows.Add(1);
                this.dataGridViewList.Rows[0].Cells[1].Value = "A";
            }
            else
            {
                this.dataGridViewList.Rows.Add(dt.Rows.Count);

                DataGridViewRow TargetRow = null;                               // 格納データグリッドビュー列格納用
                for (int i = 0; i < this.dataGridViewList.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    TargetRow = this.dataGridViewList.Rows[i];

                    TargetRow.Cells[0].Value = Convert.ToString(dr["CostID"]);                                          // 原価ID                      
                    TargetRow.Cells[1].Value = Convert.ToString(dr["CostCode"]).Substring(0, 1);                                        // 原価コード                  
                    TargetRow.Cells[2].Value = Convert.ToString(dr["CostCode"]);                                        // 原価コード                  
                    TargetRow.Cells[3].Value = Convert.ToString(dr["Item"]);                                            // 原価名称                    
                    TargetRow.Cells[4].Value = Convert.ToString(dr["ItemDetail"]);                                      // 細別                        
                    TargetRow.Cells[5].Value = Convert.ToString(dr["Unit"]);                                            // 単価                        
                    TargetRow.Cells[6].Value = CheckDecimal(Convert.ToString(dr["Cost"]));                              // 原価                        
                    TargetRow.Cells[7].Value = Convert.ToString(dr["MemberCode"]);                                      // 社員番号                    
                    TargetRow.Cells[8].Value = CheckDatetTime(Convert.ToString(dr["UpdateDate"]), "yyyyMM/dd");          // 更新日                      
                }
                TargetRow.Dispose();
            }

            // 初期M_Cost情報を格納
            dt = sh.SelectFullDescription(MaxCodeSqlMake());
            if (dt == null)
            {
                CostInf = new COSTINFO[1];
                CostInf[0].CostCodeH = "A";
                CostInf[0].CostCodeNumL = 0;
                CostInf[0].CostCodeNumU = 0;
            }
            else
            {
                CostInf = new COSTINFO[dt.Rows.Count];
                for (int i = 0; i < CostInf.Length; i++)
                {
                    dr = dt.Rows[i];

                    CostInf[i].CostCodeH = Convert.ToString(dr["CodeH"]);
                    CostInf[i].CostCodeNumL = Convert.ToInt32(dr["M_CodeML"]);
                    CostInf[i].CostCodeNumU = Convert.ToInt32(dr["M_CodeMU"]);
                }
            }
            this.labelOfficeCode.Text = Convert.ToString(this.comboBoxOfficeCode.Text);

            // Wakamatsu 20170323
            this.dataGridViewList.CurrentCell = null;
        }

        /// <summary>
        /// 最大原価コード取得用SQL作成
        /// </summary>
        /// <returns></returns>
        private string MaxCodeSqlMake()
        {
            string SetSQL1 = "";                // SQL格納用
            string SetSQL2 = "";                // SQL格納用
            string SetSQL3 = "";                // SQL格納用
            string SetSQL4 = "";                // SQL格納用
            string SetSQL5 = "";                // SQL格納用
            string SetSQL6 = "";                // SQL格納用

            // ①・・・原価コードを分割する
            SetSQL1 += "SELECT LEFT(CostCode,1) AS CodeH, ";
            SetSQL1 += "CONVERT(INT,RIGHT(CostCode,LEN(CostCode)-1)) AS CodeNum ";
            SetSQL1 += "FROM M_Cost ";
            SetSQL1 += "WHERE OfficeCode = '" + Convert.ToString(this.comboBoxOfficeCode.SelectedValue) + "'";

            // ②・・・999より小さい最大値を取得する(L用)
            SetSQL2 += "SELECT MLL.CodeH, MAX(MLL.CodeNum) AS CodeM ";
            SetSQL2 += "FROM (" + SetSQL1 + ") AS MLL ";
            SetSQL2 += "WHERE MLL.CodeNum < 999 ";
            SetSQL2 += "GROUP BY MLL.CodeH";

            // ③・・・999以上の最大値を取得する(L用)
            SetSQL3 += "SELECT MUL.CodeH, MAX(MUL.CodeNum) AS CodeM ";
            SetSQL3 += "FROM (" + SetSQL1 + ") AS MUL ";
            SetSQL3 += "WHERE MUL.CodeNum >= 999 ";
            SetSQL3 += "GROUP BY MUL.CodeH";

            // ④・・・999より小さい最大値を取得する(R用)
            SetSQL4 += "SELECT MLR.CodeH, MAX(MLR.CodeNum) AS CodeM ";
            SetSQL4 += "FROM (" + SetSQL1 + ") AS MLR ";
            SetSQL4 += "WHERE MLR.CodeNum < 999 ";
            SetSQL4 += "GROUP BY MLR.CodeH";

            // ⑤・・・999以上の最大値を取得する(R用)
            SetSQL5 += "SELECT MUR.CodeH, MAX(MUR.CodeNum) AS CodeM ";
            SetSQL5 += "FROM (" + SetSQL1 + ") AS MUR ";
            SetSQL5 += "WHERE MUR.CodeNum >= 999 ";
            SetSQL5 += "GROUP BY MUR.CodeH";

            SetSQL6 += "ADT.CodeH, ";
            SetSQL6 += "IIF(ADT.CodeML IS NULL, 0, ADT.CodeML) AS M_CodeML, ";
            SetSQL6 += "IIF(ADT.CodeMU IS NULL, 999, ADT.CodeMU) AS M_CodeMU ";
            SetSQL6 += "FROM (SELECT LLD.CodeH, LLD.CodeM AS CodeML, ULD.CodeM AS CodeMU ";
            SetSQL6 += "FROM (" + SetSQL2 + ") AS LLD ";
            SetSQL6 += "LEFT JOIN (" + SetSQL3 + ") AS ULD ";
            SetSQL6 += "ON LLD.CodeH = ULD.CodeH ";
            SetSQL6 += "UNION ALL (SELECT RMS.CodeH, RMS.CodeML, RMS.CodeMU ";
            SetSQL6 += "FROM (SELECT LRD.CodeH, LRD.CodeM AS CodeML, URD.CodeM AS CodeMU ";
            SetSQL6 += "FROM (" + SetSQL5 + ") AS URD ";
            SetSQL6 += "LEFT JOIN (" + SetSQL4 + ") AS LRD ";
            SetSQL6 += "ON URD.CodeH = LRD.CodeH) AS RMS ";
            SetSQL6 += "WHERE RMS.CodeML IS NULL)) AS ADT ";

            return SetSQL6;
        }

        /// <summary>
        /// DateTime型確認(フォーマット設定付き)
        /// </summary>                                         
        /// <param name="CheckTarget">確認対象文字列</param>
        /// <param name="SetFormat">設定フォーマット</param>
        /// <returns>フォーマット設定後文字列</returns>
        private string CheckDatetTime(string CheckTarget, string SetFormat)
        {
            DateTime CheckResult = DateTime.MinValue;

            if (DateTime.TryParse(CheckTarget, out CheckResult))
                return CheckResult.ToString(SetFormat);
            else
                return "";
        }

        /// <summary>
        /// Decimal型確認(フォーマット設定付き)
        /// </summary>
        /// <param name="CheckTarget">確認対象文字列</param>
        /// <returns>フォーマット設定後文字列</returns>
        private string CheckDecimal(string CheckTarget)
        {
            decimal CheckResult = 0;

            if (decimal.TryParse(CheckTarget, out CheckResult))
                return CheckResult.ToString("#,0.00");
            else
                return "";
        }

        /// <summary>
        /// データグリッドビュー行挿入時処理
        /// </summary>
        private void DataDridVewAdd(int AddRow)
        {
            string CostCodeH = "";                  // 原価コード

            if (AddRow != 0)
                CostCodeH = Convert.ToString(this.dataGridViewList.Rows[AddRow - 1].Cells[1].Value);
            else if (AddRow != this.dataGridViewList.Rows.Count)
                CostCodeH = Convert.ToString(this.dataGridViewList.Rows[AddRow + 1].Cells[1].Value);
            else
                CostCodeH = "A";

            if (CostCodeH == "A" || CostCodeH == "B")
            {
                // 挿入位置再設定
                string TargetPre = "";
                string TargetNext = "";

                int TargetRow = AddRow;
                if (AddRow - 1 >= 0)
                {
                    // 1行上
                    TargetPre = Convert.ToString(this.dataGridViewList.Rows[AddRow - 1].Cells[2].Value);
                    if (TargetPre.Substring(0, 1) == "A")
                        if (AddRow + 1 <= this.dataGridViewList.Rows.Count)
                        {
                            // 1行下
                            TargetNext = Convert.ToString(this.dataGridViewList.Rows[AddRow + 1].Cells[2].Value);
                            if (TargetNext.Substring(0, 1) == "B")
                                if (TargetPre.Substring(1) == TargetNext.Substring(1))
                                {
                                    // 1行下と2行下が異なる場合挿入をキャンセルし再挿入する
                                    this.dataGridViewList.Rows.RemoveAt(TargetRow);
                                    TargetRow++;
                                    this.dataGridViewList.Rows.Insert(TargetRow);
                                }
                        }
                }

                // 追加原価コードがAまたはBの場合
                int MaxCodeA = MaxCodeSet("A");             // 原価コードAの最大空き番号
                int MaxCodeB = MaxCodeSet("B");             // 原価コードBの最大空き番号
                int MaxCode = 0;                            // 設定最大空き番号

                if (MaxCodeA >= MaxCodeB)
                    MaxCode = MaxCodeA;
                else
                    MaxCode = MaxCodeB;

                this.dataGridViewList.Rows[TargetRow].Cells[1].Value = "A";
                this.dataGridViewList.Rows[TargetRow].Cells[2].Value = "A" + MaxCode.ToString("000");
                this.dataGridViewList.Rows[TargetRow].Cells[6].Value = CheckDecimal("0");
                this.dataGridViewList.Rows[TargetRow].Cells[9].Value = "I";
                this.dataGridViewList.Rows[TargetRow].Cells[10].Value = "[原価名称]を入力してください。";
                // もう一行追加
                this.dataGridViewList.Rows.Insert(TargetRow + 1);
                this.dataGridViewList.Rows[TargetRow + 1].Cells[1].Value = "B";
                this.dataGridViewList.Rows[TargetRow + 1].Cells[2].Value = "B" + MaxCode.ToString("000");
                this.dataGridViewList.Rows[TargetRow + 1].Cells[6].Value = CheckDecimal("0");
                this.dataGridViewList.Rows[TargetRow + 1].Cells[9].Value = "I";
                this.dataGridViewList.Rows[TargetRow + 1].Cells[10].Value = "[原価名称]を入力してください。";
            }
            else
            {
                this.dataGridViewList.Rows[AddRow].Cells[1].Value = CostCodeH;
                this.dataGridViewList.Rows[AddRow].Cells[2].Value = CostCodeH + MaxCodeSet(CostCodeH).ToString("000");
                this.dataGridViewList.Rows[AddRow].Cells[6].Value = CheckDecimal("0");
                this.dataGridViewList.Rows[AddRow].Cells[9].Value = "I";
                this.dataGridViewList.Rows[AddRow].Cells[10].Value = "[原価名称]を入力してください。";

                if (CostCodeH == "K")
                    this.dataGridViewList.Rows[AddRow].Cells[3].Value = CODEK;
                else if (CostCodeH == "L")
                    this.dataGridViewList.Rows[AddRow].Cells[3].Value = CODEL;
            }
        }

        /// <summary>
        /// 使用済みコード管理
        /// </summary>
        /// <param name="CodeH"></param>
        /// <returns></returns>
        private int MaxCodeSet(string CodeH)
        {
            // 空き番検索
            for (int i = 0; i < CostInf.Length; i++)
            {
                if (CostInf[i].CostCodeH == CodeH)
                {
                    if (CostInf[i].CostCodeNumL == 998)
                    {
                        // 999以上の未使用値を使用
                        CostInf[i].CostCodeNumU += 1;
                        return CostInf[i].CostCodeNumU;
                    }
                    else
                    {
                        // 999より小さい未使用値を使用
                        CostInf[i].CostCodeNumL += 1;
                        return CostInf[i].CostCodeNumL;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// データグリッドビュー変更処理
        /// </summary>
        /// <param name="TargetPoint"></param>
        private void DataGridViewChange(Point TargetPoint)
        {
            DataGridViewRow TargetdgvRow = this.dataGridViewList.Rows[TargetPoint.Y];

            switch (TargetPoint.X)
            {
                // Wakamatsu 20170302
                case 2:             // 原価コード
                    if (Convert.ToString(TargetdgvRow.Cells[9].Value) != "I")
                    {
                        TargetdgvRow.Cells[2].Value = CostKey.CostCodeH + CostKey.CostCodeNum;
                        TargetdgvRow.Dispose();
                        return;
                    }

                    // 入力確認
                    string CheckCodeU = Convert.ToString(TargetdgvRow.Cells[2].Value).ToUpper();
                    string CheckResult = CostCodeCheck(CheckCodeU, TargetPoint.Y);
                    if (CheckResult != "")
                    {
                        MessageBox.Show(CheckResult);
                        TargetdgvRow.Cells[2].Value = CostKey.CostCodeH + CostKey.CostCodeNum;
                        TargetdgvRow.Dispose();
                        return;
                    }

                    TargetdgvRow.Cells[1].Value = CheckCodeU.Substring(0, 1);
                    TargetdgvRow.Cells[2].Value = CheckCodeU;
                    string CheckCodeH = "";

                    if (CostKey.CostCodeH == "A" || CostKey.CostCodeH == "B")
                    {
                        if (CostKey.CostCodeH == "A")
                            CheckCodeH = "B";
                        else
                            CheckCodeH = "A";

                        if (Convert.ToString(TargetdgvRow.Cells[1].Value) == "A" || Convert.ToString(TargetdgvRow.Cells[1].Value) == "B")
                        {
                            // 変更前も変更後も原価コードの先頭文字列が"A"または"B"の場合
                            for (int i = 0; i < this.dataGridViewList.Rows.Count; i++)
                            {
                                if (Convert.ToString(this.dataGridViewList.Rows[i].Cells[2].Value) == CheckCodeH + CostKey.CostCodeNum &&
                                    Convert.ToString(this.dataGridViewList.Rows[i].Cells[7].Value) == CostKey.MemberCode)
                                {
                                    if (Convert.ToString(TargetdgvRow.Cells[1].Value) == "A")
                                        CheckCodeH = "B";
                                    else
                                        CheckCodeH = "A";

                                    this.dataGridViewList.Rows[i].Cells[1].Value = CheckCodeH;
                                    this.dataGridViewList.Rows[i].Cells[2].Value = CheckCodeH + Convert.ToString(TargetdgvRow.Cells[2].Value).Substring(1);
                                }
                            }
                        }
                        else
                        {
                            // 変更前の原価コードの先頭文字列が"A"または"B"で変更後の原価コードの先頭文字列が"A"または"B"でない場合
                            for (int i = 0; i < this.dataGridViewList.Rows.Count; i++)
                                if (Convert.ToString(this.dataGridViewList.Rows[i].Cells[2].Value) == CheckCodeH + CostKey.CostCodeNum &&
                                    Convert.ToString(this.dataGridViewList.Rows[i].Cells[7].Value) == CostKey.MemberCode)
                                    this.dataGridViewList.Rows.RemoveAt(i);
                            TargetdgvRow.Cells[7].Value = "";
                        }
                    }
                    else
                    {
                        if (Convert.ToString(TargetdgvRow.Cells[1].Value) == "A" || Convert.ToString(TargetdgvRow.Cells[1].Value) == "B")
                        {
                            // 変更前の原価コードの先頭文字列が"A"または"B"でなく変更後の原価コードの先頭文字列が"A"または"B"場合
                            if (Convert.ToString(TargetdgvRow.Cells[1].Value) == "A")
                                CheckCodeH = "B";
                            else
                                CheckCodeH = "A";

                            TargetdgvRow.Cells[3].Value = "";
                            TargetdgvRow.Cells[6].Value = CheckDecimal("0");
                            TargetdgvRow.Cells[7].Value = "";
                            TargetdgvRow.Cells[10].Value = "[原価名称]を入力してください。";
                            this.dataGridViewList.Rows.Insert(TargetPoint.Y + 1);
                            this.dataGridViewList.Rows[TargetPoint.Y + 1].Cells[1].Value = CheckCodeH;
                            this.dataGridViewList.Rows[TargetPoint.Y + 1].Cells[2].Value = CheckCodeH + Convert.ToString(TargetdgvRow.Cells[2].Value).Substring(1);
                            this.dataGridViewList.Rows[TargetPoint.Y + 1].Cells[3].Value = "";
                            this.dataGridViewList.Rows[TargetPoint.Y + 1].Cells[6].Value = CheckDecimal("0");
                            this.dataGridViewList.Rows[TargetPoint.Y + 1].Cells[7].Value = "";
                            this.dataGridViewList.Rows[TargetPoint.Y + 1].Cells[10].Value = "[原価名称]を入力してください。";
                        }
                    }

                    if (Convert.ToString(TargetdgvRow.Cells[1].Value) == "K")
                    {
                        if (Convert.ToString(TargetdgvRow.Cells[3].Value) == "" || Convert.ToString(TargetdgvRow.Cells[3].Value) == CODEL)
                            TargetdgvRow.Cells[3].Value = CODEK;
                    }
                    else if (Convert.ToString(TargetdgvRow.Cells[1].Value) == "L" && Convert.ToString(TargetdgvRow.Cells[3].Value) == "")
                    {
                        if (Convert.ToString(TargetdgvRow.Cells[3].Value) == "" || Convert.ToString(TargetdgvRow.Cells[3].Value) == CODEK)
                            TargetdgvRow.Cells[3].Value = CODEL;
                    }

                    break;
                // Wakamatsu 20170302
                case 3:             // 原価名称
                    if (Convert.ToString(TargetdgvRow.Cells[9].Value) != "I")
                    {
                        TargetdgvRow.Cells[3].Value = CostKey.CostName;
                        TargetdgvRow.Dispose();
                        return;
                    }

                    if (Convert.ToString(TargetdgvRow.Cells[3].Value) == "")
                    {
                        // 社員番号を空欄とする
                        TargetdgvRow.Cells[7].Value = "";
                        if (Convert.ToString(TargetdgvRow.Cells[1].Value) == "A" ||
                            Convert.ToString(TargetdgvRow.Cells[1].Value) == "B")
                            // 対となる原価コード変更
                            PairCostControl("", "");
                    }
                    else
                    {
                        if (Convert.ToString(TargetdgvRow.Cells[1].Value) == "A" ||
                            Convert.ToString(TargetdgvRow.Cells[1].Value) == "B")
                        {
                            // 原価名称編集後
                            ListFormDataOp lo = new ListFormDataOp();
                            // メンバー取得
                            MembersScData[] msdl = lo.SelectMembersScData(Convert.ToString(TargetdgvRow.Cells[3].Value), 0);
                            MembersScData msd = null;
                            if (msdl != null)
                                msd = FormMembersList.ReceiveItems(msdl);
                            if (msd == null)
                            {
                                // 選択されなかった場合
                                msd = new MembersScData();
                                msd.Name = "";
                                msd.MemberCode = "";
                                TargetdgvRow.Cells[3].Value = msd.Name;
                                TargetdgvRow.Cells[7].Value = msd.MemberCode;
                            }
                            else
                            {
                                // 選択された社員情報を格納
                                TargetdgvRow.Cells[3].Value = msd.Name;
                                TargetdgvRow.Cells[7].Value = msd.MemberCode;

                                string OverlapCode = "";                // 重複原価コード格納用
                                string[] OverlapCodeList;               // 重複原価コード格納用(配列)
                                bool OverlapFlag = false;               // 重複社員番号フラグ

                                // 選択された場合
                                for (int i = 0; i < this.dataGridViewList.Rows.Count; i++)
                                    if (Convert.ToString(this.dataGridViewList.Rows[i].Cells[1].Value) == "A" ||
                                        Convert.ToString(this.dataGridViewList.Rows[i].Cells[1].Value) == "B")
                                        // 原価コードが"A"または"B"の場合
                                        if (Convert.ToString(this.dataGridViewList.Rows[i].Cells[7].Value) == msd.MemberCode)
                                            // 社員番号が同じ場合
                                            OverlapCode += Convert.ToString(this.dataGridViewList.Rows[i].Cells[2].Value) + ",";

                                // 社員番号重複確認
                                if (OverlapCode.Length != 0)
                                    // 末尾の","を外す
                                    OverlapCode = OverlapCode.Substring(0, OverlapCode.Length - 1);
                                OverlapCodeList = OverlapCode.Split(',');
                                if (OverlapCodeList.Length == 2)
                                {
                                    // 社員番号の重複が2つの場合
                                    if (OverlapCodeList[0].Substring(0, 1) == OverlapCodeList[1].Substring(0, 1))
                                        // 社員番号が重複している行の原価コードの先頭が同じ場合登録不可
                                        OverlapFlag = true;
                                    else if (OverlapCodeList[0].Substring(1) != OverlapCodeList[1].Substring(1))
                                        // 社員番号が重複している行の原価コードの先頭以外が異なる場合登録不可
                                        OverlapFlag = true;
                                }
                                else if (OverlapCodeList.Length > 2)
                                    // 社員番号が重複している行が３行以上の場合登録不可
                                    OverlapFlag = true;

                                if (OverlapFlag == true)
                                {
                                    MessageBox.Show("原価コード:" + Convert.ToString(TargetdgvRow.Cells[2].Value) + "\r\n" +
                                                    "原価名称:" + Convert.ToString(TargetdgvRow.Cells[3].Value) + "\r\n" +
                                                    "すでに登録されている社員番号です。");
                                    TargetdgvRow.Cells[3].Value = "";
                                    TargetdgvRow.Cells[7].Value = "";
                                    msd.Name = "";
                                    msd.MemberCode = "";
                                }
                            }

                            if (Convert.ToString(TargetdgvRow.Cells[1].Value) == "B" && msd.Name != "")
                                TargetdgvRow.Cells[3].Value += CODEB;

                            // 対となる原価コード変更
                            PairCostControl(msd.Name, msd.MemberCode);
                        }
                    }
                    // 変更確認
                    if (Convert.ToString(TargetdgvRow.Cells[3].Value) != CostKey.CostName)
                        if (Convert.ToString(TargetdgvRow.Cells[9].Value) != "I")
                            TargetdgvRow.Cells[9].Value = "U";
                    break;
                case 4:             // 細別
                    // 変更確認
                    if (Convert.ToString(TargetdgvRow.Cells[4].Value) != CostKey.CostDetail)
                        if (Convert.ToString(TargetdgvRow.Cells[9].Value) != "I")
                            TargetdgvRow.Cells[9].Value = "U";
                    break;
                case 5:             // 単位
                    // 変更確認
                    if (Convert.ToString(TargetdgvRow.Cells[5].Value) != CostKey.CostUnit)
                        if (Convert.ToString(TargetdgvRow.Cells[9].Value) != "I")
                            TargetdgvRow.Cells[9].Value = "U";
                    break;
                case 6:             // 原価
                    TargetdgvRow.Cells[6].Value = CheckDecimal(Convert.ToString(TargetdgvRow.Cells[6].Value));
                    // 変更確認
                    if (Convert.ToString(TargetdgvRow.Cells[6].Value) != CostKey.SetCost)
                        if (Convert.ToString(TargetdgvRow.Cells[9].Value) != "I")
                            TargetdgvRow.Cells[9].Value = "U";
                    break;
                default:            // その他
                    break;
            }

            // 入力確認
            InputCheck(TargetPoint.Y);
            TargetdgvRow.Dispose();
        }

        /// <summary>
        /// 対となる原価コード列設定
        /// </summary>
        /// <param name="CostName"></param>
        /// <param name="MemberCode"></param>
        private void PairCostControl(string CostName, string MemberCode)
        {
            // AまたはBの同じ番号を取得する
            int TargetRow = DataGridViewCheck(ref CostKey);

            if (TargetRow >= 0)
            {
                // 選択された社員情報を格納
                this.dataGridViewList.Rows[TargetRow].Cells[3].Value = CostName;
                this.dataGridViewList.Rows[TargetRow].Cells[7].Value = MemberCode;

                if (CostKey.CostCodeH == "B" && CostName != "")
                    this.dataGridViewList.Rows[TargetRow].Cells[3].Value += CODEB;

                // 変更確認
                if (Convert.ToString(this.dataGridViewList.Rows[TargetRow].Cells[3].Value) != CostKey.CostName)
                    if (Convert.ToString(this.dataGridViewList.Rows[TargetRow].Cells[9].Value) != "I")
                        this.dataGridViewList.Rows[TargetRow].Cells[9].Value = "U";
                // 入力確認
                InputCheck(TargetRow);
            }
        }

        /// <summary>
        /// データグリッドビュー内確認
        /// </summary>
        /// <param name="CostkeyL"></param>
        /// <returns></returns>
        private int DataGridViewCheck(ref COSTKEY CostkeyL)
        {
            string CheckCode = "";

            if (CostkeyL.CostCodeH == "A")
                CheckCode = "B" + CostkeyL.CostCodeNum;
            else
                CheckCode = "A" + CostkeyL.CostCodeNum;

            for (int i = 0; i < this.dataGridViewList.Rows.Count; i++)
            {
                if (Convert.ToString(this.dataGridViewList.Rows[i].Cells[2].Value) == CheckCode &&
                    Convert.ToString(this.dataGridViewList.Rows[i].Cells[7].Value) == CostkeyL.MemberCode)
                {
                    // 原価コードが同じかつ社員番号が同じ場合
                    CostkeyL.CostCodeH = CheckCode.Substring(0, 1);
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// データグリッドビュー選択情報取得
        /// </summary>
        /// <param name="TargetRow"></param>
        private void CostKeyControl(int TargetRow)
        {
            // 選択原価情報を1世代保持
            CostKeyReg.CostID = CostKey.CostID;
            CostKeyReg.CostCodeH = CostKey.CostCodeH;
            CostKeyReg.CostCodeNum = CostKey.CostCodeNum;
            CostKeyReg.CostName = CostKey.CostName;
            CostKeyReg.CostDetail = CostKey.CostDetail;
            CostKeyReg.CostUnit = CostKey.CostUnit;
            CostKeyReg.SetCost = CostKey.SetCost;
            CostKeyReg.MemberCode = CostKey.MemberCode;

            // 選択原価情報を格納
            CostKey.CostID = Convert.ToString(this.dataGridViewList.Rows[TargetRow].Cells[0].Value);
            CostKey.CostCodeH = Convert.ToString(this.dataGridViewList.Rows[TargetRow].Cells[1].Value);
            if (Convert.ToString(this.dataGridViewList.Rows[TargetRow].Cells[2].Value).Length > 1)
                CostKey.CostCodeNum = Convert.ToString(this.dataGridViewList.Rows[TargetRow].Cells[2].Value).Substring(1);
            else
                CostKey.CostCodeNum = "";
            CostKey.CostName = Convert.ToString(this.dataGridViewList.Rows[TargetRow].Cells[3].Value);
            CostKey.CostDetail = Convert.ToString(this.dataGridViewList.Rows[TargetRow].Cells[4].Value);
            CostKey.CostUnit = Convert.ToString(this.dataGridViewList.Rows[TargetRow].Cells[5].Value);
            CostKey.SetCost = Convert.ToString(this.dataGridViewList.Rows[TargetRow].Cells[6].Value);
            CostKey.MemberCode = Convert.ToString(this.dataGridViewList.Rows[TargetRow].Cells[7].Value);
        }

        /// <summary>
        /// 入力確認
        /// </summary>
        /// <param name="TargetRow"></param>
        private void InputCheck(int TargetRow)
        {
            DataGridViewRow TargetdgvRow = this.dataGridViewList.Rows[TargetRow];

            // 入力確認(原価名称)
            if (Convert.ToString(TargetdgvRow.Cells[3].Value) == "")
            {
                TargetdgvRow.Cells[10].Value = "[原価名称]を入力してください。";
                TargetdgvRow.Dispose();
                return;
            }

            // 入力確認(原価)
            if (Convert.ToString(TargetdgvRow.Cells[6].Value) == "")
            {
                TargetdgvRow.Cells[10].Value = "[原価]を入力してください。";
                TargetdgvRow.Dispose();
                return;
            }
            TargetdgvRow.Cells[10].Value = "";
            TargetdgvRow.Dispose();
        }

        // Wakamatsu 20170302
        private string CostCodeCheck(string CostCode, int TargetRow)
        {
            // コード確認(文字数)
            if (CostCode.Length != 4 && CostCode.Length != 5)
                return "原価コードは4桁または5桁で入力してください。";

            // コード確認(先頭文字以外)
            int WorkInt = 0;
            if (int.TryParse(CostCode.Substring(1), out WorkInt) == false)
                return "原価コードの先頭文字列以外は数字で入力してください。";

            // コード確認(2文字目)
            if (CostCode.Length == 5 && CostCode.Substring(1, 1) == "0")
                return "原価コードの数字部分が999以下の場合は3桁で入力してください。\r\n"
                        + "例：" + CostCode + "→" + CostCode.Substring(0, 1) + CostCode.Substring(2);

            // コード確認(重複)
            string CheckCodeH = "";

            switch (CostCode.Substring(0, 1))
            {
                case "A":           // 基がA
                    CheckCodeH = "B";
                    break;
                case "B":           // 基がB
                    CheckCodeH = "A";
                    break;
                default:            // 上記以外
                    CheckCodeH = CostCode.Substring(0, 1);
                    break;
            }
            for (int i = 0; i < this.dataGridViewList.Rows.Count; i++)
                if (i != TargetRow)
                    if (Convert.ToString(this.dataGridViewList.Rows[i].Cells[2].Value) == CostCode ||
                        Convert.ToString(this.dataGridViewList.Rows[i].Cells[2].Value) == CheckCodeH + CostCode.Substring(1))
                        return "入力された原価コードは使用されています。";

            // 使用済み番号制御
            for (int i = 0; i < CostInf.Length; i++)
            {
                if (CostInf[i].CostCodeH == CostCode.Substring(0, 1))
                {
                    if (Convert.ToInt32(CostCode.Substring(1)) > 998)
                    {
                        if (CostInf[i].CostCodeNumU < Convert.ToInt32(CostCode.Substring(1)))
                            // 999以上の最大値を更新
                            CostInf[i].CostCodeNumU = Convert.ToInt32(CostCode.Substring(1));
                    }
                    else
                    {
                        if (CostInf[i].CostCodeNumL < Convert.ToInt32(CostCode.Substring(1)))
                            // 999より小さい最大値を更新
                            CostInf[i].CostCodeNumL = Convert.ToInt32(CostCode.Substring(1));
                    }
                }
            }

            return "";
        }
        // Wakamatsu 20170302

        /// <summary>
        /// 原価データ登録処理
        /// </summary>
        /// <returns></returns>
        private string DataRegistration()
        {
            // エラーメッセージ確認
            for (int i = 0; i < this.dataGridViewList.Rows.Count; i++)
                if (Convert.ToString(this.dataGridViewList.Rows[i].Cells[10].Value) != "")
                    return "原価コード:" + Convert.ToString(this.dataGridViewList.Rows[i].Cells[2].Value) + "\r\n" +
                            "原価名称:" + Convert.ToString(this.dataGridViewList.Rows[i].Cells[3].Value) + "\r\n" +
                            Convert.ToString(this.dataGridViewList.Rows[i].Cells[10].Value);

            MasterMaintOp mmo = new MasterMaintOp();
            if (mmo.MaintCostByUIData(this.dataGridViewList, DelCost,
                                        Convert.ToString(this.comboBoxOfficeCode.SelectedValue)) == true)
            {
                Cursor.Current = Cursors.WaitCursor;    // マウスカーソルを砂時計(Wait)
                // 原価マスタ表示
                CreateDataGridView();
                Cursor.Current = Cursors.Default;       // マウスカーソルを戻す
                return "保存完了しました。";
            }
            else
                return "保存完了できませんでした。";
        }

    }

}
