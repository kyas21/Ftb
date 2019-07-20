//-----------------------------------------------------------------------------
//
// 総括表の表示と印刷
//
// Note:
//      20190126    asakawa 
//                  完全完了で今年度実績のないものは前年度ど計算から除外するよう変更
//
//      20190203    asakawa
//                  全請求高金額。未成業務受入金算出
//
//      20190210    asakawa 2019 
//                  1/26変更を修正
//                  三か所変更
//
//      20190216    asakawa 20190216
//                  残請求高金額、未成業務受入金算出　再度修正
//                  三か所変更
// 
//      20190310    asakawa 2019
//                  原価をD_CostReportから算出するように変更
//                  3か所追加
//
//      20190315    asakawa 20190326
//                  すべての原価をD_CostReportから算出するように変更
//                  本社以外は部門コードを無視してサマリーするように変更
//                  ３か所コメントアウトと変更
//                  １か所追加
//
//-----------------------------------------------------------------------------

using ClassLibrary;
using System;
using System.Drawing;
using System.Windows.Forms;
using PrintOut;
using System.Data;


namespace VolumeProc
{
    public partial class FormReview :Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        HumanProperty hp;
        const int ColumnsCount = 13;

        private bool iniPro = true;

        private int iniRCnt = 19;               // 19行目は高さ調整のための行（未使用）
        private int[] readOnlyRows = new int[] { 1, 5, 6, 7, 9, 11, 13, 15, 16, 18 };
        private int[] readOnlyRowsW = new int[] { 0, 2, 3, 4, 8, 10, 12, 14, 17 };
        private int[] readOnlyCells = new int[] { 0 };
        private int[] monthArray = new int[] { 99, 7, 8, 9, 10, 11, 12, 1, 2, 3, 4, 5, 6 };

        private string[] OfficeArray;
        const string excelFile = "総括表.xlsx";
        private int nowFY;
        private int limCol = 0;

        private string YearPeriod = "";        // 年度情報格納用
        private int[] AIdxArray = new int[] { 1, 6 };                       // 確認行配列(残業務高)
        private int[] BIdxArray = new int[] { 6, 9 };                       // 確認行配列(未成業務受入金(請求 - 入金))
        private int[] CIdxArray = new int[] { 0, 2, 3, 4, 8, 10, 14 };      // 確認行配列(受注累計、出来高月計、原価累計)
        private int[] DIdxArray = new int[] { 5 };                          // 確認行配列(出来高累計)
        private int[] EIdxArray = new int[] { 8 };                          // 確認行配列(請求累計)
        private int[] FIdxArray = new int[] { 9 };                          // 確認行配列(入金累計)

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormReview()
        {
            InitializeComponent();
        }

        public FormReview( HumanProperty hp )
        {
            InitializeComponent();
            this.hp = hp;
        }

        public FormReview( HumanProperty hp, string SetYear )
        {
            InitializeComponent();
            this.hp = hp;
            YearPeriod = SetYear;
        }
        //----------------------------------------------------------------------------//
        //     Property                                                               //
        //----------------------------------------------------------------------------//

        //----------------------------------------------------------------------------//
        //     Method                                                                 //
        //----------------------------------------------------------------------------//
        private void FormReview_Load( object sender, EventArgs e )
        {
            UiHandling uih = new UiHandling( dataGridView1 );
            uih.DgvReadyNoRHeader();
            //並び替えができないようにする
            uih.NoSortable();

            uih.DgvColumnsWidth( 85 );
            uih.DgvColumnsWidth( 0, 90 );

            dataGridView1.Rows.Add( iniRCnt );
            uih.DgvRowsHeight( 26 );
            uih.DgvRowsReadOnly( readOnlyRows, Color.PaleGreen );
            uih.DgvRowsReadOnly( readOnlyRowsW, Color.White );
            uih.DgvColumnsReadOnly( readOnlyCells, Color.PaleGreen );

            create_cbOffice();
            create_cbDepart();
            create_cbYM();
            setColumnLimitter();

            setViewData( dataGridView1 );
        }


        private void FormReview_Shown( object sender, EventArgs e )
        {
            iniPro = false;       // 初期化処理終了
            dataGridView1.CurrentCell = null;
        }


        private void button_Click( object sender, EventArgs e )
        {
            if( iniPro ) return;

            Button btn = ( Button )sender;
            EstPlanOp epo = new EstPlanOp();

            switch( btn.Name )
            {
                case "buttonOWrite":
                    break;
                case "buttonDelete":
                    break;
                case "buttonCancel":
                    this.Close();
                    break;
                case "buttonPrint":
                    PublishReview prev = new PublishReview( Folder.DefaultExcelTemplate( excelFile ), editPublishData( dataGridView1 ) );
                    prev.ExcelFile();
                    break;
                case "buttonClose":
                    this.Close();
                    break;
                default:
                    break;
            }
        }


        private void dataGridView1_KeyDown( object sender, KeyEventArgs e )
        {
            if( iniPro ) return;
            DataGridView dgv = ( DataGridView )sender;
            switch( e.KeyCode )
            {
                case Keys.Right:
                case Keys.Tab:
                    break;
                case Keys.Left:
                    break;
                default:
                    break;
            }
        }


        private void comboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;

            ComboBox cbx = ( ComboBox )sender;
            switch( cbx.Name )
            {
                case "comboBoxFY":
                    setColumnLimitter();
                    break;
                case "comboBoxOffice":
                    create_cbDepart();
                    setColumnLimitter();
                    break;
                case "comboBoxDepart":
                    break;
                default:
                    break;
            }
            setViewData( dataGridView1 );
        }
        //---------------------------------------------------------------------//
        // Subroutine                                                          //
        //---------------------------------------------------------------------//
        // comboBox作成
        // 事業所
        private void create_cbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxOffice );
            cbe.TableData( "M_Office", "OfficeCode", "OfficeName" );

            OfficeArray = new string[cbe.ValueItem.Length];
            Array.Copy( cbe.ValueItem, 0, OfficeArray, 0, OfficeArray.Length );

            if( !String.IsNullOrEmpty( hp.OfficeCode ) )
            {
                comboBoxOffice.SelectedValue = hp.OfficeCode;
                //hp.OfficeCode = "";
            }
        }


        // 部門
        private void create_cbDepart()
        {
            comboBoxDepart.Visible = ( comboBoxOffice.Text == Sign.HQOffice ) ? true : false;

            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxDepart );
            cbe.DepartmentList( ( comboBoxOffice.Text == Sign.HQOffice ) ? "DEPH" : "DEPB", 1 );
            //comboBoxDepart.SelectedIndex = (comboBoxOffice.Text == Sign.HQOffice) ? 1 : 0;

            if( !String.IsNullOrEmpty( hp.Department ) )
            {
                if( hp.OfficeCode == Sign.HQOfficeCode )
                {
                    comboBoxDepart.SelectedValue = ( hp.Department == "0" ) ? "2" : hp.Department;
                }
                else
                {
                    comboBoxDepart.SelectedValue = "8";
                }
            }
        }


        // 会計年度
        private void create_cbYM()
        {
            DateTime dt = DateTime.Today;
            nowFY = dt.FiscalYear();
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxFY );
            cbe.DisplayItem = new string[5];
            cbe.ValueItem = new string[5];
            for( int i = 0; i < 5; i++ )
            {
                cbe.DisplayItem[i] = Convert.ToString( nowFY - i );
                cbe.ValueItem[i] = i.ToString();
            }
            cbe.Basic();
            if( iniPro )
            {
                comboBoxFY.Text = ( YearPeriod == "" ) ? nowFY.ToString() : YearPeriod;
            }
        }


        private void setViewData( DataGridView dgv )
        {
            collectViewData( dgv );
            calculateItems( dgv );
        }


        private void setColumnLimitter()
        {
            for( int i = 0; i < iniRCnt; i++ )
            {
                for( int j = 0; j < ColumnsCount; j++ )
                {
                    dataGridView1.Rows[i].Cells[j].Value = "";
                }
            }

            int limMM;
            switch( Convert.ToString( comboBoxOffice.SelectedValue ) )
            {
                case "K":
                    limMM = hp.CloseKDate.Month;
                    break;
                case "S":
                    limMM = hp.CloseSDate.Month;
                    break;
                case "T":
                    limMM = hp.CloseTDate.Month;
                    break;
                default:
                    limMM = hp.CloseHDate.Month;
                    break;
            }
            limMM = ( limMM == 12 ) ? 1 : limMM + 1;
            limCol = ( Convert.ToInt32( comboBoxFY.Text ) == nowFY ) ? Array.IndexOf( monthArray, limMM ) : monthArray.Length - 1;
        }


        private void collectViewData( DataGridView dgv )
        {
            string officeCode = Convert.ToString( comboBoxOffice.SelectedValue );
            string department = ( comboBoxOffice.Text == Sign.HQOffice ) ? Convert.ToString( comboBoxDepart.SelectedValue ) : "8";

            YearVolumeData volY;
            VolumeData vol;
            int selectYM;
            string selYMRange = "";
            //VolumeData vol2 = null;

            decimal sumVolume = 0M;
            decimal sumVolUncomp = 0M;
            decimal sumVolClaimRem = 0M;
            decimal sumVolClaim = 0M;
            decimal sumClaim = 0M;
            decimal sumVolPaid = 0M;
            decimal sumBalanceClaim = 0M;
            decimal sumBalanceIncom = 0M;
            decimal sumDeposit1 = 0M;
            decimal sumCost = 0M;
            decimal sumDeposit2 = 0M;
            decimal sumCarryOverPlanned = 0M;

            //decimal tempVolume = 0M;
            //decimal tempClaim = 0M;
            //decimal tempBlanceClaim = 0M;

            // asakawa 20190216 - 1 add
            decimal tempVolUncomp = 0M;
            decimal tempVolClaimRem = 0M;
            decimal tempVolClaim = 0M;
            decimal tempClaim = 0M;
            // asakawa 20190216_1 end

            // asakawa 20190327 add
            decimal tempDec;
            // 20190327 end

            // 20190315 asakawa 20160326 add No.4
            string wk_totalCost = "";
            // 20190315 asakawa 20160326 add end

            // asakawa 20190310 add No.3
            string wk_monthlyCost = "";
            // 20190310 end

            for ( int i = 0; i <= limCol; i++ )
            {
                selectYM = ( i < 7 ) ? Convert.ToInt32( comboBoxFY.Text ) : Convert.ToInt32( comboBoxFY.Text ) + 1;
                selectYM = ( i == 0 ) ? selectYM * 100 + 6 : selectYM * 100 + monthArray[i];

                if( i == 0 )
                {
                    // 前年度期間算出
                    int preYear = ( Convert.ToInt32( comboBoxFY.Text ) - 1 );

                    selYMRange += "BETWEEN " + Convert.ToString( preYear * 100 + 7 ) + " ";
                    selYMRange += "AND " + Convert.ToString( ( Convert.ToInt32( comboBoxFY.Text ) ) * 100 + 6 ) + " ";

                    // 完全完了になっているTaskCodeの一覧を作成
                    vol = new VolumeData();
                    string[] compTaskArray = vol.SelectCompleteTascCode( officeCode, department, selYMRange );
                    // 前年度(7月から翌年6月）すべてのデータを累計取得
                    volY = new YearVolumeData();
                    YearVolumeData[] volYA = volY.SelectYearVolume( officeCode, department, preYear );
                    vol = new VolumeData();

                    // 2018.01 asakawa 追加
                    if (compTaskArray != null && volYA != null)
                    {
                    // 追加ここまで

                    for (int j = 0;j< volYA.Length;j++ )
                    {
                        if( checkExclude( volYA[j].TaskCode, compTaskArray ) )
                        {
                                // 20190216 - 2 asakawa
                                /*
                                sumVolume += ( volYA[j].Volume == null ) ? 0 : Convert.ToDecimal( volYA[j].Volume );
                                sumVolUncomp += ( volYA[j].VolUncomp == null ) ? 0 : Convert.ToDecimal( volYA[j].VolUncomp );
                                sumVolClaimRem += ( volYA[j].VolClaimRem == null ) ? 0 : Convert.ToDecimal( volYA[j].VolClaimRem );
                                sumVolClaim += ( volYA[j].VolClaim == null ) ? 0 : Convert.ToDecimal( volYA[j].VolClaim );
                                sumClaim += ( volYA[j].Claim == null ) ? 0 : Convert.ToDecimal( volYA[j].Claim );
                                sumVolPaid += ( volYA[j].VolPaid == null ) ? 0 : Convert.ToDecimal( volYA[j].VolPaid );
                                sumBalanceClaim += ( volYA[j].BalanceClaim == null ) ? 0 : Convert.ToDecimal( volYA[j].BalanceClaim );
                                sumBalanceIncom += ( volYA[j].BalanceIncom == null ) ? 0 : Convert.ToDecimal( volYA[j].BalanceIncom );
                                sumDeposit1 += ( volYA[j].Deposit1 == null ) ? 0 : Convert.ToDecimal( volYA[j].Deposit1 );
                                sumCost += ( volYA[j].Cost == null ) ? 0 : Convert.ToDecimal( volYA[j].Cost );
                                sumDeposit2 += ( volYA[j].Deposit2 == null ) ? 0 : Convert.ToDecimal( volYA[j].Deposit2 );
                                //sumCarryOverPlanned += ( volYA[j].CarryOverPlanned == null ) ? 0 : Convert.ToDecimal( volYA[j].CarryOverPlanned );
                                */
                                sumVolume += (volYA[j].Volume == null) ? 0 : Convert.ToDecimal(volYA[j].Volume);

                                tempVolUncomp = (volYA[j].VolUncomp == null) ? 0 : Convert.ToDecimal(volYA[j].VolUncomp);
                                sumVolUncomp += tempVolUncomp;
                                tempVolClaimRem = (volYA[j].VolClaimRem == null) ? 0 : Convert.ToDecimal(volYA[j].VolClaimRem);
                                sumVolClaimRem += tempVolClaimRem;
                                tempVolClaim = (volYA[j].VolClaim == null) ? 0 : Convert.ToDecimal(volYA[j].VolClaim);
                                sumVolClaim += tempVolClaim;
                                tempClaim = (volYA[j].Claim == null) ? 0 : Convert.ToDecimal(volYA[j].Claim);
                                sumClaim += tempClaim;

                                sumVolPaid += (volYA[j].VolPaid == null) ? 0 : Convert.ToDecimal(volYA[j].VolPaid);


                                // 20190327 asakawa
                                // sumBalanceClaim += ((tempVolUncomp + tempVolClaimRem + tempVolClaim) - tempClaim);
                                tempDec = ((tempVolUncomp + tempVolClaimRem + tempVolClaim) - tempClaim);
                                if ( tempDec > 0 )
                                    sumBalanceClaim += tempDec;

                                // 20190327 end

                                sumBalanceIncom += (volYA[j].BalanceIncom == null) ? 0 : Convert.ToDecimal(volYA[j].BalanceIncom);

                                // 20190327 asakawa
                                // sumDeposit1 += (tempClaim - (tempVolUncomp + tempVolClaimRem + tempVolClaim));
                                tempDec = (tempClaim - (tempVolUncomp + tempVolClaimRem + tempVolClaim));
                                if( tempDec > 0 )
                                    sumDeposit1 += tempDec;
                                // 20190327 end


                                // 20190315 asakawa 20190326 No.1 Comment out
                                // sumCost += (volYA[j].Cost == null) ? 0 : Convert.ToDecimal(volYA[j].Cost);

                                // debug_label.Text = "";
                                sumDeposit2 += (volYA[j].Deposit2 == null) ? 0 : Convert.ToDecimal(volYA[j].Deposit2);
                                // asakawa 20190216 - 2 end
                            }

                            // 20190210 asakawa2019 追加その1
                            if (checkExclude_level2(volYA[j].TaskCode, compTaskArray))
                            {
                                sumCost += (volYA[j].Cost == null) ? 0 : Convert.ToDecimal(volYA[j].Cost);
                            }
                            // 20190210 asakawa2019 追加その1はここまで

                        }

                        // 2018.01 asakawa 追加
                    }
                    // 追加ここまで

                    // 前年度(7月から翌年6月）すべてのデータを累計取得
                    vol = new VolumeData();
                    //vol = vol.SelectSummaryVolume( officeCode, department, selYMRange );

                    // 前年度データの置き換え
                    vol.MonthlyVolume = sumVolume;
                    vol.VolUncomp = sumVolUncomp;
                    vol.VolClaimRem = sumVolClaimRem;
                    vol.VolClaim = sumVolClaim;
                    vol.MonthlyClaim = sumClaim;
                    vol.VolPaid = sumVolPaid;
                    vol.BalanceClaim = sumBalanceClaim;
                    vol.BalanceIncom = sumBalanceIncom;
                    vol.Deposit1 = sumDeposit1;

                    // 20190315 asakawa 20190326 No.2 add and commentout
                    wk_totalCost = calculateCost(officeCode, department, preYear * 100);
                    if (wk_totalCost != null)
                    {
                        vol.MonthlyCost = Convert.ToDecimal(wk_totalCost);
                    }
                    // vol.MonthlyCost = sumCost;
                    // 20190315 asakawa 20190326 add end

                    vol.Deposit2 = sumDeposit2;
                    //vol.CarryOverPlanned = sumCarryOverPlanned;

                    // 20190203 asakawa add
                    
                    if (vol.BalanceClaim == 0)
                    {
                        vol.BalanceClaim = (sumVolUncomp + sumVolClaimRem + sumVolClaim) - sumClaim;
                    }
                    if (vol.Deposit1 == 0)
                    {
                        vol.Deposit1 = sumClaim - (sumVolUncomp + sumVolClaimRem + sumVolClaim);
                    }
                    
                    // 20190203 asakawa end

                    //// 年度末のデータを取得
                    //vol2 = new VolumeData();
                    //vol2 = vol2.SelectSummaryVolume( officeCode, department, selectYM );

                    //// 未成業務受入金は年度末の値を使用する
                    ////vol.Deposit1 = vol2.Deposit1;
                    //vol.Deposit2 = vol2.Deposit2;
                    sumVolume = 0M;
                    sumClaim = 0M;
                    sumBalanceClaim = 0M;

                    // 20190216 - 3 asakawa add
                    sumVolUncomp = 0M;
                    sumVolClaimRem = 0M;
                    sumVolClaim = 0M;
                    sumVolPaid = 0M;
                    sumBalanceIncom = 0M;
                    sumDeposit1 = 0M;
                    sumCost = 0M;
                    sumDeposit2 = 0M;
                    sumCarryOverPlanned = 0M;
                    // 20190216 - 3 asakawa end
                }
                else
                {
                    vol = new VolumeData();
                    vol = vol.SelectSummaryVolume( officeCode, department, selectYM );

                    // asakawa 20190310 No.1 add
                    // CostReportから該当部門、年月の原価計を算出しVolumeの当月原価を上書きする。
                    wk_monthlyCost = calculateMonthlyCost(officeCode, department, selectYM);
                    if (wk_monthlyCost != null)
                    {
                        vol.MonthlyCost = Convert.ToDecimal(wk_monthlyCost);
                    }
                    // Anonymous 20190310 end

                }

                if ( i <= limCol )
                {
                    //  0 受注単月金額
                    if( vol.MonthlyVolume != null )
                        dgv.Rows[0].Cells[i].Value = DecimalEmpty( vol.MonthlyVolume );

                    //  2 出来高未成業務金額
                    if( vol.VolUncomp != null )
                        dgv.Rows[2].Cells[i].Value = DecimalEmpty( vol.VolUncomp );

                    //  3 出来高未請求金額
                    if( vol.VolClaimRem != null )
                        dgv.Rows[3].Cells[i].Value = DecimalEmpty( vol.VolClaimRem );

                    //  4 出来高請求金額
                    if( vol.VolClaim != null )
                        dgv.Rows[4].Cells[i].Value = DecimalEmpty( vol.VolClaim );

                    //  8 請求単月金額 
                    if( vol.MonthlyClaim != null )
                        dgv.Rows[8].Cells[i].Value = DecimalEmpty( vol.MonthlyClaim );

                    // 10 入金単月金額
                    if( vol.VolPaid != null )
                        dgv.Rows[10].Cells[i].Value = DecimalEmpty( vol.VolPaid );

                    // 12 残請求高金額
                    if( vol.BalanceClaim != null )
                        dgv.Rows[12].Cells[i].Value = MinusConvert( Convert.ToDecimal( vol.BalanceClaim ) );

                    // 13 未成業務受入金
                    if( vol.Deposit1 != null )
                        dgv.Rows[13].Cells[i].Value = MinusConvert( Convert.ToDecimal( vol.Deposit1 ) );

                    // 14 原価単月金額
                    // 20190327 asakawa
                    // if( vol.MonthlyCost != null )
                    //     dgv.Rows[14].Cells[i].Value = DecimalEmpty( vol.MonthlyCost );
                    if (vol.MonthlyCost != null)
                    {
                        int nowYear = (Convert.ToInt32(comboBoxFY.Text));
                        if (i > 0 && i < 7)
                            dgv.Rows[14].Cells[i].Value = DecimalEmpty(decimal.Parse(calculateCost(officeCode, department, nowYear * 100 + i + 6)));
                        else if (i >= 7)
                            dgv.Rows[14].Cells[i].Value = DecimalEmpty(decimal.Parse(calculateCost(officeCode, department, (nowYear + 1) * 100 + i + 6 - 12)));
                        else if (i == 0)
                            dgv.Rows[14].Cells[i].Value = DecimalEmpty(decimal.Parse(calculateCost(officeCode, department, (nowYear - 1) * 100)));
                    }
                    // 20190327 end

                    // 17 未収入金額
                    if ( vol.BalanceIncom != null )
                        dgv.Rows[17].Cells[i].Value = MinusConvert( Convert.ToDecimal( vol.BalanceIncom ) );

                    // 18 未成業務受入金
                    if( vol.Deposit2 != null )
                        dgv.Rows[18].Cells[i].Value = MinusConvert( Convert.ToDecimal( vol.Deposit2 ) );

                    if( i > 0 )
                    {
                        if( vol.MonthlyVolume != null ) sumVolume += ( decimal )vol.MonthlyVolume;
                        //sumCarryOverPlanned += ( decimal )vol.VolUncomp + ( decimal )vol.VolClaimRem + ( decimal )vol.VolClaim;
                        if( vol.VolUncomp != null ) sumClaim += ( decimal )vol.VolUncomp;
                        if( vol.VolClaimRem != null ) sumClaim += ( decimal )vol.VolClaimRem;
                        if( vol.MonthlyClaim != null ) sumClaim += ( decimal )vol.MonthlyClaim;
                        if( vol.BalanceClaim != null ) sumBalanceClaim += ( decimal) vol.BalanceClaim;
                        if( i == limCol && vol.CarryOverPlanned != null)sumCarryOverPlanned = ( decimal )vol.CarryOverPlanned;
                    }
                }
            }
            labelCarryOverPlanned.Text = MinusConvert( ( decimal )sumCarryOverPlanned );
            if (sumBalanceClaim > 0 )
            {
                labelYearComp.Text = MinusConvert( sumVolume - DecimalMask( labelCarryOverPlanned.Text ) );
            }
            else
            {
                labelYearComp.Text = MinusConvert( sumClaim - DecimalMask( labelCarryOverPlanned.Text ) );
            }
            lastYearCalculate( dgv );
        }


        private void calculateItems( DataGridView dgv )
        {
            decimal sumOrder = 0M;      // 受注累計
            decimal mSumVol = 0M;
            decimal sumVolume = 0M;     // 出来高累計
            decimal sumClaim = 0M;      // 請求累計
            decimal sumReceive = 0M;    // 入金累計
            decimal sumCost = 0M;       // 原価累計
            decimal sumOrderL = 0M;     // 受注累計(前年度から)
            decimal sumVolumeL = 0M;    // 出来高累計(前年度から)

            for( int i = 1; i <= limCol; i++ )
            {
                Calculation calc = new Calculation();

                //  1 受注-累計
                dgv.Rows[1].Cells[i].Value = calc.Cumulative( dgv, i, 0, sumOrder, checkCellValue( dgv, i, CIdxArray ), "#,0", out sumOrder );
                sumOrderL = DecimalMask( dgv.Rows[1].Cells[0].Value ) + sumOrder;

                mSumVol = 0M;
                for( int j = 2; j < 5; j++ )
                {
                    if( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[j].Cells[i].Value ) ) )
                        mSumVol += DecimalMask( dgv.Rows[j].Cells[i].Value );
                }

                //  5 出来高-単月-月計
                if( !checkCellValue( dgv, i, CIdxArray ) )
                    dgv.Rows[5].Cells[i].Value = MinusConvert( mSumVol );
                else
                    dgv.Rows[5].Cells[i].Value = "";

                //  6 出来高-累計
                dgv.Rows[6].Cells[i].Value = calc.TotalTradingVolume( dgv, i, 5, 2, 3, 4, sumVolume, "#,0", out sumVolume );
                sumVolumeL = DecimalMask( dgv.Rows[6].Cells[0].Value ) + sumVolume;

                //  7 残業務高
                if( checkCellValue( dgv, i, AIdxArray ) == false )
                    dgv.Rows[7].Cells[i].Value = MinusConvert( calc.SubtrahendVol( dgv, i, 0, 2, 3, 4 ) );
                else
                    dgv.Rows[7].Cells[i].Value = "";

                //  9 請求-累計
                if( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[8].Cells[i].Value ) ) )
                    dgv.Rows[9].Cells[i].Value = calc.Cumulative( dgv, i, 8, sumClaim, checkCellValue( dgv, i, CIdxArray ), "#,0", out sumClaim );
                else
                    dgv.Rows[9].Cells[i].Value = "";

                // 11 入金-累計
                if( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[10].Cells[i].Value ) ) )
                    dgv.Rows[11].Cells[i].Value = calc.Cumulative( dgv, i, 10, sumReceive, checkCellValue( dgv, i, CIdxArray ), "#,0", out sumReceive );
                else
                    dgv.Rows[11].Cells[i].Value = "";

                if( DecimalMask( dgv.Rows[7].Cells[i].Value ) + DecimalMask( dgv.Rows[10].Cells[i].Value ) != 0 )
                {
                    // 12 残請求高
                    if( DecimalMask( dgv.Rows[12].Cells[i].Value ) == 0 ) dgv.Rows[12].Cells[i].Value = "";

                    // 13 未成業務受入金
                    if( DecimalMask( dgv.Rows[13].Cells[i].Value ) == 0 ) dgv.Rows[13].Cells[i].Value = "";
                }
                else
                {
                    dgv.Rows[12].Cells[i].Value = "";
                    dgv.Rows[13].Cells[i].Value = "";
                }

                // 15 原価-累計
                dgv.Rows[15].Cells[i].Value = calc.Cumulative( dgv, i, 14, sumCost, checkCellValue( dgv, i, CIdxArray ), "#,0", out sumCost );

                // 16 原価-原価率
                // 20190214 asakawa 小数点下１桁に変更（小数点下２桁で四捨五入）
                // dgv.Rows[16].Cells[i].Value = ( sumVolume == 0 ) ? "" : decPointFormat( sumCost / sumVolume * 100 ) + "%";
                dgv.Rows[16].Cells[i].Value = (sumVolume == 0) ? "" : decPointFormat1(sumCost / sumVolume * 100) + "%";

                if( DecimalMask( dgv.Rows[7].Cells[i].Value ) + DecimalMask( dgv.Rows[10].Cells[i].Value ) != 0 )
                {
                    // 17 未収入金
                    if( DecimalMask( dgv.Rows[17].Cells[i].Value ) == 0 ) dgv.Rows[17].Cells[i].Value = "";

                    // 18 未成業務受入金
                    if( DecimalMask( dgv.Rows[18].Cells[i].Value ) == 0 ) dgv.Rows[18].Cells[i].Value = "";
                }
                else
                {
                    dgv.Rows[17].Cells[i].Value = "";
                    dgv.Rows[18].Cells[i].Value = "";
                }
            }
        }


        private void lastYearCalculate( DataGridView dgv )
        {
            //  1 受注-累計
            if( checkCellValue( dgv, 0, CIdxArray ) == false )
                dgv.Rows[1].Cells[0].Value = MinusConvert( DecimalMask( dgv.Rows[0].Cells[0].Value ) );
            else
                dgv.Rows[1].Cells[0].Value = "";

            //  5 出来高-単月-月計
            if( checkCellValue( dgv, 0, CIdxArray ) == false )
                dgv.Rows[5].Cells[0].Value = MinusConvert( DecimalMask( dgv.Rows[2].Cells[0].Value )
                                                            + DecimalMask( dgv.Rows[3].Cells[0].Value )
                                                            + DecimalMask( dgv.Rows[4].Cells[0].Value ) );
            else
                dgv.Rows[5].Cells[0].Value = "";

            //  6 出来高-単月-累計
            if( checkCellValue( dgv, 0, DIdxArray ) == false )
                dgv.Rows[6].Cells[0].Value = MinusConvert( DecimalMask( dgv.Rows[5].Cells[0].Value ) );
            else
                dgv.Rows[6].Cells[0].Value = "";

            //  7 残業務高（受注累計 － 出来高-単月-累計）
            if( checkCellValue( dgv, 0, AIdxArray ) == false )
                dgv.Rows[7].Cells[0].Value = MinusConvert( DecimalMask( dgv.Rows[1].Cells[0].Value )
                                                            - DecimalMask( dgv.Rows[6].Cells[0].Value ) );
            else
                dgv.Rows[7].Cells[0].Value = "";

            //  9 請求-累計
            if( checkCellValue( dgv, 0, EIdxArray ) == false )
                dgv.Rows[9].Cells[0].Value = MinusConvert( DecimalMask( dgv.Rows[8].Cells[0].Value ) );
            else
                dgv.Rows[9].Cells[0].Value = "";

            // 11 入金-累計
            if( checkCellValue( dgv, 0, FIdxArray ) == false )
                dgv.Rows[11].Cells[0].Value = MinusConvert( DecimalMask( dgv.Rows[10].Cells[0].Value ) );
            else
                dgv.Rows[11].Cells[0].Value = "";

            if( DecimalMask( dgv.Rows[7].Cells[0].Value ) + DecimalMask( dgv.Rows[10].Cells[0].Value ) != 0 )
            {
                // 12 残請求高金額
                if( DecimalMask( dgv.Rows[12].Cells[0].Value ) == 0 ) dgv.Rows[12].Cells[0].Value = "";

                // 13 未成業務受入金（出来高-単月-請求 － 入金単月） 出来高単月累計または請求累計に値があるときのみ処理
                if( DecimalMask( dgv.Rows[13].Cells[0].Value ) == 0 ) dgv.Rows[13].Cells[0].Value = "";
            }
            else
            {
                dgv.Rows[12].Cells[0].Value = "";
                dgv.Rows[13].Cells[0].Value = "";
            }

            // 15 原価-累計
            if( checkCellValue( dgv, 0, CIdxArray ) == false )
                dgv.Rows[15].Cells[0].Value = MinusConvert( DecimalMask( dgv.Rows[14].Cells[0].Value ) );
            else
                dgv.Rows[15].Cells[0].Value = "";

            // 16 原価-原価率
            if( Convert.ToString( dgv.Rows[15].Cells[0].Value ) != "" && Convert.ToString( dgv.Rows[6].Cells[0].Value ) != "" )
            {
                if( DecimalMask( dgv.Rows[6].Cells[0].Value ) != 0 )
                {
                    // 20190214 asakawa 小数点下１桁に変更（小数点下２桁で四捨五入）
                    /*
                    dgv.Rows[16].Cells[0].Value = decPointFormat( DecimalMask( dgv.Rows[15].Cells[0].Value )
                                                                            / DecimalMask( dgv.Rows[6].Cells[0].Value ) * 100 ) + "%";
                    */
                    dgv.Rows[16].Cells[0].Value = decPointFormat1(DecimalMask(dgv.Rows[15].Cells[0].Value)
                                                                            / DecimalMask(dgv.Rows[6].Cells[0].Value) * 100) + "%";
                }
                else
                    dgv.Rows[16].Cells[0].Value = "";
            }
            else
                dgv.Rows[16].Cells[0].Value = "";

            if( DecimalMask( dgv.Rows[7].Cells[0].Value ) + DecimalMask( dgv.Rows[10].Cells[0].Value ) != 0 )
            {
                // 17 未収入金
                if( DecimalMask( dgv.Rows[17].Cells[0].Value ) == 0 ) dgv.Rows[17].Cells[0].Value = "";

                // 18 未成業務受入金
                if( DecimalMask( dgv.Rows[18].Cells[0].Value ) == 0 ) dgv.Rows[18].Cells[0].Value = "";
            }
            else
            {
                dgv.Rows[17].Cells[0].Value = "";
                dgv.Rows[18].Cells[0].Value = "";
            }
        }


        private string[,] editPublishData( DataGridView dgv )
        {
            string[,] pubDat = new string[12, 13];
            //for (int i = 0; i < ColumnsCount; i++)
            for( int i = 0; i < limCol + 1; i++ )
            {
                pubDat[0, i] = SignConvert( dgv.Rows[0].Cells[i].Value );
                pubDat[1, i] = SignConvert( dgv.Rows[2].Cells[i].Value );
                pubDat[2, i] = SignConvert( dgv.Rows[3].Cells[i].Value );
                pubDat[3, i] = SignConvert( dgv.Rows[4].Cells[i].Value );
                pubDat[4, i] = SignConvert( dgv.Rows[8].Cells[i].Value );
                pubDat[5, i] = SignConvert( dgv.Rows[10].Cells[i].Value );
                pubDat[6, i] = SignConvert( dgv.Rows[12].Cells[i].Value );
                pubDat[7, i] = SignConvert( dgv.Rows[13].Cells[i].Value );
                pubDat[8, i] = SignConvert( dgv.Rows[14].Cells[i].Value );
                pubDat[9, i] = SignConvert( dgv.Rows[17].Cells[i].Value );
                pubDat[10, i] = SignConvert( dgv.Rows[18].Cells[i].Value );
            }
            pubDat[11, 0] = comboBoxFY.Text;
            pubDat[11, 1] = comboBoxOffice.Text;
            pubDat[11, 2] = comboBoxDepart.Text;
            pubDat[11, 3] = SignConvert( labelCarryOverPlanned.Text );
            pubDat[11, 4] = SignConvert( labelYearComp.Text );

            pubDat[11, 5] = Convert.ToString( limCol + 1 );

            return pubDat;
        }


        private bool checkCellValue( DataGridView dgv, int idx, int[] rIdxArray )
        {
            for( int i = 0; i < rIdxArray.Length; i++ )
            {
                if( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[rIdxArray[i]].Cells[idx].Value ) ) ) return false;
            }

            return true;
        }


        private static string decFormat( decimal decNum )
        {
            return DHandling.DecimaltoStr( decNum, "#,0" );
        }


        private static string decPointFormat( decimal decNum )
        {
            return DHandling.DecimaltoStr( decNum, "0.00" );
        }

        // 20190214 asakawa 関数追加
        private static string decPointFormat1(decimal decNum)
        {
            return DHandling.DecimaltoStr(decNum, "0.0");
        }


        private string DecimalEmpty( decimal? TargetValue )
        {
            string WorkString = Convert.ToString( TargetValue );      // 対象格納用
            decimal WorkDecimal = 0;                                // 変換結果格納用

            if( WorkString == "" ) return "";
            if( decimal.TryParse( WorkString, out WorkDecimal ) == true )
            {
                if( WorkDecimal == 0 )
                    return "";
                else
                {
                    if( WorkDecimal < 0 )
                        return "△" + ( WorkDecimal * -1 ).ToString( "#,0" );
                    else
                        return WorkDecimal.ToString( "#,0" );
                }
            }
            else
                return "";
        }


        private decimal DecimalMask( object TargetValue )
        {
            decimal WorkDecimal = 0;                // 変換結果格納用
            string WorkString = Convert.ToString( TargetValue );

            if( WorkString != "" )
            {
                // "△" → "-"コンバート
                if( WorkString.Substring( 0, 1 ) == "△" )
                {
                    Decimal.TryParse( WorkString.Substring( 1 ), out WorkDecimal );
                    return WorkDecimal * -1;
                }
                else
                {
                    Decimal.TryParse( WorkString, out WorkDecimal );
                    return WorkDecimal;
                }
            }
            else
                return WorkDecimal;
        }


        private string MinusConvert( object TargetValue )
        {
            decimal WorkDecimal = 0;
            string WorkString = Convert.ToString( TargetValue );

            if( WorkString != "" )
            {
                // "-" → "△"コンバート
                Decimal.TryParse( WorkString, out WorkDecimal );
                if( WorkDecimal < 0 )
                    return "△" + ( WorkDecimal * -1 ).ToString( "#,0" );
                else
                    return WorkDecimal.ToString( "#,0" );
            }
            return WorkDecimal.ToString( "#,0" );
        }

        private string SignConvert( object TargetValue )
        {
            decimal WorkDecimal = 0;
            string WorkString = Convert.ToString( TargetValue );

            if( WorkString != "" )
            {
                // "△" → "-"コンバート
                if( WorkString.Substring( 0, 1 ) == "△" )
                {
                    Decimal.TryParse( WorkString.Substring( 1 ), out WorkDecimal );
                    return Convert.ToString( WorkDecimal * -1 );
                }
                else
                {
                    Decimal.TryParse( WorkString, out WorkDecimal );
                    return Convert.ToString( WorkDecimal );
                }
            }
            return "";
        }

        /// Anonymous2019 20190126 comment out
        /// 既存のcheckExcludeをコメントアウトし、新規を作成
        /// /// 20190210 Anonymous2019 追加その2、復活
        /// <summary>
        /// 完全完了しているか否かを完全完了タスクコード一覧で確認。完全完了したものは処理対象外（false）とする
        /// </summary>                                         
        /// <param name="taskCode">確認対象タスクコード</param>
        /// <param name="compTaskArray">ステータスが完全完了になっているタスクコード一覧</param>
        /// <returns>真（完全完了以外）偽（完全完了したもの）</returns>

        private bool checkExclude(string taskCode, string[] compTaskArray )
        {
            for(int i = 0;i<compTaskArray.Length;i++ )
            {
                if( taskCode == compTaskArray[i] ) return false;
            }
            return true;
        }

        /// <summary>
        /// asakawa2019 20190126 add
        /// 20190210 asakawa2019 追加その3、名前をcheckExclude→checkExclude_level2に変更
        /// 完全完了AND今年度（指定年度）実績なしとなっているものは処理の除外対象とする
        /// </summary>                                         
        /// <param name="taskCode">確認対象タスクコード</param>
        /// <param name="compTaskArray">ステータスが完全完了になっているタスクコード一覧</param>
        /// <returns>真（完全完了以外&今年度実績有）偽（完全完了したもので今年度実績無）</returns>

        private bool checkExclude_level2(string taskCode, string[] compTaskArray)
        {
            string strSql = "* FROM D_YearVolume WHERE TaskCode = '" + taskCode + "' AND YearMonth > "
                            + Convert.ToString(Convert.ToInt32(comboBoxFY.Text) - 1);
            //string strSql = "* FROM D_Volume WHERE TaskCode = '" + taskCode + "' AND YearMonth > " 
            //                  + Convert.ToString( Convert.ToInt32( comboBoxFY.Text) * 100 + 6 );
            SqlHandling sh = new SqlHandling();
            for (int i = 0; i < compTaskArray.Length; i++)
            {
                if (taskCode == compTaskArray[i])
                {
                    DataTable dt = sh.SelectFullDescription(strSql);
                    if (dt == null) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// asakawa 20190310 No.2 add
        /// CostReportから該当部門、年月の原価計を算出する。
        /// </summary>    
        /// <param name="officeCode">事業所コード</param>
        /// <param name="department">部門コード</param>
        /// <param name="selectYM">対象年月</param>
        /// <returns>原価計</returns>
        private string calculateMonthlyCost(string officeCode, string department, int selectYM)
        {
            SqlHandling sh = new SqlHandling();

            int selY = selectYM / 100;
            int selM = selectYM - (selY * 100);
            string strDate = Convert.ToString(selY) + "-" + Convert.ToString(selM) + "-%";

            string sqlStr = " SUM(Cost) AS CostReportTotalMony FROM D_CostReport WHERE "
                    + "OfficeCode = " + "'" + officeCode + "' AND "
                    + "Department = " + "'" + department + "' AND "
                    + "ReportDate LIKE '" + strDate + "'";
            DataTable dt = sh.SelectFullDescription(sqlStr);
            if (dt == null) return null;
            DataRow dr = dt.Rows[0];
            if (dr["CostReportTotalMony"] != null && dr["CostReportTotalMony"] != DBNull.Value)
            {
                return Convert.ToString(dr["CostReportTotalMony"]);

            }
            else
            {
                return null;
            }


        }

        /// 20190315 asakawa 20190326 No.3 update
        /// CostReportから該当部門、年月の原価計を算出する。
        /// 月が00のものは年計とする
        /// 本社以外は部門コードを無視する
        /// </summary>    
        /// <param name="officeCode">事業所コード</param>
        /// <param name="department">部門コード</param>
        /// <param name="selectYM">対象年月</param>
        /// <returns>原価計</returns>
        private string calculateCost(string officeCode, string department, int selectYM)
        {
            SqlHandling sh = new SqlHandling();

            int selY = selectYM / 100;
            int selM = selectYM - (selY * 100);
            string strDate;

            string SqlStr2 = " sum(Cost) as TotalCost2 from D_YearVolume where YearMonth = 2014";
            SqlStr2 += " and OfficeCode = '";
            SqlStr2 += officeCode;
            SqlStr2 += "'";


            // 年計（月が0）か月計か
            if (selM == 0)
            {
                // 20190327 asakawa
                // strDate = "ReportDate BETWEEN '" + Convert.ToString(selY) + "-07-01' AND '" + Convert.ToString(selY + 1) + "-06-30'";
                string ds;
                ds = Convert.ToString(selY + 1) + "-06-30";

                // strDate = "ReportDate <= '" + Convert.ToString(selY + 1) + "-06-30'";
                // strDate += " and TaskCode not in ( select distinct TaskCode from D_Volume where TaskStat = 3 and TaskCode not in ( select distinct TaskCode from D_CostReport where ReportDate > '";
                // strDate += ds;
                // strDate +=  "' ) )";

                strDate = "ReportDate <= '" + Convert.ToString(selY + 1) + "-06-30'";
                strDate += " and TaskCode not in ( select distinct TaskCode from D_Volume where TaskStat = 3 and TaskCode not in ( select distinct TaskCode from D_Volume where YearMonth > ";
                strDate += Convert.ToString((selY+1)*100+6);
                strDate += " ) )";

                // strDate = "ReportDate <= '" + Convert.ToString(selY + 1) + "-06-30'";
                // strDate += " and TaskCode in ( select distinct TaskCode from D_Volume where  YearMonth > ";
                // strDate += Convert.ToString((selY + 1) * 100 + 6);
                // strDate += " )";

                // strDate = "ReportDate <= '" + Convert.ToString(selY + 1) + "-06-30'";
                // strDate += " and TaskCode in ( select distinct TaskCode from D_Volume where  YearMonth > ";
                // strDate += Convert.ToString((selY + 1) * 100 + 6);
                // strDate += " )";

                /*
                strDate = "ReportDate <= '" + Convert.ToString(selY + 1) + "-06-30' and Reportdate > '2014-06-30'";
                strDate += " and TaskCode in ( select distinct TaskCode from D_Volume where  YearMonth > ";
                strDate += Convert.ToString((selY + 1) * 100 + 6);
                strDate += " )";
                */

                SqlStr2 += " and TaskCode in ( select distinct TaskCode from D_Volume where  YearMonth > ";
                SqlStr2 += Convert.ToString((selY + 1) * 100 + 6);
                SqlStr2 += " )";

                // 20190327 end
            }
            else
            {
                // 20190327 asakawa
                // strDate = "ReportDate LIKE '" + Convert.ToString(selY) + "-" + Convert.ToString(selM) + "-%'";
                strDate = "CONVERT(char, ReportDate, 111) LIKE '" + Convert.ToString(selY) + "/" + selM.ToString("00") + "%'";
                // 20190327 end
            }

            string sqlStr = " SUM(Cost) AS TotalCost FROM D_CostReport WHERE "
                    + "OfficeCode = " + "'" + officeCode + "' AND ";

            // 本社か支社か
            if (officeCode == "H")
            {
                sqlStr += ("Department = " + "'" + department + "' AND ");

                SqlStr2 += " and Department = '";
                SqlStr2 += department;
                SqlStr2 += "'";
            }

            sqlStr += strDate;

            DataTable dt = sh.SelectFullDescription(sqlStr);
            if (dt == null) return null;

            DataRow dr = dt.Rows[0];
            if (dr["TotalCost"] != null && dr["TotalCost"] != DBNull.Value)
            {
                if (selM == 0)
                {
                    decimal dec1, dec2;

                    DataTable dt2 = sh.SelectFullDescription(SqlStr2);
                    if(dt2==null)
                    {
                        return null;
                    }
                    DataRow dr2 = dt2.Rows[0];
                    if( dr2["TotalCost2"] != null && dr2["TotalCost2"] != DBNull.Value )
                    {
                        dec1 = (decimal)dr["TotalCost"];
                        dec2 = (decimal)dr2["TotalCost2"];

                        return Convert.ToString(dec1+dec2);
                    }
                    else
                    {
                        return Convert.ToString(dr["TotalCost"]);
                    }
                }
                else
                {
                    return Convert.ToString(dr["TotalCost"]);
                }

            }
            else
            {
                // 20190327 asakawa
                // return null;
                return "0";
                // 20190327 end
            }

        }


    }
}
