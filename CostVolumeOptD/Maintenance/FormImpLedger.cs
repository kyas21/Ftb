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
using ClosedXML.Excel;
using System.Data.SqlClient;
using System.Transactions;

namespace Maintenance
{
    public partial class FormImpLedger : Form
    {
        //----------------------------------------------------------------------
        //     Field
        //----------------------------------------------------------------------
        /// <summary>
        /// インターフェース用構造体
        /// </summary>
        private struct INPORTIF
        {
            public string TaskCode;                 // 業務番号
            public string OfficeCode;               // 事業所コード
            public string YearMonth;                // 年月
            // Wakamatsu 20170331
            public decimal MonthlyVolume;           // 受注累計
            public decimal VolUncomp;               // 出来高未成業務累計
            public decimal VolClaimRem;             // 出来高未請求累計
            public decimal VolClaim;                // 出来高請求累計
            // Wakamatsu 20170331
            // Wakamatsu 20170302
            //public decimal VolumeCum;               // 出来高累計
            public decimal? VolumeCum;              // 出来高累計
            // Wakamatsu 20170302
            public decimal ClaimCum;                // 請求累計
            public DateTime ClaimDate;              // 請求日
            public decimal PaymentCum;              // 入金累計
            public DateTime PaymentDate;            // 入金日
            // Wakamatsu 20170302
            // Wakamatsu 20170331
            public decimal MonthlyCost;             // 原価累計
            // Wakamatsu 20170331
            //public int NotIncomFlag;                // 未収入金計算フラグ
            public string CarryOverPlanned;         // 繰越予定額
            public int TaskStat;                    // 業務状況
            public string ItemName;                 // 項目名
            public string ResultMsg;                // メッセージ
            public bool AddFlag;                    // 処理フラグ
            public string ResultValue;              // 処理結果
        }

        private bool initProc = true;               // 初期処理中true,初期処理完了false
        private string fFileName;                   // ファイル名格納用
        private string[] OfficeArray;

        private const string HQOffice = "本社";           // 部門設定フラグ

        private const int P_TASKCODECOL = 27;           // 業務コード列
        private const int P_LVOLUNCOMPCOL = 36;         // 出来高未成業務金額列
        private const int P_LVOLCLAIMREMCOL = 37;       // 出来高未請求金額列
        private const int P_LVOLCLAIMCOL = 38;          // 出来高請求金額列
        private const int P_LMONTHLYCLAIMCOL = 41;      // 請求単月金額列
        private const int P_LVOLPAIDCOL = 39;           // 入金単月金額列
        // Wakamatsu 20170331
        //private const int P_MONTHLYVOLUMECOL = 45;      // 受注単月金額列
        //private const int P_VOLUNCOMPCOL = 46;          // 出来高未成業務金額列
        //private const int P_VOLCLAIMREMCOL = 47;        // 出来高未請求金額列
        //private const int P_VOLCLAIMCOL = 48;           // 出来高請求金額列
        //private const int P_MONTHLYCLAIMCOL = 51;       // 請求単月金額列
        //private const int P_VOLPAIDCOL = 49;            // 入金単月金額列
        //private const int P_BALANCECLAIMCOL = 181;      // 残請求高金額列
        //// Wakamatsu 20170302
        //private const int P_DEPOSITCOL = 194;           // 未成業務受入金額列
        //private const int P_MONTHLYCOSTCOL = 54;        // 単月原価金額列
        //private const int P_CLAIMDATECOL = 52;          // 請求日列
        //private const int P_PAINDDATECOL = 53;          // 入金日列
        private const int P_LMONTHLYVOLUMECOL = 35;      // 受注単月金額列
        private const int P_LBALANCECLAIMCOL = 180;      // 残請求高金額列
        private const int P_LDEPOSITCOL = 193;           // 未成業務受入金額列
        private const int P_LMONTHLYCOSTCOL = 44;        // 単月原価金額列
        private const int P_LCLAIMDATECOL = 42;          // 請求日列
        private const int P_LPAINDDATECOL = 43;          // 入金日列
        // Wakamatsu 20170331
        private const int P_CARRYOVERPLANNEDCOL = 207;  // 繰越予定額列
        private const int P_TASKSTATCOL = 206;          // 業務状況列
        private const int P_NOTECOL = 165;              // コメント列

        private const int P_BALANCECLAIMDEFF = 1;       // 残請求高金額列差分
        private const int P_ATHERDEFF = 10;             // その他の列差分

        private const int P_HEADER = 2;                 // ヘッダ行                 

        //----------------------------------------------------------------------
        //     Contructor
        //----------------------------------------------------------------------
        /// <summary>
        /// コンストラクター
        /// </summary>
        public FormImpLedger()
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
        private void FormImpSurvey_Load(object sender, EventArgs e)
        {
            //部署コンボボックス設定
            CreateCbOffice();

            //部門コンボボックス設定
            CreateCbDepartment();
        }

        /// <summary>
        /// 部署コンボボックス設定
        /// </summary>
        private void CreateCbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit(comboBoxOfficeCode);
            cbe.TableData("M_Office", "OfficeCode", "OfficeName");

            OfficeArray = new string[cbe.ValueItem.Length];
            Array.Copy(cbe.ValueItem, 0, OfficeArray, 0, OfficeArray.Length);
        }

        /// <summary>
        /// 部門コンボボックス設定
        /// </summary>
        private void CreateCbDepartment()
        {
            ComboBoxEdit cbe = new ComboBoxEdit(comboBoxDepartment);
            cbe.DepartmentList((comboBoxOfficeCode.Text == HQOffice) ? "DEPH" : "DEPB", 1);
        }

        /// <summary>
        /// フォーム表示完了時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormImpSurvey_Shown(object sender, EventArgs e)
        {
            initProc = false;
        }

        /// <summary>
        /// 部署コンボボックス変更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxOfficeCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initProc) return;
            comboBoxDepartment.Visible = false;

            if (comboBoxOfficeCode.SelectedIndex == 0) comboBoxDepartment.Visible = true;
        }

        /// <summary>
        /// 各ボタン押下時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, EventArgs e)
        {
            if (initProc) return;

            Button btn = (Button)sender;

            switch (btn.Name)
            {
                case "buttonOpen":          // ファイル指定ボタン押下時

                    fFileName = Files.Open("*.xlsm", Folder.MyDocuments(), "xlsm");

                    if (fFileName == null)
                    {
                        textBoxMsg.AppendText("× " + "ファイルが指定されていません。処理続行不可能です。\r\n");
                    }
                    else
                    {
                        textBoxMsg.AppendText("☆ " + fFileName + "の内容で出来高データを登録・更新します。\r\n");
                    }
                    break;
                case "buttonCancel":        // 取消ボタン押下時
                    this.textBoxMsg.Text = "";
                    fFileName = "";
                    break;
                case "buttonStart":         // 開始ボタン押下時
                    string ResultMsg = "";

                    ResultMsg = SettingCheck();
                    // 設定項目確認
                    if (ResultMsg == "")
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        textBoxMsg.AppendText("☆ 処理を開始しました。\r\n" +
                                                "　　　　処理実行中・・・\r\n");
                        ResultMsg = ExcelInport();
                        textBoxMsg.AppendText(ResultMsg + "☆ 処理が終了しました。\r\n");
                        Cursor.Current = Cursors.Default;
                    }
                    else
                    {
                        textBoxMsg.AppendText(ResultMsg + "\r\n");
                    }
                    break;
                case "buttonEnd":           // 終了ボタン押下時
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
        /// コントロール設定確認
        /// </summary>
        /// <returns>確認結果 ALLOKで""</returns>
        private string SettingCheck()
        {
            int LastYear = 0;                           // 年格納用

            // 年度テキストボックス確認
            if (!int.TryParse(this.textBoxYear.Text, out LastYear))
            {
                return "× 年度を設定してください。";
            }

            // 部門コンボボックス確認
            if (comboBoxOfficeCode.SelectedValue.ToString() != "H" && comboBoxDepartment.Visible == true)
            {
                return "× 部署を選択してください。";
            }

            // ファイル確認
            if (!System.IO.File.Exists(fFileName))
            {
                // 未選択
                return "× 取り込むファイルを指定してください。";
            }
            else
            {
                // 拡張子確認
                if (System.IO.Path.GetExtension(fFileName) != ".xlsx" &&
                    System.IO.Path.GetExtension(fFileName) != ".xlsm")
                {
                    return "× " + fFileName + "は処理できないファイルです。";
                }
            }
            return "";
        }

        /// <summary>
        /// Excelファイル取り込み処理
        /// </summary>
        /// <returns>処理結果を示す文字列</returns>
        private string ExcelInport()
        {
            INPORTIF InportIF = new INPORTIF();                         // 処理結果格納用
            int i = P_HEADER + 2;                                       // データ開始位置
            int AddCount = 0;                                           // 処理件数格納用

            try
            {
                using (XLWorkbook wbObject = new XLWorkbook(fFileName))
                {
                    if (wbObject.Worksheet("新台帳").Cell(i, P_TASKCODECOL).GetString() != "")
                    {
                        string SetSQL = "";                                 // SQL格納用
                        int VolumeID = 0;                                   // テーブル内オートナンバー格納用
                        int YearPeriod = 0;                                 // 年度格納用

                        do
                        {
                            // 年度格納
                            YearPeriod = Convert.ToInt32(this.textBoxYear.Text);

                            InportIF.AddFlag = true;
                            // 業務番号取得
                            InportIF.TaskCode = wbObject.Worksheet("新台帳").Cell(i, P_TASKCODECOL).GetString();

                            // 事業コード取得
                            InportIF.OfficeCode = this.comboBoxOfficeCode.SelectedValue.ToString();

                            InportIF.YearMonth = this.textBoxYear.Text + "年度";

                            // 未収入金算出用変数初期化
                            // Wakamatsu 20170302
                            //InportIF.NotIncomFlag = 0;
                            // Wakamatsu 20170302
                            //InportIF.VolumeCum = 0;
                            InportIF.VolumeCum = null;
                            // Wakamatsu 20170302
                            InportIF.ClaimCum = 0;
                            InportIF.PaymentCum = 0;
                            // Wakamatsu 20170331
                            InportIF.MonthlyVolume = 0;
                            InportIF.VolUncomp = 0;
                            InportIF.VolClaim = 0;
                            InportIF.VolClaimRem = 0;
                            InportIF.MonthlyCost = 0;
                            InportIF.ClaimDate = DateTime.MinValue;
                            InportIF.PaymentDate = DateTime.MinValue;
                            // Wakamatsu 20170331

                            // 繰越額
                            InportIF.ItemName = "繰越額";
                            InportIF.CarryOverPlanned = DecimalConvert(wbObject, i, P_CARRYOVERPLANNEDCOL, ref InportIF);

                            // 業務状況取得
                            InportIF.ItemName = "業務状況";
                            InportIF.TaskStat = IntConvert(wbObject, i, P_TASKSTATCOL, ref InportIF);

                            // Wakamatsu 20170331
                            // 前年度受注
                            InportIF.ItemName = "受注単月";
                            DecimalConvert(wbObject, i, P_LMONTHLYVOLUMECOL, ref InportIF);
                            // Wakamatsu 20170331

                            // 前年度未成業務出来高
                            InportIF.ItemName = "未成業務出来高";
                            DecimalConvert(wbObject, i, P_LVOLUNCOMPCOL, ref InportIF);

                            // 前年度未請求出来高
                            InportIF.ItemName = "未請求出来高";
                            DecimalConvert(wbObject, i, P_LVOLCLAIMREMCOL, ref InportIF);

                            // 前年度請求出来高
                            InportIF.ItemName = "請求出来高";
                            DecimalConvert(wbObject, i, P_LVOLCLAIMCOL, ref InportIF);

                            // 前年度請求単月
                            InportIF.ItemName = "請求単月";
                            DecimalConvert(wbObject, i, P_LMONTHLYCLAIMCOL, ref InportIF);

                            // Wakamatsu 20170331
                            // 前年度請求日
                            InportIF.ItemName = "請求日";
                            DateConvert(wbObject, i, P_LCLAIMDATECOL, ref InportIF);
                            // Wakamatsu 20170331

                            // 前年度入金額
                            InportIF.ItemName = "入金額";
                            DecimalConvert(wbObject, i, P_LVOLPAIDCOL, ref InportIF);

                            // Wakamatsu 20170331
                            // 前年度入金日
                            InportIF.ItemName = "入金日";
                            DateConvert(wbObject, i, P_LPAINDDATECOL, ref InportIF);

                            // 前年度原価
                            InportIF.ItemName = "原価単月";
                            DecimalConvert(wbObject, i, P_LMONTHLYCOSTCOL, ref InportIF);
                            // Wakamatsu 20170331

                            if (InportIF.AddFlag == true)
                            {
                                // Wakamatsu
                                //int j = 4;          // 繰り返し処理カウント用
                                int j = 1;          // 繰り返し処理カウント用
                                // Wakamatsu

                                // 1年分繰り返す(取得年7月～取得翌年6月)
                                DbAccess clsDbAccess = new DbAccess();              // DbAccessインスタンス化

                                using (TransactionScope tran = new TransactionScope())
                                {
                                    do
                                    {
                                        // 年度設定
                                        // Wakamatsu
                                        //if(j == 10) YearPeriod++;
                                        if (j == 7) YearPeriod++;
                                        // Wakamatsu

                                        // 年月設定
                                        // Wakamatsu
                                        //InportIF.YearMonth = (j < 10 ? YearPeriod + (j + 3).ToString("00") : YearPeriod + (j - 9).ToString("00"));
                                        InportIF.YearMonth = (j < 7 ? YearPeriod + (j + 6).ToString("00") : YearPeriod + (j - 6).ToString("00"));
                                        // Wakamatsu

                                        // D_Volumeのレコード確認
                                        VolumeID = OverlapCheck(ref InportIF);

                                        SetSQL = "";
                                        // Wakamatsu 20170302
                                        //InportIF.NotIncomFlag = 0;

                                        // 登録データ作成
                                        if (VolumeID == -1)
                                        {
                                            // 新規
                                            SetSQL = SetSQL + "INSERT INTO D_Volume ";
                                            SetSQL = SetSQL + "(TaskCode, OfficeCode, Department, YearMonth, ";
                                            SetSQL = SetSQL + "MonthlyVolume, VolUncomp, VolClaimRem, VolClaim, ";
                                            SetSQL = SetSQL + "MonthlyClaim, VolPaid, BalanceClaim, MonthlyCost, ";
                                            // Wakamatsu 20170302
                                            //SetSQL = SetSQL + "BalanceIncom, ClaimDate, PaidDate, CarryOverPlanned, ";
                                            //SetSQL = SetSQL + "Comment, TaskStat, Note) ";
                                            SetSQL = SetSQL + "BalanceIncom, ClaimDate, PaidDate, Deposit1, ";
                                            SetSQL = SetSQL + "Deposit2, CarryOverPlanned, ";
                                            // Wakamatsu 20170322
                                            //SetSQL = SetSQL + "Comment, TaskStat, Note, Note2) ";
                                            SetSQL = SetSQL + "Comment, TaskStat, Note) ";
                                            // Wakamatsu 20170322
                                            // Wakamatsu 20170302
                                            SetSQL = SetSQL + "VALUES ('" + InportIF.TaskCode + "', ";
                                            SetSQL = SetSQL + "'" + InportIF.OfficeCode + "', ";
                                            if (InportIF.OfficeCode == "H")
                                            {
                                                // 本社 
                                                SetSQL = SetSQL + "'" + this.comboBoxDepartment.SelectedValue.ToString() + "', ";
                                            }
                                            else
                                            {
                                                // 本社以外
                                                // Wakamatsu
                                                //SetSQL = SetSQL + "NULL, ";
                                                SetSQL = SetSQL + "8, ";
                                                // Wakamatsu
                                            }
                                            SetSQL = SetSQL + InportIF.YearMonth + ", ";

                                            // 受注単月
                                            InportIF.ItemName = "受注単月";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + DecimalConvert(wbObject, i, P_MONTHLYVOLUMECOL + (P_ATHERDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + DecimalConvert(wbObject, i, P_LMONTHLYVOLUMECOL + (P_ATHERDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331

                                            // 未成業務出来高
                                            InportIF.ItemName = "未成業務出来高";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + DecimalConvert(wbObject, i, P_VOLUNCOMPCOL + (P_ATHERDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + DecimalConvert(wbObject, i, P_LVOLUNCOMPCOL + (P_ATHERDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331

                                            // 未請求出来高
                                            InportIF.ItemName = "未請求出来高";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + DecimalConvert(wbObject, i, P_VOLCLAIMREMCOL + (P_ATHERDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + DecimalConvert(wbObject, i, P_LVOLCLAIMREMCOL + (P_ATHERDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331

                                            // 請求出来高
                                            InportIF.ItemName = "請求出来高";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + DecimalConvert(wbObject, i, P_VOLCLAIMCOL + (P_ATHERDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + DecimalConvert(wbObject, i, P_LVOLCLAIMCOL + (P_ATHERDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331

                                            // 請求単月
                                            InportIF.ItemName = "請求単月";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + DecimalConvert(wbObject, i, P_MONTHLYCLAIMCOL + (P_ATHERDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + DecimalConvert(wbObject, i, P_LMONTHLYCLAIMCOL + (P_ATHERDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331

                                            // 入金額
                                            InportIF.ItemName = "入金額";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + DecimalConvert(wbObject, i, P_VOLPAIDCOL + (P_ATHERDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + DecimalConvert(wbObject, i, P_LVOLPAIDCOL + (P_ATHERDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331

                                            // 残請求高
                                            InportIF.ItemName = "残請求高";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + DecimalConvert(wbObject, i, P_BALANCECLAIMCOL + (P_BALANCECLAIMDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + DecimalConvert(wbObject, i, P_LBALANCECLAIMCOL + (P_BALANCECLAIMDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331

                                            // 原価単月
                                            InportIF.ItemName = "原価単月";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + DecimalConvert(wbObject, i, P_MONTHLYCOSTCOL + (P_ATHERDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + DecimalConvert(wbObject, i, P_LMONTHLYCOSTCOL + (P_ATHERDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331

                                            // 未収入金額
                                            SetSQL = SetSQL + NotIncomCal(ref InportIF) + ", ";

                                            // 請求日
                                            InportIF.ItemName = "請求日";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + DateConvert(wbObject, i, P_CLAIMDATECOL + (P_ATHERDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + DateConvert(wbObject, i, P_LCLAIMDATECOL + (P_ATHERDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331

                                            // 入金日
                                            InportIF.ItemName = "入金日";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + DateConvert(wbObject, i, P_PAINDDATECOL + (P_ATHERDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + DateConvert(wbObject, i, P_LPAINDDATECOL + (P_ATHERDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331

                                            // Wakamatsu 20170302
                                            // 未成業務受入金(請求 - 出来高)
                                            InportIF.ItemName = "未成業務受入金";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + DecimalConvert(wbObject, i, P_DEPOSITCOL + (P_BALANCECLAIMDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + DecimalConvert(wbObject, i, P_LDEPOSITCOL + (P_BALANCECLAIMDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331
                                            // 未成業務受入金(入金 - 出来高)
                                            SetSQL = SetSQL + DepositCal(ref InportIF) + ", ";
                                            // Wakamatsu 20170302

                                            SetSQL = SetSQL + InportIF.CarryOverPlanned + ", ";     // 繰越額
                                            SetSQL = SetSQL + "'', ";                               // コメント
                                            SetSQL = SetSQL + InportIF.TaskStat + ", ";             // 業務状況
                                            // Wakamatsu 20170302
                                            //SetSQL = SetSQL + "'') ";                               // 備考
                                            // Wakamatsu 20170322
                                            //SetSQL = SetSQL + "'', ";                               // 備考
                                            //SetSQL = SetSQL + "'') ";                               // 備考2
                                            SetSQL = SetSQL + "'') ";                               // 備考
                                            // Wakamatsu 20170322
                                            // Wakamatsu 20170302
                                        }
                                        else
                                        {
                                            // 修正
                                            SetSQL = SetSQL + "UPDATE D_Volume ";
                                            SetSQL = SetSQL + "SET ";

                                            // 受注単月
                                            InportIF.ItemName = "受注単月";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + "MonthlyVolume = " + DecimalConvert(wbObject, i, P_MONTHLYVOLUMECOL + (P_ATHERDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + "MonthlyVolume = " + DecimalConvert(wbObject, i, P_LMONTHLYVOLUMECOL + (P_ATHERDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331

                                            // 未成業務出来高
                                            InportIF.ItemName = "未成業務出来高";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + "VolUncomp = " + DecimalConvert(wbObject, i, P_VOLUNCOMPCOL + (P_ATHERDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + "VolUncomp = " + DecimalConvert(wbObject, i, P_LVOLUNCOMPCOL + (P_ATHERDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331

                                            // 未請求出来高
                                            InportIF.ItemName = "未請求出来高";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + "VolClaimRem = " + DecimalConvert(wbObject, i, P_VOLCLAIMREMCOL + (P_ATHERDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + "VolClaimRem = " + DecimalConvert(wbObject, i, P_LVOLCLAIMREMCOL + (P_ATHERDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331

                                            // 請求出来高
                                            InportIF.ItemName = "請求出来高";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + "VolClaim = " + DecimalConvert(wbObject, i, P_VOLCLAIMCOL + (P_ATHERDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + "VolClaim = " + DecimalConvert(wbObject, i, P_LVOLCLAIMCOL + (P_ATHERDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331

                                            // 請求単月
                                            InportIF.ItemName = "請求単月";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + "MonthlyClaim = " + DecimalConvert(wbObject, i, P_MONTHLYCLAIMCOL + (P_ATHERDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + "MonthlyClaim = " + DecimalConvert(wbObject, i, P_LMONTHLYCLAIMCOL + (P_ATHERDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331

                                            // 入金額
                                            InportIF.ItemName = "入金額";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + "VolPaid = " + DecimalConvert(wbObject, i, P_VOLPAIDCOL + (P_ATHERDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + "VolPaid = " + DecimalConvert(wbObject, i, P_LVOLPAIDCOL + (P_ATHERDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331

                                            // 残請求高
                                            InportIF.ItemName = "残請求高";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + "BalanceClaim = " + DecimalConvert(wbObject, i, P_BALANCECLAIMCOL + (P_BALANCECLAIMDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + "BalanceClaim = " + DecimalConvert(wbObject, i, P_LBALANCECLAIMCOL + (P_BALANCECLAIMDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331

                                            // 原価単月
                                            InportIF.ItemName = "原価単月";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + "MonthlyCost = " + DecimalConvert(wbObject, i, P_MONTHLYCOSTCOL + (P_ATHERDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + "MonthlyCost = " + DecimalConvert(wbObject, i, P_LMONTHLYCOSTCOL + (P_ATHERDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331

                                            // 未収入金額
                                            SetSQL = SetSQL + "BalanceIncom = " + NotIncomCal(ref InportIF) + ", ";

                                            // 請求日
                                            InportIF.ItemName = "請求日";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + "ClaimDate = " + DateConvert(wbObject, i, P_CLAIMDATECOL + (P_ATHERDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + "ClaimDate = " + DateConvert(wbObject, i, P_LCLAIMDATECOL + (P_ATHERDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331

                                            // 入金日
                                            InportIF.ItemName = "入金日";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + "PaidDate = " + DateConvert(wbObject, i, P_PAINDDATECOL + (P_ATHERDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + "PaidDate = " + DateConvert(wbObject, i, P_LPAINDDATECOL + (P_ATHERDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331

                                            // Wakamatsu 20170302
                                            // 未成業務受入金(請求 - 出来高)
                                            InportIF.ItemName = "未成業務受入金";
                                            // Wakamatsu 20170331
                                            //SetSQL = SetSQL + "Deposit1 = " + DecimalConvert(wbObject, i, P_DEPOSITCOL + (P_BALANCECLAIMDEFF * (j - 1)), ref InportIF) + ", ";
                                            SetSQL = SetSQL + "Deposit1 = " + DecimalConvert(wbObject, i, P_LDEPOSITCOL + (P_BALANCECLAIMDEFF * j), ref InportIF) + ", ";
                                            // Wakamatsu 20170331
                                            // 未成業務受入金(入金 - 出来高)
                                            SetSQL = SetSQL + "Deposit2 = " + DepositCal(ref InportIF) + ", ";
                                            // Wakamatsu 20170302

                                            SetSQL = SetSQL + "CarryOverPlanned = " + InportIF.CarryOverPlanned + ", ";     // 繰越額
                                            SetSQL = SetSQL + "TaskStat = " + InportIF.TaskStat + " ";                      // 業務状況
                                            SetSQL = SetSQL + "WHERE VolumeID = " + VolumeID;
                                        }

                                        if (InportIF.AddFlag == true)
                                        {
                                            // SQL実行
                                            InportIF.AddFlag = TableAdd(SetSQL);
                                        }
                                        if (InportIF.AddFlag == false)
                                        {
                                            // 繰り返し処理終了 
                                            // Wakamatsu
                                            //j = 3;
                                            j = 12;
                                            // Wakamatsu
                                        }
                                        // Wakamatsu
                                        //j = (j == 12 ? 1 : j + 1);
                                        j++;
                                        // Wakamatsu
                                        // Wakamatsu
                                        //} while (j != 4);
                                    } while (j < 13);
                                    // Wakamatsu
                                    // 処理件数インクリメント
                                    if (InportIF.AddFlag == true)
                                    {
                                        tran.Complete();
                                        AddCount++;
                                    }
                                }
                            }

                            // Wakamatsu 20170331
                            if (InportIF.AddFlag == true)
                            {
                                YearVolumeSave(wbObject, i, ref InportIF);
                                YearVolumeSave(ref InportIF);
                            }
                            // Wakamatsu 20170331

                            i++;
                        } while (wbObject.Worksheet("新台帳").Cell(i, P_TASKCODECOL).GetString() != "");
                    }
                }
                return InportIF.ResultMsg +
                        "　☆ " + AddCount + "件/" + (i - (P_HEADER + 2)) + "件登録しました。\r\n";
            }
            catch (Exception ex)
            {
                return ex.Message + "\r\n";
            }
        }

        /// <summary>
        /// レコード重複確認
        /// </summary>
        /// <param name="InportIF">インターフェース用構造体</param>
        /// <returns>登録済みValueID(未登録の場合-1)</returns>
        private int OverlapCheck(ref INPORTIF InportIF)
        {
            string SetSQL = "";                         // SQL格納用
            SqlHandling clsSql = new SqlHandling();
            DataTable dtObject = null;                  // テーブル格納用

            SetSQL = "";
            SetSQL = SetSQL + "VolumeID ";
            SetSQL = SetSQL + "FROM D_Volume ";
            SetSQL = SetSQL + "WHERE TaskCode = '" + InportIF.TaskCode + "' ";      // 業務コード
            SetSQL = SetSQL + "AND OfficeCode = '" + InportIF.OfficeCode + "' ";     // 事業所コード
                                                                                     // 部門コード
            if (InportIF.OfficeCode == "H")
            {
                // 本社 
                SetSQL = SetSQL + "AND Department = '" + this.comboBoxDepartment.SelectedValue.ToString() + "' ";
            }
            else
            {
                // 本社以外
                // Wakamatsu
                //SetSQL = SetSQL + "AND Department = NULL ";
                SetSQL = SetSQL + "AND Department = 8 ";
                // Wakamatsu
            }
            SetSQL = SetSQL + "AND YearMonth = '" + InportIF.YearMonth + "' ";

            dtObject = clsSql.SelectFullDescription(SetSQL);

            if (dtObject == null)
            {
                // 新規
                return -1;
            }
            else
            {
                // 修正
                return Convert.ToInt32(dtObject.Rows[0]["VolumeID"]);
            }
        }

        /// <summary>
        /// Decimal確認付き型変換(Decimal → String)※""をNULL
        /// </summary>
        /// <param name="WorkBook">対象ワークブック</param>
        /// <param name="TargetRow">対象行</param>
        /// <param name="TargetColumn">対象列</param>
        /// <param name="InportIF">インターフェース構造体</param>
        /// <returns>処理結果(エラーの場合"")</returns>
        private string DecimalConvert(XLWorkbook WorkBook, int TargetRow, int TargetColumn, ref INPORTIF InportIF)
        {
            // 処理対象文字列格納用
            // Wakamatsu
            //string TargetValue = Convert.ToString(WorkBook.Worksheet("新台帳").Cell(TargetRow,TargetColumn).GetString());
            string TargetValue = Convert.ToString(WorkBook.Worksheet("新台帳").Cell(TargetRow, TargetColumn).GetString()).Trim();
            // Wakamatsu
            decimal WorkVari = 0;       // 一時データ格納用

            if (TargetValue == "")
            {
                // 空欄の時
                InportIF.ResultValue = "NULL";
            }
            else
            {
                if (decimal.TryParse(TargetValue, out WorkVari))
                {
                    // 正常終了時
                    InportIF.ResultValue = Convert.ToString(WorkVari);

                    // 積算実行/算出フラグ確認
                    switch (InportIF.ItemName)
                    {
                        // Wakamatsu 20170302
                        //case "受注単月":
                        //    InportIF.NotIncomFlag++;
                        //    break;
                        // Wakamatsu 20170331
                        case "受注単月":
                            // Wakamatsu 20170331
                            InportIF.MonthlyVolume += WorkVari;
                            break;
                        // Wakamatsu 20170331
                        case "未成業務出来高":
                            // Wakamatsu 20170331
                            InportIF.VolUncomp += WorkVari;
                            InportIF.VolumeCum = Convert.ToDecimal(InportIF.VolumeCum ?? 0) + WorkVari;
                            break;
                        // Wakamatsu 20170331
                        case "未請求出来高":
                            // Wakamatsu 20170331
                            InportIF.VolClaimRem += WorkVari;
                            InportIF.VolumeCum = Convert.ToDecimal(InportIF.VolumeCum ?? 0) + WorkVari;
                            break;
                        // Wakamatsu 20170331
                        case "請求出来高":
                            // Wakamatsu 20170302
                            //InportIF.VolumeCum = InportIF.VolumeCum + WorkVari;
                            //InportIF.NotIncomFlag++;
                            // Wakamatsu 20170331
                            InportIF.VolClaim += WorkVari;
                            InportIF.VolumeCum = Convert.ToDecimal(InportIF.VolumeCum ?? 0) + WorkVari;
                            // Wakamatsu 20170302
                            break;
                        case "請求単月":
                            InportIF.ClaimCum = InportIF.ClaimCum + WorkVari;
                            // Wakamatsu 20170302
                            //InportIF.NotIncomFlag++;
                            break;
                        case "入金額":
                            // Wakamatsu 20170302
                            //InportIF.NotIncomFlag++;
                            InportIF.PaymentCum = InportIF.PaymentCum + WorkVari;
                            break;
                        // Wakamatsu 20170302
                        //case "原価単月":
                        //    InportIF.NotIncomFlag++;
                        //    break;
                        // Wakamatsu 20170331
                        case "原価単月":
                            InportIF.MonthlyCost += WorkVari;
                            break;
                        // Wakamatsu 20170331
                        default:
                            break;
                    }
                }
                else
                {
                    // エラー時
                    InportIF.AddFlag = false;
                    InportIF.ResultMsg = InportIF.ResultMsg + "　× 業務番号:" + InportIF.TaskCode +
                                            "　年月:" + InportIF.YearMonth + "の" +
                                            InportIF.ItemName + "が取得できませんでした。\r\n";
                    InportIF.ResultValue = "";
                }
            }
            return InportIF.ResultValue;
        }

        // Wakamatsu 20170331
        /// <summary>
        /// Decimal確認
        /// </summary>
        /// <param name="WorkBook">ワークブック</param>
        /// <param name="TargetRow">行</param>
        /// <param name="TargetColumn">列</param>
        /// <returns></returns>
        private decimal DecimalConvert(XLWorkbook WorkBook, int TargetRow, int TargetColumn)
        {
            // 処理対象文字列格納用
            string TargetValue = Convert.ToString(WorkBook.Worksheet("新台帳").Cell(TargetRow, TargetColumn).GetString()).Trim();
            decimal WorkVari = 0;       // 一時データ格納用

            decimal.TryParse(TargetValue, out WorkVari);
            return WorkVari;
        }
        // Wakamatsu 20170331

        /// <summary>
        /// 未収入金算出
        /// </summary>
        /// <param name="InportIF">インターフェース構造体</param>
        /// <returns></returns>
        private string NotIncomCal(ref INPORTIF InportIF)
        {
            decimal WorkVari = 0;           // 一時データ格納用

            // Wakamatsu 20170302
            //if (InportIF.NotIncomFlag != 0)
            //{
            // Wakamatsu 20170302
            if (InportIF.VolumeCum > InportIF.ClaimCum)
            {
                // 未収入金 = 出来高累計 - 入金累計
                // Wakamatsu 20170302
                //WorkVari = InportIF.VolumeCum - InportIF.PaymentCum;
                WorkVari = Convert.ToDecimal(InportIF.VolumeCum ?? 0) - InportIF.PaymentCum;
                // Wakamatsu 20170302
            }
            else
            {
                // 未収入金 = 請求累計 - 入金累計
                WorkVari = InportIF.ClaimCum - InportIF.PaymentCum;
            }
            // Wakamatsu 20170302
            //return (WorkVari >= 0 ? Convert.ToString(WorkVari) : "NULL");
            return (WorkVari >= 0 ? Convert.ToString(WorkVari) : "NULL");
            // Wakamatsu 20170302
            // Wakamatsu 20170302
            //}
            //else
            //{
            //    return "NULL";
            //}
            // Wakamatsu 20170302
        }

        // Wakamatsu 20170302
        /// <summary>
        /// 未成業務受入金算出
        /// </summary>
        /// <param name="InportIF">インターフェース構造体</param>
        /// <returns></returns>
        private string DepositCal(ref INPORTIF InportIF)
        {
            decimal WorkVari = 0;           // 一時データ格納用

            // 未成業務受入金 = 入金累計 - 出来高累計
            WorkVari = InportIF.PaymentCum - Convert.ToDecimal(InportIF.VolumeCum ?? 0);

            return (WorkVari > 0 ? Convert.ToString(WorkVari) : "0");
        }
        // Wakamatsu 20170302

        /// <summary>
        /// Int確認
        /// </summary>
        /// <param name="WorkBook">対象ワークブック</param>
        /// <param name="TargetRow">対象行</param>
        /// <param name="TargetColumn">対象列</param>
        /// <param name="InportIF">インターフェース構造体</param>
        /// <returns>取得値(""は-1)</returns>
        private int IntConvert(XLWorkbook WorkBook, int TargetRow, int TargetColumn, ref INPORTIF InportIF)
        {
            // 処理対象文字列格納用
            // Wakamatsu
            //string TargetValue = Convert.ToString(WorkBook.Worksheet("新台帳").Cell(TargetRow,TargetColumn).GetString());
            string TargetValue = Convert.ToString(WorkBook.Worksheet("新台帳").Cell(TargetRow, TargetColumn).GetString()).Trim();
            // Wakamatsu
            int WorkVari = -1;          // 一時データ格納用

            if (!int.TryParse(TargetValue, out WorkVari))
            {
                WorkVari = -1;
                InportIF.AddFlag = false;
                InportIF.ResultMsg = InportIF.ResultMsg + "　× 業務番号:" + InportIF.TaskCode + "の" +
                                        InportIF.ItemName + "が取得できませんでした。\r\n";
            }
            WorkVari -= 1;
            InportIF.ResultValue = Convert.ToString(WorkVari);
            return WorkVari;
        }

        /// <summary>
        /// Date確認付き型変換(Dete → String)※""をNULL
        /// </summary>
        /// <param name="WorkBook">対象ワークブック</param>
        /// <param name="TargetRow">対象行</param>
        /// <param name="TargetColumn">対象列</param>
        /// <param name="InportIF">インターフェース構造体</param>
        /// <returns>処理結果(エラーの場合"")</returns>
        private string DateConvert(XLWorkbook WorkBook, int TargetRow, int TargetColumn, ref INPORTIF InportIF)
        {
            // 処理対象文字列格納用
            // Wakamatsu
            //string TargetValue = Convert.ToString(WorkBook.Worksheet("新台帳").Cell(TargetRow,TargetColumn).GetString());
            string TargetValue = Convert.ToString(WorkBook.Worksheet("新台帳").Cell(TargetRow, TargetColumn).GetString()).Trim();
            // Wakamatsu
            DateTime WorkVari = new DateTime();         // 一時データ格納用
            // Wakamatsu
            double WorkVariS = 0;                       // 一時データ格納用
            // Wakamatsu

            if (TargetValue == "")
            {
                // 空欄の時
                InportIF.ResultValue = "NULL";
            }
            else
            {
                if (DateTime.TryParse(TargetValue, out WorkVari))
                {
                    // 正常終了時
                    InportIF.ResultValue = "'" + Convert.ToString(WorkVari.ToString("yyyy/MM/dd")) + "'";
                    // Wakamatsu 20170331
                    if (InportIF.ItemName == "請求日")
                        InportIF.ClaimDate = WorkVari;
                    else if (InportIF.ItemName == "入金日")
                        InportIF.PaymentDate = WorkVari;
                    // Wakamatsu 20170331
                }
                else
                {
                    // Wakamatsu
                    if (double.TryParse(TargetValue, out WorkVariS))
                    {
                        // 正常終了時
                        InportIF.ResultValue = "'" + Convert.ToString(DateTime.FromOADate(WorkVariS).ToString("yyyy/MM/dd")) + "'";
                        // Wakamatsu 20170331
                        if (InportIF.ItemName == "請求日")
                            InportIF.ClaimDate = DateTime.FromOADate(WorkVariS);
                        else if (InportIF.ItemName == "入金日")
                            InportIF.PaymentDate = DateTime.FromOADate(WorkVariS);
                        // Wakamatsu 20170331
                    }
                    else
                    {
                        // エラー時
                        InportIF.AddFlag = false;
                        InportIF.ResultMsg = InportIF.ResultMsg + "　× 業務番号:" + InportIF.TaskCode +
                                                "　年月:" + InportIF.YearMonth + "の" +
                                                InportIF.ItemName + "が取得できませんでした。\r\n";
                        InportIF.ResultValue = "";
                    }
                    // Wakamatsu
                }
            }
            return InportIF.ResultValue;
        }

        // Wakamatsu 20170331
        /// <summary>
        /// Date確認
        /// </summary>
        /// <param name="WorkBook">ワークブック</param>
        /// <param name="TargetRow">行</param>
        /// <param name="TargetColumn">列</param>
        /// <returns></returns>
        private DateTime? DateConvert(XLWorkbook WorkBook, int TargetRow, int TargetColumn)
        {
            // 処理対象文字列格納用
            string TargetValue = Convert.ToString(WorkBook.Worksheet("新台帳").Cell(TargetRow, TargetColumn).GetString()).Trim();
            DateTime WorkVari = new DateTime();         // 一時データ格納用
            double WorkVariS = 0;                       // 一時データ格納用

            if (DateTime.TryParse(TargetValue, out WorkVari))
                // 正常終了時
                return WorkVari;
            else
            {
                if (double.TryParse(TargetValue, out WorkVariS))
                    return DateTime.FromOADate(WorkVariS);
                else
                    return null;
            }
        }
        // Wakamatsu 20170331

        /// <summary>
        /// SQL実行
        /// </summary>
        /// <param name="SetSQL">実行SQL</param>
        /// <returns>処理結果</returns>
        private bool TableAdd(string SetSQL)
        {
            DbAccess clsDbAccess = new DbAccess();              // DbAccessインスタンス化
            bool ActResult = true;                              // 処理結果格納用

            using (SqlConnection conn = new SqlConnection(clsDbAccess.ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SetSQL, conn);
                    // SQL実行
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (SqlException sqle)
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    ActResult = false;
                }
                return ActResult;
            }
        }

        // Wakamatsu 20170331
        /// <summary>
        /// ファイル内前年度データ格納
        /// </summary>
        /// <param name="WorkBook">ワークブック</param>
        /// <param name="TargetRow">行</param>
        /// <param name="InportIF">インターフェース構造体</param>
        /// <returns></returns>
        private bool YearVolumeSave(XLWorkbook WorkBook, int TargetRow, ref INPORTIF InportIF)
        {
            decimal? WorkDecimal = null;

            VolumeData volume = new VolumeData();

            //業務番号
            volume.TaskCode = InportIF.TaskCode;

            //年度
            volume.YearMonth = Convert.ToInt32(this.textBoxYear.Text) - 1;

            //受注累計
            volume.MonthlyVolume = DecimalConvert(WorkBook, TargetRow, P_LMONTHLYVOLUMECOL);

            //出来高未成業務累計
            volume.VolUncomp = DecimalConvert(WorkBook, TargetRow, P_LVOLUNCOMPCOL);

            //出来高未請求
            volume.VolClaimRem = DecimalConvert(WorkBook, TargetRow, P_LVOLCLAIMREMCOL);

            //出来高請求
            volume.VolClaim = DecimalConvert(WorkBook, TargetRow, P_LVOLCLAIMCOL);

            //請求累計
            volume.MonthlyClaim = DecimalConvert(WorkBook, TargetRow, P_LMONTHLYCLAIMCOL);

            //請求日
            volume.ClaimDate = DateConvert(WorkBook, TargetRow, P_LCLAIMDATECOL);

            //入金累計
            volume.VolPaid = DecimalConvert(WorkBook, TargetRow, P_LVOLPAIDCOL);

            //入金日
            volume.PaidDate = DateConvert(WorkBook, TargetRow, P_LPAINDDATECOL);

            //残請求高金額
            volume.BalanceClaim = DecimalConvert(WorkBook, TargetRow, P_LBALANCECLAIMCOL);

            // 未成業務受入金(請求 - 出来高)
            volume.Deposit1 = DecimalConvert(WorkBook, TargetRow, P_LDEPOSITCOL);

            //原価累計
            volume.MonthlyCost = DecimalConvert(WorkBook, TargetRow, P_LMONTHLYCOSTCOL);

            //未収入金額
            WorkDecimal = volume.VolUncomp + volume.VolClaimRem + volume.VolClaim - volume.VolPaid;
            if (WorkDecimal > 0)
                volume.BalanceIncom = WorkDecimal;

            // 未成業務受入金(入金 - 出来高)
            WorkDecimal = volume.VolPaid - (volume.VolUncomp + volume.VolClaimRem + volume.VolClaim);
            if (WorkDecimal > 0)
                volume.Deposit2 = WorkDecimal;

            //事業所コード
            volume.OfficeCode = comboBoxOfficeCode.SelectedValue.ToString();

            //部門コード
            volume.Department = (volume.OfficeCode == "H") ? comboBoxDepartment.SelectedValue.ToString() : "8";

            if (volume.ExistenceTaskCodeYearMonth("D_YearVolume"))
                //更新
                return volume.UpdateYearVolume(volume);
            else
                //追加
                return volume.InsertYearVolume(volume);
        }

        /// <summary>
        /// 次年度用前年度データ格納処理
        /// </summary>
        /// <param name="InportIF">インターフェース構造体</param>
        /// <returns></returns>
        private bool YearVolumeSave(ref INPORTIF InportIF)
        {
            decimal? WorkDecimal = null;

            VolumeData volume = new VolumeData();

            //業務番号
            volume.TaskCode = InportIF.TaskCode;

            //年度
            volume.YearMonth = Convert.ToInt32(this.textBoxYear.Text);

            //受注累計
            volume.MonthlyVolume = InportIF.MonthlyVolume;

            //出来高未成業務累計
            volume.VolUncomp = InportIF.VolUncomp;

            //出来高未請求
            volume.VolClaimRem = InportIF.VolClaimRem;

            //出来高請求
            volume.VolClaim = InportIF.VolClaim;

            //請求累計
            volume.MonthlyClaim = InportIF.ClaimCum;

            //請求日
            volume.ClaimDate = InportIF.ClaimDate;

            //入金累計
            volume.VolPaid = InportIF.PaymentCum;

            //入金日
            volume.PaidDate = InportIF.PaymentDate;

            //残請求高金額
            WorkDecimal = volume.VolUncomp + volume.VolClaimRem + volume.VolClaim - volume.MonthlyClaim;
            if (WorkDecimal > 0)
                volume.BalanceIncom = WorkDecimal;

            // 未成業務受入金(請求 - 出来高)
            WorkDecimal = volume.MonthlyClaim - (volume.VolUncomp + volume.VolClaimRem + volume.VolClaim);
            if (WorkDecimal > 0)
                volume.BalanceIncom = WorkDecimal;

            //原価累計
            volume.MonthlyCost = InportIF.MonthlyCost;

            //未収入金額
            WorkDecimal = volume.VolUncomp + volume.VolClaimRem + volume.VolClaim - volume.VolPaid;
            if (WorkDecimal > 0)
                volume.BalanceIncom = WorkDecimal;

            // 未成業務受入金(入金 - 出来高)
            WorkDecimal = volume.VolPaid - (volume.VolUncomp + volume.VolClaimRem + volume.VolClaim);
            if (WorkDecimal > 0)
                volume.Deposit2 = WorkDecimal;

            //事業所コード
            volume.OfficeCode = comboBoxOfficeCode.SelectedValue.ToString();

            //部門コード
            volume.Department = (volume.OfficeCode == "H") ? comboBoxDepartment.SelectedValue.ToString() : "8";

            if (volume.ExistenceTaskCodeYearMonth("D_YearVolume"))
                //更新
                return volume.UpdateYearVolume(volume);
            else
                //追加
                return volume.InsertYearVolume(volume);
        }
        // Wakamatsu 20170331
    }
}
