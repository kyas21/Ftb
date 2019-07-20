using System;
using System.Windows.Forms;

namespace ClassLibrary
{
    public static class DMessage
    {
        //--------------------------------------//
        // Field
        //--------------------------------------//

        //--------------------------------------//
        // Constructor
        //--------------------------------------//

        //--------------------------------------//
        // Property
        //--------------------------------------//

        //--------------------------------------//
        // Method
        //--------------------------------------//
        //********** DialogBox
        public static DialogResult DialogCancel()
        {
            return dResult( "入力されたものは取り消されます。よろしいですか？", "確認" );
        }

        public static DialogResult DialogChange()
        {
            return dResult( "選択範囲を変更するとここまでに入力されたものは取り消されます。実行してもよろしいですか？", "確認" );
        }

        public static DialogResult DialogUpdate()
        {
            return dResult( "データを削除を含む更新です。実行すると元の状態には戻せません。実行してもよろしいですか？", "確認" );
        }

        public static DialogResult DialogDelete()
        {
            return dResult( "データを削除します。実行すると元の状態には戻せません。実行してもよろしいですか？", "確認" );
        }

        public static DialogResult DialogLongTime()
        {
            return dResult( "この条件では処理量が多いため、時間やPCの資源消費量が増加します。利用環境によっては中断の可能性も想定されます。実行してもよろしいですか？", "確認" );
        }

        public static DialogResult DialogOverWrite()
        {
            return dResult( "データを上書きします。よろしいですか？", "確認" );
        }


        public static DialogResult DialogNewLoad()
        {
            return dResult( "この処理を実行するとこれまでのデータがすべて削除されます。続行しますか？", "確認" );
        }

        public static DialogResult DialogOverLoad()
        {
            return dResult( "この処理を実行すると保存していないデータはすべて消去されます。続行しますか？", "確認" );
        }

        public static DialogResult DialogRemining()
        {
            return dResult( "保存していないデータがあります。処理を続けると消されますが、続行しますか？", "確認" );
        }

        public static DialogResult DialogCreateFolder()
        {
            return dResult( "協力会社フォルダーが存在しません。新規作成し継続しますか？", "確認" );
        }

        public static void ValueErrMsg()
        {
            mesError("不正な文字が入力されています。");
        }

        public static void StringErrMsg( string item )
        {
            mesError( "不正な文字が入力されています。文字列 = " + item);
        }

        public static void ValueErrMsg( string item )
        {
            mesError("入力値が許可された範囲を超えています。不正な値 = " + item);
        }

        public static void CodeErrMsg( string item )
        {
            mesError("入力されたコードに対するデータがありません。再入力してください。文字列 = " + item);
        }

        public static void DataExistence()
        {
            mesError("この内容は既に登録されています。");
        }


        public static void DataNotEnough()
        {
            mesError("データが不十分です。次の処理へ進めません。");
        }


        public static void DataNotExistence( string addMsg )
        {
            mesWarning( "処理対象となるデーがありません。" + "  " + addMsg);
        }


        public static void SelectInvalid()
        {
            mesWarning( "指定されたデータは存在しません。" );
        }


        public static void SelectInvalid( string para1, string para2 )
        {
            mesWarning( "指定された" + para1 + "データは存在しません。" + para2 );
        }


        public static void PastDay( DateTime clsDate )
        {
            mesWarning("締日（" + clsDate.ToLongDateString() + ") 以前は確認のみ可能です。");
        }


        public static void FolderNotExistence( string addMsg )
        {
            mesError( "処理に必要なフォルダがありません。" + "  " + addMsg);
        }


        public static void Contradiction( string addMsg )
        {
            mesWarning( addMsg + "矛盾しています。");
        }


        public static void Unsaved()
        {
            mesWarning( "保存されていません。" );
        }


        private static DialogResult dResult(string mesStrin, string para1 )
        {
            DialogResult result = MessageBox.Show( mesStrin,para1,MessageBoxButtons.YesNo,MessageBoxIcon.Information );
            return result;

        }


        private static void mesWarning(string mesString )
        {
            MessageBox.Show( mesString, "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning );
        }

        private static void mesError(string mesString )
        {
            MessageBox.Show( mesString, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
        }

    }
}
