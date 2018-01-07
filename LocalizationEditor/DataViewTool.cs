using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnityStandardUtils;

namespace LocalizationEditor
{
    public static class DataViewTool
    {
        public static void SetOriData(DataGridView view, List<Editor.LocalizationDataPair> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                view.Rows.Add();
                view.Rows[i].Cells[0].Value = data[i].ID;
                view.Rows[i].Cells[1].Value = data[i].Text;
            }
        }
        public static bool SetTraData(DataGridView view, List<Editor.LocalizationDataPair> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if ((string)view.Rows[i].Cells[0].Value == data[i].ID)
                    view.Rows[i].Cells[2].Value = data[i].Text;
                else return false;
            }
            return true;
        }

        public static List<Editor.LocalizationDataPair> GetOriData(DataGridView view)
        {
            List<Editor.LocalizationDataPair> ldp = new List<Editor.LocalizationDataPair>();

            int TotalRowCount = (view.AllowUserToAddRows) ? view.RowCount - 1 : view.RowCount;

            for(int i = 0; i < TotalRowCount; i++)
            {
                Editor.LocalizationDataPair ld = new Editor.LocalizationDataPair();
                ld.ID = (string)view.Rows[i].Cells[0].Value;
                ld.Text = (string)view.Rows[i].Cells[1].Value;
                if(ld.ID!=string.Empty)ldp.Add(ld);
            }

            return ldp;
        }

        public static List<Editor.LocalizationDataPair> GetTraData(DataGridView view)
        {
            List<Editor.LocalizationDataPair> ldp = new List<Editor.LocalizationDataPair>();

            int TotalRowCount = (view.AllowUserToAddRows) ? view.RowCount - 1 : view.RowCount;

            for (int i = 0; i < TotalRowCount; i++)
            {
                Editor.LocalizationDataPair ld = new Editor.LocalizationDataPair();
                ld.ID = (string)view.Rows[i].Cells[0].Value;
                ld.Text = (string)view.Rows[i].Cells[2].Value;
                if (ld.ID != string.Empty) ldp.Add(ld);
            }

            return ldp;
        }

    }
}
