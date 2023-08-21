// <auto-generated />
#pragma warning disable CS0105
using MagicT.Shared.Models.MemoryDatabaseModels;
using MasterMemory.Validation;
using MasterMemory;
using MessagePack;
using System.Collections.Generic;
using System;

namespace MagicT.Shared.Tables
{
   public sealed partial class UsersTable : TableBase<Users>, ITableUniqueValidate
   {
        public Func<Users, int> PrimaryKeySelector => primaryIndexSelector;
        readonly Func<Users, int> primaryIndexSelector;

        readonly Users[] secondaryIndex2;
        readonly Func<Users, string> secondaryIndex2Selector;
        readonly Users[] secondaryIndex1;
        readonly Func<Users, byte[]> secondaryIndex1Selector;

        public UsersTable(Users[] sortedData)
            : base(sortedData)
        {
            this.primaryIndexSelector = x => x.UserId;
            this.secondaryIndex2Selector = x => x.ContactIdentifier;
            this.secondaryIndex2 = CloneAndSortBy(this.secondaryIndex2Selector, System.StringComparer.Ordinal);
            this.secondaryIndex1Selector = x => x.SharedKey;
            this.secondaryIndex1 = CloneAndSortBy(this.secondaryIndex1Selector, System.Collections.Generic.Comparer<byte[]>.Default);
            OnAfterConstruct();
        }

        partial void OnAfterConstruct();

        public RangeView<Users> SortByContactIdentifier => new RangeView<Users>(secondaryIndex2, 0, secondaryIndex2.Length - 1, true);
        public RangeView<Users> SortBySharedKey => new RangeView<Users>(secondaryIndex1, 0, secondaryIndex1.Length - 1, true);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Users FindByUserId(int key)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].UserId;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { return data[mid]; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            return default;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryFindByUserId(int key, out Users result)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].UserId;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { result = data[mid]; return true; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            result = default;
            return false;
        }

        public Users FindClosestByUserId(int key, bool selectLower = true)
        {
            return FindUniqueClosestCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<int>.Default, key, selectLower);
        }

        public RangeView<Users> FindRangeByUserId(int min, int max, bool ascendant = true)
        {
            return FindUniqueRangeCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<int>.Default, min, max, ascendant);
        }

        public Users FindByContactIdentifier(string key)
        {
            return FindUniqueCore(secondaryIndex2, secondaryIndex2Selector, System.StringComparer.Ordinal, key, false);
        }
        
        public bool TryFindByContactIdentifier(string key, out Users result)
        {
            return TryFindUniqueCore(secondaryIndex2, secondaryIndex2Selector, System.StringComparer.Ordinal, key, out result);
        }

        public Users FindClosestByContactIdentifier(string key, bool selectLower = true)
        {
            return FindUniqueClosestCore(secondaryIndex2, secondaryIndex2Selector, System.StringComparer.Ordinal, key, selectLower);
        }

        public RangeView<Users> FindRangeByContactIdentifier(string min, string max, bool ascendant = true)
        {
            return FindUniqueRangeCore(secondaryIndex2, secondaryIndex2Selector, System.StringComparer.Ordinal, min, max, ascendant);
        }

        public Users FindBySharedKey(byte[] key)
        {
            return FindUniqueCore(secondaryIndex1, secondaryIndex1Selector, System.Collections.Generic.Comparer<byte[]>.Default, key, false);
        }
        
        public bool TryFindBySharedKey(byte[] key, out Users result)
        {
            return TryFindUniqueCore(secondaryIndex1, secondaryIndex1Selector, System.Collections.Generic.Comparer<byte[]>.Default, key, out result);
        }

        public Users FindClosestBySharedKey(byte[] key, bool selectLower = true)
        {
            return FindUniqueClosestCore(secondaryIndex1, secondaryIndex1Selector, System.Collections.Generic.Comparer<byte[]>.Default, key, selectLower);
        }

        public RangeView<Users> FindRangeBySharedKey(byte[] min, byte[] max, bool ascendant = true)
        {
            return FindUniqueRangeCore(secondaryIndex1, secondaryIndex1Selector, System.Collections.Generic.Comparer<byte[]>.Default, min, max, ascendant);
        }


        void ITableUniqueValidate.ValidateUnique(ValidateResult resultSet)
        {
#if !DISABLE_MASTERMEMORY_VALIDATOR

            ValidateUniqueCore(data, primaryIndexSelector, "UserId", resultSet);       
            ValidateUniqueCore(secondaryIndex2, secondaryIndex2Selector, "ContactIdentifier", resultSet);       
            ValidateUniqueCore(secondaryIndex1, secondaryIndex1Selector, "SharedKey", resultSet);       

#endif
        }

#if !DISABLE_MASTERMEMORY_METADATABASE

        public static MasterMemory.Meta.MetaTable CreateMetaTable()
        {
            return new MasterMemory.Meta.MetaTable(typeof(Users), typeof(UsersTable), "Users",
                new MasterMemory.Meta.MetaProperty[]
                {
                    new MasterMemory.Meta.MetaProperty(typeof(Users).GetProperty("UserId")),
                    new MasterMemory.Meta.MetaProperty(typeof(Users).GetProperty("ContactIdentifier")),
                    new MasterMemory.Meta.MetaProperty(typeof(Users).GetProperty("SharedKey")),
                },
                new MasterMemory.Meta.MetaIndex[]{
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Users).GetProperty("UserId"),
                    }, true, true, System.Collections.Generic.Comparer<int>.Default),
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Users).GetProperty("ContactIdentifier"),
                    }, false, true, System.StringComparer.Ordinal),
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Users).GetProperty("SharedKey"),
                    }, false, true, System.Collections.Generic.Comparer<byte[]>.Default),
                });
        }

#endif
    }
}