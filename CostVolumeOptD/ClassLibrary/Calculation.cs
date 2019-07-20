using System;
using System.Windows.Forms;

namespace ClassLibrary
{
    public class Calculation
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        public decimal Sum;
        public decimal Tax;
        public decimal GSum;
        public decimal[] SumArray;
        TaskEntryData ted;
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public Calculation()
        {
        }

        public Calculation(TaskEntryData ted)
        {
            this.ted = ted;
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public string ExtractCalcWord(string tStr)
        {
            if (tStr.Length == 0) return null;

            string chkStr = tStr.Replace(" ", "");
            chkStr = chkStr.Replace("　", "");
            chkStr = chkStr.Replace("《", "");
            chkStr = chkStr.Replace("》", "");
            if (chkStr.Length > 3) return null;
            for (int i = 0; i < Sign.StArray.Length; i++) if (chkStr == Sign.StArray[i]) return chkStr;
            return null;
        }


        public void VCalcEstimate(DataGridView dgv)
        {
            decimal wGSum = 0, wLSum = 0, wMSum = 0, wSSum = 0, wDis = 0, wExp = 0, wTax = 0;
            string keyStr;
            decimal wGExp = 0, wLExp = 0, wMExp = 0;
            decimal wGTax = 0, wLTax = 0, wMTax = 0;

            for (int i = 0; i < dgv.RowCount; i++)
            {
                keyStr = ExtractCalcWord(Convert.ToString(dgv.Rows[i].Cells["Item"].Value));
                if (keyStr == Sign.SubTotal)
                {
                    dgv.Rows[i].Cells["Amount"].Value = MinusConvert(wSSum + wExp + wTax, "#.0");
                    wSSum = 0;
                }
                else if (keyStr == Sign.Total)
                {
                    dgv.Rows[i].Cells["Amount"].Value = MinusConvert(wMSum + wMExp + wMTax, "#.0");
                    wMSum = 0; wSSum = 0;
                    wExp = 0;
                    wTax = 0;
                    wMExp = 0;
                    wMTax = 0;
                }
                else if (keyStr == Sign.SumTotal)
                {
                    dgv.Rows[i].Cells["Amount"].Value = MinusConvert(wLSum + wLExp + wLTax, "#.0");
                    wLSum = 0; wMSum = 0; wSSum = 0;
                    wExp = 0;
                    wTax = 0;
                    wMExp = 0;
                    wMTax = 0;
                    wLExp = 0;
                    wLTax = 0;
                }
                else if (keyStr == Sign.Expenses)
                {
                    wExp = wGSum * ted.Expenses;
                    wMExp += wExp;
                    wLExp += wExp;
                    wGExp += wExp;
                    dgv.Rows[i].Cells["Amount"].Value = MinusConvert(wExp, "#.0");
                }
                else if (keyStr == Sign.Discount)
                {
                    wDis = SignConvert(dgv.Rows[i].Cells["Amount"].Value);

                    wSSum += wDis;
                    wMSum += wDis;
                    wLSum += wDis;
                    wGSum += wDis;
                }
                else if (keyStr == Sign.Tax)
                {
                    wTax = (wGSum + wGExp) * ted.TaxRate;
                    wMTax += wTax;
                    wLTax += wTax;
                    wGTax += wTax;
                    dgv.Rows[i].Cells["Amount"].Value = MinusConvert(wTax, "#.0");
                }
                else if (keyStr == Sign.GrandTotal)
                {
                    dgv.Rows[i].Cells["Amount"].Value = MinusConvert(wGSum + wGExp + wGTax, "#.0");
                }
                else
                {
                    wSSum += SignConvert(dgv.Rows[i].Cells["Amount"].Value);
                    wMSum += SignConvert(dgv.Rows[i].Cells["Amount"].Value);
                    wLSum += SignConvert(dgv.Rows[i].Cells["Amount"].Value);
                    wGSum += SignConvert(dgv.Rows[i].Cells["Amount"].Value);
                }
            }

            // TRY kusano 20170427
            Tax = wGTax;
            Sum = wGSum + wGExp;
            GSum = Sum + Tax;
        }


        public void HCalcEstimateRow(DataGridViewRow dgvRow)
        {
            if (dgvRow.Cells["Unit"].Value == null || (String)dgvRow.Cells["Unit"].Value == "") return;     // 計算対象外
            if (ExtractCalcWord(Convert.ToString(dgvRow.Cells["Item"].Value)) == Sign.Expenses) return;

            if(string.IsNullOrEmpty(Convert.ToString(dgvRow.Cells["Quantity"].Value)))
            {
                dgvRow.Cells["Amount"].Value = null;
                return;
            }

            if(string.IsNullOrEmpty(Convert.ToString(dgvRow.Cells["Cost"].Value)))
            {
                dgvRow.Cells["Amount"].Value = null;
                return;
            }

            decimal qty = SignConvert(dgvRow.Cells["Quantity"].Value);
            decimal cost = SignConvert(dgvRow.Cells["Cost"].Value);
            dgvRow.Cells["Amount"].Value = MinusConvert(qty * cost, "#,0");
        }


        public void HCalcInvoiceRow( DataGridViewRow dgvRow )
        {
            if( dgvRow.Cells["Unit"].Value == null || ( String )dgvRow.Cells["Unit"].Value == "" ) return;     // 計算対象外
            if( ExtractCalcWord( Convert.ToString( dgvRow.Cells["Item"].Value ) ) == Sign.Expenses ) return;

            if( string.IsNullOrEmpty( Convert.ToString( dgvRow.Cells["Quantity"].Value ) ) )
            {
                dgvRow.Cells["Amount"].Value = null;
                return;
            }
            if( string.IsNullOrEmpty( Convert.ToString( dgvRow.Cells["CCost"].Value ) ) )
            {
                dgvRow.Cells["Amount"].Value = null;
                return;
            }

            decimal qty = SignConvert( dgvRow.Cells["Quantity"].Value );
            decimal cost = SignConvert( dgvRow.Cells["CCost"].Value );
            dgvRow.Cells["Amount"].Value = MinusConvert( qty * cost, "#,0" );
        }


        public void VCalcPlanningCont(DataGridView dgv)
        {
            SumArray = new decimal[3];

            decimal[] wGSum = new decimal[3];
            decimal[] wLSum = new decimal[3];
            decimal[] wMSum = new decimal[3];
            decimal[] wSSum = new decimal[3];
            decimal[] wDis = new decimal[3];
            decimal[] wExp = new decimal[3];
            decimal[] wTax = new decimal[3];

            decimal[] wMExp = new decimal[3];
            decimal[] wMTax = new decimal[3];
            decimal[] wLExp = new decimal[3];
            decimal[] wLTax = new decimal[3];
            decimal[] wGExp = new decimal[3];
            decimal[] wGTax = new decimal[3];

            for (int i = 0; i < 3; i++)
            {
                SumArray[i] = 0;
                wGSum[i] = 0;
                wLSum[i] = 0;
                wMSum[i] = 0;
                wSSum[i] = 0;
                wDis[i] = 0;
                wExp[i] = 0;
                wTax[i] = 0;

                wMExp[i] = 0;
                wMTax[i] = 0;
                wLExp[i] = 0;
                wLTax[i] = 0;
                wGExp[i] = 0;
                wGTax[i] = 0;
            }

            string keyStr;
            for (int i = 0; i < dgv.RowCount; i++)
            {
                keyStr = ExtractCalcWord(Convert.ToString(dgv.Rows[i].Cells["Item"].Value));
                if (keyStr == Sign.SubTotal)
                {
                    for (int j = 0; j < 3; j++) dgv.Rows[i].Cells["Amount" + j.ToString()].Value = decFormat(wSSum[j] + wExp[j] + wTax[j]);
                    for (int j = 0; j < 3; j++) wSSum[j] = 0;
                }
                else if (keyStr == Sign.Total)
                {
                    for (int j = 0; j < 3; j++) dgv.Rows[i].Cells["Amount" + j.ToString()].Value = decFormat(wMSum[j] + wMExp[j] + wMTax[j]);
                    for (int j = 0; j < 3; j++)
                    {
                        wMSum[j] = 0;
                        wMTax[j] = 0;
                        wMExp[j] = 0;
                    }
                    for (int j = 0; j < 3; j++) wSSum[j] = 0;
                }
                else if (keyStr == Sign.SumTotal)
                {
                    for (int j = 0; j < 3; j++) dgv.Rows[i].Cells["Amount" + j.ToString()].Value = decFormat(wLSum[j] + wLExp[j] + wLTax[j]);
                    for (int j = 0; j < 3; j++)
                    {
                        wLSum[j] = 0;
                        wLTax[j] = 0;
                        wLExp[j] = 0;
                    }
                    for (int j = 0; j < 3; j++)
                    {
                        wMSum[j] = 0;
                        wMTax[j] = 0;
                        wMExp[j] = 0;
                    }
                    for (int j = 0; j < 3; j++) wSSum[j] = 0;
                }
                else if (keyStr == Sign.Expenses)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        wExp[j] = wGSum[j] * ted.Expenses;
                        dgv.Rows[i].Cells["Amount" + j.ToString()].Value = decFormat(wExp[j]);
                        wMExp[j] += wExp[j];
                        wLExp[j] += wExp[j];
                        wGExp[j] += wExp[j];
                    }
                }
                else if (keyStr == Sign.Discount)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        wDis[j] = Convert.ToDecimal(dgv.Rows[i].Cells["Amount" + j.ToString()].Value);
                        wSSum[j] += wDis[j];
                        wMSum[j] += wDis[j];
                        wLSum[j] += wDis[j];
                        wGSum[j] += wDis[j];
                    }
                }
                else if (keyStr == Sign.Tax)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        wTax[j] = (wGSum[j] + wGExp[j]) * ted.TaxRate;
                        dgv.Rows[i].Cells["Amount" + j.ToString()].Value = decFormat(wTax[j]);
                        wMTax[j] += wTax[j];
                        wLTax[j] += wTax[j];
                        wGTax[j] += wTax[j];
                    }
                }
                else if (keyStr == Sign.GrandTotal)
                {
                    for (int j = 0; j < 3; j++) dgv.Rows[i].Cells["Amount" + j.ToString()].Value = decFormat(wGSum[j] + wGExp[j] + wGTax[j]);
                }
                else
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dgv.Rows[i].Cells["Amount" + j.ToString()].Value)))
                        {
                            wSSum[j] += Convert.ToDecimal(dgv.Rows[i].Cells["Amount" + j.ToString()].Value);
                            wMSum[j] += Convert.ToDecimal(dgv.Rows[i].Cells["Amount" + j.ToString()].Value);
                            wLSum[j] += Convert.ToDecimal(dgv.Rows[i].Cells["Amount" + j.ToString()].Value);
                            wGSum[j] += Convert.ToDecimal(dgv.Rows[i].Cells["Amount" + j.ToString()].Value);
                        }
                    }
                }
            }

            for (int j = 0; j < 3; j++)
            {
                SumArray[j] = wGSum[j] + wGExp[j] + wGTax[j];
                Sum = wGSum[j] + wExp[j] - wDis[j];
                Tax = wTax[j];
                GSum = wGSum[j] + wExp[j] - wDis[j] + wTax[j];
            }
        }



        public void HCalcPlanningContRow(DataGridViewRow dgvRow)
        {
            decimal qty = Convert.ToDecimal(dgvRow.Cells["Quantity"].Value);
            decimal cost = 0;
            decimal sum = 0;
            for (int j = 0; j < 3; j++)
            {
                string wk1 = Convert.ToString(dgvRow.Cells["Cost" + j.ToString()].Value);
                string wk2 = Convert.ToString(dgvRow.Cells["Amount" + j.ToString()].Value);
                if (wk1 != "")
                {
                    cost = Convert.ToDecimal(dgvRow.Cells["Cost" + j.ToString()].Value);
                    dgvRow.Cells["Amount" + j.ToString()].Value = decFormat(qty * cost);
                    sum += qty * cost;
                }
                else
                    if (dgvRow.Cells["Amount" + j.ToString()].ReadOnly == true)
                        dgvRow.Cells["Amount" + j.ToString()].Value = null;
            }
            dgvRow.Cells["Sum"].Value = decFormat(sum);
        }



        public void HCalcOutsourceRow(DataGridViewRow dgvRow)
        {
            HCalcEstimateRow(dgvRow);
        }



        public void VCalcOutsource(DataGridView dgv)
        {
            VCalcEstimate(dgv);
        }

        /// </summary>
        /// 累計算出
        /// </summary>
        /// <param name="dgv">対象データグリッドビュー</param>
        /// <param name="TargetColumn">対象列</param>
        /// <param name="TagetRow">対象行</param>
        /// <param name="LastManthTotal">前月までの累計</param>
        /// <param name="CheckFlag">表示フラグ</param>
        /// <param name="FormatSet">表示形式設定</param>
        /// <param name="DisplyValue">累計結果</param>
        /// <returns>累計表示文字列</returns>
        public string Cumulative(DataGridView dgv, int TargetColumn, int TagetRow, decimal LastManthTotal,
                                    bool CheckFlag, string FormatSet, out decimal DisplyValue)
        {
            //前年、各月の受注単月データを取得
            decimal cumulative = 0;
            DisplyValue = 0;

            //SUM($F$6:G6)
            if (!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[TagetRow].Cells[TargetColumn].Value)))
                cumulative = SignConvert(dgv.Rows[TagetRow].Cells[TargetColumn].Value);
            DisplyValue = cumulative + LastManthTotal;

            //IF(COUNT(F6,F8:F10,F15,F18,F23)>1))
            if (CheckFlag)
            {
                //SUM(H6:$R$)
                Decimal totalCumulativeWk = 0;
                if (TargetColumn != 0)
                {
                    for (int iCnt = TargetColumn + 1; iCnt < 13; iCnt++)
                    {
                        cumulative = 0;
                        if (!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[TagetRow].Cells[iCnt].Value)))
                            cumulative = SignConvert(dgv.Rows[TagetRow].Cells[iCnt].Value);
                        totalCumulativeWk = totalCumulativeWk + cumulative;
                    }
                }
                //IF(SUM(F$6:G6)*SUM(H6:$R$))
                if ((DisplyValue * totalCumulativeWk) == 0)
                    return "";
            }
            return MinusConvert(DisplyValue, FormatSet);
        }

        /// <summary>
        /// 出来高月計算出
        /// </summary>
        /// <param name="dgv">対象データグリッドビュー</param>
        /// <param name="TargetColumn">対象列</param>
        /// <param name="VoUncompRow">出来高未成業務行</param>
        /// <param name="VoClaimRemRow">出来高未請求行</param>
        /// <param name="VoClaimRow">出来高請求行</param>
        /// <returns>月計</returns>
        public decimal MonthlyTotal(DataGridView dgv, int TargetColumn, int VoUncompRow,
                                        int VoClaimRemRow, int VoClaimRow)
        {
            //未成業務
            decimal volUncomp = 0;
            if (!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[VoUncompRow].Cells[TargetColumn].Value)))
                volUncomp = SignConvert(dgv.Rows[VoUncompRow].Cells[TargetColumn].Value);

            //未請求
            decimal volClaimRem = 0;
            if (!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[VoClaimRemRow].Cells[TargetColumn].Value)))
                volClaimRem = SignConvert(dgv.Rows[VoClaimRemRow].Cells[TargetColumn].Value);

            //請求
            decimal volClaim = 0;
            if (!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[VoClaimRow].Cells[TargetColumn].Value)))
                volClaim = SignConvert(dgv.Rows[VoClaimRow].Cells[TargetColumn].Value);

            return volUncomp + volClaimRem + volClaim;
        }

        /// <summary>
        /// 出来高累計算出
        /// </summary>
        /// <param name="dgv">対象データグリッドビュー</param>
        /// <param name="TargetColumn">対象列</param>
        /// <param name="TradingRow">出来高月計行</param>
        /// <param name="VoUncompRow">出来高未成業務行</param>
        /// <param name="VoClaimRemRow">出来高未請求行</param>
        /// <param name="VoClaimRow">出来高請求行</param>
        /// <param name="LastManthTotal">前月までの累計</param>
        /// <param name="FormatSet">表示形式設定</param>
        /// <param name="DisplyValue">累計結果</param>
        /// <returns>累計表示文字列</returns>
        public string TotalTradingVolume(DataGridView dgv, int TargetColumn, int TradingRow,
                                            int VoUncompRow, int VoClaimRemRow, int VoClaimRow,
                                            decimal LastManthTotal, string FormatSet, out decimal DisplyValue)
        {
            //前年、各月の受注単月データを取得
            decimal tradingVolume = 0;
            DisplyValue = 0;

            //SUM($F12:G$12)
            if (!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[TradingRow].Cells[TargetColumn].Value)))
                tradingVolume = SignConvert(dgv.Rows[TradingRow].Cells[TargetColumn].Value);

            if (TargetColumn != 0)
                DisplyValue = tradingVolume + LastManthTotal;

            //IF(COUNT(G12)>1))
            if (String.IsNullOrEmpty(Convert.ToString(dgv.Rows[TradingRow].Cells[TargetColumn].Value)))   //出来高 月計
            {
                //SUM(H12:$R$12)
                Decimal totalTradingVolumeWk = 0;
                tradingVolume = 0;
                for (int iCnt = TargetColumn + 1; iCnt < 13; iCnt++)
                {
                    //月計を求める
                    tradingVolume = MonthlyTotal(dgv, iCnt, VoUncompRow, VoClaimRemRow, VoClaimRow);
                    totalTradingVolumeWk = totalTradingVolumeWk + tradingVolume;
                }
                //IF(SUM(F$12:H12)*SUM(I12:$R$12) = 0,"",SUM($F12:H$12))
                if ((DisplyValue * totalTradingVolumeWk) == 0)
                    return "";
            }
            return MinusConvert(DisplyValue, FormatSet);
        }

        /// <summary>
        /// ○○累計 - 出来高累計算出
        /// </summary>
        /// <param name="dgv">対象データグリッドビュー</param>
        /// <param name="TargetColumn">対象列</param>
        /// <param name="TargetRow">被減数行</param>
        /// <param name="VoUncompRow">出来高未成業務行</param>
        /// <param name="VoClaimRemRow">出来高未請求行</param>
        /// <param name="VoClaimRow">出来高請求行</param>
        /// <returns>算出結果</returns>
        public decimal SubtrahendVol(DataGridView dgv, int TargetColumn, int TargetRow, int VoUncompRow,
                                    int VoClaimRemRow, int VoClaimRow)
        {
            decimal cumulative = 0;
            decimal tradingVolume = 0;

            //SUM($F$6:G6)
            Decimal totalCumulativeWk = 0;
            Decimal totalTradingVolumeWk = 0;

            for (int iCnt = 0; iCnt < TargetColumn + 1; iCnt++)
            {
                //被減数累計
                cumulative = 0;
                if (!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[TargetRow].Cells[iCnt].Value)))
                    cumulative = SignConvert(dgv.Rows[TargetRow].Cells[iCnt].Value);

                totalCumulativeWk = totalCumulativeWk + cumulative;

                //出来高累計
                ////未成業務

                ////未請求
                //decimal volClaimRem = 0;
                //if(!String.IsNullOrEmpty( Convert.ToString( dgv.Rows[VoClaimRemRow].Cells[iCnt].Value ) ))
                //    volClaimRem = Convert.ToDecimal( dgv.Rows[VoClaimRemRow].Cells[iCnt].Value );

                ////請求
                //decimal volClaim = 0;
                //if(!String.IsNullOrEmpty( Convert.ToString( dgv.Rows[VoClaimRow].Cells[iCnt].Value ) ))
                //    volClaim = Convert.ToDecimal( dgv.Rows[VoClaimRow].Cells[iCnt].Value );

                ////月計を求める
                //tradingVolume = volUncomp + volClaimRem + volClaim;
                tradingVolume = MonthlyTotal(dgv, iCnt, VoUncompRow, VoClaimRemRow, VoClaimRow);
                totalTradingVolumeWk = totalTradingVolumeWk + tradingVolume;
            }

            //SUM($F$6:G6)-SUM($F$12:G12)
            return (totalCumulativeWk - totalTradingVolumeWk);
        }

        /// <summary>
        /// 出来高累計 - ○○累計
        /// </summary>
        /// <param name="dgv">対象データグリッドビュー</param>
        /// <param name="TargetColumn">対象列</param>
        /// <param name="VoUncompRow">請求単月行</param>
        /// <param name="VoClaimRemRow">出来高未成業務行</param>
        /// <param name="VoClaimRow">出来高未請求行</param>
        /// <param name="TagetRow">出来高請求行</param>
        /// <returns>算出結果</returns>
        public decimal MinuendVol(DataGridView dgv, int TargetColumn, int VoUncompRow, int VoClaimRemRow,
                                            int VoClaimRow, int TagetRow)
        {
            decimal decTradingVolume = 0;
            decimal cumulativeM = 0;

            //SUM($F$12:G12)
            Decimal totalTradingVolumeWk = 0;
            Decimal totalCumulativeMWk = 0;

            for (int iCnt = 0; iCnt < TargetColumn + 1; iCnt++)
            {
                //月計を求める
                decTradingVolume = MonthlyTotal(dgv, iCnt, VoUncompRow, VoClaimRemRow, VoClaimRow);
                totalTradingVolumeWk = totalTradingVolumeWk + decTradingVolume;

                //請求累計
                cumulativeM = 0;
                if (!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[TagetRow].Cells[iCnt].Value)))
                    cumulativeM = SignConvert(dgv.Rows[TagetRow].Cells[iCnt].Value);

                totalCumulativeMWk = totalCumulativeMWk + cumulativeM;
            }

            //SUM($F$12:G12)-SUM($F$15:G15)
            return (totalTradingVolumeWk - totalCumulativeMWk);
        }

        /// <summary>
        /// 原価率算出
        /// </summary>
        /// <param name="dgv">対象データグリッドビュー</param>
        /// <param name="TargetColumn">対象列</param>
        /// <param name="VolumeRow">出来高累計</param>
        /// <param name="CostRow">原価累計</param>
        /// <returns>原価率</returns>
        public string CostRate(DataGridView dgv, int TargetColumn, int VolumeRow, int CostRow)
        {
            decimal tradingVolume = 0;
            decimal cumulativeMC = 0;

            if (String.IsNullOrEmpty(Convert.ToString(dgv.Rows[VolumeRow].Cells[TargetColumn].Value)) || (dgv.Rows[VolumeRow].Cells[TargetColumn].Value.ToString().Trim() == "0"))
                return "";

            tradingVolume = SignConvert(dgv.Rows[VolumeRow].Cells[TargetColumn].Value);

            //IF(COUNT(G24)=1,F24/F13,"")
            if (!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[CostRow].Cells[TargetColumn].Value)))
            {
                decimal decCostRate = 0;
                string strCostRate = "";
                cumulativeMC = SignConvert(dgv.Rows[CostRow].Cells[TargetColumn].Value);
                //F24/F13,""
                decCostRate = (cumulativeMC / tradingVolume);
                // 20190214 asakawa 小数点下１桁に変更（小数点下２桁で四捨五入）
                // strCostRate = decCostRate.ToString("P");
                strCostRate = decCostRate.ToString("P1");
                return strCostRate.Replace(",", "");
            }
            return "";
        }

        /// <summary>
        /// 未収入金算出
        /// </summary>
        /// <param name="dgv">対象データグリッドビュー</param>
        /// <param name="TargetColumn">対象列</param>
        /// <param name="VoUncompRow">出来高未成業務行</param>
        /// <param name="VoClaimRemRow">出来高未請求行</param>
        /// <param name="VoClaimRow">出来高請求行</param>
        /// <param name="ClaimRow">請求行</param>
        /// <param name="PaidRow">入金行</param>
        /// <returns></returns>
        public decimal AccountsReceivable(DataGridView dgv, int TargetColumn, int VoUncompRow, int VoClaimRemRow,
                                            int VoClaimRow, int ClaimRow, int PaidRow)
        {
            decimal tradingVolume = 0;                           //出来高月計
            decimal cumulativeM = 0;                             //請求単月
            decimal cumulativeV = 0;                             //入金単月
            decimal accountsReceivable = 0;                      //未収入金

            // SUM($F$12:G12) SUM($F$15:G15) SUM($F$18:G18)
            Decimal totalTradingVolumeWk = 0;
            Decimal totalCumulativeMWk = 0;
            Decimal totalCumulativeVWk = 0;

            for (int iCnt = 0; iCnt < TargetColumn + 1; iCnt++)
            {
                //月計を求める
                tradingVolume = MonthlyTotal(dgv, iCnt, VoUncompRow, VoClaimRemRow, VoClaimRow);
                totalTradingVolumeWk = totalTradingVolumeWk + tradingVolume;

                //請求累計
                cumulativeM = 0;
                if (!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[ClaimRow].Cells[iCnt].Value)))
                    cumulativeM = SignConvert(dgv.Rows[ClaimRow].Cells[iCnt].Value);
                totalCumulativeMWk = totalCumulativeMWk + cumulativeM;

                //入金累計
                cumulativeV = 0;
                if (!String.IsNullOrEmpty(Convert.ToString(dgv.Rows[PaidRow].Cells[iCnt].Value)))
                    cumulativeV = SignConvert(dgv.Rows[PaidRow].Cells[iCnt].Value);

                totalCumulativeVWk = totalCumulativeVWk + cumulativeV;
            }

            //IF SUM($F12:F$12) > SUM($F15:F$15) → 出来高累計 > 請求累計 
            if (totalTradingVolumeWk > totalCumulativeMWk)
            {
                //IF(SUM($F12:I$12)-SUM($F$18:I18 ) >= 0 → 出来高累計 - 入金累計 >= 0
                if ((totalTradingVolumeWk - totalCumulativeVWk) > 0)
                    accountsReceivable = totalTradingVolumeWk - totalCumulativeVWk;
                else
                    accountsReceivable = -1;
            }
            else
            {
                //IF(SUM($F15:I$15)-SUM($F$18:I18) >= 0 → 請求累計 - 入金累計 >= 0
                if ((totalCumulativeMWk - totalCumulativeVWk) >= 0)
                    accountsReceivable = totalCumulativeMWk - totalCumulativeVWk;
                else
                    accountsReceivable = -1;
            }
            return accountsReceivable;
        }
        //---------------------------------------------------------------------
        // SubRoutine
        //---------------------------------------------------------------------
        private static string decFormat(decimal decNum)
        {
            return DHandling.DecimaltoStr(decNum, "#,0");
        }

        /// <summary>
        /// "-" → "△"変換
        /// </summary>
        /// <param name="TargetValue">対象値</param>
        /// <param name="FormatSet">設定フォーマット</param>
        /// <returns>変換結果</returns>
        private string MinusConvert(decimal TargetValue, string FormatSet)
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
    }
}
