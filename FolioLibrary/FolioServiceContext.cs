using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FolioLibrary
{
    public class FolioServiceContext : IDisposable
    {
        public FolioServiceClient FolioServiceClient { get; set; }
        Dictionary<Guid, object> objects = new Dictionary<Guid, object>();

        public FolioServiceContext(string nameOrConnectionString = "FolioServiceClient", string accessToken = null) => FolioServiceClient = new FolioServiceClient(nameOrConnectionString, accessToken);

        public bool AnyAcquisitionsUnit2s(string where = null) => FolioServiceClient.AnyAcquisitionsUnits(where);

        public int CountAcquisitionsUnit2s(string where = null) => FolioServiceClient.CountAcquisitionsUnits(where);

        public AcquisitionsUnit2[] AcquisitionsUnit2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.AcquisitionsUnits(out count, where, orderBy, skip, take).Select(jo =>
            {
                var au2 = AcquisitionsUnit2.FromJObject(jo);
                if (load && au2.CreationUserId != null) au2.CreationUser = (User2)(cache && objects.ContainsKey(au2.CreationUserId.Value) ? objects[au2.CreationUserId.Value] : objects[au2.CreationUserId.Value] = FindUser2(au2.CreationUserId));
                if (load && au2.LastWriteUserId != null) au2.LastWriteUser = (User2)(cache && objects.ContainsKey(au2.LastWriteUserId.Value) ? objects[au2.LastWriteUserId.Value] : objects[au2.LastWriteUserId.Value] = FindUser2(au2.LastWriteUserId));
                return au2;
            }).ToArray();
        }

        public IEnumerable<AcquisitionsUnit2> AcquisitionsUnit2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.AcquisitionsUnits(where, orderBy, skip, take))
            {
                var au2 = AcquisitionsUnit2.FromJObject(jo);
                if (load && au2.CreationUserId != null) au2.CreationUser = (User2)(cache && objects.ContainsKey(au2.CreationUserId.Value) ? objects[au2.CreationUserId.Value] : objects[au2.CreationUserId.Value] = FindUser2(au2.CreationUserId));
                if (load && au2.LastWriteUserId != null) au2.LastWriteUser = (User2)(cache && objects.ContainsKey(au2.LastWriteUserId.Value) ? objects[au2.LastWriteUserId.Value] : objects[au2.LastWriteUserId.Value] = FindUser2(au2.LastWriteUserId));
                yield return au2;
            }
        }

        public AcquisitionsUnit2 FindAcquisitionsUnit2(Guid? id, bool load = false, bool cache = true)
        {
            var au2 = AcquisitionsUnit2.FromJObject(FolioServiceClient.GetAcquisitionsUnit(id?.ToString()));
            if (au2 == null) return null;
            if (load && au2.CreationUserId != null) au2.CreationUser = (User2)(cache && objects.ContainsKey(au2.CreationUserId.Value) ? objects[au2.CreationUserId.Value] : objects[au2.CreationUserId.Value] = FindUser2(au2.CreationUserId));
            if (load && au2.LastWriteUserId != null) au2.LastWriteUser = (User2)(cache && objects.ContainsKey(au2.LastWriteUserId.Value) ? objects[au2.LastWriteUserId.Value] : objects[au2.LastWriteUserId.Value] = FindUser2(au2.LastWriteUserId));
            return au2;
        }

        public void Insert(AcquisitionsUnit2 acquisitionsUnit2)
        {
            if (acquisitionsUnit2.Id == null) acquisitionsUnit2.Id = Guid.NewGuid();
            FolioServiceClient.InsertAcquisitionsUnit(acquisitionsUnit2.ToJObject());
        }

        public void Update(AcquisitionsUnit2 acquisitionsUnit2) => FolioServiceClient.UpdateAcquisitionsUnit(acquisitionsUnit2.ToJObject());

        public void DeleteAcquisitionsUnit2(Guid? id) => FolioServiceClient.DeleteAcquisitionsUnit(id?.ToString());

        public bool AnyAddressType2s(string where = null) => FolioServiceClient.AnyAddressTypes(where);

        public int CountAddressType2s(string where = null) => FolioServiceClient.CountAddressTypes(where);

        public AddressType2[] AddressType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.AddressTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var at2 = AddressType2.FromJObject(jo);
                if (load && at2.CreationUserId != null) at2.CreationUser = (User2)(cache && objects.ContainsKey(at2.CreationUserId.Value) ? objects[at2.CreationUserId.Value] : objects[at2.CreationUserId.Value] = FindUser2(at2.CreationUserId));
                if (load && at2.LastWriteUserId != null) at2.LastWriteUser = (User2)(cache && objects.ContainsKey(at2.LastWriteUserId.Value) ? objects[at2.LastWriteUserId.Value] : objects[at2.LastWriteUserId.Value] = FindUser2(at2.LastWriteUserId));
                return at2;
            }).ToArray();
        }

        public IEnumerable<AddressType2> AddressType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.AddressTypes(where, orderBy, skip, take))
            {
                var at2 = AddressType2.FromJObject(jo);
                if (load && at2.CreationUserId != null) at2.CreationUser = (User2)(cache && objects.ContainsKey(at2.CreationUserId.Value) ? objects[at2.CreationUserId.Value] : objects[at2.CreationUserId.Value] = FindUser2(at2.CreationUserId));
                if (load && at2.LastWriteUserId != null) at2.LastWriteUser = (User2)(cache && objects.ContainsKey(at2.LastWriteUserId.Value) ? objects[at2.LastWriteUserId.Value] : objects[at2.LastWriteUserId.Value] = FindUser2(at2.LastWriteUserId));
                yield return at2;
            }
        }

        public AddressType2 FindAddressType2(Guid? id, bool load = false, bool cache = true)
        {
            var at2 = AddressType2.FromJObject(FolioServiceClient.GetAddressType(id?.ToString()));
            if (at2 == null) return null;
            if (load && at2.CreationUserId != null) at2.CreationUser = (User2)(cache && objects.ContainsKey(at2.CreationUserId.Value) ? objects[at2.CreationUserId.Value] : objects[at2.CreationUserId.Value] = FindUser2(at2.CreationUserId));
            if (load && at2.LastWriteUserId != null) at2.LastWriteUser = (User2)(cache && objects.ContainsKey(at2.LastWriteUserId.Value) ? objects[at2.LastWriteUserId.Value] : objects[at2.LastWriteUserId.Value] = FindUser2(at2.LastWriteUserId));
            return at2;
        }

        public void Insert(AddressType2 addressType2)
        {
            if (addressType2.Id == null) addressType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertAddressType(addressType2.ToJObject());
        }

        public void Update(AddressType2 addressType2) => FolioServiceClient.UpdateAddressType(addressType2.ToJObject());

        public void DeleteAddressType2(Guid? id) => FolioServiceClient.DeleteAddressType(id?.ToString());

        public bool AnyAlert2s(string where = null) => FolioServiceClient.AnyAlerts(where);

        public int CountAlert2s(string where = null) => FolioServiceClient.CountAlerts(where);

        public Alert2[] Alert2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Alerts(out count, where, orderBy, skip, take).Select(jo =>
            {
                var a2 = Alert2.FromJObject(jo);
                return a2;
            }).ToArray();
        }

        public IEnumerable<Alert2> Alert2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Alerts(where, orderBy, skip, take))
            {
                var a2 = Alert2.FromJObject(jo);
                yield return a2;
            }
        }

        public Alert2 FindAlert2(Guid? id, bool load = false, bool cache = true) => Alert2.FromJObject(FolioServiceClient.GetAlert(id?.ToString()));

        public void Insert(Alert2 alert2)
        {
            if (alert2.Id == null) alert2.Id = Guid.NewGuid();
            FolioServiceClient.InsertAlert(alert2.ToJObject());
        }

        public void Update(Alert2 alert2) => FolioServiceClient.UpdateAlert(alert2.ToJObject());

        public void DeleteAlert2(Guid? id) => FolioServiceClient.DeleteAlert(id?.ToString());

        public bool AnyAlternativeTitleType2s(string where = null) => FolioServiceClient.AnyAlternativeTitleTypes(where);

        public int CountAlternativeTitleType2s(string where = null) => FolioServiceClient.CountAlternativeTitleTypes(where);

        public AlternativeTitleType2[] AlternativeTitleType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.AlternativeTitleTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var att2 = AlternativeTitleType2.FromJObject(jo);
                if (load && att2.CreationUserId != null) att2.CreationUser = (User2)(cache && objects.ContainsKey(att2.CreationUserId.Value) ? objects[att2.CreationUserId.Value] : objects[att2.CreationUserId.Value] = FindUser2(att2.CreationUserId));
                if (load && att2.LastWriteUserId != null) att2.LastWriteUser = (User2)(cache && objects.ContainsKey(att2.LastWriteUserId.Value) ? objects[att2.LastWriteUserId.Value] : objects[att2.LastWriteUserId.Value] = FindUser2(att2.LastWriteUserId));
                return att2;
            }).ToArray();
        }

        public IEnumerable<AlternativeTitleType2> AlternativeTitleType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.AlternativeTitleTypes(where, orderBy, skip, take))
            {
                var att2 = AlternativeTitleType2.FromJObject(jo);
                if (load && att2.CreationUserId != null) att2.CreationUser = (User2)(cache && objects.ContainsKey(att2.CreationUserId.Value) ? objects[att2.CreationUserId.Value] : objects[att2.CreationUserId.Value] = FindUser2(att2.CreationUserId));
                if (load && att2.LastWriteUserId != null) att2.LastWriteUser = (User2)(cache && objects.ContainsKey(att2.LastWriteUserId.Value) ? objects[att2.LastWriteUserId.Value] : objects[att2.LastWriteUserId.Value] = FindUser2(att2.LastWriteUserId));
                yield return att2;
            }
        }

        public AlternativeTitleType2 FindAlternativeTitleType2(Guid? id, bool load = false, bool cache = true)
        {
            var att2 = AlternativeTitleType2.FromJObject(FolioServiceClient.GetAlternativeTitleType(id?.ToString()));
            if (att2 == null) return null;
            if (load && att2.CreationUserId != null) att2.CreationUser = (User2)(cache && objects.ContainsKey(att2.CreationUserId.Value) ? objects[att2.CreationUserId.Value] : objects[att2.CreationUserId.Value] = FindUser2(att2.CreationUserId));
            if (load && att2.LastWriteUserId != null) att2.LastWriteUser = (User2)(cache && objects.ContainsKey(att2.LastWriteUserId.Value) ? objects[att2.LastWriteUserId.Value] : objects[att2.LastWriteUserId.Value] = FindUser2(att2.LastWriteUserId));
            return att2;
        }

        public void Insert(AlternativeTitleType2 alternativeTitleType2)
        {
            if (alternativeTitleType2.Id == null) alternativeTitleType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertAlternativeTitleType(alternativeTitleType2.ToJObject());
        }

        public void Update(AlternativeTitleType2 alternativeTitleType2) => FolioServiceClient.UpdateAlternativeTitleType(alternativeTitleType2.ToJObject());

        public void DeleteAlternativeTitleType2(Guid? id) => FolioServiceClient.DeleteAlternativeTitleType(id?.ToString());

        public bool AnyBatchGroup2s(string where = null) => FolioServiceClient.AnyBatchGroups(where);

        public int CountBatchGroup2s(string where = null) => FolioServiceClient.CountBatchGroups(where);

        public BatchGroup2[] BatchGroup2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.BatchGroups(out count, where, orderBy, skip, take).Select(jo =>
            {
                var bg2 = BatchGroup2.FromJObject(jo);
                if (load && bg2.CreationUserId != null) bg2.CreationUser = (User2)(cache && objects.ContainsKey(bg2.CreationUserId.Value) ? objects[bg2.CreationUserId.Value] : objects[bg2.CreationUserId.Value] = FindUser2(bg2.CreationUserId));
                if (load && bg2.LastWriteUserId != null) bg2.LastWriteUser = (User2)(cache && objects.ContainsKey(bg2.LastWriteUserId.Value) ? objects[bg2.LastWriteUserId.Value] : objects[bg2.LastWriteUserId.Value] = FindUser2(bg2.LastWriteUserId));
                return bg2;
            }).ToArray();
        }

        public IEnumerable<BatchGroup2> BatchGroup2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.BatchGroups(where, orderBy, skip, take))
            {
                var bg2 = BatchGroup2.FromJObject(jo);
                if (load && bg2.CreationUserId != null) bg2.CreationUser = (User2)(cache && objects.ContainsKey(bg2.CreationUserId.Value) ? objects[bg2.CreationUserId.Value] : objects[bg2.CreationUserId.Value] = FindUser2(bg2.CreationUserId));
                if (load && bg2.LastWriteUserId != null) bg2.LastWriteUser = (User2)(cache && objects.ContainsKey(bg2.LastWriteUserId.Value) ? objects[bg2.LastWriteUserId.Value] : objects[bg2.LastWriteUserId.Value] = FindUser2(bg2.LastWriteUserId));
                yield return bg2;
            }
        }

        public BatchGroup2 FindBatchGroup2(Guid? id, bool load = false, bool cache = true)
        {
            var bg2 = BatchGroup2.FromJObject(FolioServiceClient.GetBatchGroup(id?.ToString()));
            if (bg2 == null) return null;
            if (load && bg2.CreationUserId != null) bg2.CreationUser = (User2)(cache && objects.ContainsKey(bg2.CreationUserId.Value) ? objects[bg2.CreationUserId.Value] : objects[bg2.CreationUserId.Value] = FindUser2(bg2.CreationUserId));
            if (load && bg2.LastWriteUserId != null) bg2.LastWriteUser = (User2)(cache && objects.ContainsKey(bg2.LastWriteUserId.Value) ? objects[bg2.LastWriteUserId.Value] : objects[bg2.LastWriteUserId.Value] = FindUser2(bg2.LastWriteUserId));
            return bg2;
        }

        public void Insert(BatchGroup2 batchGroup2)
        {
            if (batchGroup2.Id == null) batchGroup2.Id = Guid.NewGuid();
            FolioServiceClient.InsertBatchGroup(batchGroup2.ToJObject());
        }

        public void Update(BatchGroup2 batchGroup2) => FolioServiceClient.UpdateBatchGroup(batchGroup2.ToJObject());

        public void DeleteBatchGroup2(Guid? id) => FolioServiceClient.DeleteBatchGroup(id?.ToString());

        public bool AnyBatchVoucherExport2s(string where = null) => FolioServiceClient.AnyBatchVoucherExports(where);

        public int CountBatchVoucherExport2s(string where = null) => FolioServiceClient.CountBatchVoucherExports(where);

        public BatchVoucherExport2[] BatchVoucherExport2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.BatchVoucherExports(out count, where, orderBy, skip, take).Select(jo =>
            {
                var bve2 = BatchVoucherExport2.FromJObject(jo);
                if (load && bve2.BatchGroupId != null) bve2.BatchGroup = (BatchGroup2)(cache && objects.ContainsKey(bve2.BatchGroupId.Value) ? objects[bve2.BatchGroupId.Value] : objects[bve2.BatchGroupId.Value] = FindBatchGroup2(bve2.BatchGroupId));
                if (load && bve2.CreationUserId != null) bve2.CreationUser = (User2)(cache && objects.ContainsKey(bve2.CreationUserId.Value) ? objects[bve2.CreationUserId.Value] : objects[bve2.CreationUserId.Value] = FindUser2(bve2.CreationUserId));
                if (load && bve2.LastWriteUserId != null) bve2.LastWriteUser = (User2)(cache && objects.ContainsKey(bve2.LastWriteUserId.Value) ? objects[bve2.LastWriteUserId.Value] : objects[bve2.LastWriteUserId.Value] = FindUser2(bve2.LastWriteUserId));
                return bve2;
            }).ToArray();
        }

        public IEnumerable<BatchVoucherExport2> BatchVoucherExport2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.BatchVoucherExports(where, orderBy, skip, take))
            {
                var bve2 = BatchVoucherExport2.FromJObject(jo);
                if (load && bve2.BatchGroupId != null) bve2.BatchGroup = (BatchGroup2)(cache && objects.ContainsKey(bve2.BatchGroupId.Value) ? objects[bve2.BatchGroupId.Value] : objects[bve2.BatchGroupId.Value] = FindBatchGroup2(bve2.BatchGroupId));
                if (load && bve2.CreationUserId != null) bve2.CreationUser = (User2)(cache && objects.ContainsKey(bve2.CreationUserId.Value) ? objects[bve2.CreationUserId.Value] : objects[bve2.CreationUserId.Value] = FindUser2(bve2.CreationUserId));
                if (load && bve2.LastWriteUserId != null) bve2.LastWriteUser = (User2)(cache && objects.ContainsKey(bve2.LastWriteUserId.Value) ? objects[bve2.LastWriteUserId.Value] : objects[bve2.LastWriteUserId.Value] = FindUser2(bve2.LastWriteUserId));
                yield return bve2;
            }
        }

        public BatchVoucherExport2 FindBatchVoucherExport2(Guid? id, bool load = false, bool cache = true)
        {
            var bve2 = BatchVoucherExport2.FromJObject(FolioServiceClient.GetBatchVoucherExport(id?.ToString()));
            if (bve2 == null) return null;
            if (load && bve2.BatchGroupId != null) bve2.BatchGroup = (BatchGroup2)(cache && objects.ContainsKey(bve2.BatchGroupId.Value) ? objects[bve2.BatchGroupId.Value] : objects[bve2.BatchGroupId.Value] = FindBatchGroup2(bve2.BatchGroupId));
            if (load && bve2.CreationUserId != null) bve2.CreationUser = (User2)(cache && objects.ContainsKey(bve2.CreationUserId.Value) ? objects[bve2.CreationUserId.Value] : objects[bve2.CreationUserId.Value] = FindUser2(bve2.CreationUserId));
            if (load && bve2.LastWriteUserId != null) bve2.LastWriteUser = (User2)(cache && objects.ContainsKey(bve2.LastWriteUserId.Value) ? objects[bve2.LastWriteUserId.Value] : objects[bve2.LastWriteUserId.Value] = FindUser2(bve2.LastWriteUserId));
            return bve2;
        }

        public void Insert(BatchVoucherExport2 batchVoucherExport2)
        {
            if (batchVoucherExport2.Id == null) batchVoucherExport2.Id = Guid.NewGuid();
            FolioServiceClient.InsertBatchVoucherExport(batchVoucherExport2.ToJObject());
        }

        public void Update(BatchVoucherExport2 batchVoucherExport2) => FolioServiceClient.UpdateBatchVoucherExport(batchVoucherExport2.ToJObject());

        public void DeleteBatchVoucherExport2(Guid? id) => FolioServiceClient.DeleteBatchVoucherExport(id?.ToString());

        public bool AnyBatchVoucherExportConfig2s(string where = null) => FolioServiceClient.AnyBatchVoucherExportConfigs(where);

        public int CountBatchVoucherExportConfig2s(string where = null) => FolioServiceClient.CountBatchVoucherExportConfigs(where);

        public BatchVoucherExportConfig2[] BatchVoucherExportConfig2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.BatchVoucherExportConfigs(out count, where, orderBy, skip, take).Select(jo =>
            {
                var bvec2 = BatchVoucherExportConfig2.FromJObject(jo);
                if (load && bvec2.BatchGroupId != null) bvec2.BatchGroup = (BatchGroup2)(cache && objects.ContainsKey(bvec2.BatchGroupId.Value) ? objects[bvec2.BatchGroupId.Value] : objects[bvec2.BatchGroupId.Value] = FindBatchGroup2(bvec2.BatchGroupId));
                if (load && bvec2.CreationUserId != null) bvec2.CreationUser = (User2)(cache && objects.ContainsKey(bvec2.CreationUserId.Value) ? objects[bvec2.CreationUserId.Value] : objects[bvec2.CreationUserId.Value] = FindUser2(bvec2.CreationUserId));
                if (load && bvec2.LastWriteUserId != null) bvec2.LastWriteUser = (User2)(cache && objects.ContainsKey(bvec2.LastWriteUserId.Value) ? objects[bvec2.LastWriteUserId.Value] : objects[bvec2.LastWriteUserId.Value] = FindUser2(bvec2.LastWriteUserId));
                return bvec2;
            }).ToArray();
        }

        public IEnumerable<BatchVoucherExportConfig2> BatchVoucherExportConfig2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.BatchVoucherExportConfigs(where, orderBy, skip, take))
            {
                var bvec2 = BatchVoucherExportConfig2.FromJObject(jo);
                if (load && bvec2.BatchGroupId != null) bvec2.BatchGroup = (BatchGroup2)(cache && objects.ContainsKey(bvec2.BatchGroupId.Value) ? objects[bvec2.BatchGroupId.Value] : objects[bvec2.BatchGroupId.Value] = FindBatchGroup2(bvec2.BatchGroupId));
                if (load && bvec2.CreationUserId != null) bvec2.CreationUser = (User2)(cache && objects.ContainsKey(bvec2.CreationUserId.Value) ? objects[bvec2.CreationUserId.Value] : objects[bvec2.CreationUserId.Value] = FindUser2(bvec2.CreationUserId));
                if (load && bvec2.LastWriteUserId != null) bvec2.LastWriteUser = (User2)(cache && objects.ContainsKey(bvec2.LastWriteUserId.Value) ? objects[bvec2.LastWriteUserId.Value] : objects[bvec2.LastWriteUserId.Value] = FindUser2(bvec2.LastWriteUserId));
                yield return bvec2;
            }
        }

        public BatchVoucherExportConfig2 FindBatchVoucherExportConfig2(Guid? id, bool load = false, bool cache = true)
        {
            var bvec2 = BatchVoucherExportConfig2.FromJObject(FolioServiceClient.GetBatchVoucherExportConfig(id?.ToString()));
            if (bvec2 == null) return null;
            if (load && bvec2.BatchGroupId != null) bvec2.BatchGroup = (BatchGroup2)(cache && objects.ContainsKey(bvec2.BatchGroupId.Value) ? objects[bvec2.BatchGroupId.Value] : objects[bvec2.BatchGroupId.Value] = FindBatchGroup2(bvec2.BatchGroupId));
            if (load && bvec2.CreationUserId != null) bvec2.CreationUser = (User2)(cache && objects.ContainsKey(bvec2.CreationUserId.Value) ? objects[bvec2.CreationUserId.Value] : objects[bvec2.CreationUserId.Value] = FindUser2(bvec2.CreationUserId));
            if (load && bvec2.LastWriteUserId != null) bvec2.LastWriteUser = (User2)(cache && objects.ContainsKey(bvec2.LastWriteUserId.Value) ? objects[bvec2.LastWriteUserId.Value] : objects[bvec2.LastWriteUserId.Value] = FindUser2(bvec2.LastWriteUserId));
            var i = 0;
            if (bvec2.BatchVoucherExportConfigWeekdays != null) foreach (var bvecw in bvec2.BatchVoucherExportConfigWeekdays)
                {
                    bvecw.Id = (++i).ToString();
                    bvecw.BatchVoucherExportConfigId = bvec2.Id;
                }
            return bvec2;
        }

        public void Insert(BatchVoucherExportConfig2 batchVoucherExportConfig2)
        {
            if (batchVoucherExportConfig2.Id == null) batchVoucherExportConfig2.Id = Guid.NewGuid();
            FolioServiceClient.InsertBatchVoucherExportConfig(batchVoucherExportConfig2.ToJObject());
        }

        public void Update(BatchVoucherExportConfig2 batchVoucherExportConfig2) => FolioServiceClient.UpdateBatchVoucherExportConfig(batchVoucherExportConfig2.ToJObject());

        public void DeleteBatchVoucherExportConfig2(Guid? id) => FolioServiceClient.DeleteBatchVoucherExportConfig(id?.ToString());

        public bool AnyBlock2s(string where = null) => FolioServiceClient.AnyBlocks(where);

        public int CountBlock2s(string where = null) => FolioServiceClient.CountBlocks(where);

        public Block2[] Block2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Blocks(out count, where, orderBy, skip, take).Select(jo =>
            {
                var b2 = Block2.FromJObject(jo);
                if (load && b2.UserId != null) b2.User = (User2)(cache && objects.ContainsKey(b2.UserId.Value) ? objects[b2.UserId.Value] : objects[b2.UserId.Value] = FindUser2(b2.UserId));
                if (load && b2.CreationUserId != null) b2.CreationUser = (User2)(cache && objects.ContainsKey(b2.CreationUserId.Value) ? objects[b2.CreationUserId.Value] : objects[b2.CreationUserId.Value] = FindUser2(b2.CreationUserId));
                if (load && b2.LastWriteUserId != null) b2.LastWriteUser = (User2)(cache && objects.ContainsKey(b2.LastWriteUserId.Value) ? objects[b2.LastWriteUserId.Value] : objects[b2.LastWriteUserId.Value] = FindUser2(b2.LastWriteUserId));
                return b2;
            }).ToArray();
        }

        public IEnumerable<Block2> Block2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Blocks(where, orderBy, skip, take))
            {
                var b2 = Block2.FromJObject(jo);
                if (load && b2.UserId != null) b2.User = (User2)(cache && objects.ContainsKey(b2.UserId.Value) ? objects[b2.UserId.Value] : objects[b2.UserId.Value] = FindUser2(b2.UserId));
                if (load && b2.CreationUserId != null) b2.CreationUser = (User2)(cache && objects.ContainsKey(b2.CreationUserId.Value) ? objects[b2.CreationUserId.Value] : objects[b2.CreationUserId.Value] = FindUser2(b2.CreationUserId));
                if (load && b2.LastWriteUserId != null) b2.LastWriteUser = (User2)(cache && objects.ContainsKey(b2.LastWriteUserId.Value) ? objects[b2.LastWriteUserId.Value] : objects[b2.LastWriteUserId.Value] = FindUser2(b2.LastWriteUserId));
                yield return b2;
            }
        }

        public Block2 FindBlock2(Guid? id, bool load = false, bool cache = true)
        {
            var b2 = Block2.FromJObject(FolioServiceClient.GetBlock(id?.ToString()));
            if (b2 == null) return null;
            if (load && b2.UserId != null) b2.User = (User2)(cache && objects.ContainsKey(b2.UserId.Value) ? objects[b2.UserId.Value] : objects[b2.UserId.Value] = FindUser2(b2.UserId));
            if (load && b2.CreationUserId != null) b2.CreationUser = (User2)(cache && objects.ContainsKey(b2.CreationUserId.Value) ? objects[b2.CreationUserId.Value] : objects[b2.CreationUserId.Value] = FindUser2(b2.CreationUserId));
            if (load && b2.LastWriteUserId != null) b2.LastWriteUser = (User2)(cache && objects.ContainsKey(b2.LastWriteUserId.Value) ? objects[b2.LastWriteUserId.Value] : objects[b2.LastWriteUserId.Value] = FindUser2(b2.LastWriteUserId));
            return b2;
        }

        public void Insert(Block2 block2)
        {
            if (block2.Id == null) block2.Id = Guid.NewGuid();
            FolioServiceClient.InsertBlock(block2.ToJObject());
        }

        public void Update(Block2 block2) => FolioServiceClient.UpdateBlock(block2.ToJObject());

        public void DeleteBlock2(Guid? id) => FolioServiceClient.DeleteBlock(id?.ToString());

        public bool AnyBlockCondition2s(string where = null) => FolioServiceClient.AnyBlockConditions(where);

        public int CountBlockCondition2s(string where = null) => FolioServiceClient.CountBlockConditions(where);

        public BlockCondition2[] BlockCondition2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.BlockConditions(out count, where, orderBy, skip, take).Select(jo =>
            {
                var bc2 = BlockCondition2.FromJObject(jo);
                if (load && bc2.CreationUserId != null) bc2.CreationUser = (User2)(cache && objects.ContainsKey(bc2.CreationUserId.Value) ? objects[bc2.CreationUserId.Value] : objects[bc2.CreationUserId.Value] = FindUser2(bc2.CreationUserId));
                if (load && bc2.LastWriteUserId != null) bc2.LastWriteUser = (User2)(cache && objects.ContainsKey(bc2.LastWriteUserId.Value) ? objects[bc2.LastWriteUserId.Value] : objects[bc2.LastWriteUserId.Value] = FindUser2(bc2.LastWriteUserId));
                return bc2;
            }).ToArray();
        }

        public IEnumerable<BlockCondition2> BlockCondition2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.BlockConditions(where, orderBy, skip, take))
            {
                var bc2 = BlockCondition2.FromJObject(jo);
                if (load && bc2.CreationUserId != null) bc2.CreationUser = (User2)(cache && objects.ContainsKey(bc2.CreationUserId.Value) ? objects[bc2.CreationUserId.Value] : objects[bc2.CreationUserId.Value] = FindUser2(bc2.CreationUserId));
                if (load && bc2.LastWriteUserId != null) bc2.LastWriteUser = (User2)(cache && objects.ContainsKey(bc2.LastWriteUserId.Value) ? objects[bc2.LastWriteUserId.Value] : objects[bc2.LastWriteUserId.Value] = FindUser2(bc2.LastWriteUserId));
                yield return bc2;
            }
        }

        public BlockCondition2 FindBlockCondition2(Guid? id, bool load = false, bool cache = true)
        {
            var bc2 = BlockCondition2.FromJObject(FolioServiceClient.GetBlockCondition(id?.ToString()));
            if (bc2 == null) return null;
            if (load && bc2.CreationUserId != null) bc2.CreationUser = (User2)(cache && objects.ContainsKey(bc2.CreationUserId.Value) ? objects[bc2.CreationUserId.Value] : objects[bc2.CreationUserId.Value] = FindUser2(bc2.CreationUserId));
            if (load && bc2.LastWriteUserId != null) bc2.LastWriteUser = (User2)(cache && objects.ContainsKey(bc2.LastWriteUserId.Value) ? objects[bc2.LastWriteUserId.Value] : objects[bc2.LastWriteUserId.Value] = FindUser2(bc2.LastWriteUserId));
            return bc2;
        }

        public void Insert(BlockCondition2 blockCondition2)
        {
            if (blockCondition2.Id == null) blockCondition2.Id = Guid.NewGuid();
            FolioServiceClient.InsertBlockCondition(blockCondition2.ToJObject());
        }

        public void Update(BlockCondition2 blockCondition2) => FolioServiceClient.UpdateBlockCondition(blockCondition2.ToJObject());

        public void DeleteBlockCondition2(Guid? id) => FolioServiceClient.DeleteBlockCondition(id?.ToString());

        public bool AnyBlockLimit2s(string where = null) => FolioServiceClient.AnyBlockLimits(where);

        public int CountBlockLimit2s(string where = null) => FolioServiceClient.CountBlockLimits(where);

        public BlockLimit2[] BlockLimit2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.BlockLimits(out count, where, orderBy, skip, take).Select(jo =>
            {
                var bl2 = BlockLimit2.FromJObject(jo);
                if (load && bl2.GroupId != null) bl2.Group = (Group2)(cache && objects.ContainsKey(bl2.GroupId.Value) ? objects[bl2.GroupId.Value] : objects[bl2.GroupId.Value] = FindGroup2(bl2.GroupId));
                if (load && bl2.ConditionId != null) bl2.Condition = (BlockCondition2)(cache && objects.ContainsKey(bl2.ConditionId.Value) ? objects[bl2.ConditionId.Value] : objects[bl2.ConditionId.Value] = FindBlockCondition2(bl2.ConditionId));
                if (load && bl2.CreationUserId != null) bl2.CreationUser = (User2)(cache && objects.ContainsKey(bl2.CreationUserId.Value) ? objects[bl2.CreationUserId.Value] : objects[bl2.CreationUserId.Value] = FindUser2(bl2.CreationUserId));
                if (load && bl2.LastWriteUserId != null) bl2.LastWriteUser = (User2)(cache && objects.ContainsKey(bl2.LastWriteUserId.Value) ? objects[bl2.LastWriteUserId.Value] : objects[bl2.LastWriteUserId.Value] = FindUser2(bl2.LastWriteUserId));
                return bl2;
            }).ToArray();
        }

        public IEnumerable<BlockLimit2> BlockLimit2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.BlockLimits(where, orderBy, skip, take))
            {
                var bl2 = BlockLimit2.FromJObject(jo);
                if (load && bl2.GroupId != null) bl2.Group = (Group2)(cache && objects.ContainsKey(bl2.GroupId.Value) ? objects[bl2.GroupId.Value] : objects[bl2.GroupId.Value] = FindGroup2(bl2.GroupId));
                if (load && bl2.ConditionId != null) bl2.Condition = (BlockCondition2)(cache && objects.ContainsKey(bl2.ConditionId.Value) ? objects[bl2.ConditionId.Value] : objects[bl2.ConditionId.Value] = FindBlockCondition2(bl2.ConditionId));
                if (load && bl2.CreationUserId != null) bl2.CreationUser = (User2)(cache && objects.ContainsKey(bl2.CreationUserId.Value) ? objects[bl2.CreationUserId.Value] : objects[bl2.CreationUserId.Value] = FindUser2(bl2.CreationUserId));
                if (load && bl2.LastWriteUserId != null) bl2.LastWriteUser = (User2)(cache && objects.ContainsKey(bl2.LastWriteUserId.Value) ? objects[bl2.LastWriteUserId.Value] : objects[bl2.LastWriteUserId.Value] = FindUser2(bl2.LastWriteUserId));
                yield return bl2;
            }
        }

        public BlockLimit2 FindBlockLimit2(Guid? id, bool load = false, bool cache = true)
        {
            var bl2 = BlockLimit2.FromJObject(FolioServiceClient.GetBlockLimit(id?.ToString()));
            if (bl2 == null) return null;
            if (load && bl2.GroupId != null) bl2.Group = (Group2)(cache && objects.ContainsKey(bl2.GroupId.Value) ? objects[bl2.GroupId.Value] : objects[bl2.GroupId.Value] = FindGroup2(bl2.GroupId));
            if (load && bl2.ConditionId != null) bl2.Condition = (BlockCondition2)(cache && objects.ContainsKey(bl2.ConditionId.Value) ? objects[bl2.ConditionId.Value] : objects[bl2.ConditionId.Value] = FindBlockCondition2(bl2.ConditionId));
            if (load && bl2.CreationUserId != null) bl2.CreationUser = (User2)(cache && objects.ContainsKey(bl2.CreationUserId.Value) ? objects[bl2.CreationUserId.Value] : objects[bl2.CreationUserId.Value] = FindUser2(bl2.CreationUserId));
            if (load && bl2.LastWriteUserId != null) bl2.LastWriteUser = (User2)(cache && objects.ContainsKey(bl2.LastWriteUserId.Value) ? objects[bl2.LastWriteUserId.Value] : objects[bl2.LastWriteUserId.Value] = FindUser2(bl2.LastWriteUserId));
            return bl2;
        }

        public void Insert(BlockLimit2 blockLimit2)
        {
            if (blockLimit2.Id == null) blockLimit2.Id = Guid.NewGuid();
            FolioServiceClient.InsertBlockLimit(blockLimit2.ToJObject());
        }

        public void Update(BlockLimit2 blockLimit2) => FolioServiceClient.UpdateBlockLimit(blockLimit2.ToJObject());

        public void DeleteBlockLimit2(Guid? id) => FolioServiceClient.DeleteBlockLimit(id?.ToString());

        public bool AnyBoundWithPart2s(string where = null) => FolioServiceClient.AnyBoundWithParts(where);

        public int CountBoundWithPart2s(string where = null) => FolioServiceClient.CountBoundWithParts(where);

        public BoundWithPart2[] BoundWithPart2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.BoundWithParts(out count, where, orderBy, skip, take).Select(jo =>
            {
                var bwp2 = BoundWithPart2.FromJObject(jo);
                if (load && bwp2.HoldingId != null) bwp2.Holding = (Holding2)(cache && objects.ContainsKey(bwp2.HoldingId.Value) ? objects[bwp2.HoldingId.Value] : objects[bwp2.HoldingId.Value] = FindHolding2(bwp2.HoldingId));
                if (load && bwp2.ItemId != null) bwp2.Item = (Item2)(cache && objects.ContainsKey(bwp2.ItemId.Value) ? objects[bwp2.ItemId.Value] : objects[bwp2.ItemId.Value] = FindItem2(bwp2.ItemId));
                if (load && bwp2.CreationUserId != null) bwp2.CreationUser = (User2)(cache && objects.ContainsKey(bwp2.CreationUserId.Value) ? objects[bwp2.CreationUserId.Value] : objects[bwp2.CreationUserId.Value] = FindUser2(bwp2.CreationUserId));
                if (load && bwp2.LastWriteUserId != null) bwp2.LastWriteUser = (User2)(cache && objects.ContainsKey(bwp2.LastWriteUserId.Value) ? objects[bwp2.LastWriteUserId.Value] : objects[bwp2.LastWriteUserId.Value] = FindUser2(bwp2.LastWriteUserId));
                return bwp2;
            }).ToArray();
        }

        public IEnumerable<BoundWithPart2> BoundWithPart2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.BoundWithParts(where, orderBy, skip, take))
            {
                var bwp2 = BoundWithPart2.FromJObject(jo);
                if (load && bwp2.HoldingId != null) bwp2.Holding = (Holding2)(cache && objects.ContainsKey(bwp2.HoldingId.Value) ? objects[bwp2.HoldingId.Value] : objects[bwp2.HoldingId.Value] = FindHolding2(bwp2.HoldingId));
                if (load && bwp2.ItemId != null) bwp2.Item = (Item2)(cache && objects.ContainsKey(bwp2.ItemId.Value) ? objects[bwp2.ItemId.Value] : objects[bwp2.ItemId.Value] = FindItem2(bwp2.ItemId));
                if (load && bwp2.CreationUserId != null) bwp2.CreationUser = (User2)(cache && objects.ContainsKey(bwp2.CreationUserId.Value) ? objects[bwp2.CreationUserId.Value] : objects[bwp2.CreationUserId.Value] = FindUser2(bwp2.CreationUserId));
                if (load && bwp2.LastWriteUserId != null) bwp2.LastWriteUser = (User2)(cache && objects.ContainsKey(bwp2.LastWriteUserId.Value) ? objects[bwp2.LastWriteUserId.Value] : objects[bwp2.LastWriteUserId.Value] = FindUser2(bwp2.LastWriteUserId));
                yield return bwp2;
            }
        }

        public BoundWithPart2 FindBoundWithPart2(Guid? id, bool load = false, bool cache = true)
        {
            var bwp2 = BoundWithPart2.FromJObject(FolioServiceClient.GetBoundWithPart(id?.ToString()));
            if (bwp2 == null) return null;
            if (load && bwp2.HoldingId != null) bwp2.Holding = (Holding2)(cache && objects.ContainsKey(bwp2.HoldingId.Value) ? objects[bwp2.HoldingId.Value] : objects[bwp2.HoldingId.Value] = FindHolding2(bwp2.HoldingId));
            if (load && bwp2.ItemId != null) bwp2.Item = (Item2)(cache && objects.ContainsKey(bwp2.ItemId.Value) ? objects[bwp2.ItemId.Value] : objects[bwp2.ItemId.Value] = FindItem2(bwp2.ItemId));
            if (load && bwp2.CreationUserId != null) bwp2.CreationUser = (User2)(cache && objects.ContainsKey(bwp2.CreationUserId.Value) ? objects[bwp2.CreationUserId.Value] : objects[bwp2.CreationUserId.Value] = FindUser2(bwp2.CreationUserId));
            if (load && bwp2.LastWriteUserId != null) bwp2.LastWriteUser = (User2)(cache && objects.ContainsKey(bwp2.LastWriteUserId.Value) ? objects[bwp2.LastWriteUserId.Value] : objects[bwp2.LastWriteUserId.Value] = FindUser2(bwp2.LastWriteUserId));
            return bwp2;
        }

        public void Insert(BoundWithPart2 boundWithPart2)
        {
            if (boundWithPart2.Id == null) boundWithPart2.Id = Guid.NewGuid();
            FolioServiceClient.InsertBoundWithPart(boundWithPart2.ToJObject());
        }

        public void Update(BoundWithPart2 boundWithPart2) => FolioServiceClient.UpdateBoundWithPart(boundWithPart2.ToJObject());

        public void DeleteBoundWithPart2(Guid? id) => FolioServiceClient.DeleteBoundWithPart(id?.ToString());

        public bool AnyBudget2s(string where = null) => FolioServiceClient.AnyBudgets(where);

        public int CountBudget2s(string where = null) => FolioServiceClient.CountBudgets(where);

        public Budget2[] Budget2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Budgets(out count, where, orderBy, skip, take).Select(jo =>
            {
                var b2 = Budget2.FromJObject(jo);
                if (load && b2.FundId != null) b2.Fund = (Fund2)(cache && objects.ContainsKey(b2.FundId.Value) ? objects[b2.FundId.Value] : objects[b2.FundId.Value] = FindFund2(b2.FundId));
                if (load && b2.FiscalYearId != null) b2.FiscalYear = (FiscalYear2)(cache && objects.ContainsKey(b2.FiscalYearId.Value) ? objects[b2.FiscalYearId.Value] : objects[b2.FiscalYearId.Value] = FindFiscalYear2(b2.FiscalYearId));
                if (load && b2.CreationUserId != null) b2.CreationUser = (User2)(cache && objects.ContainsKey(b2.CreationUserId.Value) ? objects[b2.CreationUserId.Value] : objects[b2.CreationUserId.Value] = FindUser2(b2.CreationUserId));
                if (load && b2.LastWriteUserId != null) b2.LastWriteUser = (User2)(cache && objects.ContainsKey(b2.LastWriteUserId.Value) ? objects[b2.LastWriteUserId.Value] : objects[b2.LastWriteUserId.Value] = FindUser2(b2.LastWriteUserId));
                return b2;
            }).ToArray();
        }

        public IEnumerable<Budget2> Budget2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Budgets(where, orderBy, skip, take))
            {
                var b2 = Budget2.FromJObject(jo);
                if (load && b2.FundId != null) b2.Fund = (Fund2)(cache && objects.ContainsKey(b2.FundId.Value) ? objects[b2.FundId.Value] : objects[b2.FundId.Value] = FindFund2(b2.FundId));
                if (load && b2.FiscalYearId != null) b2.FiscalYear = (FiscalYear2)(cache && objects.ContainsKey(b2.FiscalYearId.Value) ? objects[b2.FiscalYearId.Value] : objects[b2.FiscalYearId.Value] = FindFiscalYear2(b2.FiscalYearId));
                if (load && b2.CreationUserId != null) b2.CreationUser = (User2)(cache && objects.ContainsKey(b2.CreationUserId.Value) ? objects[b2.CreationUserId.Value] : objects[b2.CreationUserId.Value] = FindUser2(b2.CreationUserId));
                if (load && b2.LastWriteUserId != null) b2.LastWriteUser = (User2)(cache && objects.ContainsKey(b2.LastWriteUserId.Value) ? objects[b2.LastWriteUserId.Value] : objects[b2.LastWriteUserId.Value] = FindUser2(b2.LastWriteUserId));
                yield return b2;
            }
        }

        public Budget2 FindBudget2(Guid? id, bool load = false, bool cache = true)
        {
            var b2 = Budget2.FromJObject(FolioServiceClient.GetBudget(id?.ToString()));
            if (b2 == null) return null;
            if (load && b2.FundId != null) b2.Fund = (Fund2)(cache && objects.ContainsKey(b2.FundId.Value) ? objects[b2.FundId.Value] : objects[b2.FundId.Value] = FindFund2(b2.FundId));
            if (load && b2.FiscalYearId != null) b2.FiscalYear = (FiscalYear2)(cache && objects.ContainsKey(b2.FiscalYearId.Value) ? objects[b2.FiscalYearId.Value] : objects[b2.FiscalYearId.Value] = FindFiscalYear2(b2.FiscalYearId));
            if (load && b2.CreationUserId != null) b2.CreationUser = (User2)(cache && objects.ContainsKey(b2.CreationUserId.Value) ? objects[b2.CreationUserId.Value] : objects[b2.CreationUserId.Value] = FindUser2(b2.CreationUserId));
            if (load && b2.LastWriteUserId != null) b2.LastWriteUser = (User2)(cache && objects.ContainsKey(b2.LastWriteUserId.Value) ? objects[b2.LastWriteUserId.Value] : objects[b2.LastWriteUserId.Value] = FindUser2(b2.LastWriteUserId));
            var i = 0;
            if (b2.BudgetAcquisitionsUnits != null) foreach (var bau in b2.BudgetAcquisitionsUnits)
                {
                    bau.Id = (++i).ToString();
                    bau.BudgetId = b2.Id;
                }
            i = 0;
            if (b2.BudgetTags != null) foreach (var bt in b2.BudgetTags)
                {
                    bt.Id = (++i).ToString();
                    bt.BudgetId = b2.Id;
                }
            return b2;
        }

        public void Insert(Budget2 budget2)
        {
            if (budget2.Id == null) budget2.Id = Guid.NewGuid();
            FolioServiceClient.InsertBudget(budget2.ToJObject());
        }

        public void Update(Budget2 budget2) => FolioServiceClient.UpdateBudget(budget2.ToJObject());

        public void DeleteBudget2(Guid? id) => FolioServiceClient.DeleteBudget(id?.ToString());

        public bool AnyBudgetExpenseClass2s(string where = null) => FolioServiceClient.AnyBudgetExpenseClasses(where);

        public int CountBudgetExpenseClass2s(string where = null) => FolioServiceClient.CountBudgetExpenseClasses(where);

        public BudgetExpenseClass2[] BudgetExpenseClass2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.BudgetExpenseClasses(out count, where, orderBy, skip, take).Select(jo =>
            {
                var bec2 = BudgetExpenseClass2.FromJObject(jo);
                if (load && bec2.BudgetId != null) bec2.Budget = (Budget2)(cache && objects.ContainsKey(bec2.BudgetId.Value) ? objects[bec2.BudgetId.Value] : objects[bec2.BudgetId.Value] = FindBudget2(bec2.BudgetId));
                if (load && bec2.ExpenseClassId != null) bec2.ExpenseClass = (ExpenseClass2)(cache && objects.ContainsKey(bec2.ExpenseClassId.Value) ? objects[bec2.ExpenseClassId.Value] : objects[bec2.ExpenseClassId.Value] = FindExpenseClass2(bec2.ExpenseClassId));
                return bec2;
            }).ToArray();
        }

        public IEnumerable<BudgetExpenseClass2> BudgetExpenseClass2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.BudgetExpenseClasses(where, orderBy, skip, take))
            {
                var bec2 = BudgetExpenseClass2.FromJObject(jo);
                if (load && bec2.BudgetId != null) bec2.Budget = (Budget2)(cache && objects.ContainsKey(bec2.BudgetId.Value) ? objects[bec2.BudgetId.Value] : objects[bec2.BudgetId.Value] = FindBudget2(bec2.BudgetId));
                if (load && bec2.ExpenseClassId != null) bec2.ExpenseClass = (ExpenseClass2)(cache && objects.ContainsKey(bec2.ExpenseClassId.Value) ? objects[bec2.ExpenseClassId.Value] : objects[bec2.ExpenseClassId.Value] = FindExpenseClass2(bec2.ExpenseClassId));
                yield return bec2;
            }
        }

        public BudgetExpenseClass2 FindBudgetExpenseClass2(Guid? id, bool load = false, bool cache = true)
        {
            var bec2 = BudgetExpenseClass2.FromJObject(FolioServiceClient.GetBudgetExpenseClass(id?.ToString()));
            if (bec2 == null) return null;
            if (load && bec2.BudgetId != null) bec2.Budget = (Budget2)(cache && objects.ContainsKey(bec2.BudgetId.Value) ? objects[bec2.BudgetId.Value] : objects[bec2.BudgetId.Value] = FindBudget2(bec2.BudgetId));
            if (load && bec2.ExpenseClassId != null) bec2.ExpenseClass = (ExpenseClass2)(cache && objects.ContainsKey(bec2.ExpenseClassId.Value) ? objects[bec2.ExpenseClassId.Value] : objects[bec2.ExpenseClassId.Value] = FindExpenseClass2(bec2.ExpenseClassId));
            return bec2;
        }

        public void Insert(BudgetExpenseClass2 budgetExpenseClass2)
        {
            if (budgetExpenseClass2.Id == null) budgetExpenseClass2.Id = Guid.NewGuid();
            FolioServiceClient.InsertBudgetExpenseClass(budgetExpenseClass2.ToJObject());
        }

        public void Update(BudgetExpenseClass2 budgetExpenseClass2) => FolioServiceClient.UpdateBudgetExpenseClass(budgetExpenseClass2.ToJObject());

        public void DeleteBudgetExpenseClass2(Guid? id) => FolioServiceClient.DeleteBudgetExpenseClass(id?.ToString());

        public bool AnyBudgetGroup2s(string where = null) => FolioServiceClient.AnyBudgetGroups(where);

        public int CountBudgetGroup2s(string where = null) => FolioServiceClient.CountBudgetGroups(where);

        public BudgetGroup2[] BudgetGroup2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.BudgetGroups(out count, where, orderBy, skip, take).Select(jo =>
            {
                var bg2 = BudgetGroup2.FromJObject(jo);
                if (load && bg2.BudgetId != null) bg2.Budget = (Budget2)(cache && objects.ContainsKey(bg2.BudgetId.Value) ? objects[bg2.BudgetId.Value] : objects[bg2.BudgetId.Value] = FindBudget2(bg2.BudgetId));
                if (load && bg2.GroupId != null) bg2.Group = (FinanceGroup2)(cache && objects.ContainsKey(bg2.GroupId.Value) ? objects[bg2.GroupId.Value] : objects[bg2.GroupId.Value] = FindFinanceGroup2(bg2.GroupId));
                if (load && bg2.FiscalYearId != null) bg2.FiscalYear = (FiscalYear2)(cache && objects.ContainsKey(bg2.FiscalYearId.Value) ? objects[bg2.FiscalYearId.Value] : objects[bg2.FiscalYearId.Value] = FindFiscalYear2(bg2.FiscalYearId));
                if (load && bg2.FundId != null) bg2.Fund = (Fund2)(cache && objects.ContainsKey(bg2.FundId.Value) ? objects[bg2.FundId.Value] : objects[bg2.FundId.Value] = FindFund2(bg2.FundId));
                return bg2;
            }).ToArray();
        }

        public IEnumerable<BudgetGroup2> BudgetGroup2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.BudgetGroups(where, orderBy, skip, take))
            {
                var bg2 = BudgetGroup2.FromJObject(jo);
                if (load && bg2.BudgetId != null) bg2.Budget = (Budget2)(cache && objects.ContainsKey(bg2.BudgetId.Value) ? objects[bg2.BudgetId.Value] : objects[bg2.BudgetId.Value] = FindBudget2(bg2.BudgetId));
                if (load && bg2.GroupId != null) bg2.Group = (FinanceGroup2)(cache && objects.ContainsKey(bg2.GroupId.Value) ? objects[bg2.GroupId.Value] : objects[bg2.GroupId.Value] = FindFinanceGroup2(bg2.GroupId));
                if (load && bg2.FiscalYearId != null) bg2.FiscalYear = (FiscalYear2)(cache && objects.ContainsKey(bg2.FiscalYearId.Value) ? objects[bg2.FiscalYearId.Value] : objects[bg2.FiscalYearId.Value] = FindFiscalYear2(bg2.FiscalYearId));
                if (load && bg2.FundId != null) bg2.Fund = (Fund2)(cache && objects.ContainsKey(bg2.FundId.Value) ? objects[bg2.FundId.Value] : objects[bg2.FundId.Value] = FindFund2(bg2.FundId));
                yield return bg2;
            }
        }

        public BudgetGroup2 FindBudgetGroup2(Guid? id, bool load = false, bool cache = true)
        {
            var bg2 = BudgetGroup2.FromJObject(FolioServiceClient.GetBudgetGroup(id?.ToString()));
            if (bg2 == null) return null;
            if (load && bg2.BudgetId != null) bg2.Budget = (Budget2)(cache && objects.ContainsKey(bg2.BudgetId.Value) ? objects[bg2.BudgetId.Value] : objects[bg2.BudgetId.Value] = FindBudget2(bg2.BudgetId));
            if (load && bg2.GroupId != null) bg2.Group = (FinanceGroup2)(cache && objects.ContainsKey(bg2.GroupId.Value) ? objects[bg2.GroupId.Value] : objects[bg2.GroupId.Value] = FindFinanceGroup2(bg2.GroupId));
            if (load && bg2.FiscalYearId != null) bg2.FiscalYear = (FiscalYear2)(cache && objects.ContainsKey(bg2.FiscalYearId.Value) ? objects[bg2.FiscalYearId.Value] : objects[bg2.FiscalYearId.Value] = FindFiscalYear2(bg2.FiscalYearId));
            if (load && bg2.FundId != null) bg2.Fund = (Fund2)(cache && objects.ContainsKey(bg2.FundId.Value) ? objects[bg2.FundId.Value] : objects[bg2.FundId.Value] = FindFund2(bg2.FundId));
            return bg2;
        }

        public void Insert(BudgetGroup2 budgetGroup2)
        {
            if (budgetGroup2.Id == null) budgetGroup2.Id = Guid.NewGuid();
            FolioServiceClient.InsertBudgetGroup(budgetGroup2.ToJObject());
        }

        public void Update(BudgetGroup2 budgetGroup2) => FolioServiceClient.UpdateBudgetGroup(budgetGroup2.ToJObject());

        public void DeleteBudgetGroup2(Guid? id) => FolioServiceClient.DeleteBudgetGroup(id?.ToString());

        public bool AnyCallNumberType2s(string where = null) => FolioServiceClient.AnyCallNumberTypes(where);

        public int CountCallNumberType2s(string where = null) => FolioServiceClient.CountCallNumberTypes(where);

        public CallNumberType2[] CallNumberType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.CallNumberTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var cnt2 = CallNumberType2.FromJObject(jo);
                if (load && cnt2.CreationUserId != null) cnt2.CreationUser = (User2)(cache && objects.ContainsKey(cnt2.CreationUserId.Value) ? objects[cnt2.CreationUserId.Value] : objects[cnt2.CreationUserId.Value] = FindUser2(cnt2.CreationUserId));
                if (load && cnt2.LastWriteUserId != null) cnt2.LastWriteUser = (User2)(cache && objects.ContainsKey(cnt2.LastWriteUserId.Value) ? objects[cnt2.LastWriteUserId.Value] : objects[cnt2.LastWriteUserId.Value] = FindUser2(cnt2.LastWriteUserId));
                return cnt2;
            }).ToArray();
        }

        public IEnumerable<CallNumberType2> CallNumberType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.CallNumberTypes(where, orderBy, skip, take))
            {
                var cnt2 = CallNumberType2.FromJObject(jo);
                if (load && cnt2.CreationUserId != null) cnt2.CreationUser = (User2)(cache && objects.ContainsKey(cnt2.CreationUserId.Value) ? objects[cnt2.CreationUserId.Value] : objects[cnt2.CreationUserId.Value] = FindUser2(cnt2.CreationUserId));
                if (load && cnt2.LastWriteUserId != null) cnt2.LastWriteUser = (User2)(cache && objects.ContainsKey(cnt2.LastWriteUserId.Value) ? objects[cnt2.LastWriteUserId.Value] : objects[cnt2.LastWriteUserId.Value] = FindUser2(cnt2.LastWriteUserId));
                yield return cnt2;
            }
        }

        public CallNumberType2 FindCallNumberType2(Guid? id, bool load = false, bool cache = true)
        {
            var cnt2 = CallNumberType2.FromJObject(FolioServiceClient.GetCallNumberType(id?.ToString()));
            if (cnt2 == null) return null;
            if (load && cnt2.CreationUserId != null) cnt2.CreationUser = (User2)(cache && objects.ContainsKey(cnt2.CreationUserId.Value) ? objects[cnt2.CreationUserId.Value] : objects[cnt2.CreationUserId.Value] = FindUser2(cnt2.CreationUserId));
            if (load && cnt2.LastWriteUserId != null) cnt2.LastWriteUser = (User2)(cache && objects.ContainsKey(cnt2.LastWriteUserId.Value) ? objects[cnt2.LastWriteUserId.Value] : objects[cnt2.LastWriteUserId.Value] = FindUser2(cnt2.LastWriteUserId));
            return cnt2;
        }

        public void Insert(CallNumberType2 callNumberType2)
        {
            if (callNumberType2.Id == null) callNumberType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertCallNumberType(callNumberType2.ToJObject());
        }

        public void Update(CallNumberType2 callNumberType2) => FolioServiceClient.UpdateCallNumberType(callNumberType2.ToJObject());

        public void DeleteCallNumberType2(Guid? id) => FolioServiceClient.DeleteCallNumberType(id?.ToString());

        public bool AnyCampus2s(string where = null) => FolioServiceClient.AnyCampuses(where);

        public int CountCampus2s(string where = null) => FolioServiceClient.CountCampuses(where);

        public Campus2[] Campus2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Campuses(out count, where, orderBy, skip, take).Select(jo =>
            {
                var c2 = Campus2.FromJObject(jo);
                if (load && c2.InstitutionId != null) c2.Institution = (Institution2)(cache && objects.ContainsKey(c2.InstitutionId.Value) ? objects[c2.InstitutionId.Value] : objects[c2.InstitutionId.Value] = FindInstitution2(c2.InstitutionId));
                if (load && c2.CreationUserId != null) c2.CreationUser = (User2)(cache && objects.ContainsKey(c2.CreationUserId.Value) ? objects[c2.CreationUserId.Value] : objects[c2.CreationUserId.Value] = FindUser2(c2.CreationUserId));
                if (load && c2.LastWriteUserId != null) c2.LastWriteUser = (User2)(cache && objects.ContainsKey(c2.LastWriteUserId.Value) ? objects[c2.LastWriteUserId.Value] : objects[c2.LastWriteUserId.Value] = FindUser2(c2.LastWriteUserId));
                return c2;
            }).ToArray();
        }

        public IEnumerable<Campus2> Campus2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Campuses(where, orderBy, skip, take))
            {
                var c2 = Campus2.FromJObject(jo);
                if (load && c2.InstitutionId != null) c2.Institution = (Institution2)(cache && objects.ContainsKey(c2.InstitutionId.Value) ? objects[c2.InstitutionId.Value] : objects[c2.InstitutionId.Value] = FindInstitution2(c2.InstitutionId));
                if (load && c2.CreationUserId != null) c2.CreationUser = (User2)(cache && objects.ContainsKey(c2.CreationUserId.Value) ? objects[c2.CreationUserId.Value] : objects[c2.CreationUserId.Value] = FindUser2(c2.CreationUserId));
                if (load && c2.LastWriteUserId != null) c2.LastWriteUser = (User2)(cache && objects.ContainsKey(c2.LastWriteUserId.Value) ? objects[c2.LastWriteUserId.Value] : objects[c2.LastWriteUserId.Value] = FindUser2(c2.LastWriteUserId));
                yield return c2;
            }
        }

        public Campus2 FindCampus2(Guid? id, bool load = false, bool cache = true)
        {
            var c2 = Campus2.FromJObject(FolioServiceClient.GetCampus(id?.ToString()));
            if (c2 == null) return null;
            if (load && c2.InstitutionId != null) c2.Institution = (Institution2)(cache && objects.ContainsKey(c2.InstitutionId.Value) ? objects[c2.InstitutionId.Value] : objects[c2.InstitutionId.Value] = FindInstitution2(c2.InstitutionId));
            if (load && c2.CreationUserId != null) c2.CreationUser = (User2)(cache && objects.ContainsKey(c2.CreationUserId.Value) ? objects[c2.CreationUserId.Value] : objects[c2.CreationUserId.Value] = FindUser2(c2.CreationUserId));
            if (load && c2.LastWriteUserId != null) c2.LastWriteUser = (User2)(cache && objects.ContainsKey(c2.LastWriteUserId.Value) ? objects[c2.LastWriteUserId.Value] : objects[c2.LastWriteUserId.Value] = FindUser2(c2.LastWriteUserId));
            return c2;
        }

        public void Insert(Campus2 campus2)
        {
            if (campus2.Id == null) campus2.Id = Guid.NewGuid();
            FolioServiceClient.InsertCampus(campus2.ToJObject());
        }

        public void Update(Campus2 campus2) => FolioServiceClient.UpdateCampus(campus2.ToJObject());

        public void DeleteCampus2(Guid? id) => FolioServiceClient.DeleteCampus(id?.ToString());

        public bool AnyCancellationReason2s(string where = null) => FolioServiceClient.AnyCancellationReasons(where);

        public int CountCancellationReason2s(string where = null) => FolioServiceClient.CountCancellationReasons(where);

        public CancellationReason2[] CancellationReason2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.CancellationReasons(out count, where, orderBy, skip, take).Select(jo =>
            {
                var cr2 = CancellationReason2.FromJObject(jo);
                if (load && cr2.CreationUserId != null) cr2.CreationUser = (User2)(cache && objects.ContainsKey(cr2.CreationUserId.Value) ? objects[cr2.CreationUserId.Value] : objects[cr2.CreationUserId.Value] = FindUser2(cr2.CreationUserId));
                if (load && cr2.LastWriteUserId != null) cr2.LastWriteUser = (User2)(cache && objects.ContainsKey(cr2.LastWriteUserId.Value) ? objects[cr2.LastWriteUserId.Value] : objects[cr2.LastWriteUserId.Value] = FindUser2(cr2.LastWriteUserId));
                return cr2;
            }).ToArray();
        }

        public IEnumerable<CancellationReason2> CancellationReason2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.CancellationReasons(where, orderBy, skip, take))
            {
                var cr2 = CancellationReason2.FromJObject(jo);
                if (load && cr2.CreationUserId != null) cr2.CreationUser = (User2)(cache && objects.ContainsKey(cr2.CreationUserId.Value) ? objects[cr2.CreationUserId.Value] : objects[cr2.CreationUserId.Value] = FindUser2(cr2.CreationUserId));
                if (load && cr2.LastWriteUserId != null) cr2.LastWriteUser = (User2)(cache && objects.ContainsKey(cr2.LastWriteUserId.Value) ? objects[cr2.LastWriteUserId.Value] : objects[cr2.LastWriteUserId.Value] = FindUser2(cr2.LastWriteUserId));
                yield return cr2;
            }
        }

        public CancellationReason2 FindCancellationReason2(Guid? id, bool load = false, bool cache = true)
        {
            var cr2 = CancellationReason2.FromJObject(FolioServiceClient.GetCancellationReason(id?.ToString()));
            if (cr2 == null) return null;
            if (load && cr2.CreationUserId != null) cr2.CreationUser = (User2)(cache && objects.ContainsKey(cr2.CreationUserId.Value) ? objects[cr2.CreationUserId.Value] : objects[cr2.CreationUserId.Value] = FindUser2(cr2.CreationUserId));
            if (load && cr2.LastWriteUserId != null) cr2.LastWriteUser = (User2)(cache && objects.ContainsKey(cr2.LastWriteUserId.Value) ? objects[cr2.LastWriteUserId.Value] : objects[cr2.LastWriteUserId.Value] = FindUser2(cr2.LastWriteUserId));
            return cr2;
        }

        public void Insert(CancellationReason2 cancellationReason2)
        {
            if (cancellationReason2.Id == null) cancellationReason2.Id = Guid.NewGuid();
            FolioServiceClient.InsertCancellationReason(cancellationReason2.ToJObject());
        }

        public void Update(CancellationReason2 cancellationReason2) => FolioServiceClient.UpdateCancellationReason(cancellationReason2.ToJObject());

        public void DeleteCancellationReason2(Guid? id) => FolioServiceClient.DeleteCancellationReason(id?.ToString());

        public bool AnyCategory2s(string where = null) => FolioServiceClient.AnyCategories(where);

        public int CountCategory2s(string where = null) => FolioServiceClient.CountCategories(where);

        public Category2[] Category2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Categories(out count, where, orderBy, skip, take).Select(jo =>
            {
                var c2 = Category2.FromJObject(jo);
                if (load && c2.CreationUserId != null) c2.CreationUser = (User2)(cache && objects.ContainsKey(c2.CreationUserId.Value) ? objects[c2.CreationUserId.Value] : objects[c2.CreationUserId.Value] = FindUser2(c2.CreationUserId));
                if (load && c2.LastWriteUserId != null) c2.LastWriteUser = (User2)(cache && objects.ContainsKey(c2.LastWriteUserId.Value) ? objects[c2.LastWriteUserId.Value] : objects[c2.LastWriteUserId.Value] = FindUser2(c2.LastWriteUserId));
                return c2;
            }).ToArray();
        }

        public IEnumerable<Category2> Category2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Categories(where, orderBy, skip, take))
            {
                var c2 = Category2.FromJObject(jo);
                if (load && c2.CreationUserId != null) c2.CreationUser = (User2)(cache && objects.ContainsKey(c2.CreationUserId.Value) ? objects[c2.CreationUserId.Value] : objects[c2.CreationUserId.Value] = FindUser2(c2.CreationUserId));
                if (load && c2.LastWriteUserId != null) c2.LastWriteUser = (User2)(cache && objects.ContainsKey(c2.LastWriteUserId.Value) ? objects[c2.LastWriteUserId.Value] : objects[c2.LastWriteUserId.Value] = FindUser2(c2.LastWriteUserId));
                yield return c2;
            }
        }

        public Category2 FindCategory2(Guid? id, bool load = false, bool cache = true)
        {
            var c2 = Category2.FromJObject(FolioServiceClient.GetCategory(id?.ToString()));
            if (c2 == null) return null;
            if (load && c2.CreationUserId != null) c2.CreationUser = (User2)(cache && objects.ContainsKey(c2.CreationUserId.Value) ? objects[c2.CreationUserId.Value] : objects[c2.CreationUserId.Value] = FindUser2(c2.CreationUserId));
            if (load && c2.LastWriteUserId != null) c2.LastWriteUser = (User2)(cache && objects.ContainsKey(c2.LastWriteUserId.Value) ? objects[c2.LastWriteUserId.Value] : objects[c2.LastWriteUserId.Value] = FindUser2(c2.LastWriteUserId));
            return c2;
        }

        public void Insert(Category2 category2)
        {
            if (category2.Id == null) category2.Id = Guid.NewGuid();
            FolioServiceClient.InsertCategory(category2.ToJObject());
        }

        public void Update(Category2 category2) => FolioServiceClient.UpdateCategory(category2.ToJObject());

        public void DeleteCategory2(Guid? id) => FolioServiceClient.DeleteCategory(id?.ToString());

        public bool AnyCheckIn2s(string where = null) => FolioServiceClient.AnyCheckIns(where);

        public int CountCheckIn2s(string where = null) => FolioServiceClient.CountCheckIns(where);

        public CheckIn2[] CheckIn2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.CheckIns(out count, where, orderBy, skip, take).Select(jo =>
            {
                var ci2 = CheckIn2.FromJObject(jo);
                if (load && ci2.ItemId != null) ci2.Item = (Item2)(cache && objects.ContainsKey(ci2.ItemId.Value) ? objects[ci2.ItemId.Value] : objects[ci2.ItemId.Value] = FindItem2(ci2.ItemId));
                if (load && ci2.ItemLocationId != null) ci2.ItemLocation = (Location2)(cache && objects.ContainsKey(ci2.ItemLocationId.Value) ? objects[ci2.ItemLocationId.Value] : objects[ci2.ItemLocationId.Value] = FindLocation2(ci2.ItemLocationId));
                if (load && ci2.ServicePointId != null) ci2.ServicePoint = (ServicePoint2)(cache && objects.ContainsKey(ci2.ServicePointId.Value) ? objects[ci2.ServicePointId.Value] : objects[ci2.ServicePointId.Value] = FindServicePoint2(ci2.ServicePointId));
                if (load && ci2.PerformedByUserId != null) ci2.PerformedByUser = (User2)(cache && objects.ContainsKey(ci2.PerformedByUserId.Value) ? objects[ci2.PerformedByUserId.Value] : objects[ci2.PerformedByUserId.Value] = FindUser2(ci2.PerformedByUserId));
                return ci2;
            }).ToArray();
        }

        public IEnumerable<CheckIn2> CheckIn2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.CheckIns(where, orderBy, skip, take))
            {
                var ci2 = CheckIn2.FromJObject(jo);
                if (load && ci2.ItemId != null) ci2.Item = (Item2)(cache && objects.ContainsKey(ci2.ItemId.Value) ? objects[ci2.ItemId.Value] : objects[ci2.ItemId.Value] = FindItem2(ci2.ItemId));
                if (load && ci2.ItemLocationId != null) ci2.ItemLocation = (Location2)(cache && objects.ContainsKey(ci2.ItemLocationId.Value) ? objects[ci2.ItemLocationId.Value] : objects[ci2.ItemLocationId.Value] = FindLocation2(ci2.ItemLocationId));
                if (load && ci2.ServicePointId != null) ci2.ServicePoint = (ServicePoint2)(cache && objects.ContainsKey(ci2.ServicePointId.Value) ? objects[ci2.ServicePointId.Value] : objects[ci2.ServicePointId.Value] = FindServicePoint2(ci2.ServicePointId));
                if (load && ci2.PerformedByUserId != null) ci2.PerformedByUser = (User2)(cache && objects.ContainsKey(ci2.PerformedByUserId.Value) ? objects[ci2.PerformedByUserId.Value] : objects[ci2.PerformedByUserId.Value] = FindUser2(ci2.PerformedByUserId));
                yield return ci2;
            }
        }

        public CheckIn2 FindCheckIn2(Guid? id, bool load = false, bool cache = true)
        {
            var ci2 = CheckIn2.FromJObject(FolioServiceClient.GetCheckIn(id?.ToString()));
            if (ci2 == null) return null;
            if (load && ci2.ItemId != null) ci2.Item = (Item2)(cache && objects.ContainsKey(ci2.ItemId.Value) ? objects[ci2.ItemId.Value] : objects[ci2.ItemId.Value] = FindItem2(ci2.ItemId));
            if (load && ci2.ItemLocationId != null) ci2.ItemLocation = (Location2)(cache && objects.ContainsKey(ci2.ItemLocationId.Value) ? objects[ci2.ItemLocationId.Value] : objects[ci2.ItemLocationId.Value] = FindLocation2(ci2.ItemLocationId));
            if (load && ci2.ServicePointId != null) ci2.ServicePoint = (ServicePoint2)(cache && objects.ContainsKey(ci2.ServicePointId.Value) ? objects[ci2.ServicePointId.Value] : objects[ci2.ServicePointId.Value] = FindServicePoint2(ci2.ServicePointId));
            if (load && ci2.PerformedByUserId != null) ci2.PerformedByUser = (User2)(cache && objects.ContainsKey(ci2.PerformedByUserId.Value) ? objects[ci2.PerformedByUserId.Value] : objects[ci2.PerformedByUserId.Value] = FindUser2(ci2.PerformedByUserId));
            return ci2;
        }

        public void Insert(CheckIn2 checkIn2)
        {
            if (checkIn2.Id == null) checkIn2.Id = Guid.NewGuid();
            FolioServiceClient.InsertCheckIn(checkIn2.ToJObject());
        }

        public void Update(CheckIn2 checkIn2) => FolioServiceClient.UpdateCheckIn(checkIn2.ToJObject());

        public void DeleteCheckIn2(Guid? id) => FolioServiceClient.DeleteCheckIn(id?.ToString());

        public CirculationRule2 FindCirculationRule2(bool load = false, bool cache = true) => CirculationRule2.FromJObject(FolioServiceClient.GetCirculationRule());

        public void Update(CirculationRule2 circulationRule2) => FolioServiceClient.UpdateCirculationRule(circulationRule2.ToJObject());

        public bool AnyClassificationType2s(string where = null) => FolioServiceClient.AnyClassificationTypes(where);

        public int CountClassificationType2s(string where = null) => FolioServiceClient.CountClassificationTypes(where);

        public ClassificationType2[] ClassificationType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ClassificationTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var ct2 = ClassificationType2.FromJObject(jo);
                if (load && ct2.CreationUserId != null) ct2.CreationUser = (User2)(cache && objects.ContainsKey(ct2.CreationUserId.Value) ? objects[ct2.CreationUserId.Value] : objects[ct2.CreationUserId.Value] = FindUser2(ct2.CreationUserId));
                if (load && ct2.LastWriteUserId != null) ct2.LastWriteUser = (User2)(cache && objects.ContainsKey(ct2.LastWriteUserId.Value) ? objects[ct2.LastWriteUserId.Value] : objects[ct2.LastWriteUserId.Value] = FindUser2(ct2.LastWriteUserId));
                return ct2;
            }).ToArray();
        }

        public IEnumerable<ClassificationType2> ClassificationType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ClassificationTypes(where, orderBy, skip, take))
            {
                var ct2 = ClassificationType2.FromJObject(jo);
                if (load && ct2.CreationUserId != null) ct2.CreationUser = (User2)(cache && objects.ContainsKey(ct2.CreationUserId.Value) ? objects[ct2.CreationUserId.Value] : objects[ct2.CreationUserId.Value] = FindUser2(ct2.CreationUserId));
                if (load && ct2.LastWriteUserId != null) ct2.LastWriteUser = (User2)(cache && objects.ContainsKey(ct2.LastWriteUserId.Value) ? objects[ct2.LastWriteUserId.Value] : objects[ct2.LastWriteUserId.Value] = FindUser2(ct2.LastWriteUserId));
                yield return ct2;
            }
        }

        public ClassificationType2 FindClassificationType2(Guid? id, bool load = false, bool cache = true)
        {
            var ct2 = ClassificationType2.FromJObject(FolioServiceClient.GetClassificationType(id?.ToString()));
            if (ct2 == null) return null;
            if (load && ct2.CreationUserId != null) ct2.CreationUser = (User2)(cache && objects.ContainsKey(ct2.CreationUserId.Value) ? objects[ct2.CreationUserId.Value] : objects[ct2.CreationUserId.Value] = FindUser2(ct2.CreationUserId));
            if (load && ct2.LastWriteUserId != null) ct2.LastWriteUser = (User2)(cache && objects.ContainsKey(ct2.LastWriteUserId.Value) ? objects[ct2.LastWriteUserId.Value] : objects[ct2.LastWriteUserId.Value] = FindUser2(ct2.LastWriteUserId));
            return ct2;
        }

        public void Insert(ClassificationType2 classificationType2)
        {
            if (classificationType2.Id == null) classificationType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertClassificationType(classificationType2.ToJObject());
        }

        public void Update(ClassificationType2 classificationType2) => FolioServiceClient.UpdateClassificationType(classificationType2.ToJObject());

        public void DeleteClassificationType2(Guid? id) => FolioServiceClient.DeleteClassificationType(id?.ToString());

        public bool AnyCloseReason2s(string where = null) => FolioServiceClient.AnyCloseReasons(where);

        public int CountCloseReason2s(string where = null) => FolioServiceClient.CountCloseReasons(where);

        public CloseReason2[] CloseReason2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.CloseReasons(out count, where, orderBy, skip, take).Select(jo =>
            {
                var cr2 = CloseReason2.FromJObject(jo);
                return cr2;
            }).ToArray();
        }

        public IEnumerable<CloseReason2> CloseReason2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.CloseReasons(where, orderBy, skip, take))
            {
                var cr2 = CloseReason2.FromJObject(jo);
                yield return cr2;
            }
        }

        public CloseReason2 FindCloseReason2(Guid? id, bool load = false, bool cache = true) => CloseReason2.FromJObject(FolioServiceClient.GetCloseReason(id?.ToString()));

        public void Insert(CloseReason2 closeReason2)
        {
            if (closeReason2.Id == null) closeReason2.Id = Guid.NewGuid();
            FolioServiceClient.InsertCloseReason(closeReason2.ToJObject());
        }

        public void Update(CloseReason2 closeReason2) => FolioServiceClient.UpdateCloseReason(closeReason2.ToJObject());

        public void DeleteCloseReason2(Guid? id) => FolioServiceClient.DeleteCloseReason(id?.ToString());

        public bool AnyComment2s(string where = null) => FolioServiceClient.AnyComments(where);

        public int CountComment2s(string where = null) => FolioServiceClient.CountComments(where);

        public Comment2[] Comment2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Comments(out count, where, orderBy, skip, take).Select(jo =>
            {
                var c2 = Comment2.FromJObject(jo);
                if (load && c2.CreationUserId != null) c2.CreationUser = (User2)(cache && objects.ContainsKey(c2.CreationUserId.Value) ? objects[c2.CreationUserId.Value] : objects[c2.CreationUserId.Value] = FindUser2(c2.CreationUserId));
                if (load && c2.LastWriteUserId != null) c2.LastWriteUser = (User2)(cache && objects.ContainsKey(c2.LastWriteUserId.Value) ? objects[c2.LastWriteUserId.Value] : objects[c2.LastWriteUserId.Value] = FindUser2(c2.LastWriteUserId));
                return c2;
            }).ToArray();
        }

        public IEnumerable<Comment2> Comment2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Comments(where, orderBy, skip, take))
            {
                var c2 = Comment2.FromJObject(jo);
                if (load && c2.CreationUserId != null) c2.CreationUser = (User2)(cache && objects.ContainsKey(c2.CreationUserId.Value) ? objects[c2.CreationUserId.Value] : objects[c2.CreationUserId.Value] = FindUser2(c2.CreationUserId));
                if (load && c2.LastWriteUserId != null) c2.LastWriteUser = (User2)(cache && objects.ContainsKey(c2.LastWriteUserId.Value) ? objects[c2.LastWriteUserId.Value] : objects[c2.LastWriteUserId.Value] = FindUser2(c2.LastWriteUserId));
                yield return c2;
            }
        }

        public Comment2 FindComment2(Guid? id, bool load = false, bool cache = true)
        {
            var c2 = Comment2.FromJObject(FolioServiceClient.GetComment(id?.ToString()));
            if (c2 == null) return null;
            if (load && c2.CreationUserId != null) c2.CreationUser = (User2)(cache && objects.ContainsKey(c2.CreationUserId.Value) ? objects[c2.CreationUserId.Value] : objects[c2.CreationUserId.Value] = FindUser2(c2.CreationUserId));
            if (load && c2.LastWriteUserId != null) c2.LastWriteUser = (User2)(cache && objects.ContainsKey(c2.LastWriteUserId.Value) ? objects[c2.LastWriteUserId.Value] : objects[c2.LastWriteUserId.Value] = FindUser2(c2.LastWriteUserId));
            return c2;
        }

        public void Insert(Comment2 comment2)
        {
            if (comment2.Id == null) comment2.Id = Guid.NewGuid();
            FolioServiceClient.InsertComment(comment2.ToJObject());
        }

        public void Update(Comment2 comment2) => FolioServiceClient.UpdateComment(comment2.ToJObject());

        public void DeleteComment2(Guid? id) => FolioServiceClient.DeleteComment(id?.ToString());

        public bool AnyConfiguration2s(string where = null) => FolioServiceClient.AnyConfigurations(where);

        public int CountConfiguration2s(string where = null) => FolioServiceClient.CountConfigurations(where);

        public Configuration2[] Configuration2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Configurations(out count, where, orderBy, skip, take).Select(jo =>
            {
                var c2 = Configuration2.FromJObject(jo);
                if (load && c2.CreationUserId != null) c2.CreationUser = (User2)(cache && objects.ContainsKey(c2.CreationUserId.Value) ? objects[c2.CreationUserId.Value] : objects[c2.CreationUserId.Value] = FindUser2(c2.CreationUserId));
                if (load && c2.LastWriteUserId != null) c2.LastWriteUser = (User2)(cache && objects.ContainsKey(c2.LastWriteUserId.Value) ? objects[c2.LastWriteUserId.Value] : objects[c2.LastWriteUserId.Value] = FindUser2(c2.LastWriteUserId));
                return c2;
            }).ToArray();
        }

        public IEnumerable<Configuration2> Configuration2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Configurations(where, orderBy, skip, take))
            {
                var c2 = Configuration2.FromJObject(jo);
                if (load && c2.CreationUserId != null) c2.CreationUser = (User2)(cache && objects.ContainsKey(c2.CreationUserId.Value) ? objects[c2.CreationUserId.Value] : objects[c2.CreationUserId.Value] = FindUser2(c2.CreationUserId));
                if (load && c2.LastWriteUserId != null) c2.LastWriteUser = (User2)(cache && objects.ContainsKey(c2.LastWriteUserId.Value) ? objects[c2.LastWriteUserId.Value] : objects[c2.LastWriteUserId.Value] = FindUser2(c2.LastWriteUserId));
                yield return c2;
            }
        }

        public Configuration2 FindConfiguration2(Guid? id, bool load = false, bool cache = true)
        {
            var c2 = Configuration2.FromJObject(FolioServiceClient.GetConfiguration(id?.ToString()));
            if (c2 == null) return null;
            if (load && c2.CreationUserId != null) c2.CreationUser = (User2)(cache && objects.ContainsKey(c2.CreationUserId.Value) ? objects[c2.CreationUserId.Value] : objects[c2.CreationUserId.Value] = FindUser2(c2.CreationUserId));
            if (load && c2.LastWriteUserId != null) c2.LastWriteUser = (User2)(cache && objects.ContainsKey(c2.LastWriteUserId.Value) ? objects[c2.LastWriteUserId.Value] : objects[c2.LastWriteUserId.Value] = FindUser2(c2.LastWriteUserId));
            return c2;
        }

        public void Insert(Configuration2 configuration2)
        {
            if (configuration2.Id == null) configuration2.Id = Guid.NewGuid();
            FolioServiceClient.InsertConfiguration(configuration2.ToJObject());
        }

        public void Update(Configuration2 configuration2) => FolioServiceClient.UpdateConfiguration(configuration2.ToJObject());

        public void DeleteConfiguration2(Guid? id) => FolioServiceClient.DeleteConfiguration(id?.ToString());

        public bool AnyContact2s(string where = null) => FolioServiceClient.AnyContacts(where);

        public int CountContact2s(string where = null) => FolioServiceClient.CountContacts(where);

        public Contact2[] Contact2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Contacts(out count, where, orderBy, skip, take).Select(jo =>
            {
                var c2 = Contact2.FromJObject(jo);
                if (load && c2.CreationUserId != null) c2.CreationUser = (User2)(cache && objects.ContainsKey(c2.CreationUserId.Value) ? objects[c2.CreationUserId.Value] : objects[c2.CreationUserId.Value] = FindUser2(c2.CreationUserId));
                if (load && c2.LastWriteUserId != null) c2.LastWriteUser = (User2)(cache && objects.ContainsKey(c2.LastWriteUserId.Value) ? objects[c2.LastWriteUserId.Value] : objects[c2.LastWriteUserId.Value] = FindUser2(c2.LastWriteUserId));
                return c2;
            }).ToArray();
        }

        public IEnumerable<Contact2> Contact2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Contacts(where, orderBy, skip, take))
            {
                var c2 = Contact2.FromJObject(jo);
                if (load && c2.CreationUserId != null) c2.CreationUser = (User2)(cache && objects.ContainsKey(c2.CreationUserId.Value) ? objects[c2.CreationUserId.Value] : objects[c2.CreationUserId.Value] = FindUser2(c2.CreationUserId));
                if (load && c2.LastWriteUserId != null) c2.LastWriteUser = (User2)(cache && objects.ContainsKey(c2.LastWriteUserId.Value) ? objects[c2.LastWriteUserId.Value] : objects[c2.LastWriteUserId.Value] = FindUser2(c2.LastWriteUserId));
                yield return c2;
            }
        }

        public Contact2 FindContact2(Guid? id, bool load = false, bool cache = true)
        {
            var c2 = Contact2.FromJObject(FolioServiceClient.GetContact(id?.ToString()));
            if (c2 == null) return null;
            if (load && c2.CreationUserId != null) c2.CreationUser = (User2)(cache && objects.ContainsKey(c2.CreationUserId.Value) ? objects[c2.CreationUserId.Value] : objects[c2.CreationUserId.Value] = FindUser2(c2.CreationUserId));
            if (load && c2.LastWriteUserId != null) c2.LastWriteUser = (User2)(cache && objects.ContainsKey(c2.LastWriteUserId.Value) ? objects[c2.LastWriteUserId.Value] : objects[c2.LastWriteUserId.Value] = FindUser2(c2.LastWriteUserId));
            var i = 0;
            if (c2.ContactAddresses != null) foreach (var ca in c2.ContactAddresses)
                {
                    ca.Id = (++i).ToString();
                    ca.ContactId = c2.Id;
                }
            i = 0;
            if (c2.ContactCategories != null) foreach (var cc in c2.ContactCategories)
                {
                    cc.Id = (++i).ToString();
                    cc.ContactId = c2.Id;
                }
            i = 0;
            if (c2.ContactEmails != null) foreach (var ce in c2.ContactEmails)
                {
                    ce.Id = (++i).ToString();
                    ce.ContactId = c2.Id;
                }
            i = 0;
            if (c2.ContactPhoneNumbers != null) foreach (var cpn in c2.ContactPhoneNumbers)
                {
                    cpn.Id = (++i).ToString();
                    cpn.ContactId = c2.Id;
                }
            i = 0;
            if (c2.ContactUrls != null) foreach (var cu in c2.ContactUrls)
                {
                    cu.Id = (++i).ToString();
                    cu.ContactId = c2.Id;
                }
            return c2;
        }

        public void Insert(Contact2 contact2)
        {
            if (contact2.Id == null) contact2.Id = Guid.NewGuid();
            FolioServiceClient.InsertContact(contact2.ToJObject());
        }

        public void Update(Contact2 contact2) => FolioServiceClient.UpdateContact(contact2.ToJObject());

        public void DeleteContact2(Guid? id) => FolioServiceClient.DeleteContact(id?.ToString());

        public bool AnyContributorNameType2s(string where = null) => FolioServiceClient.AnyContributorNameTypes(where);

        public int CountContributorNameType2s(string where = null) => FolioServiceClient.CountContributorNameTypes(where);

        public ContributorNameType2[] ContributorNameType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ContributorNameTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var cnt2 = ContributorNameType2.FromJObject(jo);
                if (load && cnt2.CreationUserId != null) cnt2.CreationUser = (User2)(cache && objects.ContainsKey(cnt2.CreationUserId.Value) ? objects[cnt2.CreationUserId.Value] : objects[cnt2.CreationUserId.Value] = FindUser2(cnt2.CreationUserId));
                if (load && cnt2.LastWriteUserId != null) cnt2.LastWriteUser = (User2)(cache && objects.ContainsKey(cnt2.LastWriteUserId.Value) ? objects[cnt2.LastWriteUserId.Value] : objects[cnt2.LastWriteUserId.Value] = FindUser2(cnt2.LastWriteUserId));
                return cnt2;
            }).ToArray();
        }

        public IEnumerable<ContributorNameType2> ContributorNameType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ContributorNameTypes(where, orderBy, skip, take))
            {
                var cnt2 = ContributorNameType2.FromJObject(jo);
                if (load && cnt2.CreationUserId != null) cnt2.CreationUser = (User2)(cache && objects.ContainsKey(cnt2.CreationUserId.Value) ? objects[cnt2.CreationUserId.Value] : objects[cnt2.CreationUserId.Value] = FindUser2(cnt2.CreationUserId));
                if (load && cnt2.LastWriteUserId != null) cnt2.LastWriteUser = (User2)(cache && objects.ContainsKey(cnt2.LastWriteUserId.Value) ? objects[cnt2.LastWriteUserId.Value] : objects[cnt2.LastWriteUserId.Value] = FindUser2(cnt2.LastWriteUserId));
                yield return cnt2;
            }
        }

        public ContributorNameType2 FindContributorNameType2(Guid? id, bool load = false, bool cache = true)
        {
            var cnt2 = ContributorNameType2.FromJObject(FolioServiceClient.GetContributorNameType(id?.ToString()));
            if (cnt2 == null) return null;
            if (load && cnt2.CreationUserId != null) cnt2.CreationUser = (User2)(cache && objects.ContainsKey(cnt2.CreationUserId.Value) ? objects[cnt2.CreationUserId.Value] : objects[cnt2.CreationUserId.Value] = FindUser2(cnt2.CreationUserId));
            if (load && cnt2.LastWriteUserId != null) cnt2.LastWriteUser = (User2)(cache && objects.ContainsKey(cnt2.LastWriteUserId.Value) ? objects[cnt2.LastWriteUserId.Value] : objects[cnt2.LastWriteUserId.Value] = FindUser2(cnt2.LastWriteUserId));
            return cnt2;
        }

        public void Insert(ContributorNameType2 contributorNameType2)
        {
            if (contributorNameType2.Id == null) contributorNameType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertContributorNameType(contributorNameType2.ToJObject());
        }

        public void Update(ContributorNameType2 contributorNameType2) => FolioServiceClient.UpdateContributorNameType(contributorNameType2.ToJObject());

        public void DeleteContributorNameType2(Guid? id) => FolioServiceClient.DeleteContributorNameType(id?.ToString());

        public bool AnyContributorType2s(string where = null) => FolioServiceClient.AnyContributorTypes(where);

        public int CountContributorType2s(string where = null) => FolioServiceClient.CountContributorTypes(where);

        public ContributorType2[] ContributorType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ContributorTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var ct2 = ContributorType2.FromJObject(jo);
                if (load && ct2.CreationUserId != null) ct2.CreationUser = (User2)(cache && objects.ContainsKey(ct2.CreationUserId.Value) ? objects[ct2.CreationUserId.Value] : objects[ct2.CreationUserId.Value] = FindUser2(ct2.CreationUserId));
                if (load && ct2.LastWriteUserId != null) ct2.LastWriteUser = (User2)(cache && objects.ContainsKey(ct2.LastWriteUserId.Value) ? objects[ct2.LastWriteUserId.Value] : objects[ct2.LastWriteUserId.Value] = FindUser2(ct2.LastWriteUserId));
                return ct2;
            }).ToArray();
        }

        public IEnumerable<ContributorType2> ContributorType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ContributorTypes(where, orderBy, skip, take))
            {
                var ct2 = ContributorType2.FromJObject(jo);
                if (load && ct2.CreationUserId != null) ct2.CreationUser = (User2)(cache && objects.ContainsKey(ct2.CreationUserId.Value) ? objects[ct2.CreationUserId.Value] : objects[ct2.CreationUserId.Value] = FindUser2(ct2.CreationUserId));
                if (load && ct2.LastWriteUserId != null) ct2.LastWriteUser = (User2)(cache && objects.ContainsKey(ct2.LastWriteUserId.Value) ? objects[ct2.LastWriteUserId.Value] : objects[ct2.LastWriteUserId.Value] = FindUser2(ct2.LastWriteUserId));
                yield return ct2;
            }
        }

        public ContributorType2 FindContributorType2(Guid? id, bool load = false, bool cache = true)
        {
            var ct2 = ContributorType2.FromJObject(FolioServiceClient.GetContributorType(id?.ToString()));
            if (ct2 == null) return null;
            if (load && ct2.CreationUserId != null) ct2.CreationUser = (User2)(cache && objects.ContainsKey(ct2.CreationUserId.Value) ? objects[ct2.CreationUserId.Value] : objects[ct2.CreationUserId.Value] = FindUser2(ct2.CreationUserId));
            if (load && ct2.LastWriteUserId != null) ct2.LastWriteUser = (User2)(cache && objects.ContainsKey(ct2.LastWriteUserId.Value) ? objects[ct2.LastWriteUserId.Value] : objects[ct2.LastWriteUserId.Value] = FindUser2(ct2.LastWriteUserId));
            return ct2;
        }

        public void Insert(ContributorType2 contributorType2)
        {
            if (contributorType2.Id == null) contributorType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertContributorType(contributorType2.ToJObject());
        }

        public void Update(ContributorType2 contributorType2) => FolioServiceClient.UpdateContributorType(contributorType2.ToJObject());

        public void DeleteContributorType2(Guid? id) => FolioServiceClient.DeleteContributorType(id?.ToString());

        public bool AnyCustomField2s(string where = null) => FolioServiceClient.AnyCustomFields(where);

        public int CountCustomField2s(string where = null) => FolioServiceClient.CountCustomFields(where);

        public CustomField2[] CustomField2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.CustomFields(out count, where, orderBy, skip, take).Select(jo =>
            {
                var cf2 = CustomField2.FromJObject(jo);
                if (load && cf2.CreationUserId != null) cf2.CreationUser = (User2)(cache && objects.ContainsKey(cf2.CreationUserId.Value) ? objects[cf2.CreationUserId.Value] : objects[cf2.CreationUserId.Value] = FindUser2(cf2.CreationUserId));
                if (load && cf2.LastWriteUserId != null) cf2.LastWriteUser = (User2)(cache && objects.ContainsKey(cf2.LastWriteUserId.Value) ? objects[cf2.LastWriteUserId.Value] : objects[cf2.LastWriteUserId.Value] = FindUser2(cf2.LastWriteUserId));
                return cf2;
            }).ToArray();
        }

        public IEnumerable<CustomField2> CustomField2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.CustomFields(where, orderBy, skip, take))
            {
                var cf2 = CustomField2.FromJObject(jo);
                if (load && cf2.CreationUserId != null) cf2.CreationUser = (User2)(cache && objects.ContainsKey(cf2.CreationUserId.Value) ? objects[cf2.CreationUserId.Value] : objects[cf2.CreationUserId.Value] = FindUser2(cf2.CreationUserId));
                if (load && cf2.LastWriteUserId != null) cf2.LastWriteUser = (User2)(cache && objects.ContainsKey(cf2.LastWriteUserId.Value) ? objects[cf2.LastWriteUserId.Value] : objects[cf2.LastWriteUserId.Value] = FindUser2(cf2.LastWriteUserId));
                yield return cf2;
            }
        }

        public CustomField2 FindCustomField2(Guid? id, bool load = false, bool cache = true)
        {
            var cf2 = CustomField2.FromJObject(FolioServiceClient.GetCustomField(id?.ToString()));
            if (cf2 == null) return null;
            if (load && cf2.CreationUserId != null) cf2.CreationUser = (User2)(cache && objects.ContainsKey(cf2.CreationUserId.Value) ? objects[cf2.CreationUserId.Value] : objects[cf2.CreationUserId.Value] = FindUser2(cf2.CreationUserId));
            if (load && cf2.LastWriteUserId != null) cf2.LastWriteUser = (User2)(cache && objects.ContainsKey(cf2.LastWriteUserId.Value) ? objects[cf2.LastWriteUserId.Value] : objects[cf2.LastWriteUserId.Value] = FindUser2(cf2.LastWriteUserId));
            var i = 0;
            if (cf2.CustomFieldValues != null) foreach (var cfv in cf2.CustomFieldValues)
                {
                    cfv.Id = (++i).ToString();
                    cfv.CustomFieldId = cf2.Id;
                }
            return cf2;
        }

        public void Insert(CustomField2 customField2)
        {
            if (customField2.Id == null) customField2.Id = Guid.NewGuid();
            FolioServiceClient.InsertCustomField(customField2.ToJObject());
        }

        public void Update(CustomField2 customField2) => FolioServiceClient.UpdateCustomField(customField2.ToJObject());

        public void DeleteCustomField2(Guid? id) => FolioServiceClient.DeleteCustomField(id?.ToString());

        public bool AnyDepartment2s(string where = null) => FolioServiceClient.AnyDepartments(where);

        public int CountDepartment2s(string where = null) => FolioServiceClient.CountDepartments(where);

        public Department2[] Department2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Departments(out count, where, orderBy, skip, take).Select(jo =>
            {
                var d2 = Department2.FromJObject(jo);
                if (load && d2.CreationUserId != null) d2.CreationUser = (User2)(cache && objects.ContainsKey(d2.CreationUserId.Value) ? objects[d2.CreationUserId.Value] : objects[d2.CreationUserId.Value] = FindUser2(d2.CreationUserId));
                if (load && d2.LastWriteUserId != null) d2.LastWriteUser = (User2)(cache && objects.ContainsKey(d2.LastWriteUserId.Value) ? objects[d2.LastWriteUserId.Value] : objects[d2.LastWriteUserId.Value] = FindUser2(d2.LastWriteUserId));
                return d2;
            }).ToArray();
        }

        public IEnumerable<Department2> Department2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Departments(where, orderBy, skip, take))
            {
                var d2 = Department2.FromJObject(jo);
                if (load && d2.CreationUserId != null) d2.CreationUser = (User2)(cache && objects.ContainsKey(d2.CreationUserId.Value) ? objects[d2.CreationUserId.Value] : objects[d2.CreationUserId.Value] = FindUser2(d2.CreationUserId));
                if (load && d2.LastWriteUserId != null) d2.LastWriteUser = (User2)(cache && objects.ContainsKey(d2.LastWriteUserId.Value) ? objects[d2.LastWriteUserId.Value] : objects[d2.LastWriteUserId.Value] = FindUser2(d2.LastWriteUserId));
                yield return d2;
            }
        }

        public Department2 FindDepartment2(Guid? id, bool load = false, bool cache = true)
        {
            var d2 = Department2.FromJObject(FolioServiceClient.GetDepartment(id?.ToString()));
            if (d2 == null) return null;
            if (load && d2.CreationUserId != null) d2.CreationUser = (User2)(cache && objects.ContainsKey(d2.CreationUserId.Value) ? objects[d2.CreationUserId.Value] : objects[d2.CreationUserId.Value] = FindUser2(d2.CreationUserId));
            if (load && d2.LastWriteUserId != null) d2.LastWriteUser = (User2)(cache && objects.ContainsKey(d2.LastWriteUserId.Value) ? objects[d2.LastWriteUserId.Value] : objects[d2.LastWriteUserId.Value] = FindUser2(d2.LastWriteUserId));
            return d2;
        }

        public void Insert(Department2 department2)
        {
            if (department2.Id == null) department2.Id = Guid.NewGuid();
            FolioServiceClient.InsertDepartment(department2.ToJObject());
        }

        public void Update(Department2 department2) => FolioServiceClient.UpdateDepartment(department2.ToJObject());

        public void DeleteDepartment2(Guid? id) => FolioServiceClient.DeleteDepartment(id?.ToString());

        public bool AnyElectronicAccessRelationship2s(string where = null) => FolioServiceClient.AnyElectronicAccessRelationships(where);

        public int CountElectronicAccessRelationship2s(string where = null) => FolioServiceClient.CountElectronicAccessRelationships(where);

        public ElectronicAccessRelationship2[] ElectronicAccessRelationship2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ElectronicAccessRelationships(out count, where, orderBy, skip, take).Select(jo =>
            {
                var ear2 = ElectronicAccessRelationship2.FromJObject(jo);
                if (load && ear2.CreationUserId != null) ear2.CreationUser = (User2)(cache && objects.ContainsKey(ear2.CreationUserId.Value) ? objects[ear2.CreationUserId.Value] : objects[ear2.CreationUserId.Value] = FindUser2(ear2.CreationUserId));
                if (load && ear2.LastWriteUserId != null) ear2.LastWriteUser = (User2)(cache && objects.ContainsKey(ear2.LastWriteUserId.Value) ? objects[ear2.LastWriteUserId.Value] : objects[ear2.LastWriteUserId.Value] = FindUser2(ear2.LastWriteUserId));
                return ear2;
            }).ToArray();
        }

        public IEnumerable<ElectronicAccessRelationship2> ElectronicAccessRelationship2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ElectronicAccessRelationships(where, orderBy, skip, take))
            {
                var ear2 = ElectronicAccessRelationship2.FromJObject(jo);
                if (load && ear2.CreationUserId != null) ear2.CreationUser = (User2)(cache && objects.ContainsKey(ear2.CreationUserId.Value) ? objects[ear2.CreationUserId.Value] : objects[ear2.CreationUserId.Value] = FindUser2(ear2.CreationUserId));
                if (load && ear2.LastWriteUserId != null) ear2.LastWriteUser = (User2)(cache && objects.ContainsKey(ear2.LastWriteUserId.Value) ? objects[ear2.LastWriteUserId.Value] : objects[ear2.LastWriteUserId.Value] = FindUser2(ear2.LastWriteUserId));
                yield return ear2;
            }
        }

        public ElectronicAccessRelationship2 FindElectronicAccessRelationship2(Guid? id, bool load = false, bool cache = true)
        {
            var ear2 = ElectronicAccessRelationship2.FromJObject(FolioServiceClient.GetElectronicAccessRelationship(id?.ToString()));
            if (ear2 == null) return null;
            if (load && ear2.CreationUserId != null) ear2.CreationUser = (User2)(cache && objects.ContainsKey(ear2.CreationUserId.Value) ? objects[ear2.CreationUserId.Value] : objects[ear2.CreationUserId.Value] = FindUser2(ear2.CreationUserId));
            if (load && ear2.LastWriteUserId != null) ear2.LastWriteUser = (User2)(cache && objects.ContainsKey(ear2.LastWriteUserId.Value) ? objects[ear2.LastWriteUserId.Value] : objects[ear2.LastWriteUserId.Value] = FindUser2(ear2.LastWriteUserId));
            return ear2;
        }

        public void Insert(ElectronicAccessRelationship2 electronicAccessRelationship2)
        {
            if (electronicAccessRelationship2.Id == null) electronicAccessRelationship2.Id = Guid.NewGuid();
            FolioServiceClient.InsertElectronicAccessRelationship(electronicAccessRelationship2.ToJObject());
        }

        public void Update(ElectronicAccessRelationship2 electronicAccessRelationship2) => FolioServiceClient.UpdateElectronicAccessRelationship(electronicAccessRelationship2.ToJObject());

        public void DeleteElectronicAccessRelationship2(Guid? id) => FolioServiceClient.DeleteElectronicAccessRelationship(id?.ToString());

        public bool AnyExpenseClass2s(string where = null) => FolioServiceClient.AnyExpenseClasses(where);

        public int CountExpenseClass2s(string where = null) => FolioServiceClient.CountExpenseClasses(where);

        public ExpenseClass2[] ExpenseClass2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ExpenseClasses(out count, where, orderBy, skip, take).Select(jo =>
            {
                var ec2 = ExpenseClass2.FromJObject(jo);
                if (load && ec2.CreationUserId != null) ec2.CreationUser = (User2)(cache && objects.ContainsKey(ec2.CreationUserId.Value) ? objects[ec2.CreationUserId.Value] : objects[ec2.CreationUserId.Value] = FindUser2(ec2.CreationUserId));
                if (load && ec2.LastWriteUserId != null) ec2.LastWriteUser = (User2)(cache && objects.ContainsKey(ec2.LastWriteUserId.Value) ? objects[ec2.LastWriteUserId.Value] : objects[ec2.LastWriteUserId.Value] = FindUser2(ec2.LastWriteUserId));
                return ec2;
            }).ToArray();
        }

        public IEnumerable<ExpenseClass2> ExpenseClass2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ExpenseClasses(where, orderBy, skip, take))
            {
                var ec2 = ExpenseClass2.FromJObject(jo);
                if (load && ec2.CreationUserId != null) ec2.CreationUser = (User2)(cache && objects.ContainsKey(ec2.CreationUserId.Value) ? objects[ec2.CreationUserId.Value] : objects[ec2.CreationUserId.Value] = FindUser2(ec2.CreationUserId));
                if (load && ec2.LastWriteUserId != null) ec2.LastWriteUser = (User2)(cache && objects.ContainsKey(ec2.LastWriteUserId.Value) ? objects[ec2.LastWriteUserId.Value] : objects[ec2.LastWriteUserId.Value] = FindUser2(ec2.LastWriteUserId));
                yield return ec2;
            }
        }

        public ExpenseClass2 FindExpenseClass2(Guid? id, bool load = false, bool cache = true)
        {
            var ec2 = ExpenseClass2.FromJObject(FolioServiceClient.GetExpenseClass(id?.ToString()));
            if (ec2 == null) return null;
            if (load && ec2.CreationUserId != null) ec2.CreationUser = (User2)(cache && objects.ContainsKey(ec2.CreationUserId.Value) ? objects[ec2.CreationUserId.Value] : objects[ec2.CreationUserId.Value] = FindUser2(ec2.CreationUserId));
            if (load && ec2.LastWriteUserId != null) ec2.LastWriteUser = (User2)(cache && objects.ContainsKey(ec2.LastWriteUserId.Value) ? objects[ec2.LastWriteUserId.Value] : objects[ec2.LastWriteUserId.Value] = FindUser2(ec2.LastWriteUserId));
            return ec2;
        }

        public void Insert(ExpenseClass2 expenseClass2)
        {
            if (expenseClass2.Id == null) expenseClass2.Id = Guid.NewGuid();
            FolioServiceClient.InsertExpenseClass(expenseClass2.ToJObject());
        }

        public void Update(ExpenseClass2 expenseClass2) => FolioServiceClient.UpdateExpenseClass(expenseClass2.ToJObject());

        public void DeleteExpenseClass2(Guid? id) => FolioServiceClient.DeleteExpenseClass(id?.ToString());

        public bool AnyFee2s(string where = null) => FolioServiceClient.AnyFees(where);

        public int CountFee2s(string where = null) => FolioServiceClient.CountFees(where);

        public Fee2[] Fee2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Fees(out count, where, orderBy, skip, take).Select(jo =>
            {
                var f2 = Fee2.FromJObject(jo);
                if (load && f2.CreationUserId != null) f2.CreationUser = (User2)(cache && objects.ContainsKey(f2.CreationUserId.Value) ? objects[f2.CreationUserId.Value] : objects[f2.CreationUserId.Value] = FindUser2(f2.CreationUserId));
                if (load && f2.LastWriteUserId != null) f2.LastWriteUser = (User2)(cache && objects.ContainsKey(f2.LastWriteUserId.Value) ? objects[f2.LastWriteUserId.Value] : objects[f2.LastWriteUserId.Value] = FindUser2(f2.LastWriteUserId));
                if (load && f2.LoanId != null) f2.Loan = (Loan2)(cache && objects.ContainsKey(f2.LoanId.Value) ? objects[f2.LoanId.Value] : objects[f2.LoanId.Value] = FindLoan2(f2.LoanId));
                if (load && f2.UserId != null) f2.User = (User2)(cache && objects.ContainsKey(f2.UserId.Value) ? objects[f2.UserId.Value] : objects[f2.UserId.Value] = FindUser2(f2.UserId));
                if (load && f2.ItemId != null) f2.Item = (Item2)(cache && objects.ContainsKey(f2.ItemId.Value) ? objects[f2.ItemId.Value] : objects[f2.ItemId.Value] = FindItem2(f2.ItemId));
                if (load && f2.MaterialTypeId != null) f2.MaterialType1 = (MaterialType2)(cache && objects.ContainsKey(f2.MaterialTypeId.Value) ? objects[f2.MaterialTypeId.Value] : objects[f2.MaterialTypeId.Value] = FindMaterialType2(f2.MaterialTypeId));
                if (load && f2.FeeTypeId != null) f2.FeeType = (FeeType2)(cache && objects.ContainsKey(f2.FeeTypeId.Value) ? objects[f2.FeeTypeId.Value] : objects[f2.FeeTypeId.Value] = FindFeeType2(f2.FeeTypeId));
                if (load && f2.OwnerId != null) f2.Owner = (Owner2)(cache && objects.ContainsKey(f2.OwnerId.Value) ? objects[f2.OwnerId.Value] : objects[f2.OwnerId.Value] = FindOwner2(f2.OwnerId));
                if (load && f2.HoldingId != null) f2.Holding = (Holding2)(cache && objects.ContainsKey(f2.HoldingId.Value) ? objects[f2.HoldingId.Value] : objects[f2.HoldingId.Value] = FindHolding2(f2.HoldingId));
                if (load && f2.InstanceId != null) f2.Instance = (Instance2)(cache && objects.ContainsKey(f2.InstanceId.Value) ? objects[f2.InstanceId.Value] : objects[f2.InstanceId.Value] = FindInstance2(f2.InstanceId));
                return f2;
            }).ToArray();
        }

        public IEnumerable<Fee2> Fee2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Fees(where, orderBy, skip, take))
            {
                var f2 = Fee2.FromJObject(jo);
                if (load && f2.CreationUserId != null) f2.CreationUser = (User2)(cache && objects.ContainsKey(f2.CreationUserId.Value) ? objects[f2.CreationUserId.Value] : objects[f2.CreationUserId.Value] = FindUser2(f2.CreationUserId));
                if (load && f2.LastWriteUserId != null) f2.LastWriteUser = (User2)(cache && objects.ContainsKey(f2.LastWriteUserId.Value) ? objects[f2.LastWriteUserId.Value] : objects[f2.LastWriteUserId.Value] = FindUser2(f2.LastWriteUserId));
                if (load && f2.LoanId != null) f2.Loan = (Loan2)(cache && objects.ContainsKey(f2.LoanId.Value) ? objects[f2.LoanId.Value] : objects[f2.LoanId.Value] = FindLoan2(f2.LoanId));
                if (load && f2.UserId != null) f2.User = (User2)(cache && objects.ContainsKey(f2.UserId.Value) ? objects[f2.UserId.Value] : objects[f2.UserId.Value] = FindUser2(f2.UserId));
                if (load && f2.ItemId != null) f2.Item = (Item2)(cache && objects.ContainsKey(f2.ItemId.Value) ? objects[f2.ItemId.Value] : objects[f2.ItemId.Value] = FindItem2(f2.ItemId));
                if (load && f2.MaterialTypeId != null) f2.MaterialType1 = (MaterialType2)(cache && objects.ContainsKey(f2.MaterialTypeId.Value) ? objects[f2.MaterialTypeId.Value] : objects[f2.MaterialTypeId.Value] = FindMaterialType2(f2.MaterialTypeId));
                if (load && f2.FeeTypeId != null) f2.FeeType = (FeeType2)(cache && objects.ContainsKey(f2.FeeTypeId.Value) ? objects[f2.FeeTypeId.Value] : objects[f2.FeeTypeId.Value] = FindFeeType2(f2.FeeTypeId));
                if (load && f2.OwnerId != null) f2.Owner = (Owner2)(cache && objects.ContainsKey(f2.OwnerId.Value) ? objects[f2.OwnerId.Value] : objects[f2.OwnerId.Value] = FindOwner2(f2.OwnerId));
                if (load && f2.HoldingId != null) f2.Holding = (Holding2)(cache && objects.ContainsKey(f2.HoldingId.Value) ? objects[f2.HoldingId.Value] : objects[f2.HoldingId.Value] = FindHolding2(f2.HoldingId));
                if (load && f2.InstanceId != null) f2.Instance = (Instance2)(cache && objects.ContainsKey(f2.InstanceId.Value) ? objects[f2.InstanceId.Value] : objects[f2.InstanceId.Value] = FindInstance2(f2.InstanceId));
                yield return f2;
            }
        }

        public Fee2 FindFee2(Guid? id, bool load = false, bool cache = true)
        {
            var f2 = Fee2.FromJObject(FolioServiceClient.GetFee(id?.ToString()));
            if (f2 == null) return null;
            if (load && f2.CreationUserId != null) f2.CreationUser = (User2)(cache && objects.ContainsKey(f2.CreationUserId.Value) ? objects[f2.CreationUserId.Value] : objects[f2.CreationUserId.Value] = FindUser2(f2.CreationUserId));
            if (load && f2.LastWriteUserId != null) f2.LastWriteUser = (User2)(cache && objects.ContainsKey(f2.LastWriteUserId.Value) ? objects[f2.LastWriteUserId.Value] : objects[f2.LastWriteUserId.Value] = FindUser2(f2.LastWriteUserId));
            if (load && f2.LoanId != null) f2.Loan = (Loan2)(cache && objects.ContainsKey(f2.LoanId.Value) ? objects[f2.LoanId.Value] : objects[f2.LoanId.Value] = FindLoan2(f2.LoanId));
            if (load && f2.UserId != null) f2.User = (User2)(cache && objects.ContainsKey(f2.UserId.Value) ? objects[f2.UserId.Value] : objects[f2.UserId.Value] = FindUser2(f2.UserId));
            if (load && f2.ItemId != null) f2.Item = (Item2)(cache && objects.ContainsKey(f2.ItemId.Value) ? objects[f2.ItemId.Value] : objects[f2.ItemId.Value] = FindItem2(f2.ItemId));
            if (load && f2.MaterialTypeId != null) f2.MaterialType1 = (MaterialType2)(cache && objects.ContainsKey(f2.MaterialTypeId.Value) ? objects[f2.MaterialTypeId.Value] : objects[f2.MaterialTypeId.Value] = FindMaterialType2(f2.MaterialTypeId));
            if (load && f2.FeeTypeId != null) f2.FeeType = (FeeType2)(cache && objects.ContainsKey(f2.FeeTypeId.Value) ? objects[f2.FeeTypeId.Value] : objects[f2.FeeTypeId.Value] = FindFeeType2(f2.FeeTypeId));
            if (load && f2.OwnerId != null) f2.Owner = (Owner2)(cache && objects.ContainsKey(f2.OwnerId.Value) ? objects[f2.OwnerId.Value] : objects[f2.OwnerId.Value] = FindOwner2(f2.OwnerId));
            if (load && f2.HoldingId != null) f2.Holding = (Holding2)(cache && objects.ContainsKey(f2.HoldingId.Value) ? objects[f2.HoldingId.Value] : objects[f2.HoldingId.Value] = FindHolding2(f2.HoldingId));
            if (load && f2.InstanceId != null) f2.Instance = (Instance2)(cache && objects.ContainsKey(f2.InstanceId.Value) ? objects[f2.InstanceId.Value] : objects[f2.InstanceId.Value] = FindInstance2(f2.InstanceId));
            return f2;
        }

        public void Insert(Fee2 fee2)
        {
            if (fee2.Id == null) fee2.Id = Guid.NewGuid();
            FolioServiceClient.InsertFee(fee2.ToJObject());
        }

        public void Update(Fee2 fee2) => FolioServiceClient.UpdateFee(fee2.ToJObject());

        public void DeleteFee2(Guid? id) => FolioServiceClient.DeleteFee(id?.ToString());

        public bool AnyFeeType2s(string where = null) => FolioServiceClient.AnyFeeTypes(where);

        public int CountFeeType2s(string where = null) => FolioServiceClient.CountFeeTypes(where);

        public FeeType2[] FeeType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.FeeTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var ft2 = FeeType2.FromJObject(jo);
                if (load && ft2.ChargeNoticeId != null) ft2.ChargeNotice = (Template2)(cache && objects.ContainsKey(ft2.ChargeNoticeId.Value) ? objects[ft2.ChargeNoticeId.Value] : objects[ft2.ChargeNoticeId.Value] = FindTemplate2(ft2.ChargeNoticeId));
                if (load && ft2.ActionNoticeId != null) ft2.ActionNotice = (Template2)(cache && objects.ContainsKey(ft2.ActionNoticeId.Value) ? objects[ft2.ActionNoticeId.Value] : objects[ft2.ActionNoticeId.Value] = FindTemplate2(ft2.ActionNoticeId));
                if (load && ft2.OwnerId != null) ft2.Owner = (Owner2)(cache && objects.ContainsKey(ft2.OwnerId.Value) ? objects[ft2.OwnerId.Value] : objects[ft2.OwnerId.Value] = FindOwner2(ft2.OwnerId));
                if (load && ft2.CreationUserId != null) ft2.CreationUser = (User2)(cache && objects.ContainsKey(ft2.CreationUserId.Value) ? objects[ft2.CreationUserId.Value] : objects[ft2.CreationUserId.Value] = FindUser2(ft2.CreationUserId));
                if (load && ft2.LastWriteUserId != null) ft2.LastWriteUser = (User2)(cache && objects.ContainsKey(ft2.LastWriteUserId.Value) ? objects[ft2.LastWriteUserId.Value] : objects[ft2.LastWriteUserId.Value] = FindUser2(ft2.LastWriteUserId));
                return ft2;
            }).ToArray();
        }

        public IEnumerable<FeeType2> FeeType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.FeeTypes(where, orderBy, skip, take))
            {
                var ft2 = FeeType2.FromJObject(jo);
                if (load && ft2.ChargeNoticeId != null) ft2.ChargeNotice = (Template2)(cache && objects.ContainsKey(ft2.ChargeNoticeId.Value) ? objects[ft2.ChargeNoticeId.Value] : objects[ft2.ChargeNoticeId.Value] = FindTemplate2(ft2.ChargeNoticeId));
                if (load && ft2.ActionNoticeId != null) ft2.ActionNotice = (Template2)(cache && objects.ContainsKey(ft2.ActionNoticeId.Value) ? objects[ft2.ActionNoticeId.Value] : objects[ft2.ActionNoticeId.Value] = FindTemplate2(ft2.ActionNoticeId));
                if (load && ft2.OwnerId != null) ft2.Owner = (Owner2)(cache && objects.ContainsKey(ft2.OwnerId.Value) ? objects[ft2.OwnerId.Value] : objects[ft2.OwnerId.Value] = FindOwner2(ft2.OwnerId));
                if (load && ft2.CreationUserId != null) ft2.CreationUser = (User2)(cache && objects.ContainsKey(ft2.CreationUserId.Value) ? objects[ft2.CreationUserId.Value] : objects[ft2.CreationUserId.Value] = FindUser2(ft2.CreationUserId));
                if (load && ft2.LastWriteUserId != null) ft2.LastWriteUser = (User2)(cache && objects.ContainsKey(ft2.LastWriteUserId.Value) ? objects[ft2.LastWriteUserId.Value] : objects[ft2.LastWriteUserId.Value] = FindUser2(ft2.LastWriteUserId));
                yield return ft2;
            }
        }

        public FeeType2 FindFeeType2(Guid? id, bool load = false, bool cache = true)
        {
            var ft2 = FeeType2.FromJObject(FolioServiceClient.GetFeeType(id?.ToString()));
            if (ft2 == null) return null;
            if (load && ft2.ChargeNoticeId != null) ft2.ChargeNotice = (Template2)(cache && objects.ContainsKey(ft2.ChargeNoticeId.Value) ? objects[ft2.ChargeNoticeId.Value] : objects[ft2.ChargeNoticeId.Value] = FindTemplate2(ft2.ChargeNoticeId));
            if (load && ft2.ActionNoticeId != null) ft2.ActionNotice = (Template2)(cache && objects.ContainsKey(ft2.ActionNoticeId.Value) ? objects[ft2.ActionNoticeId.Value] : objects[ft2.ActionNoticeId.Value] = FindTemplate2(ft2.ActionNoticeId));
            if (load && ft2.OwnerId != null) ft2.Owner = (Owner2)(cache && objects.ContainsKey(ft2.OwnerId.Value) ? objects[ft2.OwnerId.Value] : objects[ft2.OwnerId.Value] = FindOwner2(ft2.OwnerId));
            if (load && ft2.CreationUserId != null) ft2.CreationUser = (User2)(cache && objects.ContainsKey(ft2.CreationUserId.Value) ? objects[ft2.CreationUserId.Value] : objects[ft2.CreationUserId.Value] = FindUser2(ft2.CreationUserId));
            if (load && ft2.LastWriteUserId != null) ft2.LastWriteUser = (User2)(cache && objects.ContainsKey(ft2.LastWriteUserId.Value) ? objects[ft2.LastWriteUserId.Value] : objects[ft2.LastWriteUserId.Value] = FindUser2(ft2.LastWriteUserId));
            return ft2;
        }

        public void Insert(FeeType2 feeType2)
        {
            if (feeType2.Id == null) feeType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertFeeType(feeType2.ToJObject());
        }

        public void Update(FeeType2 feeType2) => FolioServiceClient.UpdateFeeType(feeType2.ToJObject());

        public void DeleteFeeType2(Guid? id) => FolioServiceClient.DeleteFeeType(id?.ToString());

        public bool AnyFinanceGroup2s(string where = null) => FolioServiceClient.AnyFinanceGroups(where);

        public int CountFinanceGroup2s(string where = null) => FolioServiceClient.CountFinanceGroups(where);

        public FinanceGroup2[] FinanceGroup2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.FinanceGroups(out count, where, orderBy, skip, take).Select(jo =>
            {
                var fg2 = FinanceGroup2.FromJObject(jo);
                if (load && fg2.CreationUserId != null) fg2.CreationUser = (User2)(cache && objects.ContainsKey(fg2.CreationUserId.Value) ? objects[fg2.CreationUserId.Value] : objects[fg2.CreationUserId.Value] = FindUser2(fg2.CreationUserId));
                if (load && fg2.LastWriteUserId != null) fg2.LastWriteUser = (User2)(cache && objects.ContainsKey(fg2.LastWriteUserId.Value) ? objects[fg2.LastWriteUserId.Value] : objects[fg2.LastWriteUserId.Value] = FindUser2(fg2.LastWriteUserId));
                return fg2;
            }).ToArray();
        }

        public IEnumerable<FinanceGroup2> FinanceGroup2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.FinanceGroups(where, orderBy, skip, take))
            {
                var fg2 = FinanceGroup2.FromJObject(jo);
                if (load && fg2.CreationUserId != null) fg2.CreationUser = (User2)(cache && objects.ContainsKey(fg2.CreationUserId.Value) ? objects[fg2.CreationUserId.Value] : objects[fg2.CreationUserId.Value] = FindUser2(fg2.CreationUserId));
                if (load && fg2.LastWriteUserId != null) fg2.LastWriteUser = (User2)(cache && objects.ContainsKey(fg2.LastWriteUserId.Value) ? objects[fg2.LastWriteUserId.Value] : objects[fg2.LastWriteUserId.Value] = FindUser2(fg2.LastWriteUserId));
                yield return fg2;
            }
        }

        public FinanceGroup2 FindFinanceGroup2(Guid? id, bool load = false, bool cache = true)
        {
            var fg2 = FinanceGroup2.FromJObject(FolioServiceClient.GetFinanceGroup(id?.ToString()));
            if (fg2 == null) return null;
            if (load && fg2.CreationUserId != null) fg2.CreationUser = (User2)(cache && objects.ContainsKey(fg2.CreationUserId.Value) ? objects[fg2.CreationUserId.Value] : objects[fg2.CreationUserId.Value] = FindUser2(fg2.CreationUserId));
            if (load && fg2.LastWriteUserId != null) fg2.LastWriteUser = (User2)(cache && objects.ContainsKey(fg2.LastWriteUserId.Value) ? objects[fg2.LastWriteUserId.Value] : objects[fg2.LastWriteUserId.Value] = FindUser2(fg2.LastWriteUserId));
            var i = 0;
            if (fg2.FinanceGroupAcquisitionsUnits != null) foreach (var fgau in fg2.FinanceGroupAcquisitionsUnits)
                {
                    fgau.Id = (++i).ToString();
                    fgau.FinanceGroupId = fg2.Id;
                }
            return fg2;
        }

        public void Insert(FinanceGroup2 financeGroup2)
        {
            if (financeGroup2.Id == null) financeGroup2.Id = Guid.NewGuid();
            FolioServiceClient.InsertFinanceGroup(financeGroup2.ToJObject());
        }

        public void Update(FinanceGroup2 financeGroup2) => FolioServiceClient.UpdateFinanceGroup(financeGroup2.ToJObject());

        public void DeleteFinanceGroup2(Guid? id) => FolioServiceClient.DeleteFinanceGroup(id?.ToString());

        public bool AnyFiscalYear2s(string where = null) => FolioServiceClient.AnyFiscalYears(where);

        public int CountFiscalYear2s(string where = null) => FolioServiceClient.CountFiscalYears(where);

        public FiscalYear2[] FiscalYear2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.FiscalYears(out count, where, orderBy, skip, take).Select(jo =>
            {
                var fy2 = FiscalYear2.FromJObject(jo);
                if (load && fy2.CreationUserId != null) fy2.CreationUser = (User2)(cache && objects.ContainsKey(fy2.CreationUserId.Value) ? objects[fy2.CreationUserId.Value] : objects[fy2.CreationUserId.Value] = FindUser2(fy2.CreationUserId));
                if (load && fy2.LastWriteUserId != null) fy2.LastWriteUser = (User2)(cache && objects.ContainsKey(fy2.LastWriteUserId.Value) ? objects[fy2.LastWriteUserId.Value] : objects[fy2.LastWriteUserId.Value] = FindUser2(fy2.LastWriteUserId));
                return fy2;
            }).ToArray();
        }

        public IEnumerable<FiscalYear2> FiscalYear2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.FiscalYears(where, orderBy, skip, take))
            {
                var fy2 = FiscalYear2.FromJObject(jo);
                if (load && fy2.CreationUserId != null) fy2.CreationUser = (User2)(cache && objects.ContainsKey(fy2.CreationUserId.Value) ? objects[fy2.CreationUserId.Value] : objects[fy2.CreationUserId.Value] = FindUser2(fy2.CreationUserId));
                if (load && fy2.LastWriteUserId != null) fy2.LastWriteUser = (User2)(cache && objects.ContainsKey(fy2.LastWriteUserId.Value) ? objects[fy2.LastWriteUserId.Value] : objects[fy2.LastWriteUserId.Value] = FindUser2(fy2.LastWriteUserId));
                yield return fy2;
            }
        }

        public FiscalYear2 FindFiscalYear2(Guid? id, bool load = false, bool cache = true)
        {
            var fy2 = FiscalYear2.FromJObject(FolioServiceClient.GetFiscalYear(id?.ToString()));
            if (fy2 == null) return null;
            if (load && fy2.CreationUserId != null) fy2.CreationUser = (User2)(cache && objects.ContainsKey(fy2.CreationUserId.Value) ? objects[fy2.CreationUserId.Value] : objects[fy2.CreationUserId.Value] = FindUser2(fy2.CreationUserId));
            if (load && fy2.LastWriteUserId != null) fy2.LastWriteUser = (User2)(cache && objects.ContainsKey(fy2.LastWriteUserId.Value) ? objects[fy2.LastWriteUserId.Value] : objects[fy2.LastWriteUserId.Value] = FindUser2(fy2.LastWriteUserId));
            var i = 0;
            if (fy2.FiscalYearAcquisitionsUnits != null) foreach (var fyau in fy2.FiscalYearAcquisitionsUnits)
                {
                    fyau.Id = (++i).ToString();
                    fyau.FiscalYearId = fy2.Id;
                }
            return fy2;
        }

        public void Insert(FiscalYear2 fiscalYear2)
        {
            if (fiscalYear2.Id == null) fiscalYear2.Id = Guid.NewGuid();
            FolioServiceClient.InsertFiscalYear(fiscalYear2.ToJObject());
        }

        public void Update(FiscalYear2 fiscalYear2) => FolioServiceClient.UpdateFiscalYear(fiscalYear2.ToJObject());

        public void DeleteFiscalYear2(Guid? id) => FolioServiceClient.DeleteFiscalYear(id?.ToString());

        public bool AnyFixedDueDateSchedule2s(string where = null) => FolioServiceClient.AnyFixedDueDateSchedules(where);

        public int CountFixedDueDateSchedule2s(string where = null) => FolioServiceClient.CountFixedDueDateSchedules(where);

        public FixedDueDateSchedule2[] FixedDueDateSchedule2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.FixedDueDateSchedules(out count, where, orderBy, skip, take).Select(jo =>
            {
                var fdds2 = FixedDueDateSchedule2.FromJObject(jo);
                if (load && fdds2.CreationUserId != null) fdds2.CreationUser = (User2)(cache && objects.ContainsKey(fdds2.CreationUserId.Value) ? objects[fdds2.CreationUserId.Value] : objects[fdds2.CreationUserId.Value] = FindUser2(fdds2.CreationUserId));
                if (load && fdds2.LastWriteUserId != null) fdds2.LastWriteUser = (User2)(cache && objects.ContainsKey(fdds2.LastWriteUserId.Value) ? objects[fdds2.LastWriteUserId.Value] : objects[fdds2.LastWriteUserId.Value] = FindUser2(fdds2.LastWriteUserId));
                return fdds2;
            }).ToArray();
        }

        public IEnumerable<FixedDueDateSchedule2> FixedDueDateSchedule2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.FixedDueDateSchedules(where, orderBy, skip, take))
            {
                var fdds2 = FixedDueDateSchedule2.FromJObject(jo);
                if (load && fdds2.CreationUserId != null) fdds2.CreationUser = (User2)(cache && objects.ContainsKey(fdds2.CreationUserId.Value) ? objects[fdds2.CreationUserId.Value] : objects[fdds2.CreationUserId.Value] = FindUser2(fdds2.CreationUserId));
                if (load && fdds2.LastWriteUserId != null) fdds2.LastWriteUser = (User2)(cache && objects.ContainsKey(fdds2.LastWriteUserId.Value) ? objects[fdds2.LastWriteUserId.Value] : objects[fdds2.LastWriteUserId.Value] = FindUser2(fdds2.LastWriteUserId));
                yield return fdds2;
            }
        }

        public FixedDueDateSchedule2 FindFixedDueDateSchedule2(Guid? id, bool load = false, bool cache = true)
        {
            var fdds2 = FixedDueDateSchedule2.FromJObject(FolioServiceClient.GetFixedDueDateSchedule(id?.ToString()));
            if (fdds2 == null) return null;
            if (load && fdds2.CreationUserId != null) fdds2.CreationUser = (User2)(cache && objects.ContainsKey(fdds2.CreationUserId.Value) ? objects[fdds2.CreationUserId.Value] : objects[fdds2.CreationUserId.Value] = FindUser2(fdds2.CreationUserId));
            if (load && fdds2.LastWriteUserId != null) fdds2.LastWriteUser = (User2)(cache && objects.ContainsKey(fdds2.LastWriteUserId.Value) ? objects[fdds2.LastWriteUserId.Value] : objects[fdds2.LastWriteUserId.Value] = FindUser2(fdds2.LastWriteUserId));
            var i = 0;
            if (fdds2.FixedDueDateScheduleSchedules != null) foreach (var fddss in fdds2.FixedDueDateScheduleSchedules)
                {
                    fddss.Id = (++i).ToString();
                    fddss.FixedDueDateScheduleId = fdds2.Id;
                }
            return fdds2;
        }

        public void Insert(FixedDueDateSchedule2 fixedDueDateSchedule2)
        {
            if (fixedDueDateSchedule2.Id == null) fixedDueDateSchedule2.Id = Guid.NewGuid();
            FolioServiceClient.InsertFixedDueDateSchedule(fixedDueDateSchedule2.ToJObject());
        }

        public void Update(FixedDueDateSchedule2 fixedDueDateSchedule2) => FolioServiceClient.UpdateFixedDueDateSchedule(fixedDueDateSchedule2.ToJObject());

        public void DeleteFixedDueDateSchedule2(Guid? id) => FolioServiceClient.DeleteFixedDueDateSchedule(id?.ToString());

        public bool AnyFormats(string where = null) => FolioServiceClient.AnyInstanceFormats(where);

        public int CountFormats(string where = null) => FolioServiceClient.CountInstanceFormats(where);

        public Format[] Formats(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.InstanceFormats(out count, where, orderBy, skip, take).Select(jo =>
            {
                var f = Format.FromJObject(jo);
                if (load && f.CreationUserId != null) f.CreationUser = (User2)(cache && objects.ContainsKey(f.CreationUserId.Value) ? objects[f.CreationUserId.Value] : objects[f.CreationUserId.Value] = FindUser2(f.CreationUserId));
                if (load && f.LastWriteUserId != null) f.LastWriteUser = (User2)(cache && objects.ContainsKey(f.LastWriteUserId.Value) ? objects[f.LastWriteUserId.Value] : objects[f.LastWriteUserId.Value] = FindUser2(f.LastWriteUserId));
                return f;
            }).ToArray();
        }

        public IEnumerable<Format> Formats(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.InstanceFormats(where, orderBy, skip, take))
            {
                var f = Format.FromJObject(jo);
                if (load && f.CreationUserId != null) f.CreationUser = (User2)(cache && objects.ContainsKey(f.CreationUserId.Value) ? objects[f.CreationUserId.Value] : objects[f.CreationUserId.Value] = FindUser2(f.CreationUserId));
                if (load && f.LastWriteUserId != null) f.LastWriteUser = (User2)(cache && objects.ContainsKey(f.LastWriteUserId.Value) ? objects[f.LastWriteUserId.Value] : objects[f.LastWriteUserId.Value] = FindUser2(f.LastWriteUserId));
                yield return f;
            }
        }

        public Format FindFormat(Guid? id, bool load = false, bool cache = true)
        {
            var f = Format.FromJObject(FolioServiceClient.GetInstanceFormat(id?.ToString()));
            if (f == null) return null;
            if (load && f.CreationUserId != null) f.CreationUser = (User2)(cache && objects.ContainsKey(f.CreationUserId.Value) ? objects[f.CreationUserId.Value] : objects[f.CreationUserId.Value] = FindUser2(f.CreationUserId));
            if (load && f.LastWriteUserId != null) f.LastWriteUser = (User2)(cache && objects.ContainsKey(f.LastWriteUserId.Value) ? objects[f.LastWriteUserId.Value] : objects[f.LastWriteUserId.Value] = FindUser2(f.LastWriteUserId));
            return f;
        }

        public void Insert(Format format)
        {
            if (format.Id == null) format.Id = Guid.NewGuid();
            FolioServiceClient.InsertInstanceFormat(format.ToJObject());
        }

        public void Update(Format format) => FolioServiceClient.UpdateInstanceFormat(format.ToJObject());

        public void DeleteFormat(Guid? id) => FolioServiceClient.DeleteInstanceFormat(id?.ToString());

        public bool AnyFund2s(string where = null) => FolioServiceClient.AnyFunds(where);

        public int CountFund2s(string where = null) => FolioServiceClient.CountFunds(where);

        public Fund2[] Fund2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Funds(out count, where, orderBy, skip, take).Select(jo =>
            {
                var f2 = Fund2.FromJObject(jo);
                if (load && f2.FundTypeId != null) f2.FundType = (FundType2)(cache && objects.ContainsKey(f2.FundTypeId.Value) ? objects[f2.FundTypeId.Value] : objects[f2.FundTypeId.Value] = FindFundType2(f2.FundTypeId));
                if (load && f2.LedgerId != null) f2.Ledger = (Ledger2)(cache && objects.ContainsKey(f2.LedgerId.Value) ? objects[f2.LedgerId.Value] : objects[f2.LedgerId.Value] = FindLedger2(f2.LedgerId));
                if (load && f2.CreationUserId != null) f2.CreationUser = (User2)(cache && objects.ContainsKey(f2.CreationUserId.Value) ? objects[f2.CreationUserId.Value] : objects[f2.CreationUserId.Value] = FindUser2(f2.CreationUserId));
                if (load && f2.LastWriteUserId != null) f2.LastWriteUser = (User2)(cache && objects.ContainsKey(f2.LastWriteUserId.Value) ? objects[f2.LastWriteUserId.Value] : objects[f2.LastWriteUserId.Value] = FindUser2(f2.LastWriteUserId));
                return f2;
            }).ToArray();
        }

        public IEnumerable<Fund2> Fund2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Funds(where, orderBy, skip, take))
            {
                var f2 = Fund2.FromJObject(jo);
                if (load && f2.FundTypeId != null) f2.FundType = (FundType2)(cache && objects.ContainsKey(f2.FundTypeId.Value) ? objects[f2.FundTypeId.Value] : objects[f2.FundTypeId.Value] = FindFundType2(f2.FundTypeId));
                if (load && f2.LedgerId != null) f2.Ledger = (Ledger2)(cache && objects.ContainsKey(f2.LedgerId.Value) ? objects[f2.LedgerId.Value] : objects[f2.LedgerId.Value] = FindLedger2(f2.LedgerId));
                if (load && f2.CreationUserId != null) f2.CreationUser = (User2)(cache && objects.ContainsKey(f2.CreationUserId.Value) ? objects[f2.CreationUserId.Value] : objects[f2.CreationUserId.Value] = FindUser2(f2.CreationUserId));
                if (load && f2.LastWriteUserId != null) f2.LastWriteUser = (User2)(cache && objects.ContainsKey(f2.LastWriteUserId.Value) ? objects[f2.LastWriteUserId.Value] : objects[f2.LastWriteUserId.Value] = FindUser2(f2.LastWriteUserId));
                yield return f2;
            }
        }

        public Fund2 FindFund2(Guid? id, bool load = false, bool cache = true)
        {
            var f2 = Fund2.FromJObject(FolioServiceClient.GetFund(id?.ToString()));
            if (f2 == null) return null;
            if (load && f2.FundTypeId != null) f2.FundType = (FundType2)(cache && objects.ContainsKey(f2.FundTypeId.Value) ? objects[f2.FundTypeId.Value] : objects[f2.FundTypeId.Value] = FindFundType2(f2.FundTypeId));
            if (load && f2.LedgerId != null) f2.Ledger = (Ledger2)(cache && objects.ContainsKey(f2.LedgerId.Value) ? objects[f2.LedgerId.Value] : objects[f2.LedgerId.Value] = FindLedger2(f2.LedgerId));
            if (load && f2.CreationUserId != null) f2.CreationUser = (User2)(cache && objects.ContainsKey(f2.CreationUserId.Value) ? objects[f2.CreationUserId.Value] : objects[f2.CreationUserId.Value] = FindUser2(f2.CreationUserId));
            if (load && f2.LastWriteUserId != null) f2.LastWriteUser = (User2)(cache && objects.ContainsKey(f2.LastWriteUserId.Value) ? objects[f2.LastWriteUserId.Value] : objects[f2.LastWriteUserId.Value] = FindUser2(f2.LastWriteUserId));
            var i = 0;
            if (f2.AllocatedFromFunds != null) foreach (var aff in f2.AllocatedFromFunds)
                {
                    aff.Id = (++i).ToString();
                    aff.FromFundId = f2.Id;
                }
            i = 0;
            if (f2.AllocatedToFunds != null) foreach (var atf in f2.AllocatedToFunds)
                {
                    atf.Id = (++i).ToString();
                    atf.ToFundId = f2.Id;
                }
            i = 0;
            if (f2.FundAcquisitionsUnits != null) foreach (var fau in f2.FundAcquisitionsUnits)
                {
                    fau.Id = (++i).ToString();
                    fau.FundId = f2.Id;
                }
            i = 0;
            if (f2.FundTags != null) foreach (var ft in f2.FundTags)
                {
                    ft.Id = (++i).ToString();
                    ft.FundId = f2.Id;
                }
            return f2;
        }

        public void Insert(Fund2 fund2)
        {
            if (fund2.Id == null) fund2.Id = Guid.NewGuid();
            FolioServiceClient.InsertFund(fund2.ToJObject());
        }

        public void Update(Fund2 fund2) => FolioServiceClient.UpdateFund(fund2.ToJObject());

        public void DeleteFund2(Guid? id) => FolioServiceClient.DeleteFund(id?.ToString());

        public bool AnyFundType2s(string where = null) => FolioServiceClient.AnyFundTypes(where);

        public int CountFundType2s(string where = null) => FolioServiceClient.CountFundTypes(where);

        public FundType2[] FundType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.FundTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var ft2 = FundType2.FromJObject(jo);
                return ft2;
            }).ToArray();
        }

        public IEnumerable<FundType2> FundType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.FundTypes(where, orderBy, skip, take))
            {
                var ft2 = FundType2.FromJObject(jo);
                yield return ft2;
            }
        }

        public FundType2 FindFundType2(Guid? id, bool load = false, bool cache = true) => FundType2.FromJObject(FolioServiceClient.GetFundType(id?.ToString()));

        public void Insert(FundType2 fundType2)
        {
            if (fundType2.Id == null) fundType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertFundType(fundType2.ToJObject());
        }

        public void Update(FundType2 fundType2) => FolioServiceClient.UpdateFundType(fundType2.ToJObject());

        public void DeleteFundType2(Guid? id) => FolioServiceClient.DeleteFundType(id?.ToString());

        public bool AnyGroup2s(string where = null) => FolioServiceClient.AnyGroups(where);

        public int CountGroup2s(string where = null) => FolioServiceClient.CountGroups(where);

        public Group2[] Group2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Groups(out count, where, orderBy, skip, take).Select(jo =>
            {
                var g2 = Group2.FromJObject(jo);
                if (load && g2.CreationUserId != null) g2.CreationUser = (User2)(cache && objects.ContainsKey(g2.CreationUserId.Value) ? objects[g2.CreationUserId.Value] : objects[g2.CreationUserId.Value] = FindUser2(g2.CreationUserId));
                if (load && g2.LastWriteUserId != null) g2.LastWriteUser = (User2)(cache && objects.ContainsKey(g2.LastWriteUserId.Value) ? objects[g2.LastWriteUserId.Value] : objects[g2.LastWriteUserId.Value] = FindUser2(g2.LastWriteUserId));
                return g2;
            }).ToArray();
        }

        public IEnumerable<Group2> Group2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Groups(where, orderBy, skip, take))
            {
                var g2 = Group2.FromJObject(jo);
                if (load && g2.CreationUserId != null) g2.CreationUser = (User2)(cache && objects.ContainsKey(g2.CreationUserId.Value) ? objects[g2.CreationUserId.Value] : objects[g2.CreationUserId.Value] = FindUser2(g2.CreationUserId));
                if (load && g2.LastWriteUserId != null) g2.LastWriteUser = (User2)(cache && objects.ContainsKey(g2.LastWriteUserId.Value) ? objects[g2.LastWriteUserId.Value] : objects[g2.LastWriteUserId.Value] = FindUser2(g2.LastWriteUserId));
                yield return g2;
            }
        }

        public Group2 FindGroup2(Guid? id, bool load = false, bool cache = true)
        {
            var g2 = Group2.FromJObject(FolioServiceClient.GetGroup(id?.ToString()));
            if (g2 == null) return null;
            if (load && g2.CreationUserId != null) g2.CreationUser = (User2)(cache && objects.ContainsKey(g2.CreationUserId.Value) ? objects[g2.CreationUserId.Value] : objects[g2.CreationUserId.Value] = FindUser2(g2.CreationUserId));
            if (load && g2.LastWriteUserId != null) g2.LastWriteUser = (User2)(cache && objects.ContainsKey(g2.LastWriteUserId.Value) ? objects[g2.LastWriteUserId.Value] : objects[g2.LastWriteUserId.Value] = FindUser2(g2.LastWriteUserId));
            return g2;
        }

        public void Insert(Group2 group2)
        {
            if (group2.Id == null) group2.Id = Guid.NewGuid();
            FolioServiceClient.InsertGroup(group2.ToJObject());
        }

        public void Update(Group2 group2) => FolioServiceClient.UpdateGroup(group2.ToJObject());

        public void DeleteGroup2(Guid? id) => FolioServiceClient.DeleteGroup(id?.ToString());

        public bool AnyHolding2s(string where = null) => FolioServiceClient.AnyHoldings(where);

        public int CountHolding2s(string where = null) => FolioServiceClient.CountHoldings(where);

        public Holding2[] Holding2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Holdings(out count, where, orderBy, skip, take).Select(jo =>
            {
                var h2 = Holding2.FromJObject(jo);
                if (load && h2.HoldingTypeId != null) h2.HoldingType = (HoldingType2)(cache && objects.ContainsKey(h2.HoldingTypeId.Value) ? objects[h2.HoldingTypeId.Value] : objects[h2.HoldingTypeId.Value] = FindHoldingType2(h2.HoldingTypeId));
                if (load && h2.InstanceId != null) h2.Instance = (Instance2)(cache && objects.ContainsKey(h2.InstanceId.Value) ? objects[h2.InstanceId.Value] : objects[h2.InstanceId.Value] = FindInstance2(h2.InstanceId));
                if (load && h2.LocationId != null) h2.Location = (Location2)(cache && objects.ContainsKey(h2.LocationId.Value) ? objects[h2.LocationId.Value] : objects[h2.LocationId.Value] = FindLocation2(h2.LocationId));
                if (load && h2.TemporaryLocationId != null) h2.TemporaryLocation = (Location2)(cache && objects.ContainsKey(h2.TemporaryLocationId.Value) ? objects[h2.TemporaryLocationId.Value] : objects[h2.TemporaryLocationId.Value] = FindLocation2(h2.TemporaryLocationId));
                if (load && h2.EffectiveLocationId != null) h2.EffectiveLocation = (Location2)(cache && objects.ContainsKey(h2.EffectiveLocationId.Value) ? objects[h2.EffectiveLocationId.Value] : objects[h2.EffectiveLocationId.Value] = FindLocation2(h2.EffectiveLocationId));
                if (load && h2.CallNumberTypeId != null) h2.CallNumberType = (CallNumberType2)(cache && objects.ContainsKey(h2.CallNumberTypeId.Value) ? objects[h2.CallNumberTypeId.Value] : objects[h2.CallNumberTypeId.Value] = FindCallNumberType2(h2.CallNumberTypeId));
                if (load && h2.IllPolicyId != null) h2.IllPolicy = (IllPolicy2)(cache && objects.ContainsKey(h2.IllPolicyId.Value) ? objects[h2.IllPolicyId.Value] : objects[h2.IllPolicyId.Value] = FindIllPolicy2(h2.IllPolicyId));
                if (load && h2.CreationUserId != null) h2.CreationUser = (User2)(cache && objects.ContainsKey(h2.CreationUserId.Value) ? objects[h2.CreationUserId.Value] : objects[h2.CreationUserId.Value] = FindUser2(h2.CreationUserId));
                if (load && h2.LastWriteUserId != null) h2.LastWriteUser = (User2)(cache && objects.ContainsKey(h2.LastWriteUserId.Value) ? objects[h2.LastWriteUserId.Value] : objects[h2.LastWriteUserId.Value] = FindUser2(h2.LastWriteUserId));
                if (load && h2.SourceId != null) h2.Source = (Source2)(cache && objects.ContainsKey(h2.SourceId.Value) ? objects[h2.SourceId.Value] : objects[h2.SourceId.Value] = FindSource2(h2.SourceId));
                return h2;
            }).ToArray();
        }

        public IEnumerable<Holding2> Holding2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Holdings(where, orderBy, skip, take))
            {
                var h2 = Holding2.FromJObject(jo);
                if (load && h2.HoldingTypeId != null) h2.HoldingType = (HoldingType2)(cache && objects.ContainsKey(h2.HoldingTypeId.Value) ? objects[h2.HoldingTypeId.Value] : objects[h2.HoldingTypeId.Value] = FindHoldingType2(h2.HoldingTypeId));
                if (load && h2.InstanceId != null) h2.Instance = (Instance2)(cache && objects.ContainsKey(h2.InstanceId.Value) ? objects[h2.InstanceId.Value] : objects[h2.InstanceId.Value] = FindInstance2(h2.InstanceId));
                if (load && h2.LocationId != null) h2.Location = (Location2)(cache && objects.ContainsKey(h2.LocationId.Value) ? objects[h2.LocationId.Value] : objects[h2.LocationId.Value] = FindLocation2(h2.LocationId));
                if (load && h2.TemporaryLocationId != null) h2.TemporaryLocation = (Location2)(cache && objects.ContainsKey(h2.TemporaryLocationId.Value) ? objects[h2.TemporaryLocationId.Value] : objects[h2.TemporaryLocationId.Value] = FindLocation2(h2.TemporaryLocationId));
                if (load && h2.EffectiveLocationId != null) h2.EffectiveLocation = (Location2)(cache && objects.ContainsKey(h2.EffectiveLocationId.Value) ? objects[h2.EffectiveLocationId.Value] : objects[h2.EffectiveLocationId.Value] = FindLocation2(h2.EffectiveLocationId));
                if (load && h2.CallNumberTypeId != null) h2.CallNumberType = (CallNumberType2)(cache && objects.ContainsKey(h2.CallNumberTypeId.Value) ? objects[h2.CallNumberTypeId.Value] : objects[h2.CallNumberTypeId.Value] = FindCallNumberType2(h2.CallNumberTypeId));
                if (load && h2.IllPolicyId != null) h2.IllPolicy = (IllPolicy2)(cache && objects.ContainsKey(h2.IllPolicyId.Value) ? objects[h2.IllPolicyId.Value] : objects[h2.IllPolicyId.Value] = FindIllPolicy2(h2.IllPolicyId));
                if (load && h2.CreationUserId != null) h2.CreationUser = (User2)(cache && objects.ContainsKey(h2.CreationUserId.Value) ? objects[h2.CreationUserId.Value] : objects[h2.CreationUserId.Value] = FindUser2(h2.CreationUserId));
                if (load && h2.LastWriteUserId != null) h2.LastWriteUser = (User2)(cache && objects.ContainsKey(h2.LastWriteUserId.Value) ? objects[h2.LastWriteUserId.Value] : objects[h2.LastWriteUserId.Value] = FindUser2(h2.LastWriteUserId));
                if (load && h2.SourceId != null) h2.Source = (Source2)(cache && objects.ContainsKey(h2.SourceId.Value) ? objects[h2.SourceId.Value] : objects[h2.SourceId.Value] = FindSource2(h2.SourceId));
                yield return h2;
            }
        }

        public Holding2 FindHolding2(Guid? id, bool load = false, bool cache = true)
        {
            var h2 = Holding2.FromJObject(FolioServiceClient.GetHolding(id?.ToString()));
            if (h2 == null) return null;
            if (load && h2.HoldingTypeId != null) h2.HoldingType = (HoldingType2)(cache && objects.ContainsKey(h2.HoldingTypeId.Value) ? objects[h2.HoldingTypeId.Value] : objects[h2.HoldingTypeId.Value] = FindHoldingType2(h2.HoldingTypeId));
            if (load && h2.InstanceId != null) h2.Instance = (Instance2)(cache && objects.ContainsKey(h2.InstanceId.Value) ? objects[h2.InstanceId.Value] : objects[h2.InstanceId.Value] = FindInstance2(h2.InstanceId));
            if (load && h2.LocationId != null) h2.Location = (Location2)(cache && objects.ContainsKey(h2.LocationId.Value) ? objects[h2.LocationId.Value] : objects[h2.LocationId.Value] = FindLocation2(h2.LocationId));
            if (load && h2.TemporaryLocationId != null) h2.TemporaryLocation = (Location2)(cache && objects.ContainsKey(h2.TemporaryLocationId.Value) ? objects[h2.TemporaryLocationId.Value] : objects[h2.TemporaryLocationId.Value] = FindLocation2(h2.TemporaryLocationId));
            if (load && h2.EffectiveLocationId != null) h2.EffectiveLocation = (Location2)(cache && objects.ContainsKey(h2.EffectiveLocationId.Value) ? objects[h2.EffectiveLocationId.Value] : objects[h2.EffectiveLocationId.Value] = FindLocation2(h2.EffectiveLocationId));
            if (load && h2.CallNumberTypeId != null) h2.CallNumberType = (CallNumberType2)(cache && objects.ContainsKey(h2.CallNumberTypeId.Value) ? objects[h2.CallNumberTypeId.Value] : objects[h2.CallNumberTypeId.Value] = FindCallNumberType2(h2.CallNumberTypeId));
            if (load && h2.IllPolicyId != null) h2.IllPolicy = (IllPolicy2)(cache && objects.ContainsKey(h2.IllPolicyId.Value) ? objects[h2.IllPolicyId.Value] : objects[h2.IllPolicyId.Value] = FindIllPolicy2(h2.IllPolicyId));
            if (load && h2.CreationUserId != null) h2.CreationUser = (User2)(cache && objects.ContainsKey(h2.CreationUserId.Value) ? objects[h2.CreationUserId.Value] : objects[h2.CreationUserId.Value] = FindUser2(h2.CreationUserId));
            if (load && h2.LastWriteUserId != null) h2.LastWriteUser = (User2)(cache && objects.ContainsKey(h2.LastWriteUserId.Value) ? objects[h2.LastWriteUserId.Value] : objects[h2.LastWriteUserId.Value] = FindUser2(h2.LastWriteUserId));
            if (load && h2.SourceId != null) h2.Source = (Source2)(cache && objects.ContainsKey(h2.SourceId.Value) ? objects[h2.SourceId.Value] : objects[h2.SourceId.Value] = FindSource2(h2.SourceId));
            var i = 0;
            if (h2.Extents != null) foreach (var e2 in h2.Extents)
                {
                    e2.Id = (++i).ToString();
                    e2.HoldingId = h2.Id;
                }
            i = 0;
            if (h2.HoldingElectronicAccesses != null) foreach (var hea in h2.HoldingElectronicAccesses)
                {
                    hea.Id = (++i).ToString();
                    hea.HoldingId = h2.Id;
                }
            i = 0;
            if (h2.HoldingEntries != null) foreach (var he in h2.HoldingEntries)
                {
                    he.Id = (++i).ToString();
                    he.HoldingId = h2.Id;
                }
            i = 0;
            if (h2.HoldingFormerIds != null) foreach (var hfi in h2.HoldingFormerIds)
                {
                    hfi.Id = (++i).ToString();
                    hfi.HoldingId = h2.Id;
                }
            i = 0;
            if (h2.HoldingNotes != null) foreach (var hn in h2.HoldingNotes)
                {
                    hn.Id = (++i).ToString();
                    hn.HoldingId = h2.Id;
                }
            i = 0;
            if (h2.HoldingStatisticalCodes != null) foreach (var hsc in h2.HoldingStatisticalCodes)
                {
                    hsc.Id = (++i).ToString();
                    hsc.HoldingId = h2.Id;
                }
            i = 0;
            if (h2.HoldingTags != null) foreach (var ht in h2.HoldingTags)
                {
                    ht.Id = (++i).ToString();
                    ht.HoldingId = h2.Id;
                }
            i = 0;
            if (h2.IndexStatements != null) foreach (var @is in h2.IndexStatements)
                {
                    @is.Id = (++i).ToString();
                    @is.HoldingId = h2.Id;
                }
            i = 0;
            if (h2.SupplementStatements != null) foreach (var ss in h2.SupplementStatements)
                {
                    ss.Id = (++i).ToString();
                    ss.HoldingId = h2.Id;
                }
            return h2;
        }

        public void Insert(Holding2 holding2)
        {
            if (holding2.Id == null) holding2.Id = Guid.NewGuid();
            FolioServiceClient.InsertHolding(holding2.ToJObject());
        }

        public void Update(Holding2 holding2) => FolioServiceClient.UpdateHolding(holding2.ToJObject());

        public void DeleteHolding2(Guid? id) => FolioServiceClient.DeleteHolding(id?.ToString());

        public bool AnyHoldingNoteType2s(string where = null) => FolioServiceClient.AnyHoldingNoteTypes(where);

        public int CountHoldingNoteType2s(string where = null) => FolioServiceClient.CountHoldingNoteTypes(where);

        public HoldingNoteType2[] HoldingNoteType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.HoldingNoteTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var hnt2 = HoldingNoteType2.FromJObject(jo);
                if (load && hnt2.CreationUserId != null) hnt2.CreationUser = (User2)(cache && objects.ContainsKey(hnt2.CreationUserId.Value) ? objects[hnt2.CreationUserId.Value] : objects[hnt2.CreationUserId.Value] = FindUser2(hnt2.CreationUserId));
                if (load && hnt2.LastWriteUserId != null) hnt2.LastWriteUser = (User2)(cache && objects.ContainsKey(hnt2.LastWriteUserId.Value) ? objects[hnt2.LastWriteUserId.Value] : objects[hnt2.LastWriteUserId.Value] = FindUser2(hnt2.LastWriteUserId));
                return hnt2;
            }).ToArray();
        }

        public IEnumerable<HoldingNoteType2> HoldingNoteType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.HoldingNoteTypes(where, orderBy, skip, take))
            {
                var hnt2 = HoldingNoteType2.FromJObject(jo);
                if (load && hnt2.CreationUserId != null) hnt2.CreationUser = (User2)(cache && objects.ContainsKey(hnt2.CreationUserId.Value) ? objects[hnt2.CreationUserId.Value] : objects[hnt2.CreationUserId.Value] = FindUser2(hnt2.CreationUserId));
                if (load && hnt2.LastWriteUserId != null) hnt2.LastWriteUser = (User2)(cache && objects.ContainsKey(hnt2.LastWriteUserId.Value) ? objects[hnt2.LastWriteUserId.Value] : objects[hnt2.LastWriteUserId.Value] = FindUser2(hnt2.LastWriteUserId));
                yield return hnt2;
            }
        }

        public HoldingNoteType2 FindHoldingNoteType2(Guid? id, bool load = false, bool cache = true)
        {
            var hnt2 = HoldingNoteType2.FromJObject(FolioServiceClient.GetHoldingNoteType(id?.ToString()));
            if (hnt2 == null) return null;
            if (load && hnt2.CreationUserId != null) hnt2.CreationUser = (User2)(cache && objects.ContainsKey(hnt2.CreationUserId.Value) ? objects[hnt2.CreationUserId.Value] : objects[hnt2.CreationUserId.Value] = FindUser2(hnt2.CreationUserId));
            if (load && hnt2.LastWriteUserId != null) hnt2.LastWriteUser = (User2)(cache && objects.ContainsKey(hnt2.LastWriteUserId.Value) ? objects[hnt2.LastWriteUserId.Value] : objects[hnt2.LastWriteUserId.Value] = FindUser2(hnt2.LastWriteUserId));
            return hnt2;
        }

        public void Insert(HoldingNoteType2 holdingNoteType2)
        {
            if (holdingNoteType2.Id == null) holdingNoteType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertHoldingNoteType(holdingNoteType2.ToJObject());
        }

        public void Update(HoldingNoteType2 holdingNoteType2) => FolioServiceClient.UpdateHoldingNoteType(holdingNoteType2.ToJObject());

        public void DeleteHoldingNoteType2(Guid? id) => FolioServiceClient.DeleteHoldingNoteType(id?.ToString());

        public bool AnyHoldingType2s(string where = null) => FolioServiceClient.AnyHoldingTypes(where);

        public int CountHoldingType2s(string where = null) => FolioServiceClient.CountHoldingTypes(where);

        public HoldingType2[] HoldingType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.HoldingTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var ht2 = HoldingType2.FromJObject(jo);
                if (load && ht2.CreationUserId != null) ht2.CreationUser = (User2)(cache && objects.ContainsKey(ht2.CreationUserId.Value) ? objects[ht2.CreationUserId.Value] : objects[ht2.CreationUserId.Value] = FindUser2(ht2.CreationUserId));
                if (load && ht2.LastWriteUserId != null) ht2.LastWriteUser = (User2)(cache && objects.ContainsKey(ht2.LastWriteUserId.Value) ? objects[ht2.LastWriteUserId.Value] : objects[ht2.LastWriteUserId.Value] = FindUser2(ht2.LastWriteUserId));
                return ht2;
            }).ToArray();
        }

        public IEnumerable<HoldingType2> HoldingType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.HoldingTypes(where, orderBy, skip, take))
            {
                var ht2 = HoldingType2.FromJObject(jo);
                if (load && ht2.CreationUserId != null) ht2.CreationUser = (User2)(cache && objects.ContainsKey(ht2.CreationUserId.Value) ? objects[ht2.CreationUserId.Value] : objects[ht2.CreationUserId.Value] = FindUser2(ht2.CreationUserId));
                if (load && ht2.LastWriteUserId != null) ht2.LastWriteUser = (User2)(cache && objects.ContainsKey(ht2.LastWriteUserId.Value) ? objects[ht2.LastWriteUserId.Value] : objects[ht2.LastWriteUserId.Value] = FindUser2(ht2.LastWriteUserId));
                yield return ht2;
            }
        }

        public HoldingType2 FindHoldingType2(Guid? id, bool load = false, bool cache = true)
        {
            var ht2 = HoldingType2.FromJObject(FolioServiceClient.GetHoldingType(id?.ToString()));
            if (ht2 == null) return null;
            if (load && ht2.CreationUserId != null) ht2.CreationUser = (User2)(cache && objects.ContainsKey(ht2.CreationUserId.Value) ? objects[ht2.CreationUserId.Value] : objects[ht2.CreationUserId.Value] = FindUser2(ht2.CreationUserId));
            if (load && ht2.LastWriteUserId != null) ht2.LastWriteUser = (User2)(cache && objects.ContainsKey(ht2.LastWriteUserId.Value) ? objects[ht2.LastWriteUserId.Value] : objects[ht2.LastWriteUserId.Value] = FindUser2(ht2.LastWriteUserId));
            return ht2;
        }

        public void Insert(HoldingType2 holdingType2)
        {
            if (holdingType2.Id == null) holdingType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertHoldingType(holdingType2.ToJObject());
        }

        public void Update(HoldingType2 holdingType2) => FolioServiceClient.UpdateHoldingType(holdingType2.ToJObject());

        public void DeleteHoldingType2(Guid? id) => FolioServiceClient.DeleteHoldingType(id?.ToString());

        public HridSetting2 FindHridSetting2(bool load = false, bool cache = true) => HridSetting2.FromJObject(FolioServiceClient.GetHridSetting());

        public void Update(HridSetting2 hridSetting2) => FolioServiceClient.UpdateHridSetting(hridSetting2.ToJObject());

        public bool AnyIdType2s(string where = null) => FolioServiceClient.AnyIdTypes(where);

        public int CountIdType2s(string where = null) => FolioServiceClient.CountIdTypes(where);

        public IdType2[] IdType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.IdTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var it2 = IdType2.FromJObject(jo);
                if (load && it2.CreationUserId != null) it2.CreationUser = (User2)(cache && objects.ContainsKey(it2.CreationUserId.Value) ? objects[it2.CreationUserId.Value] : objects[it2.CreationUserId.Value] = FindUser2(it2.CreationUserId));
                if (load && it2.LastWriteUserId != null) it2.LastWriteUser = (User2)(cache && objects.ContainsKey(it2.LastWriteUserId.Value) ? objects[it2.LastWriteUserId.Value] : objects[it2.LastWriteUserId.Value] = FindUser2(it2.LastWriteUserId));
                return it2;
            }).ToArray();
        }

        public IEnumerable<IdType2> IdType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.IdTypes(where, orderBy, skip, take))
            {
                var it2 = IdType2.FromJObject(jo);
                if (load && it2.CreationUserId != null) it2.CreationUser = (User2)(cache && objects.ContainsKey(it2.CreationUserId.Value) ? objects[it2.CreationUserId.Value] : objects[it2.CreationUserId.Value] = FindUser2(it2.CreationUserId));
                if (load && it2.LastWriteUserId != null) it2.LastWriteUser = (User2)(cache && objects.ContainsKey(it2.LastWriteUserId.Value) ? objects[it2.LastWriteUserId.Value] : objects[it2.LastWriteUserId.Value] = FindUser2(it2.LastWriteUserId));
                yield return it2;
            }
        }

        public IdType2 FindIdType2(Guid? id, bool load = false, bool cache = true)
        {
            var it2 = IdType2.FromJObject(FolioServiceClient.GetIdType(id?.ToString()));
            if (it2 == null) return null;
            if (load && it2.CreationUserId != null) it2.CreationUser = (User2)(cache && objects.ContainsKey(it2.CreationUserId.Value) ? objects[it2.CreationUserId.Value] : objects[it2.CreationUserId.Value] = FindUser2(it2.CreationUserId));
            if (load && it2.LastWriteUserId != null) it2.LastWriteUser = (User2)(cache && objects.ContainsKey(it2.LastWriteUserId.Value) ? objects[it2.LastWriteUserId.Value] : objects[it2.LastWriteUserId.Value] = FindUser2(it2.LastWriteUserId));
            return it2;
        }

        public void Insert(IdType2 idType2)
        {
            if (idType2.Id == null) idType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertIdType(idType2.ToJObject());
        }

        public void Update(IdType2 idType2) => FolioServiceClient.UpdateIdType(idType2.ToJObject());

        public void DeleteIdType2(Guid? id) => FolioServiceClient.DeleteIdType(id?.ToString());

        public bool AnyIllPolicy2s(string where = null) => FolioServiceClient.AnyIllPolicies(where);

        public int CountIllPolicy2s(string where = null) => FolioServiceClient.CountIllPolicies(where);

        public IllPolicy2[] IllPolicy2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.IllPolicies(out count, where, orderBy, skip, take).Select(jo =>
            {
                var ip2 = IllPolicy2.FromJObject(jo);
                if (load && ip2.CreationUserId != null) ip2.CreationUser = (User2)(cache && objects.ContainsKey(ip2.CreationUserId.Value) ? objects[ip2.CreationUserId.Value] : objects[ip2.CreationUserId.Value] = FindUser2(ip2.CreationUserId));
                if (load && ip2.LastWriteUserId != null) ip2.LastWriteUser = (User2)(cache && objects.ContainsKey(ip2.LastWriteUserId.Value) ? objects[ip2.LastWriteUserId.Value] : objects[ip2.LastWriteUserId.Value] = FindUser2(ip2.LastWriteUserId));
                return ip2;
            }).ToArray();
        }

        public IEnumerable<IllPolicy2> IllPolicy2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.IllPolicies(where, orderBy, skip, take))
            {
                var ip2 = IllPolicy2.FromJObject(jo);
                if (load && ip2.CreationUserId != null) ip2.CreationUser = (User2)(cache && objects.ContainsKey(ip2.CreationUserId.Value) ? objects[ip2.CreationUserId.Value] : objects[ip2.CreationUserId.Value] = FindUser2(ip2.CreationUserId));
                if (load && ip2.LastWriteUserId != null) ip2.LastWriteUser = (User2)(cache && objects.ContainsKey(ip2.LastWriteUserId.Value) ? objects[ip2.LastWriteUserId.Value] : objects[ip2.LastWriteUserId.Value] = FindUser2(ip2.LastWriteUserId));
                yield return ip2;
            }
        }

        public IllPolicy2 FindIllPolicy2(Guid? id, bool load = false, bool cache = true)
        {
            var ip2 = IllPolicy2.FromJObject(FolioServiceClient.GetIllPolicy(id?.ToString()));
            if (ip2 == null) return null;
            if (load && ip2.CreationUserId != null) ip2.CreationUser = (User2)(cache && objects.ContainsKey(ip2.CreationUserId.Value) ? objects[ip2.CreationUserId.Value] : objects[ip2.CreationUserId.Value] = FindUser2(ip2.CreationUserId));
            if (load && ip2.LastWriteUserId != null) ip2.LastWriteUser = (User2)(cache && objects.ContainsKey(ip2.LastWriteUserId.Value) ? objects[ip2.LastWriteUserId.Value] : objects[ip2.LastWriteUserId.Value] = FindUser2(ip2.LastWriteUserId));
            return ip2;
        }

        public void Insert(IllPolicy2 illPolicy2)
        {
            if (illPolicy2.Id == null) illPolicy2.Id = Guid.NewGuid();
            FolioServiceClient.InsertIllPolicy(illPolicy2.ToJObject());
        }

        public void Update(IllPolicy2 illPolicy2) => FolioServiceClient.UpdateIllPolicy(illPolicy2.ToJObject());

        public void DeleteIllPolicy2(Guid? id) => FolioServiceClient.DeleteIllPolicy(id?.ToString());

        public bool AnyInstance2s(string where = null) => FolioServiceClient.AnyInstances(where);

        public int CountInstance2s(string where = null) => FolioServiceClient.CountInstances(where);

        public Instance2[] Instance2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Instances(out count, where, orderBy, skip, take).Select(jo =>
            {
                var i2 = Instance2.FromJObject(jo);
                if (load && i2.InstanceTypeId != null) i2.InstanceType = (InstanceType2)(cache && objects.ContainsKey(i2.InstanceTypeId.Value) ? objects[i2.InstanceTypeId.Value] : objects[i2.InstanceTypeId.Value] = FindInstanceType2(i2.InstanceTypeId));
                if (load && i2.IssuanceModeId != null) i2.IssuanceMode = (IssuanceMode)(cache && objects.ContainsKey(i2.IssuanceModeId.Value) ? objects[i2.IssuanceModeId.Value] : objects[i2.IssuanceModeId.Value] = FindIssuanceMode(i2.IssuanceModeId));
                if (load && i2.StatusId != null) i2.Status = (Status)(cache && objects.ContainsKey(i2.StatusId.Value) ? objects[i2.StatusId.Value] : objects[i2.StatusId.Value] = FindStatus(i2.StatusId));
                if (load && i2.CreationUserId != null) i2.CreationUser = (User2)(cache && objects.ContainsKey(i2.CreationUserId.Value) ? objects[i2.CreationUserId.Value] : objects[i2.CreationUserId.Value] = FindUser2(i2.CreationUserId));
                if (load && i2.LastWriteUserId != null) i2.LastWriteUser = (User2)(cache && objects.ContainsKey(i2.LastWriteUserId.Value) ? objects[i2.LastWriteUserId.Value] : objects[i2.LastWriteUserId.Value] = FindUser2(i2.LastWriteUserId));
                return i2;
            }).ToArray();
        }

        public IEnumerable<Instance2> Instance2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Instances(where, orderBy, skip, take))
            {
                var i2 = Instance2.FromJObject(jo);
                if (load && i2.InstanceTypeId != null) i2.InstanceType = (InstanceType2)(cache && objects.ContainsKey(i2.InstanceTypeId.Value) ? objects[i2.InstanceTypeId.Value] : objects[i2.InstanceTypeId.Value] = FindInstanceType2(i2.InstanceTypeId));
                if (load && i2.IssuanceModeId != null) i2.IssuanceMode = (IssuanceMode)(cache && objects.ContainsKey(i2.IssuanceModeId.Value) ? objects[i2.IssuanceModeId.Value] : objects[i2.IssuanceModeId.Value] = FindIssuanceMode(i2.IssuanceModeId));
                if (load && i2.StatusId != null) i2.Status = (Status)(cache && objects.ContainsKey(i2.StatusId.Value) ? objects[i2.StatusId.Value] : objects[i2.StatusId.Value] = FindStatus(i2.StatusId));
                if (load && i2.CreationUserId != null) i2.CreationUser = (User2)(cache && objects.ContainsKey(i2.CreationUserId.Value) ? objects[i2.CreationUserId.Value] : objects[i2.CreationUserId.Value] = FindUser2(i2.CreationUserId));
                if (load && i2.LastWriteUserId != null) i2.LastWriteUser = (User2)(cache && objects.ContainsKey(i2.LastWriteUserId.Value) ? objects[i2.LastWriteUserId.Value] : objects[i2.LastWriteUserId.Value] = FindUser2(i2.LastWriteUserId));
                yield return i2;
            }
        }

        public Instance2 FindInstance2(Guid? id, bool load = false, bool cache = true)
        {
            var i2 = Instance2.FromJObject(FolioServiceClient.GetInstance(id?.ToString()));
            if (i2 == null) return null;
            if (load && i2.InstanceTypeId != null) i2.InstanceType = (InstanceType2)(cache && objects.ContainsKey(i2.InstanceTypeId.Value) ? objects[i2.InstanceTypeId.Value] : objects[i2.InstanceTypeId.Value] = FindInstanceType2(i2.InstanceTypeId));
            if (load && i2.IssuanceModeId != null) i2.IssuanceMode = (IssuanceMode)(cache && objects.ContainsKey(i2.IssuanceModeId.Value) ? objects[i2.IssuanceModeId.Value] : objects[i2.IssuanceModeId.Value] = FindIssuanceMode(i2.IssuanceModeId));
            if (load && i2.StatusId != null) i2.Status = (Status)(cache && objects.ContainsKey(i2.StatusId.Value) ? objects[i2.StatusId.Value] : objects[i2.StatusId.Value] = FindStatus(i2.StatusId));
            if (load && i2.CreationUserId != null) i2.CreationUser = (User2)(cache && objects.ContainsKey(i2.CreationUserId.Value) ? objects[i2.CreationUserId.Value] : objects[i2.CreationUserId.Value] = FindUser2(i2.CreationUserId));
            if (load && i2.LastWriteUserId != null) i2.LastWriteUser = (User2)(cache && objects.ContainsKey(i2.LastWriteUserId.Value) ? objects[i2.LastWriteUserId.Value] : objects[i2.LastWriteUserId.Value] = FindUser2(i2.LastWriteUserId));
            var i = 0;
            if (i2.AlternativeTitles != null) foreach (var at in i2.AlternativeTitles)
                {
                    at.Id = (++i).ToString();
                    at.InstanceId = i2.Id;
                }
            i = 0;
            if (i2.Classifications != null) foreach (var c in i2.Classifications)
                {
                    c.Id = (++i).ToString();
                    c.InstanceId = i2.Id;
                }
            i = 0;
            if (i2.Contributors != null) foreach (var c2 in i2.Contributors)
                {
                    c2.Id = (++i).ToString();
                    c2.InstanceId = i2.Id;
                }
            i = 0;
            if (i2.Editions != null) foreach (var e2 in i2.Editions)
                {
                    e2.Id = (++i).ToString();
                    e2.InstanceId = i2.Id;
                }
            i = 0;
            if (i2.ElectronicAccesses != null) foreach (var ea in i2.ElectronicAccesses)
                {
                    ea.Id = (++i).ToString();
                    ea.InstanceId = i2.Id;
                }
            i = 0;

            if (i2.Identifiers != null) foreach (var i3 in i2.Identifiers)
                {
                    i3.Id = (++i).ToString();
                    i3.InstanceId = i2.Id;
                    i3.IdentifierType = FindIdType2(i3.IdentifierTypeId);
                }
            i = 0;
            if (i2.InstanceFormat2s != null) foreach (var if2 in i2.InstanceFormat2s)
                {
                    if2.Id = (++i).ToString();
                    if2.InstanceId = i2.Id;
                }
            i = 0;
            if (i2.InstanceNatureOfContentTerms != null) foreach (var inoct in i2.InstanceNatureOfContentTerms)
                {
                    inoct.Id = (++i).ToString();
                    inoct.InstanceId = i2.Id;
                }
            i = 0;
            if (i2.InstanceStatisticalCodes != null) foreach (var isc in i2.InstanceStatisticalCodes)
                {
                    isc.Id = (++i).ToString();
                    isc.InstanceId = i2.Id;
                }
            i = 0;
            if (i2.InstanceTags != null) foreach (var it in i2.InstanceTags)
                {
                    it.Id = (++i).ToString();
                    it.InstanceId = i2.Id;
                }
            i = 0;
            if (i2.Languages != null) foreach (var l in i2.Languages)
                {
                    l.Id = (++i).ToString();
                    l.InstanceId = i2.Id;
                }
            i = 0;
            if (i2.Note2s != null) foreach (var n2 in i2.Note2s)
                {
                    n2.Id = (++i).ToString();
                    n2.InstanceId = i2.Id;
                }
            i = 0;
            if (i2.PhysicalDescriptions != null) foreach (var pd in i2.PhysicalDescriptions)
                {
                    pd.Id = (++i).ToString();
                    pd.InstanceId = i2.Id;
                }
            i = 0;
            if (i2.PublicationFrequencies != null) foreach (var pf in i2.PublicationFrequencies)
                {
                    pf.Id = (++i).ToString();
                    pf.InstanceId = i2.Id;
                }
            i = 0;
            if (i2.PublicationRanges != null) foreach (var pr in i2.PublicationRanges)
                {
                    pr.Id = (++i).ToString();
                    pr.InstanceId = i2.Id;
                }
            i = 0;
            if (i2.Publications != null) foreach (var p in i2.Publications)
                {
                    p.Id = (++i).ToString();
                    p.InstanceId = i2.Id;
                }
            i = 0;
            if (i2.Series != null) foreach (var s in i2.Series)
                {
                    s.Id = (++i).ToString();
                    s.InstanceId = i2.Id;
                }
            i = 0;
            if (i2.Subjects != null) foreach (var s2 in i2.Subjects)
                {
                    s2.Id = (++i).ToString();
                    s2.InstanceId = i2.Id;
                }
            return i2;
        }

        public void Insert(Instance2 instance2)
        {
            if (instance2.Id == null) instance2.Id = Guid.NewGuid();
            FolioServiceClient.InsertInstance(instance2.ToJObject());
        }

        public void Update(Instance2 instance2) => FolioServiceClient.UpdateInstance(instance2.ToJObject());

        public void DeleteInstance2(Guid? id) => FolioServiceClient.DeleteInstance(id?.ToString());

        public bool AnyInstanceNoteType2s(string where = null) => FolioServiceClient.AnyInstanceNoteTypes(where);

        public int CountInstanceNoteType2s(string where = null) => FolioServiceClient.CountInstanceNoteTypes(where);

        public InstanceNoteType2[] InstanceNoteType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.InstanceNoteTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var int2 = InstanceNoteType2.FromJObject(jo);
                if (load && int2.CreationUserId != null) int2.CreationUser = (User2)(cache && objects.ContainsKey(int2.CreationUserId.Value) ? objects[int2.CreationUserId.Value] : objects[int2.CreationUserId.Value] = FindUser2(int2.CreationUserId));
                if (load && int2.LastWriteUserId != null) int2.LastWriteUser = (User2)(cache && objects.ContainsKey(int2.LastWriteUserId.Value) ? objects[int2.LastWriteUserId.Value] : objects[int2.LastWriteUserId.Value] = FindUser2(int2.LastWriteUserId));
                return int2;
            }).ToArray();
        }

        public IEnumerable<InstanceNoteType2> InstanceNoteType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.InstanceNoteTypes(where, orderBy, skip, take))
            {
                var int2 = InstanceNoteType2.FromJObject(jo);
                if (load && int2.CreationUserId != null) int2.CreationUser = (User2)(cache && objects.ContainsKey(int2.CreationUserId.Value) ? objects[int2.CreationUserId.Value] : objects[int2.CreationUserId.Value] = FindUser2(int2.CreationUserId));
                if (load && int2.LastWriteUserId != null) int2.LastWriteUser = (User2)(cache && objects.ContainsKey(int2.LastWriteUserId.Value) ? objects[int2.LastWriteUserId.Value] : objects[int2.LastWriteUserId.Value] = FindUser2(int2.LastWriteUserId));
                yield return int2;
            }
        }

        public InstanceNoteType2 FindInstanceNoteType2(Guid? id, bool load = false, bool cache = true)
        {
            var int2 = InstanceNoteType2.FromJObject(FolioServiceClient.GetInstanceNoteType(id?.ToString()));
            if (int2 == null) return null;
            if (load && int2.CreationUserId != null) int2.CreationUser = (User2)(cache && objects.ContainsKey(int2.CreationUserId.Value) ? objects[int2.CreationUserId.Value] : objects[int2.CreationUserId.Value] = FindUser2(int2.CreationUserId));
            if (load && int2.LastWriteUserId != null) int2.LastWriteUser = (User2)(cache && objects.ContainsKey(int2.LastWriteUserId.Value) ? objects[int2.LastWriteUserId.Value] : objects[int2.LastWriteUserId.Value] = FindUser2(int2.LastWriteUserId));
            return int2;
        }

        public void Insert(InstanceNoteType2 instanceNoteType2)
        {
            if (instanceNoteType2.Id == null) instanceNoteType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertInstanceNoteType(instanceNoteType2.ToJObject());
        }

        public void Update(InstanceNoteType2 instanceNoteType2) => FolioServiceClient.UpdateInstanceNoteType(instanceNoteType2.ToJObject());

        public void DeleteInstanceNoteType2(Guid? id) => FolioServiceClient.DeleteInstanceNoteType(id?.ToString());

        public bool AnyInstanceType2s(string where = null) => FolioServiceClient.AnyInstanceTypes(where);

        public int CountInstanceType2s(string where = null) => FolioServiceClient.CountInstanceTypes(where);

        public InstanceType2[] InstanceType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.InstanceTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var it2 = InstanceType2.FromJObject(jo);
                if (load && it2.CreationUserId != null) it2.CreationUser = (User2)(cache && objects.ContainsKey(it2.CreationUserId.Value) ? objects[it2.CreationUserId.Value] : objects[it2.CreationUserId.Value] = FindUser2(it2.CreationUserId));
                if (load && it2.LastWriteUserId != null) it2.LastWriteUser = (User2)(cache && objects.ContainsKey(it2.LastWriteUserId.Value) ? objects[it2.LastWriteUserId.Value] : objects[it2.LastWriteUserId.Value] = FindUser2(it2.LastWriteUserId));
                return it2;
            }).ToArray();
        }

        public IEnumerable<InstanceType2> InstanceType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.InstanceTypes(where, orderBy, skip, take))
            {
                var it2 = InstanceType2.FromJObject(jo);
                if (load && it2.CreationUserId != null) it2.CreationUser = (User2)(cache && objects.ContainsKey(it2.CreationUserId.Value) ? objects[it2.CreationUserId.Value] : objects[it2.CreationUserId.Value] = FindUser2(it2.CreationUserId));
                if (load && it2.LastWriteUserId != null) it2.LastWriteUser = (User2)(cache && objects.ContainsKey(it2.LastWriteUserId.Value) ? objects[it2.LastWriteUserId.Value] : objects[it2.LastWriteUserId.Value] = FindUser2(it2.LastWriteUserId));
                yield return it2;
            }
        }

        public InstanceType2 FindInstanceType2(Guid? id, bool load = false, bool cache = true)
        {
            var it2 = InstanceType2.FromJObject(FolioServiceClient.GetInstanceType(id?.ToString()));
            if (it2 == null) return null;
            if (load && it2.CreationUserId != null) it2.CreationUser = (User2)(cache && objects.ContainsKey(it2.CreationUserId.Value) ? objects[it2.CreationUserId.Value] : objects[it2.CreationUserId.Value] = FindUser2(it2.CreationUserId));
            if (load && it2.LastWriteUserId != null) it2.LastWriteUser = (User2)(cache && objects.ContainsKey(it2.LastWriteUserId.Value) ? objects[it2.LastWriteUserId.Value] : objects[it2.LastWriteUserId.Value] = FindUser2(it2.LastWriteUserId));
            return it2;
        }

        public void Insert(InstanceType2 instanceType2)
        {
            if (instanceType2.Id == null) instanceType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertInstanceType(instanceType2.ToJObject());
        }

        public void Update(InstanceType2 instanceType2) => FolioServiceClient.UpdateInstanceType(instanceType2.ToJObject());

        public void DeleteInstanceType2(Guid? id) => FolioServiceClient.DeleteInstanceType(id?.ToString());

        public bool AnyInstitution2s(string where = null) => FolioServiceClient.AnyInstitutions(where);

        public int CountInstitution2s(string where = null) => FolioServiceClient.CountInstitutions(where);

        public Institution2[] Institution2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Institutions(out count, where, orderBy, skip, take).Select(jo =>
            {
                var i2 = Institution2.FromJObject(jo);
                if (load && i2.CreationUserId != null) i2.CreationUser = (User2)(cache && objects.ContainsKey(i2.CreationUserId.Value) ? objects[i2.CreationUserId.Value] : objects[i2.CreationUserId.Value] = FindUser2(i2.CreationUserId));
                if (load && i2.LastWriteUserId != null) i2.LastWriteUser = (User2)(cache && objects.ContainsKey(i2.LastWriteUserId.Value) ? objects[i2.LastWriteUserId.Value] : objects[i2.LastWriteUserId.Value] = FindUser2(i2.LastWriteUserId));
                return i2;
            }).ToArray();
        }

        public IEnumerable<Institution2> Institution2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Institutions(where, orderBy, skip, take))
            {
                var i2 = Institution2.FromJObject(jo);
                if (load && i2.CreationUserId != null) i2.CreationUser = (User2)(cache && objects.ContainsKey(i2.CreationUserId.Value) ? objects[i2.CreationUserId.Value] : objects[i2.CreationUserId.Value] = FindUser2(i2.CreationUserId));
                if (load && i2.LastWriteUserId != null) i2.LastWriteUser = (User2)(cache && objects.ContainsKey(i2.LastWriteUserId.Value) ? objects[i2.LastWriteUserId.Value] : objects[i2.LastWriteUserId.Value] = FindUser2(i2.LastWriteUserId));
                yield return i2;
            }
        }

        public Institution2 FindInstitution2(Guid? id, bool load = false, bool cache = true)
        {
            var i2 = Institution2.FromJObject(FolioServiceClient.GetInstitution(id?.ToString()));
            if (i2 == null) return null;
            if (load && i2.CreationUserId != null) i2.CreationUser = (User2)(cache && objects.ContainsKey(i2.CreationUserId.Value) ? objects[i2.CreationUserId.Value] : objects[i2.CreationUserId.Value] = FindUser2(i2.CreationUserId));
            if (load && i2.LastWriteUserId != null) i2.LastWriteUser = (User2)(cache && objects.ContainsKey(i2.LastWriteUserId.Value) ? objects[i2.LastWriteUserId.Value] : objects[i2.LastWriteUserId.Value] = FindUser2(i2.LastWriteUserId));
            return i2;
        }

        public void Insert(Institution2 institution2)
        {
            if (institution2.Id == null) institution2.Id = Guid.NewGuid();
            FolioServiceClient.InsertInstitution(institution2.ToJObject());
        }

        public void Update(Institution2 institution2) => FolioServiceClient.UpdateInstitution(institution2.ToJObject());

        public void DeleteInstitution2(Guid? id) => FolioServiceClient.DeleteInstitution(id?.ToString());

        public bool AnyInterface2s(string where = null) => FolioServiceClient.AnyInterfaces(where);

        public int CountInterface2s(string where = null) => FolioServiceClient.CountInterfaces(where);

        public Interface2[] Interface2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Interfaces(out count, where, orderBy, skip, take).Select(jo =>
            {
                var i2 = Interface2.FromJObject(jo);
                if (load && i2.CreationUserId != null) i2.CreationUser = (User2)(cache && objects.ContainsKey(i2.CreationUserId.Value) ? objects[i2.CreationUserId.Value] : objects[i2.CreationUserId.Value] = FindUser2(i2.CreationUserId));
                if (load && i2.LastWriteUserId != null) i2.LastWriteUser = (User2)(cache && objects.ContainsKey(i2.LastWriteUserId.Value) ? objects[i2.LastWriteUserId.Value] : objects[i2.LastWriteUserId.Value] = FindUser2(i2.LastWriteUserId));
                return i2;
            }).ToArray();
        }

        public IEnumerable<Interface2> Interface2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Interfaces(where, orderBy, skip, take))
            {
                var i2 = Interface2.FromJObject(jo);
                if (load && i2.CreationUserId != null) i2.CreationUser = (User2)(cache && objects.ContainsKey(i2.CreationUserId.Value) ? objects[i2.CreationUserId.Value] : objects[i2.CreationUserId.Value] = FindUser2(i2.CreationUserId));
                if (load && i2.LastWriteUserId != null) i2.LastWriteUser = (User2)(cache && objects.ContainsKey(i2.LastWriteUserId.Value) ? objects[i2.LastWriteUserId.Value] : objects[i2.LastWriteUserId.Value] = FindUser2(i2.LastWriteUserId));
                yield return i2;
            }
        }

        public Interface2 FindInterface2(Guid? id, bool load = false, bool cache = true)
        {
            var i2 = Interface2.FromJObject(FolioServiceClient.GetInterface(id?.ToString()));
            if (i2 == null) return null;
            if (load && i2.CreationUserId != null) i2.CreationUser = (User2)(cache && objects.ContainsKey(i2.CreationUserId.Value) ? objects[i2.CreationUserId.Value] : objects[i2.CreationUserId.Value] = FindUser2(i2.CreationUserId));
            if (load && i2.LastWriteUserId != null) i2.LastWriteUser = (User2)(cache && objects.ContainsKey(i2.LastWriteUserId.Value) ? objects[i2.LastWriteUserId.Value] : objects[i2.LastWriteUserId.Value] = FindUser2(i2.LastWriteUserId));
            var i = 0;
            if (i2.InterfaceTypes != null) foreach (var it in i2.InterfaceTypes)
                {
                    it.Id = (++i).ToString();
                    it.InterfaceId = i2.Id;
                }
            return i2;
        }

        public void Insert(Interface2 interface2)
        {
            if (interface2.Id == null) interface2.Id = Guid.NewGuid();
            FolioServiceClient.InsertInterface(interface2.ToJObject());
        }

        public void Update(Interface2 interface2) => FolioServiceClient.UpdateInterface(interface2.ToJObject());

        public void DeleteInterface2(Guid? id) => FolioServiceClient.DeleteInterface(id?.ToString());

        public bool AnyInvoice2s(string where = null) => FolioServiceClient.AnyInvoices(where);

        public int CountInvoice2s(string where = null) => FolioServiceClient.CountInvoices(where);

        public Invoice2[] Invoice2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Invoices(out count, where, orderBy, skip, take).Select(jo =>
            {
                var i2 = Invoice2.FromJObject(jo);
                if (load && i2.ApprovedById != null) i2.ApprovedBy = (User2)(cache && objects.ContainsKey(i2.ApprovedById.Value) ? objects[i2.ApprovedById.Value] : objects[i2.ApprovedById.Value] = FindUser2(i2.ApprovedById));
                if (load && i2.BatchGroupId != null) i2.BatchGroup = (BatchGroup2)(cache && objects.ContainsKey(i2.BatchGroupId.Value) ? objects[i2.BatchGroupId.Value] : objects[i2.BatchGroupId.Value] = FindBatchGroup2(i2.BatchGroupId));
                if (load && i2.BillToId != null) i2.BillTo = (Configuration2)(cache && objects.ContainsKey(i2.BillToId.Value) ? objects[i2.BillToId.Value] : objects[i2.BillToId.Value] = FindConfiguration2(i2.BillToId));
                if (load && i2.PaymentId != null) i2.Payment = (Transaction2)(cache && objects.ContainsKey(i2.PaymentId.Value) ? objects[i2.PaymentId.Value] : objects[i2.PaymentId.Value] = FindTransaction2(i2.PaymentId));
                if (load && i2.VendorId != null) i2.Vendor = (Organization2)(cache && objects.ContainsKey(i2.VendorId.Value) ? objects[i2.VendorId.Value] : objects[i2.VendorId.Value] = FindOrganization2(i2.VendorId));
                if (load && i2.CreationUserId != null) i2.CreationUser = (User2)(cache && objects.ContainsKey(i2.CreationUserId.Value) ? objects[i2.CreationUserId.Value] : objects[i2.CreationUserId.Value] = FindUser2(i2.CreationUserId));
                if (load && i2.LastWriteUserId != null) i2.LastWriteUser = (User2)(cache && objects.ContainsKey(i2.LastWriteUserId.Value) ? objects[i2.LastWriteUserId.Value] : objects[i2.LastWriteUserId.Value] = FindUser2(i2.LastWriteUserId));
                return i2;
            }).ToArray();
        }

        public IEnumerable<Invoice2> Invoice2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Invoices(where, orderBy, skip, take))
            {
                var i2 = Invoice2.FromJObject(jo);
                if (load && i2.ApprovedById != null) i2.ApprovedBy = (User2)(cache && objects.ContainsKey(i2.ApprovedById.Value) ? objects[i2.ApprovedById.Value] : objects[i2.ApprovedById.Value] = FindUser2(i2.ApprovedById));
                if (load && i2.BatchGroupId != null) i2.BatchGroup = (BatchGroup2)(cache && objects.ContainsKey(i2.BatchGroupId.Value) ? objects[i2.BatchGroupId.Value] : objects[i2.BatchGroupId.Value] = FindBatchGroup2(i2.BatchGroupId));
                if (load && i2.BillToId != null) i2.BillTo = (Configuration2)(cache && objects.ContainsKey(i2.BillToId.Value) ? objects[i2.BillToId.Value] : objects[i2.BillToId.Value] = FindConfiguration2(i2.BillToId));
                if (load && i2.PaymentId != null) i2.Payment = (Transaction2)(cache && objects.ContainsKey(i2.PaymentId.Value) ? objects[i2.PaymentId.Value] : objects[i2.PaymentId.Value] = FindTransaction2(i2.PaymentId));
                if (load && i2.VendorId != null) i2.Vendor = (Organization2)(cache && objects.ContainsKey(i2.VendorId.Value) ? objects[i2.VendorId.Value] : objects[i2.VendorId.Value] = FindOrganization2(i2.VendorId));
                if (load && i2.CreationUserId != null) i2.CreationUser = (User2)(cache && objects.ContainsKey(i2.CreationUserId.Value) ? objects[i2.CreationUserId.Value] : objects[i2.CreationUserId.Value] = FindUser2(i2.CreationUserId));
                if (load && i2.LastWriteUserId != null) i2.LastWriteUser = (User2)(cache && objects.ContainsKey(i2.LastWriteUserId.Value) ? objects[i2.LastWriteUserId.Value] : objects[i2.LastWriteUserId.Value] = FindUser2(i2.LastWriteUserId));
                yield return i2;
            }
        }

        public Invoice2 FindInvoice2(Guid? id, bool load = false, bool cache = true)
        {
            var i2 = Invoice2.FromJObject(FolioServiceClient.GetInvoice(id?.ToString()));
            if (i2 == null) return null;
            if (load && i2.ApprovedById != null) i2.ApprovedBy = (User2)(cache && objects.ContainsKey(i2.ApprovedById.Value) ? objects[i2.ApprovedById.Value] : objects[i2.ApprovedById.Value] = FindUser2(i2.ApprovedById));
            if (load && i2.BatchGroupId != null) i2.BatchGroup = (BatchGroup2)(cache && objects.ContainsKey(i2.BatchGroupId.Value) ? objects[i2.BatchGroupId.Value] : objects[i2.BatchGroupId.Value] = FindBatchGroup2(i2.BatchGroupId));
            if (load && i2.BillToId != null) i2.BillTo = (Configuration2)(cache && objects.ContainsKey(i2.BillToId.Value) ? objects[i2.BillToId.Value] : objects[i2.BillToId.Value] = FindConfiguration2(i2.BillToId));
            if (load && i2.PaymentId != null) i2.Payment = (Transaction2)(cache && objects.ContainsKey(i2.PaymentId.Value) ? objects[i2.PaymentId.Value] : objects[i2.PaymentId.Value] = FindTransaction2(i2.PaymentId));
            if (load && i2.VendorId != null) i2.Vendor = (Organization2)(cache && objects.ContainsKey(i2.VendorId.Value) ? objects[i2.VendorId.Value] : objects[i2.VendorId.Value] = FindOrganization2(i2.VendorId));
            if (load && i2.CreationUserId != null) i2.CreationUser = (User2)(cache && objects.ContainsKey(i2.CreationUserId.Value) ? objects[i2.CreationUserId.Value] : objects[i2.CreationUserId.Value] = FindUser2(i2.CreationUserId));
            if (load && i2.LastWriteUserId != null) i2.LastWriteUser = (User2)(cache && objects.ContainsKey(i2.LastWriteUserId.Value) ? objects[i2.LastWriteUserId.Value] : objects[i2.LastWriteUserId.Value] = FindUser2(i2.LastWriteUserId));
            var i = 0;
            if (i2.InvoiceAcquisitionsUnits != null) foreach (var iau in i2.InvoiceAcquisitionsUnits)
                {
                    iau.Id = (++i).ToString();
                    iau.InvoiceId = i2.Id;
                }
            i = 0;
            if (i2.InvoiceAdjustments != null) foreach (var ia in i2.InvoiceAdjustments)
                {
                    ia.Id = (++i).ToString();
                    ia.InvoiceId = i2.Id;
                }
            i = 0;
            if (i2.InvoiceOrderNumbers != null) foreach (var ion in i2.InvoiceOrderNumbers)
                {
                    ion.Id = (++i).ToString();
                    ion.InvoiceId = i2.Id;
                }
            i = 0;
            if (i2.InvoiceTags != null) foreach (var it in i2.InvoiceTags)
                {
                    it.Id = (++i).ToString();
                    it.InvoiceId = i2.Id;
                }
            return i2;
        }

        public void Insert(Invoice2 invoice2)
        {
            if (invoice2.Id == null) invoice2.Id = Guid.NewGuid();
            FolioServiceClient.InsertInvoice(invoice2.ToJObject());
        }

        public void Update(Invoice2 invoice2) => FolioServiceClient.UpdateInvoice(invoice2.ToJObject());

        public void DeleteInvoice2(Guid? id) => FolioServiceClient.DeleteInvoice(id?.ToString());

        public bool AnyInvoiceItem2s(string where = null) => FolioServiceClient.AnyInvoiceItems(where);

        public int CountInvoiceItem2s(string where = null) => FolioServiceClient.CountInvoiceItems(where);

        public InvoiceItem2[] InvoiceItem2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.InvoiceItems(out count, where, orderBy, skip, take).Select(jo =>
            {
                var ii2 = InvoiceItem2.FromJObject(jo);
                if (load && ii2.InvoiceId != null) ii2.Invoice = (Invoice2)(cache && objects.ContainsKey(ii2.InvoiceId.Value) ? objects[ii2.InvoiceId.Value] : objects[ii2.InvoiceId.Value] = FindInvoice2(ii2.InvoiceId));
                if (load && ii2.OrderItemId != null) ii2.OrderItem = (OrderItem2)(cache && objects.ContainsKey(ii2.OrderItemId.Value) ? objects[ii2.OrderItemId.Value] : objects[ii2.OrderItemId.Value] = FindOrderItem2(ii2.OrderItemId));
                if (load && ii2.ProductIdTypeId != null) ii2.ProductIdType = (IdType2)(cache && objects.ContainsKey(ii2.ProductIdTypeId.Value) ? objects[ii2.ProductIdTypeId.Value] : objects[ii2.ProductIdTypeId.Value] = FindIdType2(ii2.ProductIdTypeId));
                if (load && ii2.CreationUserId != null) ii2.CreationUser = (User2)(cache && objects.ContainsKey(ii2.CreationUserId.Value) ? objects[ii2.CreationUserId.Value] : objects[ii2.CreationUserId.Value] = FindUser2(ii2.CreationUserId));
                if (load && ii2.LastWriteUserId != null) ii2.LastWriteUser = (User2)(cache && objects.ContainsKey(ii2.LastWriteUserId.Value) ? objects[ii2.LastWriteUserId.Value] : objects[ii2.LastWriteUserId.Value] = FindUser2(ii2.LastWriteUserId));
                return ii2;
            }).ToArray();
        }

        public IEnumerable<InvoiceItem2> InvoiceItem2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.InvoiceItems(where, orderBy, skip, take))
            {
                var ii2 = InvoiceItem2.FromJObject(jo);
                if (load && ii2.InvoiceId != null) ii2.Invoice = (Invoice2)(cache && objects.ContainsKey(ii2.InvoiceId.Value) ? objects[ii2.InvoiceId.Value] : objects[ii2.InvoiceId.Value] = FindInvoice2(ii2.InvoiceId));
                if (load && ii2.OrderItemId != null) ii2.OrderItem = (OrderItem2)(cache && objects.ContainsKey(ii2.OrderItemId.Value) ? objects[ii2.OrderItemId.Value] : objects[ii2.OrderItemId.Value] = FindOrderItem2(ii2.OrderItemId));
                if (load && ii2.ProductIdTypeId != null) ii2.ProductIdType = (IdType2)(cache && objects.ContainsKey(ii2.ProductIdTypeId.Value) ? objects[ii2.ProductIdTypeId.Value] : objects[ii2.ProductIdTypeId.Value] = FindIdType2(ii2.ProductIdTypeId));
                if (load && ii2.CreationUserId != null) ii2.CreationUser = (User2)(cache && objects.ContainsKey(ii2.CreationUserId.Value) ? objects[ii2.CreationUserId.Value] : objects[ii2.CreationUserId.Value] = FindUser2(ii2.CreationUserId));
                if (load && ii2.LastWriteUserId != null) ii2.LastWriteUser = (User2)(cache && objects.ContainsKey(ii2.LastWriteUserId.Value) ? objects[ii2.LastWriteUserId.Value] : objects[ii2.LastWriteUserId.Value] = FindUser2(ii2.LastWriteUserId));
                yield return ii2;
            }
        }

        public InvoiceItem2 FindInvoiceItem2(Guid? id, bool load = false, bool cache = true)
        {
            var ii2 = InvoiceItem2.FromJObject(FolioServiceClient.GetInvoiceItem(id?.ToString()));
            if (ii2 == null) return null;
            if (load && ii2.InvoiceId != null) ii2.Invoice = (Invoice2)(cache && objects.ContainsKey(ii2.InvoiceId.Value) ? objects[ii2.InvoiceId.Value] : objects[ii2.InvoiceId.Value] = FindInvoice2(ii2.InvoiceId));
            if (load && ii2.OrderItemId != null) ii2.OrderItem = (OrderItem2)(cache && objects.ContainsKey(ii2.OrderItemId.Value) ? objects[ii2.OrderItemId.Value] : objects[ii2.OrderItemId.Value] = FindOrderItem2(ii2.OrderItemId));
            if (load && ii2.ProductIdTypeId != null) ii2.ProductIdType = (IdType2)(cache && objects.ContainsKey(ii2.ProductIdTypeId.Value) ? objects[ii2.ProductIdTypeId.Value] : objects[ii2.ProductIdTypeId.Value] = FindIdType2(ii2.ProductIdTypeId));
            if (load && ii2.CreationUserId != null) ii2.CreationUser = (User2)(cache && objects.ContainsKey(ii2.CreationUserId.Value) ? objects[ii2.CreationUserId.Value] : objects[ii2.CreationUserId.Value] = FindUser2(ii2.CreationUserId));
            if (load && ii2.LastWriteUserId != null) ii2.LastWriteUser = (User2)(cache && objects.ContainsKey(ii2.LastWriteUserId.Value) ? objects[ii2.LastWriteUserId.Value] : objects[ii2.LastWriteUserId.Value] = FindUser2(ii2.LastWriteUserId));
            var i = 0;
            if (ii2.InvoiceItemAdjustmentFunds != null) foreach (var iiaf in ii2.InvoiceItemAdjustmentFunds)
                {
                    iiaf.Id = (++i).ToString();
                    iiaf.InvoiceItemId = ii2.Id;
                }
            i = 0;
            if (ii2.InvoiceItemAdjustments != null) foreach (var iia in ii2.InvoiceItemAdjustments)
                {
                    iia.Id = (++i).ToString();
                    iia.InvoiceItemId = ii2.Id;
                }
            i = 0;
            if (ii2.InvoiceItemFunds != null) foreach (var iif in ii2.InvoiceItemFunds)
                {
                    iif.Id = (++i).ToString();
                    iif.InvoiceItemId = ii2.Id;
                }
            i = 0;
            if (ii2.InvoiceItemReferenceNumbers != null) foreach (var iirn in ii2.InvoiceItemReferenceNumbers)
                {
                    iirn.Id = (++i).ToString();
                    iirn.InvoiceItemId = ii2.Id;
                }
            i = 0;
            if (ii2.InvoiceItemTags != null) foreach (var iit in ii2.InvoiceItemTags)
                {
                    iit.Id = (++i).ToString();
                    iit.InvoiceItemId = ii2.Id;
                }
            return ii2;
        }

        public void Insert(InvoiceItem2 invoiceItem2)
        {
            if (invoiceItem2.Id == null) invoiceItem2.Id = Guid.NewGuid();
            FolioServiceClient.InsertInvoiceItem(invoiceItem2.ToJObject());
        }

        public void Update(InvoiceItem2 invoiceItem2) => FolioServiceClient.UpdateInvoiceItem(invoiceItem2.ToJObject());

        public void DeleteInvoiceItem2(Guid? id) => FolioServiceClient.DeleteInvoiceItem(id?.ToString());

        public InvoiceTransactionSummary2 FindInvoiceTransactionSummary2(Guid? id, bool load = false, bool cache = true)
        {
            var its2 = InvoiceTransactionSummary2.FromJObject(FolioServiceClient.GetInvoiceTransactionSummary(id?.ToString()));
            if (its2 == null) return null;
            if (load && its2.Id != null) its2.Invoice2 = (Invoice2)(cache && objects.ContainsKey(its2.Id.Value) ? objects[its2.Id.Value] : objects[its2.Id.Value] = FindInvoice2(its2.Id));
            return its2;
        }

        public void Insert(InvoiceTransactionSummary2 invoiceTransactionSummary2)
        {
            if (invoiceTransactionSummary2.Id == null) invoiceTransactionSummary2.Id = Guid.NewGuid();
            FolioServiceClient.InsertInvoiceTransactionSummary(invoiceTransactionSummary2.ToJObject());
        }

        public void Update(InvoiceTransactionSummary2 invoiceTransactionSummary2) => FolioServiceClient.UpdateInvoiceTransactionSummary(invoiceTransactionSummary2.ToJObject());

        public void DeleteInvoiceTransactionSummary2(Guid? id) => FolioServiceClient.DeleteInvoiceTransactionSummary(id?.ToString());

        public bool AnyIssuanceModes(string where = null) => FolioServiceClient.AnyModeOfIssuances(where);

        public int CountIssuanceModes(string where = null) => FolioServiceClient.CountModeOfIssuances(where);

        public IssuanceMode[] IssuanceModes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ModeOfIssuances(out count, where, orderBy, skip, take).Select(jo =>
            {
                var im = IssuanceMode.FromJObject(jo);
                if (load && im.CreationUserId != null) im.CreationUser = (User2)(cache && objects.ContainsKey(im.CreationUserId.Value) ? objects[im.CreationUserId.Value] : objects[im.CreationUserId.Value] = FindUser2(im.CreationUserId));
                if (load && im.LastWriteUserId != null) im.LastWriteUser = (User2)(cache && objects.ContainsKey(im.LastWriteUserId.Value) ? objects[im.LastWriteUserId.Value] : objects[im.LastWriteUserId.Value] = FindUser2(im.LastWriteUserId));
                return im;
            }).ToArray();
        }

        public IEnumerable<IssuanceMode> IssuanceModes(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ModeOfIssuances(where, orderBy, skip, take))
            {
                var im = IssuanceMode.FromJObject(jo);
                if (load && im.CreationUserId != null) im.CreationUser = (User2)(cache && objects.ContainsKey(im.CreationUserId.Value) ? objects[im.CreationUserId.Value] : objects[im.CreationUserId.Value] = FindUser2(im.CreationUserId));
                if (load && im.LastWriteUserId != null) im.LastWriteUser = (User2)(cache && objects.ContainsKey(im.LastWriteUserId.Value) ? objects[im.LastWriteUserId.Value] : objects[im.LastWriteUserId.Value] = FindUser2(im.LastWriteUserId));
                yield return im;
            }
        }

        public IssuanceMode FindIssuanceMode(Guid? id, bool load = false, bool cache = true)
        {
            var im = IssuanceMode.FromJObject(FolioServiceClient.GetModeOfIssuance(id?.ToString()));
            if (im == null) return null;
            if (load && im.CreationUserId != null) im.CreationUser = (User2)(cache && objects.ContainsKey(im.CreationUserId.Value) ? objects[im.CreationUserId.Value] : objects[im.CreationUserId.Value] = FindUser2(im.CreationUserId));
            if (load && im.LastWriteUserId != null) im.LastWriteUser = (User2)(cache && objects.ContainsKey(im.LastWriteUserId.Value) ? objects[im.LastWriteUserId.Value] : objects[im.LastWriteUserId.Value] = FindUser2(im.LastWriteUserId));
            return im;
        }

        public void Insert(IssuanceMode issuanceMode)
        {
            if (issuanceMode.Id == null) issuanceMode.Id = Guid.NewGuid();
            FolioServiceClient.InsertModeOfIssuance(issuanceMode.ToJObject());
        }

        public void Update(IssuanceMode issuanceMode) => FolioServiceClient.UpdateModeOfIssuance(issuanceMode.ToJObject());

        public void DeleteIssuanceMode(Guid? id) => FolioServiceClient.DeleteModeOfIssuance(id?.ToString());

        public bool AnyItem2s(string where = null) => FolioServiceClient.AnyItems(where);

        public int CountItem2s(string where = null) => FolioServiceClient.CountItems(where);

        public Item2[] Item2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Items(out count, where, orderBy, skip, take).Select(jo =>
            {
                var i2 = Item2.FromJObject(jo);
                if (load && i2.HoldingId != null) i2.Holding = (Holding2)(cache && objects.ContainsKey(i2.HoldingId.Value) ? objects[i2.HoldingId.Value] : objects[i2.HoldingId.Value] = FindHolding2(i2.HoldingId));
                if (load && i2.CallNumberTypeId != null) i2.CallNumberType = (CallNumberType2)(cache && objects.ContainsKey(i2.CallNumberTypeId.Value) ? objects[i2.CallNumberTypeId.Value] : objects[i2.CallNumberTypeId.Value] = FindCallNumberType2(i2.CallNumberTypeId));
                if (load && i2.EffectiveCallNumberTypeId != null) i2.EffectiveCallNumberType = (CallNumberType2)(cache && objects.ContainsKey(i2.EffectiveCallNumberTypeId.Value) ? objects[i2.EffectiveCallNumberTypeId.Value] : objects[i2.EffectiveCallNumberTypeId.Value] = FindCallNumberType2(i2.EffectiveCallNumberTypeId));
                if (load && i2.DamagedStatusId != null) i2.DamagedStatus = (ItemDamagedStatus2)(cache && objects.ContainsKey(i2.DamagedStatusId.Value) ? objects[i2.DamagedStatusId.Value] : objects[i2.DamagedStatusId.Value] = FindItemDamagedStatus2(i2.DamagedStatusId));
                if (load && i2.MaterialTypeId != null) i2.MaterialType = (MaterialType2)(cache && objects.ContainsKey(i2.MaterialTypeId.Value) ? objects[i2.MaterialTypeId.Value] : objects[i2.MaterialTypeId.Value] = FindMaterialType2(i2.MaterialTypeId));
                if (load && i2.PermanentLoanTypeId != null) i2.PermanentLoanType = (LoanType2)(cache && objects.ContainsKey(i2.PermanentLoanTypeId.Value) ? objects[i2.PermanentLoanTypeId.Value] : objects[i2.PermanentLoanTypeId.Value] = FindLoanType2(i2.PermanentLoanTypeId));
                if (load && i2.TemporaryLoanTypeId != null) i2.TemporaryLoanType = (LoanType2)(cache && objects.ContainsKey(i2.TemporaryLoanTypeId.Value) ? objects[i2.TemporaryLoanTypeId.Value] : objects[i2.TemporaryLoanTypeId.Value] = FindLoanType2(i2.TemporaryLoanTypeId));
                if (load && i2.PermanentLocationId != null) i2.PermanentLocation = (Location2)(cache && objects.ContainsKey(i2.PermanentLocationId.Value) ? objects[i2.PermanentLocationId.Value] : objects[i2.PermanentLocationId.Value] = FindLocation2(i2.PermanentLocationId));
                if (load && i2.TemporaryLocationId != null) i2.TemporaryLocation = (Location2)(cache && objects.ContainsKey(i2.TemporaryLocationId.Value) ? objects[i2.TemporaryLocationId.Value] : objects[i2.TemporaryLocationId.Value] = FindLocation2(i2.TemporaryLocationId));
                if (load && i2.EffectiveLocationId != null) i2.EffectiveLocation = (Location2)(cache && objects.ContainsKey(i2.EffectiveLocationId.Value) ? objects[i2.EffectiveLocationId.Value] : objects[i2.EffectiveLocationId.Value] = FindLocation2(i2.EffectiveLocationId));
                if (load && i2.InTransitDestinationServicePointId != null) i2.InTransitDestinationServicePoint = (ServicePoint2)(cache && objects.ContainsKey(i2.InTransitDestinationServicePointId.Value) ? objects[i2.InTransitDestinationServicePointId.Value] : objects[i2.InTransitDestinationServicePointId.Value] = FindServicePoint2(i2.InTransitDestinationServicePointId));
                if (load && i2.OrderItemId != null) i2.OrderItem = (OrderItem2)(cache && objects.ContainsKey(i2.OrderItemId.Value) ? objects[i2.OrderItemId.Value] : objects[i2.OrderItemId.Value] = FindOrderItem2(i2.OrderItemId));
                if (load && i2.CreationUserId != null) i2.CreationUser = (User2)(cache && objects.ContainsKey(i2.CreationUserId.Value) ? objects[i2.CreationUserId.Value] : objects[i2.CreationUserId.Value] = FindUser2(i2.CreationUserId));
                if (load && i2.LastWriteUserId != null) i2.LastWriteUser = (User2)(cache && objects.ContainsKey(i2.LastWriteUserId.Value) ? objects[i2.LastWriteUserId.Value] : objects[i2.LastWriteUserId.Value] = FindUser2(i2.LastWriteUserId));
                if (load && i2.LastCheckInServicePointId != null) i2.LastCheckInServicePoint = (ServicePoint2)(cache && objects.ContainsKey(i2.LastCheckInServicePointId.Value) ? objects[i2.LastCheckInServicePointId.Value] : objects[i2.LastCheckInServicePointId.Value] = FindServicePoint2(i2.LastCheckInServicePointId));
                if (load && i2.LastCheckInStaffMemberId != null) i2.LastCheckInStaffMember = (User2)(cache && objects.ContainsKey(i2.LastCheckInStaffMemberId.Value) ? objects[i2.LastCheckInStaffMemberId.Value] : objects[i2.LastCheckInStaffMemberId.Value] = FindUser2(i2.LastCheckInStaffMemberId));
                return i2;
            }).ToArray();
        }

        public IEnumerable<Item2> Item2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Items(where, orderBy, skip, take))
            {
                var i2 = Item2.FromJObject(jo);
                if (load && i2.HoldingId != null) i2.Holding = (Holding2)(cache && objects.ContainsKey(i2.HoldingId.Value) ? objects[i2.HoldingId.Value] : objects[i2.HoldingId.Value] = FindHolding2(i2.HoldingId));
                if (load && i2.CallNumberTypeId != null) i2.CallNumberType = (CallNumberType2)(cache && objects.ContainsKey(i2.CallNumberTypeId.Value) ? objects[i2.CallNumberTypeId.Value] : objects[i2.CallNumberTypeId.Value] = FindCallNumberType2(i2.CallNumberTypeId));
                if (load && i2.EffectiveCallNumberTypeId != null) i2.EffectiveCallNumberType = (CallNumberType2)(cache && objects.ContainsKey(i2.EffectiveCallNumberTypeId.Value) ? objects[i2.EffectiveCallNumberTypeId.Value] : objects[i2.EffectiveCallNumberTypeId.Value] = FindCallNumberType2(i2.EffectiveCallNumberTypeId));
                if (load && i2.DamagedStatusId != null) i2.DamagedStatus = (ItemDamagedStatus2)(cache && objects.ContainsKey(i2.DamagedStatusId.Value) ? objects[i2.DamagedStatusId.Value] : objects[i2.DamagedStatusId.Value] = FindItemDamagedStatus2(i2.DamagedStatusId));
                if (load && i2.MaterialTypeId != null) i2.MaterialType = (MaterialType2)(cache && objects.ContainsKey(i2.MaterialTypeId.Value) ? objects[i2.MaterialTypeId.Value] : objects[i2.MaterialTypeId.Value] = FindMaterialType2(i2.MaterialTypeId));
                if (load && i2.PermanentLoanTypeId != null) i2.PermanentLoanType = (LoanType2)(cache && objects.ContainsKey(i2.PermanentLoanTypeId.Value) ? objects[i2.PermanentLoanTypeId.Value] : objects[i2.PermanentLoanTypeId.Value] = FindLoanType2(i2.PermanentLoanTypeId));
                if (load && i2.TemporaryLoanTypeId != null) i2.TemporaryLoanType = (LoanType2)(cache && objects.ContainsKey(i2.TemporaryLoanTypeId.Value) ? objects[i2.TemporaryLoanTypeId.Value] : objects[i2.TemporaryLoanTypeId.Value] = FindLoanType2(i2.TemporaryLoanTypeId));
                if (load && i2.PermanentLocationId != null) i2.PermanentLocation = (Location2)(cache && objects.ContainsKey(i2.PermanentLocationId.Value) ? objects[i2.PermanentLocationId.Value] : objects[i2.PermanentLocationId.Value] = FindLocation2(i2.PermanentLocationId));
                if (load && i2.TemporaryLocationId != null) i2.TemporaryLocation = (Location2)(cache && objects.ContainsKey(i2.TemporaryLocationId.Value) ? objects[i2.TemporaryLocationId.Value] : objects[i2.TemporaryLocationId.Value] = FindLocation2(i2.TemporaryLocationId));
                if (load && i2.EffectiveLocationId != null) i2.EffectiveLocation = (Location2)(cache && objects.ContainsKey(i2.EffectiveLocationId.Value) ? objects[i2.EffectiveLocationId.Value] : objects[i2.EffectiveLocationId.Value] = FindLocation2(i2.EffectiveLocationId));
                if (load && i2.InTransitDestinationServicePointId != null) i2.InTransitDestinationServicePoint = (ServicePoint2)(cache && objects.ContainsKey(i2.InTransitDestinationServicePointId.Value) ? objects[i2.InTransitDestinationServicePointId.Value] : objects[i2.InTransitDestinationServicePointId.Value] = FindServicePoint2(i2.InTransitDestinationServicePointId));
                if (load && i2.OrderItemId != null) i2.OrderItem = (OrderItem2)(cache && objects.ContainsKey(i2.OrderItemId.Value) ? objects[i2.OrderItemId.Value] : objects[i2.OrderItemId.Value] = FindOrderItem2(i2.OrderItemId));
                if (load && i2.CreationUserId != null) i2.CreationUser = (User2)(cache && objects.ContainsKey(i2.CreationUserId.Value) ? objects[i2.CreationUserId.Value] : objects[i2.CreationUserId.Value] = FindUser2(i2.CreationUserId));
                if (load && i2.LastWriteUserId != null) i2.LastWriteUser = (User2)(cache && objects.ContainsKey(i2.LastWriteUserId.Value) ? objects[i2.LastWriteUserId.Value] : objects[i2.LastWriteUserId.Value] = FindUser2(i2.LastWriteUserId));
                if (load && i2.LastCheckInServicePointId != null) i2.LastCheckInServicePoint = (ServicePoint2)(cache && objects.ContainsKey(i2.LastCheckInServicePointId.Value) ? objects[i2.LastCheckInServicePointId.Value] : objects[i2.LastCheckInServicePointId.Value] = FindServicePoint2(i2.LastCheckInServicePointId));
                if (load && i2.LastCheckInStaffMemberId != null) i2.LastCheckInStaffMember = (User2)(cache && objects.ContainsKey(i2.LastCheckInStaffMemberId.Value) ? objects[i2.LastCheckInStaffMemberId.Value] : objects[i2.LastCheckInStaffMemberId.Value] = FindUser2(i2.LastCheckInStaffMemberId));
                yield return i2;
            }
        }

        public Item2 FindItem2(Guid? id, bool load = false, bool cache = true)
        {
            var i2 = Item2.FromJObject(FolioServiceClient.GetItem(id?.ToString()));
            if (i2 == null) return null;
            if (load && i2.HoldingId != null) i2.Holding = (Holding2)(cache && objects.ContainsKey(i2.HoldingId.Value) ? objects[i2.HoldingId.Value] : objects[i2.HoldingId.Value] = FindHolding2(i2.HoldingId));
            if (load && i2.CallNumberTypeId != null) i2.CallNumberType = (CallNumberType2)(cache && objects.ContainsKey(i2.CallNumberTypeId.Value) ? objects[i2.CallNumberTypeId.Value] : objects[i2.CallNumberTypeId.Value] = FindCallNumberType2(i2.CallNumberTypeId));
            if (load && i2.EffectiveCallNumberTypeId != null) i2.EffectiveCallNumberType = (CallNumberType2)(cache && objects.ContainsKey(i2.EffectiveCallNumberTypeId.Value) ? objects[i2.EffectiveCallNumberTypeId.Value] : objects[i2.EffectiveCallNumberTypeId.Value] = FindCallNumberType2(i2.EffectiveCallNumberTypeId));
            if (load && i2.DamagedStatusId != null) i2.DamagedStatus = (ItemDamagedStatus2)(cache && objects.ContainsKey(i2.DamagedStatusId.Value) ? objects[i2.DamagedStatusId.Value] : objects[i2.DamagedStatusId.Value] = FindItemDamagedStatus2(i2.DamagedStatusId));
            if (load && i2.MaterialTypeId != null) i2.MaterialType = (MaterialType2)(cache && objects.ContainsKey(i2.MaterialTypeId.Value) ? objects[i2.MaterialTypeId.Value] : objects[i2.MaterialTypeId.Value] = FindMaterialType2(i2.MaterialTypeId));
            if (load && i2.PermanentLoanTypeId != null) i2.PermanentLoanType = (LoanType2)(cache && objects.ContainsKey(i2.PermanentLoanTypeId.Value) ? objects[i2.PermanentLoanTypeId.Value] : objects[i2.PermanentLoanTypeId.Value] = FindLoanType2(i2.PermanentLoanTypeId));
            if (load && i2.TemporaryLoanTypeId != null) i2.TemporaryLoanType = (LoanType2)(cache && objects.ContainsKey(i2.TemporaryLoanTypeId.Value) ? objects[i2.TemporaryLoanTypeId.Value] : objects[i2.TemporaryLoanTypeId.Value] = FindLoanType2(i2.TemporaryLoanTypeId));
            if (load && i2.PermanentLocationId != null) i2.PermanentLocation = (Location2)(cache && objects.ContainsKey(i2.PermanentLocationId.Value) ? objects[i2.PermanentLocationId.Value] : objects[i2.PermanentLocationId.Value] = FindLocation2(i2.PermanentLocationId));
            if (load && i2.TemporaryLocationId != null) i2.TemporaryLocation = (Location2)(cache && objects.ContainsKey(i2.TemporaryLocationId.Value) ? objects[i2.TemporaryLocationId.Value] : objects[i2.TemporaryLocationId.Value] = FindLocation2(i2.TemporaryLocationId));
            if (load && i2.EffectiveLocationId != null) i2.EffectiveLocation = (Location2)(cache && objects.ContainsKey(i2.EffectiveLocationId.Value) ? objects[i2.EffectiveLocationId.Value] : objects[i2.EffectiveLocationId.Value] = FindLocation2(i2.EffectiveLocationId));
            if (load && i2.InTransitDestinationServicePointId != null) i2.InTransitDestinationServicePoint = (ServicePoint2)(cache && objects.ContainsKey(i2.InTransitDestinationServicePointId.Value) ? objects[i2.InTransitDestinationServicePointId.Value] : objects[i2.InTransitDestinationServicePointId.Value] = FindServicePoint2(i2.InTransitDestinationServicePointId));
            if (load && i2.OrderItemId != null) i2.OrderItem = (OrderItem2)(cache && objects.ContainsKey(i2.OrderItemId.Value) ? objects[i2.OrderItemId.Value] : objects[i2.OrderItemId.Value] = FindOrderItem2(i2.OrderItemId));
            if (load && i2.CreationUserId != null) i2.CreationUser = (User2)(cache && objects.ContainsKey(i2.CreationUserId.Value) ? objects[i2.CreationUserId.Value] : objects[i2.CreationUserId.Value] = FindUser2(i2.CreationUserId));
            if (load && i2.LastWriteUserId != null) i2.LastWriteUser = (User2)(cache && objects.ContainsKey(i2.LastWriteUserId.Value) ? objects[i2.LastWriteUserId.Value] : objects[i2.LastWriteUserId.Value] = FindUser2(i2.LastWriteUserId));
            if (load && i2.LastCheckInServicePointId != null) i2.LastCheckInServicePoint = (ServicePoint2)(cache && objects.ContainsKey(i2.LastCheckInServicePointId.Value) ? objects[i2.LastCheckInServicePointId.Value] : objects[i2.LastCheckInServicePointId.Value] = FindServicePoint2(i2.LastCheckInServicePointId));
            if (load && i2.LastCheckInStaffMemberId != null) i2.LastCheckInStaffMember = (User2)(cache && objects.ContainsKey(i2.LastCheckInStaffMemberId.Value) ? objects[i2.LastCheckInStaffMemberId.Value] : objects[i2.LastCheckInStaffMemberId.Value] = FindUser2(i2.LastCheckInStaffMemberId));
            var i = 0;
            if (i2.CirculationNotes != null) foreach (var cn in i2.CirculationNotes)
                {
                    cn.Id = (++i).ToString();
                    cn.ItemId = i2.Id;
                }
            i = 0;
            if (i2.ItemElectronicAccesses != null) foreach (var iea in i2.ItemElectronicAccesses)
                {
                    iea.Id = (++i).ToString();
                    iea.ItemId = i2.Id;
                }
            i = 0;
            if (i2.ItemFormerIds != null) foreach (var ifi in i2.ItemFormerIds)
                {
                    ifi.Id = (++i).ToString();
                    ifi.ItemId = i2.Id;
                }
            i = 0;
            if (i2.ItemNotes != null) foreach (var @in in i2.ItemNotes)
                {
                    @in.Id = (++i).ToString();
                    @in.ItemId = i2.Id;
                }
            i = 0;
            if (i2.ItemStatisticalCodes != null) foreach (var isc in i2.ItemStatisticalCodes)
                {
                    isc.Id = (++i).ToString();
                    isc.ItemId = i2.Id;
                }
            i = 0;
            if (i2.ItemTags != null) foreach (var it in i2.ItemTags)
                {
                    it.Id = (++i).ToString();
                    it.ItemId = i2.Id;
                }
            i = 0;
            if (i2.ItemYearCaptions != null) foreach (var iyc in i2.ItemYearCaptions)
                {
                    iyc.Id = (++i).ToString();
                    iyc.ItemId = i2.Id;
                }
            return i2;
        }

        public void Insert(Item2 item2)
        {
            if (item2.Id == null) item2.Id = Guid.NewGuid();
            FolioServiceClient.InsertItem(item2.ToJObject());
        }

        public void Update(Item2 item2) => FolioServiceClient.UpdateItem(item2.ToJObject());

        public void DeleteItem2(Guid? id) => FolioServiceClient.DeleteItem(id?.ToString());

        public bool AnyItemDamagedStatus2s(string where = null) => FolioServiceClient.AnyItemDamagedStatuses(where);

        public int CountItemDamagedStatus2s(string where = null) => FolioServiceClient.CountItemDamagedStatuses(where);

        public ItemDamagedStatus2[] ItemDamagedStatus2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ItemDamagedStatuses(out count, where, orderBy, skip, take).Select(jo =>
            {
                var ids2 = ItemDamagedStatus2.FromJObject(jo);
                if (load && ids2.CreationUserId != null) ids2.CreationUser = (User2)(cache && objects.ContainsKey(ids2.CreationUserId.Value) ? objects[ids2.CreationUserId.Value] : objects[ids2.CreationUserId.Value] = FindUser2(ids2.CreationUserId));
                if (load && ids2.LastWriteUserId != null) ids2.LastWriteUser = (User2)(cache && objects.ContainsKey(ids2.LastWriteUserId.Value) ? objects[ids2.LastWriteUserId.Value] : objects[ids2.LastWriteUserId.Value] = FindUser2(ids2.LastWriteUserId));
                return ids2;
            }).ToArray();
        }

        public IEnumerable<ItemDamagedStatus2> ItemDamagedStatus2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ItemDamagedStatuses(where, orderBy, skip, take))
            {
                var ids2 = ItemDamagedStatus2.FromJObject(jo);
                if (load && ids2.CreationUserId != null) ids2.CreationUser = (User2)(cache && objects.ContainsKey(ids2.CreationUserId.Value) ? objects[ids2.CreationUserId.Value] : objects[ids2.CreationUserId.Value] = FindUser2(ids2.CreationUserId));
                if (load && ids2.LastWriteUserId != null) ids2.LastWriteUser = (User2)(cache && objects.ContainsKey(ids2.LastWriteUserId.Value) ? objects[ids2.LastWriteUserId.Value] : objects[ids2.LastWriteUserId.Value] = FindUser2(ids2.LastWriteUserId));
                yield return ids2;
            }
        }

        public ItemDamagedStatus2 FindItemDamagedStatus2(Guid? id, bool load = false, bool cache = true)
        {
            var ids2 = ItemDamagedStatus2.FromJObject(FolioServiceClient.GetItemDamagedStatus(id?.ToString()));
            if (ids2 == null) return null;
            if (load && ids2.CreationUserId != null) ids2.CreationUser = (User2)(cache && objects.ContainsKey(ids2.CreationUserId.Value) ? objects[ids2.CreationUserId.Value] : objects[ids2.CreationUserId.Value] = FindUser2(ids2.CreationUserId));
            if (load && ids2.LastWriteUserId != null) ids2.LastWriteUser = (User2)(cache && objects.ContainsKey(ids2.LastWriteUserId.Value) ? objects[ids2.LastWriteUserId.Value] : objects[ids2.LastWriteUserId.Value] = FindUser2(ids2.LastWriteUserId));
            return ids2;
        }

        public void Insert(ItemDamagedStatus2 itemDamagedStatus2)
        {
            if (itemDamagedStatus2.Id == null) itemDamagedStatus2.Id = Guid.NewGuid();
            FolioServiceClient.InsertItemDamagedStatus(itemDamagedStatus2.ToJObject());
        }

        public void Update(ItemDamagedStatus2 itemDamagedStatus2) => FolioServiceClient.UpdateItemDamagedStatus(itemDamagedStatus2.ToJObject());

        public void DeleteItemDamagedStatus2(Guid? id) => FolioServiceClient.DeleteItemDamagedStatus(id?.ToString());

        public bool AnyItemNoteType2s(string where = null) => FolioServiceClient.AnyItemNoteTypes(where);

        public int CountItemNoteType2s(string where = null) => FolioServiceClient.CountItemNoteTypes(where);

        public ItemNoteType2[] ItemNoteType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ItemNoteTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var int2 = ItemNoteType2.FromJObject(jo);
                if (load && int2.CreationUserId != null) int2.CreationUser = (User2)(cache && objects.ContainsKey(int2.CreationUserId.Value) ? objects[int2.CreationUserId.Value] : objects[int2.CreationUserId.Value] = FindUser2(int2.CreationUserId));
                if (load && int2.LastWriteUserId != null) int2.LastWriteUser = (User2)(cache && objects.ContainsKey(int2.LastWriteUserId.Value) ? objects[int2.LastWriteUserId.Value] : objects[int2.LastWriteUserId.Value] = FindUser2(int2.LastWriteUserId));
                return int2;
            }).ToArray();
        }

        public IEnumerable<ItemNoteType2> ItemNoteType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ItemNoteTypes(where, orderBy, skip, take))
            {
                var int2 = ItemNoteType2.FromJObject(jo);
                if (load && int2.CreationUserId != null) int2.CreationUser = (User2)(cache && objects.ContainsKey(int2.CreationUserId.Value) ? objects[int2.CreationUserId.Value] : objects[int2.CreationUserId.Value] = FindUser2(int2.CreationUserId));
                if (load && int2.LastWriteUserId != null) int2.LastWriteUser = (User2)(cache && objects.ContainsKey(int2.LastWriteUserId.Value) ? objects[int2.LastWriteUserId.Value] : objects[int2.LastWriteUserId.Value] = FindUser2(int2.LastWriteUserId));
                yield return int2;
            }
        }

        public ItemNoteType2 FindItemNoteType2(Guid? id, bool load = false, bool cache = true)
        {
            var int2 = ItemNoteType2.FromJObject(FolioServiceClient.GetItemNoteType(id?.ToString()));
            if (int2 == null) return null;
            if (load && int2.CreationUserId != null) int2.CreationUser = (User2)(cache && objects.ContainsKey(int2.CreationUserId.Value) ? objects[int2.CreationUserId.Value] : objects[int2.CreationUserId.Value] = FindUser2(int2.CreationUserId));
            if (load && int2.LastWriteUserId != null) int2.LastWriteUser = (User2)(cache && objects.ContainsKey(int2.LastWriteUserId.Value) ? objects[int2.LastWriteUserId.Value] : objects[int2.LastWriteUserId.Value] = FindUser2(int2.LastWriteUserId));
            return int2;
        }

        public void Insert(ItemNoteType2 itemNoteType2)
        {
            if (itemNoteType2.Id == null) itemNoteType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertItemNoteType(itemNoteType2.ToJObject());
        }

        public void Update(ItemNoteType2 itemNoteType2) => FolioServiceClient.UpdateItemNoteType(itemNoteType2.ToJObject());

        public void DeleteItemNoteType2(Guid? id) => FolioServiceClient.DeleteItemNoteType(id?.ToString());

        public bool AnyLedger2s(string where = null) => FolioServiceClient.AnyLedgers(where);

        public int CountLedger2s(string where = null) => FolioServiceClient.CountLedgers(where);

        public Ledger2[] Ledger2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Ledgers(out count, where, orderBy, skip, take).Select(jo =>
            {
                var l2 = Ledger2.FromJObject(jo);
                if (load && l2.FiscalYearOneId != null) l2.FiscalYearOne = (FiscalYear2)(cache && objects.ContainsKey(l2.FiscalYearOneId.Value) ? objects[l2.FiscalYearOneId.Value] : objects[l2.FiscalYearOneId.Value] = FindFiscalYear2(l2.FiscalYearOneId));
                if (load && l2.CreationUserId != null) l2.CreationUser = (User2)(cache && objects.ContainsKey(l2.CreationUserId.Value) ? objects[l2.CreationUserId.Value] : objects[l2.CreationUserId.Value] = FindUser2(l2.CreationUserId));
                if (load && l2.LastWriteUserId != null) l2.LastWriteUser = (User2)(cache && objects.ContainsKey(l2.LastWriteUserId.Value) ? objects[l2.LastWriteUserId.Value] : objects[l2.LastWriteUserId.Value] = FindUser2(l2.LastWriteUserId));
                return l2;
            }).ToArray();
        }

        public IEnumerable<Ledger2> Ledger2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Ledgers(where, orderBy, skip, take))
            {
                var l2 = Ledger2.FromJObject(jo);
                if (load && l2.FiscalYearOneId != null) l2.FiscalYearOne = (FiscalYear2)(cache && objects.ContainsKey(l2.FiscalYearOneId.Value) ? objects[l2.FiscalYearOneId.Value] : objects[l2.FiscalYearOneId.Value] = FindFiscalYear2(l2.FiscalYearOneId));
                if (load && l2.CreationUserId != null) l2.CreationUser = (User2)(cache && objects.ContainsKey(l2.CreationUserId.Value) ? objects[l2.CreationUserId.Value] : objects[l2.CreationUserId.Value] = FindUser2(l2.CreationUserId));
                if (load && l2.LastWriteUserId != null) l2.LastWriteUser = (User2)(cache && objects.ContainsKey(l2.LastWriteUserId.Value) ? objects[l2.LastWriteUserId.Value] : objects[l2.LastWriteUserId.Value] = FindUser2(l2.LastWriteUserId));
                yield return l2;
            }
        }

        public Ledger2 FindLedger2(Guid? id, bool load = false, bool cache = true)
        {
            var l2 = Ledger2.FromJObject(FolioServiceClient.GetLedger(id?.ToString()));
            if (l2 == null) return null;
            if (load && l2.FiscalYearOneId != null) l2.FiscalYearOne = (FiscalYear2)(cache && objects.ContainsKey(l2.FiscalYearOneId.Value) ? objects[l2.FiscalYearOneId.Value] : objects[l2.FiscalYearOneId.Value] = FindFiscalYear2(l2.FiscalYearOneId));
            if (load && l2.CreationUserId != null) l2.CreationUser = (User2)(cache && objects.ContainsKey(l2.CreationUserId.Value) ? objects[l2.CreationUserId.Value] : objects[l2.CreationUserId.Value] = FindUser2(l2.CreationUserId));
            if (load && l2.LastWriteUserId != null) l2.LastWriteUser = (User2)(cache && objects.ContainsKey(l2.LastWriteUserId.Value) ? objects[l2.LastWriteUserId.Value] : objects[l2.LastWriteUserId.Value] = FindUser2(l2.LastWriteUserId));
            var i = 0;
            if (l2.LedgerAcquisitionsUnits != null) foreach (var lau in l2.LedgerAcquisitionsUnits)
                {
                    lau.Id = (++i).ToString();
                    lau.LedgerId = l2.Id;
                }
            return l2;
        }

        public void Insert(Ledger2 ledger2)
        {
            if (ledger2.Id == null) ledger2.Id = Guid.NewGuid();
            FolioServiceClient.InsertLedger(ledger2.ToJObject());
        }

        public void Update(Ledger2 ledger2) => FolioServiceClient.UpdateLedger(ledger2.ToJObject());

        public void DeleteLedger2(Guid? id) => FolioServiceClient.DeleteLedger(id?.ToString());

        public bool AnyLedgerRollover2s(string where = null) => FolioServiceClient.AnyLedgerRollovers(where);

        public int CountLedgerRollover2s(string where = null) => FolioServiceClient.CountLedgerRollovers(where);

        public LedgerRollover2[] LedgerRollover2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.LedgerRollovers(out count, where, orderBy, skip, take).Select(jo =>
            {
                var lr2 = LedgerRollover2.FromJObject(jo);
                if (load && lr2.LedgerId != null) lr2.Ledger = (Ledger2)(cache && objects.ContainsKey(lr2.LedgerId.Value) ? objects[lr2.LedgerId.Value] : objects[lr2.LedgerId.Value] = FindLedger2(lr2.LedgerId));
                if (load && lr2.FromFiscalYearId != null) lr2.FromFiscalYear = (FiscalYear2)(cache && objects.ContainsKey(lr2.FromFiscalYearId.Value) ? objects[lr2.FromFiscalYearId.Value] : objects[lr2.FromFiscalYearId.Value] = FindFiscalYear2(lr2.FromFiscalYearId));
                if (load && lr2.ToFiscalYearId != null) lr2.ToFiscalYear = (FiscalYear2)(cache && objects.ContainsKey(lr2.ToFiscalYearId.Value) ? objects[lr2.ToFiscalYearId.Value] : objects[lr2.ToFiscalYearId.Value] = FindFiscalYear2(lr2.ToFiscalYearId));
                if (load && lr2.CreationUserId != null) lr2.CreationUser = (User2)(cache && objects.ContainsKey(lr2.CreationUserId.Value) ? objects[lr2.CreationUserId.Value] : objects[lr2.CreationUserId.Value] = FindUser2(lr2.CreationUserId));
                if (load && lr2.LastWriteUserId != null) lr2.LastWriteUser = (User2)(cache && objects.ContainsKey(lr2.LastWriteUserId.Value) ? objects[lr2.LastWriteUserId.Value] : objects[lr2.LastWriteUserId.Value] = FindUser2(lr2.LastWriteUserId));
                return lr2;
            }).ToArray();
        }

        public IEnumerable<LedgerRollover2> LedgerRollover2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.LedgerRollovers(where, orderBy, skip, take))
            {
                var lr2 = LedgerRollover2.FromJObject(jo);
                if (load && lr2.LedgerId != null) lr2.Ledger = (Ledger2)(cache && objects.ContainsKey(lr2.LedgerId.Value) ? objects[lr2.LedgerId.Value] : objects[lr2.LedgerId.Value] = FindLedger2(lr2.LedgerId));
                if (load && lr2.FromFiscalYearId != null) lr2.FromFiscalYear = (FiscalYear2)(cache && objects.ContainsKey(lr2.FromFiscalYearId.Value) ? objects[lr2.FromFiscalYearId.Value] : objects[lr2.FromFiscalYearId.Value] = FindFiscalYear2(lr2.FromFiscalYearId));
                if (load && lr2.ToFiscalYearId != null) lr2.ToFiscalYear = (FiscalYear2)(cache && objects.ContainsKey(lr2.ToFiscalYearId.Value) ? objects[lr2.ToFiscalYearId.Value] : objects[lr2.ToFiscalYearId.Value] = FindFiscalYear2(lr2.ToFiscalYearId));
                if (load && lr2.CreationUserId != null) lr2.CreationUser = (User2)(cache && objects.ContainsKey(lr2.CreationUserId.Value) ? objects[lr2.CreationUserId.Value] : objects[lr2.CreationUserId.Value] = FindUser2(lr2.CreationUserId));
                if (load && lr2.LastWriteUserId != null) lr2.LastWriteUser = (User2)(cache && objects.ContainsKey(lr2.LastWriteUserId.Value) ? objects[lr2.LastWriteUserId.Value] : objects[lr2.LastWriteUserId.Value] = FindUser2(lr2.LastWriteUserId));
                yield return lr2;
            }
        }

        public LedgerRollover2 FindLedgerRollover2(Guid? id, bool load = false, bool cache = true)
        {
            var lr2 = LedgerRollover2.FromJObject(FolioServiceClient.GetLedgerRollover(id?.ToString()));
            if (lr2 == null) return null;
            if (load && lr2.LedgerId != null) lr2.Ledger = (Ledger2)(cache && objects.ContainsKey(lr2.LedgerId.Value) ? objects[lr2.LedgerId.Value] : objects[lr2.LedgerId.Value] = FindLedger2(lr2.LedgerId));
            if (load && lr2.FromFiscalYearId != null) lr2.FromFiscalYear = (FiscalYear2)(cache && objects.ContainsKey(lr2.FromFiscalYearId.Value) ? objects[lr2.FromFiscalYearId.Value] : objects[lr2.FromFiscalYearId.Value] = FindFiscalYear2(lr2.FromFiscalYearId));
            if (load && lr2.ToFiscalYearId != null) lr2.ToFiscalYear = (FiscalYear2)(cache && objects.ContainsKey(lr2.ToFiscalYearId.Value) ? objects[lr2.ToFiscalYearId.Value] : objects[lr2.ToFiscalYearId.Value] = FindFiscalYear2(lr2.ToFiscalYearId));
            if (load && lr2.CreationUserId != null) lr2.CreationUser = (User2)(cache && objects.ContainsKey(lr2.CreationUserId.Value) ? objects[lr2.CreationUserId.Value] : objects[lr2.CreationUserId.Value] = FindUser2(lr2.CreationUserId));
            if (load && lr2.LastWriteUserId != null) lr2.LastWriteUser = (User2)(cache && objects.ContainsKey(lr2.LastWriteUserId.Value) ? objects[lr2.LastWriteUserId.Value] : objects[lr2.LastWriteUserId.Value] = FindUser2(lr2.LastWriteUserId));
            var i = 0;
            if (lr2.LedgerRolloverBudgetsRollovers != null) foreach (var lrbr in lr2.LedgerRolloverBudgetsRollovers)
                {
                    lrbr.Id = (++i).ToString();
                    lrbr.LedgerRolloverId = lr2.Id;
                }
            i = 0;
            if (lr2.LedgerRolloverEncumbrancesRollovers != null) foreach (var lrer in lr2.LedgerRolloverEncumbrancesRollovers)
                {
                    lrer.Id = (++i).ToString();
                    lrer.LedgerRolloverId = lr2.Id;
                }
            return lr2;
        }

        public void Insert(LedgerRollover2 ledgerRollover2)
        {
            if (ledgerRollover2.Id == null) ledgerRollover2.Id = Guid.NewGuid();
            FolioServiceClient.InsertLedgerRollover(ledgerRollover2.ToJObject());
        }

        public void Update(LedgerRollover2 ledgerRollover2) => FolioServiceClient.UpdateLedgerRollover(ledgerRollover2.ToJObject());

        public void DeleteLedgerRollover2(Guid? id) => FolioServiceClient.DeleteLedgerRollover(id?.ToString());

        public bool AnyLedgerRolloverError2s(string where = null) => FolioServiceClient.AnyLedgerRolloverErrors(where);

        public int CountLedgerRolloverError2s(string where = null) => FolioServiceClient.CountLedgerRolloverErrors(where);

        public LedgerRolloverError2[] LedgerRolloverError2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.LedgerRolloverErrors(out count, where, orderBy, skip, take).Select(jo =>
            {
                var lre2 = LedgerRolloverError2.FromJObject(jo);
                if (load && lre2.LedgerRolloverId != null) lre2.LedgerRollover = (LedgerRollover2)(cache && objects.ContainsKey(lre2.LedgerRolloverId.Value) ? objects[lre2.LedgerRolloverId.Value] : objects[lre2.LedgerRolloverId.Value] = FindLedgerRollover2(lre2.LedgerRolloverId));
                if (load && lre2.CreationUserId != null) lre2.CreationUser = (User2)(cache && objects.ContainsKey(lre2.CreationUserId.Value) ? objects[lre2.CreationUserId.Value] : objects[lre2.CreationUserId.Value] = FindUser2(lre2.CreationUserId));
                if (load && lre2.LastWriteUserId != null) lre2.LastWriteUser = (User2)(cache && objects.ContainsKey(lre2.LastWriteUserId.Value) ? objects[lre2.LastWriteUserId.Value] : objects[lre2.LastWriteUserId.Value] = FindUser2(lre2.LastWriteUserId));
                return lre2;
            }).ToArray();
        }

        public IEnumerable<LedgerRolloverError2> LedgerRolloverError2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.LedgerRolloverErrors(where, orderBy, skip, take))
            {
                var lre2 = LedgerRolloverError2.FromJObject(jo);
                if (load && lre2.LedgerRolloverId != null) lre2.LedgerRollover = (LedgerRollover2)(cache && objects.ContainsKey(lre2.LedgerRolloverId.Value) ? objects[lre2.LedgerRolloverId.Value] : objects[lre2.LedgerRolloverId.Value] = FindLedgerRollover2(lre2.LedgerRolloverId));
                if (load && lre2.CreationUserId != null) lre2.CreationUser = (User2)(cache && objects.ContainsKey(lre2.CreationUserId.Value) ? objects[lre2.CreationUserId.Value] : objects[lre2.CreationUserId.Value] = FindUser2(lre2.CreationUserId));
                if (load && lre2.LastWriteUserId != null) lre2.LastWriteUser = (User2)(cache && objects.ContainsKey(lre2.LastWriteUserId.Value) ? objects[lre2.LastWriteUserId.Value] : objects[lre2.LastWriteUserId.Value] = FindUser2(lre2.LastWriteUserId));
                yield return lre2;
            }
        }

        public LedgerRolloverError2 FindLedgerRolloverError2(Guid? id, bool load = false, bool cache = true)
        {
            var lre2 = LedgerRolloverError2.FromJObject(FolioServiceClient.GetLedgerRolloverError(id?.ToString()));
            if (lre2 == null) return null;
            if (load && lre2.LedgerRolloverId != null) lre2.LedgerRollover = (LedgerRollover2)(cache && objects.ContainsKey(lre2.LedgerRolloverId.Value) ? objects[lre2.LedgerRolloverId.Value] : objects[lre2.LedgerRolloverId.Value] = FindLedgerRollover2(lre2.LedgerRolloverId));
            if (load && lre2.CreationUserId != null) lre2.CreationUser = (User2)(cache && objects.ContainsKey(lre2.CreationUserId.Value) ? objects[lre2.CreationUserId.Value] : objects[lre2.CreationUserId.Value] = FindUser2(lre2.CreationUserId));
            if (load && lre2.LastWriteUserId != null) lre2.LastWriteUser = (User2)(cache && objects.ContainsKey(lre2.LastWriteUserId.Value) ? objects[lre2.LastWriteUserId.Value] : objects[lre2.LastWriteUserId.Value] = FindUser2(lre2.LastWriteUserId));
            return lre2;
        }

        public void Insert(LedgerRolloverError2 ledgerRolloverError2)
        {
            if (ledgerRolloverError2.Id == null) ledgerRolloverError2.Id = Guid.NewGuid();
            FolioServiceClient.InsertLedgerRolloverError(ledgerRolloverError2.ToJObject());
        }

        public void Update(LedgerRolloverError2 ledgerRolloverError2) => FolioServiceClient.UpdateLedgerRolloverError(ledgerRolloverError2.ToJObject());

        public void DeleteLedgerRolloverError2(Guid? id) => FolioServiceClient.DeleteLedgerRolloverError(id?.ToString());

        public bool AnyLedgerRolloverProgress2s(string where = null) => FolioServiceClient.AnyLedgerRolloverProgresses(where);

        public int CountLedgerRolloverProgress2s(string where = null) => FolioServiceClient.CountLedgerRolloverProgresses(where);

        public LedgerRolloverProgress2[] LedgerRolloverProgress2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.LedgerRolloverProgresses(out count, where, orderBy, skip, take).Select(jo =>
            {
                var lrp2 = LedgerRolloverProgress2.FromJObject(jo);
                if (load && lrp2.LedgerRolloverId != null) lrp2.LedgerRollover = (LedgerRollover2)(cache && objects.ContainsKey(lrp2.LedgerRolloverId.Value) ? objects[lrp2.LedgerRolloverId.Value] : objects[lrp2.LedgerRolloverId.Value] = FindLedgerRollover2(lrp2.LedgerRolloverId));
                if (load && lrp2.CreationUserId != null) lrp2.CreationUser = (User2)(cache && objects.ContainsKey(lrp2.CreationUserId.Value) ? objects[lrp2.CreationUserId.Value] : objects[lrp2.CreationUserId.Value] = FindUser2(lrp2.CreationUserId));
                if (load && lrp2.LastWriteUserId != null) lrp2.LastWriteUser = (User2)(cache && objects.ContainsKey(lrp2.LastWriteUserId.Value) ? objects[lrp2.LastWriteUserId.Value] : objects[lrp2.LastWriteUserId.Value] = FindUser2(lrp2.LastWriteUserId));
                return lrp2;
            }).ToArray();
        }

        public IEnumerable<LedgerRolloverProgress2> LedgerRolloverProgress2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.LedgerRolloverProgresses(where, orderBy, skip, take))
            {
                var lrp2 = LedgerRolloverProgress2.FromJObject(jo);
                if (load && lrp2.LedgerRolloverId != null) lrp2.LedgerRollover = (LedgerRollover2)(cache && objects.ContainsKey(lrp2.LedgerRolloverId.Value) ? objects[lrp2.LedgerRolloverId.Value] : objects[lrp2.LedgerRolloverId.Value] = FindLedgerRollover2(lrp2.LedgerRolloverId));
                if (load && lrp2.CreationUserId != null) lrp2.CreationUser = (User2)(cache && objects.ContainsKey(lrp2.CreationUserId.Value) ? objects[lrp2.CreationUserId.Value] : objects[lrp2.CreationUserId.Value] = FindUser2(lrp2.CreationUserId));
                if (load && lrp2.LastWriteUserId != null) lrp2.LastWriteUser = (User2)(cache && objects.ContainsKey(lrp2.LastWriteUserId.Value) ? objects[lrp2.LastWriteUserId.Value] : objects[lrp2.LastWriteUserId.Value] = FindUser2(lrp2.LastWriteUserId));
                yield return lrp2;
            }
        }

        public LedgerRolloverProgress2 FindLedgerRolloverProgress2(Guid? id, bool load = false, bool cache = true)
        {
            var lrp2 = LedgerRolloverProgress2.FromJObject(FolioServiceClient.GetLedgerRolloverProgress(id?.ToString()));
            if (lrp2 == null) return null;
            if (load && lrp2.LedgerRolloverId != null) lrp2.LedgerRollover = (LedgerRollover2)(cache && objects.ContainsKey(lrp2.LedgerRolloverId.Value) ? objects[lrp2.LedgerRolloverId.Value] : objects[lrp2.LedgerRolloverId.Value] = FindLedgerRollover2(lrp2.LedgerRolloverId));
            if (load && lrp2.CreationUserId != null) lrp2.CreationUser = (User2)(cache && objects.ContainsKey(lrp2.CreationUserId.Value) ? objects[lrp2.CreationUserId.Value] : objects[lrp2.CreationUserId.Value] = FindUser2(lrp2.CreationUserId));
            if (load && lrp2.LastWriteUserId != null) lrp2.LastWriteUser = (User2)(cache && objects.ContainsKey(lrp2.LastWriteUserId.Value) ? objects[lrp2.LastWriteUserId.Value] : objects[lrp2.LastWriteUserId.Value] = FindUser2(lrp2.LastWriteUserId));
            return lrp2;
        }

        public void Insert(LedgerRolloverProgress2 ledgerRolloverProgress2)
        {
            if (ledgerRolloverProgress2.Id == null) ledgerRolloverProgress2.Id = Guid.NewGuid();
            FolioServiceClient.InsertLedgerRolloverProgress(ledgerRolloverProgress2.ToJObject());
        }

        public void Update(LedgerRolloverProgress2 ledgerRolloverProgress2) => FolioServiceClient.UpdateLedgerRolloverProgress(ledgerRolloverProgress2.ToJObject());

        public void DeleteLedgerRolloverProgress2(Guid? id) => FolioServiceClient.DeleteLedgerRolloverProgress(id?.ToString());

        public bool AnyLibrary2s(string where = null) => FolioServiceClient.AnyLibraries(where);

        public int CountLibrary2s(string where = null) => FolioServiceClient.CountLibraries(where);

        public Library2[] Library2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Libraries(out count, where, orderBy, skip, take).Select(jo =>
            {
                var l2 = Library2.FromJObject(jo);
                if (load && l2.CampusId != null) l2.Campus = (Campus2)(cache && objects.ContainsKey(l2.CampusId.Value) ? objects[l2.CampusId.Value] : objects[l2.CampusId.Value] = FindCampus2(l2.CampusId));
                if (load && l2.CreationUserId != null) l2.CreationUser = (User2)(cache && objects.ContainsKey(l2.CreationUserId.Value) ? objects[l2.CreationUserId.Value] : objects[l2.CreationUserId.Value] = FindUser2(l2.CreationUserId));
                if (load && l2.LastWriteUserId != null) l2.LastWriteUser = (User2)(cache && objects.ContainsKey(l2.LastWriteUserId.Value) ? objects[l2.LastWriteUserId.Value] : objects[l2.LastWriteUserId.Value] = FindUser2(l2.LastWriteUserId));
                return l2;
            }).ToArray();
        }

        public IEnumerable<Library2> Library2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Libraries(where, orderBy, skip, take))
            {
                var l2 = Library2.FromJObject(jo);
                if (load && l2.CampusId != null) l2.Campus = (Campus2)(cache && objects.ContainsKey(l2.CampusId.Value) ? objects[l2.CampusId.Value] : objects[l2.CampusId.Value] = FindCampus2(l2.CampusId));
                if (load && l2.CreationUserId != null) l2.CreationUser = (User2)(cache && objects.ContainsKey(l2.CreationUserId.Value) ? objects[l2.CreationUserId.Value] : objects[l2.CreationUserId.Value] = FindUser2(l2.CreationUserId));
                if (load && l2.LastWriteUserId != null) l2.LastWriteUser = (User2)(cache && objects.ContainsKey(l2.LastWriteUserId.Value) ? objects[l2.LastWriteUserId.Value] : objects[l2.LastWriteUserId.Value] = FindUser2(l2.LastWriteUserId));
                yield return l2;
            }
        }

        public Library2 FindLibrary2(Guid? id, bool load = false, bool cache = true)
        {
            var l2 = Library2.FromJObject(FolioServiceClient.GetLibrary(id?.ToString()));
            if (l2 == null) return null;
            if (load && l2.CampusId != null) l2.Campus = (Campus2)(cache && objects.ContainsKey(l2.CampusId.Value) ? objects[l2.CampusId.Value] : objects[l2.CampusId.Value] = FindCampus2(l2.CampusId));
            if (load && l2.CreationUserId != null) l2.CreationUser = (User2)(cache && objects.ContainsKey(l2.CreationUserId.Value) ? objects[l2.CreationUserId.Value] : objects[l2.CreationUserId.Value] = FindUser2(l2.CreationUserId));
            if (load && l2.LastWriteUserId != null) l2.LastWriteUser = (User2)(cache && objects.ContainsKey(l2.LastWriteUserId.Value) ? objects[l2.LastWriteUserId.Value] : objects[l2.LastWriteUserId.Value] = FindUser2(l2.LastWriteUserId));
            return l2;
        }

        public void Insert(Library2 library2)
        {
            if (library2.Id == null) library2.Id = Guid.NewGuid();
            FolioServiceClient.InsertLibrary(library2.ToJObject());
        }

        public void Update(Library2 library2) => FolioServiceClient.UpdateLibrary(library2.ToJObject());

        public void DeleteLibrary2(Guid? id) => FolioServiceClient.DeleteLibrary(id?.ToString());

        public bool AnyLoan2s(string where = null) => FolioServiceClient.AnyLoans(where);

        public int CountLoan2s(string where = null) => FolioServiceClient.CountLoans(where);

        public Loan2[] Loan2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Loans(out count, where, orderBy, skip, take).Select(jo =>
            {
                var l2 = Loan2.FromJObject(jo);
                if (load && l2.UserId != null) l2.User = (User2)(cache && objects.ContainsKey(l2.UserId.Value) ? objects[l2.UserId.Value] : objects[l2.UserId.Value] = FindUser2(l2.UserId));
                if (load && l2.ProxyUserId != null) l2.ProxyUser = (User2)(cache && objects.ContainsKey(l2.ProxyUserId.Value) ? objects[l2.ProxyUserId.Value] : objects[l2.ProxyUserId.Value] = FindUser2(l2.ProxyUserId));
                if (load && l2.ItemId != null) l2.Item = (Item2)(cache && objects.ContainsKey(l2.ItemId.Value) ? objects[l2.ItemId.Value] : objects[l2.ItemId.Value] = FindItem2(l2.ItemId));
                if (load && l2.ItemEffectiveLocationAtCheckOutId != null) l2.ItemEffectiveLocationAtCheckOut = (Location2)(cache && objects.ContainsKey(l2.ItemEffectiveLocationAtCheckOutId.Value) ? objects[l2.ItemEffectiveLocationAtCheckOutId.Value] : objects[l2.ItemEffectiveLocationAtCheckOutId.Value] = FindLocation2(l2.ItemEffectiveLocationAtCheckOutId));
                if (load && l2.LoanPolicyId != null) l2.LoanPolicy = (LoanPolicy2)(cache && objects.ContainsKey(l2.LoanPolicyId.Value) ? objects[l2.LoanPolicyId.Value] : objects[l2.LoanPolicyId.Value] = FindLoanPolicy2(l2.LoanPolicyId));
                if (load && l2.CheckoutServicePointId != null) l2.CheckoutServicePoint = (ServicePoint2)(cache && objects.ContainsKey(l2.CheckoutServicePointId.Value) ? objects[l2.CheckoutServicePointId.Value] : objects[l2.CheckoutServicePointId.Value] = FindServicePoint2(l2.CheckoutServicePointId));
                if (load && l2.CheckinServicePointId != null) l2.CheckinServicePoint = (ServicePoint2)(cache && objects.ContainsKey(l2.CheckinServicePointId.Value) ? objects[l2.CheckinServicePointId.Value] : objects[l2.CheckinServicePointId.Value] = FindServicePoint2(l2.CheckinServicePointId));
                if (load && l2.GroupId != null) l2.Group = (Group2)(cache && objects.ContainsKey(l2.GroupId.Value) ? objects[l2.GroupId.Value] : objects[l2.GroupId.Value] = FindGroup2(l2.GroupId));
                if (load && l2.OverdueFinePolicyId != null) l2.OverdueFinePolicy = (OverdueFinePolicy2)(cache && objects.ContainsKey(l2.OverdueFinePolicyId.Value) ? objects[l2.OverdueFinePolicyId.Value] : objects[l2.OverdueFinePolicyId.Value] = FindOverdueFinePolicy2(l2.OverdueFinePolicyId));
                if (load && l2.LostItemPolicyId != null) l2.LostItemPolicy = (LostItemFeePolicy2)(cache && objects.ContainsKey(l2.LostItemPolicyId.Value) ? objects[l2.LostItemPolicyId.Value] : objects[l2.LostItemPolicyId.Value] = FindLostItemFeePolicy2(l2.LostItemPolicyId));
                if (load && l2.CreationUserId != null) l2.CreationUser = (User2)(cache && objects.ContainsKey(l2.CreationUserId.Value) ? objects[l2.CreationUserId.Value] : objects[l2.CreationUserId.Value] = FindUser2(l2.CreationUserId));
                if (load && l2.LastWriteUserId != null) l2.LastWriteUser = (User2)(cache && objects.ContainsKey(l2.LastWriteUserId.Value) ? objects[l2.LastWriteUserId.Value] : objects[l2.LastWriteUserId.Value] = FindUser2(l2.LastWriteUserId));
                return l2;
            }).ToArray();
        }

        public IEnumerable<Loan2> Loan2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Loans(where, orderBy, skip, take))
            {
                var l2 = Loan2.FromJObject(jo);
                if (load && l2.UserId != null) l2.User = (User2)(cache && objects.ContainsKey(l2.UserId.Value) ? objects[l2.UserId.Value] : objects[l2.UserId.Value] = FindUser2(l2.UserId));
                if (load && l2.ProxyUserId != null) l2.ProxyUser = (User2)(cache && objects.ContainsKey(l2.ProxyUserId.Value) ? objects[l2.ProxyUserId.Value] : objects[l2.ProxyUserId.Value] = FindUser2(l2.ProxyUserId));
                if (load && l2.ItemId != null) l2.Item = (Item2)(cache && objects.ContainsKey(l2.ItemId.Value) ? objects[l2.ItemId.Value] : objects[l2.ItemId.Value] = FindItem2(l2.ItemId));
                if (load && l2.ItemEffectiveLocationAtCheckOutId != null) l2.ItemEffectiveLocationAtCheckOut = (Location2)(cache && objects.ContainsKey(l2.ItemEffectiveLocationAtCheckOutId.Value) ? objects[l2.ItemEffectiveLocationAtCheckOutId.Value] : objects[l2.ItemEffectiveLocationAtCheckOutId.Value] = FindLocation2(l2.ItemEffectiveLocationAtCheckOutId));
                if (load && l2.LoanPolicyId != null) l2.LoanPolicy = (LoanPolicy2)(cache && objects.ContainsKey(l2.LoanPolicyId.Value) ? objects[l2.LoanPolicyId.Value] : objects[l2.LoanPolicyId.Value] = FindLoanPolicy2(l2.LoanPolicyId));
                if (load && l2.CheckoutServicePointId != null) l2.CheckoutServicePoint = (ServicePoint2)(cache && objects.ContainsKey(l2.CheckoutServicePointId.Value) ? objects[l2.CheckoutServicePointId.Value] : objects[l2.CheckoutServicePointId.Value] = FindServicePoint2(l2.CheckoutServicePointId));
                if (load && l2.CheckinServicePointId != null) l2.CheckinServicePoint = (ServicePoint2)(cache && objects.ContainsKey(l2.CheckinServicePointId.Value) ? objects[l2.CheckinServicePointId.Value] : objects[l2.CheckinServicePointId.Value] = FindServicePoint2(l2.CheckinServicePointId));
                if (load && l2.GroupId != null) l2.Group = (Group2)(cache && objects.ContainsKey(l2.GroupId.Value) ? objects[l2.GroupId.Value] : objects[l2.GroupId.Value] = FindGroup2(l2.GroupId));
                if (load && l2.OverdueFinePolicyId != null) l2.OverdueFinePolicy = (OverdueFinePolicy2)(cache && objects.ContainsKey(l2.OverdueFinePolicyId.Value) ? objects[l2.OverdueFinePolicyId.Value] : objects[l2.OverdueFinePolicyId.Value] = FindOverdueFinePolicy2(l2.OverdueFinePolicyId));
                if (load && l2.LostItemPolicyId != null) l2.LostItemPolicy = (LostItemFeePolicy2)(cache && objects.ContainsKey(l2.LostItemPolicyId.Value) ? objects[l2.LostItemPolicyId.Value] : objects[l2.LostItemPolicyId.Value] = FindLostItemFeePolicy2(l2.LostItemPolicyId));
                if (load && l2.CreationUserId != null) l2.CreationUser = (User2)(cache && objects.ContainsKey(l2.CreationUserId.Value) ? objects[l2.CreationUserId.Value] : objects[l2.CreationUserId.Value] = FindUser2(l2.CreationUserId));
                if (load && l2.LastWriteUserId != null) l2.LastWriteUser = (User2)(cache && objects.ContainsKey(l2.LastWriteUserId.Value) ? objects[l2.LastWriteUserId.Value] : objects[l2.LastWriteUserId.Value] = FindUser2(l2.LastWriteUserId));
                yield return l2;
            }
        }

        public Loan2 FindLoan2(Guid? id, bool load = false, bool cache = true)
        {
            var l2 = Loan2.FromJObject(FolioServiceClient.GetLoan(id?.ToString()));
            if (l2 == null) return null;
            if (load && l2.UserId != null) l2.User = (User2)(cache && objects.ContainsKey(l2.UserId.Value) ? objects[l2.UserId.Value] : objects[l2.UserId.Value] = FindUser2(l2.UserId));
            if (load && l2.ProxyUserId != null) l2.ProxyUser = (User2)(cache && objects.ContainsKey(l2.ProxyUserId.Value) ? objects[l2.ProxyUserId.Value] : objects[l2.ProxyUserId.Value] = FindUser2(l2.ProxyUserId));
            if (load && l2.ItemId != null) l2.Item = (Item2)(cache && objects.ContainsKey(l2.ItemId.Value) ? objects[l2.ItemId.Value] : objects[l2.ItemId.Value] = FindItem2(l2.ItemId));
            if (load && l2.ItemEffectiveLocationAtCheckOutId != null) l2.ItemEffectiveLocationAtCheckOut = (Location2)(cache && objects.ContainsKey(l2.ItemEffectiveLocationAtCheckOutId.Value) ? objects[l2.ItemEffectiveLocationAtCheckOutId.Value] : objects[l2.ItemEffectiveLocationAtCheckOutId.Value] = FindLocation2(l2.ItemEffectiveLocationAtCheckOutId));
            if (load && l2.LoanPolicyId != null) l2.LoanPolicy = (LoanPolicy2)(cache && objects.ContainsKey(l2.LoanPolicyId.Value) ? objects[l2.LoanPolicyId.Value] : objects[l2.LoanPolicyId.Value] = FindLoanPolicy2(l2.LoanPolicyId));
            if (load && l2.CheckoutServicePointId != null) l2.CheckoutServicePoint = (ServicePoint2)(cache && objects.ContainsKey(l2.CheckoutServicePointId.Value) ? objects[l2.CheckoutServicePointId.Value] : objects[l2.CheckoutServicePointId.Value] = FindServicePoint2(l2.CheckoutServicePointId));
            if (load && l2.CheckinServicePointId != null) l2.CheckinServicePoint = (ServicePoint2)(cache && objects.ContainsKey(l2.CheckinServicePointId.Value) ? objects[l2.CheckinServicePointId.Value] : objects[l2.CheckinServicePointId.Value] = FindServicePoint2(l2.CheckinServicePointId));
            if (load && l2.GroupId != null) l2.Group = (Group2)(cache && objects.ContainsKey(l2.GroupId.Value) ? objects[l2.GroupId.Value] : objects[l2.GroupId.Value] = FindGroup2(l2.GroupId));
            if (load && l2.OverdueFinePolicyId != null) l2.OverdueFinePolicy = (OverdueFinePolicy2)(cache && objects.ContainsKey(l2.OverdueFinePolicyId.Value) ? objects[l2.OverdueFinePolicyId.Value] : objects[l2.OverdueFinePolicyId.Value] = FindOverdueFinePolicy2(l2.OverdueFinePolicyId));
            if (load && l2.LostItemPolicyId != null) l2.LostItemPolicy = (LostItemFeePolicy2)(cache && objects.ContainsKey(l2.LostItemPolicyId.Value) ? objects[l2.LostItemPolicyId.Value] : objects[l2.LostItemPolicyId.Value] = FindLostItemFeePolicy2(l2.LostItemPolicyId));
            if (load && l2.CreationUserId != null) l2.CreationUser = (User2)(cache && objects.ContainsKey(l2.CreationUserId.Value) ? objects[l2.CreationUserId.Value] : objects[l2.CreationUserId.Value] = FindUser2(l2.CreationUserId));
            if (load && l2.LastWriteUserId != null) l2.LastWriteUser = (User2)(cache && objects.ContainsKey(l2.LastWriteUserId.Value) ? objects[l2.LastWriteUserId.Value] : objects[l2.LastWriteUserId.Value] = FindUser2(l2.LastWriteUserId));
            return l2;
        }

        public void Insert(Loan2 loan2)
        {
            if (loan2.Id == null) loan2.Id = Guid.NewGuid();
            FolioServiceClient.InsertLoan(loan2.ToJObject());
        }

        public void Update(Loan2 loan2) => FolioServiceClient.UpdateLoan(loan2.ToJObject());

        public void DeleteLoan2(Guid? id) => FolioServiceClient.DeleteLoan(id?.ToString());

        public bool AnyLoanPolicy2s(string where = null) => FolioServiceClient.AnyLoanPolicies(where);

        public int CountLoanPolicy2s(string where = null) => FolioServiceClient.CountLoanPolicies(where);

        public LoanPolicy2[] LoanPolicy2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.LoanPolicies(out count, where, orderBy, skip, take).Select(jo =>
            {
                var lp2 = LoanPolicy2.FromJObject(jo);
                if (load && lp2.LoansPolicyFixedDueDateScheduleId != null) lp2.LoansPolicyFixedDueDateSchedule = (FixedDueDateSchedule2)(cache && objects.ContainsKey(lp2.LoansPolicyFixedDueDateScheduleId.Value) ? objects[lp2.LoansPolicyFixedDueDateScheduleId.Value] : objects[lp2.LoansPolicyFixedDueDateScheduleId.Value] = FindFixedDueDateSchedule2(lp2.LoansPolicyFixedDueDateScheduleId));
                if (load && lp2.RenewalsPolicyAlternateFixedDueDateScheduleId != null) lp2.RenewalsPolicyAlternateFixedDueDateSchedule = (FixedDueDateSchedule2)(cache && objects.ContainsKey(lp2.RenewalsPolicyAlternateFixedDueDateScheduleId.Value) ? objects[lp2.RenewalsPolicyAlternateFixedDueDateScheduleId.Value] : objects[lp2.RenewalsPolicyAlternateFixedDueDateScheduleId.Value] = FindFixedDueDateSchedule2(lp2.RenewalsPolicyAlternateFixedDueDateScheduleId));
                if (load && lp2.CreationUserId != null) lp2.CreationUser = (User2)(cache && objects.ContainsKey(lp2.CreationUserId.Value) ? objects[lp2.CreationUserId.Value] : objects[lp2.CreationUserId.Value] = FindUser2(lp2.CreationUserId));
                if (load && lp2.LastWriteUserId != null) lp2.LastWriteUser = (User2)(cache && objects.ContainsKey(lp2.LastWriteUserId.Value) ? objects[lp2.LastWriteUserId.Value] : objects[lp2.LastWriteUserId.Value] = FindUser2(lp2.LastWriteUserId));
                return lp2;
            }).ToArray();
        }

        public IEnumerable<LoanPolicy2> LoanPolicy2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.LoanPolicies(where, orderBy, skip, take))
            {
                var lp2 = LoanPolicy2.FromJObject(jo);
                if (load && lp2.LoansPolicyFixedDueDateScheduleId != null) lp2.LoansPolicyFixedDueDateSchedule = (FixedDueDateSchedule2)(cache && objects.ContainsKey(lp2.LoansPolicyFixedDueDateScheduleId.Value) ? objects[lp2.LoansPolicyFixedDueDateScheduleId.Value] : objects[lp2.LoansPolicyFixedDueDateScheduleId.Value] = FindFixedDueDateSchedule2(lp2.LoansPolicyFixedDueDateScheduleId));
                if (load && lp2.RenewalsPolicyAlternateFixedDueDateScheduleId != null) lp2.RenewalsPolicyAlternateFixedDueDateSchedule = (FixedDueDateSchedule2)(cache && objects.ContainsKey(lp2.RenewalsPolicyAlternateFixedDueDateScheduleId.Value) ? objects[lp2.RenewalsPolicyAlternateFixedDueDateScheduleId.Value] : objects[lp2.RenewalsPolicyAlternateFixedDueDateScheduleId.Value] = FindFixedDueDateSchedule2(lp2.RenewalsPolicyAlternateFixedDueDateScheduleId));
                if (load && lp2.CreationUserId != null) lp2.CreationUser = (User2)(cache && objects.ContainsKey(lp2.CreationUserId.Value) ? objects[lp2.CreationUserId.Value] : objects[lp2.CreationUserId.Value] = FindUser2(lp2.CreationUserId));
                if (load && lp2.LastWriteUserId != null) lp2.LastWriteUser = (User2)(cache && objects.ContainsKey(lp2.LastWriteUserId.Value) ? objects[lp2.LastWriteUserId.Value] : objects[lp2.LastWriteUserId.Value] = FindUser2(lp2.LastWriteUserId));
                yield return lp2;
            }
        }

        public LoanPolicy2 FindLoanPolicy2(Guid? id, bool load = false, bool cache = true)
        {
            var lp2 = LoanPolicy2.FromJObject(FolioServiceClient.GetLoanPolicy(id?.ToString()));
            if (lp2 == null) return null;
            if (load && lp2.LoansPolicyFixedDueDateScheduleId != null) lp2.LoansPolicyFixedDueDateSchedule = (FixedDueDateSchedule2)(cache && objects.ContainsKey(lp2.LoansPolicyFixedDueDateScheduleId.Value) ? objects[lp2.LoansPolicyFixedDueDateScheduleId.Value] : objects[lp2.LoansPolicyFixedDueDateScheduleId.Value] = FindFixedDueDateSchedule2(lp2.LoansPolicyFixedDueDateScheduleId));
            if (load && lp2.RenewalsPolicyAlternateFixedDueDateScheduleId != null) lp2.RenewalsPolicyAlternateFixedDueDateSchedule = (FixedDueDateSchedule2)(cache && objects.ContainsKey(lp2.RenewalsPolicyAlternateFixedDueDateScheduleId.Value) ? objects[lp2.RenewalsPolicyAlternateFixedDueDateScheduleId.Value] : objects[lp2.RenewalsPolicyAlternateFixedDueDateScheduleId.Value] = FindFixedDueDateSchedule2(lp2.RenewalsPolicyAlternateFixedDueDateScheduleId));
            if (load && lp2.CreationUserId != null) lp2.CreationUser = (User2)(cache && objects.ContainsKey(lp2.CreationUserId.Value) ? objects[lp2.CreationUserId.Value] : objects[lp2.CreationUserId.Value] = FindUser2(lp2.CreationUserId));
            if (load && lp2.LastWriteUserId != null) lp2.LastWriteUser = (User2)(cache && objects.ContainsKey(lp2.LastWriteUserId.Value) ? objects[lp2.LastWriteUserId.Value] : objects[lp2.LastWriteUserId.Value] = FindUser2(lp2.LastWriteUserId));
            return lp2;
        }

        public void Insert(LoanPolicy2 loanPolicy2)
        {
            if (loanPolicy2.Id == null) loanPolicy2.Id = Guid.NewGuid();
            FolioServiceClient.InsertLoanPolicy(loanPolicy2.ToJObject());
        }

        public void Update(LoanPolicy2 loanPolicy2) => FolioServiceClient.UpdateLoanPolicy(loanPolicy2.ToJObject());

        public void DeleteLoanPolicy2(Guid? id) => FolioServiceClient.DeleteLoanPolicy(id?.ToString());

        public bool AnyLoanType2s(string where = null) => FolioServiceClient.AnyLoanTypes(where);

        public int CountLoanType2s(string where = null) => FolioServiceClient.CountLoanTypes(where);

        public LoanType2[] LoanType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.LoanTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var lt2 = LoanType2.FromJObject(jo);
                if (load && lt2.CreationUserId != null) lt2.CreationUser = (User2)(cache && objects.ContainsKey(lt2.CreationUserId.Value) ? objects[lt2.CreationUserId.Value] : objects[lt2.CreationUserId.Value] = FindUser2(lt2.CreationUserId));
                if (load && lt2.LastWriteUserId != null) lt2.LastWriteUser = (User2)(cache && objects.ContainsKey(lt2.LastWriteUserId.Value) ? objects[lt2.LastWriteUserId.Value] : objects[lt2.LastWriteUserId.Value] = FindUser2(lt2.LastWriteUserId));
                return lt2;
            }).ToArray();
        }

        public IEnumerable<LoanType2> LoanType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.LoanTypes(where, orderBy, skip, take))
            {
                var lt2 = LoanType2.FromJObject(jo);
                if (load && lt2.CreationUserId != null) lt2.CreationUser = (User2)(cache && objects.ContainsKey(lt2.CreationUserId.Value) ? objects[lt2.CreationUserId.Value] : objects[lt2.CreationUserId.Value] = FindUser2(lt2.CreationUserId));
                if (load && lt2.LastWriteUserId != null) lt2.LastWriteUser = (User2)(cache && objects.ContainsKey(lt2.LastWriteUserId.Value) ? objects[lt2.LastWriteUserId.Value] : objects[lt2.LastWriteUserId.Value] = FindUser2(lt2.LastWriteUserId));
                yield return lt2;
            }
        }

        public LoanType2 FindLoanType2(Guid? id, bool load = false, bool cache = true)
        {
            var lt2 = LoanType2.FromJObject(FolioServiceClient.GetLoanType(id?.ToString()));
            if (lt2 == null) return null;
            if (load && lt2.CreationUserId != null) lt2.CreationUser = (User2)(cache && objects.ContainsKey(lt2.CreationUserId.Value) ? objects[lt2.CreationUserId.Value] : objects[lt2.CreationUserId.Value] = FindUser2(lt2.CreationUserId));
            if (load && lt2.LastWriteUserId != null) lt2.LastWriteUser = (User2)(cache && objects.ContainsKey(lt2.LastWriteUserId.Value) ? objects[lt2.LastWriteUserId.Value] : objects[lt2.LastWriteUserId.Value] = FindUser2(lt2.LastWriteUserId));
            return lt2;
        }

        public void Insert(LoanType2 loanType2)
        {
            if (loanType2.Id == null) loanType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertLoanType(loanType2.ToJObject());
        }

        public void Update(LoanType2 loanType2) => FolioServiceClient.UpdateLoanType(loanType2.ToJObject());

        public void DeleteLoanType2(Guid? id) => FolioServiceClient.DeleteLoanType(id?.ToString());

        public bool AnyLocation2s(string where = null) => FolioServiceClient.AnyLocations(where);

        public int CountLocation2s(string where = null) => FolioServiceClient.CountLocations(where);

        public Location2[] Location2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Locations(out count, where, orderBy, skip, take).Select(jo =>
            {
                var l2 = Location2.FromJObject(jo);
                if (load && l2.InstitutionId != null) l2.Institution = (Institution2)(cache && objects.ContainsKey(l2.InstitutionId.Value) ? objects[l2.InstitutionId.Value] : objects[l2.InstitutionId.Value] = FindInstitution2(l2.InstitutionId));
                if (load && l2.CampusId != null) l2.Campus = (Campus2)(cache && objects.ContainsKey(l2.CampusId.Value) ? objects[l2.CampusId.Value] : objects[l2.CampusId.Value] = FindCampus2(l2.CampusId));
                if (load && l2.LibraryId != null) l2.Library = (Library2)(cache && objects.ContainsKey(l2.LibraryId.Value) ? objects[l2.LibraryId.Value] : objects[l2.LibraryId.Value] = FindLibrary2(l2.LibraryId));
                if (load && l2.PrimaryServicePointId != null) l2.PrimaryServicePoint = (ServicePoint2)(cache && objects.ContainsKey(l2.PrimaryServicePointId.Value) ? objects[l2.PrimaryServicePointId.Value] : objects[l2.PrimaryServicePointId.Value] = FindServicePoint2(l2.PrimaryServicePointId));
                if (load && l2.CreationUserId != null) l2.CreationUser = (User2)(cache && objects.ContainsKey(l2.CreationUserId.Value) ? objects[l2.CreationUserId.Value] : objects[l2.CreationUserId.Value] = FindUser2(l2.CreationUserId));
                if (load && l2.LastWriteUserId != null) l2.LastWriteUser = (User2)(cache && objects.ContainsKey(l2.LastWriteUserId.Value) ? objects[l2.LastWriteUserId.Value] : objects[l2.LastWriteUserId.Value] = FindUser2(l2.LastWriteUserId));
                return l2;
            }).ToArray();
        }

        public IEnumerable<Location2> Location2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Locations(where, orderBy, skip, take))
            {
                var l2 = Location2.FromJObject(jo);
                if (load && l2.InstitutionId != null) l2.Institution = (Institution2)(cache && objects.ContainsKey(l2.InstitutionId.Value) ? objects[l2.InstitutionId.Value] : objects[l2.InstitutionId.Value] = FindInstitution2(l2.InstitutionId));
                if (load && l2.CampusId != null) l2.Campus = (Campus2)(cache && objects.ContainsKey(l2.CampusId.Value) ? objects[l2.CampusId.Value] : objects[l2.CampusId.Value] = FindCampus2(l2.CampusId));
                if (load && l2.LibraryId != null) l2.Library = (Library2)(cache && objects.ContainsKey(l2.LibraryId.Value) ? objects[l2.LibraryId.Value] : objects[l2.LibraryId.Value] = FindLibrary2(l2.LibraryId));
                if (load && l2.PrimaryServicePointId != null) l2.PrimaryServicePoint = (ServicePoint2)(cache && objects.ContainsKey(l2.PrimaryServicePointId.Value) ? objects[l2.PrimaryServicePointId.Value] : objects[l2.PrimaryServicePointId.Value] = FindServicePoint2(l2.PrimaryServicePointId));
                if (load && l2.CreationUserId != null) l2.CreationUser = (User2)(cache && objects.ContainsKey(l2.CreationUserId.Value) ? objects[l2.CreationUserId.Value] : objects[l2.CreationUserId.Value] = FindUser2(l2.CreationUserId));
                if (load && l2.LastWriteUserId != null) l2.LastWriteUser = (User2)(cache && objects.ContainsKey(l2.LastWriteUserId.Value) ? objects[l2.LastWriteUserId.Value] : objects[l2.LastWriteUserId.Value] = FindUser2(l2.LastWriteUserId));
                yield return l2;
            }
        }

        public Location2 FindLocation2(Guid? id, bool load = false, bool cache = true)
        {
            var l2 = Location2.FromJObject(FolioServiceClient.GetLocation(id?.ToString()));
            if (l2 == null) return null;
            if (load && l2.InstitutionId != null) l2.Institution = (Institution2)(cache && objects.ContainsKey(l2.InstitutionId.Value) ? objects[l2.InstitutionId.Value] : objects[l2.InstitutionId.Value] = FindInstitution2(l2.InstitutionId));
            if (load && l2.CampusId != null) l2.Campus = (Campus2)(cache && objects.ContainsKey(l2.CampusId.Value) ? objects[l2.CampusId.Value] : objects[l2.CampusId.Value] = FindCampus2(l2.CampusId));
            if (load && l2.LibraryId != null) l2.Library = (Library2)(cache && objects.ContainsKey(l2.LibraryId.Value) ? objects[l2.LibraryId.Value] : objects[l2.LibraryId.Value] = FindLibrary2(l2.LibraryId));
            if (load && l2.PrimaryServicePointId != null) l2.PrimaryServicePoint = (ServicePoint2)(cache && objects.ContainsKey(l2.PrimaryServicePointId.Value) ? objects[l2.PrimaryServicePointId.Value] : objects[l2.PrimaryServicePointId.Value] = FindServicePoint2(l2.PrimaryServicePointId));
            if (load && l2.CreationUserId != null) l2.CreationUser = (User2)(cache && objects.ContainsKey(l2.CreationUserId.Value) ? objects[l2.CreationUserId.Value] : objects[l2.CreationUserId.Value] = FindUser2(l2.CreationUserId));
            if (load && l2.LastWriteUserId != null) l2.LastWriteUser = (User2)(cache && objects.ContainsKey(l2.LastWriteUserId.Value) ? objects[l2.LastWriteUserId.Value] : objects[l2.LastWriteUserId.Value] = FindUser2(l2.LastWriteUserId));
            var i = 0;
            if (l2.LocationServicePoints != null) foreach (var lsp in l2.LocationServicePoints)
                {
                    lsp.Id = (++i).ToString();
                    lsp.LocationId = l2.Id;
                }
            return l2;
        }

        public void Insert(Location2 location2)
        {
            if (location2.Id == null) location2.Id = Guid.NewGuid();
            FolioServiceClient.InsertLocation(location2.ToJObject());
        }

        public void Update(Location2 location2) => FolioServiceClient.UpdateLocation(location2.ToJObject());

        public void DeleteLocation2(Guid? id) => FolioServiceClient.DeleteLocation(id?.ToString());

        public bool AnyLocationSettings(string where = null) => FolioServiceClient.AnyLocationSettings(where);

        public int CountLocationSettings(string where = null) => FolioServiceClient.CountLocationSettings(where);

        public LocationSetting[] LocationSettings(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.LocationSettings(out count, where, orderBy, skip, take).Select(jo =>
            {
                var ls = LocationSetting.FromJObject(jo);
                if (load && ls.LocationId != null) ls.Location = (Location2)(cache && objects.ContainsKey(ls.LocationId.Value) ? objects[ls.LocationId.Value] : objects[ls.LocationId.Value] = FindLocation2(ls.LocationId));
                if (load && ls.SettingsId != null) ls.Settings = (Setting)(cache && objects.ContainsKey(ls.SettingsId.Value) ? objects[ls.SettingsId.Value] : objects[ls.SettingsId.Value] = FindSetting(ls.SettingsId));
                if (load && ls.CreationUserId != null) ls.CreationUser = (User2)(cache && objects.ContainsKey(ls.CreationUserId.Value) ? objects[ls.CreationUserId.Value] : objects[ls.CreationUserId.Value] = FindUser2(ls.CreationUserId));
                if (load && ls.LastWriteUserId != null) ls.LastWriteUser = (User2)(cache && objects.ContainsKey(ls.LastWriteUserId.Value) ? objects[ls.LastWriteUserId.Value] : objects[ls.LastWriteUserId.Value] = FindUser2(ls.LastWriteUserId));
                return ls;
            }).ToArray();
        }

        public IEnumerable<LocationSetting> LocationSettings(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.LocationSettings(where, orderBy, skip, take))
            {
                var ls = LocationSetting.FromJObject(jo);
                if (load && ls.LocationId != null) ls.Location = (Location2)(cache && objects.ContainsKey(ls.LocationId.Value) ? objects[ls.LocationId.Value] : objects[ls.LocationId.Value] = FindLocation2(ls.LocationId));
                if (load && ls.SettingsId != null) ls.Settings = (Setting)(cache && objects.ContainsKey(ls.SettingsId.Value) ? objects[ls.SettingsId.Value] : objects[ls.SettingsId.Value] = FindSetting(ls.SettingsId));
                if (load && ls.CreationUserId != null) ls.CreationUser = (User2)(cache && objects.ContainsKey(ls.CreationUserId.Value) ? objects[ls.CreationUserId.Value] : objects[ls.CreationUserId.Value] = FindUser2(ls.CreationUserId));
                if (load && ls.LastWriteUserId != null) ls.LastWriteUser = (User2)(cache && objects.ContainsKey(ls.LastWriteUserId.Value) ? objects[ls.LastWriteUserId.Value] : objects[ls.LastWriteUserId.Value] = FindUser2(ls.LastWriteUserId));
                yield return ls;
            }
        }

        public LocationSetting FindLocationSetting(Guid? id, bool load = false, bool cache = true)
        {
            var ls = LocationSetting.FromJObject(FolioServiceClient.GetLocationSetting(id?.ToString()));
            if (ls == null) return null;
            if (load && ls.LocationId != null) ls.Location = (Location2)(cache && objects.ContainsKey(ls.LocationId.Value) ? objects[ls.LocationId.Value] : objects[ls.LocationId.Value] = FindLocation2(ls.LocationId));
            if (load && ls.SettingsId != null) ls.Settings = (Setting)(cache && objects.ContainsKey(ls.SettingsId.Value) ? objects[ls.SettingsId.Value] : objects[ls.SettingsId.Value] = FindSetting(ls.SettingsId));
            if (load && ls.CreationUserId != null) ls.CreationUser = (User2)(cache && objects.ContainsKey(ls.CreationUserId.Value) ? objects[ls.CreationUserId.Value] : objects[ls.CreationUserId.Value] = FindUser2(ls.CreationUserId));
            if (load && ls.LastWriteUserId != null) ls.LastWriteUser = (User2)(cache && objects.ContainsKey(ls.LastWriteUserId.Value) ? objects[ls.LastWriteUserId.Value] : objects[ls.LastWriteUserId.Value] = FindUser2(ls.LastWriteUserId));
            return ls;
        }

        public void Insert(LocationSetting locationSetting)
        {
            if (locationSetting.Id == null) locationSetting.Id = Guid.NewGuid();
            FolioServiceClient.InsertLocationSetting(locationSetting.ToJObject());
        }

        public void Update(LocationSetting locationSetting) => FolioServiceClient.UpdateLocationSetting(locationSetting.ToJObject());

        public void DeleteLocationSetting(Guid? id) => FolioServiceClient.DeleteLocationSetting(id?.ToString());

        public void Insert(Login2 login2)
        {
            if (login2.Id == null) login2.Id = Guid.NewGuid();
            FolioServiceClient.InsertLogin(login2.ToJObject());
        }

        public bool AnyLostItemFeePolicy2s(string where = null) => FolioServiceClient.AnyLostItemFeePolicies(where);

        public int CountLostItemFeePolicy2s(string where = null) => FolioServiceClient.CountLostItemFeePolicies(where);

        public LostItemFeePolicy2[] LostItemFeePolicy2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.LostItemFeePolicies(out count, where, orderBy, skip, take).Select(jo =>
            {
                var lifp2 = LostItemFeePolicy2.FromJObject(jo);
                if (load && lifp2.CreationUserId != null) lifp2.CreationUser = (User2)(cache && objects.ContainsKey(lifp2.CreationUserId.Value) ? objects[lifp2.CreationUserId.Value] : objects[lifp2.CreationUserId.Value] = FindUser2(lifp2.CreationUserId));
                if (load && lifp2.LastWriteUserId != null) lifp2.LastWriteUser = (User2)(cache && objects.ContainsKey(lifp2.LastWriteUserId.Value) ? objects[lifp2.LastWriteUserId.Value] : objects[lifp2.LastWriteUserId.Value] = FindUser2(lifp2.LastWriteUserId));
                return lifp2;
            }).ToArray();
        }

        public IEnumerable<LostItemFeePolicy2> LostItemFeePolicy2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.LostItemFeePolicies(where, orderBy, skip, take))
            {
                var lifp2 = LostItemFeePolicy2.FromJObject(jo);
                if (load && lifp2.CreationUserId != null) lifp2.CreationUser = (User2)(cache && objects.ContainsKey(lifp2.CreationUserId.Value) ? objects[lifp2.CreationUserId.Value] : objects[lifp2.CreationUserId.Value] = FindUser2(lifp2.CreationUserId));
                if (load && lifp2.LastWriteUserId != null) lifp2.LastWriteUser = (User2)(cache && objects.ContainsKey(lifp2.LastWriteUserId.Value) ? objects[lifp2.LastWriteUserId.Value] : objects[lifp2.LastWriteUserId.Value] = FindUser2(lifp2.LastWriteUserId));
                yield return lifp2;
            }
        }

        public LostItemFeePolicy2 FindLostItemFeePolicy2(Guid? id, bool load = false, bool cache = true)
        {
            var lifp2 = LostItemFeePolicy2.FromJObject(FolioServiceClient.GetLostItemFeePolicy(id?.ToString()));
            if (lifp2 == null) return null;
            if (load && lifp2.CreationUserId != null) lifp2.CreationUser = (User2)(cache && objects.ContainsKey(lifp2.CreationUserId.Value) ? objects[lifp2.CreationUserId.Value] : objects[lifp2.CreationUserId.Value] = FindUser2(lifp2.CreationUserId));
            if (load && lifp2.LastWriteUserId != null) lifp2.LastWriteUser = (User2)(cache && objects.ContainsKey(lifp2.LastWriteUserId.Value) ? objects[lifp2.LastWriteUserId.Value] : objects[lifp2.LastWriteUserId.Value] = FindUser2(lifp2.LastWriteUserId));
            return lifp2;
        }

        public void Insert(LostItemFeePolicy2 lostItemFeePolicy2)
        {
            if (lostItemFeePolicy2.Id == null) lostItemFeePolicy2.Id = Guid.NewGuid();
            FolioServiceClient.InsertLostItemFeePolicy(lostItemFeePolicy2.ToJObject());
        }

        public void Update(LostItemFeePolicy2 lostItemFeePolicy2) => FolioServiceClient.UpdateLostItemFeePolicy(lostItemFeePolicy2.ToJObject());

        public void DeleteLostItemFeePolicy2(Guid? id) => FolioServiceClient.DeleteLostItemFeePolicy(id?.ToString());

        public bool AnyManualBlockTemplate2s(string where = null) => FolioServiceClient.AnyManualBlockTemplates(where);

        public int CountManualBlockTemplate2s(string where = null) => FolioServiceClient.CountManualBlockTemplates(where);

        public ManualBlockTemplate2[] ManualBlockTemplate2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ManualBlockTemplates(out count, where, orderBy, skip, take).Select(jo =>
            {
                var mbt2 = ManualBlockTemplate2.FromJObject(jo);
                if (load && mbt2.CreationUserId != null) mbt2.CreationUser = (User2)(cache && objects.ContainsKey(mbt2.CreationUserId.Value) ? objects[mbt2.CreationUserId.Value] : objects[mbt2.CreationUserId.Value] = FindUser2(mbt2.CreationUserId));
                if (load && mbt2.LastWriteUserId != null) mbt2.LastWriteUser = (User2)(cache && objects.ContainsKey(mbt2.LastWriteUserId.Value) ? objects[mbt2.LastWriteUserId.Value] : objects[mbt2.LastWriteUserId.Value] = FindUser2(mbt2.LastWriteUserId));
                return mbt2;
            }).ToArray();
        }

        public IEnumerable<ManualBlockTemplate2> ManualBlockTemplate2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ManualBlockTemplates(where, orderBy, skip, take))
            {
                var mbt2 = ManualBlockTemplate2.FromJObject(jo);
                if (load && mbt2.CreationUserId != null) mbt2.CreationUser = (User2)(cache && objects.ContainsKey(mbt2.CreationUserId.Value) ? objects[mbt2.CreationUserId.Value] : objects[mbt2.CreationUserId.Value] = FindUser2(mbt2.CreationUserId));
                if (load && mbt2.LastWriteUserId != null) mbt2.LastWriteUser = (User2)(cache && objects.ContainsKey(mbt2.LastWriteUserId.Value) ? objects[mbt2.LastWriteUserId.Value] : objects[mbt2.LastWriteUserId.Value] = FindUser2(mbt2.LastWriteUserId));
                yield return mbt2;
            }
        }

        public ManualBlockTemplate2 FindManualBlockTemplate2(Guid? id, bool load = false, bool cache = true)
        {
            var mbt2 = ManualBlockTemplate2.FromJObject(FolioServiceClient.GetManualBlockTemplate(id?.ToString()));
            if (mbt2 == null) return null;
            if (load && mbt2.CreationUserId != null) mbt2.CreationUser = (User2)(cache && objects.ContainsKey(mbt2.CreationUserId.Value) ? objects[mbt2.CreationUserId.Value] : objects[mbt2.CreationUserId.Value] = FindUser2(mbt2.CreationUserId));
            if (load && mbt2.LastWriteUserId != null) mbt2.LastWriteUser = (User2)(cache && objects.ContainsKey(mbt2.LastWriteUserId.Value) ? objects[mbt2.LastWriteUserId.Value] : objects[mbt2.LastWriteUserId.Value] = FindUser2(mbt2.LastWriteUserId));
            return mbt2;
        }

        public void Insert(ManualBlockTemplate2 manualBlockTemplate2)
        {
            if (manualBlockTemplate2.Id == null) manualBlockTemplate2.Id = Guid.NewGuid();
            FolioServiceClient.InsertManualBlockTemplate(manualBlockTemplate2.ToJObject());
        }

        public void Update(ManualBlockTemplate2 manualBlockTemplate2) => FolioServiceClient.UpdateManualBlockTemplate(manualBlockTemplate2.ToJObject());

        public void DeleteManualBlockTemplate2(Guid? id) => FolioServiceClient.DeleteManualBlockTemplate(id?.ToString());

        public bool AnyMaterialType2s(string where = null) => FolioServiceClient.AnyMaterialTypes(where);

        public int CountMaterialType2s(string where = null) => FolioServiceClient.CountMaterialTypes(where);

        public MaterialType2[] MaterialType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.MaterialTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var mt2 = MaterialType2.FromJObject(jo);
                if (load && mt2.CreationUserId != null) mt2.CreationUser = (User2)(cache && objects.ContainsKey(mt2.CreationUserId.Value) ? objects[mt2.CreationUserId.Value] : objects[mt2.CreationUserId.Value] = FindUser2(mt2.CreationUserId));
                if (load && mt2.LastWriteUserId != null) mt2.LastWriteUser = (User2)(cache && objects.ContainsKey(mt2.LastWriteUserId.Value) ? objects[mt2.LastWriteUserId.Value] : objects[mt2.LastWriteUserId.Value] = FindUser2(mt2.LastWriteUserId));
                return mt2;
            }).ToArray();
        }

        public IEnumerable<MaterialType2> MaterialType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.MaterialTypes(where, orderBy, skip, take))
            {
                var mt2 = MaterialType2.FromJObject(jo);
                if (load && mt2.CreationUserId != null) mt2.CreationUser = (User2)(cache && objects.ContainsKey(mt2.CreationUserId.Value) ? objects[mt2.CreationUserId.Value] : objects[mt2.CreationUserId.Value] = FindUser2(mt2.CreationUserId));
                if (load && mt2.LastWriteUserId != null) mt2.LastWriteUser = (User2)(cache && objects.ContainsKey(mt2.LastWriteUserId.Value) ? objects[mt2.LastWriteUserId.Value] : objects[mt2.LastWriteUserId.Value] = FindUser2(mt2.LastWriteUserId));
                yield return mt2;
            }
        }

        public MaterialType2 FindMaterialType2(Guid? id, bool load = false, bool cache = true)
        {
            var mt2 = MaterialType2.FromJObject(FolioServiceClient.GetMaterialType(id?.ToString()));
            if (mt2 == null) return null;
            if (load && mt2.CreationUserId != null) mt2.CreationUser = (User2)(cache && objects.ContainsKey(mt2.CreationUserId.Value) ? objects[mt2.CreationUserId.Value] : objects[mt2.CreationUserId.Value] = FindUser2(mt2.CreationUserId));
            if (load && mt2.LastWriteUserId != null) mt2.LastWriteUser = (User2)(cache && objects.ContainsKey(mt2.LastWriteUserId.Value) ? objects[mt2.LastWriteUserId.Value] : objects[mt2.LastWriteUserId.Value] = FindUser2(mt2.LastWriteUserId));
            return mt2;
        }

        public void Insert(MaterialType2 materialType2)
        {
            if (materialType2.Id == null) materialType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertMaterialType(materialType2.ToJObject());
        }

        public void Update(MaterialType2 materialType2) => FolioServiceClient.UpdateMaterialType(materialType2.ToJObject());

        public void DeleteMaterialType2(Guid? id) => FolioServiceClient.DeleteMaterialType(id?.ToString());

        public bool AnyNatureOfContentTerm2s(string where = null) => FolioServiceClient.AnyNatureOfContentTerms(where);

        public int CountNatureOfContentTerm2s(string where = null) => FolioServiceClient.CountNatureOfContentTerms(where);

        public NatureOfContentTerm2[] NatureOfContentTerm2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.NatureOfContentTerms(out count, where, orderBy, skip, take).Select(jo =>
            {
                var noct2 = NatureOfContentTerm2.FromJObject(jo);
                if (load && noct2.CreationUserId != null) noct2.CreationUser = (User2)(cache && objects.ContainsKey(noct2.CreationUserId.Value) ? objects[noct2.CreationUserId.Value] : objects[noct2.CreationUserId.Value] = FindUser2(noct2.CreationUserId));
                if (load && noct2.LastWriteUserId != null) noct2.LastWriteUser = (User2)(cache && objects.ContainsKey(noct2.LastWriteUserId.Value) ? objects[noct2.LastWriteUserId.Value] : objects[noct2.LastWriteUserId.Value] = FindUser2(noct2.LastWriteUserId));
                return noct2;
            }).ToArray();
        }

        public IEnumerable<NatureOfContentTerm2> NatureOfContentTerm2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.NatureOfContentTerms(where, orderBy, skip, take))
            {
                var noct2 = NatureOfContentTerm2.FromJObject(jo);
                if (load && noct2.CreationUserId != null) noct2.CreationUser = (User2)(cache && objects.ContainsKey(noct2.CreationUserId.Value) ? objects[noct2.CreationUserId.Value] : objects[noct2.CreationUserId.Value] = FindUser2(noct2.CreationUserId));
                if (load && noct2.LastWriteUserId != null) noct2.LastWriteUser = (User2)(cache && objects.ContainsKey(noct2.LastWriteUserId.Value) ? objects[noct2.LastWriteUserId.Value] : objects[noct2.LastWriteUserId.Value] = FindUser2(noct2.LastWriteUserId));
                yield return noct2;
            }
        }

        public NatureOfContentTerm2 FindNatureOfContentTerm2(Guid? id, bool load = false, bool cache = true)
        {
            var noct2 = NatureOfContentTerm2.FromJObject(FolioServiceClient.GetNatureOfContentTerm(id?.ToString()));
            if (noct2 == null) return null;
            if (load && noct2.CreationUserId != null) noct2.CreationUser = (User2)(cache && objects.ContainsKey(noct2.CreationUserId.Value) ? objects[noct2.CreationUserId.Value] : objects[noct2.CreationUserId.Value] = FindUser2(noct2.CreationUserId));
            if (load && noct2.LastWriteUserId != null) noct2.LastWriteUser = (User2)(cache && objects.ContainsKey(noct2.LastWriteUserId.Value) ? objects[noct2.LastWriteUserId.Value] : objects[noct2.LastWriteUserId.Value] = FindUser2(noct2.LastWriteUserId));
            return noct2;
        }

        public void Insert(NatureOfContentTerm2 natureOfContentTerm2)
        {
            if (natureOfContentTerm2.Id == null) natureOfContentTerm2.Id = Guid.NewGuid();
            FolioServiceClient.InsertNatureOfContentTerm(natureOfContentTerm2.ToJObject());
        }

        public void Update(NatureOfContentTerm2 natureOfContentTerm2) => FolioServiceClient.UpdateNatureOfContentTerm(natureOfContentTerm2.ToJObject());

        public void DeleteNatureOfContentTerm2(Guid? id) => FolioServiceClient.DeleteNatureOfContentTerm(id?.ToString());

        public bool AnyNote3s(string where = null) => FolioServiceClient.AnyNotes(where);

        public int CountNote3s(string where = null) => FolioServiceClient.CountNotes(where);

        public Note3[] Note3s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Notes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var n3 = Note3.FromJObject(jo);
                if (load && n3.TypeId != null) n3.Type1 = (NoteType2)(cache && objects.ContainsKey(n3.TypeId.Value) ? objects[n3.TypeId.Value] : objects[n3.TypeId.Value] = FindNoteType2(n3.TypeId));
                if (load && n3.CreationUserId != null) n3.CreationUser = (User2)(cache && objects.ContainsKey(n3.CreationUserId.Value) ? objects[n3.CreationUserId.Value] : objects[n3.CreationUserId.Value] = FindUser2(n3.CreationUserId));
                if (load && n3.LastWriteUserId != null) n3.LastWriteUser = (User2)(cache && objects.ContainsKey(n3.LastWriteUserId.Value) ? objects[n3.LastWriteUserId.Value] : objects[n3.LastWriteUserId.Value] = FindUser2(n3.LastWriteUserId));
                if (load && n3.TemporaryTypeId != null) n3.TemporaryType = (NoteType2)(cache && objects.ContainsKey(n3.TemporaryTypeId.Value) ? objects[n3.TemporaryTypeId.Value] : objects[n3.TemporaryTypeId.Value] = FindNoteType2(n3.TemporaryTypeId));
                return n3;
            }).ToArray();
        }

        public IEnumerable<Note3> Note3s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Notes(where, orderBy, skip, take))
            {
                var n3 = Note3.FromJObject(jo);
                if (load && n3.TypeId != null) n3.Type1 = (NoteType2)(cache && objects.ContainsKey(n3.TypeId.Value) ? objects[n3.TypeId.Value] : objects[n3.TypeId.Value] = FindNoteType2(n3.TypeId));
                if (load && n3.CreationUserId != null) n3.CreationUser = (User2)(cache && objects.ContainsKey(n3.CreationUserId.Value) ? objects[n3.CreationUserId.Value] : objects[n3.CreationUserId.Value] = FindUser2(n3.CreationUserId));
                if (load && n3.LastWriteUserId != null) n3.LastWriteUser = (User2)(cache && objects.ContainsKey(n3.LastWriteUserId.Value) ? objects[n3.LastWriteUserId.Value] : objects[n3.LastWriteUserId.Value] = FindUser2(n3.LastWriteUserId));
                if (load && n3.TemporaryTypeId != null) n3.TemporaryType = (NoteType2)(cache && objects.ContainsKey(n3.TemporaryTypeId.Value) ? objects[n3.TemporaryTypeId.Value] : objects[n3.TemporaryTypeId.Value] = FindNoteType2(n3.TemporaryTypeId));
                yield return n3;
            }
        }

        public Note3 FindNote3(Guid? id, bool load = false, bool cache = true)
        {
            var n3 = Note3.FromJObject(FolioServiceClient.GetNote(id?.ToString()));
            if (n3 == null) return null;
            if (load && n3.TypeId != null) n3.Type1 = (NoteType2)(cache && objects.ContainsKey(n3.TypeId.Value) ? objects[n3.TypeId.Value] : objects[n3.TypeId.Value] = FindNoteType2(n3.TypeId));
            if (load && n3.CreationUserId != null) n3.CreationUser = (User2)(cache && objects.ContainsKey(n3.CreationUserId.Value) ? objects[n3.CreationUserId.Value] : objects[n3.CreationUserId.Value] = FindUser2(n3.CreationUserId));
            if (load && n3.LastWriteUserId != null) n3.LastWriteUser = (User2)(cache && objects.ContainsKey(n3.LastWriteUserId.Value) ? objects[n3.LastWriteUserId.Value] : objects[n3.LastWriteUserId.Value] = FindUser2(n3.LastWriteUserId));
            if (load && n3.TemporaryTypeId != null) n3.TemporaryType = (NoteType2)(cache && objects.ContainsKey(n3.TemporaryTypeId.Value) ? objects[n3.TemporaryTypeId.Value] : objects[n3.TemporaryTypeId.Value] = FindNoteType2(n3.TemporaryTypeId));
            var i = 0;
            if (n3.NoteLinks != null) foreach (var nl in n3.NoteLinks)
                {
                    nl.Id = (++i).ToString();
                    nl.NoteId = n3.Id;
                }
            return n3;
        }

        public void Insert(Note3 note3)
        {
            if (note3.Id == null) note3.Id = Guid.NewGuid();
            FolioServiceClient.InsertNote(note3.ToJObject());
        }

        public void Update(Note3 note3) => FolioServiceClient.UpdateNote(note3.ToJObject());

        public void DeleteNote3(Guid? id) => FolioServiceClient.DeleteNote(id?.ToString());

        public bool AnyNoteType2s(string where = null) => FolioServiceClient.AnyNoteTypes(where);

        public int CountNoteType2s(string where = null) => FolioServiceClient.CountNoteTypes(where);

        public NoteType2[] NoteType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.NoteTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var nt2 = NoteType2.FromJObject(jo);
                if (load && nt2.CreationUserId != null) nt2.CreationUser = (User2)(cache && objects.ContainsKey(nt2.CreationUserId.Value) ? objects[nt2.CreationUserId.Value] : objects[nt2.CreationUserId.Value] = FindUser2(nt2.CreationUserId));
                if (load && nt2.LastWriteUserId != null) nt2.LastWriteUser = (User2)(cache && objects.ContainsKey(nt2.LastWriteUserId.Value) ? objects[nt2.LastWriteUserId.Value] : objects[nt2.LastWriteUserId.Value] = FindUser2(nt2.LastWriteUserId));
                return nt2;
            }).ToArray();
        }

        public IEnumerable<NoteType2> NoteType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.NoteTypes(where, orderBy, skip, take))
            {
                var nt2 = NoteType2.FromJObject(jo);
                if (load && nt2.CreationUserId != null) nt2.CreationUser = (User2)(cache && objects.ContainsKey(nt2.CreationUserId.Value) ? objects[nt2.CreationUserId.Value] : objects[nt2.CreationUserId.Value] = FindUser2(nt2.CreationUserId));
                if (load && nt2.LastWriteUserId != null) nt2.LastWriteUser = (User2)(cache && objects.ContainsKey(nt2.LastWriteUserId.Value) ? objects[nt2.LastWriteUserId.Value] : objects[nt2.LastWriteUserId.Value] = FindUser2(nt2.LastWriteUserId));
                yield return nt2;
            }
        }

        public NoteType2 FindNoteType2(Guid? id, bool load = false, bool cache = true)
        {
            var nt2 = NoteType2.FromJObject(FolioServiceClient.GetNoteType(id?.ToString()));
            if (nt2 == null) return null;
            if (load && nt2.CreationUserId != null) nt2.CreationUser = (User2)(cache && objects.ContainsKey(nt2.CreationUserId.Value) ? objects[nt2.CreationUserId.Value] : objects[nt2.CreationUserId.Value] = FindUser2(nt2.CreationUserId));
            if (load && nt2.LastWriteUserId != null) nt2.LastWriteUser = (User2)(cache && objects.ContainsKey(nt2.LastWriteUserId.Value) ? objects[nt2.LastWriteUserId.Value] : objects[nt2.LastWriteUserId.Value] = FindUser2(nt2.LastWriteUserId));
            return nt2;
        }

        public void Insert(NoteType2 noteType2)
        {
            if (noteType2.Id == null) noteType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertNoteType(noteType2.ToJObject());
        }

        public void Update(NoteType2 noteType2) => FolioServiceClient.UpdateNoteType(noteType2.ToJObject());

        public void DeleteNoteType2(Guid? id) => FolioServiceClient.DeleteNoteType(id?.ToString());

        public bool AnyOrder2s(string where = null) => FolioServiceClient.AnyOrders(where);

        public int CountOrder2s(string where = null) => FolioServiceClient.CountOrders(where);

        public Order2[] Order2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Orders(out count, where, orderBy, skip, take).Select(jo =>
            {
                var o2 = Order2.FromJObject(jo);
                if (load && o2.ApprovedById != null) o2.ApprovedBy = (User2)(cache && objects.ContainsKey(o2.ApprovedById.Value) ? objects[o2.ApprovedById.Value] : objects[o2.ApprovedById.Value] = FindUser2(o2.ApprovedById));
                if (load && o2.AssignedToId != null) o2.AssignedTo = (User2)(cache && objects.ContainsKey(o2.AssignedToId.Value) ? objects[o2.AssignedToId.Value] : objects[o2.AssignedToId.Value] = FindUser2(o2.AssignedToId));
                if (load && o2.TemplateId != null) o2.Template = (OrderTemplate2)(cache && objects.ContainsKey(o2.TemplateId.Value) ? objects[o2.TemplateId.Value] : objects[o2.TemplateId.Value] = FindOrderTemplate2(o2.TemplateId));
                if (load && o2.VendorId != null) o2.Vendor = (Organization2)(cache && objects.ContainsKey(o2.VendorId.Value) ? objects[o2.VendorId.Value] : objects[o2.VendorId.Value] = FindOrganization2(o2.VendorId));
                if (load && o2.CreationUserId != null) o2.CreationUser = (User2)(cache && objects.ContainsKey(o2.CreationUserId.Value) ? objects[o2.CreationUserId.Value] : objects[o2.CreationUserId.Value] = FindUser2(o2.CreationUserId));
                if (load && o2.LastWriteUserId != null) o2.LastWriteUser = (User2)(cache && objects.ContainsKey(o2.LastWriteUserId.Value) ? objects[o2.LastWriteUserId.Value] : objects[o2.LastWriteUserId.Value] = FindUser2(o2.LastWriteUserId));
                return o2;
            }).ToArray();
        }

        public IEnumerable<Order2> Order2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Orders(where, orderBy, skip, take))
            {
                var o2 = Order2.FromJObject(jo);
                if (load && o2.ApprovedById != null) o2.ApprovedBy = (User2)(cache && objects.ContainsKey(o2.ApprovedById.Value) ? objects[o2.ApprovedById.Value] : objects[o2.ApprovedById.Value] = FindUser2(o2.ApprovedById));
                if (load && o2.AssignedToId != null) o2.AssignedTo = (User2)(cache && objects.ContainsKey(o2.AssignedToId.Value) ? objects[o2.AssignedToId.Value] : objects[o2.AssignedToId.Value] = FindUser2(o2.AssignedToId));
                if (load && o2.TemplateId != null) o2.Template = (OrderTemplate2)(cache && objects.ContainsKey(o2.TemplateId.Value) ? objects[o2.TemplateId.Value] : objects[o2.TemplateId.Value] = FindOrderTemplate2(o2.TemplateId));
                if (load && o2.VendorId != null) o2.Vendor = (Organization2)(cache && objects.ContainsKey(o2.VendorId.Value) ? objects[o2.VendorId.Value] : objects[o2.VendorId.Value] = FindOrganization2(o2.VendorId));
                if (load && o2.CreationUserId != null) o2.CreationUser = (User2)(cache && objects.ContainsKey(o2.CreationUserId.Value) ? objects[o2.CreationUserId.Value] : objects[o2.CreationUserId.Value] = FindUser2(o2.CreationUserId));
                if (load && o2.LastWriteUserId != null) o2.LastWriteUser = (User2)(cache && objects.ContainsKey(o2.LastWriteUserId.Value) ? objects[o2.LastWriteUserId.Value] : objects[o2.LastWriteUserId.Value] = FindUser2(o2.LastWriteUserId));
                yield return o2;
            }
        }

        public Order2 FindOrder2(Guid? id, bool load = false, bool cache = true)
        {
            var o2 = Order2.FromJObject(FolioServiceClient.GetOrder(id?.ToString()));
            if (o2 == null) return null;
            if (load && o2.ApprovedById != null) o2.ApprovedBy = (User2)(cache && objects.ContainsKey(o2.ApprovedById.Value) ? objects[o2.ApprovedById.Value] : objects[o2.ApprovedById.Value] = FindUser2(o2.ApprovedById));
            if (load && o2.AssignedToId != null) o2.AssignedTo = (User2)(cache && objects.ContainsKey(o2.AssignedToId.Value) ? objects[o2.AssignedToId.Value] : objects[o2.AssignedToId.Value] = FindUser2(o2.AssignedToId));
            if (load && o2.TemplateId != null) o2.Template = (OrderTemplate2)(cache && objects.ContainsKey(o2.TemplateId.Value) ? objects[o2.TemplateId.Value] : objects[o2.TemplateId.Value] = FindOrderTemplate2(o2.TemplateId));
            if (load && o2.VendorId != null) o2.Vendor = (Organization2)(cache && objects.ContainsKey(o2.VendorId.Value) ? objects[o2.VendorId.Value] : objects[o2.VendorId.Value] = FindOrganization2(o2.VendorId));
            if (load && o2.CreationUserId != null) o2.CreationUser = (User2)(cache && objects.ContainsKey(o2.CreationUserId.Value) ? objects[o2.CreationUserId.Value] : objects[o2.CreationUserId.Value] = FindUser2(o2.CreationUserId));
            if (load && o2.LastWriteUserId != null) o2.LastWriteUser = (User2)(cache && objects.ContainsKey(o2.LastWriteUserId.Value) ? objects[o2.LastWriteUserId.Value] : objects[o2.LastWriteUserId.Value] = FindUser2(o2.LastWriteUserId));
            var i = 0;
            if (o2.OrderAcquisitionsUnits != null) foreach (var oau in o2.OrderAcquisitionsUnits)
                {
                    oau.Id = (++i).ToString();
                    oau.OrderId = o2.Id;
                }
            i = 0;
            if (o2.OrderNotes != null) foreach (var @on in o2.OrderNotes)
                {
                    @on.Id = (++i).ToString();
                    @on.OrderId = o2.Id;
                }
            i = 0;
            if (o2.OrderTags != null) foreach (var ot in o2.OrderTags)
                {
                    ot.Id = (++i).ToString();
                    ot.OrderId = o2.Id;
                }
            return o2;
        }

        public void Insert(Order2 order2)
        {
            if (order2.Id == null) order2.Id = Guid.NewGuid();
            FolioServiceClient.InsertOrder(order2.ToJObject());
        }

        public void Update(Order2 order2) => FolioServiceClient.UpdateOrder(order2.ToJObject());

        public void DeleteOrder2(Guid? id) => FolioServiceClient.DeleteOrder(id?.ToString());

        public bool AnyOrderInvoice2s(string where = null) => FolioServiceClient.AnyOrderInvoices(where);

        public int CountOrderInvoice2s(string where = null) => FolioServiceClient.CountOrderInvoices(where);

        public OrderInvoice2[] OrderInvoice2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.OrderInvoices(out count, where, orderBy, skip, take).Select(jo =>
            {
                var oi2 = OrderInvoice2.FromJObject(jo);
                if (load && oi2.OrderId != null) oi2.Order = (Order2)(cache && objects.ContainsKey(oi2.OrderId.Value) ? objects[oi2.OrderId.Value] : objects[oi2.OrderId.Value] = FindOrder2(oi2.OrderId));
                if (load && oi2.InvoiceId != null) oi2.Invoice = (Invoice2)(cache && objects.ContainsKey(oi2.InvoiceId.Value) ? objects[oi2.InvoiceId.Value] : objects[oi2.InvoiceId.Value] = FindInvoice2(oi2.InvoiceId));
                return oi2;
            }).ToArray();
        }

        public IEnumerable<OrderInvoice2> OrderInvoice2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.OrderInvoices(where, orderBy, skip, take))
            {
                var oi2 = OrderInvoice2.FromJObject(jo);
                if (load && oi2.OrderId != null) oi2.Order = (Order2)(cache && objects.ContainsKey(oi2.OrderId.Value) ? objects[oi2.OrderId.Value] : objects[oi2.OrderId.Value] = FindOrder2(oi2.OrderId));
                if (load && oi2.InvoiceId != null) oi2.Invoice = (Invoice2)(cache && objects.ContainsKey(oi2.InvoiceId.Value) ? objects[oi2.InvoiceId.Value] : objects[oi2.InvoiceId.Value] = FindInvoice2(oi2.InvoiceId));
                yield return oi2;
            }
        }

        public OrderInvoice2 FindOrderInvoice2(Guid? id, bool load = false, bool cache = true)
        {
            var oi2 = OrderInvoice2.FromJObject(FolioServiceClient.GetOrderInvoice(id?.ToString()));
            if (oi2 == null) return null;
            if (load && oi2.OrderId != null) oi2.Order = (Order2)(cache && objects.ContainsKey(oi2.OrderId.Value) ? objects[oi2.OrderId.Value] : objects[oi2.OrderId.Value] = FindOrder2(oi2.OrderId));
            if (load && oi2.InvoiceId != null) oi2.Invoice = (Invoice2)(cache && objects.ContainsKey(oi2.InvoiceId.Value) ? objects[oi2.InvoiceId.Value] : objects[oi2.InvoiceId.Value] = FindInvoice2(oi2.InvoiceId));
            return oi2;
        }

        public void Insert(OrderInvoice2 orderInvoice2)
        {
            if (orderInvoice2.Id == null) orderInvoice2.Id = Guid.NewGuid();
            FolioServiceClient.InsertOrderInvoice(orderInvoice2.ToJObject());
        }

        public void Update(OrderInvoice2 orderInvoice2) => FolioServiceClient.UpdateOrderInvoice(orderInvoice2.ToJObject());

        public void DeleteOrderInvoice2(Guid? id) => FolioServiceClient.DeleteOrderInvoice(id?.ToString());

        public bool AnyOrderItem2s(string where = null) => FolioServiceClient.AnyOrderItems(where);

        public int CountOrderItem2s(string where = null) => FolioServiceClient.CountOrderItems(where);

        public OrderItem2[] OrderItem2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.OrderItems(out count, where, orderBy, skip, take).Select(jo =>
            {
                var oi2 = OrderItem2.FromJObject(jo);
                if (load && oi2.EresourceAccessProviderId != null) oi2.EresourceAccessProvider = (Organization2)(cache && objects.ContainsKey(oi2.EresourceAccessProviderId.Value) ? objects[oi2.EresourceAccessProviderId.Value] : objects[oi2.EresourceAccessProviderId.Value] = FindOrganization2(oi2.EresourceAccessProviderId));
                if (load && oi2.EresourceMaterialTypeId != null) oi2.EresourceMaterialType = (MaterialType2)(cache && objects.ContainsKey(oi2.EresourceMaterialTypeId.Value) ? objects[oi2.EresourceMaterialTypeId.Value] : objects[oi2.EresourceMaterialTypeId.Value] = FindMaterialType2(oi2.EresourceMaterialTypeId));
                if (load && oi2.InstanceId != null) oi2.Instance = (Instance2)(cache && objects.ContainsKey(oi2.InstanceId.Value) ? objects[oi2.InstanceId.Value] : objects[oi2.InstanceId.Value] = FindInstance2(oi2.InstanceId));
                if (load && oi2.PackageOrderItemId != null) oi2.PackageOrderItem = (OrderItem2)(cache && objects.ContainsKey(oi2.PackageOrderItemId.Value) ? objects[oi2.PackageOrderItemId.Value] : objects[oi2.PackageOrderItemId.Value] = FindOrderItem2(oi2.PackageOrderItemId));
                if (load && oi2.PhysicalMaterialTypeId != null) oi2.PhysicalMaterialType = (MaterialType2)(cache && objects.ContainsKey(oi2.PhysicalMaterialTypeId.Value) ? objects[oi2.PhysicalMaterialTypeId.Value] : objects[oi2.PhysicalMaterialTypeId.Value] = FindMaterialType2(oi2.PhysicalMaterialTypeId));
                if (load && oi2.PhysicalMaterialSupplierId != null) oi2.PhysicalMaterialSupplier = (Organization2)(cache && objects.ContainsKey(oi2.PhysicalMaterialSupplierId.Value) ? objects[oi2.PhysicalMaterialSupplierId.Value] : objects[oi2.PhysicalMaterialSupplierId.Value] = FindOrganization2(oi2.PhysicalMaterialSupplierId));
                if (load && oi2.OrderId != null) oi2.Order = (Order2)(cache && objects.ContainsKey(oi2.OrderId.Value) ? objects[oi2.OrderId.Value] : objects[oi2.OrderId.Value] = FindOrder2(oi2.OrderId));
                if (load && oi2.CreationUserId != null) oi2.CreationUser = (User2)(cache && objects.ContainsKey(oi2.CreationUserId.Value) ? objects[oi2.CreationUserId.Value] : objects[oi2.CreationUserId.Value] = FindUser2(oi2.CreationUserId));
                if (load && oi2.LastWriteUserId != null) oi2.LastWriteUser = (User2)(cache && objects.ContainsKey(oi2.LastWriteUserId.Value) ? objects[oi2.LastWriteUserId.Value] : objects[oi2.LastWriteUserId.Value] = FindUser2(oi2.LastWriteUserId));
                return oi2;
            }).ToArray();
        }

        public IEnumerable<OrderItem2> OrderItem2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.OrderItems(where, orderBy, skip, take))
            {
                var oi2 = OrderItem2.FromJObject(jo);
                if (load && oi2.EresourceAccessProviderId != null) oi2.EresourceAccessProvider = (Organization2)(cache && objects.ContainsKey(oi2.EresourceAccessProviderId.Value) ? objects[oi2.EresourceAccessProviderId.Value] : objects[oi2.EresourceAccessProviderId.Value] = FindOrganization2(oi2.EresourceAccessProviderId));
                if (load && oi2.EresourceMaterialTypeId != null) oi2.EresourceMaterialType = (MaterialType2)(cache && objects.ContainsKey(oi2.EresourceMaterialTypeId.Value) ? objects[oi2.EresourceMaterialTypeId.Value] : objects[oi2.EresourceMaterialTypeId.Value] = FindMaterialType2(oi2.EresourceMaterialTypeId));
                if (load && oi2.InstanceId != null) oi2.Instance = (Instance2)(cache && objects.ContainsKey(oi2.InstanceId.Value) ? objects[oi2.InstanceId.Value] : objects[oi2.InstanceId.Value] = FindInstance2(oi2.InstanceId));
                if (load && oi2.PackageOrderItemId != null) oi2.PackageOrderItem = (OrderItem2)(cache && objects.ContainsKey(oi2.PackageOrderItemId.Value) ? objects[oi2.PackageOrderItemId.Value] : objects[oi2.PackageOrderItemId.Value] = FindOrderItem2(oi2.PackageOrderItemId));
                if (load && oi2.PhysicalMaterialTypeId != null) oi2.PhysicalMaterialType = (MaterialType2)(cache && objects.ContainsKey(oi2.PhysicalMaterialTypeId.Value) ? objects[oi2.PhysicalMaterialTypeId.Value] : objects[oi2.PhysicalMaterialTypeId.Value] = FindMaterialType2(oi2.PhysicalMaterialTypeId));
                if (load && oi2.PhysicalMaterialSupplierId != null) oi2.PhysicalMaterialSupplier = (Organization2)(cache && objects.ContainsKey(oi2.PhysicalMaterialSupplierId.Value) ? objects[oi2.PhysicalMaterialSupplierId.Value] : objects[oi2.PhysicalMaterialSupplierId.Value] = FindOrganization2(oi2.PhysicalMaterialSupplierId));
                if (load && oi2.OrderId != null) oi2.Order = (Order2)(cache && objects.ContainsKey(oi2.OrderId.Value) ? objects[oi2.OrderId.Value] : objects[oi2.OrderId.Value] = FindOrder2(oi2.OrderId));
                if (load && oi2.CreationUserId != null) oi2.CreationUser = (User2)(cache && objects.ContainsKey(oi2.CreationUserId.Value) ? objects[oi2.CreationUserId.Value] : objects[oi2.CreationUserId.Value] = FindUser2(oi2.CreationUserId));
                if (load && oi2.LastWriteUserId != null) oi2.LastWriteUser = (User2)(cache && objects.ContainsKey(oi2.LastWriteUserId.Value) ? objects[oi2.LastWriteUserId.Value] : objects[oi2.LastWriteUserId.Value] = FindUser2(oi2.LastWriteUserId));
                yield return oi2;
            }
        }

        public OrderItem2 FindOrderItem2(Guid? id, bool load = false, bool cache = true)
        {
            var oi2 = OrderItem2.FromJObject(FolioServiceClient.GetOrderItem(id?.ToString()));
            if (oi2 == null) return null;
            if (load && oi2.EresourceAccessProviderId != null) oi2.EresourceAccessProvider = (Organization2)(cache && objects.ContainsKey(oi2.EresourceAccessProviderId.Value) ? objects[oi2.EresourceAccessProviderId.Value] : objects[oi2.EresourceAccessProviderId.Value] = FindOrganization2(oi2.EresourceAccessProviderId));
            if (load && oi2.EresourceMaterialTypeId != null) oi2.EresourceMaterialType = (MaterialType2)(cache && objects.ContainsKey(oi2.EresourceMaterialTypeId.Value) ? objects[oi2.EresourceMaterialTypeId.Value] : objects[oi2.EresourceMaterialTypeId.Value] = FindMaterialType2(oi2.EresourceMaterialTypeId));
            if (load && oi2.InstanceId != null) oi2.Instance = (Instance2)(cache && objects.ContainsKey(oi2.InstanceId.Value) ? objects[oi2.InstanceId.Value] : objects[oi2.InstanceId.Value] = FindInstance2(oi2.InstanceId));
            if (load && oi2.PackageOrderItemId != null) oi2.PackageOrderItem = (OrderItem2)(cache && objects.ContainsKey(oi2.PackageOrderItemId.Value) ? objects[oi2.PackageOrderItemId.Value] : objects[oi2.PackageOrderItemId.Value] = FindOrderItem2(oi2.PackageOrderItemId));
            if (load && oi2.PhysicalMaterialTypeId != null) oi2.PhysicalMaterialType = (MaterialType2)(cache && objects.ContainsKey(oi2.PhysicalMaterialTypeId.Value) ? objects[oi2.PhysicalMaterialTypeId.Value] : objects[oi2.PhysicalMaterialTypeId.Value] = FindMaterialType2(oi2.PhysicalMaterialTypeId));
            if (load && oi2.PhysicalMaterialSupplierId != null) oi2.PhysicalMaterialSupplier = (Organization2)(cache && objects.ContainsKey(oi2.PhysicalMaterialSupplierId.Value) ? objects[oi2.PhysicalMaterialSupplierId.Value] : objects[oi2.PhysicalMaterialSupplierId.Value] = FindOrganization2(oi2.PhysicalMaterialSupplierId));
            if (load && oi2.OrderId != null) oi2.Order = (Order2)(cache && objects.ContainsKey(oi2.OrderId.Value) ? objects[oi2.OrderId.Value] : objects[oi2.OrderId.Value] = FindOrder2(oi2.OrderId));
            if (load && oi2.CreationUserId != null) oi2.CreationUser = (User2)(cache && objects.ContainsKey(oi2.CreationUserId.Value) ? objects[oi2.CreationUserId.Value] : objects[oi2.CreationUserId.Value] = FindUser2(oi2.CreationUserId));
            if (load && oi2.LastWriteUserId != null) oi2.LastWriteUser = (User2)(cache && objects.ContainsKey(oi2.LastWriteUserId.Value) ? objects[oi2.LastWriteUserId.Value] : objects[oi2.LastWriteUserId.Value] = FindUser2(oi2.LastWriteUserId));
            var i = 0;
            if (oi2.OrderItemAlerts != null) foreach (var oia in oi2.OrderItemAlerts)
                {
                    oia.Id = (++i).ToString();
                    oia.OrderItemId = oi2.Id;
                }
            i = 0;
            if (oi2.OrderItemClaims != null) foreach (var oic in oi2.OrderItemClaims)
                {
                    oic.Id = (++i).ToString();
                    oic.OrderItemId = oi2.Id;
                }
            i = 0;
            if (oi2.OrderItemContributors != null) foreach (var oic2 in oi2.OrderItemContributors)
                {
                    oic2.Id = (++i).ToString();
                    oic2.OrderItemId = oi2.Id;
                }
            i = 0;
            if (oi2.OrderItemFunds != null) foreach (var oif in oi2.OrderItemFunds)
                {
                    oif.Id = (++i).ToString();
                    oif.OrderItemId = oi2.Id;
                }
            i = 0;
            if (oi2.OrderItemLocation2s != null) foreach (var oil2 in oi2.OrderItemLocation2s)
                {
                    oil2.Id = (++i).ToString();
                    oil2.OrderItemId = oi2.Id;
                }
            i = 0;
            if (oi2.OrderItemProductIds != null) foreach (var oipi in oi2.OrderItemProductIds)
                {
                    oipi.Id = (++i).ToString();
                    oipi.OrderItemId = oi2.Id;
                }
            i = 0;
            if (oi2.OrderItemReferenceNumbers != null) foreach (var oirn in oi2.OrderItemReferenceNumbers)
                {
                    oirn.Id = (++i).ToString();
                    oirn.OrderItemId = oi2.Id;
                }
            i = 0;
            if (oi2.OrderItemReportingCodes != null) foreach (var oirc in oi2.OrderItemReportingCodes)
                {
                    oirc.Id = (++i).ToString();
                    oirc.OrderItemId = oi2.Id;
                }
            i = 0;
            if (oi2.OrderItemTags != null) foreach (var oit in oi2.OrderItemTags)
                {
                    oit.Id = (++i).ToString();
                    oit.OrderItemId = oi2.Id;
                }
            i = 0;
            if (oi2.OrderItemVolumes != null) foreach (var oiv in oi2.OrderItemVolumes)
                {
                    oiv.Id = (++i).ToString();
                    oiv.OrderItemId = oi2.Id;
                }
            return oi2;
        }

        public void Insert(OrderItem2 orderItem2)
        {
            if (orderItem2.Id == null) orderItem2.Id = Guid.NewGuid();
            FolioServiceClient.InsertOrderItem(orderItem2.ToJObject());
        }

        public void Update(OrderItem2 orderItem2) => FolioServiceClient.UpdateOrderItem(orderItem2.ToJObject());

        public void DeleteOrderItem2(Guid? id) => FolioServiceClient.DeleteOrderItem(id?.ToString());

        public bool AnyOrderTemplate2s(string where = null) => FolioServiceClient.AnyOrderTemplates(where);

        public int CountOrderTemplate2s(string where = null) => FolioServiceClient.CountOrderTemplates(where);

        public OrderTemplate2[] OrderTemplate2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.OrderTemplates(out count, where, orderBy, skip, take).Select(jo =>
            {
                var ot2 = OrderTemplate2.FromJObject(jo);
                return ot2;
            }).ToArray();
        }

        public IEnumerable<OrderTemplate2> OrderTemplate2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.OrderTemplates(where, orderBy, skip, take))
            {
                var ot2 = OrderTemplate2.FromJObject(jo);
                yield return ot2;
            }
        }

        public OrderTemplate2 FindOrderTemplate2(Guid? id, bool load = false, bool cache = true) => OrderTemplate2.FromJObject(FolioServiceClient.GetOrderTemplate(id?.ToString()));

        public void Insert(OrderTemplate2 orderTemplate2)
        {
            if (orderTemplate2.Id == null) orderTemplate2.Id = Guid.NewGuid();
            FolioServiceClient.InsertOrderTemplate(orderTemplate2.ToJObject());
        }

        public void Update(OrderTemplate2 orderTemplate2) => FolioServiceClient.UpdateOrderTemplate(orderTemplate2.ToJObject());

        public void DeleteOrderTemplate2(Guid? id) => FolioServiceClient.DeleteOrderTemplate(id?.ToString());

        public OrderTransactionSummary2 FindOrderTransactionSummary2(Guid? id, bool load = false, bool cache = true)
        {
            var ots2 = OrderTransactionSummary2.FromJObject(FolioServiceClient.GetOrderTransactionSummary(id?.ToString()));
            if (ots2 == null) return null;
            if (load && ots2.Id != null) ots2.Order2 = (Order2)(cache && objects.ContainsKey(ots2.Id.Value) ? objects[ots2.Id.Value] : objects[ots2.Id.Value] = FindOrder2(ots2.Id));
            return ots2;
        }

        public void Insert(OrderTransactionSummary2 orderTransactionSummary2)
        {
            if (orderTransactionSummary2.Id == null) orderTransactionSummary2.Id = Guid.NewGuid();
            FolioServiceClient.InsertOrderTransactionSummary(orderTransactionSummary2.ToJObject());
        }

        public void Update(OrderTransactionSummary2 orderTransactionSummary2) => FolioServiceClient.UpdateOrderTransactionSummary(orderTransactionSummary2.ToJObject());

        public void DeleteOrderTransactionSummary2(Guid? id) => FolioServiceClient.DeleteOrderTransactionSummary(id?.ToString());

        public bool AnyOrganization2s(string where = null) => FolioServiceClient.AnyOrganizations(where);

        public int CountOrganization2s(string where = null) => FolioServiceClient.CountOrganizations(where);

        public Organization2[] Organization2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Organizations(out count, where, orderBy, skip, take).Select(jo =>
            {
                var o2 = Organization2.FromJObject(jo);
                if (load && o2.CreationUserId != null) o2.CreationUser = (User2)(cache && objects.ContainsKey(o2.CreationUserId.Value) ? objects[o2.CreationUserId.Value] : objects[o2.CreationUserId.Value] = FindUser2(o2.CreationUserId));
                if (load && o2.LastWriteUserId != null) o2.LastWriteUser = (User2)(cache && objects.ContainsKey(o2.LastWriteUserId.Value) ? objects[o2.LastWriteUserId.Value] : objects[o2.LastWriteUserId.Value] = FindUser2(o2.LastWriteUserId));
                return o2;
            }).ToArray();
        }

        public IEnumerable<Organization2> Organization2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Organizations(where, orderBy, skip, take))
            {
                var o2 = Organization2.FromJObject(jo);
                if (load && o2.CreationUserId != null) o2.CreationUser = (User2)(cache && objects.ContainsKey(o2.CreationUserId.Value) ? objects[o2.CreationUserId.Value] : objects[o2.CreationUserId.Value] = FindUser2(o2.CreationUserId));
                if (load && o2.LastWriteUserId != null) o2.LastWriteUser = (User2)(cache && objects.ContainsKey(o2.LastWriteUserId.Value) ? objects[o2.LastWriteUserId.Value] : objects[o2.LastWriteUserId.Value] = FindUser2(o2.LastWriteUserId));
                yield return o2;
            }
        }

        public Organization2 FindOrganization2(Guid? id, bool load = false, bool cache = true)
        {
            var o2 = Organization2.FromJObject(FolioServiceClient.GetOrganization(id?.ToString()));
            if (o2 == null) return null;
            if (load && o2.CreationUserId != null) o2.CreationUser = (User2)(cache && objects.ContainsKey(o2.CreationUserId.Value) ? objects[o2.CreationUserId.Value] : objects[o2.CreationUserId.Value] = FindUser2(o2.CreationUserId));
            if (load && o2.LastWriteUserId != null) o2.LastWriteUser = (User2)(cache && objects.ContainsKey(o2.LastWriteUserId.Value) ? objects[o2.LastWriteUserId.Value] : objects[o2.LastWriteUserId.Value] = FindUser2(o2.LastWriteUserId));
            var i = 0;
            if (o2.Currencies != null) foreach (var c in o2.Currencies)
                {
                    c.Id = (++i).ToString();
                    c.OrganizationId = o2.Id;
                }
            i = 0;
            if (o2.OrganizationAccounts != null) foreach (var oa in o2.OrganizationAccounts)
                {
                    oa.Id = (++i).ToString();
                    oa.OrganizationId = o2.Id;
                }
            i = 0;
            if (o2.OrganizationAcquisitionsUnits != null) foreach (var oau in o2.OrganizationAcquisitionsUnits)
                {
                    oau.Id = (++i).ToString();
                    oau.OrganizationId = o2.Id;
                }
            i = 0;
            if (o2.OrganizationAddresses != null) foreach (var oa2 in o2.OrganizationAddresses)
                {
                    oa2.Id = (++i).ToString();
                    oa2.OrganizationId = o2.Id;
                }
            i = 0;
            if (o2.OrganizationAgreements != null) foreach (var oa3 in o2.OrganizationAgreements)
                {
                    oa3.Id = (++i).ToString();
                    oa3.OrganizationId = o2.Id;
                }
            i = 0;
            if (o2.OrganizationAliases != null) foreach (var oa4 in o2.OrganizationAliases)
                {
                    oa4.Id = (++i).ToString();
                    oa4.OrganizationId = o2.Id;
                }
            i = 0;
            if (o2.OrganizationChangelogs != null) foreach (var oc in o2.OrganizationChangelogs)
                {
                    oc.Id = (++i).ToString();
                    oc.OrganizationId = o2.Id;
                }
            i = 0;
            if (o2.OrganizationContacts != null) foreach (var oc2 in o2.OrganizationContacts)
                {
                    oc2.Id = (++i).ToString();
                    oc2.OrganizationId = o2.Id;
                }
            i = 0;
            if (o2.OrganizationEmails != null) foreach (var oe in o2.OrganizationEmails)
                {
                    oe.Id = (++i).ToString();
                    oe.OrganizationId = o2.Id;
                }
            i = 0;
            if (o2.OrganizationInterfaces != null) foreach (var oi in o2.OrganizationInterfaces)
                {
                    oi.Id = (++i).ToString();
                    oi.OrganizationId = o2.Id;
                }
            i = 0;
            if (o2.OrganizationPhoneNumbers != null) foreach (var opn in o2.OrganizationPhoneNumbers)
                {
                    opn.Id = (++i).ToString();
                    opn.OrganizationId = o2.Id;
                }
            i = 0;
            if (o2.OrganizationTags != null) foreach (var ot in o2.OrganizationTags)
                {
                    ot.Id = (++i).ToString();
                    ot.OrganizationId = o2.Id;
                }
            i = 0;
            if (o2.OrganizationUrls != null) foreach (var ou in o2.OrganizationUrls)
                {
                    ou.Id = (++i).ToString();
                    ou.OrganizationId = o2.Id;
                }
            return o2;
        }

        public void Insert(Organization2 organization2)
        {
            if (organization2.Id == null) organization2.Id = Guid.NewGuid();
            FolioServiceClient.InsertOrganization(organization2.ToJObject());
        }

        public void Update(Organization2 organization2) => FolioServiceClient.UpdateOrganization(organization2.ToJObject());

        public void DeleteOrganization2(Guid? id) => FolioServiceClient.DeleteOrganization(id?.ToString());

        public bool AnyOverdueFinePolicy2s(string where = null) => FolioServiceClient.AnyOverdueFinePolicies(where);

        public int CountOverdueFinePolicy2s(string where = null) => FolioServiceClient.CountOverdueFinePolicies(where);

        public OverdueFinePolicy2[] OverdueFinePolicy2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.OverdueFinePolicies(out count, where, orderBy, skip, take).Select(jo =>
            {
                var ofp2 = OverdueFinePolicy2.FromJObject(jo);
                if (load && ofp2.CreationUserId != null) ofp2.CreationUser = (User2)(cache && objects.ContainsKey(ofp2.CreationUserId.Value) ? objects[ofp2.CreationUserId.Value] : objects[ofp2.CreationUserId.Value] = FindUser2(ofp2.CreationUserId));
                if (load && ofp2.LastWriteUserId != null) ofp2.LastWriteUser = (User2)(cache && objects.ContainsKey(ofp2.LastWriteUserId.Value) ? objects[ofp2.LastWriteUserId.Value] : objects[ofp2.LastWriteUserId.Value] = FindUser2(ofp2.LastWriteUserId));
                return ofp2;
            }).ToArray();
        }

        public IEnumerable<OverdueFinePolicy2> OverdueFinePolicy2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.OverdueFinePolicies(where, orderBy, skip, take))
            {
                var ofp2 = OverdueFinePolicy2.FromJObject(jo);
                if (load && ofp2.CreationUserId != null) ofp2.CreationUser = (User2)(cache && objects.ContainsKey(ofp2.CreationUserId.Value) ? objects[ofp2.CreationUserId.Value] : objects[ofp2.CreationUserId.Value] = FindUser2(ofp2.CreationUserId));
                if (load && ofp2.LastWriteUserId != null) ofp2.LastWriteUser = (User2)(cache && objects.ContainsKey(ofp2.LastWriteUserId.Value) ? objects[ofp2.LastWriteUserId.Value] : objects[ofp2.LastWriteUserId.Value] = FindUser2(ofp2.LastWriteUserId));
                yield return ofp2;
            }
        }

        public OverdueFinePolicy2 FindOverdueFinePolicy2(Guid? id, bool load = false, bool cache = true)
        {
            var ofp2 = OverdueFinePolicy2.FromJObject(FolioServiceClient.GetOverdueFinePolicy(id?.ToString()));
            if (ofp2 == null) return null;
            if (load && ofp2.CreationUserId != null) ofp2.CreationUser = (User2)(cache && objects.ContainsKey(ofp2.CreationUserId.Value) ? objects[ofp2.CreationUserId.Value] : objects[ofp2.CreationUserId.Value] = FindUser2(ofp2.CreationUserId));
            if (load && ofp2.LastWriteUserId != null) ofp2.LastWriteUser = (User2)(cache && objects.ContainsKey(ofp2.LastWriteUserId.Value) ? objects[ofp2.LastWriteUserId.Value] : objects[ofp2.LastWriteUserId.Value] = FindUser2(ofp2.LastWriteUserId));
            return ofp2;
        }

        public void Insert(OverdueFinePolicy2 overdueFinePolicy2)
        {
            if (overdueFinePolicy2.Id == null) overdueFinePolicy2.Id = Guid.NewGuid();
            FolioServiceClient.InsertOverdueFinePolicy(overdueFinePolicy2.ToJObject());
        }

        public void Update(OverdueFinePolicy2 overdueFinePolicy2) => FolioServiceClient.UpdateOverdueFinePolicy(overdueFinePolicy2.ToJObject());

        public void DeleteOverdueFinePolicy2(Guid? id) => FolioServiceClient.DeleteOverdueFinePolicy(id?.ToString());

        public bool AnyOwner2s(string where = null) => FolioServiceClient.AnyOwners(where);

        public int CountOwner2s(string where = null) => FolioServiceClient.CountOwners(where);

        public Owner2[] Owner2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Owners(out count, where, orderBy, skip, take).Select(jo =>
            {
                var o2 = Owner2.FromJObject(jo);
                if (load && o2.DefaultChargeNoticeId != null) o2.DefaultChargeNotice = (Template2)(cache && objects.ContainsKey(o2.DefaultChargeNoticeId.Value) ? objects[o2.DefaultChargeNoticeId.Value] : objects[o2.DefaultChargeNoticeId.Value] = FindTemplate2(o2.DefaultChargeNoticeId));
                if (load && o2.DefaultActionNoticeId != null) o2.DefaultActionNotice = (Template2)(cache && objects.ContainsKey(o2.DefaultActionNoticeId.Value) ? objects[o2.DefaultActionNoticeId.Value] : objects[o2.DefaultActionNoticeId.Value] = FindTemplate2(o2.DefaultActionNoticeId));
                if (load && o2.CreationUserId != null) o2.CreationUser = (User2)(cache && objects.ContainsKey(o2.CreationUserId.Value) ? objects[o2.CreationUserId.Value] : objects[o2.CreationUserId.Value] = FindUser2(o2.CreationUserId));
                if (load && o2.LastWriteUserId != null) o2.LastWriteUser = (User2)(cache && objects.ContainsKey(o2.LastWriteUserId.Value) ? objects[o2.LastWriteUserId.Value] : objects[o2.LastWriteUserId.Value] = FindUser2(o2.LastWriteUserId));
                return o2;
            }).ToArray();
        }

        public IEnumerable<Owner2> Owner2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Owners(where, orderBy, skip, take))
            {
                var o2 = Owner2.FromJObject(jo);
                if (load && o2.DefaultChargeNoticeId != null) o2.DefaultChargeNotice = (Template2)(cache && objects.ContainsKey(o2.DefaultChargeNoticeId.Value) ? objects[o2.DefaultChargeNoticeId.Value] : objects[o2.DefaultChargeNoticeId.Value] = FindTemplate2(o2.DefaultChargeNoticeId));
                if (load && o2.DefaultActionNoticeId != null) o2.DefaultActionNotice = (Template2)(cache && objects.ContainsKey(o2.DefaultActionNoticeId.Value) ? objects[o2.DefaultActionNoticeId.Value] : objects[o2.DefaultActionNoticeId.Value] = FindTemplate2(o2.DefaultActionNoticeId));
                if (load && o2.CreationUserId != null) o2.CreationUser = (User2)(cache && objects.ContainsKey(o2.CreationUserId.Value) ? objects[o2.CreationUserId.Value] : objects[o2.CreationUserId.Value] = FindUser2(o2.CreationUserId));
                if (load && o2.LastWriteUserId != null) o2.LastWriteUser = (User2)(cache && objects.ContainsKey(o2.LastWriteUserId.Value) ? objects[o2.LastWriteUserId.Value] : objects[o2.LastWriteUserId.Value] = FindUser2(o2.LastWriteUserId));
                yield return o2;
            }
        }

        public Owner2 FindOwner2(Guid? id, bool load = false, bool cache = true)
        {
            var o2 = Owner2.FromJObject(FolioServiceClient.GetOwner(id?.ToString()));
            if (o2 == null) return null;
            if (load && o2.DefaultChargeNoticeId != null) o2.DefaultChargeNotice = (Template2)(cache && objects.ContainsKey(o2.DefaultChargeNoticeId.Value) ? objects[o2.DefaultChargeNoticeId.Value] : objects[o2.DefaultChargeNoticeId.Value] = FindTemplate2(o2.DefaultChargeNoticeId));
            if (load && o2.DefaultActionNoticeId != null) o2.DefaultActionNotice = (Template2)(cache && objects.ContainsKey(o2.DefaultActionNoticeId.Value) ? objects[o2.DefaultActionNoticeId.Value] : objects[o2.DefaultActionNoticeId.Value] = FindTemplate2(o2.DefaultActionNoticeId));
            if (load && o2.CreationUserId != null) o2.CreationUser = (User2)(cache && objects.ContainsKey(o2.CreationUserId.Value) ? objects[o2.CreationUserId.Value] : objects[o2.CreationUserId.Value] = FindUser2(o2.CreationUserId));
            if (load && o2.LastWriteUserId != null) o2.LastWriteUser = (User2)(cache && objects.ContainsKey(o2.LastWriteUserId.Value) ? objects[o2.LastWriteUserId.Value] : objects[o2.LastWriteUserId.Value] = FindUser2(o2.LastWriteUserId));
            var i = 0;
            if (o2.ServicePointOwners != null) foreach (var spo in o2.ServicePointOwners)
                {
                    spo.Id = (++i).ToString();
                    spo.OwnerId = o2.Id;
                }
            return o2;
        }

        public void Insert(Owner2 owner2)
        {
            if (owner2.Id == null) owner2.Id = Guid.NewGuid();
            FolioServiceClient.InsertOwner(owner2.ToJObject());
        }

        public void Update(Owner2 owner2) => FolioServiceClient.UpdateOwner(owner2.ToJObject());

        public void DeleteOwner2(Guid? id) => FolioServiceClient.DeleteOwner(id?.ToString());

        public bool AnyPatronActionSession2s(string where = null) => FolioServiceClient.AnyPatronActionSessions(where);

        public int CountPatronActionSession2s(string where = null) => FolioServiceClient.CountPatronActionSessions(where);

        public PatronActionSession2[] PatronActionSession2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.PatronActionSessions(out count, where, orderBy, skip, take).Select(jo =>
            {
                var pas2 = PatronActionSession2.FromJObject(jo);
                if (load && pas2.PatronId != null) pas2.Patron = (User2)(cache && objects.ContainsKey(pas2.PatronId.Value) ? objects[pas2.PatronId.Value] : objects[pas2.PatronId.Value] = FindUser2(pas2.PatronId));
                if (load && pas2.LoanId != null) pas2.Loan = (Loan2)(cache && objects.ContainsKey(pas2.LoanId.Value) ? objects[pas2.LoanId.Value] : objects[pas2.LoanId.Value] = FindLoan2(pas2.LoanId));
                if (load && pas2.CreationUserId != null) pas2.CreationUser = (User2)(cache && objects.ContainsKey(pas2.CreationUserId.Value) ? objects[pas2.CreationUserId.Value] : objects[pas2.CreationUserId.Value] = FindUser2(pas2.CreationUserId));
                if (load && pas2.LastWriteUserId != null) pas2.LastWriteUser = (User2)(cache && objects.ContainsKey(pas2.LastWriteUserId.Value) ? objects[pas2.LastWriteUserId.Value] : objects[pas2.LastWriteUserId.Value] = FindUser2(pas2.LastWriteUserId));
                return pas2;
            }).ToArray();
        }

        public IEnumerable<PatronActionSession2> PatronActionSession2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.PatronActionSessions(where, orderBy, skip, take))
            {
                var pas2 = PatronActionSession2.FromJObject(jo);
                if (load && pas2.PatronId != null) pas2.Patron = (User2)(cache && objects.ContainsKey(pas2.PatronId.Value) ? objects[pas2.PatronId.Value] : objects[pas2.PatronId.Value] = FindUser2(pas2.PatronId));
                if (load && pas2.LoanId != null) pas2.Loan = (Loan2)(cache && objects.ContainsKey(pas2.LoanId.Value) ? objects[pas2.LoanId.Value] : objects[pas2.LoanId.Value] = FindLoan2(pas2.LoanId));
                if (load && pas2.CreationUserId != null) pas2.CreationUser = (User2)(cache && objects.ContainsKey(pas2.CreationUserId.Value) ? objects[pas2.CreationUserId.Value] : objects[pas2.CreationUserId.Value] = FindUser2(pas2.CreationUserId));
                if (load && pas2.LastWriteUserId != null) pas2.LastWriteUser = (User2)(cache && objects.ContainsKey(pas2.LastWriteUserId.Value) ? objects[pas2.LastWriteUserId.Value] : objects[pas2.LastWriteUserId.Value] = FindUser2(pas2.LastWriteUserId));
                yield return pas2;
            }
        }

        public PatronActionSession2 FindPatronActionSession2(Guid? id, bool load = false, bool cache = true)
        {
            var pas2 = PatronActionSession2.FromJObject(FolioServiceClient.GetPatronActionSession(id?.ToString()));
            if (pas2 == null) return null;
            if (load && pas2.PatronId != null) pas2.Patron = (User2)(cache && objects.ContainsKey(pas2.PatronId.Value) ? objects[pas2.PatronId.Value] : objects[pas2.PatronId.Value] = FindUser2(pas2.PatronId));
            if (load && pas2.LoanId != null) pas2.Loan = (Loan2)(cache && objects.ContainsKey(pas2.LoanId.Value) ? objects[pas2.LoanId.Value] : objects[pas2.LoanId.Value] = FindLoan2(pas2.LoanId));
            if (load && pas2.CreationUserId != null) pas2.CreationUser = (User2)(cache && objects.ContainsKey(pas2.CreationUserId.Value) ? objects[pas2.CreationUserId.Value] : objects[pas2.CreationUserId.Value] = FindUser2(pas2.CreationUserId));
            if (load && pas2.LastWriteUserId != null) pas2.LastWriteUser = (User2)(cache && objects.ContainsKey(pas2.LastWriteUserId.Value) ? objects[pas2.LastWriteUserId.Value] : objects[pas2.LastWriteUserId.Value] = FindUser2(pas2.LastWriteUserId));
            return pas2;
        }

        public void Insert(PatronActionSession2 patronActionSession2)
        {
            if (patronActionSession2.Id == null) patronActionSession2.Id = Guid.NewGuid();
            FolioServiceClient.InsertPatronActionSession(patronActionSession2.ToJObject());
        }

        public void Update(PatronActionSession2 patronActionSession2) => FolioServiceClient.UpdatePatronActionSession(patronActionSession2.ToJObject());

        public void DeletePatronActionSession2(Guid? id) => FolioServiceClient.DeletePatronActionSession(id?.ToString());

        public bool AnyPatronNoticePolicy2s(string where = null) => FolioServiceClient.AnyPatronNoticePolicies(where);

        public int CountPatronNoticePolicy2s(string where = null) => FolioServiceClient.CountPatronNoticePolicies(where);

        public PatronNoticePolicy2[] PatronNoticePolicy2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.PatronNoticePolicies(out count, where, orderBy, skip, take).Select(jo =>
            {
                var pnp2 = PatronNoticePolicy2.FromJObject(jo);
                if (load && pnp2.CreationUserId != null) pnp2.CreationUser = (User2)(cache && objects.ContainsKey(pnp2.CreationUserId.Value) ? objects[pnp2.CreationUserId.Value] : objects[pnp2.CreationUserId.Value] = FindUser2(pnp2.CreationUserId));
                if (load && pnp2.LastWriteUserId != null) pnp2.LastWriteUser = (User2)(cache && objects.ContainsKey(pnp2.LastWriteUserId.Value) ? objects[pnp2.LastWriteUserId.Value] : objects[pnp2.LastWriteUserId.Value] = FindUser2(pnp2.LastWriteUserId));
                return pnp2;
            }).ToArray();
        }

        public IEnumerable<PatronNoticePolicy2> PatronNoticePolicy2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.PatronNoticePolicies(where, orderBy, skip, take))
            {
                var pnp2 = PatronNoticePolicy2.FromJObject(jo);
                if (load && pnp2.CreationUserId != null) pnp2.CreationUser = (User2)(cache && objects.ContainsKey(pnp2.CreationUserId.Value) ? objects[pnp2.CreationUserId.Value] : objects[pnp2.CreationUserId.Value] = FindUser2(pnp2.CreationUserId));
                if (load && pnp2.LastWriteUserId != null) pnp2.LastWriteUser = (User2)(cache && objects.ContainsKey(pnp2.LastWriteUserId.Value) ? objects[pnp2.LastWriteUserId.Value] : objects[pnp2.LastWriteUserId.Value] = FindUser2(pnp2.LastWriteUserId));
                yield return pnp2;
            }
        }

        public PatronNoticePolicy2 FindPatronNoticePolicy2(Guid? id, bool load = false, bool cache = true)
        {
            var pnp2 = PatronNoticePolicy2.FromJObject(FolioServiceClient.GetPatronNoticePolicy(id?.ToString()));
            if (pnp2 == null) return null;
            if (load && pnp2.CreationUserId != null) pnp2.CreationUser = (User2)(cache && objects.ContainsKey(pnp2.CreationUserId.Value) ? objects[pnp2.CreationUserId.Value] : objects[pnp2.CreationUserId.Value] = FindUser2(pnp2.CreationUserId));
            if (load && pnp2.LastWriteUserId != null) pnp2.LastWriteUser = (User2)(cache && objects.ContainsKey(pnp2.LastWriteUserId.Value) ? objects[pnp2.LastWriteUserId.Value] : objects[pnp2.LastWriteUserId.Value] = FindUser2(pnp2.LastWriteUserId));
            var i = 0;
            if (pnp2.PatronNoticePolicyFeeFineNotices != null) foreach (var pnpffn in pnp2.PatronNoticePolicyFeeFineNotices)
                {
                    pnpffn.Id = (++i).ToString();
                    pnpffn.PatronNoticePolicyId = pnp2.Id;
                }
            i = 0;
            if (pnp2.PatronNoticePolicyLoanNotices != null) foreach (var pnpln in pnp2.PatronNoticePolicyLoanNotices)
                {
                    pnpln.Id = (++i).ToString();
                    pnpln.PatronNoticePolicyId = pnp2.Id;
                }
            i = 0;
            if (pnp2.PatronNoticePolicyRequestNotices != null) foreach (var pnprn in pnp2.PatronNoticePolicyRequestNotices)
                {
                    pnprn.Id = (++i).ToString();
                    pnprn.PatronNoticePolicyId = pnp2.Id;
                }
            return pnp2;
        }

        public void Insert(PatronNoticePolicy2 patronNoticePolicy2)
        {
            if (patronNoticePolicy2.Id == null) patronNoticePolicy2.Id = Guid.NewGuid();
            FolioServiceClient.InsertPatronNoticePolicy(patronNoticePolicy2.ToJObject());
        }

        public void Update(PatronNoticePolicy2 patronNoticePolicy2) => FolioServiceClient.UpdatePatronNoticePolicy(patronNoticePolicy2.ToJObject());

        public void DeletePatronNoticePolicy2(Guid? id) => FolioServiceClient.DeletePatronNoticePolicy(id?.ToString());

        public bool AnyPayment2s(string where = null) => FolioServiceClient.AnyPayments(where);

        public int CountPayment2s(string where = null) => FolioServiceClient.CountPayments(where);

        public Payment2[] Payment2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Payments(out count, where, orderBy, skip, take).Select(jo =>
            {
                var p2 = Payment2.FromJObject(jo);
                if (load && p2.ServicePointId != null) p2.ServicePoint = (ServicePoint2)(cache && objects.ContainsKey(p2.ServicePointId.Value) ? objects[p2.ServicePointId.Value] : objects[p2.ServicePointId.Value] = FindServicePoint2(p2.ServicePointId));
                if (load && p2.FeeId != null) p2.Fee = (Fee2)(cache && objects.ContainsKey(p2.FeeId.Value) ? objects[p2.FeeId.Value] : objects[p2.FeeId.Value] = FindFee2(p2.FeeId));
                if (load && p2.UserId != null) p2.User = (User2)(cache && objects.ContainsKey(p2.UserId.Value) ? objects[p2.UserId.Value] : objects[p2.UserId.Value] = FindUser2(p2.UserId));
                return p2;
            }).ToArray();
        }

        public IEnumerable<Payment2> Payment2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Payments(where, orderBy, skip, take))
            {
                var p2 = Payment2.FromJObject(jo);
                if (load && p2.ServicePointId != null) p2.ServicePoint = (ServicePoint2)(cache && objects.ContainsKey(p2.ServicePointId.Value) ? objects[p2.ServicePointId.Value] : objects[p2.ServicePointId.Value] = FindServicePoint2(p2.ServicePointId));
                if (load && p2.FeeId != null) p2.Fee = (Fee2)(cache && objects.ContainsKey(p2.FeeId.Value) ? objects[p2.FeeId.Value] : objects[p2.FeeId.Value] = FindFee2(p2.FeeId));
                if (load && p2.UserId != null) p2.User = (User2)(cache && objects.ContainsKey(p2.UserId.Value) ? objects[p2.UserId.Value] : objects[p2.UserId.Value] = FindUser2(p2.UserId));
                yield return p2;
            }
        }

        public Payment2 FindPayment2(Guid? id, bool load = false, bool cache = true)
        {
            var p2 = Payment2.FromJObject(FolioServiceClient.GetPayment(id?.ToString()));
            if (p2 == null) return null;
            if (load && p2.ServicePointId != null) p2.ServicePoint = (ServicePoint2)(cache && objects.ContainsKey(p2.ServicePointId.Value) ? objects[p2.ServicePointId.Value] : objects[p2.ServicePointId.Value] = FindServicePoint2(p2.ServicePointId));
            if (load && p2.FeeId != null) p2.Fee = (Fee2)(cache && objects.ContainsKey(p2.FeeId.Value) ? objects[p2.FeeId.Value] : objects[p2.FeeId.Value] = FindFee2(p2.FeeId));
            if (load && p2.UserId != null) p2.User = (User2)(cache && objects.ContainsKey(p2.UserId.Value) ? objects[p2.UserId.Value] : objects[p2.UserId.Value] = FindUser2(p2.UserId));
            return p2;
        }

        public void Insert(Payment2 payment2)
        {
            if (payment2.Id == null) payment2.Id = Guid.NewGuid();
            FolioServiceClient.InsertPayment(payment2.ToJObject());
        }

        public void Update(Payment2 payment2) => FolioServiceClient.UpdatePayment(payment2.ToJObject());

        public void DeletePayment2(Guid? id) => FolioServiceClient.DeletePayment(id?.ToString());

        public bool AnyPaymentMethod2s(string where = null) => FolioServiceClient.AnyPaymentMethods(where);

        public int CountPaymentMethod2s(string where = null) => FolioServiceClient.CountPaymentMethods(where);

        public PaymentMethod2[] PaymentMethod2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.PaymentMethods(out count, where, orderBy, skip, take).Select(jo =>
            {
                var pm2 = PaymentMethod2.FromJObject(jo);
                if (load && pm2.CreationUserId != null) pm2.CreationUser = (User2)(cache && objects.ContainsKey(pm2.CreationUserId.Value) ? objects[pm2.CreationUserId.Value] : objects[pm2.CreationUserId.Value] = FindUser2(pm2.CreationUserId));
                if (load && pm2.LastWriteUserId != null) pm2.LastWriteUser = (User2)(cache && objects.ContainsKey(pm2.LastWriteUserId.Value) ? objects[pm2.LastWriteUserId.Value] : objects[pm2.LastWriteUserId.Value] = FindUser2(pm2.LastWriteUserId));
                if (load && pm2.OwnerId != null) pm2.Owner = (Owner2)(cache && objects.ContainsKey(pm2.OwnerId.Value) ? objects[pm2.OwnerId.Value] : objects[pm2.OwnerId.Value] = FindOwner2(pm2.OwnerId));
                return pm2;
            }).ToArray();
        }

        public IEnumerable<PaymentMethod2> PaymentMethod2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.PaymentMethods(where, orderBy, skip, take))
            {
                var pm2 = PaymentMethod2.FromJObject(jo);
                if (load && pm2.CreationUserId != null) pm2.CreationUser = (User2)(cache && objects.ContainsKey(pm2.CreationUserId.Value) ? objects[pm2.CreationUserId.Value] : objects[pm2.CreationUserId.Value] = FindUser2(pm2.CreationUserId));
                if (load && pm2.LastWriteUserId != null) pm2.LastWriteUser = (User2)(cache && objects.ContainsKey(pm2.LastWriteUserId.Value) ? objects[pm2.LastWriteUserId.Value] : objects[pm2.LastWriteUserId.Value] = FindUser2(pm2.LastWriteUserId));
                if (load && pm2.OwnerId != null) pm2.Owner = (Owner2)(cache && objects.ContainsKey(pm2.OwnerId.Value) ? objects[pm2.OwnerId.Value] : objects[pm2.OwnerId.Value] = FindOwner2(pm2.OwnerId));
                yield return pm2;
            }
        }

        public PaymentMethod2 FindPaymentMethod2(Guid? id, bool load = false, bool cache = true)
        {
            var pm2 = PaymentMethod2.FromJObject(FolioServiceClient.GetPaymentMethod(id?.ToString()));
            if (pm2 == null) return null;
            if (load && pm2.CreationUserId != null) pm2.CreationUser = (User2)(cache && objects.ContainsKey(pm2.CreationUserId.Value) ? objects[pm2.CreationUserId.Value] : objects[pm2.CreationUserId.Value] = FindUser2(pm2.CreationUserId));
            if (load && pm2.LastWriteUserId != null) pm2.LastWriteUser = (User2)(cache && objects.ContainsKey(pm2.LastWriteUserId.Value) ? objects[pm2.LastWriteUserId.Value] : objects[pm2.LastWriteUserId.Value] = FindUser2(pm2.LastWriteUserId));
            if (load && pm2.OwnerId != null) pm2.Owner = (Owner2)(cache && objects.ContainsKey(pm2.OwnerId.Value) ? objects[pm2.OwnerId.Value] : objects[pm2.OwnerId.Value] = FindOwner2(pm2.OwnerId));
            return pm2;
        }

        public void Insert(PaymentMethod2 paymentMethod2)
        {
            if (paymentMethod2.Id == null) paymentMethod2.Id = Guid.NewGuid();
            FolioServiceClient.InsertPaymentMethod(paymentMethod2.ToJObject());
        }

        public void Update(PaymentMethod2 paymentMethod2) => FolioServiceClient.UpdatePaymentMethod(paymentMethod2.ToJObject());

        public void DeletePaymentMethod2(Guid? id) => FolioServiceClient.DeletePaymentMethod(id?.ToString());

        public bool AnyPermission2s(string where = null) => FolioServiceClient.AnyPermissions(where);

        public int CountPermission2s(string where = null) => FolioServiceClient.CountPermissions(where);

        public Permission2[] Permission2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Permissions(out count, where, orderBy, skip, take).Select(jo =>
            {
                var p2 = Permission2.FromJObject(jo);
                if (load && p2.CreationUserId != null) p2.CreationUser = (User2)(cache && objects.ContainsKey(p2.CreationUserId.Value) ? objects[p2.CreationUserId.Value] : objects[p2.CreationUserId.Value] = FindUser2(p2.CreationUserId));
                if (load && p2.LastWriteUserId != null) p2.LastWriteUser = (User2)(cache && objects.ContainsKey(p2.LastWriteUserId.Value) ? objects[p2.LastWriteUserId.Value] : objects[p2.LastWriteUserId.Value] = FindUser2(p2.LastWriteUserId));
                return p2;
            }).ToArray();
        }

        public IEnumerable<Permission2> Permission2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Permissions(where, orderBy, skip, take))
            {
                var p2 = Permission2.FromJObject(jo);
                if (load && p2.CreationUserId != null) p2.CreationUser = (User2)(cache && objects.ContainsKey(p2.CreationUserId.Value) ? objects[p2.CreationUserId.Value] : objects[p2.CreationUserId.Value] = FindUser2(p2.CreationUserId));
                if (load && p2.LastWriteUserId != null) p2.LastWriteUser = (User2)(cache && objects.ContainsKey(p2.LastWriteUserId.Value) ? objects[p2.LastWriteUserId.Value] : objects[p2.LastWriteUserId.Value] = FindUser2(p2.LastWriteUserId));
                yield return p2;
            }
        }

        public Permission2 FindPermission2(Guid? id, bool load = false, bool cache = true)
        {
            var p2 = Permission2.FromJObject(FolioServiceClient.GetPermission(id?.ToString()));
            if (p2 == null) return null;
            if (load && p2.CreationUserId != null) p2.CreationUser = (User2)(cache && objects.ContainsKey(p2.CreationUserId.Value) ? objects[p2.CreationUserId.Value] : objects[p2.CreationUserId.Value] = FindUser2(p2.CreationUserId));
            if (load && p2.LastWriteUserId != null) p2.LastWriteUser = (User2)(cache && objects.ContainsKey(p2.LastWriteUserId.Value) ? objects[p2.LastWriteUserId.Value] : objects[p2.LastWriteUserId.Value] = FindUser2(p2.LastWriteUserId));
            var i = 0;
            if (p2.PermissionChildOfs != null) foreach (var pco in p2.PermissionChildOfs)
                {
                    pco.Id = (++i).ToString();
                    pco.PermissionId = p2.Id;
                }
            i = 0;
            if (p2.PermissionGrantedTos != null) foreach (var pgt in p2.PermissionGrantedTos)
                {
                    pgt.Id = (++i).ToString();
                    pgt.PermissionId = p2.Id;
                }
            i = 0;
            if (p2.PermissionSubPermissions != null) foreach (var psp in p2.PermissionSubPermissions)
                {
                    psp.Id = (++i).ToString();
                    psp.PermissionId = p2.Id;
                }
            i = 0;
            if (p2.PermissionTags != null) foreach (var pt in p2.PermissionTags)
                {
                    pt.Id = (++i).ToString();
                    pt.PermissionId = p2.Id;
                }
            return p2;
        }

        public void Insert(Permission2 permission2)
        {
            if (permission2.Id == null) permission2.Id = Guid.NewGuid();
            FolioServiceClient.InsertPermission(permission2.ToJObject());
        }

        public void Update(Permission2 permission2) => FolioServiceClient.UpdatePermission(permission2.ToJObject());

        public void DeletePermission2(Guid? id) => FolioServiceClient.DeletePermission(id?.ToString());

        public bool AnyPermissionsUser2s(string where = null) => FolioServiceClient.AnyPermissionsUsers(where);

        public int CountPermissionsUser2s(string where = null) => FolioServiceClient.CountPermissionsUsers(where);

        public PermissionsUser2[] PermissionsUser2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.PermissionsUsers(out count, where, orderBy, skip, take).Select(jo =>
            {
                var pu2 = PermissionsUser2.FromJObject(jo);
                if (load && pu2.UserId != null) pu2.User = (User2)(cache && objects.ContainsKey(pu2.UserId.Value) ? objects[pu2.UserId.Value] : objects[pu2.UserId.Value] = FindUser2(pu2.UserId));
                if (load && pu2.CreationUserId != null) pu2.CreationUser = (User2)(cache && objects.ContainsKey(pu2.CreationUserId.Value) ? objects[pu2.CreationUserId.Value] : objects[pu2.CreationUserId.Value] = FindUser2(pu2.CreationUserId));
                if (load && pu2.LastWriteUserId != null) pu2.LastWriteUser = (User2)(cache && objects.ContainsKey(pu2.LastWriteUserId.Value) ? objects[pu2.LastWriteUserId.Value] : objects[pu2.LastWriteUserId.Value] = FindUser2(pu2.LastWriteUserId));
                return pu2;
            }).ToArray();
        }

        public IEnumerable<PermissionsUser2> PermissionsUser2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.PermissionsUsers(where, orderBy, skip, take))
            {
                var pu2 = PermissionsUser2.FromJObject(jo);
                if (load && pu2.UserId != null) pu2.User = (User2)(cache && objects.ContainsKey(pu2.UserId.Value) ? objects[pu2.UserId.Value] : objects[pu2.UserId.Value] = FindUser2(pu2.UserId));
                if (load && pu2.CreationUserId != null) pu2.CreationUser = (User2)(cache && objects.ContainsKey(pu2.CreationUserId.Value) ? objects[pu2.CreationUserId.Value] : objects[pu2.CreationUserId.Value] = FindUser2(pu2.CreationUserId));
                if (load && pu2.LastWriteUserId != null) pu2.LastWriteUser = (User2)(cache && objects.ContainsKey(pu2.LastWriteUserId.Value) ? objects[pu2.LastWriteUserId.Value] : objects[pu2.LastWriteUserId.Value] = FindUser2(pu2.LastWriteUserId));
                yield return pu2;
            }
        }

        public PermissionsUser2 FindPermissionsUser2(Guid? id, bool load = false, bool cache = true)
        {
            var pu2 = PermissionsUser2.FromJObject(FolioServiceClient.GetPermissionsUser(id?.ToString()));
            if (pu2 == null) return null;
            if (load && pu2.UserId != null) pu2.User = (User2)(cache && objects.ContainsKey(pu2.UserId.Value) ? objects[pu2.UserId.Value] : objects[pu2.UserId.Value] = FindUser2(pu2.UserId));
            if (load && pu2.CreationUserId != null) pu2.CreationUser = (User2)(cache && objects.ContainsKey(pu2.CreationUserId.Value) ? objects[pu2.CreationUserId.Value] : objects[pu2.CreationUserId.Value] = FindUser2(pu2.CreationUserId));
            if (load && pu2.LastWriteUserId != null) pu2.LastWriteUser = (User2)(cache && objects.ContainsKey(pu2.LastWriteUserId.Value) ? objects[pu2.LastWriteUserId.Value] : objects[pu2.LastWriteUserId.Value] = FindUser2(pu2.LastWriteUserId));
            var i = 0;
            if (pu2.PermissionsUserPermissions != null) foreach (var pup in pu2.PermissionsUserPermissions)
                {
                    pup.Id = (++i).ToString();
                    pup.PermissionsUserId = pu2.Id;
                }
            return pu2;
        }

        public void Insert(PermissionsUser2 permissionsUser2)
        {
            if (permissionsUser2.Id == null) permissionsUser2.Id = Guid.NewGuid();
            FolioServiceClient.InsertPermissionsUser(permissionsUser2.ToJObject());
        }

        public void Update(PermissionsUser2 permissionsUser2) => FolioServiceClient.UpdatePermissionsUser(permissionsUser2.ToJObject());

        public void DeletePermissionsUser2(Guid? id) => FolioServiceClient.DeletePermissionsUser(id?.ToString());

        public bool AnyPrecedingSucceedingTitle2s(string where = null) => FolioServiceClient.AnyPrecedingSucceedingTitles(where);

        public int CountPrecedingSucceedingTitle2s(string where = null) => FolioServiceClient.CountPrecedingSucceedingTitles(where);

        public PrecedingSucceedingTitle2[] PrecedingSucceedingTitle2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.PrecedingSucceedingTitles(out count, where, orderBy, skip, take).Select(jo =>
            {
                var pst2 = PrecedingSucceedingTitle2.FromJObject(jo);
                if (load && pst2.PrecedingInstanceId != null) pst2.PrecedingInstance = (Instance2)(cache && objects.ContainsKey(pst2.PrecedingInstanceId.Value) ? objects[pst2.PrecedingInstanceId.Value] : objects[pst2.PrecedingInstanceId.Value] = FindInstance2(pst2.PrecedingInstanceId));
                if (load && pst2.SucceedingInstanceId != null) pst2.SucceedingInstance = (Instance2)(cache && objects.ContainsKey(pst2.SucceedingInstanceId.Value) ? objects[pst2.SucceedingInstanceId.Value] : objects[pst2.SucceedingInstanceId.Value] = FindInstance2(pst2.SucceedingInstanceId));
                if (load && pst2.CreationUserId != null) pst2.CreationUser = (User2)(cache && objects.ContainsKey(pst2.CreationUserId.Value) ? objects[pst2.CreationUserId.Value] : objects[pst2.CreationUserId.Value] = FindUser2(pst2.CreationUserId));
                if (load && pst2.LastWriteUserId != null) pst2.LastWriteUser = (User2)(cache && objects.ContainsKey(pst2.LastWriteUserId.Value) ? objects[pst2.LastWriteUserId.Value] : objects[pst2.LastWriteUserId.Value] = FindUser2(pst2.LastWriteUserId));
                return pst2;
            }).ToArray();
        }

        public IEnumerable<PrecedingSucceedingTitle2> PrecedingSucceedingTitle2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.PrecedingSucceedingTitles(where, orderBy, skip, take))
            {
                var pst2 = PrecedingSucceedingTitle2.FromJObject(jo);
                if (load && pst2.PrecedingInstanceId != null) pst2.PrecedingInstance = (Instance2)(cache && objects.ContainsKey(pst2.PrecedingInstanceId.Value) ? objects[pst2.PrecedingInstanceId.Value] : objects[pst2.PrecedingInstanceId.Value] = FindInstance2(pst2.PrecedingInstanceId));
                if (load && pst2.SucceedingInstanceId != null) pst2.SucceedingInstance = (Instance2)(cache && objects.ContainsKey(pst2.SucceedingInstanceId.Value) ? objects[pst2.SucceedingInstanceId.Value] : objects[pst2.SucceedingInstanceId.Value] = FindInstance2(pst2.SucceedingInstanceId));
                if (load && pst2.CreationUserId != null) pst2.CreationUser = (User2)(cache && objects.ContainsKey(pst2.CreationUserId.Value) ? objects[pst2.CreationUserId.Value] : objects[pst2.CreationUserId.Value] = FindUser2(pst2.CreationUserId));
                if (load && pst2.LastWriteUserId != null) pst2.LastWriteUser = (User2)(cache && objects.ContainsKey(pst2.LastWriteUserId.Value) ? objects[pst2.LastWriteUserId.Value] : objects[pst2.LastWriteUserId.Value] = FindUser2(pst2.LastWriteUserId));
                yield return pst2;
            }
        }

        public PrecedingSucceedingTitle2 FindPrecedingSucceedingTitle2(Guid? id, bool load = false, bool cache = true)
        {
            var pst2 = PrecedingSucceedingTitle2.FromJObject(FolioServiceClient.GetPrecedingSucceedingTitle(id?.ToString()));
            if (pst2 == null) return null;
            if (load && pst2.PrecedingInstanceId != null) pst2.PrecedingInstance = (Instance2)(cache && objects.ContainsKey(pst2.PrecedingInstanceId.Value) ? objects[pst2.PrecedingInstanceId.Value] : objects[pst2.PrecedingInstanceId.Value] = FindInstance2(pst2.PrecedingInstanceId));
            if (load && pst2.SucceedingInstanceId != null) pst2.SucceedingInstance = (Instance2)(cache && objects.ContainsKey(pst2.SucceedingInstanceId.Value) ? objects[pst2.SucceedingInstanceId.Value] : objects[pst2.SucceedingInstanceId.Value] = FindInstance2(pst2.SucceedingInstanceId));
            if (load && pst2.CreationUserId != null) pst2.CreationUser = (User2)(cache && objects.ContainsKey(pst2.CreationUserId.Value) ? objects[pst2.CreationUserId.Value] : objects[pst2.CreationUserId.Value] = FindUser2(pst2.CreationUserId));
            if (load && pst2.LastWriteUserId != null) pst2.LastWriteUser = (User2)(cache && objects.ContainsKey(pst2.LastWriteUserId.Value) ? objects[pst2.LastWriteUserId.Value] : objects[pst2.LastWriteUserId.Value] = FindUser2(pst2.LastWriteUserId));
            var i = 0;
            if (pst2.PrecedingSucceedingTitleIdentifiers != null) foreach (var psti in pst2.PrecedingSucceedingTitleIdentifiers)
                {
                    psti.Id = (++i).ToString();
                    psti.PrecedingSucceedingTitleId = pst2.Id;
                }
            return pst2;
        }

        public void Insert(PrecedingSucceedingTitle2 precedingSucceedingTitle2)
        {
            if (precedingSucceedingTitle2.Id == null) precedingSucceedingTitle2.Id = Guid.NewGuid();
            FolioServiceClient.InsertPrecedingSucceedingTitle(precedingSucceedingTitle2.ToJObject());
        }

        public void Update(PrecedingSucceedingTitle2 precedingSucceedingTitle2) => FolioServiceClient.UpdatePrecedingSucceedingTitle(precedingSucceedingTitle2.ToJObject());

        public void DeletePrecedingSucceedingTitle2(Guid? id) => FolioServiceClient.DeletePrecedingSucceedingTitle(id?.ToString());

        public bool AnyPrefix2s(string where = null) => FolioServiceClient.AnyPrefixes(where);

        public int CountPrefix2s(string where = null) => FolioServiceClient.CountPrefixes(where);

        public Prefix2[] Prefix2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Prefixes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var p2 = Prefix2.FromJObject(jo);
                return p2;
            }).ToArray();
        }

        public IEnumerable<Prefix2> Prefix2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Prefixes(where, orderBy, skip, take))
            {
                var p2 = Prefix2.FromJObject(jo);
                yield return p2;
            }
        }

        public Prefix2 FindPrefix2(Guid? id, bool load = false, bool cache = true) => Prefix2.FromJObject(FolioServiceClient.GetPrefix(id?.ToString()));

        public void Insert(Prefix2 prefix2)
        {
            if (prefix2.Id == null) prefix2.Id = Guid.NewGuid();
            FolioServiceClient.InsertPrefix(prefix2.ToJObject());
        }

        public void Update(Prefix2 prefix2) => FolioServiceClient.UpdatePrefix(prefix2.ToJObject());

        public void DeletePrefix2(Guid? id) => FolioServiceClient.DeletePrefix(id?.ToString());

        public bool AnyPrinters(string where = null) => FolioServiceClient.AnyPrinters(where);

        public int CountPrinters(string where = null) => FolioServiceClient.CountPrinters(where);

        public Printer[] Printers(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Printers(out count, where, orderBy, skip, take).Select(jo =>
            {
                var p = Printer.FromJObject(jo);
                if (load && p.CreationUserId != null) p.CreationUser = (User2)(cache && objects.ContainsKey(p.CreationUserId.Value) ? objects[p.CreationUserId.Value] : objects[p.CreationUserId.Value] = FindUser2(p.CreationUserId));
                if (load && p.LastWriteUserId != null) p.LastWriteUser = (User2)(cache && objects.ContainsKey(p.LastWriteUserId.Value) ? objects[p.LastWriteUserId.Value] : objects[p.LastWriteUserId.Value] = FindUser2(p.LastWriteUserId));
                return p;
            }).ToArray();
        }

        public IEnumerable<Printer> Printers(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Printers(where, orderBy, skip, take))
            {
                var p = Printer.FromJObject(jo);
                if (load && p.CreationUserId != null) p.CreationUser = (User2)(cache && objects.ContainsKey(p.CreationUserId.Value) ? objects[p.CreationUserId.Value] : objects[p.CreationUserId.Value] = FindUser2(p.CreationUserId));
                if (load && p.LastWriteUserId != null) p.LastWriteUser = (User2)(cache && objects.ContainsKey(p.LastWriteUserId.Value) ? objects[p.LastWriteUserId.Value] : objects[p.LastWriteUserId.Value] = FindUser2(p.LastWriteUserId));
                yield return p;
            }
        }

        public Printer FindPrinter(Guid? id, bool load = false, bool cache = true)
        {
            var p = Printer.FromJObject(FolioServiceClient.GetPrinter(id?.ToString()));
            if (p == null) return null;
            if (load && p.CreationUserId != null) p.CreationUser = (User2)(cache && objects.ContainsKey(p.CreationUserId.Value) ? objects[p.CreationUserId.Value] : objects[p.CreationUserId.Value] = FindUser2(p.CreationUserId));
            if (load && p.LastWriteUserId != null) p.LastWriteUser = (User2)(cache && objects.ContainsKey(p.LastWriteUserId.Value) ? objects[p.LastWriteUserId.Value] : objects[p.LastWriteUserId.Value] = FindUser2(p.LastWriteUserId));
            return p;
        }

        public void Insert(Printer printer)
        {
            if (printer.Id == null) printer.Id = Guid.NewGuid();
            FolioServiceClient.InsertPrinter(printer.ToJObject());
        }

        public void Update(Printer printer) => FolioServiceClient.UpdatePrinter(printer.ToJObject());

        public void DeletePrinter(Guid? id) => FolioServiceClient.DeletePrinter(id?.ToString());

        public bool AnyProxy2s(string where = null) => FolioServiceClient.AnyProxies(where);

        public int CountProxy2s(string where = null) => FolioServiceClient.CountProxies(where);

        public Proxy2[] Proxy2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Proxies(out count, where, orderBy, skip, take).Select(jo =>
            {
                var p2 = Proxy2.FromJObject(jo);
                if (load && p2.UserId != null) p2.User = (User2)(cache && objects.ContainsKey(p2.UserId.Value) ? objects[p2.UserId.Value] : objects[p2.UserId.Value] = FindUser2(p2.UserId));
                if (load && p2.ProxyUserId != null) p2.ProxyUser = (User2)(cache && objects.ContainsKey(p2.ProxyUserId.Value) ? objects[p2.ProxyUserId.Value] : objects[p2.ProxyUserId.Value] = FindUser2(p2.ProxyUserId));
                if (load && p2.CreationUserId != null) p2.CreationUser = (User2)(cache && objects.ContainsKey(p2.CreationUserId.Value) ? objects[p2.CreationUserId.Value] : objects[p2.CreationUserId.Value] = FindUser2(p2.CreationUserId));
                if (load && p2.LastWriteUserId != null) p2.LastWriteUser = (User2)(cache && objects.ContainsKey(p2.LastWriteUserId.Value) ? objects[p2.LastWriteUserId.Value] : objects[p2.LastWriteUserId.Value] = FindUser2(p2.LastWriteUserId));
                return p2;
            }).ToArray();
        }

        public IEnumerable<Proxy2> Proxy2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Proxies(where, orderBy, skip, take))
            {
                var p2 = Proxy2.FromJObject(jo);
                if (load && p2.UserId != null) p2.User = (User2)(cache && objects.ContainsKey(p2.UserId.Value) ? objects[p2.UserId.Value] : objects[p2.UserId.Value] = FindUser2(p2.UserId));
                if (load && p2.ProxyUserId != null) p2.ProxyUser = (User2)(cache && objects.ContainsKey(p2.ProxyUserId.Value) ? objects[p2.ProxyUserId.Value] : objects[p2.ProxyUserId.Value] = FindUser2(p2.ProxyUserId));
                if (load && p2.CreationUserId != null) p2.CreationUser = (User2)(cache && objects.ContainsKey(p2.CreationUserId.Value) ? objects[p2.CreationUserId.Value] : objects[p2.CreationUserId.Value] = FindUser2(p2.CreationUserId));
                if (load && p2.LastWriteUserId != null) p2.LastWriteUser = (User2)(cache && objects.ContainsKey(p2.LastWriteUserId.Value) ? objects[p2.LastWriteUserId.Value] : objects[p2.LastWriteUserId.Value] = FindUser2(p2.LastWriteUserId));
                yield return p2;
            }
        }

        public Proxy2 FindProxy2(Guid? id, bool load = false, bool cache = true)
        {
            var p2 = Proxy2.FromJObject(FolioServiceClient.GetProxy(id?.ToString()));
            if (p2 == null) return null;
            if (load && p2.UserId != null) p2.User = (User2)(cache && objects.ContainsKey(p2.UserId.Value) ? objects[p2.UserId.Value] : objects[p2.UserId.Value] = FindUser2(p2.UserId));
            if (load && p2.ProxyUserId != null) p2.ProxyUser = (User2)(cache && objects.ContainsKey(p2.ProxyUserId.Value) ? objects[p2.ProxyUserId.Value] : objects[p2.ProxyUserId.Value] = FindUser2(p2.ProxyUserId));
            if (load && p2.CreationUserId != null) p2.CreationUser = (User2)(cache && objects.ContainsKey(p2.CreationUserId.Value) ? objects[p2.CreationUserId.Value] : objects[p2.CreationUserId.Value] = FindUser2(p2.CreationUserId));
            if (load && p2.LastWriteUserId != null) p2.LastWriteUser = (User2)(cache && objects.ContainsKey(p2.LastWriteUserId.Value) ? objects[p2.LastWriteUserId.Value] : objects[p2.LastWriteUserId.Value] = FindUser2(p2.LastWriteUserId));
            return p2;
        }

        public void Insert(Proxy2 proxy2)
        {
            if (proxy2.Id == null) proxy2.Id = Guid.NewGuid();
            FolioServiceClient.InsertProxy(proxy2.ToJObject());
        }

        public void Update(Proxy2 proxy2) => FolioServiceClient.UpdateProxy(proxy2.ToJObject());

        public void DeleteProxy2(Guid? id) => FolioServiceClient.DeleteProxy(id?.ToString());

        public bool AnyReceiving2s(string where = null) => FolioServiceClient.AnyReceivings(where);

        public int CountReceiving2s(string where = null) => FolioServiceClient.CountReceivings(where);

        public Receiving2[] Receiving2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Receivings(out count, where, orderBy, skip, take).Select(jo =>
            {
                var r2 = Receiving2.FromJObject(jo);
                if (load && r2.ItemId != null) r2.Item = (Item2)(cache && objects.ContainsKey(r2.ItemId.Value) ? objects[r2.ItemId.Value] : objects[r2.ItemId.Value] = FindItem2(r2.ItemId));
                if (load && r2.LocationId != null) r2.Location = (Location2)(cache && objects.ContainsKey(r2.LocationId.Value) ? objects[r2.LocationId.Value] : objects[r2.LocationId.Value] = FindLocation2(r2.LocationId));
                if (load && r2.OrderItemId != null) r2.OrderItem = (OrderItem2)(cache && objects.ContainsKey(r2.OrderItemId.Value) ? objects[r2.OrderItemId.Value] : objects[r2.OrderItemId.Value] = FindOrderItem2(r2.OrderItemId));
                if (load && r2.TitleId != null) r2.Title = (Title2)(cache && objects.ContainsKey(r2.TitleId.Value) ? objects[r2.TitleId.Value] : objects[r2.TitleId.Value] = FindTitle2(r2.TitleId));
                if (load && r2.HoldingId != null) r2.Holding = (Holding2)(cache && objects.ContainsKey(r2.HoldingId.Value) ? objects[r2.HoldingId.Value] : objects[r2.HoldingId.Value] = FindHolding2(r2.HoldingId));
                return r2;
            }).ToArray();
        }

        public IEnumerable<Receiving2> Receiving2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Receivings(where, orderBy, skip, take))
            {
                var r2 = Receiving2.FromJObject(jo);
                if (load && r2.ItemId != null) r2.Item = (Item2)(cache && objects.ContainsKey(r2.ItemId.Value) ? objects[r2.ItemId.Value] : objects[r2.ItemId.Value] = FindItem2(r2.ItemId));
                if (load && r2.LocationId != null) r2.Location = (Location2)(cache && objects.ContainsKey(r2.LocationId.Value) ? objects[r2.LocationId.Value] : objects[r2.LocationId.Value] = FindLocation2(r2.LocationId));
                if (load && r2.OrderItemId != null) r2.OrderItem = (OrderItem2)(cache && objects.ContainsKey(r2.OrderItemId.Value) ? objects[r2.OrderItemId.Value] : objects[r2.OrderItemId.Value] = FindOrderItem2(r2.OrderItemId));
                if (load && r2.TitleId != null) r2.Title = (Title2)(cache && objects.ContainsKey(r2.TitleId.Value) ? objects[r2.TitleId.Value] : objects[r2.TitleId.Value] = FindTitle2(r2.TitleId));
                if (load && r2.HoldingId != null) r2.Holding = (Holding2)(cache && objects.ContainsKey(r2.HoldingId.Value) ? objects[r2.HoldingId.Value] : objects[r2.HoldingId.Value] = FindHolding2(r2.HoldingId));
                yield return r2;
            }
        }

        public Receiving2 FindReceiving2(Guid? id, bool load = false, bool cache = true)
        {
            var r2 = Receiving2.FromJObject(FolioServiceClient.GetReceiving(id?.ToString()));
            if (r2 == null) return null;
            if (load && r2.ItemId != null) r2.Item = (Item2)(cache && objects.ContainsKey(r2.ItemId.Value) ? objects[r2.ItemId.Value] : objects[r2.ItemId.Value] = FindItem2(r2.ItemId));
            if (load && r2.LocationId != null) r2.Location = (Location2)(cache && objects.ContainsKey(r2.LocationId.Value) ? objects[r2.LocationId.Value] : objects[r2.LocationId.Value] = FindLocation2(r2.LocationId));
            if (load && r2.OrderItemId != null) r2.OrderItem = (OrderItem2)(cache && objects.ContainsKey(r2.OrderItemId.Value) ? objects[r2.OrderItemId.Value] : objects[r2.OrderItemId.Value] = FindOrderItem2(r2.OrderItemId));
            if (load && r2.TitleId != null) r2.Title = (Title2)(cache && objects.ContainsKey(r2.TitleId.Value) ? objects[r2.TitleId.Value] : objects[r2.TitleId.Value] = FindTitle2(r2.TitleId));
            if (load && r2.HoldingId != null) r2.Holding = (Holding2)(cache && objects.ContainsKey(r2.HoldingId.Value) ? objects[r2.HoldingId.Value] : objects[r2.HoldingId.Value] = FindHolding2(r2.HoldingId));
            return r2;
        }

        public void Insert(Receiving2 receiving2)
        {
            if (receiving2.Id == null) receiving2.Id = Guid.NewGuid();
            FolioServiceClient.InsertReceiving(receiving2.ToJObject());
        }

        public void Update(Receiving2 receiving2) => FolioServiceClient.UpdateReceiving(receiving2.ToJObject());

        public void DeleteReceiving2(Guid? id) => FolioServiceClient.DeleteReceiving(id?.ToString());

        public bool AnyRecord2s(string where = null) => FolioServiceClient.AnyRecords(where);

        public int CountRecord2s(string where = null) => FolioServiceClient.CountRecords(where);

        public Record2[] Record2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Records(out count, where, orderBy, skip, take).Select(jo =>
            {
                var r2 = Record2.FromJObject(jo);
                if (load && r2.SnapshotId != null) r2.Snapshot = (Snapshot2)(cache && objects.ContainsKey(r2.SnapshotId.Value) ? objects[r2.SnapshotId.Value] : objects[r2.SnapshotId.Value] = FindSnapshot2(r2.SnapshotId));
                if (load && r2.InstanceId != null) r2.Instance = (Instance2)(cache && objects.ContainsKey(r2.InstanceId.Value) ? objects[r2.InstanceId.Value] : objects[r2.InstanceId.Value] = FindInstance2(r2.InstanceId));
                if (load && r2.CreationUserId != null) r2.CreationUser = (User2)(cache && objects.ContainsKey(r2.CreationUserId.Value) ? objects[r2.CreationUserId.Value] : objects[r2.CreationUserId.Value] = FindUser2(r2.CreationUserId));
                if (load && r2.LastWriteUserId != null) r2.LastWriteUser = (User2)(cache && objects.ContainsKey(r2.LastWriteUserId.Value) ? objects[r2.LastWriteUserId.Value] : objects[r2.LastWriteUserId.Value] = FindUser2(r2.LastWriteUserId));
                return r2;
            }).ToArray();
        }

        public IEnumerable<Record2> Record2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Records(where, orderBy, skip, take))
            {
                var r2 = Record2.FromJObject(jo);
                if (load && r2.SnapshotId != null) r2.Snapshot = (Snapshot2)(cache && objects.ContainsKey(r2.SnapshotId.Value) ? objects[r2.SnapshotId.Value] : objects[r2.SnapshotId.Value] = FindSnapshot2(r2.SnapshotId));
                if (load && r2.InstanceId != null) r2.Instance = (Instance2)(cache && objects.ContainsKey(r2.InstanceId.Value) ? objects[r2.InstanceId.Value] : objects[r2.InstanceId.Value] = FindInstance2(r2.InstanceId));
                if (load && r2.CreationUserId != null) r2.CreationUser = (User2)(cache && objects.ContainsKey(r2.CreationUserId.Value) ? objects[r2.CreationUserId.Value] : objects[r2.CreationUserId.Value] = FindUser2(r2.CreationUserId));
                if (load && r2.LastWriteUserId != null) r2.LastWriteUser = (User2)(cache && objects.ContainsKey(r2.LastWriteUserId.Value) ? objects[r2.LastWriteUserId.Value] : objects[r2.LastWriteUserId.Value] = FindUser2(r2.LastWriteUserId));
                yield return r2;
            }
        }

        public Record2 FindRecord2(Guid? id, bool load = false, bool cache = true)
        {
            var r2 = Record2.FromJObject(FolioServiceClient.GetRecord(id?.ToString()));
            if (r2 == null) return null;
            if (load && r2.SnapshotId != null) r2.Snapshot = (Snapshot2)(cache && objects.ContainsKey(r2.SnapshotId.Value) ? objects[r2.SnapshotId.Value] : objects[r2.SnapshotId.Value] = FindSnapshot2(r2.SnapshotId));
            if (load && r2.InstanceId != null) r2.Instance = (Instance2)(cache && objects.ContainsKey(r2.InstanceId.Value) ? objects[r2.InstanceId.Value] : objects[r2.InstanceId.Value] = FindInstance2(r2.InstanceId));
            if (load && r2.CreationUserId != null) r2.CreationUser = (User2)(cache && objects.ContainsKey(r2.CreationUserId.Value) ? objects[r2.CreationUserId.Value] : objects[r2.CreationUserId.Value] = FindUser2(r2.CreationUserId));
            if (load && r2.LastWriteUserId != null) r2.LastWriteUser = (User2)(cache && objects.ContainsKey(r2.LastWriteUserId.Value) ? objects[r2.LastWriteUserId.Value] : objects[r2.LastWriteUserId.Value] = FindUser2(r2.LastWriteUserId));
            return r2;
        }

        public void Insert(Record2 record2)
        {
            if (record2.Id == null) record2.Id = Guid.NewGuid();
            FolioServiceClient.InsertRecord(record2.ToJObject());
        }

        public void Update(Record2 record2) => FolioServiceClient.UpdateRecord(record2.ToJObject());

        public void DeleteRecord2(Guid? id) => FolioServiceClient.DeleteRecord(id?.ToString());

        public bool AnyRefundReason2s(string where = null) => FolioServiceClient.AnyRefundReasons(where);

        public int CountRefundReason2s(string where = null) => FolioServiceClient.CountRefundReasons(where);

        public RefundReason2[] RefundReason2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.RefundReasons(out count, where, orderBy, skip, take).Select(jo =>
            {
                var rr2 = RefundReason2.FromJObject(jo);
                if (load && rr2.CreationUserId != null) rr2.CreationUser = (User2)(cache && objects.ContainsKey(rr2.CreationUserId.Value) ? objects[rr2.CreationUserId.Value] : objects[rr2.CreationUserId.Value] = FindUser2(rr2.CreationUserId));
                if (load && rr2.LastWriteUserId != null) rr2.LastWriteUser = (User2)(cache && objects.ContainsKey(rr2.LastWriteUserId.Value) ? objects[rr2.LastWriteUserId.Value] : objects[rr2.LastWriteUserId.Value] = FindUser2(rr2.LastWriteUserId));
                if (load && rr2.AccountId != null) rr2.Account = (Fee2)(cache && objects.ContainsKey(rr2.AccountId.Value) ? objects[rr2.AccountId.Value] : objects[rr2.AccountId.Value] = FindFee2(rr2.AccountId));
                return rr2;
            }).ToArray();
        }

        public IEnumerable<RefundReason2> RefundReason2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.RefundReasons(where, orderBy, skip, take))
            {
                var rr2 = RefundReason2.FromJObject(jo);
                if (load && rr2.CreationUserId != null) rr2.CreationUser = (User2)(cache && objects.ContainsKey(rr2.CreationUserId.Value) ? objects[rr2.CreationUserId.Value] : objects[rr2.CreationUserId.Value] = FindUser2(rr2.CreationUserId));
                if (load && rr2.LastWriteUserId != null) rr2.LastWriteUser = (User2)(cache && objects.ContainsKey(rr2.LastWriteUserId.Value) ? objects[rr2.LastWriteUserId.Value] : objects[rr2.LastWriteUserId.Value] = FindUser2(rr2.LastWriteUserId));
                if (load && rr2.AccountId != null) rr2.Account = (Fee2)(cache && objects.ContainsKey(rr2.AccountId.Value) ? objects[rr2.AccountId.Value] : objects[rr2.AccountId.Value] = FindFee2(rr2.AccountId));
                yield return rr2;
            }
        }

        public RefundReason2 FindRefundReason2(Guid? id, bool load = false, bool cache = true)
        {
            var rr2 = RefundReason2.FromJObject(FolioServiceClient.GetRefundReason(id?.ToString()));
            if (rr2 == null) return null;
            if (load && rr2.CreationUserId != null) rr2.CreationUser = (User2)(cache && objects.ContainsKey(rr2.CreationUserId.Value) ? objects[rr2.CreationUserId.Value] : objects[rr2.CreationUserId.Value] = FindUser2(rr2.CreationUserId));
            if (load && rr2.LastWriteUserId != null) rr2.LastWriteUser = (User2)(cache && objects.ContainsKey(rr2.LastWriteUserId.Value) ? objects[rr2.LastWriteUserId.Value] : objects[rr2.LastWriteUserId.Value] = FindUser2(rr2.LastWriteUserId));
            if (load && rr2.AccountId != null) rr2.Account = (Fee2)(cache && objects.ContainsKey(rr2.AccountId.Value) ? objects[rr2.AccountId.Value] : objects[rr2.AccountId.Value] = FindFee2(rr2.AccountId));
            return rr2;
        }

        public void Insert(RefundReason2 refundReason2)
        {
            if (refundReason2.Id == null) refundReason2.Id = Guid.NewGuid();
            FolioServiceClient.InsertRefundReason(refundReason2.ToJObject());
        }

        public void Update(RefundReason2 refundReason2) => FolioServiceClient.UpdateRefundReason(refundReason2.ToJObject());

        public void DeleteRefundReason2(Guid? id) => FolioServiceClient.DeleteRefundReason(id?.ToString());

        public bool AnyRelationships(string where = null) => FolioServiceClient.AnyInstanceRelationships(where);

        public int CountRelationships(string where = null) => FolioServiceClient.CountInstanceRelationships(where);

        public Relationship[] Relationships(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.InstanceRelationships(out count, where, orderBy, skip, take).Select(jo =>
            {
                var r = Relationship.FromJObject(jo);
                if (load && r.SuperInstanceId != null) r.SuperInstance = (Instance2)(cache && objects.ContainsKey(r.SuperInstanceId.Value) ? objects[r.SuperInstanceId.Value] : objects[r.SuperInstanceId.Value] = FindInstance2(r.SuperInstanceId));
                if (load && r.SubInstanceId != null) r.SubInstance = (Instance2)(cache && objects.ContainsKey(r.SubInstanceId.Value) ? objects[r.SubInstanceId.Value] : objects[r.SubInstanceId.Value] = FindInstance2(r.SubInstanceId));
                if (load && r.InstanceRelationshipTypeId != null) r.InstanceRelationshipType = (RelationshipType)(cache && objects.ContainsKey(r.InstanceRelationshipTypeId.Value) ? objects[r.InstanceRelationshipTypeId.Value] : objects[r.InstanceRelationshipTypeId.Value] = FindRelationshipType(r.InstanceRelationshipTypeId));
                if (load && r.CreationUserId != null) r.CreationUser = (User2)(cache && objects.ContainsKey(r.CreationUserId.Value) ? objects[r.CreationUserId.Value] : objects[r.CreationUserId.Value] = FindUser2(r.CreationUserId));
                if (load && r.LastWriteUserId != null) r.LastWriteUser = (User2)(cache && objects.ContainsKey(r.LastWriteUserId.Value) ? objects[r.LastWriteUserId.Value] : objects[r.LastWriteUserId.Value] = FindUser2(r.LastWriteUserId));
                return r;
            }).ToArray();
        }

        public IEnumerable<Relationship> Relationships(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.InstanceRelationships(where, orderBy, skip, take))
            {
                var r = Relationship.FromJObject(jo);
                if (load && r.SuperInstanceId != null) r.SuperInstance = (Instance2)(cache && objects.ContainsKey(r.SuperInstanceId.Value) ? objects[r.SuperInstanceId.Value] : objects[r.SuperInstanceId.Value] = FindInstance2(r.SuperInstanceId));
                if (load && r.SubInstanceId != null) r.SubInstance = (Instance2)(cache && objects.ContainsKey(r.SubInstanceId.Value) ? objects[r.SubInstanceId.Value] : objects[r.SubInstanceId.Value] = FindInstance2(r.SubInstanceId));
                if (load && r.InstanceRelationshipTypeId != null) r.InstanceRelationshipType = (RelationshipType)(cache && objects.ContainsKey(r.InstanceRelationshipTypeId.Value) ? objects[r.InstanceRelationshipTypeId.Value] : objects[r.InstanceRelationshipTypeId.Value] = FindRelationshipType(r.InstanceRelationshipTypeId));
                if (load && r.CreationUserId != null) r.CreationUser = (User2)(cache && objects.ContainsKey(r.CreationUserId.Value) ? objects[r.CreationUserId.Value] : objects[r.CreationUserId.Value] = FindUser2(r.CreationUserId));
                if (load && r.LastWriteUserId != null) r.LastWriteUser = (User2)(cache && objects.ContainsKey(r.LastWriteUserId.Value) ? objects[r.LastWriteUserId.Value] : objects[r.LastWriteUserId.Value] = FindUser2(r.LastWriteUserId));
                yield return r;
            }
        }

        public Relationship FindRelationship(Guid? id, bool load = false, bool cache = true)
        {
            var r = Relationship.FromJObject(FolioServiceClient.GetInstanceRelationship(id?.ToString()));
            if (r == null) return null;
            if (load && r.SuperInstanceId != null) r.SuperInstance = (Instance2)(cache && objects.ContainsKey(r.SuperInstanceId.Value) ? objects[r.SuperInstanceId.Value] : objects[r.SuperInstanceId.Value] = FindInstance2(r.SuperInstanceId));
            if (load && r.SubInstanceId != null) r.SubInstance = (Instance2)(cache && objects.ContainsKey(r.SubInstanceId.Value) ? objects[r.SubInstanceId.Value] : objects[r.SubInstanceId.Value] = FindInstance2(r.SubInstanceId));
            if (load && r.InstanceRelationshipTypeId != null) r.InstanceRelationshipType = (RelationshipType)(cache && objects.ContainsKey(r.InstanceRelationshipTypeId.Value) ? objects[r.InstanceRelationshipTypeId.Value] : objects[r.InstanceRelationshipTypeId.Value] = FindRelationshipType(r.InstanceRelationshipTypeId));
            if (load && r.CreationUserId != null) r.CreationUser = (User2)(cache && objects.ContainsKey(r.CreationUserId.Value) ? objects[r.CreationUserId.Value] : objects[r.CreationUserId.Value] = FindUser2(r.CreationUserId));
            if (load && r.LastWriteUserId != null) r.LastWriteUser = (User2)(cache && objects.ContainsKey(r.LastWriteUserId.Value) ? objects[r.LastWriteUserId.Value] : objects[r.LastWriteUserId.Value] = FindUser2(r.LastWriteUserId));
            return r;
        }

        public void Insert(Relationship relationship)
        {
            if (relationship.Id == null) relationship.Id = Guid.NewGuid();
            FolioServiceClient.InsertInstanceRelationship(relationship.ToJObject());
        }

        public void Update(Relationship relationship) => FolioServiceClient.UpdateInstanceRelationship(relationship.ToJObject());

        public void DeleteRelationship(Guid? id) => FolioServiceClient.DeleteInstanceRelationship(id?.ToString());

        public bool AnyRelationshipTypes(string where = null) => FolioServiceClient.AnyInstanceRelationshipTypes(where);

        public int CountRelationshipTypes(string where = null) => FolioServiceClient.CountInstanceRelationshipTypes(where);

        public RelationshipType[] RelationshipTypes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.InstanceRelationshipTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var rt = RelationshipType.FromJObject(jo);
                if (load && rt.CreationUserId != null) rt.CreationUser = (User2)(cache && objects.ContainsKey(rt.CreationUserId.Value) ? objects[rt.CreationUserId.Value] : objects[rt.CreationUserId.Value] = FindUser2(rt.CreationUserId));
                if (load && rt.LastWriteUserId != null) rt.LastWriteUser = (User2)(cache && objects.ContainsKey(rt.LastWriteUserId.Value) ? objects[rt.LastWriteUserId.Value] : objects[rt.LastWriteUserId.Value] = FindUser2(rt.LastWriteUserId));
                return rt;
            }).ToArray();
        }

        public IEnumerable<RelationshipType> RelationshipTypes(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.InstanceRelationshipTypes(where, orderBy, skip, take))
            {
                var rt = RelationshipType.FromJObject(jo);
                if (load && rt.CreationUserId != null) rt.CreationUser = (User2)(cache && objects.ContainsKey(rt.CreationUserId.Value) ? objects[rt.CreationUserId.Value] : objects[rt.CreationUserId.Value] = FindUser2(rt.CreationUserId));
                if (load && rt.LastWriteUserId != null) rt.LastWriteUser = (User2)(cache && objects.ContainsKey(rt.LastWriteUserId.Value) ? objects[rt.LastWriteUserId.Value] : objects[rt.LastWriteUserId.Value] = FindUser2(rt.LastWriteUserId));
                yield return rt;
            }
        }

        public RelationshipType FindRelationshipType(Guid? id, bool load = false, bool cache = true)
        {
            var rt = RelationshipType.FromJObject(FolioServiceClient.GetInstanceRelationshipType(id?.ToString()));
            if (rt == null) return null;
            if (load && rt.CreationUserId != null) rt.CreationUser = (User2)(cache && objects.ContainsKey(rt.CreationUserId.Value) ? objects[rt.CreationUserId.Value] : objects[rt.CreationUserId.Value] = FindUser2(rt.CreationUserId));
            if (load && rt.LastWriteUserId != null) rt.LastWriteUser = (User2)(cache && objects.ContainsKey(rt.LastWriteUserId.Value) ? objects[rt.LastWriteUserId.Value] : objects[rt.LastWriteUserId.Value] = FindUser2(rt.LastWriteUserId));
            return rt;
        }

        public void Insert(RelationshipType relationshipType)
        {
            if (relationshipType.Id == null) relationshipType.Id = Guid.NewGuid();
            FolioServiceClient.InsertInstanceRelationshipType(relationshipType.ToJObject());
        }

        public void Update(RelationshipType relationshipType) => FolioServiceClient.UpdateInstanceRelationshipType(relationshipType.ToJObject());

        public void DeleteRelationshipType(Guid? id) => FolioServiceClient.DeleteInstanceRelationshipType(id?.ToString());

        public bool AnyReportingCode2s(string where = null) => FolioServiceClient.AnyReportingCodes(where);

        public int CountReportingCode2s(string where = null) => FolioServiceClient.CountReportingCodes(where);

        public ReportingCode2[] ReportingCode2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ReportingCodes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var rc2 = ReportingCode2.FromJObject(jo);
                return rc2;
            }).ToArray();
        }

        public IEnumerable<ReportingCode2> ReportingCode2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ReportingCodes(where, orderBy, skip, take))
            {
                var rc2 = ReportingCode2.FromJObject(jo);
                yield return rc2;
            }
        }

        public ReportingCode2 FindReportingCode2(Guid? id, bool load = false, bool cache = true) => ReportingCode2.FromJObject(FolioServiceClient.GetReportingCode(id?.ToString()));

        public void Insert(ReportingCode2 reportingCode2)
        {
            if (reportingCode2.Id == null) reportingCode2.Id = Guid.NewGuid();
            FolioServiceClient.InsertReportingCode(reportingCode2.ToJObject());
        }

        public void Update(ReportingCode2 reportingCode2) => FolioServiceClient.UpdateReportingCode(reportingCode2.ToJObject());

        public void DeleteReportingCode2(Guid? id) => FolioServiceClient.DeleteReportingCode(id?.ToString());

        public bool AnyRequest2s(string where = null) => FolioServiceClient.AnyRequests(where);

        public int CountRequest2s(string where = null) => FolioServiceClient.CountRequests(where);

        public Request2[] Request2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Requests(out count, where, orderBy, skip, take).Select(jo =>
            {
                var r2 = Request2.FromJObject(jo);
                if (load && r2.RequesterId != null) r2.Requester = (User2)(cache && objects.ContainsKey(r2.RequesterId.Value) ? objects[r2.RequesterId.Value] : objects[r2.RequesterId.Value] = FindUser2(r2.RequesterId));
                if (load && r2.ProxyUserId != null) r2.ProxyUser = (User2)(cache && objects.ContainsKey(r2.ProxyUserId.Value) ? objects[r2.ProxyUserId.Value] : objects[r2.ProxyUserId.Value] = FindUser2(r2.ProxyUserId));
                if (load && r2.ItemId != null) r2.Item = (Item2)(cache && objects.ContainsKey(r2.ItemId.Value) ? objects[r2.ItemId.Value] : objects[r2.ItemId.Value] = FindItem2(r2.ItemId));
                if (load && r2.CancellationReasonId != null) r2.CancellationReason = (CancellationReason2)(cache && objects.ContainsKey(r2.CancellationReasonId.Value) ? objects[r2.CancellationReasonId.Value] : objects[r2.CancellationReasonId.Value] = FindCancellationReason2(r2.CancellationReasonId));
                if (load && r2.CancelledByUserId != null) r2.CancelledByUser = (User2)(cache && objects.ContainsKey(r2.CancelledByUserId.Value) ? objects[r2.CancelledByUserId.Value] : objects[r2.CancelledByUserId.Value] = FindUser2(r2.CancelledByUserId));
                if (load && r2.DeliveryAddressTypeId != null) r2.DeliveryAddressType = (AddressType2)(cache && objects.ContainsKey(r2.DeliveryAddressTypeId.Value) ? objects[r2.DeliveryAddressTypeId.Value] : objects[r2.DeliveryAddressTypeId.Value] = FindAddressType2(r2.DeliveryAddressTypeId));
                if (load && r2.PickupServicePointId != null) r2.PickupServicePoint = (ServicePoint2)(cache && objects.ContainsKey(r2.PickupServicePointId.Value) ? objects[r2.PickupServicePointId.Value] : objects[r2.PickupServicePointId.Value] = FindServicePoint2(r2.PickupServicePointId));
                if (load && r2.CreationUserId != null) r2.CreationUser = (User2)(cache && objects.ContainsKey(r2.CreationUserId.Value) ? objects[r2.CreationUserId.Value] : objects[r2.CreationUserId.Value] = FindUser2(r2.CreationUserId));
                if (load && r2.LastWriteUserId != null) r2.LastWriteUser = (User2)(cache && objects.ContainsKey(r2.LastWriteUserId.Value) ? objects[r2.LastWriteUserId.Value] : objects[r2.LastWriteUserId.Value] = FindUser2(r2.LastWriteUserId));
                return r2;
            }).ToArray();
        }

        public IEnumerable<Request2> Request2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Requests(where, orderBy, skip, take))
            {
                var r2 = Request2.FromJObject(jo);
                if (load && r2.RequesterId != null) r2.Requester = (User2)(cache && objects.ContainsKey(r2.RequesterId.Value) ? objects[r2.RequesterId.Value] : objects[r2.RequesterId.Value] = FindUser2(r2.RequesterId));
                if (load && r2.ProxyUserId != null) r2.ProxyUser = (User2)(cache && objects.ContainsKey(r2.ProxyUserId.Value) ? objects[r2.ProxyUserId.Value] : objects[r2.ProxyUserId.Value] = FindUser2(r2.ProxyUserId));
                if (load && r2.ItemId != null) r2.Item = (Item2)(cache && objects.ContainsKey(r2.ItemId.Value) ? objects[r2.ItemId.Value] : objects[r2.ItemId.Value] = FindItem2(r2.ItemId));
                if (load && r2.CancellationReasonId != null) r2.CancellationReason = (CancellationReason2)(cache && objects.ContainsKey(r2.CancellationReasonId.Value) ? objects[r2.CancellationReasonId.Value] : objects[r2.CancellationReasonId.Value] = FindCancellationReason2(r2.CancellationReasonId));
                if (load && r2.CancelledByUserId != null) r2.CancelledByUser = (User2)(cache && objects.ContainsKey(r2.CancelledByUserId.Value) ? objects[r2.CancelledByUserId.Value] : objects[r2.CancelledByUserId.Value] = FindUser2(r2.CancelledByUserId));
                if (load && r2.DeliveryAddressTypeId != null) r2.DeliveryAddressType = (AddressType2)(cache && objects.ContainsKey(r2.DeliveryAddressTypeId.Value) ? objects[r2.DeliveryAddressTypeId.Value] : objects[r2.DeliveryAddressTypeId.Value] = FindAddressType2(r2.DeliveryAddressTypeId));
                if (load && r2.PickupServicePointId != null) r2.PickupServicePoint = (ServicePoint2)(cache && objects.ContainsKey(r2.PickupServicePointId.Value) ? objects[r2.PickupServicePointId.Value] : objects[r2.PickupServicePointId.Value] = FindServicePoint2(r2.PickupServicePointId));
                if (load && r2.CreationUserId != null) r2.CreationUser = (User2)(cache && objects.ContainsKey(r2.CreationUserId.Value) ? objects[r2.CreationUserId.Value] : objects[r2.CreationUserId.Value] = FindUser2(r2.CreationUserId));
                if (load && r2.LastWriteUserId != null) r2.LastWriteUser = (User2)(cache && objects.ContainsKey(r2.LastWriteUserId.Value) ? objects[r2.LastWriteUserId.Value] : objects[r2.LastWriteUserId.Value] = FindUser2(r2.LastWriteUserId));
                yield return r2;
            }
        }

        public Request2 FindRequest2(Guid? id, bool load = false, bool cache = true)
        {
            var r2 = Request2.FromJObject(FolioServiceClient.GetRequest(id?.ToString()));
            if (r2 == null) return null;
            if (load && r2.RequesterId != null) r2.Requester = (User2)(cache && objects.ContainsKey(r2.RequesterId.Value) ? objects[r2.RequesterId.Value] : objects[r2.RequesterId.Value] = FindUser2(r2.RequesterId));
            if (load && r2.ProxyUserId != null) r2.ProxyUser = (User2)(cache && objects.ContainsKey(r2.ProxyUserId.Value) ? objects[r2.ProxyUserId.Value] : objects[r2.ProxyUserId.Value] = FindUser2(r2.ProxyUserId));
            if (load && r2.ItemId != null) r2.Item = (Item2)(cache && objects.ContainsKey(r2.ItemId.Value) ? objects[r2.ItemId.Value] : objects[r2.ItemId.Value] = FindItem2(r2.ItemId));
            if (load && r2.CancellationReasonId != null) r2.CancellationReason = (CancellationReason2)(cache && objects.ContainsKey(r2.CancellationReasonId.Value) ? objects[r2.CancellationReasonId.Value] : objects[r2.CancellationReasonId.Value] = FindCancellationReason2(r2.CancellationReasonId));
            if (load && r2.CancelledByUserId != null) r2.CancelledByUser = (User2)(cache && objects.ContainsKey(r2.CancelledByUserId.Value) ? objects[r2.CancelledByUserId.Value] : objects[r2.CancelledByUserId.Value] = FindUser2(r2.CancelledByUserId));
            if (load && r2.DeliveryAddressTypeId != null) r2.DeliveryAddressType = (AddressType2)(cache && objects.ContainsKey(r2.DeliveryAddressTypeId.Value) ? objects[r2.DeliveryAddressTypeId.Value] : objects[r2.DeliveryAddressTypeId.Value] = FindAddressType2(r2.DeliveryAddressTypeId));
            if (load && r2.PickupServicePointId != null) r2.PickupServicePoint = (ServicePoint2)(cache && objects.ContainsKey(r2.PickupServicePointId.Value) ? objects[r2.PickupServicePointId.Value] : objects[r2.PickupServicePointId.Value] = FindServicePoint2(r2.PickupServicePointId));
            if (load && r2.CreationUserId != null) r2.CreationUser = (User2)(cache && objects.ContainsKey(r2.CreationUserId.Value) ? objects[r2.CreationUserId.Value] : objects[r2.CreationUserId.Value] = FindUser2(r2.CreationUserId));
            if (load && r2.LastWriteUserId != null) r2.LastWriteUser = (User2)(cache && objects.ContainsKey(r2.LastWriteUserId.Value) ? objects[r2.LastWriteUserId.Value] : objects[r2.LastWriteUserId.Value] = FindUser2(r2.LastWriteUserId));
            var i = 0;
            if (r2.RequestIdentifiers != null) foreach (var ri in r2.RequestIdentifiers)
                {
                    ri.Id = (++i).ToString();
                    ri.RequestId = r2.Id;
                }
            i = 0;
            if (r2.RequestTags != null) foreach (var rt in r2.RequestTags)
                {
                    rt.Id = (++i).ToString();
                    rt.RequestId = r2.Id;
                }
            return r2;
        }

        public void Insert(Request2 request2)
        {
            if (request2.Id == null) request2.Id = Guid.NewGuid();
            FolioServiceClient.InsertRequest(request2.ToJObject());
        }

        public void Update(Request2 request2) => FolioServiceClient.UpdateRequest(request2.ToJObject());

        public void DeleteRequest2(Guid? id) => FolioServiceClient.DeleteRequest(id?.ToString());

        public bool AnyRequestPolicy2s(string where = null) => FolioServiceClient.AnyRequestPolicies(where);

        public int CountRequestPolicy2s(string where = null) => FolioServiceClient.CountRequestPolicies(where);

        public RequestPolicy2[] RequestPolicy2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.RequestPolicies(out count, where, orderBy, skip, take).Select(jo =>
            {
                var rp2 = RequestPolicy2.FromJObject(jo);
                if (load && rp2.CreationUserId != null) rp2.CreationUser = (User2)(cache && objects.ContainsKey(rp2.CreationUserId.Value) ? objects[rp2.CreationUserId.Value] : objects[rp2.CreationUserId.Value] = FindUser2(rp2.CreationUserId));
                if (load && rp2.LastWriteUserId != null) rp2.LastWriteUser = (User2)(cache && objects.ContainsKey(rp2.LastWriteUserId.Value) ? objects[rp2.LastWriteUserId.Value] : objects[rp2.LastWriteUserId.Value] = FindUser2(rp2.LastWriteUserId));
                return rp2;
            }).ToArray();
        }

        public IEnumerable<RequestPolicy2> RequestPolicy2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.RequestPolicies(where, orderBy, skip, take))
            {
                var rp2 = RequestPolicy2.FromJObject(jo);
                if (load && rp2.CreationUserId != null) rp2.CreationUser = (User2)(cache && objects.ContainsKey(rp2.CreationUserId.Value) ? objects[rp2.CreationUserId.Value] : objects[rp2.CreationUserId.Value] = FindUser2(rp2.CreationUserId));
                if (load && rp2.LastWriteUserId != null) rp2.LastWriteUser = (User2)(cache && objects.ContainsKey(rp2.LastWriteUserId.Value) ? objects[rp2.LastWriteUserId.Value] : objects[rp2.LastWriteUserId.Value] = FindUser2(rp2.LastWriteUserId));
                yield return rp2;
            }
        }

        public RequestPolicy2 FindRequestPolicy2(Guid? id, bool load = false, bool cache = true)
        {
            var rp2 = RequestPolicy2.FromJObject(FolioServiceClient.GetRequestPolicy(id?.ToString()));
            if (rp2 == null) return null;
            if (load && rp2.CreationUserId != null) rp2.CreationUser = (User2)(cache && objects.ContainsKey(rp2.CreationUserId.Value) ? objects[rp2.CreationUserId.Value] : objects[rp2.CreationUserId.Value] = FindUser2(rp2.CreationUserId));
            if (load && rp2.LastWriteUserId != null) rp2.LastWriteUser = (User2)(cache && objects.ContainsKey(rp2.LastWriteUserId.Value) ? objects[rp2.LastWriteUserId.Value] : objects[rp2.LastWriteUserId.Value] = FindUser2(rp2.LastWriteUserId));
            var i = 0;
            if (rp2.RequestPolicyRequestTypes != null) foreach (var rprt in rp2.RequestPolicyRequestTypes)
                {
                    rprt.Id = (++i).ToString();
                    rprt.RequestPolicyId = rp2.Id;
                }
            return rp2;
        }

        public void Insert(RequestPolicy2 requestPolicy2)
        {
            if (requestPolicy2.Id == null) requestPolicy2.Id = Guid.NewGuid();
            FolioServiceClient.InsertRequestPolicy(requestPolicy2.ToJObject());
        }

        public void Update(RequestPolicy2 requestPolicy2) => FolioServiceClient.UpdateRequestPolicy(requestPolicy2.ToJObject());

        public void DeleteRequestPolicy2(Guid? id) => FolioServiceClient.DeleteRequestPolicy(id?.ToString());

        public bool AnyScheduledNotice2s(string where = null) => FolioServiceClient.AnyScheduledNotices(where);

        public int CountScheduledNotice2s(string where = null) => FolioServiceClient.CountScheduledNotices(where);

        public ScheduledNotice2[] ScheduledNotice2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ScheduledNotices(out count, where, orderBy, skip, take).Select(jo =>
            {
                var sn2 = ScheduledNotice2.FromJObject(jo);
                if (load && sn2.LoanId != null) sn2.Loan = (Loan2)(cache && objects.ContainsKey(sn2.LoanId.Value) ? objects[sn2.LoanId.Value] : objects[sn2.LoanId.Value] = FindLoan2(sn2.LoanId));
                if (load && sn2.RequestId != null) sn2.Request = (Request2)(cache && objects.ContainsKey(sn2.RequestId.Value) ? objects[sn2.RequestId.Value] : objects[sn2.RequestId.Value] = FindRequest2(sn2.RequestId));
                if (load && sn2.PaymentId != null) sn2.Payment = (Payment2)(cache && objects.ContainsKey(sn2.PaymentId.Value) ? objects[sn2.PaymentId.Value] : objects[sn2.PaymentId.Value] = FindPayment2(sn2.PaymentId));
                if (load && sn2.RecipientUserId != null) sn2.RecipientUser = (User2)(cache && objects.ContainsKey(sn2.RecipientUserId.Value) ? objects[sn2.RecipientUserId.Value] : objects[sn2.RecipientUserId.Value] = FindUser2(sn2.RecipientUserId));
                if (load && sn2.NoticeConfigTemplateId != null) sn2.NoticeConfigTemplate = (Template2)(cache && objects.ContainsKey(sn2.NoticeConfigTemplateId.Value) ? objects[sn2.NoticeConfigTemplateId.Value] : objects[sn2.NoticeConfigTemplateId.Value] = FindTemplate2(sn2.NoticeConfigTemplateId));
                if (load && sn2.CreationUserId != null) sn2.CreationUser = (User2)(cache && objects.ContainsKey(sn2.CreationUserId.Value) ? objects[sn2.CreationUserId.Value] : objects[sn2.CreationUserId.Value] = FindUser2(sn2.CreationUserId));
                if (load && sn2.LastWriteUserId != null) sn2.LastWriteUser = (User2)(cache && objects.ContainsKey(sn2.LastWriteUserId.Value) ? objects[sn2.LastWriteUserId.Value] : objects[sn2.LastWriteUserId.Value] = FindUser2(sn2.LastWriteUserId));
                return sn2;
            }).ToArray();
        }

        public IEnumerable<ScheduledNotice2> ScheduledNotice2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ScheduledNotices(where, orderBy, skip, take))
            {
                var sn2 = ScheduledNotice2.FromJObject(jo);
                if (load && sn2.LoanId != null) sn2.Loan = (Loan2)(cache && objects.ContainsKey(sn2.LoanId.Value) ? objects[sn2.LoanId.Value] : objects[sn2.LoanId.Value] = FindLoan2(sn2.LoanId));
                if (load && sn2.RequestId != null) sn2.Request = (Request2)(cache && objects.ContainsKey(sn2.RequestId.Value) ? objects[sn2.RequestId.Value] : objects[sn2.RequestId.Value] = FindRequest2(sn2.RequestId));
                if (load && sn2.PaymentId != null) sn2.Payment = (Payment2)(cache && objects.ContainsKey(sn2.PaymentId.Value) ? objects[sn2.PaymentId.Value] : objects[sn2.PaymentId.Value] = FindPayment2(sn2.PaymentId));
                if (load && sn2.RecipientUserId != null) sn2.RecipientUser = (User2)(cache && objects.ContainsKey(sn2.RecipientUserId.Value) ? objects[sn2.RecipientUserId.Value] : objects[sn2.RecipientUserId.Value] = FindUser2(sn2.RecipientUserId));
                if (load && sn2.NoticeConfigTemplateId != null) sn2.NoticeConfigTemplate = (Template2)(cache && objects.ContainsKey(sn2.NoticeConfigTemplateId.Value) ? objects[sn2.NoticeConfigTemplateId.Value] : objects[sn2.NoticeConfigTemplateId.Value] = FindTemplate2(sn2.NoticeConfigTemplateId));
                if (load && sn2.CreationUserId != null) sn2.CreationUser = (User2)(cache && objects.ContainsKey(sn2.CreationUserId.Value) ? objects[sn2.CreationUserId.Value] : objects[sn2.CreationUserId.Value] = FindUser2(sn2.CreationUserId));
                if (load && sn2.LastWriteUserId != null) sn2.LastWriteUser = (User2)(cache && objects.ContainsKey(sn2.LastWriteUserId.Value) ? objects[sn2.LastWriteUserId.Value] : objects[sn2.LastWriteUserId.Value] = FindUser2(sn2.LastWriteUserId));
                yield return sn2;
            }
        }

        public ScheduledNotice2 FindScheduledNotice2(Guid? id, bool load = false, bool cache = true)
        {
            var sn2 = ScheduledNotice2.FromJObject(FolioServiceClient.GetScheduledNotice(id?.ToString()));
            if (sn2 == null) return null;
            if (load && sn2.LoanId != null) sn2.Loan = (Loan2)(cache && objects.ContainsKey(sn2.LoanId.Value) ? objects[sn2.LoanId.Value] : objects[sn2.LoanId.Value] = FindLoan2(sn2.LoanId));
            if (load && sn2.RequestId != null) sn2.Request = (Request2)(cache && objects.ContainsKey(sn2.RequestId.Value) ? objects[sn2.RequestId.Value] : objects[sn2.RequestId.Value] = FindRequest2(sn2.RequestId));
            if (load && sn2.PaymentId != null) sn2.Payment = (Payment2)(cache && objects.ContainsKey(sn2.PaymentId.Value) ? objects[sn2.PaymentId.Value] : objects[sn2.PaymentId.Value] = FindPayment2(sn2.PaymentId));
            if (load && sn2.RecipientUserId != null) sn2.RecipientUser = (User2)(cache && objects.ContainsKey(sn2.RecipientUserId.Value) ? objects[sn2.RecipientUserId.Value] : objects[sn2.RecipientUserId.Value] = FindUser2(sn2.RecipientUserId));
            if (load && sn2.NoticeConfigTemplateId != null) sn2.NoticeConfigTemplate = (Template2)(cache && objects.ContainsKey(sn2.NoticeConfigTemplateId.Value) ? objects[sn2.NoticeConfigTemplateId.Value] : objects[sn2.NoticeConfigTemplateId.Value] = FindTemplate2(sn2.NoticeConfigTemplateId));
            if (load && sn2.CreationUserId != null) sn2.CreationUser = (User2)(cache && objects.ContainsKey(sn2.CreationUserId.Value) ? objects[sn2.CreationUserId.Value] : objects[sn2.CreationUserId.Value] = FindUser2(sn2.CreationUserId));
            if (load && sn2.LastWriteUserId != null) sn2.LastWriteUser = (User2)(cache && objects.ContainsKey(sn2.LastWriteUserId.Value) ? objects[sn2.LastWriteUserId.Value] : objects[sn2.LastWriteUserId.Value] = FindUser2(sn2.LastWriteUserId));
            return sn2;
        }

        public void Insert(ScheduledNotice2 scheduledNotice2)
        {
            if (scheduledNotice2.Id == null) scheduledNotice2.Id = Guid.NewGuid();
            FolioServiceClient.InsertScheduledNotice(scheduledNotice2.ToJObject());
        }

        public void Update(ScheduledNotice2 scheduledNotice2) => FolioServiceClient.UpdateScheduledNotice(scheduledNotice2.ToJObject());

        public void DeleteScheduledNotice2(Guid? id) => FolioServiceClient.DeleteScheduledNotice(id?.ToString());

        public bool AnyServicePoint2s(string where = null) => FolioServiceClient.AnyServicePoints(where);

        public int CountServicePoint2s(string where = null) => FolioServiceClient.CountServicePoints(where);

        public ServicePoint2[] ServicePoint2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ServicePoints(out count, where, orderBy, skip, take).Select(jo =>
            {
                var sp2 = ServicePoint2.FromJObject(jo);
                if (load && sp2.CreationUserId != null) sp2.CreationUser = (User2)(cache && objects.ContainsKey(sp2.CreationUserId.Value) ? objects[sp2.CreationUserId.Value] : objects[sp2.CreationUserId.Value] = FindUser2(sp2.CreationUserId));
                if (load && sp2.LastWriteUserId != null) sp2.LastWriteUser = (User2)(cache && objects.ContainsKey(sp2.LastWriteUserId.Value) ? objects[sp2.LastWriteUserId.Value] : objects[sp2.LastWriteUserId.Value] = FindUser2(sp2.LastWriteUserId));
                return sp2;
            }).ToArray();
        }

        public IEnumerable<ServicePoint2> ServicePoint2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ServicePoints(where, orderBy, skip, take))
            {
                var sp2 = ServicePoint2.FromJObject(jo);
                if (load && sp2.CreationUserId != null) sp2.CreationUser = (User2)(cache && objects.ContainsKey(sp2.CreationUserId.Value) ? objects[sp2.CreationUserId.Value] : objects[sp2.CreationUserId.Value] = FindUser2(sp2.CreationUserId));
                if (load && sp2.LastWriteUserId != null) sp2.LastWriteUser = (User2)(cache && objects.ContainsKey(sp2.LastWriteUserId.Value) ? objects[sp2.LastWriteUserId.Value] : objects[sp2.LastWriteUserId.Value] = FindUser2(sp2.LastWriteUserId));
                yield return sp2;
            }
        }

        public ServicePoint2 FindServicePoint2(Guid? id, bool load = false, bool cache = true)
        {
            var sp2 = ServicePoint2.FromJObject(FolioServiceClient.GetServicePoint(id?.ToString()));
            if (sp2 == null) return null;
            if (load && sp2.CreationUserId != null) sp2.CreationUser = (User2)(cache && objects.ContainsKey(sp2.CreationUserId.Value) ? objects[sp2.CreationUserId.Value] : objects[sp2.CreationUserId.Value] = FindUser2(sp2.CreationUserId));
            if (load && sp2.LastWriteUserId != null) sp2.LastWriteUser = (User2)(cache && objects.ContainsKey(sp2.LastWriteUserId.Value) ? objects[sp2.LastWriteUserId.Value] : objects[sp2.LastWriteUserId.Value] = FindUser2(sp2.LastWriteUserId));
            var i = 0;
            if (sp2.ServicePointStaffSlips != null) foreach (var spss in sp2.ServicePointStaffSlips)
                {
                    spss.Id = (++i).ToString();
                    spss.ServicePointId = sp2.Id;
                }
            return sp2;
        }

        public void Insert(ServicePoint2 servicePoint2)
        {
            if (servicePoint2.Id == null) servicePoint2.Id = Guid.NewGuid();
            FolioServiceClient.InsertServicePoint(servicePoint2.ToJObject());
        }

        public void Update(ServicePoint2 servicePoint2) => FolioServiceClient.UpdateServicePoint(servicePoint2.ToJObject());

        public void DeleteServicePoint2(Guid? id) => FolioServiceClient.DeleteServicePoint(id?.ToString());

        public bool AnyServicePointUser2s(string where = null) => FolioServiceClient.AnyServicePointUsers(where);

        public int CountServicePointUser2s(string where = null) => FolioServiceClient.CountServicePointUsers(where);

        public ServicePointUser2[] ServicePointUser2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ServicePointUsers(out count, where, orderBy, skip, take).Select(jo =>
            {
                var spu2 = ServicePointUser2.FromJObject(jo);
                if (load && spu2.UserId != null) spu2.User = (User2)(cache && objects.ContainsKey(spu2.UserId.Value) ? objects[spu2.UserId.Value] : objects[spu2.UserId.Value] = FindUser2(spu2.UserId));
                if (load && spu2.DefaultServicePointId != null) spu2.DefaultServicePoint = (ServicePoint2)(cache && objects.ContainsKey(spu2.DefaultServicePointId.Value) ? objects[spu2.DefaultServicePointId.Value] : objects[spu2.DefaultServicePointId.Value] = FindServicePoint2(spu2.DefaultServicePointId));
                if (load && spu2.CreationUserId != null) spu2.CreationUser = (User2)(cache && objects.ContainsKey(spu2.CreationUserId.Value) ? objects[spu2.CreationUserId.Value] : objects[spu2.CreationUserId.Value] = FindUser2(spu2.CreationUserId));
                if (load && spu2.LastWriteUserId != null) spu2.LastWriteUser = (User2)(cache && objects.ContainsKey(spu2.LastWriteUserId.Value) ? objects[spu2.LastWriteUserId.Value] : objects[spu2.LastWriteUserId.Value] = FindUser2(spu2.LastWriteUserId));
                return spu2;
            }).ToArray();
        }

        public IEnumerable<ServicePointUser2> ServicePointUser2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ServicePointUsers(where, orderBy, skip, take))
            {
                var spu2 = ServicePointUser2.FromJObject(jo);
                if (load && spu2.UserId != null) spu2.User = (User2)(cache && objects.ContainsKey(spu2.UserId.Value) ? objects[spu2.UserId.Value] : objects[spu2.UserId.Value] = FindUser2(spu2.UserId));
                if (load && spu2.DefaultServicePointId != null) spu2.DefaultServicePoint = (ServicePoint2)(cache && objects.ContainsKey(spu2.DefaultServicePointId.Value) ? objects[spu2.DefaultServicePointId.Value] : objects[spu2.DefaultServicePointId.Value] = FindServicePoint2(spu2.DefaultServicePointId));
                if (load && spu2.CreationUserId != null) spu2.CreationUser = (User2)(cache && objects.ContainsKey(spu2.CreationUserId.Value) ? objects[spu2.CreationUserId.Value] : objects[spu2.CreationUserId.Value] = FindUser2(spu2.CreationUserId));
                if (load && spu2.LastWriteUserId != null) spu2.LastWriteUser = (User2)(cache && objects.ContainsKey(spu2.LastWriteUserId.Value) ? objects[spu2.LastWriteUserId.Value] : objects[spu2.LastWriteUserId.Value] = FindUser2(spu2.LastWriteUserId));
                yield return spu2;
            }
        }

        public ServicePointUser2 FindServicePointUser2(Guid? id, bool load = false, bool cache = true)
        {
            var spu2 = ServicePointUser2.FromJObject(FolioServiceClient.GetServicePointUser(id?.ToString()));
            if (spu2 == null) return null;
            if (load && spu2.UserId != null) spu2.User = (User2)(cache && objects.ContainsKey(spu2.UserId.Value) ? objects[spu2.UserId.Value] : objects[spu2.UserId.Value] = FindUser2(spu2.UserId));
            if (load && spu2.DefaultServicePointId != null) spu2.DefaultServicePoint = (ServicePoint2)(cache && objects.ContainsKey(spu2.DefaultServicePointId.Value) ? objects[spu2.DefaultServicePointId.Value] : objects[spu2.DefaultServicePointId.Value] = FindServicePoint2(spu2.DefaultServicePointId));
            if (load && spu2.CreationUserId != null) spu2.CreationUser = (User2)(cache && objects.ContainsKey(spu2.CreationUserId.Value) ? objects[spu2.CreationUserId.Value] : objects[spu2.CreationUserId.Value] = FindUser2(spu2.CreationUserId));
            if (load && spu2.LastWriteUserId != null) spu2.LastWriteUser = (User2)(cache && objects.ContainsKey(spu2.LastWriteUserId.Value) ? objects[spu2.LastWriteUserId.Value] : objects[spu2.LastWriteUserId.Value] = FindUser2(spu2.LastWriteUserId));
            var i = 0;
            if (spu2.ServicePointUserServicePoints != null) foreach (var spusp in spu2.ServicePointUserServicePoints)
                {
                    spusp.Id = (++i).ToString();
                    spusp.ServicePointUserId = spu2.Id;
                }
            return spu2;
        }

        public void Insert(ServicePointUser2 servicePointUser2)
        {
            if (servicePointUser2.Id == null) servicePointUser2.Id = Guid.NewGuid();
            FolioServiceClient.InsertServicePointUser(servicePointUser2.ToJObject());
        }

        public void Update(ServicePointUser2 servicePointUser2) => FolioServiceClient.UpdateServicePointUser(servicePointUser2.ToJObject());

        public void DeleteServicePointUser2(Guid? id) => FolioServiceClient.DeleteServicePointUser(id?.ToString());

        public bool AnySettings(string where = null) => FolioServiceClient.AnySettings(where);

        public int CountSettings(string where = null) => FolioServiceClient.CountSettings(where);

        public Setting[] Settings(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Settings(out count, where, orderBy, skip, take).Select(jo =>
            {
                var s = Setting.FromJObject(jo);
                if (load && s.CreationUserId != null) s.CreationUser = (User2)(cache && objects.ContainsKey(s.CreationUserId.Value) ? objects[s.CreationUserId.Value] : objects[s.CreationUserId.Value] = FindUser2(s.CreationUserId));
                if (load && s.LastWriteUserId != null) s.LastWriteUser = (User2)(cache && objects.ContainsKey(s.LastWriteUserId.Value) ? objects[s.LastWriteUserId.Value] : objects[s.LastWriteUserId.Value] = FindUser2(s.LastWriteUserId));
                return s;
            }).ToArray();
        }

        public IEnumerable<Setting> Settings(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Settings(where, orderBy, skip, take))
            {
                var s = Setting.FromJObject(jo);
                if (load && s.CreationUserId != null) s.CreationUser = (User2)(cache && objects.ContainsKey(s.CreationUserId.Value) ? objects[s.CreationUserId.Value] : objects[s.CreationUserId.Value] = FindUser2(s.CreationUserId));
                if (load && s.LastWriteUserId != null) s.LastWriteUser = (User2)(cache && objects.ContainsKey(s.LastWriteUserId.Value) ? objects[s.LastWriteUserId.Value] : objects[s.LastWriteUserId.Value] = FindUser2(s.LastWriteUserId));
                yield return s;
            }
        }

        public Setting FindSetting(Guid? id, bool load = false, bool cache = true)
        {
            var s = Setting.FromJObject(FolioServiceClient.GetSetting(id?.ToString()));
            if (s == null) return null;
            if (load && s.CreationUserId != null) s.CreationUser = (User2)(cache && objects.ContainsKey(s.CreationUserId.Value) ? objects[s.CreationUserId.Value] : objects[s.CreationUserId.Value] = FindUser2(s.CreationUserId));
            if (load && s.LastWriteUserId != null) s.LastWriteUser = (User2)(cache && objects.ContainsKey(s.LastWriteUserId.Value) ? objects[s.LastWriteUserId.Value] : objects[s.LastWriteUserId.Value] = FindUser2(s.LastWriteUserId));
            return s;
        }

        public void Insert(Setting setting)
        {
            if (setting.Id == null) setting.Id = Guid.NewGuid();
            FolioServiceClient.InsertSetting(setting.ToJObject());
        }

        public void Update(Setting setting) => FolioServiceClient.UpdateSetting(setting.ToJObject());

        public void DeleteSetting(Guid? id) => FolioServiceClient.DeleteSetting(id?.ToString());

        public bool AnySnapshot2s(string where = null) => FolioServiceClient.AnySnapshots(where);

        public int CountSnapshot2s(string where = null) => FolioServiceClient.CountSnapshots(where);

        public Snapshot2[] Snapshot2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Snapshots(out count, where, orderBy, skip, take).Select(jo =>
            {
                var s2 = Snapshot2.FromJObject(jo);
                if (load && s2.CreationUserId != null) s2.CreationUser = (User2)(cache && objects.ContainsKey(s2.CreationUserId.Value) ? objects[s2.CreationUserId.Value] : objects[s2.CreationUserId.Value] = FindUser2(s2.CreationUserId));
                if (load && s2.LastWriteUserId != null) s2.LastWriteUser = (User2)(cache && objects.ContainsKey(s2.LastWriteUserId.Value) ? objects[s2.LastWriteUserId.Value] : objects[s2.LastWriteUserId.Value] = FindUser2(s2.LastWriteUserId));
                return s2;
            }).ToArray();
        }

        public IEnumerable<Snapshot2> Snapshot2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Snapshots(where, orderBy, skip, take))
            {
                var s2 = Snapshot2.FromJObject(jo);
                if (load && s2.CreationUserId != null) s2.CreationUser = (User2)(cache && objects.ContainsKey(s2.CreationUserId.Value) ? objects[s2.CreationUserId.Value] : objects[s2.CreationUserId.Value] = FindUser2(s2.CreationUserId));
                if (load && s2.LastWriteUserId != null) s2.LastWriteUser = (User2)(cache && objects.ContainsKey(s2.LastWriteUserId.Value) ? objects[s2.LastWriteUserId.Value] : objects[s2.LastWriteUserId.Value] = FindUser2(s2.LastWriteUserId));
                yield return s2;
            }
        }

        public Snapshot2 FindSnapshot2(Guid? id, bool load = false, bool cache = true)
        {
            var s2 = Snapshot2.FromJObject(FolioServiceClient.GetSnapshot(id?.ToString()));
            if (s2 == null) return null;
            if (load && s2.CreationUserId != null) s2.CreationUser = (User2)(cache && objects.ContainsKey(s2.CreationUserId.Value) ? objects[s2.CreationUserId.Value] : objects[s2.CreationUserId.Value] = FindUser2(s2.CreationUserId));
            if (load && s2.LastWriteUserId != null) s2.LastWriteUser = (User2)(cache && objects.ContainsKey(s2.LastWriteUserId.Value) ? objects[s2.LastWriteUserId.Value] : objects[s2.LastWriteUserId.Value] = FindUser2(s2.LastWriteUserId));
            return s2;
        }

        public void Insert(Snapshot2 snapshot2)
        {
            if (snapshot2.Id == null) snapshot2.Id = Guid.NewGuid();
            FolioServiceClient.InsertSnapshot(snapshot2.ToJObject());
        }

        public void Update(Snapshot2 snapshot2) => FolioServiceClient.UpdateSnapshot(snapshot2.ToJObject());

        public void DeleteSnapshot2(Guid? id) => FolioServiceClient.DeleteSnapshot(id?.ToString());

        public bool AnySource2s(string where = null) => FolioServiceClient.AnySources(where);

        public int CountSource2s(string where = null) => FolioServiceClient.CountSources(where);

        public Source2[] Source2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Sources(out count, where, orderBy, skip, take).Select(jo =>
            {
                var s2 = Source2.FromJObject(jo);
                if (load && s2.CreationUserId != null) s2.CreationUser = (User2)(cache && objects.ContainsKey(s2.CreationUserId.Value) ? objects[s2.CreationUserId.Value] : objects[s2.CreationUserId.Value] = FindUser2(s2.CreationUserId));
                if (load && s2.LastWriteUserId != null) s2.LastWriteUser = (User2)(cache && objects.ContainsKey(s2.LastWriteUserId.Value) ? objects[s2.LastWriteUserId.Value] : objects[s2.LastWriteUserId.Value] = FindUser2(s2.LastWriteUserId));
                return s2;
            }).ToArray();
        }

        public IEnumerable<Source2> Source2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Sources(where, orderBy, skip, take))
            {
                var s2 = Source2.FromJObject(jo);
                if (load && s2.CreationUserId != null) s2.CreationUser = (User2)(cache && objects.ContainsKey(s2.CreationUserId.Value) ? objects[s2.CreationUserId.Value] : objects[s2.CreationUserId.Value] = FindUser2(s2.CreationUserId));
                if (load && s2.LastWriteUserId != null) s2.LastWriteUser = (User2)(cache && objects.ContainsKey(s2.LastWriteUserId.Value) ? objects[s2.LastWriteUserId.Value] : objects[s2.LastWriteUserId.Value] = FindUser2(s2.LastWriteUserId));
                yield return s2;
            }
        }

        public Source2 FindSource2(Guid? id, bool load = false, bool cache = true)
        {
            var s2 = Source2.FromJObject(FolioServiceClient.GetSource(id?.ToString()));
            if (s2 == null) return null;
            if (load && s2.CreationUserId != null) s2.CreationUser = (User2)(cache && objects.ContainsKey(s2.CreationUserId.Value) ? objects[s2.CreationUserId.Value] : objects[s2.CreationUserId.Value] = FindUser2(s2.CreationUserId));
            if (load && s2.LastWriteUserId != null) s2.LastWriteUser = (User2)(cache && objects.ContainsKey(s2.LastWriteUserId.Value) ? objects[s2.LastWriteUserId.Value] : objects[s2.LastWriteUserId.Value] = FindUser2(s2.LastWriteUserId));
            return s2;
        }

        public void Insert(Source2 source2)
        {
            if (source2.Id == null) source2.Id = Guid.NewGuid();
            FolioServiceClient.InsertSource(source2.ToJObject());
        }

        public void Update(Source2 source2) => FolioServiceClient.UpdateSource(source2.ToJObject());

        public void DeleteSource2(Guid? id) => FolioServiceClient.DeleteSource(id?.ToString());

        public bool AnyStaffSlip2s(string where = null) => FolioServiceClient.AnyStaffSlips(where);

        public int CountStaffSlip2s(string where = null) => FolioServiceClient.CountStaffSlips(where);

        public StaffSlip2[] StaffSlip2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.StaffSlips(out count, where, orderBy, skip, take).Select(jo =>
            {
                var ss2 = StaffSlip2.FromJObject(jo);
                if (load && ss2.CreationUserId != null) ss2.CreationUser = (User2)(cache && objects.ContainsKey(ss2.CreationUserId.Value) ? objects[ss2.CreationUserId.Value] : objects[ss2.CreationUserId.Value] = FindUser2(ss2.CreationUserId));
                if (load && ss2.LastWriteUserId != null) ss2.LastWriteUser = (User2)(cache && objects.ContainsKey(ss2.LastWriteUserId.Value) ? objects[ss2.LastWriteUserId.Value] : objects[ss2.LastWriteUserId.Value] = FindUser2(ss2.LastWriteUserId));
                return ss2;
            }).ToArray();
        }

        public IEnumerable<StaffSlip2> StaffSlip2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.StaffSlips(where, orderBy, skip, take))
            {
                var ss2 = StaffSlip2.FromJObject(jo);
                if (load && ss2.CreationUserId != null) ss2.CreationUser = (User2)(cache && objects.ContainsKey(ss2.CreationUserId.Value) ? objects[ss2.CreationUserId.Value] : objects[ss2.CreationUserId.Value] = FindUser2(ss2.CreationUserId));
                if (load && ss2.LastWriteUserId != null) ss2.LastWriteUser = (User2)(cache && objects.ContainsKey(ss2.LastWriteUserId.Value) ? objects[ss2.LastWriteUserId.Value] : objects[ss2.LastWriteUserId.Value] = FindUser2(ss2.LastWriteUserId));
                yield return ss2;
            }
        }

        public StaffSlip2 FindStaffSlip2(Guid? id, bool load = false, bool cache = true)
        {
            var ss2 = StaffSlip2.FromJObject(FolioServiceClient.GetStaffSlip(id?.ToString()));
            if (ss2 == null) return null;
            if (load && ss2.CreationUserId != null) ss2.CreationUser = (User2)(cache && objects.ContainsKey(ss2.CreationUserId.Value) ? objects[ss2.CreationUserId.Value] : objects[ss2.CreationUserId.Value] = FindUser2(ss2.CreationUserId));
            if (load && ss2.LastWriteUserId != null) ss2.LastWriteUser = (User2)(cache && objects.ContainsKey(ss2.LastWriteUserId.Value) ? objects[ss2.LastWriteUserId.Value] : objects[ss2.LastWriteUserId.Value] = FindUser2(ss2.LastWriteUserId));
            return ss2;
        }

        public void Insert(StaffSlip2 staffSlip2)
        {
            if (staffSlip2.Id == null) staffSlip2.Id = Guid.NewGuid();
            FolioServiceClient.InsertStaffSlip(staffSlip2.ToJObject());
        }

        public void Update(StaffSlip2 staffSlip2) => FolioServiceClient.UpdateStaffSlip(staffSlip2.ToJObject());

        public void DeleteStaffSlip2(Guid? id) => FolioServiceClient.DeleteStaffSlip(id?.ToString());

        public bool AnyStatisticalCode2s(string where = null) => FolioServiceClient.AnyStatisticalCodes(where);

        public int CountStatisticalCode2s(string where = null) => FolioServiceClient.CountStatisticalCodes(where);

        public StatisticalCode2[] StatisticalCode2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.StatisticalCodes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var sc2 = StatisticalCode2.FromJObject(jo);
                if (load && sc2.StatisticalCodeTypeId != null) sc2.StatisticalCodeType = (StatisticalCodeType2)(cache && objects.ContainsKey(sc2.StatisticalCodeTypeId.Value) ? objects[sc2.StatisticalCodeTypeId.Value] : objects[sc2.StatisticalCodeTypeId.Value] = FindStatisticalCodeType2(sc2.StatisticalCodeTypeId));
                if (load && sc2.CreationUserId != null) sc2.CreationUser = (User2)(cache && objects.ContainsKey(sc2.CreationUserId.Value) ? objects[sc2.CreationUserId.Value] : objects[sc2.CreationUserId.Value] = FindUser2(sc2.CreationUserId));
                if (load && sc2.LastWriteUserId != null) sc2.LastWriteUser = (User2)(cache && objects.ContainsKey(sc2.LastWriteUserId.Value) ? objects[sc2.LastWriteUserId.Value] : objects[sc2.LastWriteUserId.Value] = FindUser2(sc2.LastWriteUserId));
                return sc2;
            }).ToArray();
        }

        public IEnumerable<StatisticalCode2> StatisticalCode2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.StatisticalCodes(where, orderBy, skip, take))
            {
                var sc2 = StatisticalCode2.FromJObject(jo);
                if (load && sc2.StatisticalCodeTypeId != null) sc2.StatisticalCodeType = (StatisticalCodeType2)(cache && objects.ContainsKey(sc2.StatisticalCodeTypeId.Value) ? objects[sc2.StatisticalCodeTypeId.Value] : objects[sc2.StatisticalCodeTypeId.Value] = FindStatisticalCodeType2(sc2.StatisticalCodeTypeId));
                if (load && sc2.CreationUserId != null) sc2.CreationUser = (User2)(cache && objects.ContainsKey(sc2.CreationUserId.Value) ? objects[sc2.CreationUserId.Value] : objects[sc2.CreationUserId.Value] = FindUser2(sc2.CreationUserId));
                if (load && sc2.LastWriteUserId != null) sc2.LastWriteUser = (User2)(cache && objects.ContainsKey(sc2.LastWriteUserId.Value) ? objects[sc2.LastWriteUserId.Value] : objects[sc2.LastWriteUserId.Value] = FindUser2(sc2.LastWriteUserId));
                yield return sc2;
            }
        }

        public StatisticalCode2 FindStatisticalCode2(Guid? id, bool load = false, bool cache = true)
        {
            var sc2 = StatisticalCode2.FromJObject(FolioServiceClient.GetStatisticalCode(id?.ToString()));
            if (sc2 == null) return null;
            if (load && sc2.StatisticalCodeTypeId != null) sc2.StatisticalCodeType = (StatisticalCodeType2)(cache && objects.ContainsKey(sc2.StatisticalCodeTypeId.Value) ? objects[sc2.StatisticalCodeTypeId.Value] : objects[sc2.StatisticalCodeTypeId.Value] = FindStatisticalCodeType2(sc2.StatisticalCodeTypeId));
            if (load && sc2.CreationUserId != null) sc2.CreationUser = (User2)(cache && objects.ContainsKey(sc2.CreationUserId.Value) ? objects[sc2.CreationUserId.Value] : objects[sc2.CreationUserId.Value] = FindUser2(sc2.CreationUserId));
            if (load && sc2.LastWriteUserId != null) sc2.LastWriteUser = (User2)(cache && objects.ContainsKey(sc2.LastWriteUserId.Value) ? objects[sc2.LastWriteUserId.Value] : objects[sc2.LastWriteUserId.Value] = FindUser2(sc2.LastWriteUserId));
            return sc2;
        }

        public void Insert(StatisticalCode2 statisticalCode2)
        {
            if (statisticalCode2.Id == null) statisticalCode2.Id = Guid.NewGuid();
            FolioServiceClient.InsertStatisticalCode(statisticalCode2.ToJObject());
        }

        public void Update(StatisticalCode2 statisticalCode2) => FolioServiceClient.UpdateStatisticalCode(statisticalCode2.ToJObject());

        public void DeleteStatisticalCode2(Guid? id) => FolioServiceClient.DeleteStatisticalCode(id?.ToString());

        public bool AnyStatisticalCodeType2s(string where = null) => FolioServiceClient.AnyStatisticalCodeTypes(where);

        public int CountStatisticalCodeType2s(string where = null) => FolioServiceClient.CountStatisticalCodeTypes(where);

        public StatisticalCodeType2[] StatisticalCodeType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.StatisticalCodeTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var sct2 = StatisticalCodeType2.FromJObject(jo);
                if (load && sct2.CreationUserId != null) sct2.CreationUser = (User2)(cache && objects.ContainsKey(sct2.CreationUserId.Value) ? objects[sct2.CreationUserId.Value] : objects[sct2.CreationUserId.Value] = FindUser2(sct2.CreationUserId));
                if (load && sct2.LastWriteUserId != null) sct2.LastWriteUser = (User2)(cache && objects.ContainsKey(sct2.LastWriteUserId.Value) ? objects[sct2.LastWriteUserId.Value] : objects[sct2.LastWriteUserId.Value] = FindUser2(sct2.LastWriteUserId));
                return sct2;
            }).ToArray();
        }

        public IEnumerable<StatisticalCodeType2> StatisticalCodeType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.StatisticalCodeTypes(where, orderBy, skip, take))
            {
                var sct2 = StatisticalCodeType2.FromJObject(jo);
                if (load && sct2.CreationUserId != null) sct2.CreationUser = (User2)(cache && objects.ContainsKey(sct2.CreationUserId.Value) ? objects[sct2.CreationUserId.Value] : objects[sct2.CreationUserId.Value] = FindUser2(sct2.CreationUserId));
                if (load && sct2.LastWriteUserId != null) sct2.LastWriteUser = (User2)(cache && objects.ContainsKey(sct2.LastWriteUserId.Value) ? objects[sct2.LastWriteUserId.Value] : objects[sct2.LastWriteUserId.Value] = FindUser2(sct2.LastWriteUserId));
                yield return sct2;
            }
        }

        public StatisticalCodeType2 FindStatisticalCodeType2(Guid? id, bool load = false, bool cache = true)
        {
            var sct2 = StatisticalCodeType2.FromJObject(FolioServiceClient.GetStatisticalCodeType(id?.ToString()));
            if (sct2 == null) return null;
            if (load && sct2.CreationUserId != null) sct2.CreationUser = (User2)(cache && objects.ContainsKey(sct2.CreationUserId.Value) ? objects[sct2.CreationUserId.Value] : objects[sct2.CreationUserId.Value] = FindUser2(sct2.CreationUserId));
            if (load && sct2.LastWriteUserId != null) sct2.LastWriteUser = (User2)(cache && objects.ContainsKey(sct2.LastWriteUserId.Value) ? objects[sct2.LastWriteUserId.Value] : objects[sct2.LastWriteUserId.Value] = FindUser2(sct2.LastWriteUserId));
            return sct2;
        }

        public void Insert(StatisticalCodeType2 statisticalCodeType2)
        {
            if (statisticalCodeType2.Id == null) statisticalCodeType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertStatisticalCodeType(statisticalCodeType2.ToJObject());
        }

        public void Update(StatisticalCodeType2 statisticalCodeType2) => FolioServiceClient.UpdateStatisticalCodeType(statisticalCodeType2.ToJObject());

        public void DeleteStatisticalCodeType2(Guid? id) => FolioServiceClient.DeleteStatisticalCodeType(id?.ToString());

        public bool AnyStatuses(string where = null) => FolioServiceClient.AnyInstanceStatuses(where);

        public int CountStatuses(string where = null) => FolioServiceClient.CountInstanceStatuses(where);

        public Status[] Statuses(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.InstanceStatuses(out count, where, orderBy, skip, take).Select(jo =>
            {
                var s = Status.FromJObject(jo);
                if (load && s.CreationUserId != null) s.CreationUser = (User2)(cache && objects.ContainsKey(s.CreationUserId.Value) ? objects[s.CreationUserId.Value] : objects[s.CreationUserId.Value] = FindUser2(s.CreationUserId));
                if (load && s.LastWriteUserId != null) s.LastWriteUser = (User2)(cache && objects.ContainsKey(s.LastWriteUserId.Value) ? objects[s.LastWriteUserId.Value] : objects[s.LastWriteUserId.Value] = FindUser2(s.LastWriteUserId));
                return s;
            }).ToArray();
        }

        public IEnumerable<Status> Statuses(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.InstanceStatuses(where, orderBy, skip, take))
            {
                var s = Status.FromJObject(jo);
                if (load && s.CreationUserId != null) s.CreationUser = (User2)(cache && objects.ContainsKey(s.CreationUserId.Value) ? objects[s.CreationUserId.Value] : objects[s.CreationUserId.Value] = FindUser2(s.CreationUserId));
                if (load && s.LastWriteUserId != null) s.LastWriteUser = (User2)(cache && objects.ContainsKey(s.LastWriteUserId.Value) ? objects[s.LastWriteUserId.Value] : objects[s.LastWriteUserId.Value] = FindUser2(s.LastWriteUserId));
                yield return s;
            }
        }

        public Status FindStatus(Guid? id, bool load = false, bool cache = true)
        {
            var s = Status.FromJObject(FolioServiceClient.GetInstanceStatus(id?.ToString()));
            if (s == null) return null;
            if (load && s.CreationUserId != null) s.CreationUser = (User2)(cache && objects.ContainsKey(s.CreationUserId.Value) ? objects[s.CreationUserId.Value] : objects[s.CreationUserId.Value] = FindUser2(s.CreationUserId));
            if (load && s.LastWriteUserId != null) s.LastWriteUser = (User2)(cache && objects.ContainsKey(s.LastWriteUserId.Value) ? objects[s.LastWriteUserId.Value] : objects[s.LastWriteUserId.Value] = FindUser2(s.LastWriteUserId));
            return s;
        }

        public void Insert(Status status)
        {
            if (status.Id == null) status.Id = Guid.NewGuid();
            FolioServiceClient.InsertInstanceStatus(status.ToJObject());
        }

        public void Update(Status status) => FolioServiceClient.UpdateInstanceStatus(status.ToJObject());

        public void DeleteStatus(Guid? id) => FolioServiceClient.DeleteInstanceStatus(id?.ToString());

        public bool AnySuffix2s(string where = null) => FolioServiceClient.AnySuffixes(where);

        public int CountSuffix2s(string where = null) => FolioServiceClient.CountSuffixes(where);

        public Suffix2[] Suffix2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Suffixes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var s2 = Suffix2.FromJObject(jo);
                return s2;
            }).ToArray();
        }

        public IEnumerable<Suffix2> Suffix2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Suffixes(where, orderBy, skip, take))
            {
                var s2 = Suffix2.FromJObject(jo);
                yield return s2;
            }
        }

        public Suffix2 FindSuffix2(Guid? id, bool load = false, bool cache = true) => Suffix2.FromJObject(FolioServiceClient.GetSuffix(id?.ToString()));

        public void Insert(Suffix2 suffix2)
        {
            if (suffix2.Id == null) suffix2.Id = Guid.NewGuid();
            FolioServiceClient.InsertSuffix(suffix2.ToJObject());
        }

        public void Update(Suffix2 suffix2) => FolioServiceClient.UpdateSuffix(suffix2.ToJObject());

        public void DeleteSuffix2(Guid? id) => FolioServiceClient.DeleteSuffix(id?.ToString());

        public bool AnyTag2s(string where = null) => FolioServiceClient.AnyTags(where);

        public int CountTag2s(string where = null) => FolioServiceClient.CountTags(where);

        public Tag2[] Tag2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Tags(out count, where, orderBy, skip, take).Select(jo =>
            {
                var t2 = Tag2.FromJObject(jo);
                if (load && t2.CreationUserId != null) t2.CreationUser = (User2)(cache && objects.ContainsKey(t2.CreationUserId.Value) ? objects[t2.CreationUserId.Value] : objects[t2.CreationUserId.Value] = FindUser2(t2.CreationUserId));
                if (load && t2.LastWriteUserId != null) t2.LastWriteUser = (User2)(cache && objects.ContainsKey(t2.LastWriteUserId.Value) ? objects[t2.LastWriteUserId.Value] : objects[t2.LastWriteUserId.Value] = FindUser2(t2.LastWriteUserId));
                return t2;
            }).ToArray();
        }

        public IEnumerable<Tag2> Tag2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Tags(where, orderBy, skip, take))
            {
                var t2 = Tag2.FromJObject(jo);
                if (load && t2.CreationUserId != null) t2.CreationUser = (User2)(cache && objects.ContainsKey(t2.CreationUserId.Value) ? objects[t2.CreationUserId.Value] : objects[t2.CreationUserId.Value] = FindUser2(t2.CreationUserId));
                if (load && t2.LastWriteUserId != null) t2.LastWriteUser = (User2)(cache && objects.ContainsKey(t2.LastWriteUserId.Value) ? objects[t2.LastWriteUserId.Value] : objects[t2.LastWriteUserId.Value] = FindUser2(t2.LastWriteUserId));
                yield return t2;
            }
        }

        public Tag2 FindTag2(Guid? id, bool load = false, bool cache = true)
        {
            var t2 = Tag2.FromJObject(FolioServiceClient.GetTag(id?.ToString()));
            if (t2 == null) return null;
            if (load && t2.CreationUserId != null) t2.CreationUser = (User2)(cache && objects.ContainsKey(t2.CreationUserId.Value) ? objects[t2.CreationUserId.Value] : objects[t2.CreationUserId.Value] = FindUser2(t2.CreationUserId));
            if (load && t2.LastWriteUserId != null) t2.LastWriteUser = (User2)(cache && objects.ContainsKey(t2.LastWriteUserId.Value) ? objects[t2.LastWriteUserId.Value] : objects[t2.LastWriteUserId.Value] = FindUser2(t2.LastWriteUserId));
            return t2;
        }

        public void Insert(Tag2 tag2)
        {
            if (tag2.Id == null) tag2.Id = Guid.NewGuid();
            FolioServiceClient.InsertTag(tag2.ToJObject());
        }

        public void Update(Tag2 tag2) => FolioServiceClient.UpdateTag(tag2.ToJObject());

        public void DeleteTag2(Guid? id) => FolioServiceClient.DeleteTag(id?.ToString());

        public bool AnyTemplate2s(string where = null) => FolioServiceClient.AnyTemplates(where);

        public int CountTemplate2s(string where = null) => FolioServiceClient.CountTemplates(where);

        public Template2[] Template2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Templates(out count, where, orderBy, skip, take).Select(jo =>
            {
                var t2 = Template2.FromJObject(jo);
                if (load && t2.CreationUserId != null) t2.CreationUser = (User2)(cache && objects.ContainsKey(t2.CreationUserId.Value) ? objects[t2.CreationUserId.Value] : objects[t2.CreationUserId.Value] = FindUser2(t2.CreationUserId));
                if (load && t2.LastWriteUserId != null) t2.LastWriteUser = (User2)(cache && objects.ContainsKey(t2.LastWriteUserId.Value) ? objects[t2.LastWriteUserId.Value] : objects[t2.LastWriteUserId.Value] = FindUser2(t2.LastWriteUserId));
                return t2;
            }).ToArray();
        }

        public IEnumerable<Template2> Template2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Templates(where, orderBy, skip, take))
            {
                var t2 = Template2.FromJObject(jo);
                if (load && t2.CreationUserId != null) t2.CreationUser = (User2)(cache && objects.ContainsKey(t2.CreationUserId.Value) ? objects[t2.CreationUserId.Value] : objects[t2.CreationUserId.Value] = FindUser2(t2.CreationUserId));
                if (load && t2.LastWriteUserId != null) t2.LastWriteUser = (User2)(cache && objects.ContainsKey(t2.LastWriteUserId.Value) ? objects[t2.LastWriteUserId.Value] : objects[t2.LastWriteUserId.Value] = FindUser2(t2.LastWriteUserId));
                yield return t2;
            }
        }

        public Template2 FindTemplate2(Guid? id, bool load = false, bool cache = true)
        {
            var t2 = Template2.FromJObject(FolioServiceClient.GetTemplate(id?.ToString()));
            if (t2 == null) return null;
            if (load && t2.CreationUserId != null) t2.CreationUser = (User2)(cache && objects.ContainsKey(t2.CreationUserId.Value) ? objects[t2.CreationUserId.Value] : objects[t2.CreationUserId.Value] = FindUser2(t2.CreationUserId));
            if (load && t2.LastWriteUserId != null) t2.LastWriteUser = (User2)(cache && objects.ContainsKey(t2.LastWriteUserId.Value) ? objects[t2.LastWriteUserId.Value] : objects[t2.LastWriteUserId.Value] = FindUser2(t2.LastWriteUserId));
            var i = 0;
            if (t2.TemplateOutputFormats != null) foreach (var tof in t2.TemplateOutputFormats)
                {
                    tof.Id = (++i).ToString();
                    tof.TemplateId = t2.Id;
                }
            return t2;
        }

        public void Insert(Template2 template2)
        {
            if (template2.Id == null) template2.Id = Guid.NewGuid();
            FolioServiceClient.InsertTemplate(template2.ToJObject());
        }

        public void Update(Template2 template2) => FolioServiceClient.UpdateTemplate(template2.ToJObject());

        public void DeleteTemplate2(Guid? id) => FolioServiceClient.DeleteTemplate(id?.ToString());

        public bool AnyTitle2s(string where = null) => FolioServiceClient.AnyTitles(where);

        public int CountTitle2s(string where = null) => FolioServiceClient.CountTitles(where);

        public Title2[] Title2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Titles(out count, where, orderBy, skip, take).Select(jo =>
            {
                var t2 = Title2.FromJObject(jo);
                if (load && t2.OrderItemId != null) t2.OrderItem = (OrderItem2)(cache && objects.ContainsKey(t2.OrderItemId.Value) ? objects[t2.OrderItemId.Value] : objects[t2.OrderItemId.Value] = FindOrderItem2(t2.OrderItemId));
                if (load && t2.InstanceId != null) t2.Instance = (Instance2)(cache && objects.ContainsKey(t2.InstanceId.Value) ? objects[t2.InstanceId.Value] : objects[t2.InstanceId.Value] = FindInstance2(t2.InstanceId));
                if (load && t2.CreationUserId != null) t2.CreationUser = (User2)(cache && objects.ContainsKey(t2.CreationUserId.Value) ? objects[t2.CreationUserId.Value] : objects[t2.CreationUserId.Value] = FindUser2(t2.CreationUserId));
                if (load && t2.LastWriteUserId != null) t2.LastWriteUser = (User2)(cache && objects.ContainsKey(t2.LastWriteUserId.Value) ? objects[t2.LastWriteUserId.Value] : objects[t2.LastWriteUserId.Value] = FindUser2(t2.LastWriteUserId));
                return t2;
            }).ToArray();
        }

        public IEnumerable<Title2> Title2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Titles(where, orderBy, skip, take))
            {
                var t2 = Title2.FromJObject(jo);
                if (load && t2.OrderItemId != null) t2.OrderItem = (OrderItem2)(cache && objects.ContainsKey(t2.OrderItemId.Value) ? objects[t2.OrderItemId.Value] : objects[t2.OrderItemId.Value] = FindOrderItem2(t2.OrderItemId));
                if (load && t2.InstanceId != null) t2.Instance = (Instance2)(cache && objects.ContainsKey(t2.InstanceId.Value) ? objects[t2.InstanceId.Value] : objects[t2.InstanceId.Value] = FindInstance2(t2.InstanceId));
                if (load && t2.CreationUserId != null) t2.CreationUser = (User2)(cache && objects.ContainsKey(t2.CreationUserId.Value) ? objects[t2.CreationUserId.Value] : objects[t2.CreationUserId.Value] = FindUser2(t2.CreationUserId));
                if (load && t2.LastWriteUserId != null) t2.LastWriteUser = (User2)(cache && objects.ContainsKey(t2.LastWriteUserId.Value) ? objects[t2.LastWriteUserId.Value] : objects[t2.LastWriteUserId.Value] = FindUser2(t2.LastWriteUserId));
                yield return t2;
            }
        }

        public Title2 FindTitle2(Guid? id, bool load = false, bool cache = true)
        {
            var t2 = Title2.FromJObject(FolioServiceClient.GetTitle(id?.ToString()));
            if (t2 == null) return null;
            if (load && t2.OrderItemId != null) t2.OrderItem = (OrderItem2)(cache && objects.ContainsKey(t2.OrderItemId.Value) ? objects[t2.OrderItemId.Value] : objects[t2.OrderItemId.Value] = FindOrderItem2(t2.OrderItemId));
            if (load && t2.InstanceId != null) t2.Instance = (Instance2)(cache && objects.ContainsKey(t2.InstanceId.Value) ? objects[t2.InstanceId.Value] : objects[t2.InstanceId.Value] = FindInstance2(t2.InstanceId));
            if (load && t2.CreationUserId != null) t2.CreationUser = (User2)(cache && objects.ContainsKey(t2.CreationUserId.Value) ? objects[t2.CreationUserId.Value] : objects[t2.CreationUserId.Value] = FindUser2(t2.CreationUserId));
            if (load && t2.LastWriteUserId != null) t2.LastWriteUser = (User2)(cache && objects.ContainsKey(t2.LastWriteUserId.Value) ? objects[t2.LastWriteUserId.Value] : objects[t2.LastWriteUserId.Value] = FindUser2(t2.LastWriteUserId));
            var i = 0;
            if (t2.TitleContributors != null) foreach (var tc in t2.TitleContributors)
                {
                    tc.Id = (++i).ToString();
                    tc.TitleId = t2.Id;
                }
            i = 0;
            if (t2.TitleProductIds != null) foreach (var tpi in t2.TitleProductIds)
                {
                    tpi.Id = (++i).ToString();
                    tpi.TitleId = t2.Id;
                }
            return t2;
        }

        public void Insert(Title2 title2)
        {
            if (title2.Id == null) title2.Id = Guid.NewGuid();
            FolioServiceClient.InsertTitle(title2.ToJObject());
        }

        public void Update(Title2 title2) => FolioServiceClient.UpdateTitle(title2.ToJObject());

        public void DeleteTitle2(Guid? id) => FolioServiceClient.DeleteTitle(id?.ToString());

        public bool AnyTransaction2s(string where = null) => FolioServiceClient.AnyTransactions(where);

        public int CountTransaction2s(string where = null) => FolioServiceClient.CountTransactions(where);

        public Transaction2[] Transaction2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Transactions(out count, where, orderBy, skip, take).Select(jo =>
            {
                var t2 = Transaction2.FromJObject(jo);
                if (load && t2.AwaitingPaymentEncumbranceId != null) t2.AwaitingPaymentEncumbrance = (Transaction2)(cache && objects.ContainsKey(t2.AwaitingPaymentEncumbranceId.Value) ? objects[t2.AwaitingPaymentEncumbranceId.Value] : objects[t2.AwaitingPaymentEncumbranceId.Value] = FindTransaction2(t2.AwaitingPaymentEncumbranceId));
                if (load && t2.OrderId != null) t2.Order = (Order2)(cache && objects.ContainsKey(t2.OrderId.Value) ? objects[t2.OrderId.Value] : objects[t2.OrderId.Value] = FindOrder2(t2.OrderId));
                if (load && t2.OrderItemId != null) t2.OrderItem = (OrderItem2)(cache && objects.ContainsKey(t2.OrderItemId.Value) ? objects[t2.OrderItemId.Value] : objects[t2.OrderItemId.Value] = FindOrderItem2(t2.OrderItemId));
                if (load && t2.ExpenseClassId != null) t2.ExpenseClass = (ExpenseClass2)(cache && objects.ContainsKey(t2.ExpenseClassId.Value) ? objects[t2.ExpenseClassId.Value] : objects[t2.ExpenseClassId.Value] = FindExpenseClass2(t2.ExpenseClassId));
                if (load && t2.FiscalYearId != null) t2.FiscalYear = (FiscalYear2)(cache && objects.ContainsKey(t2.FiscalYearId.Value) ? objects[t2.FiscalYearId.Value] : objects[t2.FiscalYearId.Value] = FindFiscalYear2(t2.FiscalYearId));
                if (load && t2.FromFundId != null) t2.FromFund = (Fund2)(cache && objects.ContainsKey(t2.FromFundId.Value) ? objects[t2.FromFundId.Value] : objects[t2.FromFundId.Value] = FindFund2(t2.FromFundId));
                if (load && t2.PaymentEncumbranceId != null) t2.PaymentEncumbrance = (Transaction2)(cache && objects.ContainsKey(t2.PaymentEncumbranceId.Value) ? objects[t2.PaymentEncumbranceId.Value] : objects[t2.PaymentEncumbranceId.Value] = FindTransaction2(t2.PaymentEncumbranceId));
                if (load && t2.SourceFiscalYearId != null) t2.SourceFiscalYear = (FiscalYear2)(cache && objects.ContainsKey(t2.SourceFiscalYearId.Value) ? objects[t2.SourceFiscalYearId.Value] : objects[t2.SourceFiscalYearId.Value] = FindFiscalYear2(t2.SourceFiscalYearId));
                if (load && t2.InvoiceId != null) t2.Invoice = (Invoice2)(cache && objects.ContainsKey(t2.InvoiceId.Value) ? objects[t2.InvoiceId.Value] : objects[t2.InvoiceId.Value] = FindInvoice2(t2.InvoiceId));
                if (load && t2.InvoiceItemId != null) t2.InvoiceItem = (InvoiceItem2)(cache && objects.ContainsKey(t2.InvoiceItemId.Value) ? objects[t2.InvoiceItemId.Value] : objects[t2.InvoiceItemId.Value] = FindInvoiceItem2(t2.InvoiceItemId));
                if (load && t2.ToFundId != null) t2.ToFund = (Fund2)(cache && objects.ContainsKey(t2.ToFundId.Value) ? objects[t2.ToFundId.Value] : objects[t2.ToFundId.Value] = FindFund2(t2.ToFundId));
                if (load && t2.CreationUserId != null) t2.CreationUser = (User2)(cache && objects.ContainsKey(t2.CreationUserId.Value) ? objects[t2.CreationUserId.Value] : objects[t2.CreationUserId.Value] = FindUser2(t2.CreationUserId));
                if (load && t2.LastWriteUserId != null) t2.LastWriteUser = (User2)(cache && objects.ContainsKey(t2.LastWriteUserId.Value) ? objects[t2.LastWriteUserId.Value] : objects[t2.LastWriteUserId.Value] = FindUser2(t2.LastWriteUserId));
                return t2;
            }).ToArray();
        }

        public IEnumerable<Transaction2> Transaction2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Transactions(where, orderBy, skip, take))
            {
                var t2 = Transaction2.FromJObject(jo);
                if (load && t2.AwaitingPaymentEncumbranceId != null) t2.AwaitingPaymentEncumbrance = (Transaction2)(cache && objects.ContainsKey(t2.AwaitingPaymentEncumbranceId.Value) ? objects[t2.AwaitingPaymentEncumbranceId.Value] : objects[t2.AwaitingPaymentEncumbranceId.Value] = FindTransaction2(t2.AwaitingPaymentEncumbranceId));
                if (load && t2.OrderId != null) t2.Order = (Order2)(cache && objects.ContainsKey(t2.OrderId.Value) ? objects[t2.OrderId.Value] : objects[t2.OrderId.Value] = FindOrder2(t2.OrderId));
                if (load && t2.OrderItemId != null) t2.OrderItem = (OrderItem2)(cache && objects.ContainsKey(t2.OrderItemId.Value) ? objects[t2.OrderItemId.Value] : objects[t2.OrderItemId.Value] = FindOrderItem2(t2.OrderItemId));
                if (load && t2.ExpenseClassId != null) t2.ExpenseClass = (ExpenseClass2)(cache && objects.ContainsKey(t2.ExpenseClassId.Value) ? objects[t2.ExpenseClassId.Value] : objects[t2.ExpenseClassId.Value] = FindExpenseClass2(t2.ExpenseClassId));
                if (load && t2.FiscalYearId != null) t2.FiscalYear = (FiscalYear2)(cache && objects.ContainsKey(t2.FiscalYearId.Value) ? objects[t2.FiscalYearId.Value] : objects[t2.FiscalYearId.Value] = FindFiscalYear2(t2.FiscalYearId));
                if (load && t2.FromFundId != null) t2.FromFund = (Fund2)(cache && objects.ContainsKey(t2.FromFundId.Value) ? objects[t2.FromFundId.Value] : objects[t2.FromFundId.Value] = FindFund2(t2.FromFundId));
                if (load && t2.PaymentEncumbranceId != null) t2.PaymentEncumbrance = (Transaction2)(cache && objects.ContainsKey(t2.PaymentEncumbranceId.Value) ? objects[t2.PaymentEncumbranceId.Value] : objects[t2.PaymentEncumbranceId.Value] = FindTransaction2(t2.PaymentEncumbranceId));
                if (load && t2.SourceFiscalYearId != null) t2.SourceFiscalYear = (FiscalYear2)(cache && objects.ContainsKey(t2.SourceFiscalYearId.Value) ? objects[t2.SourceFiscalYearId.Value] : objects[t2.SourceFiscalYearId.Value] = FindFiscalYear2(t2.SourceFiscalYearId));
                if (load && t2.InvoiceId != null) t2.Invoice = (Invoice2)(cache && objects.ContainsKey(t2.InvoiceId.Value) ? objects[t2.InvoiceId.Value] : objects[t2.InvoiceId.Value] = FindInvoice2(t2.InvoiceId));
                if (load && t2.InvoiceItemId != null) t2.InvoiceItem = (InvoiceItem2)(cache && objects.ContainsKey(t2.InvoiceItemId.Value) ? objects[t2.InvoiceItemId.Value] : objects[t2.InvoiceItemId.Value] = FindInvoiceItem2(t2.InvoiceItemId));
                if (load && t2.ToFundId != null) t2.ToFund = (Fund2)(cache && objects.ContainsKey(t2.ToFundId.Value) ? objects[t2.ToFundId.Value] : objects[t2.ToFundId.Value] = FindFund2(t2.ToFundId));
                if (load && t2.CreationUserId != null) t2.CreationUser = (User2)(cache && objects.ContainsKey(t2.CreationUserId.Value) ? objects[t2.CreationUserId.Value] : objects[t2.CreationUserId.Value] = FindUser2(t2.CreationUserId));
                if (load && t2.LastWriteUserId != null) t2.LastWriteUser = (User2)(cache && objects.ContainsKey(t2.LastWriteUserId.Value) ? objects[t2.LastWriteUserId.Value] : objects[t2.LastWriteUserId.Value] = FindUser2(t2.LastWriteUserId));
                yield return t2;
            }
        }

        public Transaction2 FindTransaction2(Guid? id, bool load = false, bool cache = true)
        {
            var t2 = Transaction2.FromJObject(FolioServiceClient.GetTransaction(id?.ToString()));
            if (t2 == null) return null;
            if (load && t2.AwaitingPaymentEncumbranceId != null) t2.AwaitingPaymentEncumbrance = (Transaction2)(cache && objects.ContainsKey(t2.AwaitingPaymentEncumbranceId.Value) ? objects[t2.AwaitingPaymentEncumbranceId.Value] : objects[t2.AwaitingPaymentEncumbranceId.Value] = FindTransaction2(t2.AwaitingPaymentEncumbranceId));
            if (load && t2.OrderId != null) t2.Order = (Order2)(cache && objects.ContainsKey(t2.OrderId.Value) ? objects[t2.OrderId.Value] : objects[t2.OrderId.Value] = FindOrder2(t2.OrderId));
            if (load && t2.OrderItemId != null) t2.OrderItem = (OrderItem2)(cache && objects.ContainsKey(t2.OrderItemId.Value) ? objects[t2.OrderItemId.Value] : objects[t2.OrderItemId.Value] = FindOrderItem2(t2.OrderItemId));
            if (load && t2.ExpenseClassId != null) t2.ExpenseClass = (ExpenseClass2)(cache && objects.ContainsKey(t2.ExpenseClassId.Value) ? objects[t2.ExpenseClassId.Value] : objects[t2.ExpenseClassId.Value] = FindExpenseClass2(t2.ExpenseClassId));
            if (load && t2.FiscalYearId != null) t2.FiscalYear = (FiscalYear2)(cache && objects.ContainsKey(t2.FiscalYearId.Value) ? objects[t2.FiscalYearId.Value] : objects[t2.FiscalYearId.Value] = FindFiscalYear2(t2.FiscalYearId));
            if (load && t2.FromFundId != null) t2.FromFund = (Fund2)(cache && objects.ContainsKey(t2.FromFundId.Value) ? objects[t2.FromFundId.Value] : objects[t2.FromFundId.Value] = FindFund2(t2.FromFundId));
            if (load && t2.PaymentEncumbranceId != null) t2.PaymentEncumbrance = (Transaction2)(cache && objects.ContainsKey(t2.PaymentEncumbranceId.Value) ? objects[t2.PaymentEncumbranceId.Value] : objects[t2.PaymentEncumbranceId.Value] = FindTransaction2(t2.PaymentEncumbranceId));
            if (load && t2.SourceFiscalYearId != null) t2.SourceFiscalYear = (FiscalYear2)(cache && objects.ContainsKey(t2.SourceFiscalYearId.Value) ? objects[t2.SourceFiscalYearId.Value] : objects[t2.SourceFiscalYearId.Value] = FindFiscalYear2(t2.SourceFiscalYearId));
            if (load && t2.InvoiceId != null) t2.Invoice = (Invoice2)(cache && objects.ContainsKey(t2.InvoiceId.Value) ? objects[t2.InvoiceId.Value] : objects[t2.InvoiceId.Value] = FindInvoice2(t2.InvoiceId));
            if (load && t2.InvoiceItemId != null) t2.InvoiceItem = (InvoiceItem2)(cache && objects.ContainsKey(t2.InvoiceItemId.Value) ? objects[t2.InvoiceItemId.Value] : objects[t2.InvoiceItemId.Value] = FindInvoiceItem2(t2.InvoiceItemId));
            if (load && t2.ToFundId != null) t2.ToFund = (Fund2)(cache && objects.ContainsKey(t2.ToFundId.Value) ? objects[t2.ToFundId.Value] : objects[t2.ToFundId.Value] = FindFund2(t2.ToFundId));
            if (load && t2.CreationUserId != null) t2.CreationUser = (User2)(cache && objects.ContainsKey(t2.CreationUserId.Value) ? objects[t2.CreationUserId.Value] : objects[t2.CreationUserId.Value] = FindUser2(t2.CreationUserId));
            if (load && t2.LastWriteUserId != null) t2.LastWriteUser = (User2)(cache && objects.ContainsKey(t2.LastWriteUserId.Value) ? objects[t2.LastWriteUserId.Value] : objects[t2.LastWriteUserId.Value] = FindUser2(t2.LastWriteUserId));
            var i = 0;
            if (t2.TransactionTags != null) foreach (var tt in t2.TransactionTags)
                {
                    tt.Id = (++i).ToString();
                    tt.TransactionId = t2.Id;
                }
            return t2;
        }

        public void Insert(Transaction2 transaction2)
        {
            if (transaction2.Id == null) transaction2.Id = Guid.NewGuid();
            FolioServiceClient.InsertTransaction(transaction2.ToJObject());
        }

        public void Update(Transaction2 transaction2) => FolioServiceClient.UpdateTransaction(transaction2.ToJObject());

        public void DeleteTransaction2(Guid? id) => FolioServiceClient.DeleteTransaction(id?.ToString());

        public bool AnyTransferAccount2s(string where = null) => FolioServiceClient.AnyTransferAccounts(where);

        public int CountTransferAccount2s(string where = null) => FolioServiceClient.CountTransferAccounts(where);

        public TransferAccount2[] TransferAccount2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.TransferAccounts(out count, where, orderBy, skip, take).Select(jo =>
            {
                var ta2 = TransferAccount2.FromJObject(jo);
                if (load && ta2.CreationUserId != null) ta2.CreationUser = (User2)(cache && objects.ContainsKey(ta2.CreationUserId.Value) ? objects[ta2.CreationUserId.Value] : objects[ta2.CreationUserId.Value] = FindUser2(ta2.CreationUserId));
                if (load && ta2.LastWriteUserId != null) ta2.LastWriteUser = (User2)(cache && objects.ContainsKey(ta2.LastWriteUserId.Value) ? objects[ta2.LastWriteUserId.Value] : objects[ta2.LastWriteUserId.Value] = FindUser2(ta2.LastWriteUserId));
                if (load && ta2.OwnerId != null) ta2.Owner = (Owner2)(cache && objects.ContainsKey(ta2.OwnerId.Value) ? objects[ta2.OwnerId.Value] : objects[ta2.OwnerId.Value] = FindOwner2(ta2.OwnerId));
                return ta2;
            }).ToArray();
        }

        public IEnumerable<TransferAccount2> TransferAccount2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.TransferAccounts(where, orderBy, skip, take))
            {
                var ta2 = TransferAccount2.FromJObject(jo);
                if (load && ta2.CreationUserId != null) ta2.CreationUser = (User2)(cache && objects.ContainsKey(ta2.CreationUserId.Value) ? objects[ta2.CreationUserId.Value] : objects[ta2.CreationUserId.Value] = FindUser2(ta2.CreationUserId));
                if (load && ta2.LastWriteUserId != null) ta2.LastWriteUser = (User2)(cache && objects.ContainsKey(ta2.LastWriteUserId.Value) ? objects[ta2.LastWriteUserId.Value] : objects[ta2.LastWriteUserId.Value] = FindUser2(ta2.LastWriteUserId));
                if (load && ta2.OwnerId != null) ta2.Owner = (Owner2)(cache && objects.ContainsKey(ta2.OwnerId.Value) ? objects[ta2.OwnerId.Value] : objects[ta2.OwnerId.Value] = FindOwner2(ta2.OwnerId));
                yield return ta2;
            }
        }

        public TransferAccount2 FindTransferAccount2(Guid? id, bool load = false, bool cache = true)
        {
            var ta2 = TransferAccount2.FromJObject(FolioServiceClient.GetTransferAccount(id?.ToString()));
            if (ta2 == null) return null;
            if (load && ta2.CreationUserId != null) ta2.CreationUser = (User2)(cache && objects.ContainsKey(ta2.CreationUserId.Value) ? objects[ta2.CreationUserId.Value] : objects[ta2.CreationUserId.Value] = FindUser2(ta2.CreationUserId));
            if (load && ta2.LastWriteUserId != null) ta2.LastWriteUser = (User2)(cache && objects.ContainsKey(ta2.LastWriteUserId.Value) ? objects[ta2.LastWriteUserId.Value] : objects[ta2.LastWriteUserId.Value] = FindUser2(ta2.LastWriteUserId));
            if (load && ta2.OwnerId != null) ta2.Owner = (Owner2)(cache && objects.ContainsKey(ta2.OwnerId.Value) ? objects[ta2.OwnerId.Value] : objects[ta2.OwnerId.Value] = FindOwner2(ta2.OwnerId));
            return ta2;
        }

        public void Insert(TransferAccount2 transferAccount2)
        {
            if (transferAccount2.Id == null) transferAccount2.Id = Guid.NewGuid();
            FolioServiceClient.InsertTransferAccount(transferAccount2.ToJObject());
        }

        public void Update(TransferAccount2 transferAccount2) => FolioServiceClient.UpdateTransferAccount(transferAccount2.ToJObject());

        public void DeleteTransferAccount2(Guid? id) => FolioServiceClient.DeleteTransferAccount(id?.ToString());

        public bool AnyTransferCriteria2s(string where = null) => FolioServiceClient.AnyTransferCriterias(where);

        public int CountTransferCriteria2s(string where = null) => FolioServiceClient.CountTransferCriterias(where);

        public TransferCriteria2[] TransferCriteria2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.TransferCriterias(out count, where, orderBy, skip, take).Select(jo =>
            {
                var tc2 = TransferCriteria2.FromJObject(jo);
                return tc2;
            }).ToArray();
        }

        public IEnumerable<TransferCriteria2> TransferCriteria2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.TransferCriterias(where, orderBy, skip, take))
            {
                var tc2 = TransferCriteria2.FromJObject(jo);
                yield return tc2;
            }
        }

        public TransferCriteria2 FindTransferCriteria2(Guid? id, bool load = false, bool cache = true) => TransferCriteria2.FromJObject(FolioServiceClient.GetTransferCriteria(id?.ToString()));

        public void Insert(TransferCriteria2 transferCriteria2)
        {
            if (transferCriteria2.Id == null) transferCriteria2.Id = Guid.NewGuid();
            FolioServiceClient.InsertTransferCriteria(transferCriteria2.ToJObject());
        }

        public void Update(TransferCriteria2 transferCriteria2) => FolioServiceClient.UpdateTransferCriteria(transferCriteria2.ToJObject());

        public void DeleteTransferCriteria2(Guid? id) => FolioServiceClient.DeleteTransferCriteria(id?.ToString());

        public bool AnyUser2s(string where = null) => FolioServiceClient.AnyUsers(where);

        public int CountUser2s(string where = null) => FolioServiceClient.CountUsers(where);

        public User2[] User2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Users(out count, where, orderBy, skip, take).Select(jo =>
            {
                var u2 = User2.FromJObject(jo);
                if (load && u2.GroupId != null) u2.Group = (Group2)(cache && objects.ContainsKey(u2.GroupId.Value) ? objects[u2.GroupId.Value] : objects[u2.GroupId.Value] = FindGroup2(u2.GroupId));
                if (load && u2.CreationUserId != null) u2.CreationUser = (User2)(cache && objects.ContainsKey(u2.CreationUserId.Value) ? objects[u2.CreationUserId.Value] : objects[u2.CreationUserId.Value] = FindUser2(u2.CreationUserId));
                if (load && u2.LastWriteUserId != null) u2.LastWriteUser = (User2)(cache && objects.ContainsKey(u2.LastWriteUserId.Value) ? objects[u2.LastWriteUserId.Value] : objects[u2.LastWriteUserId.Value] = FindUser2(u2.LastWriteUserId));
                return u2;
            }).ToArray();
        }

        public IEnumerable<User2> User2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Users(where, orderBy, skip, take))
            {
                var u2 = User2.FromJObject(jo);
                if (load && u2.GroupId != null) u2.Group = (Group2)(cache && objects.ContainsKey(u2.GroupId.Value) ? objects[u2.GroupId.Value] : objects[u2.GroupId.Value] = FindGroup2(u2.GroupId));
                if (load && u2.CreationUserId != null) u2.CreationUser = (User2)(cache && objects.ContainsKey(u2.CreationUserId.Value) ? objects[u2.CreationUserId.Value] : objects[u2.CreationUserId.Value] = FindUser2(u2.CreationUserId));
                if (load && u2.LastWriteUserId != null) u2.LastWriteUser = (User2)(cache && objects.ContainsKey(u2.LastWriteUserId.Value) ? objects[u2.LastWriteUserId.Value] : objects[u2.LastWriteUserId.Value] = FindUser2(u2.LastWriteUserId));
                yield return u2;
            }
        }

        public User2 FindUser2(Guid? id, bool load = false, bool cache = true)
        {
            var u2 = User2.FromJObject(FolioServiceClient.GetUser(id?.ToString()));
            if (u2 == null) return null;
            if (load && u2.GroupId != null) u2.Group = (Group2)(cache && objects.ContainsKey(u2.GroupId.Value) ? objects[u2.GroupId.Value] : objects[u2.GroupId.Value] = FindGroup2(u2.GroupId));
            if (load && u2.CreationUserId != null) u2.CreationUser = (User2)(cache && objects.ContainsKey(u2.CreationUserId.Value) ? objects[u2.CreationUserId.Value] : objects[u2.CreationUserId.Value] = FindUser2(u2.CreationUserId));
            if (load && u2.LastWriteUserId != null) u2.LastWriteUser = (User2)(cache && objects.ContainsKey(u2.LastWriteUserId.Value) ? objects[u2.LastWriteUserId.Value] : objects[u2.LastWriteUserId.Value] = FindUser2(u2.LastWriteUserId));
            var i = 0;
            if (u2.UserAddresses != null) foreach (var ua in u2.UserAddresses)
                {
                    ua.Id = (++i).ToString();
                    ua.UserId = u2.Id;
                }
            i = 0;
            if (u2.UserDepartments != null) foreach (var ud in u2.UserDepartments)
                {
                    ud.Id = (++i).ToString();
                    ud.UserId = u2.Id;
                }
            i = 0;
            if (u2.UserTags != null) foreach (var ut in u2.UserTags)
                {
                    ut.Id = (++i).ToString();
                    ut.UserId = u2.Id;
                }
            return u2;
        }

        public void Insert(User2 user2)
        {
            if (user2.Id == null) user2.Id = Guid.NewGuid();
            FolioServiceClient.InsertUser(user2.ToJObject());
        }

        public void Update(User2 user2) => FolioServiceClient.UpdateUser(user2.ToJObject());

        public void DeleteUser2(Guid? id) => FolioServiceClient.DeleteUser(id?.ToString());

        public bool AnyUserAcquisitionsUnit2s(string where = null) => FolioServiceClient.AnyUserAcquisitionsUnits(where);

        public int CountUserAcquisitionsUnit2s(string where = null) => FolioServiceClient.CountUserAcquisitionsUnits(where);

        public UserAcquisitionsUnit2[] UserAcquisitionsUnit2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.UserAcquisitionsUnits(out count, where, orderBy, skip, take).Select(jo =>
            {
                var uau2 = UserAcquisitionsUnit2.FromJObject(jo);
                if (load && uau2.UserId != null) uau2.User = (User2)(cache && objects.ContainsKey(uau2.UserId.Value) ? objects[uau2.UserId.Value] : objects[uau2.UserId.Value] = FindUser2(uau2.UserId));
                if (load && uau2.AcquisitionsUnitId != null) uau2.AcquisitionsUnit = (AcquisitionsUnit2)(cache && objects.ContainsKey(uau2.AcquisitionsUnitId.Value) ? objects[uau2.AcquisitionsUnitId.Value] : objects[uau2.AcquisitionsUnitId.Value] = FindAcquisitionsUnit2(uau2.AcquisitionsUnitId));
                if (load && uau2.CreationUserId != null) uau2.CreationUser = (User2)(cache && objects.ContainsKey(uau2.CreationUserId.Value) ? objects[uau2.CreationUserId.Value] : objects[uau2.CreationUserId.Value] = FindUser2(uau2.CreationUserId));
                if (load && uau2.LastWriteUserId != null) uau2.LastWriteUser = (User2)(cache && objects.ContainsKey(uau2.LastWriteUserId.Value) ? objects[uau2.LastWriteUserId.Value] : objects[uau2.LastWriteUserId.Value] = FindUser2(uau2.LastWriteUserId));
                return uau2;
            }).ToArray();
        }

        public IEnumerable<UserAcquisitionsUnit2> UserAcquisitionsUnit2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.UserAcquisitionsUnits(where, orderBy, skip, take))
            {
                var uau2 = UserAcquisitionsUnit2.FromJObject(jo);
                if (load && uau2.UserId != null) uau2.User = (User2)(cache && objects.ContainsKey(uau2.UserId.Value) ? objects[uau2.UserId.Value] : objects[uau2.UserId.Value] = FindUser2(uau2.UserId));
                if (load && uau2.AcquisitionsUnitId != null) uau2.AcquisitionsUnit = (AcquisitionsUnit2)(cache && objects.ContainsKey(uau2.AcquisitionsUnitId.Value) ? objects[uau2.AcquisitionsUnitId.Value] : objects[uau2.AcquisitionsUnitId.Value] = FindAcquisitionsUnit2(uau2.AcquisitionsUnitId));
                if (load && uau2.CreationUserId != null) uau2.CreationUser = (User2)(cache && objects.ContainsKey(uau2.CreationUserId.Value) ? objects[uau2.CreationUserId.Value] : objects[uau2.CreationUserId.Value] = FindUser2(uau2.CreationUserId));
                if (load && uau2.LastWriteUserId != null) uau2.LastWriteUser = (User2)(cache && objects.ContainsKey(uau2.LastWriteUserId.Value) ? objects[uau2.LastWriteUserId.Value] : objects[uau2.LastWriteUserId.Value] = FindUser2(uau2.LastWriteUserId));
                yield return uau2;
            }
        }

        public UserAcquisitionsUnit2 FindUserAcquisitionsUnit2(Guid? id, bool load = false, bool cache = true)
        {
            var uau2 = UserAcquisitionsUnit2.FromJObject(FolioServiceClient.GetUserAcquisitionsUnit(id?.ToString()));
            if (uau2 == null) return null;
            if (load && uau2.UserId != null) uau2.User = (User2)(cache && objects.ContainsKey(uau2.UserId.Value) ? objects[uau2.UserId.Value] : objects[uau2.UserId.Value] = FindUser2(uau2.UserId));
            if (load && uau2.AcquisitionsUnitId != null) uau2.AcquisitionsUnit = (AcquisitionsUnit2)(cache && objects.ContainsKey(uau2.AcquisitionsUnitId.Value) ? objects[uau2.AcquisitionsUnitId.Value] : objects[uau2.AcquisitionsUnitId.Value] = FindAcquisitionsUnit2(uau2.AcquisitionsUnitId));
            if (load && uau2.CreationUserId != null) uau2.CreationUser = (User2)(cache && objects.ContainsKey(uau2.CreationUserId.Value) ? objects[uau2.CreationUserId.Value] : objects[uau2.CreationUserId.Value] = FindUser2(uau2.CreationUserId));
            if (load && uau2.LastWriteUserId != null) uau2.LastWriteUser = (User2)(cache && objects.ContainsKey(uau2.LastWriteUserId.Value) ? objects[uau2.LastWriteUserId.Value] : objects[uau2.LastWriteUserId.Value] = FindUser2(uau2.LastWriteUserId));
            return uau2;
        }

        public void Insert(UserAcquisitionsUnit2 userAcquisitionsUnit2)
        {
            if (userAcquisitionsUnit2.Id == null) userAcquisitionsUnit2.Id = Guid.NewGuid();
            FolioServiceClient.InsertUserAcquisitionsUnit(userAcquisitionsUnit2.ToJObject());
        }

        public void Update(UserAcquisitionsUnit2 userAcquisitionsUnit2) => FolioServiceClient.UpdateUserAcquisitionsUnit(userAcquisitionsUnit2.ToJObject());

        public void DeleteUserAcquisitionsUnit2(Guid? id) => FolioServiceClient.DeleteUserAcquisitionsUnit(id?.ToString());

        public bool AnyUserRequestPreference2s(string where = null) => FolioServiceClient.AnyUserRequestPreferences(where);

        public int CountUserRequestPreference2s(string where = null) => FolioServiceClient.CountUserRequestPreferences(where);

        public UserRequestPreference2[] UserRequestPreference2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.UserRequestPreferences(out count, where, orderBy, skip, take).Select(jo =>
            {
                var urp2 = UserRequestPreference2.FromJObject(jo);
                if (load && urp2.UserId != null) urp2.User = (User2)(cache && objects.ContainsKey(urp2.UserId.Value) ? objects[urp2.UserId.Value] : objects[urp2.UserId.Value] = FindUser2(urp2.UserId));
                if (load && urp2.DefaultServicePointId != null) urp2.DefaultServicePoint = (ServicePoint2)(cache && objects.ContainsKey(urp2.DefaultServicePointId.Value) ? objects[urp2.DefaultServicePointId.Value] : objects[urp2.DefaultServicePointId.Value] = FindServicePoint2(urp2.DefaultServicePointId));
                if (load && urp2.DefaultDeliveryAddressTypeId != null) urp2.DefaultDeliveryAddressType = (AddressType2)(cache && objects.ContainsKey(urp2.DefaultDeliveryAddressTypeId.Value) ? objects[urp2.DefaultDeliveryAddressTypeId.Value] : objects[urp2.DefaultDeliveryAddressTypeId.Value] = FindAddressType2(urp2.DefaultDeliveryAddressTypeId));
                if (load && urp2.CreationUserId != null) urp2.CreationUser = (User2)(cache && objects.ContainsKey(urp2.CreationUserId.Value) ? objects[urp2.CreationUserId.Value] : objects[urp2.CreationUserId.Value] = FindUser2(urp2.CreationUserId));
                if (load && urp2.LastWriteUserId != null) urp2.LastWriteUser = (User2)(cache && objects.ContainsKey(urp2.LastWriteUserId.Value) ? objects[urp2.LastWriteUserId.Value] : objects[urp2.LastWriteUserId.Value] = FindUser2(urp2.LastWriteUserId));
                return urp2;
            }).ToArray();
        }

        public IEnumerable<UserRequestPreference2> UserRequestPreference2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.UserRequestPreferences(where, orderBy, skip, take))
            {
                var urp2 = UserRequestPreference2.FromJObject(jo);
                if (load && urp2.UserId != null) urp2.User = (User2)(cache && objects.ContainsKey(urp2.UserId.Value) ? objects[urp2.UserId.Value] : objects[urp2.UserId.Value] = FindUser2(urp2.UserId));
                if (load && urp2.DefaultServicePointId != null) urp2.DefaultServicePoint = (ServicePoint2)(cache && objects.ContainsKey(urp2.DefaultServicePointId.Value) ? objects[urp2.DefaultServicePointId.Value] : objects[urp2.DefaultServicePointId.Value] = FindServicePoint2(urp2.DefaultServicePointId));
                if (load && urp2.DefaultDeliveryAddressTypeId != null) urp2.DefaultDeliveryAddressType = (AddressType2)(cache && objects.ContainsKey(urp2.DefaultDeliveryAddressTypeId.Value) ? objects[urp2.DefaultDeliveryAddressTypeId.Value] : objects[urp2.DefaultDeliveryAddressTypeId.Value] = FindAddressType2(urp2.DefaultDeliveryAddressTypeId));
                if (load && urp2.CreationUserId != null) urp2.CreationUser = (User2)(cache && objects.ContainsKey(urp2.CreationUserId.Value) ? objects[urp2.CreationUserId.Value] : objects[urp2.CreationUserId.Value] = FindUser2(urp2.CreationUserId));
                if (load && urp2.LastWriteUserId != null) urp2.LastWriteUser = (User2)(cache && objects.ContainsKey(urp2.LastWriteUserId.Value) ? objects[urp2.LastWriteUserId.Value] : objects[urp2.LastWriteUserId.Value] = FindUser2(urp2.LastWriteUserId));
                yield return urp2;
            }
        }

        public UserRequestPreference2 FindUserRequestPreference2(Guid? id, bool load = false, bool cache = true)
        {
            var urp2 = UserRequestPreference2.FromJObject(FolioServiceClient.GetUserRequestPreference(id?.ToString()));
            if (urp2 == null) return null;
            if (load && urp2.UserId != null) urp2.User = (User2)(cache && objects.ContainsKey(urp2.UserId.Value) ? objects[urp2.UserId.Value] : objects[urp2.UserId.Value] = FindUser2(urp2.UserId));
            if (load && urp2.DefaultServicePointId != null) urp2.DefaultServicePoint = (ServicePoint2)(cache && objects.ContainsKey(urp2.DefaultServicePointId.Value) ? objects[urp2.DefaultServicePointId.Value] : objects[urp2.DefaultServicePointId.Value] = FindServicePoint2(urp2.DefaultServicePointId));
            if (load && urp2.DefaultDeliveryAddressTypeId != null) urp2.DefaultDeliveryAddressType = (AddressType2)(cache && objects.ContainsKey(urp2.DefaultDeliveryAddressTypeId.Value) ? objects[urp2.DefaultDeliveryAddressTypeId.Value] : objects[urp2.DefaultDeliveryAddressTypeId.Value] = FindAddressType2(urp2.DefaultDeliveryAddressTypeId));
            if (load && urp2.CreationUserId != null) urp2.CreationUser = (User2)(cache && objects.ContainsKey(urp2.CreationUserId.Value) ? objects[urp2.CreationUserId.Value] : objects[urp2.CreationUserId.Value] = FindUser2(urp2.CreationUserId));
            if (load && urp2.LastWriteUserId != null) urp2.LastWriteUser = (User2)(cache && objects.ContainsKey(urp2.LastWriteUserId.Value) ? objects[urp2.LastWriteUserId.Value] : objects[urp2.LastWriteUserId.Value] = FindUser2(urp2.LastWriteUserId));
            return urp2;
        }

        public void Insert(UserRequestPreference2 userRequestPreference2)
        {
            if (userRequestPreference2.Id == null) userRequestPreference2.Id = Guid.NewGuid();
            FolioServiceClient.InsertUserRequestPreference(userRequestPreference2.ToJObject());
        }

        public void Update(UserRequestPreference2 userRequestPreference2) => FolioServiceClient.UpdateUserRequestPreference(userRequestPreference2.ToJObject());

        public void DeleteUserRequestPreference2(Guid? id) => FolioServiceClient.DeleteUserRequestPreference(id?.ToString());

        public bool AnyVoucher2s(string where = null) => FolioServiceClient.AnyVouchers(where);

        public int CountVoucher2s(string where = null) => FolioServiceClient.CountVouchers(where);

        public Voucher2[] Voucher2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Vouchers(out count, where, orderBy, skip, take).Select(jo =>
            {
                var v2 = Voucher2.FromJObject(jo);
                if (load && v2.BatchGroupId != null) v2.BatchGroup = (BatchGroup2)(cache && objects.ContainsKey(v2.BatchGroupId.Value) ? objects[v2.BatchGroupId.Value] : objects[v2.BatchGroupId.Value] = FindBatchGroup2(v2.BatchGroupId));
                if (load && v2.InvoiceId != null) v2.Invoice = (Invoice2)(cache && objects.ContainsKey(v2.InvoiceId.Value) ? objects[v2.InvoiceId.Value] : objects[v2.InvoiceId.Value] = FindInvoice2(v2.InvoiceId));
                if (load && v2.VendorId != null) v2.Vendor = (Organization2)(cache && objects.ContainsKey(v2.VendorId.Value) ? objects[v2.VendorId.Value] : objects[v2.VendorId.Value] = FindOrganization2(v2.VendorId));
                if (load && v2.CreationUserId != null) v2.CreationUser = (User2)(cache && objects.ContainsKey(v2.CreationUserId.Value) ? objects[v2.CreationUserId.Value] : objects[v2.CreationUserId.Value] = FindUser2(v2.CreationUserId));
                if (load && v2.LastWriteUserId != null) v2.LastWriteUser = (User2)(cache && objects.ContainsKey(v2.LastWriteUserId.Value) ? objects[v2.LastWriteUserId.Value] : objects[v2.LastWriteUserId.Value] = FindUser2(v2.LastWriteUserId));
                return v2;
            }).ToArray();
        }

        public IEnumerable<Voucher2> Voucher2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Vouchers(where, orderBy, skip, take))
            {
                var v2 = Voucher2.FromJObject(jo);
                if (load && v2.BatchGroupId != null) v2.BatchGroup = (BatchGroup2)(cache && objects.ContainsKey(v2.BatchGroupId.Value) ? objects[v2.BatchGroupId.Value] : objects[v2.BatchGroupId.Value] = FindBatchGroup2(v2.BatchGroupId));
                if (load && v2.InvoiceId != null) v2.Invoice = (Invoice2)(cache && objects.ContainsKey(v2.InvoiceId.Value) ? objects[v2.InvoiceId.Value] : objects[v2.InvoiceId.Value] = FindInvoice2(v2.InvoiceId));
                if (load && v2.VendorId != null) v2.Vendor = (Organization2)(cache && objects.ContainsKey(v2.VendorId.Value) ? objects[v2.VendorId.Value] : objects[v2.VendorId.Value] = FindOrganization2(v2.VendorId));
                if (load && v2.CreationUserId != null) v2.CreationUser = (User2)(cache && objects.ContainsKey(v2.CreationUserId.Value) ? objects[v2.CreationUserId.Value] : objects[v2.CreationUserId.Value] = FindUser2(v2.CreationUserId));
                if (load && v2.LastWriteUserId != null) v2.LastWriteUser = (User2)(cache && objects.ContainsKey(v2.LastWriteUserId.Value) ? objects[v2.LastWriteUserId.Value] : objects[v2.LastWriteUserId.Value] = FindUser2(v2.LastWriteUserId));
                yield return v2;
            }
        }

        public Voucher2 FindVoucher2(Guid? id, bool load = false, bool cache = true)
        {
            var v2 = Voucher2.FromJObject(FolioServiceClient.GetVoucher(id?.ToString()));
            if (v2 == null) return null;
            if (load && v2.BatchGroupId != null) v2.BatchGroup = (BatchGroup2)(cache && objects.ContainsKey(v2.BatchGroupId.Value) ? objects[v2.BatchGroupId.Value] : objects[v2.BatchGroupId.Value] = FindBatchGroup2(v2.BatchGroupId));
            if (load && v2.InvoiceId != null) v2.Invoice = (Invoice2)(cache && objects.ContainsKey(v2.InvoiceId.Value) ? objects[v2.InvoiceId.Value] : objects[v2.InvoiceId.Value] = FindInvoice2(v2.InvoiceId));
            if (load && v2.VendorId != null) v2.Vendor = (Organization2)(cache && objects.ContainsKey(v2.VendorId.Value) ? objects[v2.VendorId.Value] : objects[v2.VendorId.Value] = FindOrganization2(v2.VendorId));
            if (load && v2.CreationUserId != null) v2.CreationUser = (User2)(cache && objects.ContainsKey(v2.CreationUserId.Value) ? objects[v2.CreationUserId.Value] : objects[v2.CreationUserId.Value] = FindUser2(v2.CreationUserId));
            if (load && v2.LastWriteUserId != null) v2.LastWriteUser = (User2)(cache && objects.ContainsKey(v2.LastWriteUserId.Value) ? objects[v2.LastWriteUserId.Value] : objects[v2.LastWriteUserId.Value] = FindUser2(v2.LastWriteUserId));
            var i = 0;
            if (v2.VoucherAcquisitionsUnits != null) foreach (var vau in v2.VoucherAcquisitionsUnits)
                {
                    vau.Id = (++i).ToString();
                    vau.VoucherId = v2.Id;
                }
            return v2;
        }

        public void Insert(Voucher2 voucher2)
        {
            if (voucher2.Id == null) voucher2.Id = Guid.NewGuid();
            FolioServiceClient.InsertVoucher(voucher2.ToJObject());
        }

        public void Update(Voucher2 voucher2) => FolioServiceClient.UpdateVoucher(voucher2.ToJObject());

        public void DeleteVoucher2(Guid? id) => FolioServiceClient.DeleteVoucher(id?.ToString());

        public bool AnyVoucherItem2s(string where = null) => FolioServiceClient.AnyVoucherItems(where);

        public int CountVoucherItem2s(string where = null) => FolioServiceClient.CountVoucherItems(where);

        public VoucherItem2[] VoucherItem2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.VoucherItems(out count, where, orderBy, skip, take).Select(jo =>
            {
                var vi2 = VoucherItem2.FromJObject(jo);
                if (load && vi2.SubTransactionId != null) vi2.SubTransaction = (Transaction2)(cache && objects.ContainsKey(vi2.SubTransactionId.Value) ? objects[vi2.SubTransactionId.Value] : objects[vi2.SubTransactionId.Value] = FindTransaction2(vi2.SubTransactionId));
                if (load && vi2.VoucherId != null) vi2.Voucher = (Voucher2)(cache && objects.ContainsKey(vi2.VoucherId.Value) ? objects[vi2.VoucherId.Value] : objects[vi2.VoucherId.Value] = FindVoucher2(vi2.VoucherId));
                if (load && vi2.CreationUserId != null) vi2.CreationUser = (User2)(cache && objects.ContainsKey(vi2.CreationUserId.Value) ? objects[vi2.CreationUserId.Value] : objects[vi2.CreationUserId.Value] = FindUser2(vi2.CreationUserId));
                if (load && vi2.LastWriteUserId != null) vi2.LastWriteUser = (User2)(cache && objects.ContainsKey(vi2.LastWriteUserId.Value) ? objects[vi2.LastWriteUserId.Value] : objects[vi2.LastWriteUserId.Value] = FindUser2(vi2.LastWriteUserId));
                return vi2;
            }).ToArray();
        }

        public IEnumerable<VoucherItem2> VoucherItem2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.VoucherItems(where, orderBy, skip, take))
            {
                var vi2 = VoucherItem2.FromJObject(jo);
                if (load && vi2.SubTransactionId != null) vi2.SubTransaction = (Transaction2)(cache && objects.ContainsKey(vi2.SubTransactionId.Value) ? objects[vi2.SubTransactionId.Value] : objects[vi2.SubTransactionId.Value] = FindTransaction2(vi2.SubTransactionId));
                if (load && vi2.VoucherId != null) vi2.Voucher = (Voucher2)(cache && objects.ContainsKey(vi2.VoucherId.Value) ? objects[vi2.VoucherId.Value] : objects[vi2.VoucherId.Value] = FindVoucher2(vi2.VoucherId));
                if (load && vi2.CreationUserId != null) vi2.CreationUser = (User2)(cache && objects.ContainsKey(vi2.CreationUserId.Value) ? objects[vi2.CreationUserId.Value] : objects[vi2.CreationUserId.Value] = FindUser2(vi2.CreationUserId));
                if (load && vi2.LastWriteUserId != null) vi2.LastWriteUser = (User2)(cache && objects.ContainsKey(vi2.LastWriteUserId.Value) ? objects[vi2.LastWriteUserId.Value] : objects[vi2.LastWriteUserId.Value] = FindUser2(vi2.LastWriteUserId));
                yield return vi2;
            }
        }

        public VoucherItem2 FindVoucherItem2(Guid? id, bool load = false, bool cache = true)
        {
            var vi2 = VoucherItem2.FromJObject(FolioServiceClient.GetVoucherItem(id?.ToString()));
            if (vi2 == null) return null;
            if (load && vi2.SubTransactionId != null) vi2.SubTransaction = (Transaction2)(cache && objects.ContainsKey(vi2.SubTransactionId.Value) ? objects[vi2.SubTransactionId.Value] : objects[vi2.SubTransactionId.Value] = FindTransaction2(vi2.SubTransactionId));
            if (load && vi2.VoucherId != null) vi2.Voucher = (Voucher2)(cache && objects.ContainsKey(vi2.VoucherId.Value) ? objects[vi2.VoucherId.Value] : objects[vi2.VoucherId.Value] = FindVoucher2(vi2.VoucherId));
            if (load && vi2.CreationUserId != null) vi2.CreationUser = (User2)(cache && objects.ContainsKey(vi2.CreationUserId.Value) ? objects[vi2.CreationUserId.Value] : objects[vi2.CreationUserId.Value] = FindUser2(vi2.CreationUserId));
            if (load && vi2.LastWriteUserId != null) vi2.LastWriteUser = (User2)(cache && objects.ContainsKey(vi2.LastWriteUserId.Value) ? objects[vi2.LastWriteUserId.Value] : objects[vi2.LastWriteUserId.Value] = FindUser2(vi2.LastWriteUserId));
            var i = 0;
            if (vi2.VoucherItemFunds != null) foreach (var vif in vi2.VoucherItemFunds)
                {
                    vif.Id = (++i).ToString();
                    vif.VoucherItemId = vi2.Id;
                }
            i = 0;
            if (vi2.VoucherItemInvoiceItems != null) foreach (var viii in vi2.VoucherItemInvoiceItems)
                {
                    viii.Id = (++i).ToString();
                    viii.VoucherItemId = vi2.Id;
                }
            return vi2;
        }

        public void Insert(VoucherItem2 voucherItem2)
        {
            if (voucherItem2.Id == null) voucherItem2.Id = Guid.NewGuid();
            FolioServiceClient.InsertVoucherItem(voucherItem2.ToJObject());
        }

        public void Update(VoucherItem2 voucherItem2) => FolioServiceClient.UpdateVoucherItem(voucherItem2.ToJObject());

        public void DeleteVoucherItem2(Guid? id) => FolioServiceClient.DeleteVoucherItem(id?.ToString());

        public bool AnyWaiveReason2s(string where = null) => FolioServiceClient.AnyWaiveReasons(where);

        public int CountWaiveReason2s(string where = null) => FolioServiceClient.CountWaiveReasons(where);

        public WaiveReason2[] WaiveReason2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            return FolioServiceClient.WaiveReasons(out count, where, orderBy, skip, take).Select(jo =>
            {
                var wr2 = WaiveReason2.FromJObject(jo);
                if (load && wr2.CreationUserId != null) wr2.CreationUser = (User2)(cache && objects.ContainsKey(wr2.CreationUserId.Value) ? objects[wr2.CreationUserId.Value] : objects[wr2.CreationUserId.Value] = FindUser2(wr2.CreationUserId));
                if (load && wr2.LastWriteUserId != null) wr2.LastWriteUser = (User2)(cache && objects.ContainsKey(wr2.LastWriteUserId.Value) ? objects[wr2.LastWriteUserId.Value] : objects[wr2.LastWriteUserId.Value] = FindUser2(wr2.LastWriteUserId));
                if (load && wr2.AccountId != null) wr2.Account = (Fee2)(cache && objects.ContainsKey(wr2.AccountId.Value) ? objects[wr2.AccountId.Value] : objects[wr2.AccountId.Value] = FindFee2(wr2.AccountId));
                return wr2;
            }).ToArray();
        }

        public IEnumerable<WaiveReason2> WaiveReason2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.WaiveReasons(where, orderBy, skip, take))
            {
                var wr2 = WaiveReason2.FromJObject(jo);
                if (load && wr2.CreationUserId != null) wr2.CreationUser = (User2)(cache && objects.ContainsKey(wr2.CreationUserId.Value) ? objects[wr2.CreationUserId.Value] : objects[wr2.CreationUserId.Value] = FindUser2(wr2.CreationUserId));
                if (load && wr2.LastWriteUserId != null) wr2.LastWriteUser = (User2)(cache && objects.ContainsKey(wr2.LastWriteUserId.Value) ? objects[wr2.LastWriteUserId.Value] : objects[wr2.LastWriteUserId.Value] = FindUser2(wr2.LastWriteUserId));
                if (load && wr2.AccountId != null) wr2.Account = (Fee2)(cache && objects.ContainsKey(wr2.AccountId.Value) ? objects[wr2.AccountId.Value] : objects[wr2.AccountId.Value] = FindFee2(wr2.AccountId));
                yield return wr2;
            }
        }

        public WaiveReason2 FindWaiveReason2(Guid? id, bool load = false, bool cache = true)
        {
            var wr2 = WaiveReason2.FromJObject(FolioServiceClient.GetWaiveReason(id?.ToString()));
            if (wr2 == null) return null;
            if (load && wr2.CreationUserId != null) wr2.CreationUser = (User2)(cache && objects.ContainsKey(wr2.CreationUserId.Value) ? objects[wr2.CreationUserId.Value] : objects[wr2.CreationUserId.Value] = FindUser2(wr2.CreationUserId));
            if (load && wr2.LastWriteUserId != null) wr2.LastWriteUser = (User2)(cache && objects.ContainsKey(wr2.LastWriteUserId.Value) ? objects[wr2.LastWriteUserId.Value] : objects[wr2.LastWriteUserId.Value] = FindUser2(wr2.LastWriteUserId));
            if (load && wr2.AccountId != null) wr2.Account = (Fee2)(cache && objects.ContainsKey(wr2.AccountId.Value) ? objects[wr2.AccountId.Value] : objects[wr2.AccountId.Value] = FindFee2(wr2.AccountId));
            return wr2;
        }

        public void Insert(WaiveReason2 waiveReason2)
        {
            if (waiveReason2.Id == null) waiveReason2.Id = Guid.NewGuid();
            FolioServiceClient.InsertWaiveReason(waiveReason2.ToJObject());
        }

        public void Update(WaiveReason2 waiveReason2) => FolioServiceClient.UpdateWaiveReason(waiveReason2.ToJObject());

        public void DeleteWaiveReason2(Guid? id) => FolioServiceClient.DeleteWaiveReason(id?.ToString());

        public void Dispose()
        {
            FolioServiceClient.Dispose();
        }
    }
}
