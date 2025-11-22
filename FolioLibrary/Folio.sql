DROP SCHEMA IF EXISTS uc_agreements CASCADE;
CREATE SCHEMA uc_agreements;
CREATE TABLE uc_agreements.agreements (
    id UUID NOT NULL,
    jsonb JSONB NOT NULL,
    CONSTRAINT pk_agreements PRIMARY KEY(id)
);
CREATE TABLE uc_agreements.agreement_items (
    id UUID NOT NULL,
    jsonb JSONB NOT NULL,
    CONSTRAINT pk_agreement_items PRIMARY KEY(id)
);
CREATE TABLE uc_agreements.reference_datas (
    id UUID NOT NULL,
    jsonb JSONB NOT NULL,
    CONSTRAINT pk_reference_datas PRIMARY KEY(id)
);
DROP SCHEMA IF EXISTS uc CASCADE;
CREATE SCHEMA uc;
CREATE FUNCTION uc.date_cast(IN TEXT) RETURNS DATE LANGUAGE PLPGSQL IMMUTABLE AS 'BEGIN RETURN $1::DATE; EXCEPTION WHEN OTHERS THEN RETURN NULL; END';
CREATE FUNCTION uc.timestamp_cast(IN TEXT) RETURNS TIMESTAMP WITH TIME ZONE LANGUAGE PLPGSQL IMMUTABLE AS 'BEGIN RETURN $1::TIMESTAMP WITH TIME ZONE; EXCEPTION WHEN OTHERS THEN RETURN NULL; END';
CREATE FUNCTION uc.int_cast(IN TEXT) RETURNS INT LANGUAGE PLPGSQL IMMUTABLE AS 'BEGIN RETURN $1::INT; EXCEPTION WHEN OTHERS THEN RETURN NULL; END';
CREATE VIEW uc.acquisition_methods AS
SELECT
am.id AS id,
am.jsonb->>'value' AS value,
am.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(am.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
am.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(am.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(am.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
am.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_orders_storage.acquisition_method am;
CREATE VIEW uc.acquisitions_units AS
SELECT
au.id AS id,
au.jsonb->>'name' AS name,
au.jsonb->>'description' AS description,
CAST(au.jsonb->>'isDeleted' AS BOOLEAN) AS is_deleted,
CAST(au.jsonb->>'protectCreate' AS BOOLEAN) AS protect_create,
CAST(au.jsonb->>'protectRead' AS BOOLEAN) AS protect_read,
CAST(au.jsonb->>'protectUpdate' AS BOOLEAN) AS protect_update,
CAST(au.jsonb->>'protectDelete' AS BOOLEAN) AS protect_delete,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(au.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
au.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(au.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(au.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
au.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_orders_storage.acquisitions_unit au;
CREATE VIEW uc.actual_cost_record_identifiers AS
SELECT
uuid_generate_v5(acr.id, acri.ordinality::text)::text AS id,
acr.id AS actual_cost_record_id,
acri.jsonb->>'value' AS value,
acri.jsonb->>'identifierType' AS identifier_type,
CAST(acri.jsonb->>'identifierTypeId' AS UUID) AS identifier_type_id
FROM uchicago_mod_circulation_storage.actual_cost_record acr, jsonb_array_elements(acr.jsonb#>'{instance,identifiers}') WITH ORDINALITY acri (jsonb);
CREATE VIEW uc.actual_cost_record_contributors AS
SELECT
uuid_generate_v5(acr.id, acrc.ordinality::text)::text AS id,
acr.id AS actual_cost_record_id,
acrc.jsonb->>'name' AS name
FROM uchicago_mod_circulation_storage.actual_cost_record acr, jsonb_array_elements(acr.jsonb#>'{instance,contributors}') WITH ORDINALITY acrc (jsonb);
CREATE VIEW uc.actual_cost_records AS
SELECT
acr.id AS id,
acr.jsonb->>'lossType' AS loss_type,
uc.DATE_CAST(acr.jsonb->>'lossDate') AS loss_date,
uc.DATE_CAST(acr.jsonb->>'expirationDate') AS expiration_date,
acr.jsonb#>>'{user,barcode}' AS user_barcode,
acr.jsonb#>>'{user,firstName}' AS user_first_name,
acr.jsonb#>>'{user,lastName}' AS user_last_name,
acr.jsonb#>>'{user,middleName}' AS user_middle_name,
CAST(acr.jsonb#>>'{user,patronGroupId}' AS UUID) AS user_patron_group_id,
acr.jsonb#>>'{user,patronGroup}' AS user_patron_group,
acr.jsonb#>>'{item,barcode}' AS item_barcode,
CAST(acr.jsonb#>>'{item,materialTypeId}' AS UUID) AS item_material_type_id,
acr.jsonb#>>'{item,materialType}' AS item_material_type,
CAST(acr.jsonb#>>'{item,permanentLocationId}' AS UUID) AS item_permanent_location_id,
acr.jsonb#>>'{item,permanentLocation}' AS item_permanent_location,
CAST(acr.jsonb#>>'{item,effectiveLocationId}' AS UUID) AS item_effective_location_id,
acr.jsonb#>>'{item,effectiveLocation}' AS item_effective_location,
CAST(acr.jsonb#>>'{item,loanTypeId}' AS UUID) AS item_loan_type_id,
acr.jsonb#>>'{item,loanType}' AS item_loan_type,
CAST(acr.jsonb#>>'{item,holdingsRecordId}' AS UUID) AS item_holdings_record_id,
acr.jsonb#>>'{item,effectiveCallNumberComponents,callNumber}' AS item_effective_call_number_components_call_number,
acr.jsonb#>>'{item,effectiveCallNumberComponents,prefix}' AS item_effective_call_number_components_prefix,
acr.jsonb#>>'{item,effectiveCallNumberComponents,suffix}' AS item_effective_call_number_components_suffix,
acr.jsonb#>>'{item,volume}' AS item_volume,
acr.jsonb#>>'{item,enumeration}' AS item_enumeration,
acr.jsonb#>>'{item,chronology}' AS item_chronology,
acr.jsonb#>>'{item,displaySummary}' AS item_display_summary,
acr.jsonb#>>'{item,copyNumber}' AS item_copy_number,
acr.jsonb#>>'{instance,title}' AS instance_title,
CAST(acr.jsonb#>>'{feeFine,accountId}' AS UUID) AS fee_fine_account_id,
CAST(acr.jsonb#>>'{feeFine,billedAmount}' AS DECIMAL(19,2)) AS fee_fine_billed_amount,
CAST(acr.jsonb#>>'{feeFine,ownerId}' AS UUID) AS fee_fine_owner_id,
acr.jsonb#>>'{feeFine,owner}' AS fee_fine_owner,
CAST(acr.jsonb#>>'{feeFine,typeId}' AS UUID) AS fee_fine_type_id,
acr.jsonb#>>'{feeFine,type}' AS fee_fine_type,
acr.jsonb->>'status' AS status,
acr.jsonb->>'additionalInfoForStaff' AS additional_info_for_staff,
acr.jsonb->>'additionalInfoForPatron' AS additional_info_for_patron,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(acr.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
acr.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(acr.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(acr.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
acr.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_circulation_storage.actual_cost_record acr;
CREATE VIEW uc.address_types AS
SELECT
at.id AS id,
at.jsonb->>'addressType' AS address_type,
at.jsonb->>'desc' AS desc,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(at.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
at.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(at.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(at.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
at.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_users.addresstype at;
CREATE VIEW uc.agreement_organizations AS
SELECT
uuid_generate_v5(a.id, ao.ordinality::text)::text AS id,
a.id AS agreement_id,
CAST(ao.jsonb->>'primaryOrg' AS BOOLEAN) AS primary_org,
CAST(ao.jsonb#>>'{org,orgsUuid}' AS UUID) AS organization_id
FROM uc_agreements.agreements a, jsonb_array_elements(a.jsonb->'orgs') WITH ORDINALITY ao (jsonb);
CREATE VIEW uc.agreement_periods AS
SELECT
uuid_generate_v5(a.id, ap.ordinality::text)::text AS id,
a.id AS agreement_id,
uc.DATE_CAST(ap.jsonb->>'startDate') AS start_date,
ap.jsonb->>'periodStatus' AS period_status
FROM uc_agreements.agreements a, jsonb_array_elements(a.jsonb->'periods') WITH ORDINALITY ap (jsonb);
CREATE VIEW uc.agreements AS
SELECT
a.id AS id,
CAST(a.jsonb->>'name' AS VARCHAR(255)) AS name,
uc.DATE_CAST(a.jsonb->>'startDate') AS start_date,
uc.DATE_CAST(a.jsonb->>'endDate') AS end_date,
uc.DATE_CAST(a.jsonb->>'cancellationDeadline') AS cancellation_deadline,
a.jsonb#>>'{agreementStatus,value}' AS agreement_status_value,
a.jsonb#>>'{agreementStatus,label}' AS agreement_status_label,
a.jsonb#>>'{isPerpetual,label}' AS is_perpetual_label,
a.jsonb#>>'{renewalPriority,label}' AS renewal_priority_label,
a.jsonb->>'description' AS description,
uc.TIMESTAMP_CAST(a.jsonb->>'dateCreated') AS date_created,
uc.TIMESTAMP_CAST(a.jsonb->>'lastUpdated') AS last_updated,
jsonb_pretty(a.jsonb) AS content
FROM uc_agreements.agreements a;
CREATE VIEW uc.agreement_item_order_items AS
SELECT
uuid_generate_v5(ai.id, aipl.ordinality::text)::text AS id,
ai.id AS agreement_item_id,
CAST(aipl.jsonb->>'poLineId' AS UUID) AS order_item_id
FROM uc_agreements.agreement_items ai, jsonb_array_elements(ai.jsonb->'poLines') WITH ORDINALITY aipl (jsonb);
CREATE VIEW uc.agreement_items AS
SELECT
ai.id AS id,
ai.jsonb->>'type' AS type,
CAST(ai.jsonb->>'suppressFromDiscovery' AS BOOLEAN) AS suppress_from_discovery,
ai.jsonb->>'note' AS note,
ai.jsonb->>'description' AS description,
CAST(ai.jsonb->>'customCoverage' AS BOOLEAN) AS custom_coverage,
uc.DATE_CAST(ai.jsonb->>'startDate') AS start_date,
uc.DATE_CAST(ai.jsonb->>'endDate') AS end_date,
uc.DATE_CAST(ai.jsonb->>'activeFrom') AS active_from,
uc.DATE_CAST(ai.jsonb->>'activeTo') AS active_to,
uc.TIMESTAMP_CAST(ai.jsonb->>'contentUpdated') AS content_updated,
CAST(ai.jsonb->>'haveAccess' AS BOOLEAN) AS have_access,
uc.TIMESTAMP_CAST(ai.jsonb->>'dateCreated') AS date_created,
uc.TIMESTAMP_CAST(ai.jsonb->>'lastUpdated') AS last_updated,
CAST(jsonb#>>'{owner,id}' AS UUID) AS agreement_id,
jsonb_pretty(ai.jsonb) AS content
FROM uc_agreements.agreement_items ai;
CREATE VIEW uc.alternative_title_types AS
SELECT
att.id AS id,
att.jsonb->>'name' AS name,
att.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(att.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
att.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(att.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(att.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
att.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.alternative_title_type att;
CREATE VIEW uc.auth_attempts AS
SELECT
aa.id AS id,
CAST(aa.jsonb->>'userId' AS UUID) AS user_id,
uc.TIMESTAMP_CAST(aa.jsonb->>'lastAttempt') AS last_attempt,
CAST(aa.jsonb->>'attemptCount' AS INTEGER) AS attempt_count,
jsonb_pretty(aa.jsonb) AS content
FROM uchicago_mod_login.auth_attempts aa;
CREATE VIEW uc.auth_credentials_histories AS
SELECT
ach.id AS id,
CAST(ach.jsonb->>'userId' AS UUID) AS user_id,
ach.jsonb->>'hash' AS hash,
ach.jsonb->>'salt' AS salt,
uc.DATE_CAST(ach.jsonb->>'date') AS date,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(ach.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
ach.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(ach.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(ach.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
ach.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_login.auth_credentials_history ach;
CREATE VIEW uc.batch_groups AS
SELECT
bg.id AS id,
bg.jsonb->>'name' AS name,
bg.jsonb->>'description' AS description,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(bg.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
bg.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(bg.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(bg.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
bg.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_invoice_storage.batch_groups bg;
CREATE VIEW uc.blocks AS
SELECT
b.id AS id,
b.jsonb->>'type' AS type,
b.jsonb->>'desc' AS desc,
b.jsonb->>'code' AS code,
b.jsonb->>'staffInformation' AS staff_information,
b.jsonb->>'patronMessage' AS patron_message,
uc.DATE_CAST(b.jsonb->>'expirationDate') AS expiration_date,
CAST(b.jsonb->>'borrowing' AS BOOLEAN) AS borrowing,
CAST(b.jsonb->>'renewals' AS BOOLEAN) AS renewals,
CAST(b.jsonb->>'requests' AS BOOLEAN) AS requests,
CAST(b.jsonb->>'userId' AS UUID) AS user_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(b.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
b.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(b.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(b.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
b.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_feesfines.manualblocks b;
CREATE VIEW uc.block_conditions AS
SELECT
bc.id AS id,
bc.jsonb->>'name' AS name,
CAST(bc.jsonb->>'blockBorrowing' AS BOOLEAN) AS block_borrowing,
CAST(bc.jsonb->>'blockRenewals' AS BOOLEAN) AS block_renewals,
CAST(bc.jsonb->>'blockRequests' AS BOOLEAN) AS block_requests,
bc.jsonb->>'valueType' AS value_type,
bc.jsonb->>'message' AS message,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(bc.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
bc.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(bc.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(bc.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
bc.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_patron_blocks.patron_block_conditions bc;
CREATE VIEW uc.block_limits AS
SELECT
bl.id AS id,
CAST(bl.jsonb->>'patronGroupId' AS UUID) AS group_id,
CAST(bl.jsonb->>'conditionId' AS UUID) AS condition_id,
CAST(bl.jsonb->>'value' AS DECIMAL(19,2)) AS value,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(bl.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
bl.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(bl.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(bl.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
bl.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content,
bl.conditionid AS conditionid
FROM uchicago_mod_patron_blocks.patron_block_limits bl;
CREATE VIEW uc.bound_with_parts AS
SELECT
bwp.id AS id,
CAST(bwp.jsonb->>'holdingsRecordId' AS UUID) AS holding_id,
CAST(bwp.jsonb->>'itemId' AS UUID) AS item_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(bwp.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
bwp.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(bwp.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(bwp.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
bwp.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.bound_with_part bwp;
CREATE VIEW uc.budget_acquisitions_units AS
SELECT
uuid_generate_v5(b.id, bau.ordinality::text)::text AS id,
b.id AS budget_id,
CAST(bau.jsonb AS UUID) AS acquisitions_unit_id
FROM uchicago_mod_finance_storage.budget b, jsonb_array_elements_text(b.jsonb->'acqUnitIds') WITH ORDINALITY bau (jsonb);
CREATE VIEW uc.budget_tags AS
SELECT
uuid_generate_v5(b.id, bt.ordinality::text)::text AS id,
b.id AS budget_id,
CAST(bt.jsonb AS UUID) AS tag_id
FROM uchicago_mod_finance_storage.budget b, jsonb_array_elements_text(b.jsonb#>'{tags,tagList}') WITH ORDINALITY bt (jsonb);
CREATE VIEW uc.budgets AS
SELECT
b.id AS id,
CAST(b.jsonb->>'_version' AS INTEGER) AS _version,
b.jsonb->>'name' AS name,
b.jsonb->>'budgetStatus' AS budget_status,
CAST(b.jsonb->>'allowableEncumbrance' AS DECIMAL(19,2)) AS allowable_encumbrance,
CAST(b.jsonb->>'allowableExpenditure' AS DECIMAL(19,2)) AS allowable_expenditure,
CAST(b.jsonb->>'allocated' AS DECIMAL(19,2)) AS allocated,
CAST(b.jsonb->>'awaitingPayment' AS DECIMAL(19,2)) AS awaiting_payment,
CAST(b.jsonb->>'available' AS DECIMAL(19,2)) AS available,
CAST(b.jsonb->>'credits' AS DECIMAL(19,2)) AS credits,
CAST(b.jsonb->>'encumbered' AS DECIMAL(19,2)) AS encumbered,
CAST(b.jsonb->>'expenditures' AS DECIMAL(19,2)) AS expenditures,
CAST(b.jsonb->>'netTransfers' AS DECIMAL(19,2)) AS net_transfers,
CAST(b.jsonb->>'unavailable' AS DECIMAL(19,2)) AS unavailable,
CAST(b.jsonb->>'overEncumbrance' AS DECIMAL(19,2)) AS over_encumbrance,
CAST(b.jsonb->>'overExpended' AS DECIMAL(19,2)) AS over_expended,
CAST(b.jsonb->>'fundId' AS UUID) AS fund_id,
CAST(b.jsonb->>'fiscalYearId' AS UUID) AS fiscal_year_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(b.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
b.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(b.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(b.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
b.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
CAST(b.jsonb->>'initialAllocation' AS DECIMAL(19,2)) AS initial_allocation,
CAST(b.jsonb->>'allocationTo' AS DECIMAL(19,2)) AS allocation_to,
CAST(b.jsonb->>'allocationFrom' AS DECIMAL(19,2)) AS allocation_from,
CAST(b.jsonb->>'totalFunding' AS DECIMAL(19,2)) AS total_funding,
CAST(b.jsonb->>'cashBalance' AS DECIMAL(19,2)) AS cash_balance,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_finance_storage.budget b;
CREATE VIEW uc.budget_expense_classes AS
SELECT
bec.id AS id,
CAST(bec.jsonb->>'_version' AS INTEGER) AS _version,
CAST(bec.jsonb->>'budgetId' AS UUID) AS budget_id,
CAST(bec.jsonb->>'expenseClassId' AS UUID) AS expense_class_id,
bec.jsonb->>'status' AS status,
jsonb_pretty(bec.jsonb) AS content
FROM uchicago_mod_finance_storage.budget_expense_class bec;
CREATE VIEW uc.budget_groups AS
SELECT
bg.id AS id,
CAST(bg.jsonb->>'_version' AS INTEGER) AS _version,
CAST(bg.jsonb->>'budgetId' AS UUID) AS budget_id,
CAST(bg.jsonb->>'groupId' AS UUID) AS group_id,
CAST(bg.jsonb->>'fiscalYearId' AS UUID) AS fiscal_year_id,
CAST(bg.jsonb->>'fundId' AS UUID) AS fund_id,
jsonb_pretty(bg.jsonb) AS content
FROM uchicago_mod_finance_storage.group_fund_fiscal_year bg;
CREATE VIEW uc.call_number_types AS
SELECT
cnt.id AS id,
cnt.jsonb->>'name' AS name,
cnt.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(cnt.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
cnt.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(cnt.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(cnt.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
cnt.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.call_number_type cnt;
CREATE VIEW uc.campuses AS
SELECT
c.id AS id,
c.jsonb->>'name' AS name,
c.jsonb->>'code' AS code,
CAST(c.jsonb->>'institutionId' AS UUID) AS institution_id,
CAST(c.jsonb->>'isShadow' AS BOOLEAN) AS is_shadow,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(c.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
c.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(c.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(c.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
c.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.loccampus c;
CREATE VIEW uc.cancellation_reasons AS
SELECT
cr.id AS id,
cr.jsonb->>'name' AS name,
cr.jsonb->>'description' AS description,
cr.jsonb->>'publicDescription' AS public_description,
CAST(cr.jsonb->>'requiresAdditionalInformation' AS BOOLEAN) AS requires_additional_information,
cr.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(cr.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
cr.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(cr.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(cr.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
cr.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_circulation_storage.cancellation_reason cr;
CREATE VIEW uc.categories AS
SELECT
c.id AS id,
c.jsonb->>'value' AS value,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(c.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
c.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(c.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(c.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
c.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_organizations_storage.categories c;
CREATE VIEW uc.check_ins AS
SELECT
ci.id AS id,
uc.TIMESTAMP_CAST(ci.jsonb->>'occurredDateTime') AS occurred_date_time,
CAST(ci.jsonb->>'itemId' AS UUID) AS item_id,
ci.jsonb->>'itemStatusPriorToCheckIn' AS item_status_prior_to_check_in,
CAST(ci.jsonb->>'requestQueueSize' AS INTEGER) AS request_queue_size,
CAST(ci.jsonb->>'itemLocationId' AS UUID) AS item_location_id,
CAST(ci.jsonb->>'servicePointId' AS UUID) AS service_point_id,
CAST(ci.jsonb->>'performedByUserId' AS UUID) AS performed_by_user_id,
jsonb_pretty(ci.jsonb) AS content
FROM uchicago_mod_circulation_storage.check_in ci;
CREATE VIEW uc.circulation_rules AS
SELECT
cr.id AS id,
cr.jsonb->>'rulesAsText' AS rules_as_text,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(cr.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
cr.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(cr.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(cr.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
cr.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content,
cr.lock AS lock
FROM uchicago_mod_circulation_storage.circulation_rules cr;
CREATE VIEW uc.classification_types AS
SELECT
ct.id AS id,
ct.jsonb->>'name' AS name,
ct.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(ct.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
ct.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(ct.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(ct.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
ct.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.classification_type ct;
CREATE VIEW uc.close_reasons AS
SELECT
cr.id AS id,
cr.jsonb->>'reason' AS name,
cr.jsonb->>'source' AS source,
jsonb_pretty(cr.jsonb) AS content
FROM uchicago_mod_orders_storage.reasons_for_closure cr;
CREATE VIEW uc.comments AS
SELECT
c.id AS id,
CAST(c.jsonb->>'paid' AS BOOLEAN) AS paid,
CAST(c.jsonb->>'waived' AS BOOLEAN) AS waived,
CAST(c.jsonb->>'refunded' AS BOOLEAN) AS refunded,
CAST(c.jsonb->>'transferredManually' AS BOOLEAN) AS transferred_manually,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(c.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
c.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(c.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(c.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
c.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_feesfines.comments c;
CREATE VIEW uc.configurations AS
SELECT
c.id AS id,
c.jsonb->>'module' AS module,
c.jsonb->>'configName' AS config_name,
c.jsonb->>'code' AS code,
c.jsonb->>'description' AS description,
CAST(c.jsonb->>'default' AS BOOLEAN) AS default,
CAST(c.jsonb->>'enabled' AS BOOLEAN) AS enabled,
c.jsonb->>'value' AS value,
CAST(c.jsonb->>'userId' AS VARCHAR(128)) AS user_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(c.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
c.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(c.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(c.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
c.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_configuration.config_data c;
CREATE VIEW uc.contact_phone_number_categories AS
SELECT
uuid_generate_v5(c.id, cpn.ordinality::text || '-' || cpnc.ordinality::text)::text AS id,
uuid_generate_v5(c.id, cpn.ordinality::text)::text AS contact_phone_number_id,
CAST(cpnc.jsonb AS UUID) AS category_id
FROM uchicago_mod_organizations_storage.contacts c, jsonb_array_elements(c.jsonb->'phoneNumbers') WITH ORDINALITY cpn (jsonb), jsonb_array_elements_text(cpn.jsonb->'categories') WITH ORDINALITY cpnc (jsonb);
CREATE VIEW uc.contact_phone_numbers AS
SELECT
uuid_generate_v5(c.id, cpn.ordinality::text)::text AS id,
c.id AS contact_id,
CAST(cpn.jsonb->>'id' AS UUID) AS id2,
cpn.jsonb->>'phoneNumber' AS phone_number,
cpn.jsonb->>'type' AS type,
CAST(cpn.jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
cpn.jsonb->>'language' AS language,
uc.TIMESTAMP_CAST(cpn.jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(cpn.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
cpn.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(cpn.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(cpn.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
cpn.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username
FROM uchicago_mod_organizations_storage.contacts c, jsonb_array_elements(c.jsonb->'phoneNumbers') WITH ORDINALITY cpn (jsonb);
CREATE VIEW uc.contact_email_categories AS
SELECT
uuid_generate_v5(c.id, ce.ordinality::text || '-' || cec.ordinality::text)::text AS id,
uuid_generate_v5(c.id, ce.ordinality::text)::text AS contact_email_id,
CAST(cec.jsonb AS UUID) AS category_id
FROM uchicago_mod_organizations_storage.contacts c, jsonb_array_elements(c.jsonb->'emails') WITH ORDINALITY ce (jsonb), jsonb_array_elements_text(ce.jsonb->'categories') WITH ORDINALITY cec (jsonb);
CREATE VIEW uc.contact_emails AS
SELECT
uuid_generate_v5(c.id, ce.ordinality::text)::text AS id,
c.id AS contact_id,
CAST(ce.jsonb->>'id' AS UUID) AS id2,
ce.jsonb->>'value' AS value,
ce.jsonb->>'description' AS description,
CAST(ce.jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
ce.jsonb->>'language' AS language,
uc.TIMESTAMP_CAST(ce.jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(ce.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
ce.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(ce.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(ce.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
ce.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username
FROM uchicago_mod_organizations_storage.contacts c, jsonb_array_elements(c.jsonb->'emails') WITH ORDINALITY ce (jsonb);
CREATE VIEW uc.contact_address_categories AS
SELECT
uuid_generate_v5(c.id, ca.ordinality::text || '-' || cac.ordinality::text)::text AS id,
uuid_generate_v5(c.id, ca.ordinality::text)::text AS contact_address_id,
CAST(cac.jsonb AS UUID) AS category_id
FROM uchicago_mod_organizations_storage.contacts c, jsonb_array_elements(c.jsonb->'addresses') WITH ORDINALITY ca (jsonb), jsonb_array_elements_text(ca.jsonb->'categories') WITH ORDINALITY cac (jsonb);
CREATE VIEW uc.contact_addresses AS
SELECT
uuid_generate_v5(c.id, ca.ordinality::text)::text AS id,
c.id AS contact_id,
CAST(ca.jsonb->>'id' AS UUID) AS id2,
ca.jsonb->>'addressLine1' AS address_line1,
ca.jsonb->>'addressLine2' AS address_line2,
ca.jsonb->>'city' AS city,
ca.jsonb->>'stateRegion' AS state_region,
ca.jsonb->>'zipCode' AS zip_code,
ca.jsonb->>'country' AS country,
CAST(ca.jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
ca.jsonb->>'language' AS language,
uc.TIMESTAMP_CAST(ca.jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(ca.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
ca.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(ca.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(ca.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
ca.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username
FROM uchicago_mod_organizations_storage.contacts c, jsonb_array_elements(c.jsonb->'addresses') WITH ORDINALITY ca (jsonb);
CREATE VIEW uc.contact_url_categories AS
SELECT
uuid_generate_v5(c.id, cu.ordinality::text || '-' || cuc.ordinality::text)::text AS id,
uuid_generate_v5(c.id, cu.ordinality::text)::text AS contact_url_id,
CAST(cuc.jsonb AS UUID) AS category_id
FROM uchicago_mod_organizations_storage.contacts c, jsonb_array_elements(c.jsonb->'urls') WITH ORDINALITY cu (jsonb), jsonb_array_elements_text(cu.jsonb->'categories') WITH ORDINALITY cuc (jsonb);
CREATE VIEW uc.contact_urls AS
SELECT
uuid_generate_v5(c.id, cu.ordinality::text)::text AS id,
c.id AS contact_id,
CAST(cu.jsonb->>'id' AS UUID) AS id2,
cu.jsonb->>'value' AS value,
cu.jsonb->>'description' AS description,
cu.jsonb->>'language' AS language,
CAST(cu.jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
cu.jsonb->>'notes' AS notes,
uc.TIMESTAMP_CAST(cu.jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(cu.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
cu.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(cu.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(cu.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
cu.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username
FROM uchicago_mod_organizations_storage.contacts c, jsonb_array_elements(c.jsonb->'urls') WITH ORDINALITY cu (jsonb);
CREATE VIEW uc.contact_categories AS
SELECT
uuid_generate_v5(c.id, cc.ordinality::text)::text AS id,
c.id AS contact_id,
CAST(cc.jsonb AS UUID) AS category_id
FROM uchicago_mod_organizations_storage.contacts c, jsonb_array_elements_text(c.jsonb->'categories') WITH ORDINALITY cc (jsonb);
CREATE VIEW uc.contacts AS
SELECT
c.id AS id,
CAST(c.jsonb->>'_version' AS INTEGER) AS _version,
CAST(CONCAT_WS(' ', jsonb#>>'{firstName}', jsonb#>>'{lastName}') AS VARCHAR(1024)) AS name,
c.jsonb->>'prefix' AS prefix,
c.jsonb->>'firstName' AS first_name,
c.jsonb->>'lastName' AS last_name,
c.jsonb->>'language' AS language,
c.jsonb->>'notes' AS notes,
CAST(c.jsonb->>'inactive' AS BOOLEAN) AS inactive,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(c.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
c.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(c.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(c.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
c.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_organizations_storage.contacts c;
CREATE VIEW uc.contributor_name_types AS
SELECT
cnt.id AS id,
cnt.jsonb->>'name' AS name,
cnt.jsonb->>'ordering' AS ordering,
cnt.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(cnt.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
cnt.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(cnt.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(cnt.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
cnt.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.contributor_name_type cnt;
CREATE VIEW uc.contributor_types AS
SELECT
ct.id AS id,
ct.jsonb->>'name' AS name,
ct.jsonb->>'code' AS code,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(ct.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
ct.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(ct.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(ct.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
ct.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.contributor_type ct;
CREATE VIEW uc.custom_field_values AS
SELECT
uuid_generate_v5(cf.id, cfv.ordinality::text)::text AS id,
cf.id AS custom_field_id,
cfv.jsonb->>'id' AS id2,
cfv.jsonb->>'value' AS value,
CAST(cfv.jsonb->>'default' AS BOOLEAN) AS default
FROM uchicago_mod_users.custom_fields cf, jsonb_array_elements(cf.jsonb#>'{selectField,options,values}') WITH ORDINALITY cfv (jsonb);
CREATE VIEW uc.custom_fields AS
SELECT
cf.id AS id,
cf.jsonb->>'name' AS name,
CAST(cf.jsonb->>'refId' AS VARCHAR(128)) AS ref_id,
cf.jsonb->>'type' AS type,
cf.jsonb->>'entityType' AS entity_type,
CAST(cf.jsonb->>'visible' AS BOOLEAN) AS visible,
CAST(cf.jsonb->>'required' AS BOOLEAN) AS required,
CAST(cf.jsonb->>'isRepeatable' AS BOOLEAN) AS is_repeatable,
CAST(cf.jsonb->>'order' AS INTEGER) AS order,
cf.jsonb->>'helpText' AS help_text,
CAST(cf.jsonb#>>'{checkboxField,default}' AS BOOLEAN) AS checkbox_field_default,
CAST(cf.jsonb#>>'{selectField,multiSelect}' AS BOOLEAN) AS select_field_multi_select,
cf.jsonb#>>'{selectField,options,sortingOrder}' AS select_field_options_sorting_order,
cf.jsonb#>>'{textField,fieldFormat}' AS text_field_field_format,
cf.jsonb->>'displayInAccordion' AS display_in_accordion,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(cf.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
cf.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(cf.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(cf.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
cf.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_users.custom_fields cf;
CREATE VIEW uc.date_types AS
SELECT
dt.id AS id,
dt.jsonb->>'name' AS name,
CAST(dt.jsonb->>'code' AS VARCHAR(1)) AS code,
dt.jsonb#>>'{displayFormat,delimiter}' AS display_format_delimiter,
CAST(dt.jsonb#>>'{displayFormat,keepDelimiter}' AS BOOLEAN) AS display_format_keep_delimiter,
dt.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(dt.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
dt.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(dt.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(dt.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
dt.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.instance_date_type dt;
CREATE VIEW uc.departments AS
SELECT
d.id AS id,
d.jsonb->>'name' AS name,
d.jsonb->>'code' AS code,
CAST(d.jsonb->>'usageNumber' AS INTEGER) AS usage_number,
d.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(d.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
d.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(d.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(d.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
d.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_users.departments d;
CREATE VIEW uc.documents AS
SELECT
d.id AS id,
d.jsonb#>>'{documentMetadata,name}' AS document_metadata_name,
CAST(d.jsonb#>>'{documentMetadata,invoiceId}' AS UUID) AS document_metadata_invoice_id,
d.jsonb#>>'{documentMetadata,url}' AS document_metadata_url,
uc.TIMESTAMP_CAST(d.jsonb#>>'{documentMetadata,metadata,createdDate}') AS document_metadata_metadata_created_date,
CAST(d.jsonb#>>'{documentMetadata,metadata,createdByUserId}' AS UUID) AS document_metadata_metadata_created_by_user_id,
d.jsonb#>>'{documentMetadata,metadata,createdByUsername}' AS document_metadata_metadata_created_by_username,
uc.TIMESTAMP_CAST(d.jsonb#>>'{documentMetadata,metadata,updatedDate}') AS document_metadata_metadata_updated_date,
CAST(d.jsonb#>>'{documentMetadata,metadata,updatedByUserId}' AS UUID) AS document_metadata_metadata_updated_by_user_id,
d.jsonb#>>'{documentMetadata,metadata,updatedByUsername}' AS document_metadata_metadata_updated_by_username,
d.jsonb#>>'{contents,data}' AS contents_data,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(d.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
d.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(d.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(d.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
d.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content,
d.invoiceid AS invoiceid,
d.document_data AS document_data
FROM uchicago_mod_invoice_storage.documents d;
CREATE VIEW uc.electronic_access_relationships AS
SELECT
ear.id AS id,
ear.jsonb->>'name' AS name,
ear.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(ear.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
ear.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(ear.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(ear.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
ear.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.electronic_access_relationship ear;
CREATE VIEW uc.error_records AS
SELECT
er.id AS id,
er.content AS content,
er.description AS description
FROM uchicago_mod_source_record_storage.error_records_lb er;
CREATE VIEW uc.event_logs AS
SELECT
el.id AS id,
el.jsonb->>'tenant' AS tenant,
CAST(el.jsonb->>'userId' AS UUID) AS user_id,
el.jsonb->>'ip' AS ip,
el.jsonb->>'browserInformation' AS browser_information,
uc.TIMESTAMP_CAST(el.jsonb->>'timestamp') AS timestamp,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(el.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
el.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(el.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(el.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
el.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_login.event_logs el;
CREATE VIEW uc.expense_classes AS
SELECT
ec.id AS id,
CAST(ec.jsonb->>'_version' AS INTEGER) AS _version,
ec.jsonb->>'code' AS code,
ec.jsonb->>'externalAccountNumberExt' AS external_account_number_ext,
ec.jsonb->>'name' AS name,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(ec.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
ec.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(ec.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(ec.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
ec.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_finance_storage.expense_class ec;
CREATE VIEW uc.fees AS
SELECT
f.id AS id,
CAST(f.jsonb->>'amount' AS DECIMAL(19,2)) AS amount,
CAST(f.jsonb->>'remaining' AS DECIMAL(19,2)) AS remaining,
uc.DATE_CAST(f.jsonb->>'dateCreated') AS date_created,
uc.DATE_CAST(f.jsonb->>'dateUpdated') AS date_updated,
f.jsonb#>>'{status,name}' AS status_name,
f.jsonb#>>'{paymentStatus,name}' AS payment_status_name,
f.jsonb->>'feeFineType' AS fee_fine_type,
f.jsonb->>'feeFineOwner' AS fee_fine_owner,
f.jsonb->>'title' AS title,
f.jsonb->>'callNumber' AS call_number,
f.jsonb->>'barcode' AS barcode,
f.jsonb->>'materialType' AS material_type,
f.jsonb#>>'{itemStatus,name}' AS item_status_name,
f.jsonb->>'location' AS location,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(f.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
f.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(f.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(f.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
f.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
uc.TIMESTAMP_CAST(f.jsonb->>'dueDate') AS due_date,
uc.TIMESTAMP_CAST(f.jsonb->>'returnedDate') AS returned_date,
CAST(f.jsonb->>'loanId' AS UUID) AS loan_id,
CAST(f.jsonb->>'userId' AS UUID) AS user_id,
CAST(f.jsonb->>'itemId' AS UUID) AS item_id,
CAST(f.jsonb->>'materialTypeId' AS UUID) AS material_type_id,
CAST(f.jsonb->>'feeFineId' AS UUID) AS fee_type_id,
CAST(f.jsonb->>'ownerId' AS UUID) AS owner_id,
CAST(f.jsonb->>'holdingsRecordId' AS UUID) AS holding_id,
CAST(f.jsonb->>'instanceId' AS UUID) AS instance_id,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_feesfines.accounts f;
CREATE VIEW uc.fee_types AS
SELECT
ft.id AS id,
CAST(ft.jsonb->>'automatic' AS BOOLEAN) AS automatic,
ft.jsonb->>'feeFineType' AS fee_fine_type,
CAST(ft.jsonb->>'defaultAmount' AS DECIMAL(19,2)) AS default_amount,
CAST(ft.jsonb->>'chargeNoticeId' AS UUID) AS charge_notice_id,
CAST(ft.jsonb->>'actionNoticeId' AS UUID) AS action_notice_id,
CAST(ft.jsonb->>'ownerId' AS UUID) AS owner_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(ft.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
ft.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(ft.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(ft.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
ft.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_feesfines.feefines ft;
CREATE VIEW uc.finance_group_acquisitions_units AS
SELECT
uuid_generate_v5(fg.id, fgau.ordinality::text)::text AS id,
fg.id AS finance_group_id,
CAST(fgau.jsonb AS UUID) AS acquisitions_unit_id
FROM uchicago_mod_finance_storage.groups fg, jsonb_array_elements_text(fg.jsonb->'acqUnitIds') WITH ORDINALITY fgau (jsonb);
CREATE VIEW uc.finance_groups AS
SELECT
fg.id AS id,
CAST(fg.jsonb->>'_version' AS INTEGER) AS _version,
fg.jsonb->>'code' AS code,
fg.jsonb->>'description' AS description,
fg.jsonb->>'name' AS name,
fg.jsonb->>'status' AS status,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(fg.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
fg.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(fg.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(fg.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
fg.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_finance_storage.groups fg;
CREATE VIEW uc.fiscal_year_acquisitions_units AS
SELECT
uuid_generate_v5(fy.id, fyau.ordinality::text)::text AS id,
fy.id AS fiscal_year_id,
CAST(fyau.jsonb AS UUID) AS acquisitions_unit_id
FROM uchicago_mod_finance_storage.fiscal_year fy, jsonb_array_elements_text(fy.jsonb->'acqUnitIds') WITH ORDINALITY fyau (jsonb);
CREATE VIEW uc.fiscal_years AS
SELECT
fy.id AS id,
CAST(fy.jsonb->>'_version' AS INTEGER) AS _version,
fy.jsonb->>'name' AS name,
fy.jsonb->>'code' AS code,
fy.jsonb->>'currency' AS currency,
fy.jsonb->>'description' AS description,
uc.DATE_CAST(fy.jsonb->>'periodStart') AS period_start,
uc.DATE_CAST(fy.jsonb->>'periodEnd') AS period_end,
fy.jsonb->>'series' AS series,
CAST(fy.jsonb#>>'{financialSummary,allocated}' AS DECIMAL(19,2)) AS financial_summary_allocated,
CAST(fy.jsonb#>>'{financialSummary,available}' AS DECIMAL(19,2)) AS financial_summary_available,
CAST(fy.jsonb#>>'{financialSummary,unavailable}' AS DECIMAL(19,2)) AS financial_summary_unavailable,
CAST(fy.jsonb#>>'{financialSummary,initialAllocation}' AS DECIMAL(19,2)) AS financial_summary_initial_allocation,
CAST(fy.jsonb#>>'{financialSummary,allocationTo}' AS DECIMAL(19,2)) AS financial_summary_allocation_to,
CAST(fy.jsonb#>>'{financialSummary,allocationFrom}' AS DECIMAL(19,2)) AS financial_summary_allocation_from,
CAST(fy.jsonb#>>'{financialSummary,totalFunding}' AS DECIMAL(19,2)) AS financial_summary_total_funding,
CAST(fy.jsonb#>>'{financialSummary,cashBalance}' AS DECIMAL(19,2)) AS financial_summary_cash_balance,
CAST(fy.jsonb#>>'{financialSummary,awaitingPayment}' AS DECIMAL(19,2)) AS financial_summary_awaiting_payment,
CAST(fy.jsonb#>>'{financialSummary,credits}' AS DECIMAL(19,2)) AS financial_summary_credits,
CAST(fy.jsonb#>>'{financialSummary,encumbered}' AS DECIMAL(19,2)) AS financial_summary_encumbered,
CAST(fy.jsonb#>>'{financialSummary,expenditures}' AS DECIMAL(19,2)) AS financial_summary_expenditures,
CAST(fy.jsonb#>>'{financialSummary,overEncumbrance}' AS DECIMAL(19,2)) AS financial_summary_over_encumbrance,
CAST(fy.jsonb#>>'{financialSummary,overExpended}' AS DECIMAL(19,2)) AS financial_summary_over_expended,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(fy.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
fy.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(fy.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(fy.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
fy.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_finance_storage.fiscal_year fy;
CREATE VIEW uc.fixed_due_date_schedule_schedules AS
SELECT
uuid_generate_v5(fdds.id, fddss.ordinality::text)::text AS id,
fdds.id AS fixed_due_date_schedule_id,
uc.TIMESTAMP_CAST(fddss.jsonb->>'from') AS from,
uc.TIMESTAMP_CAST(fddss.jsonb->>'to') AS to,
uc.TIMESTAMP_CAST(fddss.jsonb->>'due') AS due
FROM uchicago_mod_circulation_storage.fixed_due_date_schedule fdds, jsonb_array_elements(fdds.jsonb->'schedules') WITH ORDINALITY fddss (jsonb);
CREATE VIEW uc.fixed_due_date_schedules AS
SELECT
fdds.id AS id,
fdds.jsonb->>'name' AS name,
fdds.jsonb->>'description' AS description,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(fdds.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
fdds.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(fdds.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(fdds.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
fdds.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_circulation_storage.fixed_due_date_schedule fdds;
CREATE VIEW uc.allocated_from_funds AS
SELECT
uuid_generate_v5(f.id, aff.ordinality::text)::text AS id,
f.id AS fund_id,
CAST(aff.jsonb AS UUID) AS from_fund_id
FROM uchicago_mod_finance_storage.fund f, jsonb_array_elements_text(f.jsonb->'allocatedFromIds') WITH ORDINALITY aff (jsonb);
CREATE VIEW uc.allocated_to_funds AS
SELECT
uuid_generate_v5(f.id, atf.ordinality::text)::text AS id,
f.id AS fund_id,
CAST(atf.jsonb AS UUID) AS to_fund_id
FROM uchicago_mod_finance_storage.fund f, jsonb_array_elements_text(f.jsonb->'allocatedToIds') WITH ORDINALITY atf (jsonb);
CREATE VIEW uc.fund_acquisitions_units AS
SELECT
uuid_generate_v5(f.id, fau.ordinality::text)::text AS id,
f.id AS fund_id,
CAST(fau.jsonb AS UUID) AS acquisitions_unit_id
FROM uchicago_mod_finance_storage.fund f, jsonb_array_elements_text(f.jsonb->'acqUnitIds') WITH ORDINALITY fau (jsonb);
CREATE VIEW uc.fund_organizations AS
SELECT
uuid_generate_v5(f.id, fo.ordinality::text)::text AS id,
f.id AS fund_id,
CAST(fo.jsonb AS UUID) AS organization_id
FROM uchicago_mod_finance_storage.fund f, jsonb_array_elements_text(f.jsonb->'donorOrganizationIds') WITH ORDINALITY fo (jsonb);
CREATE VIEW uc.fund_locations AS
SELECT
uuid_generate_v5(f.id, fl.ordinality::text)::text AS id,
f.id AS fund_id,
CAST(fl.jsonb->>'locationId' AS UUID) AS location_id,
CAST(fl.jsonb->>'tenantId' AS UUID) AS tenant_id
FROM uchicago_mod_finance_storage.fund f, jsonb_array_elements(f.jsonb->'locations') WITH ORDINALITY fl (jsonb);
CREATE VIEW uc.fund_tags AS
SELECT
uuid_generate_v5(f.id, ft.ordinality::text)::text AS id,
f.id AS fund_id,
CAST(ft.jsonb AS UUID) AS tag_id
FROM uchicago_mod_finance_storage.fund f, jsonb_array_elements_text(f.jsonb#>'{tags,tagList}') WITH ORDINALITY ft (jsonb);
CREATE VIEW uc.funds AS
SELECT
f.id AS id,
CAST(f.jsonb->>'_version' AS INTEGER) AS _version,
f.jsonb->>'code' AS code,
f.jsonb->>'description' AS description,
f.jsonb->>'externalAccountNo' AS external_account_no,
f.jsonb->>'fundStatus' AS fund_status,
CAST(f.jsonb->>'fundTypeId' AS UUID) AS fund_type_id,
CAST(f.jsonb->>'ledgerId' AS UUID) AS ledger_id,
f.jsonb->>'name' AS name,
CAST(f.jsonb->>'restrictByLocations' AS BOOLEAN) AS restrict_by_locations,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(f.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
f.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(f.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(f.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
f.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_finance_storage.fund f;
CREATE VIEW uc.fund_types AS
SELECT
ft.id AS id,
CAST(ft.jsonb->>'_version' AS INTEGER) AS _version,
ft.jsonb->>'name' AS name,
jsonb_pretty(ft.jsonb) AS content
FROM uchicago_mod_finance_storage.fund_type ft;
CREATE VIEW uc.groups AS
SELECT
g.id AS id,
g.jsonb->>'group' AS group,
g.jsonb->>'desc' AS desc,
CAST(g.jsonb->>'expirationOffsetInDays' AS INTEGER) AS expiration_offset_in_days,
g.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(g.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
g.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(g.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(g.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
g.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_users.groups g;
CREATE VIEW uc.holding_former_ids AS
SELECT
uuid_generate_v5(h.id, hfi.ordinality::text)::text AS id,
h.id AS holding_id,
CAST(hfi.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_inventory_storage.holdings_record h, jsonb_array_elements_text(h.jsonb->'formerIds') WITH ORDINALITY hfi (jsonb);
CREATE VIEW uc.holding_electronic_accesses AS
SELECT
uuid_generate_v5(h.id, hea.ordinality::text)::text AS id,
h.id AS holding_id,
hea.jsonb->>'uri' AS uri,
hea.jsonb->>'linkText' AS link_text,
hea.jsonb->>'materialsSpecification' AS materials_specification,
hea.jsonb->>'publicNote' AS public_note,
CAST(hea.jsonb->>'relationshipId' AS UUID) AS relationship_id
FROM uchicago_mod_inventory_storage.holdings_record h, jsonb_array_elements(h.jsonb->'electronicAccess') WITH ORDINALITY hea (jsonb);
CREATE VIEW uc.holding_additional_call_numbers AS
SELECT
uuid_generate_v5(h.id, hacn.ordinality::text)::text AS id,
h.id AS holding_id,
CAST(hacn.jsonb->>'typeId' AS UUID) AS type_id,
hacn.jsonb->>'prefix' AS prefix,
hacn.jsonb->>'callNumber' AS call_number,
hacn.jsonb->>'suffix' AS suffix
FROM uchicago_mod_inventory_storage.holdings_record h, jsonb_array_elements(h.jsonb->'additionalCallNumbers') WITH ORDINALITY hacn (jsonb);
CREATE VIEW uc.holding_administrative_notes AS
SELECT
uuid_generate_v5(h.id, han.ordinality::text)::text AS id,
h.id AS holding_id,
CAST(han.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_inventory_storage.holdings_record h, jsonb_array_elements_text(h.jsonb->'administrativeNotes') WITH ORDINALITY han (jsonb);
CREATE VIEW uc.holding_notes AS
SELECT
uuid_generate_v5(h.id, hn.ordinality::text)::text AS id,
h.id AS holding_id,
CAST(hn.jsonb->>'holdingsNoteTypeId' AS UUID) AS holding_note_type_id,
hn.jsonb->>'note' AS note,
CAST(hn.jsonb->>'staffOnly' AS BOOLEAN) AS staff_only,
uc.TIMESTAMP_CAST(h.jsonb#>>'{metadata,createdDate}' || CASE WHEN h.jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(h.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
uc.TIMESTAMP_CAST(h.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(h.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id
FROM uchicago_mod_inventory_storage.holdings_record h, jsonb_array_elements(h.jsonb->'notes') WITH ORDINALITY hn (jsonb);
CREATE VIEW uc.extents AS
SELECT
uuid_generate_v5(h.id, e2.ordinality::text)::text AS id,
h.id AS holding_id,
e2.jsonb->>'statement' AS statement,
e2.jsonb->>'note' AS note,
e2.jsonb->>'staffNote' AS staff_note
FROM uchicago_mod_inventory_storage.holdings_record h, jsonb_array_elements(h.jsonb->'holdingsStatements') WITH ORDINALITY e2 (jsonb);
CREATE VIEW uc.index_statements AS
SELECT
uuid_generate_v5(h.id, "is".ordinality::text)::text AS id,
h.id AS holding_id,
"is".jsonb->>'statement' AS statement,
"is".jsonb->>'note' AS note,
"is".jsonb->>'staffNote' AS staff_note
FROM uchicago_mod_inventory_storage.holdings_record h, jsonb_array_elements(h.jsonb->'holdingsStatementsForIndexes') WITH ORDINALITY "is" (jsonb);
CREATE VIEW uc.supplement_statements AS
SELECT
uuid_generate_v5(h.id, ss.ordinality::text)::text AS id,
h.id AS holding_id,
ss.jsonb->>'statement' AS statement,
ss.jsonb->>'note' AS note,
ss.jsonb->>'staffNote' AS staff_note
FROM uchicago_mod_inventory_storage.holdings_record h, jsonb_array_elements(h.jsonb->'holdingsStatementsForSupplements') WITH ORDINALITY ss (jsonb);
CREATE VIEW uc.holding_entries AS
SELECT
uuid_generate_v5(h.id, he.ordinality::text)::text AS id,
h.id AS holding_id,
CAST(he.jsonb->>'publicDisplay' AS BOOLEAN) AS public_display,
he.jsonb->>'enumeration' AS enumeration,
he.jsonb->>'chronology' AS chronology
FROM uchicago_mod_inventory_storage.holdings_record h, jsonb_array_elements(h.jsonb#>'{receivingHistory,entries}') WITH ORDINALITY he (jsonb);
CREATE VIEW uc.holding_statistical_codes AS
SELECT
uuid_generate_v5(h.id, hsc.ordinality::text)::text AS id,
h.id AS holding_id,
CAST(hsc.jsonb AS UUID) AS statistical_code_id
FROM uchicago_mod_inventory_storage.holdings_record h, jsonb_array_elements_text(h.jsonb->'statisticalCodeIds') WITH ORDINALITY hsc (jsonb);
CREATE VIEW uc.holding_tags AS
SELECT
uuid_generate_v5(h.id, ht.ordinality::text)::text AS id,
h.id AS holding_id,
CAST(ht.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_inventory_storage.holdings_record h, jsonb_array_elements_text(h.jsonb#>'{tags,tagList}') WITH ORDINALITY ht (jsonb);
CREATE VIEW uc.holdings AS
SELECT
h.id AS id,
CAST(h.jsonb->>'_version' AS INTEGER) AS _version,
CAST(h.jsonb->>'sourceId' AS UUID) AS source_id,
CAST(h.jsonb->>'hrid' AS INTEGER) AS hrid,
CAST(h.jsonb->>'holdingsTypeId' AS UUID) AS holding_type_id,
CAST(h.jsonb->>'instanceId' AS UUID) AS instance_id,
CAST(h.jsonb->>'permanentLocationId' AS UUID) AS permanent_location_id,
CAST(h.jsonb->>'temporaryLocationId' AS UUID) AS temporary_location_id,
CAST(h.jsonb->>'effectiveLocationId' AS UUID) AS effective_location_id,
CAST(h.jsonb->>'callNumberTypeId' AS UUID) AS call_number_type_id,
h.jsonb->>'callNumberPrefix' AS call_number_prefix,
h.jsonb->>'callNumber' AS call_number,
h.jsonb->>'callNumberSuffix' AS call_number_suffix,
h.jsonb->>'shelvingTitle' AS shelving_title,
h.jsonb->>'acquisitionFormat' AS acquisition_format,
h.jsonb->>'acquisitionMethod' AS acquisition_method,
h.jsonb->>'receiptStatus' AS receipt_status,
CAST(h.jsonb->>'illPolicyId' AS UUID) AS ill_policy_id,
h.jsonb->>'retentionPolicy' AS retention_policy,
h.jsonb->>'digitizationPolicy' AS digitization_policy,
h.jsonb->>'copyNumber' AS copy_number,
h.jsonb->>'numberOfItems' AS number_of_items,
h.jsonb#>>'{receivingHistory,displayType}' AS receiving_history_display_type,
CAST(h.jsonb->>'discoverySuppress' AS BOOLEAN) AS discovery_suppress,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(h.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
h.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(h.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(h.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
h.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.holdings_record h;
CREATE VIEW uc.holding_note_types AS
SELECT
hnt.id AS id,
hnt.jsonb->>'name' AS name,
hnt.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(hnt.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
hnt.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(hnt.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(hnt.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
hnt.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.holdings_note_type hnt;
CREATE VIEW uc.holding_types AS
SELECT
ht.id AS id,
ht.jsonb->>'name' AS name,
ht.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(ht.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
ht.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(ht.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(ht.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
ht.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.holdings_type ht;
CREATE VIEW uc.hrid_settings AS
SELECT
hs.id AS id,
hs.jsonb#>>'{instances,prefix}' AS instances_prefix,
CAST(hs.jsonb#>>'{instances,startNumber}' AS INTEGER) AS instances_start_number,
CAST(hs.jsonb#>>'{instances,currentNumber}' AS INTEGER) AS instances_current_number,
hs.jsonb#>>'{holdings,prefix}' AS holdings_prefix,
CAST(hs.jsonb#>>'{holdings,startNumber}' AS INTEGER) AS holdings_start_number,
CAST(hs.jsonb#>>'{holdings,currentNumber}' AS INTEGER) AS holdings_current_number,
hs.jsonb#>>'{items,prefix}' AS items_prefix,
CAST(hs.jsonb#>>'{items,startNumber}' AS INTEGER) AS items_start_number,
CAST(hs.jsonb#>>'{items,currentNumber}' AS INTEGER) AS items_current_number,
CAST(hs.jsonb->>'commonRetainLeadingZeroes' AS BOOLEAN) AS common_retain_leading_zeroes,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(hs.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
hs.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(hs.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(hs.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
hs.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content,
hs.lock AS lock
FROM uchicago_mod_inventory_storage.hrid_settings hs;
CREATE VIEW uc.id_types AS
SELECT
it.id AS id,
it.jsonb->>'name' AS name,
it.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(it.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
it.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(it.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(it.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
it.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.identifier_type it;
CREATE VIEW uc.ill_policies AS
SELECT
ip.id AS id,
ip.jsonb->>'name' AS name,
ip.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(ip.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
ip.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(ip.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(ip.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
ip.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.ill_policy ip;
CREATE VIEW uc.alternative_titles AS
SELECT
uuid_generate_v5(i.id, at.ordinality::text)::text AS id,
i.id AS instance_id,
CAST(at.jsonb->>'alternativeTitleTypeId' AS UUID) AS alternative_title_type_id,
at.jsonb->>'alternativeTitle' AS alternative_title,
CAST(at.jsonb->>'authorityId' AS UUID) AS authority_id
FROM uchicago_mod_inventory_storage.instance i, jsonb_array_elements(i.jsonb->'alternativeTitles') WITH ORDINALITY at (jsonb);
CREATE VIEW uc.editions AS
SELECT
uuid_generate_v5(i.id, e2.ordinality::text)::text AS id,
i.id AS instance_id,
CAST(e2.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_inventory_storage.instance i, jsonb_array_elements_text(i.jsonb->'editions') WITH ORDINALITY e2 (jsonb);
CREATE VIEW uc.series AS
SELECT
uuid_generate_v5(i.id, s.ordinality::text)::text AS id,
i.id AS instance_id,
s.jsonb->>'value' AS name,
CAST(s.jsonb->>'authorityId' AS UUID) AS authority_id
FROM uchicago_mod_inventory_storage.instance i, jsonb_array_elements(i.jsonb->'series') WITH ORDINALITY s (jsonb);
CREATE VIEW uc.identifiers AS
SELECT
uuid_generate_v5(i.id, i2.ordinality::text)::text AS id,
i.id AS instance_id,
i2.jsonb->>'value' AS value,
CAST(i2.jsonb->>'identifierTypeId' AS UUID) AS identifier_type_id
FROM uchicago_mod_inventory_storage.instance i, jsonb_array_elements(i.jsonb->'identifiers') WITH ORDINALITY i2 (jsonb);
CREATE VIEW uc.contributors AS
SELECT
uuid_generate_v5(i.id, c.ordinality::text)::text AS id,
i.id AS instance_id,
c.jsonb->>'name' AS name,
CAST(c.jsonb->>'contributorTypeId' AS UUID) AS contributor_type_id,
c.jsonb->>'contributorTypeText' AS contributor_type_text,
CAST(c.jsonb->>'contributorNameTypeId' AS UUID) AS contributor_name_type_id,
CAST(c.jsonb->>'authorityId' AS UUID) AS authority_id,
CAST(c.jsonb->>'primary' AS BOOLEAN) AS primary
FROM uchicago_mod_inventory_storage.instance i, jsonb_array_elements(i.jsonb->'contributors') WITH ORDINALITY c (jsonb);
CREATE VIEW uc.subjects AS
SELECT
uuid_generate_v5(i.id, s.ordinality::text)::text AS id,
i.id AS instance_id,
s.jsonb->>'value' AS name,
CAST(s.jsonb->>'authorityId' AS UUID) AS authority_id,
CAST(s.jsonb->>'sourceId' AS UUID) AS source_id,
CAST(s.jsonb->>'typeId' AS UUID) AS type_id
FROM uchicago_mod_inventory_storage.instance i, jsonb_array_elements(i.jsonb->'subjects') WITH ORDINALITY s (jsonb);
CREATE VIEW uc.classifications AS
SELECT
uuid_generate_v5(i.id, c.ordinality::text)::text AS id,
i.id AS instance_id,
c.jsonb->>'classificationNumber' AS classification_number,
CAST(c.jsonb->>'classificationTypeId' AS UUID) AS classification_type_id
FROM uchicago_mod_inventory_storage.instance i, jsonb_array_elements(i.jsonb->'classifications') WITH ORDINALITY c (jsonb);
CREATE VIEW uc.publications AS
SELECT
uuid_generate_v5(i.id, p.ordinality::text)::text AS id,
i.id AS instance_id,
p.jsonb->>'publisher' AS publisher,
p.jsonb->>'place' AS place,
p.jsonb->>'dateOfPublication' AS date_of_publication,
p.jsonb->>'role' AS role
FROM uchicago_mod_inventory_storage.instance i, jsonb_array_elements(i.jsonb->'publication') WITH ORDINALITY p (jsonb);
CREATE VIEW uc.publication_frequency AS
SELECT
uuid_generate_v5(i.id, pf.ordinality::text)::text AS id,
i.id AS instance_id,
CAST(pf.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_inventory_storage.instance i, jsonb_array_elements_text(i.jsonb->'publicationFrequency') WITH ORDINALITY pf (jsonb);
CREATE VIEW uc.publication_range AS
SELECT
uuid_generate_v5(i.id, pr.ordinality::text)::text AS id,
i.id AS instance_id,
CAST(pr.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_inventory_storage.instance i, jsonb_array_elements_text(i.jsonb->'publicationRange') WITH ORDINALITY pr (jsonb);
CREATE VIEW uc.electronic_accesses AS
SELECT
uuid_generate_v5(i.id, ea.ordinality::text)::text AS id,
i.id AS instance_id,
ea.jsonb->>'uri' AS uri,
ea.jsonb->>'linkText' AS link_text,
ea.jsonb->>'materialsSpecification' AS materials_specification,
ea.jsonb->>'publicNote' AS public_note,
CAST(ea.jsonb->>'relationshipId' AS UUID) AS relationship_id
FROM uchicago_mod_inventory_storage.instance i, jsonb_array_elements(i.jsonb->'electronicAccess') WITH ORDINALITY ea (jsonb);
CREATE VIEW uc.instance_formats AS
SELECT
uuid_generate_v5(i.id, "if".ordinality::text)::text AS id,
i.id AS instance_id,
CAST("if".jsonb AS UUID) AS format_id
FROM uchicago_mod_inventory_storage.instance i, jsonb_array_elements_text(i.jsonb->'instanceFormatIds') WITH ORDINALITY "if" (jsonb);
CREATE VIEW uc.physical_descriptions AS
SELECT
uuid_generate_v5(i.id, pd.ordinality::text)::text AS id,
i.id AS instance_id,
CAST(pd.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_inventory_storage.instance i, jsonb_array_elements_text(i.jsonb->'physicalDescriptions') WITH ORDINALITY pd (jsonb);
CREATE VIEW uc.languages AS
SELECT
uuid_generate_v5(i.id, l.ordinality::text)::text AS id,
i.id AS instance_id,
CAST(l.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_inventory_storage.instance i, jsonb_array_elements_text(i.jsonb->'languages') WITH ORDINALITY l (jsonb);
CREATE VIEW uc.instance_notes AS
SELECT
uuid_generate_v5(i.id, "in".ordinality::text)::text AS id,
i.id AS instance_id,
CAST("in".jsonb->>'instanceNoteTypeId' AS UUID) AS instance_note_type_id,
"in".jsonb->>'note' AS note,
CAST("in".jsonb->>'staffOnly' AS BOOLEAN) AS staff_only
FROM uchicago_mod_inventory_storage.instance i, jsonb_array_elements(i.jsonb->'notes') WITH ORDINALITY "in" (jsonb);
CREATE VIEW uc.administrative_notes AS
SELECT
uuid_generate_v5(i.id, an.ordinality::text)::text AS id,
i.id AS instance_id,
CAST(an.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_inventory_storage.instance i, jsonb_array_elements_text(i.jsonb->'administrativeNotes') WITH ORDINALITY an (jsonb);
CREATE VIEW uc.instance_statistical_codes AS
SELECT
uuid_generate_v5(i.id, isc.ordinality::text)::text AS id,
i.id AS instance_id,
CAST(isc.jsonb AS UUID) AS statistical_code_id
FROM uchicago_mod_inventory_storage.instance i, jsonb_array_elements_text(i.jsonb->'statisticalCodeIds') WITH ORDINALITY isc (jsonb);
CREATE VIEW uc.instance_tags AS
SELECT
uuid_generate_v5(i.id, it.ordinality::text)::text AS id,
i.id AS instance_id,
CAST(it.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_inventory_storage.instance i, jsonb_array_elements_text(i.jsonb#>'{tags,tagList}') WITH ORDINALITY it (jsonb);
CREATE VIEW uc.instance_nature_of_content_terms AS
SELECT
uuid_generate_v5(i.id, inoct.ordinality::text)::text AS id,
i.id AS instance_id,
CAST(inoct.jsonb AS UUID) AS nature_of_content_term_id
FROM uchicago_mod_inventory_storage.instance i, jsonb_array_elements_text(i.jsonb->'natureOfContentTermIds') WITH ORDINALITY inoct (jsonb);
CREATE VIEW uc.instances AS
SELECT
i.id AS id,
CAST(i.jsonb->>'_version' AS INTEGER) AS _version,
CAST(i.jsonb->>'hrid' AS INTEGER) AS hrid,
i.jsonb->>'matchKey' AS match_key,
i.jsonb->>'sourceUri' AS source_uri,
i.jsonb->>'source' AS source,
i.jsonb->>'title' AS title,
jsonb#>>'{contributors,0,name}' AS author,
i.jsonb->>'indexTitle' AS index_title,
CAST(i.jsonb#>>'{dates,dateTypeId}' AS UUID) AS date_type_id,
CAST(i.jsonb#>>'{dates,date1}' AS VARCHAR(4)) AS date_1,
CAST(i.jsonb#>>'{dates,date2}' AS VARCHAR(4)) AS date_2,
CAST(i.jsonb->>'instanceTypeId' AS UUID) AS instance_type_id,
CAST(i.jsonb->>'modeOfIssuanceId' AS UUID) AS mode_of_issuance_id,
uc.TIMESTAMP_CAST(i.jsonb->>'catalogedDate') AS cataloged_date,
CAST(i.jsonb->>'previouslyHeld' AS BOOLEAN) AS previously_held,
CAST(i.jsonb->>'staffSuppress' AS BOOLEAN) AS staff_suppress,
CAST(i.jsonb->>'discoverySuppress' AS BOOLEAN) AS discovery_suppress,
CAST(i.jsonb->>'deleted' AS BOOLEAN) AS deleted,
i.jsonb->>'sourceRecordFormat' AS source_record_format,
CAST(i.jsonb->>'statusId' AS UUID) AS status_id,
uc.TIMESTAMP_CAST(i.jsonb->>'statusUpdatedDate') AS status_updated_date,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(i.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
i.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(i.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(i.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
i.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content,
i.complete_updated_date AS completion_time,
i.dates_datetypeid AS dates_datetypeid
FROM uchicago_mod_inventory_storage.instance i;
CREATE VIEW uc.formats AS
SELECT
f.id AS id,
f.jsonb->>'name' AS name,
f.jsonb->>'code' AS code,
f.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(f.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
f.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(f.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(f.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
f.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.instance_format f;
CREATE VIEW uc.instance_note_types AS
SELECT
int.id AS id,
int.jsonb->>'name' AS name,
int.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(int.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
int.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(int.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(int.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
int.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.instance_note_type int;
CREATE VIEW uc.relationships AS
SELECT
r.id AS id,
CAST(r.jsonb->>'superInstanceId' AS UUID) AS super_instance_id,
CAST(r.jsonb->>'subInstanceId' AS UUID) AS sub_instance_id,
CAST(r.jsonb->>'instanceRelationshipTypeId' AS UUID) AS instance_relationship_type_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(r.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
r.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(r.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(r.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
r.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.instance_relationship r;
CREATE VIEW uc.relationship_types AS
SELECT
rt.id AS id,
rt.jsonb->>'name' AS name,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(rt.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
rt.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(rt.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(rt.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
rt.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.instance_relationship_type rt;
CREATE VIEW uc.source_marc_fields AS
SELECT
uuid_generate_v5(sm.id, smf.ordinality::text)::text AS id,
sm.id AS source_marc_id,
CAST(smf.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_inventory_storage.instance_source_marc sm, jsonb_array_elements_text(sm.jsonb->'fields') WITH ORDINALITY smf (jsonb);
CREATE VIEW uc.source_marcs AS
SELECT
sm.id AS id,
CAST(sm.jsonb->>'leader' AS VARCHAR(24)) AS leader,
jsonb_pretty(sm.jsonb) AS content
FROM uchicago_mod_inventory_storage.instance_source_marc sm;
CREATE VIEW uc.statuses AS
SELECT
s.id AS id,
s.jsonb->>'code' AS code,
s.jsonb->>'name' AS name,
s.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(s.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
s.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(s.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(s.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
s.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.instance_status s;
CREATE VIEW uc.instance_types AS
SELECT
it.id AS id,
it.jsonb->>'name' AS name,
it.jsonb->>'code' AS code,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(it.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
it.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(it.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(it.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
it.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.instance_type it;
CREATE VIEW uc.institutions AS
SELECT
i.id AS id,
i.jsonb->>'name' AS name,
i.jsonb->>'code' AS code,
CAST(i.jsonb->>'isShadow' AS BOOLEAN) AS is_shadow,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(i.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
i.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(i.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(i.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
i.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.locinstitution i;
CREATE VIEW uc.interface_type AS
SELECT
uuid_generate_v5(i.id, it.ordinality::text)::text AS id,
i.id AS interface_id,
CAST(it.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_organizations_storage.interfaces i, jsonb_array_elements_text(i.jsonb->'type') WITH ORDINALITY it (jsonb);
CREATE VIEW uc.interfaces AS
SELECT
i.id AS id,
i.jsonb->>'name' AS name,
i.jsonb->>'uri' AS uri,
i.jsonb->>'notes' AS notes,
CAST(i.jsonb->>'available' AS BOOLEAN) AS available,
i.jsonb->>'deliveryMethod' AS delivery_method,
i.jsonb->>'statisticsFormat' AS statistics_format,
i.jsonb->>'locallyStored' AS locally_stored,
i.jsonb->>'onlineLocation' AS online_location,
i.jsonb->>'statisticsNotes' AS statistics_notes,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(i.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
i.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(i.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(i.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
i.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_organizations_storage.interfaces i;
CREATE VIEW uc.interface_credentials AS
SELECT
ic.id AS id,
ic.jsonb->>'username' AS username,
ic.jsonb->>'password' AS password,
CAST(ic.jsonb->>'interfaceId' AS UUID) AS interface_id,
jsonb_pretty(ic.jsonb) AS content
FROM uchicago_mod_organizations_storage.interface_credentials ic;
CREATE VIEW uc.invoice_adjustment_fund_distributions AS
SELECT
uuid_generate_v5(i.id, ia.ordinality::text || '-' || iafd.ordinality::text)::text AS id,
uuid_generate_v5(i.id, ia.ordinality::text)::text AS invoice_adjustment_id,
iafd.jsonb->>'code' AS code,
CAST(iafd.jsonb->>'encumbrance' AS UUID) AS encumbrance_id,
CAST(iafd.jsonb->>'fundId' AS UUID) AS fund_id,
CAST(iafd.jsonb->>'expenseClassId' AS UUID) AS expense_class_id,
iafd.jsonb->>'distributionType' AS distribution_type,
CAST(iafd.jsonb->>'value' AS DECIMAL(19,2)) AS value
FROM uchicago_mod_invoice_storage.invoices i, jsonb_array_elements(i.jsonb->'adjustments') WITH ORDINALITY ia (jsonb), jsonb_array_elements(ia.jsonb->'fundDistributions') WITH ORDINALITY iafd (jsonb);
CREATE VIEW uc.invoice_adjustments AS
SELECT
uuid_generate_v5(i.id, ia.ordinality::text)::text AS id,
i.id AS invoice_id,
CAST(ia.jsonb->>'id' AS UUID) AS id2,
CAST(ia.jsonb->>'adjustmentId' AS UUID) AS adjustment_id,
ia.jsonb->>'description' AS description,
CAST(ia.jsonb->>'exportToAccounting' AS BOOLEAN) AS export_to_accounting,
ia.jsonb->>'prorate' AS prorate,
ia.jsonb->>'relationToTotal' AS relation_to_total,
ia.jsonb->>'type' AS type,
CAST(ia.jsonb->>'value' AS DECIMAL(19,2)) AS value,
CAST(ia.jsonb->>'totalAmount' AS DECIMAL(19,2)) AS total_amount
FROM uchicago_mod_invoice_storage.invoices i, jsonb_array_elements(i.jsonb->'adjustments') WITH ORDINALITY ia (jsonb);
CREATE VIEW uc.invoice_order_numbers AS
SELECT
uuid_generate_v5(i.id, ion.ordinality::text)::text AS id,
i.id AS invoice_id,
CAST(ion.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_invoice_storage.invoices i, jsonb_array_elements_text(i.jsonb->'poNumbers') WITH ORDINALITY ion (jsonb);
CREATE VIEW uc.invoice_acquisitions_units AS
SELECT
uuid_generate_v5(i.id, iau.ordinality::text)::text AS id,
i.id AS invoice_id,
CAST(iau.jsonb AS UUID) AS acquisitions_unit_id
FROM uchicago_mod_invoice_storage.invoices i, jsonb_array_elements_text(i.jsonb->'acqUnitIds') WITH ORDINALITY iau (jsonb);
CREATE VIEW uc.invoice_tags AS
SELECT
uuid_generate_v5(i.id, it.ordinality::text)::text AS id,
i.id AS invoice_id,
CAST(it.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_invoice_storage.invoices i, jsonb_array_elements_text(i.jsonb#>'{tags,tagList}') WITH ORDINALITY it (jsonb);
CREATE VIEW uc.invoices AS
SELECT
i.id AS id,
i.jsonb->>'accountingCode' AS accounting_code,
CAST(i.jsonb->>'adjustmentsTotal' AS DECIMAL(19,2)) AS adjustments_total,
CAST(i.jsonb->>'approvedBy' AS UUID) AS approved_by_id,
uc.DATE_CAST(i.jsonb->>'approvalDate') AS approval_date,
CAST(i.jsonb->>'batchGroupId' AS UUID) AS batch_group_id,
CAST(i.jsonb->>'billTo' AS UUID) AS bill_to_id,
CAST(i.jsonb->>'chkSubscriptionOverlap' AS BOOLEAN) AS chk_subscription_overlap,
i.jsonb->>'cancellationNote' AS cancellation_note,
i.jsonb->>'currency' AS currency,
CAST(i.jsonb->>'enclosureNeeded' AS BOOLEAN) AS enclosure_needed,
CAST(i.jsonb->>'exchangeRate' AS DECIMAL(19,2)) AS exchange_rate,
i.jsonb->>'operationMode' AS operation_mode,
CAST(i.jsonb->>'exportToAccounting' AS BOOLEAN) AS export_to_accounting,
i.jsonb->>'folioInvoiceNo' AS folio_invoice_no,
uc.DATE_CAST(i.jsonb->>'invoiceDate') AS invoice_date,
CAST(i.jsonb->>'lockTotal' AS DECIMAL(19,2)) AS lock_total,
i.jsonb->>'note' AS note,
uc.DATE_CAST(i.jsonb->>'paymentDue') AS payment_due,
uc.DATE_CAST(i.jsonb->>'paymentDate') AS payment_date,
i.jsonb->>'paymentTerms' AS payment_terms,
i.jsonb->>'paymentMethod' AS payment_method,
i.jsonb->>'status' AS status,
i.jsonb->>'source' AS source,
CAST(i.jsonb->>'subTotal' AS DECIMAL(19,2)) AS sub_total,
CAST(i.jsonb->>'total' AS DECIMAL(19,2)) AS total,
i.jsonb->>'vendorInvoiceNo' AS vendor_invoice_no,
i.jsonb->>'disbursementNumber' AS disbursement_number,
i.jsonb->>'voucherNumber' AS voucher_number,
CAST(i.jsonb->>'paymentId' AS UUID) AS payment_id,
uc.DATE_CAST(i.jsonb->>'disbursementDate') AS disbursement_date,
CAST(i.jsonb->>'vendorId' AS UUID) AS vendor_id,
CAST(i.jsonb->>'fiscalYearId' AS UUID) AS fiscal_year_id,
i.jsonb->>'accountNo' AS account_no,
CAST(i.jsonb->>'manualPayment' AS BOOLEAN) AS manual_payment,
CAST(i.jsonb->>'nextInvoiceLineNumber' AS INTEGER) AS next_invoice_line_number,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(i.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
i.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(i.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(i.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
i.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_invoice_storage.invoices i;
CREATE VIEW uc.invoice_item_adjustment_fund_distributions AS
SELECT
uuid_generate_v5(ii.id, iia.ordinality::text || '-' || iiafd.ordinality::text)::text AS id,
uuid_generate_v5(ii.id, iia.ordinality::text)::text AS invoice_item_adjustment_id,
iiafd.jsonb->>'code' AS code,
CAST(iiafd.jsonb->>'encumbrance' AS UUID) AS encumbrance_id,
CAST(iiafd.jsonb->>'fundId' AS UUID) AS fund_id,
CAST(iiafd.jsonb->>'expenseClassId' AS UUID) AS expense_class_id,
iiafd.jsonb->>'distributionType' AS distribution_type,
CAST(iiafd.jsonb->>'value' AS DECIMAL(19,2)) AS value
FROM uchicago_mod_invoice_storage.invoice_lines ii, jsonb_array_elements(ii.jsonb->'adjustments') WITH ORDINALITY iia (jsonb), jsonb_array_elements(iia.jsonb->'fundDistributions') WITH ORDINALITY iiafd (jsonb);
CREATE VIEW uc.invoice_item_adjustments AS
SELECT
uuid_generate_v5(ii.id, iia.ordinality::text)::text AS id,
ii.id AS invoice_item_id,
CAST(iia.jsonb->>'id' AS UUID) AS id2,
CAST(iia.jsonb->>'adjustmentId' AS UUID) AS adjustment_id,
iia.jsonb->>'description' AS description,
CAST(iia.jsonb->>'exportToAccounting' AS BOOLEAN) AS export_to_accounting,
iia.jsonb->>'prorate' AS prorate,
iia.jsonb->>'relationToTotal' AS relation_to_total,
iia.jsonb->>'type' AS type,
CAST(iia.jsonb->>'value' AS DECIMAL(19,2)) AS value,
CAST(iia.jsonb->>'totalAmount' AS DECIMAL(19,2)) AS total_amount
FROM uchicago_mod_invoice_storage.invoice_lines ii, jsonb_array_elements(ii.jsonb->'adjustments') WITH ORDINALITY iia (jsonb);
CREATE VIEW uc.invoice_item_fund_distributions AS
SELECT
uuid_generate_v5(ii.id, iifd.ordinality::text)::text AS id,
ii.id AS invoice_item_id,
iifd.jsonb->>'code' AS code,
CAST(iifd.jsonb->>'encumbrance' AS UUID) AS encumbrance_id,
CAST(iifd.jsonb->>'fundId' AS UUID) AS fund_id,
CAST(iifd.jsonb->>'expenseClassId' AS UUID) AS expense_class_id,
iifd.jsonb->>'distributionType' AS distribution_type,
CAST(iifd.jsonb->>'value' AS DECIMAL(19,2)) AS value
FROM uchicago_mod_invoice_storage.invoice_lines ii, jsonb_array_elements(ii.jsonb->'fundDistributions') WITH ORDINALITY iifd (jsonb);
CREATE VIEW uc.invoice_item_reference_numbers AS
SELECT
uuid_generate_v5(ii.id, iirn.ordinality::text)::text AS id,
ii.id AS invoice_item_id,
iirn.jsonb->>'refNumber' AS ref_number,
iirn.jsonb->>'refNumberType' AS ref_number_type,
iirn.jsonb->>'vendorDetailsSource' AS vendor_details_source
FROM uchicago_mod_invoice_storage.invoice_lines ii, jsonb_array_elements(ii.jsonb->'referenceNumbers') WITH ORDINALITY iirn (jsonb);
CREATE VIEW uc.invoice_item_tags AS
SELECT
uuid_generate_v5(ii.id, iit.ordinality::text)::text AS id,
ii.id AS invoice_item_id,
CAST(iit.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_invoice_storage.invoice_lines ii, jsonb_array_elements_text(ii.jsonb#>'{tags,tagList}') WITH ORDINALITY iit (jsonb);
CREATE VIEW uc.invoice_items AS
SELECT
ii.id AS id,
ii.jsonb->>'accountingCode' AS accounting_code,
ii.jsonb->>'accountNumber' AS account_number,
CAST(ii.jsonb->>'adjustmentsTotal' AS DECIMAL(19,2)) AS adjustments_total,
ii.jsonb->>'comment' AS comment,
ii.jsonb->>'description' AS description,
CAST(ii.jsonb->>'invoiceId' AS UUID) AS invoice_id,
ii.jsonb->>'invoiceLineNumber' AS invoice_line_number,
ii.jsonb->>'invoiceLineStatus' AS invoice_line_status,
CAST(ii.jsonb->>'poLineId' AS UUID) AS po_line_id,
ii.jsonb->>'productId' AS product_id,
CAST(ii.jsonb->>'productIdType' AS UUID) AS product_id_type_id,
CAST(ii.jsonb->>'quantity' AS INTEGER) AS quantity,
CAST(ii.jsonb->>'releaseEncumbrance' AS BOOLEAN) AS release_encumbrance,
ii.jsonb->>'subscriptionInfo' AS subscription_info,
uc.DATE_CAST(ii.jsonb->>'subscriptionStart') AS subscription_start,
uc.DATE_CAST(ii.jsonb->>'subscriptionEnd') AS subscription_end,
CAST(ii.jsonb->>'subTotal' AS DECIMAL(19,2)) AS sub_total,
CAST(ii.jsonb->>'total' AS DECIMAL(19,2)) AS total,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(ii.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
ii.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(ii.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(ii.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
ii.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_invoice_storage.invoice_lines ii;
CREATE VIEW uc.item_former_ids AS
SELECT
uuid_generate_v5(i.id, ifi.ordinality::text)::text AS id,
i.id AS item_id,
CAST(ifi.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_inventory_storage.item i, jsonb_array_elements_text(i.jsonb->'formerIds') WITH ORDINALITY ifi (jsonb);
CREATE VIEW uc.item_additional_call_numbers AS
SELECT
uuid_generate_v5(i.id, iacn.ordinality::text)::text AS id,
i.id AS item_id,
CAST(iacn.jsonb->>'typeId' AS UUID) AS type_id,
iacn.jsonb->>'prefix' AS prefix,
iacn.jsonb->>'callNumber' AS call_number,
iacn.jsonb->>'suffix' AS suffix
FROM uchicago_mod_inventory_storage.item i, jsonb_array_elements(i.jsonb->'additionalCallNumbers') WITH ORDINALITY iacn (jsonb);
CREATE VIEW uc.item_year_caption AS
SELECT
uuid_generate_v5(i.id, iyc.ordinality::text)::text AS id,
i.id AS item_id,
CAST(iyc.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_inventory_storage.item i, jsonb_array_elements_text(i.jsonb->'yearCaption') WITH ORDINALITY iyc (jsonb);
CREATE VIEW uc.item_administrative_notes AS
SELECT
uuid_generate_v5(i.id, ian.ordinality::text)::text AS id,
i.id AS item_id,
CAST(ian.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_inventory_storage.item i, jsonb_array_elements_text(i.jsonb->'administrativeNotes') WITH ORDINALITY ian (jsonb);
CREATE VIEW uc.item_notes AS
SELECT
uuid_generate_v5(i.id, "in".ordinality::text)::text AS id,
i.id AS item_id,
CAST("in".jsonb->>'itemNoteTypeId' AS UUID) AS item_note_type_id,
"in".jsonb->>'note' AS note,
CAST("in".jsonb->>'staffOnly' AS BOOLEAN) AS staff_only,
uc.TIMESTAMP_CAST(i.jsonb#>>'{metadata,createdDate}' || CASE WHEN i.jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(i.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
uc.TIMESTAMP_CAST(i.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(i.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id
FROM uchicago_mod_inventory_storage.item i, jsonb_array_elements(i.jsonb->'notes') WITH ORDINALITY "in" (jsonb);
CREATE VIEW uc.circulation_notes AS
SELECT
uuid_generate_v5(i.id, icn.ordinality::text)::text AS id,
i.id AS item_id,
icn.jsonb->>'id' AS id2,
icn.jsonb->>'noteType' AS note_type,
icn.jsonb->>'note' AS note,
icn.jsonb#>>'{source,id}' AS source_id,
icn.jsonb#>>'{source,personal,lastName}' AS source_personal_last_name,
icn.jsonb#>>'{source,personal,firstName}' AS source_personal_first_name,
icn.jsonb->>'date' AS date,
CAST(icn.jsonb->>'staffOnly' AS BOOLEAN) AS staff_only
FROM uchicago_mod_inventory_storage.item i, jsonb_array_elements(i.jsonb->'circulationNotes') WITH ORDINALITY icn (jsonb);
CREATE VIEW uc.item_electronic_accesses AS
SELECT
uuid_generate_v5(i.id, iea.ordinality::text)::text AS id,
i.id AS item_id,
iea.jsonb->>'uri' AS uri,
iea.jsonb->>'linkText' AS link_text,
iea.jsonb->>'materialsSpecification' AS materials_specification,
iea.jsonb->>'publicNote' AS public_note,
CAST(iea.jsonb->>'relationshipId' AS UUID) AS relationship_id
FROM uchicago_mod_inventory_storage.item i, jsonb_array_elements(i.jsonb->'electronicAccess') WITH ORDINALITY iea (jsonb);
CREATE VIEW uc.item_statistical_codes AS
SELECT
uuid_generate_v5(i.id, isc.ordinality::text)::text AS id,
i.id AS item_id,
CAST(isc.jsonb AS UUID) AS statistical_code_id
FROM uchicago_mod_inventory_storage.item i, jsonb_array_elements_text(i.jsonb->'statisticalCodeIds') WITH ORDINALITY isc (jsonb);
CREATE VIEW uc.item_tags AS
SELECT
uuid_generate_v5(i.id, it.ordinality::text)::text AS id,
i.id AS item_id,
CAST(it.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_inventory_storage.item i, jsonb_array_elements_text(i.jsonb#>'{tags,tagList}') WITH ORDINALITY it (jsonb);
CREATE VIEW uc.items AS
SELECT
i.id AS id,
CAST(i.jsonb->>'_version' AS INTEGER) AS _version,
CAST(i.jsonb->>'hrid' AS INTEGER) AS hrid,
CAST(i.jsonb->>'holdingsRecordId' AS UUID) AS holding_id,
CAST(i.jsonb->>'order' AS INTEGER) AS order,
CAST(i.jsonb->>'discoverySuppress' AS BOOLEAN) AS discovery_suppress,
i.jsonb->>'displaySummary' AS display_summary,
i.jsonb->>'accessionNumber' AS accession_number,
i.jsonb->>'barcode' AS barcode,
i.jsonb->>'effectiveShelvingOrder' AS effective_shelving_order,
i.jsonb->>'itemLevelCallNumber' AS call_number,
i.jsonb->>'itemLevelCallNumberPrefix' AS call_number_prefix,
i.jsonb->>'itemLevelCallNumberSuffix' AS call_number_suffix,
CAST(i.jsonb->>'itemLevelCallNumberTypeId' AS UUID) AS call_number_type_id,
i.jsonb#>>'{effectiveCallNumberComponents,callNumber}' AS effective_call_number,
i.jsonb#>>'{effectiveCallNumberComponents,prefix}' AS effective_call_number_prefix,
i.jsonb#>>'{effectiveCallNumberComponents,suffix}' AS effective_call_number_suffix,
CAST(i.jsonb#>>'{effectiveCallNumberComponents,typeId}' AS UUID) AS effective_call_number_type_id,
i.jsonb->>'volume' AS volume,
i.jsonb->>'enumeration' AS enumeration,
i.jsonb->>'chronology' AS chronology,
i.jsonb->>'itemIdentifier' AS item_identifier,
i.jsonb->>'copyNumber' AS copy_number,
i.jsonb->>'numberOfPieces' AS number_of_pieces,
i.jsonb->>'descriptionOfPieces' AS description_of_pieces,
i.jsonb->>'numberOfMissingPieces' AS number_of_missing_pieces,
i.jsonb->>'missingPieces' AS missing_pieces,
uc.TIMESTAMP_CAST(i.jsonb->>'missingPiecesDate') AS missing_pieces_date,
CAST(i.jsonb->>'itemDamagedStatusId' AS UUID) AS item_damaged_status_id,
uc.TIMESTAMP_CAST(i.jsonb->>'itemDamagedStatusDate') AS item_damaged_status_date,
i.jsonb#>>'{status,name}' AS status_name,
uc.TIMESTAMP_CAST(i.jsonb#>>'{status,date}') AS status_date,
CAST(i.jsonb->>'materialTypeId' AS UUID) AS material_type_id,
CAST(i.jsonb->>'permanentLoanTypeId' AS UUID) AS permanent_loan_type_id,
CAST(i.jsonb->>'temporaryLoanTypeId' AS UUID) AS temporary_loan_type_id,
CAST(i.jsonb->>'permanentLocationId' AS UUID) AS permanent_location_id,
CAST(i.jsonb->>'temporaryLocationId' AS UUID) AS temporary_location_id,
CAST(i.jsonb->>'effectiveLocationId' AS UUID) AS effective_location_id,
CAST(i.jsonb->>'inTransitDestinationServicePointId' AS UUID) AS in_transit_destination_service_point_id,
CAST(i.jsonb->>'purchaseOrderLineIdentifier' AS UUID) AS order_item_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(i.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
i.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(i.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(i.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
i.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
uc.TIMESTAMP_CAST(i.jsonb#>>'{lastCheckIn,dateTime}') AS last_check_in_date_time,
CAST(i.jsonb#>>'{lastCheckIn,servicePointId}' AS UUID) AS last_check_in_service_point_id,
CAST(i.jsonb#>>'{lastCheckIn,staffMemberId}' AS UUID) AS last_check_in_staff_member_id,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.item i;
CREATE VIEW uc.item_damaged_statuses AS
SELECT
ids.id AS id,
ids.jsonb->>'name' AS name,
ids.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(ids.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
ids.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(ids.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(ids.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
ids.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.item_damaged_status ids;
CREATE VIEW uc.item_note_types AS
SELECT
int.id AS id,
int.jsonb->>'name' AS name,
int.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(int.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
int.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(int.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(int.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
int.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.item_note_type int;
CREATE VIEW uc.ledger_acquisitions_units AS
SELECT
uuid_generate_v5(l.id, lau.ordinality::text)::text AS id,
l.id AS ledger_id,
CAST(lau.jsonb AS UUID) AS acquisitions_unit_id
FROM uchicago_mod_finance_storage.ledger l, jsonb_array_elements_text(l.jsonb->'acqUnitIds') WITH ORDINALITY lau (jsonb);
CREATE VIEW uc.ledgers AS
SELECT
l.id AS id,
CAST(l.jsonb->>'_version' AS INTEGER) AS _version,
l.jsonb->>'name' AS name,
l.jsonb->>'code' AS code,
l.jsonb->>'description' AS description,
CAST(l.jsonb->>'fiscalYearOneId' AS UUID) AS fiscal_year_one_id,
l.jsonb->>'ledgerStatus' AS ledger_status,
CAST(l.jsonb->>'allocated' AS DECIMAL(19,2)) AS allocated,
CAST(l.jsonb->>'available' AS DECIMAL(19,2)) AS available,
CAST(l.jsonb->>'netTransfers' AS DECIMAL(19,2)) AS net_transfers,
CAST(l.jsonb->>'unavailable' AS DECIMAL(19,2)) AS unavailable,
l.jsonb->>'currency' AS currency,
CAST(l.jsonb->>'restrictEncumbrance' AS BOOLEAN) AS restrict_encumbrance,
CAST(l.jsonb->>'restrictExpenditures' AS BOOLEAN) AS restrict_expenditures,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(l.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
l.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(l.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(l.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
l.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
CAST(l.jsonb->>'initialAllocation' AS DECIMAL(19,2)) AS initial_allocation,
CAST(l.jsonb->>'allocationTo' AS DECIMAL(19,2)) AS allocation_to,
CAST(l.jsonb->>'allocationFrom' AS DECIMAL(19,2)) AS allocation_from,
CAST(l.jsonb->>'totalFunding' AS DECIMAL(19,2)) AS total_funding,
CAST(l.jsonb->>'cashBalance' AS DECIMAL(19,2)) AS cash_balance,
CAST(l.jsonb->>'awaitingPayment' AS DECIMAL(19,2)) AS awaiting_payment,
CAST(l.jsonb->>'credits' AS DECIMAL(19,2)) AS credits,
CAST(l.jsonb->>'encumbered' AS DECIMAL(19,2)) AS encumbered,
CAST(l.jsonb->>'expenditures' AS DECIMAL(19,2)) AS expenditures,
CAST(l.jsonb->>'overEncumbrance' AS DECIMAL(19,2)) AS over_encumbrance,
CAST(l.jsonb->>'overExpended' AS DECIMAL(19,2)) AS over_expended,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_finance_storage.ledger l;
CREATE VIEW uc.libraries AS
SELECT
l.id AS id,
l.jsonb->>'name' AS name,
l.jsonb->>'code' AS code,
CAST(l.jsonb->>'campusId' AS UUID) AS campus_id,
CAST(l.jsonb->>'isShadow' AS BOOLEAN) AS is_shadow,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(l.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
l.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(l.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(l.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
l.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.loclibrary l;
CREATE VIEW uc.loans AS
SELECT
l.id AS id,
CAST(l.jsonb->>'userId' AS UUID) AS user_id,
CAST(l.jsonb->>'proxyUserId' AS UUID) AS proxy_user_id,
CAST(l.jsonb->>'itemId' AS UUID) AS item_id,
CAST(l.jsonb->>'itemEffectiveLocationIdAtCheckOut' AS UUID) AS item_effective_location_at_check_out_id,
l.jsonb#>>'{status,name}' AS status_name,
uc.TIMESTAMP_CAST(l.jsonb->>'loanDate') AS loan_date,
uc.TIMESTAMP_CAST(l.jsonb->>'dueDate') AS due_date,
l.jsonb->>'returnDate' AS return_date,
uc.TIMESTAMP_CAST(l.jsonb->>'systemReturnDate') AS system_return_date,
l.jsonb->>'action' AS action,
l.jsonb->>'actionComment' AS action_comment,
l.jsonb->>'itemStatus' AS item_status,
CAST(l.jsonb->>'renewalCount' AS INTEGER) AS renewal_count,
CAST(l.jsonb->>'loanPolicyId' AS UUID) AS loan_policy_id,
CAST(l.jsonb->>'checkoutServicePointId' AS UUID) AS checkout_service_point_id,
CAST(l.jsonb->>'checkinServicePointId' AS UUID) AS checkin_service_point_id,
CAST(l.jsonb->>'patronGroupIdAtCheckout' AS UUID) AS group_id,
CAST(l.jsonb->>'dueDateChangedByRecall' AS BOOLEAN) AS due_date_changed_by_recall,
CAST(l.jsonb->>'isDcb' AS BOOLEAN) AS is_dcb,
uc.DATE_CAST(l.jsonb->>'declaredLostDate') AS declared_lost_date,
uc.DATE_CAST(l.jsonb->>'claimedReturnedDate') AS claimed_returned_date,
CAST(l.jsonb->>'overdueFinePolicyId' AS UUID) AS overdue_fine_policy_id,
CAST(l.jsonb->>'lostItemPolicyId' AS UUID) AS lost_item_policy_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(l.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
l.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(l.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(l.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
l.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
CAST(l.jsonb#>>'{agedToLostDelayedBilling,lostItemHasBeenBilled}' AS BOOLEAN) AS aged_to_lost_delayed_billing_lost_item_has_been_billed,
uc.TIMESTAMP_CAST(l.jsonb#>>'{agedToLostDelayedBilling,dateLostItemShouldBeBilled}') AS aged_to_lost_delayed_billing_date_lost_item_should_be_billed,
uc.DATE_CAST(l.jsonb#>>'{agedToLostDelayedBilling,agedToLostDate}') AS aged_to_lost_delayed_billing_aged_to_lost_date,
CAST(l.jsonb#>>'{reminders,lastFeeBilled,number}' AS INTEGER) AS reminders_last_fee_billed_number,
uc.DATE_CAST(l.jsonb#>>'{reminders,lastFeeBilled,date}') AS reminders_last_fee_billed_date,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_circulation_storage.loan l;
CREATE VIEW uc.loan_events AS
SELECT
le.id AS id,
le.jsonb->>'operation' AS operation,
le.jsonb->>'creationDate' AS creation_date,
CAST(le.jsonb#>>'{loan,userId}' AS UUID) AS loan_user_id,
CAST(le.jsonb#>>'{loan,proxyUserId}' AS UUID) AS loan_proxy_user_id,
CAST(le.jsonb#>>'{loan,itemId}' AS UUID) AS loan_item_id,
CAST(le.jsonb#>>'{loan,itemEffectiveLocationIdAtCheckOut}' AS UUID) AS loan_item_effective_location_id_at_check_out_id,
le.jsonb#>>'{loan,status,name}' AS loan_status_name,
le.jsonb#>>'{loan,loanDate}' AS loan_loan_date,
uc.DATE_CAST(le.jsonb#>>'{loan,dueDate}') AS loan_due_date,
le.jsonb#>>'{loan,returnDate}' AS loan_return_date,
uc.DATE_CAST(le.jsonb#>>'{loan,systemReturnDate}') AS loan_system_return_date,
le.jsonb#>>'{loan,action}' AS loan_action,
le.jsonb#>>'{loan,actionComment}' AS loan_action_comment,
le.jsonb#>>'{loan,itemStatus}' AS loan_item_status,
CAST(le.jsonb#>>'{loan,renewalCount}' AS INTEGER) AS loan_renewal_count,
CAST(le.jsonb#>>'{loan,loanPolicyId}' AS UUID) AS loan_loan_policy_id,
CAST(le.jsonb#>>'{loan,checkoutServicePointId}' AS UUID) AS loan_checkout_service_point_id,
CAST(le.jsonb#>>'{loan,checkinServicePointId}' AS UUID) AS loan_checkin_service_point_id,
le.jsonb#>>'{loan,patronGroupIdAtCheckout}' AS loan_patron_group_id_at_checkout,
CAST(le.jsonb#>>'{loan,dueDateChangedByRecall}' AS BOOLEAN) AS loan_due_date_changed_by_recall,
CAST(le.jsonb#>>'{loan,isDcb}' AS BOOLEAN) AS loan_is_dcb,
uc.DATE_CAST(le.jsonb#>>'{loan,declaredLostDate}') AS loan_declared_lost_date,
uc.DATE_CAST(le.jsonb#>>'{loan,claimedReturnedDate}') AS loan_claimed_returned_date,
CAST(le.jsonb#>>'{loan,overdueFinePolicyId}' AS UUID) AS loan_overdue_fine_policy_id,
CAST(le.jsonb#>>'{loan,lostItemPolicyId}' AS UUID) AS loan_lost_item_policy_id,
uc.DATE_CAST(le.jsonb#>>'{loan,metadata,createdDate}') AS loan_metadata_created_date,
CAST(le.jsonb#>>'{loan,metadata,createdByUserId}' AS UUID) AS loan_metadata_created_by_user_id,
le.jsonb#>>'{loan,metadata,createdByUsername}' AS loan_metadata_created_by_username,
uc.DATE_CAST(le.jsonb#>>'{loan,metadata,updatedDate}') AS loan_metadata_updated_date,
CAST(le.jsonb#>>'{loan,metadata,updatedByUserId}' AS UUID) AS loan_metadata_updated_by_user_id,
le.jsonb#>>'{loan,metadata,updatedByUsername}' AS loan_metadata_updated_by_username,
CAST(le.jsonb#>>'{loan,agedToLostDelayedBilling,lostItemHasBeenBilled}' AS BOOLEAN) AS loan_aged_to_lost_delayed_billing_lost_item_has_been_billed,
uc.TIMESTAMP_CAST(le.jsonb#>>'{loan,agedToLostDelayedBilling,dateLostItemShouldBeBilled}') AS loan_aged_to_lost_delayed_billing_date_lost_item_should_be_billed,
uc.DATE_CAST(le.jsonb#>>'{loan,agedToLostDelayedBilling,agedToLostDate}') AS loan_aged_to_lost_delayed_billing_aged_to_lost_date,
CAST(le.jsonb#>>'{loan,reminders,lastFeeBilled,number}' AS INTEGER) AS loan_reminders_last_fee_billed_number,
uc.DATE_CAST(le.jsonb#>>'{loan,reminders,lastFeeBilled,date}') AS loan_reminders_last_fee_billed_date,
jsonb_pretty(le.jsonb) AS content
FROM uchicago_mod_circulation_storage.audit_loan le;
CREATE VIEW uc.loan_policies AS
SELECT
lp.id AS id,
lp.jsonb->>'name' AS name,
lp.jsonb->>'description' AS description,
CAST(lp.jsonb->>'loanable' AS BOOLEAN) AS loanable,
lp.jsonb#>>'{loansPolicy,profileId}' AS loans_policy_profile_id,
CAST(lp.jsonb#>>'{loansPolicy,period,duration}' AS INTEGER) AS loans_policy_period_duration,
lp.jsonb#>>'{loansPolicy,period,intervalId}' AS loans_policy_period_interval_id,
lp.jsonb#>>'{loansPolicy,closedLibraryDueDateManagementId}' AS loans_policy_closed_library_due_date_management_id,
CAST(lp.jsonb#>>'{loansPolicy,gracePeriod,duration}' AS INTEGER) AS loans_policy_grace_period_duration,
lp.jsonb#>>'{loansPolicy,gracePeriod,intervalId}' AS loans_policy_grace_period_interval_id,
CAST(lp.jsonb#>>'{loansPolicy,openingTimeOffset,duration}' AS INTEGER) AS loans_policy_opening_time_offset_duration,
lp.jsonb#>>'{loansPolicy,openingTimeOffset,intervalId}' AS loans_policy_opening_time_offset_interval_id,
CAST(lp.jsonb#>>'{loansPolicy,fixedDueDateScheduleId}' AS UUID) AS loans_policy_fixed_due_date_schedule_id,
CAST(lp.jsonb#>>'{loansPolicy,itemLimit}' AS INTEGER) AS loans_policy_item_limit,
CAST(lp.jsonb->>'renewable' AS BOOLEAN) AS renewable,
CAST(lp.jsonb#>>'{renewalsPolicy,unlimited}' AS BOOLEAN) AS renewals_policy_unlimited,
CAST(lp.jsonb#>>'{renewalsPolicy,numberAllowed}' AS DECIMAL(19,2)) AS renewals_policy_number_allowed,
lp.jsonb#>>'{renewalsPolicy,renewFromId}' AS renewals_policy_renew_from_id,
CAST(lp.jsonb#>>'{renewalsPolicy,differentPeriod}' AS BOOLEAN) AS renewals_policy_different_period,
CAST(lp.jsonb#>>'{renewalsPolicy,period,duration}' AS INTEGER) AS renewals_policy_period_duration,
lp.jsonb#>>'{renewalsPolicy,period,intervalId}' AS renewals_policy_period_interval_id,
CAST(lp.jsonb#>>'{renewalsPolicy,alternateFixedDueDateScheduleId}' AS UUID) AS renewals_policy_alternate_fixed_due_date_schedule_id,
CAST(lp.jsonb#>>'{requestManagement,recalls,alternateGracePeriod,duration}' AS INTEGER) AS recalls_alternate_grace_period_duration,
lp.jsonb#>>'{requestManagement,recalls,alternateGracePeriod,intervalId}' AS recalls_alternate_grace_period_interval_id,
CAST(lp.jsonb#>>'{requestManagement,recalls,minimumGuaranteedLoanPeriod,duration}' AS INTEGER) AS recalls_minimum_guaranteed_loan_period_duration,
lp.jsonb#>>'{requestManagement,recalls,minimumGuaranteedLoanPeriod,intervalId}' AS recalls_minimum_guaranteed_loan_period_interval_id,
CAST(lp.jsonb#>>'{requestManagement,recalls,recallReturnInterval,duration}' AS INTEGER) AS recalls_recall_return_interval_duration,
lp.jsonb#>>'{requestManagement,recalls,recallReturnInterval,intervalId}' AS recalls_recall_return_interval_interval_id,
CAST(lp.jsonb#>>'{requestManagement,recalls,allowRecallsToExtendOverdueLoans}' AS BOOLEAN) AS recalls_allow_recalls_to_extend_overdue_loans,
CAST(lp.jsonb#>>'{requestManagement,recalls,alternateRecallReturnInterval,duration}' AS INTEGER) AS recalls_alternate_recall_return_interval_duration,
lp.jsonb#>>'{requestManagement,recalls,alternateRecallReturnInterval,intervalId}' AS recalls_alternate_recall_return_interval_interval_id,
CAST(lp.jsonb#>>'{requestManagement,holds,alternateCheckoutLoanPeriod,duration}' AS INTEGER) AS holds_alternate_checkout_loan_period_duration,
lp.jsonb#>>'{requestManagement,holds,alternateCheckoutLoanPeriod,intervalId}' AS holds_alternate_checkout_loan_period_interval_id,
CAST(lp.jsonb#>>'{requestManagement,holds,renewItemsWithRequest}' AS BOOLEAN) AS holds_renew_items_with_request,
CAST(lp.jsonb#>>'{requestManagement,holds,alternateRenewalLoanPeriod,duration}' AS INTEGER) AS holds_alternate_renewal_loan_period_duration,
lp.jsonb#>>'{requestManagement,holds,alternateRenewalLoanPeriod,intervalId}' AS holds_alternate_renewal_loan_period_interval_id,
CAST(lp.jsonb#>>'{requestManagement,pages,alternateCheckoutLoanPeriod,duration}' AS INTEGER) AS pages_alternate_checkout_loan_period_duration,
lp.jsonb#>>'{requestManagement,pages,alternateCheckoutLoanPeriod,intervalId}' AS pages_alternate_checkout_loan_period_interval_id,
CAST(lp.jsonb#>>'{requestManagement,pages,renewItemsWithRequest}' AS BOOLEAN) AS pages_renew_items_with_request,
CAST(lp.jsonb#>>'{requestManagement,pages,alternateRenewalLoanPeriod,duration}' AS INTEGER) AS pages_alternate_renewal_loan_period_duration,
lp.jsonb#>>'{requestManagement,pages,alternateRenewalLoanPeriod,intervalId}' AS pages_alternate_renewal_loan_period_interval_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(lp.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
lp.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(lp.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(lp.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
lp.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_circulation_storage.loan_policy lp;
CREATE VIEW uc.loan_types AS
SELECT
lt.id AS id,
lt.jsonb->>'name' AS name,
lt.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(lt.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
lt.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(lt.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(lt.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
lt.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.loan_type lt;
CREATE VIEW uc.location_service_points AS
SELECT
uuid_generate_v5(l.id, lsp.ordinality::text)::text AS id,
l.id AS location_id,
CAST(lsp.jsonb AS UUID) AS service_point_id
FROM uchicago_mod_inventory_storage.location l, jsonb_array_elements_text(l.jsonb->'servicePointIds') WITH ORDINALITY lsp (jsonb);
CREATE VIEW uc.locations AS
SELECT
l.id AS id,
l.jsonb->>'name' AS name,
l.jsonb->>'code' AS code,
l.jsonb->>'description' AS description,
l.jsonb->>'discoveryDisplayName' AS discovery_display_name,
CAST(l.jsonb->>'isActive' AS BOOLEAN) AS is_active,
CAST(l.jsonb->>'institutionId' AS UUID) AS institution_id,
CAST(l.jsonb->>'campusId' AS UUID) AS campus_id,
CAST(l.jsonb->>'libraryId' AS UUID) AS library_id,
CAST(l.jsonb->>'primaryServicePoint' AS UUID) AS primary_service_point_id,
CAST(l.jsonb->>'isFloatingCollection' AS BOOLEAN) AS is_floating_collection,
CAST(l.jsonb->>'isShadow' AS BOOLEAN) AS is_shadow,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(l.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
l.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(l.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(l.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
l.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.location l;
CREATE VIEW uc.logins AS
SELECT
l.id AS id,
CAST(l.jsonb->>'userId' AS UUID) AS user_id,
l.jsonb->>'hash' AS hash,
l.jsonb->>'salt' AS salt,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(l.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
l.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(l.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(l.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
l.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_login.auth_credentials l;
CREATE VIEW uc.lost_item_fee_policies AS
SELECT
lifp.id AS id,
lifp.jsonb->>'name' AS name,
lifp.jsonb->>'description' AS description,
CAST(lifp.jsonb#>>'{itemAgedLostOverdue,duration}' AS INTEGER) AS item_aged_lost_overdue_duration,
lifp.jsonb#>>'{itemAgedLostOverdue,intervalId}' AS item_aged_lost_overdue_interval_id,
CAST(lifp.jsonb#>>'{patronBilledAfterAgedLost,duration}' AS INTEGER) AS patron_billed_after_aged_lost_duration,
lifp.jsonb#>>'{patronBilledAfterAgedLost,intervalId}' AS patron_billed_after_aged_lost_interval_id,
CAST(lifp.jsonb#>>'{recalledItemAgedLostOverdue,duration}' AS INTEGER) AS recalled_item_aged_lost_overdue_duration,
lifp.jsonb#>>'{recalledItemAgedLostOverdue,intervalId}' AS recalled_item_aged_lost_overdue_interval_id,
CAST(lifp.jsonb#>>'{patronBilledAfterRecalledItemAgedLost,duration}' AS INTEGER) AS patron_billed_after_recalled_item_aged_lost_duration,
lifp.jsonb#>>'{patronBilledAfterRecalledItemAgedLost,intervalId}' AS patron_billed_after_recalled_item_aged_lost_interval_id,
lifp.jsonb#>>'{chargeAmountItem,chargeType}' AS charge_amount_item_charge_type,
CAST(lifp.jsonb#>>'{chargeAmountItem,amount}' AS DECIMAL(19,2)) AS charge_amount_item_amount,
CAST(lifp.jsonb->>'lostItemProcessingFee' AS DECIMAL(19,2)) AS lost_item_processing_fee,
CAST(lifp.jsonb->>'chargeAmountItemPatron' AS BOOLEAN) AS charge_amount_item_patron,
CAST(lifp.jsonb->>'chargeAmountItemSystem' AS BOOLEAN) AS charge_amount_item_system,
CAST(lifp.jsonb#>>'{lostItemChargeFeeFine,duration}' AS INTEGER) AS lost_item_charge_fee_fine_duration,
lifp.jsonb#>>'{lostItemChargeFeeFine,intervalId}' AS lost_item_charge_fee_fine_interval_id,
CAST(lifp.jsonb->>'returnedLostItemProcessingFee' AS BOOLEAN) AS returned_lost_item_processing_fee,
CAST(lifp.jsonb->>'replacedLostItemProcessingFee' AS BOOLEAN) AS replaced_lost_item_processing_fee,
CAST(lifp.jsonb->>'replacementProcessingFee' AS DECIMAL(19,2)) AS replacement_processing_fee,
CAST(lifp.jsonb->>'replacementAllowed' AS BOOLEAN) AS replacement_allowed,
lifp.jsonb->>'lostItemReturned' AS lost_item_returned,
CAST(lifp.jsonb#>>'{feesFinesShallRefunded,duration}' AS INTEGER) AS fees_fines_shall_refunded_duration,
lifp.jsonb#>>'{feesFinesShallRefunded,intervalId}' AS fees_fines_shall_refunded_interval_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(lifp.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
lifp.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(lifp.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(lifp.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
lifp.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_feesfines.lost_item_fee_policy lifp;
CREATE VIEW uc.manual_block_templates AS
SELECT
mbt.id AS id,
mbt.jsonb->>'name' AS name,
mbt.jsonb->>'code' AS code,
mbt.jsonb->>'desc' AS desc,
mbt.jsonb#>>'{blockTemplate,desc}' AS block_template_desc,
mbt.jsonb#>>'{blockTemplate,patronMessage}' AS block_template_patron_message,
CAST(mbt.jsonb#>>'{blockTemplate,borrowing}' AS BOOLEAN) AS block_template_borrowing,
CAST(mbt.jsonb#>>'{blockTemplate,renewals}' AS BOOLEAN) AS block_template_renewals,
CAST(mbt.jsonb#>>'{blockTemplate,requests}' AS BOOLEAN) AS block_template_requests,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(mbt.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
mbt.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(mbt.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(mbt.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
mbt.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_feesfines.manual_block_templates mbt;
CREATE VIEW uc.marc_records AS
SELECT
mr.id AS id,
jsonb_pretty(mr.content) AS content
FROM uchicago_mod_source_record_storage.marc_records_lb mr;
CREATE VIEW uc.material_types AS
SELECT
mt.id AS id,
mt.jsonb->>'name' AS name,
mt.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(mt.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
mt.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(mt.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(mt.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
mt.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.material_type mt;
CREATE VIEW uc.mode_of_issuances AS
SELECT
moi.id AS id,
moi.jsonb->>'name' AS name,
moi.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(moi.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
moi.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(moi.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(moi.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
moi.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.mode_of_issuance moi;
CREATE VIEW uc.nature_of_content_terms AS
SELECT
noct.id AS id,
noct.jsonb->>'name' AS name,
noct.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(noct.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
noct.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(noct.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(noct.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
noct.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.nature_of_content_term noct;
CREATE VIEW uc.notes AS
SELECT
n.id AS id,
n.title AS title,
n.content AS content,
n.indexed_content AS indexed_content,
n.domain AS domain,
n.type_id AS type_id,
n.pop_up_on_user AS pop_up_on_user,
n.pop_up_on_check_out AS pop_up_on_check_out,
n.created_by AS creation_user_id,
n.created_date AS creation_time,
n.updated_by AS updated_by_user_id,
n.updated_date AS last_write_time
FROM uchicago_mod_notes.note n;
CREATE VIEW uc.note_types AS
SELECT
nt.id AS id,
nt.name AS name,
nt.created_by AS creation_user_id,
nt.created_date AS creation_time,
nt.updated_by AS updated_by_user_id,
nt.updated_date AS last_write_time
FROM uchicago_mod_notes.type nt;
CREATE VIEW uc.order_notes AS
SELECT
uuid_generate_v5(o.id, "on".ordinality::text)::text AS id,
o.id AS order_id,
CAST("on".jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_orders_storage.purchase_order o, jsonb_array_elements_text(o.jsonb->'notes') WITH ORDINALITY "on" (jsonb);
CREATE VIEW uc.order_acquisitions_units AS
SELECT
uuid_generate_v5(o.id, oau.ordinality::text)::text AS id,
o.id AS order_id,
CAST(oau.jsonb AS UUID) AS acquisitions_unit_id
FROM uchicago_mod_orders_storage.purchase_order o, jsonb_array_elements_text(o.jsonb->'acqUnitIds') WITH ORDINALITY oau (jsonb);
CREATE VIEW uc.order_tags AS
SELECT
uuid_generate_v5(o.id, ot.ordinality::text)::text AS id,
o.id AS order_id,
CAST(ot.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_orders_storage.purchase_order o, jsonb_array_elements_text(o.jsonb#>'{tags,tagList}') WITH ORDINALITY ot (jsonb);
CREATE VIEW uc.orders AS
SELECT
o.id AS id,
CAST(o.jsonb->>'approved' AS BOOLEAN) AS approved,
CAST(o.jsonb->>'approvedById' AS UUID) AS approved_by_id,
uc.DATE_CAST(o.jsonb->>'approvalDate') AS approval_date,
CAST(o.jsonb->>'assignedTo' AS UUID) AS assigned_to_id,
CAST(o.jsonb->>'billTo' AS UUID) AS bill_to_id,
o.jsonb#>>'{closeReason,reason}' AS close_reason_reason,
o.jsonb#>>'{closeReason,note}' AS close_reason_note,
CAST(o.jsonb->>'openedById' AS UUID) AS opened_by_id,
uc.DATE_CAST(o.jsonb->>'dateOrdered') AS date_ordered,
CAST(o.jsonb->>'manualPo' AS BOOLEAN) AS manual_po,
o.jsonb->>'poNumber' AS po_number,
o.jsonb->>'poNumberPrefix' AS po_number_prefix,
o.jsonb->>'poNumberSuffix' AS po_number_suffix,
o.jsonb->>'orderType' AS order_type,
CAST(o.jsonb->>'reEncumber' AS BOOLEAN) AS re_encumber,
CAST(o.jsonb#>>'{ongoing,interval}' AS INTEGER) AS ongoing_interval,
CAST(o.jsonb#>>'{ongoing,isSubscription}' AS BOOLEAN) AS ongoing_is_subscription,
CAST(o.jsonb#>>'{ongoing,manualRenewal}' AS BOOLEAN) AS ongoing_manual_renewal,
o.jsonb#>>'{ongoing,notes}' AS ongoing_notes,
CAST(o.jsonb#>>'{ongoing,reviewPeriod}' AS INTEGER) AS ongoing_review_period,
uc.DATE_CAST(o.jsonb#>>'{ongoing,renewalDate}') AS ongoing_renewal_date,
uc.DATE_CAST(o.jsonb#>>'{ongoing,reviewDate}') AS ongoing_review_date,
CAST(o.jsonb->>'shipTo' AS UUID) AS ship_to_id,
CAST(o.jsonb->>'template' AS UUID) AS template_id,
CAST(o.jsonb->>'vendor' AS UUID) AS vendor_id,
o.jsonb->>'workflowStatus' AS status,
CAST(o.jsonb->>'nextPolNumber' AS INTEGER) AS next_pol_number,
CAST(o.jsonb->>'fiscalYearId' AS UUID) AS fiscal_year_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(o.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
o.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(o.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(o.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
o.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_orders_storage.purchase_order o;
CREATE VIEW uc.order_invoices AS
SELECT
oi.id AS id,
CAST(oi.jsonb->>'purchaseOrderId' AS UUID) AS order_id,
CAST(oi.jsonb->>'invoiceId' AS UUID) AS invoice_id,
jsonb_pretty(oi.jsonb) AS content
FROM uchicago_mod_orders_storage.order_invoice_relationship oi;
CREATE VIEW uc.order_item_claims AS
SELECT
uuid_generate_v5(oi.id, oic.ordinality::text)::text AS id,
oi.id AS order_item_id,
CAST(oic.jsonb->>'claimed' AS BOOLEAN) AS claimed,
uc.TIMESTAMP_CAST(oic.jsonb->>'sent') AS sent,
CAST(oic.jsonb->>'grace' AS INTEGER) AS grace
FROM uchicago_mod_orders_storage.po_line oi, jsonb_array_elements(oi.jsonb->'claims') WITH ORDINALITY oic (jsonb);
CREATE VIEW uc.order_item_contributors AS
SELECT
uuid_generate_v5(oi.id, oic.ordinality::text)::text AS id,
oi.id AS order_item_id,
oic.jsonb->>'contributor' AS contributor,
CAST(oic.jsonb->>'contributorNameTypeId' AS UUID) AS contributor_name_type_id
FROM uchicago_mod_orders_storage.po_line oi, jsonb_array_elements(oi.jsonb->'contributors') WITH ORDINALITY oic (jsonb);
CREATE VIEW uc.order_item_product_ids AS
SELECT
uuid_generate_v5(oi.id, oipi.ordinality::text)::text AS id,
oi.id AS order_item_id,
oipi.jsonb->>'productId' AS product_id,
CAST(oipi.jsonb->>'productIdType' AS UUID) AS product_id_type_id,
oipi.jsonb->>'qualifier' AS qualifier
FROM uchicago_mod_orders_storage.po_line oi, jsonb_array_elements(oi.jsonb#>'{details,productIds}') WITH ORDINALITY oipi (jsonb);
CREATE VIEW uc.order_item_organizations AS
SELECT
uuid_generate_v5(oi.id, oio.ordinality::text)::text AS id,
oi.id AS order_item_id,
CAST(oio.jsonb AS UUID) AS organization_id
FROM uchicago_mod_orders_storage.po_line oi, jsonb_array_elements_text(oi.jsonb->'donorOrganizationIds') WITH ORDINALITY oio (jsonb);
CREATE VIEW uc.order_item_fund_distributions AS
SELECT
uuid_generate_v5(oi.id, oifd.ordinality::text)::text AS id,
oi.id AS order_item_id,
oifd.jsonb->>'code' AS code,
CAST(oifd.jsonb->>'encumbrance' AS UUID) AS encumbrance_id,
CAST(oifd.jsonb->>'fundId' AS UUID) AS fund_id,
CAST(oifd.jsonb->>'expenseClassId' AS UUID) AS expense_class_id,
oifd.jsonb->>'distributionType' AS distribution_type,
CAST(oifd.jsonb->>'value' AS DECIMAL(19,2)) AS value
FROM uchicago_mod_orders_storage.po_line oi, jsonb_array_elements(oi.jsonb->'fundDistribution') WITH ORDINALITY oifd (jsonb);
CREATE VIEW uc.order_item_locations AS
SELECT
uuid_generate_v5(oi.id, oil.ordinality::text)::text AS id,
oi.id AS order_item_id,
CAST(oil.jsonb->>'locationId' AS UUID) AS location_id,
CAST(oil.jsonb->>'holdingId' AS UUID) AS holding_id,
CAST(oil.jsonb->>'quantity' AS INTEGER) AS quantity,
CAST(oil.jsonb->>'quantityElectronic' AS INTEGER) AS quantity_electronic,
CAST(oil.jsonb->>'quantityPhysical' AS INTEGER) AS quantity_physical,
CAST(oil.jsonb->>'tenantId' AS UUID) AS tenant_id
FROM uchicago_mod_orders_storage.po_line oi, jsonb_array_elements(oi.jsonb->'locations') WITH ORDINALITY oil (jsonb);
CREATE VIEW uc.order_item_search_locations AS
SELECT
uuid_generate_v5(oi.id, oisl.ordinality::text)::text AS id,
oi.id AS order_item_id,
CAST(oisl.jsonb AS UUID) AS location_id
FROM uchicago_mod_orders_storage.po_line oi, jsonb_array_elements_text(oi.jsonb->'searchLocationIds') WITH ORDINALITY oisl (jsonb);
CREATE VIEW uc.order_item_volumes AS
SELECT
uuid_generate_v5(oi.id, oiv.ordinality::text)::text AS id,
oi.id AS order_item_id,
CAST(oiv.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_orders_storage.po_line oi, jsonb_array_elements_text(oi.jsonb#>'{physical,volumes}') WITH ORDINALITY oiv (jsonb);
CREATE VIEW uc.order_item_tags AS
SELECT
uuid_generate_v5(oi.id, oit.ordinality::text)::text AS id,
oi.id AS order_item_id,
CAST(oit.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_orders_storage.po_line oi, jsonb_array_elements_text(oi.jsonb#>'{tags,tagList}') WITH ORDINALITY oit (jsonb);
CREATE VIEW uc.order_item_reference_numbers AS
SELECT
uuid_generate_v5(oi.id, oirn.ordinality::text)::text AS id,
oi.id AS order_item_id,
oirn.jsonb->>'refNumber' AS ref_number,
oirn.jsonb->>'refNumberType' AS ref_number_type,
oirn.jsonb->>'vendorDetailsSource' AS vendor_details_source
FROM uchicago_mod_orders_storage.po_line oi, jsonb_array_elements(oi.jsonb#>'{vendorDetail,referenceNumbers}') WITH ORDINALITY oirn (jsonb);
CREATE VIEW uc.order_items AS
SELECT
oi.id AS id,
oi.jsonb->>'edition' AS edition,
CAST(oi.jsonb->>'checkinItems' AS BOOLEAN) AS checkin_items,
CAST(oi.jsonb->>'agreementId' AS UUID) AS agreement_id,
CAST(oi.jsonb->>'acquisitionMethod' AS UUID) AS acquisition_method_id,
CAST(oi.jsonb->>'automaticExport' AS BOOLEAN) AS automatic_export,
CAST(oi.jsonb->>'cancellationRestriction' AS BOOLEAN) AS cancellation_restriction,
oi.jsonb->>'cancellationRestrictionNote' AS cancellation_restriction_note,
CAST(oi.jsonb->>'claimingActive' AS BOOLEAN) AS claiming_active,
CAST(oi.jsonb->>'claimingInterval' AS INTEGER) AS claiming_interval,
CAST(oi.jsonb->>'collection' AS BOOLEAN) AS collection,
CAST(oi.jsonb#>>'{cost,listUnitPrice}' AS DECIMAL(19,2)) AS cost_list_unit_price,
CAST(oi.jsonb#>>'{cost,listUnitPriceElectronic}' AS DECIMAL(19,2)) AS cost_list_unit_price_electronic,
oi.jsonb#>>'{cost,currency}' AS cost_currency,
CAST(oi.jsonb#>>'{cost,additionalCost}' AS DECIMAL(19,2)) AS cost_additional_cost,
CAST(oi.jsonb#>>'{cost,discount}' AS DECIMAL(19,2)) AS cost_discount,
oi.jsonb#>>'{cost,discountType}' AS cost_discount_type,
CAST(oi.jsonb#>>'{cost,exchangeRate}' AS DECIMAL(19,2)) AS cost_exchange_rate,
CAST(oi.jsonb#>>'{cost,quantityPhysical}' AS INTEGER) AS cost_quantity_physical,
CAST(oi.jsonb#>>'{cost,quantityElectronic}' AS INTEGER) AS cost_quantity_electronic,
CAST(oi.jsonb#>>'{cost,poLineEstimatedPrice}' AS DECIMAL(19,2)) AS cost_po_line_estimated_price,
CAST(oi.jsonb#>>'{cost,fyroAdjustmentAmount}' AS DECIMAL(19,2)) AS cost_fyro_adjustment_amount,
oi.jsonb->>'description' AS description,
oi.jsonb#>>'{details,receivingNote}' AS details_receiving_note,
CAST(oi.jsonb#>>'{details,isAcknowledged}' AS BOOLEAN) AS details_is_acknowledged,
CAST(oi.jsonb#>>'{details,isBinderyActive}' AS BOOLEAN) AS details_is_bindery_active,
uc.TIMESTAMP_CAST(oi.jsonb#>>'{details,subscriptionFrom}') AS details_subscription_from,
CAST(oi.jsonb#>>'{details,subscriptionInterval}' AS INTEGER) AS details_subscription_interval,
uc.TIMESTAMP_CAST(oi.jsonb#>>'{details,subscriptionTo}') AS details_subscription_to,
oi.jsonb->>'donor' AS donor,
CAST(oi.jsonb#>>'{eresource,activated}' AS BOOLEAN) AS eresource_activated,
CAST(oi.jsonb#>>'{eresource,activationDue}' AS INTEGER) AS eresource_activation_due,
oi.jsonb#>>'{eresource,createInventory}' AS eresource_create_inventory,
CAST(oi.jsonb#>>'{eresource,trial}' AS BOOLEAN) AS eresource_trial,
uc.DATE_CAST(oi.jsonb#>>'{eresource,expectedActivation}') AS eresource_expected_activation,
oi.jsonb#>>'{eresource,userLimit}' AS eresource_user_limit,
CAST(oi.jsonb#>>'{eresource,accessProvider}' AS UUID) AS eresource_access_provider_id,
oi.jsonb#>>'{eresource,license,code}' AS eresource_license_code,
oi.jsonb#>>'{eresource,license,description}' AS eresource_license_description,
oi.jsonb#>>'{eresource,license,reference}' AS eresource_license_reference,
CAST(oi.jsonb#>>'{eresource,materialType}' AS UUID) AS eresource_material_type_id,
oi.jsonb#>>'{eresource,resourceUrl}' AS eresource_resource_url,
CAST(oi.jsonb->>'instanceId' AS UUID) AS instance_id,
CAST(oi.jsonb->>'isPackage' AS BOOLEAN) AS is_package,
uc.DATE_CAST(oi.jsonb->>'lastEDIExportDate') AS last_edi_export_date,
oi.jsonb->>'orderFormat' AS order_format,
CAST(oi.jsonb->>'packagePoLineId' AS UUID) AS package_po_line_id,
oi.jsonb->>'paymentStatus' AS payment_status,
oi.jsonb#>>'{physical,createInventory}' AS physical_create_inventory,
CAST(oi.jsonb#>>'{physical,materialType}' AS UUID) AS physical_material_type_id,
CAST(oi.jsonb#>>'{physical,materialSupplier}' AS UUID) AS physical_material_supplier_id,
uc.DATE_CAST(oi.jsonb#>>'{physical,expectedReceiptDate}') AS physical_expected_receipt_date,
uc.TIMESTAMP_CAST(oi.jsonb#>>'{physical,receiptDue}') AS physical_receipt_due,
oi.jsonb->>'poLineDescription' AS po_line_description,
oi.jsonb->>'poLineNumber' AS po_line_number,
oi.jsonb->>'publicationDate' AS publication_year,
oi.jsonb->>'publisher' AS publisher,
CAST(oi.jsonb->>'purchaseOrderId' AS UUID) AS order_id,
uc.DATE_CAST(oi.jsonb->>'receiptDate') AS receipt_date,
oi.jsonb->>'receiptStatus' AS receipt_status,
oi.jsonb->>'renewalNote' AS renewal_note,
oi.jsonb->>'requester' AS requester,
CAST(oi.jsonb->>'rush' AS BOOLEAN) AS rush,
oi.jsonb->>'selector' AS selector,
oi.jsonb->>'source' AS source,
oi.jsonb->>'titleOrPackage' AS title_or_package,
oi.jsonb#>>'{vendorDetail,instructions}' AS vendor_detail_instructions,
oi.jsonb#>>'{vendorDetail,noteFromVendor}' AS vendor_detail_note_from_vendor,
oi.jsonb#>>'{vendorDetail,vendorAccount}' AS vendor_detail_vendor_account,
CAST(oi.jsonb->>'suppressInstanceFromDiscovery' AS BOOLEAN) AS suppress_instance_from_discovery,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(oi.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
oi.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(oi.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(oi.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
oi.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_orders_storage.po_line oi;
CREATE VIEW uc.organization_organization_types AS
SELECT
uuid_generate_v5(o.id, oot.ordinality::text)::text AS id,
o.id AS organization_id,
CAST(oot.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements_text(o.jsonb->'organizationTypes') WITH ORDINALITY oot (jsonb);
CREATE VIEW uc.organization_aliases AS
SELECT
uuid_generate_v5(o.id, oa.ordinality::text)::text AS id,
o.id AS organization_id,
oa.jsonb->>'value' AS value,
oa.jsonb->>'description' AS description
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements(o.jsonb->'aliases') WITH ORDINALITY oa (jsonb);
CREATE VIEW uc.organization_address_categories AS
SELECT
uuid_generate_v5(o.id, oa.ordinality::text || '-' || oac.ordinality::text)::text AS id,
uuid_generate_v5(o.id, oa.ordinality::text)::text AS organization_address_id,
CAST(oac.jsonb AS UUID) AS category_id
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements(o.jsonb->'addresses') WITH ORDINALITY oa (jsonb), jsonb_array_elements_text(oa.jsonb->'categories') WITH ORDINALITY oac (jsonb);
CREATE VIEW uc.organization_addresses AS
SELECT
uuid_generate_v5(o.id, oa.ordinality::text)::text AS id,
o.id AS organization_id,
CAST(oa.jsonb->>'id' AS UUID) AS id2,
oa.jsonb->>'addressLine1' AS address_line1,
oa.jsonb->>'addressLine2' AS address_line2,
oa.jsonb->>'city' AS city,
oa.jsonb->>'stateRegion' AS state_region,
oa.jsonb->>'zipCode' AS zip_code,
oa.jsonb->>'country' AS country,
CAST(oa.jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
oa.jsonb->>'language' AS language,
uc.TIMESTAMP_CAST(oa.jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(oa.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
oa.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(oa.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(oa.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
oa.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements(o.jsonb->'addresses') WITH ORDINALITY oa (jsonb);
CREATE VIEW uc.organization_phone_number_categories AS
SELECT
uuid_generate_v5(o.id, opn.ordinality::text || '-' || opnc.ordinality::text)::text AS id,
uuid_generate_v5(o.id, opn.ordinality::text)::text AS organization_phone_number_id,
CAST(opnc.jsonb AS UUID) AS category_id
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements(o.jsonb->'phoneNumbers') WITH ORDINALITY opn (jsonb), jsonb_array_elements_text(opn.jsonb->'categories') WITH ORDINALITY opnc (jsonb);
CREATE VIEW uc.organization_phone_numbers AS
SELECT
uuid_generate_v5(o.id, opn.ordinality::text)::text AS id,
o.id AS organization_id,
CAST(opn.jsonb->>'id' AS UUID) AS id2,
opn.jsonb->>'phoneNumber' AS phone_number,
opn.jsonb->>'type' AS type,
CAST(opn.jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
opn.jsonb->>'language' AS language,
uc.TIMESTAMP_CAST(opn.jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(opn.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
opn.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(opn.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(opn.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
opn.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements(o.jsonb->'phoneNumbers') WITH ORDINALITY opn (jsonb);
CREATE VIEW uc.organization_email_categories AS
SELECT
uuid_generate_v5(o.id, oe.ordinality::text || '-' || oec.ordinality::text)::text AS id,
uuid_generate_v5(o.id, oe.ordinality::text)::text AS organization_email_id,
CAST(oec.jsonb AS UUID) AS category_id
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements(o.jsonb->'emails') WITH ORDINALITY oe (jsonb), jsonb_array_elements_text(oe.jsonb->'categories') WITH ORDINALITY oec (jsonb);
CREATE VIEW uc.organization_emails AS
SELECT
uuid_generate_v5(o.id, oe.ordinality::text)::text AS id,
o.id AS organization_id,
CAST(oe.jsonb->>'id' AS UUID) AS id2,
oe.jsonb->>'value' AS value,
oe.jsonb->>'description' AS description,
CAST(oe.jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
oe.jsonb->>'language' AS language,
uc.TIMESTAMP_CAST(oe.jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(oe.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
oe.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(oe.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(oe.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
oe.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements(o.jsonb->'emails') WITH ORDINALITY oe (jsonb);
CREATE VIEW uc.organization_url_categories AS
SELECT
uuid_generate_v5(o.id, ou.ordinality::text || '-' || ouc.ordinality::text)::text AS id,
uuid_generate_v5(o.id, ou.ordinality::text)::text AS organization_url_id,
CAST(ouc.jsonb AS UUID) AS category_id
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements(o.jsonb->'urls') WITH ORDINALITY ou (jsonb), jsonb_array_elements_text(ou.jsonb->'categories') WITH ORDINALITY ouc (jsonb);
CREATE VIEW uc.organization_urls AS
SELECT
uuid_generate_v5(o.id, ou.ordinality::text)::text AS id,
o.id AS organization_id,
CAST(ou.jsonb->>'id' AS UUID) AS id2,
ou.jsonb->>'value' AS value,
ou.jsonb->>'description' AS description,
ou.jsonb->>'language' AS language,
CAST(ou.jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
ou.jsonb->>'notes' AS notes,
uc.TIMESTAMP_CAST(ou.jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(ou.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
ou.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(ou.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(ou.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
ou.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements(o.jsonb->'urls') WITH ORDINALITY ou (jsonb);
CREATE VIEW uc.organization_contacts AS
SELECT
uuid_generate_v5(o.id, oc.ordinality::text)::text AS id,
o.id AS organization_id,
CAST(oc.jsonb AS UUID) AS contact_id
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements_text(o.jsonb->'contacts') WITH ORDINALITY oc (jsonb);
CREATE VIEW uc.organization_privileged_contacts AS
SELECT
uuid_generate_v5(o.id, opc.ordinality::text)::text AS id,
o.id AS organization_id,
CAST(opc.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements_text(o.jsonb->'privilegedContacts') WITH ORDINALITY opc (jsonb);
CREATE VIEW uc.organization_agreement_orgs AS
SELECT
uuid_generate_v5(o.id, oa.ordinality::text || '-' || oao.ordinality::text)::text AS id,
uuid_generate_v5(o.id, oa.ordinality::text)::text AS organization_agreement_id,
CAST(oao.jsonb->>'primaryOrg' AS BOOLEAN) AS primary_org,
CAST(oao.jsonb#>>'{org,orgsUuid}' AS UUID) AS org_orgs_uuid_id
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements(o.jsonb->'agreements') WITH ORDINALITY oa (jsonb), jsonb_array_elements(oa.jsonb->'orgs') WITH ORDINALITY oao (jsonb);
CREATE VIEW uc.organization_agreement_periods AS
SELECT
uuid_generate_v5(o.id, oa.ordinality::text || '-' || oap.ordinality::text)::text AS id,
uuid_generate_v5(o.id, oa.ordinality::text)::text AS organization_agreement_id,
uc.DATE_CAST(oap.jsonb->>'startDate') AS start_date,
oap.jsonb->>'periodStatus' AS period_status
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements(o.jsonb->'agreements') WITH ORDINALITY oa (jsonb), jsonb_array_elements(oa.jsonb->'periods') WITH ORDINALITY oap (jsonb);
CREATE VIEW uc.organization_agreements AS
SELECT
uuid_generate_v5(o.id, oa.ordinality::text)::text AS id,
o.id AS organization_id,
oa.jsonb->>'id' AS id2,
CAST(oa.jsonb->>'name' AS VARCHAR(255)) AS name,
uc.DATE_CAST(oa.jsonb->>'startDate') AS start_date,
uc.DATE_CAST(oa.jsonb->>'endDate') AS end_date,
uc.TIMESTAMP_CAST(oa.jsonb->>'cancellationDeadline') AS cancellation_deadline,
oa.jsonb#>>'{agreementStatus,value}' AS agreement_status_value,
oa.jsonb#>>'{agreementStatus,label}' AS agreement_status_label,
oa.jsonb#>>'{isPerpetual,label}' AS is_perpetual_label,
oa.jsonb#>>'{renewalPriority,label}' AS renewal_priority_label,
oa.jsonb->>'description' AS description,
uc.DATE_CAST(oa.jsonb->>'dateCreated') AS date_created,
uc.TIMESTAMP_CAST(oa.jsonb->>'lastUpdated') AS last_updated
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements(o.jsonb->'agreements') WITH ORDINALITY oa (jsonb);
CREATE VIEW uc.currencies AS
SELECT
uuid_generate_v5(o.id, ovc.ordinality::text)::text AS id,
o.id AS organization_id,
CAST(ovc.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements_text(o.jsonb->'vendorCurrencies') WITH ORDINALITY ovc (jsonb);
CREATE VIEW uc.organization_interfaces AS
SELECT
uuid_generate_v5(o.id, oi.ordinality::text)::text AS id,
o.id AS organization_id,
CAST(oi.jsonb AS UUID) AS interface_id
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements_text(o.jsonb->'interfaces') WITH ORDINALITY oi (jsonb);
CREATE VIEW uc.organization_account_acquisitions_units AS
SELECT
uuid_generate_v5(o.id, oa.ordinality::text || '-' || oaau.ordinality::text)::text AS id,
uuid_generate_v5(o.id, oa.ordinality::text)::text AS organization_account_id,
CAST(oaau.jsonb AS UUID) AS acquisitions_unit_id
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements(o.jsonb->'accounts') WITH ORDINALITY oa (jsonb), jsonb_array_elements_text(oa.jsonb->'acqUnitIds') WITH ORDINALITY oaau (jsonb);
CREATE VIEW uc.organization_accounts AS
SELECT
uuid_generate_v5(o.id, oa.ordinality::text)::text AS id,
o.id AS organization_id,
oa.jsonb->>'name' AS name,
oa.jsonb->>'accountNo' AS account_no,
oa.jsonb->>'description' AS description,
oa.jsonb->>'appSystemNo' AS app_system_no,
oa.jsonb->>'paymentMethod' AS payment_method,
oa.jsonb->>'accountStatus' AS account_status,
oa.jsonb->>'contactInfo' AS contact_info,
oa.jsonb->>'libraryCode' AS library_code,
oa.jsonb->>'libraryEdiCode' AS library_edi_code,
oa.jsonb->>'notes' AS notes
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements(o.jsonb->'accounts') WITH ORDINALITY oa (jsonb);
CREATE VIEW uc.organization_changelogs AS
SELECT
uuid_generate_v5(o.id, oc.ordinality::text)::text AS id,
o.id AS organization_id,
oc.jsonb->>'description' AS description,
uc.TIMESTAMP_CAST(oc.jsonb->>'timestamp') AS timestamp
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements(o.jsonb->'changelogs') WITH ORDINALITY oc (jsonb);
CREATE VIEW uc.organization_acquisitions_units AS
SELECT
uuid_generate_v5(o.id, oau.ordinality::text)::text AS id,
o.id AS organization_id,
CAST(oau.jsonb AS UUID) AS acquisitions_unit_id
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements_text(o.jsonb->'acqUnitIds') WITH ORDINALITY oau (jsonb);
CREATE VIEW uc.organization_tags AS
SELECT
uuid_generate_v5(o.id, ot.ordinality::text)::text AS id,
o.id AS organization_id,
CAST(ot.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_organizations_storage.organizations o, jsonb_array_elements_text(o.jsonb#>'{tags,tagList}') WITH ORDINALITY ot (jsonb);
CREATE VIEW uc.organizations AS
SELECT
o.id AS id,
o.jsonb->>'name' AS name,
o.jsonb->>'code' AS code,
o.jsonb->>'description' AS description,
CAST(o.jsonb->>'exportToAccounting' AS BOOLEAN) AS export_to_accounting,
o.jsonb->>'status' AS status,
o.jsonb->>'language' AS language,
o.jsonb->>'erpCode' AS erp_code,
o.jsonb->>'paymentMethod' AS payment_method,
CAST(o.jsonb->>'accessProvider' AS BOOLEAN) AS access_provider,
CAST(o.jsonb->>'governmental' AS BOOLEAN) AS governmental,
CAST(o.jsonb->>'licensor' AS BOOLEAN) AS licensor,
CAST(o.jsonb->>'materialSupplier' AS BOOLEAN) AS material_supplier,
CAST(o.jsonb->>'claimingInterval' AS INTEGER) AS claiming_interval,
CAST(o.jsonb->>'discountPercent' AS DECIMAL(19,2)) AS discount_percent,
CAST(o.jsonb->>'expectedActivationInterval' AS INTEGER) AS expected_activation_interval,
CAST(o.jsonb->>'expectedInvoiceInterval' AS INTEGER) AS expected_invoice_interval,
CAST(o.jsonb->>'renewalActivationInterval' AS INTEGER) AS renewal_activation_interval,
CAST(o.jsonb->>'subscriptionInterval' AS INTEGER) AS subscription_interval,
CAST(o.jsonb->>'expectedReceiptInterval' AS INTEGER) AS expected_receipt_interval,
o.jsonb->>'taxId' AS tax_id,
CAST(o.jsonb->>'liableForVat' AS BOOLEAN) AS liable_for_vat,
CAST(o.jsonb->>'taxPercentage' AS DECIMAL(19,2)) AS tax_percentage,
o.jsonb#>>'{edi,vendorEdiCode}' AS edi_vendor_edi_code,
o.jsonb#>>'{edi,vendorEdiType}' AS edi_vendor_edi_type,
o.jsonb#>>'{edi,libEdiCode}' AS edi_lib_edi_code,
o.jsonb#>>'{edi,libEdiType}' AS edi_lib_edi_type,
CAST(o.jsonb#>>'{edi,prorateTax}' AS BOOLEAN) AS edi_prorate_tax,
CAST(o.jsonb#>>'{edi,prorateFees}' AS BOOLEAN) AS edi_prorate_fees,
o.jsonb#>>'{edi,ediNamingConvention}' AS edi_naming_convention,
CAST(o.jsonb#>>'{edi,sendAcctNum}' AS BOOLEAN) AS edi_send_acct_num,
CAST(o.jsonb#>>'{edi,supportOrder}' AS BOOLEAN) AS edi_support_order,
CAST(o.jsonb#>>'{edi,supportInvoice}' AS BOOLEAN) AS edi_support_invoice,
o.jsonb#>>'{edi,notes}' AS edi_notes,
o.jsonb#>>'{edi,ediFtp,ftpFormat}' AS edi_ftp_ftp_format,
o.jsonb#>>'{edi,ediFtp,serverAddress}' AS edi_ftp_server_address,
o.jsonb#>>'{edi,ediFtp,username}' AS edi_ftp_username,
o.jsonb#>>'{edi,ediFtp,password}' AS edi_ftp_password,
o.jsonb#>>'{edi,ediFtp,ftpMode}' AS edi_ftp_ftp_mode,
o.jsonb#>>'{edi,ediFtp,ftpConnMode}' AS edi_ftp_ftp_conn_mode,
CAST(o.jsonb#>>'{edi,ediFtp,ftpPort}' AS INTEGER) AS edi_ftp_ftp_port,
o.jsonb#>>'{edi,ediFtp,orderDirectory}' AS edi_ftp_order_directory,
o.jsonb#>>'{edi,ediFtp,invoiceDirectory}' AS edi_ftp_invoice_directory,
o.jsonb#>>'{edi,ediFtp,notes}' AS edi_ftp_notes,
CAST(o.jsonb#>>'{edi,ediJob,scheduleEdi}' AS BOOLEAN) AS edi_job_schedule_edi,
uc.DATE_CAST(o.jsonb#>>'{edi,ediJob,schedulingDate}') AS edi_job_scheduling_date,
o.jsonb#>>'{edi,ediJob,time}' AS edi_job_time,
CAST(o.jsonb#>>'{edi,ediJob,isMonday}' AS BOOLEAN) AS edi_job_is_monday,
CAST(o.jsonb#>>'{edi,ediJob,isTuesday}' AS BOOLEAN) AS edi_job_is_tuesday,
CAST(o.jsonb#>>'{edi,ediJob,isWednesday}' AS BOOLEAN) AS edi_job_is_wednesday,
CAST(o.jsonb#>>'{edi,ediJob,isThursday}' AS BOOLEAN) AS edi_job_is_thursday,
CAST(o.jsonb#>>'{edi,ediJob,isFriday}' AS BOOLEAN) AS edi_job_is_friday,
CAST(o.jsonb#>>'{edi,ediJob,isSaturday}' AS BOOLEAN) AS edi_job_is_saturday,
CAST(o.jsonb#>>'{edi,ediJob,isSunday}' AS BOOLEAN) AS edi_job_is_sunday,
o.jsonb#>>'{edi,ediJob,sendToEmails}' AS edi_job_send_to_emails,
CAST(o.jsonb#>>'{edi,ediJob,notifyAllEdi}' AS BOOLEAN) AS edi_job_notify_all_edi,
CAST(o.jsonb#>>'{edi,ediJob,notifyInvoiceOnly}' AS BOOLEAN) AS edi_job_notify_invoice_only,
CAST(o.jsonb#>>'{edi,ediJob,notifyErrorOnly}' AS BOOLEAN) AS edi_job_notify_error_only,
o.jsonb#>>'{edi,ediJob,schedulingNotes}' AS edi_job_scheduling_notes,
CAST(o.jsonb->>'isVendor' AS BOOLEAN) AS is_vendor,
CAST(o.jsonb->>'isDonor' AS BOOLEAN) AS is_donor,
o.jsonb->>'sanCode' AS san_code,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(o.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
o.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(o.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(o.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
o.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_organizations_storage.organizations o;
CREATE VIEW uc.overdue_fine_policies AS
SELECT
ofp.id AS id,
ofp.jsonb->>'name' AS name,
ofp.jsonb->>'description' AS description,
CAST(ofp.jsonb#>>'{overdueFine,quantity}' AS DECIMAL(19,2)) AS overdue_fine_quantity,
ofp.jsonb#>>'{overdueFine,intervalId}' AS overdue_fine_interval_id,
CAST(ofp.jsonb->>'countClosed' AS BOOLEAN) AS count_closed,
CAST(ofp.jsonb->>'maxOverdueFine' AS DECIMAL(19,2)) AS max_overdue_fine,
CAST(ofp.jsonb->>'forgiveOverdueFine' AS BOOLEAN) AS forgive_overdue_fine,
CAST(ofp.jsonb#>>'{overdueRecallFine,quantity}' AS DECIMAL(19,2)) AS overdue_recall_fine_quantity,
ofp.jsonb#>>'{overdueRecallFine,intervalId}' AS overdue_recall_fine_interval_id,
CAST(ofp.jsonb->>'gracePeriodRecall' AS BOOLEAN) AS grace_period_recall,
CAST(ofp.jsonb->>'maxOverdueRecallFine' AS DECIMAL(19,2)) AS max_overdue_recall_fine,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(ofp.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
ofp.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(ofp.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(ofp.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
ofp.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_feesfines.overdue_fine_policy ofp;
CREATE VIEW uc.service_point_owners AS
SELECT
uuid_generate_v5(o.id, spo.ordinality::text)::text AS id,
o.id AS owner_id,
CAST(spo.jsonb->>'value' AS UUID) AS service_point_id,
spo.jsonb->>'label' AS label
FROM uchicago_mod_feesfines.owners o, jsonb_array_elements(o.jsonb->'servicePointOwner') WITH ORDINALITY spo (jsonb);
CREATE VIEW uc.owners AS
SELECT
o.id AS id,
o.jsonb->>'owner' AS owner,
o.jsonb->>'desc' AS desc,
CAST(o.jsonb->>'defaultChargeNoticeId' AS UUID) AS default_charge_notice_id,
CAST(o.jsonb->>'defaultActionNoticeId' AS UUID) AS default_action_notice_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(o.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
o.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(o.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(o.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
o.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_feesfines.owners o;
CREATE VIEW uc.patron_action_sessions AS
SELECT
pas.id AS id,
CAST(pas.jsonb->>'sessionId' AS UUID) AS session_id,
CAST(pas.jsonb->>'patronId' AS UUID) AS patron_id,
CAST(pas.jsonb->>'loanId' AS UUID) AS loan_id,
pas.jsonb->>'actionType' AS action_type,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(pas.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
pas.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(pas.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(pas.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
pas.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_circulation_storage.patron_action_session pas;
CREATE VIEW uc.patron_notice_policy_loan_notices AS
SELECT
uuid_generate_v5(pnp.id, pnpln.ordinality::text)::text AS id,
pnp.id AS patron_notice_policy_id,
pnpln.jsonb->>'name' AS name,
CAST(pnpln.jsonb->>'templateId' AS UUID) AS template_id,
pnpln.jsonb->>'templateName' AS template_name,
pnpln.jsonb->>'format' AS format,
pnpln.jsonb->>'frequency' AS frequency,
CAST(pnpln.jsonb->>'realTime' AS BOOLEAN) AS real_time,
pnpln.jsonb#>>'{sendOptions,sendHow}' AS send_options_send_how,
pnpln.jsonb#>>'{sendOptions,sendWhen}' AS send_options_send_when,
CAST(pnpln.jsonb#>>'{sendOptions,sendBy,duration}' AS INTEGER) AS send_options_send_by_duration,
pnpln.jsonb#>>'{sendOptions,sendBy,intervalId}' AS send_options_send_by_interval_id,
CAST(pnpln.jsonb#>>'{sendOptions,sendEvery,duration}' AS INTEGER) AS send_options_send_every_duration,
pnpln.jsonb#>>'{sendOptions,sendEvery,intervalId}' AS send_options_send_every_interval_id
FROM uchicago_mod_circulation_storage.patron_notice_policy pnp, jsonb_array_elements(pnp.jsonb->'loanNotices') WITH ORDINALITY pnpln (jsonb);
CREATE VIEW uc.patron_notice_policy_fee_fine_notices AS
SELECT
uuid_generate_v5(pnp.id, pnpffn.ordinality::text)::text AS id,
pnp.id AS patron_notice_policy_id,
pnpffn.jsonb->>'name' AS name,
CAST(pnpffn.jsonb->>'templateId' AS UUID) AS template_id,
pnpffn.jsonb->>'templateName' AS template_name,
pnpffn.jsonb->>'format' AS format,
pnpffn.jsonb->>'frequency' AS frequency,
CAST(pnpffn.jsonb->>'realTime' AS BOOLEAN) AS real_time,
pnpffn.jsonb#>>'{sendOptions,sendHow}' AS send_options_send_how,
pnpffn.jsonb#>>'{sendOptions,sendWhen}' AS send_options_send_when,
CAST(pnpffn.jsonb#>>'{sendOptions,sendBy,duration}' AS INTEGER) AS send_options_send_by_duration,
pnpffn.jsonb#>>'{sendOptions,sendBy,intervalId}' AS send_options_send_by_interval_id,
CAST(pnpffn.jsonb#>>'{sendOptions,sendEvery,duration}' AS INTEGER) AS send_options_send_every_duration,
pnpffn.jsonb#>>'{sendOptions,sendEvery,intervalId}' AS send_options_send_every_interval_id
FROM uchicago_mod_circulation_storage.patron_notice_policy pnp, jsonb_array_elements(pnp.jsonb->'feeFineNotices') WITH ORDINALITY pnpffn (jsonb);
CREATE VIEW uc.patron_notice_policy_request_notices AS
SELECT
uuid_generate_v5(pnp.id, pnprn.ordinality::text)::text AS id,
pnp.id AS patron_notice_policy_id,
pnprn.jsonb->>'name' AS name,
CAST(pnprn.jsonb->>'templateId' AS UUID) AS template_id,
pnprn.jsonb->>'templateName' AS template_name,
pnprn.jsonb->>'format' AS format,
pnprn.jsonb->>'frequency' AS frequency,
CAST(pnprn.jsonb->>'realTime' AS BOOLEAN) AS real_time,
pnprn.jsonb#>>'{sendOptions,sendHow}' AS send_options_send_how,
pnprn.jsonb#>>'{sendOptions,sendWhen}' AS send_options_send_when,
CAST(pnprn.jsonb#>>'{sendOptions,sendBy,duration}' AS INTEGER) AS send_options_send_by_duration,
pnprn.jsonb#>>'{sendOptions,sendBy,intervalId}' AS send_options_send_by_interval_id,
CAST(pnprn.jsonb#>>'{sendOptions,sendEvery,duration}' AS INTEGER) AS send_options_send_every_duration,
pnprn.jsonb#>>'{sendOptions,sendEvery,intervalId}' AS send_options_send_every_interval_id
FROM uchicago_mod_circulation_storage.patron_notice_policy pnp, jsonb_array_elements(pnp.jsonb->'requestNotices') WITH ORDINALITY pnprn (jsonb);
CREATE VIEW uc.patron_notice_policies AS
SELECT
pnp.id AS id,
pnp.jsonb->>'name' AS name,
pnp.jsonb->>'description' AS description,
CAST(pnp.jsonb->>'active' AS BOOLEAN) AS active,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(pnp.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
pnp.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(pnp.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(pnp.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
pnp.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_circulation_storage.patron_notice_policy pnp;
CREATE VIEW uc.payments AS
SELECT
p.id AS id,
uc.TIMESTAMP_CAST(p.jsonb->>'dateAction') AS date_action,
p.jsonb->>'typeAction' AS type_action,
p.jsonb->>'comments' AS comments,
CAST(p.jsonb->>'notify' AS BOOLEAN) AS notify,
CAST(p.jsonb->>'amountAction' AS DECIMAL(19,2)) AS amount_action,
CAST(p.jsonb->>'balance' AS DECIMAL(19,2)) AS balance,
p.jsonb->>'transactionInformation' AS transaction_information,
p.jsonb->>'createdAt' AS service_point,
p.jsonb->>'source' AS source,
p.jsonb->>'paymentMethod' AS payment_method,
CAST(p.jsonb->>'accountId' AS UUID) AS fee_id,
CAST(p.jsonb->>'userId' AS UUID) AS user_id,
jsonb_pretty(p.jsonb) AS content
FROM uchicago_mod_feesfines.feefineactions p;
CREATE VIEW uc.payment_methods AS
SELECT
pm.id AS id,
pm.jsonb->>'nameMethod' AS name,
CAST(pm.jsonb->>'allowedRefundMethod' AS BOOLEAN) AS allowed_refund_method,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(pm.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
pm.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(pm.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(pm.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
pm.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
CAST(pm.jsonb->>'ownerId' AS UUID) AS owner_id,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_feesfines.payments pm;
CREATE VIEW uc.permission_tags AS
SELECT
uuid_generate_v5(p.id, pt.ordinality::text)::text AS id,
p.id AS permission_id,
CAST(pt.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_permissions.permissions p, jsonb_array_elements_text(p.jsonb->'tags') WITH ORDINALITY pt (jsonb);
CREATE VIEW uc.permission_sub_permissions AS
SELECT
uuid_generate_v5(p.id, psp.ordinality::text)::text AS id,
p.id AS permission_id,
CAST(psp.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_permissions.permissions p, jsonb_array_elements_text(p.jsonb->'subPermissions') WITH ORDINALITY psp (jsonb);
CREATE VIEW uc.permission_child_of AS
SELECT
uuid_generate_v5(p.id, pco.ordinality::text)::text AS id,
p.id AS permission_id,
CAST(pco.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_permissions.permissions p, jsonb_array_elements_text(p.jsonb->'childOf') WITH ORDINALITY pco (jsonb);
CREATE VIEW uc.permission_granted_to AS
SELECT
uuid_generate_v5(p.id, pgt.ordinality::text)::text AS id,
p.id AS permission_id,
CAST(pgt.jsonb AS UUID) AS permissions_user_id
FROM uchicago_mod_permissions.permissions p, jsonb_array_elements_text(p.jsonb->'grantedTo') WITH ORDINALITY pgt (jsonb);
CREATE VIEW uc.permissions AS
SELECT
p.id AS id,
p.jsonb->>'permissionName' AS permission_name,
p.jsonb->>'displayName' AS display_name,
p.jsonb->>'description' AS description,
CAST(p.jsonb->>'mutable' AS BOOLEAN) AS mutable,
CAST(p.jsonb->>'visible' AS BOOLEAN) AS visible,
CAST(p.jsonb->>'dummy' AS BOOLEAN) AS dummy,
CAST(p.jsonb->>'deprecated' AS BOOLEAN) AS deprecated,
p.jsonb->>'moduleName' AS module_name,
p.jsonb->>'moduleVersion' AS module_version,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(p.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
p.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(p.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(p.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
p.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_permissions.permissions p;
CREATE VIEW uc.permissions_user_permissions AS
SELECT
uuid_generate_v5(pu.id, pup.ordinality::text)::text AS id,
pu.id AS permissions_user_id,
CAST(pup.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_permissions.permissions_users pu, jsonb_array_elements_text(pu.jsonb->'permissions') WITH ORDINALITY pup (jsonb);
CREATE VIEW uc.permissions_users AS
SELECT
pu.id AS id,
CAST(pu.jsonb->>'userId' AS UUID) AS user_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(pu.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
pu.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(pu.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(pu.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
pu.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_permissions.permissions_users pu;
CREATE VIEW uc.preceding_succeeding_title_identifiers AS
SELECT
uuid_generate_v5(pst.id, psti.ordinality::text)::text AS id,
pst.id AS preceding_succeeding_title_id,
psti.jsonb->>'value' AS value,
CAST(psti.jsonb->>'identifierTypeId' AS UUID) AS identifier_type_id
FROM uchicago_mod_inventory_storage.preceding_succeeding_title pst, jsonb_array_elements(pst.jsonb->'identifiers') WITH ORDINALITY psti (jsonb);
CREATE VIEW uc.preceding_succeeding_titles AS
SELECT
pst.id AS id,
CAST(pst.jsonb->>'precedingInstanceId' AS UUID) AS preceding_instance_id,
CAST(pst.jsonb->>'succeedingInstanceId' AS UUID) AS succeeding_instance_id,
pst.jsonb->>'title' AS title,
pst.jsonb->>'hrid' AS hrid,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(pst.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
pst.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(pst.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(pst.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
pst.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.preceding_succeeding_title pst;
CREATE VIEW uc.proxies AS
SELECT
p.id AS id,
CAST(p.jsonb->>'userId' AS UUID) AS user_id,
CAST(p.jsonb->>'proxyUserId' AS UUID) AS proxy_user_id,
p.jsonb->>'requestForSponsor' AS request_for_sponsor,
p.jsonb->>'notificationsTo' AS notifications_to,
p.jsonb->>'accrueTo' AS accrue_to,
p.jsonb->>'status' AS status,
uc.DATE_CAST(p.jsonb->>'expirationDate') AS expiration_date,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(p.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
p.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(p.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(p.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
p.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_users.proxyfor p;
CREATE VIEW uc.raw_records AS
SELECT
rr.id AS id,
rr.content AS content2
FROM uchicago_mod_source_record_storage.raw_records_lb rr;
CREATE VIEW uc.receivings AS
SELECT
r.id AS id,
r.jsonb->>'displaySummary' AS display_summary,
r.jsonb->>'comment' AS comment,
r.jsonb->>'format' AS format,
CAST(r.jsonb->>'itemId' AS UUID) AS item_id,
CAST(r.jsonb->>'bindItemId' AS UUID) AS bind_item_id,
CAST(r.jsonb->>'bindItemTenantId' AS UUID) AS bind_item_tenant_id,
CAST(r.jsonb->>'locationId' AS UUID) AS location_id,
CAST(r.jsonb->>'poLineId' AS UUID) AS po_line_id,
CAST(r.jsonb->>'titleId' AS UUID) AS title_id,
CAST(r.jsonb->>'holdingId' AS UUID) AS holding_id,
CAST(r.jsonb->>'receivingTenantId' AS UUID) AS receiving_tenant_id,
CAST(r.jsonb->>'displayOnHolding' AS BOOLEAN) AS display_on_holding,
CAST(r.jsonb->>'displayToPublic' AS BOOLEAN) AS display_to_public,
r.jsonb->>'enumeration' AS enumeration,
r.jsonb->>'chronology' AS chronology,
r.jsonb->>'barcode' AS barcode,
r.jsonb->>'accessionNumber' AS accession_number,
r.jsonb->>'callNumber' AS call_number,
CAST(r.jsonb->>'discoverySuppress' AS BOOLEAN) AS discovery_suppress,
r.jsonb->>'copyNumber' AS copy_number,
r.jsonb->>'receivingStatus' AS receiving_status,
CAST(r.jsonb->>'supplement' AS BOOLEAN) AS supplement,
CAST(r.jsonb->>'isBound' AS BOOLEAN) AS is_bound,
uc.TIMESTAMP_CAST(r.jsonb->>'receiptDate') AS receipt_date,
uc.TIMESTAMP_CAST(r.jsonb->>'receivedDate') AS received_date,
uc.DATE_CAST(r.jsonb->>'statusUpdatedDate') AS status_updated_date,
CAST(r.jsonb->>'claimingInterval' AS INTEGER) AS claiming_interval,
r.jsonb->>'internalNote' AS internal_note,
r.jsonb->>'externalNote' AS external_note,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(r.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
r.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(r.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(r.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
r.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_orders_storage.pieces r;
CREATE VIEW uc.records AS
SELECT
r.id AS id,
r.snapshot_id AS snapshot_id,
r.matched_id AS matched_id,
r.generation AS generation,
r.record_type AS record_type,
r.external_id AS instance_id,
r.state AS state,
r.leader_record_status AS leader_record_status,
r."order" AS "order",
r.suppress_discovery AS suppress_discovery,
r.created_by_user_id AS creation_user_id,
r.created_date AS creation_time,
r.updated_by_user_id AS last_write_user_id,
r.updated_date AS last_write_time,
r.external_hrid AS instance_short_id
FROM uchicago_mod_source_record_storage.records_lb r;
CREATE VIEW uc.reference_datas AS
SELECT
rd.id AS id,
rd.jsonb->>'label' AS label,
rd.jsonb->>'value' AS value,
jsonb_pretty(rd.jsonb) AS content
FROM uc_agreements.reference_datas rd;
CREATE VIEW uc.refund_reasons AS
SELECT
rr.id AS id,
rr.jsonb->>'nameReason' AS name,
rr.jsonb->>'description' AS description,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(rr.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
rr.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(rr.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(rr.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
rr.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
CAST(rr.jsonb->>'accountId' AS UUID) AS account_id,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_feesfines.refunds rr;
CREATE VIEW uc.request_identifiers AS
SELECT
uuid_generate_v5(r.id, ri.ordinality::text)::text AS id,
r.id AS request_id,
ri.jsonb->>'value' AS value,
CAST(ri.jsonb->>'identifierTypeId' AS UUID) AS identifier_type_id
FROM uchicago_mod_circulation_storage.request r, jsonb_array_elements(r.jsonb#>'{instance,identifiers}') WITH ORDINALITY ri (jsonb);
CREATE VIEW uc.request_tags AS
SELECT
uuid_generate_v5(r.id, rt.ordinality::text)::text AS id,
r.id AS request_id,
CAST(rt.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_circulation_storage.request r, jsonb_array_elements_text(r.jsonb#>'{tags,tagList}') WITH ORDINALITY rt (jsonb);
CREATE VIEW uc.requests AS
SELECT
r.id AS id,
r.jsonb->>'requestLevel' AS request_level,
r.jsonb->>'requestType' AS request_type,
r.jsonb->>'ecsRequestPhase' AS ecs_request_phase,
uc.DATE_CAST(r.jsonb->>'requestDate') AS request_date,
r.jsonb->>'patronComments' AS patron_comments,
CAST(r.jsonb->>'requesterId' AS UUID) AS requester_id,
CAST(r.jsonb->>'proxyUserId' AS UUID) AS proxy_user_id,
CAST(r.jsonb->>'instanceId' AS UUID) AS instance_id,
CAST(r.jsonb->>'holdingsRecordId' AS UUID) AS holding_id,
CAST(r.jsonb->>'itemId' AS UUID) AS item_id,
r.jsonb->>'status' AS status,
CAST(r.jsonb->>'cancellationReasonId' AS UUID) AS cancellation_reason_id,
CAST(r.jsonb->>'cancelledByUserId' AS UUID) AS cancelled_by_user_id,
r.jsonb->>'cancellationAdditionalInformation' AS cancellation_additional_information,
uc.DATE_CAST(r.jsonb->>'cancelledDate') AS cancelled_date,
CAST(r.jsonb->>'position' AS INTEGER) AS position,
r.jsonb#>>'{instance,title}' AS instance_title,
r.jsonb#>>'{item,barcode}' AS item_barcode,
r.jsonb#>>'{requester,firstName}' AS requester_first_name,
r.jsonb#>>'{requester,lastName}' AS requester_last_name,
r.jsonb#>>'{requester,middleName}' AS requester_middle_name,
r.jsonb#>>'{requester,barcode}' AS requester_barcode,
r.jsonb#>>'{requester,patronGroup}' AS requester_patron_group,
r.jsonb#>>'{proxy,firstName}' AS proxy_first_name,
r.jsonb#>>'{proxy,lastName}' AS proxy_last_name,
r.jsonb#>>'{proxy,middleName}' AS proxy_middle_name,
r.jsonb#>>'{proxy,barcode}' AS proxy_barcode,
r.jsonb#>>'{proxy,patronGroup}' AS proxy_patron_group,
r.jsonb->>'fulfillmentPreference' AS fulfillment_preference,
CAST(r.jsonb->>'deliveryAddressTypeId' AS UUID) AS delivery_address_type_id,
uc.DATE_CAST(r.jsonb->>'requestExpirationDate') AS request_expiration_date,
uc.DATE_CAST(r.jsonb->>'holdShelfExpirationDate') AS hold_shelf_expiration_date,
CAST(r.jsonb->>'pickupServicePointId' AS UUID) AS pickup_service_point_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(r.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
r.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(r.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(r.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
r.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
CAST(r.jsonb#>>'{printDetails,printCount}' AS INTEGER) AS print_details_print_count,
CAST(r.jsonb#>>'{printDetails,requesterId}' AS UUID) AS print_details_requester_id,
CAST(r.jsonb#>>'{printDetails,isPrinted}' AS BOOLEAN) AS print_details_is_printed,
uc.DATE_CAST(r.jsonb#>>'{printDetails,printEventDate}') AS print_details_print_event_date,
uc.DATE_CAST(r.jsonb->>'awaitingPickupRequestClosedDate') AS awaiting_pickup_request_closed_date,
r.jsonb#>>'{searchIndex,callNumberComponents,callNumber}' AS search_index_call_number_components_call_number,
r.jsonb#>>'{searchIndex,callNumberComponents,prefix}' AS search_index_call_number_components_prefix,
r.jsonb#>>'{searchIndex,callNumberComponents,suffix}' AS search_index_call_number_components_suffix,
r.jsonb#>>'{searchIndex,shelvingOrder}' AS search_index_shelving_order,
r.jsonb#>>'{searchIndex,pickupServicePointName}' AS search_index_pickup_service_point_name,
r.jsonb->>'itemLocationCode' AS item_location_code,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_circulation_storage.request r;
CREATE VIEW uc.request_policy_request_types AS
SELECT
uuid_generate_v5(rp.id, rprt.ordinality::text)::text AS id,
rp.id AS request_policy_id,
CAST(rprt.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_circulation_storage.request_policy rp, jsonb_array_elements_text(rp.jsonb->'requestTypes') WITH ORDINALITY rprt (jsonb);
CREATE VIEW uc.request_policies AS
SELECT
rp.id AS id,
rp.jsonb->>'name' AS name,
rp.jsonb->>'description' AS description,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(rp.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
rp.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(rp.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(rp.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
rp.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_circulation_storage.request_policy rp;
CREATE VIEW uc.rollover_budgets_rollover AS
SELECT
uuid_generate_v5(r.id, rbr.ordinality::text)::text AS id,
r.id AS rollover_id,
CAST(rbr.jsonb->>'fundTypeId' AS UUID) AS fund_type_id,
CAST(rbr.jsonb->>'rolloverAllocation' AS BOOLEAN) AS rollover_allocation,
rbr.jsonb->>'rolloverBudgetValue' AS rollover_budget_value,
CAST(rbr.jsonb->>'setAllowances' AS BOOLEAN) AS set_allowances,
CAST(rbr.jsonb->>'adjustAllocation' AS DECIMAL(19,2)) AS adjust_allocation,
rbr.jsonb->>'addAvailableTo' AS add_available_to,
CAST(rbr.jsonb->>'allowableEncumbrance' AS DECIMAL(19,2)) AS allowable_encumbrance,
CAST(rbr.jsonb->>'allowableExpenditure' AS DECIMAL(19,2)) AS allowable_expenditure
FROM uchicago_mod_finance_storage.ledger_fiscal_year_rollover r, jsonb_array_elements(r.jsonb->'budgetsRollover') WITH ORDINALITY rbr (jsonb);
CREATE VIEW uc.rollover_encumbrances_rollover AS
SELECT
uuid_generate_v5(r.id, rer.ordinality::text)::text AS id,
r.id AS rollover_id,
rer.jsonb->>'orderType' AS order_type,
rer.jsonb->>'basedOn' AS based_on,
CAST(rer.jsonb->>'increaseBy' AS DECIMAL(19,2)) AS increase_by
FROM uchicago_mod_finance_storage.ledger_fiscal_year_rollover r, jsonb_array_elements(r.jsonb->'encumbrancesRollover') WITH ORDINALITY rer (jsonb);
CREATE VIEW uc.rollovers AS
SELECT
r.id AS id,
CAST(r.jsonb->>'_version' AS INTEGER) AS _version,
CAST(r.jsonb->>'ledgerId' AS UUID) AS ledger_id,
r.jsonb->>'rolloverType' AS rollover_type,
CAST(r.jsonb->>'fromFiscalYearId' AS UUID) AS from_fiscal_year_id,
CAST(r.jsonb->>'toFiscalYearId' AS UUID) AS to_fiscal_year_id,
CAST(r.jsonb->>'restrictEncumbrance' AS BOOLEAN) AS restrict_encumbrance,
CAST(r.jsonb->>'restrictExpenditures' AS BOOLEAN) AS restrict_expenditures,
CAST(r.jsonb->>'needCloseBudgets' AS BOOLEAN) AS need_close_budgets,
CAST(r.jsonb->>'currencyFactor' AS INTEGER) AS currency_factor,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(r.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
r.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(r.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(r.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
r.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_finance_storage.ledger_fiscal_year_rollover r;
CREATE VIEW uc.rollover_budget_acquisitions_units AS
SELECT
uuid_generate_v5(rb.id, rbau.ordinality::text)::text AS id,
rb.id AS rollover_budget_id,
CAST(rbau.jsonb AS UUID) AS acquisitions_unit_id
FROM uchicago_mod_finance_storage.ledger_fiscal_year_rollover_budget rb, jsonb_array_elements_text(rb.jsonb#>'{fundDetails,acqUnitIds}') WITH ORDINALITY rbau (jsonb);
CREATE VIEW uc.rollover_budget_organizations AS
SELECT
uuid_generate_v5(rb.id, rbo.ordinality::text)::text AS id,
rb.id AS rollover_budget_id,
CAST(rbo.jsonb AS UUID) AS organization_id
FROM uchicago_mod_finance_storage.ledger_fiscal_year_rollover_budget rb, jsonb_array_elements_text(rb.jsonb#>'{fundDetails,donorOrganizationIds}') WITH ORDINALITY rbo (jsonb);
CREATE VIEW uc.rollover_budget_locations AS
SELECT
uuid_generate_v5(rb.id, rbl.ordinality::text)::text AS id,
rb.id AS rollover_budget_id,
CAST(rbl.jsonb->>'locationId' AS UUID) AS location_id,
CAST(rbl.jsonb->>'tenantId' AS UUID) AS tenant_id
FROM uchicago_mod_finance_storage.ledger_fiscal_year_rollover_budget rb, jsonb_array_elements(rb.jsonb#>'{fundDetails,locations}') WITH ORDINALITY rbl (jsonb);
CREATE VIEW uc.rollover_budget_from_funds AS
SELECT
uuid_generate_v5(rb.id, rbff.ordinality::text)::text AS id,
rb.id AS rollover_budget_id,
CAST(rbff.jsonb AS UUID) AS fund_id
FROM uchicago_mod_finance_storage.ledger_fiscal_year_rollover_budget rb, jsonb_array_elements_text(rb.jsonb#>'{fundDetails,allocatedFromIds}') WITH ORDINALITY rbff (jsonb);
CREATE VIEW uc.rollover_budget_allocated_from_names AS
SELECT
uuid_generate_v5(rb.id, rbafn.ordinality::text)::text AS id,
rb.id AS rollover_budget_id,
CAST(rbafn.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_finance_storage.ledger_fiscal_year_rollover_budget rb, jsonb_array_elements_text(rb.jsonb#>'{fundDetails,allocatedFromNames}') WITH ORDINALITY rbafn (jsonb);
CREATE VIEW uc.rollover_budget_to_funds AS
SELECT
uuid_generate_v5(rb.id, rbtf.ordinality::text)::text AS id,
rb.id AS rollover_budget_id,
CAST(rbtf.jsonb AS UUID) AS fund_id
FROM uchicago_mod_finance_storage.ledger_fiscal_year_rollover_budget rb, jsonb_array_elements_text(rb.jsonb#>'{fundDetails,allocatedToIds}') WITH ORDINALITY rbtf (jsonb);
CREATE VIEW uc.rollover_budget_allocated_to_names AS
SELECT
uuid_generate_v5(rb.id, rbatn.ordinality::text)::text AS id,
rb.id AS rollover_budget_id,
CAST(rbatn.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_finance_storage.ledger_fiscal_year_rollover_budget rb, jsonb_array_elements_text(rb.jsonb#>'{fundDetails,allocatedToNames}') WITH ORDINALITY rbatn (jsonb);
CREATE VIEW uc.rollover_budget_expense_class_details AS
SELECT
uuid_generate_v5(rb.id, rbecd.ordinality::text)::text AS id,
rb.id AS rollover_budget_id,
CAST(rbecd.jsonb->>'id' AS UUID) AS id2,
rbecd.jsonb->>'expenseClassName' AS expense_class_name,
rbecd.jsonb->>'expenseClassCode' AS expense_class_code,
rbecd.jsonb->>'expenseClassStatus' AS expense_class_status,
CAST(rbecd.jsonb->>'encumbered' AS DECIMAL(19,2)) AS encumbered,
CAST(rbecd.jsonb->>'awaitingPayment' AS DECIMAL(19,2)) AS awaiting_payment,
CAST(rbecd.jsonb->>'credited' AS DECIMAL(19,2)) AS credited,
CAST(rbecd.jsonb->>'percentageCredited' AS DECIMAL(19,2)) AS percentage_credited,
CAST(rbecd.jsonb->>'expended' AS DECIMAL(19,2)) AS expended,
CAST(rbecd.jsonb->>'percentageExpended' AS DECIMAL(19,2)) AS percentage_expended
FROM uchicago_mod_finance_storage.ledger_fiscal_year_rollover_budget rb, jsonb_array_elements(rb.jsonb->'expenseClassDetails') WITH ORDINALITY rbecd (jsonb);
CREATE VIEW uc.rollover_budget_acquisitions_units2 AS
SELECT
uuid_generate_v5(rb.id, rbau2.ordinality::text)::text AS id,
rb.id AS rollover_budget_id,
CAST(rbau2.jsonb AS UUID) AS acquisitions_unit_id
FROM uchicago_mod_finance_storage.ledger_fiscal_year_rollover_budget rb, jsonb_array_elements_text(rb.jsonb->'acqUnitIds') WITH ORDINALITY rbau2 (jsonb);
CREATE VIEW uc.rollover_budget_tags AS
SELECT
uuid_generate_v5(rb.id, rbt.ordinality::text)::text AS id,
rb.id AS rollover_budget_id,
CAST(rbt.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_finance_storage.ledger_fiscal_year_rollover_budget rb, jsonb_array_elements_text(rb.jsonb#>'{tags,tagList}') WITH ORDINALITY rbt (jsonb);
CREATE VIEW uc.rollover_budgets AS
SELECT
rb.id AS id,
CAST(rb.jsonb->>'_version' AS INTEGER) AS _version,
CAST(rb.jsonb->>'budgetId' AS UUID) AS budget_id,
CAST(rb.jsonb->>'ledgerRolloverId' AS UUID) AS rollover_id,
rb.jsonb->>'name' AS name,
rb.jsonb#>>'{fundDetails,name}' AS fund_details_name,
rb.jsonb#>>'{fundDetails,code}' AS fund_details_code,
rb.jsonb#>>'{fundDetails,fundStatus}' AS fund_details_fund_status,
CAST(rb.jsonb#>>'{fundDetails,fundTypeId}' AS UUID) AS fund_details_fund_type_id,
rb.jsonb#>>'{fundDetails,fundTypeName}' AS fund_details_fund_type_name,
rb.jsonb#>>'{fundDetails,externalAccountNo}' AS fund_details_external_account_no,
rb.jsonb#>>'{fundDetails,description}' AS fund_details_description,
CAST(rb.jsonb#>>'{fundDetails,restrictByLocations}' AS BOOLEAN) AS fund_details_restrict_by_locations,
rb.jsonb->>'budgetStatus' AS budget_status,
CAST(rb.jsonb->>'allowableEncumbrance' AS DECIMAL(19,2)) AS allowable_encumbrance,
CAST(rb.jsonb->>'allowableExpenditure' AS DECIMAL(19,2)) AS allowable_expenditure,
CAST(rb.jsonb->>'allocated' AS DECIMAL(19,2)) AS allocated,
CAST(rb.jsonb->>'awaitingPayment' AS DECIMAL(19,2)) AS awaiting_payment,
CAST(rb.jsonb->>'available' AS DECIMAL(19,2)) AS available,
CAST(rb.jsonb->>'credits' AS DECIMAL(19,2)) AS credits,
CAST(rb.jsonb->>'encumbered' AS DECIMAL(19,2)) AS encumbered,
CAST(rb.jsonb->>'expenditures' AS DECIMAL(19,2)) AS expenditures,
CAST(rb.jsonb->>'netTransfers' AS DECIMAL(19,2)) AS net_transfers,
CAST(rb.jsonb->>'unavailable' AS DECIMAL(19,2)) AS unavailable,
CAST(rb.jsonb->>'overEncumbrance' AS DECIMAL(19,2)) AS over_encumbrance,
CAST(rb.jsonb->>'overExpended' AS DECIMAL(19,2)) AS over_expended,
CAST(rb.jsonb->>'fundId' AS UUID) AS fund_id,
CAST(rb.jsonb->>'fiscalYearId' AS UUID) AS fiscal_year_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(rb.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
rb.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(rb.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(rb.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
rb.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
CAST(rb.jsonb->>'initialAllocation' AS DECIMAL(19,2)) AS initial_allocation,
CAST(rb.jsonb->>'allocationTo' AS DECIMAL(19,2)) AS allocation_to,
CAST(rb.jsonb->>'allocationFrom' AS DECIMAL(19,2)) AS allocation_from,
CAST(rb.jsonb->>'totalFunding' AS DECIMAL(19,2)) AS total_funding,
CAST(rb.jsonb->>'cashBalance' AS DECIMAL(19,2)) AS cash_balance,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_finance_storage.ledger_fiscal_year_rollover_budget rb;
CREATE VIEW uc.rollover_errors AS
SELECT
re.id AS id,
CAST(re.jsonb->>'ledgerRolloverId' AS UUID) AS rollover_id,
re.jsonb->>'errorType' AS error_type,
re.jsonb->>'failedAction' AS failed_action,
re.jsonb->>'errorMessage' AS error_message,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(re.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
re.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(re.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(re.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
re.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_finance_storage.ledger_fiscal_year_rollover_error re;
CREATE VIEW uc.rollover_progresses AS
SELECT
rp.id AS id,
CAST(rp.jsonb->>'ledgerRolloverId' AS UUID) AS rollover_id,
rp.jsonb->>'overallRolloverStatus' AS overall_rollover_status,
rp.jsonb->>'budgetsClosingRolloverStatus' AS budgets_closing_rollover_status,
rp.jsonb->>'financialRolloverStatus' AS financial_rollover_status,
rp.jsonb->>'ordersRolloverStatus' AS orders_rollover_status,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(rp.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
rp.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(rp.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(rp.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
rp.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_finance_storage.ledger_fiscal_year_rollover_progress rp;
CREATE VIEW uc.scheduled_notices AS
SELECT
sn.id AS id,
CAST(sn.jsonb->>'loanId' AS UUID) AS loan_id,
CAST(sn.jsonb->>'requestId' AS UUID) AS request_id,
CAST(sn.jsonb->>'feeFineActionId' AS UUID) AS payment_id,
CAST(sn.jsonb->>'recipientUserId' AS UUID) AS recipient_user_id,
CAST(sn.jsonb->>'sessionId' AS UUID) AS session_id,
uc.TIMESTAMP_CAST(sn.jsonb->>'nextRunTime') AS next_run_time,
sn.jsonb->>'triggeringEvent' AS triggering_event,
sn.jsonb#>>'{noticeConfig,timing}' AS notice_config_timing,
CAST(sn.jsonb#>>'{noticeConfig,recurringPeriod,duration}' AS INTEGER) AS notice_config_recurring_period_duration,
sn.jsonb#>>'{noticeConfig,recurringPeriod,intervalId}' AS notice_config_recurring_period_interval_id,
CAST(sn.jsonb#>>'{noticeConfig,templateId}' AS UUID) AS notice_config_template_id,
sn.jsonb#>>'{noticeConfig,format}' AS notice_config_format,
CAST(sn.jsonb#>>'{noticeConfig,sendInRealTime}' AS BOOLEAN) AS notice_config_send_in_real_time,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(sn.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
sn.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(sn.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(sn.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
sn.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_circulation_storage.scheduled_notice sn;
CREATE VIEW uc.service_point_staff_slips AS
SELECT
uuid_generate_v5(sp.id, spss.ordinality::text)::text AS id,
sp.id AS service_point_id,
CAST(spss.jsonb->>'id' AS UUID) AS staff_slip_id,
CAST(spss.jsonb->>'printByDefault' AS BOOLEAN) AS print_by_default
FROM uchicago_mod_inventory_storage.service_point sp, jsonb_array_elements(sp.jsonb->'staffSlips') WITH ORDINALITY spss (jsonb);
CREATE VIEW uc.service_points AS
SELECT
sp.id AS id,
sp.jsonb->>'name' AS name,
sp.jsonb->>'code' AS code,
sp.jsonb->>'discoveryDisplayName' AS discovery_display_name,
sp.jsonb->>'description' AS description,
CAST(sp.jsonb->>'shelvingLagTime' AS INTEGER) AS shelving_lag_time,
CAST(sp.jsonb->>'pickupLocation' AS BOOLEAN) AS pickup_location,
CAST(sp.jsonb#>>'{holdShelfExpiryPeriod,duration}' AS INTEGER) AS hold_shelf_expiry_period_duration,
sp.jsonb#>>'{holdShelfExpiryPeriod,intervalId}' AS hold_shelf_expiry_period_interval_id,
sp.jsonb->>'holdShelfClosedLibraryDateManagement' AS hold_shelf_closed_library_date_management,
sp.jsonb->>'defaultCheckInActionForUseAtLocation' AS default_check_in_action_for_use_at_location,
CAST(sp.jsonb->>'ecsRequestRouting' AS BOOLEAN) AS ecs_request_routing,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(sp.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
sp.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(sp.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(sp.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
sp.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.service_point sp;
CREATE VIEW uc.service_point_user_service_points AS
SELECT
uuid_generate_v5(spu.id, spusp.ordinality::text)::text AS id,
spu.id AS service_point_user_id,
CAST(spusp.jsonb AS UUID) AS service_point_id
FROM uchicago_mod_inventory_storage.service_point_user spu, jsonb_array_elements_text(spu.jsonb->'servicePointsIds') WITH ORDINALITY spusp (jsonb);
CREATE VIEW uc.service_point_users AS
SELECT
spu.id AS id,
CAST(spu.jsonb->>'userId' AS UUID) AS user_id,
CAST(spu.jsonb->>'defaultServicePointId' AS UUID) AS default_service_point_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(spu.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
spu.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(spu.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(spu.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
spu.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.service_point_user spu;
CREATE VIEW uc.snapshots AS
SELECT
s.id AS id,
s.status AS status,
s.processing_started_date AS processing_started_date,
s.created_by_user_id AS creation_user_id,
s.created_date AS creation_time,
s.updated_by_user_id AS last_write_user_id,
s.updated_date AS last_write_time
FROM uchicago_mod_source_record_storage.snapshots_lb s;
CREATE VIEW uc.sources AS
SELECT
s.id AS id,
s.jsonb->>'name' AS name,
s.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(s.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
s.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(s.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(s.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
s.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.holdings_records_source s;
CREATE VIEW uc.staff_slips AS
SELECT
ss.id AS id,
ss.jsonb->>'name' AS name,
ss.jsonb->>'description' AS description,
CAST(ss.jsonb->>'active' AS BOOLEAN) AS active,
ss.jsonb->>'template' AS template,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(ss.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
ss.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(ss.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(ss.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
ss.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_circulation_storage.staff_slips ss;
CREATE VIEW uc.statistical_codes AS
SELECT
sc.id AS id,
sc.jsonb->>'code' AS code,
sc.jsonb->>'name' AS name,
CAST(sc.jsonb->>'statisticalCodeTypeId' AS UUID) AS statistical_code_type_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(sc.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
sc.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(sc.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(sc.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
sc.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.statistical_code sc;
CREATE VIEW uc.statistical_code_types AS
SELECT
sct.id AS id,
sct.jsonb->>'name' AS name,
sct.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(sct.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
sct.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(sct.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(sct.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
sct.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.statistical_code_type sct;
CREATE VIEW uc.subject_sources AS
SELECT
ss.id AS id,
ss.jsonb->>'name' AS name,
ss.jsonb->>'code' AS code,
ss.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(ss.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
ss.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(ss.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(ss.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
ss.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.subject_source ss;
CREATE VIEW uc.subject_types AS
SELECT
st.id AS id,
st.jsonb->>'name' AS name,
st.jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(st.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
st.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(st.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(st.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
st.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_inventory_storage.subject_type st;
CREATE VIEW uc.tags AS
SELECT
t.id AS id,
t.created_by AS creation_user_id,
t.label AS label,
t.description AS description,
t.created_date AS creation_time,
t.updated_date AS last_write_time,
t.updated_by AS updated_by_user_id
FROM uchicago_mod_tags.tags t;
CREATE VIEW uc.template_output_formats AS
SELECT
uuid_generate_v5(t.id, tof.ordinality::text)::text AS id,
t.id AS template_id,
CAST(tof.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_template_engine.template t, jsonb_array_elements_text(t.jsonb->'outputFormats') WITH ORDINALITY tof (jsonb);
CREATE VIEW uc.templates AS
SELECT
t.id AS id,
t.jsonb->>'name' AS name,
CAST(t.jsonb->>'active' AS BOOLEAN) AS active,
t.jsonb->>'category' AS category,
t.jsonb->>'description' AS description,
t.jsonb->>'templateResolver' AS template_resolver,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(t.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
t.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(t.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(t.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
t.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_template_engine.template t;
CREATE VIEW uc.title_product_ids AS
SELECT
uuid_generate_v5(t.id, tpi.ordinality::text)::text AS id,
t.id AS title_id,
tpi.jsonb->>'productId' AS product_id,
CAST(tpi.jsonb->>'productIdType' AS UUID) AS product_id_type_id,
tpi.jsonb->>'qualifier' AS qualifier
FROM uchicago_mod_orders_storage.titles t, jsonb_array_elements(t.jsonb->'productIds') WITH ORDINALITY tpi (jsonb);
CREATE VIEW uc.title_contributors AS
SELECT
uuid_generate_v5(t.id, tc.ordinality::text)::text AS id,
t.id AS title_id,
tc.jsonb->>'contributor' AS contributor,
CAST(tc.jsonb->>'contributorNameTypeId' AS UUID) AS contributor_name_type_id
FROM uchicago_mod_orders_storage.titles t, jsonb_array_elements(t.jsonb->'contributors') WITH ORDINALITY tc (jsonb);
CREATE VIEW uc.title_bind_item_ids AS
SELECT
uuid_generate_v5(t.id, tbii.ordinality::text)::text AS id,
t.id AS title_id,
CAST(tbii.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_orders_storage.titles t, jsonb_array_elements_text(t.jsonb->'bindItemIds') WITH ORDINALITY tbii (jsonb);
CREATE VIEW uc.title_acquisitions_units AS
SELECT
uuid_generate_v5(t.id, tau.ordinality::text)::text AS id,
t.id AS title_id,
CAST(tau.jsonb AS UUID) AS acquisitions_unit_id
FROM uchicago_mod_orders_storage.titles t, jsonb_array_elements_text(t.jsonb->'acqUnitIds') WITH ORDINALITY tau (jsonb);
CREATE VIEW uc.titles AS
SELECT
t.id AS id,
uc.DATE_CAST(t.jsonb->>'expectedReceiptDate') AS expected_receipt_date,
t.jsonb->>'title' AS title,
CAST(t.jsonb->>'poLineId' AS UUID) AS po_line_id,
CAST(t.jsonb->>'instanceId' AS UUID) AS instance_id,
t.jsonb->>'publisher' AS publisher,
t.jsonb->>'edition' AS edition,
t.jsonb->>'packageName' AS package_name,
t.jsonb->>'poLineNumber' AS po_line_number,
t.jsonb->>'publishedDate' AS published_date,
t.jsonb->>'receivingNote' AS receiving_note,
uc.TIMESTAMP_CAST(t.jsonb->>'subscriptionFrom') AS subscription_from,
uc.TIMESTAMP_CAST(t.jsonb->>'subscriptionTo') AS subscription_to,
CAST(t.jsonb->>'subscriptionInterval' AS INTEGER) AS subscription_interval,
CAST(t.jsonb->>'claimingActive' AS BOOLEAN) AS claiming_active,
CAST(t.jsonb->>'claimingInterval' AS INTEGER) AS claiming_interval,
CAST(t.jsonb->>'isAcknowledged' AS BOOLEAN) AS is_acknowledged,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(t.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
t.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(t.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(t.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
t.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_orders_storage.titles t;
CREATE VIEW uc.transaction_tags AS
SELECT
uuid_generate_v5(t.id, tt.ordinality::text)::text AS id,
t.id AS transaction_id,
CAST(tt.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_finance_storage.transaction t, jsonb_array_elements_text(t.jsonb#>'{tags,tagList}') WITH ORDINALITY tt (jsonb);
CREATE VIEW uc.transactions AS
SELECT
t.id AS id,
CAST(t.jsonb->>'_version' AS INTEGER) AS _version,
CAST(t.jsonb->>'amount' AS DECIMAL(19,2)) AS amount,
CAST(t.jsonb#>>'{awaitingPayment,encumbranceId}' AS UUID) AS awaiting_payment_encumbrance_id,
CAST(t.jsonb#>>'{awaitingPayment,releaseEncumbrance}' AS BOOLEAN) AS awaiting_payment_release_encumbrance,
t.jsonb->>'currency' AS currency,
t.jsonb->>'description' AS description,
CAST(t.jsonb#>>'{encumbrance,amountAwaitingPayment}' AS DECIMAL(19,2)) AS encumbrance_amount_awaiting_payment,
CAST(t.jsonb#>>'{encumbrance,amountCredited}' AS DECIMAL(19,2)) AS encumbrance_amount_credited,
CAST(t.jsonb#>>'{encumbrance,amountExpended}' AS DECIMAL(19,2)) AS encumbrance_amount_expended,
CAST(t.jsonb#>>'{encumbrance,initialAmountEncumbered}' AS DECIMAL(19,2)) AS encumbrance_initial_amount_encumbered,
t.jsonb#>>'{encumbrance,status}' AS encumbrance_status,
t.jsonb#>>'{encumbrance,orderType}' AS encumbrance_order_type,
t.jsonb#>>'{encumbrance,orderStatus}' AS encumbrance_order_status,
CAST(t.jsonb#>>'{encumbrance,subscription}' AS BOOLEAN) AS encumbrance_subscription,
CAST(t.jsonb#>>'{encumbrance,reEncumber}' AS BOOLEAN) AS encumbrance_re_encumber,
CAST(t.jsonb#>>'{encumbrance,sourcePurchaseOrderId}' AS UUID) AS encumbrance_source_purchase_order_id,
CAST(t.jsonb#>>'{encumbrance,sourcePoLineId}' AS UUID) AS encumbrance_source_po_line_id,
CAST(t.jsonb->>'expenseClassId' AS UUID) AS expense_class_id,
CAST(t.jsonb->>'fiscalYearId' AS UUID) AS fiscal_year_id,
CAST(t.jsonb->>'fromFundId' AS UUID) AS from_fund_id,
CAST(t.jsonb->>'invoiceCancelled' AS BOOLEAN) AS invoice_cancelled,
CAST(t.jsonb->>'paymentEncumbranceId' AS UUID) AS payment_encumbrance_id,
t.jsonb->>'source' AS source,
CAST(t.jsonb->>'sourceFiscalYearId' AS UUID) AS source_fiscal_year_id,
CAST(t.jsonb->>'sourceInvoiceId' AS UUID) AS source_invoice_id,
CAST(t.jsonb->>'sourceInvoiceLineId' AS UUID) AS source_invoice_line_id,
CAST(t.jsonb->>'toFundId' AS UUID) AS to_fund_id,
t.jsonb->>'transactionType' AS transaction_type,
CAST(t.jsonb->>'voidedAmount' AS DECIMAL(19,2)) AS voided_amount,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(t.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
t.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(t.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(t.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
t.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_finance_storage.transaction t;
CREATE VIEW uc.transfer_accounts AS
SELECT
ta.id AS id,
ta.jsonb->>'accountName' AS name,
ta.jsonb->>'desc' AS desc,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(ta.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
ta.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(ta.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(ta.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
ta.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
CAST(ta.jsonb->>'ownerId' AS UUID) AS owner_id,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_feesfines.transfers ta;
CREATE VIEW uc.transfer_criterias AS
SELECT
tc.id AS id,
tc.jsonb->>'criteria' AS criteria,
tc.jsonb->>'type' AS type,
CAST(tc.jsonb->>'value' AS DECIMAL(19,2)) AS value,
tc.jsonb->>'interval' AS interval,
jsonb_pretty(tc.jsonb) AS content
FROM uchicago_mod_feesfines.transfer_criteria tc;
CREATE VIEW uc.user_departments AS
SELECT
uuid_generate_v5(u.id, ud.ordinality::text)::text AS id,
u.id AS user_id,
CAST(ud.jsonb AS UUID) AS department_id
FROM uchicago_mod_users.users u, jsonb_array_elements_text(u.jsonb->'departments') WITH ORDINALITY ud (jsonb);
CREATE VIEW uc.user_addresses AS
SELECT
uuid_generate_v5(u.id, ua.ordinality::text)::text AS id,
u.id AS user_id,
CAST(ua.jsonb->>'id' AS UUID) AS id2,
ua.jsonb->>'countryId' AS country_id,
ua.jsonb->>'addressLine1' AS address_line1,
ua.jsonb->>'addressLine2' AS address_line2,
ua.jsonb->>'city' AS city,
ua.jsonb->>'region' AS region,
ua.jsonb->>'postalCode' AS postal_code,
CAST(ua.jsonb->>'addressTypeId' AS UUID) AS address_type_id,
CAST(ua.jsonb->>'primaryAddress' AS BOOLEAN) AS primary_address
FROM uchicago_mod_users.users u, jsonb_array_elements(u.jsonb#>'{personal,addresses}') WITH ORDINALITY ua (jsonb);
CREATE VIEW uc.user_tags AS
SELECT
uuid_generate_v5(u.id, ut.ordinality::text)::text AS id,
u.id AS user_id,
CAST(ut.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_users.users u, jsonb_array_elements_text(u.jsonb#>'{tags,tagList}') WITH ORDINALITY ut (jsonb);
CREATE VIEW uc.preferred_email_communications AS
SELECT
uuid_generate_v5(u.id, pec.ordinality::text)::text AS id,
u.id AS user_id,
CAST(pec.jsonb AS VARCHAR(1024)) AS content
FROM uchicago_mod_users.users u, jsonb_array_elements_text(u.jsonb->'preferredEmailCommunication') WITH ORDINALITY pec (jsonb);
CREATE VIEW uc.users AS
SELECT
u.id AS id,
u.jsonb->>'username' AS username,
u.jsonb->>'externalSystemId' AS external_system_id,
u.jsonb->>'barcode' AS barcode,
CAST(u.jsonb->>'active' AS BOOLEAN) AS active,
u.jsonb->>'type' AS type,
CAST(u.jsonb->>'patronGroup' AS UUID) AS group_id,
CAST(u.jsonb#>>'{personal,pronouns}' AS VARCHAR(300)) AS pronouns,
CAST(CONCAT_WS(' ', jsonb#>>'{personal,firstName}', jsonb#>>'{personal,middleName}', jsonb#>>'{personal,lastName}') AS VARCHAR(1024)) AS name,
u.jsonb#>>'{personal,lastName}' AS last_name,
u.jsonb#>>'{personal,firstName}' AS first_name,
u.jsonb#>>'{personal,middleName}' AS middle_name,
u.jsonb#>>'{personal,preferredFirstName}' AS preferred_first_name,
u.jsonb#>>'{personal,email}' AS email,
u.jsonb#>>'{personal,phone}' AS phone,
u.jsonb#>>'{personal,mobilePhone}' AS mobile_phone,
uc.DATE_CAST(u.jsonb#>>'{personal,dateOfBirth}') AS date_of_birth,
u.jsonb#>>'{personal,preferredContactTypeId}' AS preferred_contact_type_id,
u.jsonb#>>'{personal,profilePictureLink}' AS profile_picture_link,
uc.DATE_CAST(u.jsonb->>'enrollmentDate') AS enrollment_date,
uc.DATE_CAST(u.jsonb->>'expirationDate') AS expiration_date,
CAST(jsonb#>>'{customFields,source}' AS VARCHAR(1024)) AS source,
CAST(jsonb#>>'{customFields,category}' AS VARCHAR(1024)) AS category_code,
CAST(jsonb#>>'{customFields,category_2}' AS VARCHAR(1024)) AS category_id,
CAST(jsonb#>>'{customFields,status}' AS VARCHAR(1024)) AS status,
CAST(jsonb#>>'{customFields,statuses}' AS VARCHAR(1024)) AS statuses,
CAST(jsonb#>>'{customFields,staffStatus}' AS VARCHAR(1024)) AS staff_status,
CAST(jsonb#>>'{customFields,staffPrivileges}' AS VARCHAR(1024)) AS staff_privileges,
CAST(jsonb#>>'{customFields,staffDivision}' AS VARCHAR(1024)) AS staff_division,
CAST(jsonb#>>'{customFields,staffDepartment}' AS VARCHAR(1024)) AS staff_department,
CAST(jsonb#>>'{customFields,studentId}' AS VARCHAR(1024)) AS student_id,
CAST(jsonb#>>'{customFields,studentStatus}' AS VARCHAR(1024)) AS student_status,
CAST(jsonb#>>'{customFields,studentRestriction}' AS BOOLEAN) AS student_restriction,
CAST(jsonb#>>'{customFields,studentDivision}' AS VARCHAR(1024)) AS student_division,
CAST(jsonb#>>'{customFields,studentDepartment}' AS VARCHAR(1024)) AS student_department,
CAST(jsonb#>>'{customFields,deceased}' AS BOOLEAN) AS deceased,
CAST(jsonb#>>'{customFields,collections}' AS BOOLEAN) AS collections,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(u.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
u.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(u.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(u.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
u.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_users.users u;
CREATE VIEW uc.user_acquisitions_units AS
SELECT
uau.id AS id,
CAST(uau.jsonb->>'userId' AS UUID) AS user_id,
CAST(uau.jsonb->>'acquisitionsUnitId' AS UUID) AS acquisitions_unit_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(uau.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
uau.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(uau.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(uau.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
uau.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_orders_storage.acquisitions_unit_membership uau;
CREATE VIEW uc.user_request_preferences AS
SELECT
urp.id AS id,
CAST(urp.jsonb->>'userId' AS UUID) AS user_id,
CAST(urp.jsonb->>'holdShelf' AS BOOLEAN) AS hold_shelf,
CAST(urp.jsonb->>'delivery' AS BOOLEAN) AS delivery,
CAST(urp.jsonb->>'defaultServicePointId' AS UUID) AS default_service_point_id,
CAST(urp.jsonb->>'defaultDeliveryAddressTypeId' AS UUID) AS default_delivery_address_type_id,
urp.jsonb->>'fulfillment' AS fulfillment,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(urp.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
urp.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(urp.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(urp.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
urp.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_circulation_storage.user_request_preference urp;
CREATE VIEW uc.user_summary_open_loans AS
SELECT
uuid_generate_v5(us.id, usol.ordinality::text)::text AS id,
us.id AS user_summary_id,
CAST(usol.jsonb->>'loanId' AS UUID) AS loan_id,
uc.DATE_CAST(usol.jsonb->>'dueDate') AS due_date,
CAST(usol.jsonb->>'recall' AS BOOLEAN) AS recall,
CAST(usol.jsonb->>'itemLost' AS BOOLEAN) AS item_lost,
CAST(usol.jsonb->>'itemClaimedReturned' AS BOOLEAN) AS item_claimed_returned,
CAST(usol.jsonb#>>'{gracePeriod,duration}' AS INTEGER) AS grace_period_duration,
usol.jsonb#>>'{gracePeriod,intervalId}' AS grace_period_interval_id
FROM uchicago_mod_patron_blocks.user_summary us, jsonb_array_elements(us.jsonb->'openLoans') WITH ORDINALITY usol (jsonb);
CREATE VIEW uc.user_summary_open_fees_fines AS
SELECT
uuid_generate_v5(us.id, usoff.ordinality::text)::text AS id,
us.id AS user_summary_id,
CAST(usoff.jsonb->>'feeFineId' AS UUID) AS fee_fine_id,
CAST(usoff.jsonb->>'feeFineTypeId' AS UUID) AS fee_fine_type_id,
CAST(usoff.jsonb->>'loanId' AS UUID) AS loan_id,
CAST(usoff.jsonb->>'balance' AS DECIMAL(19,2)) AS balance
FROM uchicago_mod_patron_blocks.user_summary us, jsonb_array_elements(us.jsonb->'openFeesFines') WITH ORDINALITY usoff (jsonb);
CREATE VIEW uc.user_summaries AS
SELECT
us.id AS id,
CAST(us.jsonb->>'_version' AS INTEGER) AS _version,
CAST(us.jsonb->>'userId' AS UUID) AS user_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(us.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
us.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(us.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(us.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
us.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_patron_blocks.user_summary us;
CREATE VIEW uc.voucher_acquisitions_units AS
SELECT
uuid_generate_v5(v.id, vau.ordinality::text)::text AS id,
v.id AS voucher_id,
CAST(vau.jsonb AS UUID) AS acquisitions_unit_id
FROM uchicago_mod_invoice_storage.vouchers v, jsonb_array_elements_text(v.jsonb->'acqUnitIds') WITH ORDINALITY vau (jsonb);
CREATE VIEW uc.vouchers AS
SELECT
v.id AS id,
v.jsonb->>'accountingCode' AS accounting_code,
v.jsonb->>'accountNo' AS account_no,
CAST(v.jsonb->>'amount' AS DECIMAL(19,2)) AS amount,
CAST(v.jsonb->>'batchGroupId' AS UUID) AS batch_group_id,
v.jsonb->>'disbursementNumber' AS disbursement_number,
uc.DATE_CAST(v.jsonb->>'disbursementDate') AS disbursement_date,
CAST(v.jsonb->>'disbursementAmount' AS DECIMAL(19,2)) AS disbursement_amount,
CAST(v.jsonb->>'enclosureNeeded' AS BOOLEAN) AS enclosure_needed,
v.jsonb->>'invoiceCurrency' AS invoice_currency,
CAST(v.jsonb->>'invoiceId' AS UUID) AS invoice_id,
CAST(v.jsonb->>'exchangeRate' AS DECIMAL(19,2)) AS exchange_rate,
v.jsonb->>'operationMode' AS operation_mode,
CAST(v.jsonb->>'exportToAccounting' AS BOOLEAN) AS export_to_accounting,
v.jsonb->>'status' AS status,
v.jsonb->>'systemCurrency' AS system_currency,
v.jsonb->>'type' AS type,
uc.DATE_CAST(v.jsonb->>'voucherDate') AS voucher_date,
v.jsonb->>'voucherNumber' AS voucher_number,
CAST(v.jsonb->>'vendorId' AS UUID) AS vendor_id,
v.jsonb#>>'{vendorAddress,addressLine1}' AS vendor_address_address_line1,
v.jsonb#>>'{vendorAddress,addressLine2}' AS vendor_address_address_line2,
v.jsonb#>>'{vendorAddress,city}' AS vendor_address_city,
v.jsonb#>>'{vendorAddress,stateRegion}' AS vendor_address_state_region,
v.jsonb#>>'{vendorAddress,zipCode}' AS vendor_address_zip_code,
v.jsonb#>>'{vendorAddress,country}' AS vendor_address_country,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(v.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
v.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(v.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(v.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
v.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_invoice_storage.vouchers v;
CREATE VIEW uc.voucher_item_fund_distributions AS
SELECT
uuid_generate_v5(vi.id, vifd.ordinality::text)::text AS id,
vi.id AS voucher_item_id,
vifd.jsonb->>'code' AS code,
CAST(vifd.jsonb->>'encumbrance' AS UUID) AS encumbrance_id,
CAST(vifd.jsonb->>'fundId' AS UUID) AS fund_id,
CAST(vifd.jsonb->>'expenseClassId' AS UUID) AS expense_class_id,
vifd.jsonb->>'distributionType' AS distribution_type,
CAST(vifd.jsonb->>'value' AS DECIMAL(19,2)) AS value
FROM uchicago_mod_invoice_storage.voucher_lines vi, jsonb_array_elements(vi.jsonb->'fundDistributions') WITH ORDINALITY vifd (jsonb);
CREATE VIEW uc.voucher_item_invoice_items AS
SELECT
uuid_generate_v5(vi.id, viii.ordinality::text)::text AS id,
vi.id AS voucher_item_id,
CAST(viii.jsonb AS UUID) AS invoice_item_id
FROM uchicago_mod_invoice_storage.voucher_lines vi, jsonb_array_elements_text(vi.jsonb->'sourceIds') WITH ORDINALITY viii (jsonb);
CREATE VIEW uc.voucher_items AS
SELECT
vi.id AS id,
CAST(vi.jsonb->>'amount' AS DECIMAL(19,2)) AS amount,
vi.jsonb->>'externalAccountNumber' AS external_account_number,
CAST(vi.jsonb->>'subTransactionId' AS UUID) AS sub_transaction_id,
CAST(vi.jsonb->>'voucherId' AS UUID) AS voucher_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(vi.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
vi.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(vi.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(vi.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
vi.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_invoice_storage.voucher_lines vi;
CREATE VIEW uc.waive_reasons AS
SELECT
wr.id AS id,
wr.jsonb->>'nameReason' AS name,
wr.jsonb->>'description' AS description,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(wr.jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
wr.jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(wr.jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(wr.jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
wr.jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
CAST(wr.jsonb->>'accountId' AS UUID) AS account_id,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM uchicago_mod_feesfines.waives wr;
CREATE VIEW uc.addresses AS
SELECT 
c.id, 
(c.value::jsonb)->>'name' AS name, 
(c.value::jsonb)->>'address' AS content, 
c.enabled, 
c.created_date, 
c.created_by_user_id, 
c.updated_date, 
c.updated_by_user_id   
FROM uc.configurations c
WHERE c.config_name = 'tenant.addresses';
CREATE VIEW uc.printers AS
SELECT 
c.id, 
(c.value::jsonb)->>'computerName' AS computer_name, 
(c.value::jsonb)->>'name' AS name, 
((c.value::jsonb)->>'left')::int AS left, 
((c.value::jsonb)->>'top')::int AS top, 
((c.value::jsonb)->>'width')::int AS width, 
((c.value::jsonb)->>'height')::int AS height, 
c.enabled, 
c.created_date, 
c.created_by_user_id, 
c.updated_date, 
c.updated_by_user_id   
FROM uc.configurations c
WHERE c.module = 'uc' AND c.config_name = 'printers';
CREATE VIEW uc.settings AS
SELECT 
c.id, 
(c.value::jsonb)->>'name' AS name, 
((c.value::jsonb)->>'orientation')::int AS orientation, 
(c.value::jsonb)->>'fontFamily' AS font_family, 
((c.value::jsonb)->>'fontSize')::int AS font_size, 
((c.value::jsonb)->>'fontWeight')::int AS font_weight, 
c.enabled, 
c.created_date, 
c.created_by_user_id, 
c.updated_date, 
c.updated_by_user_id   
FROM uc.configurations c
WHERE c.module = 'uc' AND c.config_name = 'settings';
CREATE VIEW uc.location_settings AS
SELECT 
c.id, 
((c.value::jsonb)->>'locationId')::uuid AS location_id, 
((c.value::jsonb)->>'settingsId')::uuid AS settings_id, 
c.enabled, 
c.created_date, 
c.created_by_user_id, 
c.updated_date, 
c.updated_by_user_id   
FROM uc.configurations c
WHERE c.module = 'uc' AND c.config_name = 'location_settings';
CREATE TABLE uc.countries (
    id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    alpha2_code VARCHAR(2) NOT NULL,
    alpha3_code VARCHAR(3) NOT NULL,
    name VARCHAR(128) NOT NULL,
    CONSTRAINT pk_countries PRIMARY KEY(id)
);
CREATE VIEW uc.object_notes AS
SELECT 
uuid_generate_v5(l.object_id::UUID, nl.note_id::TEXT) AS id,
l.object_id::UUID,
l.object_type AS type,
nl.note_id 
FROM uchicago_mod_notes.note_link nl
JOIN uchicago_mod_notes.link l ON l.id = nl.link_id;
CREATE VIEW uc.organization_notes AS
SELECT 
id, 
object_id AS organization_id,
note_id 
FROM uc.object_notes
WHERE type = 'organization';
CREATE VIEW uc.order_item_notes AS
SELECT 
id, 
object_id AS order_item_id,
note_id 
FROM uc.object_notes
WHERE type = 'poLine';
CREATE VIEW uc.request_notes AS
SELECT 
id, 
object_id AS request_id,
note_id 
FROM uc.object_notes
WHERE type = 'request';
CREATE VIEW uc.user_notes AS
SELECT 
id, 
object_id AS user_id,
note_id 
FROM uc.object_notes
WHERE type = 'user';
INSERT INTO uc.countries (alpha2_code, alpha3_code, name) VALUES ('AD', 'AND', 'Andorra'), ('AE', 'ARE', 'United Arab Emirates'), ('AF', 'AFG', 'Afghanistan'), ('AG', 'ATG', 'Antigua and Barbuda'), ('AI', 'AIA', 'Anguilla'), ('AL', 'ALB', 'Albania'), ('AM', 'ARM', 'Armenia'), ('AN', 'ANT', 'Netherlands Antilles'), ('AO', 'AGO', 'Angola'), ('AQ', 'ATA', 'Antarctica'), ('AR', 'ARG', 'Argentina'), ('AS', 'ASM', 'American Samoa'), ('AT', 'AUT', 'Austria'), ('AU', 'AUS', 'Australia'), ('AW', 'ABW', 'Aruba'), ('AX', 'ALA', 'Åland Islands'), ('AZ', 'AZE', 'Azerbaijan'), ('BA', 'BIH', 'Bosnia and Herzegovina'), ('BB', 'BRB', 'Barbados'), ('BD', 'BGD', 'Bangladesh'), ('BE', 'BEL', 'Belgium'), ('BF', 'BFA', 'Burkina Faso'), ('BG', 'BGR', 'Bulgaria'), ('BH', 'BHR', 'Bahrain'), ('BI', 'BDI', 'Burundi'), ('BJ', 'BEN', 'Benin'), ('BL', 'BLM', 'Saint BarthÃ©lemy'), ('BM', 'BMU', 'Bermuda'), ('BN', 'BRN', 'Brunei Darussalam'), ('BO', 'BOL', 'Bolivia, Plurinational State Of'), ('BR', 'BRA', 'Brazil'), ('BS', 'BHS', 'Bahamas'), ('BT', 'BTN', 'Bhutan'), ('BV', 'BVT', 'Bouvet Island'), ('BW', 'BWA', 'Botswana'), ('BY', 'BLR', 'Belarus'), ('BZ', 'BLZ', 'Belize'), ('CA', 'CAN', 'Canada'), ('CC', 'CCK', 'Cocos (Keeling), Islands'), ('CD', 'COD', 'Congo, The Democratic Republic Of The'), ('CF', 'CAF', 'Central African Republic'), ('CG', 'COG', 'Congo'), ('CH', 'CHE', 'Switzerland'), ('CI', 'CIV', 'CÃ´te D''Ivoire'), ('CK', 'COK', 'Cook Islands'), ('CL', 'CHL', 'Chile'), ('CM', 'CMR', 'Cameroon'), ('CN', 'CHN', 'China'), ('CO', 'COL', 'Colombia'), ('CR', 'CRI', 'Costa Rica'), ('CU', 'CUB', 'Cuba'), ('CV', 'CPV', 'Cape Verde'), ('CX', 'CXR', 'Christmas Island'), ('CY', 'CYP', 'Cyprus'), ('CZ', 'CZE', 'Czech Republic'), ('DE', 'DEU', 'Germany'), ('DJ', 'DJI', 'Djibouti'), ('DK', 'DNK', 'Denmark'), ('DM', 'DMA', 'Dominica'), ('DO', 'DOM', 'Dominican Republic'), ('DZ', 'DZA', 'Algeria'), ('EC', 'ECU', 'Ecuador'), ('EE', 'EST', 'Estonia'), ('EG', 'EGY', 'Egypt'), ('EH', 'ESH', 'Western Sahara'), ('ER', 'ERI', 'Eritrea'), ('ES', 'ESP', 'Spain'), ('ET', 'ETH', 'Ethiopia'), ('FI', 'FIN', 'Finland'), ('FJ', 'FJI', 'Fiji'), ('FK', 'FLK', 'Falkland Islands (Malvinas),'), ('FM', 'FSM', 'Micronesia, Federated States Of'), ('FO', 'FRO', 'Faroe Islands'), ('FR', 'FRA', 'France'), ('GA', 'GAB', 'Gabon'), ('GB', 'GBR', 'United Kingdom'), ('GD', 'GRD', 'Grenada'), ('GE', 'GEO', 'Georgia'), ('GF', 'GUF', 'French Guiana'), ('GG', 'GGY', 'Guernsey'), ('GH', 'GHA', 'Ghana'), ('GI', 'GIB', 'Gibraltar'), ('GL', 'GRL', 'Greenland'), ('GM', 'GMB', 'Gambia'), ('GN', 'GIN', 'Guinea'), ('GP', 'GLP', 'Guadeloupe'), ('GQ', 'GNQ', 'Equatorial Guinea'), ('GR', 'GRC', 'Greece'), ('GS', 'SGS', 'South Georgia and South Sandwich Islands'), ('GT', 'GTM', 'Guatemala'), ('GU', 'GUM', 'Guam'), ('GW', 'GNB', 'Guinea-Bissau'), ('GY', 'GUY', 'Guyana'), ('HK', 'HKG', 'Hong Kong'), ('HM', 'HMD', 'Heard and McDonald Islands'), ('HN', 'HND', 'Honduras'), ('HR', 'HRV', 'Croatia'), ('HT', 'HTI', 'Haiti'), ('HU', 'HUN', 'Hungary'), ('ID', 'IDN', 'Indonesia'), ('IE', 'IRL', 'Ireland'), ('IL', 'ISR', 'Israel'), ('IM', 'IMN', 'Isle of Man'), ('IN', 'IND', 'India'), ('IO', 'IOT', 'British Indian Ocean Territory'), ('IQ', 'IRQ', 'Iraq'), ('IR', 'IRN', 'Iran, Islamic Republic Of'), ('IS', 'ISL', 'Iceland'), ('IT', 'ITA', 'Italy'), ('JE', 'JEY', 'Jersey'), ('JM', 'JAM', 'Jamaica'), ('JO', 'JOR', 'Jordan'), ('JP', 'JPN', 'Japan'), ('KE', 'KEN', 'Kenya'), ('KG', 'KGZ', 'Kyrgyzstan'), ('KH', 'KHM', 'Cambodia'), ('KI', 'KIR', 'Kiribati'), ('KM', 'COM', 'Comoros'), ('KN', 'KNA', 'Saint Kitts And Nevis'), ('KP', 'PRK', 'Korea, Democratic People''s Republic Of'), ('KR', 'KOR', 'Korea, Republic of'), ('KW', 'KWT', 'Kuwait'), ('KY', 'CYM', 'Cayman Islands'), ('KZ', 'KAZ', 'Kazakhstan'), ('LA', 'LAO', 'Lao People''s Democratic Republic'), ('LB', 'LBN', 'Lebanon'), ('LC', 'LCA', 'Saint Lucia'), ('LI', 'LIE', 'Liechtenstein'), ('LK', 'LKA', 'Sri Lanka'), ('LR', 'LBR', 'Liberia'), ('LS', 'LSO', 'Lesotho'), ('LT', 'LTU', 'Lithuania'), ('LU', 'LUX', 'Luxembourg'), ('LV', 'LVA', 'Latvia'), ('LY', 'LBY', 'Libyan Arab Jamahiriya'), ('MA', 'MAR', 'Morocco'), ('MC', 'MCO', 'Monaco'), ('MD', 'MDA', 'Moldova, Republic of'), ('ME', 'MNE', 'Montenegro'), ('MF', 'MAF', 'Saint Martin'), ('MG', 'MDG', 'Madagascar'), ('MH', 'MHL', 'Marshall Islands'), ('MK', 'MKD', 'Macedonia, the Fmr. Yugoslav Republic Of'), ('ML', 'MLI', 'Mali'), ('MM', 'MMR', 'Myanmar'), ('MN', 'MNG', 'Mongolia'), ('MO', 'MAC', 'Macao'), ('MP', 'MNP', 'Northern Mariana Islands'), ('MQ', 'MTQ', 'Martinique'), ('MR', 'MRT', 'Mauritania'), ('MS', 'MSR', 'Montserrat'), ('MT', 'MLT', 'Malta'), ('MU', 'MUS', 'Mauritius'), ('MV', 'MDV', 'Maldives'), ('MW', 'MWI', 'Malawi'), ('MX', 'MEX', 'Mexico'), ('MY', 'MYS', 'Malaysia'), ('MZ', 'MOZ', 'Mozambique'), ('NA', 'NAM', 'Namibia'), ('NC', 'NCL', 'New Caledonia'), ('NE', 'NER', 'Niger'), ('NF', 'NFK', 'Norfolk Island'), ('NG', 'NGA', 'Nigeria'), ('NI', 'NIC', 'Nicaragua'), ('NL', 'NLD', 'Netherlands'), ('NO', 'NOR', 'Norway'), ('NP', 'NPL', 'Nepal'), ('NR', 'NRU', 'Nauru'), ('NU', 'NIU', 'Niue'), ('NZ', 'NZL', 'New Zealand'), ('OM', 'OMN', 'Oman'), ('PA', 'PAN', 'Panama'), ('PE', 'PER', 'Peru'), ('PF', 'PYF', 'French Polynesia'), ('PG', 'PNG', 'Papua New Guinea'), ('PH', 'PHL', 'Philippines'), ('PK', 'PAK', 'Pakistan'), ('PL', 'POL', 'Poland'), ('PM', 'SPM', 'Saint Pierre And Miquelon'), ('PN', 'PCN', 'Pitcairn'), ('PR', 'PRI', 'Puerto Rico'), ('PS', 'PSE', 'Palestinian Territory, Occupied'), ('PT', 'PRT', 'Portugal'), ('PW', 'PLW', 'Palau'), ('PY', 'PRY', 'Paraguay'), ('QA', 'QAT', 'Qatar'), ('RE', 'REU', 'RÃ©union'), ('RO', 'ROU', 'Romania'), ('RS', 'SRB', 'Serbia'), ('RU', 'RUS', 'Russian Federation'), ('RW', 'RWA', 'Rwanda'), ('SA', 'SAU', 'Saudi Arabia'), ('SB', 'SLB', 'Solomon Islands'), ('SC', 'SYC', 'Seychelles'), ('SD', 'SDN', 'Sudan'), ('SE', 'SWE', 'Sweden'), ('SG', 'SGP', 'Singapore'), ('SH', 'SHN', 'St. Helena, Ascension, Tristan Da Cunha'), ('SI', 'SVN', 'Slovenia'), ('SJ', 'SJM', 'Svalbard And Jan Mayen'), ('SK', 'SVK', 'Slovakia'), ('SL', 'SLE', 'Sierra Leone'), ('SM', 'SMR', 'San Marino'), ('SN', 'SEN', 'Senegal'), ('SO', 'SOM', 'Somalia'), ('SR', 'SUR', 'Suriname'), ('ST', 'STP', 'Sao Tome and Principe'), ('SV', 'SLV', 'El Salvador'), ('SY', 'SYR', 'Syrian Arab Republic'), ('SZ', 'SWZ', 'Swaziland'), ('TC', 'TCA', 'Turks and Caicos Islands'), ('TD', 'TCD', 'Chad'), ('TF', 'ATF', 'French Southern Territories'), ('TG', 'TGO', 'Togo'), ('TH', 'THA', 'Thailand'), ('TJ', 'TJK', 'Tajikistan'), ('TK', 'TKL', 'Tokelau'), ('TL', 'TLS', 'Timor-Leste'), ('TM', 'TKM', 'Turkmenistan'), ('TN', 'TUN', 'Tunisia'), ('TO', 'TON', 'Tonga'), ('TR', 'TUR', 'Turkey'), ('TT', 'TTO', 'Trinidad and Tobago'), ('TV', 'TUV', 'Tuvalu'), ('TW', 'TWN', 'Taiwan, Province Of China'), ('TZ', 'TZA', 'Tanzania, United Republic of'), ('UA', 'UKR', 'Ukraine'), ('UG', 'UGA', 'Uganda'), ('UM', 'UMI', 'United States Minor Outlying Islands'), ('US', 'USA', 'United States'), ('UY', 'URY', 'Uruguay'), ('UZ', 'UZB', 'Uzbekistan'), ('VA', 'VAT', 'Holy See (Vatican City State),'), ('VC', 'VCT', 'Saint Vincent And The Grenedines'), ('VE', 'VEN', 'Venezuela, Bolivarian Republic of'), ('VG', 'VGB', 'Virgin Islands, British'), ('VI', 'VIR', 'Virgin Islands, U.S.'), ('VN', 'VNM', 'Viet Nam'), ('VU', 'VUT', 'Vanuatu'), ('WF', 'WLF', 'Wallis and Futuna'), ('WS', 'WSM', 'Samoa'), ('YE', 'YEM', 'Yemen'), ('YT', 'MYT', 'Mayotte'), ('ZA', 'ZAF', 'South Africa'), ('ZM', 'ZMB', 'Zambia'), ('ZW', 'ZWE', 'Zimbabwe'), ('ZZ', 'ZZZ', 'Other Countries');
CREATE TABLE uc.contact_types (
    id VARCHAR(3) NOT NULL,
    name VARCHAR(128) NOT NULL,
    PRIMARY KEY(id)
);
INSERT INTO uc.contact_types VALUES ('001', 'Mail'),  ('002', 'Email'),  ('003', 'Text message');
CREATE TABLE uc.item_statuses (
    id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    name VARCHAR(128) NOT NULL,
    creation_time TIMESTAMP,
    creation_username VARCHAR(128),
    last_write_time TIMESTAMP,
    last_write_username VARCHAR(128),
    CONSTRAINT pk_item_statuses PRIMARY KEY(id)
);
INSERT INTO uc.item_statuses (name, creation_time, creation_username, last_write_time, last_write_username) VALUES 
('Aged to lost', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Available', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Awaiting pickup', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Awaiting delivery', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Checked out', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Claimed returned', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Declared lost', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('In process', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('In process (non-requestable)', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('In transit', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Intellectual item', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Long missing', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Lost and paid', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Missing', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('On order', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Paged', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Restricted', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Order closed', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Unavailable', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Unknown', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Withdrawn', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller');
CREATE TABLE uc.receipt_statuses (
    id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    name VARCHAR(128) NOT NULL,
    creation_time TIMESTAMP,
    creation_username VARCHAR(128),
    last_write_time TIMESTAMP,
    last_write_username VARCHAR(128),
    CONSTRAINT pk_receipt_statuses PRIMARY KEY(id)
);
INSERT INTO uc.receipt_statuses (name, creation_time, creation_username, last_write_time, last_write_username) VALUES 
('Awaiting Receipt', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Cancelled', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Fully Received', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Partially Received', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Pending', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Receipt Not Required', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Ongoing', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller');
CREATE TABLE uc.invoice_statuses (
    id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    name VARCHAR(128) NOT NULL,
    creation_time TIMESTAMP,
    creation_username VARCHAR(128),
    last_write_time TIMESTAMP,
    last_write_username VARCHAR(128),
    CONSTRAINT pk_invoice_statuses PRIMARY KEY(id)
);
INSERT INTO uc.invoice_statuses (name, creation_time, creation_username, last_write_time, last_write_username) VALUES 
('Open', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Reviewed', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Approved', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Paid', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Cancelled', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller');
CREATE TABLE uc.order_statuses (
    id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    name VARCHAR(128) NOT NULL,
    creation_time TIMESTAMP,
    creation_username VARCHAR(128),
    last_write_time TIMESTAMP,
    last_write_username VARCHAR(128),
    CONSTRAINT pk_order_statuses PRIMARY KEY(id)
);
INSERT INTO uc.order_statuses (name, creation_time, creation_username, last_write_time, last_write_username) VALUES 
('Pending', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Open', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Closed', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Cancelled', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller')
;
CREATE TABLE uc.order_types (
    id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    name VARCHAR(128) NOT NULL,
    creation_time TIMESTAMP,
    creation_username VARCHAR(128),
    last_write_time TIMESTAMP,
    last_write_username VARCHAR(128),
    CONSTRAINT pk_order_types PRIMARY KEY(id)
);
INSERT INTO uc.order_types (name, creation_time, creation_username, last_write_time, last_write_username) VALUES 
('One-Time', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Ongoing', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller');
CREATE TABLE uc.payment_types (
    id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    name VARCHAR(128) NOT NULL,
    creation_time TIMESTAMP,
    creation_username VARCHAR(128) NOT NULL,
    last_write_time TIMESTAMP,
    last_write_username VARCHAR(128) NOT NULL,
    CONSTRAINT pk_payment_types PRIMARY KEY(id)
);
INSERT INTO uc.payment_types (id, name, creation_time, creation_username, last_write_time, last_write_username) VALUES 
(42, '42', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
(49, '49', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
(52, '52', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller');
CREATE TABLE uc.voucher_statuses (
    id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    name VARCHAR(128) NOT NULL,
    creation_time TIMESTAMP,
    creation_username VARCHAR(128) NOT NULL,
    last_write_time TIMESTAMP,
    last_write_username VARCHAR(128) NOT NULL,
    CONSTRAINT pk_voucher_statuses PRIMARY KEY(id)
);
INSERT INTO uc.voucher_statuses (name, creation_time, creation_username, last_write_time, last_write_username) VALUES 
('Awaiting payment', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Cancelled', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('Paid', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller');
CREATE TABLE uc.user_categories (
    id VARCHAR(7) NOT NULL,
    name VARCHAR(128) NOT NULL,
    code VARCHAR(128) NOT NULL,
    creation_time TIMESTAMP,
    creation_username VARCHAR(128),
    last_write_time TIMESTAMP,
    last_write_username VARCHAR(128),
    CONSTRAINT pk_user_categories PRIMARY KEY(id)
);
INSERT INTO uc.user_categories VALUES
('opt_1', 'Access, Paid', 'ACCESS_PAID', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_2', 'Argonne Laboratory Retiree', 'ARGRET', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_3', 'Argonne Laboratory Staff', 'ARG', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_4', 'Art Institute Staff', 'ARTINS', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_5', 'Assoc. Member, CEAS', 'CEASIA', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_6', 'Assoc. Member, CEERES', 'CEERES', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_7', 'Assoc. Member, CLAS', 'CLAS', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_8', 'Assoc. Member, CMES', 'CMIDEAST', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_9', 'Assoc. Member, COSAS', 'CSASIA', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_10', 'ATLA Staff', 'ATLA', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_11', 'Bindery Charges - Normal', 'BINDN', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_12', 'Bindery Charges - Problem', 'BINDP', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_13', 'Bindery Charges - Rush', 'BINDR', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_14', 'Blue Card Academic', 'BLUECARD', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_15', 'Board Mem., Smart Museum', 'SMART', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_16', 'Brookfield Zoo Prof. Staff', 'BROOKZOO', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_17', 'Chapin Hall Staff', 'CHAPIN', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_18', 'Charter School Teacher', 'CHARTTEACH', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_19', 'BTAA Faculty', 'BTAAFAC', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_20', 'BTAA Student', 'BTAASTU', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_21', 'Civic Knowledge Project', 'CKP', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_22', 'Crerar Corp., Non-Profit', 'CORMEMN', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_23', 'Crerar Corp., Profit', 'CORMEMP', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_24', 'CRL Staff', 'CRL', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_25', 'CTS Faculty', 'CTSFAC', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_26', 'CTS Staff', 'CTSSTF', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_27', 'CTS Student', 'CTSSTU', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_28', 'CTU Faculty', 'CTUFAC', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_29', 'CTU Student', 'CTUSTU', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_30', 'D''Angelo Law Library Carrel', 'CARREL', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_31', 'Dep. Child, UC Faculty', 'FACCHI', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_32', 'Div School Teaching Pastor', 'DIVPAST', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_33', 'Encyc. Britannica Staff', 'ENBRIT', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_34', 'Fermi Laboratory Staff', 'FERMI', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_35', 'Field Museum Staff', 'FLDMUS', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_36', 'Field Work Instructor', 'FLDINST', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_37', 'Google Book Project', 'GOOGLE', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_38', 'GSCS Student, Other ND NC', 'GSCSOTHER', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_39', 'HHMI Staff', 'HHMI', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_40', 'ILL CIC', 'ILLCIC', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_41', 'ILL ILLINET', 'ILLILL', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_42', 'ILL ILLINET/CIC', 'ILLILLC', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_43', 'ILL Other', 'ILLOTH', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_44', 'ILL Reciprocal', 'ILLREC', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_45', 'ILL SHARES', 'ILLSHA', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_46', 'JKM Library Staff', 'JKMSTF', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_47', 'La Rabida Hospital Staff', 'RABIDA', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_48', 'Lab School Faculty', 'LABFAC', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_49', 'Lab School Student', 'LABSTU', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_50', 'LSTC Adjunct Faculty', 'LSTCADJ', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_51', 'LSTC Faculty', 'LSTCFAC', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_52', 'LSTC Staff', 'LSTCSTF', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_53', 'LSTC Student', 'LSTCSTU', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_54', 'Mansueto Only Borrower', 'MANB', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_55', 'MCC Faculty', 'MCCFAC', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_56', 'MCC Staff', 'MCCSTF', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_57', 'MCC Student', 'MCCSTU', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_58', 'Medical House Staff', 'MEDHS', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_59', 'Member, Jesuit House', 'JESUIT', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_60', 'Member, UC Advisory Council', 'ADVCNL', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_61', 'Member, UC Women''s Board', 'WOMBRD', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_62', 'MLTS Adjunct Faculty', 'MEALOMADJ', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_63', 'MLTS Faculty', 'MEALOMFAC', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_64', 'MLTS Staff', 'MEALOMSTF', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_65', 'MLTS Student', 'MEALOMSTU', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_66', 'Newberry Library Acad. Staff', 'NEWBSTF', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_67', 'Newberry Library Fellow', 'NEWBFEL', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_68', 'Non-UC Faculty, Access', 'NONUCFAC_FREE', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_69', 'Non-UC Faculty, Borrowing', 'NONUCFAC_PAID', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_70', 'NORC Staff', 'NORC', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_71', 'NorthShore Medical Assoc.', 'NORTHSHORE', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_72', 'NU College Student', 'NWUCOL', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_73', 'NU Faculty', 'NWUFAC', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_74', 'NU Graduate Student', 'NWUGRAD', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_75', 'NU Staff', 'NWUSTF', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_76', 'OCLC Recip. Fac. Borrowing', 'OCLC', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_77', 'Paulson Institute Staff', 'PAULSON', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_78', 'Proxy Borrower', 'PROXY', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_79', 'Rockefeller Relig. Advisers', 'ROCKEFELLER', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_80', 'SHARES Affiliate', 'SHARES', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_81', 'Spec. Arrang., Free', 'SPCAR_FREE', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_82', 'Spec. Arrang., Paid', 'SPCAR_PAID', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_83', 'Spouse/Partner, UC Acad. "A"', 'ACDAPART', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_84', 'Spouse/Partner, UC Acad. "L"', 'ACDLPART', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_85', 'Spouse/Partner, UC Acad. Inc.', 'ACDIPART', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_86', 'Spouse/Partner, UC Alum', 'ALUMPART', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_87', 'Spouse/Partner, UC Assoc.', 'ASCPART', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_88', 'Spouse/Partner, UC Faculty', 'FACPART', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_89', 'Spouse/Partner, UC Med. Staff', 'MEDPART', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_90', 'Spouse/Partner, UC Postdoc', 'PDCPART', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_91', 'Spouse/Partner, UC Staff', 'STFPART', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_92', 'Spouse/Partner, UC Student', 'STUPART', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_93', 'Spouse/Partner, UC Trustee', 'TRUPART', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_94', 'Spouse/Partner, UC Adv. Couc.', 'ADVCNLPART', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_95', 'SSA Adjunct Lecturer', 'SSALECT', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_96', 'TTI Faculty', 'TOYOTA', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_97', 'UC & Lab School Student', 'STU6', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_98', 'UC Academic "A"', 'ACDA', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_99', 'UC Academic "L"', 'ACDL', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_100', 'UC Academic, Incoming', 'ACDI', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_101', 'UC Alum, Baccalaureate', 'ALUMB', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_102', 'UC Alum, Doctoral', 'ALUMD', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_103', 'UC Alum, Lab School', 'ALUML', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_104', 'UC Alum, Masters', 'ALUMM', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_105', 'UC Alum, Non-Degree', 'ALUMND', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_106', 'UC Associate', 'ASC', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_107', 'UC Department, Indefinite', 'DEPTI', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_108', 'UC Department, Quarter', 'DEPTQ', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_109', 'UC Faculty', 'FAC', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_110', 'UC Faculty, Emeritus', 'FACE', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_111', 'UC Faculty, Incoming', 'FACI', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_112', 'UC Medicine Staff', 'MED', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_113', 'UC Postdoctoral Scholar', 'PDC', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_114', 'UC Staff', 'STF', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_115', 'UC Student, At-Large', 'STU5', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_116', 'UC Student, College', 'STU1', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_117', 'UC Student, Grad/Non-PhD', 'STU2', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_118', 'UC Student, Grad/PhD', 'STU3', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_119', 'UC Student, Incoming', 'STUINC', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_120', 'UC Student, Non-Degree', 'STU9', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_121', 'UC Student, Visiting/Exchange', 'STU8', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_122', 'UC Trustee', 'TRU', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_123', 'UIC User', 'UIC', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_124', 'Washington Park Artist', 'WASHPARKART', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_125', 'Woodlawn Academy Staff', 'WOODSTF', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_126', 'Woodlawn Academy Student', 'WOODSTU', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_127', 'Borrow Direct Borrower', 'BORDIR', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_128', 'UC Retired Staff', 'RETIREE', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_129', 'UC Student, Unregistered', 'UNREST', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_130', 'MBL Staff', 'MBLSTF', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_131', 'IUN Faculty', 'IUNFAC', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_132', 'BTAA Staff', 'BTAASTF', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_133', 'Polsky Exchange Member', 'POLSKY', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_134', 'CASE Member', 'CASEMEM', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_135', 'Bulletin of Atomic Scientists Staff', 'BASSTF', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_136', 'MacLean Center Fellows', 'MAC_FEL', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_137', 'SHARES On-Site Borrower', 'SHARESBOR', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_138', 'Charter School Student', 'CHRSTU', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller'),
('opt_139', 'Visitor', 'VISITOR', CURRENT_TIMESTAMP, 'jemiller', CURRENT_TIMESTAMP, 'jemiller');
CREATE VIEW uc.oclc_numbers AS
SELECT * FROM
(
SELECT 
id, 
instance_id,
uc.int_cast(SUBSTRING(value, 8)) AS content
FROM uc.identifiers
WHERE identifier_type_id = '439bfbae-75bc-4f74-9fc7-b2a2d47ce3ef'
) a
WHERE content IS NOT NULL;
CREATE VIEW uc.isbns AS
SELECT * FROM
(
SELECT 
id, 
instance_id,
NULLIF(REGEXP_REPLACE(UPPER(TRIM(value)), '( .*)|[^0-9X]', '', 'g'), '')::VARCHAR(128) AS content
FROM uc.identifiers
WHERE identifier_type_id = '8261054f-be78-422d-bd51-4ed9f33c3422'
) a
WHERE content IS NOT NULL;
CREATE VIEW uc.issns AS
SELECT * FROM
(
SELECT 
id, 
instance_id,
NULLIF(TRIM(REGEXP_REPLACE(value, '[^0-9X]', '', 'g')), '')::VARCHAR(128) AS content
FROM uc.identifiers
WHERE identifier_type_id = '913300b2-03ed-469a-8179-c1092c991227'
) a
WHERE content IS NOT NULL;
CREATE VIEW uc.holding_donors AS
SELECT
uuid_generate_v5(holding_id, UPPER(TRIM(note))) AS id,
holding_id,
UPPER(TRIM(note)) AS donor_code,
created_date,
created_by_user_id,
updated_date,
updated_by_user_id
FROM uc.holding_notes
WHERE holding_note_type_id = '88914775-f677-4759-b57b-1a33b90b24e0';
CREATE VIEW uc.item_donors AS
SELECT
uuid_generate_v5(item_id, UPPER(TRIM(note))) AS id,
item_id,
UPPER(TRIM(note)) AS donor_code,
created_date,
created_by_user_id,
updated_date,
updated_by_user_id
FROM uc.item_notes
WHERE item_note_type_id = 'f3ae3823-d096-4c65-8734-0c1efd2ffea8'

