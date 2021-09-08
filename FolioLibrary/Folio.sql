DROP SCHEMA IF EXISTS uc CASCADE;
CREATE SCHEMA uc;
CREATE FUNCTION uc.timestamp_cast(IN TEXT) RETURNS TIMESTAMP WITH TIME ZONE LANGUAGE PLPGSQL IMMUTABLE AS 'BEGIN RETURN $1::TIMESTAMP WITH TIME ZONE; EXCEPTION WHEN OTHERS THEN RETURN NULL; END';
CREATE VIEW uc.acquisitions_units AS
SELECT
id AS id,
jsonb->>'name' AS name,
CAST(jsonb->>'isDeleted' AS BOOLEAN) AS is_deleted,
CAST(jsonb->>'protectCreate' AS BOOLEAN) AS protect_create,
CAST(jsonb->>'protectRead' AS BOOLEAN) AS protect_read,
CAST(jsonb->>'protectUpdate' AS BOOLEAN) AS protect_update,
CAST(jsonb->>'protectDelete' AS BOOLEAN) AS protect_delete,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_orders_storage.acquisitions_unit;
CREATE VIEW uc.address_types AS
SELECT
id AS id,
jsonb->>'addressType' AS address_type,
jsonb->>'desc' AS desc,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_users.addresstype;
CREATE VIEW uc.alerts AS
SELECT
id AS id,
jsonb->>'alert' AS alert,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.alert;
CREATE VIEW uc.alternative_title_types AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.alternative_title_type;
CREATE VIEW uc.auth_attempts AS
SELECT
id AS id,
CAST(jsonb->>'userId' AS UUID) AS user_id,
uc.TIMESTAMP_CAST(jsonb->>'lastAttempt') AS last_attempt,
CAST(jsonb->>'attemptCount' AS INTEGER) AS attempt_count,
jsonb_pretty(jsonb) AS content
FROM diku_mod_login.auth_attempts;
CREATE VIEW uc.auth_credentials_histories AS
SELECT
id AS id,
CAST(jsonb->>'userId' AS UUID) AS user_id,
jsonb->>'hash' AS hash,
jsonb->>'salt' AS salt,
uc.TIMESTAMP_CAST(jsonb->>'date') AS date,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_login.auth_credentials_history;
CREATE VIEW uc.batch_groups AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'description' AS description,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_invoice_storage.batch_groups;
CREATE VIEW uc.batch_voucher_batched_voucher_batched_voucher_line_fund_codes AS
SELECT
id AS id,
batch_voucher_batched_voucher_batched_voucher_line_id AS batch_voucher_batched_voucher_batched_voucher_line_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS batch_voucher_batched_voucher_batched_voucher_line_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS batch_voucher_batched_voucher_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS batch_voucher_id, value AS jsonb FROM diku_mod_invoice_storage.batch_vouchers, jsonb_array_elements((jsonb->>'batchedVouchers')::jsonb) WITH ORDINALITY) a, jsonb_array_elements((jsonb->>'batchedVoucherLines')::jsonb) WITH ORDINALITY) a, jsonb_array_elements_text((jsonb->>'fundCodes')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.batch_voucher_batched_voucher_batched_voucher_lines AS
SELECT
id AS id,
batch_voucher_batched_voucher_id AS batch_voucher_batched_voucher_id,
CAST(jsonb->>'amount' AS DECIMAL(19,2)) AS amount,
jsonb->>'externalAccountNumber' AS external_account_number
FROM (SELECT id::text || ordinality::text AS id, id AS batch_voucher_batched_voucher_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS batch_voucher_id, value AS jsonb FROM diku_mod_invoice_storage.batch_vouchers, jsonb_array_elements((jsonb->>'batchedVouchers')::jsonb) WITH ORDINALITY) a, jsonb_array_elements((jsonb->>'batchedVoucherLines')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.batch_voucher_batched_vouchers AS
SELECT
id AS id,
batch_voucher_id AS batch_voucher_id,
jsonb->>'accountingCode' AS accounting_code,
jsonb->>'accountNo' AS account_no,
CAST(jsonb->>'amount' AS DECIMAL(19,2)) AS amount,
jsonb->>'disbursementNumber' AS disbursement_number,
uc.TIMESTAMP_CAST(jsonb->>'disbursementDate') AS disbursement_date,
CAST(jsonb->>'disbursementAmount' AS DECIMAL(19,2)) AS disbursement_amount,
CAST(jsonb->>'enclosureNeeded' AS BOOLEAN) AS enclosure_needed,
CAST(jsonb->>'exchangeRate' AS DECIMAL(19,2)) AS exchange_rate,
jsonb->>'folioInvoiceNo' AS folio_invoice_no,
jsonb->>'invoiceCurrency' AS invoice_currency,
jsonb->>'invoiceNote' AS invoice_note,
jsonb->>'status' AS status,
jsonb->>'systemCurrency' AS system_currency,
jsonb->>'type' AS type,
jsonb->>'vendorInvoiceNo' AS vendor_invoice_no,
jsonb->>'vendorName' AS vendor_name,
uc.TIMESTAMP_CAST(jsonb->>'voucherDate') AS voucher_date,
jsonb->>'voucherNumber' AS voucher_number,
jsonb#>>'{vendorAddress,addressLine1}' AS vendor_address_address_line1,
jsonb#>>'{vendorAddress,addressLine2}' AS vendor_address_address_line2,
jsonb#>>'{vendorAddress,city}' AS vendor_address_city,
jsonb#>>'{vendorAddress,stateRegion}' AS vendor_address_state_region,
jsonb#>>'{vendorAddress,zipCode}' AS vendor_address_zip_code,
jsonb#>>'{vendorAddress,country}' AS vendor_address_country
FROM (SELECT id::text || ordinality::text AS id, id AS batch_voucher_id, value AS jsonb FROM diku_mod_invoice_storage.batch_vouchers, jsonb_array_elements((jsonb->>'batchedVouchers')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.batch_vouchers AS
SELECT
id AS id,
jsonb->>'batchGroup' AS batch_group,
uc.TIMESTAMP_CAST(jsonb->>'created') AS created,
uc.TIMESTAMP_CAST(jsonb->>'start') AS start,
uc.TIMESTAMP_CAST(jsonb->>'end') AS end,
CAST(jsonb->>'totalRecords' AS INTEGER) AS total_records,
jsonb_pretty(jsonb) AS content
FROM diku_mod_invoice_storage.batch_vouchers;
CREATE VIEW uc.batch_voucher_exports AS
SELECT
id AS id,
jsonb->>'status' AS status,
jsonb->>'message' AS message,
CAST(jsonb->>'batchGroupId' AS UUID) AS batch_group_id,
uc.TIMESTAMP_CAST(jsonb->>'start') AS start,
uc.TIMESTAMP_CAST(jsonb->>'end') AS end,
CAST(jsonb->>'batchVoucherId' AS UUID) AS batch_voucher_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_invoice_storage.batch_voucher_exports;
CREATE VIEW uc.batch_voucher_export_config_weekdays AS
SELECT
id AS id,
batch_voucher_export_config_id AS batch_voucher_export_config_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS batch_voucher_export_config_id, value AS jsonb FROM diku_mod_invoice_storage.batch_voucher_export_configs, jsonb_array_elements_text((jsonb->>'weekdays')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.batch_voucher_export_configs AS
SELECT
id AS id,
CAST(jsonb->>'batchGroupId' AS UUID) AS batch_group_id,
CAST(jsonb->>'enableScheduledExport' AS BOOLEAN) AS enable_scheduled_export,
jsonb->>'format' AS format,
jsonb->>'startTime' AS start_time,
jsonb->>'uploadURI' AS upload_uri,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_invoice_storage.batch_voucher_export_configs;
CREATE VIEW uc.blocks AS
SELECT
id AS id,
jsonb->>'type' AS type,
jsonb->>'desc' AS desc,
jsonb->>'code' AS code,
jsonb->>'staffInformation' AS staff_information,
jsonb->>'patronMessage' AS patron_message,
uc.TIMESTAMP_CAST(jsonb->>'expirationDate') AS expiration_date,
CAST(jsonb->>'borrowing' AS BOOLEAN) AS borrowing,
CAST(jsonb->>'renewals' AS BOOLEAN) AS renewals,
CAST(jsonb->>'requests' AS BOOLEAN) AS requests,
CAST(jsonb->>'userId' AS UUID) AS user_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_feesfines.manualblocks;
CREATE VIEW uc.block_conditions AS
SELECT
id AS id,
jsonb->>'name' AS name,
CAST(jsonb->>'blockBorrowing' AS BOOLEAN) AS block_borrowing,
CAST(jsonb->>'blockRenewals' AS BOOLEAN) AS block_renewals,
CAST(jsonb->>'blockRequests' AS BOOLEAN) AS block_requests,
jsonb->>'valueType' AS value_type,
jsonb->>'message' AS message,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_patron_blocks.patron_block_conditions;
CREATE VIEW uc.block_limits AS
SELECT
id AS id,
CAST(jsonb->>'patronGroupId' AS UUID) AS group_id,
CAST(jsonb->>'conditionId' AS UUID) AS condition_id,
CAST(jsonb->>'value' AS DECIMAL(19,2)) AS value,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content,
conditionid AS conditionid
FROM diku_mod_patron_blocks.patron_block_limits;
CREATE VIEW uc.bound_with_parts AS
SELECT
id AS id,
CAST(jsonb->>'holdingsRecordId' AS UUID) AS holding_id,
CAST(jsonb->>'itemId' AS UUID) AS item_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.bound_with_part;
CREATE VIEW uc.budget_acquisitions_units AS
SELECT
id AS id,
budget_id AS budget_id,
CAST(jsonb AS UUID) AS acquisitions_unit_id
FROM (SELECT id::text || ordinality::text AS id, id AS budget_id, value AS jsonb FROM diku_mod_finance_storage.budget, jsonb_array_elements_text((jsonb->>'acqUnitIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.budget_tags AS
SELECT
id AS id,
budget_id AS budget_id,
CAST(jsonb AS UUID) AS tag_id
FROM (SELECT id::text || ordinality::text AS id, id AS budget_id, value AS jsonb FROM diku_mod_finance_storage.budget, jsonb_array_elements_text((jsonb#>>'{tags,tagList}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.budgets AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'budgetStatus' AS budget_status,
CAST(jsonb->>'allowableEncumbrance' AS DECIMAL(19,2)) AS allowable_encumbrance,
CAST(jsonb->>'allowableExpenditure' AS DECIMAL(19,2)) AS allowable_expenditure,
CAST(jsonb->>'allocated' AS DECIMAL(19,2)) AS allocated,
CAST(jsonb->>'awaitingPayment' AS DECIMAL(19,2)) AS awaiting_payment,
CAST(jsonb->>'available' AS DECIMAL(19,2)) AS available,
CAST(jsonb->>'encumbered' AS DECIMAL(19,2)) AS encumbered,
CAST(jsonb->>'expenditures' AS DECIMAL(19,2)) AS expenditures,
CAST(jsonb->>'netTransfers' AS DECIMAL(19,2)) AS net_transfers,
CAST(jsonb->>'unavailable' AS DECIMAL(19,2)) AS unavailable,
CAST(jsonb->>'overEncumbrance' AS DECIMAL(19,2)) AS over_encumbrance,
CAST(jsonb->>'overExpended' AS DECIMAL(19,2)) AS over_expended,
CAST(jsonb->>'fundId' AS UUID) AS fund_id,
CAST(jsonb->>'fiscalYearId' AS UUID) AS fiscal_year_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
CAST(jsonb->>'initialAllocation' AS DECIMAL(19,2)) AS initial_allocation,
CAST(jsonb->>'allocationTo' AS DECIMAL(19,2)) AS allocation_to,
CAST(jsonb->>'allocationFrom' AS DECIMAL(19,2)) AS allocation_from,
CAST(jsonb->>'totalFunding' AS DECIMAL(19,2)) AS total_funding,
CAST(jsonb->>'cashBalance' AS DECIMAL(19,2)) AS cash_balance,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_finance_storage.budget;
CREATE VIEW uc.budget_expense_classes AS
SELECT
id AS id,
CAST(jsonb->>'budgetId' AS UUID) AS budget_id,
CAST(jsonb->>'expenseClassId' AS UUID) AS expense_class_id,
jsonb->>'status' AS status,
jsonb_pretty(jsonb) AS content
FROM diku_mod_finance_storage.budget_expense_class;
CREATE VIEW uc.budget_groups AS
SELECT
id AS id,
CAST(jsonb->>'budgetId' AS UUID) AS budget_id,
CAST(jsonb->>'groupId' AS UUID) AS group_id,
CAST(jsonb->>'fiscalYearId' AS UUID) AS fiscal_year_id,
CAST(jsonb->>'fundId' AS UUID) AS fund_id,
jsonb_pretty(jsonb) AS content
FROM diku_mod_finance_storage.group_fund_fiscal_year;
CREATE VIEW uc.call_number_types AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.call_number_type;
CREATE VIEW uc.campuses AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'code' AS code,
CAST(jsonb->>'institutionId' AS UUID) AS institution_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.loccampus;
CREATE VIEW uc.cancellation_reasons AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'description' AS description,
jsonb->>'publicDescription' AS public_description,
CAST(jsonb->>'requiresAdditionalInformation' AS BOOLEAN) AS requires_additional_information,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_circulation_storage.cancellation_reason;
CREATE VIEW uc.categories AS
SELECT
id AS id,
jsonb->>'value' AS value,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_organizations_storage.categories;
CREATE VIEW uc.check_ins AS
SELECT
id AS id,
uc.TIMESTAMP_CAST(jsonb->>'occurredDateTime') AS occurred_date_time,
CAST(jsonb->>'itemId' AS UUID) AS item_id,
jsonb->>'itemStatusPriorToCheckIn' AS item_status_prior_to_check_in,
CAST(jsonb->>'requestQueueSize' AS INTEGER) AS request_queue_size,
CAST(jsonb->>'itemLocationId' AS UUID) AS item_location_id,
CAST(jsonb->>'servicePointId' AS UUID) AS service_point_id,
CAST(jsonb->>'performedByUserId' AS UUID) AS performed_by_user_id,
jsonb_pretty(jsonb) AS content
FROM diku_mod_circulation_storage.check_in;
CREATE VIEW uc.circulation_rules AS
SELECT
id AS id,
jsonb->>'rulesAsText' AS rules_as_text,
jsonb_pretty(jsonb) AS content,
lock AS lock
FROM diku_mod_circulation_storage.circulation_rules;
CREATE VIEW uc.classification_types AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.classification_type;
CREATE VIEW uc.close_reasons AS
SELECT
id AS id,
jsonb->>'reason' AS name,
jsonb->>'source' AS source,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.reasons_for_closure;
CREATE VIEW uc.comments AS
SELECT
id AS id,
CAST(jsonb->>'paid' AS BOOLEAN) AS paid,
CAST(jsonb->>'waived' AS BOOLEAN) AS waived,
CAST(jsonb->>'refunded' AS BOOLEAN) AS refunded,
CAST(jsonb->>'transferredManually' AS BOOLEAN) AS transferred_manually,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_feesfines.comments;
CREATE VIEW uc.configurations AS
SELECT
id AS id,
jsonb->>'module' AS module,
jsonb->>'configName' AS config_name,
jsonb->>'code' AS code,
jsonb->>'description' AS description,
CAST(jsonb->>'default' AS BOOLEAN) AS default,
CAST(jsonb->>'enabled' AS BOOLEAN) AS enabled,
jsonb->>'value' AS value,
CAST(jsonb->>'userId' AS VARCHAR(128)) AS user_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_configuration.config_data;
CREATE VIEW uc.contact_phone_number_categories AS
SELECT
id AS id,
contact_phone_number_id AS contact_phone_number_id,
CAST(jsonb AS UUID) AS category_id
FROM (SELECT id::text || ordinality::text AS id, id AS contact_phone_number_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS contact_id, value AS jsonb FROM diku_mod_organizations_storage.contacts, jsonb_array_elements((jsonb->>'phoneNumbers')::jsonb) WITH ORDINALITY) a, jsonb_array_elements_text((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.contact_phone_numbers AS
SELECT
id AS id,
contact_id AS contact_id,
CAST(jsonb->>'id' AS UUID) AS id2,
jsonb->>'phoneNumber' AS phone_number,
jsonb->>'type' AS type,
CAST(jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
jsonb->>'language' AS language,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username
FROM (SELECT id::text || ordinality::text AS id, id AS contact_id, value AS jsonb FROM diku_mod_organizations_storage.contacts, jsonb_array_elements((jsonb->>'phoneNumbers')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.contact_email_categories AS
SELECT
id AS id,
contact_email_id AS contact_email_id,
CAST(jsonb AS UUID) AS category_id
FROM (SELECT id::text || ordinality::text AS id, id AS contact_email_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS contact_id, value AS jsonb FROM diku_mod_organizations_storage.contacts, jsonb_array_elements((jsonb->>'emails')::jsonb) WITH ORDINALITY) a, jsonb_array_elements_text((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.contact_emails AS
SELECT
id AS id,
contact_id AS contact_id,
CAST(jsonb->>'id' AS UUID) AS id2,
jsonb->>'value' AS value,
jsonb->>'description' AS description,
CAST(jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
jsonb->>'language' AS language,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username
FROM (SELECT id::text || ordinality::text AS id, id AS contact_id, value AS jsonb FROM diku_mod_organizations_storage.contacts, jsonb_array_elements((jsonb->>'emails')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.contact_address_categories AS
SELECT
id AS id,
contact_address_id AS contact_address_id,
CAST(jsonb AS UUID) AS category_id
FROM (SELECT id::text || ordinality::text AS id, id AS contact_address_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS contact_id, value AS jsonb FROM diku_mod_organizations_storage.contacts, jsonb_array_elements((jsonb->>'addresses')::jsonb) WITH ORDINALITY) a, jsonb_array_elements_text((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.contact_addresses AS
SELECT
id AS id,
contact_id AS contact_id,
CAST(jsonb->>'id' AS UUID) AS id2,
jsonb->>'addressLine1' AS address_line1,
jsonb->>'addressLine2' AS address_line2,
jsonb->>'city' AS city,
jsonb->>'stateRegion' AS state_region,
jsonb->>'zipCode' AS zip_code,
jsonb->>'country' AS country,
CAST(jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
jsonb->>'language' AS language,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username
FROM (SELECT id::text || ordinality::text AS id, id AS contact_id, value AS jsonb FROM diku_mod_organizations_storage.contacts, jsonb_array_elements((jsonb->>'addresses')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.contact_url_categories AS
SELECT
id AS id,
contact_url_id AS contact_url_id,
CAST(jsonb AS UUID) AS category_id
FROM (SELECT id::text || ordinality::text AS id, id AS contact_url_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS contact_id, value AS jsonb FROM diku_mod_organizations_storage.contacts, jsonb_array_elements((jsonb->>'urls')::jsonb) WITH ORDINALITY) a, jsonb_array_elements_text((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.contact_urls AS
SELECT
id AS id,
contact_id AS contact_id,
CAST(jsonb->>'id' AS UUID) AS id2,
jsonb->>'value' AS value,
jsonb->>'description' AS description,
jsonb->>'language' AS language,
CAST(jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
jsonb->>'notes' AS notes,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username
FROM (SELECT id::text || ordinality::text AS id, id AS contact_id, value AS jsonb FROM diku_mod_organizations_storage.contacts, jsonb_array_elements((jsonb->>'urls')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.contact_categories AS
SELECT
id AS id,
contact_id AS contact_id,
CAST(jsonb AS UUID) AS category_id
FROM (SELECT id::text || ordinality::text AS id, id AS contact_id, value AS jsonb FROM diku_mod_organizations_storage.contacts, jsonb_array_elements_text((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.contacts AS
SELECT
id AS id,
CAST(CONCAT_WS(' ', jsonb#>>'{firstName}', jsonb#>>'{lastName}') AS VARCHAR(1024)) AS name,
jsonb->>'prefix' AS prefix,
jsonb->>'firstName' AS first_name,
jsonb->>'lastName' AS last_name,
jsonb->>'language' AS language,
jsonb->>'notes' AS notes,
CAST(jsonb->>'inactive' AS BOOLEAN) AS inactive,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_organizations_storage.contacts;
CREATE VIEW uc.contributor_name_types AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'ordering' AS ordering,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.contributor_name_type;
CREATE VIEW uc.contributor_types AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'code' AS code,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.contributor_type;
CREATE VIEW uc.custom_field_values AS
SELECT
id AS id,
custom_field_id AS custom_field_id,
jsonb->>'id' AS id2,
jsonb->>'value' AS value,
CAST(jsonb->>'default' AS BOOLEAN) AS default
FROM (SELECT id::text || ordinality::text AS id, id AS custom_field_id, value AS jsonb FROM diku_mod_users.custom_fields, jsonb_array_elements((jsonb#>>'{selectField,options,values}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.custom_fields AS
SELECT
id AS id,
jsonb->>'name' AS name,
CAST(jsonb->>'refId' AS VARCHAR(128)) AS ref_id,
jsonb->>'type' AS type,
jsonb->>'entityType' AS entity_type,
CAST(jsonb->>'visible' AS BOOLEAN) AS visible,
CAST(jsonb->>'required' AS BOOLEAN) AS required,
CAST(jsonb->>'isRepeatable' AS BOOLEAN) AS is_repeatable,
CAST(jsonb->>'order' AS INTEGER) AS order,
jsonb->>'helpText' AS help_text,
CAST(jsonb#>>'{checkboxField,default}' AS BOOLEAN) AS checkbox_field_default,
CAST(jsonb#>>'{selectField,multiSelect}' AS BOOLEAN) AS select_field_multi_select,
jsonb#>>'{selectField,options,sortingOrder}' AS select_field_options_sorting_order,
jsonb#>>'{textField,fieldFormat}' AS text_field_field_format,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_users.custom_fields;
CREATE VIEW uc.departments AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'code' AS code,
CAST(jsonb->>'usageNumber' AS INTEGER) AS usage_number,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_users.departments;
CREATE VIEW uc.documents AS
SELECT
id AS id,
jsonb#>>'{documentMetadata,name}' AS document_metadata_name,
CAST(jsonb#>>'{documentMetadata,invoiceId}' AS UUID) AS document_metadata_invoice_id,
jsonb#>>'{documentMetadata,url}' AS document_metadata_url,
uc.TIMESTAMP_CAST(jsonb#>>'{documentMetadata,metadata,createdDate}') AS document_metadata_metadata_created_date,
CAST(jsonb#>>'{documentMetadata,metadata,createdByUserId}' AS UUID) AS document_metadata_metadata_created_by_user_id,
jsonb#>>'{documentMetadata,metadata,createdByUsername}' AS document_metadata_metadata_created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{documentMetadata,metadata,updatedDate}') AS document_metadata_metadata_updated_date,
CAST(jsonb#>>'{documentMetadata,metadata,updatedByUserId}' AS UUID) AS document_metadata_metadata_updated_by_user_id,
jsonb#>>'{documentMetadata,metadata,updatedByUsername}' AS document_metadata_metadata_updated_by_username,
jsonb#>>'{contents,data}' AS contents_data,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content,
invoiceid AS invoiceid,
document_data AS document_data
FROM diku_mod_invoice_storage.documents;
CREATE VIEW uc.electronic_access_relationships AS
SELECT
id AS id,
jsonb->>'name' AS name,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.electronic_access_relationship;
CREATE VIEW uc.error_records AS
SELECT
id AS id,
content AS content,
description AS description
FROM diku_mod_source_record_storage.error_records_lb;
CREATE VIEW uc.event_logs AS
SELECT
id AS id,
jsonb->>'tenant' AS tenant,
CAST(jsonb->>'userId' AS UUID) AS user_id,
jsonb->>'ip' AS ip,
jsonb->>'browserInformation' AS browser_information,
uc.TIMESTAMP_CAST(jsonb->>'timestamp') AS timestamp,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_login.event_logs;
CREATE VIEW uc.expense_classes AS
SELECT
id AS id,
jsonb->>'code' AS code,
jsonb->>'externalAccountNumberExt' AS external_account_number_ext,
jsonb->>'name' AS name,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_finance_storage.expense_class;
CREATE VIEW uc.export_config_credentials AS
SELECT
id AS id,
jsonb->>'username' AS username,
jsonb->>'password' AS password,
CAST(jsonb->>'exportConfigId' AS UUID) AS export_config_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_invoice_storage.export_config_credentials;
CREATE VIEW uc.fees AS
SELECT
id AS id,
CAST(jsonb->>'amount' AS DECIMAL(19,2)) AS amount,
CAST(jsonb->>'remaining' AS DECIMAL(19,2)) AS remaining,
uc.TIMESTAMP_CAST(jsonb->>'dateCreated') AS date_created,
uc.TIMESTAMP_CAST(jsonb->>'dateUpdated') AS date_updated,
jsonb#>>'{status,name}' AS status_name,
jsonb#>>'{paymentStatus,name}' AS payment_status_name,
jsonb->>'feeFineType' AS fee_fine_type,
jsonb->>'feeFineOwner' AS fee_fine_owner,
jsonb->>'title' AS title,
jsonb->>'callNumber' AS call_number,
jsonb->>'barcode' AS barcode,
jsonb->>'materialType' AS material_type,
jsonb#>>'{itemStatus,name}' AS item_status_name,
jsonb->>'location' AS location,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
uc.TIMESTAMP_CAST(jsonb->>'dueDate') AS due_date,
uc.TIMESTAMP_CAST(jsonb->>'returnedDate') AS returned_date,
CAST(jsonb->>'loanId' AS UUID) AS loan_id,
CAST(jsonb->>'userId' AS UUID) AS user_id,
CAST(jsonb->>'itemId' AS UUID) AS item_id,
CAST(jsonb->>'materialTypeId' AS UUID) AS material_type_id,
CAST(jsonb->>'feeFineId' AS UUID) AS fee_type_id,
CAST(jsonb->>'ownerId' AS UUID) AS owner_id,
CAST(jsonb->>'holdingsRecordId' AS UUID) AS holding_id,
CAST(jsonb->>'instanceId' AS UUID) AS instance_id,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_feesfines.accounts;
CREATE VIEW uc.fee_types AS
SELECT
id AS id,
CAST(jsonb->>'automatic' AS BOOLEAN) AS automatic,
jsonb->>'feeFineType' AS fee_fine_type,
CAST(jsonb->>'defaultAmount' AS DECIMAL(19,2)) AS default_amount,
CAST(jsonb->>'chargeNoticeId' AS UUID) AS charge_notice_id,
CAST(jsonb->>'actionNoticeId' AS UUID) AS action_notice_id,
CAST(jsonb->>'ownerId' AS UUID) AS owner_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_feesfines.feefines;
CREATE VIEW uc.finance_group_acquisitions_units AS
SELECT
id AS id,
finance_group_id AS finance_group_id,
CAST(jsonb AS UUID) AS acquisitions_unit_id
FROM (SELECT id::text || ordinality::text AS id, id AS finance_group_id, value AS jsonb FROM diku_mod_finance_storage.groups, jsonb_array_elements_text((jsonb->>'acqUnitIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.finance_groups AS
SELECT
id AS id,
jsonb->>'code' AS code,
jsonb->>'description' AS description,
jsonb->>'name' AS name,
jsonb->>'status' AS status,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_finance_storage.groups;
CREATE VIEW uc.fiscal_year_acquisitions_units AS
SELECT
id AS id,
fiscal_year_id AS fiscal_year_id,
CAST(jsonb AS UUID) AS acquisitions_unit_id
FROM (SELECT id::text || ordinality::text AS id, id AS fiscal_year_id, value AS jsonb FROM diku_mod_finance_storage.fiscal_year, jsonb_array_elements_text((jsonb->>'acqUnitIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.fiscal_years AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'code' AS code,
jsonb->>'currency' AS currency,
jsonb->>'description' AS description,
uc.TIMESTAMP_CAST(jsonb->>'periodStart') AS period_start,
uc.TIMESTAMP_CAST(jsonb->>'periodEnd') AS period_end,
jsonb->>'series' AS series,
CAST(jsonb#>>'{financialSummary,allocated}' AS DECIMAL(19,2)) AS financial_summary_allocated,
CAST(jsonb#>>'{financialSummary,available}' AS DECIMAL(19,2)) AS financial_summary_available,
CAST(jsonb#>>'{financialSummary,unavailable}' AS DECIMAL(19,2)) AS financial_summary_unavailable,
CAST(jsonb#>>'{financialSummary,initialAllocation}' AS DECIMAL(19,2)) AS financial_summary_initial_allocation,
CAST(jsonb#>>'{financialSummary,allocationTo}' AS DECIMAL(19,2)) AS financial_summary_allocation_to,
CAST(jsonb#>>'{financialSummary,allocationFrom}' AS DECIMAL(19,2)) AS financial_summary_allocation_from,
CAST(jsonb#>>'{financialSummary,totalFunding}' AS DECIMAL(19,2)) AS financial_summary_total_funding,
CAST(jsonb#>>'{financialSummary,cashBalance}' AS DECIMAL(19,2)) AS financial_summary_cash_balance,
CAST(jsonb#>>'{financialSummary,awaitingPayment}' AS DECIMAL(19,2)) AS financial_summary_awaiting_payment,
CAST(jsonb#>>'{financialSummary,encumbered}' AS DECIMAL(19,2)) AS financial_summary_encumbered,
CAST(jsonb#>>'{financialSummary,expenditures}' AS DECIMAL(19,2)) AS financial_summary_expenditures,
CAST(jsonb#>>'{financialSummary,overEncumbrance}' AS DECIMAL(19,2)) AS financial_summary_over_encumbrance,
CAST(jsonb#>>'{financialSummary,overExpended}' AS DECIMAL(19,2)) AS financial_summary_over_expended,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_finance_storage.fiscal_year;
CREATE VIEW uc.fixed_due_date_schedule_schedules AS
SELECT
id AS id,
fixed_due_date_schedule_id AS fixed_due_date_schedule_id,
uc.TIMESTAMP_CAST(jsonb->>'from') AS from,
uc.TIMESTAMP_CAST(jsonb->>'to') AS to,
uc.TIMESTAMP_CAST(jsonb->>'due') AS due
FROM (SELECT id::text || ordinality::text AS id, id AS fixed_due_date_schedule_id, value AS jsonb FROM diku_mod_circulation_storage.fixed_due_date_schedule, jsonb_array_elements((jsonb->>'schedules')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.fixed_due_date_schedules AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'description' AS description,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_circulation_storage.fixed_due_date_schedule;
CREATE VIEW uc.allocated_from_funds AS
SELECT
id AS id,
fund_id AS fund_id,
CAST(jsonb AS UUID) AS from_fund_id
FROM (SELECT id::text || ordinality::text AS id, id AS fund_id, value AS jsonb FROM diku_mod_finance_storage.fund, jsonb_array_elements_text((jsonb->>'allocatedFromIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.allocated_to_funds AS
SELECT
id AS id,
fund_id AS fund_id,
CAST(jsonb AS UUID) AS to_fund_id
FROM (SELECT id::text || ordinality::text AS id, id AS fund_id, value AS jsonb FROM diku_mod_finance_storage.fund, jsonb_array_elements_text((jsonb->>'allocatedToIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.fund_acquisitions_units AS
SELECT
id AS id,
fund_id AS fund_id,
CAST(jsonb AS UUID) AS acquisitions_unit_id
FROM (SELECT id::text || ordinality::text AS id, id AS fund_id, value AS jsonb FROM diku_mod_finance_storage.fund, jsonb_array_elements_text((jsonb->>'acqUnitIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.fund_tags AS
SELECT
id AS id,
fund_id AS fund_id,
CAST(jsonb AS UUID) AS tag_id
FROM (SELECT id::text || ordinality::text AS id, id AS fund_id, value AS jsonb FROM diku_mod_finance_storage.fund, jsonb_array_elements_text((jsonb#>>'{tags,tagList}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.funds AS
SELECT
id AS id,
jsonb->>'code' AS code,
jsonb->>'description' AS description,
jsonb->>'externalAccountNo' AS external_account_no,
jsonb->>'fundStatus' AS fund_status,
CAST(jsonb->>'fundTypeId' AS UUID) AS fund_type_id,
CAST(jsonb->>'ledgerId' AS UUID) AS ledger_id,
jsonb->>'name' AS name,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_finance_storage.fund;
CREATE VIEW uc.fund_types AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb_pretty(jsonb) AS content
FROM diku_mod_finance_storage.fund_type;
CREATE VIEW uc.groups AS
SELECT
id AS id,
jsonb->>'group' AS group,
jsonb->>'desc' AS desc,
CAST(jsonb->>'expirationOffsetInDays' AS INTEGER) AS expiration_offset_in_days,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_users.groups;
CREATE VIEW uc.holding_former_ids AS
SELECT
id AS id,
holding_id AS holding_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS holding_id, value AS jsonb FROM diku_mod_inventory_storage.holdings_record, jsonb_array_elements_text((jsonb->>'formerIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.holding_electronic_accesses AS
SELECT
id AS id,
holding_id AS holding_id,
jsonb->>'uri' AS uri,
jsonb->>'linkText' AS link_text,
jsonb->>'materialsSpecification' AS materials_specification,
jsonb->>'publicNote' AS public_note,
CAST(jsonb->>'relationshipId' AS UUID) AS relationship_id
FROM (SELECT id::text || ordinality::text AS id, id AS holding_id, value AS jsonb FROM diku_mod_inventory_storage.holdings_record, jsonb_array_elements((jsonb->>'electronicAccess')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.holding_notes AS
SELECT
id AS id,
holding_id AS holding_id,
CAST(jsonb->>'holdingsNoteTypeId' AS UUID) AS holding_note_type_id,
jsonb->>'note' AS note,
CAST(jsonb->>'staffOnly' AS BOOLEAN) AS staff_only
FROM (SELECT id::text || ordinality::text AS id, id AS holding_id, value AS jsonb FROM diku_mod_inventory_storage.holdings_record, jsonb_array_elements((jsonb->>'notes')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.extents AS
SELECT
id AS id,
holding_id AS holding_id,
jsonb->>'statement' AS statement,
jsonb->>'note' AS note,
jsonb->>'staffNote' AS staff_note
FROM (SELECT id::text || ordinality::text AS id, id AS holding_id, value AS jsonb FROM diku_mod_inventory_storage.holdings_record, jsonb_array_elements((jsonb->>'holdingsStatements')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.index_statements AS
SELECT
id AS id,
holding_id AS holding_id,
jsonb->>'statement' AS statement,
jsonb->>'note' AS note,
jsonb->>'staffNote' AS staff_note
FROM (SELECT id::text || ordinality::text AS id, id AS holding_id, value AS jsonb FROM diku_mod_inventory_storage.holdings_record, jsonb_array_elements((jsonb->>'holdingsStatementsForIndexes')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.supplement_statements AS
SELECT
id AS id,
holding_id AS holding_id,
jsonb->>'statement' AS statement,
jsonb->>'note' AS note,
jsonb->>'staffNote' AS staff_note
FROM (SELECT id::text || ordinality::text AS id, id AS holding_id, value AS jsonb FROM diku_mod_inventory_storage.holdings_record, jsonb_array_elements((jsonb->>'holdingsStatementsForSupplements')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.holding_entries AS
SELECT
id AS id,
holding_id AS holding_id,
CAST(jsonb->>'publicDisplay' AS BOOLEAN) AS public_display,
jsonb->>'enumeration' AS enumeration,
jsonb->>'chronology' AS chronology
FROM (SELECT id::text || ordinality::text AS id, id AS holding_id, value AS jsonb FROM diku_mod_inventory_storage.holdings_record, jsonb_array_elements((jsonb#>>'{receivingHistory,entries}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.holding_statistical_codes AS
SELECT
id AS id,
holding_id AS holding_id,
CAST(jsonb AS UUID) AS statistical_code_id
FROM (SELECT id::text || ordinality::text AS id, id AS holding_id, value AS jsonb FROM diku_mod_inventory_storage.holdings_record, jsonb_array_elements_text((jsonb->>'statisticalCodeIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.holding_tags AS
SELECT
id AS id,
holding_id AS holding_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS holding_id, value AS jsonb FROM diku_mod_inventory_storage.holdings_record, jsonb_array_elements_text((jsonb#>>'{tags,tagList}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.holdings AS
SELECT
id AS id,
CAST(jsonb->>'_version' AS INTEGER) AS _version,
CAST(jsonb->>'hrid' AS INTEGER) AS hrid,
CAST(jsonb->>'holdingsTypeId' AS UUID) AS holding_type_id,
CAST(jsonb->>'instanceId' AS UUID) AS instance_id,
CAST(jsonb->>'permanentLocationId' AS UUID) AS permanent_location_id,
CAST(jsonb->>'temporaryLocationId' AS UUID) AS temporary_location_id,
CAST(jsonb->>'effectiveLocationId' AS UUID) AS effective_location_id,
CAST(jsonb->>'callNumberTypeId' AS UUID) AS call_number_type_id,
jsonb->>'callNumberPrefix' AS call_number_prefix,
jsonb->>'callNumber' AS call_number,
jsonb->>'callNumberSuffix' AS call_number_suffix,
jsonb->>'shelvingTitle' AS shelving_title,
jsonb->>'acquisitionFormat' AS acquisition_format,
jsonb->>'acquisitionMethod' AS acquisition_method,
jsonb->>'receiptStatus' AS receipt_status,
CAST(jsonb->>'illPolicyId' AS UUID) AS ill_policy_id,
jsonb->>'retentionPolicy' AS retention_policy,
jsonb->>'digitizationPolicy' AS digitization_policy,
jsonb->>'copyNumber' AS copy_number,
jsonb->>'numberOfItems' AS number_of_items,
jsonb#>>'{receivingHistory,displayType}' AS receiving_history_display_type,
CAST(jsonb->>'discoverySuppress' AS BOOLEAN) AS discovery_suppress,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
CAST(jsonb->>'sourceId' AS UUID) AS source_id,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.holdings_record;
CREATE VIEW uc.holding_note_types AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.holdings_note_type;
CREATE VIEW uc.holding_types AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.holdings_type;
CREATE VIEW uc.hrid_settings AS
SELECT
id AS id,
jsonb#>>'{instances,prefix}' AS instances_prefix,
CAST(jsonb#>>'{instances,startNumber}' AS INTEGER) AS instances_start_number,
jsonb#>>'{holdings,prefix}' AS holdings_prefix,
CAST(jsonb#>>'{holdings,startNumber}' AS INTEGER) AS holdings_start_number,
jsonb#>>'{items,prefix}' AS items_prefix,
CAST(jsonb#>>'{items,startNumber}' AS INTEGER) AS items_start_number,
CAST(jsonb->>'commonRetainLeadingZeroes' AS BOOLEAN) AS common_retain_leading_zeroes,
jsonb_pretty(jsonb) AS content,
lock AS lock
FROM diku_mod_inventory_storage.hrid_settings;
CREATE VIEW uc.id_types AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.identifier_type;
CREATE VIEW uc.ill_policies AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.ill_policy;
CREATE VIEW uc.alternative_titles AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb->>'alternativeTitleTypeId' AS UUID) AS alternative_title_type_id,
jsonb->>'alternativeTitle' AS alternative_title
FROM (SELECT id::text || ordinality::text AS id, id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements((jsonb->>'alternativeTitles')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.editions AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements_text((jsonb->>'editions')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.series AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements_text((jsonb->>'series')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.identifiers AS
SELECT
id AS id,
instance_id AS instance_id,
jsonb->>'value' AS value,
CAST(jsonb->>'identifierTypeId' AS UUID) AS identifier_type_id
FROM (SELECT id::text || ordinality::text AS id, id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements((jsonb->>'identifiers')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.contributors AS
SELECT
id AS id,
instance_id AS instance_id,
jsonb->>'name' AS name,
CAST(jsonb->>'contributorTypeId' AS UUID) AS contributor_type_id,
jsonb->>'contributorTypeText' AS contributor_type_text,
CAST(jsonb->>'contributorNameTypeId' AS UUID) AS contributor_name_type_id,
CAST(jsonb->>'primary' AS BOOLEAN) AS primary
FROM (SELECT id::text || ordinality::text AS id, id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements((jsonb->>'contributors')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.subjects AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements_text((jsonb->>'subjects')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.classifications AS
SELECT
id AS id,
instance_id AS instance_id,
jsonb->>'classificationNumber' AS classification_number,
CAST(jsonb->>'classificationTypeId' AS UUID) AS classification_type_id
FROM (SELECT id::text || ordinality::text AS id, id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements((jsonb->>'classifications')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.publications AS
SELECT
id AS id,
instance_id AS instance_id,
jsonb->>'publisher' AS publisher,
jsonb->>'place' AS place,
jsonb->>'dateOfPublication' AS date_of_publication,
jsonb->>'role' AS role
FROM (SELECT id::text || ordinality::text AS id, id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements((jsonb->>'publication')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.publication_frequency AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements_text((jsonb->>'publicationFrequency')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.publication_range AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements_text((jsonb->>'publicationRange')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.electronic_accesses AS
SELECT
id AS id,
instance_id AS instance_id,
jsonb->>'uri' AS uri,
jsonb->>'linkText' AS link_text,
jsonb->>'materialsSpecification' AS materials_specification,
jsonb->>'publicNote' AS public_note,
CAST(jsonb->>'relationshipId' AS UUID) AS relationship_id
FROM (SELECT id::text || ordinality::text AS id, id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements((jsonb->>'electronicAccess')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.instance_formats AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb AS UUID) AS format_id
FROM (SELECT id::text || ordinality::text AS id, id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements_text((jsonb->>'instanceFormatIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.physical_descriptions AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements_text((jsonb->>'physicalDescriptions')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.languages AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements_text((jsonb->>'languages')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.notes AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb->>'instanceNoteTypeId' AS UUID) AS instance_note_type_id,
jsonb->>'note' AS note,
CAST(jsonb->>'staffOnly' AS BOOLEAN) AS staff_only
FROM (SELECT id::text || ordinality::text AS id, id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements((jsonb->>'notes')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.instance_statistical_codes AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb AS UUID) AS statistical_code_id
FROM (SELECT id::text || ordinality::text AS id, id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements_text((jsonb->>'statisticalCodeIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.instance_tags AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements_text((jsonb#>>'{tags,tagList}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.instance_nature_of_content_terms AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb AS UUID) AS nature_of_content_term_id
FROM (SELECT id::text || ordinality::text AS id, id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements_text((jsonb->>'natureOfContentTermIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.instances AS
SELECT
id AS id,
CAST(jsonb->>'_version' AS INTEGER) AS _version,
CAST(jsonb->>'hrid' AS INTEGER) AS hrid,
jsonb->>'matchKey' AS match_key,
jsonb->>'source' AS source,
jsonb->>'title' AS title,
jsonb#>>'{contributors,0,name}' AS author,
jsonb#>>'{publication,0,dateOfPublication}' AS publication_year,
jsonb->>'indexTitle' AS index_title,
CAST(jsonb#>>'{publicationPeriod,start}' AS INTEGER) AS publication_period_start,
CAST(jsonb#>>'{publicationPeriod,end}' AS INTEGER) AS publication_period_end,
CAST(jsonb->>'instanceTypeId' AS UUID) AS instance_type_id,
CAST(jsonb->>'modeOfIssuanceId' AS UUID) AS mode_of_issuance_id,
uc.TIMESTAMP_CAST(jsonb->>'catalogedDate') AS cataloged_date,
CAST(jsonb->>'previouslyHeld' AS BOOLEAN) AS previously_held,
CAST(jsonb->>'staffSuppress' AS BOOLEAN) AS staff_suppress,
CAST(jsonb->>'discoverySuppress' AS BOOLEAN) AS discovery_suppress,
jsonb->>'sourceRecordFormat' AS source_record_format,
CAST(jsonb->>'statusId' AS UUID) AS status_id,
uc.TIMESTAMP_CAST(jsonb->>'statusUpdatedDate') AS status_updated_date,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.instance;
CREATE VIEW uc.formats AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'code' AS code,
jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.instance_format;
CREATE VIEW uc.instance_note_types AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.instance_note_type;
CREATE VIEW uc.relationships AS
SELECT
id AS id,
CAST(jsonb->>'superInstanceId' AS UUID) AS super_instance_id,
CAST(jsonb->>'subInstanceId' AS UUID) AS sub_instance_id,
CAST(jsonb->>'instanceRelationshipTypeId' AS UUID) AS instance_relationship_type_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.instance_relationship;
CREATE VIEW uc.relationship_types AS
SELECT
id AS id,
jsonb->>'name' AS name,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.instance_relationship_type;
CREATE VIEW uc.source_marc_fields AS
SELECT
id AS id,
source_marc_id AS source_marc_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS source_marc_id, value AS jsonb FROM diku_mod_inventory_storage.instance_source_marc, jsonb_array_elements_text((jsonb->>'fields')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.source_marcs AS
SELECT
id AS id,
CAST(jsonb->>'leader' AS VARCHAR(24)) AS leader,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.instance_source_marc;
CREATE VIEW uc.statuses AS
SELECT
id AS id,
jsonb->>'code' AS code,
jsonb->>'name' AS name,
jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.instance_status;
CREATE VIEW uc.instance_types AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'code' AS code,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.instance_type;
CREATE VIEW uc.institutions AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'code' AS code,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.locinstitution;
CREATE VIEW uc.interface_type AS
SELECT
id AS id,
interface_id AS interface_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS interface_id, value AS jsonb FROM diku_mod_organizations_storage.interfaces, jsonb_array_elements_text((jsonb->>'type')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.interfaces AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'uri' AS uri,
jsonb->>'notes' AS notes,
CAST(jsonb->>'available' AS BOOLEAN) AS available,
jsonb->>'deliveryMethod' AS delivery_method,
jsonb->>'statisticsFormat' AS statistics_format,
jsonb->>'locallyStored' AS locally_stored,
jsonb->>'onlineLocation' AS online_location,
jsonb->>'statisticsNotes' AS statistics_notes,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_organizations_storage.interfaces;
CREATE VIEW uc.interface_credentials AS
SELECT
id AS id,
jsonb->>'username' AS username,
jsonb->>'password' AS password,
CAST(jsonb->>'interfaceId' AS UUID) AS interface_id,
jsonb_pretty(jsonb) AS content
FROM diku_mod_organizations_storage.interface_credentials;
CREATE VIEW uc.invoice_adjustment_fund_distributions AS
SELECT
id AS id,
invoice_adjustment_id AS invoice_adjustment_id,
jsonb->>'code' AS code,
CAST(jsonb->>'encumbrance' AS UUID) AS encumbrance_id,
CAST(jsonb->>'fundId' AS UUID) AS fund_id,
CAST(jsonb->>'invoiceLineId' AS UUID) AS invoice_item_id,
jsonb->>'distributionType' AS distribution_type,
CAST(jsonb->>'expenseClassId' AS UUID) AS expense_class_id,
CAST(jsonb->>'value' AS DECIMAL(19,2)) AS value
FROM (SELECT id::text || ordinality::text AS id, id AS invoice_adjustment_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS invoice_id, value AS jsonb FROM diku_mod_invoice_storage.invoices, jsonb_array_elements((jsonb->>'adjustments')::jsonb) WITH ORDINALITY) a, jsonb_array_elements((jsonb->>'fundDistributions')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.invoice_adjustments AS
SELECT
id AS id,
invoice_id AS invoice_id,
CAST(jsonb->>'id' AS UUID) AS id2,
CAST(jsonb->>'adjustmentId' AS UUID) AS adjustment_id,
jsonb->>'description' AS description,
CAST(jsonb->>'exportToAccounting' AS BOOLEAN) AS export_to_accounting,
jsonb->>'prorate' AS prorate,
jsonb->>'relationToTotal' AS relation_to_total,
jsonb->>'type' AS type,
CAST(jsonb->>'value' AS DECIMAL(19,2)) AS value
FROM (SELECT id::text || ordinality::text AS id, id AS invoice_id, value AS jsonb FROM diku_mod_invoice_storage.invoices, jsonb_array_elements((jsonb->>'adjustments')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.invoice_order_numbers AS
SELECT
id AS id,
invoice_id AS invoice_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS invoice_id, value AS jsonb FROM diku_mod_invoice_storage.invoices, jsonb_array_elements_text((jsonb->>'poNumbers')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.invoice_acquisitions_units AS
SELECT
id AS id,
invoice_id AS invoice_id,
CAST(jsonb AS UUID) AS acquisitions_unit_id
FROM (SELECT id::text || ordinality::text AS id, id AS invoice_id, value AS jsonb FROM diku_mod_invoice_storage.invoices, jsonb_array_elements_text((jsonb->>'acqUnitIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.invoice_tags AS
SELECT
id AS id,
invoice_id AS invoice_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS invoice_id, value AS jsonb FROM diku_mod_invoice_storage.invoices, jsonb_array_elements_text((jsonb#>>'{tags,tagList}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.invoices AS
SELECT
id AS id,
jsonb->>'accountingCode' AS accounting_code,
CAST(jsonb->>'adjustmentsTotal' AS DECIMAL(19,2)) AS adjustments_total,
CAST(jsonb->>'approvedBy' AS UUID) AS approved_by_id,
uc.TIMESTAMP_CAST(jsonb->>'approvalDate') AS approval_date,
CAST(jsonb->>'batchGroupId' AS UUID) AS batch_group_id,
CAST(jsonb->>'billTo' AS UUID) AS bill_to_id,
CAST(jsonb->>'chkSubscriptionOverlap' AS BOOLEAN) AS chk_subscription_overlap,
jsonb->>'cancellationNote' AS cancellation_note,
jsonb->>'currency' AS currency,
CAST(jsonb->>'enclosureNeeded' AS BOOLEAN) AS enclosure_needed,
CAST(jsonb->>'exchangeRate' AS DECIMAL(19,2)) AS exchange_rate,
CAST(jsonb->>'exportToAccounting' AS BOOLEAN) AS export_to_accounting,
jsonb->>'folioInvoiceNo' AS folio_invoice_no,
uc.TIMESTAMP_CAST(jsonb->>'invoiceDate') AS invoice_date,
CAST(jsonb->>'lockTotal' AS DECIMAL(19,2)) AS lock_total,
jsonb->>'note' AS note,
uc.TIMESTAMP_CAST(jsonb->>'paymentDue') AS payment_due,
uc.TIMESTAMP_CAST(jsonb->>'paymentDate') AS payment_date,
jsonb->>'paymentTerms' AS payment_terms,
jsonb->>'paymentMethod' AS payment_method,
jsonb->>'status' AS status,
jsonb->>'source' AS source,
CAST(jsonb->>'subTotal' AS DECIMAL(19,2)) AS sub_total,
CAST(jsonb->>'total' AS DECIMAL(19,2)) AS total,
jsonb->>'vendorInvoiceNo' AS vendor_invoice_no,
jsonb->>'disbursementNumber' AS disbursement_number,
jsonb->>'voucherNumber' AS voucher_number,
CAST(jsonb->>'paymentId' AS UUID) AS payment_id,
uc.TIMESTAMP_CAST(jsonb->>'disbursementDate') AS disbursement_date,
CAST(jsonb->>'vendorId' AS UUID) AS vendor_id,
jsonb->>'accountNo' AS account_no,
CAST(jsonb->>'manualPayment' AS BOOLEAN) AS manual_payment,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_invoice_storage.invoices;
CREATE VIEW uc.invoice_item_adjustment_fund_distributions AS
SELECT
id AS id,
invoice_item_adjustment_id AS invoice_item_adjustment_id,
jsonb->>'code' AS code,
CAST(jsonb->>'encumbrance' AS UUID) AS encumbrance_id,
CAST(jsonb->>'fundId' AS UUID) AS fund_id,
CAST(jsonb->>'invoiceLineId' AS UUID) AS invoice_item_id,
jsonb->>'distributionType' AS distribution_type,
CAST(jsonb->>'expenseClassId' AS UUID) AS expense_class_id,
CAST(jsonb->>'value' AS DECIMAL(19,2)) AS value
FROM (SELECT id::text || ordinality::text AS id, id AS invoice_item_adjustment_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS invoice_item_id, value AS jsonb FROM diku_mod_invoice_storage.invoice_lines, jsonb_array_elements((jsonb->>'adjustments')::jsonb) WITH ORDINALITY) a, jsonb_array_elements((jsonb->>'fundDistributions')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.invoice_item_adjustments AS
SELECT
id AS id,
invoice_item_id AS invoice_item_id,
CAST(jsonb->>'id' AS UUID) AS id2,
CAST(jsonb->>'adjustmentId' AS UUID) AS adjustment_id,
jsonb->>'description' AS description,
CAST(jsonb->>'exportToAccounting' AS BOOLEAN) AS export_to_accounting,
jsonb->>'prorate' AS prorate,
jsonb->>'relationToTotal' AS relation_to_total,
jsonb->>'type' AS type,
CAST(jsonb->>'value' AS DECIMAL(19,2)) AS value
FROM (SELECT id::text || ordinality::text AS id, id AS invoice_item_id, value AS jsonb FROM diku_mod_invoice_storage.invoice_lines, jsonb_array_elements((jsonb->>'adjustments')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.invoice_item_fund_distributions AS
SELECT
id AS id,
invoice_item_id AS invoice_item_id,
jsonb->>'code' AS code,
CAST(jsonb->>'encumbrance' AS UUID) AS encumbrance_id,
CAST(jsonb->>'fundId' AS UUID) AS fund_id,
jsonb->>'distributionType' AS distribution_type,
CAST(jsonb->>'expenseClassId' AS UUID) AS expense_class_id,
CAST(jsonb->>'value' AS DECIMAL(19,2)) AS value
FROM (SELECT id::text || ordinality::text AS id, id AS invoice_item_id, value AS jsonb FROM diku_mod_invoice_storage.invoice_lines, jsonb_array_elements((jsonb->>'fundDistributions')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.invoice_item_reference_numbers AS
SELECT
id AS id,
invoice_item_id AS invoice_item_id,
jsonb->>'refNumber' AS ref_number,
jsonb->>'refNumberType' AS ref_number_type,
jsonb->>'vendorDetailsSource' AS vendor_details_source
FROM (SELECT id::text || ordinality::text AS id, id AS invoice_item_id, value AS jsonb FROM diku_mod_invoice_storage.invoice_lines, jsonb_array_elements((jsonb->>'referenceNumbers')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.invoice_item_tags AS
SELECT
id AS id,
invoice_item_id AS invoice_item_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS invoice_item_id, value AS jsonb FROM diku_mod_invoice_storage.invoice_lines, jsonb_array_elements_text((jsonb#>>'{tags,tagList}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.invoice_items AS
SELECT
id AS id,
jsonb->>'accountingCode' AS accounting_code,
jsonb->>'accountNumber' AS account_number,
CAST(jsonb->>'adjustmentsTotal' AS DECIMAL(19,2)) AS adjustments_total,
jsonb->>'comment' AS comment,
jsonb->>'description' AS description,
CAST(jsonb->>'invoiceId' AS UUID) AS invoice_id,
jsonb->>'invoiceLineNumber' AS invoice_line_number,
jsonb->>'invoiceLineStatus' AS invoice_line_status,
CAST(jsonb->>'poLineId' AS UUID) AS po_line_id,
jsonb->>'productId' AS product_id,
CAST(jsonb->>'productIdType' AS UUID) AS product_id_type_id,
CAST(jsonb->>'quantity' AS INTEGER) AS quantity,
CAST(jsonb->>'releaseEncumbrance' AS BOOLEAN) AS release_encumbrance,
jsonb->>'subscriptionInfo' AS subscription_info,
uc.TIMESTAMP_CAST(jsonb->>'subscriptionStart') AS subscription_start,
uc.TIMESTAMP_CAST(jsonb->>'subscriptionEnd') AS subscription_end,
CAST(jsonb->>'subTotal' AS DECIMAL(19,2)) AS sub_total,
CAST(jsonb->>'total' AS DECIMAL(19,2)) AS total,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_invoice_storage.invoice_lines;
CREATE VIEW uc.invoice_transaction_summaries AS
SELECT
id AS id,
CAST(jsonb->>'numPendingPayments' AS INTEGER) AS num_pending_payments,
CAST(jsonb->>'numPaymentsCredits' AS INTEGER) AS num_payments_credits,
jsonb_pretty(jsonb) AS content
FROM diku_mod_finance_storage.invoice_transaction_summaries;
CREATE VIEW uc.item_former_ids AS
SELECT
id AS id,
item_id AS item_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS item_id, value AS jsonb FROM diku_mod_inventory_storage.item, jsonb_array_elements_text((jsonb->>'formerIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.item_year_caption AS
SELECT
id AS id,
item_id AS item_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS item_id, value AS jsonb FROM diku_mod_inventory_storage.item, jsonb_array_elements_text((jsonb->>'yearCaption')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.item_notes AS
SELECT
id AS id,
item_id AS item_id,
CAST(jsonb->>'itemNoteTypeId' AS UUID) AS item_note_type_id,
jsonb->>'note' AS note,
CAST(jsonb->>'staffOnly' AS BOOLEAN) AS staff_only
FROM (SELECT id::text || ordinality::text AS id, id AS item_id, value AS jsonb FROM diku_mod_inventory_storage.item, jsonb_array_elements((jsonb->>'notes')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.circulation_notes AS
SELECT
id AS id,
item_id AS item_id,
jsonb->>'id' AS id2,
jsonb->>'noteType' AS note_type,
jsonb->>'note' AS note,
jsonb#>>'{source,id}' AS source_id,
jsonb#>>'{source,personal,lastName}' AS source_personal_last_name,
jsonb#>>'{source,personal,firstName}' AS source_personal_first_name,
jsonb->>'date' AS date,
CAST(jsonb->>'staffOnly' AS BOOLEAN) AS staff_only
FROM (SELECT id::text || ordinality::text AS id, id AS item_id, value AS jsonb FROM diku_mod_inventory_storage.item, jsonb_array_elements((jsonb->>'circulationNotes')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.item_electronic_accesses AS
SELECT
id AS id,
item_id AS item_id,
jsonb->>'uri' AS uri,
jsonb->>'linkText' AS link_text,
jsonb->>'materialsSpecification' AS materials_specification,
jsonb->>'publicNote' AS public_note,
CAST(jsonb->>'relationshipId' AS UUID) AS relationship_id
FROM (SELECT id::text || ordinality::text AS id, id AS item_id, value AS jsonb FROM diku_mod_inventory_storage.item, jsonb_array_elements((jsonb->>'electronicAccess')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.item_statistical_codes AS
SELECT
id AS id,
item_id AS item_id,
CAST(jsonb AS UUID) AS statistical_code_id
FROM (SELECT id::text || ordinality::text AS id, id AS item_id, value AS jsonb FROM diku_mod_inventory_storage.item, jsonb_array_elements_text((jsonb->>'statisticalCodeIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.item_tags AS
SELECT
id AS id,
item_id AS item_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS item_id, value AS jsonb FROM diku_mod_inventory_storage.item, jsonb_array_elements_text((jsonb#>>'{tags,tagList}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.items AS
SELECT
id AS id,
CAST(jsonb->>'_version' AS INTEGER) AS _version,
CAST(jsonb->>'hrid' AS INTEGER) AS hrid,
CAST(jsonb->>'holdingsRecordId' AS UUID) AS holding_id,
CAST(jsonb->>'discoverySuppress' AS BOOLEAN) AS discovery_suppress,
jsonb->>'accessionNumber' AS accession_number,
jsonb->>'barcode' AS barcode,
jsonb->>'effectiveShelvingOrder' AS effective_shelving_order,
jsonb->>'itemLevelCallNumber' AS call_number,
jsonb->>'itemLevelCallNumberPrefix' AS call_number_prefix,
jsonb->>'itemLevelCallNumberSuffix' AS call_number_suffix,
CAST(jsonb->>'itemLevelCallNumberTypeId' AS UUID) AS call_number_type_id,
jsonb#>>'{effectiveCallNumberComponents,callNumber}' AS effective_call_number,
jsonb#>>'{effectiveCallNumberComponents,prefix}' AS effective_call_number_prefix,
jsonb#>>'{effectiveCallNumberComponents,suffix}' AS effective_call_number_suffix,
CAST(jsonb#>>'{effectiveCallNumberComponents,typeId}' AS UUID) AS effective_call_number_type_id,
jsonb->>'volume' AS volume,
jsonb->>'enumeration' AS enumeration,
jsonb->>'chronology' AS chronology,
jsonb->>'itemIdentifier' AS item_identifier,
jsonb->>'copyNumber' AS copy_number,
jsonb->>'numberOfPieces' AS number_of_pieces,
jsonb->>'descriptionOfPieces' AS description_of_pieces,
jsonb->>'numberOfMissingPieces' AS number_of_missing_pieces,
jsonb->>'missingPieces' AS missing_pieces,
uc.TIMESTAMP_CAST(jsonb->>'missingPiecesDate') AS missing_pieces_date,
CAST(jsonb->>'itemDamagedStatusId' AS UUID) AS item_damaged_status_id,
uc.TIMESTAMP_CAST(jsonb->>'itemDamagedStatusDate') AS item_damaged_status_date,
jsonb#>>'{status,name}' AS status_name,
uc.TIMESTAMP_CAST(jsonb#>>'{status,date}') AS status_date,
CAST(jsonb->>'materialTypeId' AS UUID) AS material_type_id,
CAST(jsonb->>'permanentLoanTypeId' AS UUID) AS permanent_loan_type_id,
CAST(jsonb->>'temporaryLoanTypeId' AS UUID) AS temporary_loan_type_id,
CAST(jsonb->>'permanentLocationId' AS UUID) AS permanent_location_id,
CAST(jsonb->>'temporaryLocationId' AS UUID) AS temporary_location_id,
CAST(jsonb->>'effectiveLocationId' AS UUID) AS effective_location_id,
CAST(jsonb->>'inTransitDestinationServicePointId' AS UUID) AS in_transit_destination_service_point_id,
CAST(jsonb->>'purchaseOrderLineIdentifier' AS UUID) AS order_item_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{lastCheckIn,dateTime}') AS last_check_in_date_time,
CAST(jsonb#>>'{lastCheckIn,servicePointId}' AS UUID) AS last_check_in_service_point_id,
CAST(jsonb#>>'{lastCheckIn,staffMemberId}' AS UUID) AS last_check_in_staff_member_id,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.item;
CREATE VIEW uc.item_damaged_statuses AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.item_damaged_status;
CREATE VIEW uc.item_note_types AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.item_note_type;
CREATE VIEW uc.job_executions AS
SELECT
id AS id,
jsonb->>'hrId' AS hr_id,
CAST(jsonb->>'parentJobId' AS UUID) AS parent_job_id,
jsonb->>'subordinationType' AS subordination_type,
jsonb#>>'{jobProfileInfo,name}' AS job_profile_info_name,
jsonb#>>'{jobProfileInfo,dataType}' AS job_profile_info_data_type,
CAST(jsonb#>>'{jobProfileSnapshotWrapper,profileId}' AS UUID) AS job_profile_snapshot_wrapper_profile_id,
jsonb#>>'{jobProfileSnapshotWrapper,contentType}' AS job_profile_snapshot_wrapper_content_type,
jsonb#>>'{jobProfileSnapshotWrapper,reactTo}' AS job_profile_snapshot_wrapper_react_to,
CAST(jsonb#>>'{jobProfileSnapshotWrapper,order}' AS INTEGER) AS job_profile_snapshot_wrapper_order,
jsonb->>'sourcePath' AS source_path,
jsonb->>'fileName' AS file_name,
jsonb#>>'{runBy,firstName}' AS run_by_first_name,
jsonb#>>'{runBy,lastName}' AS run_by_last_name,
CAST(jsonb#>>'{progress,jobExecutionId}' AS UUID) AS progress_job_execution_id,
CAST(jsonb#>>'{progress,current}' AS INTEGER) AS progress_current,
CAST(jsonb#>>'{progress,total}' AS INTEGER) AS progress_total,
uc.TIMESTAMP_CAST(jsonb->>'startedDate') AS started_date,
uc.TIMESTAMP_CAST(jsonb->>'completedDate') AS completed_date,
jsonb->>'status' AS status,
jsonb->>'uiStatus' AS ui_status,
jsonb->>'errorStatus' AS error_status,
CAST(jsonb->>'userId' AS UUID) AS user_id,
jsonb_pretty(jsonb) AS content
FROM diku_mod_source_record_manager.job_executions;
CREATE VIEW uc.job_execution_progresses AS
SELECT
id AS id,
CAST(jsonb->>'jobExecutionId' AS UUID) AS job_execution_id,
CAST(jsonb->>'currentlySucceeded' AS INTEGER) AS currently_succeeded,
CAST(jsonb->>'currentlyFailed' AS INTEGER) AS currently_failed,
CAST(jsonb->>'total' AS INTEGER) AS total,
jsonb_pretty(jsonb) AS content
FROM diku_mod_source_record_manager.job_execution_progress;
CREATE VIEW uc.job_execution_source_chunks AS
SELECT
id AS id,
CAST(jsonb->>'jobExecutionId' AS UUID) AS job_execution_id,
CAST(jsonb->>'last' AS BOOLEAN) AS last,
jsonb->>'state' AS state,
CAST(jsonb->>'chunkSize' AS INTEGER) AS chunk_size,
CAST(jsonb->>'processedAmount' AS INTEGER) AS processed_amount,
uc.TIMESTAMP_CAST(jsonb->>'completedDate') AS completed_date,
jsonb->>'error' AS error,
jsonb_pretty(jsonb) AS content
FROM diku_mod_source_record_manager.job_execution_source_chunks;
CREATE VIEW uc.job_monitorings AS
SELECT
id AS id,
job_execution_id AS job_execution_id,
last_event_timestamp AS last_event_timestamp,
notification_sent AS notification_sent
FROM diku_mod_source_record_manager.job_monitoring;
CREATE VIEW uc.journal_records AS
SELECT
id AS id,
job_execution_id AS job_execution_id,
source_id AS source_id,
entity_type AS entity_type,
entity_id AS entity_id,
entity_hrid AS entity_hrid,
action_type AS action_type,
action_status AS action_status,
action_date AS action_date,
source_record_order AS source_record_order,
error AS error,
title AS title
FROM diku_mod_source_record_manager.journal_records;
CREATE VIEW uc.ledger_acquisitions_units AS
SELECT
id AS id,
ledger_id AS ledger_id,
CAST(jsonb AS UUID) AS acquisitions_unit_id
FROM (SELECT id::text || ordinality::text AS id, id AS ledger_id, value AS jsonb FROM diku_mod_finance_storage.ledger, jsonb_array_elements_text((jsonb->>'acqUnitIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.ledgers AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'code' AS code,
jsonb->>'description' AS description,
CAST(jsonb->>'fiscalYearOneId' AS UUID) AS fiscal_year_one_id,
jsonb->>'ledgerStatus' AS ledger_status,
CAST(jsonb->>'allocated' AS DECIMAL(19,2)) AS allocated,
CAST(jsonb->>'available' AS DECIMAL(19,2)) AS available,
CAST(jsonb->>'netTransfers' AS DECIMAL(19,2)) AS net_transfers,
CAST(jsonb->>'unavailable' AS DECIMAL(19,2)) AS unavailable,
jsonb->>'currency' AS currency,
CAST(jsonb->>'restrictEncumbrance' AS BOOLEAN) AS restrict_encumbrance,
CAST(jsonb->>'restrictExpenditures' AS BOOLEAN) AS restrict_expenditures,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
CAST(jsonb->>'initialAllocation' AS DECIMAL(19,2)) AS initial_allocation,
CAST(jsonb->>'allocationTo' AS DECIMAL(19,2)) AS allocation_to,
CAST(jsonb->>'allocationFrom' AS DECIMAL(19,2)) AS allocation_from,
CAST(jsonb->>'totalFunding' AS DECIMAL(19,2)) AS total_funding,
CAST(jsonb->>'cashBalance' AS DECIMAL(19,2)) AS cash_balance,
CAST(jsonb->>'awaitingPayment' AS DECIMAL(19,2)) AS awaiting_payment,
CAST(jsonb->>'encumbered' AS DECIMAL(19,2)) AS encumbered,
CAST(jsonb->>'expenditures' AS DECIMAL(19,2)) AS expenditures,
CAST(jsonb->>'overEncumbrance' AS DECIMAL(19,2)) AS over_encumbrance,
CAST(jsonb->>'overExpended' AS DECIMAL(19,2)) AS over_expended,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_finance_storage.ledger;
CREATE VIEW uc.ledger_rollover_budgets_rollover AS
SELECT
id AS id,
ledger_rollover_id AS ledger_rollover_id,
CAST(jsonb->>'fundTypeId' AS UUID) AS fund_type_id,
CAST(jsonb->>'rolloverAllocation' AS BOOLEAN) AS rollover_allocation,
CAST(jsonb->>'rolloverAvailable' AS BOOLEAN) AS rollover_available,
CAST(jsonb->>'setAllowances' AS BOOLEAN) AS set_allowances,
CAST(jsonb->>'adjustAllocation' AS DECIMAL(19,2)) AS adjust_allocation,
jsonb->>'addAvailableTo' AS add_available_to,
CAST(jsonb->>'allowableEncumbrance' AS DECIMAL(19,2)) AS allowable_encumbrance,
CAST(jsonb->>'allowableExpenditure' AS DECIMAL(19,2)) AS allowable_expenditure
FROM (SELECT id::text || ordinality::text AS id, id AS ledger_rollover_id, value AS jsonb FROM diku_mod_finance_storage.ledger_fiscal_year_rollover, jsonb_array_elements((jsonb->>'budgetsRollover')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.ledger_rollover_encumbrances_rollover AS
SELECT
id AS id,
ledger_rollover_id AS ledger_rollover_id,
jsonb->>'orderType' AS order_type,
jsonb->>'basedOn' AS based_on,
CAST(jsonb->>'increaseBy' AS DECIMAL(19,2)) AS increase_by
FROM (SELECT id::text || ordinality::text AS id, id AS ledger_rollover_id, value AS jsonb FROM diku_mod_finance_storage.ledger_fiscal_year_rollover, jsonb_array_elements((jsonb->>'encumbrancesRollover')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.ledger_rollovers AS
SELECT
id AS id,
CAST(jsonb->>'ledgerId' AS UUID) AS ledger_id,
CAST(jsonb->>'fromFiscalYearId' AS UUID) AS from_fiscal_year_id,
CAST(jsonb->>'toFiscalYearId' AS UUID) AS to_fiscal_year_id,
CAST(jsonb->>'restrictEncumbrance' AS BOOLEAN) AS restrict_encumbrance,
CAST(jsonb->>'restrictExpenditures' AS BOOLEAN) AS restrict_expenditures,
CAST(jsonb->>'needCloseBudgets' AS BOOLEAN) AS need_close_budgets,
CAST(jsonb->>'currencyFactor' AS INTEGER) AS currency_factor,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_finance_storage.ledger_fiscal_year_rollover;
CREATE VIEW uc.ledger_rollover_errors AS
SELECT
id AS id,
CAST(jsonb->>'ledgerRolloverId' AS UUID) AS ledger_rollover_id,
jsonb->>'errorType' AS error_type,
jsonb->>'failedAction' AS failed_action,
jsonb->>'errorMessage' AS error_message,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_finance_storage.ledger_fiscal_year_rollover_error;
CREATE VIEW uc.ledger_rollover_progresses AS
SELECT
id AS id,
CAST(jsonb->>'ledgerRolloverId' AS UUID) AS ledger_rollover_id,
jsonb->>'overallRolloverStatus' AS overall_rollover_status,
jsonb->>'budgetsClosingRolloverStatus' AS budgets_closing_rollover_status,
jsonb->>'financialRolloverStatus' AS financial_rollover_status,
jsonb->>'ordersRolloverStatus' AS orders_rollover_status,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_finance_storage.ledger_fiscal_year_rollover_progress;
CREATE VIEW uc.libraries AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'code' AS code,
CAST(jsonb->>'campusId' AS UUID) AS campus_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.loclibrary;
CREATE VIEW uc.loans AS
SELECT
id AS id,
CAST(jsonb->>'userId' AS UUID) AS user_id,
CAST(jsonb->>'proxyUserId' AS UUID) AS proxy_user_id,
CAST(jsonb->>'itemId' AS UUID) AS item_id,
CAST(jsonb->>'itemEffectiveLocationIdAtCheckOut' AS UUID) AS item_effective_location_at_check_out_id,
jsonb#>>'{status,name}' AS status_name,
uc.TIMESTAMP_CAST(jsonb->>'loanDate') AS loan_date,
uc.TIMESTAMP_CAST(jsonb->>'dueDate') AS due_date,
jsonb->>'returnDate' AS return_date,
uc.TIMESTAMP_CAST(jsonb->>'systemReturnDate') AS system_return_date,
jsonb->>'action' AS action,
jsonb->>'actionComment' AS action_comment,
jsonb->>'itemStatus' AS item_status,
CAST(jsonb->>'renewalCount' AS INTEGER) AS renewal_count,
CAST(jsonb->>'loanPolicyId' AS UUID) AS loan_policy_id,
CAST(jsonb->>'checkoutServicePointId' AS UUID) AS checkout_service_point_id,
CAST(jsonb->>'checkinServicePointId' AS UUID) AS checkin_service_point_id,
CAST(jsonb->>'patronGroupIdAtCheckout' AS UUID) AS group_id,
CAST(jsonb->>'dueDateChangedByRecall' AS BOOLEAN) AS due_date_changed_by_recall,
uc.TIMESTAMP_CAST(jsonb->>'declaredLostDate') AS declared_lost_date,
uc.TIMESTAMP_CAST(jsonb->>'claimedReturnedDate') AS claimed_returned_date,
CAST(jsonb->>'overdueFinePolicyId' AS UUID) AS overdue_fine_policy_id,
CAST(jsonb->>'lostItemPolicyId' AS UUID) AS lost_item_policy_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
CAST(jsonb#>>'{agedToLostDelayedBilling,lostItemHasBeenBilled}' AS BOOLEAN) AS aged_to_lost_delayed_billing_lost_item_has_been_billed,
uc.TIMESTAMP_CAST(jsonb#>>'{agedToLostDelayedBilling,dateLostItemShouldBeBilled}') AS aged_to_lost_delayed_billing_date_lost_item_should_be_billed,
uc.TIMESTAMP_CAST(jsonb#>>'{agedToLostDelayedBilling,agedToLostDate}') AS aged_to_lost_delayed_billing_aged_to_lost_date,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_circulation_storage.loan;
CREATE VIEW uc.loan_events AS
SELECT
id AS id,
jsonb->>'operation' AS operation,
jsonb->>'creationDate' AS creation_date,
CAST(jsonb#>>'{loan,userId}' AS UUID) AS loan_user_id,
CAST(jsonb#>>'{loan,proxyUserId}' AS UUID) AS loan_proxy_user_id,
CAST(jsonb#>>'{loan,itemId}' AS UUID) AS loan_item_id,
CAST(jsonb#>>'{loan,itemEffectiveLocationIdAtCheckOut}' AS UUID) AS loan_item_effective_location_id_at_check_out_id,
jsonb#>>'{loan,status,name}' AS loan_status_name,
jsonb#>>'{loan,loanDate}' AS loan_loan_date,
uc.TIMESTAMP_CAST(jsonb#>>'{loan,dueDate}') AS loan_due_date,
jsonb#>>'{loan,returnDate}' AS loan_return_date,
uc.TIMESTAMP_CAST(jsonb#>>'{loan,systemReturnDate}') AS loan_system_return_date,
jsonb#>>'{loan,action}' AS loan_action,
jsonb#>>'{loan,actionComment}' AS loan_action_comment,
jsonb#>>'{loan,itemStatus}' AS loan_item_status,
CAST(jsonb#>>'{loan,renewalCount}' AS INTEGER) AS loan_renewal_count,
CAST(jsonb#>>'{loan,loanPolicyId}' AS UUID) AS loan_loan_policy_id,
CAST(jsonb#>>'{loan,checkoutServicePointId}' AS UUID) AS loan_checkout_service_point_id,
CAST(jsonb#>>'{loan,checkinServicePointId}' AS UUID) AS loan_checkin_service_point_id,
jsonb#>>'{loan,patronGroupIdAtCheckout}' AS loan_patron_group_id_at_checkout,
CAST(jsonb#>>'{loan,dueDateChangedByRecall}' AS BOOLEAN) AS loan_due_date_changed_by_recall,
uc.TIMESTAMP_CAST(jsonb#>>'{loan,declaredLostDate}') AS loan_declared_lost_date,
uc.TIMESTAMP_CAST(jsonb#>>'{loan,claimedReturnedDate}') AS loan_claimed_returned_date,
CAST(jsonb#>>'{loan,overdueFinePolicyId}' AS UUID) AS loan_overdue_fine_policy_id,
CAST(jsonb#>>'{loan,lostItemPolicyId}' AS UUID) AS loan_lost_item_policy_id,
uc.TIMESTAMP_CAST(jsonb#>>'{loan,metadata,createdDate}') AS loan_metadata_created_date,
CAST(jsonb#>>'{loan,metadata,createdByUserId}' AS UUID) AS loan_metadata_created_by_user_id,
jsonb#>>'{loan,metadata,createdByUsername}' AS loan_metadata_created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{loan,metadata,updatedDate}') AS loan_metadata_updated_date,
CAST(jsonb#>>'{loan,metadata,updatedByUserId}' AS UUID) AS loan_metadata_updated_by_user_id,
jsonb#>>'{loan,metadata,updatedByUsername}' AS loan_metadata_updated_by_username,
CAST(jsonb#>>'{loan,agedToLostDelayedBilling,lostItemHasBeenBilled}' AS BOOLEAN) AS loan_aged_to_lost_delayed_billing_lost_item_has_been_billed,
uc.TIMESTAMP_CAST(jsonb#>>'{loan,agedToLostDelayedBilling,dateLostItemShouldBeBilled}') AS loan_aged_to_lost_delayed_billing_date_lost_item_should_be_billed,
uc.TIMESTAMP_CAST(jsonb#>>'{loan,agedToLostDelayedBilling,agedToLostDate}') AS loan_aged_to_lost_delayed_billing_aged_to_lost_date,
jsonb_pretty(jsonb) AS content
FROM diku_mod_circulation_storage.audit_loan;
CREATE VIEW uc.loan_policies AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'description' AS description,
CAST(jsonb->>'loanable' AS BOOLEAN) AS loanable,
jsonb#>>'{loansPolicy,profileId}' AS loans_policy_profile_id,
CAST(jsonb#>>'{loansPolicy,period,duration}' AS INTEGER) AS loans_policy_period_duration,
jsonb#>>'{loansPolicy,period,intervalId}' AS loans_policy_period_interval_id,
jsonb#>>'{loansPolicy,closedLibraryDueDateManagementId}' AS loans_policy_closed_library_due_date_management_id,
CAST(jsonb#>>'{loansPolicy,gracePeriod,duration}' AS INTEGER) AS loans_policy_grace_period_duration,
jsonb#>>'{loansPolicy,gracePeriod,intervalId}' AS loans_policy_grace_period_interval_id,
CAST(jsonb#>>'{loansPolicy,openingTimeOffset,duration}' AS INTEGER) AS loans_policy_opening_time_offset_duration,
jsonb#>>'{loansPolicy,openingTimeOffset,intervalId}' AS loans_policy_opening_time_offset_interval_id,
CAST(jsonb#>>'{loansPolicy,fixedDueDateScheduleId}' AS UUID) AS loans_policy_fixed_due_date_schedule_id,
CAST(jsonb#>>'{loansPolicy,itemLimit}' AS INTEGER) AS loans_policy_item_limit,
CAST(jsonb->>'renewable' AS BOOLEAN) AS renewable,
CAST(jsonb#>>'{renewalsPolicy,unlimited}' AS BOOLEAN) AS renewals_policy_unlimited,
CAST(jsonb#>>'{renewalsPolicy,numberAllowed}' AS DECIMAL(19,2)) AS renewals_policy_number_allowed,
jsonb#>>'{renewalsPolicy,renewFromId}' AS renewals_policy_renew_from_id,
CAST(jsonb#>>'{renewalsPolicy,differentPeriod}' AS BOOLEAN) AS renewals_policy_different_period,
CAST(jsonb#>>'{renewalsPolicy,period,duration}' AS INTEGER) AS renewals_policy_period_duration,
jsonb#>>'{renewalsPolicy,period,intervalId}' AS renewals_policy_period_interval_id,
CAST(jsonb#>>'{renewalsPolicy,alternateFixedDueDateScheduleId}' AS UUID) AS renewals_policy_alternate_fixed_due_date_schedule_id,
CAST(jsonb#>>'{requestManagement,recalls,alternateGracePeriod,duration}' AS INTEGER) AS recalls_alternate_grace_period_duration,
jsonb#>>'{requestManagement,recalls,alternateGracePeriod,intervalId}' AS recalls_alternate_grace_period_interval_id,
CAST(jsonb#>>'{requestManagement,recalls,minimumGuaranteedLoanPeriod,duration}' AS INTEGER) AS recalls_minimum_guaranteed_loan_period_duration,
jsonb#>>'{requestManagement,recalls,minimumGuaranteedLoanPeriod,intervalId}' AS recalls_minimum_guaranteed_loan_period_interval_id,
CAST(jsonb#>>'{requestManagement,recalls,recallReturnInterval,duration}' AS INTEGER) AS recalls_recall_return_interval_duration,
jsonb#>>'{requestManagement,recalls,recallReturnInterval,intervalId}' AS recalls_recall_return_interval_interval_id,
CAST(jsonb#>>'{requestManagement,recalls,allowRecallsToExtendOverdueLoans}' AS BOOLEAN) AS recalls_allow_recalls_to_extend_overdue_loans,
CAST(jsonb#>>'{requestManagement,recalls,alternateRecallReturnInterval,duration}' AS INTEGER) AS recalls_alternate_recall_return_interval_duration,
jsonb#>>'{requestManagement,recalls,alternateRecallReturnInterval,intervalId}' AS recalls_alternate_recall_return_interval_interval_id,
CAST(jsonb#>>'{requestManagement,holds,alternateCheckoutLoanPeriod,duration}' AS INTEGER) AS holds_alternate_checkout_loan_period_duration,
jsonb#>>'{requestManagement,holds,alternateCheckoutLoanPeriod,intervalId}' AS holds_alternate_checkout_loan_period_interval_id,
CAST(jsonb#>>'{requestManagement,holds,renewItemsWithRequest}' AS BOOLEAN) AS holds_renew_items_with_request,
CAST(jsonb#>>'{requestManagement,holds,alternateRenewalLoanPeriod,duration}' AS INTEGER) AS holds_alternate_renewal_loan_period_duration,
jsonb#>>'{requestManagement,holds,alternateRenewalLoanPeriod,intervalId}' AS holds_alternate_renewal_loan_period_interval_id,
CAST(jsonb#>>'{requestManagement,pages,alternateCheckoutLoanPeriod,duration}' AS INTEGER) AS pages_alternate_checkout_loan_period_duration,
jsonb#>>'{requestManagement,pages,alternateCheckoutLoanPeriod,intervalId}' AS pages_alternate_checkout_loan_period_interval_id,
CAST(jsonb#>>'{requestManagement,pages,renewItemsWithRequest}' AS BOOLEAN) AS pages_renew_items_with_request,
CAST(jsonb#>>'{requestManagement,pages,alternateRenewalLoanPeriod,duration}' AS INTEGER) AS pages_alternate_renewal_loan_period_duration,
jsonb#>>'{requestManagement,pages,alternateRenewalLoanPeriod,intervalId}' AS pages_alternate_renewal_loan_period_interval_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_circulation_storage.loan_policy;
CREATE VIEW uc.loan_types AS
SELECT
id AS id,
jsonb->>'name' AS name,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.loan_type;
CREATE VIEW uc.location_service_points AS
SELECT
id AS id,
location_id AS location_id,
CAST(jsonb AS UUID) AS service_point_id
FROM (SELECT id::text || ordinality::text AS id, id AS location_id, value AS jsonb FROM diku_mod_inventory_storage.location, jsonb_array_elements_text((jsonb->>'servicePointIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.locations AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'code' AS code,
jsonb->>'description' AS description,
jsonb->>'discoveryDisplayName' AS discovery_display_name,
CAST(jsonb->>'isActive' AS BOOLEAN) AS is_active,
CAST(jsonb->>'institutionId' AS UUID) AS institution_id,
CAST(jsonb->>'campusId' AS UUID) AS campus_id,
CAST(jsonb->>'libraryId' AS UUID) AS library_id,
CAST(jsonb->>'primaryServicePoint' AS UUID) AS primary_service_point_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.location;
CREATE VIEW uc.logins AS
SELECT
id AS id,
CAST(jsonb->>'userId' AS UUID) AS user_id,
jsonb->>'hash' AS hash,
jsonb->>'salt' AS salt,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_login.auth_credentials;
CREATE VIEW uc.lost_item_fee_policies AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'description' AS description,
CAST(jsonb#>>'{itemAgedLostOverdue,duration}' AS INTEGER) AS item_aged_lost_overdue_duration,
jsonb#>>'{itemAgedLostOverdue,intervalId}' AS item_aged_lost_overdue_interval_id,
CAST(jsonb#>>'{patronBilledAfterAgedLost,duration}' AS INTEGER) AS patron_billed_after_aged_lost_duration,
jsonb#>>'{patronBilledAfterAgedLost,intervalId}' AS patron_billed_after_aged_lost_interval_id,
CAST(jsonb#>>'{recalledItemAgedLostOverdue,duration}' AS INTEGER) AS recalled_item_aged_lost_overdue_duration,
jsonb#>>'{recalledItemAgedLostOverdue,intervalId}' AS recalled_item_aged_lost_overdue_interval_id,
CAST(jsonb#>>'{patronBilledAfterRecalledItemAgedLost,duration}' AS INTEGER) AS patron_billed_after_recalled_item_aged_lost_duration,
jsonb#>>'{patronBilledAfterRecalledItemAgedLost,intervalId}' AS patron_billed_after_recalled_item_aged_lost_interval_id,
jsonb#>>'{chargeAmountItem,chargeType}' AS charge_amount_item_charge_type,
CAST(jsonb#>>'{chargeAmountItem,amount}' AS DECIMAL(19,2)) AS charge_amount_item_amount,
CAST(jsonb->>'lostItemProcessingFee' AS DECIMAL(19,2)) AS lost_item_processing_fee,
CAST(jsonb->>'chargeAmountItemPatron' AS BOOLEAN) AS charge_amount_item_patron,
CAST(jsonb->>'chargeAmountItemSystem' AS BOOLEAN) AS charge_amount_item_system,
CAST(jsonb#>>'{lostItemChargeFeeFine,duration}' AS INTEGER) AS lost_item_charge_fee_fine_duration,
jsonb#>>'{lostItemChargeFeeFine,intervalId}' AS lost_item_charge_fee_fine_interval_id,
CAST(jsonb->>'returnedLostItemProcessingFee' AS BOOLEAN) AS returned_lost_item_processing_fee,
CAST(jsonb->>'replacedLostItemProcessingFee' AS BOOLEAN) AS replaced_lost_item_processing_fee,
CAST(jsonb->>'replacementProcessingFee' AS DECIMAL(19,2)) AS replacement_processing_fee,
CAST(jsonb->>'replacementAllowed' AS BOOLEAN) AS replacement_allowed,
jsonb->>'lostItemReturned' AS lost_item_returned,
CAST(jsonb#>>'{feesFinesShallRefunded,duration}' AS INTEGER) AS fees_fines_shall_refunded_duration,
jsonb#>>'{feesFinesShallRefunded,intervalId}' AS fees_fines_shall_refunded_interval_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_feesfines.lost_item_fee_policy;
CREATE VIEW uc.manual_block_templates AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'code' AS code,
jsonb->>'desc' AS desc,
jsonb#>>'{blockTemplate,desc}' AS block_template_desc,
jsonb#>>'{blockTemplate,patronMessage}' AS block_template_patron_message,
CAST(jsonb#>>'{blockTemplate,borrowing}' AS BOOLEAN) AS block_template_borrowing,
CAST(jsonb#>>'{blockTemplate,renewals}' AS BOOLEAN) AS block_template_renewals,
CAST(jsonb#>>'{blockTemplate,requests}' AS BOOLEAN) AS block_template_requests,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_feesfines.manual_block_templates;
CREATE VIEW uc.marc_records AS
SELECT
id AS id,
jsonb_pretty(content) AS content
FROM diku_mod_source_record_storage.marc_records_lb;
CREATE VIEW uc.material_types AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.material_type;
CREATE VIEW uc.mode_of_issuances AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.mode_of_issuance;
CREATE VIEW uc.nature_of_content_terms AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.nature_of_content_term;
CREATE VIEW uc.note_links AS
SELECT
id AS id,
note_id AS note_id,
jsonb->>'id' AS id2,
jsonb->>'type' AS type
FROM (SELECT id::text || ordinality::text AS id, id AS note_id, value AS jsonb FROM diku_mod_notes.note_data, jsonb_array_elements((jsonb->>'links')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.notes2 AS
SELECT
id AS id,
CAST(jsonb->>'typeId' AS UUID) AS type_id,
jsonb->>'type' AS type,
jsonb->>'domain' AS domain,
CAST(jsonb->>'title' AS VARCHAR(255)) AS title,
jsonb->>'content' AS content2,
jsonb->>'status' AS status,
jsonb#>>'{creator,lastName}' AS creator_last_name,
jsonb#>>'{creator,firstName}' AS creator_first_name,
jsonb#>>'{creator,middleName}' AS creator_middle_name,
jsonb#>>'{updater,lastName}' AS updater_last_name,
jsonb#>>'{updater,firstName}' AS updater_first_name,
jsonb#>>'{updater,middleName}' AS updater_middle_name,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content,
temporary_type_id AS temporary_type_id,
search_content AS search_content
FROM diku_mod_notes.note_data;
CREATE VIEW uc.note_types AS
SELECT
id AS id,
jsonb->>'name' AS name,
CAST(jsonb#>>'{usage,noteTotal}' AS INTEGER) AS usage_note_total,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_notes.note_type;
CREATE VIEW uc.order_notes AS
SELECT
id AS id,
order_id AS order_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS order_id, value AS jsonb FROM diku_mod_orders_storage.purchase_order, jsonb_array_elements_text((jsonb->>'notes')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.order_acquisitions_units AS
SELECT
id AS id,
order_id AS order_id,
CAST(jsonb AS UUID) AS acquisitions_unit_id
FROM (SELECT id::text || ordinality::text AS id, id AS order_id, value AS jsonb FROM diku_mod_orders_storage.purchase_order, jsonb_array_elements_text((jsonb->>'acqUnitIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.order_tags AS
SELECT
id AS id,
order_id AS order_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS order_id, value AS jsonb FROM diku_mod_orders_storage.purchase_order, jsonb_array_elements_text((jsonb#>>'{tags,tagList}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.orders AS
SELECT
id AS id,
CAST(jsonb->>'approved' AS BOOLEAN) AS approved,
CAST(jsonb->>'approvedById' AS UUID) AS approved_by_id,
uc.TIMESTAMP_CAST(jsonb->>'approvalDate') AS approval_date,
CAST(jsonb->>'assignedTo' AS UUID) AS assigned_to_id,
CAST(jsonb->>'billTo' AS UUID) AS bill_to_id,
jsonb#>>'{closeReason,reason}' AS close_reason_reason,
jsonb#>>'{closeReason,note}' AS close_reason_note,
uc.TIMESTAMP_CAST(jsonb->>'dateOrdered') AS date_ordered,
CAST(jsonb->>'manualPo' AS BOOLEAN) AS manual_po,
jsonb->>'poNumber' AS po_number,
jsonb->>'poNumberPrefix' AS po_number_prefix,
jsonb->>'poNumberSuffix' AS po_number_suffix,
jsonb->>'orderType' AS order_type,
CAST(jsonb->>'reEncumber' AS BOOLEAN) AS re_encumber,
CAST(jsonb#>>'{ongoing,interval}' AS INTEGER) AS ongoing_interval,
CAST(jsonb#>>'{ongoing,isSubscription}' AS BOOLEAN) AS ongoing_is_subscription,
CAST(jsonb#>>'{ongoing,manualRenewal}' AS BOOLEAN) AS ongoing_manual_renewal,
jsonb#>>'{ongoing,notes}' AS ongoing_notes,
CAST(jsonb#>>'{ongoing,reviewPeriod}' AS INTEGER) AS ongoing_review_period,
uc.TIMESTAMP_CAST(jsonb#>>'{ongoing,renewalDate}') AS ongoing_renewal_date,
uc.TIMESTAMP_CAST(jsonb#>>'{ongoing,reviewDate}') AS ongoing_review_date,
CAST(jsonb->>'shipTo' AS UUID) AS ship_to_id,
CAST(jsonb->>'template' AS UUID) AS template_id,
CAST(jsonb->>'vendor' AS UUID) AS vendor_id,
jsonb->>'workflowStatus' AS status,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_orders_storage.purchase_order;
CREATE VIEW uc.order_invoices AS
SELECT
id AS id,
CAST(jsonb->>'purchaseOrderId' AS UUID) AS order_id,
CAST(jsonb->>'invoiceId' AS UUID) AS invoice_id,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.order_invoice_relationship;
CREATE VIEW uc.order_item_alerts AS
SELECT
id AS id,
order_item_id AS order_item_id,
CAST(jsonb AS UUID) AS alert_id
FROM (SELECT id::text || ordinality::text AS id, id AS order_item_id, value AS jsonb FROM diku_mod_orders_storage.po_line, jsonb_array_elements_text((jsonb->>'alerts')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.order_item_claims AS
SELECT
id AS id,
order_item_id AS order_item_id,
CAST(jsonb->>'claimed' AS BOOLEAN) AS claimed,
uc.TIMESTAMP_CAST(jsonb->>'sent') AS sent,
CAST(jsonb->>'grace' AS INTEGER) AS grace
FROM (SELECT id::text || ordinality::text AS id, id AS order_item_id, value AS jsonb FROM diku_mod_orders_storage.po_line, jsonb_array_elements((jsonb->>'claims')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.order_item_contributors AS
SELECT
id AS id,
order_item_id AS order_item_id,
jsonb->>'contributor' AS contributor,
CAST(jsonb->>'contributorNameTypeId' AS UUID) AS contributor_name_type_id
FROM (SELECT id::text || ordinality::text AS id, id AS order_item_id, value AS jsonb FROM diku_mod_orders_storage.po_line, jsonb_array_elements((jsonb->>'contributors')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.order_item_product_ids AS
SELECT
id AS id,
order_item_id AS order_item_id,
jsonb->>'productId' AS product_id,
CAST(jsonb->>'productIdType' AS UUID) AS product_id_type_id,
jsonb->>'qualifier' AS qualifier
FROM (SELECT id::text || ordinality::text AS id, id AS order_item_id, value AS jsonb FROM diku_mod_orders_storage.po_line, jsonb_array_elements((jsonb#>>'{details,productIds}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.order_item_fund_distributions AS
SELECT
id AS id,
order_item_id AS order_item_id,
jsonb->>'code' AS code,
CAST(jsonb->>'encumbrance' AS UUID) AS encumbrance_id,
CAST(jsonb->>'fundId' AS UUID) AS fund_id,
CAST(jsonb->>'expenseClassId' AS UUID) AS expense_class_id,
jsonb->>'distributionType' AS distribution_type,
CAST(jsonb->>'value' AS DECIMAL(19,2)) AS value
FROM (SELECT id::text || ordinality::text AS id, id AS order_item_id, value AS jsonb FROM diku_mod_orders_storage.po_line, jsonb_array_elements((jsonb->>'fundDistribution')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.order_item_locations AS
SELECT
id AS id,
order_item_id AS order_item_id,
CAST(jsonb->>'locationId' AS UUID) AS location_id,
CAST(jsonb->>'holdingId' AS UUID) AS holding_id,
CAST(jsonb->>'quantity' AS INTEGER) AS quantity,
CAST(jsonb->>'quantityElectronic' AS INTEGER) AS quantity_electronic,
CAST(jsonb->>'quantityPhysical' AS INTEGER) AS quantity_physical
FROM (SELECT id::text || ordinality::text AS id, id AS order_item_id, value AS jsonb FROM diku_mod_orders_storage.po_line, jsonb_array_elements((jsonb->>'locations')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.order_item_volumes AS
SELECT
id AS id,
order_item_id AS order_item_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS order_item_id, value AS jsonb FROM diku_mod_orders_storage.po_line, jsonb_array_elements_text((jsonb#>>'{physical,volumes}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.order_item_reporting_codes AS
SELECT
id AS id,
order_item_id AS order_item_id,
CAST(jsonb AS UUID) AS reporting_code_id
FROM (SELECT id::text || ordinality::text AS id, id AS order_item_id, value AS jsonb FROM diku_mod_orders_storage.po_line, jsonb_array_elements_text((jsonb->>'reportingCodes')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.order_item_tags AS
SELECT
id AS id,
order_item_id AS order_item_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS order_item_id, value AS jsonb FROM diku_mod_orders_storage.po_line, jsonb_array_elements_text((jsonb#>>'{tags,tagList}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.order_item_reference_numbers AS
SELECT
id AS id,
order_item_id AS order_item_id,
jsonb->>'refNumber' AS ref_number,
jsonb->>'refNumberType' AS ref_number_type,
jsonb->>'vendorDetailsSource' AS vendor_details_source
FROM (SELECT id::text || ordinality::text AS id, id AS order_item_id, value AS jsonb FROM diku_mod_orders_storage.po_line, jsonb_array_elements((jsonb#>>'{vendorDetail,referenceNumbers}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.order_items AS
SELECT
id AS id,
jsonb->>'edition' AS edition,
CAST(jsonb->>'checkinItems' AS BOOLEAN) AS checkin_items,
CAST(jsonb->>'agreementId' AS UUID) AS agreement_id,
jsonb->>'acquisitionMethod' AS acquisition_method,
CAST(jsonb->>'cancellationRestriction' AS BOOLEAN) AS cancellation_restriction,
jsonb->>'cancellationRestrictionNote' AS cancellation_restriction_note,
CAST(jsonb->>'collection' AS BOOLEAN) AS collection,
CAST(jsonb#>>'{cost,listUnitPrice}' AS DECIMAL(19,2)) AS cost_list_unit_price,
CAST(jsonb#>>'{cost,listUnitPriceElectronic}' AS DECIMAL(19,2)) AS cost_list_unit_price_electronic,
jsonb#>>'{cost,currency}' AS cost_currency,
CAST(jsonb#>>'{cost,additionalCost}' AS DECIMAL(19,2)) AS cost_additional_cost,
CAST(jsonb#>>'{cost,discount}' AS DECIMAL(19,2)) AS cost_discount,
jsonb#>>'{cost,discountType}' AS cost_discount_type,
CAST(jsonb#>>'{cost,exchangeRate}' AS DECIMAL(19,2)) AS cost_exchange_rate,
CAST(jsonb#>>'{cost,quantityPhysical}' AS INTEGER) AS cost_quantity_physical,
CAST(jsonb#>>'{cost,quantityElectronic}' AS INTEGER) AS cost_quantity_electronic,
CAST(jsonb#>>'{cost,poLineEstimatedPrice}' AS DECIMAL(19,2)) AS cost_po_line_estimated_price,
CAST(jsonb#>>'{cost,fyroAdjustmentAmount}' AS DECIMAL(19,2)) AS cost_fyro_adjustment_amount,
jsonb->>'description' AS description,
jsonb#>>'{details,receivingNote}' AS details_receiving_note,
uc.TIMESTAMP_CAST(jsonb#>>'{details,subscriptionFrom}') AS details_subscription_from,
CAST(jsonb#>>'{details,subscriptionInterval}' AS INTEGER) AS details_subscription_interval,
uc.TIMESTAMP_CAST(jsonb#>>'{details,subscriptionTo}') AS details_subscription_to,
jsonb->>'donor' AS donor,
CAST(jsonb#>>'{eresource,activated}' AS BOOLEAN) AS eresource_activated,
CAST(jsonb#>>'{eresource,activationDue}' AS INTEGER) AS eresource_activation_due,
jsonb#>>'{eresource,createInventory}' AS eresource_create_inventory,
CAST(jsonb#>>'{eresource,trial}' AS BOOLEAN) AS eresource_trial,
uc.TIMESTAMP_CAST(jsonb#>>'{eresource,expectedActivation}') AS eresource_expected_activation,
CAST(jsonb#>>'{eresource,userLimit}' AS INTEGER) AS eresource_user_limit,
CAST(jsonb#>>'{eresource,accessProvider}' AS UUID) AS eresource_access_provider_id,
jsonb#>>'{eresource,license,code}' AS eresource_license_code,
jsonb#>>'{eresource,license,description}' AS eresource_license_description,
jsonb#>>'{eresource,license,reference}' AS eresource_license_reference,
CAST(jsonb#>>'{eresource,materialType}' AS UUID) AS eresource_material_type_id,
jsonb#>>'{eresource,resourceUrl}' AS eresource_resource_url,
CAST(jsonb->>'instanceId' AS UUID) AS instance_id,
CAST(jsonb->>'isPackage' AS BOOLEAN) AS is_package,
jsonb->>'orderFormat' AS order_format,
CAST(jsonb->>'packagePoLineId' AS UUID) AS package_po_line_id,
jsonb->>'paymentStatus' AS payment_status,
jsonb#>>'{physical,createInventory}' AS physical_create_inventory,
CAST(jsonb#>>'{physical,materialType}' AS UUID) AS physical_material_type_id,
CAST(jsonb#>>'{physical,materialSupplier}' AS UUID) AS physical_material_supplier_id,
uc.TIMESTAMP_CAST(jsonb#>>'{physical,expectedReceiptDate}') AS physical_expected_receipt_date,
uc.TIMESTAMP_CAST(jsonb#>>'{physical,receiptDue}') AS physical_receipt_due,
jsonb->>'poLineDescription' AS po_line_description,
jsonb->>'poLineNumber' AS po_line_number,
jsonb->>'publicationDate' AS publication_year,
jsonb->>'publisher' AS publisher,
CAST(jsonb->>'purchaseOrderId' AS UUID) AS order_id,
uc.TIMESTAMP_CAST(jsonb->>'receiptDate') AS receipt_date,
jsonb->>'receiptStatus' AS receipt_status,
jsonb->>'requester' AS requester,
CAST(jsonb->>'rush' AS BOOLEAN) AS rush,
jsonb->>'selector' AS selector,
jsonb->>'source' AS source,
jsonb->>'titleOrPackage' AS title_or_package,
jsonb#>>'{vendorDetail,instructions}' AS vendor_detail_instructions,
jsonb#>>'{vendorDetail,noteFromVendor}' AS vendor_detail_note_from_vendor,
jsonb#>>'{vendorDetail,vendorAccount}' AS vendor_detail_vendor_account,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_orders_storage.po_line;
CREATE VIEW uc.order_templates AS
SELECT
id AS id,
jsonb->>'templateName' AS template_name,
jsonb->>'templateCode' AS template_code,
jsonb->>'templateDescription' AS template_description,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.order_templates;
CREATE VIEW uc.order_transaction_summaries AS
SELECT
id AS id,
CAST(jsonb->>'numTransactions' AS INTEGER) AS num_transactions,
jsonb_pretty(jsonb) AS content
FROM diku_mod_finance_storage.order_transaction_summaries;
CREATE VIEW uc.organization_aliases AS
SELECT
id AS id,
organization_id AS organization_id,
jsonb->>'value' AS value,
jsonb->>'description' AS description
FROM (SELECT id::text || ordinality::text AS id, id AS organization_id, value AS jsonb FROM diku_mod_organizations_storage.organizations, jsonb_array_elements((jsonb->>'aliases')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.organization_address_categories AS
SELECT
id AS id,
organization_address_id AS organization_address_id,
CAST(jsonb AS UUID) AS category_id
FROM (SELECT id::text || ordinality::text AS id, id AS organization_address_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS organization_id, value AS jsonb FROM diku_mod_organizations_storage.organizations, jsonb_array_elements((jsonb->>'addresses')::jsonb) WITH ORDINALITY) a, jsonb_array_elements_text((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.organization_addresses AS
SELECT
id AS id,
organization_id AS organization_id,
CAST(jsonb->>'id' AS UUID) AS id2,
jsonb->>'addressLine1' AS address_line1,
jsonb->>'addressLine2' AS address_line2,
jsonb->>'city' AS city,
jsonb->>'stateRegion' AS state_region,
jsonb->>'zipCode' AS zip_code,
jsonb->>'country' AS country,
CAST(jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
jsonb->>'language' AS language,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username
FROM (SELECT id::text || ordinality::text AS id, id AS organization_id, value AS jsonb FROM diku_mod_organizations_storage.organizations, jsonb_array_elements((jsonb->>'addresses')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.organization_phone_number_categories AS
SELECT
id AS id,
organization_phone_number_id AS organization_phone_number_id,
CAST(jsonb AS UUID) AS category_id
FROM (SELECT id::text || ordinality::text AS id, id AS organization_phone_number_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS organization_id, value AS jsonb FROM diku_mod_organizations_storage.organizations, jsonb_array_elements((jsonb->>'phoneNumbers')::jsonb) WITH ORDINALITY) a, jsonb_array_elements_text((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.organization_phone_numbers AS
SELECT
id AS id,
organization_id AS organization_id,
CAST(jsonb->>'id' AS UUID) AS id2,
jsonb->>'phoneNumber' AS phone_number,
jsonb->>'type' AS type,
CAST(jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
jsonb->>'language' AS language,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username
FROM (SELECT id::text || ordinality::text AS id, id AS organization_id, value AS jsonb FROM diku_mod_organizations_storage.organizations, jsonb_array_elements((jsonb->>'phoneNumbers')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.organization_email_categories AS
SELECT
id AS id,
organization_email_id AS organization_email_id,
CAST(jsonb AS UUID) AS category_id
FROM (SELECT id::text || ordinality::text AS id, id AS organization_email_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS organization_id, value AS jsonb FROM diku_mod_organizations_storage.organizations, jsonb_array_elements((jsonb->>'emails')::jsonb) WITH ORDINALITY) a, jsonb_array_elements_text((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.organization_emails AS
SELECT
id AS id,
organization_id AS organization_id,
CAST(jsonb->>'id' AS UUID) AS id2,
jsonb->>'value' AS value,
jsonb->>'description' AS description,
CAST(jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
jsonb->>'language' AS language,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username
FROM (SELECT id::text || ordinality::text AS id, id AS organization_id, value AS jsonb FROM diku_mod_organizations_storage.organizations, jsonb_array_elements((jsonb->>'emails')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.organization_url_categories AS
SELECT
id AS id,
organization_url_id AS organization_url_id,
CAST(jsonb AS UUID) AS category_id
FROM (SELECT id::text || ordinality::text AS id, id AS organization_url_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS organization_id, value AS jsonb FROM diku_mod_organizations_storage.organizations, jsonb_array_elements((jsonb->>'urls')::jsonb) WITH ORDINALITY) a, jsonb_array_elements_text((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.organization_urls AS
SELECT
id AS id,
organization_id AS organization_id,
CAST(jsonb->>'id' AS UUID) AS id2,
jsonb->>'value' AS value,
jsonb->>'description' AS description,
jsonb->>'language' AS language,
CAST(jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
jsonb->>'notes' AS notes,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username
FROM (SELECT id::text || ordinality::text AS id, id AS organization_id, value AS jsonb FROM diku_mod_organizations_storage.organizations, jsonb_array_elements((jsonb->>'urls')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.organization_contacts AS
SELECT
id AS id,
organization_id AS organization_id,
CAST(jsonb AS UUID) AS contact_id
FROM (SELECT id::text || ordinality::text AS id, id AS organization_id, value AS jsonb FROM diku_mod_organizations_storage.organizations, jsonb_array_elements_text((jsonb->>'contacts')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.organization_agreements AS
SELECT
id AS id,
organization_id AS organization_id,
jsonb->>'name' AS name,
CAST(jsonb->>'discount' AS DECIMAL(19,2)) AS discount,
jsonb->>'referenceUrl' AS reference_url,
jsonb->>'notes' AS notes
FROM (SELECT id::text || ordinality::text AS id, id AS organization_id, value AS jsonb FROM diku_mod_organizations_storage.organizations, jsonb_array_elements((jsonb->>'agreements')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.currencies AS
SELECT
id AS id,
organization_id AS organization_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS organization_id, value AS jsonb FROM diku_mod_organizations_storage.organizations, jsonb_array_elements_text((jsonb->>'vendorCurrencies')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.organization_interfaces AS
SELECT
id AS id,
organization_id AS organization_id,
CAST(jsonb AS UUID) AS interface_id
FROM (SELECT id::text || ordinality::text AS id, id AS organization_id, value AS jsonb FROM diku_mod_organizations_storage.organizations, jsonb_array_elements_text((jsonb->>'interfaces')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.organization_account_acquisitions_units AS
SELECT
id AS id,
organization_account_id AS organization_account_id,
CAST(jsonb AS UUID) AS acquisitions_unit_id
FROM (SELECT id::text || ordinality::text AS id, id AS organization_account_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS organization_id, value AS jsonb FROM diku_mod_organizations_storage.organizations, jsonb_array_elements((jsonb->>'accounts')::jsonb) WITH ORDINALITY) a, jsonb_array_elements_text((jsonb->>'acqUnitIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.organization_accounts AS
SELECT
id AS id,
organization_id AS organization_id,
jsonb->>'name' AS name,
jsonb->>'accountNo' AS account_no,
jsonb->>'description' AS description,
jsonb->>'appSystemNo' AS app_system_no,
jsonb->>'paymentMethod' AS payment_method,
jsonb->>'accountStatus' AS account_status,
jsonb->>'contactInfo' AS contact_info,
jsonb->>'libraryCode' AS library_code,
jsonb->>'libraryEdiCode' AS library_edi_code,
jsonb->>'notes' AS notes
FROM (SELECT id::text || ordinality::text AS id, id AS organization_id, value AS jsonb FROM diku_mod_organizations_storage.organizations, jsonb_array_elements((jsonb->>'accounts')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.organization_changelogs AS
SELECT
id AS id,
organization_id AS organization_id,
jsonb->>'description' AS description,
uc.TIMESTAMP_CAST(jsonb->>'timestamp') AS timestamp
FROM (SELECT id::text || ordinality::text AS id, id AS organization_id, value AS jsonb FROM diku_mod_organizations_storage.organizations, jsonb_array_elements((jsonb->>'changelogs')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.organization_acquisitions_units AS
SELECT
id AS id,
organization_id AS organization_id,
CAST(jsonb AS UUID) AS acquisitions_unit_id
FROM (SELECT id::text || ordinality::text AS id, id AS organization_id, value AS jsonb FROM diku_mod_organizations_storage.organizations, jsonb_array_elements_text((jsonb->>'acqUnitIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.organization_tags AS
SELECT
id AS id,
organization_id AS organization_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS organization_id, value AS jsonb FROM diku_mod_organizations_storage.organizations, jsonb_array_elements_text((jsonb#>>'{tags,tagList}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.organizations AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'code' AS code,
jsonb->>'description' AS description,
CAST(jsonb->>'exportToAccounting' AS BOOLEAN) AS export_to_accounting,
jsonb->>'status' AS status,
jsonb->>'language' AS language,
jsonb->>'erpCode' AS erp_code,
jsonb->>'paymentMethod' AS payment_method,
CAST(jsonb->>'accessProvider' AS BOOLEAN) AS access_provider,
CAST(jsonb->>'governmental' AS BOOLEAN) AS governmental,
CAST(jsonb->>'licensor' AS BOOLEAN) AS licensor,
CAST(jsonb->>'materialSupplier' AS BOOLEAN) AS material_supplier,
CAST(jsonb->>'claimingInterval' AS INTEGER) AS claiming_interval,
CAST(jsonb->>'discountPercent' AS DECIMAL(19,2)) AS discount_percent,
CAST(jsonb->>'expectedActivationInterval' AS INTEGER) AS expected_activation_interval,
CAST(jsonb->>'expectedInvoiceInterval' AS INTEGER) AS expected_invoice_interval,
CAST(jsonb->>'renewalActivationInterval' AS INTEGER) AS renewal_activation_interval,
CAST(jsonb->>'subscriptionInterval' AS INTEGER) AS subscription_interval,
CAST(jsonb->>'expectedReceiptInterval' AS INTEGER) AS expected_receipt_interval,
jsonb->>'taxId' AS tax_id,
CAST(jsonb->>'liableForVat' AS BOOLEAN) AS liable_for_vat,
CAST(jsonb->>'taxPercentage' AS DECIMAL(19,2)) AS tax_percentage,
jsonb#>>'{edi,vendorEdiCode}' AS edi_vendor_edi_code,
jsonb#>>'{edi,vendorEdiType}' AS edi_vendor_edi_type,
jsonb#>>'{edi,libEdiCode}' AS edi_lib_edi_code,
jsonb#>>'{edi,libEdiType}' AS edi_lib_edi_type,
CAST(jsonb#>>'{edi,prorateTax}' AS BOOLEAN) AS edi_prorate_tax,
CAST(jsonb#>>'{edi,prorateFees}' AS BOOLEAN) AS edi_prorate_fees,
jsonb#>>'{edi,ediNamingConvention}' AS edi_naming_convention,
CAST(jsonb#>>'{edi,sendAcctNum}' AS BOOLEAN) AS edi_send_acct_num,
CAST(jsonb#>>'{edi,supportOrder}' AS BOOLEAN) AS edi_support_order,
CAST(jsonb#>>'{edi,supportInvoice}' AS BOOLEAN) AS edi_support_invoice,
jsonb#>>'{edi,notes}' AS edi_notes,
jsonb#>>'{edi,ediFtp,ftpFormat}' AS edi_ftp_ftp_format,
jsonb#>>'{edi,ediFtp,serverAddress}' AS edi_ftp_server_address,
jsonb#>>'{edi,ediFtp,username}' AS edi_ftp_username,
jsonb#>>'{edi,ediFtp,password}' AS edi_ftp_password,
jsonb#>>'{edi,ediFtp,ftpMode}' AS edi_ftp_ftp_mode,
jsonb#>>'{edi,ediFtp,ftpConnMode}' AS edi_ftp_ftp_conn_mode,
CAST(jsonb#>>'{edi,ediFtp,ftpPort}' AS INTEGER) AS edi_ftp_ftp_port,
jsonb#>>'{edi,ediFtp,orderDirectory}' AS edi_ftp_order_directory,
jsonb#>>'{edi,ediFtp,invoiceDirectory}' AS edi_ftp_invoice_directory,
jsonb#>>'{edi,ediFtp,notes}' AS edi_ftp_notes,
CAST(jsonb#>>'{edi,ediJob,scheduleEdi}' AS BOOLEAN) AS edi_job_schedule_edi,
uc.TIMESTAMP_CAST(jsonb#>>'{edi,ediJob,schedulingDate}') AS edi_job_scheduling_date,
jsonb#>>'{edi,ediJob,time}' AS edi_job_time,
CAST(jsonb#>>'{edi,ediJob,isMonday}' AS BOOLEAN) AS edi_job_is_monday,
CAST(jsonb#>>'{edi,ediJob,isTuesday}' AS BOOLEAN) AS edi_job_is_tuesday,
CAST(jsonb#>>'{edi,ediJob,isWednesday}' AS BOOLEAN) AS edi_job_is_wednesday,
CAST(jsonb#>>'{edi,ediJob,isThursday}' AS BOOLEAN) AS edi_job_is_thursday,
CAST(jsonb#>>'{edi,ediJob,isFriday}' AS BOOLEAN) AS edi_job_is_friday,
CAST(jsonb#>>'{edi,ediJob,isSaturday}' AS BOOLEAN) AS edi_job_is_saturday,
CAST(jsonb#>>'{edi,ediJob,isSunday}' AS BOOLEAN) AS edi_job_is_sunday,
jsonb#>>'{edi,ediJob,sendToEmails}' AS edi_job_send_to_emails,
CAST(jsonb#>>'{edi,ediJob,notifyAllEdi}' AS BOOLEAN) AS edi_job_notify_all_edi,
CAST(jsonb#>>'{edi,ediJob,notifyInvoiceOnly}' AS BOOLEAN) AS edi_job_notify_invoice_only,
CAST(jsonb#>>'{edi,ediJob,notifyErrorOnly}' AS BOOLEAN) AS edi_job_notify_error_only,
jsonb#>>'{edi,ediJob,schedulingNotes}' AS edi_job_scheduling_notes,
CAST(jsonb->>'isVendor' AS BOOLEAN) AS is_vendor,
jsonb->>'sanCode' AS san_code,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_organizations_storage.organizations;
CREATE VIEW uc.overdue_fine_policies AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'description' AS description,
CAST(jsonb#>>'{overdueFine,quantity}' AS DECIMAL(19,2)) AS overdue_fine_quantity,
jsonb#>>'{overdueFine,intervalId}' AS overdue_fine_interval_id,
CAST(jsonb->>'countClosed' AS BOOLEAN) AS count_closed,
CAST(jsonb->>'maxOverdueFine' AS DECIMAL(19,2)) AS max_overdue_fine,
CAST(jsonb->>'forgiveOverdueFine' AS BOOLEAN) AS forgive_overdue_fine,
CAST(jsonb#>>'{overdueRecallFine,quantity}' AS DECIMAL(19,2)) AS overdue_recall_fine_quantity,
jsonb#>>'{overdueRecallFine,intervalId}' AS overdue_recall_fine_interval_id,
CAST(jsonb->>'gracePeriodRecall' AS BOOLEAN) AS grace_period_recall,
CAST(jsonb->>'maxOverdueRecallFine' AS DECIMAL(19,2)) AS max_overdue_recall_fine,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_feesfines.overdue_fine_policy;
CREATE VIEW uc.service_point_owners AS
SELECT
id AS id,
owner_id AS owner_id,
CAST(jsonb->>'value' AS UUID) AS service_point_id,
jsonb->>'label' AS label
FROM (SELECT id::text || ordinality::text AS id, id AS owner_id, value AS jsonb FROM diku_mod_feesfines.owners, jsonb_array_elements((jsonb->>'servicePointOwner')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.owners AS
SELECT
id AS id,
jsonb->>'owner' AS owner,
jsonb->>'desc' AS desc,
CAST(jsonb->>'defaultChargeNoticeId' AS UUID) AS default_charge_notice_id,
CAST(jsonb->>'defaultActionNoticeId' AS UUID) AS default_action_notice_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_feesfines.owners;
CREATE VIEW uc.patron_action_sessions AS
SELECT
id AS id,
CAST(jsonb->>'patronId' AS UUID) AS patron_id,
CAST(jsonb->>'loanId' AS UUID) AS loan_id,
jsonb->>'actionType' AS action_type,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_circulation_storage.patron_action_session;
CREATE VIEW uc.patron_notice_policy_loan_notices AS
SELECT
id AS id,
patron_notice_policy_id AS patron_notice_policy_id,
jsonb->>'name' AS name,
CAST(jsonb->>'templateId' AS UUID) AS template_id,
jsonb->>'templateName' AS template_name,
jsonb->>'format' AS format,
jsonb->>'frequency' AS frequency,
CAST(jsonb->>'realTime' AS BOOLEAN) AS real_time,
jsonb#>>'{sendOptions,sendHow}' AS send_options_send_how,
jsonb#>>'{sendOptions,sendWhen}' AS send_options_send_when,
CAST(jsonb#>>'{sendOptions,sendBy,duration}' AS INTEGER) AS send_options_send_by_duration,
jsonb#>>'{sendOptions,sendBy,intervalId}' AS send_options_send_by_interval_id,
CAST(jsonb#>>'{sendOptions,sendEvery,duration}' AS INTEGER) AS send_options_send_every_duration,
jsonb#>>'{sendOptions,sendEvery,intervalId}' AS send_options_send_every_interval_id
FROM (SELECT id::text || ordinality::text AS id, id AS patron_notice_policy_id, value AS jsonb FROM diku_mod_circulation_storage.patron_notice_policy, jsonb_array_elements((jsonb->>'loanNotices')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.patron_notice_policy_fee_fine_notices AS
SELECT
id AS id,
patron_notice_policy_id AS patron_notice_policy_id,
jsonb->>'name' AS name,
CAST(jsonb->>'templateId' AS UUID) AS template_id,
jsonb->>'templateName' AS template_name,
jsonb->>'format' AS format,
jsonb->>'frequency' AS frequency,
CAST(jsonb->>'realTime' AS BOOLEAN) AS real_time,
jsonb#>>'{sendOptions,sendHow}' AS send_options_send_how,
jsonb#>>'{sendOptions,sendWhen}' AS send_options_send_when,
CAST(jsonb#>>'{sendOptions,sendBy,duration}' AS INTEGER) AS send_options_send_by_duration,
jsonb#>>'{sendOptions,sendBy,intervalId}' AS send_options_send_by_interval_id,
CAST(jsonb#>>'{sendOptions,sendEvery,duration}' AS INTEGER) AS send_options_send_every_duration,
jsonb#>>'{sendOptions,sendEvery,intervalId}' AS send_options_send_every_interval_id
FROM (SELECT id::text || ordinality::text AS id, id AS patron_notice_policy_id, value AS jsonb FROM diku_mod_circulation_storage.patron_notice_policy, jsonb_array_elements((jsonb->>'feeFineNotices')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.patron_notice_policy_request_notices AS
SELECT
id AS id,
patron_notice_policy_id AS patron_notice_policy_id,
jsonb->>'name' AS name,
CAST(jsonb->>'templateId' AS UUID) AS template_id,
jsonb->>'templateName' AS template_name,
jsonb->>'format' AS format,
jsonb->>'frequency' AS frequency,
CAST(jsonb->>'realTime' AS BOOLEAN) AS real_time,
jsonb#>>'{sendOptions,sendHow}' AS send_options_send_how,
jsonb#>>'{sendOptions,sendWhen}' AS send_options_send_when,
CAST(jsonb#>>'{sendOptions,sendBy,duration}' AS INTEGER) AS send_options_send_by_duration,
jsonb#>>'{sendOptions,sendBy,intervalId}' AS send_options_send_by_interval_id,
CAST(jsonb#>>'{sendOptions,sendEvery,duration}' AS INTEGER) AS send_options_send_every_duration,
jsonb#>>'{sendOptions,sendEvery,intervalId}' AS send_options_send_every_interval_id
FROM (SELECT id::text || ordinality::text AS id, id AS patron_notice_policy_id, value AS jsonb FROM diku_mod_circulation_storage.patron_notice_policy, jsonb_array_elements((jsonb->>'requestNotices')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.patron_notice_policies AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'description' AS description,
CAST(jsonb->>'active' AS BOOLEAN) AS active,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_circulation_storage.patron_notice_policy;
CREATE VIEW uc.payments AS
SELECT
id AS id,
uc.TIMESTAMP_CAST(jsonb->>'dateAction') AS date_action,
jsonb->>'typeAction' AS type_action,
jsonb->>'comments' AS comments,
CAST(jsonb->>'notify' AS BOOLEAN) AS notify,
CAST(jsonb->>'amountAction' AS DECIMAL(19,2)) AS amount_action,
CAST(jsonb->>'balance' AS DECIMAL(19,2)) AS balance,
jsonb->>'transactionInformation' AS transaction_information,
CAST(jsonb->>'createdAt' AS UUID) AS service_point_id,
jsonb->>'source' AS source,
jsonb->>'paymentMethod' AS payment_method,
CAST(jsonb->>'accountId' AS UUID) AS fee_id,
CAST(jsonb->>'userId' AS UUID) AS user_id,
jsonb_pretty(jsonb) AS content
FROM diku_mod_feesfines.feefineactions;
CREATE VIEW uc.payment_methods AS
SELECT
id AS id,
jsonb->>'nameMethod' AS name,
CAST(jsonb->>'allowedRefundMethod' AS BOOLEAN) AS allowed_refund_method,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
CAST(jsonb->>'ownerId' AS UUID) AS owner_id,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_feesfines.payments;
CREATE VIEW uc.permission_tags AS
SELECT
id AS id,
permission_id AS permission_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS permission_id, value AS jsonb FROM diku_mod_permissions.permissions, jsonb_array_elements_text((jsonb->>'tags')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.permission_sub_permissions AS
SELECT
id AS id,
permission_id AS permission_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS permission_id, value AS jsonb FROM diku_mod_permissions.permissions, jsonb_array_elements_text((jsonb->>'subPermissions')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.permission_child_of AS
SELECT
id AS id,
permission_id AS permission_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS permission_id, value AS jsonb FROM diku_mod_permissions.permissions, jsonb_array_elements_text((jsonb->>'childOf')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.permission_granted_to AS
SELECT
id AS id,
permission_id AS permission_id,
CAST(jsonb AS UUID) AS permissions_user_id
FROM (SELECT id::text || ordinality::text AS id, id AS permission_id, value AS jsonb FROM diku_mod_permissions.permissions, jsonb_array_elements_text((jsonb->>'grantedTo')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.permissions AS
SELECT
id AS id,
jsonb->>'permissionName' AS permission_name,
jsonb->>'displayName' AS display_name,
jsonb->>'description' AS description,
CAST(jsonb->>'mutable' AS BOOLEAN) AS mutable,
CAST(jsonb->>'visible' AS BOOLEAN) AS visible,
CAST(jsonb->>'dummy' AS BOOLEAN) AS dummy,
CAST(jsonb->>'deprecated' AS BOOLEAN) AS deprecated,
jsonb->>'moduleName' AS module_name,
jsonb->>'moduleVersion' AS module_version,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_permissions.permissions;
CREATE VIEW uc.permissions_user_permissions AS
SELECT
id AS id,
permissions_user_id AS permissions_user_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS permissions_user_id, value AS jsonb FROM diku_mod_permissions.permissions_users, jsonb_array_elements_text((jsonb->>'permissions')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.permissions_users AS
SELECT
id AS id,
CAST(jsonb->>'userId' AS UUID) AS user_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_permissions.permissions_users;
CREATE VIEW uc.preceding_succeeding_title_identifiers AS
SELECT
id AS id,
preceding_succeeding_title_id AS preceding_succeeding_title_id,
jsonb->>'value' AS value,
CAST(jsonb->>'identifierTypeId' AS UUID) AS identifier_type_id
FROM (SELECT id::text || ordinality::text AS id, id AS preceding_succeeding_title_id, value AS jsonb FROM diku_mod_inventory_storage.preceding_succeeding_title, jsonb_array_elements((jsonb->>'identifiers')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.preceding_succeeding_titles AS
SELECT
id AS id,
CAST(jsonb->>'precedingInstanceId' AS UUID) AS preceding_instance_id,
CAST(jsonb->>'succeedingInstanceId' AS UUID) AS succeeding_instance_id,
jsonb->>'title' AS title,
jsonb->>'hrid' AS hrid,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.preceding_succeeding_title;
CREATE VIEW uc.prefixes AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'description' AS description,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.prefixes;
CREATE VIEW uc.proxies AS
SELECT
id AS id,
CAST(jsonb->>'userId' AS UUID) AS user_id,
CAST(jsonb->>'proxyUserId' AS UUID) AS proxy_user_id,
jsonb->>'requestForSponsor' AS request_for_sponsor,
jsonb->>'notificationsTo' AS notifications_to,
jsonb->>'accrueTo' AS accrue_to,
jsonb->>'status' AS status,
uc.TIMESTAMP_CAST(jsonb->>'expirationDate') AS expiration_date,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_users.proxyfor;
CREATE VIEW uc.raw_records AS
SELECT
id AS id,
content AS content
FROM diku_mod_source_record_storage.raw_records_lb;
CREATE VIEW uc.receivings AS
SELECT
id AS id,
jsonb->>'caption' AS caption,
jsonb->>'comment' AS comment,
jsonb->>'format' AS format,
CAST(jsonb->>'itemId' AS UUID) AS item_id,
CAST(jsonb->>'locationId' AS UUID) AS location_id,
CAST(jsonb->>'poLineId' AS UUID) AS po_line_id,
CAST(jsonb->>'titleId' AS UUID) AS title_id,
CAST(jsonb->>'holdingId' AS UUID) AS holding_id,
CAST(jsonb->>'displayOnHolding' AS BOOLEAN) AS display_on_holding,
jsonb->>'enumeration' AS enumeration,
jsonb->>'chronology' AS chronology,
CAST(jsonb->>'discoverySuppress' AS BOOLEAN) AS discovery_suppress,
jsonb->>'receivingStatus' AS receiving_status,
CAST(jsonb->>'supplement' AS BOOLEAN) AS supplement,
uc.TIMESTAMP_CAST(jsonb->>'receiptDate') AS receipt_date,
uc.TIMESTAMP_CAST(jsonb->>'receivedDate') AS received_date,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.pieces;
CREATE VIEW uc.records AS
SELECT
id AS id,
snapshot_id AS snapshot_id,
matched_id AS matched_id,
generation AS generation,
record_type AS record_type,
instance_id AS instance_id,
state AS state,
leader_record_status AS leader_record_status,
"order" AS "order",
suppress_discovery AS suppress_discovery,
created_by_user_id AS creation_user_id,
created_date AS creation_time,
updated_by_user_id AS last_write_user_id,
updated_date AS last_write_time,
instance_hrid AS instance_hrid
FROM diku_mod_source_record_storage.records_lb;
CREATE VIEW uc.refund_reasons AS
SELECT
id AS id,
jsonb->>'nameReason' AS name,
jsonb->>'description' AS description,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
CAST(jsonb->>'accountId' AS UUID) AS account_id,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_feesfines.refunds;
CREATE VIEW uc.reporting_codes AS
SELECT
id AS id,
jsonb->>'code' AS code,
jsonb->>'description' AS description,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.reporting_code;
CREATE VIEW uc.request_identifiers AS
SELECT
id AS id,
request_id AS request_id,
jsonb->>'value' AS value,
CAST(jsonb->>'identifierTypeId' AS UUID) AS identifier_type_id
FROM (SELECT id::text || ordinality::text AS id, id AS request_id, value AS jsonb FROM diku_mod_circulation_storage.request, jsonb_array_elements((jsonb#>>'{item,identifiers}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.request_tags AS
SELECT
id AS id,
request_id AS request_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS request_id, value AS jsonb FROM diku_mod_circulation_storage.request, jsonb_array_elements_text((jsonb#>>'{tags,tagList}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.requests AS
SELECT
id AS id,
jsonb->>'requestType' AS request_type,
uc.TIMESTAMP_CAST(jsonb->>'requestDate') AS request_date,
jsonb->>'patronComments' AS patron_comments,
CAST(jsonb->>'requesterId' AS UUID) AS requester_id,
CAST(jsonb->>'proxyUserId' AS UUID) AS proxy_user_id,
CAST(jsonb->>'itemId' AS UUID) AS item_id,
jsonb->>'status' AS status,
CAST(jsonb->>'cancellationReasonId' AS UUID) AS cancellation_reason_id,
CAST(jsonb->>'cancelledByUserId' AS UUID) AS cancelled_by_user_id,
jsonb->>'cancellationAdditionalInformation' AS cancellation_additional_information,
uc.TIMESTAMP_CAST(jsonb->>'cancelledDate') AS cancelled_date,
CAST(jsonb->>'position' AS INTEGER) AS position,
jsonb#>>'{item,title}' AS item_title,
jsonb#>>'{item,barcode}' AS item_barcode,
jsonb#>>'{requester,firstName}' AS requester_first_name,
jsonb#>>'{requester,lastName}' AS requester_last_name,
jsonb#>>'{requester,middleName}' AS requester_middle_name,
jsonb#>>'{requester,barcode}' AS requester_barcode,
jsonb#>>'{requester,patronGroup}' AS requester_patron_group,
jsonb#>>'{proxy,firstName}' AS proxy_first_name,
jsonb#>>'{proxy,lastName}' AS proxy_last_name,
jsonb#>>'{proxy,middleName}' AS proxy_middle_name,
jsonb#>>'{proxy,barcode}' AS proxy_barcode,
jsonb#>>'{proxy,patronGroup}' AS proxy_patron_group,
jsonb->>'fulfilmentPreference' AS fulfilment_preference,
CAST(jsonb->>'deliveryAddressTypeId' AS UUID) AS delivery_address_type_id,
uc.TIMESTAMP_CAST(jsonb->>'requestExpirationDate') AS request_expiration_date,
uc.TIMESTAMP_CAST(jsonb->>'holdShelfExpirationDate') AS hold_shelf_expiration_date,
CAST(jsonb->>'pickupServicePointId' AS UUID) AS pickup_service_point_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
uc.TIMESTAMP_CAST(jsonb->>'awaitingPickupRequestClosedDate') AS awaiting_pickup_request_closed_date,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_circulation_storage.request;
CREATE VIEW uc.request_policy_request_types AS
SELECT
id AS id,
request_policy_id AS request_policy_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS request_policy_id, value AS jsonb FROM diku_mod_circulation_storage.request_policy, jsonb_array_elements_text((jsonb->>'requestTypes')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.request_policies AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'description' AS description,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_circulation_storage.request_policy;
CREATE VIEW uc.scheduled_notices AS
SELECT
id AS id,
CAST(jsonb->>'loanId' AS UUID) AS loan_id,
CAST(jsonb->>'requestId' AS UUID) AS request_id,
CAST(jsonb->>'feeFineActionId' AS UUID) AS payment_id,
CAST(jsonb->>'recipientUserId' AS UUID) AS recipient_user_id,
uc.TIMESTAMP_CAST(jsonb->>'nextRunTime') AS next_run_time,
jsonb->>'triggeringEvent' AS triggering_event,
jsonb#>>'{noticeConfig,timing}' AS notice_config_timing,
CAST(jsonb#>>'{noticeConfig,recurringPeriod,duration}' AS INTEGER) AS notice_config_recurring_period_duration,
jsonb#>>'{noticeConfig,recurringPeriod,intervalId}' AS notice_config_recurring_period_interval_id,
CAST(jsonb#>>'{noticeConfig,templateId}' AS UUID) AS notice_config_template_id,
jsonb#>>'{noticeConfig,format}' AS notice_config_format,
CAST(jsonb#>>'{noticeConfig,sendInRealTime}' AS BOOLEAN) AS notice_config_send_in_real_time,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_circulation_storage.scheduled_notice;
CREATE VIEW uc.service_point_staff_slips AS
SELECT
id AS id,
service_point_id AS service_point_id,
CAST(jsonb->>'id' AS UUID) AS staff_slip_id,
CAST(jsonb->>'printByDefault' AS BOOLEAN) AS print_by_default
FROM (SELECT id::text || ordinality::text AS id, id AS service_point_id, value AS jsonb FROM diku_mod_inventory_storage.service_point, jsonb_array_elements((jsonb->>'staffSlips')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.service_points AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'code' AS code,
jsonb->>'discoveryDisplayName' AS discovery_display_name,
jsonb->>'description' AS description,
CAST(jsonb->>'shelvingLagTime' AS INTEGER) AS shelving_lag_time,
CAST(jsonb->>'pickupLocation' AS BOOLEAN) AS pickup_location,
CAST(jsonb#>>'{holdShelfExpiryPeriod,duration}' AS INTEGER) AS hold_shelf_expiry_period_duration,
jsonb#>>'{holdShelfExpiryPeriod,intervalId}' AS hold_shelf_expiry_period_interval_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.service_point;
CREATE VIEW uc.service_point_user_service_points AS
SELECT
id AS id,
service_point_user_id AS service_point_user_id,
CAST(jsonb AS UUID) AS service_point_id
FROM (SELECT id::text || ordinality::text AS id, id AS service_point_user_id, value AS jsonb FROM diku_mod_inventory_storage.service_point_user, jsonb_array_elements_text((jsonb->>'servicePointsIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.service_point_users AS
SELECT
id AS id,
CAST(jsonb->>'userId' AS UUID) AS user_id,
CAST(jsonb->>'defaultServicePointId' AS UUID) AS default_service_point_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.service_point_user;
CREATE VIEW uc.snapshots AS
SELECT
id AS id,
status AS status,
processing_started_date AS processing_started_date,
created_by_user_id AS creation_user_id,
created_date AS creation_time,
updated_by_user_id AS last_write_user_id,
updated_date AS last_write_time
FROM diku_mod_source_record_storage.snapshots_lb;
CREATE VIEW uc.sources AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.holdings_records_source;
CREATE VIEW uc.staff_slips AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'description' AS description,
CAST(jsonb->>'active' AS BOOLEAN) AS active,
jsonb->>'template' AS template,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_circulation_storage.staff_slips;
CREATE VIEW uc.statistical_codes AS
SELECT
id AS id,
jsonb->>'code' AS code,
jsonb->>'name' AS name,
CAST(jsonb->>'statisticalCodeTypeId' AS UUID) AS statistical_code_type_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.statistical_code;
CREATE VIEW uc.statistical_code_types AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'source' AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_inventory_storage.statistical_code_type;
CREATE VIEW uc.suffixes AS
SELECT
id AS id,
jsonb->>'name' AS name,
jsonb->>'description' AS description,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.suffixes;
CREATE VIEW uc.tags AS
SELECT
id AS id,
jsonb->>'label' AS label,
jsonb->>'description' AS description,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_tags.tags;
CREATE VIEW uc.template_output_formats AS
SELECT
id AS id,
template_id AS template_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS template_id, value AS jsonb FROM diku_mod_template_engine.template, jsonb_array_elements_text((jsonb->>'outputFormats')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.templates AS
SELECT
id AS id,
jsonb->>'name' AS name,
CAST(jsonb->>'active' AS BOOLEAN) AS active,
jsonb->>'category' AS category,
jsonb->>'description' AS description,
jsonb->>'templateResolver' AS template_resolver,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_template_engine.template;
CREATE VIEW uc.title_product_ids AS
SELECT
id AS id,
title_id AS title_id,
jsonb->>'productId' AS product_id,
CAST(jsonb->>'productIdType' AS UUID) AS product_id_type_id,
jsonb->>'qualifier' AS qualifier
FROM (SELECT id::text || ordinality::text AS id, id AS title_id, value AS jsonb FROM diku_mod_orders_storage.titles, jsonb_array_elements((jsonb->>'productIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.title_contributors AS
SELECT
id AS id,
title_id AS title_id,
jsonb->>'contributor' AS contributor,
CAST(jsonb->>'contributorNameTypeId' AS UUID) AS contributor_name_type_id
FROM (SELECT id::text || ordinality::text AS id, id AS title_id, value AS jsonb FROM diku_mod_orders_storage.titles, jsonb_array_elements((jsonb->>'contributors')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.titles AS
SELECT
id AS id,
uc.TIMESTAMP_CAST(jsonb->>'expectedReceiptDate') AS expected_receipt_date,
jsonb->>'title' AS title,
CAST(jsonb->>'poLineId' AS UUID) AS po_line_id,
CAST(jsonb->>'instanceId' AS UUID) AS instance_id,
jsonb->>'publisher' AS publisher,
jsonb->>'edition' AS edition,
jsonb->>'packageName' AS package_name,
jsonb->>'poLineNumber' AS po_line_number,
jsonb->>'publishedDate' AS published_date,
jsonb->>'receivingNote' AS receiving_note,
uc.TIMESTAMP_CAST(jsonb->>'subscriptionFrom') AS subscription_from,
uc.TIMESTAMP_CAST(jsonb->>'subscriptionTo') AS subscription_to,
CAST(jsonb->>'subscriptionInterval' AS INTEGER) AS subscription_interval,
CAST(jsonb->>'isAcknowledged' AS BOOLEAN) AS is_acknowledged,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_orders_storage.titles;
CREATE VIEW uc.transaction_tags AS
SELECT
id AS id,
transaction_id AS transaction_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS transaction_id, value AS jsonb FROM diku_mod_finance_storage.transaction, jsonb_array_elements_text((jsonb#>>'{tags,tagList}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.transactions AS
SELECT
id AS id,
CAST(jsonb->>'amount' AS DECIMAL(19,2)) AS amount,
CAST(jsonb#>>'{awaitingPayment,encumbranceId}' AS UUID) AS awaiting_payment_encumbrance_id,
CAST(jsonb#>>'{awaitingPayment,releaseEncumbrance}' AS BOOLEAN) AS awaiting_payment_release_encumbrance,
jsonb->>'currency' AS currency,
jsonb->>'description' AS description,
CAST(jsonb#>>'{encumbrance,amountAwaitingPayment}' AS DECIMAL(19,2)) AS encumbrance_amount_awaiting_payment,
CAST(jsonb#>>'{encumbrance,amountExpended}' AS DECIMAL(19,2)) AS encumbrance_amount_expended,
CAST(jsonb#>>'{encumbrance,initialAmountEncumbered}' AS DECIMAL(19,2)) AS encumbrance_initial_amount_encumbered,
jsonb#>>'{encumbrance,status}' AS encumbrance_status,
jsonb#>>'{encumbrance,orderType}' AS encumbrance_order_type,
jsonb#>>'{encumbrance,orderStatus}' AS encumbrance_order_status,
CAST(jsonb#>>'{encumbrance,subscription}' AS BOOLEAN) AS encumbrance_subscription,
CAST(jsonb#>>'{encumbrance,reEncumber}' AS BOOLEAN) AS encumbrance_re_encumber,
CAST(jsonb#>>'{encumbrance,sourcePurchaseOrderId}' AS UUID) AS encumbrance_source_purchase_order_id,
CAST(jsonb#>>'{encumbrance,sourcePoLineId}' AS UUID) AS encumbrance_source_po_line_id,
CAST(jsonb->>'expenseClassId' AS UUID) AS expense_class_id,
CAST(jsonb->>'fiscalYearId' AS UUID) AS fiscal_year_id,
CAST(jsonb->>'fromFundId' AS UUID) AS from_fund_id,
CAST(jsonb->>'invoiceCancelled' AS BOOLEAN) AS invoice_cancelled,
CAST(jsonb->>'paymentEncumbranceId' AS UUID) AS payment_encumbrance_id,
jsonb->>'source' AS source,
CAST(jsonb->>'sourceFiscalYearId' AS UUID) AS source_fiscal_year_id,
CAST(jsonb->>'sourceInvoiceId' AS UUID) AS source_invoice_id,
CAST(jsonb->>'sourceInvoiceLineId' AS UUID) AS source_invoice_line_id,
CAST(jsonb->>'toFundId' AS UUID) AS to_fund_id,
jsonb->>'transactionType' AS transaction_type,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_finance_storage.transaction;
CREATE VIEW uc.transfer_accounts AS
SELECT
id AS id,
jsonb->>'accountName' AS name,
jsonb->>'desc' AS desc,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
CAST(jsonb->>'ownerId' AS UUID) AS owner_id,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_feesfines.transfers;
CREATE VIEW uc.transfer_criterias AS
SELECT
id AS id,
jsonb->>'criteria' AS criteria,
jsonb->>'type' AS type,
CAST(jsonb->>'value' AS DECIMAL(19,2)) AS value,
jsonb->>'interval' AS interval,
jsonb_pretty(jsonb) AS content
FROM diku_mod_feesfines.transfer_criteria;
CREATE VIEW uc.user_departments AS
SELECT
id AS id,
user_id AS user_id,
CAST(jsonb AS UUID) AS department_id
FROM (SELECT id::text || ordinality::text AS id, id AS user_id, value AS jsonb FROM diku_mod_users.users, jsonb_array_elements_text((jsonb->>'departments')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.user_addresses AS
SELECT
id AS id,
user_id AS user_id,
CAST(jsonb->>'id' AS UUID) AS id2,
jsonb->>'countryId' AS country_id,
jsonb->>'addressLine1' AS address_line1,
jsonb->>'addressLine2' AS address_line2,
jsonb->>'city' AS city,
jsonb->>'region' AS region,
jsonb->>'postalCode' AS postal_code,
CAST(jsonb->>'addressTypeId' AS UUID) AS address_type_id,
CAST(jsonb->>'primaryAddress' AS BOOLEAN) AS primary_address
FROM (SELECT id::text || ordinality::text AS id, id AS user_id, value AS jsonb FROM diku_mod_users.users, jsonb_array_elements((jsonb#>>'{personal,addresses}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.user_tags AS
SELECT
id AS id,
user_id AS user_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS user_id, value AS jsonb FROM diku_mod_users.users, jsonb_array_elements_text((jsonb#>>'{tags,tagList}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.users AS
SELECT
id AS id,
jsonb->>'username' AS username,
jsonb->>'externalSystemId' AS external_system_id,
jsonb->>'barcode' AS barcode,
CAST(jsonb->>'active' AS BOOLEAN) AS active,
jsonb->>'type' AS type,
CAST(jsonb->>'patronGroup' AS UUID) AS group_id,
CAST(CONCAT_WS(' ', jsonb#>>'{personal,firstName}', jsonb#>>'{personal,middleName}', jsonb#>>'{personal,lastName}') AS VARCHAR(1024)) AS name,
jsonb#>>'{personal,lastName}' AS last_name,
jsonb#>>'{personal,firstName}' AS first_name,
jsonb#>>'{personal,middleName}' AS middle_name,
jsonb#>>'{personal,preferredFirstName}' AS preferred_first_name,
jsonb#>>'{personal,email}' AS email,
jsonb#>>'{personal,phone}' AS phone,
jsonb#>>'{personal,mobilePhone}' AS mobile_phone,
uc.TIMESTAMP_CAST(jsonb#>>'{personal,dateOfBirth}') AS date_of_birth,
jsonb#>>'{personal,preferredContactTypeId}' AS preferred_contact_type_id,
uc.TIMESTAMP_CAST(jsonb->>'enrollmentDate') AS enrollment_date,
uc.TIMESTAMP_CAST(jsonb->>'expirationDate') AS expiration_date,
CAST(jsonb#>>'{customFields,source}' AS VARCHAR(1024)) AS source,
CAST(jsonb#>>'{customFields,category}' AS VARCHAR(1024)) AS category,
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
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_users.users;
CREATE VIEW uc.user_acquisitions_units AS
SELECT
id AS id,
CAST(jsonb->>'userId' AS UUID) AS user_id,
CAST(jsonb->>'acquisitionsUnitId' AS UUID) AS acquisitions_unit_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_orders_storage.acquisitions_unit_membership;
CREATE VIEW uc.user_request_preferences AS
SELECT
id AS id,
CAST(jsonb->>'userId' AS UUID) AS user_id,
CAST(jsonb->>'holdShelf' AS BOOLEAN) AS hold_shelf,
CAST(jsonb->>'delivery' AS BOOLEAN) AS delivery,
CAST(jsonb->>'defaultServicePointId' AS UUID) AS default_service_point_id,
CAST(jsonb->>'defaultDeliveryAddressTypeId' AS UUID) AS default_delivery_address_type_id,
jsonb->>'fulfillment' AS fulfillment,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_circulation_storage.user_request_preference;
CREATE VIEW uc.user_summary_open_loans AS
SELECT
id AS id,
user_summary_id AS user_summary_id,
CAST(jsonb->>'loanId' AS UUID) AS loan_id,
uc.TIMESTAMP_CAST(jsonb->>'dueDate') AS due_date,
CAST(jsonb->>'recall' AS BOOLEAN) AS recall,
CAST(jsonb->>'itemLost' AS BOOLEAN) AS item_lost,
CAST(jsonb->>'itemClaimedReturned' AS BOOLEAN) AS item_claimed_returned,
CAST(jsonb#>>'{gracePeriod,duration}' AS INTEGER) AS grace_period_duration,
jsonb#>>'{gracePeriod,intervalId}' AS grace_period_interval_id
FROM (SELECT id::text || ordinality::text AS id, id AS user_summary_id, value AS jsonb FROM diku_mod_patron_blocks.user_summary, jsonb_array_elements((jsonb->>'openLoans')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.user_summary_open_fees_fines AS
SELECT
id AS id,
user_summary_id AS user_summary_id,
CAST(jsonb->>'feeFineId' AS UUID) AS fee_fine_id,
CAST(jsonb->>'feeFineTypeId' AS UUID) AS fee_fine_type_id,
CAST(jsonb->>'loanId' AS UUID) AS loan_id,
CAST(jsonb->>'balance' AS DECIMAL(19,2)) AS balance
FROM (SELECT id::text || ordinality::text AS id, id AS user_summary_id, value AS jsonb FROM diku_mod_patron_blocks.user_summary, jsonb_array_elements((jsonb->>'openFeesFines')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.user_summaries AS
SELECT
id AS id,
CAST(jsonb->>'_version' AS INTEGER) AS _version,
CAST(jsonb->>'userId' AS UUID) AS user_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_patron_blocks.user_summary;
CREATE VIEW uc.voucher_acquisitions_units AS
SELECT
id AS id,
voucher_id AS voucher_id,
CAST(jsonb AS UUID) AS acquisitions_unit_id
FROM (SELECT id::text || ordinality::text AS id, id AS voucher_id, value AS jsonb FROM diku_mod_invoice_storage.vouchers, jsonb_array_elements_text((jsonb->>'acqUnitIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.vouchers AS
SELECT
id AS id,
jsonb->>'accountingCode' AS accounting_code,
jsonb->>'accountNo' AS account_no,
CAST(jsonb->>'amount' AS DECIMAL(19,2)) AS amount,
CAST(jsonb->>'batchGroupId' AS UUID) AS batch_group_id,
jsonb->>'disbursementNumber' AS disbursement_number,
uc.TIMESTAMP_CAST(jsonb->>'disbursementDate') AS disbursement_date,
CAST(jsonb->>'disbursementAmount' AS DECIMAL(19,2)) AS disbursement_amount,
CAST(jsonb->>'enclosureNeeded' AS BOOLEAN) AS enclosure_needed,
jsonb->>'invoiceCurrency' AS invoice_currency,
CAST(jsonb->>'invoiceId' AS UUID) AS invoice_id,
CAST(jsonb->>'exchangeRate' AS DECIMAL(19,2)) AS exchange_rate,
CAST(jsonb->>'exportToAccounting' AS BOOLEAN) AS export_to_accounting,
jsonb->>'status' AS status,
jsonb->>'systemCurrency' AS system_currency,
jsonb->>'type' AS type,
uc.TIMESTAMP_CAST(jsonb->>'voucherDate') AS voucher_date,
jsonb->>'voucherNumber' AS voucher_number,
CAST(jsonb->>'vendorId' AS UUID) AS vendor_id,
jsonb#>>'{vendorAddress,addressLine1}' AS vendor_address_address_line1,
jsonb#>>'{vendorAddress,addressLine2}' AS vendor_address_address_line2,
jsonb#>>'{vendorAddress,city}' AS vendor_address_city,
jsonb#>>'{vendorAddress,stateRegion}' AS vendor_address_state_region,
jsonb#>>'{vendorAddress,zipCode}' AS vendor_address_zip_code,
jsonb#>>'{vendorAddress,country}' AS vendor_address_country,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_invoice_storage.vouchers;
CREATE VIEW uc.voucher_item_fund_distributions AS
SELECT
id AS id,
voucher_item_id AS voucher_item_id,
jsonb->>'code' AS code,
CAST(jsonb->>'encumbrance' AS UUID) AS encumbrance_id,
CAST(jsonb->>'fundId' AS UUID) AS fund_id,
CAST(jsonb->>'invoiceLineId' AS UUID) AS invoice_item_id,
jsonb->>'distributionType' AS distribution_type,
CAST(jsonb->>'expenseClassId' AS UUID) AS expense_class_id,
CAST(jsonb->>'value' AS DECIMAL(19,2)) AS value
FROM (SELECT id::text || ordinality::text AS id, id AS voucher_item_id, value AS jsonb FROM diku_mod_invoice_storage.voucher_lines, jsonb_array_elements((jsonb->>'fundDistributions')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.voucher_item_invoice_items AS
SELECT
id AS id,
voucher_item_id AS voucher_item_id,
CAST(jsonb AS UUID) AS invoice_item_id
FROM (SELECT id::text || ordinality::text AS id, id AS voucher_item_id, value AS jsonb FROM diku_mod_invoice_storage.voucher_lines, jsonb_array_elements_text((jsonb->>'sourceIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.voucher_items AS
SELECT
id AS id,
CAST(jsonb->>'amount' AS DECIMAL(19,2)) AS amount,
jsonb->>'externalAccountNumber' AS external_account_number,
CAST(jsonb->>'subTransactionId' AS UUID) AS sub_transaction_id,
CAST(jsonb->>'voucherId' AS UUID) AS voucher_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_invoice_storage.voucher_lines;
CREATE VIEW uc.waive_reasons AS
SELECT
id AS id,
jsonb->>'nameReason' AS name,
jsonb->>'description' AS description,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}' || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END) AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
jsonb#>>'{metadata,createdByUsername}' AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
jsonb#>>'{metadata,updatedByUsername}' AS updated_by_username,
CAST(jsonb->>'accountId' AS UUID) AS account_id,
jsonb_pretty(COALESCE(jsonb_set(jsonb, '{metadata,createdDate}', ('"' || (jsonb#>>'{metadata,createdDate}') || CASE WHEN jsonb#>>'{metadata,createdDate}' !~ '([-+]\d\d:\d\d)|Z$' THEN '+00:00' ELSE '' END || '"')::jsonb), jsonb)) AS content
FROM diku_mod_feesfines.waives;
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
    alpha2_code VARCHAR(2) NOT NULL,
    alpha3_code VARCHAR(3) NOT NULL,
    name VARCHAR(128) NOT NULL,
    PRIMARY KEY(alpha2_code)
);
INSERT INTO uc.countries VALUES ('AD', 'AND', 'Andorra'), ('AE', 'ARE', 'United Arab Emirates'), ('AF', 'AFG', 'Afghanistan'), ('AG', 'ATG', 'Antigua and Barbuda'), ('AI', 'AIA', 'Anguilla'), ('AL', 'ALB', 'Albania'), ('AM', 'ARM', 'Armenia'), ('AN', 'ANT', 'Netherlands Antilles'), ('AO', 'AGO', 'Angola'), ('AQ', 'ATA', 'Antarctica'), ('AR', 'ARG', 'Argentina'), ('AS', 'ASM', 'American Samoa'), ('AT', 'AUT', 'Austria'), ('AU', 'AUS', 'Australia'), ('AW', 'ABW', 'Aruba'), ('AX', 'ALA', 'Åland Islands'), ('AZ', 'AZE', 'Azerbaijan'), ('BA', 'BIH', 'Bosnia and Herzegovina'), ('BB', 'BRB', 'Barbados'), ('BD', 'BGD', 'Bangladesh'), ('BE', 'BEL', 'Belgium'), ('BF', 'BFA', 'Burkina Faso'), ('BG', 'BGR', 'Bulgaria'), ('BH', 'BHR', 'Bahrain'), ('BI', 'BDI', 'Burundi'), ('BJ', 'BEN', 'Benin'), ('BL', 'BLM', 'Saint BarthÃ©lemy'), ('BM', 'BMU', 'Bermuda'), ('BN', 'BRN', 'Brunei Darussalam'), ('BO', 'BOL', 'Bolivia, Plurinational State Of'), ('BR', 'BRA', 'Brazil'), ('BS', 'BHS', 'Bahamas'), ('BT', 'BTN', 'Bhutan'), ('BV', 'BVT', 'Bouvet Island'), ('BW', 'BWA', 'Botswana'), ('BY', 'BLR', 'Belarus'), ('BZ', 'BLZ', 'Belize'), ('CA', 'CAN', 'Canada'), ('CC', 'CCK', 'Cocos (Keeling), Islands'), ('CD', 'COD', 'Congo, The Democratic Republic Of The'), ('CF', 'CAF', 'Central African Republic'), ('CG', 'COG', 'Congo'), ('CH', 'CHE', 'Switzerland'), ('CI', 'CIV', 'CÃ´te D''Ivoire'), ('CK', 'COK', 'Cook Islands'), ('CL', 'CHL', 'Chile'), ('CM', 'CMR', 'Cameroon'), ('CN', 'CHN', 'China'), ('CO', 'COL', 'Colombia'), ('CR', 'CRI', 'Costa Rica'), ('CU', 'CUB', 'Cuba'), ('CV', 'CPV', 'Cape Verde'), ('CX', 'CXR', 'Christmas Island'), ('CY', 'CYP', 'Cyprus'), ('CZ', 'CZE', 'Czech Republic'), ('DE', 'DEU', 'Germany'), ('DJ', 'DJI', 'Djibouti'), ('DK', 'DNK', 'Denmark'), ('DM', 'DMA', 'Dominica'), ('DO', 'DOM', 'Dominican Republic'), ('DZ', 'DZA', 'Algeria'), ('EC', 'ECU', 'Ecuador'), ('EE', 'EST', 'Estonia'), ('EG', 'EGY', 'Egypt'), ('EH', 'ESH', 'Western Sahara'), ('ER', 'ERI', 'Eritrea'), ('ES', 'ESP', 'Spain'), ('ET', 'ETH', 'Ethiopia'), ('FI', 'FIN', 'Finland'), ('FJ', 'FJI', 'Fiji'), ('FK', 'FLK', 'Falkland Islands (Malvinas),'), ('FM', 'FSM', 'Micronesia, Federated States Of'), ('FO', 'FRO', 'Faroe Islands'), ('FR', 'FRA', 'France'), ('GA', 'GAB', 'Gabon'), ('GB', 'GBR', 'United Kingdom'), ('GD', 'GRD', 'Grenada'), ('GE', 'GEO', 'Georgia'), ('GF', 'GUF', 'French Guiana'), ('GG', 'GGY', 'Guernsey'), ('GH', 'GHA', 'Ghana'), ('GI', 'GIB', 'Gibraltar'), ('GL', 'GRL', 'Greenland'), ('GM', 'GMB', 'Gambia'), ('GN', 'GIN', 'Guinea'), ('GP', 'GLP', 'Guadeloupe'), ('GQ', 'GNQ', 'Equatorial Guinea'), ('GR', 'GRC', 'Greece'), ('GS', 'SGS', 'South Georgia and South Sandwich Islands'), ('GT', 'GTM', 'Guatemala'), ('GU', 'GUM', 'Guam'), ('GW', 'GNB', 'Guinea-Bissau'), ('GY', 'GUY', 'Guyana'), ('HK', 'HKG', 'Hong Kong'), ('HM', 'HMD', 'Heard and McDonald Islands'), ('HN', 'HND', 'Honduras'), ('HR', 'HRV', 'Croatia'), ('HT', 'HTI', 'Haiti'), ('HU', 'HUN', 'Hungary'), ('ID', 'IDN', 'Indonesia'), ('IE', 'IRL', 'Ireland'), ('IL', 'ISR', 'Israel'), ('IM', 'IMN', 'Isle of Man'), ('IN', 'IND', 'India'), ('IO', 'IOT', 'British Indian Ocean Territory'), ('IQ', 'IRQ', 'Iraq'), ('IR', 'IRN', 'Iran, Islamic Republic Of'), ('IS', 'ISL', 'Iceland'), ('IT', 'ITA', 'Italy'), ('JE', 'JEY', 'Jersey'), ('JM', 'JAM', 'Jamaica'), ('JO', 'JOR', 'Jordan'), ('JP', 'JPN', 'Japan'), ('KE', 'KEN', 'Kenya'), ('KG', 'KGZ', 'Kyrgyzstan'), ('KH', 'KHM', 'Cambodia'), ('KI', 'KIR', 'Kiribati'), ('KM', 'COM', 'Comoros'), ('KN', 'KNA', 'Saint Kitts And Nevis'), ('KP', 'PRK', 'Korea, Democratic People''s Republic Of'), ('KR', 'KOR', 'Korea, Republic of'), ('KW', 'KWT', 'Kuwait'), ('KY', 'CYM', 'Cayman Islands'), ('KZ', 'KAZ', 'Kazakhstan'), ('LA', 'LAO', 'Lao People''s Democratic Republic'), ('LB', 'LBN', 'Lebanon'), ('LC', 'LCA', 'Saint Lucia'), ('LI', 'LIE', 'Liechtenstein'), ('LK', 'LKA', 'Sri Lanka'), ('LR', 'LBR', 'Liberia'), ('LS', 'LSO', 'Lesotho'), ('LT', 'LTU', 'Lithuania'), ('LU', 'LUX', 'Luxembourg'), ('LV', 'LVA', 'Latvia'), ('LY', 'LBY', 'Libyan Arab Jamahiriya'), ('MA', 'MAR', 'Morocco'), ('MC', 'MCO', 'Monaco'), ('MD', 'MDA', 'Moldova, Republic of'), ('ME', 'MNE', 'Montenegro'), ('MF', 'MAF', 'Saint Martin'), ('MG', 'MDG', 'Madagascar'), ('MH', 'MHL', 'Marshall Islands'), ('MK', 'MKD', 'Macedonia, the Fmr. Yugoslav Republic Of'), ('ML', 'MLI', 'Mali'), ('MM', 'MMR', 'Myanmar'), ('MN', 'MNG', 'Mongolia'), ('MO', 'MAC', 'Macao'), ('MP', 'MNP', 'Northern Mariana Islands'), ('MQ', 'MTQ', 'Martinique'), ('MR', 'MRT', 'Mauritania'), ('MS', 'MSR', 'Montserrat'), ('MT', 'MLT', 'Malta'), ('MU', 'MUS', 'Mauritius'), ('MV', 'MDV', 'Maldives'), ('MW', 'MWI', 'Malawi'), ('MX', 'MEX', 'Mexico'), ('MY', 'MYS', 'Malaysia'), ('MZ', 'MOZ', 'Mozambique'), ('NA', 'NAM', 'Namibia'), ('NC', 'NCL', 'New Caledonia'), ('NE', 'NER', 'Niger'), ('NF', 'NFK', 'Norfolk Island'), ('NG', 'NGA', 'Nigeria'), ('NI', 'NIC', 'Nicaragua'), ('NL', 'NLD', 'Netherlands'), ('NO', 'NOR', 'Norway'), ('NP', 'NPL', 'Nepal'), ('NR', 'NRU', 'Nauru'), ('NU', 'NIU', 'Niue'), ('NZ', 'NZL', 'New Zealand'), ('OM', 'OMN', 'Oman'), ('PA', 'PAN', 'Panama'), ('PE', 'PER', 'Peru'), ('PF', 'PYF', 'French Polynesia'), ('PG', 'PNG', 'Papua New Guinea'), ('PH', 'PHL', 'Philippines'), ('PK', 'PAK', 'Pakistan'), ('PL', 'POL', 'Poland'), ('PM', 'SPM', 'Saint Pierre And Miquelon'), ('PN', 'PCN', 'Pitcairn'), ('PR', 'PRI', 'Puerto Rico'), ('PS', 'PSE', 'Palestinian Territory, Occupied'), ('PT', 'PRT', 'Portugal'), ('PW', 'PLW', 'Palau'), ('PY', 'PRY', 'Paraguay'), ('QA', 'QAT', 'Qatar'), ('RE', 'REU', 'RÃ©union'), ('RO', 'ROU', 'Romania'), ('RS', 'SRB', 'Serbia'), ('RU', 'RUS', 'Russian Federation'), ('RW', 'RWA', 'Rwanda'), ('SA', 'SAU', 'Saudi Arabia'), ('SB', 'SLB', 'Solomon Islands'), ('SC', 'SYC', 'Seychelles'), ('SD', 'SDN', 'Sudan'), ('SE', 'SWE', 'Sweden'), ('SG', 'SGP', 'Singapore'), ('SH', 'SHN', 'St. Helena, Ascension, Tristan Da Cunha'), ('SI', 'SVN', 'Slovenia'), ('SJ', 'SJM', 'Svalbard And Jan Mayen'), ('SK', 'SVK', 'Slovakia'), ('SL', 'SLE', 'Sierra Leone'), ('SM', 'SMR', 'San Marino'), ('SN', 'SEN', 'Senegal'), ('SO', 'SOM', 'Somalia'), ('SR', 'SUR', 'Suriname'), ('ST', 'STP', 'Sao Tome and Principe'), ('SV', 'SLV', 'El Salvador'), ('SY', 'SYR', 'Syrian Arab Republic'), ('SZ', 'SWZ', 'Swaziland'), ('TC', 'TCA', 'Turks and Caicos Islands'), ('TD', 'TCD', 'Chad'), ('TF', 'ATF', 'French Southern Territories'), ('TG', 'TGO', 'Togo'), ('TH', 'THA', 'Thailand'), ('TJ', 'TJK', 'Tajikistan'), ('TK', 'TKL', 'Tokelau'), ('TL', 'TLS', 'Timor-Leste'), ('TM', 'TKM', 'Turkmenistan'), ('TN', 'TUN', 'Tunisia'), ('TO', 'TON', 'Tonga'), ('TR', 'TUR', 'Turkey'), ('TT', 'TTO', 'Trinidad and Tobago'), ('TV', 'TUV', 'Tuvalu'), ('TW', 'TWN', 'Taiwan, Province Of China'), ('TZ', 'TZA', 'Tanzania, United Republic of'), ('UA', 'UKR', 'Ukraine'), ('UG', 'UGA', 'Uganda'), ('UM', 'UMI', 'United States Minor Outlying Islands'), ('US', 'USA', 'United States'), ('UY', 'URY', 'Uruguay'), ('UZ', 'UZB', 'Uzbekistan'), ('VA', 'VAT', 'Holy See (Vatican City State),'), ('VC', 'VCT', 'Saint Vincent And The Grenedines'), ('VE', 'VEN', 'Venezuela, Bolivarian Republic of'), ('VG', 'VGB', 'Virgin Islands, British'), ('VI', 'VIR', 'Virgin Islands, U.S.'), ('VN', 'VNM', 'Viet Nam'), ('VU', 'VUT', 'Vanuatu'), ('WF', 'WLF', 'Wallis and Futuna'), ('WS', 'WSM', 'Samoa'), ('YE', 'YEM', 'Yemen'), ('YT', 'MYT', 'Mayotte'), ('ZA', 'ZAF', 'South Africa'), ('ZM', 'ZMB', 'Zambia'), ('ZW', 'ZWE', 'Zimbabwe'), ('ZZ', 'ZZZ', 'Other Countries');
CREATE TABLE uc.contact_types (
    id VARCHAR(3) NOT NULL,
    name VARCHAR(128) NOT NULL,
    PRIMARY KEY(id)
);
INSERT INTO uc.contact_types VALUES ('001', 'Mail'),  ('002', 'Email'),  ('003', 'Text message');
