using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace FolioLibrary
{
    public class FolioServiceContext : IDisposable
    {
        private bool pool;
        public FolioServiceClient FolioServiceClient { get; set; }
        Dictionary<Guid, object> objects = new Dictionary<Guid, object>();

        public FolioServiceContext(string nameOrConnectionString = "FolioServiceClient", string accessToken = null, TimeSpan? timeout = null, bool pool = false)
        {
            FolioServiceClient = new FolioServiceClient(nameOrConnectionString, accessToken, timeout);
            this.pool = pool;
        }

        public bool AnyAcquisitionMethod2s(string where = null) => FolioServiceClient.AnyAcquisitionMethods(where);

        public int CountAcquisitionMethod2s(string where = null) => FolioServiceClient.CountAcquisitionMethods(where);

        public AcquisitionMethod2[] AcquisitionMethod2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.AcquisitionMethods(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var am2 = cache ? (AcquisitionMethod2)(objects.ContainsKey(id) ? objects[id] : objects[id] = AcquisitionMethod2.FromJObject(jo)) : AcquisitionMethod2.FromJObject(jo);
                if (load && am2.CreationUserId != null) am2.CreationUser = FindUser2(am2.CreationUserId, cache: cache);
                if (load && am2.LastWriteUserId != null) am2.LastWriteUser = FindUser2(am2.LastWriteUserId, cache: cache);
                return am2;
            }).ToArray();
        }

        public IEnumerable<AcquisitionMethod2> AcquisitionMethod2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.AcquisitionMethods(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var am2 = cache ? (AcquisitionMethod2)(objects.ContainsKey(id) ? objects[id] : objects[id] = AcquisitionMethod2.FromJObject(jo)) : AcquisitionMethod2.FromJObject(jo);
                if (load && am2.CreationUserId != null) am2.CreationUser = FindUser2(am2.CreationUserId, cache: cache);
                if (load && am2.LastWriteUserId != null) am2.LastWriteUser = FindUser2(am2.LastWriteUserId, cache: cache);
                yield return am2;
            }
        }

        public AcquisitionMethod2 FindAcquisitionMethod2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var am2 = cache ? (AcquisitionMethod2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = AcquisitionMethod2.FromJObject(FolioServiceClient.GetAcquisitionMethod(id?.ToString()))) : AcquisitionMethod2.FromJObject(FolioServiceClient.GetAcquisitionMethod(id?.ToString()));
            if (am2 == null) return null;
            if (load && am2.CreationUserId != null) am2.CreationUser = FindUser2(am2.CreationUserId, cache: cache);
            if (load && am2.LastWriteUserId != null) am2.LastWriteUser = FindUser2(am2.LastWriteUserId, cache: cache);
            return am2;
        }

        public void Insert(AcquisitionMethod2 acquisitionMethod2)
        {
            if (acquisitionMethod2.Id == null) acquisitionMethod2.Id = Guid.NewGuid();
            FolioServiceClient.InsertAcquisitionMethod(acquisitionMethod2.ToJObject());
        }

        public void Update(AcquisitionMethod2 acquisitionMethod2) => FolioServiceClient.UpdateAcquisitionMethod(acquisitionMethod2.ToJObject());

        public void UpdateOrInsert(AcquisitionMethod2 acquisitionMethod2)
        {
            if (acquisitionMethod2.Id == null)
                Insert(acquisitionMethod2);
            else
                try
                {
                    Update(acquisitionMethod2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(acquisitionMethod2); else throw;
                }
        }

        public void InsertOrUpdate(AcquisitionMethod2 acquisitionMethod2)
        {
            try
            {
                Insert(acquisitionMethod2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(acquisitionMethod2); else throw;
            }
        }

        public void DeleteAcquisitionMethod2(Guid? id) => FolioServiceClient.DeleteAcquisitionMethod(id?.ToString());

        public bool AnyAcquisitionsUnit2s(string where = null) => FolioServiceClient.AnyAcquisitionsUnits(where);

        public int CountAcquisitionsUnit2s(string where = null) => FolioServiceClient.CountAcquisitionsUnits(where);

        public AcquisitionsUnit2[] AcquisitionsUnit2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.AcquisitionsUnits(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var au2 = cache ? (AcquisitionsUnit2)(objects.ContainsKey(id) ? objects[id] : objects[id] = AcquisitionsUnit2.FromJObject(jo)) : AcquisitionsUnit2.FromJObject(jo);
                if (load && au2.CreationUserId != null) au2.CreationUser = FindUser2(au2.CreationUserId, cache: cache);
                if (load && au2.LastWriteUserId != null) au2.LastWriteUser = FindUser2(au2.LastWriteUserId, cache: cache);
                return au2;
            }).ToArray();
        }

        public IEnumerable<AcquisitionsUnit2> AcquisitionsUnit2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.AcquisitionsUnits(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var au2 = cache ? (AcquisitionsUnit2)(objects.ContainsKey(id) ? objects[id] : objects[id] = AcquisitionsUnit2.FromJObject(jo)) : AcquisitionsUnit2.FromJObject(jo);
                if (load && au2.CreationUserId != null) au2.CreationUser = FindUser2(au2.CreationUserId, cache: cache);
                if (load && au2.LastWriteUserId != null) au2.LastWriteUser = FindUser2(au2.LastWriteUserId, cache: cache);
                yield return au2;
            }
        }

        public AcquisitionsUnit2 FindAcquisitionsUnit2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var au2 = cache ? (AcquisitionsUnit2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = AcquisitionsUnit2.FromJObject(FolioServiceClient.GetAcquisitionsUnit(id?.ToString()))) : AcquisitionsUnit2.FromJObject(FolioServiceClient.GetAcquisitionsUnit(id?.ToString()));
            if (au2 == null) return null;
            if (load && au2.CreationUserId != null) au2.CreationUser = FindUser2(au2.CreationUserId, cache: cache);
            if (load && au2.LastWriteUserId != null) au2.LastWriteUser = FindUser2(au2.LastWriteUserId, cache: cache);
            return au2;
        }

        public void Insert(AcquisitionsUnit2 acquisitionsUnit2)
        {
            if (acquisitionsUnit2.Id == null) acquisitionsUnit2.Id = Guid.NewGuid();
            FolioServiceClient.InsertAcquisitionsUnit(acquisitionsUnit2.ToJObject());
        }

        public void Update(AcquisitionsUnit2 acquisitionsUnit2) => FolioServiceClient.UpdateAcquisitionsUnit(acquisitionsUnit2.ToJObject());

        public void UpdateOrInsert(AcquisitionsUnit2 acquisitionsUnit2)
        {
            if (acquisitionsUnit2.Id == null)
                Insert(acquisitionsUnit2);
            else
                try
                {
                    Update(acquisitionsUnit2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(acquisitionsUnit2); else throw;
                }
        }

        public void InsertOrUpdate(AcquisitionsUnit2 acquisitionsUnit2)
        {
            try
            {
                Insert(acquisitionsUnit2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(acquisitionsUnit2); else throw;
            }
        }

        public void DeleteAcquisitionsUnit2(Guid? id) => FolioServiceClient.DeleteAcquisitionsUnit(id?.ToString());

        public bool AnyActualCostRecord2s(string where = null) => FolioServiceClient.AnyActualCostRecords(where);

        public int CountActualCostRecord2s(string where = null) => FolioServiceClient.CountActualCostRecords(where);

        public ActualCostRecord2[] ActualCostRecord2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ActualCostRecords(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var acr2 = cache ? (ActualCostRecord2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ActualCostRecord2.FromJObject(jo)) : ActualCostRecord2.FromJObject(jo);
                if (load && acr2.UserGroupId != null) acr2.UserGroup = FindGroup2(acr2.UserGroupId, cache: cache);
                if (load && acr2.ItemMaterialTypeId != null) acr2.ItemMaterialType = FindMaterialType2(acr2.ItemMaterialTypeId, cache: cache);
                if (load && acr2.ItemPermanentLocationId != null) acr2.ItemPermanentLocation = FindLocation2(acr2.ItemPermanentLocationId, cache: cache);
                if (load && acr2.ItemEffectiveLocationId != null) acr2.ItemEffectiveLocation = FindLocation2(acr2.ItemEffectiveLocationId, cache: cache);
                if (load && acr2.ItemLoanTypeId != null) acr2.ItemLoanType = FindLoanType2(acr2.ItemLoanTypeId, cache: cache);
                if (load && acr2.ItemHoldingId != null) acr2.ItemHolding = FindHolding2(acr2.ItemHoldingId, cache: cache);
                if (load && acr2.FeeId != null) acr2.Fee = FindFee2(acr2.FeeId, cache: cache);
                if (load && acr2.OwnerId != null) acr2.Owner = FindOwner2(acr2.OwnerId, cache: cache);
                if (load && acr2.FeeTypeId != null) acr2.FeeType = FindFeeType2(acr2.FeeTypeId, cache: cache);
                if (load && acr2.CreationUserId != null) acr2.CreationUser = FindUser2(acr2.CreationUserId, cache: cache);
                if (load && acr2.LastWriteUserId != null) acr2.LastWriteUser = FindUser2(acr2.LastWriteUserId, cache: cache);
                return acr2;
            }).ToArray();
        }

        public IEnumerable<ActualCostRecord2> ActualCostRecord2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ActualCostRecords(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var acr2 = cache ? (ActualCostRecord2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ActualCostRecord2.FromJObject(jo)) : ActualCostRecord2.FromJObject(jo);
                if (load && acr2.UserGroupId != null) acr2.UserGroup = FindGroup2(acr2.UserGroupId, cache: cache);
                if (load && acr2.ItemMaterialTypeId != null) acr2.ItemMaterialType = FindMaterialType2(acr2.ItemMaterialTypeId, cache: cache);
                if (load && acr2.ItemPermanentLocationId != null) acr2.ItemPermanentLocation = FindLocation2(acr2.ItemPermanentLocationId, cache: cache);
                if (load && acr2.ItemEffectiveLocationId != null) acr2.ItemEffectiveLocation = FindLocation2(acr2.ItemEffectiveLocationId, cache: cache);
                if (load && acr2.ItemLoanTypeId != null) acr2.ItemLoanType = FindLoanType2(acr2.ItemLoanTypeId, cache: cache);
                if (load && acr2.ItemHoldingId != null) acr2.ItemHolding = FindHolding2(acr2.ItemHoldingId, cache: cache);
                if (load && acr2.FeeId != null) acr2.Fee = FindFee2(acr2.FeeId, cache: cache);
                if (load && acr2.OwnerId != null) acr2.Owner = FindOwner2(acr2.OwnerId, cache: cache);
                if (load && acr2.FeeTypeId != null) acr2.FeeType = FindFeeType2(acr2.FeeTypeId, cache: cache);
                if (load && acr2.CreationUserId != null) acr2.CreationUser = FindUser2(acr2.CreationUserId, cache: cache);
                if (load && acr2.LastWriteUserId != null) acr2.LastWriteUser = FindUser2(acr2.LastWriteUserId, cache: cache);
                yield return acr2;
            }
        }

        public ActualCostRecord2 FindActualCostRecord2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var acr2 = cache ? (ActualCostRecord2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = ActualCostRecord2.FromJObject(FolioServiceClient.GetActualCostRecord(id?.ToString()))) : ActualCostRecord2.FromJObject(FolioServiceClient.GetActualCostRecord(id?.ToString()));
            if (acr2 == null) return null;
            if (load && acr2.UserGroupId != null) acr2.UserGroup = FindGroup2(acr2.UserGroupId, cache: cache);
            if (load && acr2.ItemMaterialTypeId != null) acr2.ItemMaterialType = FindMaterialType2(acr2.ItemMaterialTypeId, cache: cache);
            if (load && acr2.ItemPermanentLocationId != null) acr2.ItemPermanentLocation = FindLocation2(acr2.ItemPermanentLocationId, cache: cache);
            if (load && acr2.ItemEffectiveLocationId != null) acr2.ItemEffectiveLocation = FindLocation2(acr2.ItemEffectiveLocationId, cache: cache);
            if (load && acr2.ItemLoanTypeId != null) acr2.ItemLoanType = FindLoanType2(acr2.ItemLoanTypeId, cache: cache);
            if (load && acr2.ItemHoldingId != null) acr2.ItemHolding = FindHolding2(acr2.ItemHoldingId, cache: cache);
            if (load && acr2.FeeId != null) acr2.Fee = FindFee2(acr2.FeeId, cache: cache);
            if (load && acr2.OwnerId != null) acr2.Owner = FindOwner2(acr2.OwnerId, cache: cache);
            if (load && acr2.FeeTypeId != null) acr2.FeeType = FindFeeType2(acr2.FeeTypeId, cache: cache);
            if (load && acr2.CreationUserId != null) acr2.CreationUser = FindUser2(acr2.CreationUserId, cache: cache);
            if (load && acr2.LastWriteUserId != null) acr2.LastWriteUser = FindUser2(acr2.LastWriteUserId, cache: cache);
            var i = 0;
            if (acr2.ActualCostRecordContributors != null) foreach (var acrc in acr2.ActualCostRecordContributors)
                {
                    acrc.Id = (++i).ToString();
                    acrc.ActualCostRecordId = acr2.Id;
                    acrc.ActualCostRecord = acr2;
                }
            i = 0;
            if (acr2.ActualCostRecordIdentifiers != null) foreach (var acri in acr2.ActualCostRecordIdentifiers)
                {
                    acri.Id = (++i).ToString();
                    acri.ActualCostRecordId = acr2.Id;
                    acri.ActualCostRecord = acr2;
                    if (load && acri.IdentifierTypeId != null) acri.IdentifierType1 = FindIdType2(acri.IdentifierTypeId, cache: cache);
                }
            return acr2;
        }

        public void Insert(ActualCostRecord2 actualCostRecord2)
        {
            if (actualCostRecord2.Id == null) actualCostRecord2.Id = Guid.NewGuid();
            FolioServiceClient.InsertActualCostRecord(actualCostRecord2.ToJObject());
        }

        public void Update(ActualCostRecord2 actualCostRecord2) => FolioServiceClient.UpdateActualCostRecord(actualCostRecord2.ToJObject());

        public void UpdateOrInsert(ActualCostRecord2 actualCostRecord2)
        {
            if (actualCostRecord2.Id == null)
                Insert(actualCostRecord2);
            else
                try
                {
                    Update(actualCostRecord2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(actualCostRecord2); else throw;
                }
        }

        public void InsertOrUpdate(ActualCostRecord2 actualCostRecord2)
        {
            try
            {
                Insert(actualCostRecord2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(actualCostRecord2); else throw;
            }
        }

        public void DeleteActualCostRecord2(Guid? id) => FolioServiceClient.DeleteActualCostRecord(id?.ToString());

        public bool AnyAddressType2s(string where = null) => FolioServiceClient.AnyAddressTypes(where);

        public int CountAddressType2s(string where = null) => FolioServiceClient.CountAddressTypes(where);

        public AddressType2[] AddressType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.AddressTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var at2 = cache ? (AddressType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = AddressType2.FromJObject(jo)) : AddressType2.FromJObject(jo);
                if (load && at2.CreationUserId != null) at2.CreationUser = FindUser2(at2.CreationUserId, cache: cache);
                if (load && at2.LastWriteUserId != null) at2.LastWriteUser = FindUser2(at2.LastWriteUserId, cache: cache);
                return at2;
            }).ToArray();
        }

        public IEnumerable<AddressType2> AddressType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.AddressTypes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var at2 = cache ? (AddressType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = AddressType2.FromJObject(jo)) : AddressType2.FromJObject(jo);
                if (load && at2.CreationUserId != null) at2.CreationUser = FindUser2(at2.CreationUserId, cache: cache);
                if (load && at2.LastWriteUserId != null) at2.LastWriteUser = FindUser2(at2.LastWriteUserId, cache: cache);
                yield return at2;
            }
        }

        public AddressType2 FindAddressType2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var at2 = cache ? (AddressType2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = AddressType2.FromJObject(FolioServiceClient.GetAddressType(id?.ToString()))) : AddressType2.FromJObject(FolioServiceClient.GetAddressType(id?.ToString()));
            if (at2 == null) return null;
            if (load && at2.CreationUserId != null) at2.CreationUser = FindUser2(at2.CreationUserId, cache: cache);
            if (load && at2.LastWriteUserId != null) at2.LastWriteUser = FindUser2(at2.LastWriteUserId, cache: cache);
            return at2;
        }

        public void Insert(AddressType2 addressType2)
        {
            if (addressType2.Id == null) addressType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertAddressType(addressType2.ToJObject());
        }

        public void Update(AddressType2 addressType2) => FolioServiceClient.UpdateAddressType(addressType2.ToJObject());

        public void UpdateOrInsert(AddressType2 addressType2)
        {
            if (addressType2.Id == null)
                Insert(addressType2);
            else
                try
                {
                    Update(addressType2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(addressType2); else throw;
                }
        }

        public void InsertOrUpdate(AddressType2 addressType2)
        {
            try
            {
                Insert(addressType2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(addressType2); else throw;
            }
        }

        public void DeleteAddressType2(Guid? id) => FolioServiceClient.DeleteAddressType(id?.ToString());

        public bool AnyAgreement2s(string where = null) => FolioServiceClient.AnyAgreements(where);

        public int CountAgreement2s(string where = null) => FolioServiceClient.CountAgreements(where);

        public Agreement2[] Agreement2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Agreements(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var a2 = cache ? (Agreement2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Agreement2.FromJObject(jo)) : Agreement2.FromJObject(jo);
                return a2;
            }).ToArray();
        }

        public IEnumerable<Agreement2> Agreement2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Agreements(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var a2 = cache ? (Agreement2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Agreement2.FromJObject(jo)) : Agreement2.FromJObject(jo);
                yield return a2;
            }
        }

        public Agreement2 FindAgreement2(Guid? id, bool load = false, bool cache = true) => Agreement2.FromJObject(FolioServiceClient.GetAgreement(id?.ToString()));

        public void Insert(Agreement2 agreement2)
        {
            if (agreement2.Id == null) agreement2.Id = Guid.NewGuid();
            var jo = FolioServiceClient.InsertAgreement(agreement2.ToJObject());
            agreement2.Id = (Guid?)jo["id"];
        }

        public void Update(Agreement2 agreement2) => FolioServiceClient.UpdateAgreement(agreement2.ToJObject());

        public void UpdateOrInsert(Agreement2 agreement2)
        {
            if (agreement2.Id == null)
                Insert(agreement2);
            else
                try
                {
                    Update(agreement2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(agreement2); else throw;
                }
        }

        public void InsertOrUpdate(Agreement2 agreement2)
        {
            try
            {
                Insert(agreement2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(agreement2); else throw;
            }
        }

        public void DeleteAgreement2(Guid? id) => FolioServiceClient.DeleteAgreement(id?.ToString());

        public bool AnyAgreementItem2s(string where = null) => FolioServiceClient.AnyAgreementItems(where);

        public int CountAgreementItem2s(string where = null) => FolioServiceClient.CountAgreementItems(where);

        public AgreementItem2[] AgreementItem2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.AgreementItems(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var ai2 = cache ? (AgreementItem2)(objects.ContainsKey(id) ? objects[id] : objects[id] = AgreementItem2.FromJObject(jo)) : AgreementItem2.FromJObject(jo);
                if (load && ai2.AgreementId != null) ai2.Agreement = FindAgreement2(ai2.AgreementId, cache: cache);
                return ai2;
            }).ToArray();
        }

        public IEnumerable<AgreementItem2> AgreementItem2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.AgreementItems(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var ai2 = cache ? (AgreementItem2)(objects.ContainsKey(id) ? objects[id] : objects[id] = AgreementItem2.FromJObject(jo)) : AgreementItem2.FromJObject(jo);
                if (load && ai2.AgreementId != null) ai2.Agreement = FindAgreement2(ai2.AgreementId, cache: cache);
                yield return ai2;
            }
        }

        public AgreementItem2 FindAgreementItem2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var ai2 = cache ? (AgreementItem2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = AgreementItem2.FromJObject(FolioServiceClient.GetAgreementItem(id?.ToString()))) : AgreementItem2.FromJObject(FolioServiceClient.GetAgreementItem(id?.ToString()));
            if (ai2 == null) return null;
            if (load && ai2.AgreementId != null) ai2.Agreement = FindAgreement2(ai2.AgreementId, cache: cache);
            var i = 0;
            if (ai2.AgreementItemOrderItems != null) foreach (var aioi in ai2.AgreementItemOrderItems)
                {
                    aioi.Id = (++i).ToString();
                    aioi.AgreementItemId = ai2.Id;
                    aioi.AgreementItem = ai2;
                    if (load && aioi.OrderItemId != null) aioi.OrderItem = FindOrderItem2(aioi.OrderItemId, cache: cache);
                }
            return ai2;
        }

        public void Insert(AgreementItem2 agreementItem2)
        {
            if (agreementItem2.Id == null) agreementItem2.Id = Guid.NewGuid();
            FolioServiceClient.InsertAgreementItem(agreementItem2.ToJObject());
        }

        public void Update(AgreementItem2 agreementItem2) => FolioServiceClient.UpdateAgreementItem(agreementItem2.ToJObject());

        public void UpdateOrInsert(AgreementItem2 agreementItem2)
        {
            if (agreementItem2.Id == null)
                Insert(agreementItem2);
            else
                try
                {
                    Update(agreementItem2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(agreementItem2); else throw;
                }
        }

        public void InsertOrUpdate(AgreementItem2 agreementItem2)
        {
            try
            {
                Insert(agreementItem2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(agreementItem2); else throw;
            }
        }

        public void DeleteAgreementItem2(Guid? id) => FolioServiceClient.DeleteAgreementItem(id?.ToString());

        public bool AnyAlternativeTitleType2s(string where = null) => FolioServiceClient.AnyAlternativeTitleTypes(where);

        public int CountAlternativeTitleType2s(string where = null) => FolioServiceClient.CountAlternativeTitleTypes(where);

        public AlternativeTitleType2[] AlternativeTitleType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.AlternativeTitleTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var att2 = cache ? (AlternativeTitleType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = AlternativeTitleType2.FromJObject(jo)) : AlternativeTitleType2.FromJObject(jo);
                if (load && att2.CreationUserId != null) att2.CreationUser = FindUser2(att2.CreationUserId, cache: cache);
                if (load && att2.LastWriteUserId != null) att2.LastWriteUser = FindUser2(att2.LastWriteUserId, cache: cache);
                return att2;
            }).ToArray();
        }

        public IEnumerable<AlternativeTitleType2> AlternativeTitleType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.AlternativeTitleTypes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var att2 = cache ? (AlternativeTitleType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = AlternativeTitleType2.FromJObject(jo)) : AlternativeTitleType2.FromJObject(jo);
                if (load && att2.CreationUserId != null) att2.CreationUser = FindUser2(att2.CreationUserId, cache: cache);
                if (load && att2.LastWriteUserId != null) att2.LastWriteUser = FindUser2(att2.LastWriteUserId, cache: cache);
                yield return att2;
            }
        }

        public AlternativeTitleType2 FindAlternativeTitleType2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var att2 = cache ? (AlternativeTitleType2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = AlternativeTitleType2.FromJObject(FolioServiceClient.GetAlternativeTitleType(id?.ToString()))) : AlternativeTitleType2.FromJObject(FolioServiceClient.GetAlternativeTitleType(id?.ToString()));
            if (att2 == null) return null;
            if (load && att2.CreationUserId != null) att2.CreationUser = FindUser2(att2.CreationUserId, cache: cache);
            if (load && att2.LastWriteUserId != null) att2.LastWriteUser = FindUser2(att2.LastWriteUserId, cache: cache);
            return att2;
        }

        public void Insert(AlternativeTitleType2 alternativeTitleType2)
        {
            if (alternativeTitleType2.Id == null) alternativeTitleType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertAlternativeTitleType(alternativeTitleType2.ToJObject());
        }

        public void Update(AlternativeTitleType2 alternativeTitleType2) => FolioServiceClient.UpdateAlternativeTitleType(alternativeTitleType2.ToJObject());

        public void UpdateOrInsert(AlternativeTitleType2 alternativeTitleType2)
        {
            if (alternativeTitleType2.Id == null)
                Insert(alternativeTitleType2);
            else
                try
                {
                    Update(alternativeTitleType2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(alternativeTitleType2); else throw;
                }
        }

        public void InsertOrUpdate(AlternativeTitleType2 alternativeTitleType2)
        {
            try
            {
                Insert(alternativeTitleType2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(alternativeTitleType2); else throw;
            }
        }

        public void DeleteAlternativeTitleType2(Guid? id) => FolioServiceClient.DeleteAlternativeTitleType(id?.ToString());

        public bool AnyBatchGroup2s(string where = null) => FolioServiceClient.AnyBatchGroups(where);

        public int CountBatchGroup2s(string where = null) => FolioServiceClient.CountBatchGroups(where);

        public BatchGroup2[] BatchGroup2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.BatchGroups(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var bg2 = cache ? (BatchGroup2)(objects.ContainsKey(id) ? objects[id] : objects[id] = BatchGroup2.FromJObject(jo)) : BatchGroup2.FromJObject(jo);
                if (load && bg2.CreationUserId != null) bg2.CreationUser = FindUser2(bg2.CreationUserId, cache: cache);
                if (load && bg2.LastWriteUserId != null) bg2.LastWriteUser = FindUser2(bg2.LastWriteUserId, cache: cache);
                return bg2;
            }).ToArray();
        }

        public IEnumerable<BatchGroup2> BatchGroup2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.BatchGroups(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var bg2 = cache ? (BatchGroup2)(objects.ContainsKey(id) ? objects[id] : objects[id] = BatchGroup2.FromJObject(jo)) : BatchGroup2.FromJObject(jo);
                if (load && bg2.CreationUserId != null) bg2.CreationUser = FindUser2(bg2.CreationUserId, cache: cache);
                if (load && bg2.LastWriteUserId != null) bg2.LastWriteUser = FindUser2(bg2.LastWriteUserId, cache: cache);
                yield return bg2;
            }
        }

        public BatchGroup2 FindBatchGroup2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var bg2 = cache ? (BatchGroup2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = BatchGroup2.FromJObject(FolioServiceClient.GetBatchGroup(id?.ToString()))) : BatchGroup2.FromJObject(FolioServiceClient.GetBatchGroup(id?.ToString()));
            if (bg2 == null) return null;
            if (load && bg2.CreationUserId != null) bg2.CreationUser = FindUser2(bg2.CreationUserId, cache: cache);
            if (load && bg2.LastWriteUserId != null) bg2.LastWriteUser = FindUser2(bg2.LastWriteUserId, cache: cache);
            return bg2;
        }

        public void Insert(BatchGroup2 batchGroup2)
        {
            if (batchGroup2.Id == null) batchGroup2.Id = Guid.NewGuid();
            FolioServiceClient.InsertBatchGroup(batchGroup2.ToJObject());
        }

        public void Update(BatchGroup2 batchGroup2) => FolioServiceClient.UpdateBatchGroup(batchGroup2.ToJObject());

        public void UpdateOrInsert(BatchGroup2 batchGroup2)
        {
            if (batchGroup2.Id == null)
                Insert(batchGroup2);
            else
                try
                {
                    Update(batchGroup2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(batchGroup2); else throw;
                }
        }

        public void InsertOrUpdate(BatchGroup2 batchGroup2)
        {
            try
            {
                Insert(batchGroup2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(batchGroup2); else throw;
            }
        }

        public void DeleteBatchGroup2(Guid? id) => FolioServiceClient.DeleteBatchGroup(id?.ToString());

        public bool AnyBlock2s(string where = null) => FolioServiceClient.AnyBlocks(where);

        public int CountBlock2s(string where = null) => FolioServiceClient.CountBlocks(where);

        public Block2[] Block2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Blocks(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var b2 = cache ? (Block2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Block2.FromJObject(jo)) : Block2.FromJObject(jo);
                if (load && b2.UserId != null) b2.User = FindUser2(b2.UserId, cache: cache);
                if (load && b2.CreationUserId != null) b2.CreationUser = FindUser2(b2.CreationUserId, cache: cache);
                if (load && b2.LastWriteUserId != null) b2.LastWriteUser = FindUser2(b2.LastWriteUserId, cache: cache);
                return b2;
            }).ToArray();
        }

        public IEnumerable<Block2> Block2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Blocks(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var b2 = cache ? (Block2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Block2.FromJObject(jo)) : Block2.FromJObject(jo);
                if (load && b2.UserId != null) b2.User = FindUser2(b2.UserId, cache: cache);
                if (load && b2.CreationUserId != null) b2.CreationUser = FindUser2(b2.CreationUserId, cache: cache);
                if (load && b2.LastWriteUserId != null) b2.LastWriteUser = FindUser2(b2.LastWriteUserId, cache: cache);
                yield return b2;
            }
        }

        public Block2 FindBlock2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var b2 = cache ? (Block2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Block2.FromJObject(FolioServiceClient.GetBlock(id?.ToString()))) : Block2.FromJObject(FolioServiceClient.GetBlock(id?.ToString()));
            if (b2 == null) return null;
            if (load && b2.UserId != null) b2.User = FindUser2(b2.UserId, cache: cache);
            if (load && b2.CreationUserId != null) b2.CreationUser = FindUser2(b2.CreationUserId, cache: cache);
            if (load && b2.LastWriteUserId != null) b2.LastWriteUser = FindUser2(b2.LastWriteUserId, cache: cache);
            return b2;
        }

        public void Insert(Block2 block2)
        {
            if (block2.Id == null) block2.Id = Guid.NewGuid();
            FolioServiceClient.InsertBlock(block2.ToJObject());
        }

        public void Update(Block2 block2) => FolioServiceClient.UpdateBlock(block2.ToJObject());

        public void UpdateOrInsert(Block2 block2)
        {
            if (block2.Id == null)
                Insert(block2);
            else
                try
                {
                    Update(block2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(block2); else throw;
                }
        }

        public void InsertOrUpdate(Block2 block2)
        {
            try
            {
                Insert(block2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(block2); else throw;
            }
        }

        public void DeleteBlock2(Guid? id) => FolioServiceClient.DeleteBlock(id?.ToString());

        public bool AnyBlockCondition2s(string where = null) => FolioServiceClient.AnyBlockConditions(where);

        public int CountBlockCondition2s(string where = null) => FolioServiceClient.CountBlockConditions(where);

        public BlockCondition2[] BlockCondition2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.BlockConditions(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var bc2 = cache ? (BlockCondition2)(objects.ContainsKey(id) ? objects[id] : objects[id] = BlockCondition2.FromJObject(jo)) : BlockCondition2.FromJObject(jo);
                if (load && bc2.CreationUserId != null) bc2.CreationUser = FindUser2(bc2.CreationUserId, cache: cache);
                if (load && bc2.LastWriteUserId != null) bc2.LastWriteUser = FindUser2(bc2.LastWriteUserId, cache: cache);
                return bc2;
            }).ToArray();
        }

        public IEnumerable<BlockCondition2> BlockCondition2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.BlockConditions(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var bc2 = cache ? (BlockCondition2)(objects.ContainsKey(id) ? objects[id] : objects[id] = BlockCondition2.FromJObject(jo)) : BlockCondition2.FromJObject(jo);
                if (load && bc2.CreationUserId != null) bc2.CreationUser = FindUser2(bc2.CreationUserId, cache: cache);
                if (load && bc2.LastWriteUserId != null) bc2.LastWriteUser = FindUser2(bc2.LastWriteUserId, cache: cache);
                yield return bc2;
            }
        }

        public BlockCondition2 FindBlockCondition2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var bc2 = cache ? (BlockCondition2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = BlockCondition2.FromJObject(FolioServiceClient.GetBlockCondition(id?.ToString()))) : BlockCondition2.FromJObject(FolioServiceClient.GetBlockCondition(id?.ToString()));
            if (bc2 == null) return null;
            if (load && bc2.CreationUserId != null) bc2.CreationUser = FindUser2(bc2.CreationUserId, cache: cache);
            if (load && bc2.LastWriteUserId != null) bc2.LastWriteUser = FindUser2(bc2.LastWriteUserId, cache: cache);
            return bc2;
        }

        public void Insert(BlockCondition2 blockCondition2)
        {
            if (blockCondition2.Id == null) blockCondition2.Id = Guid.NewGuid();
            FolioServiceClient.InsertBlockCondition(blockCondition2.ToJObject());
        }

        public void Update(BlockCondition2 blockCondition2) => FolioServiceClient.UpdateBlockCondition(blockCondition2.ToJObject());

        public void UpdateOrInsert(BlockCondition2 blockCondition2)
        {
            if (blockCondition2.Id == null)
                Insert(blockCondition2);
            else
                try
                {
                    Update(blockCondition2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(blockCondition2); else throw;
                }
        }

        public void InsertOrUpdate(BlockCondition2 blockCondition2)
        {
            try
            {
                Insert(blockCondition2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(blockCondition2); else throw;
            }
        }

        public void DeleteBlockCondition2(Guid? id) => FolioServiceClient.DeleteBlockCondition(id?.ToString());

        public bool AnyBlockLimit2s(string where = null) => FolioServiceClient.AnyBlockLimits(where);

        public int CountBlockLimit2s(string where = null) => FolioServiceClient.CountBlockLimits(where);

        public BlockLimit2[] BlockLimit2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.BlockLimits(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var bl2 = cache ? (BlockLimit2)(objects.ContainsKey(id) ? objects[id] : objects[id] = BlockLimit2.FromJObject(jo)) : BlockLimit2.FromJObject(jo);
                if (load && bl2.GroupId != null) bl2.Group = FindGroup2(bl2.GroupId, cache: cache);
                if (load && bl2.ConditionId != null) bl2.Condition = FindBlockCondition2(bl2.ConditionId, cache: cache);
                if (load && bl2.CreationUserId != null) bl2.CreationUser = FindUser2(bl2.CreationUserId, cache: cache);
                if (load && bl2.LastWriteUserId != null) bl2.LastWriteUser = FindUser2(bl2.LastWriteUserId, cache: cache);
                return bl2;
            }).ToArray();
        }

        public IEnumerable<BlockLimit2> BlockLimit2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.BlockLimits(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var bl2 = cache ? (BlockLimit2)(objects.ContainsKey(id) ? objects[id] : objects[id] = BlockLimit2.FromJObject(jo)) : BlockLimit2.FromJObject(jo);
                if (load && bl2.GroupId != null) bl2.Group = FindGroup2(bl2.GroupId, cache: cache);
                if (load && bl2.ConditionId != null) bl2.Condition = FindBlockCondition2(bl2.ConditionId, cache: cache);
                if (load && bl2.CreationUserId != null) bl2.CreationUser = FindUser2(bl2.CreationUserId, cache: cache);
                if (load && bl2.LastWriteUserId != null) bl2.LastWriteUser = FindUser2(bl2.LastWriteUserId, cache: cache);
                yield return bl2;
            }
        }

        public BlockLimit2 FindBlockLimit2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var bl2 = cache ? (BlockLimit2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = BlockLimit2.FromJObject(FolioServiceClient.GetBlockLimit(id?.ToString()))) : BlockLimit2.FromJObject(FolioServiceClient.GetBlockLimit(id?.ToString()));
            if (bl2 == null) return null;
            if (load && bl2.GroupId != null) bl2.Group = FindGroup2(bl2.GroupId, cache: cache);
            if (load && bl2.ConditionId != null) bl2.Condition = FindBlockCondition2(bl2.ConditionId, cache: cache);
            if (load && bl2.CreationUserId != null) bl2.CreationUser = FindUser2(bl2.CreationUserId, cache: cache);
            if (load && bl2.LastWriteUserId != null) bl2.LastWriteUser = FindUser2(bl2.LastWriteUserId, cache: cache);
            return bl2;
        }

        public void Insert(BlockLimit2 blockLimit2)
        {
            if (blockLimit2.Id == null) blockLimit2.Id = Guid.NewGuid();
            FolioServiceClient.InsertBlockLimit(blockLimit2.ToJObject());
        }

        public void Update(BlockLimit2 blockLimit2) => FolioServiceClient.UpdateBlockLimit(blockLimit2.ToJObject());

        public void UpdateOrInsert(BlockLimit2 blockLimit2)
        {
            if (blockLimit2.Id == null)
                Insert(blockLimit2);
            else
                try
                {
                    Update(blockLimit2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(blockLimit2); else throw;
                }
        }

        public void InsertOrUpdate(BlockLimit2 blockLimit2)
        {
            try
            {
                Insert(blockLimit2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(blockLimit2); else throw;
            }
        }

        public void DeleteBlockLimit2(Guid? id) => FolioServiceClient.DeleteBlockLimit(id?.ToString());

        public bool AnyBoundWithPart2s(string where = null) => FolioServiceClient.AnyBoundWithParts(where);

        public int CountBoundWithPart2s(string where = null) => FolioServiceClient.CountBoundWithParts(where);

        public BoundWithPart2[] BoundWithPart2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.BoundWithParts(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var bwp2 = cache ? (BoundWithPart2)(objects.ContainsKey(id) ? objects[id] : objects[id] = BoundWithPart2.FromJObject(jo)) : BoundWithPart2.FromJObject(jo);
                if (load && bwp2.HoldingId != null) bwp2.Holding = FindHolding2(bwp2.HoldingId, cache: cache);
                if (load && bwp2.ItemId != null) bwp2.Item = FindItem2(bwp2.ItemId, cache: cache);
                if (load && bwp2.CreationUserId != null) bwp2.CreationUser = FindUser2(bwp2.CreationUserId, cache: cache);
                if (load && bwp2.LastWriteUserId != null) bwp2.LastWriteUser = FindUser2(bwp2.LastWriteUserId, cache: cache);
                return bwp2;
            }).ToArray();
        }

        public IEnumerable<BoundWithPart2> BoundWithPart2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.BoundWithParts(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var bwp2 = cache ? (BoundWithPart2)(objects.ContainsKey(id) ? objects[id] : objects[id] = BoundWithPart2.FromJObject(jo)) : BoundWithPart2.FromJObject(jo);
                if (load && bwp2.HoldingId != null) bwp2.Holding = FindHolding2(bwp2.HoldingId, cache: cache);
                if (load && bwp2.ItemId != null) bwp2.Item = FindItem2(bwp2.ItemId, cache: cache);
                if (load && bwp2.CreationUserId != null) bwp2.CreationUser = FindUser2(bwp2.CreationUserId, cache: cache);
                if (load && bwp2.LastWriteUserId != null) bwp2.LastWriteUser = FindUser2(bwp2.LastWriteUserId, cache: cache);
                yield return bwp2;
            }
        }

        public BoundWithPart2 FindBoundWithPart2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var bwp2 = cache ? (BoundWithPart2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = BoundWithPart2.FromJObject(FolioServiceClient.GetBoundWithPart(id?.ToString()))) : BoundWithPart2.FromJObject(FolioServiceClient.GetBoundWithPart(id?.ToString()));
            if (bwp2 == null) return null;
            if (load && bwp2.HoldingId != null) bwp2.Holding = FindHolding2(bwp2.HoldingId, cache: cache);
            if (load && bwp2.ItemId != null) bwp2.Item = FindItem2(bwp2.ItemId, cache: cache);
            if (load && bwp2.CreationUserId != null) bwp2.CreationUser = FindUser2(bwp2.CreationUserId, cache: cache);
            if (load && bwp2.LastWriteUserId != null) bwp2.LastWriteUser = FindUser2(bwp2.LastWriteUserId, cache: cache);
            return bwp2;
        }

        public void Insert(BoundWithPart2 boundWithPart2)
        {
            if (boundWithPart2.Id == null) boundWithPart2.Id = Guid.NewGuid();
            FolioServiceClient.InsertBoundWithPart(boundWithPart2.ToJObject());
        }

        public void Update(BoundWithPart2 boundWithPart2) => FolioServiceClient.UpdateBoundWithPart(boundWithPart2.ToJObject());

        public void UpdateOrInsert(BoundWithPart2 boundWithPart2)
        {
            if (boundWithPart2.Id == null)
                Insert(boundWithPart2);
            else
                try
                {
                    Update(boundWithPart2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(boundWithPart2); else throw;
                }
        }

        public void InsertOrUpdate(BoundWithPart2 boundWithPart2)
        {
            try
            {
                Insert(boundWithPart2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(boundWithPart2); else throw;
            }
        }

        public void DeleteBoundWithPart2(Guid? id) => FolioServiceClient.DeleteBoundWithPart(id?.ToString());

        public bool AnyBudget2s(string where = null) => FolioServiceClient.AnyBudgets(where);

        public int CountBudget2s(string where = null) => FolioServiceClient.CountBudgets(where);

        public Budget2[] Budget2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Budgets(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var b2 = cache ? (Budget2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Budget2.FromJObject(jo)) : Budget2.FromJObject(jo);
                if (load && b2.FundId != null) b2.Fund = FindFund2(b2.FundId, cache: cache);
                if (load && b2.FiscalYearId != null) b2.FiscalYear = FindFiscalYear2(b2.FiscalYearId, cache: cache);
                if (load && b2.CreationUserId != null) b2.CreationUser = FindUser2(b2.CreationUserId, cache: cache);
                if (load && b2.LastWriteUserId != null) b2.LastWriteUser = FindUser2(b2.LastWriteUserId, cache: cache);
                return b2;
            }).ToArray();
        }

        public IEnumerable<Budget2> Budget2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Budgets(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var b2 = cache ? (Budget2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Budget2.FromJObject(jo)) : Budget2.FromJObject(jo);
                if (load && b2.FundId != null) b2.Fund = FindFund2(b2.FundId, cache: cache);
                if (load && b2.FiscalYearId != null) b2.FiscalYear = FindFiscalYear2(b2.FiscalYearId, cache: cache);
                if (load && b2.CreationUserId != null) b2.CreationUser = FindUser2(b2.CreationUserId, cache: cache);
                if (load && b2.LastWriteUserId != null) b2.LastWriteUser = FindUser2(b2.LastWriteUserId, cache: cache);
                yield return b2;
            }
        }

        public Budget2 FindBudget2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var b2 = cache ? (Budget2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Budget2.FromJObject(FolioServiceClient.GetBudget(id?.ToString()))) : Budget2.FromJObject(FolioServiceClient.GetBudget(id?.ToString()));
            if (b2 == null) return null;
            if (load && b2.FundId != null) b2.Fund = FindFund2(b2.FundId, cache: cache);
            if (load && b2.FiscalYearId != null) b2.FiscalYear = FindFiscalYear2(b2.FiscalYearId, cache: cache);
            if (load && b2.CreationUserId != null) b2.CreationUser = FindUser2(b2.CreationUserId, cache: cache);
            if (load && b2.LastWriteUserId != null) b2.LastWriteUser = FindUser2(b2.LastWriteUserId, cache: cache);
            var i = 0;
            if (b2.BudgetAcquisitionsUnits != null) foreach (var bau in b2.BudgetAcquisitionsUnits)
                {
                    bau.Id = (++i).ToString();
                    bau.BudgetId = b2.Id;
                    bau.Budget = b2;
                    if (load && bau.AcquisitionsUnitId != null) bau.AcquisitionsUnit = FindAcquisitionsUnit2(bau.AcquisitionsUnitId, cache: cache);
                }
            i = 0;
            if (b2.BudgetTags != null) foreach (var bt in b2.BudgetTags)
                {
                    bt.Id = (++i).ToString();
                    bt.BudgetId = b2.Id;
                    bt.Budget = b2;
                    if (load && bt.TagId != null) bt.Tag = FindTag2(bt.TagId, cache: cache);
                }
            return b2;
        }

        public void Insert(Budget2 budget2)
        {
            if (budget2.Id == null) budget2.Id = Guid.NewGuid();
            FolioServiceClient.InsertBudget(budget2.ToJObject());
        }

        public void Update(Budget2 budget2) => FolioServiceClient.UpdateBudget(budget2.ToJObject());

        public void UpdateOrInsert(Budget2 budget2)
        {
            if (budget2.Id == null)
                Insert(budget2);
            else
                try
                {
                    Update(budget2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(budget2); else throw;
                }
        }

        public void InsertOrUpdate(Budget2 budget2)
        {
            try
            {
                Insert(budget2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(budget2); else throw;
            }
        }

        public void DeleteBudget2(Guid? id) => FolioServiceClient.DeleteBudget(id?.ToString());

        public bool AnyBudgetExpenseClass2s(string where = null) => FolioServiceClient.AnyBudgetExpenseClasses(where);

        public int CountBudgetExpenseClass2s(string where = null) => FolioServiceClient.CountBudgetExpenseClasses(where);

        public BudgetExpenseClass2[] BudgetExpenseClass2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.BudgetExpenseClasses(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var bec2 = cache ? (BudgetExpenseClass2)(objects.ContainsKey(id) ? objects[id] : objects[id] = BudgetExpenseClass2.FromJObject(jo)) : BudgetExpenseClass2.FromJObject(jo);
                if (load && bec2.BudgetId != null) bec2.Budget = FindBudget2(bec2.BudgetId, cache: cache);
                if (load && bec2.ExpenseClassId != null) bec2.ExpenseClass = FindExpenseClass2(bec2.ExpenseClassId, cache: cache);
                return bec2;
            }).ToArray();
        }

        public IEnumerable<BudgetExpenseClass2> BudgetExpenseClass2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.BudgetExpenseClasses(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var bec2 = cache ? (BudgetExpenseClass2)(objects.ContainsKey(id) ? objects[id] : objects[id] = BudgetExpenseClass2.FromJObject(jo)) : BudgetExpenseClass2.FromJObject(jo);
                if (load && bec2.BudgetId != null) bec2.Budget = FindBudget2(bec2.BudgetId, cache: cache);
                if (load && bec2.ExpenseClassId != null) bec2.ExpenseClass = FindExpenseClass2(bec2.ExpenseClassId, cache: cache);
                yield return bec2;
            }
        }

        public BudgetExpenseClass2 FindBudgetExpenseClass2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var bec2 = cache ? (BudgetExpenseClass2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = BudgetExpenseClass2.FromJObject(FolioServiceClient.GetBudgetExpenseClass(id?.ToString()))) : BudgetExpenseClass2.FromJObject(FolioServiceClient.GetBudgetExpenseClass(id?.ToString()));
            if (bec2 == null) return null;
            if (load && bec2.BudgetId != null) bec2.Budget = FindBudget2(bec2.BudgetId, cache: cache);
            if (load && bec2.ExpenseClassId != null) bec2.ExpenseClass = FindExpenseClass2(bec2.ExpenseClassId, cache: cache);
            return bec2;
        }

        public void Insert(BudgetExpenseClass2 budgetExpenseClass2)
        {
            if (budgetExpenseClass2.Id == null) budgetExpenseClass2.Id = Guid.NewGuid();
            FolioServiceClient.InsertBudgetExpenseClass(budgetExpenseClass2.ToJObject());
        }

        public void Update(BudgetExpenseClass2 budgetExpenseClass2) => FolioServiceClient.UpdateBudgetExpenseClass(budgetExpenseClass2.ToJObject());

        public void UpdateOrInsert(BudgetExpenseClass2 budgetExpenseClass2)
        {
            if (budgetExpenseClass2.Id == null)
                Insert(budgetExpenseClass2);
            else
                try
                {
                    Update(budgetExpenseClass2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(budgetExpenseClass2); else throw;
                }
        }

        public void InsertOrUpdate(BudgetExpenseClass2 budgetExpenseClass2)
        {
            try
            {
                Insert(budgetExpenseClass2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(budgetExpenseClass2); else throw;
            }
        }

        public void DeleteBudgetExpenseClass2(Guid? id) => FolioServiceClient.DeleteBudgetExpenseClass(id?.ToString());

        public bool AnyBudgetGroup2s(string where = null) => FolioServiceClient.AnyBudgetGroups(where);

        public int CountBudgetGroup2s(string where = null) => FolioServiceClient.CountBudgetGroups(where);

        public BudgetGroup2[] BudgetGroup2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.BudgetGroups(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var bg2 = cache ? (BudgetGroup2)(objects.ContainsKey(id) ? objects[id] : objects[id] = BudgetGroup2.FromJObject(jo)) : BudgetGroup2.FromJObject(jo);
                if (load && bg2.BudgetId != null) bg2.Budget = FindBudget2(bg2.BudgetId, cache: cache);
                if (load && bg2.GroupId != null) bg2.Group = FindFinanceGroup2(bg2.GroupId, cache: cache);
                if (load && bg2.FiscalYearId != null) bg2.FiscalYear = FindFiscalYear2(bg2.FiscalYearId, cache: cache);
                if (load && bg2.FundId != null) bg2.Fund = FindFund2(bg2.FundId, cache: cache);
                return bg2;
            }).ToArray();
        }

        public IEnumerable<BudgetGroup2> BudgetGroup2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.BudgetGroups(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var bg2 = cache ? (BudgetGroup2)(objects.ContainsKey(id) ? objects[id] : objects[id] = BudgetGroup2.FromJObject(jo)) : BudgetGroup2.FromJObject(jo);
                if (load && bg2.BudgetId != null) bg2.Budget = FindBudget2(bg2.BudgetId, cache: cache);
                if (load && bg2.GroupId != null) bg2.Group = FindFinanceGroup2(bg2.GroupId, cache: cache);
                if (load && bg2.FiscalYearId != null) bg2.FiscalYear = FindFiscalYear2(bg2.FiscalYearId, cache: cache);
                if (load && bg2.FundId != null) bg2.Fund = FindFund2(bg2.FundId, cache: cache);
                yield return bg2;
            }
        }

        public BudgetGroup2 FindBudgetGroup2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var bg2 = cache ? (BudgetGroup2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = BudgetGroup2.FromJObject(FolioServiceClient.GetBudgetGroup(id?.ToString()))) : BudgetGroup2.FromJObject(FolioServiceClient.GetBudgetGroup(id?.ToString()));
            if (bg2 == null) return null;
            if (load && bg2.BudgetId != null) bg2.Budget = FindBudget2(bg2.BudgetId, cache: cache);
            if (load && bg2.GroupId != null) bg2.Group = FindFinanceGroup2(bg2.GroupId, cache: cache);
            if (load && bg2.FiscalYearId != null) bg2.FiscalYear = FindFiscalYear2(bg2.FiscalYearId, cache: cache);
            if (load && bg2.FundId != null) bg2.Fund = FindFund2(bg2.FundId, cache: cache);
            return bg2;
        }

        public void Insert(BudgetGroup2 budgetGroup2)
        {
            if (budgetGroup2.Id == null) budgetGroup2.Id = Guid.NewGuid();
            FolioServiceClient.InsertBudgetGroup(budgetGroup2.ToJObject());
        }

        public void Update(BudgetGroup2 budgetGroup2) => FolioServiceClient.UpdateBudgetGroup(budgetGroup2.ToJObject());

        public void UpdateOrInsert(BudgetGroup2 budgetGroup2)
        {
            if (budgetGroup2.Id == null)
                Insert(budgetGroup2);
            else
                try
                {
                    Update(budgetGroup2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(budgetGroup2); else throw;
                }
        }

        public void InsertOrUpdate(BudgetGroup2 budgetGroup2)
        {
            try
            {
                Insert(budgetGroup2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(budgetGroup2); else throw;
            }
        }

        public void DeleteBudgetGroup2(Guid? id) => FolioServiceClient.DeleteBudgetGroup(id?.ToString());

        public bool AnyCallNumberType2s(string where = null) => FolioServiceClient.AnyCallNumberTypes(where);

        public int CountCallNumberType2s(string where = null) => FolioServiceClient.CountCallNumberTypes(where);

        public CallNumberType2[] CallNumberType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.CallNumberTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var cnt2 = cache ? (CallNumberType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = CallNumberType2.FromJObject(jo)) : CallNumberType2.FromJObject(jo);
                if (load && cnt2.CreationUserId != null) cnt2.CreationUser = FindUser2(cnt2.CreationUserId, cache: cache);
                if (load && cnt2.LastWriteUserId != null) cnt2.LastWriteUser = FindUser2(cnt2.LastWriteUserId, cache: cache);
                return cnt2;
            }).ToArray();
        }

        public IEnumerable<CallNumberType2> CallNumberType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.CallNumberTypes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var cnt2 = cache ? (CallNumberType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = CallNumberType2.FromJObject(jo)) : CallNumberType2.FromJObject(jo);
                if (load && cnt2.CreationUserId != null) cnt2.CreationUser = FindUser2(cnt2.CreationUserId, cache: cache);
                if (load && cnt2.LastWriteUserId != null) cnt2.LastWriteUser = FindUser2(cnt2.LastWriteUserId, cache: cache);
                yield return cnt2;
            }
        }

        public CallNumberType2 FindCallNumberType2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var cnt2 = cache ? (CallNumberType2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = CallNumberType2.FromJObject(FolioServiceClient.GetCallNumberType(id?.ToString()))) : CallNumberType2.FromJObject(FolioServiceClient.GetCallNumberType(id?.ToString()));
            if (cnt2 == null) return null;
            if (load && cnt2.CreationUserId != null) cnt2.CreationUser = FindUser2(cnt2.CreationUserId, cache: cache);
            if (load && cnt2.LastWriteUserId != null) cnt2.LastWriteUser = FindUser2(cnt2.LastWriteUserId, cache: cache);
            return cnt2;
        }

        public void Insert(CallNumberType2 callNumberType2)
        {
            if (callNumberType2.Id == null) callNumberType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertCallNumberType(callNumberType2.ToJObject());
        }

        public void Update(CallNumberType2 callNumberType2) => FolioServiceClient.UpdateCallNumberType(callNumberType2.ToJObject());

        public void UpdateOrInsert(CallNumberType2 callNumberType2)
        {
            if (callNumberType2.Id == null)
                Insert(callNumberType2);
            else
                try
                {
                    Update(callNumberType2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(callNumberType2); else throw;
                }
        }

        public void InsertOrUpdate(CallNumberType2 callNumberType2)
        {
            try
            {
                Insert(callNumberType2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(callNumberType2); else throw;
            }
        }

        public void DeleteCallNumberType2(Guid? id) => FolioServiceClient.DeleteCallNumberType(id?.ToString());

        public bool AnyCampus2s(string where = null) => FolioServiceClient.AnyCampuses(where);

        public int CountCampus2s(string where = null) => FolioServiceClient.CountCampuses(where);

        public Campus2[] Campus2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Campuses(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var c2 = cache ? (Campus2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Campus2.FromJObject(jo)) : Campus2.FromJObject(jo);
                if (load && c2.InstitutionId != null) c2.Institution = FindInstitution2(c2.InstitutionId, cache: cache);
                if (load && c2.CreationUserId != null) c2.CreationUser = FindUser2(c2.CreationUserId, cache: cache);
                if (load && c2.LastWriteUserId != null) c2.LastWriteUser = FindUser2(c2.LastWriteUserId, cache: cache);
                return c2;
            }).ToArray();
        }

        public IEnumerable<Campus2> Campus2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Campuses(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var c2 = cache ? (Campus2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Campus2.FromJObject(jo)) : Campus2.FromJObject(jo);
                if (load && c2.InstitutionId != null) c2.Institution = FindInstitution2(c2.InstitutionId, cache: cache);
                if (load && c2.CreationUserId != null) c2.CreationUser = FindUser2(c2.CreationUserId, cache: cache);
                if (load && c2.LastWriteUserId != null) c2.LastWriteUser = FindUser2(c2.LastWriteUserId, cache: cache);
                yield return c2;
            }
        }

        public Campus2 FindCampus2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var c2 = cache ? (Campus2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Campus2.FromJObject(FolioServiceClient.GetCampus(id?.ToString()))) : Campus2.FromJObject(FolioServiceClient.GetCampus(id?.ToString()));
            if (c2 == null) return null;
            if (load && c2.InstitutionId != null) c2.Institution = FindInstitution2(c2.InstitutionId, cache: cache);
            if (load && c2.CreationUserId != null) c2.CreationUser = FindUser2(c2.CreationUserId, cache: cache);
            if (load && c2.LastWriteUserId != null) c2.LastWriteUser = FindUser2(c2.LastWriteUserId, cache: cache);
            return c2;
        }

        public void Insert(Campus2 campus2)
        {
            if (campus2.Id == null) campus2.Id = Guid.NewGuid();
            FolioServiceClient.InsertCampus(campus2.ToJObject());
        }

        public void Update(Campus2 campus2) => FolioServiceClient.UpdateCampus(campus2.ToJObject());

        public void UpdateOrInsert(Campus2 campus2)
        {
            if (campus2.Id == null)
                Insert(campus2);
            else
                try
                {
                    Update(campus2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(campus2); else throw;
                }
        }

        public void InsertOrUpdate(Campus2 campus2)
        {
            try
            {
                Insert(campus2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(campus2); else throw;
            }
        }

        public void DeleteCampus2(Guid? id) => FolioServiceClient.DeleteCampus(id?.ToString());

        public bool AnyCancellationReason2s(string where = null) => FolioServiceClient.AnyCancellationReasons(where);

        public int CountCancellationReason2s(string where = null) => FolioServiceClient.CountCancellationReasons(where);

        public CancellationReason2[] CancellationReason2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.CancellationReasons(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var cr2 = cache ? (CancellationReason2)(objects.ContainsKey(id) ? objects[id] : objects[id] = CancellationReason2.FromJObject(jo)) : CancellationReason2.FromJObject(jo);
                if (load && cr2.CreationUserId != null) cr2.CreationUser = FindUser2(cr2.CreationUserId, cache: cache);
                if (load && cr2.LastWriteUserId != null) cr2.LastWriteUser = FindUser2(cr2.LastWriteUserId, cache: cache);
                return cr2;
            }).ToArray();
        }

        public IEnumerable<CancellationReason2> CancellationReason2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.CancellationReasons(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var cr2 = cache ? (CancellationReason2)(objects.ContainsKey(id) ? objects[id] : objects[id] = CancellationReason2.FromJObject(jo)) : CancellationReason2.FromJObject(jo);
                if (load && cr2.CreationUserId != null) cr2.CreationUser = FindUser2(cr2.CreationUserId, cache: cache);
                if (load && cr2.LastWriteUserId != null) cr2.LastWriteUser = FindUser2(cr2.LastWriteUserId, cache: cache);
                yield return cr2;
            }
        }

        public CancellationReason2 FindCancellationReason2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var cr2 = cache ? (CancellationReason2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = CancellationReason2.FromJObject(FolioServiceClient.GetCancellationReason(id?.ToString()))) : CancellationReason2.FromJObject(FolioServiceClient.GetCancellationReason(id?.ToString()));
            if (cr2 == null) return null;
            if (load && cr2.CreationUserId != null) cr2.CreationUser = FindUser2(cr2.CreationUserId, cache: cache);
            if (load && cr2.LastWriteUserId != null) cr2.LastWriteUser = FindUser2(cr2.LastWriteUserId, cache: cache);
            return cr2;
        }

        public void Insert(CancellationReason2 cancellationReason2)
        {
            if (cancellationReason2.Id == null) cancellationReason2.Id = Guid.NewGuid();
            FolioServiceClient.InsertCancellationReason(cancellationReason2.ToJObject());
        }

        public void Update(CancellationReason2 cancellationReason2) => FolioServiceClient.UpdateCancellationReason(cancellationReason2.ToJObject());

        public void UpdateOrInsert(CancellationReason2 cancellationReason2)
        {
            if (cancellationReason2.Id == null)
                Insert(cancellationReason2);
            else
                try
                {
                    Update(cancellationReason2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(cancellationReason2); else throw;
                }
        }

        public void InsertOrUpdate(CancellationReason2 cancellationReason2)
        {
            try
            {
                Insert(cancellationReason2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(cancellationReason2); else throw;
            }
        }

        public void DeleteCancellationReason2(Guid? id) => FolioServiceClient.DeleteCancellationReason(id?.ToString());

        public bool AnyCategory2s(string where = null) => FolioServiceClient.AnyCategories(where);

        public int CountCategory2s(string where = null) => FolioServiceClient.CountCategories(where);

        public Category2[] Category2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Categories(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var c2 = cache ? (Category2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Category2.FromJObject(jo)) : Category2.FromJObject(jo);
                if (load && c2.CreationUserId != null) c2.CreationUser = FindUser2(c2.CreationUserId, cache: cache);
                if (load && c2.LastWriteUserId != null) c2.LastWriteUser = FindUser2(c2.LastWriteUserId, cache: cache);
                return c2;
            }).ToArray();
        }

        public IEnumerable<Category2> Category2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Categories(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var c2 = cache ? (Category2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Category2.FromJObject(jo)) : Category2.FromJObject(jo);
                if (load && c2.CreationUserId != null) c2.CreationUser = FindUser2(c2.CreationUserId, cache: cache);
                if (load && c2.LastWriteUserId != null) c2.LastWriteUser = FindUser2(c2.LastWriteUserId, cache: cache);
                yield return c2;
            }
        }

        public Category2 FindCategory2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var c2 = cache ? (Category2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Category2.FromJObject(FolioServiceClient.GetCategory(id?.ToString()))) : Category2.FromJObject(FolioServiceClient.GetCategory(id?.ToString()));
            if (c2 == null) return null;
            if (load && c2.CreationUserId != null) c2.CreationUser = FindUser2(c2.CreationUserId, cache: cache);
            if (load && c2.LastWriteUserId != null) c2.LastWriteUser = FindUser2(c2.LastWriteUserId, cache: cache);
            return c2;
        }

        public void Insert(Category2 category2)
        {
            if (category2.Id == null) category2.Id = Guid.NewGuid();
            FolioServiceClient.InsertCategory(category2.ToJObject());
        }

        public void Update(Category2 category2) => FolioServiceClient.UpdateCategory(category2.ToJObject());

        public void UpdateOrInsert(Category2 category2)
        {
            if (category2.Id == null)
                Insert(category2);
            else
                try
                {
                    Update(category2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(category2); else throw;
                }
        }

        public void InsertOrUpdate(Category2 category2)
        {
            try
            {
                Insert(category2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(category2); else throw;
            }
        }

        public void DeleteCategory2(Guid? id) => FolioServiceClient.DeleteCategory(id?.ToString());

        public bool AnyCheckIn2s(string where = null) => FolioServiceClient.AnyCheckIns(where);

        public int CountCheckIn2s(string where = null) => FolioServiceClient.CountCheckIns(where);

        public CheckIn2[] CheckIn2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.CheckIns(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var ci2 = cache ? (CheckIn2)(objects.ContainsKey(id) ? objects[id] : objects[id] = CheckIn2.FromJObject(jo)) : CheckIn2.FromJObject(jo);
                if (load && ci2.ItemId != null) ci2.Item = FindItem2(ci2.ItemId, cache: cache);
                if (load && ci2.ItemLocationId != null) ci2.ItemLocation = FindLocation2(ci2.ItemLocationId, cache: cache);
                if (load && ci2.ServicePointId != null) ci2.ServicePoint = FindServicePoint2(ci2.ServicePointId, cache: cache);
                if (load && ci2.PerformedByUserId != null) ci2.PerformedByUser = FindUser2(ci2.PerformedByUserId, cache: cache);
                return ci2;
            }).ToArray();
        }

        public IEnumerable<CheckIn2> CheckIn2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.CheckIns(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var ci2 = cache ? (CheckIn2)(objects.ContainsKey(id) ? objects[id] : objects[id] = CheckIn2.FromJObject(jo)) : CheckIn2.FromJObject(jo);
                if (load && ci2.ItemId != null) ci2.Item = FindItem2(ci2.ItemId, cache: cache);
                if (load && ci2.ItemLocationId != null) ci2.ItemLocation = FindLocation2(ci2.ItemLocationId, cache: cache);
                if (load && ci2.ServicePointId != null) ci2.ServicePoint = FindServicePoint2(ci2.ServicePointId, cache: cache);
                if (load && ci2.PerformedByUserId != null) ci2.PerformedByUser = FindUser2(ci2.PerformedByUserId, cache: cache);
                yield return ci2;
            }
        }

        public CheckIn2 FindCheckIn2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var ci2 = cache ? (CheckIn2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = CheckIn2.FromJObject(FolioServiceClient.GetCheckIn(id?.ToString()))) : CheckIn2.FromJObject(FolioServiceClient.GetCheckIn(id?.ToString()));
            if (ci2 == null) return null;
            if (load && ci2.ItemId != null) ci2.Item = FindItem2(ci2.ItemId, cache: cache);
            if (load && ci2.ItemLocationId != null) ci2.ItemLocation = FindLocation2(ci2.ItemLocationId, cache: cache);
            if (load && ci2.ServicePointId != null) ci2.ServicePoint = FindServicePoint2(ci2.ServicePointId, cache: cache);
            if (load && ci2.PerformedByUserId != null) ci2.PerformedByUser = FindUser2(ci2.PerformedByUserId, cache: cache);
            return ci2;
        }

        public void Insert(CheckIn2 checkIn2)
        {
            if (checkIn2.Id == null) checkIn2.Id = Guid.NewGuid();
            FolioServiceClient.InsertCheckIn(checkIn2.ToJObject());
        }

        public void Update(CheckIn2 checkIn2) => FolioServiceClient.UpdateCheckIn(checkIn2.ToJObject());

        public void UpdateOrInsert(CheckIn2 checkIn2)
        {
            if (checkIn2.Id == null)
                Insert(checkIn2);
            else
                try
                {
                    Update(checkIn2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(checkIn2); else throw;
                }
        }

        public void InsertOrUpdate(CheckIn2 checkIn2)
        {
            try
            {
                Insert(checkIn2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(checkIn2); else throw;
            }
        }

        public void DeleteCheckIn2(Guid? id) => FolioServiceClient.DeleteCheckIn(id?.ToString());

        public CirculationRule2 FindCirculationRule2(bool load = false, bool cache = true)
        {
            var cr2 = CirculationRule2.FromJObject(FolioServiceClient.GetCirculationRule());
            if (cr2 == null) return null;
            if (load && cr2.CreationUserId != null) cr2.CreationUser = FindUser2(cr2.CreationUserId, cache: cache);
            if (load && cr2.LastWriteUserId != null) cr2.LastWriteUser = FindUser2(cr2.LastWriteUserId, cache: cache);
            return cr2;
        }

        public void Update(CirculationRule2 circulationRule2) => FolioServiceClient.UpdateCirculationRule(circulationRule2.ToJObject());

        public bool AnyClassificationType2s(string where = null) => FolioServiceClient.AnyClassificationTypes(where);

        public int CountClassificationType2s(string where = null) => FolioServiceClient.CountClassificationTypes(where);

        public ClassificationType2[] ClassificationType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ClassificationTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var ct2 = cache ? (ClassificationType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ClassificationType2.FromJObject(jo)) : ClassificationType2.FromJObject(jo);
                if (load && ct2.CreationUserId != null) ct2.CreationUser = FindUser2(ct2.CreationUserId, cache: cache);
                if (load && ct2.LastWriteUserId != null) ct2.LastWriteUser = FindUser2(ct2.LastWriteUserId, cache: cache);
                return ct2;
            }).ToArray();
        }

        public IEnumerable<ClassificationType2> ClassificationType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ClassificationTypes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var ct2 = cache ? (ClassificationType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ClassificationType2.FromJObject(jo)) : ClassificationType2.FromJObject(jo);
                if (load && ct2.CreationUserId != null) ct2.CreationUser = FindUser2(ct2.CreationUserId, cache: cache);
                if (load && ct2.LastWriteUserId != null) ct2.LastWriteUser = FindUser2(ct2.LastWriteUserId, cache: cache);
                yield return ct2;
            }
        }

        public ClassificationType2 FindClassificationType2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var ct2 = cache ? (ClassificationType2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = ClassificationType2.FromJObject(FolioServiceClient.GetClassificationType(id?.ToString()))) : ClassificationType2.FromJObject(FolioServiceClient.GetClassificationType(id?.ToString()));
            if (ct2 == null) return null;
            if (load && ct2.CreationUserId != null) ct2.CreationUser = FindUser2(ct2.CreationUserId, cache: cache);
            if (load && ct2.LastWriteUserId != null) ct2.LastWriteUser = FindUser2(ct2.LastWriteUserId, cache: cache);
            return ct2;
        }

        public void Insert(ClassificationType2 classificationType2)
        {
            if (classificationType2.Id == null) classificationType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertClassificationType(classificationType2.ToJObject());
        }

        public void Update(ClassificationType2 classificationType2) => FolioServiceClient.UpdateClassificationType(classificationType2.ToJObject());

        public void UpdateOrInsert(ClassificationType2 classificationType2)
        {
            if (classificationType2.Id == null)
                Insert(classificationType2);
            else
                try
                {
                    Update(classificationType2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(classificationType2); else throw;
                }
        }

        public void InsertOrUpdate(ClassificationType2 classificationType2)
        {
            try
            {
                Insert(classificationType2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(classificationType2); else throw;
            }
        }

        public void DeleteClassificationType2(Guid? id) => FolioServiceClient.DeleteClassificationType(id?.ToString());

        public bool AnyCloseReason2s(string where = null) => FolioServiceClient.AnyCloseReasons(where);

        public int CountCloseReason2s(string where = null) => FolioServiceClient.CountCloseReasons(where);

        public CloseReason2[] CloseReason2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.CloseReasons(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var cr2 = cache ? (CloseReason2)(objects.ContainsKey(id) ? objects[id] : objects[id] = CloseReason2.FromJObject(jo)) : CloseReason2.FromJObject(jo);
                return cr2;
            }).ToArray();
        }

        public IEnumerable<CloseReason2> CloseReason2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.CloseReasons(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var cr2 = cache ? (CloseReason2)(objects.ContainsKey(id) ? objects[id] : objects[id] = CloseReason2.FromJObject(jo)) : CloseReason2.FromJObject(jo);
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

        public void UpdateOrInsert(CloseReason2 closeReason2)
        {
            if (closeReason2.Id == null)
                Insert(closeReason2);
            else
                try
                {
                    Update(closeReason2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(closeReason2); else throw;
                }
        }

        public void InsertOrUpdate(CloseReason2 closeReason2)
        {
            try
            {
                Insert(closeReason2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(closeReason2); else throw;
            }
        }

        public void DeleteCloseReason2(Guid? id) => FolioServiceClient.DeleteCloseReason(id?.ToString());

        public bool AnyComment2s(string where = null) => FolioServiceClient.AnyComments(where);

        public int CountComment2s(string where = null) => FolioServiceClient.CountComments(where);

        public Comment2[] Comment2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Comments(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var c2 = cache ? (Comment2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Comment2.FromJObject(jo)) : Comment2.FromJObject(jo);
                if (load && c2.CreationUserId != null) c2.CreationUser = FindUser2(c2.CreationUserId, cache: cache);
                if (load && c2.LastWriteUserId != null) c2.LastWriteUser = FindUser2(c2.LastWriteUserId, cache: cache);
                return c2;
            }).ToArray();
        }

        public IEnumerable<Comment2> Comment2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Comments(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var c2 = cache ? (Comment2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Comment2.FromJObject(jo)) : Comment2.FromJObject(jo);
                if (load && c2.CreationUserId != null) c2.CreationUser = FindUser2(c2.CreationUserId, cache: cache);
                if (load && c2.LastWriteUserId != null) c2.LastWriteUser = FindUser2(c2.LastWriteUserId, cache: cache);
                yield return c2;
            }
        }

        public Comment2 FindComment2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var c2 = cache ? (Comment2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Comment2.FromJObject(FolioServiceClient.GetComment(id?.ToString()))) : Comment2.FromJObject(FolioServiceClient.GetComment(id?.ToString()));
            if (c2 == null) return null;
            if (load && c2.CreationUserId != null) c2.CreationUser = FindUser2(c2.CreationUserId, cache: cache);
            if (load && c2.LastWriteUserId != null) c2.LastWriteUser = FindUser2(c2.LastWriteUserId, cache: cache);
            return c2;
        }

        public void Insert(Comment2 comment2)
        {
            if (comment2.Id == null) comment2.Id = Guid.NewGuid();
            FolioServiceClient.InsertComment(comment2.ToJObject());
        }

        public void Update(Comment2 comment2) => FolioServiceClient.UpdateComment(comment2.ToJObject());

        public void UpdateOrInsert(Comment2 comment2)
        {
            if (comment2.Id == null)
                Insert(comment2);
            else
                try
                {
                    Update(comment2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(comment2); else throw;
                }
        }

        public void InsertOrUpdate(Comment2 comment2)
        {
            try
            {
                Insert(comment2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(comment2); else throw;
            }
        }

        public void DeleteComment2(Guid? id) => FolioServiceClient.DeleteComment(id?.ToString());

        public bool AnyConfiguration2s(string where = null) => FolioServiceClient.AnyConfigurations(where);

        public int CountConfiguration2s(string where = null) => FolioServiceClient.CountConfigurations(where);

        public Configuration2[] Configuration2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Configurations(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var c2 = cache ? (Configuration2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Configuration2.FromJObject(jo)) : Configuration2.FromJObject(jo);
                if (load && c2.CreationUserId != null) c2.CreationUser = FindUser2(c2.CreationUserId, cache: cache);
                if (load && c2.LastWriteUserId != null) c2.LastWriteUser = FindUser2(c2.LastWriteUserId, cache: cache);
                return c2;
            }).ToArray();
        }

        public IEnumerable<Configuration2> Configuration2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Configurations(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var c2 = cache ? (Configuration2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Configuration2.FromJObject(jo)) : Configuration2.FromJObject(jo);
                if (load && c2.CreationUserId != null) c2.CreationUser = FindUser2(c2.CreationUserId, cache: cache);
                if (load && c2.LastWriteUserId != null) c2.LastWriteUser = FindUser2(c2.LastWriteUserId, cache: cache);
                yield return c2;
            }
        }

        public Configuration2 FindConfiguration2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var c2 = cache ? (Configuration2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Configuration2.FromJObject(FolioServiceClient.GetConfiguration(id?.ToString()))) : Configuration2.FromJObject(FolioServiceClient.GetConfiguration(id?.ToString()));
            if (c2 == null) return null;
            if (load && c2.CreationUserId != null) c2.CreationUser = FindUser2(c2.CreationUserId, cache: cache);
            if (load && c2.LastWriteUserId != null) c2.LastWriteUser = FindUser2(c2.LastWriteUserId, cache: cache);
            return c2;
        }

        public void Insert(Configuration2 configuration2)
        {
            if (configuration2.Id == null) configuration2.Id = Guid.NewGuid();
            FolioServiceClient.InsertConfiguration(configuration2.ToJObject());
        }

        public void Update(Configuration2 configuration2) => FolioServiceClient.UpdateConfiguration(configuration2.ToJObject());

        public void UpdateOrInsert(Configuration2 configuration2)
        {
            if (configuration2.Id == null)
                Insert(configuration2);
            else
                try
                {
                    Update(configuration2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(configuration2); else throw;
                }
        }

        public void InsertOrUpdate(Configuration2 configuration2)
        {
            try
            {
                Insert(configuration2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(configuration2); else throw;
            }
        }

        public void DeleteConfiguration2(Guid? id) => FolioServiceClient.DeleteConfiguration(id?.ToString());

        public bool AnyContact2s(string where = null) => FolioServiceClient.AnyContacts(where);

        public int CountContact2s(string where = null) => FolioServiceClient.CountContacts(where);

        public Contact2[] Contact2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Contacts(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var c2 = cache ? (Contact2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Contact2.FromJObject(jo)) : Contact2.FromJObject(jo);
                if (load && c2.CreationUserId != null) c2.CreationUser = FindUser2(c2.CreationUserId, cache: cache);
                if (load && c2.LastWriteUserId != null) c2.LastWriteUser = FindUser2(c2.LastWriteUserId, cache: cache);
                return c2;
            }).ToArray();
        }

        public IEnumerable<Contact2> Contact2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Contacts(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var c2 = cache ? (Contact2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Contact2.FromJObject(jo)) : Contact2.FromJObject(jo);
                if (load && c2.CreationUserId != null) c2.CreationUser = FindUser2(c2.CreationUserId, cache: cache);
                if (load && c2.LastWriteUserId != null) c2.LastWriteUser = FindUser2(c2.LastWriteUserId, cache: cache);
                yield return c2;
            }
        }

        public Contact2 FindContact2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var c2 = cache ? (Contact2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Contact2.FromJObject(FolioServiceClient.GetContact(id?.ToString()))) : Contact2.FromJObject(FolioServiceClient.GetContact(id?.ToString()));
            if (c2 == null) return null;
            if (load && c2.CreationUserId != null) c2.CreationUser = FindUser2(c2.CreationUserId, cache: cache);
            if (load && c2.LastWriteUserId != null) c2.LastWriteUser = FindUser2(c2.LastWriteUserId, cache: cache);
            var i = 0;
            if (c2.ContactAddresses != null) foreach (var ca in c2.ContactAddresses)
                {
                    ca.Id = (++i).ToString();
                    ca.ContactId = c2.Id;
                    ca.Contact = c2;
                    if (load && ca.CreationUserId != null) ca.CreationUser = FindUser2(ca.CreationUserId, cache: cache);
                    if (load && ca.LastWriteUserId != null) ca.LastWriteUser = FindUser2(ca.LastWriteUserId, cache: cache);
                }
            i = 0;
            if (c2.ContactCategories != null) foreach (var cc in c2.ContactCategories)
                {
                    cc.Id = (++i).ToString();
                    cc.ContactId = c2.Id;
                    cc.Contact = c2;
                    if (load && cc.CategoryId != null) cc.Category = FindCategory2(cc.CategoryId, cache: cache);
                }
            i = 0;
            if (c2.ContactEmails != null) foreach (var ce in c2.ContactEmails)
                {
                    ce.Id = (++i).ToString();
                    ce.ContactId = c2.Id;
                    ce.Contact = c2;
                    if (load && ce.CreationUserId != null) ce.CreationUser = FindUser2(ce.CreationUserId, cache: cache);
                    if (load && ce.LastWriteUserId != null) ce.LastWriteUser = FindUser2(ce.LastWriteUserId, cache: cache);
                }
            i = 0;
            if (c2.ContactPhoneNumbers != null) foreach (var cpn in c2.ContactPhoneNumbers)
                {
                    cpn.Id = (++i).ToString();
                    cpn.ContactId = c2.Id;
                    cpn.Contact = c2;
                    if (load && cpn.CreationUserId != null) cpn.CreationUser = FindUser2(cpn.CreationUserId, cache: cache);
                    if (load && cpn.LastWriteUserId != null) cpn.LastWriteUser = FindUser2(cpn.LastWriteUserId, cache: cache);
                }
            i = 0;
            if (c2.ContactUrls != null) foreach (var cu in c2.ContactUrls)
                {
                    cu.Id = (++i).ToString();
                    cu.ContactId = c2.Id;
                    cu.Contact = c2;
                    if (load && cu.CreationUserId != null) cu.CreationUser = FindUser2(cu.CreationUserId, cache: cache);
                    if (load && cu.LastWriteUserId != null) cu.LastWriteUser = FindUser2(cu.LastWriteUserId, cache: cache);
                }
            return c2;
        }

        public void Insert(Contact2 contact2)
        {
            if (contact2.Id == null) contact2.Id = Guid.NewGuid();
            FolioServiceClient.InsertContact(contact2.ToJObject());
        }

        public void Update(Contact2 contact2) => FolioServiceClient.UpdateContact(contact2.ToJObject());

        public void UpdateOrInsert(Contact2 contact2)
        {
            if (contact2.Id == null)
                Insert(contact2);
            else
                try
                {
                    Update(contact2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(contact2); else throw;
                }
        }

        public void InsertOrUpdate(Contact2 contact2)
        {
            try
            {
                Insert(contact2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(contact2); else throw;
            }
        }

        public void DeleteContact2(Guid? id) => FolioServiceClient.DeleteContact(id?.ToString());

        public bool AnyContributorNameType2s(string where = null) => FolioServiceClient.AnyContributorNameTypes(where);

        public int CountContributorNameType2s(string where = null) => FolioServiceClient.CountContributorNameTypes(where);

        public ContributorNameType2[] ContributorNameType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ContributorNameTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var cnt2 = cache ? (ContributorNameType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ContributorNameType2.FromJObject(jo)) : ContributorNameType2.FromJObject(jo);
                if (load && cnt2.CreationUserId != null) cnt2.CreationUser = FindUser2(cnt2.CreationUserId, cache: cache);
                if (load && cnt2.LastWriteUserId != null) cnt2.LastWriteUser = FindUser2(cnt2.LastWriteUserId, cache: cache);
                return cnt2;
            }).ToArray();
        }

        public IEnumerable<ContributorNameType2> ContributorNameType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ContributorNameTypes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var cnt2 = cache ? (ContributorNameType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ContributorNameType2.FromJObject(jo)) : ContributorNameType2.FromJObject(jo);
                if (load && cnt2.CreationUserId != null) cnt2.CreationUser = FindUser2(cnt2.CreationUserId, cache: cache);
                if (load && cnt2.LastWriteUserId != null) cnt2.LastWriteUser = FindUser2(cnt2.LastWriteUserId, cache: cache);
                yield return cnt2;
            }
        }

        public ContributorNameType2 FindContributorNameType2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var cnt2 = cache ? (ContributorNameType2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = ContributorNameType2.FromJObject(FolioServiceClient.GetContributorNameType(id?.ToString()))) : ContributorNameType2.FromJObject(FolioServiceClient.GetContributorNameType(id?.ToString()));
            if (cnt2 == null) return null;
            if (load && cnt2.CreationUserId != null) cnt2.CreationUser = FindUser2(cnt2.CreationUserId, cache: cache);
            if (load && cnt2.LastWriteUserId != null) cnt2.LastWriteUser = FindUser2(cnt2.LastWriteUserId, cache: cache);
            return cnt2;
        }

        public void Insert(ContributorNameType2 contributorNameType2)
        {
            if (contributorNameType2.Id == null) contributorNameType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertContributorNameType(contributorNameType2.ToJObject());
        }

        public void Update(ContributorNameType2 contributorNameType2) => FolioServiceClient.UpdateContributorNameType(contributorNameType2.ToJObject());

        public void UpdateOrInsert(ContributorNameType2 contributorNameType2)
        {
            if (contributorNameType2.Id == null)
                Insert(contributorNameType2);
            else
                try
                {
                    Update(contributorNameType2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(contributorNameType2); else throw;
                }
        }

        public void InsertOrUpdate(ContributorNameType2 contributorNameType2)
        {
            try
            {
                Insert(contributorNameType2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(contributorNameType2); else throw;
            }
        }

        public void DeleteContributorNameType2(Guid? id) => FolioServiceClient.DeleteContributorNameType(id?.ToString());

        public bool AnyContributorType2s(string where = null) => FolioServiceClient.AnyContributorTypes(where);

        public int CountContributorType2s(string where = null) => FolioServiceClient.CountContributorTypes(where);

        public ContributorType2[] ContributorType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ContributorTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var ct2 = cache ? (ContributorType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ContributorType2.FromJObject(jo)) : ContributorType2.FromJObject(jo);
                if (load && ct2.CreationUserId != null) ct2.CreationUser = FindUser2(ct2.CreationUserId, cache: cache);
                if (load && ct2.LastWriteUserId != null) ct2.LastWriteUser = FindUser2(ct2.LastWriteUserId, cache: cache);
                return ct2;
            }).ToArray();
        }

        public IEnumerable<ContributorType2> ContributorType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ContributorTypes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var ct2 = cache ? (ContributorType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ContributorType2.FromJObject(jo)) : ContributorType2.FromJObject(jo);
                if (load && ct2.CreationUserId != null) ct2.CreationUser = FindUser2(ct2.CreationUserId, cache: cache);
                if (load && ct2.LastWriteUserId != null) ct2.LastWriteUser = FindUser2(ct2.LastWriteUserId, cache: cache);
                yield return ct2;
            }
        }

        public ContributorType2 FindContributorType2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var ct2 = cache ? (ContributorType2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = ContributorType2.FromJObject(FolioServiceClient.GetContributorType(id?.ToString()))) : ContributorType2.FromJObject(FolioServiceClient.GetContributorType(id?.ToString()));
            if (ct2 == null) return null;
            if (load && ct2.CreationUserId != null) ct2.CreationUser = FindUser2(ct2.CreationUserId, cache: cache);
            if (load && ct2.LastWriteUserId != null) ct2.LastWriteUser = FindUser2(ct2.LastWriteUserId, cache: cache);
            return ct2;
        }

        public void Insert(ContributorType2 contributorType2)
        {
            if (contributorType2.Id == null) contributorType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertContributorType(contributorType2.ToJObject());
        }

        public void Update(ContributorType2 contributorType2) => FolioServiceClient.UpdateContributorType(contributorType2.ToJObject());

        public void UpdateOrInsert(ContributorType2 contributorType2)
        {
            if (contributorType2.Id == null)
                Insert(contributorType2);
            else
                try
                {
                    Update(contributorType2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(contributorType2); else throw;
                }
        }

        public void InsertOrUpdate(ContributorType2 contributorType2)
        {
            try
            {
                Insert(contributorType2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(contributorType2); else throw;
            }
        }

        public void DeleteContributorType2(Guid? id) => FolioServiceClient.DeleteContributorType(id?.ToString());

        public bool AnyCustomField2s(string where = null) => FolioServiceClient.AnyCustomFields(where);

        public int CountCustomField2s(string where = null) => FolioServiceClient.CountCustomFields(where);

        public CustomField2[] CustomField2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.CustomFields(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var cf2 = cache ? (CustomField2)(objects.ContainsKey(id) ? objects[id] : objects[id] = CustomField2.FromJObject(jo)) : CustomField2.FromJObject(jo);
                if (load && cf2.CreationUserId != null) cf2.CreationUser = FindUser2(cf2.CreationUserId, cache: cache);
                if (load && cf2.LastWriteUserId != null) cf2.LastWriteUser = FindUser2(cf2.LastWriteUserId, cache: cache);
                return cf2;
            }).ToArray();
        }

        public IEnumerable<CustomField2> CustomField2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.CustomFields(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var cf2 = cache ? (CustomField2)(objects.ContainsKey(id) ? objects[id] : objects[id] = CustomField2.FromJObject(jo)) : CustomField2.FromJObject(jo);
                if (load && cf2.CreationUserId != null) cf2.CreationUser = FindUser2(cf2.CreationUserId, cache: cache);
                if (load && cf2.LastWriteUserId != null) cf2.LastWriteUser = FindUser2(cf2.LastWriteUserId, cache: cache);
                yield return cf2;
            }
        }

        public CustomField2 FindCustomField2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var cf2 = cache ? (CustomField2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = CustomField2.FromJObject(FolioServiceClient.GetCustomField(id?.ToString()))) : CustomField2.FromJObject(FolioServiceClient.GetCustomField(id?.ToString()));
            if (cf2 == null) return null;
            if (load && cf2.CreationUserId != null) cf2.CreationUser = FindUser2(cf2.CreationUserId, cache: cache);
            if (load && cf2.LastWriteUserId != null) cf2.LastWriteUser = FindUser2(cf2.LastWriteUserId, cache: cache);
            var i = 0;
            if (cf2.CustomFieldValues != null) foreach (var cfv in cf2.CustomFieldValues)
                {
                    cfv.Id = (++i).ToString();
                    cfv.CustomFieldId = cf2.Id;
                    cfv.CustomField = cf2;
                }
            return cf2;
        }

        public void Insert(CustomField2 customField2)
        {
            if (customField2.Id == null) customField2.Id = Guid.NewGuid();
            FolioServiceClient.InsertCustomField(customField2.ToJObject());
        }

        public void Update(CustomField2 customField2) => FolioServiceClient.UpdateCustomField(customField2.ToJObject());

        public void UpdateOrInsert(CustomField2 customField2)
        {
            if (customField2.Id == null)
                Insert(customField2);
            else
                try
                {
                    Update(customField2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(customField2); else throw;
                }
        }

        public void InsertOrUpdate(CustomField2 customField2)
        {
            try
            {
                Insert(customField2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(customField2); else throw;
            }
        }

        public void DeleteCustomField2(Guid? id) => FolioServiceClient.DeleteCustomField(id?.ToString());

        public bool AnyDateType2s(string where = null) => FolioServiceClient.AnyDateTypes(where);

        public int CountDateType2s(string where = null) => FolioServiceClient.CountDateTypes(where);

        public DateType2[] DateType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.DateTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var dt2 = cache ? (DateType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = DateType2.FromJObject(jo)) : DateType2.FromJObject(jo);
                if (load && dt2.CreationUserId != null) dt2.CreationUser = FindUser2(dt2.CreationUserId, cache: cache);
                if (load && dt2.LastWriteUserId != null) dt2.LastWriteUser = FindUser2(dt2.LastWriteUserId, cache: cache);
                return dt2;
            }).ToArray();
        }

        public IEnumerable<DateType2> DateType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.DateTypes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var dt2 = cache ? (DateType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = DateType2.FromJObject(jo)) : DateType2.FromJObject(jo);
                if (load && dt2.CreationUserId != null) dt2.CreationUser = FindUser2(dt2.CreationUserId, cache: cache);
                if (load && dt2.LastWriteUserId != null) dt2.LastWriteUser = FindUser2(dt2.LastWriteUserId, cache: cache);
                yield return dt2;
            }
        }

        public DateType2 FindDateType2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var dt2 = cache ? (DateType2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = DateType2.FromJObject(FolioServiceClient.GetDateType(id?.ToString()))) : DateType2.FromJObject(FolioServiceClient.GetDateType(id?.ToString()));
            if (dt2 == null) return null;
            if (load && dt2.CreationUserId != null) dt2.CreationUser = FindUser2(dt2.CreationUserId, cache: cache);
            if (load && dt2.LastWriteUserId != null) dt2.LastWriteUser = FindUser2(dt2.LastWriteUserId, cache: cache);
            return dt2;
        }

        public void Insert(DateType2 dateType2)
        {
            if (dateType2.Id == null) dateType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertDateType(dateType2.ToJObject());
        }

        public void Update(DateType2 dateType2) => FolioServiceClient.UpdateDateType(dateType2.ToJObject());

        public void UpdateOrInsert(DateType2 dateType2)
        {
            if (dateType2.Id == null)
                Insert(dateType2);
            else
                try
                {
                    Update(dateType2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(dateType2); else throw;
                }
        }

        public void InsertOrUpdate(DateType2 dateType2)
        {
            try
            {
                Insert(dateType2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(dateType2); else throw;
            }
        }

        public void DeleteDateType2(Guid? id) => FolioServiceClient.DeleteDateType(id?.ToString());

        public bool AnyDepartment2s(string where = null) => FolioServiceClient.AnyDepartments(where);

        public int CountDepartment2s(string where = null) => FolioServiceClient.CountDepartments(where);

        public Department2[] Department2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Departments(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var d2 = cache ? (Department2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Department2.FromJObject(jo)) : Department2.FromJObject(jo);
                if (load && d2.CreationUserId != null) d2.CreationUser = FindUser2(d2.CreationUserId, cache: cache);
                if (load && d2.LastWriteUserId != null) d2.LastWriteUser = FindUser2(d2.LastWriteUserId, cache: cache);
                return d2;
            }).ToArray();
        }

        public IEnumerable<Department2> Department2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Departments(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var d2 = cache ? (Department2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Department2.FromJObject(jo)) : Department2.FromJObject(jo);
                if (load && d2.CreationUserId != null) d2.CreationUser = FindUser2(d2.CreationUserId, cache: cache);
                if (load && d2.LastWriteUserId != null) d2.LastWriteUser = FindUser2(d2.LastWriteUserId, cache: cache);
                yield return d2;
            }
        }

        public Department2 FindDepartment2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var d2 = cache ? (Department2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Department2.FromJObject(FolioServiceClient.GetDepartment(id?.ToString()))) : Department2.FromJObject(FolioServiceClient.GetDepartment(id?.ToString()));
            if (d2 == null) return null;
            if (load && d2.CreationUserId != null) d2.CreationUser = FindUser2(d2.CreationUserId, cache: cache);
            if (load && d2.LastWriteUserId != null) d2.LastWriteUser = FindUser2(d2.LastWriteUserId, cache: cache);
            return d2;
        }

        public void Insert(Department2 department2)
        {
            if (department2.Id == null) department2.Id = Guid.NewGuid();
            FolioServiceClient.InsertDepartment(department2.ToJObject());
        }

        public void Update(Department2 department2) => FolioServiceClient.UpdateDepartment(department2.ToJObject());

        public void UpdateOrInsert(Department2 department2)
        {
            if (department2.Id == null)
                Insert(department2);
            else
                try
                {
                    Update(department2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(department2); else throw;
                }
        }

        public void InsertOrUpdate(Department2 department2)
        {
            try
            {
                Insert(department2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(department2); else throw;
            }
        }

        public void DeleteDepartment2(Guid? id) => FolioServiceClient.DeleteDepartment(id?.ToString());

        public bool AnyDocument2s(Guid? invoiceId, string where = null) => FolioServiceClient.AnyDocuments(invoiceId?.ToString(), where);

        public int CountDocument2s(Guid? invoiceId, string where = null) => FolioServiceClient.CountDocuments(invoiceId?.ToString(), where);

        public Document2[] Document2s(Guid? invoiceId, out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Documents(invoiceId?.ToString(), out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var d2 = cache ? (Document2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Document2.FromJObject(jo)) : Document2.FromJObject(jo);
                if (load && d2.InvoiceId != null) d2.Invoice = FindInvoice2(d2.InvoiceId, cache: cache);
                if (load && d2.CreationUserId != null) d2.CreationUser = FindUser2(d2.CreationUserId, cache: cache);
                if (load && d2.LastWriteUserId != null) d2.LastWriteUser = FindUser2(d2.LastWriteUserId, cache: cache);
                if (load && d2.CreationUserId2 != null) d2.User2 = FindUser2(d2.CreationUserId2, cache: cache);
                if (load && d2.LastWriteUserId2 != null) d2.User3 = FindUser2(d2.LastWriteUserId2, cache: cache);
                return d2;
            }).ToArray();
        }

        public IEnumerable<Document2> Document2s(Guid? invoiceId, string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Documents(invoiceId?.ToString(), where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var d2 = cache ? (Document2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Document2.FromJObject(jo)) : Document2.FromJObject(jo);
                if (load && d2.InvoiceId != null) d2.Invoice = FindInvoice2(d2.InvoiceId, cache: cache);
                if (load && d2.CreationUserId != null) d2.CreationUser = FindUser2(d2.CreationUserId, cache: cache);
                if (load && d2.LastWriteUserId != null) d2.LastWriteUser = FindUser2(d2.LastWriteUserId, cache: cache);
                if (load && d2.CreationUserId2 != null) d2.User2 = FindUser2(d2.CreationUserId2, cache: cache);
                if (load && d2.LastWriteUserId2 != null) d2.User3 = FindUser2(d2.LastWriteUserId2, cache: cache);
                yield return d2;
            }
        }

        public Document2 FindDocument2(Guid? invoiceId, Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var d2 = cache ? (Document2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Document2.FromJObject(FolioServiceClient.GetDocument(invoiceId?.ToString(), id?.ToString()))) : Document2.FromJObject(FolioServiceClient.GetDocument(invoiceId?.ToString(), id?.ToString()));
            if (d2 == null) return null;
            if (load && d2.InvoiceId != null) d2.Invoice = FindInvoice2(d2.InvoiceId, cache: cache);
            if (load && d2.CreationUserId != null) d2.CreationUser = FindUser2(d2.CreationUserId, cache: cache);
            if (load && d2.LastWriteUserId != null) d2.LastWriteUser = FindUser2(d2.LastWriteUserId, cache: cache);
            if (load && d2.CreationUserId2 != null) d2.User2 = FindUser2(d2.CreationUserId2, cache: cache);
            if (load && d2.LastWriteUserId2 != null) d2.User3 = FindUser2(d2.LastWriteUserId2, cache: cache);
            return d2;
        }

        public void Insert(Guid? invoiceId, Document2 document2)
        {
            if (document2.Id == null) document2.Id = Guid.NewGuid();
            FolioServiceClient.InsertDocument(invoiceId?.ToString(), document2.ToJObject());
        }

        public void DeleteDocument2(Guid? invoiceId, Guid? id) => FolioServiceClient.DeleteDocument(invoiceId?.ToString(), id?.ToString());

        public bool AnyElectronicAccessRelationship2s(string where = null) => FolioServiceClient.AnyElectronicAccessRelationships(where);

        public int CountElectronicAccessRelationship2s(string where = null) => FolioServiceClient.CountElectronicAccessRelationships(where);

        public ElectronicAccessRelationship2[] ElectronicAccessRelationship2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ElectronicAccessRelationships(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var ear2 = cache ? (ElectronicAccessRelationship2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ElectronicAccessRelationship2.FromJObject(jo)) : ElectronicAccessRelationship2.FromJObject(jo);
                if (load && ear2.CreationUserId != null) ear2.CreationUser = FindUser2(ear2.CreationUserId, cache: cache);
                if (load && ear2.LastWriteUserId != null) ear2.LastWriteUser = FindUser2(ear2.LastWriteUserId, cache: cache);
                return ear2;
            }).ToArray();
        }

        public IEnumerable<ElectronicAccessRelationship2> ElectronicAccessRelationship2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ElectronicAccessRelationships(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var ear2 = cache ? (ElectronicAccessRelationship2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ElectronicAccessRelationship2.FromJObject(jo)) : ElectronicAccessRelationship2.FromJObject(jo);
                if (load && ear2.CreationUserId != null) ear2.CreationUser = FindUser2(ear2.CreationUserId, cache: cache);
                if (load && ear2.LastWriteUserId != null) ear2.LastWriteUser = FindUser2(ear2.LastWriteUserId, cache: cache);
                yield return ear2;
            }
        }

        public ElectronicAccessRelationship2 FindElectronicAccessRelationship2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var ear2 = cache ? (ElectronicAccessRelationship2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = ElectronicAccessRelationship2.FromJObject(FolioServiceClient.GetElectronicAccessRelationship(id?.ToString()))) : ElectronicAccessRelationship2.FromJObject(FolioServiceClient.GetElectronicAccessRelationship(id?.ToString()));
            if (ear2 == null) return null;
            if (load && ear2.CreationUserId != null) ear2.CreationUser = FindUser2(ear2.CreationUserId, cache: cache);
            if (load && ear2.LastWriteUserId != null) ear2.LastWriteUser = FindUser2(ear2.LastWriteUserId, cache: cache);
            return ear2;
        }

        public void Insert(ElectronicAccessRelationship2 electronicAccessRelationship2)
        {
            if (electronicAccessRelationship2.Id == null) electronicAccessRelationship2.Id = Guid.NewGuid();
            FolioServiceClient.InsertElectronicAccessRelationship(electronicAccessRelationship2.ToJObject());
        }

        public void Update(ElectronicAccessRelationship2 electronicAccessRelationship2) => FolioServiceClient.UpdateElectronicAccessRelationship(electronicAccessRelationship2.ToJObject());

        public void UpdateOrInsert(ElectronicAccessRelationship2 electronicAccessRelationship2)
        {
            if (electronicAccessRelationship2.Id == null)
                Insert(electronicAccessRelationship2);
            else
                try
                {
                    Update(electronicAccessRelationship2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(electronicAccessRelationship2); else throw;
                }
        }

        public void InsertOrUpdate(ElectronicAccessRelationship2 electronicAccessRelationship2)
        {
            try
            {
                Insert(electronicAccessRelationship2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(electronicAccessRelationship2); else throw;
            }
        }

        public void DeleteElectronicAccessRelationship2(Guid? id) => FolioServiceClient.DeleteElectronicAccessRelationship(id?.ToString());

        public bool AnyExpenseClass2s(string where = null) => FolioServiceClient.AnyExpenseClasses(where);

        public int CountExpenseClass2s(string where = null) => FolioServiceClient.CountExpenseClasses(where);

        public ExpenseClass2[] ExpenseClass2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ExpenseClasses(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var ec2 = cache ? (ExpenseClass2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ExpenseClass2.FromJObject(jo)) : ExpenseClass2.FromJObject(jo);
                if (load && ec2.CreationUserId != null) ec2.CreationUser = FindUser2(ec2.CreationUserId, cache: cache);
                if (load && ec2.LastWriteUserId != null) ec2.LastWriteUser = FindUser2(ec2.LastWriteUserId, cache: cache);
                return ec2;
            }).ToArray();
        }

        public IEnumerable<ExpenseClass2> ExpenseClass2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ExpenseClasses(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var ec2 = cache ? (ExpenseClass2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ExpenseClass2.FromJObject(jo)) : ExpenseClass2.FromJObject(jo);
                if (load && ec2.CreationUserId != null) ec2.CreationUser = FindUser2(ec2.CreationUserId, cache: cache);
                if (load && ec2.LastWriteUserId != null) ec2.LastWriteUser = FindUser2(ec2.LastWriteUserId, cache: cache);
                yield return ec2;
            }
        }

        public ExpenseClass2 FindExpenseClass2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var ec2 = cache ? (ExpenseClass2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = ExpenseClass2.FromJObject(FolioServiceClient.GetExpenseClass(id?.ToString()))) : ExpenseClass2.FromJObject(FolioServiceClient.GetExpenseClass(id?.ToString()));
            if (ec2 == null) return null;
            if (load && ec2.CreationUserId != null) ec2.CreationUser = FindUser2(ec2.CreationUserId, cache: cache);
            if (load && ec2.LastWriteUserId != null) ec2.LastWriteUser = FindUser2(ec2.LastWriteUserId, cache: cache);
            return ec2;
        }

        public void Insert(ExpenseClass2 expenseClass2)
        {
            if (expenseClass2.Id == null) expenseClass2.Id = Guid.NewGuid();
            FolioServiceClient.InsertExpenseClass(expenseClass2.ToJObject());
        }

        public void Update(ExpenseClass2 expenseClass2) => FolioServiceClient.UpdateExpenseClass(expenseClass2.ToJObject());

        public void UpdateOrInsert(ExpenseClass2 expenseClass2)
        {
            if (expenseClass2.Id == null)
                Insert(expenseClass2);
            else
                try
                {
                    Update(expenseClass2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(expenseClass2); else throw;
                }
        }

        public void InsertOrUpdate(ExpenseClass2 expenseClass2)
        {
            try
            {
                Insert(expenseClass2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(expenseClass2); else throw;
            }
        }

        public void DeleteExpenseClass2(Guid? id) => FolioServiceClient.DeleteExpenseClass(id?.ToString());

        public bool AnyFee2s(string where = null) => FolioServiceClient.AnyFees(where);

        public int CountFee2s(string where = null) => FolioServiceClient.CountFees(where);

        public Fee2[] Fee2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Fees(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var f2 = cache ? (Fee2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Fee2.FromJObject(jo)) : Fee2.FromJObject(jo);
                if (load && f2.CreationUserId != null) f2.CreationUser = FindUser2(f2.CreationUserId, cache: cache);
                if (load && f2.LastWriteUserId != null) f2.LastWriteUser = FindUser2(f2.LastWriteUserId, cache: cache);
                if (load && f2.LoanId != null) f2.Loan = FindLoan2(f2.LoanId, cache: cache);
                if (load && f2.UserId != null) f2.User = FindUser2(f2.UserId, cache: cache);
                if (load && f2.ItemId != null) f2.Item = FindItem2(f2.ItemId, cache: cache);
                if (load && f2.MaterialTypeId != null) f2.MaterialType1 = FindMaterialType2(f2.MaterialTypeId, cache: cache);
                if (load && f2.FeeTypeId != null) f2.FeeType = FindFeeType2(f2.FeeTypeId, cache: cache);
                if (load && f2.OwnerId != null) f2.Owner = FindOwner2(f2.OwnerId, cache: cache);
                if (load && f2.HoldingId != null) f2.Holding = FindHolding2(f2.HoldingId, cache: cache);
                if (load && f2.InstanceId != null) f2.Instance = FindInstance2(f2.InstanceId, cache: cache);
                return f2;
            }).ToArray();
        }

        public IEnumerable<Fee2> Fee2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Fees(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var f2 = cache ? (Fee2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Fee2.FromJObject(jo)) : Fee2.FromJObject(jo);
                if (load && f2.CreationUserId != null) f2.CreationUser = FindUser2(f2.CreationUserId, cache: cache);
                if (load && f2.LastWriteUserId != null) f2.LastWriteUser = FindUser2(f2.LastWriteUserId, cache: cache);
                if (load && f2.LoanId != null) f2.Loan = FindLoan2(f2.LoanId, cache: cache);
                if (load && f2.UserId != null) f2.User = FindUser2(f2.UserId, cache: cache);
                if (load && f2.ItemId != null) f2.Item = FindItem2(f2.ItemId, cache: cache);
                if (load && f2.MaterialTypeId != null) f2.MaterialType1 = FindMaterialType2(f2.MaterialTypeId, cache: cache);
                if (load && f2.FeeTypeId != null) f2.FeeType = FindFeeType2(f2.FeeTypeId, cache: cache);
                if (load && f2.OwnerId != null) f2.Owner = FindOwner2(f2.OwnerId, cache: cache);
                if (load && f2.HoldingId != null) f2.Holding = FindHolding2(f2.HoldingId, cache: cache);
                if (load && f2.InstanceId != null) f2.Instance = FindInstance2(f2.InstanceId, cache: cache);
                yield return f2;
            }
        }

        public Fee2 FindFee2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var f2 = cache ? (Fee2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Fee2.FromJObject(FolioServiceClient.GetFee(id?.ToString()))) : Fee2.FromJObject(FolioServiceClient.GetFee(id?.ToString()));
            if (f2 == null) return null;
            if (load && f2.CreationUserId != null) f2.CreationUser = FindUser2(f2.CreationUserId, cache: cache);
            if (load && f2.LastWriteUserId != null) f2.LastWriteUser = FindUser2(f2.LastWriteUserId, cache: cache);
            if (load && f2.LoanId != null) f2.Loan = FindLoan2(f2.LoanId, cache: cache);
            if (load && f2.UserId != null) f2.User = FindUser2(f2.UserId, cache: cache);
            if (load && f2.ItemId != null) f2.Item = FindItem2(f2.ItemId, cache: cache);
            if (load && f2.MaterialTypeId != null) f2.MaterialType1 = FindMaterialType2(f2.MaterialTypeId, cache: cache);
            if (load && f2.FeeTypeId != null) f2.FeeType = FindFeeType2(f2.FeeTypeId, cache: cache);
            if (load && f2.OwnerId != null) f2.Owner = FindOwner2(f2.OwnerId, cache: cache);
            if (load && f2.HoldingId != null) f2.Holding = FindHolding2(f2.HoldingId, cache: cache);
            if (load && f2.InstanceId != null) f2.Instance = FindInstance2(f2.InstanceId, cache: cache);
            return f2;
        }

        public void Insert(Fee2 fee2)
        {
            if (fee2.Id == null) fee2.Id = Guid.NewGuid();
            FolioServiceClient.InsertFee(fee2.ToJObject());
        }

        public void Update(Fee2 fee2) => FolioServiceClient.UpdateFee(fee2.ToJObject());

        public void UpdateOrInsert(Fee2 fee2)
        {
            if (fee2.Id == null)
                Insert(fee2);
            else
                try
                {
                    Update(fee2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(fee2); else throw;
                }
        }

        public void InsertOrUpdate(Fee2 fee2)
        {
            try
            {
                Insert(fee2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(fee2); else throw;
            }
        }

        public void DeleteFee2(Guid? id) => FolioServiceClient.DeleteFee(id?.ToString());

        public bool AnyFeeType2s(string where = null) => FolioServiceClient.AnyFeeTypes(where);

        public int CountFeeType2s(string where = null) => FolioServiceClient.CountFeeTypes(where);

        public FeeType2[] FeeType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.FeeTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var ft2 = cache ? (FeeType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = FeeType2.FromJObject(jo)) : FeeType2.FromJObject(jo);
                if (load && ft2.ChargeNoticeId != null) ft2.ChargeNotice = FindTemplate2(ft2.ChargeNoticeId, cache: cache);
                if (load && ft2.ActionNoticeId != null) ft2.ActionNotice = FindTemplate2(ft2.ActionNoticeId, cache: cache);
                if (load && ft2.OwnerId != null) ft2.Owner = FindOwner2(ft2.OwnerId, cache: cache);
                if (load && ft2.CreationUserId != null) ft2.CreationUser = FindUser2(ft2.CreationUserId, cache: cache);
                if (load && ft2.LastWriteUserId != null) ft2.LastWriteUser = FindUser2(ft2.LastWriteUserId, cache: cache);
                return ft2;
            }).ToArray();
        }

        public IEnumerable<FeeType2> FeeType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.FeeTypes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var ft2 = cache ? (FeeType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = FeeType2.FromJObject(jo)) : FeeType2.FromJObject(jo);
                if (load && ft2.ChargeNoticeId != null) ft2.ChargeNotice = FindTemplate2(ft2.ChargeNoticeId, cache: cache);
                if (load && ft2.ActionNoticeId != null) ft2.ActionNotice = FindTemplate2(ft2.ActionNoticeId, cache: cache);
                if (load && ft2.OwnerId != null) ft2.Owner = FindOwner2(ft2.OwnerId, cache: cache);
                if (load && ft2.CreationUserId != null) ft2.CreationUser = FindUser2(ft2.CreationUserId, cache: cache);
                if (load && ft2.LastWriteUserId != null) ft2.LastWriteUser = FindUser2(ft2.LastWriteUserId, cache: cache);
                yield return ft2;
            }
        }

        public FeeType2 FindFeeType2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var ft2 = cache ? (FeeType2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = FeeType2.FromJObject(FolioServiceClient.GetFeeType(id?.ToString()))) : FeeType2.FromJObject(FolioServiceClient.GetFeeType(id?.ToString()));
            if (ft2 == null) return null;
            if (load && ft2.ChargeNoticeId != null) ft2.ChargeNotice = FindTemplate2(ft2.ChargeNoticeId, cache: cache);
            if (load && ft2.ActionNoticeId != null) ft2.ActionNotice = FindTemplate2(ft2.ActionNoticeId, cache: cache);
            if (load && ft2.OwnerId != null) ft2.Owner = FindOwner2(ft2.OwnerId, cache: cache);
            if (load && ft2.CreationUserId != null) ft2.CreationUser = FindUser2(ft2.CreationUserId, cache: cache);
            if (load && ft2.LastWriteUserId != null) ft2.LastWriteUser = FindUser2(ft2.LastWriteUserId, cache: cache);
            return ft2;
        }

        public void Insert(FeeType2 feeType2)
        {
            if (feeType2.Id == null) feeType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertFeeType(feeType2.ToJObject());
        }

        public void Update(FeeType2 feeType2) => FolioServiceClient.UpdateFeeType(feeType2.ToJObject());

        public void UpdateOrInsert(FeeType2 feeType2)
        {
            if (feeType2.Id == null)
                Insert(feeType2);
            else
                try
                {
                    Update(feeType2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(feeType2); else throw;
                }
        }

        public void InsertOrUpdate(FeeType2 feeType2)
        {
            try
            {
                Insert(feeType2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(feeType2); else throw;
            }
        }

        public void DeleteFeeType2(Guid? id) => FolioServiceClient.DeleteFeeType(id?.ToString());

        public bool AnyFinanceGroup2s(string where = null) => FolioServiceClient.AnyFinanceGroups(where);

        public int CountFinanceGroup2s(string where = null) => FolioServiceClient.CountFinanceGroups(where);

        public FinanceGroup2[] FinanceGroup2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.FinanceGroups(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var fg2 = cache ? (FinanceGroup2)(objects.ContainsKey(id) ? objects[id] : objects[id] = FinanceGroup2.FromJObject(jo)) : FinanceGroup2.FromJObject(jo);
                if (load && fg2.CreationUserId != null) fg2.CreationUser = FindUser2(fg2.CreationUserId, cache: cache);
                if (load && fg2.LastWriteUserId != null) fg2.LastWriteUser = FindUser2(fg2.LastWriteUserId, cache: cache);
                return fg2;
            }).ToArray();
        }

        public IEnumerable<FinanceGroup2> FinanceGroup2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.FinanceGroups(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var fg2 = cache ? (FinanceGroup2)(objects.ContainsKey(id) ? objects[id] : objects[id] = FinanceGroup2.FromJObject(jo)) : FinanceGroup2.FromJObject(jo);
                if (load && fg2.CreationUserId != null) fg2.CreationUser = FindUser2(fg2.CreationUserId, cache: cache);
                if (load && fg2.LastWriteUserId != null) fg2.LastWriteUser = FindUser2(fg2.LastWriteUserId, cache: cache);
                yield return fg2;
            }
        }

        public FinanceGroup2 FindFinanceGroup2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var fg2 = cache ? (FinanceGroup2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = FinanceGroup2.FromJObject(FolioServiceClient.GetFinanceGroup(id?.ToString()))) : FinanceGroup2.FromJObject(FolioServiceClient.GetFinanceGroup(id?.ToString()));
            if (fg2 == null) return null;
            if (load && fg2.CreationUserId != null) fg2.CreationUser = FindUser2(fg2.CreationUserId, cache: cache);
            if (load && fg2.LastWriteUserId != null) fg2.LastWriteUser = FindUser2(fg2.LastWriteUserId, cache: cache);
            var i = 0;
            if (fg2.FinanceGroupAcquisitionsUnits != null) foreach (var fgau in fg2.FinanceGroupAcquisitionsUnits)
                {
                    fgau.Id = (++i).ToString();
                    fgau.FinanceGroupId = fg2.Id;
                    fgau.FinanceGroup = fg2;
                    if (load && fgau.AcquisitionsUnitId != null) fgau.AcquisitionsUnit = FindAcquisitionsUnit2(fgau.AcquisitionsUnitId, cache: cache);
                }
            return fg2;
        }

        public void Insert(FinanceGroup2 financeGroup2)
        {
            if (financeGroup2.Id == null) financeGroup2.Id = Guid.NewGuid();
            FolioServiceClient.InsertFinanceGroup(financeGroup2.ToJObject());
        }

        public void Update(FinanceGroup2 financeGroup2) => FolioServiceClient.UpdateFinanceGroup(financeGroup2.ToJObject());

        public void UpdateOrInsert(FinanceGroup2 financeGroup2)
        {
            if (financeGroup2.Id == null)
                Insert(financeGroup2);
            else
                try
                {
                    Update(financeGroup2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(financeGroup2); else throw;
                }
        }

        public void InsertOrUpdate(FinanceGroup2 financeGroup2)
        {
            try
            {
                Insert(financeGroup2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(financeGroup2); else throw;
            }
        }

        public void DeleteFinanceGroup2(Guid? id) => FolioServiceClient.DeleteFinanceGroup(id?.ToString());

        public bool AnyFiscalYear2s(string where = null) => FolioServiceClient.AnyFiscalYears(where);

        public int CountFiscalYear2s(string where = null) => FolioServiceClient.CountFiscalYears(where);

        public FiscalYear2[] FiscalYear2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.FiscalYears(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var fy2 = cache ? (FiscalYear2)(objects.ContainsKey(id) ? objects[id] : objects[id] = FiscalYear2.FromJObject(jo)) : FiscalYear2.FromJObject(jo);
                if (load && fy2.CreationUserId != null) fy2.CreationUser = FindUser2(fy2.CreationUserId, cache: cache);
                if (load && fy2.LastWriteUserId != null) fy2.LastWriteUser = FindUser2(fy2.LastWriteUserId, cache: cache);
                return fy2;
            }).ToArray();
        }

        public IEnumerable<FiscalYear2> FiscalYear2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.FiscalYears(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var fy2 = cache ? (FiscalYear2)(objects.ContainsKey(id) ? objects[id] : objects[id] = FiscalYear2.FromJObject(jo)) : FiscalYear2.FromJObject(jo);
                if (load && fy2.CreationUserId != null) fy2.CreationUser = FindUser2(fy2.CreationUserId, cache: cache);
                if (load && fy2.LastWriteUserId != null) fy2.LastWriteUser = FindUser2(fy2.LastWriteUserId, cache: cache);
                yield return fy2;
            }
        }

        public FiscalYear2 FindFiscalYear2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var fy2 = cache ? (FiscalYear2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = FiscalYear2.FromJObject(FolioServiceClient.GetFiscalYear(id?.ToString()))) : FiscalYear2.FromJObject(FolioServiceClient.GetFiscalYear(id?.ToString()));
            if (fy2 == null) return null;
            if (load && fy2.CreationUserId != null) fy2.CreationUser = FindUser2(fy2.CreationUserId, cache: cache);
            if (load && fy2.LastWriteUserId != null) fy2.LastWriteUser = FindUser2(fy2.LastWriteUserId, cache: cache);
            var i = 0;
            if (fy2.FiscalYearAcquisitionsUnits != null) foreach (var fyau in fy2.FiscalYearAcquisitionsUnits)
                {
                    fyau.Id = (++i).ToString();
                    fyau.FiscalYearId = fy2.Id;
                    fyau.FiscalYear = fy2;
                    if (load && fyau.AcquisitionsUnitId != null) fyau.AcquisitionsUnit = FindAcquisitionsUnit2(fyau.AcquisitionsUnitId, cache: cache);
                }
            return fy2;
        }

        public void Insert(FiscalYear2 fiscalYear2)
        {
            if (fiscalYear2.Id == null) fiscalYear2.Id = Guid.NewGuid();
            FolioServiceClient.InsertFiscalYear(fiscalYear2.ToJObject());
        }

        public void Update(FiscalYear2 fiscalYear2) => FolioServiceClient.UpdateFiscalYear(fiscalYear2.ToJObject());

        public void UpdateOrInsert(FiscalYear2 fiscalYear2)
        {
            if (fiscalYear2.Id == null)
                Insert(fiscalYear2);
            else
                try
                {
                    Update(fiscalYear2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(fiscalYear2); else throw;
                }
        }

        public void InsertOrUpdate(FiscalYear2 fiscalYear2)
        {
            try
            {
                Insert(fiscalYear2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(fiscalYear2); else throw;
            }
        }

        public void DeleteFiscalYear2(Guid? id) => FolioServiceClient.DeleteFiscalYear(id?.ToString());

        public bool AnyFixedDueDateSchedule2s(string where = null) => FolioServiceClient.AnyFixedDueDateSchedules(where);

        public int CountFixedDueDateSchedule2s(string where = null) => FolioServiceClient.CountFixedDueDateSchedules(where);

        public FixedDueDateSchedule2[] FixedDueDateSchedule2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.FixedDueDateSchedules(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var fdds2 = cache ? (FixedDueDateSchedule2)(objects.ContainsKey(id) ? objects[id] : objects[id] = FixedDueDateSchedule2.FromJObject(jo)) : FixedDueDateSchedule2.FromJObject(jo);
                if (load && fdds2.CreationUserId != null) fdds2.CreationUser = FindUser2(fdds2.CreationUserId, cache: cache);
                if (load && fdds2.LastWriteUserId != null) fdds2.LastWriteUser = FindUser2(fdds2.LastWriteUserId, cache: cache);
                return fdds2;
            }).ToArray();
        }

        public IEnumerable<FixedDueDateSchedule2> FixedDueDateSchedule2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.FixedDueDateSchedules(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var fdds2 = cache ? (FixedDueDateSchedule2)(objects.ContainsKey(id) ? objects[id] : objects[id] = FixedDueDateSchedule2.FromJObject(jo)) : FixedDueDateSchedule2.FromJObject(jo);
                if (load && fdds2.CreationUserId != null) fdds2.CreationUser = FindUser2(fdds2.CreationUserId, cache: cache);
                if (load && fdds2.LastWriteUserId != null) fdds2.LastWriteUser = FindUser2(fdds2.LastWriteUserId, cache: cache);
                yield return fdds2;
            }
        }

        public FixedDueDateSchedule2 FindFixedDueDateSchedule2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var fdds2 = cache ? (FixedDueDateSchedule2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = FixedDueDateSchedule2.FromJObject(FolioServiceClient.GetFixedDueDateSchedule(id?.ToString()))) : FixedDueDateSchedule2.FromJObject(FolioServiceClient.GetFixedDueDateSchedule(id?.ToString()));
            if (fdds2 == null) return null;
            if (load && fdds2.CreationUserId != null) fdds2.CreationUser = FindUser2(fdds2.CreationUserId, cache: cache);
            if (load && fdds2.LastWriteUserId != null) fdds2.LastWriteUser = FindUser2(fdds2.LastWriteUserId, cache: cache);
            var i = 0;
            if (fdds2.FixedDueDateScheduleSchedules != null) foreach (var fddss in fdds2.FixedDueDateScheduleSchedules)
                {
                    fddss.Id = (++i).ToString();
                    fddss.FixedDueDateScheduleId = fdds2.Id;
                    fddss.FixedDueDateSchedule = fdds2;
                }
            return fdds2;
        }

        public void Insert(FixedDueDateSchedule2 fixedDueDateSchedule2)
        {
            if (fixedDueDateSchedule2.Id == null) fixedDueDateSchedule2.Id = Guid.NewGuid();
            FolioServiceClient.InsertFixedDueDateSchedule(fixedDueDateSchedule2.ToJObject());
        }

        public void Update(FixedDueDateSchedule2 fixedDueDateSchedule2) => FolioServiceClient.UpdateFixedDueDateSchedule(fixedDueDateSchedule2.ToJObject());

        public void UpdateOrInsert(FixedDueDateSchedule2 fixedDueDateSchedule2)
        {
            if (fixedDueDateSchedule2.Id == null)
                Insert(fixedDueDateSchedule2);
            else
                try
                {
                    Update(fixedDueDateSchedule2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(fixedDueDateSchedule2); else throw;
                }
        }

        public void InsertOrUpdate(FixedDueDateSchedule2 fixedDueDateSchedule2)
        {
            try
            {
                Insert(fixedDueDateSchedule2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(fixedDueDateSchedule2); else throw;
            }
        }

        public void DeleteFixedDueDateSchedule2(Guid? id) => FolioServiceClient.DeleteFixedDueDateSchedule(id?.ToString());

        public bool AnyFormats(string where = null) => FolioServiceClient.AnyInstanceFormats(where);

        public int CountFormats(string where = null) => FolioServiceClient.CountInstanceFormats(where);

        public Format[] Formats(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.InstanceFormats(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var f = cache ? (Format)(objects.ContainsKey(id) ? objects[id] : objects[id] = Format.FromJObject(jo)) : Format.FromJObject(jo);
                if (load && f.CreationUserId != null) f.CreationUser = FindUser2(f.CreationUserId, cache: cache);
                if (load && f.LastWriteUserId != null) f.LastWriteUser = FindUser2(f.LastWriteUserId, cache: cache);
                return f;
            }).ToArray();
        }

        public IEnumerable<Format> Formats(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.InstanceFormats(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var f = cache ? (Format)(objects.ContainsKey(id) ? objects[id] : objects[id] = Format.FromJObject(jo)) : Format.FromJObject(jo);
                if (load && f.CreationUserId != null) f.CreationUser = FindUser2(f.CreationUserId, cache: cache);
                if (load && f.LastWriteUserId != null) f.LastWriteUser = FindUser2(f.LastWriteUserId, cache: cache);
                yield return f;
            }
        }

        public Format FindFormat(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var f = cache ? (Format)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Format.FromJObject(FolioServiceClient.GetInstanceFormat(id?.ToString()))) : Format.FromJObject(FolioServiceClient.GetInstanceFormat(id?.ToString()));
            if (f == null) return null;
            if (load && f.CreationUserId != null) f.CreationUser = FindUser2(f.CreationUserId, cache: cache);
            if (load && f.LastWriteUserId != null) f.LastWriteUser = FindUser2(f.LastWriteUserId, cache: cache);
            return f;
        }

        public void Insert(Format format)
        {
            if (format.Id == null) format.Id = Guid.NewGuid();
            FolioServiceClient.InsertInstanceFormat(format.ToJObject());
        }

        public void Update(Format format) => FolioServiceClient.UpdateInstanceFormat(format.ToJObject());

        public void UpdateOrInsert(Format format)
        {
            if (format.Id == null)
                Insert(format);
            else
                try
                {
                    Update(format);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(format); else throw;
                }
        }

        public void InsertOrUpdate(Format format)
        {
            try
            {
                Insert(format);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(format); else throw;
            }
        }

        public void DeleteFormat(Guid? id) => FolioServiceClient.DeleteInstanceFormat(id?.ToString());

        public bool AnyFund2s(string where = null) => FolioServiceClient.AnyFunds(where);

        public int CountFund2s(string where = null) => FolioServiceClient.CountFunds(where);

        public Fund2[] Fund2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Funds(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var f2 = cache ? (Fund2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Fund2.FromJObject(jo)) : Fund2.FromJObject(jo);
                if (load && f2.FundTypeId != null) f2.FundType = FindFundType2(f2.FundTypeId, cache: cache);
                if (load && f2.LedgerId != null) f2.Ledger = FindLedger2(f2.LedgerId, cache: cache);
                if (load && f2.CreationUserId != null) f2.CreationUser = FindUser2(f2.CreationUserId, cache: cache);
                if (load && f2.LastWriteUserId != null) f2.LastWriteUser = FindUser2(f2.LastWriteUserId, cache: cache);
                return f2;
            }).ToArray();
        }

        public IEnumerable<Fund2> Fund2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Funds(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var f2 = cache ? (Fund2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Fund2.FromJObject(jo)) : Fund2.FromJObject(jo);
                if (load && f2.FundTypeId != null) f2.FundType = FindFundType2(f2.FundTypeId, cache: cache);
                if (load && f2.LedgerId != null) f2.Ledger = FindLedger2(f2.LedgerId, cache: cache);
                if (load && f2.CreationUserId != null) f2.CreationUser = FindUser2(f2.CreationUserId, cache: cache);
                if (load && f2.LastWriteUserId != null) f2.LastWriteUser = FindUser2(f2.LastWriteUserId, cache: cache);
                yield return f2;
            }
        }

        public Fund2 FindFund2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var f2 = cache ? (Fund2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Fund2.FromJObject(FolioServiceClient.GetFund(id?.ToString()))) : Fund2.FromJObject(FolioServiceClient.GetFund(id?.ToString()));
            if (f2 == null) return null;
            if (load && f2.FundTypeId != null) f2.FundType = FindFundType2(f2.FundTypeId, cache: cache);
            if (load && f2.LedgerId != null) f2.Ledger = FindLedger2(f2.LedgerId, cache: cache);
            if (load && f2.CreationUserId != null) f2.CreationUser = FindUser2(f2.CreationUserId, cache: cache);
            if (load && f2.LastWriteUserId != null) f2.LastWriteUser = FindUser2(f2.LastWriteUserId, cache: cache);
            var i = 0;
            if (f2.AllocatedFromFunds != null) foreach (var aff in f2.AllocatedFromFunds)
                {
                    aff.Id = (++i).ToString();
                    aff.FromFundId = f2.Id;
                    aff.FromFund = f2;
                    if (load && aff.FundId != null) aff.Fund = FindFund2(aff.FundId, cache: cache);
                }
            i = 0;
            if (f2.AllocatedToFunds != null) foreach (var atf in f2.AllocatedToFunds)
                {
                    atf.Id = (++i).ToString();
                    atf.ToFundId = f2.Id;
                    atf.ToFund = f2;
                    if (load && atf.FundId != null) atf.Fund = FindFund2(atf.FundId, cache: cache);
                }
            i = 0;
            if (f2.FundAcquisitionsUnits != null) foreach (var fau in f2.FundAcquisitionsUnits)
                {
                    fau.Id = (++i).ToString();
                    fau.FundId = f2.Id;
                    fau.Fund = f2;
                    if (load && fau.AcquisitionsUnitId != null) fau.AcquisitionsUnit = FindAcquisitionsUnit2(fau.AcquisitionsUnitId, cache: cache);
                }
            i = 0;
            if (f2.FundLocation2s != null) foreach (var fl2 in f2.FundLocation2s)
                {
                    fl2.Id = (++i).ToString();
                    fl2.FundId = f2.Id;
                    fl2.Fund = f2;
                    if (load && fl2.LocationId != null) fl2.Location = FindLocation2(fl2.LocationId, cache: cache);
                }
            i = 0;
            if (f2.FundOrganization2s != null) foreach (var fo2 in f2.FundOrganization2s)
                {
                    fo2.Id = (++i).ToString();
                    fo2.FundId = f2.Id;
                    fo2.Fund = f2;
                    if (load && fo2.OrganizationId != null) fo2.Organization = FindOrganization2(fo2.OrganizationId, cache: cache);
                }
            i = 0;
            if (f2.FundTags != null) foreach (var ft in f2.FundTags)
                {
                    ft.Id = (++i).ToString();
                    ft.FundId = f2.Id;
                    ft.Fund = f2;
                    if (load && ft.TagId != null) ft.Tag = FindTag2(ft.TagId, cache: cache);
                }
            return f2;
        }

        public void Insert(Fund2 fund2)
        {
            if (fund2.Id == null) fund2.Id = Guid.NewGuid();
            FolioServiceClient.InsertFund(fund2.ToJObject());
        }

        public void Update(Fund2 fund2) => FolioServiceClient.UpdateFund(fund2.ToJObject());

        public void UpdateOrInsert(Fund2 fund2)
        {
            if (fund2.Id == null)
                Insert(fund2);
            else
                try
                {
                    Update(fund2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(fund2); else throw;
                }
        }

        public void InsertOrUpdate(Fund2 fund2)
        {
            try
            {
                Insert(fund2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(fund2); else throw;
            }
        }

        public void DeleteFund2(Guid? id) => FolioServiceClient.DeleteFund(id?.ToString());

        public bool AnyFundType2s(string where = null) => FolioServiceClient.AnyFundTypes(where);

        public int CountFundType2s(string where = null) => FolioServiceClient.CountFundTypes(where);

        public FundType2[] FundType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.FundTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var ft2 = cache ? (FundType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = FundType2.FromJObject(jo)) : FundType2.FromJObject(jo);
                return ft2;
            }).ToArray();
        }

        public IEnumerable<FundType2> FundType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.FundTypes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var ft2 = cache ? (FundType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = FundType2.FromJObject(jo)) : FundType2.FromJObject(jo);
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

        public void UpdateOrInsert(FundType2 fundType2)
        {
            if (fundType2.Id == null)
                Insert(fundType2);
            else
                try
                {
                    Update(fundType2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(fundType2); else throw;
                }
        }

        public void InsertOrUpdate(FundType2 fundType2)
        {
            try
            {
                Insert(fundType2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(fundType2); else throw;
            }
        }

        public void DeleteFundType2(Guid? id) => FolioServiceClient.DeleteFundType(id?.ToString());

        public bool AnyGroup2s(string where = null) => FolioServiceClient.AnyGroups(where);

        public int CountGroup2s(string where = null) => FolioServiceClient.CountGroups(where);

        public Group2[] Group2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Groups(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var g2 = cache ? (Group2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Group2.FromJObject(jo)) : Group2.FromJObject(jo);
                if (load && g2.CreationUserId != null) g2.CreationUser = FindUser2(g2.CreationUserId, cache: cache);
                if (load && g2.LastWriteUserId != null) g2.LastWriteUser = FindUser2(g2.LastWriteUserId, cache: cache);
                return g2;
            }).ToArray();
        }

        public IEnumerable<Group2> Group2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Groups(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var g2 = cache ? (Group2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Group2.FromJObject(jo)) : Group2.FromJObject(jo);
                if (load && g2.CreationUserId != null) g2.CreationUser = FindUser2(g2.CreationUserId, cache: cache);
                if (load && g2.LastWriteUserId != null) g2.LastWriteUser = FindUser2(g2.LastWriteUserId, cache: cache);
                yield return g2;
            }
        }

        public Group2 FindGroup2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var g2 = cache ? (Group2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Group2.FromJObject(FolioServiceClient.GetGroup(id?.ToString()))) : Group2.FromJObject(FolioServiceClient.GetGroup(id?.ToString()));
            if (g2 == null) return null;
            if (load && g2.CreationUserId != null) g2.CreationUser = FindUser2(g2.CreationUserId, cache: cache);
            if (load && g2.LastWriteUserId != null) g2.LastWriteUser = FindUser2(g2.LastWriteUserId, cache: cache);
            return g2;
        }

        public void Insert(Group2 group2)
        {
            if (group2.Id == null) group2.Id = Guid.NewGuid();
            FolioServiceClient.InsertGroup(group2.ToJObject());
        }

        public void Update(Group2 group2) => FolioServiceClient.UpdateGroup(group2.ToJObject());

        public void UpdateOrInsert(Group2 group2)
        {
            if (group2.Id == null)
                Insert(group2);
            else
                try
                {
                    Update(group2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(group2); else throw;
                }
        }

        public void InsertOrUpdate(Group2 group2)
        {
            try
            {
                Insert(group2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(group2); else throw;
            }
        }

        public void DeleteGroup2(Guid? id) => FolioServiceClient.DeleteGroup(id?.ToString());

        public bool AnyHolding2s(string where = null) => FolioServiceClient.AnyHoldings(where);

        public int CountHolding2s(string where = null) => FolioServiceClient.CountHoldings(where);

        public Holding2[] Holding2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Holdings(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var h2 = cache ? (Holding2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Holding2.FromJObject(jo)) : Holding2.FromJObject(jo);
                if (load && h2.SourceId != null) h2.Source = FindSource2(h2.SourceId, cache: cache);
                if (load && h2.HoldingTypeId != null) h2.HoldingType = FindHoldingType2(h2.HoldingTypeId, cache: cache);
                if (load && h2.InstanceId != null) h2.Instance = FindInstance2(h2.InstanceId, cache: cache);
                if (load && h2.LocationId != null) h2.Location = FindLocation2(h2.LocationId, cache: cache);
                if (load && h2.TemporaryLocationId != null) h2.TemporaryLocation = FindLocation2(h2.TemporaryLocationId, cache: cache);
                if (load && h2.EffectiveLocationId != null) h2.EffectiveLocation = FindLocation2(h2.EffectiveLocationId, cache: cache);
                if (load && h2.CallNumberTypeId != null) h2.CallNumberType = FindCallNumberType2(h2.CallNumberTypeId, cache: cache);
                if (load && h2.IllPolicyId != null) h2.IllPolicy = FindIllPolicy2(h2.IllPolicyId, cache: cache);
                if (load && h2.CreationUserId != null) h2.CreationUser = FindUser2(h2.CreationUserId, cache: cache);
                if (load && h2.LastWriteUserId != null) h2.LastWriteUser = FindUser2(h2.LastWriteUserId, cache: cache);
                return h2;
            }).ToArray();
        }

        public IEnumerable<Holding2> Holding2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Holdings(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var h2 = cache ? (Holding2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Holding2.FromJObject(jo)) : Holding2.FromJObject(jo);
                if (load && h2.SourceId != null) h2.Source = FindSource2(h2.SourceId, cache: cache);
                if (load && h2.HoldingTypeId != null) h2.HoldingType = FindHoldingType2(h2.HoldingTypeId, cache: cache);
                if (load && h2.InstanceId != null) h2.Instance = FindInstance2(h2.InstanceId, cache: cache);
                if (load && h2.LocationId != null) h2.Location = FindLocation2(h2.LocationId, cache: cache);
                if (load && h2.TemporaryLocationId != null) h2.TemporaryLocation = FindLocation2(h2.TemporaryLocationId, cache: cache);
                if (load && h2.EffectiveLocationId != null) h2.EffectiveLocation = FindLocation2(h2.EffectiveLocationId, cache: cache);
                if (load && h2.CallNumberTypeId != null) h2.CallNumberType = FindCallNumberType2(h2.CallNumberTypeId, cache: cache);
                if (load && h2.IllPolicyId != null) h2.IllPolicy = FindIllPolicy2(h2.IllPolicyId, cache: cache);
                if (load && h2.CreationUserId != null) h2.CreationUser = FindUser2(h2.CreationUserId, cache: cache);
                if (load && h2.LastWriteUserId != null) h2.LastWriteUser = FindUser2(h2.LastWriteUserId, cache: cache);
                yield return h2;
            }
        }

        public Holding2 FindHolding2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var h2 = cache ? (Holding2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Holding2.FromJObject(FolioServiceClient.GetHolding(id?.ToString()))) : Holding2.FromJObject(FolioServiceClient.GetHolding(id?.ToString()));
            if (h2 == null) return null;
            if (load && h2.SourceId != null) h2.Source = FindSource2(h2.SourceId, cache: cache);
            if (load && h2.HoldingTypeId != null) h2.HoldingType = FindHoldingType2(h2.HoldingTypeId, cache: cache);
            if (load && h2.InstanceId != null) h2.Instance = FindInstance2(h2.InstanceId, cache: cache);
            if (load && h2.LocationId != null) h2.Location = FindLocation2(h2.LocationId, cache: cache);
            if (load && h2.TemporaryLocationId != null) h2.TemporaryLocation = FindLocation2(h2.TemporaryLocationId, cache: cache);
            if (load && h2.EffectiveLocationId != null) h2.EffectiveLocation = FindLocation2(h2.EffectiveLocationId, cache: cache);
            if (load && h2.CallNumberTypeId != null) h2.CallNumberType = FindCallNumberType2(h2.CallNumberTypeId, cache: cache);
            if (load && h2.IllPolicyId != null) h2.IllPolicy = FindIllPolicy2(h2.IllPolicyId, cache: cache);
            if (load && h2.CreationUserId != null) h2.CreationUser = FindUser2(h2.CreationUserId, cache: cache);
            if (load && h2.LastWriteUserId != null) h2.LastWriteUser = FindUser2(h2.LastWriteUserId, cache: cache);
            var i = 0;
            if (h2.Extents != null) foreach (var e2 in h2.Extents)
                {
                    e2.Id = (++i).ToString();
                    e2.HoldingId = h2.Id;
                    e2.Holding = h2;
                }
            i = 0;
            if (h2.HoldingAdditionalCallNumbers != null) foreach (var hacn in h2.HoldingAdditionalCallNumbers)
                {
                    hacn.Id = (++i).ToString();
                    hacn.HoldingId = h2.Id;
                    hacn.Holding = h2;
                    if (load && hacn.TypeId != null) hacn.Type = FindCallNumberType2(hacn.TypeId, cache: cache);
                }
            i = 0;
            if (h2.HoldingAdministrativeNotes != null) foreach (var han in h2.HoldingAdministrativeNotes)
                {
                    han.Id = (++i).ToString();
                    han.HoldingId = h2.Id;
                    han.Holding = h2;
                }
            i = 0;
            if (h2.HoldingElectronicAccesses != null) foreach (var hea in h2.HoldingElectronicAccesses)
                {
                    hea.Id = (++i).ToString();
                    hea.HoldingId = h2.Id;
                    hea.Holding = h2;
                    if (load && hea.RelationshipId != null) hea.Relationship = FindElectronicAccessRelationship2(hea.RelationshipId, cache: cache);
                }
            i = 0;
            if (h2.HoldingEntries != null) foreach (var he in h2.HoldingEntries)
                {
                    he.Id = (++i).ToString();
                    he.HoldingId = h2.Id;
                    he.Holding = h2;
                }
            i = 0;
            if (h2.HoldingFormerIds != null) foreach (var hfi in h2.HoldingFormerIds)
                {
                    hfi.Id = (++i).ToString();
                    hfi.HoldingId = h2.Id;
                    hfi.Holding = h2;
                }
            i = 0;
            if (h2.HoldingNotes != null) foreach (var hn in h2.HoldingNotes)
                {
                    hn.Id = (++i).ToString();
                    hn.HoldingId = h2.Id;
                    hn.Holding = h2;
                    if (load && hn.HoldingNoteTypeId != null) hn.HoldingNoteType = FindHoldingNoteType2(hn.HoldingNoteTypeId, cache: cache);
                    if (load && hn.CreationUserId != null) hn.CreationUser = FindUser2(hn.CreationUserId, cache: cache);
                    if (load && hn.LastWriteUserId != null) hn.LastWriteUser = FindUser2(hn.LastWriteUserId, cache: cache);
                }
            i = 0;
            if (h2.HoldingStatisticalCodes != null) foreach (var hsc in h2.HoldingStatisticalCodes)
                {
                    hsc.Id = (++i).ToString();
                    hsc.HoldingId = h2.Id;
                    hsc.Holding = h2;
                    if (load && hsc.StatisticalCodeId != null) hsc.StatisticalCode = FindStatisticalCode2(hsc.StatisticalCodeId, cache: cache);
                }
            i = 0;
            if (h2.HoldingTags != null) foreach (var ht in h2.HoldingTags)
                {
                    ht.Id = (++i).ToString();
                    ht.HoldingId = h2.Id;
                    ht.Holding = h2;
                }
            i = 0;
            if (h2.IndexStatements != null) foreach (var @is in h2.IndexStatements)
                {
                    @is.Id = (++i).ToString();
                    @is.HoldingId = h2.Id;
                    @is.Holding = h2;
                }
            i = 0;
            if (h2.SupplementStatements != null) foreach (var ss in h2.SupplementStatements)
                {
                    ss.Id = (++i).ToString();
                    ss.HoldingId = h2.Id;
                    ss.Holding = h2;
                }
            return h2;
        }

        public void Insert(Holding2 holding2)
        {
            if (holding2.Id == null) holding2.Id = Guid.NewGuid();
            FolioServiceClient.InsertHolding(holding2.ToJObject());
        }

        public void Update(Holding2 holding2) => FolioServiceClient.UpdateHolding(holding2.ToJObject());

        public void UpdateOrInsert(Holding2 holding2)
        {
            if (holding2.Id == null)
                Insert(holding2);
            else
                try
                {
                    Update(holding2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(holding2); else throw;
                }
        }

        public void InsertOrUpdate(Holding2 holding2)
        {
            try
            {
                Insert(holding2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(holding2); else throw;
            }
        }

        public void DeleteHolding2(Guid? id) => FolioServiceClient.DeleteHolding(id?.ToString());

        public bool AnyHoldingNoteType2s(string where = null) => FolioServiceClient.AnyHoldingNoteTypes(where);

        public int CountHoldingNoteType2s(string where = null) => FolioServiceClient.CountHoldingNoteTypes(where);

        public HoldingNoteType2[] HoldingNoteType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.HoldingNoteTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var hnt2 = cache ? (HoldingNoteType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = HoldingNoteType2.FromJObject(jo)) : HoldingNoteType2.FromJObject(jo);
                if (load && hnt2.CreationUserId != null) hnt2.CreationUser = FindUser2(hnt2.CreationUserId, cache: cache);
                if (load && hnt2.LastWriteUserId != null) hnt2.LastWriteUser = FindUser2(hnt2.LastWriteUserId, cache: cache);
                return hnt2;
            }).ToArray();
        }

        public IEnumerable<HoldingNoteType2> HoldingNoteType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.HoldingNoteTypes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var hnt2 = cache ? (HoldingNoteType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = HoldingNoteType2.FromJObject(jo)) : HoldingNoteType2.FromJObject(jo);
                if (load && hnt2.CreationUserId != null) hnt2.CreationUser = FindUser2(hnt2.CreationUserId, cache: cache);
                if (load && hnt2.LastWriteUserId != null) hnt2.LastWriteUser = FindUser2(hnt2.LastWriteUserId, cache: cache);
                yield return hnt2;
            }
        }

        public HoldingNoteType2 FindHoldingNoteType2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var hnt2 = cache ? (HoldingNoteType2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = HoldingNoteType2.FromJObject(FolioServiceClient.GetHoldingNoteType(id?.ToString()))) : HoldingNoteType2.FromJObject(FolioServiceClient.GetHoldingNoteType(id?.ToString()));
            if (hnt2 == null) return null;
            if (load && hnt2.CreationUserId != null) hnt2.CreationUser = FindUser2(hnt2.CreationUserId, cache: cache);
            if (load && hnt2.LastWriteUserId != null) hnt2.LastWriteUser = FindUser2(hnt2.LastWriteUserId, cache: cache);
            return hnt2;
        }

        public void Insert(HoldingNoteType2 holdingNoteType2)
        {
            if (holdingNoteType2.Id == null) holdingNoteType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertHoldingNoteType(holdingNoteType2.ToJObject());
        }

        public void Update(HoldingNoteType2 holdingNoteType2) => FolioServiceClient.UpdateHoldingNoteType(holdingNoteType2.ToJObject());

        public void UpdateOrInsert(HoldingNoteType2 holdingNoteType2)
        {
            if (holdingNoteType2.Id == null)
                Insert(holdingNoteType2);
            else
                try
                {
                    Update(holdingNoteType2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(holdingNoteType2); else throw;
                }
        }

        public void InsertOrUpdate(HoldingNoteType2 holdingNoteType2)
        {
            try
            {
                Insert(holdingNoteType2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(holdingNoteType2); else throw;
            }
        }

        public void DeleteHoldingNoteType2(Guid? id) => FolioServiceClient.DeleteHoldingNoteType(id?.ToString());

        public bool AnyHoldingType2s(string where = null) => FolioServiceClient.AnyHoldingTypes(where);

        public int CountHoldingType2s(string where = null) => FolioServiceClient.CountHoldingTypes(where);

        public HoldingType2[] HoldingType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.HoldingTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var ht2 = cache ? (HoldingType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = HoldingType2.FromJObject(jo)) : HoldingType2.FromJObject(jo);
                if (load && ht2.CreationUserId != null) ht2.CreationUser = FindUser2(ht2.CreationUserId, cache: cache);
                if (load && ht2.LastWriteUserId != null) ht2.LastWriteUser = FindUser2(ht2.LastWriteUserId, cache: cache);
                return ht2;
            }).ToArray();
        }

        public IEnumerable<HoldingType2> HoldingType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.HoldingTypes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var ht2 = cache ? (HoldingType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = HoldingType2.FromJObject(jo)) : HoldingType2.FromJObject(jo);
                if (load && ht2.CreationUserId != null) ht2.CreationUser = FindUser2(ht2.CreationUserId, cache: cache);
                if (load && ht2.LastWriteUserId != null) ht2.LastWriteUser = FindUser2(ht2.LastWriteUserId, cache: cache);
                yield return ht2;
            }
        }

        public HoldingType2 FindHoldingType2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var ht2 = cache ? (HoldingType2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = HoldingType2.FromJObject(FolioServiceClient.GetHoldingType(id?.ToString()))) : HoldingType2.FromJObject(FolioServiceClient.GetHoldingType(id?.ToString()));
            if (ht2 == null) return null;
            if (load && ht2.CreationUserId != null) ht2.CreationUser = FindUser2(ht2.CreationUserId, cache: cache);
            if (load && ht2.LastWriteUserId != null) ht2.LastWriteUser = FindUser2(ht2.LastWriteUserId, cache: cache);
            return ht2;
        }

        public void Insert(HoldingType2 holdingType2)
        {
            if (holdingType2.Id == null) holdingType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertHoldingType(holdingType2.ToJObject());
        }

        public void Update(HoldingType2 holdingType2) => FolioServiceClient.UpdateHoldingType(holdingType2.ToJObject());

        public void UpdateOrInsert(HoldingType2 holdingType2)
        {
            if (holdingType2.Id == null)
                Insert(holdingType2);
            else
                try
                {
                    Update(holdingType2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(holdingType2); else throw;
                }
        }

        public void InsertOrUpdate(HoldingType2 holdingType2)
        {
            try
            {
                Insert(holdingType2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(holdingType2); else throw;
            }
        }

        public void DeleteHoldingType2(Guid? id) => FolioServiceClient.DeleteHoldingType(id?.ToString());

        public HridSetting2 FindHridSetting2(bool load = false, bool cache = true)
        {
            var hs2 = HridSetting2.FromJObject(FolioServiceClient.GetHridSetting());
            if (hs2 == null) return null;
            if (load && hs2.CreationUserId != null) hs2.CreationUser = FindUser2(hs2.CreationUserId, cache: cache);
            if (load && hs2.LastWriteUserId != null) hs2.LastWriteUser = FindUser2(hs2.LastWriteUserId, cache: cache);
            return hs2;
        }

        public void Update(HridSetting2 hridSetting2) => FolioServiceClient.UpdateHridSetting(hridSetting2.ToJObject());

        public bool AnyIdType2s(string where = null) => FolioServiceClient.AnyIdTypes(where);

        public int CountIdType2s(string where = null) => FolioServiceClient.CountIdTypes(where);

        public IdType2[] IdType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.IdTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var it2 = cache ? (IdType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = IdType2.FromJObject(jo)) : IdType2.FromJObject(jo);
                if (load && it2.CreationUserId != null) it2.CreationUser = FindUser2(it2.CreationUserId, cache: cache);
                if (load && it2.LastWriteUserId != null) it2.LastWriteUser = FindUser2(it2.LastWriteUserId, cache: cache);
                return it2;
            }).ToArray();
        }

        public IEnumerable<IdType2> IdType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.IdTypes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var it2 = cache ? (IdType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = IdType2.FromJObject(jo)) : IdType2.FromJObject(jo);
                if (load && it2.CreationUserId != null) it2.CreationUser = FindUser2(it2.CreationUserId, cache: cache);
                if (load && it2.LastWriteUserId != null) it2.LastWriteUser = FindUser2(it2.LastWriteUserId, cache: cache);
                yield return it2;
            }
        }

        public IdType2 FindIdType2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var it2 = cache ? (IdType2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = IdType2.FromJObject(FolioServiceClient.GetIdType(id?.ToString()))) : IdType2.FromJObject(FolioServiceClient.GetIdType(id?.ToString()));
            if (it2 == null) return null;
            if (load && it2.CreationUserId != null) it2.CreationUser = FindUser2(it2.CreationUserId, cache: cache);
            if (load && it2.LastWriteUserId != null) it2.LastWriteUser = FindUser2(it2.LastWriteUserId, cache: cache);
            return it2;
        }

        public void Insert(IdType2 idType2)
        {
            if (idType2.Id == null) idType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertIdType(idType2.ToJObject());
        }

        public void Update(IdType2 idType2) => FolioServiceClient.UpdateIdType(idType2.ToJObject());

        public void UpdateOrInsert(IdType2 idType2)
        {
            if (idType2.Id == null)
                Insert(idType2);
            else
                try
                {
                    Update(idType2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(idType2); else throw;
                }
        }

        public void InsertOrUpdate(IdType2 idType2)
        {
            try
            {
                Insert(idType2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(idType2); else throw;
            }
        }

        public void DeleteIdType2(Guid? id) => FolioServiceClient.DeleteIdType(id?.ToString());

        public bool AnyIllPolicy2s(string where = null) => FolioServiceClient.AnyIllPolicies(where);

        public int CountIllPolicy2s(string where = null) => FolioServiceClient.CountIllPolicies(where);

        public IllPolicy2[] IllPolicy2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.IllPolicies(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var ip2 = cache ? (IllPolicy2)(objects.ContainsKey(id) ? objects[id] : objects[id] = IllPolicy2.FromJObject(jo)) : IllPolicy2.FromJObject(jo);
                if (load && ip2.CreationUserId != null) ip2.CreationUser = FindUser2(ip2.CreationUserId, cache: cache);
                if (load && ip2.LastWriteUserId != null) ip2.LastWriteUser = FindUser2(ip2.LastWriteUserId, cache: cache);
                return ip2;
            }).ToArray();
        }

        public IEnumerable<IllPolicy2> IllPolicy2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.IllPolicies(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var ip2 = cache ? (IllPolicy2)(objects.ContainsKey(id) ? objects[id] : objects[id] = IllPolicy2.FromJObject(jo)) : IllPolicy2.FromJObject(jo);
                if (load && ip2.CreationUserId != null) ip2.CreationUser = FindUser2(ip2.CreationUserId, cache: cache);
                if (load && ip2.LastWriteUserId != null) ip2.LastWriteUser = FindUser2(ip2.LastWriteUserId, cache: cache);
                yield return ip2;
            }
        }

        public IllPolicy2 FindIllPolicy2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var ip2 = cache ? (IllPolicy2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = IllPolicy2.FromJObject(FolioServiceClient.GetIllPolicy(id?.ToString()))) : IllPolicy2.FromJObject(FolioServiceClient.GetIllPolicy(id?.ToString()));
            if (ip2 == null) return null;
            if (load && ip2.CreationUserId != null) ip2.CreationUser = FindUser2(ip2.CreationUserId, cache: cache);
            if (load && ip2.LastWriteUserId != null) ip2.LastWriteUser = FindUser2(ip2.LastWriteUserId, cache: cache);
            return ip2;
        }

        public void Insert(IllPolicy2 illPolicy2)
        {
            if (illPolicy2.Id == null) illPolicy2.Id = Guid.NewGuid();
            FolioServiceClient.InsertIllPolicy(illPolicy2.ToJObject());
        }

        public void Update(IllPolicy2 illPolicy2) => FolioServiceClient.UpdateIllPolicy(illPolicy2.ToJObject());

        public void UpdateOrInsert(IllPolicy2 illPolicy2)
        {
            if (illPolicy2.Id == null)
                Insert(illPolicy2);
            else
                try
                {
                    Update(illPolicy2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(illPolicy2); else throw;
                }
        }

        public void InsertOrUpdate(IllPolicy2 illPolicy2)
        {
            try
            {
                Insert(illPolicy2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(illPolicy2); else throw;
            }
        }

        public void DeleteIllPolicy2(Guid? id) => FolioServiceClient.DeleteIllPolicy(id?.ToString());

        public bool AnyInstance2s(string where = null) => FolioServiceClient.AnyInstances(where);

        public int CountInstance2s(string where = null) => FolioServiceClient.CountInstances(where);

        public Instance2[] Instance2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Instances(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var i2 = cache ? (Instance2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Instance2.FromJObject(jo)) : Instance2.FromJObject(jo);
                if (load && i2.DateTypeId != null) i2.DateType = FindDateType2(i2.DateTypeId, cache: cache);
                if (load && i2.InstanceTypeId != null) i2.InstanceType = FindInstanceType2(i2.InstanceTypeId, cache: cache);
                if (load && i2.IssuanceModeId != null) i2.IssuanceMode = FindIssuanceMode(i2.IssuanceModeId, cache: cache);
                if (load && i2.StatusId != null) i2.Status = FindStatus(i2.StatusId, cache: cache);
                if (load && i2.CreationUserId != null) i2.CreationUser = FindUser2(i2.CreationUserId, cache: cache);
                if (load && i2.LastWriteUserId != null) i2.LastWriteUser = FindUser2(i2.LastWriteUserId, cache: cache);
                return i2;
            }).ToArray();
        }

        public IEnumerable<Instance2> Instance2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Instances(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var i2 = cache ? (Instance2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Instance2.FromJObject(jo)) : Instance2.FromJObject(jo);
                if (load && i2.DateTypeId != null) i2.DateType = FindDateType2(i2.DateTypeId, cache: cache);
                if (load && i2.InstanceTypeId != null) i2.InstanceType = FindInstanceType2(i2.InstanceTypeId, cache: cache);
                if (load && i2.IssuanceModeId != null) i2.IssuanceMode = FindIssuanceMode(i2.IssuanceModeId, cache: cache);
                if (load && i2.StatusId != null) i2.Status = FindStatus(i2.StatusId, cache: cache);
                if (load && i2.CreationUserId != null) i2.CreationUser = FindUser2(i2.CreationUserId, cache: cache);
                if (load && i2.LastWriteUserId != null) i2.LastWriteUser = FindUser2(i2.LastWriteUserId, cache: cache);
                yield return i2;
            }
        }

        public Instance2 FindInstance2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var i2 = cache ? (Instance2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Instance2.FromJObject(FolioServiceClient.GetInstance(id?.ToString()))) : Instance2.FromJObject(FolioServiceClient.GetInstance(id?.ToString()));
            if (i2 == null) return null;
            if (load && i2.DateTypeId != null) i2.DateType = FindDateType2(i2.DateTypeId, cache: cache);
            if (load && i2.InstanceTypeId != null) i2.InstanceType = FindInstanceType2(i2.InstanceTypeId, cache: cache);
            if (load && i2.IssuanceModeId != null) i2.IssuanceMode = FindIssuanceMode(i2.IssuanceModeId, cache: cache);
            if (load && i2.StatusId != null) i2.Status = FindStatus(i2.StatusId, cache: cache);
            if (load && i2.CreationUserId != null) i2.CreationUser = FindUser2(i2.CreationUserId, cache: cache);
            if (load && i2.LastWriteUserId != null) i2.LastWriteUser = FindUser2(i2.LastWriteUserId, cache: cache);
            var i = 0;
            if (i2.AdministrativeNotes != null) foreach (var an in i2.AdministrativeNotes)
                {
                    an.Id = (++i).ToString();
                    an.InstanceId = i2.Id;
                    an.Instance = i2;
                }
            i = 0;
            if (i2.AlternativeTitles != null) foreach (var at in i2.AlternativeTitles)
                {
                    at.Id = (++i).ToString();
                    at.InstanceId = i2.Id;
                    at.Instance = i2;
                    if (load && at.AlternativeTitleTypeId != null) at.AlternativeTitleType = FindAlternativeTitleType2(at.AlternativeTitleTypeId, cache: cache);
                }
            i = 0;
            if (i2.Classifications != null) foreach (var c in i2.Classifications)
                {
                    c.Id = (++i).ToString();
                    c.InstanceId = i2.Id;
                    c.Instance = i2;
                    if (load && c.ClassificationTypeId != null) c.ClassificationType = FindClassificationType2(c.ClassificationTypeId, cache: cache);
                }
            i = 0;
            if (i2.Contributors != null) foreach (var c2 in i2.Contributors)
                {
                    c2.Id = (++i).ToString();
                    c2.InstanceId = i2.Id;
                    c2.Instance = i2;
                    if (load && c2.ContributorTypeId != null) c2.ContributorType = FindContributorType2(c2.ContributorTypeId, cache: cache);
                    if (load && c2.ContributorNameTypeId != null) c2.ContributorNameType = FindContributorNameType2(c2.ContributorNameTypeId, cache: cache);
                }
            i = 0;
            if (i2.Editions != null) foreach (var e2 in i2.Editions)
                {
                    e2.Id = (++i).ToString();
                    e2.InstanceId = i2.Id;
                    e2.Instance = i2;
                }
            i = 0;
            if (i2.ElectronicAccesses != null) foreach (var ea in i2.ElectronicAccesses)
                {
                    ea.Id = (++i).ToString();
                    ea.InstanceId = i2.Id;
                    ea.Instance = i2;
                    if (load && ea.RelationshipId != null) ea.Relationship = FindElectronicAccessRelationship2(ea.RelationshipId, cache: cache);
                }
            i = 0;
            if (i2.Identifiers != null) foreach (var i3 in i2.Identifiers)
                {
                    i3.Id = (++i).ToString();
                    i3.InstanceId = i2.Id;
                    i3.Instance = i2;
                    if (load && i3.IdentifierTypeId != null) i3.IdentifierType = FindIdType2(i3.IdentifierTypeId, cache: cache);
                }
            i = 0;
            if (i2.InstanceFormat2s != null) foreach (var if2 in i2.InstanceFormat2s)
                {
                    if2.Id = (++i).ToString();
                    if2.InstanceId = i2.Id;
                    if2.Instance = i2;
                    if (load && if2.FormatId != null) if2.Format = FindFormat(if2.FormatId, cache: cache);
                }
            i = 0;
            if (i2.InstanceNatureOfContentTerms != null) foreach (var inoct in i2.InstanceNatureOfContentTerms)
                {
                    inoct.Id = (++i).ToString();
                    inoct.InstanceId = i2.Id;
                    inoct.Instance = i2;
                    if (load && inoct.NatureOfContentTermId != null) inoct.NatureOfContentTerm = FindNatureOfContentTerm2(inoct.NatureOfContentTermId, cache: cache);
                }
            i = 0;
            if (i2.InstanceNotes != null) foreach (var @in in i2.InstanceNotes)
                {
                    @in.Id = (++i).ToString();
                    @in.InstanceId = i2.Id;
                    @in.Instance = i2;
                    if (load && @in.InstanceNoteTypeId != null) @in.InstanceNoteType = FindInstanceNoteType2(@in.InstanceNoteTypeId, cache: cache);
                }
            i = 0;
            if (i2.InstanceStatisticalCodes != null) foreach (var isc in i2.InstanceStatisticalCodes)
                {
                    isc.Id = (++i).ToString();
                    isc.InstanceId = i2.Id;
                    isc.Instance = i2;
                    if (load && isc.StatisticalCodeId != null) isc.StatisticalCode = FindStatisticalCode2(isc.StatisticalCodeId, cache: cache);
                }
            i = 0;
            if (i2.InstanceTags != null) foreach (var it in i2.InstanceTags)
                {
                    it.Id = (++i).ToString();
                    it.InstanceId = i2.Id;
                    it.Instance = i2;
                }
            i = 0;
            if (i2.Languages != null) foreach (var l in i2.Languages)
                {
                    l.Id = (++i).ToString();
                    l.InstanceId = i2.Id;
                    l.Instance = i2;
                }
            i = 0;
            if (i2.PhysicalDescriptions != null) foreach (var pd in i2.PhysicalDescriptions)
                {
                    pd.Id = (++i).ToString();
                    pd.InstanceId = i2.Id;
                    pd.Instance = i2;
                }
            i = 0;
            if (i2.PublicationFrequencies != null) foreach (var pf in i2.PublicationFrequencies)
                {
                    pf.Id = (++i).ToString();
                    pf.InstanceId = i2.Id;
                    pf.Instance = i2;
                }
            i = 0;
            if (i2.PublicationRanges != null) foreach (var pr in i2.PublicationRanges)
                {
                    pr.Id = (++i).ToString();
                    pr.InstanceId = i2.Id;
                    pr.Instance = i2;
                }
            i = 0;
            if (i2.Publications != null) foreach (var p in i2.Publications)
                {
                    p.Id = (++i).ToString();
                    p.InstanceId = i2.Id;
                    p.Instance = i2;
                }
            i = 0;
            if (i2.Series != null) foreach (var s in i2.Series)
                {
                    s.Id = (++i).ToString();
                    s.InstanceId = i2.Id;
                    s.Instance = i2;
                }
            i = 0;
            if (i2.Subjects != null) foreach (var s2 in i2.Subjects)
                {
                    s2.Id = (++i).ToString();
                    s2.InstanceId = i2.Id;
                    s2.Instance = i2;
                    if (load && s2.SourceId != null) s2.Source = FindSubjectSource2(s2.SourceId, cache: cache);
                    if (load && s2.TypeId != null) s2.Type = FindSubjectType2(s2.TypeId, cache: cache);
                }
            return i2;
        }

        public void Insert(Instance2 instance2)
        {
            if (instance2.Id == null) instance2.Id = Guid.NewGuid();
            FolioServiceClient.InsertInstance(instance2.ToJObject());
        }

        public void Update(Instance2 instance2) => FolioServiceClient.UpdateInstance(instance2.ToJObject());

        public void UpdateOrInsert(Instance2 instance2)
        {
            if (instance2.Id == null)
                Insert(instance2);
            else
                try
                {
                    Update(instance2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(instance2); else throw;
                }
        }

        public void InsertOrUpdate(Instance2 instance2)
        {
            try
            {
                Insert(instance2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(instance2); else throw;
            }
        }

        public void DeleteInstance2(Guid? id) => FolioServiceClient.DeleteInstance(id?.ToString());

        public bool AnyInstanceNoteType2s(string where = null) => FolioServiceClient.AnyInstanceNoteTypes(where);

        public int CountInstanceNoteType2s(string where = null) => FolioServiceClient.CountInstanceNoteTypes(where);

        public InstanceNoteType2[] InstanceNoteType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.InstanceNoteTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var int2 = cache ? (InstanceNoteType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = InstanceNoteType2.FromJObject(jo)) : InstanceNoteType2.FromJObject(jo);
                if (load && int2.CreationUserId != null) int2.CreationUser = FindUser2(int2.CreationUserId, cache: cache);
                if (load && int2.LastWriteUserId != null) int2.LastWriteUser = FindUser2(int2.LastWriteUserId, cache: cache);
                return int2;
            }).ToArray();
        }

        public IEnumerable<InstanceNoteType2> InstanceNoteType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.InstanceNoteTypes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var int2 = cache ? (InstanceNoteType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = InstanceNoteType2.FromJObject(jo)) : InstanceNoteType2.FromJObject(jo);
                if (load && int2.CreationUserId != null) int2.CreationUser = FindUser2(int2.CreationUserId, cache: cache);
                if (load && int2.LastWriteUserId != null) int2.LastWriteUser = FindUser2(int2.LastWriteUserId, cache: cache);
                yield return int2;
            }
        }

        public InstanceNoteType2 FindInstanceNoteType2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var int2 = cache ? (InstanceNoteType2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = InstanceNoteType2.FromJObject(FolioServiceClient.GetInstanceNoteType(id?.ToString()))) : InstanceNoteType2.FromJObject(FolioServiceClient.GetInstanceNoteType(id?.ToString()));
            if (int2 == null) return null;
            if (load && int2.CreationUserId != null) int2.CreationUser = FindUser2(int2.CreationUserId, cache: cache);
            if (load && int2.LastWriteUserId != null) int2.LastWriteUser = FindUser2(int2.LastWriteUserId, cache: cache);
            return int2;
        }

        public void Insert(InstanceNoteType2 instanceNoteType2)
        {
            if (instanceNoteType2.Id == null) instanceNoteType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertInstanceNoteType(instanceNoteType2.ToJObject());
        }

        public void Update(InstanceNoteType2 instanceNoteType2) => FolioServiceClient.UpdateInstanceNoteType(instanceNoteType2.ToJObject());

        public void UpdateOrInsert(InstanceNoteType2 instanceNoteType2)
        {
            if (instanceNoteType2.Id == null)
                Insert(instanceNoteType2);
            else
                try
                {
                    Update(instanceNoteType2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(instanceNoteType2); else throw;
                }
        }

        public void InsertOrUpdate(InstanceNoteType2 instanceNoteType2)
        {
            try
            {
                Insert(instanceNoteType2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(instanceNoteType2); else throw;
            }
        }

        public void DeleteInstanceNoteType2(Guid? id) => FolioServiceClient.DeleteInstanceNoteType(id?.ToString());

        public bool AnyInstanceType2s(string where = null) => FolioServiceClient.AnyInstanceTypes(where);

        public int CountInstanceType2s(string where = null) => FolioServiceClient.CountInstanceTypes(where);

        public InstanceType2[] InstanceType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.InstanceTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var it2 = cache ? (InstanceType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = InstanceType2.FromJObject(jo)) : InstanceType2.FromJObject(jo);
                if (load && it2.CreationUserId != null) it2.CreationUser = FindUser2(it2.CreationUserId, cache: cache);
                if (load && it2.LastWriteUserId != null) it2.LastWriteUser = FindUser2(it2.LastWriteUserId, cache: cache);
                return it2;
            }).ToArray();
        }

        public IEnumerable<InstanceType2> InstanceType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.InstanceTypes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var it2 = cache ? (InstanceType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = InstanceType2.FromJObject(jo)) : InstanceType2.FromJObject(jo);
                if (load && it2.CreationUserId != null) it2.CreationUser = FindUser2(it2.CreationUserId, cache: cache);
                if (load && it2.LastWriteUserId != null) it2.LastWriteUser = FindUser2(it2.LastWriteUserId, cache: cache);
                yield return it2;
            }
        }

        public InstanceType2 FindInstanceType2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var it2 = cache ? (InstanceType2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = InstanceType2.FromJObject(FolioServiceClient.GetInstanceType(id?.ToString()))) : InstanceType2.FromJObject(FolioServiceClient.GetInstanceType(id?.ToString()));
            if (it2 == null) return null;
            if (load && it2.CreationUserId != null) it2.CreationUser = FindUser2(it2.CreationUserId, cache: cache);
            if (load && it2.LastWriteUserId != null) it2.LastWriteUser = FindUser2(it2.LastWriteUserId, cache: cache);
            return it2;
        }

        public void Insert(InstanceType2 instanceType2)
        {
            if (instanceType2.Id == null) instanceType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertInstanceType(instanceType2.ToJObject());
        }

        public void Update(InstanceType2 instanceType2) => FolioServiceClient.UpdateInstanceType(instanceType2.ToJObject());

        public void UpdateOrInsert(InstanceType2 instanceType2)
        {
            if (instanceType2.Id == null)
                Insert(instanceType2);
            else
                try
                {
                    Update(instanceType2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(instanceType2); else throw;
                }
        }

        public void InsertOrUpdate(InstanceType2 instanceType2)
        {
            try
            {
                Insert(instanceType2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(instanceType2); else throw;
            }
        }

        public void DeleteInstanceType2(Guid? id) => FolioServiceClient.DeleteInstanceType(id?.ToString());

        public bool AnyInstitution2s(string where = null) => FolioServiceClient.AnyInstitutions(where);

        public int CountInstitution2s(string where = null) => FolioServiceClient.CountInstitutions(where);

        public Institution2[] Institution2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Institutions(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var i2 = cache ? (Institution2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Institution2.FromJObject(jo)) : Institution2.FromJObject(jo);
                if (load && i2.CreationUserId != null) i2.CreationUser = FindUser2(i2.CreationUserId, cache: cache);
                if (load && i2.LastWriteUserId != null) i2.LastWriteUser = FindUser2(i2.LastWriteUserId, cache: cache);
                return i2;
            }).ToArray();
        }

        public IEnumerable<Institution2> Institution2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Institutions(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var i2 = cache ? (Institution2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Institution2.FromJObject(jo)) : Institution2.FromJObject(jo);
                if (load && i2.CreationUserId != null) i2.CreationUser = FindUser2(i2.CreationUserId, cache: cache);
                if (load && i2.LastWriteUserId != null) i2.LastWriteUser = FindUser2(i2.LastWriteUserId, cache: cache);
                yield return i2;
            }
        }

        public Institution2 FindInstitution2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var i2 = cache ? (Institution2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Institution2.FromJObject(FolioServiceClient.GetInstitution(id?.ToString()))) : Institution2.FromJObject(FolioServiceClient.GetInstitution(id?.ToString()));
            if (i2 == null) return null;
            if (load && i2.CreationUserId != null) i2.CreationUser = FindUser2(i2.CreationUserId, cache: cache);
            if (load && i2.LastWriteUserId != null) i2.LastWriteUser = FindUser2(i2.LastWriteUserId, cache: cache);
            return i2;
        }

        public void Insert(Institution2 institution2)
        {
            if (institution2.Id == null) institution2.Id = Guid.NewGuid();
            FolioServiceClient.InsertInstitution(institution2.ToJObject());
        }

        public void Update(Institution2 institution2) => FolioServiceClient.UpdateInstitution(institution2.ToJObject());

        public void UpdateOrInsert(Institution2 institution2)
        {
            if (institution2.Id == null)
                Insert(institution2);
            else
                try
                {
                    Update(institution2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(institution2); else throw;
                }
        }

        public void InsertOrUpdate(Institution2 institution2)
        {
            try
            {
                Insert(institution2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(institution2); else throw;
            }
        }

        public void DeleteInstitution2(Guid? id) => FolioServiceClient.DeleteInstitution(id?.ToString());

        public bool AnyInterface2s(string where = null) => FolioServiceClient.AnyInterfaces(where);

        public int CountInterface2s(string where = null) => FolioServiceClient.CountInterfaces(where);

        public Interface2[] Interface2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Interfaces(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var i2 = cache ? (Interface2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Interface2.FromJObject(jo)) : Interface2.FromJObject(jo);
                if (load && i2.CreationUserId != null) i2.CreationUser = FindUser2(i2.CreationUserId, cache: cache);
                if (load && i2.LastWriteUserId != null) i2.LastWriteUser = FindUser2(i2.LastWriteUserId, cache: cache);
                return i2;
            }).ToArray();
        }

        public IEnumerable<Interface2> Interface2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Interfaces(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var i2 = cache ? (Interface2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Interface2.FromJObject(jo)) : Interface2.FromJObject(jo);
                if (load && i2.CreationUserId != null) i2.CreationUser = FindUser2(i2.CreationUserId, cache: cache);
                if (load && i2.LastWriteUserId != null) i2.LastWriteUser = FindUser2(i2.LastWriteUserId, cache: cache);
                yield return i2;
            }
        }

        public Interface2 FindInterface2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var i2 = cache ? (Interface2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Interface2.FromJObject(FolioServiceClient.GetInterface(id?.ToString()))) : Interface2.FromJObject(FolioServiceClient.GetInterface(id?.ToString()));
            if (i2 == null) return null;
            if (load && i2.CreationUserId != null) i2.CreationUser = FindUser2(i2.CreationUserId, cache: cache);
            if (load && i2.LastWriteUserId != null) i2.LastWriteUser = FindUser2(i2.LastWriteUserId, cache: cache);
            var i = 0;
            if (i2.InterfaceTypes != null) foreach (var it in i2.InterfaceTypes)
                {
                    it.Id = (++i).ToString();
                    it.InterfaceId = i2.Id;
                    it.Interface = i2;
                }
            return i2;
        }

        public void Insert(Interface2 interface2)
        {
            if (interface2.Id == null) interface2.Id = Guid.NewGuid();
            FolioServiceClient.InsertInterface(interface2.ToJObject());
        }

        public void Update(Interface2 interface2) => FolioServiceClient.UpdateInterface(interface2.ToJObject());

        public void UpdateOrInsert(Interface2 interface2)
        {
            if (interface2.Id == null)
                Insert(interface2);
            else
                try
                {
                    Update(interface2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(interface2); else throw;
                }
        }

        public void InsertOrUpdate(Interface2 interface2)
        {
            try
            {
                Insert(interface2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(interface2); else throw;
            }
        }

        public void DeleteInterface2(Guid? id) => FolioServiceClient.DeleteInterface(id?.ToString());

        public bool AnyInvoice2s(string where = null) => FolioServiceClient.AnyInvoices(where);

        public int CountInvoice2s(string where = null) => FolioServiceClient.CountInvoices(where);

        public Invoice2[] Invoice2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Invoices(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var i2 = cache ? (Invoice2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Invoice2.FromJObject(jo)) : Invoice2.FromJObject(jo);
                if (load && i2.ApprovedById != null) i2.ApprovedBy = FindUser2(i2.ApprovedById, cache: cache);
                if (load && i2.BatchGroupId != null) i2.BatchGroup = FindBatchGroup2(i2.BatchGroupId, cache: cache);
                if (load && i2.BillToId != null) i2.BillTo = FindConfiguration2(i2.BillToId, cache: cache);
                if (load && i2.PaymentId != null) i2.Payment = FindTransaction2(i2.PaymentId, cache: cache);
                if (load && i2.VendorId != null) i2.Vendor = FindOrganization2(i2.VendorId, cache: cache);
                if (load && i2.FiscalYearId != null) i2.FiscalYear = FindFiscalYear2(i2.FiscalYearId, cache: cache);
                if (load && i2.CreationUserId != null) i2.CreationUser = FindUser2(i2.CreationUserId, cache: cache);
                if (load && i2.LastWriteUserId != null) i2.LastWriteUser = FindUser2(i2.LastWriteUserId, cache: cache);
                return i2;
            }).ToArray();
        }

        public IEnumerable<Invoice2> Invoice2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Invoices(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var i2 = cache ? (Invoice2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Invoice2.FromJObject(jo)) : Invoice2.FromJObject(jo);
                if (load && i2.ApprovedById != null) i2.ApprovedBy = FindUser2(i2.ApprovedById, cache: cache);
                if (load && i2.BatchGroupId != null) i2.BatchGroup = FindBatchGroup2(i2.BatchGroupId, cache: cache);
                if (load && i2.BillToId != null) i2.BillTo = FindConfiguration2(i2.BillToId, cache: cache);
                if (load && i2.PaymentId != null) i2.Payment = FindTransaction2(i2.PaymentId, cache: cache);
                if (load && i2.VendorId != null) i2.Vendor = FindOrganization2(i2.VendorId, cache: cache);
                if (load && i2.FiscalYearId != null) i2.FiscalYear = FindFiscalYear2(i2.FiscalYearId, cache: cache);
                if (load && i2.CreationUserId != null) i2.CreationUser = FindUser2(i2.CreationUserId, cache: cache);
                if (load && i2.LastWriteUserId != null) i2.LastWriteUser = FindUser2(i2.LastWriteUserId, cache: cache);
                yield return i2;
            }
        }

        public Invoice2 FindInvoice2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var i2 = cache ? (Invoice2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Invoice2.FromJObject(FolioServiceClient.GetInvoice(id?.ToString()))) : Invoice2.FromJObject(FolioServiceClient.GetInvoice(id?.ToString()));
            if (i2 == null) return null;
            if (load && i2.ApprovedById != null) i2.ApprovedBy = FindUser2(i2.ApprovedById, cache: cache);
            if (load && i2.BatchGroupId != null) i2.BatchGroup = FindBatchGroup2(i2.BatchGroupId, cache: cache);
            if (load && i2.BillToId != null) i2.BillTo = FindConfiguration2(i2.BillToId, cache: cache);
            if (load && i2.PaymentId != null) i2.Payment = FindTransaction2(i2.PaymentId, cache: cache);
            if (load && i2.VendorId != null) i2.Vendor = FindOrganization2(i2.VendorId, cache: cache);
            if (load && i2.FiscalYearId != null) i2.FiscalYear = FindFiscalYear2(i2.FiscalYearId, cache: cache);
            if (load && i2.CreationUserId != null) i2.CreationUser = FindUser2(i2.CreationUserId, cache: cache);
            if (load && i2.LastWriteUserId != null) i2.LastWriteUser = FindUser2(i2.LastWriteUserId, cache: cache);
            var i = 0;
            if (i2.InvoiceAcquisitionsUnits != null) foreach (var iau in i2.InvoiceAcquisitionsUnits)
                {
                    iau.Id = (++i).ToString();
                    iau.InvoiceId = i2.Id;
                    iau.Invoice = i2;
                    if (load && iau.AcquisitionsUnitId != null) iau.AcquisitionsUnit = FindAcquisitionsUnit2(iau.AcquisitionsUnitId, cache: cache);
                }
            i = 0;
            if (i2.InvoiceAdjustments != null) foreach (var ia in i2.InvoiceAdjustments)
                {
                    ia.Id = (++i).ToString();
                    ia.InvoiceId = i2.Id;
                    ia.Invoice = i2;
                }
            i = 0;
            if (i2.InvoiceOrderNumbers != null) foreach (var ion in i2.InvoiceOrderNumbers)
                {
                    ion.Id = (++i).ToString();
                    ion.InvoiceId = i2.Id;
                    ion.Invoice = i2;
                }
            i = 0;
            if (i2.InvoiceTags != null) foreach (var it in i2.InvoiceTags)
                {
                    it.Id = (++i).ToString();
                    it.InvoiceId = i2.Id;
                    it.Invoice = i2;
                }
            return i2;
        }

        public void Insert(Invoice2 invoice2)
        {
            if (invoice2.Id == null) invoice2.Id = Guid.NewGuid();
            FolioServiceClient.InsertInvoice(invoice2.ToJObject());
        }

        public void Update(Invoice2 invoice2) => FolioServiceClient.UpdateInvoice(invoice2.ToJObject());

        public void UpdateOrInsert(Invoice2 invoice2)
        {
            if (invoice2.Id == null)
                Insert(invoice2);
            else
                try
                {
                    Update(invoice2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(invoice2); else throw;
                }
        }

        public void InsertOrUpdate(Invoice2 invoice2)
        {
            try
            {
                Insert(invoice2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(invoice2); else throw;
            }
        }

        public void DeleteInvoice2(Guid? id) => FolioServiceClient.DeleteInvoice(id?.ToString());

        public bool AnyInvoiceItem2s(string where = null) => FolioServiceClient.AnyInvoiceItems(where);

        public int CountInvoiceItem2s(string where = null) => FolioServiceClient.CountInvoiceItems(where);

        public InvoiceItem2[] InvoiceItem2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.InvoiceItems(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var ii2 = cache ? (InvoiceItem2)(objects.ContainsKey(id) ? objects[id] : objects[id] = InvoiceItem2.FromJObject(jo)) : InvoiceItem2.FromJObject(jo);
                if (load && ii2.InvoiceId != null) ii2.Invoice = FindInvoice2(ii2.InvoiceId, cache: cache);
                if (load && ii2.OrderItemId != null) ii2.OrderItem = FindOrderItem2(ii2.OrderItemId, cache: cache);
                if (load && ii2.ProductIdTypeId != null) ii2.ProductIdType = FindIdType2(ii2.ProductIdTypeId, cache: cache);
                if (load && ii2.CreationUserId != null) ii2.CreationUser = FindUser2(ii2.CreationUserId, cache: cache);
                if (load && ii2.LastWriteUserId != null) ii2.LastWriteUser = FindUser2(ii2.LastWriteUserId, cache: cache);
                return ii2;
            }).ToArray();
        }

        public IEnumerable<InvoiceItem2> InvoiceItem2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.InvoiceItems(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var ii2 = cache ? (InvoiceItem2)(objects.ContainsKey(id) ? objects[id] : objects[id] = InvoiceItem2.FromJObject(jo)) : InvoiceItem2.FromJObject(jo);
                if (load && ii2.InvoiceId != null) ii2.Invoice = FindInvoice2(ii2.InvoiceId, cache: cache);
                if (load && ii2.OrderItemId != null) ii2.OrderItem = FindOrderItem2(ii2.OrderItemId, cache: cache);
                if (load && ii2.ProductIdTypeId != null) ii2.ProductIdType = FindIdType2(ii2.ProductIdTypeId, cache: cache);
                if (load && ii2.CreationUserId != null) ii2.CreationUser = FindUser2(ii2.CreationUserId, cache: cache);
                if (load && ii2.LastWriteUserId != null) ii2.LastWriteUser = FindUser2(ii2.LastWriteUserId, cache: cache);
                yield return ii2;
            }
        }

        public InvoiceItem2 FindInvoiceItem2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var ii2 = cache ? (InvoiceItem2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = InvoiceItem2.FromJObject(FolioServiceClient.GetInvoiceItem(id?.ToString()))) : InvoiceItem2.FromJObject(FolioServiceClient.GetInvoiceItem(id?.ToString()));
            if (ii2 == null) return null;
            if (load && ii2.InvoiceId != null) ii2.Invoice = FindInvoice2(ii2.InvoiceId, cache: cache);
            if (load && ii2.OrderItemId != null) ii2.OrderItem = FindOrderItem2(ii2.OrderItemId, cache: cache);
            if (load && ii2.ProductIdTypeId != null) ii2.ProductIdType = FindIdType2(ii2.ProductIdTypeId, cache: cache);
            if (load && ii2.CreationUserId != null) ii2.CreationUser = FindUser2(ii2.CreationUserId, cache: cache);
            if (load && ii2.LastWriteUserId != null) ii2.LastWriteUser = FindUser2(ii2.LastWriteUserId, cache: cache);
            var i = 0;
            if (ii2.InvoiceItemAdjustments != null) foreach (var iia in ii2.InvoiceItemAdjustments)
                {
                    iia.Id = (++i).ToString();
                    iia.InvoiceItemId = ii2.Id;
                    iia.InvoiceItem = ii2;
                }
            i = 0;
            if (ii2.InvoiceItemFunds != null) foreach (var iif in ii2.InvoiceItemFunds)
                {
                    iif.Id = (++i).ToString();
                    iif.InvoiceItemId = ii2.Id;
                    iif.InvoiceItem = ii2;
                    if (load && iif.EncumbranceId != null) iif.Encumbrance = FindTransaction2(iif.EncumbranceId, cache: cache);
                    if (load && iif.FundId != null) iif.Fund = FindFund2(iif.FundId, cache: cache);
                    if (load && iif.ExpenseClassId != null) iif.ExpenseClass = FindExpenseClass2(iif.ExpenseClassId, cache: cache);
                }
            i = 0;
            if (ii2.InvoiceItemReferenceNumbers != null) foreach (var iirn in ii2.InvoiceItemReferenceNumbers)
                {
                    iirn.Id = (++i).ToString();
                    iirn.InvoiceItemId = ii2.Id;
                    iirn.InvoiceItem = ii2;
                }
            i = 0;
            if (ii2.InvoiceItemTags != null) foreach (var iit in ii2.InvoiceItemTags)
                {
                    iit.Id = (++i).ToString();
                    iit.InvoiceItemId = ii2.Id;
                    iit.InvoiceItem = ii2;
                }
            return ii2;
        }

        public void Insert(InvoiceItem2 invoiceItem2)
        {
            if (invoiceItem2.Id == null) invoiceItem2.Id = Guid.NewGuid();
            FolioServiceClient.InsertInvoiceItem(invoiceItem2.ToJObject());
        }

        public void Update(InvoiceItem2 invoiceItem2) => FolioServiceClient.UpdateInvoiceItem(invoiceItem2.ToJObject());

        public void UpdateOrInsert(InvoiceItem2 invoiceItem2)
        {
            if (invoiceItem2.Id == null)
                Insert(invoiceItem2);
            else
                try
                {
                    Update(invoiceItem2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(invoiceItem2); else throw;
                }
        }

        public void InsertOrUpdate(InvoiceItem2 invoiceItem2)
        {
            try
            {
                Insert(invoiceItem2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(invoiceItem2); else throw;
            }
        }

        public void DeleteInvoiceItem2(Guid? id) => FolioServiceClient.DeleteInvoiceItem(id?.ToString());

        public bool AnyIssuanceModes(string where = null) => FolioServiceClient.AnyModeOfIssuances(where);

        public int CountIssuanceModes(string where = null) => FolioServiceClient.CountModeOfIssuances(where);

        public IssuanceMode[] IssuanceModes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ModeOfIssuances(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var im = cache ? (IssuanceMode)(objects.ContainsKey(id) ? objects[id] : objects[id] = IssuanceMode.FromJObject(jo)) : IssuanceMode.FromJObject(jo);
                if (load && im.CreationUserId != null) im.CreationUser = FindUser2(im.CreationUserId, cache: cache);
                if (load && im.LastWriteUserId != null) im.LastWriteUser = FindUser2(im.LastWriteUserId, cache: cache);
                return im;
            }).ToArray();
        }

        public IEnumerable<IssuanceMode> IssuanceModes(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ModeOfIssuances(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var im = cache ? (IssuanceMode)(objects.ContainsKey(id) ? objects[id] : objects[id] = IssuanceMode.FromJObject(jo)) : IssuanceMode.FromJObject(jo);
                if (load && im.CreationUserId != null) im.CreationUser = FindUser2(im.CreationUserId, cache: cache);
                if (load && im.LastWriteUserId != null) im.LastWriteUser = FindUser2(im.LastWriteUserId, cache: cache);
                yield return im;
            }
        }

        public IssuanceMode FindIssuanceMode(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var im = cache ? (IssuanceMode)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = IssuanceMode.FromJObject(FolioServiceClient.GetModeOfIssuance(id?.ToString()))) : IssuanceMode.FromJObject(FolioServiceClient.GetModeOfIssuance(id?.ToString()));
            if (im == null) return null;
            if (load && im.CreationUserId != null) im.CreationUser = FindUser2(im.CreationUserId, cache: cache);
            if (load && im.LastWriteUserId != null) im.LastWriteUser = FindUser2(im.LastWriteUserId, cache: cache);
            return im;
        }

        public void Insert(IssuanceMode issuanceMode)
        {
            if (issuanceMode.Id == null) issuanceMode.Id = Guid.NewGuid();
            FolioServiceClient.InsertModeOfIssuance(issuanceMode.ToJObject());
        }

        public void Update(IssuanceMode issuanceMode) => FolioServiceClient.UpdateModeOfIssuance(issuanceMode.ToJObject());

        public void UpdateOrInsert(IssuanceMode issuanceMode)
        {
            if (issuanceMode.Id == null)
                Insert(issuanceMode);
            else
                try
                {
                    Update(issuanceMode);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(issuanceMode); else throw;
                }
        }

        public void InsertOrUpdate(IssuanceMode issuanceMode)
        {
            try
            {
                Insert(issuanceMode);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(issuanceMode); else throw;
            }
        }

        public void DeleteIssuanceMode(Guid? id) => FolioServiceClient.DeleteModeOfIssuance(id?.ToString());

        public bool AnyItem2s(string where = null) => FolioServiceClient.AnyItems(where);

        public int CountItem2s(string where = null) => FolioServiceClient.CountItems(where);

        public Item2[] Item2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Items(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var i2 = cache ? (Item2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Item2.FromJObject(jo)) : Item2.FromJObject(jo);
                if (load && i2.HoldingId != null) i2.Holding = FindHolding2(i2.HoldingId, cache: cache);
                if (load && i2.CallNumberTypeId != null) i2.CallNumberType = FindCallNumberType2(i2.CallNumberTypeId, cache: cache);
                if (load && i2.EffectiveCallNumberTypeId != null) i2.EffectiveCallNumberType = FindCallNumberType2(i2.EffectiveCallNumberTypeId, cache: cache);
                if (load && i2.DamagedStatusId != null) i2.DamagedStatus = FindItemDamagedStatus2(i2.DamagedStatusId, cache: cache);
                if (load && i2.MaterialTypeId != null) i2.MaterialType = FindMaterialType2(i2.MaterialTypeId, cache: cache);
                if (load && i2.PermanentLoanTypeId != null) i2.PermanentLoanType = FindLoanType2(i2.PermanentLoanTypeId, cache: cache);
                if (load && i2.TemporaryLoanTypeId != null) i2.TemporaryLoanType = FindLoanType2(i2.TemporaryLoanTypeId, cache: cache);
                if (load && i2.PermanentLocationId != null) i2.PermanentLocation = FindLocation2(i2.PermanentLocationId, cache: cache);
                if (load && i2.TemporaryLocationId != null) i2.TemporaryLocation = FindLocation2(i2.TemporaryLocationId, cache: cache);
                if (load && i2.EffectiveLocationId != null) i2.EffectiveLocation = FindLocation2(i2.EffectiveLocationId, cache: cache);
                if (load && i2.InTransitDestinationServicePointId != null) i2.InTransitDestinationServicePoint = FindServicePoint2(i2.InTransitDestinationServicePointId, cache: cache);
                if (load && i2.OrderItemId != null) i2.OrderItem = FindOrderItem2(i2.OrderItemId, cache: cache);
                if (load && i2.CreationUserId != null) i2.CreationUser = FindUser2(i2.CreationUserId, cache: cache);
                if (load && i2.LastWriteUserId != null) i2.LastWriteUser = FindUser2(i2.LastWriteUserId, cache: cache);
                if (load && i2.LastCheckInServicePointId != null) i2.LastCheckInServicePoint = FindServicePoint2(i2.LastCheckInServicePointId, cache: cache);
                if (load && i2.LastCheckInStaffMemberId != null) i2.LastCheckInStaffMember = FindUser2(i2.LastCheckInStaffMemberId, cache: cache);
                return i2;
            }).ToArray();
        }

        public IEnumerable<Item2> Item2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Items(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var i2 = cache ? (Item2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Item2.FromJObject(jo)) : Item2.FromJObject(jo);
                if (load && i2.HoldingId != null) i2.Holding = FindHolding2(i2.HoldingId, cache: cache);
                if (load && i2.CallNumberTypeId != null) i2.CallNumberType = FindCallNumberType2(i2.CallNumberTypeId, cache: cache);
                if (load && i2.EffectiveCallNumberTypeId != null) i2.EffectiveCallNumberType = FindCallNumberType2(i2.EffectiveCallNumberTypeId, cache: cache);
                if (load && i2.DamagedStatusId != null) i2.DamagedStatus = FindItemDamagedStatus2(i2.DamagedStatusId, cache: cache);
                if (load && i2.MaterialTypeId != null) i2.MaterialType = FindMaterialType2(i2.MaterialTypeId, cache: cache);
                if (load && i2.PermanentLoanTypeId != null) i2.PermanentLoanType = FindLoanType2(i2.PermanentLoanTypeId, cache: cache);
                if (load && i2.TemporaryLoanTypeId != null) i2.TemporaryLoanType = FindLoanType2(i2.TemporaryLoanTypeId, cache: cache);
                if (load && i2.PermanentLocationId != null) i2.PermanentLocation = FindLocation2(i2.PermanentLocationId, cache: cache);
                if (load && i2.TemporaryLocationId != null) i2.TemporaryLocation = FindLocation2(i2.TemporaryLocationId, cache: cache);
                if (load && i2.EffectiveLocationId != null) i2.EffectiveLocation = FindLocation2(i2.EffectiveLocationId, cache: cache);
                if (load && i2.InTransitDestinationServicePointId != null) i2.InTransitDestinationServicePoint = FindServicePoint2(i2.InTransitDestinationServicePointId, cache: cache);
                if (load && i2.OrderItemId != null) i2.OrderItem = FindOrderItem2(i2.OrderItemId, cache: cache);
                if (load && i2.CreationUserId != null) i2.CreationUser = FindUser2(i2.CreationUserId, cache: cache);
                if (load && i2.LastWriteUserId != null) i2.LastWriteUser = FindUser2(i2.LastWriteUserId, cache: cache);
                if (load && i2.LastCheckInServicePointId != null) i2.LastCheckInServicePoint = FindServicePoint2(i2.LastCheckInServicePointId, cache: cache);
                if (load && i2.LastCheckInStaffMemberId != null) i2.LastCheckInStaffMember = FindUser2(i2.LastCheckInStaffMemberId, cache: cache);
                yield return i2;
            }
        }

        public Item2 FindItem2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var i2 = cache ? (Item2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Item2.FromJObject(FolioServiceClient.GetItem(id?.ToString()))) : Item2.FromJObject(FolioServiceClient.GetItem(id?.ToString()));
            if (i2 == null) return null;
            if (load && i2.HoldingId != null) i2.Holding = FindHolding2(i2.HoldingId, cache: cache);
            if (load && i2.CallNumberTypeId != null) i2.CallNumberType = FindCallNumberType2(i2.CallNumberTypeId, cache: cache);
            if (load && i2.EffectiveCallNumberTypeId != null) i2.EffectiveCallNumberType = FindCallNumberType2(i2.EffectiveCallNumberTypeId, cache: cache);
            if (load && i2.DamagedStatusId != null) i2.DamagedStatus = FindItemDamagedStatus2(i2.DamagedStatusId, cache: cache);
            if (load && i2.MaterialTypeId != null) i2.MaterialType = FindMaterialType2(i2.MaterialTypeId, cache: cache);
            if (load && i2.PermanentLoanTypeId != null) i2.PermanentLoanType = FindLoanType2(i2.PermanentLoanTypeId, cache: cache);
            if (load && i2.TemporaryLoanTypeId != null) i2.TemporaryLoanType = FindLoanType2(i2.TemporaryLoanTypeId, cache: cache);
            if (load && i2.PermanentLocationId != null) i2.PermanentLocation = FindLocation2(i2.PermanentLocationId, cache: cache);
            if (load && i2.TemporaryLocationId != null) i2.TemporaryLocation = FindLocation2(i2.TemporaryLocationId, cache: cache);
            if (load && i2.EffectiveLocationId != null) i2.EffectiveLocation = FindLocation2(i2.EffectiveLocationId, cache: cache);
            if (load && i2.InTransitDestinationServicePointId != null) i2.InTransitDestinationServicePoint = FindServicePoint2(i2.InTransitDestinationServicePointId, cache: cache);
            if (load && i2.OrderItemId != null) i2.OrderItem = FindOrderItem2(i2.OrderItemId, cache: cache);
            if (load && i2.CreationUserId != null) i2.CreationUser = FindUser2(i2.CreationUserId, cache: cache);
            if (load && i2.LastWriteUserId != null) i2.LastWriteUser = FindUser2(i2.LastWriteUserId, cache: cache);
            if (load && i2.LastCheckInServicePointId != null) i2.LastCheckInServicePoint = FindServicePoint2(i2.LastCheckInServicePointId, cache: cache);
            if (load && i2.LastCheckInStaffMemberId != null) i2.LastCheckInStaffMember = FindUser2(i2.LastCheckInStaffMemberId, cache: cache);
            var i = 0;
            if (i2.CirculationNotes != null) foreach (var cn in i2.CirculationNotes)
                {
                    cn.Id = (++i).ToString();
                    cn.ItemId = i2.Id;
                    cn.Item = i2;
                }
            i = 0;
            if (i2.ItemAdditionalCallNumbers != null) foreach (var iacn in i2.ItemAdditionalCallNumbers)
                {
                    iacn.Id = (++i).ToString();
                    iacn.ItemId = i2.Id;
                    iacn.Item = i2;
                    if (load && iacn.TypeId != null) iacn.Type = FindCallNumberType2(iacn.TypeId, cache: cache);
                }
            i = 0;
            if (i2.ItemAdministrativeNotes != null) foreach (var ian in i2.ItemAdministrativeNotes)
                {
                    ian.Id = (++i).ToString();
                    ian.ItemId = i2.Id;
                    ian.Item = i2;
                }
            i = 0;
            if (i2.ItemElectronicAccesses != null) foreach (var iea in i2.ItemElectronicAccesses)
                {
                    iea.Id = (++i).ToString();
                    iea.ItemId = i2.Id;
                    iea.Item = i2;
                    if (load && iea.RelationshipId != null) iea.Relationship = FindElectronicAccessRelationship2(iea.RelationshipId, cache: cache);
                }
            i = 0;
            if (i2.ItemFormerIds != null) foreach (var ifi in i2.ItemFormerIds)
                {
                    ifi.Id = (++i).ToString();
                    ifi.ItemId = i2.Id;
                    ifi.Item = i2;
                }
            i = 0;
            if (i2.ItemNotes != null) foreach (var @in in i2.ItemNotes)
                {
                    @in.Id = (++i).ToString();
                    @in.ItemId = i2.Id;
                    @in.Item = i2;
                    if (load && @in.ItemNoteTypeId != null) @in.ItemNoteType = FindItemNoteType2(@in.ItemNoteTypeId, cache: cache);
                    if (load && @in.CreationUserId != null) @in.CreationUser = FindUser2(@in.CreationUserId, cache: cache);
                    if (load && @in.LastWriteUserId != null) @in.LastWriteUser = FindUser2(@in.LastWriteUserId, cache: cache);
                }
            i = 0;
            if (i2.ItemStatisticalCodes != null) foreach (var isc in i2.ItemStatisticalCodes)
                {
                    isc.Id = (++i).ToString();
                    isc.ItemId = i2.Id;
                    isc.Item = i2;
                    if (load && isc.StatisticalCodeId != null) isc.StatisticalCode = FindStatisticalCode2(isc.StatisticalCodeId, cache: cache);
                }
            i = 0;
            if (i2.ItemTags != null) foreach (var it in i2.ItemTags)
                {
                    it.Id = (++i).ToString();
                    it.ItemId = i2.Id;
                    it.Item = i2;
                }
            i = 0;
            if (i2.ItemYearCaptions != null) foreach (var iyc in i2.ItemYearCaptions)
                {
                    iyc.Id = (++i).ToString();
                    iyc.ItemId = i2.Id;
                    iyc.Item = i2;
                }
            return i2;
        }

        public void Insert(Item2 item2)
        {
            if (item2.Id == null) item2.Id = Guid.NewGuid();
            FolioServiceClient.InsertItem(item2.ToJObject());
        }

        public void Update(Item2 item2) => FolioServiceClient.UpdateItem(item2.ToJObject());

        public void UpdateOrInsert(Item2 item2)
        {
            if (item2.Id == null)
                Insert(item2);
            else
                try
                {
                    Update(item2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(item2); else throw;
                }
        }

        public void InsertOrUpdate(Item2 item2)
        {
            try
            {
                Insert(item2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(item2); else throw;
            }
        }

        public void DeleteItem2(Guid? id) => FolioServiceClient.DeleteItem(id?.ToString());

        public bool AnyItemDamagedStatus2s(string where = null) => FolioServiceClient.AnyItemDamagedStatuses(where);

        public int CountItemDamagedStatus2s(string where = null) => FolioServiceClient.CountItemDamagedStatuses(where);

        public ItemDamagedStatus2[] ItemDamagedStatus2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ItemDamagedStatuses(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var ids2 = cache ? (ItemDamagedStatus2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ItemDamagedStatus2.FromJObject(jo)) : ItemDamagedStatus2.FromJObject(jo);
                if (load && ids2.CreationUserId != null) ids2.CreationUser = FindUser2(ids2.CreationUserId, cache: cache);
                if (load && ids2.LastWriteUserId != null) ids2.LastWriteUser = FindUser2(ids2.LastWriteUserId, cache: cache);
                return ids2;
            }).ToArray();
        }

        public IEnumerable<ItemDamagedStatus2> ItemDamagedStatus2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ItemDamagedStatuses(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var ids2 = cache ? (ItemDamagedStatus2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ItemDamagedStatus2.FromJObject(jo)) : ItemDamagedStatus2.FromJObject(jo);
                if (load && ids2.CreationUserId != null) ids2.CreationUser = FindUser2(ids2.CreationUserId, cache: cache);
                if (load && ids2.LastWriteUserId != null) ids2.LastWriteUser = FindUser2(ids2.LastWriteUserId, cache: cache);
                yield return ids2;
            }
        }

        public ItemDamagedStatus2 FindItemDamagedStatus2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var ids2 = cache ? (ItemDamagedStatus2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = ItemDamagedStatus2.FromJObject(FolioServiceClient.GetItemDamagedStatus(id?.ToString()))) : ItemDamagedStatus2.FromJObject(FolioServiceClient.GetItemDamagedStatus(id?.ToString()));
            if (ids2 == null) return null;
            if (load && ids2.CreationUserId != null) ids2.CreationUser = FindUser2(ids2.CreationUserId, cache: cache);
            if (load && ids2.LastWriteUserId != null) ids2.LastWriteUser = FindUser2(ids2.LastWriteUserId, cache: cache);
            return ids2;
        }

        public void Insert(ItemDamagedStatus2 itemDamagedStatus2)
        {
            if (itemDamagedStatus2.Id == null) itemDamagedStatus2.Id = Guid.NewGuid();
            FolioServiceClient.InsertItemDamagedStatus(itemDamagedStatus2.ToJObject());
        }

        public void Update(ItemDamagedStatus2 itemDamagedStatus2) => FolioServiceClient.UpdateItemDamagedStatus(itemDamagedStatus2.ToJObject());

        public void UpdateOrInsert(ItemDamagedStatus2 itemDamagedStatus2)
        {
            if (itemDamagedStatus2.Id == null)
                Insert(itemDamagedStatus2);
            else
                try
                {
                    Update(itemDamagedStatus2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(itemDamagedStatus2); else throw;
                }
        }

        public void InsertOrUpdate(ItemDamagedStatus2 itemDamagedStatus2)
        {
            try
            {
                Insert(itemDamagedStatus2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(itemDamagedStatus2); else throw;
            }
        }

        public void DeleteItemDamagedStatus2(Guid? id) => FolioServiceClient.DeleteItemDamagedStatus(id?.ToString());

        public bool AnyItemNoteType2s(string where = null) => FolioServiceClient.AnyItemNoteTypes(where);

        public int CountItemNoteType2s(string where = null) => FolioServiceClient.CountItemNoteTypes(where);

        public ItemNoteType2[] ItemNoteType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ItemNoteTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var int2 = cache ? (ItemNoteType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ItemNoteType2.FromJObject(jo)) : ItemNoteType2.FromJObject(jo);
                if (load && int2.CreationUserId != null) int2.CreationUser = FindUser2(int2.CreationUserId, cache: cache);
                if (load && int2.LastWriteUserId != null) int2.LastWriteUser = FindUser2(int2.LastWriteUserId, cache: cache);
                return int2;
            }).ToArray();
        }

        public IEnumerable<ItemNoteType2> ItemNoteType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ItemNoteTypes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var int2 = cache ? (ItemNoteType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ItemNoteType2.FromJObject(jo)) : ItemNoteType2.FromJObject(jo);
                if (load && int2.CreationUserId != null) int2.CreationUser = FindUser2(int2.CreationUserId, cache: cache);
                if (load && int2.LastWriteUserId != null) int2.LastWriteUser = FindUser2(int2.LastWriteUserId, cache: cache);
                yield return int2;
            }
        }

        public ItemNoteType2 FindItemNoteType2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var int2 = cache ? (ItemNoteType2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = ItemNoteType2.FromJObject(FolioServiceClient.GetItemNoteType(id?.ToString()))) : ItemNoteType2.FromJObject(FolioServiceClient.GetItemNoteType(id?.ToString()));
            if (int2 == null) return null;
            if (load && int2.CreationUserId != null) int2.CreationUser = FindUser2(int2.CreationUserId, cache: cache);
            if (load && int2.LastWriteUserId != null) int2.LastWriteUser = FindUser2(int2.LastWriteUserId, cache: cache);
            return int2;
        }

        public void Insert(ItemNoteType2 itemNoteType2)
        {
            if (itemNoteType2.Id == null) itemNoteType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertItemNoteType(itemNoteType2.ToJObject());
        }

        public void Update(ItemNoteType2 itemNoteType2) => FolioServiceClient.UpdateItemNoteType(itemNoteType2.ToJObject());

        public void UpdateOrInsert(ItemNoteType2 itemNoteType2)
        {
            if (itemNoteType2.Id == null)
                Insert(itemNoteType2);
            else
                try
                {
                    Update(itemNoteType2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(itemNoteType2); else throw;
                }
        }

        public void InsertOrUpdate(ItemNoteType2 itemNoteType2)
        {
            try
            {
                Insert(itemNoteType2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(itemNoteType2); else throw;
            }
        }

        public void DeleteItemNoteType2(Guid? id) => FolioServiceClient.DeleteItemNoteType(id?.ToString());

        public bool AnyLedger2s(string where = null) => FolioServiceClient.AnyLedgers(where);

        public int CountLedger2s(string where = null) => FolioServiceClient.CountLedgers(where);

        public Ledger2[] Ledger2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Ledgers(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var l2 = cache ? (Ledger2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Ledger2.FromJObject(jo)) : Ledger2.FromJObject(jo);
                if (load && l2.FiscalYearOneId != null) l2.FiscalYearOne = FindFiscalYear2(l2.FiscalYearOneId, cache: cache);
                if (load && l2.CreationUserId != null) l2.CreationUser = FindUser2(l2.CreationUserId, cache: cache);
                if (load && l2.LastWriteUserId != null) l2.LastWriteUser = FindUser2(l2.LastWriteUserId, cache: cache);
                return l2;
            }).ToArray();
        }

        public IEnumerable<Ledger2> Ledger2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Ledgers(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var l2 = cache ? (Ledger2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Ledger2.FromJObject(jo)) : Ledger2.FromJObject(jo);
                if (load && l2.FiscalYearOneId != null) l2.FiscalYearOne = FindFiscalYear2(l2.FiscalYearOneId, cache: cache);
                if (load && l2.CreationUserId != null) l2.CreationUser = FindUser2(l2.CreationUserId, cache: cache);
                if (load && l2.LastWriteUserId != null) l2.LastWriteUser = FindUser2(l2.LastWriteUserId, cache: cache);
                yield return l2;
            }
        }

        public Ledger2 FindLedger2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var l2 = cache ? (Ledger2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Ledger2.FromJObject(FolioServiceClient.GetLedger(id?.ToString()))) : Ledger2.FromJObject(FolioServiceClient.GetLedger(id?.ToString()));
            if (l2 == null) return null;
            if (load && l2.FiscalYearOneId != null) l2.FiscalYearOne = FindFiscalYear2(l2.FiscalYearOneId, cache: cache);
            if (load && l2.CreationUserId != null) l2.CreationUser = FindUser2(l2.CreationUserId, cache: cache);
            if (load && l2.LastWriteUserId != null) l2.LastWriteUser = FindUser2(l2.LastWriteUserId, cache: cache);
            var i = 0;
            if (l2.LedgerAcquisitionsUnits != null) foreach (var lau in l2.LedgerAcquisitionsUnits)
                {
                    lau.Id = (++i).ToString();
                    lau.LedgerId = l2.Id;
                    lau.Ledger = l2;
                    if (load && lau.AcquisitionsUnitId != null) lau.AcquisitionsUnit = FindAcquisitionsUnit2(lau.AcquisitionsUnitId, cache: cache);
                }
            return l2;
        }

        public void Insert(Ledger2 ledger2)
        {
            if (ledger2.Id == null) ledger2.Id = Guid.NewGuid();
            FolioServiceClient.InsertLedger(ledger2.ToJObject());
        }

        public void Update(Ledger2 ledger2) => FolioServiceClient.UpdateLedger(ledger2.ToJObject());

        public void UpdateOrInsert(Ledger2 ledger2)
        {
            if (ledger2.Id == null)
                Insert(ledger2);
            else
                try
                {
                    Update(ledger2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(ledger2); else throw;
                }
        }

        public void InsertOrUpdate(Ledger2 ledger2)
        {
            try
            {
                Insert(ledger2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(ledger2); else throw;
            }
        }

        public void DeleteLedger2(Guid? id) => FolioServiceClient.DeleteLedger(id?.ToString());

        public bool AnyLibrary2s(string where = null) => FolioServiceClient.AnyLibraries(where);

        public int CountLibrary2s(string where = null) => FolioServiceClient.CountLibraries(where);

        public Library2[] Library2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Libraries(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var l2 = cache ? (Library2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Library2.FromJObject(jo)) : Library2.FromJObject(jo);
                if (load && l2.CampusId != null) l2.Campus = FindCampus2(l2.CampusId, cache: cache);
                if (load && l2.CreationUserId != null) l2.CreationUser = FindUser2(l2.CreationUserId, cache: cache);
                if (load && l2.LastWriteUserId != null) l2.LastWriteUser = FindUser2(l2.LastWriteUserId, cache: cache);
                return l2;
            }).ToArray();
        }

        public IEnumerable<Library2> Library2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Libraries(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var l2 = cache ? (Library2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Library2.FromJObject(jo)) : Library2.FromJObject(jo);
                if (load && l2.CampusId != null) l2.Campus = FindCampus2(l2.CampusId, cache: cache);
                if (load && l2.CreationUserId != null) l2.CreationUser = FindUser2(l2.CreationUserId, cache: cache);
                if (load && l2.LastWriteUserId != null) l2.LastWriteUser = FindUser2(l2.LastWriteUserId, cache: cache);
                yield return l2;
            }
        }

        public Library2 FindLibrary2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var l2 = cache ? (Library2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Library2.FromJObject(FolioServiceClient.GetLibrary(id?.ToString()))) : Library2.FromJObject(FolioServiceClient.GetLibrary(id?.ToString()));
            if (l2 == null) return null;
            if (load && l2.CampusId != null) l2.Campus = FindCampus2(l2.CampusId, cache: cache);
            if (load && l2.CreationUserId != null) l2.CreationUser = FindUser2(l2.CreationUserId, cache: cache);
            if (load && l2.LastWriteUserId != null) l2.LastWriteUser = FindUser2(l2.LastWriteUserId, cache: cache);
            return l2;
        }

        public void Insert(Library2 library2)
        {
            if (library2.Id == null) library2.Id = Guid.NewGuid();
            FolioServiceClient.InsertLibrary(library2.ToJObject());
        }

        public void Update(Library2 library2) => FolioServiceClient.UpdateLibrary(library2.ToJObject());

        public void UpdateOrInsert(Library2 library2)
        {
            if (library2.Id == null)
                Insert(library2);
            else
                try
                {
                    Update(library2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(library2); else throw;
                }
        }

        public void InsertOrUpdate(Library2 library2)
        {
            try
            {
                Insert(library2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(library2); else throw;
            }
        }

        public void DeleteLibrary2(Guid? id) => FolioServiceClient.DeleteLibrary(id?.ToString());

        public bool AnyLoan2s(string where = null) => FolioServiceClient.AnyLoans(where);

        public int CountLoan2s(string where = null) => FolioServiceClient.CountLoans(where);

        public Loan2[] Loan2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Loans(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var l2 = cache ? (Loan2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Loan2.FromJObject(jo)) : Loan2.FromJObject(jo);
                if (load && l2.UserId != null) l2.User = FindUser2(l2.UserId, cache: cache);
                if (load && l2.ProxyUserId != null) l2.ProxyUser = FindUser2(l2.ProxyUserId, cache: cache);
                if (load && l2.ItemId != null) l2.Item = FindItem2(l2.ItemId, cache: cache);
                if (load && l2.ItemEffectiveLocationAtCheckOutId != null) l2.ItemEffectiveLocationAtCheckOut = FindLocation2(l2.ItemEffectiveLocationAtCheckOutId, cache: cache);
                if (load && l2.LoanPolicyId != null) l2.LoanPolicy = FindLoanPolicy2(l2.LoanPolicyId, cache: cache);
                if (load && l2.CheckoutServicePointId != null) l2.CheckoutServicePoint = FindServicePoint2(l2.CheckoutServicePointId, cache: cache);
                if (load && l2.CheckinServicePointId != null) l2.CheckinServicePoint = FindServicePoint2(l2.CheckinServicePointId, cache: cache);
                if (load && l2.GroupId != null) l2.Group = FindGroup2(l2.GroupId, cache: cache);
                if (load && l2.OverdueFinePolicyId != null) l2.OverdueFinePolicy = FindOverdueFinePolicy2(l2.OverdueFinePolicyId, cache: cache);
                if (load && l2.LostItemPolicyId != null) l2.LostItemPolicy = FindLostItemFeePolicy2(l2.LostItemPolicyId, cache: cache);
                if (load && l2.CreationUserId != null) l2.CreationUser = FindUser2(l2.CreationUserId, cache: cache);
                if (load && l2.LastWriteUserId != null) l2.LastWriteUser = FindUser2(l2.LastWriteUserId, cache: cache);
                return l2;
            }).ToArray();
        }

        public IEnumerable<Loan2> Loan2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Loans(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var l2 = cache ? (Loan2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Loan2.FromJObject(jo)) : Loan2.FromJObject(jo);
                if (load && l2.UserId != null) l2.User = FindUser2(l2.UserId, cache: cache);
                if (load && l2.ProxyUserId != null) l2.ProxyUser = FindUser2(l2.ProxyUserId, cache: cache);
                if (load && l2.ItemId != null) l2.Item = FindItem2(l2.ItemId, cache: cache);
                if (load && l2.ItemEffectiveLocationAtCheckOutId != null) l2.ItemEffectiveLocationAtCheckOut = FindLocation2(l2.ItemEffectiveLocationAtCheckOutId, cache: cache);
                if (load && l2.LoanPolicyId != null) l2.LoanPolicy = FindLoanPolicy2(l2.LoanPolicyId, cache: cache);
                if (load && l2.CheckoutServicePointId != null) l2.CheckoutServicePoint = FindServicePoint2(l2.CheckoutServicePointId, cache: cache);
                if (load && l2.CheckinServicePointId != null) l2.CheckinServicePoint = FindServicePoint2(l2.CheckinServicePointId, cache: cache);
                if (load && l2.GroupId != null) l2.Group = FindGroup2(l2.GroupId, cache: cache);
                if (load && l2.OverdueFinePolicyId != null) l2.OverdueFinePolicy = FindOverdueFinePolicy2(l2.OverdueFinePolicyId, cache: cache);
                if (load && l2.LostItemPolicyId != null) l2.LostItemPolicy = FindLostItemFeePolicy2(l2.LostItemPolicyId, cache: cache);
                if (load && l2.CreationUserId != null) l2.CreationUser = FindUser2(l2.CreationUserId, cache: cache);
                if (load && l2.LastWriteUserId != null) l2.LastWriteUser = FindUser2(l2.LastWriteUserId, cache: cache);
                yield return l2;
            }
        }

        public Loan2 FindLoan2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var l2 = cache ? (Loan2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Loan2.FromJObject(FolioServiceClient.GetLoan(id?.ToString()))) : Loan2.FromJObject(FolioServiceClient.GetLoan(id?.ToString()));
            if (l2 == null) return null;
            if (load && l2.UserId != null) l2.User = FindUser2(l2.UserId, cache: cache);
            if (load && l2.ProxyUserId != null) l2.ProxyUser = FindUser2(l2.ProxyUserId, cache: cache);
            if (load && l2.ItemId != null) l2.Item = FindItem2(l2.ItemId, cache: cache);
            if (load && l2.ItemEffectiveLocationAtCheckOutId != null) l2.ItemEffectiveLocationAtCheckOut = FindLocation2(l2.ItemEffectiveLocationAtCheckOutId, cache: cache);
            if (load && l2.LoanPolicyId != null) l2.LoanPolicy = FindLoanPolicy2(l2.LoanPolicyId, cache: cache);
            if (load && l2.CheckoutServicePointId != null) l2.CheckoutServicePoint = FindServicePoint2(l2.CheckoutServicePointId, cache: cache);
            if (load && l2.CheckinServicePointId != null) l2.CheckinServicePoint = FindServicePoint2(l2.CheckinServicePointId, cache: cache);
            if (load && l2.GroupId != null) l2.Group = FindGroup2(l2.GroupId, cache: cache);
            if (load && l2.OverdueFinePolicyId != null) l2.OverdueFinePolicy = FindOverdueFinePolicy2(l2.OverdueFinePolicyId, cache: cache);
            if (load && l2.LostItemPolicyId != null) l2.LostItemPolicy = FindLostItemFeePolicy2(l2.LostItemPolicyId, cache: cache);
            if (load && l2.CreationUserId != null) l2.CreationUser = FindUser2(l2.CreationUserId, cache: cache);
            if (load && l2.LastWriteUserId != null) l2.LastWriteUser = FindUser2(l2.LastWriteUserId, cache: cache);
            return l2;
        }

        public void Insert(Loan2 loan2)
        {
            if (loan2.Id == null) loan2.Id = Guid.NewGuid();
            FolioServiceClient.InsertLoan(loan2.ToJObject());
        }

        public void Update(Loan2 loan2) => FolioServiceClient.UpdateLoan(loan2.ToJObject());

        public void UpdateOrInsert(Loan2 loan2)
        {
            if (loan2.Id == null)
                Insert(loan2);
            else
                try
                {
                    Update(loan2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(loan2); else throw;
                }
        }

        public void InsertOrUpdate(Loan2 loan2)
        {
            try
            {
                Insert(loan2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(loan2); else throw;
            }
        }

        public void DeleteLoan2(Guid? id) => FolioServiceClient.DeleteLoan(id?.ToString());

        public bool AnyLoanPolicy2s(string where = null) => FolioServiceClient.AnyLoanPolicies(where);

        public int CountLoanPolicy2s(string where = null) => FolioServiceClient.CountLoanPolicies(where);

        public LoanPolicy2[] LoanPolicy2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.LoanPolicies(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var lp2 = cache ? (LoanPolicy2)(objects.ContainsKey(id) ? objects[id] : objects[id] = LoanPolicy2.FromJObject(jo)) : LoanPolicy2.FromJObject(jo);
                if (load && lp2.LoansPolicyFixedDueDateScheduleId != null) lp2.LoansPolicyFixedDueDateSchedule = FindFixedDueDateSchedule2(lp2.LoansPolicyFixedDueDateScheduleId, cache: cache);
                if (load && lp2.RenewalsPolicyAlternateFixedDueDateScheduleId != null) lp2.RenewalsPolicyAlternateFixedDueDateSchedule = FindFixedDueDateSchedule2(lp2.RenewalsPolicyAlternateFixedDueDateScheduleId, cache: cache);
                if (load && lp2.CreationUserId != null) lp2.CreationUser = FindUser2(lp2.CreationUserId, cache: cache);
                if (load && lp2.LastWriteUserId != null) lp2.LastWriteUser = FindUser2(lp2.LastWriteUserId, cache: cache);
                return lp2;
            }).ToArray();
        }

        public IEnumerable<LoanPolicy2> LoanPolicy2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.LoanPolicies(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var lp2 = cache ? (LoanPolicy2)(objects.ContainsKey(id) ? objects[id] : objects[id] = LoanPolicy2.FromJObject(jo)) : LoanPolicy2.FromJObject(jo);
                if (load && lp2.LoansPolicyFixedDueDateScheduleId != null) lp2.LoansPolicyFixedDueDateSchedule = FindFixedDueDateSchedule2(lp2.LoansPolicyFixedDueDateScheduleId, cache: cache);
                if (load && lp2.RenewalsPolicyAlternateFixedDueDateScheduleId != null) lp2.RenewalsPolicyAlternateFixedDueDateSchedule = FindFixedDueDateSchedule2(lp2.RenewalsPolicyAlternateFixedDueDateScheduleId, cache: cache);
                if (load && lp2.CreationUserId != null) lp2.CreationUser = FindUser2(lp2.CreationUserId, cache: cache);
                if (load && lp2.LastWriteUserId != null) lp2.LastWriteUser = FindUser2(lp2.LastWriteUserId, cache: cache);
                yield return lp2;
            }
        }

        public LoanPolicy2 FindLoanPolicy2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var lp2 = cache ? (LoanPolicy2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = LoanPolicy2.FromJObject(FolioServiceClient.GetLoanPolicy(id?.ToString()))) : LoanPolicy2.FromJObject(FolioServiceClient.GetLoanPolicy(id?.ToString()));
            if (lp2 == null) return null;
            if (load && lp2.LoansPolicyFixedDueDateScheduleId != null) lp2.LoansPolicyFixedDueDateSchedule = FindFixedDueDateSchedule2(lp2.LoansPolicyFixedDueDateScheduleId, cache: cache);
            if (load && lp2.RenewalsPolicyAlternateFixedDueDateScheduleId != null) lp2.RenewalsPolicyAlternateFixedDueDateSchedule = FindFixedDueDateSchedule2(lp2.RenewalsPolicyAlternateFixedDueDateScheduleId, cache: cache);
            if (load && lp2.CreationUserId != null) lp2.CreationUser = FindUser2(lp2.CreationUserId, cache: cache);
            if (load && lp2.LastWriteUserId != null) lp2.LastWriteUser = FindUser2(lp2.LastWriteUserId, cache: cache);
            return lp2;
        }

        public void Insert(LoanPolicy2 loanPolicy2)
        {
            if (loanPolicy2.Id == null) loanPolicy2.Id = Guid.NewGuid();
            FolioServiceClient.InsertLoanPolicy(loanPolicy2.ToJObject());
        }

        public void Update(LoanPolicy2 loanPolicy2) => FolioServiceClient.UpdateLoanPolicy(loanPolicy2.ToJObject());

        public void UpdateOrInsert(LoanPolicy2 loanPolicy2)
        {
            if (loanPolicy2.Id == null)
                Insert(loanPolicy2);
            else
                try
                {
                    Update(loanPolicy2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(loanPolicy2); else throw;
                }
        }

        public void InsertOrUpdate(LoanPolicy2 loanPolicy2)
        {
            try
            {
                Insert(loanPolicy2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(loanPolicy2); else throw;
            }
        }

        public void DeleteLoanPolicy2(Guid? id) => FolioServiceClient.DeleteLoanPolicy(id?.ToString());

        public bool AnyLoanType2s(string where = null) => FolioServiceClient.AnyLoanTypes(where);

        public int CountLoanType2s(string where = null) => FolioServiceClient.CountLoanTypes(where);

        public LoanType2[] LoanType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.LoanTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var lt2 = cache ? (LoanType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = LoanType2.FromJObject(jo)) : LoanType2.FromJObject(jo);
                if (load && lt2.CreationUserId != null) lt2.CreationUser = FindUser2(lt2.CreationUserId, cache: cache);
                if (load && lt2.LastWriteUserId != null) lt2.LastWriteUser = FindUser2(lt2.LastWriteUserId, cache: cache);
                return lt2;
            }).ToArray();
        }

        public IEnumerable<LoanType2> LoanType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.LoanTypes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var lt2 = cache ? (LoanType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = LoanType2.FromJObject(jo)) : LoanType2.FromJObject(jo);
                if (load && lt2.CreationUserId != null) lt2.CreationUser = FindUser2(lt2.CreationUserId, cache: cache);
                if (load && lt2.LastWriteUserId != null) lt2.LastWriteUser = FindUser2(lt2.LastWriteUserId, cache: cache);
                yield return lt2;
            }
        }

        public LoanType2 FindLoanType2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var lt2 = cache ? (LoanType2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = LoanType2.FromJObject(FolioServiceClient.GetLoanType(id?.ToString()))) : LoanType2.FromJObject(FolioServiceClient.GetLoanType(id?.ToString()));
            if (lt2 == null) return null;
            if (load && lt2.CreationUserId != null) lt2.CreationUser = FindUser2(lt2.CreationUserId, cache: cache);
            if (load && lt2.LastWriteUserId != null) lt2.LastWriteUser = FindUser2(lt2.LastWriteUserId, cache: cache);
            return lt2;
        }

        public void Insert(LoanType2 loanType2)
        {
            if (loanType2.Id == null) loanType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertLoanType(loanType2.ToJObject());
        }

        public void Update(LoanType2 loanType2) => FolioServiceClient.UpdateLoanType(loanType2.ToJObject());

        public void UpdateOrInsert(LoanType2 loanType2)
        {
            if (loanType2.Id == null)
                Insert(loanType2);
            else
                try
                {
                    Update(loanType2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(loanType2); else throw;
                }
        }

        public void InsertOrUpdate(LoanType2 loanType2)
        {
            try
            {
                Insert(loanType2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(loanType2); else throw;
            }
        }

        public void DeleteLoanType2(Guid? id) => FolioServiceClient.DeleteLoanType(id?.ToString());

        public bool AnyLocation2s(string where = null) => FolioServiceClient.AnyLocations(where);

        public int CountLocation2s(string where = null) => FolioServiceClient.CountLocations(where);

        public Location2[] Location2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Locations(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var l2 = cache ? (Location2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Location2.FromJObject(jo)) : Location2.FromJObject(jo);
                if (load && l2.InstitutionId != null) l2.Institution = FindInstitution2(l2.InstitutionId, cache: cache);
                if (load && l2.CampusId != null) l2.Campus = FindCampus2(l2.CampusId, cache: cache);
                if (load && l2.LibraryId != null) l2.Library = FindLibrary2(l2.LibraryId, cache: cache);
                if (load && l2.PrimaryServicePointId != null) l2.PrimaryServicePoint = FindServicePoint2(l2.PrimaryServicePointId, cache: cache);
                if (load && l2.CreationUserId != null) l2.CreationUser = FindUser2(l2.CreationUserId, cache: cache);
                if (load && l2.LastWriteUserId != null) l2.LastWriteUser = FindUser2(l2.LastWriteUserId, cache: cache);
                return l2;
            }).ToArray();
        }

        public IEnumerable<Location2> Location2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Locations(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var l2 = cache ? (Location2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Location2.FromJObject(jo)) : Location2.FromJObject(jo);
                if (load && l2.InstitutionId != null) l2.Institution = FindInstitution2(l2.InstitutionId, cache: cache);
                if (load && l2.CampusId != null) l2.Campus = FindCampus2(l2.CampusId, cache: cache);
                if (load && l2.LibraryId != null) l2.Library = FindLibrary2(l2.LibraryId, cache: cache);
                if (load && l2.PrimaryServicePointId != null) l2.PrimaryServicePoint = FindServicePoint2(l2.PrimaryServicePointId, cache: cache);
                if (load && l2.CreationUserId != null) l2.CreationUser = FindUser2(l2.CreationUserId, cache: cache);
                if (load && l2.LastWriteUserId != null) l2.LastWriteUser = FindUser2(l2.LastWriteUserId, cache: cache);
                yield return l2;
            }
        }

        public Location2 FindLocation2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var l2 = cache ? (Location2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Location2.FromJObject(FolioServiceClient.GetLocation(id?.ToString()))) : Location2.FromJObject(FolioServiceClient.GetLocation(id?.ToString()));
            if (l2 == null) return null;
            if (load && l2.InstitutionId != null) l2.Institution = FindInstitution2(l2.InstitutionId, cache: cache);
            if (load && l2.CampusId != null) l2.Campus = FindCampus2(l2.CampusId, cache: cache);
            if (load && l2.LibraryId != null) l2.Library = FindLibrary2(l2.LibraryId, cache: cache);
            if (load && l2.PrimaryServicePointId != null) l2.PrimaryServicePoint = FindServicePoint2(l2.PrimaryServicePointId, cache: cache);
            if (load && l2.CreationUserId != null) l2.CreationUser = FindUser2(l2.CreationUserId, cache: cache);
            if (load && l2.LastWriteUserId != null) l2.LastWriteUser = FindUser2(l2.LastWriteUserId, cache: cache);
            var i = 0;
            if (l2.LocationServicePoints != null) foreach (var lsp in l2.LocationServicePoints)
                {
                    lsp.Id = (++i).ToString();
                    lsp.LocationId = l2.Id;
                    lsp.Location = l2;
                    if (load && lsp.ServicePointId != null) lsp.ServicePoint = FindServicePoint2(lsp.ServicePointId, cache: cache);
                }
            return l2;
        }

        public void Insert(Location2 location2)
        {
            if (location2.Id == null) location2.Id = Guid.NewGuid();
            FolioServiceClient.InsertLocation(location2.ToJObject());
        }

        public void Update(Location2 location2) => FolioServiceClient.UpdateLocation(location2.ToJObject());

        public void UpdateOrInsert(Location2 location2)
        {
            if (location2.Id == null)
                Insert(location2);
            else
                try
                {
                    Update(location2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(location2); else throw;
                }
        }

        public void InsertOrUpdate(Location2 location2)
        {
            try
            {
                Insert(location2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(location2); else throw;
            }
        }

        public void DeleteLocation2(Guid? id) => FolioServiceClient.DeleteLocation(id?.ToString());

        public bool AnyLocationSettings(string where = null) => FolioServiceClient.AnyLocationSettings(where);

        public int CountLocationSettings(string where = null) => FolioServiceClient.CountLocationSettings(where);

        public LocationSetting[] LocationSettings(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.LocationSettings(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var ls = cache ? (LocationSetting)(objects.ContainsKey(id) ? objects[id] : objects[id] = LocationSetting.FromJObject(jo)) : LocationSetting.FromJObject(jo);
                if (load && ls.LocationId != null) ls.Location = FindLocation2(ls.LocationId, cache: cache);
                if (load && ls.SettingsId != null) ls.Settings = FindSetting(ls.SettingsId, cache: cache);
                if (load && ls.CreationUserId != null) ls.CreationUser = FindUser2(ls.CreationUserId, cache: cache);
                if (load && ls.LastWriteUserId != null) ls.LastWriteUser = FindUser2(ls.LastWriteUserId, cache: cache);
                return ls;
            }).ToArray();
        }

        public IEnumerable<LocationSetting> LocationSettings(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.LocationSettings(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var ls = cache ? (LocationSetting)(objects.ContainsKey(id) ? objects[id] : objects[id] = LocationSetting.FromJObject(jo)) : LocationSetting.FromJObject(jo);
                if (load && ls.LocationId != null) ls.Location = FindLocation2(ls.LocationId, cache: cache);
                if (load && ls.SettingsId != null) ls.Settings = FindSetting(ls.SettingsId, cache: cache);
                if (load && ls.CreationUserId != null) ls.CreationUser = FindUser2(ls.CreationUserId, cache: cache);
                if (load && ls.LastWriteUserId != null) ls.LastWriteUser = FindUser2(ls.LastWriteUserId, cache: cache);
                yield return ls;
            }
        }

        public LocationSetting FindLocationSetting(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var ls = cache ? (LocationSetting)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = LocationSetting.FromJObject(FolioServiceClient.GetLocationSetting(id?.ToString()))) : LocationSetting.FromJObject(FolioServiceClient.GetLocationSetting(id?.ToString()));
            if (ls == null) return null;
            if (load && ls.LocationId != null) ls.Location = FindLocation2(ls.LocationId, cache: cache);
            if (load && ls.SettingsId != null) ls.Settings = FindSetting(ls.SettingsId, cache: cache);
            if (load && ls.CreationUserId != null) ls.CreationUser = FindUser2(ls.CreationUserId, cache: cache);
            if (load && ls.LastWriteUserId != null) ls.LastWriteUser = FindUser2(ls.LastWriteUserId, cache: cache);
            return ls;
        }

        public void Insert(LocationSetting locationSetting)
        {
            if (locationSetting.Id == null) locationSetting.Id = Guid.NewGuid();
            FolioServiceClient.InsertLocationSetting(locationSetting.ToJObject());
        }

        public void Update(LocationSetting locationSetting) => FolioServiceClient.UpdateLocationSetting(locationSetting.ToJObject());

        public void UpdateOrInsert(LocationSetting locationSetting)
        {
            if (locationSetting.Id == null)
                Insert(locationSetting);
            else
                try
                {
                    Update(locationSetting);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(locationSetting); else throw;
                }
        }

        public void InsertOrUpdate(LocationSetting locationSetting)
        {
            try
            {
                Insert(locationSetting);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(locationSetting); else throw;
            }
        }

        public void DeleteLocationSetting(Guid? id) => FolioServiceClient.DeleteLocationSetting(id?.ToString());

        public void Insert(Login2 login2)
        {
            if (login2.Id == null) login2.Id = Guid.NewGuid();
            FolioServiceClient.InsertLogin(login2.ToJObject());
        }

        public bool AnyLostItemFeePolicy2s(string where = null) => FolioServiceClient.AnyLostItemFeePolicies(where);

        public int CountLostItemFeePolicy2s(string where = null) => FolioServiceClient.CountLostItemFeePolicies(where);

        public LostItemFeePolicy2[] LostItemFeePolicy2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.LostItemFeePolicies(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var lifp2 = cache ? (LostItemFeePolicy2)(objects.ContainsKey(id) ? objects[id] : objects[id] = LostItemFeePolicy2.FromJObject(jo)) : LostItemFeePolicy2.FromJObject(jo);
                if (load && lifp2.CreationUserId != null) lifp2.CreationUser = FindUser2(lifp2.CreationUserId, cache: cache);
                if (load && lifp2.LastWriteUserId != null) lifp2.LastWriteUser = FindUser2(lifp2.LastWriteUserId, cache: cache);
                return lifp2;
            }).ToArray();
        }

        public IEnumerable<LostItemFeePolicy2> LostItemFeePolicy2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.LostItemFeePolicies(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var lifp2 = cache ? (LostItemFeePolicy2)(objects.ContainsKey(id) ? objects[id] : objects[id] = LostItemFeePolicy2.FromJObject(jo)) : LostItemFeePolicy2.FromJObject(jo);
                if (load && lifp2.CreationUserId != null) lifp2.CreationUser = FindUser2(lifp2.CreationUserId, cache: cache);
                if (load && lifp2.LastWriteUserId != null) lifp2.LastWriteUser = FindUser2(lifp2.LastWriteUserId, cache: cache);
                yield return lifp2;
            }
        }

        public LostItemFeePolicy2 FindLostItemFeePolicy2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var lifp2 = cache ? (LostItemFeePolicy2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = LostItemFeePolicy2.FromJObject(FolioServiceClient.GetLostItemFeePolicy(id?.ToString()))) : LostItemFeePolicy2.FromJObject(FolioServiceClient.GetLostItemFeePolicy(id?.ToString()));
            if (lifp2 == null) return null;
            if (load && lifp2.CreationUserId != null) lifp2.CreationUser = FindUser2(lifp2.CreationUserId, cache: cache);
            if (load && lifp2.LastWriteUserId != null) lifp2.LastWriteUser = FindUser2(lifp2.LastWriteUserId, cache: cache);
            return lifp2;
        }

        public void Insert(LostItemFeePolicy2 lostItemFeePolicy2)
        {
            if (lostItemFeePolicy2.Id == null) lostItemFeePolicy2.Id = Guid.NewGuid();
            FolioServiceClient.InsertLostItemFeePolicy(lostItemFeePolicy2.ToJObject());
        }

        public void Update(LostItemFeePolicy2 lostItemFeePolicy2) => FolioServiceClient.UpdateLostItemFeePolicy(lostItemFeePolicy2.ToJObject());

        public void UpdateOrInsert(LostItemFeePolicy2 lostItemFeePolicy2)
        {
            if (lostItemFeePolicy2.Id == null)
                Insert(lostItemFeePolicy2);
            else
                try
                {
                    Update(lostItemFeePolicy2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(lostItemFeePolicy2); else throw;
                }
        }

        public void InsertOrUpdate(LostItemFeePolicy2 lostItemFeePolicy2)
        {
            try
            {
                Insert(lostItemFeePolicy2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(lostItemFeePolicy2); else throw;
            }
        }

        public void DeleteLostItemFeePolicy2(Guid? id) => FolioServiceClient.DeleteLostItemFeePolicy(id?.ToString());

        public bool AnyManualBlockTemplate2s(string where = null) => FolioServiceClient.AnyManualBlockTemplates(where);

        public int CountManualBlockTemplate2s(string where = null) => FolioServiceClient.CountManualBlockTemplates(where);

        public ManualBlockTemplate2[] ManualBlockTemplate2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ManualBlockTemplates(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var mbt2 = cache ? (ManualBlockTemplate2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ManualBlockTemplate2.FromJObject(jo)) : ManualBlockTemplate2.FromJObject(jo);
                if (load && mbt2.CreationUserId != null) mbt2.CreationUser = FindUser2(mbt2.CreationUserId, cache: cache);
                if (load && mbt2.LastWriteUserId != null) mbt2.LastWriteUser = FindUser2(mbt2.LastWriteUserId, cache: cache);
                return mbt2;
            }).ToArray();
        }

        public IEnumerable<ManualBlockTemplate2> ManualBlockTemplate2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ManualBlockTemplates(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var mbt2 = cache ? (ManualBlockTemplate2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ManualBlockTemplate2.FromJObject(jo)) : ManualBlockTemplate2.FromJObject(jo);
                if (load && mbt2.CreationUserId != null) mbt2.CreationUser = FindUser2(mbt2.CreationUserId, cache: cache);
                if (load && mbt2.LastWriteUserId != null) mbt2.LastWriteUser = FindUser2(mbt2.LastWriteUserId, cache: cache);
                yield return mbt2;
            }
        }

        public ManualBlockTemplate2 FindManualBlockTemplate2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var mbt2 = cache ? (ManualBlockTemplate2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = ManualBlockTemplate2.FromJObject(FolioServiceClient.GetManualBlockTemplate(id?.ToString()))) : ManualBlockTemplate2.FromJObject(FolioServiceClient.GetManualBlockTemplate(id?.ToString()));
            if (mbt2 == null) return null;
            if (load && mbt2.CreationUserId != null) mbt2.CreationUser = FindUser2(mbt2.CreationUserId, cache: cache);
            if (load && mbt2.LastWriteUserId != null) mbt2.LastWriteUser = FindUser2(mbt2.LastWriteUserId, cache: cache);
            return mbt2;
        }

        public void Insert(ManualBlockTemplate2 manualBlockTemplate2)
        {
            if (manualBlockTemplate2.Id == null) manualBlockTemplate2.Id = Guid.NewGuid();
            FolioServiceClient.InsertManualBlockTemplate(manualBlockTemplate2.ToJObject());
        }

        public void Update(ManualBlockTemplate2 manualBlockTemplate2) => FolioServiceClient.UpdateManualBlockTemplate(manualBlockTemplate2.ToJObject());

        public void UpdateOrInsert(ManualBlockTemplate2 manualBlockTemplate2)
        {
            if (manualBlockTemplate2.Id == null)
                Insert(manualBlockTemplate2);
            else
                try
                {
                    Update(manualBlockTemplate2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(manualBlockTemplate2); else throw;
                }
        }

        public void InsertOrUpdate(ManualBlockTemplate2 manualBlockTemplate2)
        {
            try
            {
                Insert(manualBlockTemplate2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(manualBlockTemplate2); else throw;
            }
        }

        public void DeleteManualBlockTemplate2(Guid? id) => FolioServiceClient.DeleteManualBlockTemplate(id?.ToString());

        public bool AnyMaterialType2s(string where = null) => FolioServiceClient.AnyMaterialTypes(where);

        public int CountMaterialType2s(string where = null) => FolioServiceClient.CountMaterialTypes(where);

        public MaterialType2[] MaterialType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.MaterialTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var mt2 = cache ? (MaterialType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = MaterialType2.FromJObject(jo)) : MaterialType2.FromJObject(jo);
                if (load && mt2.CreationUserId != null) mt2.CreationUser = FindUser2(mt2.CreationUserId, cache: cache);
                if (load && mt2.LastWriteUserId != null) mt2.LastWriteUser = FindUser2(mt2.LastWriteUserId, cache: cache);
                return mt2;
            }).ToArray();
        }

        public IEnumerable<MaterialType2> MaterialType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.MaterialTypes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var mt2 = cache ? (MaterialType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = MaterialType2.FromJObject(jo)) : MaterialType2.FromJObject(jo);
                if (load && mt2.CreationUserId != null) mt2.CreationUser = FindUser2(mt2.CreationUserId, cache: cache);
                if (load && mt2.LastWriteUserId != null) mt2.LastWriteUser = FindUser2(mt2.LastWriteUserId, cache: cache);
                yield return mt2;
            }
        }

        public MaterialType2 FindMaterialType2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var mt2 = cache ? (MaterialType2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = MaterialType2.FromJObject(FolioServiceClient.GetMaterialType(id?.ToString()))) : MaterialType2.FromJObject(FolioServiceClient.GetMaterialType(id?.ToString()));
            if (mt2 == null) return null;
            if (load && mt2.CreationUserId != null) mt2.CreationUser = FindUser2(mt2.CreationUserId, cache: cache);
            if (load && mt2.LastWriteUserId != null) mt2.LastWriteUser = FindUser2(mt2.LastWriteUserId, cache: cache);
            return mt2;
        }

        public void Insert(MaterialType2 materialType2)
        {
            if (materialType2.Id == null) materialType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertMaterialType(materialType2.ToJObject());
        }

        public void Update(MaterialType2 materialType2) => FolioServiceClient.UpdateMaterialType(materialType2.ToJObject());

        public void UpdateOrInsert(MaterialType2 materialType2)
        {
            if (materialType2.Id == null)
                Insert(materialType2);
            else
                try
                {
                    Update(materialType2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(materialType2); else throw;
                }
        }

        public void InsertOrUpdate(MaterialType2 materialType2)
        {
            try
            {
                Insert(materialType2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(materialType2); else throw;
            }
        }

        public void DeleteMaterialType2(Guid? id) => FolioServiceClient.DeleteMaterialType(id?.ToString());

        public bool AnyNatureOfContentTerm2s(string where = null) => FolioServiceClient.AnyNatureOfContentTerms(where);

        public int CountNatureOfContentTerm2s(string where = null) => FolioServiceClient.CountNatureOfContentTerms(where);

        public NatureOfContentTerm2[] NatureOfContentTerm2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.NatureOfContentTerms(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var noct2 = cache ? (NatureOfContentTerm2)(objects.ContainsKey(id) ? objects[id] : objects[id] = NatureOfContentTerm2.FromJObject(jo)) : NatureOfContentTerm2.FromJObject(jo);
                if (load && noct2.CreationUserId != null) noct2.CreationUser = FindUser2(noct2.CreationUserId, cache: cache);
                if (load && noct2.LastWriteUserId != null) noct2.LastWriteUser = FindUser2(noct2.LastWriteUserId, cache: cache);
                return noct2;
            }).ToArray();
        }

        public IEnumerable<NatureOfContentTerm2> NatureOfContentTerm2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.NatureOfContentTerms(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var noct2 = cache ? (NatureOfContentTerm2)(objects.ContainsKey(id) ? objects[id] : objects[id] = NatureOfContentTerm2.FromJObject(jo)) : NatureOfContentTerm2.FromJObject(jo);
                if (load && noct2.CreationUserId != null) noct2.CreationUser = FindUser2(noct2.CreationUserId, cache: cache);
                if (load && noct2.LastWriteUserId != null) noct2.LastWriteUser = FindUser2(noct2.LastWriteUserId, cache: cache);
                yield return noct2;
            }
        }

        public NatureOfContentTerm2 FindNatureOfContentTerm2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var noct2 = cache ? (NatureOfContentTerm2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = NatureOfContentTerm2.FromJObject(FolioServiceClient.GetNatureOfContentTerm(id?.ToString()))) : NatureOfContentTerm2.FromJObject(FolioServiceClient.GetNatureOfContentTerm(id?.ToString()));
            if (noct2 == null) return null;
            if (load && noct2.CreationUserId != null) noct2.CreationUser = FindUser2(noct2.CreationUserId, cache: cache);
            if (load && noct2.LastWriteUserId != null) noct2.LastWriteUser = FindUser2(noct2.LastWriteUserId, cache: cache);
            return noct2;
        }

        public void Insert(NatureOfContentTerm2 natureOfContentTerm2)
        {
            if (natureOfContentTerm2.Id == null) natureOfContentTerm2.Id = Guid.NewGuid();
            FolioServiceClient.InsertNatureOfContentTerm(natureOfContentTerm2.ToJObject());
        }

        public void Update(NatureOfContentTerm2 natureOfContentTerm2) => FolioServiceClient.UpdateNatureOfContentTerm(natureOfContentTerm2.ToJObject());

        public void UpdateOrInsert(NatureOfContentTerm2 natureOfContentTerm2)
        {
            if (natureOfContentTerm2.Id == null)
                Insert(natureOfContentTerm2);
            else
                try
                {
                    Update(natureOfContentTerm2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(natureOfContentTerm2); else throw;
                }
        }

        public void InsertOrUpdate(NatureOfContentTerm2 natureOfContentTerm2)
        {
            try
            {
                Insert(natureOfContentTerm2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(natureOfContentTerm2); else throw;
            }
        }

        public void DeleteNatureOfContentTerm2(Guid? id) => FolioServiceClient.DeleteNatureOfContentTerm(id?.ToString());

        public bool AnyNote2s(string where = null) => FolioServiceClient.AnyNotes(where);

        public int CountNote2s(string where = null) => FolioServiceClient.CountNotes(where);

        public Note2[] Note2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Notes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var n2 = cache ? (Note2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Note2.FromJObject(jo)) : Note2.FromJObject(jo);
                if (load && n2.TypeId != null) n2.Type = FindNoteType2(n2.TypeId, cache: cache);
                if (load && n2.CreationUserId != null) n2.CreationUser = FindUser2(n2.CreationUserId, cache: cache);
                if (load && n2.LastWriteUserId != null) n2.LastWriteUser = FindUser2(n2.LastWriteUserId, cache: cache);
                return n2;
            }).ToArray();
        }

        public IEnumerable<Note2> Note2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Notes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var n2 = cache ? (Note2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Note2.FromJObject(jo)) : Note2.FromJObject(jo);
                if (load && n2.TypeId != null) n2.Type = FindNoteType2(n2.TypeId, cache: cache);
                if (load && n2.CreationUserId != null) n2.CreationUser = FindUser2(n2.CreationUserId, cache: cache);
                if (load && n2.LastWriteUserId != null) n2.LastWriteUser = FindUser2(n2.LastWriteUserId, cache: cache);
                yield return n2;
            }
        }

        public Note2 FindNote2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var n2 = cache ? (Note2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Note2.FromJObject(FolioServiceClient.GetNote(id?.ToString()))) : Note2.FromJObject(FolioServiceClient.GetNote(id?.ToString()));
            if (n2 == null) return null;
            if (load && n2.TypeId != null) n2.Type = FindNoteType2(n2.TypeId, cache: cache);
            if (load && n2.CreationUserId != null) n2.CreationUser = FindUser2(n2.CreationUserId, cache: cache);
            if (load && n2.LastWriteUserId != null) n2.LastWriteUser = FindUser2(n2.LastWriteUserId, cache: cache);
            return n2;
        }

        public void Insert(Note2 note2)
        {
            if (note2.Id == null) note2.Id = Guid.NewGuid();
            FolioServiceClient.InsertNote(note2.ToJObject());
        }

        public void Update(Note2 note2) => FolioServiceClient.UpdateNote(note2.ToJObject());

        public void UpdateOrInsert(Note2 note2)
        {
            if (note2.Id == null)
                Insert(note2);
            else
                try
                {
                    Update(note2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(note2); else throw;
                }
        }

        public void InsertOrUpdate(Note2 note2)
        {
            try
            {
                Insert(note2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(note2); else throw;
            }
        }

        public void DeleteNote2(Guid? id) => FolioServiceClient.DeleteNote(id?.ToString());

        public bool AnyNoteType2s(string where = null) => FolioServiceClient.AnyNoteTypes(where);

        public int CountNoteType2s(string where = null) => FolioServiceClient.CountNoteTypes(where);

        public NoteType2[] NoteType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.NoteTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var nt2 = cache ? (NoteType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = NoteType2.FromJObject(jo)) : NoteType2.FromJObject(jo);
                if (load && nt2.CreationUserId != null) nt2.CreationUser = FindUser2(nt2.CreationUserId, cache: cache);
                if (load && nt2.LastWriteUserId != null) nt2.LastWriteUser = FindUser2(nt2.LastWriteUserId, cache: cache);
                return nt2;
            }).ToArray();
        }

        public IEnumerable<NoteType2> NoteType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.NoteTypes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var nt2 = cache ? (NoteType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = NoteType2.FromJObject(jo)) : NoteType2.FromJObject(jo);
                if (load && nt2.CreationUserId != null) nt2.CreationUser = FindUser2(nt2.CreationUserId, cache: cache);
                if (load && nt2.LastWriteUserId != null) nt2.LastWriteUser = FindUser2(nt2.LastWriteUserId, cache: cache);
                yield return nt2;
            }
        }

        public NoteType2 FindNoteType2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var nt2 = cache ? (NoteType2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = NoteType2.FromJObject(FolioServiceClient.GetNoteType(id?.ToString()))) : NoteType2.FromJObject(FolioServiceClient.GetNoteType(id?.ToString()));
            if (nt2 == null) return null;
            if (load && nt2.CreationUserId != null) nt2.CreationUser = FindUser2(nt2.CreationUserId, cache: cache);
            if (load && nt2.LastWriteUserId != null) nt2.LastWriteUser = FindUser2(nt2.LastWriteUserId, cache: cache);
            return nt2;
        }

        public void Insert(NoteType2 noteType2)
        {
            if (noteType2.Id == null) noteType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertNoteType(noteType2.ToJObject());
        }

        public void Update(NoteType2 noteType2) => FolioServiceClient.UpdateNoteType(noteType2.ToJObject());

        public void UpdateOrInsert(NoteType2 noteType2)
        {
            if (noteType2.Id == null)
                Insert(noteType2);
            else
                try
                {
                    Update(noteType2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(noteType2); else throw;
                }
        }

        public void InsertOrUpdate(NoteType2 noteType2)
        {
            try
            {
                Insert(noteType2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(noteType2); else throw;
            }
        }

        public void DeleteNoteType2(Guid? id) => FolioServiceClient.DeleteNoteType(id?.ToString());

        public bool AnyOrder2s(string where = null) => FolioServiceClient.AnyOrders(where);

        public int CountOrder2s(string where = null) => FolioServiceClient.CountOrders(where);

        public Order2[] Order2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Orders(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var o2 = cache ? (Order2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Order2.FromJObject(jo)) : Order2.FromJObject(jo);
                if (load && o2.ApprovedById != null) o2.ApprovedBy = FindUser2(o2.ApprovedById, cache: cache);
                if (load && o2.AssignedToId != null) o2.AssignedTo = FindUser2(o2.AssignedToId, cache: cache);
                if (load && o2.OpenedById != null) o2.OpenedBy = FindUser2(o2.OpenedById, cache: cache);
                if (load && o2.VendorId != null) o2.Vendor = FindOrganization2(o2.VendorId, cache: cache);
                if (load && o2.FiscalYearId != null) o2.FiscalYear = FindFiscalYear2(o2.FiscalYearId, cache: cache);
                if (load && o2.CreationUserId != null) o2.CreationUser = FindUser2(o2.CreationUserId, cache: cache);
                if (load && o2.LastWriteUserId != null) o2.LastWriteUser = FindUser2(o2.LastWriteUserId, cache: cache);
                return o2;
            }).ToArray();
        }

        public IEnumerable<Order2> Order2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Orders(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var o2 = cache ? (Order2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Order2.FromJObject(jo)) : Order2.FromJObject(jo);
                if (load && o2.ApprovedById != null) o2.ApprovedBy = FindUser2(o2.ApprovedById, cache: cache);
                if (load && o2.AssignedToId != null) o2.AssignedTo = FindUser2(o2.AssignedToId, cache: cache);
                if (load && o2.OpenedById != null) o2.OpenedBy = FindUser2(o2.OpenedById, cache: cache);
                if (load && o2.VendorId != null) o2.Vendor = FindOrganization2(o2.VendorId, cache: cache);
                if (load && o2.FiscalYearId != null) o2.FiscalYear = FindFiscalYear2(o2.FiscalYearId, cache: cache);
                if (load && o2.CreationUserId != null) o2.CreationUser = FindUser2(o2.CreationUserId, cache: cache);
                if (load && o2.LastWriteUserId != null) o2.LastWriteUser = FindUser2(o2.LastWriteUserId, cache: cache);
                yield return o2;
            }
        }

        public Order2 FindOrder2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var o2 = cache ? (Order2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Order2.FromJObject(FolioServiceClient.GetOrder(id?.ToString()))) : Order2.FromJObject(FolioServiceClient.GetOrder(id?.ToString()));
            if (o2 == null) return null;
            if (load && o2.ApprovedById != null) o2.ApprovedBy = FindUser2(o2.ApprovedById, cache: cache);
            if (load && o2.AssignedToId != null) o2.AssignedTo = FindUser2(o2.AssignedToId, cache: cache);
            if (load && o2.OpenedById != null) o2.OpenedBy = FindUser2(o2.OpenedById, cache: cache);
            if (load && o2.VendorId != null) o2.Vendor = FindOrganization2(o2.VendorId, cache: cache);
            if (load && o2.FiscalYearId != null) o2.FiscalYear = FindFiscalYear2(o2.FiscalYearId, cache: cache);
            if (load && o2.CreationUserId != null) o2.CreationUser = FindUser2(o2.CreationUserId, cache: cache);
            if (load && o2.LastWriteUserId != null) o2.LastWriteUser = FindUser2(o2.LastWriteUserId, cache: cache);
            var i = 0;
            if (o2.OrderAcquisitionsUnits != null) foreach (var oau in o2.OrderAcquisitionsUnits)
                {
                    oau.Id = (++i).ToString();
                    oau.OrderId = o2.Id;
                    oau.Order = o2;
                    if (load && oau.AcquisitionsUnitId != null) oau.AcquisitionsUnit = FindAcquisitionsUnit2(oau.AcquisitionsUnitId, cache: cache);
                }
            i = 0;
            if (o2.OrderNotes != null) foreach (var @on in o2.OrderNotes)
                {
                    @on.Id = (++i).ToString();
                    @on.OrderId = o2.Id;
                    @on.Order = o2;
                }
            i = 0;
            if (o2.OrderTags != null) foreach (var ot in o2.OrderTags)
                {
                    ot.Id = (++i).ToString();
                    ot.OrderId = o2.Id;
                    ot.Order = o2;
                }
            return o2;
        }

        public void Insert(Order2 order2)
        {
            if (order2.Id == null) order2.Id = Guid.NewGuid();
            FolioServiceClient.InsertOrder(order2.ToJObject());
        }

        public void Update(Order2 order2) => FolioServiceClient.UpdateOrder(order2.ToJObject());

        public void UpdateOrInsert(Order2 order2)
        {
            if (order2.Id == null)
                Insert(order2);
            else
                try
                {
                    Update(order2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(order2); else throw;
                }
        }

        public void InsertOrUpdate(Order2 order2)
        {
            try
            {
                Insert(order2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(order2); else throw;
            }
        }

        public void DeleteOrder2(Guid? id) => FolioServiceClient.DeleteOrder(id?.ToString());

        public bool AnyOrderInvoice2s(string where = null) => FolioServiceClient.AnyOrderInvoices(where);

        public int CountOrderInvoice2s(string where = null) => FolioServiceClient.CountOrderInvoices(where);

        public OrderInvoice2[] OrderInvoice2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.OrderInvoices(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var oi2 = cache ? (OrderInvoice2)(objects.ContainsKey(id) ? objects[id] : objects[id] = OrderInvoice2.FromJObject(jo)) : OrderInvoice2.FromJObject(jo);
                if (load && oi2.OrderId != null) oi2.Order = FindOrder2(oi2.OrderId, cache: cache);
                if (load && oi2.InvoiceId != null) oi2.Invoice = FindInvoice2(oi2.InvoiceId, cache: cache);
                return oi2;
            }).ToArray();
        }

        public IEnumerable<OrderInvoice2> OrderInvoice2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.OrderInvoices(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var oi2 = cache ? (OrderInvoice2)(objects.ContainsKey(id) ? objects[id] : objects[id] = OrderInvoice2.FromJObject(jo)) : OrderInvoice2.FromJObject(jo);
                if (load && oi2.OrderId != null) oi2.Order = FindOrder2(oi2.OrderId, cache: cache);
                if (load && oi2.InvoiceId != null) oi2.Invoice = FindInvoice2(oi2.InvoiceId, cache: cache);
                yield return oi2;
            }
        }

        public OrderInvoice2 FindOrderInvoice2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var oi2 = cache ? (OrderInvoice2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = OrderInvoice2.FromJObject(FolioServiceClient.GetOrderInvoice(id?.ToString()))) : OrderInvoice2.FromJObject(FolioServiceClient.GetOrderInvoice(id?.ToString()));
            if (oi2 == null) return null;
            if (load && oi2.OrderId != null) oi2.Order = FindOrder2(oi2.OrderId, cache: cache);
            if (load && oi2.InvoiceId != null) oi2.Invoice = FindInvoice2(oi2.InvoiceId, cache: cache);
            return oi2;
        }

        public void Insert(OrderInvoice2 orderInvoice2)
        {
            if (orderInvoice2.Id == null) orderInvoice2.Id = Guid.NewGuid();
            FolioServiceClient.InsertOrderInvoice(orderInvoice2.ToJObject());
        }

        public void Update(OrderInvoice2 orderInvoice2) => FolioServiceClient.UpdateOrderInvoice(orderInvoice2.ToJObject());

        public void UpdateOrInsert(OrderInvoice2 orderInvoice2)
        {
            if (orderInvoice2.Id == null)
                Insert(orderInvoice2);
            else
                try
                {
                    Update(orderInvoice2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(orderInvoice2); else throw;
                }
        }

        public void InsertOrUpdate(OrderInvoice2 orderInvoice2)
        {
            try
            {
                Insert(orderInvoice2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(orderInvoice2); else throw;
            }
        }

        public void DeleteOrderInvoice2(Guid? id) => FolioServiceClient.DeleteOrderInvoice(id?.ToString());

        public bool AnyOrderItem2s(string where = null) => FolioServiceClient.AnyOrderItems(where);

        public int CountOrderItem2s(string where = null) => FolioServiceClient.CountOrderItems(where);

        public OrderItem2[] OrderItem2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.OrderItems(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var oi2 = cache ? (OrderItem2)(objects.ContainsKey(id) ? objects[id] : objects[id] = OrderItem2.FromJObject(jo)) : OrderItem2.FromJObject(jo);
                if (load && oi2.AgreementId != null) oi2.Agreement = FindAgreement2(oi2.AgreementId, cache: cache);
                if (load && oi2.AcquisitionMethodId != null) oi2.AcquisitionMethod = FindAcquisitionMethod2(oi2.AcquisitionMethodId, cache: cache);
                if (load && oi2.EresourceAccessProviderId != null) oi2.EresourceAccessProvider = FindOrganization2(oi2.EresourceAccessProviderId, cache: cache);
                if (load && oi2.EresourceMaterialTypeId != null) oi2.EresourceMaterialType = FindMaterialType2(oi2.EresourceMaterialTypeId, cache: cache);
                if (load && oi2.InstanceId != null) oi2.Instance = FindInstance2(oi2.InstanceId, cache: cache);
                if (load && oi2.PackageOrderItemId != null) oi2.PackageOrderItem = FindOrderItem2(oi2.PackageOrderItemId, cache: cache);
                if (load && oi2.PhysicalMaterialTypeId != null) oi2.PhysicalMaterialType = FindMaterialType2(oi2.PhysicalMaterialTypeId, cache: cache);
                if (load && oi2.PhysicalMaterialSupplierId != null) oi2.PhysicalMaterialSupplier = FindOrganization2(oi2.PhysicalMaterialSupplierId, cache: cache);
                if (load && oi2.OrderId != null) oi2.Order = FindOrder2(oi2.OrderId, cache: cache);
                if (load && oi2.CreationUserId != null) oi2.CreationUser = FindUser2(oi2.CreationUserId, cache: cache);
                if (load && oi2.LastWriteUserId != null) oi2.LastWriteUser = FindUser2(oi2.LastWriteUserId, cache: cache);
                return oi2;
            }).ToArray();
        }

        public IEnumerable<OrderItem2> OrderItem2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.OrderItems(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var oi2 = cache ? (OrderItem2)(objects.ContainsKey(id) ? objects[id] : objects[id] = OrderItem2.FromJObject(jo)) : OrderItem2.FromJObject(jo);
                if (load && oi2.AgreementId != null) oi2.Agreement = FindAgreement2(oi2.AgreementId, cache: cache);
                if (load && oi2.AcquisitionMethodId != null) oi2.AcquisitionMethod = FindAcquisitionMethod2(oi2.AcquisitionMethodId, cache: cache);
                if (load && oi2.EresourceAccessProviderId != null) oi2.EresourceAccessProvider = FindOrganization2(oi2.EresourceAccessProviderId, cache: cache);
                if (load && oi2.EresourceMaterialTypeId != null) oi2.EresourceMaterialType = FindMaterialType2(oi2.EresourceMaterialTypeId, cache: cache);
                if (load && oi2.InstanceId != null) oi2.Instance = FindInstance2(oi2.InstanceId, cache: cache);
                if (load && oi2.PackageOrderItemId != null) oi2.PackageOrderItem = FindOrderItem2(oi2.PackageOrderItemId, cache: cache);
                if (load && oi2.PhysicalMaterialTypeId != null) oi2.PhysicalMaterialType = FindMaterialType2(oi2.PhysicalMaterialTypeId, cache: cache);
                if (load && oi2.PhysicalMaterialSupplierId != null) oi2.PhysicalMaterialSupplier = FindOrganization2(oi2.PhysicalMaterialSupplierId, cache: cache);
                if (load && oi2.OrderId != null) oi2.Order = FindOrder2(oi2.OrderId, cache: cache);
                if (load && oi2.CreationUserId != null) oi2.CreationUser = FindUser2(oi2.CreationUserId, cache: cache);
                if (load && oi2.LastWriteUserId != null) oi2.LastWriteUser = FindUser2(oi2.LastWriteUserId, cache: cache);
                yield return oi2;
            }
        }

        public OrderItem2 FindOrderItem2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var oi2 = cache ? (OrderItem2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = OrderItem2.FromJObject(FolioServiceClient.GetOrderItem(id?.ToString()))) : OrderItem2.FromJObject(FolioServiceClient.GetOrderItem(id?.ToString()));
            if (oi2 == null) return null;
            if (load && oi2.AgreementId != null) oi2.Agreement = FindAgreement2(oi2.AgreementId, cache: cache);
            if (load && oi2.AcquisitionMethodId != null) oi2.AcquisitionMethod = FindAcquisitionMethod2(oi2.AcquisitionMethodId, cache: cache);
            if (load && oi2.EresourceAccessProviderId != null) oi2.EresourceAccessProvider = FindOrganization2(oi2.EresourceAccessProviderId, cache: cache);
            if (load && oi2.EresourceMaterialTypeId != null) oi2.EresourceMaterialType = FindMaterialType2(oi2.EresourceMaterialTypeId, cache: cache);
            if (load && oi2.InstanceId != null) oi2.Instance = FindInstance2(oi2.InstanceId, cache: cache);
            if (load && oi2.PackageOrderItemId != null) oi2.PackageOrderItem = FindOrderItem2(oi2.PackageOrderItemId, cache: cache);
            if (load && oi2.PhysicalMaterialTypeId != null) oi2.PhysicalMaterialType = FindMaterialType2(oi2.PhysicalMaterialTypeId, cache: cache);
            if (load && oi2.PhysicalMaterialSupplierId != null) oi2.PhysicalMaterialSupplier = FindOrganization2(oi2.PhysicalMaterialSupplierId, cache: cache);
            if (load && oi2.OrderId != null) oi2.Order = FindOrder2(oi2.OrderId, cache: cache);
            if (load && oi2.CreationUserId != null) oi2.CreationUser = FindUser2(oi2.CreationUserId, cache: cache);
            if (load && oi2.LastWriteUserId != null) oi2.LastWriteUser = FindUser2(oi2.LastWriteUserId, cache: cache);
            var i = 0;
            if (oi2.OrderItemClaims != null) foreach (var oic in oi2.OrderItemClaims)
                {
                    oic.Id = (++i).ToString();
                    oic.OrderItemId = oi2.Id;
                    oic.OrderItem = oi2;
                }
            i = 0;
            if (oi2.OrderItemContributors != null) foreach (var oic2 in oi2.OrderItemContributors)
                {
                    oic2.Id = (++i).ToString();
                    oic2.OrderItemId = oi2.Id;
                    oic2.OrderItem = oi2;
                    if (load && oic2.ContributorNameTypeId != null) oic2.ContributorNameType = FindContributorNameType2(oic2.ContributorNameTypeId, cache: cache);
                }
            i = 0;
            if (oi2.OrderItemFunds != null) foreach (var oif in oi2.OrderItemFunds)
                {
                    oif.Id = (++i).ToString();
                    oif.OrderItemId = oi2.Id;
                    oif.OrderItem = oi2;
                    if (load && oif.EncumbranceId != null) oif.Encumbrance = FindTransaction2(oif.EncumbranceId, cache: cache);
                    if (load && oif.FundId != null) oif.Fund = FindFund2(oif.FundId, cache: cache);
                    if (load && oif.ExpenseClassId != null) oif.ExpenseClass = FindExpenseClass2(oif.ExpenseClassId, cache: cache);
                }
            i = 0;
            if (oi2.OrderItemLocation2s != null) foreach (var oil2 in oi2.OrderItemLocation2s)
                {
                    oil2.Id = (++i).ToString();
                    oil2.OrderItemId = oi2.Id;
                    oil2.OrderItem = oi2;
                    if (load && oil2.LocationId != null) oil2.Location = FindLocation2(oil2.LocationId, cache: cache);
                    if (load && oil2.HoldingId != null) oil2.Holding = FindHolding2(oil2.HoldingId, cache: cache);
                }
            i = 0;
            if (oi2.OrderItemOrganizations != null) foreach (var oio in oi2.OrderItemOrganizations)
                {
                    oio.Id = (++i).ToString();
                    oio.OrderItemId = oi2.Id;
                    oio.OrderItem = oi2;
                    if (load && oio.OrganizationId != null) oio.Organization = FindOrganization2(oio.OrganizationId, cache: cache);
                }
            i = 0;
            if (oi2.OrderItemProductIds != null) foreach (var oipi in oi2.OrderItemProductIds)
                {
                    oipi.Id = (++i).ToString();
                    oipi.OrderItemId = oi2.Id;
                    oipi.OrderItem = oi2;
                    if (load && oipi.ProductIdTypeId != null) oipi.ProductIdType = FindIdType2(oipi.ProductIdTypeId, cache: cache);
                }
            i = 0;
            if (oi2.OrderItemReferenceNumbers != null) foreach (var oirn in oi2.OrderItemReferenceNumbers)
                {
                    oirn.Id = (++i).ToString();
                    oirn.OrderItemId = oi2.Id;
                    oirn.OrderItem = oi2;
                }
            i = 0;
            if (oi2.OrderItemSearchLocations != null) foreach (var oisl in oi2.OrderItemSearchLocations)
                {
                    oisl.Id = (++i).ToString();
                    oisl.OrderItemId = oi2.Id;
                    oisl.OrderItem = oi2;
                    if (load && oisl.LocationId != null) oisl.Location = FindLocation2(oisl.LocationId, cache: cache);
                }
            i = 0;
            if (oi2.OrderItemTags != null) foreach (var oit in oi2.OrderItemTags)
                {
                    oit.Id = (++i).ToString();
                    oit.OrderItemId = oi2.Id;
                    oit.OrderItem = oi2;
                }
            i = 0;
            if (oi2.OrderItemVolumes != null) foreach (var oiv in oi2.OrderItemVolumes)
                {
                    oiv.Id = (++i).ToString();
                    oiv.OrderItemId = oi2.Id;
                    oiv.OrderItem = oi2;
                }
            return oi2;
        }

        public void Insert(OrderItem2 orderItem2)
        {
            if (orderItem2.Id == null) orderItem2.Id = Guid.NewGuid();
            FolioServiceClient.InsertOrderItem(orderItem2.ToJObject());
        }

        public void Update(OrderItem2 orderItem2) => FolioServiceClient.UpdateOrderItem(orderItem2.ToJObject());

        public void UpdateOrInsert(OrderItem2 orderItem2)
        {
            if (orderItem2.Id == null)
                Insert(orderItem2);
            else
                try
                {
                    Update(orderItem2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(orderItem2); else throw;
                }
        }

        public void InsertOrUpdate(OrderItem2 orderItem2)
        {
            try
            {
                Insert(orderItem2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(orderItem2); else throw;
            }
        }

        public void DeleteOrderItem2(Guid? id) => FolioServiceClient.DeleteOrderItem(id?.ToString());

        public bool AnyOrganization2s(string where = null) => FolioServiceClient.AnyOrganizations(where);

        public int CountOrganization2s(string where = null) => FolioServiceClient.CountOrganizations(where);

        public Organization2[] Organization2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Organizations(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var o2 = cache ? (Organization2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Organization2.FromJObject(jo)) : Organization2.FromJObject(jo);
                if (load && o2.CreationUserId != null) o2.CreationUser = FindUser2(o2.CreationUserId, cache: cache);
                if (load && o2.LastWriteUserId != null) o2.LastWriteUser = FindUser2(o2.LastWriteUserId, cache: cache);
                return o2;
            }).ToArray();
        }

        public IEnumerable<Organization2> Organization2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Organizations(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var o2 = cache ? (Organization2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Organization2.FromJObject(jo)) : Organization2.FromJObject(jo);
                if (load && o2.CreationUserId != null) o2.CreationUser = FindUser2(o2.CreationUserId, cache: cache);
                if (load && o2.LastWriteUserId != null) o2.LastWriteUser = FindUser2(o2.LastWriteUserId, cache: cache);
                yield return o2;
            }
        }

        public Organization2 FindOrganization2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var o2 = cache ? (Organization2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Organization2.FromJObject(FolioServiceClient.GetOrganization(id?.ToString()))) : Organization2.FromJObject(FolioServiceClient.GetOrganization(id?.ToString()));
            if (o2 == null) return null;
            if (load && o2.CreationUserId != null) o2.CreationUser = FindUser2(o2.CreationUserId, cache: cache);
            if (load && o2.LastWriteUserId != null) o2.LastWriteUser = FindUser2(o2.LastWriteUserId, cache: cache);
            var i = 0;
            if (o2.Currencies != null) foreach (var c in o2.Currencies)
                {
                    c.Id = (++i).ToString();
                    c.OrganizationId = o2.Id;
                    c.Organization = o2;
                }
            i = 0;
            if (o2.OrganizationAccounts != null) foreach (var oa in o2.OrganizationAccounts)
                {
                    oa.Id = (++i).ToString();
                    oa.OrganizationId = o2.Id;
                    oa.Organization = o2;
                }
            i = 0;
            if (o2.OrganizationAcquisitionsUnits != null) foreach (var oau in o2.OrganizationAcquisitionsUnits)
                {
                    oau.Id = (++i).ToString();
                    oau.OrganizationId = o2.Id;
                    oau.Organization = o2;
                    if (load && oau.AcquisitionsUnitId != null) oau.AcquisitionsUnit = FindAcquisitionsUnit2(oau.AcquisitionsUnitId, cache: cache);
                }
            i = 0;
            if (o2.OrganizationAddresses != null) foreach (var oa2 in o2.OrganizationAddresses)
                {
                    oa2.Id = (++i).ToString();
                    oa2.OrganizationId = o2.Id;
                    oa2.Organization = o2;
                    if (load && oa2.CreationUserId != null) oa2.CreationUser = FindUser2(oa2.CreationUserId, cache: cache);
                    if (load && oa2.LastWriteUserId != null) oa2.LastWriteUser = FindUser2(oa2.LastWriteUserId, cache: cache);
                }
            i = 0;
            if (o2.OrganizationAgreements != null) foreach (var oa3 in o2.OrganizationAgreements)
                {
                    oa3.Id = (++i).ToString();
                    oa3.OrganizationId = o2.Id;
                    oa3.Organization = o2;
                }
            i = 0;
            if (o2.OrganizationAliases != null) foreach (var oa4 in o2.OrganizationAliases)
                {
                    oa4.Id = (++i).ToString();
                    oa4.OrganizationId = o2.Id;
                    oa4.Organization = o2;
                }
            i = 0;
            if (o2.OrganizationChangelogs != null) foreach (var oc in o2.OrganizationChangelogs)
                {
                    oc.Id = (++i).ToString();
                    oc.OrganizationId = o2.Id;
                    oc.Organization = o2;
                }
            i = 0;
            if (o2.OrganizationContacts != null) foreach (var oc2 in o2.OrganizationContacts)
                {
                    oc2.Id = (++i).ToString();
                    oc2.OrganizationId = o2.Id;
                    oc2.Organization = o2;
                    if (load && oc2.ContactId != null) oc2.Contact = FindContact2(oc2.ContactId, cache: cache);
                }
            i = 0;
            if (o2.OrganizationEmails != null) foreach (var oe in o2.OrganizationEmails)
                {
                    oe.Id = (++i).ToString();
                    oe.OrganizationId = o2.Id;
                    oe.Organization = o2;
                    if (load && oe.CreationUserId != null) oe.CreationUser = FindUser2(oe.CreationUserId, cache: cache);
                    if (load && oe.LastWriteUserId != null) oe.LastWriteUser = FindUser2(oe.LastWriteUserId, cache: cache);
                }
            i = 0;
            if (o2.OrganizationInterfaces != null) foreach (var oi in o2.OrganizationInterfaces)
                {
                    oi.Id = (++i).ToString();
                    oi.OrganizationId = o2.Id;
                    oi.Organization = o2;
                    if (load && oi.InterfaceId != null) oi.Interface = FindInterface2(oi.InterfaceId, cache: cache);
                }
            i = 0;
            if (o2.OrganizationPhoneNumbers != null) foreach (var opn in o2.OrganizationPhoneNumbers)
                {
                    opn.Id = (++i).ToString();
                    opn.OrganizationId = o2.Id;
                    opn.Organization = o2;
                    if (load && opn.CreationUserId != null) opn.CreationUser = FindUser2(opn.CreationUserId, cache: cache);
                    if (load && opn.LastWriteUserId != null) opn.LastWriteUser = FindUser2(opn.LastWriteUserId, cache: cache);
                }
            i = 0;
            if (o2.OrganizationPrivilegedContacts != null) foreach (var opc in o2.OrganizationPrivilegedContacts)
                {
                    opc.Id = (++i).ToString();
                    opc.OrganizationId = o2.Id;
                    opc.Organization = o2;
                }
            i = 0;
            if (o2.OrganizationTags != null) foreach (var ot in o2.OrganizationTags)
                {
                    ot.Id = (++i).ToString();
                    ot.OrganizationId = o2.Id;
                    ot.Organization = o2;
                }
            i = 0;
            if (o2.OrganizationTypes != null) foreach (var ot2 in o2.OrganizationTypes)
                {
                    ot2.Id = (++i).ToString();
                    ot2.OrganizationId = o2.Id;
                    ot2.Organization = o2;
                }
            i = 0;
            if (o2.OrganizationUrls != null) foreach (var ou in o2.OrganizationUrls)
                {
                    ou.Id = (++i).ToString();
                    ou.OrganizationId = o2.Id;
                    ou.Organization = o2;
                    if (load && ou.CreationUserId != null) ou.CreationUser = FindUser2(ou.CreationUserId, cache: cache);
                    if (load && ou.LastWriteUserId != null) ou.LastWriteUser = FindUser2(ou.LastWriteUserId, cache: cache);
                }
            return o2;
        }

        public void Insert(Organization2 organization2)
        {
            if (organization2.Id == null) organization2.Id = Guid.NewGuid();
            FolioServiceClient.InsertOrganization(organization2.ToJObject());
        }

        public void Update(Organization2 organization2) => FolioServiceClient.UpdateOrganization(organization2.ToJObject());

        public void UpdateOrInsert(Organization2 organization2)
        {
            if (organization2.Id == null)
                Insert(organization2);
            else
                try
                {
                    Update(organization2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(organization2); else throw;
                }
        }

        public void InsertOrUpdate(Organization2 organization2)
        {
            try
            {
                Insert(organization2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(organization2); else throw;
            }
        }

        public void DeleteOrganization2(Guid? id) => FolioServiceClient.DeleteOrganization(id?.ToString());

        public bool AnyOverdueFinePolicy2s(string where = null) => FolioServiceClient.AnyOverdueFinePolicies(where);

        public int CountOverdueFinePolicy2s(string where = null) => FolioServiceClient.CountOverdueFinePolicies(where);

        public OverdueFinePolicy2[] OverdueFinePolicy2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.OverdueFinePolicies(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var ofp2 = cache ? (OverdueFinePolicy2)(objects.ContainsKey(id) ? objects[id] : objects[id] = OverdueFinePolicy2.FromJObject(jo)) : OverdueFinePolicy2.FromJObject(jo);
                if (load && ofp2.CreationUserId != null) ofp2.CreationUser = FindUser2(ofp2.CreationUserId, cache: cache);
                if (load && ofp2.LastWriteUserId != null) ofp2.LastWriteUser = FindUser2(ofp2.LastWriteUserId, cache: cache);
                return ofp2;
            }).ToArray();
        }

        public IEnumerable<OverdueFinePolicy2> OverdueFinePolicy2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.OverdueFinePolicies(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var ofp2 = cache ? (OverdueFinePolicy2)(objects.ContainsKey(id) ? objects[id] : objects[id] = OverdueFinePolicy2.FromJObject(jo)) : OverdueFinePolicy2.FromJObject(jo);
                if (load && ofp2.CreationUserId != null) ofp2.CreationUser = FindUser2(ofp2.CreationUserId, cache: cache);
                if (load && ofp2.LastWriteUserId != null) ofp2.LastWriteUser = FindUser2(ofp2.LastWriteUserId, cache: cache);
                yield return ofp2;
            }
        }

        public OverdueFinePolicy2 FindOverdueFinePolicy2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var ofp2 = cache ? (OverdueFinePolicy2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = OverdueFinePolicy2.FromJObject(FolioServiceClient.GetOverdueFinePolicy(id?.ToString()))) : OverdueFinePolicy2.FromJObject(FolioServiceClient.GetOverdueFinePolicy(id?.ToString()));
            if (ofp2 == null) return null;
            if (load && ofp2.CreationUserId != null) ofp2.CreationUser = FindUser2(ofp2.CreationUserId, cache: cache);
            if (load && ofp2.LastWriteUserId != null) ofp2.LastWriteUser = FindUser2(ofp2.LastWriteUserId, cache: cache);
            return ofp2;
        }

        public void Insert(OverdueFinePolicy2 overdueFinePolicy2)
        {
            if (overdueFinePolicy2.Id == null) overdueFinePolicy2.Id = Guid.NewGuid();
            FolioServiceClient.InsertOverdueFinePolicy(overdueFinePolicy2.ToJObject());
        }

        public void Update(OverdueFinePolicy2 overdueFinePolicy2) => FolioServiceClient.UpdateOverdueFinePolicy(overdueFinePolicy2.ToJObject());

        public void UpdateOrInsert(OverdueFinePolicy2 overdueFinePolicy2)
        {
            if (overdueFinePolicy2.Id == null)
                Insert(overdueFinePolicy2);
            else
                try
                {
                    Update(overdueFinePolicy2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(overdueFinePolicy2); else throw;
                }
        }

        public void InsertOrUpdate(OverdueFinePolicy2 overdueFinePolicy2)
        {
            try
            {
                Insert(overdueFinePolicy2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(overdueFinePolicy2); else throw;
            }
        }

        public void DeleteOverdueFinePolicy2(Guid? id) => FolioServiceClient.DeleteOverdueFinePolicy(id?.ToString());

        public bool AnyOwner2s(string where = null) => FolioServiceClient.AnyOwners(where);

        public int CountOwner2s(string where = null) => FolioServiceClient.CountOwners(where);

        public Owner2[] Owner2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Owners(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var o2 = cache ? (Owner2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Owner2.FromJObject(jo)) : Owner2.FromJObject(jo);
                if (load && o2.DefaultChargeNoticeId != null) o2.DefaultChargeNotice = FindTemplate2(o2.DefaultChargeNoticeId, cache: cache);
                if (load && o2.DefaultActionNoticeId != null) o2.DefaultActionNotice = FindTemplate2(o2.DefaultActionNoticeId, cache: cache);
                if (load && o2.CreationUserId != null) o2.CreationUser = FindUser2(o2.CreationUserId, cache: cache);
                if (load && o2.LastWriteUserId != null) o2.LastWriteUser = FindUser2(o2.LastWriteUserId, cache: cache);
                return o2;
            }).ToArray();
        }

        public IEnumerable<Owner2> Owner2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Owners(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var o2 = cache ? (Owner2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Owner2.FromJObject(jo)) : Owner2.FromJObject(jo);
                if (load && o2.DefaultChargeNoticeId != null) o2.DefaultChargeNotice = FindTemplate2(o2.DefaultChargeNoticeId, cache: cache);
                if (load && o2.DefaultActionNoticeId != null) o2.DefaultActionNotice = FindTemplate2(o2.DefaultActionNoticeId, cache: cache);
                if (load && o2.CreationUserId != null) o2.CreationUser = FindUser2(o2.CreationUserId, cache: cache);
                if (load && o2.LastWriteUserId != null) o2.LastWriteUser = FindUser2(o2.LastWriteUserId, cache: cache);
                yield return o2;
            }
        }

        public Owner2 FindOwner2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var o2 = cache ? (Owner2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Owner2.FromJObject(FolioServiceClient.GetOwner(id?.ToString()))) : Owner2.FromJObject(FolioServiceClient.GetOwner(id?.ToString()));
            if (o2 == null) return null;
            if (load && o2.DefaultChargeNoticeId != null) o2.DefaultChargeNotice = FindTemplate2(o2.DefaultChargeNoticeId, cache: cache);
            if (load && o2.DefaultActionNoticeId != null) o2.DefaultActionNotice = FindTemplate2(o2.DefaultActionNoticeId, cache: cache);
            if (load && o2.CreationUserId != null) o2.CreationUser = FindUser2(o2.CreationUserId, cache: cache);
            if (load && o2.LastWriteUserId != null) o2.LastWriteUser = FindUser2(o2.LastWriteUserId, cache: cache);
            var i = 0;
            if (o2.ServicePointOwners != null) foreach (var spo in o2.ServicePointOwners)
                {
                    spo.Id = (++i).ToString();
                    spo.OwnerId = o2.Id;
                    spo.Owner = o2;
                    if (load && spo.ServicePointId != null) spo.ServicePoint = FindServicePoint2(spo.ServicePointId, cache: cache);
                }
            return o2;
        }

        public void Insert(Owner2 owner2)
        {
            if (owner2.Id == null) owner2.Id = Guid.NewGuid();
            FolioServiceClient.InsertOwner(owner2.ToJObject());
        }

        public void Update(Owner2 owner2) => FolioServiceClient.UpdateOwner(owner2.ToJObject());

        public void UpdateOrInsert(Owner2 owner2)
        {
            if (owner2.Id == null)
                Insert(owner2);
            else
                try
                {
                    Update(owner2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(owner2); else throw;
                }
        }

        public void InsertOrUpdate(Owner2 owner2)
        {
            try
            {
                Insert(owner2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(owner2); else throw;
            }
        }

        public void DeleteOwner2(Guid? id) => FolioServiceClient.DeleteOwner(id?.ToString());

        public bool AnyPatronActionSession2s(string where = null) => FolioServiceClient.AnyPatronActionSessions(where);

        public int CountPatronActionSession2s(string where = null) => FolioServiceClient.CountPatronActionSessions(where);

        public PatronActionSession2[] PatronActionSession2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.PatronActionSessions(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var pas2 = cache ? (PatronActionSession2)(objects.ContainsKey(id) ? objects[id] : objects[id] = PatronActionSession2.FromJObject(jo)) : PatronActionSession2.FromJObject(jo);
                if (load && pas2.PatronId != null) pas2.Patron = FindUser2(pas2.PatronId, cache: cache);
                if (load && pas2.LoanId != null) pas2.Loan = FindLoan2(pas2.LoanId, cache: cache);
                if (load && pas2.CreationUserId != null) pas2.CreationUser = FindUser2(pas2.CreationUserId, cache: cache);
                if (load && pas2.LastWriteUserId != null) pas2.LastWriteUser = FindUser2(pas2.LastWriteUserId, cache: cache);
                return pas2;
            }).ToArray();
        }

        public IEnumerable<PatronActionSession2> PatronActionSession2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.PatronActionSessions(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var pas2 = cache ? (PatronActionSession2)(objects.ContainsKey(id) ? objects[id] : objects[id] = PatronActionSession2.FromJObject(jo)) : PatronActionSession2.FromJObject(jo);
                if (load && pas2.PatronId != null) pas2.Patron = FindUser2(pas2.PatronId, cache: cache);
                if (load && pas2.LoanId != null) pas2.Loan = FindLoan2(pas2.LoanId, cache: cache);
                if (load && pas2.CreationUserId != null) pas2.CreationUser = FindUser2(pas2.CreationUserId, cache: cache);
                if (load && pas2.LastWriteUserId != null) pas2.LastWriteUser = FindUser2(pas2.LastWriteUserId, cache: cache);
                yield return pas2;
            }
        }

        public PatronActionSession2 FindPatronActionSession2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var pas2 = cache ? (PatronActionSession2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = PatronActionSession2.FromJObject(FolioServiceClient.GetPatronActionSession(id?.ToString()))) : PatronActionSession2.FromJObject(FolioServiceClient.GetPatronActionSession(id?.ToString()));
            if (pas2 == null) return null;
            if (load && pas2.PatronId != null) pas2.Patron = FindUser2(pas2.PatronId, cache: cache);
            if (load && pas2.LoanId != null) pas2.Loan = FindLoan2(pas2.LoanId, cache: cache);
            if (load && pas2.CreationUserId != null) pas2.CreationUser = FindUser2(pas2.CreationUserId, cache: cache);
            if (load && pas2.LastWriteUserId != null) pas2.LastWriteUser = FindUser2(pas2.LastWriteUserId, cache: cache);
            return pas2;
        }

        public void Insert(PatronActionSession2 patronActionSession2)
        {
            if (patronActionSession2.Id == null) patronActionSession2.Id = Guid.NewGuid();
            FolioServiceClient.InsertPatronActionSession(patronActionSession2.ToJObject());
        }

        public void Update(PatronActionSession2 patronActionSession2) => FolioServiceClient.UpdatePatronActionSession(patronActionSession2.ToJObject());

        public void UpdateOrInsert(PatronActionSession2 patronActionSession2)
        {
            if (patronActionSession2.Id == null)
                Insert(patronActionSession2);
            else
                try
                {
                    Update(patronActionSession2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(patronActionSession2); else throw;
                }
        }

        public void InsertOrUpdate(PatronActionSession2 patronActionSession2)
        {
            try
            {
                Insert(patronActionSession2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(patronActionSession2); else throw;
            }
        }

        public void DeletePatronActionSession2(Guid? id) => FolioServiceClient.DeletePatronActionSession(id?.ToString());

        public bool AnyPatronNoticePolicy2s(string where = null) => FolioServiceClient.AnyPatronNoticePolicies(where);

        public int CountPatronNoticePolicy2s(string where = null) => FolioServiceClient.CountPatronNoticePolicies(where);

        public PatronNoticePolicy2[] PatronNoticePolicy2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.PatronNoticePolicies(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var pnp2 = cache ? (PatronNoticePolicy2)(objects.ContainsKey(id) ? objects[id] : objects[id] = PatronNoticePolicy2.FromJObject(jo)) : PatronNoticePolicy2.FromJObject(jo);
                if (load && pnp2.CreationUserId != null) pnp2.CreationUser = FindUser2(pnp2.CreationUserId, cache: cache);
                if (load && pnp2.LastWriteUserId != null) pnp2.LastWriteUser = FindUser2(pnp2.LastWriteUserId, cache: cache);
                return pnp2;
            }).ToArray();
        }

        public IEnumerable<PatronNoticePolicy2> PatronNoticePolicy2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.PatronNoticePolicies(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var pnp2 = cache ? (PatronNoticePolicy2)(objects.ContainsKey(id) ? objects[id] : objects[id] = PatronNoticePolicy2.FromJObject(jo)) : PatronNoticePolicy2.FromJObject(jo);
                if (load && pnp2.CreationUserId != null) pnp2.CreationUser = FindUser2(pnp2.CreationUserId, cache: cache);
                if (load && pnp2.LastWriteUserId != null) pnp2.LastWriteUser = FindUser2(pnp2.LastWriteUserId, cache: cache);
                yield return pnp2;
            }
        }

        public PatronNoticePolicy2 FindPatronNoticePolicy2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var pnp2 = cache ? (PatronNoticePolicy2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = PatronNoticePolicy2.FromJObject(FolioServiceClient.GetPatronNoticePolicy(id?.ToString()))) : PatronNoticePolicy2.FromJObject(FolioServiceClient.GetPatronNoticePolicy(id?.ToString()));
            if (pnp2 == null) return null;
            if (load && pnp2.CreationUserId != null) pnp2.CreationUser = FindUser2(pnp2.CreationUserId, cache: cache);
            if (load && pnp2.LastWriteUserId != null) pnp2.LastWriteUser = FindUser2(pnp2.LastWriteUserId, cache: cache);
            var i = 0;
            if (pnp2.PatronNoticePolicyFeeFineNotices != null) foreach (var pnpffn in pnp2.PatronNoticePolicyFeeFineNotices)
                {
                    pnpffn.Id = (++i).ToString();
                    pnpffn.PatronNoticePolicyId = pnp2.Id;
                    pnpffn.PatronNoticePolicy = pnp2;
                    if (load && pnpffn.TemplateId != null) pnpffn.Template = FindTemplate2(pnpffn.TemplateId, cache: cache);
                }
            i = 0;
            if (pnp2.PatronNoticePolicyLoanNotices != null) foreach (var pnpln in pnp2.PatronNoticePolicyLoanNotices)
                {
                    pnpln.Id = (++i).ToString();
                    pnpln.PatronNoticePolicyId = pnp2.Id;
                    pnpln.PatronNoticePolicy = pnp2;
                    if (load && pnpln.TemplateId != null) pnpln.Template = FindTemplate2(pnpln.TemplateId, cache: cache);
                }
            i = 0;
            if (pnp2.PatronNoticePolicyRequestNotices != null) foreach (var pnprn in pnp2.PatronNoticePolicyRequestNotices)
                {
                    pnprn.Id = (++i).ToString();
                    pnprn.PatronNoticePolicyId = pnp2.Id;
                    pnprn.PatronNoticePolicy = pnp2;
                    if (load && pnprn.TemplateId != null) pnprn.Template = FindTemplate2(pnprn.TemplateId, cache: cache);
                }
            return pnp2;
        }

        public void Insert(PatronNoticePolicy2 patronNoticePolicy2)
        {
            if (patronNoticePolicy2.Id == null) patronNoticePolicy2.Id = Guid.NewGuid();
            FolioServiceClient.InsertPatronNoticePolicy(patronNoticePolicy2.ToJObject());
        }

        public void Update(PatronNoticePolicy2 patronNoticePolicy2) => FolioServiceClient.UpdatePatronNoticePolicy(patronNoticePolicy2.ToJObject());

        public void UpdateOrInsert(PatronNoticePolicy2 patronNoticePolicy2)
        {
            if (patronNoticePolicy2.Id == null)
                Insert(patronNoticePolicy2);
            else
                try
                {
                    Update(patronNoticePolicy2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(patronNoticePolicy2); else throw;
                }
        }

        public void InsertOrUpdate(PatronNoticePolicy2 patronNoticePolicy2)
        {
            try
            {
                Insert(patronNoticePolicy2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(patronNoticePolicy2); else throw;
            }
        }

        public void DeletePatronNoticePolicy2(Guid? id) => FolioServiceClient.DeletePatronNoticePolicy(id?.ToString());

        public bool AnyPayment2s(string where = null) => FolioServiceClient.AnyPayments(where);

        public int CountPayment2s(string where = null) => FolioServiceClient.CountPayments(where);

        public Payment2[] Payment2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Payments(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var p2 = cache ? (Payment2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Payment2.FromJObject(jo)) : Payment2.FromJObject(jo);
                if (load && p2.FeeId != null) p2.Fee = FindFee2(p2.FeeId, cache: cache);
                if (load && p2.UserId != null) p2.User = FindUser2(p2.UserId, cache: cache);
                return p2;
            }).ToArray();
        }

        public IEnumerable<Payment2> Payment2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Payments(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var p2 = cache ? (Payment2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Payment2.FromJObject(jo)) : Payment2.FromJObject(jo);
                if (load && p2.FeeId != null) p2.Fee = FindFee2(p2.FeeId, cache: cache);
                if (load && p2.UserId != null) p2.User = FindUser2(p2.UserId, cache: cache);
                yield return p2;
            }
        }

        public Payment2 FindPayment2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var p2 = cache ? (Payment2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Payment2.FromJObject(FolioServiceClient.GetPayment(id?.ToString()))) : Payment2.FromJObject(FolioServiceClient.GetPayment(id?.ToString()));
            if (p2 == null) return null;
            if (load && p2.FeeId != null) p2.Fee = FindFee2(p2.FeeId, cache: cache);
            if (load && p2.UserId != null) p2.User = FindUser2(p2.UserId, cache: cache);
            return p2;
        }

        public void Insert(Payment2 payment2)
        {
            if (payment2.Id == null) payment2.Id = Guid.NewGuid();
            FolioServiceClient.InsertPayment(payment2.ToJObject());
        }

        public void Update(Payment2 payment2) => FolioServiceClient.UpdatePayment(payment2.ToJObject());

        public void UpdateOrInsert(Payment2 payment2)
        {
            if (payment2.Id == null)
                Insert(payment2);
            else
                try
                {
                    Update(payment2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(payment2); else throw;
                }
        }

        public void InsertOrUpdate(Payment2 payment2)
        {
            try
            {
                Insert(payment2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(payment2); else throw;
            }
        }

        public void DeletePayment2(Guid? id) => FolioServiceClient.DeletePayment(id?.ToString());

        public bool AnyPaymentMethod2s(string where = null) => FolioServiceClient.AnyPaymentMethods(where);

        public int CountPaymentMethod2s(string where = null) => FolioServiceClient.CountPaymentMethods(where);

        public PaymentMethod2[] PaymentMethod2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.PaymentMethods(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var pm2 = cache ? (PaymentMethod2)(objects.ContainsKey(id) ? objects[id] : objects[id] = PaymentMethod2.FromJObject(jo)) : PaymentMethod2.FromJObject(jo);
                if (load && pm2.CreationUserId != null) pm2.CreationUser = FindUser2(pm2.CreationUserId, cache: cache);
                if (load && pm2.LastWriteUserId != null) pm2.LastWriteUser = FindUser2(pm2.LastWriteUserId, cache: cache);
                if (load && pm2.OwnerId != null) pm2.Owner = FindOwner2(pm2.OwnerId, cache: cache);
                return pm2;
            }).ToArray();
        }

        public IEnumerable<PaymentMethod2> PaymentMethod2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.PaymentMethods(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var pm2 = cache ? (PaymentMethod2)(objects.ContainsKey(id) ? objects[id] : objects[id] = PaymentMethod2.FromJObject(jo)) : PaymentMethod2.FromJObject(jo);
                if (load && pm2.CreationUserId != null) pm2.CreationUser = FindUser2(pm2.CreationUserId, cache: cache);
                if (load && pm2.LastWriteUserId != null) pm2.LastWriteUser = FindUser2(pm2.LastWriteUserId, cache: cache);
                if (load && pm2.OwnerId != null) pm2.Owner = FindOwner2(pm2.OwnerId, cache: cache);
                yield return pm2;
            }
        }

        public PaymentMethod2 FindPaymentMethod2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var pm2 = cache ? (PaymentMethod2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = PaymentMethod2.FromJObject(FolioServiceClient.GetPaymentMethod(id?.ToString()))) : PaymentMethod2.FromJObject(FolioServiceClient.GetPaymentMethod(id?.ToString()));
            if (pm2 == null) return null;
            if (load && pm2.CreationUserId != null) pm2.CreationUser = FindUser2(pm2.CreationUserId, cache: cache);
            if (load && pm2.LastWriteUserId != null) pm2.LastWriteUser = FindUser2(pm2.LastWriteUserId, cache: cache);
            if (load && pm2.OwnerId != null) pm2.Owner = FindOwner2(pm2.OwnerId, cache: cache);
            return pm2;
        }

        public void Insert(PaymentMethod2 paymentMethod2)
        {
            if (paymentMethod2.Id == null) paymentMethod2.Id = Guid.NewGuid();
            FolioServiceClient.InsertPaymentMethod(paymentMethod2.ToJObject());
        }

        public void Update(PaymentMethod2 paymentMethod2) => FolioServiceClient.UpdatePaymentMethod(paymentMethod2.ToJObject());

        public void UpdateOrInsert(PaymentMethod2 paymentMethod2)
        {
            if (paymentMethod2.Id == null)
                Insert(paymentMethod2);
            else
                try
                {
                    Update(paymentMethod2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(paymentMethod2); else throw;
                }
        }

        public void InsertOrUpdate(PaymentMethod2 paymentMethod2)
        {
            try
            {
                Insert(paymentMethod2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(paymentMethod2); else throw;
            }
        }

        public void DeletePaymentMethod2(Guid? id) => FolioServiceClient.DeletePaymentMethod(id?.ToString());

        public bool AnyPermission2s(string where = null) => FolioServiceClient.AnyPermissions(where);

        public int CountPermission2s(string where = null) => FolioServiceClient.CountPermissions(where);

        public Permission2[] Permission2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Permissions(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var p2 = cache ? (Permission2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Permission2.FromJObject(jo)) : Permission2.FromJObject(jo);
                if (load && p2.CreationUserId != null) p2.CreationUser = FindUser2(p2.CreationUserId, cache: cache);
                if (load && p2.LastWriteUserId != null) p2.LastWriteUser = FindUser2(p2.LastWriteUserId, cache: cache);
                return p2;
            }).ToArray();
        }

        public IEnumerable<Permission2> Permission2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Permissions(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var p2 = cache ? (Permission2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Permission2.FromJObject(jo)) : Permission2.FromJObject(jo);
                if (load && p2.CreationUserId != null) p2.CreationUser = FindUser2(p2.CreationUserId, cache: cache);
                if (load && p2.LastWriteUserId != null) p2.LastWriteUser = FindUser2(p2.LastWriteUserId, cache: cache);
                yield return p2;
            }
        }

        public Permission2 FindPermission2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var p2 = cache ? (Permission2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Permission2.FromJObject(FolioServiceClient.GetPermission(id?.ToString()))) : Permission2.FromJObject(FolioServiceClient.GetPermission(id?.ToString()));
            if (p2 == null) return null;
            if (load && p2.CreationUserId != null) p2.CreationUser = FindUser2(p2.CreationUserId, cache: cache);
            if (load && p2.LastWriteUserId != null) p2.LastWriteUser = FindUser2(p2.LastWriteUserId, cache: cache);
            var i = 0;
            if (p2.PermissionChildOfs != null) foreach (var pco in p2.PermissionChildOfs)
                {
                    pco.Id = (++i).ToString();
                    pco.PermissionId = p2.Id;
                    pco.Permission = p2;
                }
            i = 0;
            if (p2.PermissionGrantedTos != null) foreach (var pgt in p2.PermissionGrantedTos)
                {
                    pgt.Id = (++i).ToString();
                    pgt.PermissionId = p2.Id;
                    pgt.Permission = p2;
                    if (load && pgt.PermissionsUserId != null) pgt.PermissionsUser = FindPermissionsUser2(pgt.PermissionsUserId, cache: cache);
                }
            i = 0;
            if (p2.PermissionSubPermissions != null) foreach (var psp in p2.PermissionSubPermissions)
                {
                    psp.Id = (++i).ToString();
                    psp.PermissionId = p2.Id;
                    psp.Permission = p2;
                }
            i = 0;
            if (p2.PermissionTags != null) foreach (var pt in p2.PermissionTags)
                {
                    pt.Id = (++i).ToString();
                    pt.PermissionId = p2.Id;
                    pt.Permission = p2;
                }
            return p2;
        }

        public void Insert(Permission2 permission2)
        {
            if (permission2.Id == null) permission2.Id = Guid.NewGuid();
            FolioServiceClient.InsertPermission(permission2.ToJObject());
        }

        public void Update(Permission2 permission2) => FolioServiceClient.UpdatePermission(permission2.ToJObject());

        public void UpdateOrInsert(Permission2 permission2)
        {
            if (permission2.Id == null)
                Insert(permission2);
            else
                try
                {
                    Update(permission2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(permission2); else throw;
                }
        }

        public void InsertOrUpdate(Permission2 permission2)
        {
            try
            {
                Insert(permission2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(permission2); else throw;
            }
        }

        public void DeletePermission2(Guid? id) => FolioServiceClient.DeletePermission(id?.ToString());

        public bool AnyPermissionsUser2s(string where = null) => FolioServiceClient.AnyPermissionsUsers(where);

        public int CountPermissionsUser2s(string where = null) => FolioServiceClient.CountPermissionsUsers(where);

        public PermissionsUser2[] PermissionsUser2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.PermissionsUsers(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var pu2 = cache ? (PermissionsUser2)(objects.ContainsKey(id) ? objects[id] : objects[id] = PermissionsUser2.FromJObject(jo)) : PermissionsUser2.FromJObject(jo);
                if (load && pu2.UserId != null) pu2.User = FindUser2(pu2.UserId, cache: cache);
                if (load && pu2.CreationUserId != null) pu2.CreationUser = FindUser2(pu2.CreationUserId, cache: cache);
                if (load && pu2.LastWriteUserId != null) pu2.LastWriteUser = FindUser2(pu2.LastWriteUserId, cache: cache);
                return pu2;
            }).ToArray();
        }

        public IEnumerable<PermissionsUser2> PermissionsUser2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.PermissionsUsers(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var pu2 = cache ? (PermissionsUser2)(objects.ContainsKey(id) ? objects[id] : objects[id] = PermissionsUser2.FromJObject(jo)) : PermissionsUser2.FromJObject(jo);
                if (load && pu2.UserId != null) pu2.User = FindUser2(pu2.UserId, cache: cache);
                if (load && pu2.CreationUserId != null) pu2.CreationUser = FindUser2(pu2.CreationUserId, cache: cache);
                if (load && pu2.LastWriteUserId != null) pu2.LastWriteUser = FindUser2(pu2.LastWriteUserId, cache: cache);
                yield return pu2;
            }
        }

        public PermissionsUser2 FindPermissionsUser2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var pu2 = cache ? (PermissionsUser2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = PermissionsUser2.FromJObject(FolioServiceClient.GetPermissionsUser(id?.ToString()))) : PermissionsUser2.FromJObject(FolioServiceClient.GetPermissionsUser(id?.ToString()));
            if (pu2 == null) return null;
            if (load && pu2.UserId != null) pu2.User = FindUser2(pu2.UserId, cache: cache);
            if (load && pu2.CreationUserId != null) pu2.CreationUser = FindUser2(pu2.CreationUserId, cache: cache);
            if (load && pu2.LastWriteUserId != null) pu2.LastWriteUser = FindUser2(pu2.LastWriteUserId, cache: cache);
            var i = 0;
            if (pu2.PermissionsUserPermissions != null) foreach (var pup in pu2.PermissionsUserPermissions)
                {
                    pup.Id = (++i).ToString();
                    pup.PermissionsUserId = pu2.Id;
                    pup.PermissionsUser = pu2;
                }
            return pu2;
        }

        public void Insert(PermissionsUser2 permissionsUser2)
        {
            if (permissionsUser2.Id == null) permissionsUser2.Id = Guid.NewGuid();
            FolioServiceClient.InsertPermissionsUser(permissionsUser2.ToJObject());
        }

        public void Update(PermissionsUser2 permissionsUser2) => FolioServiceClient.UpdatePermissionsUser(permissionsUser2.ToJObject());

        public void UpdateOrInsert(PermissionsUser2 permissionsUser2)
        {
            if (permissionsUser2.Id == null)
                Insert(permissionsUser2);
            else
                try
                {
                    Update(permissionsUser2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(permissionsUser2); else throw;
                }
        }

        public void InsertOrUpdate(PermissionsUser2 permissionsUser2)
        {
            try
            {
                Insert(permissionsUser2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(permissionsUser2); else throw;
            }
        }

        public void DeletePermissionsUser2(Guid? id) => FolioServiceClient.DeletePermissionsUser(id?.ToString());

        public bool AnyPrecedingSucceedingTitle2s(string where = null) => FolioServiceClient.AnyPrecedingSucceedingTitles(where);

        public int CountPrecedingSucceedingTitle2s(string where = null) => FolioServiceClient.CountPrecedingSucceedingTitles(where);

        public PrecedingSucceedingTitle2[] PrecedingSucceedingTitle2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.PrecedingSucceedingTitles(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var pst2 = cache ? (PrecedingSucceedingTitle2)(objects.ContainsKey(id) ? objects[id] : objects[id] = PrecedingSucceedingTitle2.FromJObject(jo)) : PrecedingSucceedingTitle2.FromJObject(jo);
                if (load && pst2.PrecedingInstanceId != null) pst2.PrecedingInstance = FindInstance2(pst2.PrecedingInstanceId, cache: cache);
                if (load && pst2.SucceedingInstanceId != null) pst2.SucceedingInstance = FindInstance2(pst2.SucceedingInstanceId, cache: cache);
                if (load && pst2.CreationUserId != null) pst2.CreationUser = FindUser2(pst2.CreationUserId, cache: cache);
                if (load && pst2.LastWriteUserId != null) pst2.LastWriteUser = FindUser2(pst2.LastWriteUserId, cache: cache);
                return pst2;
            }).ToArray();
        }

        public IEnumerable<PrecedingSucceedingTitle2> PrecedingSucceedingTitle2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.PrecedingSucceedingTitles(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var pst2 = cache ? (PrecedingSucceedingTitle2)(objects.ContainsKey(id) ? objects[id] : objects[id] = PrecedingSucceedingTitle2.FromJObject(jo)) : PrecedingSucceedingTitle2.FromJObject(jo);
                if (load && pst2.PrecedingInstanceId != null) pst2.PrecedingInstance = FindInstance2(pst2.PrecedingInstanceId, cache: cache);
                if (load && pst2.SucceedingInstanceId != null) pst2.SucceedingInstance = FindInstance2(pst2.SucceedingInstanceId, cache: cache);
                if (load && pst2.CreationUserId != null) pst2.CreationUser = FindUser2(pst2.CreationUserId, cache: cache);
                if (load && pst2.LastWriteUserId != null) pst2.LastWriteUser = FindUser2(pst2.LastWriteUserId, cache: cache);
                yield return pst2;
            }
        }

        public PrecedingSucceedingTitle2 FindPrecedingSucceedingTitle2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var pst2 = cache ? (PrecedingSucceedingTitle2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = PrecedingSucceedingTitle2.FromJObject(FolioServiceClient.GetPrecedingSucceedingTitle(id?.ToString()))) : PrecedingSucceedingTitle2.FromJObject(FolioServiceClient.GetPrecedingSucceedingTitle(id?.ToString()));
            if (pst2 == null) return null;
            if (load && pst2.PrecedingInstanceId != null) pst2.PrecedingInstance = FindInstance2(pst2.PrecedingInstanceId, cache: cache);
            if (load && pst2.SucceedingInstanceId != null) pst2.SucceedingInstance = FindInstance2(pst2.SucceedingInstanceId, cache: cache);
            if (load && pst2.CreationUserId != null) pst2.CreationUser = FindUser2(pst2.CreationUserId, cache: cache);
            if (load && pst2.LastWriteUserId != null) pst2.LastWriteUser = FindUser2(pst2.LastWriteUserId, cache: cache);
            var i = 0;
            if (pst2.PrecedingSucceedingTitleIdentifiers != null) foreach (var psti in pst2.PrecedingSucceedingTitleIdentifiers)
                {
                    psti.Id = (++i).ToString();
                    psti.PrecedingSucceedingTitleId = pst2.Id;
                    psti.PrecedingSucceedingTitle = pst2;
                    if (load && psti.IdentifierTypeId != null) psti.IdentifierType = FindIdType2(psti.IdentifierTypeId, cache: cache);
                }
            return pst2;
        }

        public void Insert(PrecedingSucceedingTitle2 precedingSucceedingTitle2)
        {
            if (precedingSucceedingTitle2.Id == null) precedingSucceedingTitle2.Id = Guid.NewGuid();
            FolioServiceClient.InsertPrecedingSucceedingTitle(precedingSucceedingTitle2.ToJObject());
        }

        public void Update(PrecedingSucceedingTitle2 precedingSucceedingTitle2) => FolioServiceClient.UpdatePrecedingSucceedingTitle(precedingSucceedingTitle2.ToJObject());

        public void UpdateOrInsert(PrecedingSucceedingTitle2 precedingSucceedingTitle2)
        {
            if (precedingSucceedingTitle2.Id == null)
                Insert(precedingSucceedingTitle2);
            else
                try
                {
                    Update(precedingSucceedingTitle2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(precedingSucceedingTitle2); else throw;
                }
        }

        public void InsertOrUpdate(PrecedingSucceedingTitle2 precedingSucceedingTitle2)
        {
            try
            {
                Insert(precedingSucceedingTitle2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(precedingSucceedingTitle2); else throw;
            }
        }

        public void DeletePrecedingSucceedingTitle2(Guid? id) => FolioServiceClient.DeletePrecedingSucceedingTitle(id?.ToString());

        public bool AnyPrinters(string where = null) => FolioServiceClient.AnyPrinters(where);

        public int CountPrinters(string where = null) => FolioServiceClient.CountPrinters(where);

        public Printer[] Printers(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Printers(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var p = cache ? (Printer)(objects.ContainsKey(id) ? objects[id] : objects[id] = Printer.FromJObject(jo)) : Printer.FromJObject(jo);
                if (load && p.CreationUserId != null) p.CreationUser = FindUser2(p.CreationUserId, cache: cache);
                if (load && p.LastWriteUserId != null) p.LastWriteUser = FindUser2(p.LastWriteUserId, cache: cache);
                return p;
            }).ToArray();
        }

        public IEnumerable<Printer> Printers(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Printers(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var p = cache ? (Printer)(objects.ContainsKey(id) ? objects[id] : objects[id] = Printer.FromJObject(jo)) : Printer.FromJObject(jo);
                if (load && p.CreationUserId != null) p.CreationUser = FindUser2(p.CreationUserId, cache: cache);
                if (load && p.LastWriteUserId != null) p.LastWriteUser = FindUser2(p.LastWriteUserId, cache: cache);
                yield return p;
            }
        }

        public Printer FindPrinter(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var p = cache ? (Printer)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Printer.FromJObject(FolioServiceClient.GetPrinter(id?.ToString()))) : Printer.FromJObject(FolioServiceClient.GetPrinter(id?.ToString()));
            if (p == null) return null;
            if (load && p.CreationUserId != null) p.CreationUser = FindUser2(p.CreationUserId, cache: cache);
            if (load && p.LastWriteUserId != null) p.LastWriteUser = FindUser2(p.LastWriteUserId, cache: cache);
            return p;
        }

        public void Insert(Printer printer)
        {
            if (printer.Id == null) printer.Id = Guid.NewGuid();
            FolioServiceClient.InsertPrinter(printer.ToJObject());
        }

        public void Update(Printer printer) => FolioServiceClient.UpdatePrinter(printer.ToJObject());

        public void UpdateOrInsert(Printer printer)
        {
            if (printer.Id == null)
                Insert(printer);
            else
                try
                {
                    Update(printer);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(printer); else throw;
                }
        }

        public void InsertOrUpdate(Printer printer)
        {
            try
            {
                Insert(printer);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(printer); else throw;
            }
        }

        public void DeletePrinter(Guid? id) => FolioServiceClient.DeletePrinter(id?.ToString());

        public bool AnyProxy2s(string where = null) => FolioServiceClient.AnyProxies(where);

        public int CountProxy2s(string where = null) => FolioServiceClient.CountProxies(where);

        public Proxy2[] Proxy2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Proxies(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var p2 = cache ? (Proxy2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Proxy2.FromJObject(jo)) : Proxy2.FromJObject(jo);
                if (load && p2.UserId != null) p2.User = FindUser2(p2.UserId, cache: cache);
                if (load && p2.ProxyUserId != null) p2.ProxyUser = FindUser2(p2.ProxyUserId, cache: cache);
                if (load && p2.CreationUserId != null) p2.CreationUser = FindUser2(p2.CreationUserId, cache: cache);
                if (load && p2.LastWriteUserId != null) p2.LastWriteUser = FindUser2(p2.LastWriteUserId, cache: cache);
                return p2;
            }).ToArray();
        }

        public IEnumerable<Proxy2> Proxy2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Proxies(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var p2 = cache ? (Proxy2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Proxy2.FromJObject(jo)) : Proxy2.FromJObject(jo);
                if (load && p2.UserId != null) p2.User = FindUser2(p2.UserId, cache: cache);
                if (load && p2.ProxyUserId != null) p2.ProxyUser = FindUser2(p2.ProxyUserId, cache: cache);
                if (load && p2.CreationUserId != null) p2.CreationUser = FindUser2(p2.CreationUserId, cache: cache);
                if (load && p2.LastWriteUserId != null) p2.LastWriteUser = FindUser2(p2.LastWriteUserId, cache: cache);
                yield return p2;
            }
        }

        public Proxy2 FindProxy2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var p2 = cache ? (Proxy2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Proxy2.FromJObject(FolioServiceClient.GetProxy(id?.ToString()))) : Proxy2.FromJObject(FolioServiceClient.GetProxy(id?.ToString()));
            if (p2 == null) return null;
            if (load && p2.UserId != null) p2.User = FindUser2(p2.UserId, cache: cache);
            if (load && p2.ProxyUserId != null) p2.ProxyUser = FindUser2(p2.ProxyUserId, cache: cache);
            if (load && p2.CreationUserId != null) p2.CreationUser = FindUser2(p2.CreationUserId, cache: cache);
            if (load && p2.LastWriteUserId != null) p2.LastWriteUser = FindUser2(p2.LastWriteUserId, cache: cache);
            return p2;
        }

        public void Insert(Proxy2 proxy2)
        {
            if (proxy2.Id == null) proxy2.Id = Guid.NewGuid();
            FolioServiceClient.InsertProxy(proxy2.ToJObject());
        }

        public void Update(Proxy2 proxy2) => FolioServiceClient.UpdateProxy(proxy2.ToJObject());

        public void UpdateOrInsert(Proxy2 proxy2)
        {
            if (proxy2.Id == null)
                Insert(proxy2);
            else
                try
                {
                    Update(proxy2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(proxy2); else throw;
                }
        }

        public void InsertOrUpdate(Proxy2 proxy2)
        {
            try
            {
                Insert(proxy2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(proxy2); else throw;
            }
        }

        public void DeleteProxy2(Guid? id) => FolioServiceClient.DeleteProxy(id?.ToString());

        public bool AnyReceiving2s(string where = null) => FolioServiceClient.AnyReceivings(where);

        public int CountReceiving2s(string where = null) => FolioServiceClient.CountReceivings(where);

        public Receiving2[] Receiving2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Receivings(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var r2 = cache ? (Receiving2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Receiving2.FromJObject(jo)) : Receiving2.FromJObject(jo);
                if (load && r2.ItemId != null) r2.Item = FindItem2(r2.ItemId, cache: cache);
                if (load && r2.LocationId != null) r2.Location = FindLocation2(r2.LocationId, cache: cache);
                if (load && r2.OrderItemId != null) r2.OrderItem = FindOrderItem2(r2.OrderItemId, cache: cache);
                if (load && r2.TitleId != null) r2.Title = FindTitle2(r2.TitleId, cache: cache);
                if (load && r2.HoldingId != null) r2.Holding = FindHolding2(r2.HoldingId, cache: cache);
                if (load && r2.CreationUserId != null) r2.CreationUser = FindUser2(r2.CreationUserId, cache: cache);
                if (load && r2.LastWriteUserId != null) r2.LastWriteUser = FindUser2(r2.LastWriteUserId, cache: cache);
                return r2;
            }).ToArray();
        }

        public IEnumerable<Receiving2> Receiving2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Receivings(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var r2 = cache ? (Receiving2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Receiving2.FromJObject(jo)) : Receiving2.FromJObject(jo);
                if (load && r2.ItemId != null) r2.Item = FindItem2(r2.ItemId, cache: cache);
                if (load && r2.LocationId != null) r2.Location = FindLocation2(r2.LocationId, cache: cache);
                if (load && r2.OrderItemId != null) r2.OrderItem = FindOrderItem2(r2.OrderItemId, cache: cache);
                if (load && r2.TitleId != null) r2.Title = FindTitle2(r2.TitleId, cache: cache);
                if (load && r2.HoldingId != null) r2.Holding = FindHolding2(r2.HoldingId, cache: cache);
                if (load && r2.CreationUserId != null) r2.CreationUser = FindUser2(r2.CreationUserId, cache: cache);
                if (load && r2.LastWriteUserId != null) r2.LastWriteUser = FindUser2(r2.LastWriteUserId, cache: cache);
                yield return r2;
            }
        }

        public Receiving2 FindReceiving2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var r2 = cache ? (Receiving2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Receiving2.FromJObject(FolioServiceClient.GetReceiving(id?.ToString()))) : Receiving2.FromJObject(FolioServiceClient.GetReceiving(id?.ToString()));
            if (r2 == null) return null;
            if (load && r2.ItemId != null) r2.Item = FindItem2(r2.ItemId, cache: cache);
            if (load && r2.LocationId != null) r2.Location = FindLocation2(r2.LocationId, cache: cache);
            if (load && r2.OrderItemId != null) r2.OrderItem = FindOrderItem2(r2.OrderItemId, cache: cache);
            if (load && r2.TitleId != null) r2.Title = FindTitle2(r2.TitleId, cache: cache);
            if (load && r2.HoldingId != null) r2.Holding = FindHolding2(r2.HoldingId, cache: cache);
            if (load && r2.CreationUserId != null) r2.CreationUser = FindUser2(r2.CreationUserId, cache: cache);
            if (load && r2.LastWriteUserId != null) r2.LastWriteUser = FindUser2(r2.LastWriteUserId, cache: cache);
            return r2;
        }

        public void Insert(Receiving2 receiving2)
        {
            if (receiving2.Id == null) receiving2.Id = Guid.NewGuid();
            FolioServiceClient.InsertReceiving(receiving2.ToJObject());
        }

        public void Update(Receiving2 receiving2) => FolioServiceClient.UpdateReceiving(receiving2.ToJObject());

        public void UpdateOrInsert(Receiving2 receiving2)
        {
            if (receiving2.Id == null)
                Insert(receiving2);
            else
                try
                {
                    Update(receiving2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(receiving2); else throw;
                }
        }

        public void InsertOrUpdate(Receiving2 receiving2)
        {
            try
            {
                Insert(receiving2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(receiving2); else throw;
            }
        }

        public void DeleteReceiving2(Guid? id) => FolioServiceClient.DeleteReceiving(id?.ToString());

        public bool AnyRecord2s(string where = null) => FolioServiceClient.AnyRecords(where);

        public int CountRecord2s(string where = null) => FolioServiceClient.CountRecords(where);

        public Record2[] Record2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Records(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var r2 = cache ? (Record2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Record2.FromJObject(jo)) : Record2.FromJObject(jo);
                if (load && r2.SnapshotId != null) r2.Snapshot = FindSnapshot2(r2.SnapshotId, cache: cache);
                if (load && r2.InstanceId != null) r2.Instance = FindInstance2(r2.InstanceId, cache: cache);
                if (load && r2.CreationUserId != null) r2.CreationUser = FindUser2(r2.CreationUserId, cache: cache);
                if (load && r2.LastWriteUserId != null) r2.LastWriteUser = FindUser2(r2.LastWriteUserId, cache: cache);
                return r2;
            }).ToArray();
        }

        public IEnumerable<Record2> Record2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Records(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var r2 = cache ? (Record2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Record2.FromJObject(jo)) : Record2.FromJObject(jo);
                if (load && r2.SnapshotId != null) r2.Snapshot = FindSnapshot2(r2.SnapshotId, cache: cache);
                if (load && r2.InstanceId != null) r2.Instance = FindInstance2(r2.InstanceId, cache: cache);
                if (load && r2.CreationUserId != null) r2.CreationUser = FindUser2(r2.CreationUserId, cache: cache);
                if (load && r2.LastWriteUserId != null) r2.LastWriteUser = FindUser2(r2.LastWriteUserId, cache: cache);
                yield return r2;
            }
        }

        public Record2 FindRecord2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var r2 = cache ? (Record2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Record2.FromJObject(FolioServiceClient.GetRecord(id?.ToString()))) : Record2.FromJObject(FolioServiceClient.GetRecord(id?.ToString()));
            if (r2 == null) return null;
            if (load && r2.SnapshotId != null) r2.Snapshot = FindSnapshot2(r2.SnapshotId, cache: cache);
            if (load && r2.InstanceId != null) r2.Instance = FindInstance2(r2.InstanceId, cache: cache);
            if (load && r2.CreationUserId != null) r2.CreationUser = FindUser2(r2.CreationUserId, cache: cache);
            if (load && r2.LastWriteUserId != null) r2.LastWriteUser = FindUser2(r2.LastWriteUserId, cache: cache);
            return r2;
        }

        public void Insert(Record2 record2)
        {
            if (record2.Id == null) record2.Id = Guid.NewGuid();
            FolioServiceClient.InsertRecord(record2.ToJObject());
        }

        public void Update(Record2 record2) => FolioServiceClient.UpdateRecord(record2.ToJObject());

        public void UpdateOrInsert(Record2 record2)
        {
            if (record2.Id == null)
                Insert(record2);
            else
                try
                {
                    Update(record2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(record2); else throw;
                }
        }

        public void InsertOrUpdate(Record2 record2)
        {
            try
            {
                Insert(record2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(record2); else throw;
            }
        }

        public void DeleteRecord2(Guid? id) => FolioServiceClient.DeleteRecord(id?.ToString());

        public bool AnyReferenceData2s(string where = null) => FolioServiceClient.AnyReferenceDatas(where);

        public int CountReferenceData2s(string where = null) => FolioServiceClient.CountReferenceDatas(where);

        public ReferenceData2[] ReferenceData2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ReferenceDatas(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var rd2 = cache ? (ReferenceData2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ReferenceData2.FromJObject(jo)) : ReferenceData2.FromJObject(jo);
                return rd2;
            }).ToArray();
        }

        public IEnumerable<ReferenceData2> ReferenceData2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ReferenceDatas(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var rd2 = cache ? (ReferenceData2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ReferenceData2.FromJObject(jo)) : ReferenceData2.FromJObject(jo);
                yield return rd2;
            }
        }

        public ReferenceData2 FindReferenceData2(Guid? id, bool load = false, bool cache = true) => ReferenceData2.FromJObject(FolioServiceClient.GetReferenceData(id?.ToString()));

        public void Insert(ReferenceData2 referenceData2)
        {
            if (referenceData2.Id == null) referenceData2.Id = Guid.NewGuid();
            FolioServiceClient.InsertReferenceData(referenceData2.ToJObject());
        }

        public void Update(ReferenceData2 referenceData2) => FolioServiceClient.UpdateReferenceData(referenceData2.ToJObject());

        public void UpdateOrInsert(ReferenceData2 referenceData2)
        {
            if (referenceData2.Id == null)
                Insert(referenceData2);
            else
                try
                {
                    Update(referenceData2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(referenceData2); else throw;
                }
        }

        public void InsertOrUpdate(ReferenceData2 referenceData2)
        {
            try
            {
                Insert(referenceData2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(referenceData2); else throw;
            }
        }

        public void DeleteReferenceData2(Guid? id) => FolioServiceClient.DeleteReferenceData(id?.ToString());

        public bool AnyRefundReason2s(string where = null) => FolioServiceClient.AnyRefundReasons(where);

        public int CountRefundReason2s(string where = null) => FolioServiceClient.CountRefundReasons(where);

        public RefundReason2[] RefundReason2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.RefundReasons(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var rr2 = cache ? (RefundReason2)(objects.ContainsKey(id) ? objects[id] : objects[id] = RefundReason2.FromJObject(jo)) : RefundReason2.FromJObject(jo);
                if (load && rr2.CreationUserId != null) rr2.CreationUser = FindUser2(rr2.CreationUserId, cache: cache);
                if (load && rr2.LastWriteUserId != null) rr2.LastWriteUser = FindUser2(rr2.LastWriteUserId, cache: cache);
                if (load && rr2.AccountId != null) rr2.Account = FindFee2(rr2.AccountId, cache: cache);
                return rr2;
            }).ToArray();
        }

        public IEnumerable<RefundReason2> RefundReason2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.RefundReasons(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var rr2 = cache ? (RefundReason2)(objects.ContainsKey(id) ? objects[id] : objects[id] = RefundReason2.FromJObject(jo)) : RefundReason2.FromJObject(jo);
                if (load && rr2.CreationUserId != null) rr2.CreationUser = FindUser2(rr2.CreationUserId, cache: cache);
                if (load && rr2.LastWriteUserId != null) rr2.LastWriteUser = FindUser2(rr2.LastWriteUserId, cache: cache);
                if (load && rr2.AccountId != null) rr2.Account = FindFee2(rr2.AccountId, cache: cache);
                yield return rr2;
            }
        }

        public RefundReason2 FindRefundReason2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var rr2 = cache ? (RefundReason2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = RefundReason2.FromJObject(FolioServiceClient.GetRefundReason(id?.ToString()))) : RefundReason2.FromJObject(FolioServiceClient.GetRefundReason(id?.ToString()));
            if (rr2 == null) return null;
            if (load && rr2.CreationUserId != null) rr2.CreationUser = FindUser2(rr2.CreationUserId, cache: cache);
            if (load && rr2.LastWriteUserId != null) rr2.LastWriteUser = FindUser2(rr2.LastWriteUserId, cache: cache);
            if (load && rr2.AccountId != null) rr2.Account = FindFee2(rr2.AccountId, cache: cache);
            return rr2;
        }

        public void Insert(RefundReason2 refundReason2)
        {
            if (refundReason2.Id == null) refundReason2.Id = Guid.NewGuid();
            FolioServiceClient.InsertRefundReason(refundReason2.ToJObject());
        }

        public void Update(RefundReason2 refundReason2) => FolioServiceClient.UpdateRefundReason(refundReason2.ToJObject());

        public void UpdateOrInsert(RefundReason2 refundReason2)
        {
            if (refundReason2.Id == null)
                Insert(refundReason2);
            else
                try
                {
                    Update(refundReason2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(refundReason2); else throw;
                }
        }

        public void InsertOrUpdate(RefundReason2 refundReason2)
        {
            try
            {
                Insert(refundReason2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(refundReason2); else throw;
            }
        }

        public void DeleteRefundReason2(Guid? id) => FolioServiceClient.DeleteRefundReason(id?.ToString());

        public bool AnyRelationships(string where = null) => FolioServiceClient.AnyInstanceRelationships(where);

        public int CountRelationships(string where = null) => FolioServiceClient.CountInstanceRelationships(where);

        public Relationship[] Relationships(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.InstanceRelationships(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var r = cache ? (Relationship)(objects.ContainsKey(id) ? objects[id] : objects[id] = Relationship.FromJObject(jo)) : Relationship.FromJObject(jo);
                if (load && r.SuperInstanceId != null) r.SuperInstance = FindInstance2(r.SuperInstanceId, cache: cache);
                if (load && r.SubInstanceId != null) r.SubInstance = FindInstance2(r.SubInstanceId, cache: cache);
                if (load && r.InstanceRelationshipTypeId != null) r.InstanceRelationshipType = FindRelationshipType(r.InstanceRelationshipTypeId, cache: cache);
                if (load && r.CreationUserId != null) r.CreationUser = FindUser2(r.CreationUserId, cache: cache);
                if (load && r.LastWriteUserId != null) r.LastWriteUser = FindUser2(r.LastWriteUserId, cache: cache);
                return r;
            }).ToArray();
        }

        public IEnumerable<Relationship> Relationships(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.InstanceRelationships(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var r = cache ? (Relationship)(objects.ContainsKey(id) ? objects[id] : objects[id] = Relationship.FromJObject(jo)) : Relationship.FromJObject(jo);
                if (load && r.SuperInstanceId != null) r.SuperInstance = FindInstance2(r.SuperInstanceId, cache: cache);
                if (load && r.SubInstanceId != null) r.SubInstance = FindInstance2(r.SubInstanceId, cache: cache);
                if (load && r.InstanceRelationshipTypeId != null) r.InstanceRelationshipType = FindRelationshipType(r.InstanceRelationshipTypeId, cache: cache);
                if (load && r.CreationUserId != null) r.CreationUser = FindUser2(r.CreationUserId, cache: cache);
                if (load && r.LastWriteUserId != null) r.LastWriteUser = FindUser2(r.LastWriteUserId, cache: cache);
                yield return r;
            }
        }

        public Relationship FindRelationship(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var r = cache ? (Relationship)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Relationship.FromJObject(FolioServiceClient.GetInstanceRelationship(id?.ToString()))) : Relationship.FromJObject(FolioServiceClient.GetInstanceRelationship(id?.ToString()));
            if (r == null) return null;
            if (load && r.SuperInstanceId != null) r.SuperInstance = FindInstance2(r.SuperInstanceId, cache: cache);
            if (load && r.SubInstanceId != null) r.SubInstance = FindInstance2(r.SubInstanceId, cache: cache);
            if (load && r.InstanceRelationshipTypeId != null) r.InstanceRelationshipType = FindRelationshipType(r.InstanceRelationshipTypeId, cache: cache);
            if (load && r.CreationUserId != null) r.CreationUser = FindUser2(r.CreationUserId, cache: cache);
            if (load && r.LastWriteUserId != null) r.LastWriteUser = FindUser2(r.LastWriteUserId, cache: cache);
            return r;
        }

        public void Insert(Relationship relationship)
        {
            if (relationship.Id == null) relationship.Id = Guid.NewGuid();
            FolioServiceClient.InsertInstanceRelationship(relationship.ToJObject());
        }

        public void Update(Relationship relationship) => FolioServiceClient.UpdateInstanceRelationship(relationship.ToJObject());

        public void UpdateOrInsert(Relationship relationship)
        {
            if (relationship.Id == null)
                Insert(relationship);
            else
                try
                {
                    Update(relationship);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(relationship); else throw;
                }
        }

        public void InsertOrUpdate(Relationship relationship)
        {
            try
            {
                Insert(relationship);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(relationship); else throw;
            }
        }

        public void DeleteRelationship(Guid? id) => FolioServiceClient.DeleteInstanceRelationship(id?.ToString());

        public bool AnyRelationshipTypes(string where = null) => FolioServiceClient.AnyInstanceRelationshipTypes(where);

        public int CountRelationshipTypes(string where = null) => FolioServiceClient.CountInstanceRelationshipTypes(where);

        public RelationshipType[] RelationshipTypes(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.InstanceRelationshipTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var rt = cache ? (RelationshipType)(objects.ContainsKey(id) ? objects[id] : objects[id] = RelationshipType.FromJObject(jo)) : RelationshipType.FromJObject(jo);
                if (load && rt.CreationUserId != null) rt.CreationUser = FindUser2(rt.CreationUserId, cache: cache);
                if (load && rt.LastWriteUserId != null) rt.LastWriteUser = FindUser2(rt.LastWriteUserId, cache: cache);
                return rt;
            }).ToArray();
        }

        public IEnumerable<RelationshipType> RelationshipTypes(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.InstanceRelationshipTypes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var rt = cache ? (RelationshipType)(objects.ContainsKey(id) ? objects[id] : objects[id] = RelationshipType.FromJObject(jo)) : RelationshipType.FromJObject(jo);
                if (load && rt.CreationUserId != null) rt.CreationUser = FindUser2(rt.CreationUserId, cache: cache);
                if (load && rt.LastWriteUserId != null) rt.LastWriteUser = FindUser2(rt.LastWriteUserId, cache: cache);
                yield return rt;
            }
        }

        public RelationshipType FindRelationshipType(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var rt = cache ? (RelationshipType)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = RelationshipType.FromJObject(FolioServiceClient.GetInstanceRelationshipType(id?.ToString()))) : RelationshipType.FromJObject(FolioServiceClient.GetInstanceRelationshipType(id?.ToString()));
            if (rt == null) return null;
            if (load && rt.CreationUserId != null) rt.CreationUser = FindUser2(rt.CreationUserId, cache: cache);
            if (load && rt.LastWriteUserId != null) rt.LastWriteUser = FindUser2(rt.LastWriteUserId, cache: cache);
            return rt;
        }

        public void Insert(RelationshipType relationshipType)
        {
            if (relationshipType.Id == null) relationshipType.Id = Guid.NewGuid();
            FolioServiceClient.InsertInstanceRelationshipType(relationshipType.ToJObject());
        }

        public void Update(RelationshipType relationshipType) => FolioServiceClient.UpdateInstanceRelationshipType(relationshipType.ToJObject());

        public void UpdateOrInsert(RelationshipType relationshipType)
        {
            if (relationshipType.Id == null)
                Insert(relationshipType);
            else
                try
                {
                    Update(relationshipType);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(relationshipType); else throw;
                }
        }

        public void InsertOrUpdate(RelationshipType relationshipType)
        {
            try
            {
                Insert(relationshipType);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(relationshipType); else throw;
            }
        }

        public void DeleteRelationshipType(Guid? id) => FolioServiceClient.DeleteInstanceRelationshipType(id?.ToString());

        public bool AnyRequest2s(string where = null) => FolioServiceClient.AnyRequests(where);

        public int CountRequest2s(string where = null) => FolioServiceClient.CountRequests(where);

        public Request2[] Request2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Requests(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var r2 = cache ? (Request2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Request2.FromJObject(jo)) : Request2.FromJObject(jo);
                if (load && r2.RequesterId != null) r2.Requester = FindUser2(r2.RequesterId, cache: cache);
                if (load && r2.ProxyUserId != null) r2.ProxyUser = FindUser2(r2.ProxyUserId, cache: cache);
                if (load && r2.InstanceId != null) r2.Instance = FindInstance2(r2.InstanceId, cache: cache);
                if (load && r2.HoldingId != null) r2.Holding = FindHolding2(r2.HoldingId, cache: cache);
                if (load && r2.ItemId != null) r2.Item = FindItem2(r2.ItemId, cache: cache);
                if (load && r2.CancellationReasonId != null) r2.CancellationReason = FindCancellationReason2(r2.CancellationReasonId, cache: cache);
                if (load && r2.CancelledByUserId != null) r2.CancelledByUser = FindUser2(r2.CancelledByUserId, cache: cache);
                if (load && r2.DeliveryAddressTypeId != null) r2.DeliveryAddressType = FindAddressType2(r2.DeliveryAddressTypeId, cache: cache);
                if (load && r2.PickupServicePointId != null) r2.PickupServicePoint = FindServicePoint2(r2.PickupServicePointId, cache: cache);
                if (load && r2.CreationUserId != null) r2.CreationUser = FindUser2(r2.CreationUserId, cache: cache);
                if (load && r2.LastWriteUserId != null) r2.LastWriteUser = FindUser2(r2.LastWriteUserId, cache: cache);
                if (load && r2.PrintDetailsRequesterId != null) r2.PrintDetailsRequester = FindUser2(r2.PrintDetailsRequesterId, cache: cache);
                return r2;
            }).ToArray();
        }

        public IEnumerable<Request2> Request2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Requests(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var r2 = cache ? (Request2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Request2.FromJObject(jo)) : Request2.FromJObject(jo);
                if (load && r2.RequesterId != null) r2.Requester = FindUser2(r2.RequesterId, cache: cache);
                if (load && r2.ProxyUserId != null) r2.ProxyUser = FindUser2(r2.ProxyUserId, cache: cache);
                if (load && r2.InstanceId != null) r2.Instance = FindInstance2(r2.InstanceId, cache: cache);
                if (load && r2.HoldingId != null) r2.Holding = FindHolding2(r2.HoldingId, cache: cache);
                if (load && r2.ItemId != null) r2.Item = FindItem2(r2.ItemId, cache: cache);
                if (load && r2.CancellationReasonId != null) r2.CancellationReason = FindCancellationReason2(r2.CancellationReasonId, cache: cache);
                if (load && r2.CancelledByUserId != null) r2.CancelledByUser = FindUser2(r2.CancelledByUserId, cache: cache);
                if (load && r2.DeliveryAddressTypeId != null) r2.DeliveryAddressType = FindAddressType2(r2.DeliveryAddressTypeId, cache: cache);
                if (load && r2.PickupServicePointId != null) r2.PickupServicePoint = FindServicePoint2(r2.PickupServicePointId, cache: cache);
                if (load && r2.CreationUserId != null) r2.CreationUser = FindUser2(r2.CreationUserId, cache: cache);
                if (load && r2.LastWriteUserId != null) r2.LastWriteUser = FindUser2(r2.LastWriteUserId, cache: cache);
                if (load && r2.PrintDetailsRequesterId != null) r2.PrintDetailsRequester = FindUser2(r2.PrintDetailsRequesterId, cache: cache);
                yield return r2;
            }
        }

        public Request2 FindRequest2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var r2 = cache ? (Request2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Request2.FromJObject(FolioServiceClient.GetRequest(id?.ToString()))) : Request2.FromJObject(FolioServiceClient.GetRequest(id?.ToString()));
            if (r2 == null) return null;
            if (load && r2.RequesterId != null) r2.Requester = FindUser2(r2.RequesterId, cache: cache);
            if (load && r2.ProxyUserId != null) r2.ProxyUser = FindUser2(r2.ProxyUserId, cache: cache);
            if (load && r2.InstanceId != null) r2.Instance = FindInstance2(r2.InstanceId, cache: cache);
            if (load && r2.HoldingId != null) r2.Holding = FindHolding2(r2.HoldingId, cache: cache);
            if (load && r2.ItemId != null) r2.Item = FindItem2(r2.ItemId, cache: cache);
            if (load && r2.CancellationReasonId != null) r2.CancellationReason = FindCancellationReason2(r2.CancellationReasonId, cache: cache);
            if (load && r2.CancelledByUserId != null) r2.CancelledByUser = FindUser2(r2.CancelledByUserId, cache: cache);
            if (load && r2.DeliveryAddressTypeId != null) r2.DeliveryAddressType = FindAddressType2(r2.DeliveryAddressTypeId, cache: cache);
            if (load && r2.PickupServicePointId != null) r2.PickupServicePoint = FindServicePoint2(r2.PickupServicePointId, cache: cache);
            if (load && r2.CreationUserId != null) r2.CreationUser = FindUser2(r2.CreationUserId, cache: cache);
            if (load && r2.LastWriteUserId != null) r2.LastWriteUser = FindUser2(r2.LastWriteUserId, cache: cache);
            if (load && r2.PrintDetailsRequesterId != null) r2.PrintDetailsRequester = FindUser2(r2.PrintDetailsRequesterId, cache: cache);
            var i = 0;
            if (r2.RequestIdentifiers != null) foreach (var ri in r2.RequestIdentifiers)
                {
                    ri.Id = (++i).ToString();
                    ri.RequestId = r2.Id;
                    ri.Request = r2;
                    if (load && ri.IdentifierTypeId != null) ri.IdentifierType = FindIdType2(ri.IdentifierTypeId, cache: cache);
                }
            i = 0;
            if (r2.RequestTags != null) foreach (var rt in r2.RequestTags)
                {
                    rt.Id = (++i).ToString();
                    rt.RequestId = r2.Id;
                    rt.Request = r2;
                }
            return r2;
        }

        public void Insert(Request2 request2)
        {
            if (request2.Id == null) request2.Id = Guid.NewGuid();
            FolioServiceClient.InsertRequest(request2.ToJObject());
        }

        public void Update(Request2 request2) => FolioServiceClient.UpdateRequest(request2.ToJObject());

        public void UpdateOrInsert(Request2 request2)
        {
            if (request2.Id == null)
                Insert(request2);
            else
                try
                {
                    Update(request2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(request2); else throw;
                }
        }

        public void InsertOrUpdate(Request2 request2)
        {
            try
            {
                Insert(request2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(request2); else throw;
            }
        }

        public void DeleteRequest2(Guid? id) => FolioServiceClient.DeleteRequest(id?.ToString());

        public bool AnyRequestPolicy2s(string where = null) => FolioServiceClient.AnyRequestPolicies(where);

        public int CountRequestPolicy2s(string where = null) => FolioServiceClient.CountRequestPolicies(where);

        public RequestPolicy2[] RequestPolicy2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.RequestPolicies(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var rp2 = cache ? (RequestPolicy2)(objects.ContainsKey(id) ? objects[id] : objects[id] = RequestPolicy2.FromJObject(jo)) : RequestPolicy2.FromJObject(jo);
                if (load && rp2.CreationUserId != null) rp2.CreationUser = FindUser2(rp2.CreationUserId, cache: cache);
                if (load && rp2.LastWriteUserId != null) rp2.LastWriteUser = FindUser2(rp2.LastWriteUserId, cache: cache);
                return rp2;
            }).ToArray();
        }

        public IEnumerable<RequestPolicy2> RequestPolicy2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.RequestPolicies(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var rp2 = cache ? (RequestPolicy2)(objects.ContainsKey(id) ? objects[id] : objects[id] = RequestPolicy2.FromJObject(jo)) : RequestPolicy2.FromJObject(jo);
                if (load && rp2.CreationUserId != null) rp2.CreationUser = FindUser2(rp2.CreationUserId, cache: cache);
                if (load && rp2.LastWriteUserId != null) rp2.LastWriteUser = FindUser2(rp2.LastWriteUserId, cache: cache);
                yield return rp2;
            }
        }

        public RequestPolicy2 FindRequestPolicy2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var rp2 = cache ? (RequestPolicy2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = RequestPolicy2.FromJObject(FolioServiceClient.GetRequestPolicy(id?.ToString()))) : RequestPolicy2.FromJObject(FolioServiceClient.GetRequestPolicy(id?.ToString()));
            if (rp2 == null) return null;
            if (load && rp2.CreationUserId != null) rp2.CreationUser = FindUser2(rp2.CreationUserId, cache: cache);
            if (load && rp2.LastWriteUserId != null) rp2.LastWriteUser = FindUser2(rp2.LastWriteUserId, cache: cache);
            var i = 0;
            if (rp2.RequestPolicyRequestTypes != null) foreach (var rprt in rp2.RequestPolicyRequestTypes)
                {
                    rprt.Id = (++i).ToString();
                    rprt.RequestPolicyId = rp2.Id;
                    rprt.RequestPolicy = rp2;
                }
            return rp2;
        }

        public void Insert(RequestPolicy2 requestPolicy2)
        {
            if (requestPolicy2.Id == null) requestPolicy2.Id = Guid.NewGuid();
            FolioServiceClient.InsertRequestPolicy(requestPolicy2.ToJObject());
        }

        public void Update(RequestPolicy2 requestPolicy2) => FolioServiceClient.UpdateRequestPolicy(requestPolicy2.ToJObject());

        public void UpdateOrInsert(RequestPolicy2 requestPolicy2)
        {
            if (requestPolicy2.Id == null)
                Insert(requestPolicy2);
            else
                try
                {
                    Update(requestPolicy2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(requestPolicy2); else throw;
                }
        }

        public void InsertOrUpdate(RequestPolicy2 requestPolicy2)
        {
            try
            {
                Insert(requestPolicy2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(requestPolicy2); else throw;
            }
        }

        public void DeleteRequestPolicy2(Guid? id) => FolioServiceClient.DeleteRequestPolicy(id?.ToString());

        public bool AnyRollover2s(string where = null) => FolioServiceClient.AnyRollovers(where);

        public int CountRollover2s(string where = null) => FolioServiceClient.CountRollovers(where);

        public Rollover2[] Rollover2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Rollovers(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var r2 = cache ? (Rollover2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Rollover2.FromJObject(jo)) : Rollover2.FromJObject(jo);
                if (load && r2.LedgerId != null) r2.Ledger = FindLedger2(r2.LedgerId, cache: cache);
                if (load && r2.FromFiscalYearId != null) r2.FromFiscalYear = FindFiscalYear2(r2.FromFiscalYearId, cache: cache);
                if (load && r2.ToFiscalYearId != null) r2.ToFiscalYear = FindFiscalYear2(r2.ToFiscalYearId, cache: cache);
                if (load && r2.CreationUserId != null) r2.CreationUser = FindUser2(r2.CreationUserId, cache: cache);
                if (load && r2.LastWriteUserId != null) r2.LastWriteUser = FindUser2(r2.LastWriteUserId, cache: cache);
                return r2;
            }).ToArray();
        }

        public IEnumerable<Rollover2> Rollover2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Rollovers(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var r2 = cache ? (Rollover2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Rollover2.FromJObject(jo)) : Rollover2.FromJObject(jo);
                if (load && r2.LedgerId != null) r2.Ledger = FindLedger2(r2.LedgerId, cache: cache);
                if (load && r2.FromFiscalYearId != null) r2.FromFiscalYear = FindFiscalYear2(r2.FromFiscalYearId, cache: cache);
                if (load && r2.ToFiscalYearId != null) r2.ToFiscalYear = FindFiscalYear2(r2.ToFiscalYearId, cache: cache);
                if (load && r2.CreationUserId != null) r2.CreationUser = FindUser2(r2.CreationUserId, cache: cache);
                if (load && r2.LastWriteUserId != null) r2.LastWriteUser = FindUser2(r2.LastWriteUserId, cache: cache);
                yield return r2;
            }
        }

        public Rollover2 FindRollover2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var r2 = cache ? (Rollover2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Rollover2.FromJObject(FolioServiceClient.GetRollover(id?.ToString()))) : Rollover2.FromJObject(FolioServiceClient.GetRollover(id?.ToString()));
            if (r2 == null) return null;
            if (load && r2.LedgerId != null) r2.Ledger = FindLedger2(r2.LedgerId, cache: cache);
            if (load && r2.FromFiscalYearId != null) r2.FromFiscalYear = FindFiscalYear2(r2.FromFiscalYearId, cache: cache);
            if (load && r2.ToFiscalYearId != null) r2.ToFiscalYear = FindFiscalYear2(r2.ToFiscalYearId, cache: cache);
            if (load && r2.CreationUserId != null) r2.CreationUser = FindUser2(r2.CreationUserId, cache: cache);
            if (load && r2.LastWriteUserId != null) r2.LastWriteUser = FindUser2(r2.LastWriteUserId, cache: cache);
            var i = 0;
            if (r2.RolloverBudgetsRollovers != null) foreach (var rbr in r2.RolloverBudgetsRollovers)
                {
                    rbr.Id = (++i).ToString();
                    rbr.RolloverId = r2.Id;
                    rbr.Rollover = r2;
                    if (load && rbr.FundTypeId != null) rbr.FundType = FindFundType2(rbr.FundTypeId, cache: cache);
                }
            i = 0;
            if (r2.RolloverEncumbrancesRollovers != null) foreach (var rer in r2.RolloverEncumbrancesRollovers)
                {
                    rer.Id = (++i).ToString();
                    rer.RolloverId = r2.Id;
                    rer.Rollover = r2;
                }
            return r2;
        }

        public void Insert(Rollover2 rollover2)
        {
            if (rollover2.Id == null) rollover2.Id = Guid.NewGuid();
            FolioServiceClient.InsertRollover(rollover2.ToJObject());
        }

        public void Update(Rollover2 rollover2) => FolioServiceClient.UpdateRollover(rollover2.ToJObject());

        public void UpdateOrInsert(Rollover2 rollover2)
        {
            if (rollover2.Id == null)
                Insert(rollover2);
            else
                try
                {
                    Update(rollover2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(rollover2); else throw;
                }
        }

        public void InsertOrUpdate(Rollover2 rollover2)
        {
            try
            {
                Insert(rollover2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(rollover2); else throw;
            }
        }

        public void DeleteRollover2(Guid? id) => FolioServiceClient.DeleteRollover(id?.ToString());

        public bool AnyRolloverBudget2s(string where = null) => FolioServiceClient.AnyRolloverBudgets(where);

        public int CountRolloverBudget2s(string where = null) => FolioServiceClient.CountRolloverBudgets(where);

        public RolloverBudget2[] RolloverBudget2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.RolloverBudgets(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var rb2 = cache ? (RolloverBudget2)(objects.ContainsKey(id) ? objects[id] : objects[id] = RolloverBudget2.FromJObject(jo)) : RolloverBudget2.FromJObject(jo);
                if (load && rb2.BudgetId != null) rb2.Budget = FindBudget2(rb2.BudgetId, cache: cache);
                if (load && rb2.RolloverId != null) rb2.Rollover = FindRollover2(rb2.RolloverId, cache: cache);
                if (load && rb2.FundDetailsFundTypeId != null) rb2.FundDetailsFundType = FindFundType2(rb2.FundDetailsFundTypeId, cache: cache);
                if (load && rb2.FundId != null) rb2.Fund = FindFund2(rb2.FundId, cache: cache);
                if (load && rb2.FiscalYearId != null) rb2.FiscalYear = FindFiscalYear2(rb2.FiscalYearId, cache: cache);
                if (load && rb2.CreationUserId != null) rb2.CreationUser = FindUser2(rb2.CreationUserId, cache: cache);
                if (load && rb2.LastWriteUserId != null) rb2.LastWriteUser = FindUser2(rb2.LastWriteUserId, cache: cache);
                return rb2;
            }).ToArray();
        }

        public IEnumerable<RolloverBudget2> RolloverBudget2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.RolloverBudgets(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var rb2 = cache ? (RolloverBudget2)(objects.ContainsKey(id) ? objects[id] : objects[id] = RolloverBudget2.FromJObject(jo)) : RolloverBudget2.FromJObject(jo);
                if (load && rb2.BudgetId != null) rb2.Budget = FindBudget2(rb2.BudgetId, cache: cache);
                if (load && rb2.RolloverId != null) rb2.Rollover = FindRollover2(rb2.RolloverId, cache: cache);
                if (load && rb2.FundDetailsFundTypeId != null) rb2.FundDetailsFundType = FindFundType2(rb2.FundDetailsFundTypeId, cache: cache);
                if (load && rb2.FundId != null) rb2.Fund = FindFund2(rb2.FundId, cache: cache);
                if (load && rb2.FiscalYearId != null) rb2.FiscalYear = FindFiscalYear2(rb2.FiscalYearId, cache: cache);
                if (load && rb2.CreationUserId != null) rb2.CreationUser = FindUser2(rb2.CreationUserId, cache: cache);
                if (load && rb2.LastWriteUserId != null) rb2.LastWriteUser = FindUser2(rb2.LastWriteUserId, cache: cache);
                yield return rb2;
            }
        }

        public RolloverBudget2 FindRolloverBudget2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var rb2 = cache ? (RolloverBudget2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = RolloverBudget2.FromJObject(FolioServiceClient.GetRolloverBudget(id?.ToString()))) : RolloverBudget2.FromJObject(FolioServiceClient.GetRolloverBudget(id?.ToString()));
            if (rb2 == null) return null;
            if (load && rb2.BudgetId != null) rb2.Budget = FindBudget2(rb2.BudgetId, cache: cache);
            if (load && rb2.RolloverId != null) rb2.Rollover = FindRollover2(rb2.RolloverId, cache: cache);
            if (load && rb2.FundDetailsFundTypeId != null) rb2.FundDetailsFundType = FindFundType2(rb2.FundDetailsFundTypeId, cache: cache);
            if (load && rb2.FundId != null) rb2.Fund = FindFund2(rb2.FundId, cache: cache);
            if (load && rb2.FiscalYearId != null) rb2.FiscalYear = FindFiscalYear2(rb2.FiscalYearId, cache: cache);
            if (load && rb2.CreationUserId != null) rb2.CreationUser = FindUser2(rb2.CreationUserId, cache: cache);
            if (load && rb2.LastWriteUserId != null) rb2.LastWriteUser = FindUser2(rb2.LastWriteUserId, cache: cache);
            var i = 0;
            if (rb2.RolloverBudgetAcquisitionsUnit2s != null) foreach (var rbau2 in rb2.RolloverBudgetAcquisitionsUnit2s)
                {
                    rbau2.Id = (++i).ToString();
                    rbau2.RolloverBudgetId = rb2.Id;
                    rbau2.RolloverBudget = rb2;
                    if (load && rbau2.AcquisitionsUnitId != null) rbau2.AcquisitionsUnit = FindAcquisitionsUnit2(rbau2.AcquisitionsUnitId, cache: cache);
                }
            i = 0;
            if (rb2.RolloverBudgetAcquisitionsUnits != null) foreach (var rbau in rb2.RolloverBudgetAcquisitionsUnits)
                {
                    rbau.Id = (++i).ToString();
                    rbau.RolloverBudgetId = rb2.Id;
                    rbau.RolloverBudget = rb2;
                    if (load && rbau.AcquisitionsUnitId != null) rbau.AcquisitionsUnit = FindAcquisitionsUnit2(rbau.AcquisitionsUnitId, cache: cache);
                }
            i = 0;
            if (rb2.RolloverBudgetAllocatedFromNames != null) foreach (var rbafn in rb2.RolloverBudgetAllocatedFromNames)
                {
                    rbafn.Id = (++i).ToString();
                    rbafn.RolloverBudgetId = rb2.Id;
                    rbafn.RolloverBudget = rb2;
                }
            i = 0;
            if (rb2.RolloverBudgetAllocatedToNames != null) foreach (var rbatn in rb2.RolloverBudgetAllocatedToNames)
                {
                    rbatn.Id = (++i).ToString();
                    rbatn.RolloverBudgetId = rb2.Id;
                    rbatn.RolloverBudget = rb2;
                }
            i = 0;
            if (rb2.RolloverBudgetExpenseClassDetails != null) foreach (var rbecd in rb2.RolloverBudgetExpenseClassDetails)
                {
                    rbecd.Id = (++i).ToString();
                    rbecd.RolloverBudgetId = rb2.Id;
                    rbecd.RolloverBudget = rb2;
                }
            i = 0;
            if (rb2.RolloverBudgetFromFunds != null) foreach (var rbff in rb2.RolloverBudgetFromFunds)
                {
                    rbff.Id = (++i).ToString();
                    rbff.RolloverBudgetId = rb2.Id;
                    rbff.RolloverBudget = rb2;
                    if (load && rbff.FundId != null) rbff.Fund = FindFund2(rbff.FundId, cache: cache);
                }
            i = 0;
            if (rb2.RolloverBudgetLocations != null) foreach (var rbl in rb2.RolloverBudgetLocations)
                {
                    rbl.Id = (++i).ToString();
                    rbl.RolloverBudgetId = rb2.Id;
                    rbl.RolloverBudget = rb2;
                    if (load && rbl.LocationId != null) rbl.Location = FindLocation2(rbl.LocationId, cache: cache);
                }
            i = 0;
            if (rb2.RolloverBudgetOrganizations != null) foreach (var rbo in rb2.RolloverBudgetOrganizations)
                {
                    rbo.Id = (++i).ToString();
                    rbo.RolloverBudgetId = rb2.Id;
                    rbo.RolloverBudget = rb2;
                    if (load && rbo.OrganizationId != null) rbo.Organization = FindOrganization2(rbo.OrganizationId, cache: cache);
                }
            i = 0;
            if (rb2.RolloverBudgetTags != null) foreach (var rbt in rb2.RolloverBudgetTags)
                {
                    rbt.Id = (++i).ToString();
                    rbt.RolloverBudgetId = rb2.Id;
                    rbt.RolloverBudget = rb2;
                }
            i = 0;
            if (rb2.RolloverBudgetToFunds != null) foreach (var rbtf in rb2.RolloverBudgetToFunds)
                {
                    rbtf.Id = (++i).ToString();
                    rbtf.RolloverBudgetId = rb2.Id;
                    rbtf.RolloverBudget = rb2;
                    if (load && rbtf.FundId != null) rbtf.Fund = FindFund2(rbtf.FundId, cache: cache);
                }
            return rb2;
        }

        public void Insert(RolloverBudget2 rolloverBudget2)
        {
            if (rolloverBudget2.Id == null) rolloverBudget2.Id = Guid.NewGuid();
            FolioServiceClient.InsertRolloverBudget(rolloverBudget2.ToJObject());
        }

        public void Update(RolloverBudget2 rolloverBudget2) => FolioServiceClient.UpdateRolloverBudget(rolloverBudget2.ToJObject());

        public void UpdateOrInsert(RolloverBudget2 rolloverBudget2)
        {
            if (rolloverBudget2.Id == null)
                Insert(rolloverBudget2);
            else
                try
                {
                    Update(rolloverBudget2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(rolloverBudget2); else throw;
                }
        }

        public void InsertOrUpdate(RolloverBudget2 rolloverBudget2)
        {
            try
            {
                Insert(rolloverBudget2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(rolloverBudget2); else throw;
            }
        }

        public void DeleteRolloverBudget2(Guid? id) => FolioServiceClient.DeleteRolloverBudget(id?.ToString());

        public bool AnyRolloverError2s(string where = null) => FolioServiceClient.AnyRolloverErrors(where);

        public int CountRolloverError2s(string where = null) => FolioServiceClient.CountRolloverErrors(where);

        public RolloverError2[] RolloverError2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.RolloverErrors(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var re2 = cache ? (RolloverError2)(objects.ContainsKey(id) ? objects[id] : objects[id] = RolloverError2.FromJObject(jo)) : RolloverError2.FromJObject(jo);
                if (load && re2.RolloverId != null) re2.Rollover = FindRollover2(re2.RolloverId, cache: cache);
                if (load && re2.CreationUserId != null) re2.CreationUser = FindUser2(re2.CreationUserId, cache: cache);
                if (load && re2.LastWriteUserId != null) re2.LastWriteUser = FindUser2(re2.LastWriteUserId, cache: cache);
                return re2;
            }).ToArray();
        }

        public IEnumerable<RolloverError2> RolloverError2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.RolloverErrors(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var re2 = cache ? (RolloverError2)(objects.ContainsKey(id) ? objects[id] : objects[id] = RolloverError2.FromJObject(jo)) : RolloverError2.FromJObject(jo);
                if (load && re2.RolloverId != null) re2.Rollover = FindRollover2(re2.RolloverId, cache: cache);
                if (load && re2.CreationUserId != null) re2.CreationUser = FindUser2(re2.CreationUserId, cache: cache);
                if (load && re2.LastWriteUserId != null) re2.LastWriteUser = FindUser2(re2.LastWriteUserId, cache: cache);
                yield return re2;
            }
        }

        public RolloverError2 FindRolloverError2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var re2 = cache ? (RolloverError2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = RolloverError2.FromJObject(FolioServiceClient.GetRolloverError(id?.ToString()))) : RolloverError2.FromJObject(FolioServiceClient.GetRolloverError(id?.ToString()));
            if (re2 == null) return null;
            if (load && re2.RolloverId != null) re2.Rollover = FindRollover2(re2.RolloverId, cache: cache);
            if (load && re2.CreationUserId != null) re2.CreationUser = FindUser2(re2.CreationUserId, cache: cache);
            if (load && re2.LastWriteUserId != null) re2.LastWriteUser = FindUser2(re2.LastWriteUserId, cache: cache);
            return re2;
        }

        public void Insert(RolloverError2 rolloverError2)
        {
            if (rolloverError2.Id == null) rolloverError2.Id = Guid.NewGuid();
            FolioServiceClient.InsertRolloverError(rolloverError2.ToJObject());
        }

        public void Update(RolloverError2 rolloverError2) => FolioServiceClient.UpdateRolloverError(rolloverError2.ToJObject());

        public void UpdateOrInsert(RolloverError2 rolloverError2)
        {
            if (rolloverError2.Id == null)
                Insert(rolloverError2);
            else
                try
                {
                    Update(rolloverError2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(rolloverError2); else throw;
                }
        }

        public void InsertOrUpdate(RolloverError2 rolloverError2)
        {
            try
            {
                Insert(rolloverError2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(rolloverError2); else throw;
            }
        }

        public void DeleteRolloverError2(Guid? id) => FolioServiceClient.DeleteRolloverError(id?.ToString());

        public bool AnyRolloverProgress2s(string where = null) => FolioServiceClient.AnyRolloverProgresses(where);

        public int CountRolloverProgress2s(string where = null) => FolioServiceClient.CountRolloverProgresses(where);

        public RolloverProgress2[] RolloverProgress2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.RolloverProgresses(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var rp2 = cache ? (RolloverProgress2)(objects.ContainsKey(id) ? objects[id] : objects[id] = RolloverProgress2.FromJObject(jo)) : RolloverProgress2.FromJObject(jo);
                if (load && rp2.RolloverId != null) rp2.Rollover = FindRollover2(rp2.RolloverId, cache: cache);
                if (load && rp2.CreationUserId != null) rp2.CreationUser = FindUser2(rp2.CreationUserId, cache: cache);
                if (load && rp2.LastWriteUserId != null) rp2.LastWriteUser = FindUser2(rp2.LastWriteUserId, cache: cache);
                return rp2;
            }).ToArray();
        }

        public IEnumerable<RolloverProgress2> RolloverProgress2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.RolloverProgresses(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var rp2 = cache ? (RolloverProgress2)(objects.ContainsKey(id) ? objects[id] : objects[id] = RolloverProgress2.FromJObject(jo)) : RolloverProgress2.FromJObject(jo);
                if (load && rp2.RolloverId != null) rp2.Rollover = FindRollover2(rp2.RolloverId, cache: cache);
                if (load && rp2.CreationUserId != null) rp2.CreationUser = FindUser2(rp2.CreationUserId, cache: cache);
                if (load && rp2.LastWriteUserId != null) rp2.LastWriteUser = FindUser2(rp2.LastWriteUserId, cache: cache);
                yield return rp2;
            }
        }

        public RolloverProgress2 FindRolloverProgress2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var rp2 = cache ? (RolloverProgress2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = RolloverProgress2.FromJObject(FolioServiceClient.GetRolloverProgress(id?.ToString()))) : RolloverProgress2.FromJObject(FolioServiceClient.GetRolloverProgress(id?.ToString()));
            if (rp2 == null) return null;
            if (load && rp2.RolloverId != null) rp2.Rollover = FindRollover2(rp2.RolloverId, cache: cache);
            if (load && rp2.CreationUserId != null) rp2.CreationUser = FindUser2(rp2.CreationUserId, cache: cache);
            if (load && rp2.LastWriteUserId != null) rp2.LastWriteUser = FindUser2(rp2.LastWriteUserId, cache: cache);
            return rp2;
        }

        public void Insert(RolloverProgress2 rolloverProgress2)
        {
            if (rolloverProgress2.Id == null) rolloverProgress2.Id = Guid.NewGuid();
            FolioServiceClient.InsertRolloverProgress(rolloverProgress2.ToJObject());
        }

        public void Update(RolloverProgress2 rolloverProgress2) => FolioServiceClient.UpdateRolloverProgress(rolloverProgress2.ToJObject());

        public void UpdateOrInsert(RolloverProgress2 rolloverProgress2)
        {
            if (rolloverProgress2.Id == null)
                Insert(rolloverProgress2);
            else
                try
                {
                    Update(rolloverProgress2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(rolloverProgress2); else throw;
                }
        }

        public void InsertOrUpdate(RolloverProgress2 rolloverProgress2)
        {
            try
            {
                Insert(rolloverProgress2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(rolloverProgress2); else throw;
            }
        }

        public void DeleteRolloverProgress2(Guid? id) => FolioServiceClient.DeleteRolloverProgress(id?.ToString());

        public bool AnyScheduledNotice2s(string where = null) => FolioServiceClient.AnyScheduledNotices(where);

        public int CountScheduledNotice2s(string where = null) => FolioServiceClient.CountScheduledNotices(where);

        public ScheduledNotice2[] ScheduledNotice2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ScheduledNotices(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var sn2 = cache ? (ScheduledNotice2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ScheduledNotice2.FromJObject(jo)) : ScheduledNotice2.FromJObject(jo);
                if (load && sn2.LoanId != null) sn2.Loan = FindLoan2(sn2.LoanId, cache: cache);
                if (load && sn2.RequestId != null) sn2.Request = FindRequest2(sn2.RequestId, cache: cache);
                if (load && sn2.PaymentId != null) sn2.Payment = FindPayment2(sn2.PaymentId, cache: cache);
                if (load && sn2.RecipientUserId != null) sn2.RecipientUser = FindUser2(sn2.RecipientUserId, cache: cache);
                if (load && sn2.NoticeConfigTemplateId != null) sn2.NoticeConfigTemplate = FindTemplate2(sn2.NoticeConfigTemplateId, cache: cache);
                if (load && sn2.CreationUserId != null) sn2.CreationUser = FindUser2(sn2.CreationUserId, cache: cache);
                if (load && sn2.LastWriteUserId != null) sn2.LastWriteUser = FindUser2(sn2.LastWriteUserId, cache: cache);
                return sn2;
            }).ToArray();
        }

        public IEnumerable<ScheduledNotice2> ScheduledNotice2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ScheduledNotices(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var sn2 = cache ? (ScheduledNotice2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ScheduledNotice2.FromJObject(jo)) : ScheduledNotice2.FromJObject(jo);
                if (load && sn2.LoanId != null) sn2.Loan = FindLoan2(sn2.LoanId, cache: cache);
                if (load && sn2.RequestId != null) sn2.Request = FindRequest2(sn2.RequestId, cache: cache);
                if (load && sn2.PaymentId != null) sn2.Payment = FindPayment2(sn2.PaymentId, cache: cache);
                if (load && sn2.RecipientUserId != null) sn2.RecipientUser = FindUser2(sn2.RecipientUserId, cache: cache);
                if (load && sn2.NoticeConfigTemplateId != null) sn2.NoticeConfigTemplate = FindTemplate2(sn2.NoticeConfigTemplateId, cache: cache);
                if (load && sn2.CreationUserId != null) sn2.CreationUser = FindUser2(sn2.CreationUserId, cache: cache);
                if (load && sn2.LastWriteUserId != null) sn2.LastWriteUser = FindUser2(sn2.LastWriteUserId, cache: cache);
                yield return sn2;
            }
        }

        public ScheduledNotice2 FindScheduledNotice2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var sn2 = cache ? (ScheduledNotice2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = ScheduledNotice2.FromJObject(FolioServiceClient.GetScheduledNotice(id?.ToString()))) : ScheduledNotice2.FromJObject(FolioServiceClient.GetScheduledNotice(id?.ToString()));
            if (sn2 == null) return null;
            if (load && sn2.LoanId != null) sn2.Loan = FindLoan2(sn2.LoanId, cache: cache);
            if (load && sn2.RequestId != null) sn2.Request = FindRequest2(sn2.RequestId, cache: cache);
            if (load && sn2.PaymentId != null) sn2.Payment = FindPayment2(sn2.PaymentId, cache: cache);
            if (load && sn2.RecipientUserId != null) sn2.RecipientUser = FindUser2(sn2.RecipientUserId, cache: cache);
            if (load && sn2.NoticeConfigTemplateId != null) sn2.NoticeConfigTemplate = FindTemplate2(sn2.NoticeConfigTemplateId, cache: cache);
            if (load && sn2.CreationUserId != null) sn2.CreationUser = FindUser2(sn2.CreationUserId, cache: cache);
            if (load && sn2.LastWriteUserId != null) sn2.LastWriteUser = FindUser2(sn2.LastWriteUserId, cache: cache);
            return sn2;
        }

        public void Insert(ScheduledNotice2 scheduledNotice2)
        {
            if (scheduledNotice2.Id == null) scheduledNotice2.Id = Guid.NewGuid();
            FolioServiceClient.InsertScheduledNotice(scheduledNotice2.ToJObject());
        }

        public void Update(ScheduledNotice2 scheduledNotice2) => FolioServiceClient.UpdateScheduledNotice(scheduledNotice2.ToJObject());

        public void UpdateOrInsert(ScheduledNotice2 scheduledNotice2)
        {
            if (scheduledNotice2.Id == null)
                Insert(scheduledNotice2);
            else
                try
                {
                    Update(scheduledNotice2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(scheduledNotice2); else throw;
                }
        }

        public void InsertOrUpdate(ScheduledNotice2 scheduledNotice2)
        {
            try
            {
                Insert(scheduledNotice2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(scheduledNotice2); else throw;
            }
        }

        public void DeleteScheduledNotice2(Guid? id) => FolioServiceClient.DeleteScheduledNotice(id?.ToString());

        public bool AnyServicePoint2s(string where = null) => FolioServiceClient.AnyServicePoints(where);

        public int CountServicePoint2s(string where = null) => FolioServiceClient.CountServicePoints(where);

        public ServicePoint2[] ServicePoint2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ServicePoints(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var sp2 = cache ? (ServicePoint2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ServicePoint2.FromJObject(jo)) : ServicePoint2.FromJObject(jo);
                if (load && sp2.CreationUserId != null) sp2.CreationUser = FindUser2(sp2.CreationUserId, cache: cache);
                if (load && sp2.LastWriteUserId != null) sp2.LastWriteUser = FindUser2(sp2.LastWriteUserId, cache: cache);
                return sp2;
            }).ToArray();
        }

        public IEnumerable<ServicePoint2> ServicePoint2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ServicePoints(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var sp2 = cache ? (ServicePoint2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ServicePoint2.FromJObject(jo)) : ServicePoint2.FromJObject(jo);
                if (load && sp2.CreationUserId != null) sp2.CreationUser = FindUser2(sp2.CreationUserId, cache: cache);
                if (load && sp2.LastWriteUserId != null) sp2.LastWriteUser = FindUser2(sp2.LastWriteUserId, cache: cache);
                yield return sp2;
            }
        }

        public ServicePoint2 FindServicePoint2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var sp2 = cache ? (ServicePoint2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = ServicePoint2.FromJObject(FolioServiceClient.GetServicePoint(id?.ToString()))) : ServicePoint2.FromJObject(FolioServiceClient.GetServicePoint(id?.ToString()));
            if (sp2 == null) return null;
            if (load && sp2.CreationUserId != null) sp2.CreationUser = FindUser2(sp2.CreationUserId, cache: cache);
            if (load && sp2.LastWriteUserId != null) sp2.LastWriteUser = FindUser2(sp2.LastWriteUserId, cache: cache);
            var i = 0;
            if (sp2.ServicePointStaffSlips != null) foreach (var spss in sp2.ServicePointStaffSlips)
                {
                    spss.Id = (++i).ToString();
                    spss.ServicePointId = sp2.Id;
                    spss.ServicePoint = sp2;
                    if (load && spss.StaffSlipId != null) spss.StaffSlip = FindStaffSlip2(spss.StaffSlipId, cache: cache);
                }
            return sp2;
        }

        public void Insert(ServicePoint2 servicePoint2)
        {
            if (servicePoint2.Id == null) servicePoint2.Id = Guid.NewGuid();
            FolioServiceClient.InsertServicePoint(servicePoint2.ToJObject());
        }

        public void Update(ServicePoint2 servicePoint2) => FolioServiceClient.UpdateServicePoint(servicePoint2.ToJObject());

        public void UpdateOrInsert(ServicePoint2 servicePoint2)
        {
            if (servicePoint2.Id == null)
                Insert(servicePoint2);
            else
                try
                {
                    Update(servicePoint2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(servicePoint2); else throw;
                }
        }

        public void InsertOrUpdate(ServicePoint2 servicePoint2)
        {
            try
            {
                Insert(servicePoint2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(servicePoint2); else throw;
            }
        }

        public void DeleteServicePoint2(Guid? id) => FolioServiceClient.DeleteServicePoint(id?.ToString());

        public bool AnyServicePointUser2s(string where = null) => FolioServiceClient.AnyServicePointUsers(where);

        public int CountServicePointUser2s(string where = null) => FolioServiceClient.CountServicePointUsers(where);

        public ServicePointUser2[] ServicePointUser2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.ServicePointUsers(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var spu2 = cache ? (ServicePointUser2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ServicePointUser2.FromJObject(jo)) : ServicePointUser2.FromJObject(jo);
                if (load && spu2.UserId != null) spu2.User = FindUser2(spu2.UserId, cache: cache);
                if (load && spu2.DefaultServicePointId != null) spu2.DefaultServicePoint = FindServicePoint2(spu2.DefaultServicePointId, cache: cache);
                if (load && spu2.CreationUserId != null) spu2.CreationUser = FindUser2(spu2.CreationUserId, cache: cache);
                if (load && spu2.LastWriteUserId != null) spu2.LastWriteUser = FindUser2(spu2.LastWriteUserId, cache: cache);
                return spu2;
            }).ToArray();
        }

        public IEnumerable<ServicePointUser2> ServicePointUser2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.ServicePointUsers(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var spu2 = cache ? (ServicePointUser2)(objects.ContainsKey(id) ? objects[id] : objects[id] = ServicePointUser2.FromJObject(jo)) : ServicePointUser2.FromJObject(jo);
                if (load && spu2.UserId != null) spu2.User = FindUser2(spu2.UserId, cache: cache);
                if (load && spu2.DefaultServicePointId != null) spu2.DefaultServicePoint = FindServicePoint2(spu2.DefaultServicePointId, cache: cache);
                if (load && spu2.CreationUserId != null) spu2.CreationUser = FindUser2(spu2.CreationUserId, cache: cache);
                if (load && spu2.LastWriteUserId != null) spu2.LastWriteUser = FindUser2(spu2.LastWriteUserId, cache: cache);
                yield return spu2;
            }
        }

        public ServicePointUser2 FindServicePointUser2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var spu2 = cache ? (ServicePointUser2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = ServicePointUser2.FromJObject(FolioServiceClient.GetServicePointUser(id?.ToString()))) : ServicePointUser2.FromJObject(FolioServiceClient.GetServicePointUser(id?.ToString()));
            if (spu2 == null) return null;
            if (load && spu2.UserId != null) spu2.User = FindUser2(spu2.UserId, cache: cache);
            if (load && spu2.DefaultServicePointId != null) spu2.DefaultServicePoint = FindServicePoint2(spu2.DefaultServicePointId, cache: cache);
            if (load && spu2.CreationUserId != null) spu2.CreationUser = FindUser2(spu2.CreationUserId, cache: cache);
            if (load && spu2.LastWriteUserId != null) spu2.LastWriteUser = FindUser2(spu2.LastWriteUserId, cache: cache);
            var i = 0;
            if (spu2.ServicePointUserServicePoints != null) foreach (var spusp in spu2.ServicePointUserServicePoints)
                {
                    spusp.Id = (++i).ToString();
                    spusp.ServicePointUserId = spu2.Id;
                    spusp.ServicePointUser = spu2;
                    if (load && spusp.ServicePointId != null) spusp.ServicePoint = FindServicePoint2(spusp.ServicePointId, cache: cache);
                }
            return spu2;
        }

        public void Insert(ServicePointUser2 servicePointUser2)
        {
            if (servicePointUser2.Id == null) servicePointUser2.Id = Guid.NewGuid();
            FolioServiceClient.InsertServicePointUser(servicePointUser2.ToJObject());
        }

        public void Update(ServicePointUser2 servicePointUser2) => FolioServiceClient.UpdateServicePointUser(servicePointUser2.ToJObject());

        public void UpdateOrInsert(ServicePointUser2 servicePointUser2)
        {
            if (servicePointUser2.Id == null)
                Insert(servicePointUser2);
            else
                try
                {
                    Update(servicePointUser2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(servicePointUser2); else throw;
                }
        }

        public void InsertOrUpdate(ServicePointUser2 servicePointUser2)
        {
            try
            {
                Insert(servicePointUser2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(servicePointUser2); else throw;
            }
        }

        public void DeleteServicePointUser2(Guid? id) => FolioServiceClient.DeleteServicePointUser(id?.ToString());

        public bool AnySettings(string where = null) => FolioServiceClient.AnySettings(where);

        public int CountSettings(string where = null) => FolioServiceClient.CountSettings(where);

        public Setting[] Settings(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Settings(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var s = cache ? (Setting)(objects.ContainsKey(id) ? objects[id] : objects[id] = Setting.FromJObject(jo)) : Setting.FromJObject(jo);
                if (load && s.CreationUserId != null) s.CreationUser = FindUser2(s.CreationUserId, cache: cache);
                if (load && s.LastWriteUserId != null) s.LastWriteUser = FindUser2(s.LastWriteUserId, cache: cache);
                return s;
            }).ToArray();
        }

        public IEnumerable<Setting> Settings(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Settings(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var s = cache ? (Setting)(objects.ContainsKey(id) ? objects[id] : objects[id] = Setting.FromJObject(jo)) : Setting.FromJObject(jo);
                if (load && s.CreationUserId != null) s.CreationUser = FindUser2(s.CreationUserId, cache: cache);
                if (load && s.LastWriteUserId != null) s.LastWriteUser = FindUser2(s.LastWriteUserId, cache: cache);
                yield return s;
            }
        }

        public Setting FindSetting(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var s = cache ? (Setting)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Setting.FromJObject(FolioServiceClient.GetSetting(id?.ToString()))) : Setting.FromJObject(FolioServiceClient.GetSetting(id?.ToString()));
            if (s == null) return null;
            if (load && s.CreationUserId != null) s.CreationUser = FindUser2(s.CreationUserId, cache: cache);
            if (load && s.LastWriteUserId != null) s.LastWriteUser = FindUser2(s.LastWriteUserId, cache: cache);
            return s;
        }

        public void Insert(Setting setting)
        {
            if (setting.Id == null) setting.Id = Guid.NewGuid();
            FolioServiceClient.InsertSetting(setting.ToJObject());
        }

        public void Update(Setting setting) => FolioServiceClient.UpdateSetting(setting.ToJObject());

        public void UpdateOrInsert(Setting setting)
        {
            if (setting.Id == null)
                Insert(setting);
            else
                try
                {
                    Update(setting);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(setting); else throw;
                }
        }

        public void InsertOrUpdate(Setting setting)
        {
            try
            {
                Insert(setting);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(setting); else throw;
            }
        }

        public void DeleteSetting(Guid? id) => FolioServiceClient.DeleteSetting(id?.ToString());

        public bool AnySnapshot2s(string where = null) => FolioServiceClient.AnySnapshots(where);

        public int CountSnapshot2s(string where = null) => FolioServiceClient.CountSnapshots(where);

        public Snapshot2[] Snapshot2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Snapshots(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var s2 = cache ? (Snapshot2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Snapshot2.FromJObject(jo)) : Snapshot2.FromJObject(jo);
                if (load && s2.CreationUserId != null) s2.CreationUser = FindUser2(s2.CreationUserId, cache: cache);
                if (load && s2.LastWriteUserId != null) s2.LastWriteUser = FindUser2(s2.LastWriteUserId, cache: cache);
                return s2;
            }).ToArray();
        }

        public IEnumerable<Snapshot2> Snapshot2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Snapshots(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var s2 = cache ? (Snapshot2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Snapshot2.FromJObject(jo)) : Snapshot2.FromJObject(jo);
                if (load && s2.CreationUserId != null) s2.CreationUser = FindUser2(s2.CreationUserId, cache: cache);
                if (load && s2.LastWriteUserId != null) s2.LastWriteUser = FindUser2(s2.LastWriteUserId, cache: cache);
                yield return s2;
            }
        }

        public Snapshot2 FindSnapshot2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var s2 = cache ? (Snapshot2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Snapshot2.FromJObject(FolioServiceClient.GetSnapshot(id?.ToString()))) : Snapshot2.FromJObject(FolioServiceClient.GetSnapshot(id?.ToString()));
            if (s2 == null) return null;
            if (load && s2.CreationUserId != null) s2.CreationUser = FindUser2(s2.CreationUserId, cache: cache);
            if (load && s2.LastWriteUserId != null) s2.LastWriteUser = FindUser2(s2.LastWriteUserId, cache: cache);
            return s2;
        }

        public void Insert(Snapshot2 snapshot2)
        {
            if (snapshot2.Id == null) snapshot2.Id = Guid.NewGuid();
            FolioServiceClient.InsertSnapshot(snapshot2.ToJObject());
        }

        public void Update(Snapshot2 snapshot2) => FolioServiceClient.UpdateSnapshot(snapshot2.ToJObject());

        public void UpdateOrInsert(Snapshot2 snapshot2)
        {
            if (snapshot2.Id == null)
                Insert(snapshot2);
            else
                try
                {
                    Update(snapshot2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(snapshot2); else throw;
                }
        }

        public void InsertOrUpdate(Snapshot2 snapshot2)
        {
            try
            {
                Insert(snapshot2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(snapshot2); else throw;
            }
        }

        public void DeleteSnapshot2(Guid? id) => FolioServiceClient.DeleteSnapshot(id?.ToString());

        public bool AnySource2s(string where = null) => FolioServiceClient.AnySources(where);

        public int CountSource2s(string where = null) => FolioServiceClient.CountSources(where);

        public Source2[] Source2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Sources(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var s2 = cache ? (Source2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Source2.FromJObject(jo)) : Source2.FromJObject(jo);
                if (load && s2.CreationUserId != null) s2.CreationUser = FindUser2(s2.CreationUserId, cache: cache);
                if (load && s2.LastWriteUserId != null) s2.LastWriteUser = FindUser2(s2.LastWriteUserId, cache: cache);
                return s2;
            }).ToArray();
        }

        public IEnumerable<Source2> Source2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Sources(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var s2 = cache ? (Source2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Source2.FromJObject(jo)) : Source2.FromJObject(jo);
                if (load && s2.CreationUserId != null) s2.CreationUser = FindUser2(s2.CreationUserId, cache: cache);
                if (load && s2.LastWriteUserId != null) s2.LastWriteUser = FindUser2(s2.LastWriteUserId, cache: cache);
                yield return s2;
            }
        }

        public Source2 FindSource2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var s2 = cache ? (Source2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Source2.FromJObject(FolioServiceClient.GetSource(id?.ToString()))) : Source2.FromJObject(FolioServiceClient.GetSource(id?.ToString()));
            if (s2 == null) return null;
            if (load && s2.CreationUserId != null) s2.CreationUser = FindUser2(s2.CreationUserId, cache: cache);
            if (load && s2.LastWriteUserId != null) s2.LastWriteUser = FindUser2(s2.LastWriteUserId, cache: cache);
            return s2;
        }

        public void Insert(Source2 source2)
        {
            if (source2.Id == null) source2.Id = Guid.NewGuid();
            FolioServiceClient.InsertSource(source2.ToJObject());
        }

        public void Update(Source2 source2) => FolioServiceClient.UpdateSource(source2.ToJObject());

        public void UpdateOrInsert(Source2 source2)
        {
            if (source2.Id == null)
                Insert(source2);
            else
                try
                {
                    Update(source2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(source2); else throw;
                }
        }

        public void InsertOrUpdate(Source2 source2)
        {
            try
            {
                Insert(source2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(source2); else throw;
            }
        }

        public void DeleteSource2(Guid? id) => FolioServiceClient.DeleteSource(id?.ToString());

        public bool AnyStaffSlip2s(string where = null) => FolioServiceClient.AnyStaffSlips(where);

        public int CountStaffSlip2s(string where = null) => FolioServiceClient.CountStaffSlips(where);

        public StaffSlip2[] StaffSlip2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.StaffSlips(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var ss2 = cache ? (StaffSlip2)(objects.ContainsKey(id) ? objects[id] : objects[id] = StaffSlip2.FromJObject(jo)) : StaffSlip2.FromJObject(jo);
                if (load && ss2.CreationUserId != null) ss2.CreationUser = FindUser2(ss2.CreationUserId, cache: cache);
                if (load && ss2.LastWriteUserId != null) ss2.LastWriteUser = FindUser2(ss2.LastWriteUserId, cache: cache);
                return ss2;
            }).ToArray();
        }

        public IEnumerable<StaffSlip2> StaffSlip2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.StaffSlips(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var ss2 = cache ? (StaffSlip2)(objects.ContainsKey(id) ? objects[id] : objects[id] = StaffSlip2.FromJObject(jo)) : StaffSlip2.FromJObject(jo);
                if (load && ss2.CreationUserId != null) ss2.CreationUser = FindUser2(ss2.CreationUserId, cache: cache);
                if (load && ss2.LastWriteUserId != null) ss2.LastWriteUser = FindUser2(ss2.LastWriteUserId, cache: cache);
                yield return ss2;
            }
        }

        public StaffSlip2 FindStaffSlip2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var ss2 = cache ? (StaffSlip2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = StaffSlip2.FromJObject(FolioServiceClient.GetStaffSlip(id?.ToString()))) : StaffSlip2.FromJObject(FolioServiceClient.GetStaffSlip(id?.ToString()));
            if (ss2 == null) return null;
            if (load && ss2.CreationUserId != null) ss2.CreationUser = FindUser2(ss2.CreationUserId, cache: cache);
            if (load && ss2.LastWriteUserId != null) ss2.LastWriteUser = FindUser2(ss2.LastWriteUserId, cache: cache);
            return ss2;
        }

        public void Insert(StaffSlip2 staffSlip2)
        {
            if (staffSlip2.Id == null) staffSlip2.Id = Guid.NewGuid();
            FolioServiceClient.InsertStaffSlip(staffSlip2.ToJObject());
        }

        public void Update(StaffSlip2 staffSlip2) => FolioServiceClient.UpdateStaffSlip(staffSlip2.ToJObject());

        public void UpdateOrInsert(StaffSlip2 staffSlip2)
        {
            if (staffSlip2.Id == null)
                Insert(staffSlip2);
            else
                try
                {
                    Update(staffSlip2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(staffSlip2); else throw;
                }
        }

        public void InsertOrUpdate(StaffSlip2 staffSlip2)
        {
            try
            {
                Insert(staffSlip2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(staffSlip2); else throw;
            }
        }

        public void DeleteStaffSlip2(Guid? id) => FolioServiceClient.DeleteStaffSlip(id?.ToString());

        public bool AnyStatisticalCode2s(string where = null) => FolioServiceClient.AnyStatisticalCodes(where);

        public int CountStatisticalCode2s(string where = null) => FolioServiceClient.CountStatisticalCodes(where);

        public StatisticalCode2[] StatisticalCode2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.StatisticalCodes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var sc2 = cache ? (StatisticalCode2)(objects.ContainsKey(id) ? objects[id] : objects[id] = StatisticalCode2.FromJObject(jo)) : StatisticalCode2.FromJObject(jo);
                if (load && sc2.StatisticalCodeTypeId != null) sc2.StatisticalCodeType = FindStatisticalCodeType2(sc2.StatisticalCodeTypeId, cache: cache);
                if (load && sc2.CreationUserId != null) sc2.CreationUser = FindUser2(sc2.CreationUserId, cache: cache);
                if (load && sc2.LastWriteUserId != null) sc2.LastWriteUser = FindUser2(sc2.LastWriteUserId, cache: cache);
                return sc2;
            }).ToArray();
        }

        public IEnumerable<StatisticalCode2> StatisticalCode2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.StatisticalCodes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var sc2 = cache ? (StatisticalCode2)(objects.ContainsKey(id) ? objects[id] : objects[id] = StatisticalCode2.FromJObject(jo)) : StatisticalCode2.FromJObject(jo);
                if (load && sc2.StatisticalCodeTypeId != null) sc2.StatisticalCodeType = FindStatisticalCodeType2(sc2.StatisticalCodeTypeId, cache: cache);
                if (load && sc2.CreationUserId != null) sc2.CreationUser = FindUser2(sc2.CreationUserId, cache: cache);
                if (load && sc2.LastWriteUserId != null) sc2.LastWriteUser = FindUser2(sc2.LastWriteUserId, cache: cache);
                yield return sc2;
            }
        }

        public StatisticalCode2 FindStatisticalCode2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var sc2 = cache ? (StatisticalCode2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = StatisticalCode2.FromJObject(FolioServiceClient.GetStatisticalCode(id?.ToString()))) : StatisticalCode2.FromJObject(FolioServiceClient.GetStatisticalCode(id?.ToString()));
            if (sc2 == null) return null;
            if (load && sc2.StatisticalCodeTypeId != null) sc2.StatisticalCodeType = FindStatisticalCodeType2(sc2.StatisticalCodeTypeId, cache: cache);
            if (load && sc2.CreationUserId != null) sc2.CreationUser = FindUser2(sc2.CreationUserId, cache: cache);
            if (load && sc2.LastWriteUserId != null) sc2.LastWriteUser = FindUser2(sc2.LastWriteUserId, cache: cache);
            return sc2;
        }

        public void Insert(StatisticalCode2 statisticalCode2)
        {
            if (statisticalCode2.Id == null) statisticalCode2.Id = Guid.NewGuid();
            FolioServiceClient.InsertStatisticalCode(statisticalCode2.ToJObject());
        }

        public void Update(StatisticalCode2 statisticalCode2) => FolioServiceClient.UpdateStatisticalCode(statisticalCode2.ToJObject());

        public void UpdateOrInsert(StatisticalCode2 statisticalCode2)
        {
            if (statisticalCode2.Id == null)
                Insert(statisticalCode2);
            else
                try
                {
                    Update(statisticalCode2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(statisticalCode2); else throw;
                }
        }

        public void InsertOrUpdate(StatisticalCode2 statisticalCode2)
        {
            try
            {
                Insert(statisticalCode2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(statisticalCode2); else throw;
            }
        }

        public void DeleteStatisticalCode2(Guid? id) => FolioServiceClient.DeleteStatisticalCode(id?.ToString());

        public bool AnyStatisticalCodeType2s(string where = null) => FolioServiceClient.AnyStatisticalCodeTypes(where);

        public int CountStatisticalCodeType2s(string where = null) => FolioServiceClient.CountStatisticalCodeTypes(where);

        public StatisticalCodeType2[] StatisticalCodeType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.StatisticalCodeTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var sct2 = cache ? (StatisticalCodeType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = StatisticalCodeType2.FromJObject(jo)) : StatisticalCodeType2.FromJObject(jo);
                if (load && sct2.CreationUserId != null) sct2.CreationUser = FindUser2(sct2.CreationUserId, cache: cache);
                if (load && sct2.LastWriteUserId != null) sct2.LastWriteUser = FindUser2(sct2.LastWriteUserId, cache: cache);
                return sct2;
            }).ToArray();
        }

        public IEnumerable<StatisticalCodeType2> StatisticalCodeType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.StatisticalCodeTypes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var sct2 = cache ? (StatisticalCodeType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = StatisticalCodeType2.FromJObject(jo)) : StatisticalCodeType2.FromJObject(jo);
                if (load && sct2.CreationUserId != null) sct2.CreationUser = FindUser2(sct2.CreationUserId, cache: cache);
                if (load && sct2.LastWriteUserId != null) sct2.LastWriteUser = FindUser2(sct2.LastWriteUserId, cache: cache);
                yield return sct2;
            }
        }

        public StatisticalCodeType2 FindStatisticalCodeType2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var sct2 = cache ? (StatisticalCodeType2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = StatisticalCodeType2.FromJObject(FolioServiceClient.GetStatisticalCodeType(id?.ToString()))) : StatisticalCodeType2.FromJObject(FolioServiceClient.GetStatisticalCodeType(id?.ToString()));
            if (sct2 == null) return null;
            if (load && sct2.CreationUserId != null) sct2.CreationUser = FindUser2(sct2.CreationUserId, cache: cache);
            if (load && sct2.LastWriteUserId != null) sct2.LastWriteUser = FindUser2(sct2.LastWriteUserId, cache: cache);
            return sct2;
        }

        public void Insert(StatisticalCodeType2 statisticalCodeType2)
        {
            if (statisticalCodeType2.Id == null) statisticalCodeType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertStatisticalCodeType(statisticalCodeType2.ToJObject());
        }

        public void Update(StatisticalCodeType2 statisticalCodeType2) => FolioServiceClient.UpdateStatisticalCodeType(statisticalCodeType2.ToJObject());

        public void UpdateOrInsert(StatisticalCodeType2 statisticalCodeType2)
        {
            if (statisticalCodeType2.Id == null)
                Insert(statisticalCodeType2);
            else
                try
                {
                    Update(statisticalCodeType2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(statisticalCodeType2); else throw;
                }
        }

        public void InsertOrUpdate(StatisticalCodeType2 statisticalCodeType2)
        {
            try
            {
                Insert(statisticalCodeType2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(statisticalCodeType2); else throw;
            }
        }

        public void DeleteStatisticalCodeType2(Guid? id) => FolioServiceClient.DeleteStatisticalCodeType(id?.ToString());

        public bool AnyStatuses(string where = null) => FolioServiceClient.AnyInstanceStatuses(where);

        public int CountStatuses(string where = null) => FolioServiceClient.CountInstanceStatuses(where);

        public Status[] Statuses(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.InstanceStatuses(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var s = cache ? (Status)(objects.ContainsKey(id) ? objects[id] : objects[id] = Status.FromJObject(jo)) : Status.FromJObject(jo);
                if (load && s.CreationUserId != null) s.CreationUser = FindUser2(s.CreationUserId, cache: cache);
                if (load && s.LastWriteUserId != null) s.LastWriteUser = FindUser2(s.LastWriteUserId, cache: cache);
                return s;
            }).ToArray();
        }

        public IEnumerable<Status> Statuses(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.InstanceStatuses(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var s = cache ? (Status)(objects.ContainsKey(id) ? objects[id] : objects[id] = Status.FromJObject(jo)) : Status.FromJObject(jo);
                if (load && s.CreationUserId != null) s.CreationUser = FindUser2(s.CreationUserId, cache: cache);
                if (load && s.LastWriteUserId != null) s.LastWriteUser = FindUser2(s.LastWriteUserId, cache: cache);
                yield return s;
            }
        }

        public Status FindStatus(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var s = cache ? (Status)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Status.FromJObject(FolioServiceClient.GetInstanceStatus(id?.ToString()))) : Status.FromJObject(FolioServiceClient.GetInstanceStatus(id?.ToString()));
            if (s == null) return null;
            if (load && s.CreationUserId != null) s.CreationUser = FindUser2(s.CreationUserId, cache: cache);
            if (load && s.LastWriteUserId != null) s.LastWriteUser = FindUser2(s.LastWriteUserId, cache: cache);
            return s;
        }

        public void Insert(Status status)
        {
            if (status.Id == null) status.Id = Guid.NewGuid();
            FolioServiceClient.InsertInstanceStatus(status.ToJObject());
        }

        public void Update(Status status) => FolioServiceClient.UpdateInstanceStatus(status.ToJObject());

        public void UpdateOrInsert(Status status)
        {
            if (status.Id == null)
                Insert(status);
            else
                try
                {
                    Update(status);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(status); else throw;
                }
        }

        public void InsertOrUpdate(Status status)
        {
            try
            {
                Insert(status);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(status); else throw;
            }
        }

        public void DeleteStatus(Guid? id) => FolioServiceClient.DeleteInstanceStatus(id?.ToString());

        public bool AnySubjectSource2s(string where = null) => FolioServiceClient.AnySubjectSources(where);

        public int CountSubjectSource2s(string where = null) => FolioServiceClient.CountSubjectSources(where);

        public SubjectSource2[] SubjectSource2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.SubjectSources(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var ss2 = cache ? (SubjectSource2)(objects.ContainsKey(id) ? objects[id] : objects[id] = SubjectSource2.FromJObject(jo)) : SubjectSource2.FromJObject(jo);
                if (load && ss2.CreationUserId != null) ss2.CreationUser = FindUser2(ss2.CreationUserId, cache: cache);
                if (load && ss2.LastWriteUserId != null) ss2.LastWriteUser = FindUser2(ss2.LastWriteUserId, cache: cache);
                return ss2;
            }).ToArray();
        }

        public IEnumerable<SubjectSource2> SubjectSource2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.SubjectSources(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var ss2 = cache ? (SubjectSource2)(objects.ContainsKey(id) ? objects[id] : objects[id] = SubjectSource2.FromJObject(jo)) : SubjectSource2.FromJObject(jo);
                if (load && ss2.CreationUserId != null) ss2.CreationUser = FindUser2(ss2.CreationUserId, cache: cache);
                if (load && ss2.LastWriteUserId != null) ss2.LastWriteUser = FindUser2(ss2.LastWriteUserId, cache: cache);
                yield return ss2;
            }
        }

        public SubjectSource2 FindSubjectSource2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var ss2 = cache ? (SubjectSource2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = SubjectSource2.FromJObject(FolioServiceClient.GetSubjectSource(id?.ToString()))) : SubjectSource2.FromJObject(FolioServiceClient.GetSubjectSource(id?.ToString()));
            if (ss2 == null) return null;
            if (load && ss2.CreationUserId != null) ss2.CreationUser = FindUser2(ss2.CreationUserId, cache: cache);
            if (load && ss2.LastWriteUserId != null) ss2.LastWriteUser = FindUser2(ss2.LastWriteUserId, cache: cache);
            return ss2;
        }

        public void Insert(SubjectSource2 subjectSource2)
        {
            if (subjectSource2.Id == null) subjectSource2.Id = Guid.NewGuid();
            FolioServiceClient.InsertSubjectSource(subjectSource2.ToJObject());
        }

        public void Update(SubjectSource2 subjectSource2) => FolioServiceClient.UpdateSubjectSource(subjectSource2.ToJObject());

        public void UpdateOrInsert(SubjectSource2 subjectSource2)
        {
            if (subjectSource2.Id == null)
                Insert(subjectSource2);
            else
                try
                {
                    Update(subjectSource2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(subjectSource2); else throw;
                }
        }

        public void InsertOrUpdate(SubjectSource2 subjectSource2)
        {
            try
            {
                Insert(subjectSource2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(subjectSource2); else throw;
            }
        }

        public void DeleteSubjectSource2(Guid? id) => FolioServiceClient.DeleteSubjectSource(id?.ToString());

        public bool AnySubjectType2s(string where = null) => FolioServiceClient.AnySubjectTypes(where);

        public int CountSubjectType2s(string where = null) => FolioServiceClient.CountSubjectTypes(where);

        public SubjectType2[] SubjectType2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.SubjectTypes(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var st2 = cache ? (SubjectType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = SubjectType2.FromJObject(jo)) : SubjectType2.FromJObject(jo);
                if (load && st2.CreationUserId != null) st2.CreationUser = FindUser2(st2.CreationUserId, cache: cache);
                if (load && st2.LastWriteUserId != null) st2.LastWriteUser = FindUser2(st2.LastWriteUserId, cache: cache);
                return st2;
            }).ToArray();
        }

        public IEnumerable<SubjectType2> SubjectType2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.SubjectTypes(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var st2 = cache ? (SubjectType2)(objects.ContainsKey(id) ? objects[id] : objects[id] = SubjectType2.FromJObject(jo)) : SubjectType2.FromJObject(jo);
                if (load && st2.CreationUserId != null) st2.CreationUser = FindUser2(st2.CreationUserId, cache: cache);
                if (load && st2.LastWriteUserId != null) st2.LastWriteUser = FindUser2(st2.LastWriteUserId, cache: cache);
                yield return st2;
            }
        }

        public SubjectType2 FindSubjectType2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var st2 = cache ? (SubjectType2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = SubjectType2.FromJObject(FolioServiceClient.GetSubjectType(id?.ToString()))) : SubjectType2.FromJObject(FolioServiceClient.GetSubjectType(id?.ToString()));
            if (st2 == null) return null;
            if (load && st2.CreationUserId != null) st2.CreationUser = FindUser2(st2.CreationUserId, cache: cache);
            if (load && st2.LastWriteUserId != null) st2.LastWriteUser = FindUser2(st2.LastWriteUserId, cache: cache);
            return st2;
        }

        public void Insert(SubjectType2 subjectType2)
        {
            if (subjectType2.Id == null) subjectType2.Id = Guid.NewGuid();
            FolioServiceClient.InsertSubjectType(subjectType2.ToJObject());
        }

        public void Update(SubjectType2 subjectType2) => FolioServiceClient.UpdateSubjectType(subjectType2.ToJObject());

        public void UpdateOrInsert(SubjectType2 subjectType2)
        {
            if (subjectType2.Id == null)
                Insert(subjectType2);
            else
                try
                {
                    Update(subjectType2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(subjectType2); else throw;
                }
        }

        public void InsertOrUpdate(SubjectType2 subjectType2)
        {
            try
            {
                Insert(subjectType2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(subjectType2); else throw;
            }
        }

        public void DeleteSubjectType2(Guid? id) => FolioServiceClient.DeleteSubjectType(id?.ToString());

        public bool AnyTag2s(string where = null) => FolioServiceClient.AnyTags(where);

        public int CountTag2s(string where = null) => FolioServiceClient.CountTags(where);

        public Tag2[] Tag2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Tags(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var t2 = cache ? (Tag2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Tag2.FromJObject(jo)) : Tag2.FromJObject(jo);
                if (load && t2.CreationUserId != null) t2.CreationUser = FindUser2(t2.CreationUserId, cache: cache);
                if (load && t2.LastWriteUserId != null) t2.LastWriteUser = FindUser2(t2.LastWriteUserId, cache: cache);
                return t2;
            }).ToArray();
        }

        public IEnumerable<Tag2> Tag2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Tags(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var t2 = cache ? (Tag2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Tag2.FromJObject(jo)) : Tag2.FromJObject(jo);
                if (load && t2.CreationUserId != null) t2.CreationUser = FindUser2(t2.CreationUserId, cache: cache);
                if (load && t2.LastWriteUserId != null) t2.LastWriteUser = FindUser2(t2.LastWriteUserId, cache: cache);
                yield return t2;
            }
        }

        public Tag2 FindTag2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var t2 = cache ? (Tag2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Tag2.FromJObject(FolioServiceClient.GetTag(id?.ToString()))) : Tag2.FromJObject(FolioServiceClient.GetTag(id?.ToString()));
            if (t2 == null) return null;
            if (load && t2.CreationUserId != null) t2.CreationUser = FindUser2(t2.CreationUserId, cache: cache);
            if (load && t2.LastWriteUserId != null) t2.LastWriteUser = FindUser2(t2.LastWriteUserId, cache: cache);
            return t2;
        }

        public void Insert(Tag2 tag2)
        {
            if (tag2.Id == null) tag2.Id = Guid.NewGuid();
            FolioServiceClient.InsertTag(tag2.ToJObject());
        }

        public void Update(Tag2 tag2) => FolioServiceClient.UpdateTag(tag2.ToJObject());

        public void UpdateOrInsert(Tag2 tag2)
        {
            if (tag2.Id == null)
                Insert(tag2);
            else
                try
                {
                    Update(tag2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(tag2); else throw;
                }
        }

        public void InsertOrUpdate(Tag2 tag2)
        {
            try
            {
                Insert(tag2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(tag2); else throw;
            }
        }

        public void DeleteTag2(Guid? id) => FolioServiceClient.DeleteTag(id?.ToString());

        public bool AnyTemplate2s(string where = null) => FolioServiceClient.AnyTemplates(where);

        public int CountTemplate2s(string where = null) => FolioServiceClient.CountTemplates(where);

        public Template2[] Template2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Templates(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var t2 = cache ? (Template2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Template2.FromJObject(jo)) : Template2.FromJObject(jo);
                if (load && t2.CreationUserId != null) t2.CreationUser = FindUser2(t2.CreationUserId, cache: cache);
                if (load && t2.LastWriteUserId != null) t2.LastWriteUser = FindUser2(t2.LastWriteUserId, cache: cache);
                return t2;
            }).ToArray();
        }

        public IEnumerable<Template2> Template2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Templates(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var t2 = cache ? (Template2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Template2.FromJObject(jo)) : Template2.FromJObject(jo);
                if (load && t2.CreationUserId != null) t2.CreationUser = FindUser2(t2.CreationUserId, cache: cache);
                if (load && t2.LastWriteUserId != null) t2.LastWriteUser = FindUser2(t2.LastWriteUserId, cache: cache);
                yield return t2;
            }
        }

        public Template2 FindTemplate2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var t2 = cache ? (Template2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Template2.FromJObject(FolioServiceClient.GetTemplate(id?.ToString()))) : Template2.FromJObject(FolioServiceClient.GetTemplate(id?.ToString()));
            if (t2 == null) return null;
            if (load && t2.CreationUserId != null) t2.CreationUser = FindUser2(t2.CreationUserId, cache: cache);
            if (load && t2.LastWriteUserId != null) t2.LastWriteUser = FindUser2(t2.LastWriteUserId, cache: cache);
            var i = 0;
            if (t2.TemplateOutputFormats != null) foreach (var tof in t2.TemplateOutputFormats)
                {
                    tof.Id = (++i).ToString();
                    tof.TemplateId = t2.Id;
                    tof.Template = t2;
                }
            return t2;
        }

        public void Insert(Template2 template2)
        {
            if (template2.Id == null) template2.Id = Guid.NewGuid();
            FolioServiceClient.InsertTemplate(template2.ToJObject());
        }

        public void Update(Template2 template2) => FolioServiceClient.UpdateTemplate(template2.ToJObject());

        public void UpdateOrInsert(Template2 template2)
        {
            if (template2.Id == null)
                Insert(template2);
            else
                try
                {
                    Update(template2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(template2); else throw;
                }
        }

        public void InsertOrUpdate(Template2 template2)
        {
            try
            {
                Insert(template2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(template2); else throw;
            }
        }

        public void DeleteTemplate2(Guid? id) => FolioServiceClient.DeleteTemplate(id?.ToString());

        public bool AnyTitle2s(string where = null) => FolioServiceClient.AnyTitles(where);

        public int CountTitle2s(string where = null) => FolioServiceClient.CountTitles(where);

        public Title2[] Title2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Titles(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var t2 = cache ? (Title2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Title2.FromJObject(jo)) : Title2.FromJObject(jo);
                if (load && t2.OrderItemId != null) t2.OrderItem = FindOrderItem2(t2.OrderItemId, cache: cache);
                if (load && t2.InstanceId != null) t2.Instance = FindInstance2(t2.InstanceId, cache: cache);
                if (load && t2.CreationUserId != null) t2.CreationUser = FindUser2(t2.CreationUserId, cache: cache);
                if (load && t2.LastWriteUserId != null) t2.LastWriteUser = FindUser2(t2.LastWriteUserId, cache: cache);
                return t2;
            }).ToArray();
        }

        public IEnumerable<Title2> Title2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Titles(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var t2 = cache ? (Title2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Title2.FromJObject(jo)) : Title2.FromJObject(jo);
                if (load && t2.OrderItemId != null) t2.OrderItem = FindOrderItem2(t2.OrderItemId, cache: cache);
                if (load && t2.InstanceId != null) t2.Instance = FindInstance2(t2.InstanceId, cache: cache);
                if (load && t2.CreationUserId != null) t2.CreationUser = FindUser2(t2.CreationUserId, cache: cache);
                if (load && t2.LastWriteUserId != null) t2.LastWriteUser = FindUser2(t2.LastWriteUserId, cache: cache);
                yield return t2;
            }
        }

        public Title2 FindTitle2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var t2 = cache ? (Title2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Title2.FromJObject(FolioServiceClient.GetTitle(id?.ToString()))) : Title2.FromJObject(FolioServiceClient.GetTitle(id?.ToString()));
            if (t2 == null) return null;
            if (load && t2.OrderItemId != null) t2.OrderItem = FindOrderItem2(t2.OrderItemId, cache: cache);
            if (load && t2.InstanceId != null) t2.Instance = FindInstance2(t2.InstanceId, cache: cache);
            if (load && t2.CreationUserId != null) t2.CreationUser = FindUser2(t2.CreationUserId, cache: cache);
            if (load && t2.LastWriteUserId != null) t2.LastWriteUser = FindUser2(t2.LastWriteUserId, cache: cache);
            var i = 0;
            if (t2.TitleAcquisitionsUnits != null) foreach (var tau in t2.TitleAcquisitionsUnits)
                {
                    tau.Id = (++i).ToString();
                    tau.TitleId = t2.Id;
                    tau.Title = t2;
                    if (load && tau.AcquisitionsUnitId != null) tau.AcquisitionsUnit = FindAcquisitionsUnit2(tau.AcquisitionsUnitId, cache: cache);
                }
            i = 0;
            if (t2.TitleBindItemIds != null) foreach (var tbii in t2.TitleBindItemIds)
                {
                    tbii.Id = (++i).ToString();
                    tbii.TitleId = t2.Id;
                    tbii.Title = t2;
                }
            i = 0;
            if (t2.TitleContributors != null) foreach (var tc in t2.TitleContributors)
                {
                    tc.Id = (++i).ToString();
                    tc.TitleId = t2.Id;
                    tc.Title = t2;
                    if (load && tc.ContributorNameTypeId != null) tc.ContributorNameType = FindContributorNameType2(tc.ContributorNameTypeId, cache: cache);
                }
            i = 0;
            if (t2.TitleProductIds != null) foreach (var tpi in t2.TitleProductIds)
                {
                    tpi.Id = (++i).ToString();
                    tpi.TitleId = t2.Id;
                    tpi.Title = t2;
                    if (load && tpi.ProductIdTypeId != null) tpi.ProductIdType = FindIdType2(tpi.ProductIdTypeId, cache: cache);
                }
            return t2;
        }

        public void Insert(Title2 title2)
        {
            if (title2.Id == null) title2.Id = Guid.NewGuid();
            FolioServiceClient.InsertTitle(title2.ToJObject());
        }

        public void Update(Title2 title2) => FolioServiceClient.UpdateTitle(title2.ToJObject());

        public void UpdateOrInsert(Title2 title2)
        {
            if (title2.Id == null)
                Insert(title2);
            else
                try
                {
                    Update(title2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(title2); else throw;
                }
        }

        public void InsertOrUpdate(Title2 title2)
        {
            try
            {
                Insert(title2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(title2); else throw;
            }
        }

        public void DeleteTitle2(Guid? id) => FolioServiceClient.DeleteTitle(id?.ToString());

        public bool AnyTransaction2s(string where = null) => FolioServiceClient.AnyTransactions(where);

        public int CountTransaction2s(string where = null) => FolioServiceClient.CountTransactions(where);

        public Transaction2[] Transaction2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Transactions(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var t2 = cache ? (Transaction2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Transaction2.FromJObject(jo)) : Transaction2.FromJObject(jo);
                if (load && t2.AwaitingPaymentEncumbranceId != null) t2.AwaitingPaymentEncumbrance = FindTransaction2(t2.AwaitingPaymentEncumbranceId, cache: cache);
                if (load && t2.OrderId != null) t2.Order = FindOrder2(t2.OrderId, cache: cache);
                if (load && t2.OrderItemId != null) t2.OrderItem = FindOrderItem2(t2.OrderItemId, cache: cache);
                if (load && t2.ExpenseClassId != null) t2.ExpenseClass = FindExpenseClass2(t2.ExpenseClassId, cache: cache);
                if (load && t2.FiscalYearId != null) t2.FiscalYear = FindFiscalYear2(t2.FiscalYearId, cache: cache);
                if (load && t2.FromFundId != null) t2.FromFund = FindFund2(t2.FromFundId, cache: cache);
                if (load && t2.PaymentEncumbranceId != null) t2.PaymentEncumbrance = FindTransaction2(t2.PaymentEncumbranceId, cache: cache);
                if (load && t2.SourceFiscalYearId != null) t2.SourceFiscalYear = FindFiscalYear2(t2.SourceFiscalYearId, cache: cache);
                if (load && t2.InvoiceId != null) t2.Invoice = FindInvoice2(t2.InvoiceId, cache: cache);
                if (load && t2.InvoiceItemId != null) t2.InvoiceItem = FindInvoiceItem2(t2.InvoiceItemId, cache: cache);
                if (load && t2.ToFundId != null) t2.ToFund = FindFund2(t2.ToFundId, cache: cache);
                if (load && t2.CreationUserId != null) t2.CreationUser = FindUser2(t2.CreationUserId, cache: cache);
                if (load && t2.LastWriteUserId != null) t2.LastWriteUser = FindUser2(t2.LastWriteUserId, cache: cache);
                return t2;
            }).ToArray();
        }

        public IEnumerable<Transaction2> Transaction2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Transactions(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var t2 = cache ? (Transaction2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Transaction2.FromJObject(jo)) : Transaction2.FromJObject(jo);
                if (load && t2.AwaitingPaymentEncumbranceId != null) t2.AwaitingPaymentEncumbrance = FindTransaction2(t2.AwaitingPaymentEncumbranceId, cache: cache);
                if (load && t2.OrderId != null) t2.Order = FindOrder2(t2.OrderId, cache: cache);
                if (load && t2.OrderItemId != null) t2.OrderItem = FindOrderItem2(t2.OrderItemId, cache: cache);
                if (load && t2.ExpenseClassId != null) t2.ExpenseClass = FindExpenseClass2(t2.ExpenseClassId, cache: cache);
                if (load && t2.FiscalYearId != null) t2.FiscalYear = FindFiscalYear2(t2.FiscalYearId, cache: cache);
                if (load && t2.FromFundId != null) t2.FromFund = FindFund2(t2.FromFundId, cache: cache);
                if (load && t2.PaymentEncumbranceId != null) t2.PaymentEncumbrance = FindTransaction2(t2.PaymentEncumbranceId, cache: cache);
                if (load && t2.SourceFiscalYearId != null) t2.SourceFiscalYear = FindFiscalYear2(t2.SourceFiscalYearId, cache: cache);
                if (load && t2.InvoiceId != null) t2.Invoice = FindInvoice2(t2.InvoiceId, cache: cache);
                if (load && t2.InvoiceItemId != null) t2.InvoiceItem = FindInvoiceItem2(t2.InvoiceItemId, cache: cache);
                if (load && t2.ToFundId != null) t2.ToFund = FindFund2(t2.ToFundId, cache: cache);
                if (load && t2.CreationUserId != null) t2.CreationUser = FindUser2(t2.CreationUserId, cache: cache);
                if (load && t2.LastWriteUserId != null) t2.LastWriteUser = FindUser2(t2.LastWriteUserId, cache: cache);
                yield return t2;
            }
        }

        public Transaction2 FindTransaction2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var t2 = cache ? (Transaction2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Transaction2.FromJObject(FolioServiceClient.GetTransaction(id?.ToString()))) : Transaction2.FromJObject(FolioServiceClient.GetTransaction(id?.ToString()));
            if (t2 == null) return null;
            if (load && t2.AwaitingPaymentEncumbranceId != null) t2.AwaitingPaymentEncumbrance = FindTransaction2(t2.AwaitingPaymentEncumbranceId, cache: cache);
            if (load && t2.OrderId != null) t2.Order = FindOrder2(t2.OrderId, cache: cache);
            if (load && t2.OrderItemId != null) t2.OrderItem = FindOrderItem2(t2.OrderItemId, cache: cache);
            if (load && t2.ExpenseClassId != null) t2.ExpenseClass = FindExpenseClass2(t2.ExpenseClassId, cache: cache);
            if (load && t2.FiscalYearId != null) t2.FiscalYear = FindFiscalYear2(t2.FiscalYearId, cache: cache);
            if (load && t2.FromFundId != null) t2.FromFund = FindFund2(t2.FromFundId, cache: cache);
            if (load && t2.PaymentEncumbranceId != null) t2.PaymentEncumbrance = FindTransaction2(t2.PaymentEncumbranceId, cache: cache);
            if (load && t2.SourceFiscalYearId != null) t2.SourceFiscalYear = FindFiscalYear2(t2.SourceFiscalYearId, cache: cache);
            if (load && t2.InvoiceId != null) t2.Invoice = FindInvoice2(t2.InvoiceId, cache: cache);
            if (load && t2.InvoiceItemId != null) t2.InvoiceItem = FindInvoiceItem2(t2.InvoiceItemId, cache: cache);
            if (load && t2.ToFundId != null) t2.ToFund = FindFund2(t2.ToFundId, cache: cache);
            if (load && t2.CreationUserId != null) t2.CreationUser = FindUser2(t2.CreationUserId, cache: cache);
            if (load && t2.LastWriteUserId != null) t2.LastWriteUser = FindUser2(t2.LastWriteUserId, cache: cache);
            var i = 0;
            if (t2.TransactionTags != null) foreach (var tt in t2.TransactionTags)
                {
                    tt.Id = (++i).ToString();
                    tt.TransactionId = t2.Id;
                    tt.Transaction = t2;
                }
            return t2;
        }

        public void Insert(Transaction2 transaction2)
        {
            if (transaction2.Id == null) transaction2.Id = Guid.NewGuid();
            FolioServiceClient.InsertTransaction(transaction2.ToJObject());
        }

        public void Update(Transaction2 transaction2) => FolioServiceClient.UpdateTransaction(transaction2.ToJObject());

        public void UpdateOrInsert(Transaction2 transaction2)
        {
            if (transaction2.Id == null)
                Insert(transaction2);
            else
                try
                {
                    Update(transaction2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(transaction2); else throw;
                }
        }

        public void InsertOrUpdate(Transaction2 transaction2)
        {
            try
            {
                Insert(transaction2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(transaction2); else throw;
            }
        }

        public void DeleteTransaction2(Guid? id) => FolioServiceClient.DeleteTransaction(id?.ToString());

        public bool AnyTransferAccount2s(string where = null) => FolioServiceClient.AnyTransferAccounts(where);

        public int CountTransferAccount2s(string where = null) => FolioServiceClient.CountTransferAccounts(where);

        public TransferAccount2[] TransferAccount2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.TransferAccounts(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var ta2 = cache ? (TransferAccount2)(objects.ContainsKey(id) ? objects[id] : objects[id] = TransferAccount2.FromJObject(jo)) : TransferAccount2.FromJObject(jo);
                if (load && ta2.CreationUserId != null) ta2.CreationUser = FindUser2(ta2.CreationUserId, cache: cache);
                if (load && ta2.LastWriteUserId != null) ta2.LastWriteUser = FindUser2(ta2.LastWriteUserId, cache: cache);
                if (load && ta2.OwnerId != null) ta2.Owner = FindOwner2(ta2.OwnerId, cache: cache);
                return ta2;
            }).ToArray();
        }

        public IEnumerable<TransferAccount2> TransferAccount2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.TransferAccounts(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var ta2 = cache ? (TransferAccount2)(objects.ContainsKey(id) ? objects[id] : objects[id] = TransferAccount2.FromJObject(jo)) : TransferAccount2.FromJObject(jo);
                if (load && ta2.CreationUserId != null) ta2.CreationUser = FindUser2(ta2.CreationUserId, cache: cache);
                if (load && ta2.LastWriteUserId != null) ta2.LastWriteUser = FindUser2(ta2.LastWriteUserId, cache: cache);
                if (load && ta2.OwnerId != null) ta2.Owner = FindOwner2(ta2.OwnerId, cache: cache);
                yield return ta2;
            }
        }

        public TransferAccount2 FindTransferAccount2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var ta2 = cache ? (TransferAccount2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = TransferAccount2.FromJObject(FolioServiceClient.GetTransferAccount(id?.ToString()))) : TransferAccount2.FromJObject(FolioServiceClient.GetTransferAccount(id?.ToString()));
            if (ta2 == null) return null;
            if (load && ta2.CreationUserId != null) ta2.CreationUser = FindUser2(ta2.CreationUserId, cache: cache);
            if (load && ta2.LastWriteUserId != null) ta2.LastWriteUser = FindUser2(ta2.LastWriteUserId, cache: cache);
            if (load && ta2.OwnerId != null) ta2.Owner = FindOwner2(ta2.OwnerId, cache: cache);
            return ta2;
        }

        public void Insert(TransferAccount2 transferAccount2)
        {
            if (transferAccount2.Id == null) transferAccount2.Id = Guid.NewGuid();
            FolioServiceClient.InsertTransferAccount(transferAccount2.ToJObject());
        }

        public void Update(TransferAccount2 transferAccount2) => FolioServiceClient.UpdateTransferAccount(transferAccount2.ToJObject());

        public void UpdateOrInsert(TransferAccount2 transferAccount2)
        {
            if (transferAccount2.Id == null)
                Insert(transferAccount2);
            else
                try
                {
                    Update(transferAccount2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(transferAccount2); else throw;
                }
        }

        public void InsertOrUpdate(TransferAccount2 transferAccount2)
        {
            try
            {
                Insert(transferAccount2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(transferAccount2); else throw;
            }
        }

        public void DeleteTransferAccount2(Guid? id) => FolioServiceClient.DeleteTransferAccount(id?.ToString());

        public bool AnyTransferCriteria2s(string where = null) => FolioServiceClient.AnyTransferCriterias(where);

        public int CountTransferCriteria2s(string where = null) => FolioServiceClient.CountTransferCriterias(where);

        public TransferCriteria2[] TransferCriteria2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.TransferCriterias(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var tc2 = cache ? (TransferCriteria2)(objects.ContainsKey(id) ? objects[id] : objects[id] = TransferCriteria2.FromJObject(jo)) : TransferCriteria2.FromJObject(jo);
                return tc2;
            }).ToArray();
        }

        public IEnumerable<TransferCriteria2> TransferCriteria2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.TransferCriterias(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var tc2 = cache ? (TransferCriteria2)(objects.ContainsKey(id) ? objects[id] : objects[id] = TransferCriteria2.FromJObject(jo)) : TransferCriteria2.FromJObject(jo);
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

        public void UpdateOrInsert(TransferCriteria2 transferCriteria2)
        {
            if (transferCriteria2.Id == null)
                Insert(transferCriteria2);
            else
                try
                {
                    Update(transferCriteria2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(transferCriteria2); else throw;
                }
        }

        public void InsertOrUpdate(TransferCriteria2 transferCriteria2)
        {
            try
            {
                Insert(transferCriteria2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(transferCriteria2); else throw;
            }
        }

        public void DeleteTransferCriteria2(Guid? id) => FolioServiceClient.DeleteTransferCriteria(id?.ToString());

        public bool AnyUser2s(string where = null) => FolioServiceClient.AnyUsers(where);

        public int CountUser2s(string where = null) => FolioServiceClient.CountUsers(where);

        public User2[] User2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Users(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var u2 = cache ? (User2)(objects.ContainsKey(id) ? objects[id] : objects[id] = User2.FromJObject(jo)) : User2.FromJObject(jo);
                if (load && u2.GroupId != null) u2.Group = FindGroup2(u2.GroupId, cache: cache);
                if (load && u2.CreationUserId != null) u2.CreationUser = FindUser2(u2.CreationUserId, cache: cache);
                if (load && u2.LastWriteUserId != null) u2.LastWriteUser = FindUser2(u2.LastWriteUserId, cache: cache);
                return u2;
            }).ToArray();
        }

        public IEnumerable<User2> User2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Users(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var u2 = cache ? (User2)(objects.ContainsKey(id) ? objects[id] : objects[id] = User2.FromJObject(jo)) : User2.FromJObject(jo);
                if (load && u2.GroupId != null) u2.Group = FindGroup2(u2.GroupId, cache: cache);
                if (load && u2.CreationUserId != null) u2.CreationUser = FindUser2(u2.CreationUserId, cache: cache);
                if (load && u2.LastWriteUserId != null) u2.LastWriteUser = FindUser2(u2.LastWriteUserId, cache: cache);
                yield return u2;
            }
        }

        public User2 FindUser2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var u2 = cache ? (User2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = User2.FromJObject(FolioServiceClient.GetUser(id?.ToString()))) : User2.FromJObject(FolioServiceClient.GetUser(id?.ToString()));
            if (u2 == null) return null;
            if (load && u2.GroupId != null) u2.Group = FindGroup2(u2.GroupId, cache: cache);
            if (load && u2.CreationUserId != null) u2.CreationUser = FindUser2(u2.CreationUserId, cache: cache);
            if (load && u2.LastWriteUserId != null) u2.LastWriteUser = FindUser2(u2.LastWriteUserId, cache: cache);
            var i = 0;
            if (u2.PreferredEmailCommunications != null) foreach (var pec in u2.PreferredEmailCommunications)
                {
                    pec.Id = (++i).ToString();
                    pec.UserId = u2.Id;
                    pec.User = u2;
                }
            i = 0;
            if (u2.UserAddresses != null) foreach (var ua in u2.UserAddresses)
                {
                    ua.Id = (++i).ToString();
                    ua.UserId = u2.Id;
                    ua.User = u2;
                    if (load && ua.AddressTypeId != null) ua.AddressType = FindAddressType2(ua.AddressTypeId, cache: cache);
                }
            i = 0;
            if (u2.UserDepartments != null) foreach (var ud in u2.UserDepartments)
                {
                    ud.Id = (++i).ToString();
                    ud.UserId = u2.Id;
                    ud.User = u2;
                    if (load && ud.DepartmentId != null) ud.Department = FindDepartment2(ud.DepartmentId, cache: cache);
                }
            i = 0;
            if (u2.UserTags != null) foreach (var ut in u2.UserTags)
                {
                    ut.Id = (++i).ToString();
                    ut.UserId = u2.Id;
                    ut.User = u2;
                }
            return u2;
        }

        public void Insert(User2 user2)
        {
            if (user2.Id == null) user2.Id = Guid.NewGuid();
            FolioServiceClient.InsertUser(user2.ToJObject());
        }

        public void Update(User2 user2) => FolioServiceClient.UpdateUser(user2.ToJObject());

        public void UpdateOrInsert(User2 user2)
        {
            if (user2.Id == null)
                Insert(user2);
            else
                try
                {
                    Update(user2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(user2); else throw;
                }
        }

        public void InsertOrUpdate(User2 user2)
        {
            try
            {
                Insert(user2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(user2); else throw;
            }
        }

        public void DeleteUser2(Guid? id) => FolioServiceClient.DeleteUser(id?.ToString());

        public bool AnyUserAcquisitionsUnit2s(string where = null) => FolioServiceClient.AnyUserAcquisitionsUnits(where);

        public int CountUserAcquisitionsUnit2s(string where = null) => FolioServiceClient.CountUserAcquisitionsUnits(where);

        public UserAcquisitionsUnit2[] UserAcquisitionsUnit2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.UserAcquisitionsUnits(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var uau2 = cache ? (UserAcquisitionsUnit2)(objects.ContainsKey(id) ? objects[id] : objects[id] = UserAcquisitionsUnit2.FromJObject(jo)) : UserAcquisitionsUnit2.FromJObject(jo);
                if (load && uau2.UserId != null) uau2.User = FindUser2(uau2.UserId, cache: cache);
                if (load && uau2.AcquisitionsUnitId != null) uau2.AcquisitionsUnit = FindAcquisitionsUnit2(uau2.AcquisitionsUnitId, cache: cache);
                if (load && uau2.CreationUserId != null) uau2.CreationUser = FindUser2(uau2.CreationUserId, cache: cache);
                if (load && uau2.LastWriteUserId != null) uau2.LastWriteUser = FindUser2(uau2.LastWriteUserId, cache: cache);
                return uau2;
            }).ToArray();
        }

        public IEnumerable<UserAcquisitionsUnit2> UserAcquisitionsUnit2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.UserAcquisitionsUnits(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var uau2 = cache ? (UserAcquisitionsUnit2)(objects.ContainsKey(id) ? objects[id] : objects[id] = UserAcquisitionsUnit2.FromJObject(jo)) : UserAcquisitionsUnit2.FromJObject(jo);
                if (load && uau2.UserId != null) uau2.User = FindUser2(uau2.UserId, cache: cache);
                if (load && uau2.AcquisitionsUnitId != null) uau2.AcquisitionsUnit = FindAcquisitionsUnit2(uau2.AcquisitionsUnitId, cache: cache);
                if (load && uau2.CreationUserId != null) uau2.CreationUser = FindUser2(uau2.CreationUserId, cache: cache);
                if (load && uau2.LastWriteUserId != null) uau2.LastWriteUser = FindUser2(uau2.LastWriteUserId, cache: cache);
                yield return uau2;
            }
        }

        public UserAcquisitionsUnit2 FindUserAcquisitionsUnit2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var uau2 = cache ? (UserAcquisitionsUnit2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = UserAcquisitionsUnit2.FromJObject(FolioServiceClient.GetUserAcquisitionsUnit(id?.ToString()))) : UserAcquisitionsUnit2.FromJObject(FolioServiceClient.GetUserAcquisitionsUnit(id?.ToString()));
            if (uau2 == null) return null;
            if (load && uau2.UserId != null) uau2.User = FindUser2(uau2.UserId, cache: cache);
            if (load && uau2.AcquisitionsUnitId != null) uau2.AcquisitionsUnit = FindAcquisitionsUnit2(uau2.AcquisitionsUnitId, cache: cache);
            if (load && uau2.CreationUserId != null) uau2.CreationUser = FindUser2(uau2.CreationUserId, cache: cache);
            if (load && uau2.LastWriteUserId != null) uau2.LastWriteUser = FindUser2(uau2.LastWriteUserId, cache: cache);
            return uau2;
        }

        public void Insert(UserAcquisitionsUnit2 userAcquisitionsUnit2)
        {
            if (userAcquisitionsUnit2.Id == null) userAcquisitionsUnit2.Id = Guid.NewGuid();
            FolioServiceClient.InsertUserAcquisitionsUnit(userAcquisitionsUnit2.ToJObject());
        }

        public void Update(UserAcquisitionsUnit2 userAcquisitionsUnit2) => FolioServiceClient.UpdateUserAcquisitionsUnit(userAcquisitionsUnit2.ToJObject());

        public void UpdateOrInsert(UserAcquisitionsUnit2 userAcquisitionsUnit2)
        {
            if (userAcquisitionsUnit2.Id == null)
                Insert(userAcquisitionsUnit2);
            else
                try
                {
                    Update(userAcquisitionsUnit2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(userAcquisitionsUnit2); else throw;
                }
        }

        public void InsertOrUpdate(UserAcquisitionsUnit2 userAcquisitionsUnit2)
        {
            try
            {
                Insert(userAcquisitionsUnit2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(userAcquisitionsUnit2); else throw;
            }
        }

        public void DeleteUserAcquisitionsUnit2(Guid? id) => FolioServiceClient.DeleteUserAcquisitionsUnit(id?.ToString());

        public bool AnyUserRequestPreference2s(string where = null) => FolioServiceClient.AnyUserRequestPreferences(where);

        public int CountUserRequestPreference2s(string where = null) => FolioServiceClient.CountUserRequestPreferences(where);

        public UserRequestPreference2[] UserRequestPreference2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.UserRequestPreferences(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var urp2 = cache ? (UserRequestPreference2)(objects.ContainsKey(id) ? objects[id] : objects[id] = UserRequestPreference2.FromJObject(jo)) : UserRequestPreference2.FromJObject(jo);
                if (load && urp2.UserId != null) urp2.User = FindUser2(urp2.UserId, cache: cache);
                if (load && urp2.DefaultServicePointId != null) urp2.DefaultServicePoint = FindServicePoint2(urp2.DefaultServicePointId, cache: cache);
                if (load && urp2.DefaultDeliveryAddressTypeId != null) urp2.DefaultDeliveryAddressType = FindAddressType2(urp2.DefaultDeliveryAddressTypeId, cache: cache);
                if (load && urp2.CreationUserId != null) urp2.CreationUser = FindUser2(urp2.CreationUserId, cache: cache);
                if (load && urp2.LastWriteUserId != null) urp2.LastWriteUser = FindUser2(urp2.LastWriteUserId, cache: cache);
                return urp2;
            }).ToArray();
        }

        public IEnumerable<UserRequestPreference2> UserRequestPreference2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.UserRequestPreferences(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var urp2 = cache ? (UserRequestPreference2)(objects.ContainsKey(id) ? objects[id] : objects[id] = UserRequestPreference2.FromJObject(jo)) : UserRequestPreference2.FromJObject(jo);
                if (load && urp2.UserId != null) urp2.User = FindUser2(urp2.UserId, cache: cache);
                if (load && urp2.DefaultServicePointId != null) urp2.DefaultServicePoint = FindServicePoint2(urp2.DefaultServicePointId, cache: cache);
                if (load && urp2.DefaultDeliveryAddressTypeId != null) urp2.DefaultDeliveryAddressType = FindAddressType2(urp2.DefaultDeliveryAddressTypeId, cache: cache);
                if (load && urp2.CreationUserId != null) urp2.CreationUser = FindUser2(urp2.CreationUserId, cache: cache);
                if (load && urp2.LastWriteUserId != null) urp2.LastWriteUser = FindUser2(urp2.LastWriteUserId, cache: cache);
                yield return urp2;
            }
        }

        public UserRequestPreference2 FindUserRequestPreference2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var urp2 = cache ? (UserRequestPreference2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = UserRequestPreference2.FromJObject(FolioServiceClient.GetUserRequestPreference(id?.ToString()))) : UserRequestPreference2.FromJObject(FolioServiceClient.GetUserRequestPreference(id?.ToString()));
            if (urp2 == null) return null;
            if (load && urp2.UserId != null) urp2.User = FindUser2(urp2.UserId, cache: cache);
            if (load && urp2.DefaultServicePointId != null) urp2.DefaultServicePoint = FindServicePoint2(urp2.DefaultServicePointId, cache: cache);
            if (load && urp2.DefaultDeliveryAddressTypeId != null) urp2.DefaultDeliveryAddressType = FindAddressType2(urp2.DefaultDeliveryAddressTypeId, cache: cache);
            if (load && urp2.CreationUserId != null) urp2.CreationUser = FindUser2(urp2.CreationUserId, cache: cache);
            if (load && urp2.LastWriteUserId != null) urp2.LastWriteUser = FindUser2(urp2.LastWriteUserId, cache: cache);
            return urp2;
        }

        public void Insert(UserRequestPreference2 userRequestPreference2)
        {
            if (userRequestPreference2.Id == null) userRequestPreference2.Id = Guid.NewGuid();
            FolioServiceClient.InsertUserRequestPreference(userRequestPreference2.ToJObject());
        }

        public void Update(UserRequestPreference2 userRequestPreference2) => FolioServiceClient.UpdateUserRequestPreference(userRequestPreference2.ToJObject());

        public void UpdateOrInsert(UserRequestPreference2 userRequestPreference2)
        {
            if (userRequestPreference2.Id == null)
                Insert(userRequestPreference2);
            else
                try
                {
                    Update(userRequestPreference2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(userRequestPreference2); else throw;
                }
        }

        public void InsertOrUpdate(UserRequestPreference2 userRequestPreference2)
        {
            try
            {
                Insert(userRequestPreference2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(userRequestPreference2); else throw;
            }
        }

        public void DeleteUserRequestPreference2(Guid? id) => FolioServiceClient.DeleteUserRequestPreference(id?.ToString());

        public bool AnyVoucher2s(string where = null) => FolioServiceClient.AnyVouchers(where);

        public int CountVoucher2s(string where = null) => FolioServiceClient.CountVouchers(where);

        public Voucher2[] Voucher2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.Vouchers(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var v2 = cache ? (Voucher2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Voucher2.FromJObject(jo)) : Voucher2.FromJObject(jo);
                if (load && v2.BatchGroupId != null) v2.BatchGroup = FindBatchGroup2(v2.BatchGroupId, cache: cache);
                if (load && v2.InvoiceId != null) v2.Invoice = FindInvoice2(v2.InvoiceId, cache: cache);
                if (load && v2.VendorId != null) v2.Vendor = FindOrganization2(v2.VendorId, cache: cache);
                if (load && v2.CreationUserId != null) v2.CreationUser = FindUser2(v2.CreationUserId, cache: cache);
                if (load && v2.LastWriteUserId != null) v2.LastWriteUser = FindUser2(v2.LastWriteUserId, cache: cache);
                return v2;
            }).ToArray();
        }

        public IEnumerable<Voucher2> Voucher2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.Vouchers(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var v2 = cache ? (Voucher2)(objects.ContainsKey(id) ? objects[id] : objects[id] = Voucher2.FromJObject(jo)) : Voucher2.FromJObject(jo);
                if (load && v2.BatchGroupId != null) v2.BatchGroup = FindBatchGroup2(v2.BatchGroupId, cache: cache);
                if (load && v2.InvoiceId != null) v2.Invoice = FindInvoice2(v2.InvoiceId, cache: cache);
                if (load && v2.VendorId != null) v2.Vendor = FindOrganization2(v2.VendorId, cache: cache);
                if (load && v2.CreationUserId != null) v2.CreationUser = FindUser2(v2.CreationUserId, cache: cache);
                if (load && v2.LastWriteUserId != null) v2.LastWriteUser = FindUser2(v2.LastWriteUserId, cache: cache);
                yield return v2;
            }
        }

        public Voucher2 FindVoucher2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var v2 = cache ? (Voucher2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = Voucher2.FromJObject(FolioServiceClient.GetVoucher(id?.ToString()))) : Voucher2.FromJObject(FolioServiceClient.GetVoucher(id?.ToString()));
            if (v2 == null) return null;
            if (load && v2.BatchGroupId != null) v2.BatchGroup = FindBatchGroup2(v2.BatchGroupId, cache: cache);
            if (load && v2.InvoiceId != null) v2.Invoice = FindInvoice2(v2.InvoiceId, cache: cache);
            if (load && v2.VendorId != null) v2.Vendor = FindOrganization2(v2.VendorId, cache: cache);
            if (load && v2.CreationUserId != null) v2.CreationUser = FindUser2(v2.CreationUserId, cache: cache);
            if (load && v2.LastWriteUserId != null) v2.LastWriteUser = FindUser2(v2.LastWriteUserId, cache: cache);
            var i = 0;
            if (v2.VoucherAcquisitionsUnits != null) foreach (var vau in v2.VoucherAcquisitionsUnits)
                {
                    vau.Id = (++i).ToString();
                    vau.VoucherId = v2.Id;
                    vau.Voucher = v2;
                    if (load && vau.AcquisitionsUnitId != null) vau.AcquisitionsUnit = FindAcquisitionsUnit2(vau.AcquisitionsUnitId, cache: cache);
                }
            return v2;
        }

        public void Insert(Voucher2 voucher2)
        {
            if (voucher2.Id == null) voucher2.Id = Guid.NewGuid();
            FolioServiceClient.InsertVoucher(voucher2.ToJObject());
        }

        public void Update(Voucher2 voucher2) => FolioServiceClient.UpdateVoucher(voucher2.ToJObject());

        public void UpdateOrInsert(Voucher2 voucher2)
        {
            if (voucher2.Id == null)
                Insert(voucher2);
            else
                try
                {
                    Update(voucher2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(voucher2); else throw;
                }
        }

        public void InsertOrUpdate(Voucher2 voucher2)
        {
            try
            {
                Insert(voucher2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(voucher2); else throw;
            }
        }

        public void DeleteVoucher2(Guid? id) => FolioServiceClient.DeleteVoucher(id?.ToString());

        public bool AnyVoucherItem2s(string where = null) => FolioServiceClient.AnyVoucherItems(where);

        public int CountVoucherItem2s(string where = null) => FolioServiceClient.CountVoucherItems(where);

        public VoucherItem2[] VoucherItem2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.VoucherItems(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var vi2 = cache ? (VoucherItem2)(objects.ContainsKey(id) ? objects[id] : objects[id] = VoucherItem2.FromJObject(jo)) : VoucherItem2.FromJObject(jo);
                if (load && vi2.SubTransactionId != null) vi2.SubTransaction = FindTransaction2(vi2.SubTransactionId, cache: cache);
                if (load && vi2.VoucherId != null) vi2.Voucher = FindVoucher2(vi2.VoucherId, cache: cache);
                if (load && vi2.CreationUserId != null) vi2.CreationUser = FindUser2(vi2.CreationUserId, cache: cache);
                if (load && vi2.LastWriteUserId != null) vi2.LastWriteUser = FindUser2(vi2.LastWriteUserId, cache: cache);
                return vi2;
            }).ToArray();
        }

        public IEnumerable<VoucherItem2> VoucherItem2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.VoucherItems(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var vi2 = cache ? (VoucherItem2)(objects.ContainsKey(id) ? objects[id] : objects[id] = VoucherItem2.FromJObject(jo)) : VoucherItem2.FromJObject(jo);
                if (load && vi2.SubTransactionId != null) vi2.SubTransaction = FindTransaction2(vi2.SubTransactionId, cache: cache);
                if (load && vi2.VoucherId != null) vi2.Voucher = FindVoucher2(vi2.VoucherId, cache: cache);
                if (load && vi2.CreationUserId != null) vi2.CreationUser = FindUser2(vi2.CreationUserId, cache: cache);
                if (load && vi2.LastWriteUserId != null) vi2.LastWriteUser = FindUser2(vi2.LastWriteUserId, cache: cache);
                yield return vi2;
            }
        }

        public VoucherItem2 FindVoucherItem2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var vi2 = cache ? (VoucherItem2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = VoucherItem2.FromJObject(FolioServiceClient.GetVoucherItem(id?.ToString()))) : VoucherItem2.FromJObject(FolioServiceClient.GetVoucherItem(id?.ToString()));
            if (vi2 == null) return null;
            if (load && vi2.SubTransactionId != null) vi2.SubTransaction = FindTransaction2(vi2.SubTransactionId, cache: cache);
            if (load && vi2.VoucherId != null) vi2.Voucher = FindVoucher2(vi2.VoucherId, cache: cache);
            if (load && vi2.CreationUserId != null) vi2.CreationUser = FindUser2(vi2.CreationUserId, cache: cache);
            if (load && vi2.LastWriteUserId != null) vi2.LastWriteUser = FindUser2(vi2.LastWriteUserId, cache: cache);
            var i = 0;
            if (vi2.VoucherItemFunds != null) foreach (var vif in vi2.VoucherItemFunds)
                {
                    vif.Id = (++i).ToString();
                    vif.VoucherItemId = vi2.Id;
                    vif.VoucherItem = vi2;
                    if (load && vif.EncumbranceId != null) vif.Encumbrance = FindTransaction2(vif.EncumbranceId, cache: cache);
                    if (load && vif.FundId != null) vif.Fund = FindFund2(vif.FundId, cache: cache);
                    if (load && vif.ExpenseClassId != null) vif.ExpenseClass = FindExpenseClass2(vif.ExpenseClassId, cache: cache);
                }
            i = 0;
            if (vi2.VoucherItemInvoiceItems != null) foreach (var viii in vi2.VoucherItemInvoiceItems)
                {
                    viii.Id = (++i).ToString();
                    viii.VoucherItemId = vi2.Id;
                    viii.VoucherItem = vi2;
                    if (load && viii.InvoiceItemId != null) viii.InvoiceItem = FindInvoiceItem2(viii.InvoiceItemId, cache: cache);
                }
            return vi2;
        }

        public void Insert(VoucherItem2 voucherItem2)
        {
            if (voucherItem2.Id == null) voucherItem2.Id = Guid.NewGuid();
            FolioServiceClient.InsertVoucherItem(voucherItem2.ToJObject());
        }

        public void Update(VoucherItem2 voucherItem2) => FolioServiceClient.UpdateVoucherItem(voucherItem2.ToJObject());

        public void UpdateOrInsert(VoucherItem2 voucherItem2)
        {
            if (voucherItem2.Id == null)
                Insert(voucherItem2);
            else
                try
                {
                    Update(voucherItem2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(voucherItem2); else throw;
                }
        }

        public void InsertOrUpdate(VoucherItem2 voucherItem2)
        {
            try
            {
                Insert(voucherItem2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(voucherItem2); else throw;
            }
        }

        public void DeleteVoucherItem2(Guid? id) => FolioServiceClient.DeleteVoucherItem(id?.ToString());

        public bool AnyWaiveReason2s(string where = null) => FolioServiceClient.AnyWaiveReasons(where);

        public int CountWaiveReason2s(string where = null) => FolioServiceClient.CountWaiveReasons(where);

        public WaiveReason2[] WaiveReason2s(out int count, string where = null, string orderBy = null, int? skip = null, int? take = 100, bool load = false, bool cache = true)
        {
            return FolioServiceClient.WaiveReasons(out count, where, orderBy, skip, take).Select(jo =>
            {
                var id = Guid.Parse((string)jo["id"]);
                var wr2 = cache ? (WaiveReason2)(objects.ContainsKey(id) ? objects[id] : objects[id] = WaiveReason2.FromJObject(jo)) : WaiveReason2.FromJObject(jo);
                if (load && wr2.CreationUserId != null) wr2.CreationUser = FindUser2(wr2.CreationUserId, cache: cache);
                if (load && wr2.LastWriteUserId != null) wr2.LastWriteUser = FindUser2(wr2.LastWriteUserId, cache: cache);
                if (load && wr2.AccountId != null) wr2.Account = FindFee2(wr2.AccountId, cache: cache);
                return wr2;
            }).ToArray();
        }

        public IEnumerable<WaiveReason2> WaiveReason2s(string where = null, string orderBy = null, int? skip = null, int? take = null, bool load = false, bool cache = true)
        {
            foreach (var jo in FolioServiceClient.WaiveReasons(where, orderBy, skip, take))
            {
                var id = Guid.Parse((string)jo["id"]);
                var wr2 = cache ? (WaiveReason2)(objects.ContainsKey(id) ? objects[id] : objects[id] = WaiveReason2.FromJObject(jo)) : WaiveReason2.FromJObject(jo);
                if (load && wr2.CreationUserId != null) wr2.CreationUser = FindUser2(wr2.CreationUserId, cache: cache);
                if (load && wr2.LastWriteUserId != null) wr2.LastWriteUser = FindUser2(wr2.LastWriteUserId, cache: cache);
                if (load && wr2.AccountId != null) wr2.Account = FindFee2(wr2.AccountId, cache: cache);
                yield return wr2;
            }
        }

        public WaiveReason2 FindWaiveReason2(Guid? id, bool load = false, bool cache = true)
        {
            if (id == null) return null;
            var wr2 = cache ? (WaiveReason2)(objects.ContainsKey(id.Value) ? objects[id.Value] : objects[id.Value] = WaiveReason2.FromJObject(FolioServiceClient.GetWaiveReason(id?.ToString()))) : WaiveReason2.FromJObject(FolioServiceClient.GetWaiveReason(id?.ToString()));
            if (wr2 == null) return null;
            if (load && wr2.CreationUserId != null) wr2.CreationUser = FindUser2(wr2.CreationUserId, cache: cache);
            if (load && wr2.LastWriteUserId != null) wr2.LastWriteUser = FindUser2(wr2.LastWriteUserId, cache: cache);
            if (load && wr2.AccountId != null) wr2.Account = FindFee2(wr2.AccountId, cache: cache);
            return wr2;
        }

        public void Insert(WaiveReason2 waiveReason2)
        {
            if (waiveReason2.Id == null) waiveReason2.Id = Guid.NewGuid();
            FolioServiceClient.InsertWaiveReason(waiveReason2.ToJObject());
        }

        public void Update(WaiveReason2 waiveReason2) => FolioServiceClient.UpdateWaiveReason(waiveReason2.ToJObject());

        public void UpdateOrInsert(WaiveReason2 waiveReason2)
        {
            if (waiveReason2.Id == null)
                Insert(waiveReason2);
            else
                try
                {
                    Update(waiveReason2);
                }
                catch (HttpRequestException e)
                {
                    if (e.Message.Contains("NotFound")) Insert(waiveReason2); else throw;
                }
        }

        public void InsertOrUpdate(WaiveReason2 waiveReason2)
        {
            try
            {
                Insert(waiveReason2);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("duplicate key")) Update(waiveReason2); else throw;
            }
        }

        public void DeleteWaiveReason2(Guid? id) => FolioServiceClient.DeleteWaiveReason(id?.ToString());

        public void Dispose()
        {
            if (!pool)
                FolioServiceClient.Dispose();
            else
            {
                objects.Clear();
                FolioServiceContextPool.AddFolioServiceContext(this);
            }
        }
    }
}
