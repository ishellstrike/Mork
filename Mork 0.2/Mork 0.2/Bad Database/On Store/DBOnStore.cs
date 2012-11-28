using System.Collections.Generic;

namespace Mork
{
    public class DBOnStore
    {
        public Dictionary<OnStoreID, OnStoreData> data = new Dictionary<OnStoreID, OnStoreData>();

        public DBOnStore()
        {
            data.Add(OnStoreID.Nothing, new OnStoreData(null, "������", "", "", ""));
            data.Add(OnStoreID.WoodLog, new OnStoreData(Main.onstore_tex[Main.OnStoreTexes.Wood_log], "������", "��", "", ""));
            data.Add(OnStoreID.StoneBoulder, new OnStoreData(Main.onstore_tex[Main.OnStoreTexes.Stone], "�����", "��", "", ""));
        }
    }
}