using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnityStandardUtils;

namespace LocalizationEditor
{
    public partial class Form1 : Form
    {
        private string CurrentOriPath = string.Empty;
        private Localization.LanguageSet OriSet = new Localization.LanguageSet();

        private string CurrentTraPath = string.Empty;
        private Localization.LanguageSet TraSet = new Localization.LanguageSet();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {     
            DataView.Rows.Clear();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog.Filter = "XML本地化语言文件|*.xml";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 0;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                TraSet = new Localization.LanguageSet();
                CurrentTraPath = string.Empty;
                FILE_TRA.Text = "未加载";
                LANG_TRA.Text = "未加载";

                try
                {
                    OriSet = Editor.LoadSetFromFile(openFileDialog.FileName);
                    CurrentOriPath = openFileDialog.FileName;
                    Hint.Text = "原文已加载！";

                    FILE_ORI.Text = OriSet.FileName;
                    LANG_ORI.Text = OriSet.Language;

                    DataViewTool.SetOriData(DataView, Editor.GetAllData(OriSet.Data));
                    if (CurrentTraPath == CurrentOriPath) throw new Exception();
                }
                catch
                {
                    OriSet = new Localization.LanguageSet();
                    CurrentOriPath = string.Empty;
                    Hint.Text = "原文加载失败！";

                    FILE_ORI.Text = "未加载";
                    LANG_ORI.Text = "未加载";

                    DataView.Rows.Clear();
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void LOAD_TRA_Click(object sender, EventArgs e)
        {
            if (CurrentOriPath == string.Empty)
            {
                Hint.Text = "未加载原文!";
            }
            else
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Environment.CurrentDirectory;
                openFileDialog.Filter = "XML本地化语言文件|*.xml";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.FilterIndex = 0;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        TraSet = Editor.LoadSetFromFile(openFileDialog.FileName);
                        CurrentTraPath = openFileDialog.FileName;
                        Hint.Text = "译文已加载！";

                        FILE_TRA.Text = TraSet.FileName;
                        LANG_TRA.Text = TraSet.Language;

                        bool IsOK = DataViewTool.SetTraData(DataView, Editor.GetAllData(TraSet.Data));
                        if (!IsOK) throw new Exception();
                        if (CurrentTraPath == CurrentOriPath) throw new Exception();
                    }
                    catch
                    {
                        TraSet = new Localization.LanguageSet();
                        CurrentTraPath = string.Empty;
                        Hint.Text = "译文加载失败！";

                        FILE_TRA.Text = "未加载";
                        LANG_TRA.Text = "未加载";

                        for (int i = 0; i < DataView.RowCount; i++)
                        {
                            DataView.Rows[i].Cells[2].Value = null;
                        }
                    }
                }



            }

        }

        private void SAVE_ORI_Click(object sender, EventArgs e)
        {
            if (CurrentOriPath == string.Empty && CurrentTraPath == string.Empty)
            {
                Hint.Text = "请先加载原文！";
                return;
            }
            string HintText = "已保存";

            try
            {
                if (CurrentOriPath != string.Empty)
                {
                    Editor.SaveSetToFile(DataViewTool.GetOriData(DataView), OriSet.Language, CurrentOriPath);
                    HintText += " 原文 ";
                }
                if (CurrentTraPath != string.Empty)
                {
                    Editor.SaveSetToFile(DataViewTool.GetTraData(DataView), TraSet.Language, CurrentTraPath);
                    HintText += " 和译文 ";
                }
                Hint.Text = HintText;
            }
            catch
            {
                Hint.Text = "保存失败!";
            }
        }

        private void SAVE_TRA_Click(object sender, EventArgs e)
        {

            if (CurrentTraPath == string.Empty)
            {
                Hint.Text = "未加载译文!";
            }
            else
            {
                try
                {
                    Editor.SaveSetToFile(DataViewTool.GetTraData(DataView), TraSet.Language, CurrentTraPath);
                    Hint.Text = "保存成功!" + DateTime.Now;
                }
                catch
                {
                    Hint.Text = "保存失败!";
                }



            }
        }
    }
}
