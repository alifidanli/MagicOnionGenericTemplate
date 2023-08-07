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
   public sealed partial class PermissionsTable : TableBase<Permissions>, ITableUniqueValidate
   {
        public Func<Permissions, int> PrimaryKeySelector => primaryIndexSelector;
        readonly Func<Permissions, int> primaryIndexSelector;

        readonly Permissions[] secondaryIndex0;
        readonly Func<Permissions, int> secondaryIndex0Selector;
        readonly Permissions[] secondaryIndex1;
        readonly Func<Permissions, string> secondaryIndex1Selector;

        public PermissionsTable(Permissions[] sortedData)
            : base(sortedData)
        {
            this.primaryIndexSelector = x => x.Id;
            this.secondaryIndex0Selector = x => x.UserRefNo;
            this.secondaryIndex0 = CloneAndSortBy(this.secondaryIndex0Selector, System.Collections.Generic.Comparer<int>.Default);
            this.secondaryIndex1Selector = x => x.AuthType;
            this.secondaryIndex1 = CloneAndSortBy(this.secondaryIndex1Selector, System.StringComparer.Ordinal);
            OnAfterConstruct();
        }

        partial void OnAfterConstruct();

        public RangeView<Permissions> SortByUserRefNo => new RangeView<Permissions>(secondaryIndex0, 0, secondaryIndex0.Length - 1, true);
        public RangeView<Permissions> SortByAuthType => new RangeView<Permissions>(secondaryIndex1, 0, secondaryIndex1.Length - 1, true);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Permissions FindById(int key)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].Id;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { return data[mid]; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            return default;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryFindById(int key, out Permissions result)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].Id;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { result = data[mid]; return true; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            result = default;
            return false;
        }

        public Permissions FindClosestById(int key, bool selectLower = true)
        {
            return FindUniqueClosestCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<int>.Default, key, selectLower);
        }

        public RangeView<Permissions> FindRangeById(int min, int max, bool ascendant = true)
        {
            return FindUniqueRangeCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<int>.Default, min, max, ascendant);
        }

        public Permissions FindByUserRefNo(int key)
        {
            return FindUniqueCoreInt(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<int>.Default, key, false);
        }
        
        public bool TryFindByUserRefNo(int key, out Permissions result)
        {
            return TryFindUniqueCoreInt(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<int>.Default, key, out result);
        }

        public Permissions FindClosestByUserRefNo(int key, bool selectLower = true)
        {
            return FindUniqueClosestCore(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<int>.Default, key, selectLower);
        }

        public RangeView<Permissions> FindRangeByUserRefNo(int min, int max, bool ascendant = true)
        {
            return FindUniqueRangeCore(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<int>.Default, min, max, ascendant);
        }

        public Permissions FindByAuthType(string key)
        {
            return FindUniqueCore(secondaryIndex1, secondaryIndex1Selector, System.StringComparer.Ordinal, key, false);
        }
        
        public bool TryFindByAuthType(string key, out Permissions result)
        {
            return TryFindUniqueCore(secondaryIndex1, secondaryIndex1Selector, System.StringComparer.Ordinal, key, out result);
        }

        public Permissions FindClosestByAuthType(string key, bool selectLower = true)
        {
            return FindUniqueClosestCore(secondaryIndex1, secondaryIndex1Selector, System.StringComparer.Ordinal, key, selectLower);
        }

        public RangeView<Permissions> FindRangeByAuthType(string min, string max, bool ascendant = true)
        {
            return FindUniqueRangeCore(secondaryIndex1, secondaryIndex1Selector, System.StringComparer.Ordinal, min, max, ascendant);
        }


        void ITableUniqueValidate.ValidateUnique(ValidateResult resultSet)
        {
#if !DISABLE_MASTERMEMORY_VALIDATOR

            ValidateUniqueCore(data, primaryIndexSelector, "Id", resultSet);       
            ValidateUniqueCore(secondaryIndex0, secondaryIndex0Selector, "UserRefNo", resultSet);       
            ValidateUniqueCore(secondaryIndex1, secondaryIndex1Selector, "AuthType", resultSet);       

#endif
        }

#if !DISABLE_MASTERMEMORY_METADATABASE

        public static MasterMemory.Meta.MetaTable CreateMetaTable()
        {
            return new MasterMemory.Meta.MetaTable(typeof(Permissions), typeof(PermissionsTable), "Permissions",
                new MasterMemory.Meta.MetaProperty[]
                {
                    new MasterMemory.Meta.MetaProperty(typeof(Permissions).GetProperty("Id")),
                    new MasterMemory.Meta.MetaProperty(typeof(Permissions).GetProperty("UserRefNo")),
                    new MasterMemory.Meta.MetaProperty(typeof(Permissions).GetProperty("AuthType")),
                },
                new MasterMemory.Meta.MetaIndex[]{
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Permissions).GetProperty("Id"),
                    }, true, true, System.Collections.Generic.Comparer<int>.Default),
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Permissions).GetProperty("UserRefNo"),
                    }, false, true, System.Collections.Generic.Comparer<int>.Default),
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Permissions).GetProperty("AuthType"),
                    }, false, true, System.StringComparer.Ordinal),
                });
        }

#endif
    }
}