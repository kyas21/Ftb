using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClassLibrary;
using Microsoft.Office.Interop.Excel;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace PrintOut
{
    public partial class FormPdfOut : Form
    {
        private ComboBox l_TaskCode;            // 業務番号設定用
        /// <summary>
        /// 業務番号設定用コンボボックス 
        /// </summary>
        public ComboBox TaskCode
        {
            get { return l_TaskCode; }
            set { l_TaskCode = value; }
        }

        private ComboBox l_OfficeCode;          // 部署設定用
        /// <summary>
        /// 部署設定用コンボボックス
        /// </summary>
        public ComboBox OfficeCode
        {
            get { return l_OfficeCode; }
            set { l_OfficeCode = value; }
        }

        private ComboBox l_Department;          // 部門設定用
        /// <summary>
        /// 部門設定用コンボボックス
        /// </summary>
        public ComboBox Department
        {
            get { return l_Department; }
            set { l_Department = value; }
        }

        private DataGridView l_SetGridVew;          // 出力データ設定用(選択業務番号)
        /// <summary>
        /// 業務番号指定時出力データ設定用データグリッドビュー
        /// </summary>
        public DataGridView SetGridVew
        {
            get { return l_SetGridVew; }
            set { l_SetGridVew = value; }
        }

        private ComboBox l_TaskState;               // 業務形態設定用
        /// <summary>
        /// 業務形態設定用コンボボックス
        /// </summary>
        public ComboBox TaskState
        {
            get { return l_TaskState; }
            set { l_TaskState = value; }
        }

        private string l_SetYear;           // 年設定用
        /// <summary>
        /// 年設定用
        /// </summary>
        public string SetYear               // 年設定用
        {
            get { return l_SetYear; }
            set { l_SetYear = value; }
        }

        private string l_SetCarryOver;      // 繰越予定額設定用
        /// <summary>
        /// 繰越予定額設定用
        /// </summary>
        public string SetCarryOver
        {
            get { return l_SetCarryOver; }
            set { l_SetCarryOver = value; }
        }

        // Wakamatsu
        //private string l_SetCompletionHigh;     // 年度内完工高設定用
        ///// <summary>
        ///// 年度内完工高設定用
        ///// </summary>
        //public string SetCompletionHigh
        //{
        //    get { return l_SetCompletionHigh; }
        //    set { l_SetCompletionHigh = value; }
        //}
        // Wakamatsu

        private string l_SetNote;           // 備考設定用
        /// <summary>
        /// 備考設定用
        /// </summary>
        public string SetNote
        {
            get { return l_SetNote; }
            set { l_SetNote = value; }
        }

        // Wakamatsu
        // Wakamatsu 20170322
        //private string l_SetNote2;           // 備考2設定用
        ///// <summary>
        ///// 備考2設定用
        ///// </summary>
        //public string SetNote2
        //{
        //    get { return l_SetNote2; }
        //    set { l_SetNote2 = value; }
        //}
        // Wakamatsu 20170322

        private int l_ClosingDate;          // 最終締め月設定用
        /// <summary>
        /// 最終締め月設定用
        /// </summary>
        public int ClosingDate
        {
            get { return l_ClosingDate; }
            set { l_ClosingDate = value; }
        }
        // Wakamatsu

        VolumeData[] volumedata;            // ボリュームデータ格納用
        CostReportData[] crdM7;             // 7月用
        CostReportData[] crdM8;             // 8月用
        CostReportData[] crdM9;             // 9月用
        CostReportData[] crdM10;　　　　　　// 10月用
        CostReportData[] crdM11;        　　// 11月用
        CostReportData[] crdM12;            // 12月用
        CostReportData[] crdM1;             // 1月用
        CostReportData[] crdM2;             // 2月用
        CostReportData[] crdM3;             // 3月用
        CostReportData[] crdM4;             // 4月用
        CostReportData[] crdM5;             // 5月用
        CostReportData[] crdM6;             // 6月用

        private decimal[] totalCumulativeAry = new decimal[13];    //受注累計
        private decimal[] totalTradingVolumeAry = new decimal[13]; //出来高 累計
        private decimal[] totalCumulativeMAry = new decimal[13];   //請求 累計
        private decimal[] totalCumulativeVAry = new decimal[13];   //入金 累計
        private decimal[] totalCumulativeMCAry = new decimal[13];  //原価 累計
        private int iniRCnt = 22;

        // Wakamatsu
        //private string l_GetCarryOver;          // 繰り越し予定額設定用
        //private string l_GetCompletionHigh;     // 年度内完工高設定用
        //private string l_GetNote;               // 備考設定用
        //private string l_GetYearCompHigh;       // 
        private string l_GetCarryOver = "";             // 繰り越し予定額設定用
        private string l_GetCompletionHigh = "";        // 年度内完工高設定用
        private string l_GetNote = "";                  // 備考設定用
        private string l_GetYearCompHigh = "";          // 
        // Wakamatsu
        // Wakamatsu
        // Wakamatsu 20170322
        //private string l_GetNote2;              // 備考2設定用
        private int[] monthArray = new int[] { 0, 7, 8, 9, 10, 11, 12, 1, 2, 3, 4, 5, 6 };
        // Wakamatsu

        public FormPdfOut()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームロード時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormPdfOut_Load(object sender, EventArgs e)
        {
            try
            {
                // 初期設定
                this.dgvOutPut.Visible = false;

                // オプションボタン制御
                if (l_TaskCode.Text == "")
                {
                    // 前画面で"管理番号"が設定されていない場合
                    this.optSelect.Enabled = false;
                    this.optSelect.Checked = false;
                    this.optAll.Checked = true;
                    this.optRange.Checked = false;
                }
                else
                {
                    // 前画面で"管理番号"が設定されていた場合
                    this.optSelect.Text = l_TaskCode.Text + "のみ";
                    this.optSelect.Checked = true;
                    this.optAll.Checked = false;
                    this.optRange.Checked = false;
                }

                for (int i = 0; i < l_TaskCode.Items.Count; i++)
                {
                    // コンボボックスに前画面の業務番号を設定する
                    this.cmbStart.Items.Add((((DataRowView)l_TaskCode.Items[i]).Row[1].ToString()));
                    this.cmbEnd.Items.Add((((DataRowView)l_TaskCode.Items[i]).Row[1].ToString()));
                }

                // コンボボックス制御
                ComboBoxSetting(false);

                // データグリッドビューの初期設定
                this.dgvOutPut.Rows.Add(iniRCnt);
                this.dgvOutPut.AllowUserToAddRows = false;

                // 処理中ラベル表示
                this.lblMessage.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// コンボボックス制御
        /// </summary>
        /// <param name="bolCheckState">設定ステート</param>
        private void ComboBoxSetting(bool bolCheckState)
        {
            try
            {
                // コンボボックスイネーブル制御
                this.cmbStart.Text = "";
                this.cmbStart.Enabled = bolCheckState;
                this.cmbEnd.Text = "";
                this.cmbEnd.Enabled = bolCheckState;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 業務番号選択オプションボタン変更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optSelect_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.optSelect.Checked == true)          // 業務番号選択オプションボタンチェック時
                {
                    // コンボボックス制御
                    ComboBoxSetting(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// すべてオプションボタン変更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.optAll.Checked == true)            // すべてオプションボタンチェック時
                {
                    // コンボボックス制御
                    ComboBoxSetting(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 業務番号範囲指定オプションボタン変更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optRange_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.optRange.Checked == true)          // 業務番号範囲指定オプションボタンチェック時
                {
                    // コンボボックス制御
                    ComboBoxSetting(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 印刷ボタン押下時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            // 2018.01 asakawa
            //   作業用の一時ファイルの作成場所を、デスクトップ上の「出来高台帳帳票」フォルダに変更。
            // asakawa //

            int StartIndex = 0;                             // 開始業務番号インデックス
            int EndIndex = 0;                               // 終了業務番号インデックス
            string FilePath = Folder.DefaultLocation();     // ファイル保存パス

            // 2018.01 asakawa
            FilePath += @"\出来高台帳帳票";
            if (!System.IO.Directory.Exists(FilePath))
            {
                // Func<DialogResult> dialogCreateFolder = DMessage.DialogCreateFolder;
                // if (dialogCreateFolder() == DialogResult.No) return;
                System.IO.Directory.CreateDirectory(FilePath);
                MessageBox.Show("「出来高台帳帳票」フォルダを作成しました。");
            }
            // 2018.01 asakawa //

            if (this.optSelect.Checked == true)             // 業務番号選択時
            {
                if (this.cmbStart.Items.Contains(l_TaskCode.Text))
                {
                    // 開始番号 = 終了番号
                    StartIndex = this.cmbStart.Items.IndexOf(l_TaskCode.Text);
                    EndIndex = StartIndex;
                }
            }
            else if (this.optAll.Checked == true)           // すべて選択時
            {
                // 開始番号 = コンボボックスの先頭 / 終了番号 = コンボボックスの終端
                StartIndex = 0;
                EndIndex = this.cmbStart.Items.Count - 1;
            }
            else if (this.optRange.Checked == true)         // 業務番号範囲選択時
            {
                // 業務番号範囲指定出力時コンボボックス確認
                if (!this.cmbStart.Items.Contains(this.cmbStart.Text))
                {
                    MessageBox.Show("印刷を行う業務番号の範囲を選択してください");
                    return;
                }
                if (!this.cmbEnd.Items.Contains(this.cmbEnd.Text))
                {
                    MessageBox.Show("印刷を行う業務番号の範囲を選択してください");
                    return;
                }

                if (this.cmbStart.SelectedIndex <= this.cmbEnd.SelectedIndex)
                {
                    // 開始番号 = 開始コンボボックス / 終了番号 = 終了コンボボックス
                    StartIndex = this.cmbStart.Items.IndexOf(this.cmbStart.Text);
                    EndIndex = this.cmbEnd.Items.IndexOf(this.cmbEnd.Text);
                }
                else
                {
                    // 開始番号 = 終了コンボボックス / 終了番号 = 開始コンボボックス
                    StartIndex = this.cmbEnd.Items.IndexOf(this.cmbEnd.Text);
                    EndIndex = this.cmbStart.Items.IndexOf(this.cmbStart.Text);
                }
            }

            this.lblMessage.Visible = true;
            // Wakamatsu
            this.Refresh();
            Cursor.Current = Cursors.WaitCursor;

            string officeCode = Convert.ToString(l_OfficeCode.SelectedValue);
            string department = (officeCode == "H") ? Convert.ToString(l_Department.SelectedValue) : "";

            // print 処理
            // Wakamatsu 20170301
            //PublishVolume publ = new PublishVolume(Folder.DefaultExcelTemplate("Volume.xlsx"));
            PublishVolume publ = new PublishVolume(Folder.DefaultExcelTemplate("出来高台帳.xlsx"));
            // Wakamatsu 20170301

            // 設定された開始から終了まで繰り返す
            for (int i = StartIndex; i <= EndIndex; i++)
            {
                PublishData pd;

                if (this.cmbStart.Items[i].ToString() == l_TaskCode.Text)
                {
                    // Wakamatsu
                    // 締め月以降マスク処理
                    ClosingDateMask();
                    // Wakamatsu
                    // 前画面で表示されていた業務番号
                    pd = SetTaskInfContents(l_TaskCode.Text, officeCode, department);
                    // Wakamatsu
                    //publ.CreateExcelForPdf("Volume",pd,l_SetGridVew);
                    // Wakamatsu
                }
                else
                {
                    // 前画面で表示されていなかった業務番号
                    pd = ScreenDisplay(this.cmbStart.Items[i].ToString(), officeCode, department);
                    // Wakamatsu
                    //publ.CreateExcelForPdf("Volume",pd,this.dgvOutPut);
                    // Wakamatsu
                }
                // Wakamatsu
                publ.CreateExcelForPdf("Volume", pd, this.dgvOutPut);
                // Wakamatsu

                string sourcePath = FilePath + @"\" + pd.vTaskCode + ".xlsx";
                // Wakamatsu
                //string targetPath = sourcePath.Replace(".xlsx", ".pdf");
                //XlFixedFormatType format = XlFixedFormatType.xlTypePDF;
                //XlFixedFormatQuality quality = XlFixedFormatQuality.xlQualityStandard;
                //Microsoft.Office.Interop.Excel.Application app = null;
                //Workbook workbook = null;
                //try
                //{
                //    app = new Microsoft.Office.Interop.Excel.Application();
                //    workbook = app.Workbooks.Open(sourcePath);                  //---ブックを開いて
                //    workbook.ExportAsFixedFormat(format, targetPath, quality);  //--- PDF形式で出力
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}
                //finally
                //{
                //    if (workbook != null)
                //    {
                //        System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                //        workbook = null;
                //    }

                //    app.Quit();
                //    System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                //    app = null;

                //    if (sourcePath != "")
                //    {
                //        if (File.Exists(sourcePath))
                //            File.Delete(sourcePath);
                //    }
                //}
                PublishExcelToPdf pdfMake = new PublishExcelToPdf();
                if (pdfMake.ExcelToPDF(sourcePath) == false)
                {
                    // 処理途中PDFファイル削除
                    for (int j = StartIndex; j <= EndIndex; j++)
                        if (File.Exists(FilePath + @"\" + this.cmbStart.Items[j].ToString() + ".pdf"))
                            // 対象Excelファイル削除
                            File.Delete(FilePath + @"\" + this.cmbStart.Items[j].ToString() + ".pdf");
                    MessageBox.Show("PDF印刷が完了できませんでした。");
                    return;
                }
                // Wakamatsu
            }

            // PDFファイル結合
            if (StartIndex != EndIndex)                 // 開始業務番号と終了業務番号が異なる場合結合を行う
            {
                PdfReader ReaderObject = null;          // リーダーオブジェクト
                Document DocumentObject = null;         // ドキュメントオブジェクト
                PdfCopy WriterObject = null;            // ライターオブジェクト

                try
                {
                    // リーダーの作成
                    ReaderObject = new PdfReader(FilePath + @"\" + this.cmbStart.Items[StartIndex].ToString() + ".pdf");
                    // Document作成
                    DocumentObject = new Document(ReaderObject.GetPageSizeWithRotation(1));
                    // 出力ファイルPdfCopy作成
                    WriterObject = new PdfCopy(DocumentObject, new FileStream(FilePath + @"\" +
                                                             this.cmbStart.Items[StartIndex].ToString() + "-" +
                                                             this.cmbStart.Items[EndIndex].ToString() + ".pdf", FileMode.Create));
                    // 出力ファイルDocumentを開く
                    DocumentObject.Open();
                    // 文書プロパティ設定
                    foreach (string s in ReaderObject.Info.Keys)
                    {
                        if (s == "Keywords") DocumentObject.AddKeywords((string)ReaderObject.Info["Keywords"]);
                        else if (s == "Author") DocumentObject.AddAuthor((string)ReaderObject.Info["Author"]);
                        else if (s == "Title") DocumentObject.AddTitle((string)ReaderObject.Info["Title"]);
                        else if (s == "Creator") DocumentObject.AddCreator((string)ReaderObject.Info["Creator"]);
                        else if (s == "Subject") DocumentObject.AddSubject((string)ReaderObject.Info["Subject"]);
                    }
                    // リーダーを閉じる
                    ReaderObject.Close();

                    // ファイル結合
                    for (int i = StartIndex; i <= EndIndex; i++)
                    {
                        // リーダーの作成
                        ReaderObject = new PdfReader(FilePath + @"\" + this.cmbStart.Items[i].ToString() + ".pdf");
                        // PDFのページを、copyに追加
                        for (int pn = 1; pn <= ReaderObject.NumberOfPages; pn++)
                        {
                            PdfImportedPage page = WriterObject.GetImportedPage(ReaderObject, pn);
                            WriterObject.AddPage(page);
                        }
                        // リーダーを閉じる
                        ReaderObject.Close();
                        // PDFファイル削除
                        File.Delete(FilePath + @"\" + this.cmbStart.Items[i].ToString() + ".pdf");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (WriterObject != null) WriterObject.Close();
                    if (DocumentObject != null) DocumentObject.Close();
                }
            }

            Cursor.Current = Cursors.Default;
            this.lblMessage.Visible = false;

            MessageBox.Show("PDF印刷が完了しました。");
        }

        /// <summary>
        /// 印刷設定用データ作成
        /// </summary>
        /// <param name="taskCode">業務番号</param>
        /// <param name="officeCode">部署</param>
        /// <param name="department">部門</param>
        /// <returns></returns>
        private PublishData SetTaskInfContents(string taskCode, string officeCode, string department)
        {
            PublishData pd = new PublishData();

            if (taskCode.Trim() == "") return pd;

            SqlHandling sh = new SqlHandling();
            string departmentSql = "";
            if (department != "")
                departmentSql = " AND D_V.Department = " + "'" + department + "'";

            // 2019.01.21 asakawa
            // 2019.01.29 asakawa 再度修正
            // string sqlStr = "DISTINCT D_T.TaskName AS TaskName, D_T.StartDate AS StartDate, D_T.EndDate AS EndDate, D_T.OrdersForm AS OrdersForm, D_T.TaskLeader AS TaskLeader, D_T.ClaimForm AS ClaimForm, D_T.PayNote AS PayNote, D_TI.TaskCode AS TaskCode, M_P.PartnerName AS PartnerName, ISNULL(D_V.TaskStat, 0) AS TaskStat FROM D_Task D_T "
            string sqlStr = "DISTINCT D_T.TaskName AS TaskName, D_T.StartDate AS StartDate, D_T.EndDate AS EndDate, D_T.OrdersForm AS OrdersForm, D_T.SalesMCode AS SalesMCode, D_TI.LeaderMCode AS LeaderMCode, D_T.ClaimForm AS ClaimForm, D_T.PayNote AS PayNote, D_TI.TaskCode AS TaskCode, M_P.PartnerName AS PartnerName, ISNULL(D_V.TaskStat, 0) AS TaskStat FROM D_Task D_T "
                    + " INNER JOIN D_TaskInd D_TI ON D_T.TaskID = D_TI.TaskID "
                    + " LEFT JOIN M_Partners M_P ON D_T.PartnerCode = M_P.PartnerCode "
                    + " LEFT JOIN D_Volume D_V ON D_TI.TaskCode = D_V.TaskCode "
                    + " AND D_TI.OfficeCode = D_V.OfficeCode AND D_V.YearMonth = " + "'" + l_SetYear + "07" + "'"
                    + departmentSql
                    + " WHERE D_TI.TaskCode = " + "'" + taskCode + "'"
                    + " AND D_TI.OfficeCode =" + "'" + officeCode + "'";

            System.Data.DataTable dt = sh.SelectFullDescription(sqlStr);

            if ((dt != null) && (dt.Rows.Count > 0))
            {
                DataRow dr = dt.Rows[0];

                //業務名
                if (dr["TaskName"] != null && dr["TaskName"] != DBNull.Value)
                    pd.vTaskName = Convert.ToString(dr["TaskName"]);
                //取引先名
                if (dr["PartnerName"] != null && dr["PartnerName"] != DBNull.Value)
                    pd.vSupplierName = Convert.ToString(dr["PartnerName"]);
                //工期開始日
                if (dr["StartDate"] != null && dr["StartDate"] != DBNull.Value)
                {
                    DateTime dateStartDate = Convert.ToDateTime(dr["StartDate"]);
                    pd.vStartDate = dateStartDate.ToString("yyyy/MM/dd");
                }
                //工期終了日
                if (dr["EndDate"] != null && dr["EndDate"] != DBNull.Value)
                {
                    DateTime dateEndDate = Convert.ToDateTime(dr["EndDate"]);
                    pd.vEndDate = dateEndDate.ToString("yyyy/MM/dd");
                }
                //受注形態
                if (dr["OrdersForm"] != null && dr["OrdersForm"] != DBNull.Value)
                {
                    pd.vOrdersForm = (Convert.ToInt32(dr["OrdersForm"]) == 0) ? "請負" : "常傭";
                }
                //請求形態
                if (dr["ClaimForm"] != null && dr["ClaimForm"] != DBNull.Value)
                {
                    pd.vClaimform = (Convert.ToInt32(dr["ClaimForm"]) == 0) ? "月次" : "完成";
                }

                //担当者
                // 2019.01.21 asakawa
                // 2019.01.29 asakawa 再度修正
                // if (dr["TaskLeader"] != null && dr["TaskLeader"] != DBNull.Value)
                //     pd.vContact = Convert.ToString(dr["TaskLeader"]);
                MembersData md = new MembersData();
                MembersData md2 = new MembersData();
                pd.vContact = "";
                if (dr["SalesMCode"] != null && dr["SalesMCode"] != DBNull.Value)
                    pd.vContact += md.SelectMemberName(Convert.ToString(dr["SalesMCode"]));
                pd.vContact += ":";
                if (dr["LeaderMCode"] != null && dr["LeaderMCode"] != DBNull.Value)
                    pd.vContact += md2.SelectMemberName(Convert.ToString(dr["LeaderMCode"]));

                //支払条件
                if (dr["PayNote"] != null && dr["PayNote"] != DBNull.Value)
                    pd.vPayNote = Convert.ToString(dr["PayNote"]);

                //業務状態
                if (dr["TaskStat"] != null && dr["TaskStat"] != DBNull.Value)
                {
                    pd.vTaskStat = (((DataRowView)l_TaskState.Items[Convert.ToInt32(dr["TaskStat"])]).Row[1].ToString());
                }

                // 業務番号
                pd.vTaskCode = taskCode;

                //繰越予定額
                pd.vCarryOverPlanned = l_SetCarryOver;

                //年内完工高
                // Wakamatsu
                //pd.vYearCompletionHigh = l_SetCompletionHigh;

                // 20190307 asakawa
                // その１　1行削除、追加 
                //pd.vYearCompletionHigh = l_GetCompletionHigh;
                if (l_SetCarryOver.Trim() == "")
                    pd.vYearCompletionHigh = "";
                else
                    pd.vYearCompletionHigh = l_GetYearCompHigh;
                // 20190307 asakawa end

                // Wakamatsu

                //年
                pd.vYear = l_SetYear + "年度" + "－" + l_OfficeCode.Text;
                if (l_Department.Visible)
                    pd.vYear = pd.vYear + "－" + l_Department.Text;

                // 備考
                pd.vNote = l_SetNote;
                // Wakamatsu
                // 備考2
                // Wakamatsu 20170322
                //pd.vNote2 = l_SetNote2;

            }

            return pd;
        }

        /// <summary>
        /// データグリッドビューに表示する
        /// </summary>
        /// <param name="taskCode">業務番号</param>
        /// <param name="officeCode">部署</param>
        /// <param name="department">部門</param>
        /// <returns></returns>
        private PublishData ScreenDisplay(string taskCode, string officeCode, string department)
        {
            initGridData();
            l_GetCarryOver = "";
            l_GetCompletionHigh = "";
            l_GetNote = "";
            l_GetYearCompHigh = "";
            // Wakamatsu
            // Wakamatsu 20170322
            //l_GetNote2 = "";

            for (int i = 0; i <= 12; i++)
            {
                ClrVolumeInf(i);
            }

            //業務テーブル関連
            PublishData pd = SetTaskInfContents(taskCode, officeCode, department);

            //出来高データ取得（前年度）
            dispPreYearVolumeData(taskCode, Convert.ToInt32(l_SetYear), Convert.ToInt32(l_SetYear) - 1,
                                    officeCode, department);

            //出来高データ取得（今年度）
            // Wakamatsu
            int curYear = Convert.ToInt32(l_SetYear);
            int curMonth;
            int DisplyLimit = Array.IndexOf(monthArray, l_ClosingDate) - 1;
            // Wakamatsu
            for (int i = 0; i < 12; i++)
            {
                // Wakamatsu
                curMonth = i + 7;
                if (curMonth > 12) curMonth = curMonth - 12;
                if (curMonth == 1) curYear++;
                //int month = i + 7;
                //if ( month > 12 )
                //{
                //    month = month - 12;
                //    //yearMonth = nextYear.ToString();
                //}
                //dispVolumeData( month, taskCode, Convert.ToInt32( l_SetYear ) * 100 + month,
                //                officeCode, department, i );
                if (i <= DisplyLimit)
                    dispVolumeData(curMonth, taskCode, curYear * 100 + curMonth, officeCode, department, i);
                // Wakamatsu
            }
            //原価
            SetOriginalCost(taskCode, officeCode);

            //自動計算処理
            AutoCalc(this.dgvOutPut);

            //繰越予定額
            pd.vCarryOverPlanned = l_GetCarryOver;
            //年内完工高

            // 20190307 asakawa
            // その２ 
            //pd.vYearCompletionHigh = l_GetCompletionHigh;
            if (l_SetCarryOver.Trim() == "")
                pd.vYearCompletionHigh = "";
            else
                pd.vYearCompletionHigh = l_GetYearCompHigh;
            // 20190307 asakawa end

            // 備考
            pd.vNote = l_GetNote;
            // Wakamatsu
            // 備考2
            // Wakamatsu 20170322
            //pd.vNote2 = l_GetNote2;

            return pd;
        }

        private void initGridData()
        {
            for (int i = 0; i < 13; i++)
            {
                totalCumulativeAry[i] = 0;
                totalTradingVolumeAry[i] = 0;
                totalCumulativeMAry[i] = 0;
                totalCumulativeVAry[i] = 0;
                totalCumulativeMCAry[i] = 0;
            }
        }

        /// <summary>
        /// データグリッドビューのオールクリア
        /// </summary>
        /// <param name="i">列番号</param>
        private void ClrVolumeInf(int i)
        {
            foreach (var row in this.dgvOutPut.Rows.Cast<DataGridViewRow>())
            {
                row.Cells[i].Value = "";
            }
        }

        /// <summary>
        /// 前年度データ取得
        /// </summary>
        /// <param name="taskCode">業務番号</param>
        /// <param name="yearData">今年度</param>
        /// <param name="preYearData">前年度</param>
        /// <param name="officeCode">部署</param>
        /// <param name="department">部門</param>
        /// <returns>処理結果</returns>
        private bool dispPreYearVolumeData(string taskCode, int yearData, int preYearData, string officeCode, string department)
        {
            VolumeData vd = new VolumeData();

            //string sqlDepartment = "";
            //if (department != "") sqlDepartment = " AND D_V.Department =" + "'" + department + "'";
            //string strSql = " NULL AS TaskCode, NULL AS YearMonth, NULL AS Publisher, NULL AS ClaimDate, NULL AS PaidDate, NULL AS Comment, NULL AS TaskStat, NULL AS CarryOverPlanned, NULL AS OfficeCode, NULL AS Department, NULL AS Note, "
            //+ " SUM(D_V.MonthlyVolume) AS MonthlyVolume, SUM(D_V.VolUncomp) AS VolUncomp, SUM(D_V.VolClaimRem) AS VolClaimRem, SUM(D_V.VolClaim) AS VolClaim, SUM(D_V.VolPaid) AS VolPaid, SUM(D_V.MonthlyClaim) AS MonthlyClaim, SUM(D_V.MonthlyCost) AS MonthlyCost, SUM(D_V.BalanceClaim) AS BalanceClaim, SUM(D_V.BalanceIncom) AS BalanceIncom FROM D_Volume AS D_V"
            //+ " WHERE D_V.TaskCode = " + "'" + taskCode + "'"
            //+ " AND D_V.OfficeCode =" + "'" + officeCode + "'"
            //+ sqlDepartment
            //+ " AND ( D_V.YearMonth BETWEEN " + "'" + preYearData + "07'" + " AND " + "'" + yearData + "06' ) "
            //+ " AND ISNULL(D_V.TaskStat, 0) < 3";//業務状態が完全完了のものはカウントしない。
            //volumedata = vd.SelectVolumeData(strSql);
            volumedata = vd.SelectVolumeData(officeCode, department, taskCode, yearData, preYearData);

            if (volumedata == null) return false;

            loadPreYearVolumeData(volumedata, this.dgvOutPut);
            return true;
        }

        /// <summary>
        /// データグリッドビューに前年度のデータを格納する
        /// </summary>
        /// <param name="volumedata">前年度データ</param>
        /// <param name="dgv">格納対象データグリッドビュー</param>
        private void loadPreYearVolumeData(VolumeData[] volumedata, DataGridView dgv)
        {
            if (volumedata.Count() < 1) return;
            dgv.Rows[0].Cells["LY"].Value = "";
            dgv.Rows[2].Cells["LY"].Value = "";
            dgv.Rows[3].Cells["LY"].Value = "";
            dgv.Rows[4].Cells["LY"].Value = "";
            dgv.Rows[8].Cells["LY"].Value = "";
            dgv.Rows[10].Cells["LY"].Value = "";
            dgv.Rows[11].Cells["LY"].Value = "";
            dgv.Rows[13].Cells["LY"].Value = "";
            dgv.Rows[16].Cells["LY"].Value = "";
            dgv.Rows[21].Cells["LY"].Value = "";

            // Wakamatsu 20170331
            //if (volumedata[0].MonthlyVolume != null)
            if (volumedata[0].MonthlyVolume != null && volumedata[0].MonthlyVolume != 0)
                dgv.Rows[0].Cells["LY"].Value = decFormat(Convert.ToDecimal(volumedata[0].MonthlyVolume));//受注単月
            // Wakamatsu 20170331
            //if (volumedata[0].VolUncomp != null)
            if (volumedata[0].VolUncomp != null && volumedata[0].VolUncomp != 0)
                dgv.Rows[2].Cells["LY"].Value = decFormat(Convert.ToDecimal(volumedata[0].VolUncomp));    //出来高未成業務
            // Wakamatsu 20170331
            //if (volumedata[0].VolClaimRem != null)
            if (volumedata[0].VolClaimRem != null && volumedata[0].VolClaimRem != 0)
                dgv.Rows[3].Cells["LY"].Value = decFormat(Convert.ToDecimal(volumedata[0].VolClaimRem));  //出来高未請求
            // Wakamatsu 20170331
            //if (volumedata[0].VolClaim != null)
            if (volumedata[0].VolClaim != null && volumedata[0].VolClaim != 0)
                dgv.Rows[4].Cells["LY"].Value = decFormat(Convert.ToDecimal(volumedata[0].VolClaim));     //出来高請求
            // Wakamatsu 20170331
            //if (volumedata[0].MonthlyClaim != null)
            if (volumedata[0].MonthlyClaim != null && volumedata[0].MonthlyClaim != 0)
                // Wakamatsu
                //dgv.Rows[8].Cells["LY"].Value = volumedata[0].MonthlyClaim; //請求単月
                dgv.Rows[8].Cells["LY"].Value = decFormat(Convert.ToDecimal(volumedata[0].MonthlyClaim)); //請求単月
                                                                                                          // Wakamatsu

            // Wakamatsu 20170331
            //if (volumedata[0].VolPaid != null)
            if (volumedata[0].VolPaid != null && volumedata[0].VolPaid != 0)
                // Wakamatsu
                //dgv.Rows[11].Cells["LY"].Value = volumedata[0].VolPaid;     //入金単月
                dgv.Rows[11].Cells["LY"].Value = decFormat(Convert.ToDecimal(volumedata[0].VolPaid));     //入金単月
                                                                                                          // Wakamatsu
                                                                                                          // Wakamatsu

            // Wakamatsu 20170331
            //if (volumedata[0].ClaimDate != null)
            if (volumedata[0].ClaimDate != null && volumedata[0].ClaimDate != DateTime.MinValue)
                dgv.Rows[10].Cells["LY"].Value = Convert.ToDateTime(volumedata[0].ClaimDate).ToString("yyyy/MM/dd");     //請求日
            // Wakamatsu 20170331
            //if (volumedata[0].PaidDate != null)
            if (volumedata[0].PaidDate != null && volumedata[0].PaidDate != DateTime.MinValue)
                dgv.Rows[13].Cells["LY"].Value = Convert.ToDateTime(volumedata[0].PaidDate).ToString("yyyy/MM/dd");     //入金日
            // Wakamatsu

            // Wakamatsu 20170331
            //if (volumedata[0].MonthlyCost != null)
            if (volumedata[0].MonthlyCost != null && volumedata[0].MonthlyCost != 0)
                dgv.Rows[16].Cells["LY"].Value = MinusConvert(volumedata[0].MonthlyCost);     //原価単月
            // Wakamatsu 20170331
        }

        /// <summary>
        /// Decimal型の値を表示形式をそろえてString型とする
        /// </summary>
        /// <param name="decNum">処理対象</param>
        /// <returns>処理結果</returns>
        private static string decFormat(decimal decNum)
        {
            return DHandling.DecimaltoStr(decNum, "#,0");
        }

        /// <summary>
        /// 今年度のデータを取得する
        /// </summary>
        /// <param name="colName">処理カウント</param>
        /// <param name="taskCode">業務番号</param>
        /// <param name="yearMonth">年情報</param>
        /// <param name="officeCode">部署</param>
        /// <param name="department">部門</param>
        /// <param name="colCnt">月情報</param>
        /// <returns>処理結果</returns>
        private bool dispVolumeData(int colName, string taskCode, int yearMonth, string officeCode, string department, int colCnt)
        {
            VolumeData vd = new VolumeData();
            volumedata = vd.SelectVolumeData(taskCode, yearMonth, officeCode, department);

            if (volumedata == null) return false;
            loadVolumeData(colName, volumedata, this.dgvOutPut, colCnt);

            return true;
        }

        /// <summary>
        /// 今年度のデータをデータグリッドビューに格納する
        /// </summary>
        /// <param name="intColName">処理カウント</param>
        /// <param name="volumedata">出力データ</param>
        /// <param name="dgv">データグリッドビュー</param>
        /// <param name="intColCnt">月情報</param>
        private void loadVolumeData(int intColName, VolumeData[] volumedata, DataGridView dgv, int intColCnt)
        {
            if (volumedata.Count() < 1) return;

            string strM = "M";
            strM = strM + intColName.ToString();
            intColCnt = 0;
            dgv.Rows[0].Cells[strM].Value = "";
            dgv.Rows[2].Cells[strM].Value = "";
            dgv.Rows[3].Cells[strM].Value = "";
            dgv.Rows[4].Cells[strM].Value = "";
            dgv.Rows[8].Cells[strM].Value = "";
            dgv.Rows[10].Cells[strM].Value = "";
            dgv.Rows[11].Cells[strM].Value = "";
            dgv.Rows[13].Cells[strM].Value = "";
            dgv.Rows[16].Cells[strM].Value = "";
            dgv.Rows[21].Cells[strM].Value = "";

            if (volumedata[intColCnt].MonthlyVolume != null)
                dgv.Rows[0].Cells[strM].Value = decFormat(Convert.ToDecimal(volumedata[intColCnt].MonthlyVolume));//受注単月
            if (volumedata[intColCnt].VolUncomp != null)
                dgv.Rows[2].Cells[strM].Value = decFormat(Convert.ToDecimal(volumedata[intColCnt].VolUncomp));    //出来高未成業務
            if (volumedata[intColCnt].VolClaimRem != null)
                dgv.Rows[3].Cells[strM].Value = decFormat(Convert.ToDecimal(volumedata[intColCnt].VolClaimRem));  //出来高未請求
            if (volumedata[intColCnt].VolClaim != null)
                dgv.Rows[4].Cells[strM].Value = decFormat(Convert.ToDecimal(volumedata[intColCnt].VolClaim));     //出来高請求
            if (volumedata[intColCnt].MonthlyClaim != null)
                dgv.Rows[8].Cells[strM].Value = decFormat(Convert.ToDecimal(volumedata[intColCnt].MonthlyClaim)); //請求単月
            if (volumedata[intColCnt].ClaimDate != null)
            {
                DateTime dtClaimDate = Convert.ToDateTime(volumedata[intColCnt].ClaimDate);
                dgv.Rows[10].Cells[strM].Value = dtClaimDate.ToString("yyyy/MM/dd");   //請求日
            }

            if (volumedata[intColCnt].VolPaid != null)
                dgv.Rows[11].Cells[strM].Value = decFormat(Convert.ToDecimal(volumedata[intColCnt].VolPaid));     //入金単月

            if (volumedata[intColCnt].PaidDate != null)
            {
                DateTime dtPaidDate = Convert.ToDateTime(volumedata[intColCnt].PaidDate);
                dgv.Rows[13].Cells[strM].Value = dtPaidDate.ToString("yyyy/MM/dd");  //入金日
            }

            if (volumedata[intColCnt].MonthlyCost != null)
                dgv.Rows[16].Cells[strM].Value = volumedata[intColCnt].MonthlyCost; //原価単月
            if (volumedata[intColCnt].Comment != null)
                dgv.Rows[21].Cells[strM].Value = volumedata[intColCnt].Comment;    //コメント
            if (volumedata[intColCnt].CarryOverPlanned != null)
            {
                // Wakamatsu 20170308
                //l_GetCarryOver = DHandling.DecimaltoStr(Convert.ToDecimal(volumedata[intColCnt].CarryOverPlanned), "#,0");
                l_GetCarryOver = MinusConvert(volumedata[intColCnt].CarryOverPlanned);
                // Wakamatsu 20170308
            }

            if (intColCnt == 0)
            {
                if (volumedata[intColCnt].Note != null)
                {
                    l_GetNote = volumedata[intColCnt].Note;     //備考
                    // Wakamatsu
                    // Wakamatsu 20170322
                    //l_GetNote2 = volumedata[intColCnt].Note2;   //備考2
                }
            }
            string strTaskState = Convert.ToString(volumedata[intColCnt].TaskStat);
        }

        /// <summary>
        /// 原価データ取得
        /// </summary>
        private void SetOriginalCost(string taskCode, string officeCode)
        {
            string yearMonth = "";
            string month = "";
            crdM7 = null;
            crdM8 = null;
            crdM9 = null;
            crdM10 = null;
            crdM11 = null;
            crdM12 = null;
            crdM1 = null;
            crdM2 = null;
            crdM3 = null;
            crdM4 = null;
            crdM5 = null;
            crdM6 = null;

            if (taskCode.Trim() == "") return;
            string strSql = "";

            // Wakamatsu
            int DisplyLimit = Array.IndexOf(monthArray, l_ClosingDate);

            // Wakamatsu 20170331
            //for (int i = 0; i <= 12; i++)
            for (int i = 1; i <= 12; i++)
            // Wakamatsu 20170331
            {
                // Wakamatsu 20170331
                //if (i == 0)
                //{
                //    // Wakamatsu 20170329
                //    //strSql = " AND ReportDate BETWEEN " + "'" + Convert.ToString(Convert.ToInt32(l_SetYear) - 1) + "-07-01'"
                //    //                                                                 + " AND " + "'" + l_SetYear + "-06-30'";
                //    strSql = " AND ReportDate <= '" + l_SetYear + "-06-30'";
                //    // Wakamatsu 20170329
                //}
                //else
                //{
                yearMonth = l_SetYear;
                int intMonth = i + 6;
                if (intMonth > 12)
                {
                    intMonth = intMonth - 12;
                    yearMonth = Convert.ToString(Convert.ToInt32(l_SetYear) + 1);
                }
                month = "0" + intMonth.ToString();
                if (month.Length > 2)
                    month = month.Substring(1, 2);
                yearMonth = yearMonth + "-" + month;
                strSql = " AND ReportDate LIKE " + "'" + yearMonth + "%'";
                //}
                // Wakamatsu 20170331

                // Wakamatsu
                if (i <= DisplyLimit)
                {
                    SqlHandling sh = new SqlHandling();

                    string sqlStr = " SUM(Cost) AS CostReportTotalMony FROM D_CostReport "
                                    + " WHERE TaskCode = " + "'" + taskCode + "'"
                                    + " AND OfficeCode = " + "'" + officeCode + "'"
                                    + strSql;
                    System.Data.DataTable dt = sh.SelectFullDescription(sqlStr);

                    this.dgvOutPut.Rows[16].Cells[i].Value = "";
                    if ((dt != null) && (dt.Rows.Count > 0))
                    {
                        DataRow dr = dt.Rows[0];
                        if (dr["CostReportTotalMony"] != null && dr["CostReportTotalMony"] != DBNull.Value)
                            this.dgvOutPut.Rows[16].Cells[i].Value = DHandling.DecimaltoStr(Convert.ToDecimal(dr["CostReportTotalMony"]), "#,0");
                        //注文明細を設定
                        sqlStr = " ReportDate, SlipNo, ItemCode, Item, Unit, UnitPrice, Quantity, Cost, SUM(Cost) AS CostReportTotalMony FROM D_CostReport "
                                    + " WHERE TaskCode = " + "'" + taskCode + "'"
                                    + " AND OfficeCode = " + "'" + officeCode + "'"
                                    + strSql
                                    + " GROUP BY ReportDate, SlipNo, ItemCode, Item, Unit, UnitPrice, Quantity, Cost "
                                    + " ORDER BY ReportDate ASC ";
                        dt = sh.SelectFullDescription(sqlStr);

                        if ((dt != null) && (dt.Rows.Count > 0))
                        {
                            CostReportData[] crd = new CostReportData[dt.Rows.Count];
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                CostReportData crdata = new CostReportData();
                                crd[j] = crdata;
                                dr = dt.Rows[j];
                                // Wakamatsu
                                //if (dr["ReportDate"] != null)
                                //    crd[j].ReportDate = Convert.ToDateTime(dr["ReportDate"]);
                                //if (dr["SlipNo"] != null)
                                //    crd[j].SlipNo = Convert.ToInt32(dr["SlipNo"]);
                                //if (dr["ItemCode"] != null)
                                //    crd[j].ItemCode = Convert.ToString(dr["ItemCode"]);
                                //if (dr["Item"] != null)
                                //    crd[j].Item = Convert.ToString(dr["Item"]);
                                //if (dr["Unit"] != null)
                                //    crd[j].Unit = Convert.ToString(dr["Unit"]);
                                //if (dr["UnitPrice"] != null)
                                //    crd[j].UnitPrice = Convert.ToDecimal(dr["UnitPrice"]);
                                //if (dr["Quantity"] != null)
                                //    crd[j].Quantity = Convert.ToDecimal(dr["Quantity"]);
                                //if (dr["Cost"] != null)
                                //    crd[j].Cost = Convert.ToDecimal(dr["Cost"]);
                                if (!String.IsNullOrEmpty(Convert.ToString(dr["ReportDate"])))
                                    crd[j].ReportDate = Convert.ToDateTime(dr["ReportDate"]);
                                if (!String.IsNullOrEmpty(Convert.ToString(dr["SlipNo"])))
                                    crd[j].SlipNo = Convert.ToInt32(dr["SlipNo"]);
                                if (!String.IsNullOrEmpty(Convert.ToString(dr["ItemCode"])))
                                    crd[j].ItemCode = Convert.ToString(dr["ItemCode"]);
                                if (!String.IsNullOrEmpty(Convert.ToString(dr["Item"])))
                                    crd[j].Item = Convert.ToString(dr["Item"]);
                                if (!String.IsNullOrEmpty(Convert.ToString(dr["Unit"])))
                                    crd[j].Unit = Convert.ToString(dr["Unit"]);
                                if (!String.IsNullOrEmpty(Convert.ToString(dr["UnitPrice"])))
                                    crd[j].UnitPrice = Convert.ToDecimal(dr["UnitPrice"]);
                                if (!String.IsNullOrEmpty(Convert.ToString(dr["Quantity"])))
                                    crd[j].Quantity = Convert.ToDecimal(dr["Quantity"]);
                                if (!String.IsNullOrEmpty(Convert.ToString(dr["Cost"])))
                                    crd[j].Cost = Convert.ToDecimal(dr["Cost"]);
                                // Wakamatsu
                            }

                            switch (month)
                            {
                                case "07":
                                    crdM7 = crd;
                                    break;
                                case "08":
                                    crdM8 = crd;
                                    break;
                                case "09":
                                    crdM9 = crd;
                                    break;
                                case "10":
                                    crdM10 = crd;
                                    break;
                                case "11":
                                    crdM11 = crd;
                                    break;
                                case "12":
                                    crdM12 = crd;
                                    break;
                                case "01":
                                    crdM1 = crd;
                                    break;
                                case "02":
                                    crdM2 = crd;
                                    break;
                                case "03":
                                    crdM3 = crd;
                                    break;
                                case "04":
                                    crdM4 = crd;
                                    break;
                                case "05":
                                    crdM5 = crd;
                                    break;
                                case "06":
                                    crdM6 = crd;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                // Wakamatsu
            }
        }

        /// <summary>
        /// 各自動計算部の算出
        /// </summary>
        /// <param name="dgv">対象データグリッドビュー</param>
        private void AutoCalc(DataGridView dgv)
        {
            // Wakamatsu
            int DisplyLimit = Array.IndexOf(monthArray, l_ClosingDate);
            // Wakamatsu

            // Wakamatsu
            //for ( int i = 0; i <= 12; i++)
            for (int i = 0; i < monthArray.Length; i++)
            {
                // Wakamatsu
                if (i <= DisplyLimit)
                {
                    SetCumulative(dgv, i);        //受注累計
                    SetMonthlyTotal(dgv, i);      //月計
                    SetTotalTradingVolume(dgv, i);//累計
                    SetOverTime(dgv, i);          //残業務高
                    SetResidualClaimHigh(dgv, i); //残請求高
                    SetCumulativeM(dgv, i);       //請求累計
                    SetUncompBusAccept(dgv, i);   //未成業務受入金
                    SetCumulativeV(dgv, i);       //入金累計
                    SetAccountsReceivable(dgv, i);//未収入金
                    SetUncompBusAcceptM(dgv, i);  //未成業務受入金
                    SetCumulativeMC(dgv, i);      //原価累計
                    SetCostRate(dgv, i);          //原価率
                }
                // Wakamatsu
            }
        }

        /// <summary>
        /// 受注累計算出
        /// </summary>
        /// <param name="dgv">対象データグリッドビュー</param>
        /// <param name="i">処理カウント</param>
        private void SetCumulative(DataGridView dgv, int i)
        {
            if (i == 0)
            {
                // Wakamatsu
                //if ((dgv.Rows[0].Cells[i].Value != null) && (dgv.Rows[0].Cells[i].Value.ToString().Trim() != ""))
                if (!checkCellValue(dgv, i))
                // Wakamatsu
                {
                    // Wakamatsu
                    if (!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[0].Cells[i].Value)))
                    // Wakamatsu
                    {
                        dgv.Rows[1].Cells[i].Value = dgv.Rows[0].Cells[i].Value;
                        // Wakamatsu 20170308
                        //totalCumulativeAry[i] = Convert.ToDecimal(dgv.Rows[0].Cells[i].Value);
                        totalCumulativeAry[i] = SignConvert(dgv.Rows[0].Cells[i].Value);
                        // Wakamatsu 20170308
                    }
                    // Wakamatsu
                    else
                    {
                        dgv.Rows[1].Cells[i].Value = 0;
                        totalCumulativeAry[i] = 0;
                    }
                    // Wakamatsu
                }
                return;
            }

            dgv.Rows[1].Cells[i].Value = "";//受注累計

            // Wakamatsu
            ////前年、各月の受注単月データを取得
            //decimal cumulative = 0;

            //if((dgv.Rows[0].Cells[i].Value != null) && (dgv.Rows[0].Cells[i].Value.ToString().Trim() != ""))
            //    cumulative = Convert.ToDecimal(dgv.Rows[0].Cells[i].Value);
            //totalCumulativeAry[i] = cumulative + totalCumulativeAry[i - 1];
            //dgv.Rows[1].Cells[i].Value = DHandling.DecimaltoStr(totalCumulativeAry[i],"#,0");

            //if(((dgv.Rows[0].Cells[i].Value == null) || (dgv.Rows[0].Cells[i].Value.ToString() == "")) &&   //受注 単月
            //    ((dgv.Rows[2].Cells[i].Value == null) || (dgv.Rows[2].Cells[i].Value.ToString() == "")) &&   //出来高 未成業務
            //    ((dgv.Rows[3].Cells[i].Value == null) || (dgv.Rows[3].Cells[i].Value.ToString() == "")) &&   //出来高 未請求
            //    ((dgv.Rows[4].Cells[i].Value == null) || (dgv.Rows[4].Cells[i].Value.ToString() == "")) &&   //出来高 請求
            //    ((dgv.Rows[8].Cells[i].Value == null) || (dgv.Rows[8].Cells[i].Value.ToString() == "")) &&   //請求 単月
            //    ((dgv.Rows[11].Cells[i].Value == null) || (dgv.Rows[11].Cells[i].Value.ToString() == "")) &&  //入金 単月
            //    ((dgv.Rows[16].Cells[i].Value == null) || (dgv.Rows[16].Cells[i].Value.ToString() == "")))    //原価 単月
            //{
            //    Decimal totalCumulativeWk = 0;
            //    if(i != 0)
            //    {
            //        for(int iCnt = i + 1;iCnt < 13;iCnt++)
            //        {
            //            cumulative = 0;
            //            if((dgv.Rows[0].Cells[iCnt].Value != null) && (dgv.Rows[0].Cells[iCnt].Value.ToString().Trim() != ""))
            //                cumulative = Convert.ToDecimal(dgv.Rows[0].Cells[iCnt].Value);
            //            totalCumulativeWk = totalCumulativeWk + cumulative;
            //        }
            //    }
            //    if((totalCumulativeAry[i] * totalCumulativeWk) == 0)
            //        dgv.Rows[1].Cells[i].Value = "";
            //}
            Calculation calc = new Calculation();
            // 受注累計算出
            dgv.Rows[1].Cells[i].Value = calc.Cumulative(dgv, i, 0, totalCumulativeAry[i - 1], checkCellValue(dgv, i), "#,0", out totalCumulativeAry[i]);
            // Wakamatsu

            //年度内完工高更新
            //繰越予定額
            decimal decCarryOverPlanned = 0;
            if (l_GetCarryOver.Trim() != "")
                // Wakamatsu 20170308
                //decCarryOverPlanned = Convert.ToDecimal(l_GetCarryOver);
                decCarryOverPlanned = SignConvert(l_GetCarryOver);
            // Wakamatsu 20170308
            // Wakamatsu
            //decimal decYearCompletionHigh = totalCumulativeAry[i] - decCarryOverPlanned;
            //l_GetYearCompHigh = totalCumulativeAry[i].ToString();
            decimal decYearCompletionHigh = totalCumulativeAry[i] - totalCumulativeAry[0] - decCarryOverPlanned;
            l_GetYearCompHigh = totalCumulativeAry[i].ToString();
            // Wakamatsu

            l_GetCompletionHigh = "";
            // Wakamatsu
            for (int j = 1; j <= i; j++)
            // Wakamatsu
            {
                // Wakamatsu
                if (!string.IsNullOrEmpty(Convert.ToString(dgv.Rows[0].Cells[j].Value)))
                // Wakamatsu
                {
                    if (decYearCompletionHigh != 0)
                    {
                        // Wakamatsu 20170308
                        //if (decYearCompletionHigh > 0)
                        //{
                        //    l_GetCompletionHigh = DHandling.DecimaltoStr(Convert.ToDecimal(decYearCompletionHigh), "#,0");
                        //}
                        //else
                        //{
                        //    string strYearCompletionHigh = DHandling.DecimaltoStr(Convert.ToDecimal(decYearCompletionHigh), "#,0");
                        //    l_GetCompletionHigh = strYearCompletionHigh.Replace("-", "△");
                        //}
                        l_GetCompletionHigh = MinusConvert(decYearCompletionHigh);
                        // Wakamatsu 20170308
                    }
                }
            }

            // 20190307 asakawa
            // その３
            // add start
            if (l_SetCarryOver.Trim() != "")
            {
                decCarryOverPlanned = SignConvert(l_SetCarryOver);
            }
            decYearCompletionHigh = totalCumulativeAry[i] - SignConvert(dgv.Rows[5].Cells[0].Value) - decCarryOverPlanned;
            l_GetYearCompHigh = decYearCompletionHigh.ToString();
            // asakawa add end

        }

        /// <summary>
        /// 月計算出
        /// </summary>
        /// <param name="dgv">データグリッドビュー</param>
        /// <param name="i">処理カウント</param>
        private void SetMonthlyTotal(DataGridView dgv, int i)
        {
            dgv.Rows[5].Cells[i].Value = "";

            // Wakamatsu
            ////未成業務
            //decimal volUncomp = 0;
            //if((dgv.Rows[2].Cells[i].Value != null) && (dgv.Rows[2].Cells[i].Value.ToString() != ""))
            //{
            //    volUncomp = Convert.ToDecimal(dgv.Rows[2].Cells[i].Value);
            //}

            ////未請求
            //decimal volClaimRem = 0;
            //if((dgv.Rows[3].Cells[i].Value != null) && (dgv.Rows[3].Cells[i].Value.ToString() != ""))
            //{
            //    volClaimRem = Convert.ToDecimal(dgv.Rows[3].Cells[i].Value);
            //}

            ////請求
            //decimal volClaim = 0;
            //if((dgv.Rows[4].Cells[i].Value != null) && (dgv.Rows[4].Cells[i].Value.ToString() != ""))
            //{
            //    volClaim = Convert.ToDecimal(dgv.Rows[4].Cells[i].Value);
            //}
            // Wakamatsu

            decimal monthlyTotal = 0;

            // Wakamatsu
            //if (((dgv.Rows[0].Cells[i].Value != null) && (dgv.Rows[0].Cells[i].Value.ToString() != "")) ||   //受注 単月
            //    ((dgv.Rows[2].Cells[i].Value != null) && (dgv.Rows[2].Cells[i].Value.ToString() != "")) ||   //出来高 未成業務
            //    ((dgv.Rows[3].Cells[i].Value != null) && (dgv.Rows[3].Cells[i].Value.ToString() != "")) ||   //出来高 未請求
            //    ((dgv.Rows[4].Cells[i].Value != null) && (dgv.Rows[4].Cells[i].Value.ToString() != "")) ||   //出来高 請求
            //    ((dgv.Rows[8].Cells[i].Value != null) && (dgv.Rows[8].Cells[i].Value.ToString() != "")) ||   //請求 単月
            //    ((dgv.Rows[11].Cells[i].Value != null) && (dgv.Rows[11].Cells[i].Value.ToString() != "")) ||  //入金 単月
            //    ((dgv.Rows[16].Cells[i].Value != null) && (dgv.Rows[16].Cells[i].Value.ToString() != "")))    //原価 単月
            if (!checkCellValue(dgv, i))
            // Wakamatsu
            {
                // Wakamatsu
                //monthlyTotal = volUncomp + volClaimRem + volClaim;
                Calculation calc = new Calculation();
                // 月計算出
                monthlyTotal = calc.MonthlyTotal(dgv, i, 2, 3, 4);
                // Wakamatsu
                // Wakamatsu 20170308
                //dgv.Rows[5].Cells[i].Value = DHandling.DecimaltoStr(monthlyTotal, "#,0");//出来高 単月 月計
                dgv.Rows[5].Cells[i].Value = MinusConvert(monthlyTotal);     //出来高 単月 月計
                // Wakamatsu 20170308
            }
        }

        /// <summary>
        /// 累計算出
        /// </summary>
        /// <param name="dgv">データグリッドビュー</param>
        /// <param name="i">処理カウント</param>
        private void SetTotalTradingVolume(DataGridView dgv, int i)
        {
            if (i == 0)
            {
                // Wakamatsu
                //if ((dgv.Rows[5].Cells[i].Value != null) && (dgv.Rows[5].Cells[i].Value.ToString().Trim() != ""))
                if (!checkCellValue(dgv, i))
                // Wakamatsu
                {
                    // Wakamatsu
                    if (!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[5].Cells[i].Value)))
                    // Wakamatsu
                    {
                        dgv.Rows[6].Cells[i].Value = dgv.Rows[5].Cells[i].Value;
                        // Wakamatsu 20170308
                        //totalTradingVolumeAry[i] = Convert.ToDecimal(dgv.Rows[5].Cells[i].Value);
                        totalTradingVolumeAry[i] = SignConvert(dgv.Rows[5].Cells[i].Value);
                        // Wakamatsu 20170308
                    }
                    // Wakamatsu
                    else
                    {
                        dgv.Rows[6].Cells[i].Value = 0;
                        totalTradingVolumeAry[i] = 0;
                    }
                    // Wakamatsu
                }
                return;
            }

            dgv.Rows[6].Cells[i].Value = "";//受注累計

            // Wakamatsu
            ////前年、各月の受注単月データを取得
            //decimal tradingVolume = 0;

            ////SUM($F12:G$12)
            //if ((dgv.Rows[5].Cells[i].Value != null) && (dgv.Rows[5].Cells[i].Value.ToString().Trim() != ""))
            //    tradingVolume = Convert.ToDecimal(dgv.Rows[5].Cells[i].Value);

            //if (i != 0)
            //{
            //    totalTradingVolumeAry[i] = tradingVolume + totalTradingVolumeAry[i - 1];
            //}
            //dgv.Rows[6].Cells[i].Value = DHandling.DecimaltoStr(totalTradingVolumeAry[i], "#,0");

            ////IF(COUNT(G12)>1))
            //if ((dgv.Rows[5].Cells[i].Value == null) || (dgv.Rows[5].Cells[i].Value.ToString().Trim() == ""))   //出来高 月計
            //{
            //    //SUM(H12:$R$12)
            //    Decimal totalTradingVolumeWk = 0;
            //    tradingVolume = 0;
            //    for (int iCnt = i + 1; iCnt < 13; iCnt++)
            //    {
            //        //未成業務
            //        decimal volUncomp = 0;
            //        if ((dgv.Rows[2].Cells[iCnt].Value != null) && (dgv.Rows[2].Cells[iCnt].Value.ToString() != ""))
            //        {
            //            volUncomp = Convert.ToDecimal(dgv.Rows[2].Cells[iCnt].Value);
            //        }

            //        //未請求
            //        decimal volClaimRem = 0;
            //        if ((dgv.Rows[3].Cells[iCnt].Value != null) && (dgv.Rows[3].Cells[iCnt].Value.ToString() != ""))
            //        {
            //            volClaimRem = Convert.ToDecimal(dgv.Rows[3].Cells[iCnt].Value);
            //        }

            //        //請求
            //        decimal volClaim = 0;
            //        if ((dgv.Rows[4].Cells[iCnt].Value != null) && (dgv.Rows[4].Cells[iCnt].Value.ToString() != ""))
            //        {
            //            volClaim = Convert.ToDecimal(dgv.Rows[4].Cells[iCnt].Value);
            //        }
            //        //月計を求める
            //        tradingVolume = volUncomp + volClaimRem + volClaim;
            //        totalTradingVolumeWk = totalTradingVolumeWk + tradingVolume;
            //    }
            //    //IF(SUM(F$12:H12)*SUM(I12:$R$12) = 0,"",SUM($F12:H$12))
            //    if ((totalTradingVolumeAry[i] * totalTradingVolumeWk) == 0)
            //        dgv.Rows[6].Cells[i].Value = "";
            //}

            Calculation calc = new Calculation();
            // 出来高累計算出
            dgv.Rows[6].Cells[i].Value = calc.TotalTradingVolume(dgv, i, 5, 2, 3, 4, totalTradingVolumeAry[i - 1], "#,0", out totalTradingVolumeAry[i]);
            // Wakamatsu

            // Wakamatsu 20170308
            //TotalCumulativeOverCheck(dgv, 6, i);  //受注額を越えたかのチェック
        }

        // Wakamatsu 20170308
        ///// <summary>
        ///// 受注額を超えたか確認
        ///// </summary>
        ///// <param name="dgv">データグリッドビュー</param>
        ///// <param name="rowPos">行</param>
        ///// <param name="colPos">列</param>
        //private void TotalCumulativeOverCheck(DataGridView dgv, int rowPos, int colPos)
        //{
        //    //受注額を越えたかのチェック
        //    decimal totalCumulative = 0;     //受注累計
        //    decimal totalVolume = 0;         //比較する累計
        //    int month = colPos + 6;
        //    if (month > 12) month -= 12;

        //    if ((dgv.Rows[1].Cells[colPos].Value != null) && (dgv.Rows[1].Cells[colPos].Value.ToString() != ""))
        //        totalCumulative = Convert.ToDecimal(dgv.Rows[1].Cells[colPos].Value);
        //    if ((dgv.Rows[rowPos].Cells[colPos].Value != null) && (dgv.Rows[rowPos].Cells[colPos].Value.ToString() != ""))
        //        totalVolume = Convert.ToDecimal(dgv.Rows[rowPos].Cells[colPos].Value);

        //    dgv.Rows[rowPos].Cells[colPos].Style.BackColor = Color.PaleGreen;
        //    if (totalVolume > totalCumulative)
        //        dgv.Rows[rowPos].Cells[colPos].Style.BackColor = Color.Red;
        //}
        // Wakamatsu 20170308

        /// <summary>
        /// 残業務高算出
        /// </summary>
        /// <param name="dgv">データグリッドビュー</param>
        /// <param name="i">処理カウント</param>
        private void SetOverTime(DataGridView dgv, int i)
        {
            dgv.Rows[7].Cells[i].Value = "";//残業務高

            // Wakamatsu
            //decimal cumulative = 0;
            //decimal tradingVolume = 0;
            // Wakamatsu
            // Wakamatsu 20170308
            //decimal overTime = 0;

            // Wakamatsu
            //IF(COUNT(F7,F13)>1))
            //if (((dgv.Rows[1].Cells[i].Value != null) && (dgv.Rows[1].Cells[i].Value.ToString() != "")) ||   //受注 累計
            //    ((dgv.Rows[6].Cells[i].Value != null) && (dgv.Rows[6].Cells[i].Value.ToString() != "")))     //出来高累計
            if ((!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[1].Cells[i].Value))) ||
                (!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[6].Cells[i].Value))))
            // Wakamatsu
            {
                // Wakamatsu
                ////SUM($F$6:G6)
                //Decimal totalCumulativeWk = 0;
                //Decimal totalTradingVolumeWk = 0;

                //for (int iCnt = 0; iCnt < i + 1; iCnt++)
                //{
                //    //受注累計
                //    cumulative = 0;
                //    if ((dgv.Rows[0].Cells[iCnt].Value != null) && (dgv.Rows[0].Cells[iCnt].Value.ToString().Trim() != ""))
                //        cumulative = Convert.ToDecimal(dgv.Rows[0].Cells[iCnt].Value);
                //    totalCumulativeWk = totalCumulativeWk + cumulative;


                //    //出来高累計
                //    //未成業務
                //    decimal volUncomp = 0;
                //    if ((dgv.Rows[2].Cells[iCnt].Value != null) && (dgv.Rows[2].Cells[iCnt].Value.ToString() != ""))
                //    {
                //        volUncomp = Convert.ToDecimal(dgv.Rows[2].Cells[iCnt].Value);
                //    }

                //    //未請求
                //    decimal volClaimRem = 0;
                //    if ((dgv.Rows[3].Cells[iCnt].Value != null) && (dgv.Rows[3].Cells[iCnt].Value.ToString() != ""))
                //    {
                //        volClaimRem = Convert.ToDecimal(dgv.Rows[3].Cells[iCnt].Value);
                //    }

                //    //請求
                //    decimal volClaim = 0;
                //    if ((dgv.Rows[4].Cells[iCnt].Value != null) && (dgv.Rows[4].Cells[iCnt].Value.ToString() != ""))
                //    {
                //        volClaim = Convert.ToDecimal(dgv.Rows[4].Cells[iCnt].Value);
                //    }
                //    //月計を求める
                //    tradingVolume = volUncomp + volClaimRem + volClaim;
                //    totalTradingVolumeWk = totalTradingVolumeWk + tradingVolume;
                //}

                ////SUM($F$6:G6)-SUM($F$12:G12)
                //overTime = (totalCumulativeWk - totalTradingVolumeWk);
                Calculation calc = new Calculation();
                // 未成業務受入金算出
                // Wakamatsu 20170308
                //overTime = calc.SubtrahendVol(dgv, i, 0, 2, 3, 4);
                //// Wakamatsu

                //dgv.Rows[7].Cells[i].Value = DHandling.DecimaltoStr(overTime, "#,0");
                //if (overTime < 0)
                //{
                //    string strOverTime = DHandling.DecimaltoStr(overTime, "#,0");
                //    dgv.Rows[7].Cells[i].Value = strOverTime.Replace("-", "△");
                //}
                dgv.Rows[7].Cells[i].Value = MinusConvert(calc.SubtrahendVol(dgv, i, 0, 2, 3, 4));
            }
        }

        /// <summary>
        /// 残請求高算出
        /// </summary>
        /// <param name="dgv">データグリッドビュー</param>
        /// <param name="i">処理カウント</param>
        private void SetResidualClaimHigh(DataGridView dgv, int i)
        {
            dgv.Rows[14].Cells[i].Value = "";//残請求高
            dgv[i, 14].Style.BackColor = Color.PaleGreen;

            // Wakamatsu
            //decimal decTradingVolume = 0;
            //decimal cumulativeM = 0;
            // Wakamatsu
            decimal decResidualClaimHigh = 0;

            //IF(COUNT(F13,F16)>1))
            // Wakamatsu
            //if (((dgv.Rows[6].Cells[i].Value != null) && (dgv.Rows[6].Cells[i].Value.ToString() != "")) ||   //出来高累計
            //    ((dgv.Rows[9].Cells[i].Value != null) && (dgv.Rows[9].Cells[i].Value.ToString() != "")))     //請求累計
            if ((!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[6].Cells[i].Value))) ||
                (!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[9].Cells[i].Value))))
            // Wakamatsu
            {
                // Wakamatsu
                ////SUM($F$12:G12)
                //Decimal totalTradingVolumeWk = 0;
                //Decimal totalCumulativeMWk = 0;

                //for (int iCnt = 0; iCnt < i + 1; iCnt++)
                //{
                //    //出来高累計
                //    //未成業務
                //    decimal volUncomp = 0;
                //    if ((dgv.Rows[2].Cells[iCnt].Value != null) && (dgv.Rows[2].Cells[iCnt].Value.ToString() != ""))
                //    {
                //        volUncomp = Convert.ToDecimal(dgv.Rows[2].Cells[iCnt].Value);
                //    }

                //    //未請求
                //    decimal volClaimRem = 0;
                //    if ((dgv.Rows[3].Cells[iCnt].Value != null) && (dgv.Rows[3].Cells[iCnt].Value.ToString() != ""))
                //    {
                //        volClaimRem = Convert.ToDecimal(dgv.Rows[3].Cells[iCnt].Value);
                //    }

                //    //請求
                //    decimal volClaim = 0;
                //    if ((dgv.Rows[4].Cells[iCnt].Value != null) && (dgv.Rows[4].Cells[iCnt].Value.ToString() != ""))
                //    {
                //        volClaim = Convert.ToDecimal(dgv.Rows[4].Cells[iCnt].Value);
                //    }
                //    //月計を求める
                //    decTradingVolume = volUncomp + volClaimRem + volClaim;
                //    totalTradingVolumeWk = totalTradingVolumeWk + decTradingVolume;

                //    //請求累計
                //    cumulativeM = 0;
                //    if ((dgv.Rows[8].Cells[iCnt].Value != null) && (dgv.Rows[8].Cells[iCnt].Value.ToString().Trim() != ""))
                //        cumulativeM = Convert.ToDecimal(dgv.Rows[8].Cells[iCnt].Value);
                //    totalCumulativeMWk = totalCumulativeMWk + cumulativeM;
                //}

                ////SUM($F$12:G12)-SUM($F$15:G15)
                //decResidualClaimHigh = (totalTradingVolumeWk - totalCumulativeMWk);
                Calculation calc = new Calculation();
                // 残請求高算出
                decResidualClaimHigh = calc.MinuendVol(dgv, i, 2, 3, 4, 8);
                // Wakamatsu

                if (decResidualClaimHigh > 0)
                {
                    // Wakamatsu 20170308
                    //dgv.Rows[14].Cells[i].Value = DHandling.DecimaltoStr(decResidualClaimHigh, "#,0");
                    dgv.Rows[14].Cells[i].Value = MinusConvert(decResidualClaimHigh);
                    // Wakamatsu 20170308
                }
            }
        }

        /// <summary>
        /// 請求累計算出 
        /// </summary>
        /// <param name="dgv">データグリッドビュー</param>
        /// <param name="i">処理カウント</param>
        private void SetCumulativeM(DataGridView dgv, int i)
        {
            if (i == 0)
            {
                // Wakamatsu
                //if ((dgv.Rows[8].Cells[i].Value != null) && (dgv.Rows[8].Cells[i].Value.ToString().Trim() != ""))
                if (!checkCellValue(dgv, i))
                // Wakamatsu
                {
                    // Wakamatsu
                    if (!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[8].Cells[i].Value)))
                    // Wakamatsu
                    {
                        dgv.Rows[9].Cells[i].Value = dgv.Rows[8].Cells[i].Value;
                        // Wakamatsu 20170308
                        //totalCumulativeMAry[i] = Convert.ToDecimal(dgv.Rows[8].Cells[i].Value);
                        totalCumulativeMAry[i] = SignConvert(dgv.Rows[8].Cells[i].Value);
                        // Wakamatsu 20170308
                    }
                    // Wakamatsu
                    else
                    {
                        dgv.Rows[9].Cells[i].Value = 0;
                        totalCumulativeMAry[i] = 0;
                    }
                    // Wakamatsu
                }
                return;
            }

            dgv.Rows[9].Cells[i].Value = "";//受注累計

            // Wakamatsu
            ////前年、各月の受注単月データを取得
            //decimal cumulativeM = 0;

            ////SUM($F$15:G15)
            //if ((dgv.Rows[8].Cells[i].Value != null) && (dgv.Rows[8].Cells[i].Value.ToString().Trim() != ""))
            //    cumulativeM = Convert.ToDecimal(dgv.Rows[8].Cells[i].Value);
            //totalCumulativeMAry[i] = cumulativeM + totalCumulativeMAry[i - 1];
            //dgv.Rows[9].Cells[i].Value = DHandling.DecimaltoStr(totalCumulativeMAry[i], "#,0");

            ////IF(COUNT(F6,F8:F10,F15,F18,F23)>1))
            //if (((dgv.Rows[0].Cells[i].Value == null) || (dgv.Rows[0].Cells[i].Value.ToString() == "")) &&   //受注 単月
            //    ((dgv.Rows[2].Cells[i].Value == null) || (dgv.Rows[2].Cells[i].Value.ToString() == "")) &&   //出来高 未成業務
            //    ((dgv.Rows[3].Cells[i].Value == null) || (dgv.Rows[3].Cells[i].Value.ToString() == "")) &&   //出来高 未請求
            //    ((dgv.Rows[4].Cells[i].Value == null) || (dgv.Rows[4].Cells[i].Value.ToString() == "")) &&   //出来高 請求
            //    ((dgv.Rows[8].Cells[i].Value == null) || (dgv.Rows[8].Cells[i].Value.ToString() == "")) &&   //請求 単月
            //    ((dgv.Rows[11].Cells[i].Value == null) || (dgv.Rows[11].Cells[i].Value.ToString() == "")) &&  //入金 単月
            //    ((dgv.Rows[16].Cells[i].Value == null) || (dgv.Rows[16].Cells[i].Value.ToString() == "")))    //原価 単月
            //{
            //    //SUM(H15:$R$15)
            //    Decimal totalCumulativeMWk = 0;
            //    if (i != 0)
            //    {
            //        //
            //        for (int iCnt = i + 1; iCnt < 13; iCnt++)
            //        {
            //            cumulativeM = 0;
            //            if ((dgv.Rows[8].Cells[iCnt].Value != null) && (dgv.Rows[8].Cells[iCnt].Value.ToString().Trim() != ""))
            //                cumulativeM = Convert.ToDecimal(dgv.Rows[8].Cells[iCnt].Value);
            //            totalCumulativeMWk = totalCumulativeMWk + cumulativeM;
            //        }
            //    }
            //    //IF(SUM(F$6:G6)*SUM(H6:$R$))
            //    if ((totalCumulativeMAry[i] * totalCumulativeMWk) == 0)
            //        dgv.Rows[9].Cells[i].Value = "";
            //}

            Calculation calc = new Calculation();
            // 請求累計算出
            dgv.Rows[9].Cells[i].Value = calc.Cumulative(dgv, i, 8, totalCumulativeMAry[i - 1], checkCellValue(dgv, i), "#,0", out totalCumulativeMAry[i]);
            // Wakamatsu

            // Wakamatsu 20170308
            //TotalCumulativeOverCheck(dgv, 9, i);  //受注額を越えたかのチェック
        }

        /// <summary>
        /// 未成業務受入金
        /// </summary>
        /// <param name="dgv">データグリッドビュー</param>
        /// <param name="i">処理カウント</param>
        private void SetUncompBusAccept(DataGridView dgv, int i)
        {
            dgv.Rows[15].Cells[i].Value = "";//未成業務受入金

            // Wakamatsu
            //decimal tradingVolume = 0;
            //decimal cumulativeM = 0;
            // Wakamatsu
            decimal uncompBusAccept = 0;

            //IF(COUNT(F13,F16)>1))
            // Wakamatsu
            //if (((dgv.Rows[9].Cells[i].Value != null) && (dgv.Rows[9].Cells[i].Value.ToString() != "")) ||   //請求累計
            //    ((dgv.Rows[6].Cells[i].Value != null) && (dgv.Rows[6].Cells[i].Value.ToString() != "")))     //出来高累計
            if ((!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[6].Cells[i].Value))) ||
                (!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[9].Cells[i].Value))))
            // Wakamatsu
            {
                // Wakamatsu
                ////SUM($F$12:G12)
                //Decimal totalCumulativeMWk = 0;
                //Decimal totalTradingVolumeWk = 0;

                //for (int iCnt = 0; iCnt < i + 1; iCnt++)
                //{
                //    //請求累計
                //    cumulativeM = 0;
                //    if ((dgv.Rows[8].Cells[iCnt].Value != null) && (dgv.Rows[8].Cells[iCnt].Value.ToString().Trim() != ""))
                //        cumulativeM = Convert.ToDecimal(dgv.Rows[8].Cells[iCnt].Value);
                //    totalCumulativeMWk = totalCumulativeMWk + cumulativeM;


                //    //出来高累計
                //    //未成業務
                //    decimal volUncomp = 0;
                //    if ((dgv.Rows[2].Cells[iCnt].Value != null) && (dgv.Rows[2].Cells[iCnt].Value.ToString() != ""))
                //    {
                //        volUncomp = Convert.ToDecimal(dgv.Rows[2].Cells[iCnt].Value);
                //    }

                //    //未請求
                //    decimal volClaimRem = 0;
                //    if ((dgv.Rows[3].Cells[iCnt].Value != null) && (dgv.Rows[3].Cells[iCnt].Value.ToString() != ""))
                //    {
                //        volClaimRem = Convert.ToDecimal(dgv.Rows[3].Cells[iCnt].Value);
                //    }

                //    //請求
                //    decimal volClaim = 0;
                //    if ((dgv.Rows[4].Cells[iCnt].Value != null) && (dgv.Rows[4].Cells[iCnt].Value.ToString() != ""))
                //    {
                //        volClaim = Convert.ToDecimal(dgv.Rows[4].Cells[iCnt].Value);
                //    }
                //    //月計を求める
                //    tradingVolume = volUncomp + volClaimRem + volClaim;
                //    totalTradingVolumeWk = totalTradingVolumeWk + tradingVolume;

                //}

                ////SUM($F$15:G15) - SUM($F$12:G12)
                //uncompBusAccept = (totalCumulativeMWk - totalTradingVolumeWk);
                Calculation calc = new Calculation();
                // 未成業務受入金算出
                uncompBusAccept = calc.SubtrahendVol(dgv, i, 8, 2, 3, 4);
                // Wakamatsu

                if (uncompBusAccept > 0)
                    // Wakamatsu 20170308
                    //dgv.Rows[15].Cells[i].Value = DHandling.DecimaltoStr(uncompBusAccept, "#,0");
                    dgv.Rows[15].Cells[i].Value = MinusConvert(uncompBusAccept);
                // Wakamatsu 20170308
            }
        }

        /// <summary>
        /// 入金累計
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="i"></param>
        private void SetCumulativeV(DataGridView dgv, int i)
        {
            if (i == 0)
            {
                // Wakamatsu
                //if ((dgv.Rows[11].Cells[i].Value != null) && (dgv.Rows[11].Cells[i].Value.ToString().Trim() != ""))
                if (!checkCellValue(dgv, i))
                // Wakamatsu
                {
                    // Wakamatsu
                    if (!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[11].Cells[i].Value)))
                    {
                        dgv.Rows[12].Cells[i].Value = dgv.Rows[11].Cells[i].Value;
                        // Wakamatsu 20170308
                        //totalCumulativeVAry[i] = Convert.ToDecimal(dgv.Rows[11].Cells[i].Value);
                        totalCumulativeVAry[i] = SignConvert(dgv.Rows[11].Cells[i].Value);
                        // Wakamatsu 20170308
                    }
                    // Wakamatsu
                    else
                    {
                        dgv.Rows[12].Cells[i].Value = 0;
                        totalCumulativeVAry[i] = 0;
                    }
                    // Wakamatsu
                }
                return;
            }

            dgv.Rows[12].Cells[i].Value = "";//入金累計

            // Wakamatsu
            //decimal cumulativeV = 0;

            ////SUM($F$18:G18)
            //if ((dgv.Rows[11].Cells[i].Value != null) && (dgv.Rows[11].Cells[i].Value.ToString().Trim() != ""))
            //    cumulativeV = Convert.ToDecimal(dgv.Rows[11].Cells[i].Value);
            //totalCumulativeVAry[i] = cumulativeV + totalCumulativeVAry[i - 1];
            //dgv.Rows[12].Cells[i].Value = DHandling.DecimaltoStr(totalCumulativeVAry[i], "#,0");

            ////IF(COUNT(F6,F8:F10,F15,F18,F23)>1))
            //if (((dgv.Rows[0].Cells[i].Value == null) || (dgv.Rows[0].Cells[i].Value.ToString() == "")) &&   //受注 単月
            //    ((dgv.Rows[2].Cells[i].Value == null) || (dgv.Rows[2].Cells[i].Value.ToString() == "")) &&   //出来高 未成業務
            //    ((dgv.Rows[3].Cells[i].Value == null) || (dgv.Rows[3].Cells[i].Value.ToString() == "")) &&   //出来高 未請求
            //    ((dgv.Rows[4].Cells[i].Value == null) || (dgv.Rows[4].Cells[i].Value.ToString() == "")) &&   //出来高 請求
            //    ((dgv.Rows[8].Cells[i].Value == null) || (dgv.Rows[8].Cells[i].Value.ToString() == "")) &&   //請求 単月
            //    ((dgv.Rows[11].Cells[i].Value == null) || (dgv.Rows[11].Cells[i].Value.ToString() == "")) &&  //入金 単月
            //    ((dgv.Rows[16].Cells[i].Value == null) || (dgv.Rows[16].Cells[i].Value.ToString() == "")))    //原価 単月
            //{
            //    //SUM(H18:$R$18)
            //    Decimal totalCumulativeVWk = 0;
            //    if (i != 0)
            //    {
            //        for (int iCnt = i + 1; iCnt < 13; iCnt++)
            //        {
            //            cumulativeV = 0;
            //            if ((dgv.Rows[11].Cells[iCnt].Value != null) && (dgv.Rows[11].Cells[iCnt].Value.ToString().Trim() != ""))
            //                cumulativeV = Convert.ToDecimal(dgv.Rows[11].Cells[iCnt].Value);
            //            totalCumulativeVWk = totalCumulativeVWk + cumulativeV;
            //        }
            //    }
            //    //IF(SUM(F$18:G18)*SUM(H$18:$R18))
            //    if ((totalCumulativeVAry[i] * totalCumulativeVWk) == 0)
            //        dgv.Rows[12].Cells[i].Value = "";
            //}

            Calculation calc = new Calculation();
            // 入金累計算出
            dgv.Rows[12].Cells[i].Value = calc.Cumulative(dgv, i, 11, totalCumulativeVAry[i - 1], checkCellValue(dgv, i), "#,0", out totalCumulativeVAry[i]);
            // Wakamatsu

            // Wakamatsu 20170308
            //TotalCumulativeOverCheck(dgv, 12, i);  //受注額を越えたかのチェック
        }

        /// <summary>
        /// 未収入金算出
        /// </summary>
        /// <param name="dgv">データグリッドビュー</param>
        /// <param name="i">処理カウント</param>
        private void SetAccountsReceivable(DataGridView dgv, int i)
        {
            dgv.Rows[19].Cells[i].Value = "";             //未収入金
            dgv[i, 19].Style.BackColor = Color.PaleGreen;
            // Wakamatsu
            //decimal tradingVolume = 0;                           //出来高月計
            //decimal cumulativeM = 0;                             //請求単月
            //decimal cumulativeV = 0;                             //入金単月
            // Wakamatsu
            decimal accountsReceivable = 0;                      //未収入金

            // Wakamatsu
            dgv.Rows[19].Cells[i].Value = "";
            // Wakamatsu

            //IF(COUNT(F6,F8:F10,F15,F18,F23)>1))
            // Wakamatsu
            //if (((dgv.Rows[0].Cells[i].Value != null) && (dgv.Rows[0].Cells[i].Value.ToString() != "")) ||   //受注 単月
            //    ((dgv.Rows[2].Cells[i].Value != null) && (dgv.Rows[2].Cells[i].Value.ToString() != "")) ||   //出来高 未成業務
            //    ((dgv.Rows[3].Cells[i].Value != null) && (dgv.Rows[3].Cells[i].Value.ToString() != "")) ||   //出来高 未請求
            //    ((dgv.Rows[4].Cells[i].Value != null) && (dgv.Rows[4].Cells[i].Value.ToString() != "")) ||   //出来高 請求
            //    ((dgv.Rows[8].Cells[i].Value != null) && (dgv.Rows[8].Cells[i].Value.ToString() != "")) ||   //請求 単月
            //    ((dgv.Rows[11].Cells[i].Value != null) && (dgv.Rows[11].Cells[i].Value.ToString() != "")) ||  //入金 単月
            //    ((dgv.Rows[16].Cells[i].Value != null) && (dgv.Rows[16].Cells[i].Value.ToString() != "")))    //原価 単月
            if (!checkCellValue(dgv, i))
            // Wakamatsu
            {
                // Wakamatsu
                //// SUM($F$12:G12) SUM($F$15:G15) SUM($F$18:G18)
                //Decimal totalTradingVolumeWk = 0;
                //Decimal totalCumulativeMWk = 0;
                //Decimal totalCumulativeVWk = 0;

                //for (int iCnt = 0; iCnt < i + 1; iCnt++)
                //{
                //    //出来高累計
                //    //未成業務
                //    decimal volUncomp = 0;
                //    if ((dgv.Rows[2].Cells[iCnt].Value != null) && (dgv.Rows[2].Cells[iCnt].Value.ToString() != ""))
                //    {
                //        volUncomp = Convert.ToDecimal(dgv.Rows[2].Cells[iCnt].Value);
                //    }

                //    //未請求
                //    decimal volClaimRem = 0;
                //    if ((dgv.Rows[3].Cells[iCnt].Value != null) && (dgv.Rows[3].Cells[iCnt].Value.ToString() != ""))
                //    {
                //        volClaimRem = Convert.ToDecimal(dgv.Rows[3].Cells[iCnt].Value);
                //    }

                //    //請求
                //    decimal volClaim = 0;
                //    if ((dgv.Rows[4].Cells[iCnt].Value != null) && (dgv.Rows[4].Cells[iCnt].Value.ToString() != ""))
                //    {
                //        volClaim = Convert.ToDecimal(dgv.Rows[4].Cells[iCnt].Value);
                //    }
                //    //月計を求める
                //    tradingVolume = volUncomp + volClaimRem + volClaim;
                //    totalTradingVolumeWk = totalTradingVolumeWk + tradingVolume;


                //    //請求累計
                //    cumulativeM = 0;
                //    if ((dgv.Rows[8].Cells[iCnt].Value != null) && (dgv.Rows[8].Cells[iCnt].Value.ToString().Trim() != ""))
                //        cumulativeM = Convert.ToDecimal(dgv.Rows[8].Cells[iCnt].Value);
                //    totalCumulativeMWk = totalCumulativeMWk + cumulativeM;

                //    //入金累計
                //    cumulativeV = 0;
                //    if ((dgv.Rows[11].Cells[iCnt].Value != null) && (dgv.Rows[11].Cells[iCnt].Value.ToString().Trim() != ""))
                //        cumulativeV = Convert.ToDecimal(dgv.Rows[11].Cells[iCnt].Value);
                //    totalCumulativeVWk = totalCumulativeVWk + cumulativeV;

                //}

                ////IF SUM($F12:F$12) > SUM($F15:F$15) → 出来高累計 > 請求累計 
                //if (totalTradingVolumeWk > totalCumulativeMWk)
                //{
                //    //IF(SUM($F12:I$12)-SUM($F$18:I18 ) >= 0 → 出来高累計 - 入金累計 >= 0
                //    if ((totalTradingVolumeWk - totalCumulativeVWk) > 0)
                //    {
                //        accountsReceivable = totalTradingVolumeWk - totalCumulativeVWk;
                //    }
                //}
                //else
                //{
                //    //IF(SUM($F15:I$15)-SUM($F$18:I18) >= 0 → 請求累計 - 入金累計 >= 0
                //    if ((totalCumulativeMWk - totalCumulativeVWk) > 0)
                //    {
                //        accountsReceivable = totalCumulativeMWk - totalCumulativeVWk;
                //    }
                //}

                Calculation calc = new Calculation();
                // 未収入金算出
                accountsReceivable = calc.AccountsReceivable(dgv, i, 2, 3, 4, 8, 11);
                // Wakamatsu

                // Wakamatsu
                if (accountsReceivable >= 0)
                    // Wakamatsu
                    // Wakamatsu 20170308
                    //dgv.Rows[19].Cells[i].Value = DHandling.DecimaltoStr(accountsReceivable, "#,0");
                    dgv.Rows[19].Cells[i].Value = MinusConvert(accountsReceivable);
                // Wakamatsu 20170308
            }
        }

        /// <summary>
        /// 未成業務受入金
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="i"></param>
        private void SetUncompBusAcceptM(DataGridView dgv, int i)
        {
            dgv.Rows[20].Cells[i].Value = "";//未成業務受入金
            // Wakamatsu
            //decimal cumulativeV = 0;//入金単月
            //decimal tradingVolume = 0;//出来高月計
            // Wakamatsu
            decimal uncompBusAcceptM = 0;//未成業務受入金

            //IF(COUNT(F6,F8:F10,F15,F18,F23)>1))
            // Wakamatsu
            //if (((dgv.Rows[0].Cells[i].Value != null) && (dgv.Rows[0].Cells[i].Value.ToString() != "")) ||   //受注 単月
            //    ((dgv.Rows[2].Cells[i].Value != null) && (dgv.Rows[2].Cells[i].Value.ToString() != "")) ||   //出来高 未成業務
            //    ((dgv.Rows[3].Cells[i].Value != null) && (dgv.Rows[3].Cells[i].Value.ToString() != "")) ||   //出来高 未請求
            //    ((dgv.Rows[4].Cells[i].Value != null) && (dgv.Rows[4].Cells[i].Value.ToString() != "")) ||   //出来高 請求
            //    ((dgv.Rows[8].Cells[i].Value != null) && (dgv.Rows[8].Cells[i].Value.ToString() != "")) ||   //請求 単月
            //    ((dgv.Rows[11].Cells[i].Value != null) && (dgv.Rows[11].Cells[i].Value.ToString() != "")) ||  //入金 単月
            //    ((dgv.Rows[16].Cells[i].Value != null) && (dgv.Rows[16].Cells[i].Value.ToString() != "")))    //原価 単月
            if (!checkCellValue(dgv, i))
            // Wakamatsu
            {
                // Wakamatsu
                ////SUM($F$18:G18) SUM($F$12:G12)
                //Decimal totalCumulativeVWk = 0;
                //Decimal totalTradingVolumeWk = 0;

                //for (int iCnt = 0; iCnt < i + 1; iCnt++)
                //{
                //    //入金累計
                //    cumulativeV = 0;
                //    if ((dgv.Rows[11].Cells[iCnt].Value != null) && (dgv.Rows[11].Cells[iCnt].Value.ToString().Trim() != ""))
                //        cumulativeV = Convert.ToDecimal(dgv.Rows[11].Cells[iCnt].Value);
                //    totalCumulativeVWk = totalCumulativeVWk + cumulativeV;

                //    //出来高累計
                //    //未成業務
                //    decimal volUncomp = 0;
                //    if ((dgv.Rows[2].Cells[iCnt].Value != null) && (dgv.Rows[2].Cells[iCnt].Value.ToString() != ""))
                //    {
                //        volUncomp = Convert.ToDecimal(dgv.Rows[2].Cells[iCnt].Value);
                //    }

                //    //未請求
                //    decimal volClaimRem = 0;
                //    if ((dgv.Rows[3].Cells[iCnt].Value != null) && (dgv.Rows[3].Cells[iCnt].Value.ToString() != ""))
                //    {
                //        volClaimRem = Convert.ToDecimal(dgv.Rows[3].Cells[iCnt].Value);
                //    }

                //    //請求
                //    decimal volClaim = 0;
                //    if ((dgv.Rows[4].Cells[iCnt].Value != null) && (dgv.Rows[4].Cells[iCnt].Value.ToString() != ""))
                //    {
                //        volClaim = Convert.ToDecimal(dgv.Rows[4].Cells[iCnt].Value);
                //    }
                //    //月計を求める
                //    tradingVolume = volUncomp + volClaimRem + volClaim;
                //    totalTradingVolumeWk = totalTradingVolumeWk + tradingVolume;

                //}

                ////SUM($F$15:G15) - SUM($F$12:G12)
                //uncompBusAcceptM = (totalCumulativeVWk - totalTradingVolumeWk);

                Calculation calc = new Calculation();
                uncompBusAcceptM = calc.SubtrahendVol(dgv, i, 11, 2, 3, 4);
                // Wakamatsu

                if (uncompBusAcceptM > 0)
                    // Wakamatsu 20170308
                    //dgv.Rows[20].Cells[i].Value = DHandling.DecimaltoStr(uncompBusAcceptM, "#,0");
                    dgv.Rows[20].Cells[i].Value = MinusConvert(uncompBusAcceptM);
                // Wakamatsu 20170308
            }
        }

        /// <summary>
        /// 原価累計算出 
        /// </summary>
        /// <param name="dgv">データグリッドビュー</param>
        /// <param name="i">処理カウント</param>
        private void SetCumulativeMC(DataGridView dgv, int i)
        {
            if (i == 0)
            {
                // Wakamatsu
                //if ((dgv.Rows[16].Cells[i].Value != null) && (dgv.Rows[16].Cells[i].Value.ToString().Trim() != ""))
                if (!checkCellValue(dgv, i))
                // Wakamatsu
                {
                    // Wakamatsu
                    if (!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[16].Cells[i].Value)))
                    // Wakamatsu
                    {
                        dgv.Rows[17].Cells[i].Value = dgv.Rows[16].Cells[i].Value;
                        // Wakamatsu 20170308
                        //totalCumulativeMCAry[i] = Convert.ToDecimal(dgv.Rows[16].Cells[i].Value);
                        totalCumulativeMCAry[i] = SignConvert(dgv.Rows[16].Cells[i].Value);
                        // Wakamatsu 20170308
                    }
                    // Wakamatsu
                    else
                    {
                        dgv.Rows[17].Cells[i].Value = 0;
                        totalCumulativeMCAry[i] = 0;
                    }
                    // Wakamatsu
                }
                return;
            }

            dgv.Rows[17].Cells[i].Value = "";//原価累計

            // Wakamatsu
            //decimal cumulativeMC = 0;

            ////SUM($F$18:G18)
            //if ((dgv.Rows[16].Cells[i].Value != null) && (dgv.Rows[16].Cells[i].Value.ToString().Trim() != ""))
            //    cumulativeMC = Convert.ToDecimal(dgv.Rows[16].Cells[i].Value);
            //totalCumulativeMCAry[i] = cumulativeMC + totalCumulativeMCAry[i - 1];
            //dgv.Rows[17].Cells[i].Value = DHandling.DecimaltoStr(totalCumulativeMCAry[i], "#,0");

            ////IF(COUNT(F6,F8:F10,F15,F18,F23)>1))
            //if (((dgv.Rows[0].Cells[i].Value == null) || (dgv.Rows[0].Cells[i].Value.ToString() == "")) &&   //受注 単月
            //    ((dgv.Rows[2].Cells[i].Value == null) || (dgv.Rows[2].Cells[i].Value.ToString() == "")) &&   //出来高 未成業務
            //    ((dgv.Rows[3].Cells[i].Value == null) || (dgv.Rows[3].Cells[i].Value.ToString() == "")) &&   //出来高 未請求
            //    ((dgv.Rows[4].Cells[i].Value == null) || (dgv.Rows[4].Cells[i].Value.ToString() == "")) &&   //出来高 請求
            //    ((dgv.Rows[8].Cells[i].Value == null) || (dgv.Rows[8].Cells[i].Value.ToString() == "")) &&   //請求 単月
            //    ((dgv.Rows[11].Cells[i].Value == null) || (dgv.Rows[11].Cells[i].Value.ToString() == "")) &&  //入金 単月
            //    ((dgv.Rows[16].Cells[i].Value == null) || (dgv.Rows[16].Cells[i].Value.ToString() == "")))    //原価 単月
            //{
            //    //SUM(H23:$R$23)
            //    Decimal totalCumulativeMCWk = 0;
            //    if (i != 0)
            //    {
            //        for (int iCnt = i + 1; iCnt < 13; iCnt++)
            //        {
            //            cumulativeMC = 0;
            //            if ((dgv.Rows[16].Cells[iCnt].Value != null) && (dgv.Rows[16].Cells[iCnt].Value.ToString().Trim() != ""))
            //                cumulativeMC = Convert.ToDecimal(dgv.Rows[16].Cells[iCnt].Value);
            //            totalCumulativeMCWk = totalCumulativeMCWk + cumulativeMC;
            //        }
            //    }
            //    //IF(SUM(F$18:G18)*SUM(H$18:$R18))
            //    if ((totalCumulativeMCAry[i] * totalCumulativeMCWk) == 0)
            //        dgv.Rows[17].Cells[i].Value = "";
            //}

            Calculation calc = new Calculation();
            // 原価累計算出
            dgv.Rows[17].Cells[i].Value = calc.Cumulative(dgv, i, 16, totalCumulativeMCAry[i - 1], checkCellValue(dgv, i), "#,0", out totalCumulativeMCAry[i]);
            // Wakamatsu
        }

        /// <summary>
        /// 原価率算出
        /// </summary>
        /// <param name="dgv">データグリッドビュー</param>
        /// <param name="i">処理カウント</param>
        private void SetCostRate(DataGridView dgv, int i)
        {
            // Wakamatsu
            //decimal tradingVolume = 0;
            //decimal cumulativeMC = 0;
            //dgv.Rows[18].Cells[i].Value = ""; //原価率
            //if ((dgv.Rows[6].Cells[i].Value == null) || (dgv.Rows[6].Cells[i].Value.ToString().Trim() == "") || (dgv.Rows[6].Cells[i].Value.ToString().Trim() == "0"))
            //    return;

            //tradingVolume = Convert.ToDecimal(dgv.Rows[6].Cells[i].Value);

            ////IF(COUNT(G24)=1,F24/F13,"")
            //if ((dgv.Rows[17].Cells[i].Value != null) && (dgv.Rows[17].Cells[i].Value.ToString().Trim() != ""))
            //{
            //    decimal decCostRate = 0;
            //    string strCostRate = "";
            //    cumulativeMC = Convert.ToDecimal(dgv.Rows[17].Cells[i].Value);
            //    //F24/F13,""
            //    decCostRate = (cumulativeMC / tradingVolume);
            //    strCostRate = decCostRate.ToString("P");
            //    dgv.Rows[18].Cells[i].Value = strCostRate.Replace(",", "");
            //}

            Calculation calc = new Calculation();
            // 原価算出
            dgv.Rows[18].Cells[i].Value = calc.CostRate(dgv, i, 6, 17);
            // Wakamatsu
        }

        // Wakamatsu
        /// <summary>
        /// データグリッドビューマスク処理
        /// </summary>
        private void ClosingDateMask()
        {
            int DisplyLimit = Array.IndexOf(monthArray, l_ClosingDate);

            for (int i = 0; i < monthArray.Length; i++)
            {
                if (i <= DisplyLimit)
                {
                    SetCumulative(l_SetGridVew, i);        //受注累計

                    for (int j = 0; j < l_SetGridVew.Rows.Count; j++)
                        // 値の移動
                        this.dgvOutPut.Rows[j].Cells[i].Value = l_SetGridVew.Rows[j].Cells[i].Value;
                }
            }
        }

        private bool checkCellValue(DataGridView dgv, int idx)
        {
            int[] rIdxArray = new int[] { 0, 2, 3, 4, 8, 11, 16 };
            for (int i = 0; i < rIdxArray.Length; i++)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[rIdxArray[i]].Cells[idx].Value))) return false;
            }

            return true;
        }
        // Wakamatsu

        // Wakamatsu 20170308
        /// <summary>
        /// "-" → "△"変換
        /// </summary>
        /// <param name="TargetValue">対象値</param>
        /// <returns>変換結果</returns>
        private string MinusConvert(object TargetValue)
        {
            decimal WorkDecimal = 0;
            string WorkString = Convert.ToString(TargetValue);

            if (WorkString != "")
            {
                // "-" → "△"コンバート
                Decimal.TryParse(WorkString, out WorkDecimal);
                if (WorkDecimal < 0)
                    return "△" + (WorkDecimal * -1).ToString("#,0");
                else
                    return WorkDecimal.ToString("#,0");
            }
            return "";
        }

        /// <summary>
        /// "△" → "-"変換
        /// </summary>
        /// <param name="TargetValue">対象値</param>
        /// <returns>変換結果</returns>
        private decimal SignConvert(object TargetValue)
        {
            decimal WorkDecimal = 0;
            string WorkString = Convert.ToString(TargetValue);

            if (WorkString != "")
            {
                // "△" → "-"コンバート
                if (WorkString.Substring(0, 1) == "△")
                {
                    Decimal.TryParse(WorkString.Substring(1), out WorkDecimal);
                    return WorkDecimal * -1;
                }
                else
                {
                    Decimal.TryParse(WorkString, out WorkDecimal);
                    return WorkDecimal;
                }
            }
            return 0;
        }
        // Wakamatsu 20170308

        /// <summary>
        /// 終了ボタン押下時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnd_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}