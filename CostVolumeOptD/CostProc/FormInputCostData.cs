using System;
using System.Data;
using System.Windows.Forms;
using ClassLibrary;
using ListForm;

namespace CostProc
{
    public partial class FormInputCostData : Form
    {
        //--------------------------------------------------------------------------//
        //     Field                                                                //
        //--------------------------------------------------------------------------//
        private DataGridViewCellStyle defaultCellStyle;
        //TaskWkData td;
        CostReportData[] crd;
        HumanProperty hp;
        CostData[] cmd;
        TaskCodeNameData[] tcd;
        private bool iniPro = true;
        private bool newEntry = false;
        private int iniRCnt = 9;

        private int curSlipNo;
        private int holdSlipNo;

        private int[] sNoArray = new int[] { -1 };
        private int sNoIndex;

        const string HQOffice = "本社";
        const string noPrevMes = "「前データ」はありません。";
        const string noNextMes = "「次データ」はありません。";
        const string noDataMes = "処理できるデータがありません。";
        const string faildProc = "処理に失敗しました。";
        //private string[] OfficeArray;
        private bool allChecked = false;
        //--------------------------------------------------------------------------//
        //     Constructor                                                          //
        //--------------------------------------------------------------------------//
        public FormInputCostData()
        {
            InitializeComponent();
        }

        public FormInputCostData(HumanProperty hp)
        {
            InitializeComponent();
            this.hp = hp;
        }

        //--------------------------------------------------------------------------//
        //     Property                                                             //
        //--------------------------------------------------------------------------//

        //--------------------------------------------------------------------------//
        //     Method                                                               //
        //--------------------------------------------------------------------------//
        private void FormInputCostData_Load(object sender, EventArgs e)
        {
            this.defaultCellStyle = new DataGridViewCellStyle();
            UiHandling uih = new UiHandling(dataGridView1);
            uih.DgvReadyNoRHeader();
            uih.DgvNotSortable(dataGridView1);

            viewModeProperty();
            //reverseProperty();
            initializeScreen();
            initialDisplay();
            calculateAmount();
        }


        private void FormInputCostData_Shown(object sender, EventArgs e)
        {
            iniPro = false;       // 初期化処理終了
        }

        
        private void button_Click(object sender, EventArgs e)
        {
            if (iniPro) return;

            Button btn = (Button)sender;

            labelMessage.Text = "";
            switch (btn.Name)
            {
                case "buttonPrev":
                    if (sNoArray[0] == -1)
                    {
                        while (true)
                        {
                            curSlipNo--;
                            if (curSlipNo < CostReportData.ReadMinSlipNo())
                            {
                                curSlipNo = CostReportData.ReadMinSlipNo();
                                labelMessage.Text = noDataMes;
                                return;
                            }
                            if (dispCostReportData(curSlipNo)) break;
                        }
                    }
                    else
                    {
                        if (sNoIndex == 0)
                        {
                            labelMessage.Text = "絞込んだデータの表示はこれより前にはありません。通常の表示に戻ります。";
                            sNoArray = new int[] { -1 };
                            curSlipNo = holdSlipNo;
                        }
                        else
                        { 
                            labelMessage.Text = "絞込みデータ処理中。";
                            sNoIndex--;
                            curSlipNo = sNoArray[sNoIndex];
                        }
                        dispCostReportData(curSlipNo);
                    }
                    
                    newEntry = false;
                    break;
                case "buttonNext":
                    if (sNoArray[0] == -1)
                    {
                        while (true)
                        {
                            curSlipNo++;
                            if (curSlipNo > CostReportData.ReadNowSlipNo())
                            {
                                curSlipNo = CostReportData.ReadNowSlipNo();
                                labelMessage.Text = noDataMes;
                                return;
                            }
                            if (dispCostReportData(curSlipNo)) break;
                        }
                    }
                    else
                    {
                        if (sNoIndex == sNoArray.Length - 1)
                        {
                            labelMessage.Text = "絞込んだデータの表示はこれより後にはありません。通常の表示に戻ります。";
                            sNoArray = new int[] { -1 };
                            curSlipNo = holdSlipNo;
                        }
                        else
                        {
                            labelMessage.Text = "絞込みデータ処理中。";
                            sNoIndex++;
                            curSlipNo = sNoArray[sNoIndex];
                        }
                        dispCostReportData(curSlipNo);
                    }

                    newEntry = false;
                    break;
                case "buttonNew":
                    initializeScreen();
                    textBoxSlipNo.ReadOnly = true;
                    newModeProperty();
                    //reverseProperty();
                    newEntry = true;
                    break;
                case "buttonSave":
                    int dataCount = procDgvDataCount(dataGridView1);
                    if (textBoxTaskName.Text == "" || dataCount == 0)
                    {
                        MessageBox.Show(noDataMes);
                        return;
                    }
                    storeCostReportData(dataGridView1);
                    //#####################
                    dispCostReportData(curSlipNo);
                    //#####################
                    viewModeProperty();
                    //reverseProperty();
                    newEntry = false;
                    // Wakamatsu 20170307
                    textBoxSlipNo.ReadOnly = false;
                    break;
                case "buttonDelete":
                    if (newEntry) return;
                    //deleteCostReportData(dataGridView1, Convert.ToInt32(textBoxSlipNo.Text));
                    deleteCostReportData(dataGridView1, curSlipNo);
                    initializeScreen();
                    initialDisplay();
                    break;
                case "buttonCancel":
                    initializeScreen();
                    dispCostReportData(curSlipNo);
                    if (newEntry)
                    {
                        viewModeProperty();
                        //reverseProperty();
                        newEntry = false;
                    }
                    break;
                case "buttonSearch":
                    createSearchKeyArray();
                    if (sNoArray[0] != -1)
                    {
                        if (dispCostReportData(sNoArray[0])) break;
                    }
                    newEntry = false;
                    break;
                case "buttonEnd":
                    this.Close();
                    break;
                default:
                    break;
            }
            calculateAmount();
        }


        private void comboBox_TextChanged(object sender, EventArgs e)
        {
            if (iniPro) return;

            ComboBox cb = (ComboBox)sender;
            switch (cb.Name)
            {
                case "comboBoxOffice":
                    ListFormDataOp lo = new ListFormDataOp();
                    tcd = lo.SelectTaskCodeNameData(Convert.ToString(comboBoxOffice.SelectedValue));  // Task情報
                    cmd = lo.SelectCostData(Convert.ToString(comboBoxOffice.SelectedValue));          // Cost情報
                    create_cbDepart();
                    break;
                case "comboBoxDepart":
                    break;
                default:
                    break;
            }
        }


        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (iniPro) return;

            TextBox tb = (TextBox)sender;
            //if (tb.Name == "textBoxSlipNo")
            //{
            //    if (textBoxSlipNo.Text == "")
            //    {
            //        DMessage.ValueErrMsg();
            //        return;
            //    }

            //    if (DHandling.IsNumeric(textBoxSlipNo.Text))
            //    {
            //        curSlipNo = Convert.ToInt32(textBoxSlipNo.Text);
            //    }
            //    else
            //    {
            //        DMessage.ValueErrMsg();
            //        return;
            //    }
            //}
        }


        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (iniPro) return;

            TextBox tb = (TextBox)sender;

            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                switch (tb.Name)
                {
                    case "textBoxSlipNo":
                        if (textBoxSlipNo.Text == "")
                        {
                            DMessage.ValueErrMsg();
                            return;
                        }

                        if (!DHandling.IsNumeric(textBoxSlipNo.Text))
                        {
                            DMessage.ValueErrMsg();
                            return;
                        }

                        if (curSlipNo == Convert.ToInt32(textBoxSlipNo.Text)) return;

                        if (!dispCostReportData(Convert.ToInt32(textBoxSlipNo.Text)))
                        {
                            MessageBox.Show("指定された伝票番号のデータはありません");
                            return;
                        }

                        curSlipNo = Convert.ToInt32(textBoxSlipNo.Text);

                        break;
                    case "textBoxTaskCode":
                        if (!dispTaskInformation(textBoxTaskCode.Text))
                        {
                            MessageBox.Show("指定された業務番号のデータはありません");
                        }
                        break;
                    default:
                        break;
                }
            }

            if ((e.Modifiers & Keys.Control) != Keys.Control) return;   // Ctrlキーが押下された時のみ以下処理

            switch (e.KeyCode)
            {
                case Keys.A:
                    chooseTaskCodeNameData();
                    break;
                
                default:
                    break;
            }

        }


        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (iniPro) return;
            CheckBox ckb = (CheckBox)sender;
            dataGridView1.CurrentCell = dataGridView1[1, 0];
            if (ckb.Name == "checkBoxAll") dataGridViewAllChecked(dataGridView1);
        }


        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (iniPro) return;

            DataGridView dgv = (DataGridView)sender;
            dgv.CurrentCell = dgv[1, 0];
            if (e.ColumnIndex == 0) dataGridViewAllChecked(dgv);
        }


        /// <summary>
        /// [Ctrl]と組み合わせたDataGridViewの操作用Short-Cut Key
        /// 前提：コントロールがDataGridViewにある時 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (iniPro) return;

            DataGridView dgv = (DataGridView)sender;
            if ((e.Modifiers & Keys.Control) != Keys.Control) return;

            switch (e.KeyCode)
            {
                case Keys.A:
                    chooseCostMasterData(dgv.CurrentCellAddress.Y);
                    dgv.Rows[dgv.CurrentCellAddress.Y].Cells[1].Style = this.defaultCellStyle;
                    break;
                case Keys.C:
                    Clipboard.SetDataObject(dgv.GetClipboardContent());
                    break;
                case Keys.I:
                case Keys.D:
                    break;
                case Keys.R:
                    calculateAmount();
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// DataGridView Cellの内容に変更があった時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (iniPro) return;   // 初期化中

            DataGridView dgv = (DataGridView)sender;
            switch (e.ColumnIndex)
            {
                case 1:     // 「コード」列
                    dispCostMaster(dgv.Rows[e.RowIndex], Convert.ToString(dgv.Rows[e.RowIndex].Cells["CostCode"].Value), Convert.ToString(comboBoxOffice.SelectedValue));
                    break;
                case 3:     // 「数量」列
                    calculateLine(dgv.Rows[e.RowIndex]);
                    break;
                case 5:     // 「単価」列
                    calculateLine(dgv.Rows[e.RowIndex]);
                    break;
                case 7:     // 「備考」列
                    break;
                default:
                    break;
            }
        }


        private void dateTimePickerEntryDate_ValueChanged(object sender, EventArgs e)
        {

        }
        //--------------------------------------------------------------------------//
        //     SubRoutine                                                           //
        //--------------------------------------------------------------------------//

        private void initializeScreen()
        {
            labelMessage.Text = "";

            textBoxSlipNo.Text = "";
            textBoxSlipNo.ReadOnly = false;

            textBoxTaskCode.Text = "";
            textBoxTaskName.Text = "";
            labelPartnerName.Text = "";
            labelTaskPlace.Text = "";

            labelStartDate.Text = "";
            labelEndDate.Text = "";

            labelLeaderMName.Text = "";
            labelLeaderMCode.Text = "";

            labelSalesMName.Text = "";
            labelSalesMCode.Text = "";

            labelSum.Text = "0";
            labelAmount.Text = "0";
            labelTax.Text = "0";

            labelPartnerCode.Text = "";

            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(iniRCnt);
            //buttonNumbering();

            create_cbOffice();
            //comboBoxOffice.Text = hp.OfficeCode;        // 初期値
            //comboBoxOffice.SelectedIndex = oList.IndexOf(hp.OfficeCode); 
            comboBoxOffice.SelectedIndex = Conv.oList.IndexOf(hp.OfficeCode); 
            create_cbDepart();
            comboBoxDepart.Text = hp.Department;        // 初期値

            dateTimePickerEntryDate.Value = DateTime.Now;


            ListFormDataOp lo = new ListFormDataOp();
            tcd = lo.SelectTaskCodeNameData(hp.OfficeCode);  // Task情報
            cmd = lo.SelectCostData(hp.OfficeCode);          // Cost情報
        }

        private void initialDisplay()
        {
            curSlipNo = CostReportData.ReadNowSlipNo();
            dispCostReportData(curSlipNo);
        }


        private void viewModeProperty()
        {
            buttonPrev.Enabled = true;
            buttonNext.Enabled = true;
            textBoxSlipNo.Enabled = true;
            /*
            comboBoxOffice.Enabled = false;
            comboBoxDepart.Enabled = false;
            dateTimePickerEntryDate.Enabled = false;
            textBoxTaskCode.Enabled = false;
            */
            comboBoxOffice.Enabled = true;
            comboBoxDepart.Enabled = true;
            dateTimePickerEntryDate.Enabled = true;
            textBoxTaskCode.Enabled = true;

            textBoxTaskName.Enabled = false;
        }

        private void newModeProperty()
        {
            buttonPrev.Enabled = false;
            buttonNext.Enabled = false;
            textBoxSlipNo.Enabled = false;
            comboBoxOffice.Enabled = true;
            comboBoxDepart.Enabled = true;
            dateTimePickerEntryDate.Enabled = true;
            textBoxTaskCode.Enabled = true;
            textBoxTaskName.Enabled = true;
        }


        // comboBox作成
        // 事業所
        private void create_cbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit(comboBoxOffice);
            cbe.TableData("M_Office", "OfficeCode", "OfficeName");
            //OfficeArray = new string[cbe.ValueItem.Length];
            //Array.Copy(cbe.ValueItem, 0, OfficeArray, 0, OfficeArray.Length);
        }


        // 部門
        private void create_cbDepart()
        {
            ComboBoxEdit cbe = new ComboBoxEdit(comboBoxDepart);
            cbe.DepartmentList((comboBoxOffice.Text == HQOffice) ? "DEPH" : "DEPB");
        }


        // DataGridViewButtonの番号を再採番
        private void buttonNumbering()
        {
            int startNo = 1;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = (startNo + i).ToString();
            }
        }

        
        private void dataGridViewAllChecked(DataGridView dgv)
        {
            allChecked = !allChecked;
            for (int i = 0; i < dgv.RowCount; i++)
            {
                if (dgv.Rows[i].Cells["CostCode"].Value != null && Convert.ToString(dgv.Rows[i].Cells["CostCode"].Value) != "")
                    dgv.Rows[i].Cells["Check"].Value = allChecked;
            }
        }


        private void initializeCheckMark()
        {
            if (checkBoxAll.Checked) checkBoxAll.Checked = false;
            allChecked = false;
        }


        private bool dispCostReportData(int slipNo)
        {
            CostReportData crp = new CostReportData();
            crd = crp.SelectCostReport(slipNo);
            if (crd == null) return false;
            loadCostRecordData(crd, dataGridView1);
            dispTaskInformation(textBoxTaskCode.Text);
            initializeCheckMark();
            //readOnlyProperty(dataGridView1);
            return true;
        }


        private void loadCostRecordData(CostReportData[] crd, DataGridView dgv)
        {
            dgv.Rows.Clear();
            if (crd.GetLength(0) > iniRCnt)
            {
                dgv.Rows.Add(crd.GetLength(0) - 1);
            }
            else
            {
                dgv.Rows.Add(iniRCnt);
            }
            char markChar;
            dateTimePickerEntryDate.Value = crd[0].ReportDate;
            for (int i = 0; i < crd.GetLength(0); i++)
            {
                dgv.Rows[i].Cells["CostCode"].Value = crd[i].ItemCode;
                if (crd[i].ItemCode.TrimEnd() == "K999")
                {
                    markChar = (crd[i].Item == "") ? ' ' : crd[i].Item[0];
                    dgv.Rows[i].Cells["Item"].Value = (markChar == '●') ? crd[i].Item : "●" + crd[i].Item;　 
                }
                else
                {
                    dgv.Rows[i].Cells["Item"].Value = crd[i].Item;
                }
                dgv.Rows[i].Cells["ItemDetail"].Value = "";
                dgv.Rows[i].Cells["Quantity"].Value = decPointFormat(crd[i].Quantity);
                dgv.Rows[i].Cells["Unit"].Value = crd[i].Unit;
                dgv.Rows[i].Cells["UnitPrice"].Value = (Convert.ToString(crd[i].UnitPrice) == "") ? "0": decFormat(crd[i].UnitPrice);
                dgv.Rows[i].Cells["Cost"].Value = (Convert.ToString(crd[i].Cost) == "") ? "0": decFormat(crd[i].Cost);
                dgv.Rows[i].Cells["CostID"].Value = 0;
                dgv.Rows[i].Cells["CostReportID"].Value = crd[i].CostReportID;
                dgv.Rows[i].Cells["Subject"].Value = Convert.ToString(crd[i].Subject);
                dgv.Rows[i].Cells["SubCoCode"].Value = Convert.ToString(crd[i].SubCoCode);
                dgv.Rows[i].Cells["Note"].Value = Convert.ToString(crd[i].Note);

                if (Convert.ToString(crd[i].MemberCode) == "")
                {
                    dgv.Rows[i].Cells["MemberCode"].Value = (labelLeaderMCode.Text == "") ? hp.MemberCode : labelLeaderMCode.Text;
                }
                else
                {
                    dgv.Rows[i].Cells["MemberCode"].Value = Convert.ToString(crd[i].MemberCode);
                }

                dgv.Rows[i].Cells["SalesMCode"].Value = (Convert.ToString(crd[i].SalesMCode) == "") ? "" : Convert.ToString(crd[i].SalesMCode);
                dgv.Rows[i].Cells["AccountCode"].Value = (Convert.ToString(crd[i].AccountCode) == "") ? "": Convert.ToString(crd[i].AccountCode);
            }

            textBoxSlipNo.Text = Convert.ToString(crd[0].SlipNo);
            comboBoxOffice.SelectedValue = crd[0].OfficeCode;
            comboBoxDepart.SelectedValue = crd[0].Department;
            textBoxTaskCode.Text = crd[0].TaskCode;
            labelLeaderMCode.Text = crd[0].LeaderMCode;
            labelCoTaskCode.Text = String.IsNullOrEmpty( crd[0].CoTaskCode ) ? "" : crd[0].CoTaskCode;
        }

        /// <summary>
        /// 原価コードで入力制限をかける
        /// dataGridViewの一行目の原価コードで判断する
        /// </summary>
        /// <param name="dgv"></param>
        private void readOnlyProperty(DataGridView dgv)
        {
            if (Convert.ToString(dgv.Rows[0].Cells["Subject"].Value) == "F")
            {
                dgv.Columns["Quantity"].ReadOnly = true;
                dgv.Columns["Cost"].ReadOnly = false;
            }
            else
            {
                dgv.Columns["Quantity"].ReadOnly = false;
                dgv.Columns["Cost"].ReadOnly = true;
            }
        }


        // 原価マスタデータを得る
        private void chooseCostMasterData(int lNo)
        {
            CostData cmds = FormCostList.ReceiveItems(cmd);
            if (cmds == null) return;

            loadCostMasterData(dataGridView1.Rows[lNo], cmds);
        }

        private void chooseTaskCodeNameData()
        {
            TaskCodeNameData tcds = FormTaskCodeNameList.ReceiveItems(tcd);
            if (tcds == null) return;

            textBoxTaskCode.Text = tcds.TaskCode;
            textBoxTaskName.Text = tcds.TaskName;
            dispTaskInformation(textBoxTaskCode.Text);
        }
        
        private bool dispTaskInformation(string taskCode)
        {
            ListFormDataOp lo = new ListFormDataOp();
            TaskCodeNameData tcnd = lo.SelectTaskCodeNameData(taskCode,Convert.ToString(comboBoxOffice.SelectedValue)); 
            TaskData td = lo.SelectTaskData(taskCode);
            if (td == null) return false;
            textBoxTaskName.Text = td.TaskName;
            labelTaskPlace.Text = td.TaskPlace;
            labelStartDate.Text = (td.StartDate.StripTime()).ToString("yyyy年MM月dd日");
            labelEndDate.Text = (td.EndDate.StripTime()).ToString("yyyy年MM月dd日");
            labelSalesMCode.Text = td.SalesMCode;

            DataTable dt;
            DataRow dr;
            SqlHandling sql;
            if (td.PartnerCode != null)
            {
                sql = new SqlHandling("M_Partners");
                if ((dt = sql.SelectAllData("WHERE PartnerCode = '" + td.PartnerCode + "'")) != null)
                {
                    dr = dt.Rows[0];
                    labelPartnerName.Text = Convert.ToString(dr["PartnerName"]);
                    labelPartnerCode.Text = Convert.ToString(dr["PartnerCode"]);
                }
            }


            // 20190302 asakawa
            // add start
            labelLeaderMName.Text = "";
            labelSalesMName.Text = "";
            // add end


            //if (tcnd.LeaderMCode != null && tcnd.LeaderMCode != " ")
            if (tcnd != null && tcnd.LeaderMCode != null && tcnd.LeaderMCode != " ")
            {
                sql = new SqlHandling("M_Members");
                if ((dt = sql.SelectAllData("WHERE MemberCode = '" + tcnd.LeaderMCode + "'")) != null)
                {
                    dr = dt.Rows[0];
                    labelLeaderMName.Text = Convert.ToString(dr["Name"]);
                    labelLeaderMCode.Text = tcnd.LeaderMCode;
                }
            }

            if (td.SalesMCode != " ")
            {
                sql = new SqlHandling("M_Members");
                if ((dt = sql.SelectAllData("WHERE MemberCode = '" + td.SalesMCode + "'")) != null)
                {
                    dr = dt.Rows[0];
                    labelSalesMName.Text = Convert.ToString(dr["Name"]);
                }
            }


            return true;
        }


        private bool dispCostMaster(DataGridViewRow dgvRow ,string costCode,string officeCode)
        {
            SqlHandling sql = new SqlHandling("M_Cost");
            DataTable dt;
            if ((dt = sql.SelectAllData("WHERE CostCode = '" + costCode + "' AND OfficeCode = '" + officeCode + "'")) == null) return false;

            DataRow dr = dt.Rows[0];
            CostData cmds = new CostData(dr);
            loadCostMasterData(dgvRow, cmds);
            return true;
        }

        private void loadCostMasterData(DataGridViewRow dgvRow,CostData cmds)
        {
            dgvRow.Cells["CostCode"].Value = cmds.CostCode;
            dgvRow.Cells["Item"].Value = cmds.Item;
            dgvRow.Cells["ItemDetail"].Value = cmds.ItemDetail;
            dgvRow.Cells["Unit"].Value = cmds.Unit;
            dgvRow.Cells["UnitPrice"].Value = decFormat(cmds.Cost);
            dgvRow.Cells["CostID"].Value = cmds.CostID;
        }


        private void storeCostReportData(DataGridView dgv)
        {
            int newSlipNo = 0;
            //if (!newEntry && textBoxSlipNo.Text != "")
            //    newSlipNo = Convert.ToInt32(textBoxSlipNo.Text);
            if (!newEntry) newSlipNo = curSlipNo;

            // Data Count
            int dataCount = procDgvDataCount(dataGridView1);
            if (dataCount == 0) return;
            CostReportData[] crda = new CostReportData[dataCount];
            int j = 0;
            for (int i = 0; i < dgv.RowCount; i++)
            {
                if (Convert.ToString(dgv.Rows[i].Cells["CostCode"].Value) != "")
                {
                    crda[j] = new CostReportData();

                    crda[j].SlipNo = newSlipNo;
                    crda[j].OfficeCode = Convert.ToString(comboBoxOffice.SelectedValue);
                    crda[j].Department = Convert.ToString(comboBoxDepart.SelectedValue);
                    crda[j].TaskCode = textBoxTaskCode.Text;
                    crda[j].ReportDate = dateTimePickerEntryDate.Value;
                    crda[j].ItemCode = Convert.ToString(dgv.Rows[i].Cells["CostCode"].Value);
                    crda[j].Item = Convert.ToString(dgv.Rows[i].Cells["Item"].Value);
                    crda[j].UnitPrice = DHandling.ToRegDecimal(Convert.ToString(dgv.Rows[i].Cells["UnitPrice"].Value));
                    crda[j].Quantity = DHandling.ToRegDecimal(Convert.ToString(dgv.Rows[i].Cells["Quantity"].Value));
                    crda[j].Cost = DHandling.ToRegDecimal(Convert.ToString(dgv.Rows[i].Cells["Cost"].Value));
                    crda[j].Unit = Convert.ToString(dgv.Rows[i].Cells["Unit"].Value);
                    crda[j].CustoCode = labelPartnerCode.Text;
                    crda[j].SubCoCode = Convert.ToString(dgv.Rows[i].Cells["SubCoCode"].Value);
                    crda[j].Subject = Convert.ToString(dgv.Rows[i].Cells["Subject"].Value);
                    crda[j].MemberCode = Convert.ToString(dgv.Rows[i].Cells["MemberCode"].Value);
                    crda[j].LeaderMCode = labelLeaderMCode.Text;
                    crda[j].SalesMCode = Convert.ToString(dgv.Rows[i].Cells["SalesMCode"].Value);
                    crda[j].AccountCode = Convert.ToString(dgv.Rows[i].Cells["AccountCode"].Value);
                    crda[j].Note = Convert.ToString(dgv.Rows[i].Cells["Note"].Value);
                    crda[j].CostReportID = (Convert.ToString(dgv.Rows[i].Cells["CostReportID"].Value) == "") ? 0 : Convert.ToInt32(dgv.Rows[i].Cells["CostReportID"].Value);
                    crda[j].CoTaskCode = labelCoTaskCode.Text;
                    j++;
                    if (newSlipNo != 0) involvedDataUpdate(dgv.Rows[i], newSlipNo);
                }
            }

            CostReportData crp = new CostReportData();
            int val = crp.StoreCostReportData(crda);
            if (val < 0)
            {
                labelMessage.Text = faildProc;
                return;
            }
            if (val > 0) textBoxSlipNo.Text = Convert.ToString(val);
            // Wakamatsu 20170307
            //curSlipNo = newSlipNo;
            if (newSlipNo != 0)
                curSlipNo = newSlipNo;
            else
                curSlipNo = val;
            // Wakamatsu 20170307
        }


        private void deleteCostReportData(DataGridView dgv, int slipNo)
        {
            CostReportData crp = new CostReportData();

            for(int i = 0;i < dgv.RowCount; i++)
            {
                if (Convert.ToString(dgv.Rows[i].Cells["CostCode"].Value) == "") continue;
                if (Convert.ToBoolean(dgv.Rows[i].Cells["Check"].Value))
                {
                    if (!crp.DeleteCostReport("@crID", Convert.ToInt32(dgv.Rows[i].Cells["CostReportID"].Value)))
                    {
                        deleteErrorMessage("原価実績データ");
                        return;
                    }
                    if (!involvedDataDelete(dgv.Rows[i], slipNo)) return; 
                }
            }
        }


        private bool involvedDataDelete(DataGridViewRow dgvRow, int slipNo)
        {
            WorkReportData wrp;
            OsWkDetailData owd;
            OsPayOffData ofd;
            OsPaymentData omd;

            switch (Convert.ToString(dgvRow.Cells["Subject"].Value))
            {
                case "A":
                case "B":
                case "K":
                    wrp = new WorkReportData();
                    if (wrp.ExistenceSlipNo(slipNo))
                    {
                        if (!wrp.ClearPartWorkReport(slipNo, Convert.ToInt32(dgvRow.Cells["CostReportID"].Value)))
                        {
                            deleteErrorMessage("作業内訳データ");
                            return false;
                        }
                    }
                    else
                    {
                        owd = new OsWkDetailData();
                        if (owd.ExistenceSlipNo(slipNo, Convert.ToInt32(dgvRow.Cells["CostReportID"].Value)))
                        {
                            if (!owd.DeleteOsWkDetail("@slip", slipNo))
                            {
                                deleteErrorMessage("外注作業内訳データ");
                                return false;
                            }
                        }
                    }
                    break;
                case "F":
                    ofd = new OsPayOffData();
                    if (ofd.ExistenceSlipNo(slipNo))
                    {
                        if (!ofd.DeletePayOff("@slip", slipNo))
                        {
                            deleteErrorMessage("外注精算データ");
                            return false;
                        }
                    }
                    else
                    {
                        omd = new OsPaymentData();
                        if (omd.ExistenceSlipNo(slipNo))
                        {
                            if (!omd.DeletePayment("@slip", slipNo))
                            {
                                deleteErrorMessage("外注出来高データ");
                                return false;
                            }
                        }
                    }
                    break;
                default:
                    owd = new OsWkDetailData();
                    if (owd.ExistenceSlipNo(slipNo, Convert.ToInt32(dgvRow.Cells["CostReportID"].Value)))
                    {
                        if (!owd.DeleteOsWkDetail("@slip", slipNo))
                        {
                            deleteErrorMessage("外注作業内訳データ");
                            return false;
                        }
                    }
                    break;
            }
            return true;
        }


        private bool involvedDataUpdate(DataGridViewRow dgvRow, int slipNo)
        {
            WorkReportData wrp;
            OsWkDetailData owd;
            OsPayOffData ofd;
            OsPaymentData omd;

            decimal unitPrice = DHandling.ToRegDecimal(Convert.ToString(dgvRow.Cells["UnitPrice"].Value));
            decimal cost = DHandling.ToRegDecimal(Convert.ToString(dgvRow.Cells["Cost"].Value));
            decimal qty = DHandling.ToRegDecimal(Convert.ToString(dgvRow.Cells["Quantity"].Value));

            switch (Convert.ToString(dgvRow.Cells["Subject"].Value))
            {
                case "A":
                case "B":
                case "K":
                    wrp = new WorkReportData();
                    if (wrp.ExistenceSlipNo(slipNo))
                    {
                        if (!wrp.UpdatePartWorkReport(slipNo, Convert.ToInt32(dgvRow.Cells["CostReportID"].Value), qty))
                        {
                            updateErrorMessage("作業内訳データ");
                            return false;
                        }
                    }
                    else
                    {
                        owd = new OsWkDetailData();
                        if (owd.ExistenceSlipNo(slipNo, Convert.ToInt32(dgvRow.Cells["CostReportID"].Value)))
                        {
                            if (!owd.UpdatePartOsWkDetail(slipNo, Convert.ToInt32(dgvRow.Cells["CostReportID"].Value), qty, cost))
                            {
                                updateErrorMessage("外注作業内訳データ");
                                return false;
                            }
                        }
                    }


                    break;
                case "F":
                    ofd = new OsPayOffData();
                    if (ofd.ExistenceSlipNo(slipNo))
                    {
                        ofd = ofd.SelectOsPayOff(slipNo);
                        ofd.Cost = cost;
                        if (!ofd.UpdatePayOff())
                        {
                            updateErrorMessage("外注精算データ");
                            return false;
                        }

                        //if (!ofd.UpdatePartPayOff(slipNo, cost))
                        //{
                        //    updateErrorMessage("外注精算データ");
                        //    return false;
                        //}
                    }
                    else
                    {
                        omd = new OsPaymentData();
                        if (omd.ExistenceSlipNo(slipNo))
                        {
                            omd = omd.SelectPayment(slipNo);
                            omd.Amount = cost;
                            if (!omd.UpdatePayment())
                            {
                                updateErrorMessage("外注出来高データ");
                                return false;
                            }
                            //if (!omd.UpdatePartPayment(slipNo, cost))
                            //{
                            //    updateErrorMessage("外注出来高データ");
                            //    return false;
                            //}
                        }
                    }

                    break;
                default:
                    owd = new OsWkDetailData();
                    if (owd.ExistenceSlipNo(slipNo, Convert.ToInt32(dgvRow.Cells["CostReportID"].Value)))
                    {
                        if (!owd.UpdatePartOsWkDetail(slipNo, Convert.ToInt32(dgvRow.Cells["CostReportID"].Value),qty, cost))
                        {
                            deleteErrorMessage("外注作業内訳データ");
                            return false;
                        }
                    }
                    break;
            }
            return true;
        }


        private int procDgvDataCount(DataGridView dgv)
        {
            int dataCount = 0;
            for (int i = 0; i < dgv.RowCount; i++)
            {
                if (Convert.ToString(dgv.Rows[i].Cells["CostCode"].Value) != "") dataCount++;
            }
            return dataCount;
        }
       
        
        private void calculateLine(DataGridViewRow dgvRow)
        {
            // Wakamatsu 20170307
            //if (!(DHandling.IsDecimal(Convert.ToString(dgvRow.Cells["Quantity"].Value)))) return;
            //if (Convert.ToString(dgvRow.Cells["UnitPrice"].Value) == "") return;

            //decimal calcUnitPrice = toRegDecimal(Convert.ToString(dgvRow.Cells["UnitPrice"].Value));
            //decimal calcQuantity = toRegDecimal(Convert.ToString(dgvRow.Cells["Quantity"].Value));
            decimal calcQuantity = 0;
            decimal calcUnitPrice = 0;

            if ((DHandling.IsDecimal(Convert.ToString(dgvRow.Cells["Quantity"].Value))))
                calcQuantity = toRegDecimal(Convert.ToString(dgvRow.Cells["Quantity"].Value));
            if ((DHandling.IsDecimal(Convert.ToString(dgvRow.Cells["UnitPrice"].Value))))
                calcUnitPrice = toRegDecimal(Convert.ToString(dgvRow.Cells["UnitPrice"].Value));
            // Wakamatsu 20170307
            decimal calcCost = calcUnitPrice * calcQuantity;

            dgvRow.Cells["Quantity"].Value = decPointFormat(calcQuantity);
            dgvRow.Cells["UnitPrice"].Value = decFormat(calcUnitPrice);
            dgvRow.Cells["Cost"].Value = decFormat(calcCost);
            calculateAmount();
        }
        
         

        private void calculateAmount()
        {
            decimal sumCost = 0M;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (Convert.ToString(dataGridView1.Rows[i].Cells["CostCode"].Value) != "")
                {
                    sumCost += toRegDecimal(Convert.ToString(dataGridView1.Rows[i].Cells["Cost"].Value));
                }
            }

            labelAmount.Text = decFormat(sumCost);
            labelTax.Text = decFormat(sumCost * hp.TaxRate);
            labelSum.Text = decFormat(sumCost + (sumCost * hp.TaxRate));
        }


        private void deleteErrorMessage(string table)
        {
            MessageBox.Show("データ削除に失敗しました。", table, MessageBoxButtons.OK);
        }

        private void updateErrorMessage(string table)
        {
            MessageBox.Show("データ更新に失敗しました。", table, MessageBoxButtons.OK);
        }
        

        private void createSearchKeyArray()
        {
            CostReportData crdp = new CostReportData();
            if (textBoxTaskCode.Text == null) textBoxTaskCode.Text = "";
            sNoArray = crdp.SelectCostSlipNo(dateTimePickerEntryDate.Value, textBoxTaskCode.Text, Convert.ToString(comboBoxOffice.SelectedValue));
            labelMessage.Text = (sNoArray[0] == -1) ? "指定された条件では対象となる伝票がありませんでした。"
                                                    : Convert.ToString(sNoArray.Length) + "件のデータがあります。";
            sNoIndex = 0;
            holdSlipNo = curSlipNo;
        }

        private static decimal toRegDecimal(string decStr)
        {
            return DHandling.ToRegDecimal(decStr);
        }

        private static string decFormat(decimal decNum)
        {
            return DHandling.DecimaltoStr(decNum, "#,0");
        }

        private static string decPointFormat(decimal decNum)
        {
            return DHandling.DecimaltoStr(decNum, "0.00");
        }

        
    }
}
