using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yourapp.data
{
    public static class LoadData
    {
       // public static MyClass m_myclass = new MyClass();
        public static void LoadTheData()
        {
            m_my_class=(MyClass) DataManaginigMethods.SetUpAndLoadDataFile<MyClass>("filename", "hash", m_myclass);
        }
        public static void saveData() {
            DataManaginigMethods.SaveSomeData(m_myclass, "filename", "hash");
        }
    }
}
