using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhitelistedEraser {
    static class Util {
        public static void AssignCheckedListBoxItems(CheckedListBox box, IList items, IList checkedItems) {
            AssignList(box.Items, items);
            for (int i = 0; i < items.Count; i++) {
                box.SetItemChecked(i, checkedItems.Contains(items[i]));
            }
        }

        public static void AssignList(IList list, IList items) {
            while (list.Count > items.Count)
                list.RemoveAt(list.Count - 1);
            for (int i = 0; i < list.Count; i++) {
                list[i] = items[i];
            }
            for (int i = list.Count; i < items.Count; i++)
                list.Add(items[i]);
        }

        public static void AssignGenericList<T>(IList<T> list, IList<T> items) {
            while (list.Count > items.Count)
                list.RemoveAt(list.Count - 1);
            for (int i = 0; i < list.Count; i++) {
                list[i] = items[i];
            }
            for (int i = list.Count; i < items.Count; i++)
                list.Add(items[i]);
        }
    }
}
