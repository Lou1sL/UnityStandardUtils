using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityStandardUtils
{

    /// <summary>
    /// 物品栏管理
    /// </summary>
    public class InventoryManager
    {

        private static Item EmptyItem = new Item();
        /// <summary>
        /// 物品
        /// </summary>
        public class Item
        {
            internal string Name = "";
            internal string Discription = "";
            internal ushort Weight = 1;

            public Item() { }

            public Item(string name)
            {
                Name = name;
            }
            public Item(string name,string discription)
            {
                Name = name;
                Discription = discription;
            }
            public Item(string name, string discription,ushort weight)
            {
                Name = name;
                Discription = discription;
                Weight = weight;
            }

            /// <summary>
            /// 空物品
            /// </summary>
            public static Item Empty
            {
                get
                {
                    return EmptyItem;
                }
            }
        }
        
        /// <summary>
        /// 组合
        /// </summary>
        public class Combination
        {
            public ushort MaterialA;
            public ushort MaterialB;

            public ushort Product;
        }

        //物品列
        internal List<Item> GlobalItemList = new List<Item>();
        //组合列
        internal List<Combination> CombinationList = new List<Combination>();

        /// <summary>
        /// 添加一个物品
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(Item item)
        {
            GlobalItemList.Add(item);
        }

        public void AddItem(Item[] items)
        {
            foreach (Item item in items) GlobalItemList.Add(item);
        }

        /// <summary>
        /// 添加一个组合
        /// </summary>
        /// <param name="combination"></param>
        public void AddCombination(Combination combination)
        {
            CombinationList.Add(combination);
        }

        public void AddCombination(Combination[] combinations)
        {
            foreach (Combination combination in combinations)
                CombinationList.Add(combination);
        }

        /// <summary>
        /// 获取两个下标组合后的下标值
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public int Combine(ushort A,ushort B)
        {
            foreach(Combination combination in CombinationList)
            {
                if (A == combination.MaterialA && B == combination.MaterialB) return combination.Product;
            }
            return -1;
        }


        /// <summary>
        /// 包
        /// </summary>
        public class Bag
        {
            private ushort Capacity;
            private List<ushort> Container = new List<ushort>();

            private InventoryManager ivtMgr;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="capacity">包容积</param>
            /// <param name="inventoryManager">适用的物品管理器</param>
            public Bag(ushort capacity,InventoryManager inventoryManager)
            {
                Capacity = capacity;
                ivtMgr = inventoryManager;
            }

            /// <summary>
            /// 给包中添加物品
            /// </summary>
            /// <param name="positionInGlobalList">物品在GlobalList的下标</param>
            /// <returns></returns>
            public bool Push(ushort positionInGlobalList)
            {
                if (positionInGlobalList >= ivtMgr.GlobalItemList.Count) return false;

                Item item = ivtMgr.GlobalItemList[positionInGlobalList];
                if (WeightCal() + item.Weight > Capacity) return false;

                Container.Add(positionInGlobalList);
                return true;
            }
            /// <summary>
            /// 删除包中对应位置物品
            /// </summary>
            /// <param name="position">物品在包中的下标</param>
            /// <returns></returns>
            public bool Pop(ushort position)
            {
                if (position >= Container.Count) return false;

                Container.RemoveAt(position);
                return true;
            }
            /// <summary>
            /// 删除包中特定物品
            /// </summary>
            /// <param name="positionInGlobalList">物品在GlobalList的下标</param>
            /// <returns></returns>
            public bool PopByGlobalPosition(ushort positionInGlobalList)
            {
                if (positionInGlobalList >= ivtMgr.GlobalItemList.Count) return false;
                return Container.Remove(positionInGlobalList);
            }
            /// <summary>
            /// 获取包中对应位置物品
            /// </summary>
            /// <param name="position">物品在包中的下标</param>
            /// <returns></returns>
            public Item GetItemByPosition(ushort position)
            {
                if (position >= Container.Count) return Item.Empty;
                else return ivtMgr.GlobalItemList[Container[position]];
            }

            /// <summary>
            /// 组合物品，并自动删除包中原有物品添加新物品
            /// </summary>
            /// <param name="p1">物品1在包中的下标</param>
            /// <param name="p2">物品2在包中的下标</param>
            /// <returns>是否成功</returns>
            public bool TryCombineThenPush(ushort p1,ushort p2)
            {
                if (p1 >= Container.Count || p2 >= Container.Count) return false;
                int result = ivtMgr.Combine(Container[p1], Container[p2]);
                if (result < 0) return false;


                ushort p1Weight = ivtMgr.GlobalItemList[Container[p1]].Weight;
                ushort p2Weight = ivtMgr.GlobalItemList[Container[p2]].Weight;
                ushort resultWeight = ivtMgr.GlobalItemList[result].Weight;

                if (WeightCal()-p1Weight-p2Weight+resultWeight > Capacity) return false;

                Pop(p1);
                Pop(p2);
                Push((ushort)result);
                return true;
            }

            /// <summary>
            /// 计算包当前重量
            /// </summary>
            /// <returns></returns>
            private ushort WeightCal()
            {
                ushort rtn = 0;
                foreach(ushort item in Container)
                {
                    rtn += ivtMgr.GlobalItemList[item].Weight;
                }
                return rtn;
            }
        }


    }


}
