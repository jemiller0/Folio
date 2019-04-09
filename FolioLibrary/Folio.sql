DROP SCHEMA IF EXISTS uc CASCADE;
CREATE SCHEMA uc;
CREATE FUNCTION uc.timestamp_cast(IN TEXT) RETURNS TIMESTAMP WITH TIME ZONE AS 'SELECT CAST($1 AS TIMESTAMP WITH TIME ZONE);' LANGUAGE 'sql' IMMUTABLE;
CREATE VIEW uc.accounts AS
SELECT
id AS id,
CAST(jsonb->>'amount' AS DECIMAL(19,2)) AS amount,
CAST(jsonb->>'remaining' AS DECIMAL(19,2)) AS remaining,
uc.TIMESTAMP_CAST(jsonb->>'dateCreated') AS date_created,
uc.TIMESTAMP_CAST(jsonb->>'dateUpdated') AS date_updated,
CAST(jsonb#>>'{status,name}' AS VARCHAR(1024)) AS status_name,
CAST(jsonb#>>'{paymentStatus,name}' AS VARCHAR(1024)) AS payment_status_name,
CAST(jsonb->>'feeFineType' AS VARCHAR(1024)) AS fee_fine_type,
CAST(jsonb->>'feeFineOwner' AS VARCHAR(1024)) AS fee_fine_owner,
CAST(jsonb->>'title' AS VARCHAR(1024)) AS title,
CAST(jsonb->>'callNumber' AS VARCHAR(1024)) AS call_number,
CAST(jsonb->>'barcode' AS VARCHAR(1024)) AS barcode,
CAST(jsonb->>'materialType' AS VARCHAR(1024)) AS material_type,
CAST(jsonb#>>'{itemStatus,name}' AS VARCHAR(1024)) AS item_status_name,
CAST(jsonb->>'location' AS VARCHAR(1024)) AS location,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
uc.TIMESTAMP_CAST(jsonb->>'dueDate') AS due_date,
uc.TIMESTAMP_CAST(jsonb->>'returnedDate') AS returned_date,
CAST(jsonb->>'loanId' AS UUID) AS loan_id,
CAST(jsonb->>'userId' AS UUID) AS user_id,
CAST(jsonb->>'itemId' AS UUID) AS item_id,
CAST(jsonb->>'materialTypeId' AS UUID) AS material_type_id,
CAST(jsonb->>'feeFineId' AS UUID) AS fee_id,
CAST(jsonb->>'ownerId' AS UUID) AS owner_id,
jsonb_pretty(jsonb) AS content
FROM diku_mod_feesfines.accounts;
CREATE VIEW uc.account2s AS
SELECT
id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'account_no' AS VARCHAR(1024)) AS account_no,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
CAST(jsonb->>'app_system_no' AS VARCHAR(1024)) AS app_system_no,
CAST(jsonb->>'payment_method' AS VARCHAR(1024)) AS payment_method,
CAST(jsonb->>'account_status' AS VARCHAR(1024)) AS account_status,
CAST(jsonb->>'contact_info' AS VARCHAR(1024)) AS contact_info,
CAST(jsonb->>'library_code' AS VARCHAR(1024)) AS library_code,
CAST(jsonb->>'library_edi_code' AS VARCHAR(1024)) AS library_edi_code,
CAST(jsonb->>'notes' AS VARCHAR(1024)) AS notes,
jsonb_pretty(jsonb) AS content
FROM diku_mod_vendors.account;
CREATE VIEW uc.address_categories AS
SELECT
id AS id,
address_id AS address_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS address_id, value AS jsonb FROM diku_mod_vendors.address, jsonb_array_elements_text((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.addresses AS
SELECT
id AS id,
CAST(jsonb->>'addressLine1' AS VARCHAR(1024)) AS address_line1,
CAST(jsonb->>'addressLine2' AS VARCHAR(1024)) AS address_line2,
CAST(jsonb->>'city' AS VARCHAR(1024)) AS city,
CAST(jsonb->>'stateRegion' AS VARCHAR(1024)) AS state_region,
CAST(jsonb->>'zipCode' AS VARCHAR(1024)) AS zip_code,
CAST(jsonb->>'country' AS VARCHAR(1024)) AS country,
CAST(jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
CAST(jsonb->>'language' AS VARCHAR(1024)) AS language,
jsonb_pretty(jsonb) AS content
FROM diku_mod_vendors.address;
CREATE VIEW uc.address_types AS
SELECT
id AS id,
CAST(jsonb->>'addressType' AS VARCHAR(1024)) AS address_type,
CAST(jsonb->>'desc' AS VARCHAR(1024)) AS desc,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_users.addresstype;
CREATE VIEW uc.agreements AS
SELECT
id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'discount' AS DECIMAL(19,2)) AS discount,
CAST(jsonb->>'reference_url' AS VARCHAR(1024)) AS reference_url,
CAST(jsonb->>'notes' AS VARCHAR(1024)) AS notes,
jsonb_pretty(jsonb) AS content
FROM diku_mod_vendors.agreement;
CREATE VIEW uc.alerts AS
SELECT
id AS id,
CAST(jsonb->>'alert' AS VARCHAR(1024)) AS alert,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.alert;
CREATE VIEW uc.aliases AS
SELECT
id AS id,
CAST(jsonb->>'value' AS VARCHAR(1024)) AS value,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
jsonb_pretty(jsonb) AS content
FROM diku_mod_vendors.alias;
CREATE VIEW uc.alternative_title_types AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'source' AS VARCHAR(1024)) AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.alternative_title_type;
CREATE VIEW uc.auth_attempts AS
SELECT
_id AS id,
CAST(jsonb->>'userId' AS UUID) AS user_id,
uc.TIMESTAMP_CAST(jsonb->>'lastAttempt') AS last_attempt,
CAST(jsonb->>'attemptCount' AS INTEGER) AS attempt_count,
jsonb_pretty(jsonb) AS content
FROM diku_mod_login.auth_attempts;
CREATE VIEW uc.auth_credentials_histories AS
SELECT
_id AS id,
CAST(jsonb->>'userId' AS UUID) AS user_id,
CAST(jsonb->>'hash' AS VARCHAR(1024)) AS hash,
CAST(jsonb->>'salt' AS VARCHAR(1024)) AS salt,
uc.TIMESTAMP_CAST(jsonb->>'date') AS date,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_login.auth_credentials_history;
CREATE VIEW uc.blocks AS
SELECT
id AS id,
CAST(jsonb->>'type' AS VARCHAR(1024)) AS type,
CAST(jsonb->>'desc' AS VARCHAR(1024)) AS desc,
CAST(jsonb->>'staffInformation' AS VARCHAR(1024)) AS staff_information,
CAST(jsonb->>'patronMessage' AS VARCHAR(1024)) AS patron_message,
uc.TIMESTAMP_CAST(jsonb->>'expirationDate') AS expiration_date,
CAST(jsonb->>'borrowing' AS BOOLEAN) AS borrowing,
CAST(jsonb->>'renewals' AS BOOLEAN) AS renewals,
CAST(jsonb->>'requests' AS BOOLEAN) AS requests,
CAST(jsonb->>'userId' AS UUID) AS user_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_feesfines.manualblocks;
CREATE VIEW uc.budget_tags AS
SELECT
id AS id,
budget_id AS budget_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS budget_id, value AS jsonb FROM diku_mod_finance_storage.budget, jsonb_array_elements_text((jsonb->>'tags')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.budgets AS
SELECT
id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'budget_status' AS VARCHAR(1024)) AS budget_status,
CAST(jsonb->>'code' AS VARCHAR(1024)) AS code,
CAST(jsonb->>'limit_enc_percent' AS DECIMAL(19,2)) AS limit_enc_percent,
CAST(jsonb->>'limit_exp_percent' AS DECIMAL(19,2)) AS limit_exp_percent,
CAST(jsonb->>'allocation' AS DECIMAL(19,2)) AS allocation,
CAST(jsonb->>'awaiting_payment' AS DECIMAL(19,2)) AS awaiting_payment,
CAST(jsonb->>'available' AS DECIMAL(19,2)) AS available,
CAST(jsonb->>'encumbered' AS DECIMAL(19,2)) AS encumbered,
CAST(jsonb->>'expenditures' AS DECIMAL(19,2)) AS expenditures,
CAST(jsonb->>'over_encumbrance' AS DECIMAL(19,2)) AS over_encumbrance,
CAST(jsonb->>'fund_id' AS UUID) AS fund_id2,
CAST(jsonb->>'fiscal_year_id' AS UUID) AS fiscal_year_id2,
jsonb_pretty(jsonb) AS content,
fund_id AS fund_id,
fiscal_year_id AS fiscal_year_id
FROM diku_mod_finance_storage.budget;
CREATE VIEW uc.call_number_types AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'source' AS VARCHAR(1024)) AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.call_number_type;
CREATE VIEW uc.campuses AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'code' AS VARCHAR(1024)) AS code,
CAST(jsonb->>'institutionId' AS UUID) AS institution_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content,
institutionid AS institutionid
FROM diku_mod_inventory_storage.loccampus;
CREATE VIEW uc.cancellation_reasons AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
CAST(jsonb->>'publicDescription' AS VARCHAR(1024)) AS public_description,
CAST(jsonb->>'requiresAdditionalInformation' AS BOOLEAN) AS requires_additional_information,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_circulation_storage.cancellation_reason;
CREATE VIEW uc.categories AS
SELECT
id AS id,
CAST(jsonb->>'value' AS VARCHAR(1024)) AS value,
jsonb_pretty(jsonb) AS content
FROM diku_mod_vendors.category;
CREATE VIEW uc.circulation_rules AS
SELECT
_id AS id,
CAST(jsonb->>'rulesAsText' AS VARCHAR(1024)) AS rules_as_text,
jsonb_pretty(jsonb) AS content,
lock AS lock
FROM diku_mod_circulation_storage.circulation_rules;
CREATE VIEW uc.claims AS
SELECT
id AS id,
CAST(jsonb->>'claimed' AS BOOLEAN) AS claimed,
uc.TIMESTAMP_CAST(jsonb->>'sent') AS sent,
CAST(jsonb->>'grace' AS INTEGER) AS grace,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.claim;
CREATE VIEW uc.classification_types AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.classification_type;
CREATE VIEW uc.comments AS
SELECT
id AS id,
CAST(jsonb->>'paid' AS BOOLEAN) AS paid,
CAST(jsonb->>'waived' AS BOOLEAN) AS waived,
CAST(jsonb->>'refunded' AS BOOLEAN) AS refunded,
CAST(jsonb->>'transferredManually' AS BOOLEAN) AS transferred_manually,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_feesfines.comments;
CREATE VIEW uc.contact_phone_number_categories AS
SELECT
id AS id,
contact_phone_number_id AS contact_phone_number_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS contact_phone_number_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS contact_id, value AS jsonb FROM diku_mod_vendors.contact, jsonb_array_elements((jsonb->>'phone_numbers')::jsonb) WITH ORDINALITY) a, jsonb_array_elements_text((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.contact_phone_number_types AS
SELECT
id AS id,
contact_phone_number_id AS contact_phone_number_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS contact_phone_number_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS contact_id, value AS jsonb FROM diku_mod_vendors.contact, jsonb_array_elements((jsonb->>'phone_numbers')::jsonb) WITH ORDINALITY) a, jsonb_array_elements_text((jsonb->>'types')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.contact_phone_numbers AS
SELECT
id AS id,
contact_id AS contact_id,
CAST(jsonb->>'id' AS UUID) AS id2,
CAST(jsonb->>'phone_number' AS VARCHAR(1024)) AS phone_number,
CAST(jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
CAST(jsonb->>'language' AS VARCHAR(1024)) AS language
FROM (SELECT id::text || ordinality::text AS id, id AS contact_id, value AS jsonb FROM diku_mod_vendors.contact, jsonb_array_elements((jsonb->>'phone_numbers')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.contact_email_categories AS
SELECT
id AS id,
contact_email_id AS contact_email_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS contact_email_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS contact_id, value AS jsonb FROM diku_mod_vendors.contact, jsonb_array_elements((jsonb->>'emails')::jsonb) WITH ORDINALITY) a, jsonb_array_elements_text((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.contact_emails AS
SELECT
id AS id,
contact_id AS contact_id,
CAST(jsonb->>'id' AS UUID) AS id2,
CAST(jsonb->>'value' AS VARCHAR(1024)) AS value,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
CAST(jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
CAST(jsonb->>'language' AS VARCHAR(1024)) AS language
FROM (SELECT id::text || ordinality::text AS id, id AS contact_id, value AS jsonb FROM diku_mod_vendors.contact, jsonb_array_elements((jsonb->>'emails')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.contact_address_categories AS
SELECT
id AS id,
contact_address_id AS contact_address_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS contact_address_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS contact_id, value AS jsonb FROM diku_mod_vendors.contact, jsonb_array_elements((jsonb->>'addresses')::jsonb) WITH ORDINALITY) a, jsonb_array_elements_text((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.contact_addresses AS
SELECT
id AS id,
contact_id AS contact_id,
CAST(jsonb->>'id' AS UUID) AS id2,
CAST(jsonb->>'addressLine1' AS VARCHAR(1024)) AS address_line1,
CAST(jsonb->>'addressLine2' AS VARCHAR(1024)) AS address_line2,
CAST(jsonb->>'city' AS VARCHAR(1024)) AS city,
CAST(jsonb->>'stateRegion' AS VARCHAR(1024)) AS state_region,
CAST(jsonb->>'zipCode' AS VARCHAR(1024)) AS zip_code,
CAST(jsonb->>'country' AS VARCHAR(1024)) AS country,
CAST(jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
CAST(jsonb->>'language' AS VARCHAR(1024)) AS language
FROM (SELECT id::text || ordinality::text AS id, id AS contact_id, value AS jsonb FROM diku_mod_vendors.contact, jsonb_array_elements((jsonb->>'addresses')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.contact_url_categories AS
SELECT
id AS id,
contact_url_id AS contact_url_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS contact_url_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS contact_id, value AS jsonb FROM diku_mod_vendors.contact, jsonb_array_elements((jsonb->>'urls')::jsonb) WITH ORDINALITY) a, jsonb_array_elements_text((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.contact_urls AS
SELECT
id AS id,
contact_id AS contact_id,
CAST(jsonb->>'id' AS UUID) AS id2,
CAST(jsonb->>'value' AS VARCHAR(1024)) AS value,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
CAST(jsonb->>'language' AS VARCHAR(1024)) AS language,
CAST(jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
CAST(jsonb->>'notes' AS VARCHAR(1024)) AS notes
FROM (SELECT id::text || ordinality::text AS id, id AS contact_id, value AS jsonb FROM diku_mod_vendors.contact, jsonb_array_elements((jsonb->>'urls')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.contact_categories AS
SELECT
id AS id,
contact_id AS contact_id,
CAST(jsonb->>'id' AS UUID) AS id2,
CAST(jsonb->>'value' AS VARCHAR(1024)) AS value
FROM (SELECT id::text || ordinality::text AS id, id AS contact_id, value AS jsonb FROM diku_mod_vendors.contact, jsonb_array_elements((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.contacts AS
SELECT
id AS id,
CAST(jsonb->>'prefix' AS VARCHAR(1024)) AS prefix,
CAST(jsonb->>'first_name' AS VARCHAR(1024)) AS first_name,
CAST(jsonb->>'last_name' AS VARCHAR(1024)) AS last_name,
CAST(jsonb->>'language' AS VARCHAR(1024)) AS language,
CAST(jsonb->>'notes' AS VARCHAR(1024)) AS notes,
CAST(jsonb->>'inactive' AS BOOLEAN) AS inactive,
jsonb_pretty(jsonb) AS content
FROM diku_mod_vendors.contact;
CREATE VIEW uc.contributor_name_types AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'ordering' AS VARCHAR(1024)) AS ordering,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.contributor_name_type;
CREATE VIEW uc.contributor_types AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'code' AS VARCHAR(1024)) AS code,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.contributor_type;
CREATE VIEW uc.costs AS
SELECT
id AS id,
CAST(jsonb->>'listUnitPrice' AS DECIMAL(19,2)) AS list_unit_price,
CAST(jsonb->>'listUnitPriceElectronic' AS DECIMAL(19,2)) AS list_unit_price_electronic,
CAST(jsonb->>'currency' AS VARCHAR(1024)) AS currency,
CAST(jsonb->>'additionalCost' AS DECIMAL(19,2)) AS additional_cost,
CAST(jsonb->>'discount' AS DECIMAL(19,2)) AS discount,
CAST(jsonb->>'discountType' AS VARCHAR(1024)) AS discount_type,
CAST(jsonb->>'quantityPhysical' AS INTEGER) AS quantity_physical,
CAST(jsonb->>'quantityElectronic' AS INTEGER) AS quantity_electronic,
CAST(jsonb->>'poLineEstimatedPrice' AS DECIMAL(19,2)) AS po_line_estimated_price,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.cost;
CREATE VIEW uc.detail_product_ids AS
SELECT
id AS id,
detail_id AS detail_id,
CAST(jsonb->>'productId' AS VARCHAR(1024)) AS product_id,
CAST(jsonb->>'productIdType' AS VARCHAR(1024)) AS product_id_type
FROM (SELECT id::text || ordinality::text AS id, id AS detail_id, value AS jsonb FROM diku_mod_orders_storage.details, jsonb_array_elements((jsonb->>'productIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.details AS
SELECT
id AS id,
CAST(jsonb->>'receivingNote' AS VARCHAR(1024)) AS receiving_note,
uc.TIMESTAMP_CAST(jsonb->>'subscriptionFrom') AS subscription_from,
CAST(jsonb->>'subscriptionInterval' AS INTEGER) AS subscription_interval,
uc.TIMESTAMP_CAST(jsonb->>'subscriptionTo') AS subscription_to,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.details;
CREATE VIEW uc.electronic_access_relationships AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.electronic_access_relationship;
CREATE VIEW uc.email_categories AS
SELECT
id AS id,
email_id AS email_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS email_id, value AS jsonb FROM diku_mod_vendors.email, jsonb_array_elements_text((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.emails AS
SELECT
id AS id,
CAST(jsonb->>'value' AS VARCHAR(1024)) AS value,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
CAST(jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
CAST(jsonb->>'language' AS VARCHAR(1024)) AS language,
jsonb_pretty(jsonb) AS content
FROM diku_mod_vendors.email;
CREATE VIEW uc.eresources AS
SELECT
id AS id,
CAST(jsonb->>'activated' AS BOOLEAN) AS activated,
CAST(jsonb->>'activationDue' AS INTEGER) AS activation_due,
CAST(jsonb->>'createInventory' AS VARCHAR(1024)) AS create_inventory,
CAST(jsonb->>'trial' AS BOOLEAN) AS trial,
uc.TIMESTAMP_CAST(jsonb->>'expectedActivation') AS expected_activation,
CAST(jsonb->>'userLimit' AS INTEGER) AS user_limit,
CAST(jsonb->>'accessProvider' AS UUID) AS access_provider_id,
CAST(jsonb#>>'{license,code}' AS VARCHAR(1024)) AS license_code,
CAST(jsonb#>>'{license,description}' AS VARCHAR(1024)) AS license_description,
CAST(jsonb#>>'{license,reference}' AS VARCHAR(1024)) AS license_reference,
CAST(jsonb->>'materialType' AS UUID) AS material_type_id,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.eresource;
CREATE VIEW uc.event_logs AS
SELECT
_id AS id,
CAST(jsonb->>'tenant' AS VARCHAR(1024)) AS tenant,
CAST(jsonb->>'userId' AS UUID) AS user_id,
CAST(jsonb->>'ip' AS VARCHAR(1024)) AS ip,
CAST(jsonb->>'browserInformation' AS VARCHAR(1024)) AS browser_information,
uc.TIMESTAMP_CAST(jsonb->>'timestamp') AS timestamp,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_login.event_logs;
CREATE VIEW uc.fees AS
SELECT
id AS id,
CAST(jsonb->>'feeFineType' AS VARCHAR(1024)) AS fee_fine_type,
CAST(jsonb->>'defaultAmount' AS DECIMAL(19,2)) AS default_amount,
CAST(jsonb->>'chargeNoticeId' AS UUID) AS charge_notice_id,
CAST(jsonb->>'actionNoticeId' AS UUID) AS action_notice_id,
CAST(jsonb->>'ownerId' AS UUID) AS owner_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content,
ownerid AS ownerid
FROM diku_mod_feesfines.feefines;
CREATE VIEW uc.fee_actions AS
SELECT
id AS id,
uc.TIMESTAMP_CAST(jsonb->>'dateAction') AS date_action,
CAST(jsonb->>'typeAction' AS VARCHAR(1024)) AS type_action,
CAST(jsonb->>'comments' AS VARCHAR(1024)) AS comments,
CAST(jsonb->>'amountAction' AS DECIMAL(19,2)) AS amount_action,
CAST(jsonb->>'balance' AS DECIMAL(19,2)) AS balance,
CAST(jsonb->>'transactionInformation' AS VARCHAR(1024)) AS transaction_information,
CAST(jsonb->>'createdAt' AS VARCHAR(1024)) AS created_at,
CAST(jsonb->>'source' AS VARCHAR(1024)) AS source,
CAST(jsonb->>'paymentMethod' AS VARCHAR(1024)) AS payment_method,
CAST(jsonb->>'accountId' AS UUID) AS account_id,
CAST(jsonb->>'userId' AS UUID) AS user_id,
jsonb_pretty(jsonb) AS content
FROM diku_mod_feesfines.feefineactions;
CREATE VIEW uc.fiscal_years AS
SELECT
id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'code' AS VARCHAR(1024)) AS code,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
uc.TIMESTAMP_CAST(jsonb->>'start_date') AS start_date,
uc.TIMESTAMP_CAST(jsonb->>'end_date') AS end_date,
jsonb_pretty(jsonb) AS content
FROM diku_mod_finance_storage.fiscal_year;
CREATE VIEW uc.fixed_due_date_schedule_schedules AS
SELECT
id AS id,
fixed_due_date_schedule_id AS fixed_due_date_schedule_id,
uc.TIMESTAMP_CAST(jsonb->>'from') AS from,
uc.TIMESTAMP_CAST(jsonb->>'to') AS to,
uc.TIMESTAMP_CAST(jsonb->>'due') AS due
FROM (SELECT _id::text || ordinality::text AS id, _id AS fixed_due_date_schedule_id, value AS jsonb FROM diku_mod_circulation_storage.fixed_due_date_schedule, jsonb_array_elements((jsonb->>'schedules')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.fixed_due_date_schedules AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_circulation_storage.fixed_due_date_schedule;
CREATE VIEW uc.fund_allocation_from AS
SELECT
id AS id,
fund_id AS fund_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS fund_id, value AS jsonb FROM diku_mod_finance_storage.fund, jsonb_array_elements_text((jsonb->>'allocation_from')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.fund_allocation_to AS
SELECT
id AS id,
fund_id AS fund_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS fund_id, value AS jsonb FROM diku_mod_finance_storage.fund, jsonb_array_elements_text((jsonb->>'allocation_to')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.fund_tags AS
SELECT
id AS id,
fund_id AS fund_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS fund_id, value AS jsonb FROM diku_mod_finance_storage.fund, jsonb_array_elements_text((jsonb->>'tags')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.funds AS
SELECT
id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'code' AS VARCHAR(1024)) AS code,
uc.TIMESTAMP_CAST(jsonb->>'created_date') AS created_date,
CAST(jsonb->>'fund_status' AS VARCHAR(1024)) AS fund_status,
CAST(jsonb->>'currency' AS VARCHAR(1024)) AS currency,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
CAST(jsonb->>'ledger_id' AS UUID) AS ledger_id2,
jsonb_pretty(jsonb) AS content,
ledger_id AS ledger_id
FROM diku_mod_finance_storage.fund;
CREATE VIEW uc.fund_distributions AS
SELECT
id AS id,
CAST(jsonb->>'amount' AS DECIMAL(19,2)) AS amount,
CAST(jsonb->>'currency' AS VARCHAR(1024)) AS currency,
CAST(jsonb->>'percent' AS DECIMAL(19,2)) AS percent,
CAST(jsonb->>'source_type' AS VARCHAR(1024)) AS source_type,
CAST(jsonb->>'source_id' AS UUID) AS source_id,
CAST(jsonb->>'budget_id' AS UUID) AS budget_id2,
jsonb_pretty(jsonb) AS content,
budget_id AS budget_id
FROM diku_mod_finance_storage.fund_distribution;
CREATE VIEW uc.fund_distribution2s AS
SELECT
id AS id,
CAST(jsonb->>'code' AS VARCHAR(1024)) AS code,
CAST(jsonb->>'encumbrance' AS UUID) AS encumbrance_id,
CAST(jsonb->>'percentage' AS DECIMAL(19,2)) AS percentage,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.fund_distribution;
CREATE VIEW uc.groups AS
SELECT
id AS id,
CAST(jsonb->>'group' AS VARCHAR(1024)) AS group,
CAST(jsonb->>'desc' AS VARCHAR(1024)) AS desc,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_users.groups;
CREATE VIEW uc.holding_former_ids AS
SELECT
id AS id,
holding_id AS holding_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS holding_id, value AS jsonb FROM diku_mod_inventory_storage.holdings_record, jsonb_array_elements_text((jsonb->>'formerIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.holding_electronic_accesses AS
SELECT
id AS id,
holding_id AS holding_id,
CAST(jsonb->>'uri' AS VARCHAR(1024)) AS uri,
CAST(jsonb->>'linkText' AS VARCHAR(1024)) AS link_text,
CAST(jsonb->>'materialsSpecification' AS VARCHAR(1024)) AS materials_specification,
CAST(jsonb->>'publicNote' AS VARCHAR(1024)) AS public_note,
CAST(jsonb->>'relationshipId' AS UUID) AS relationship_id
FROM (SELECT _id::text || ordinality::text AS id, _id AS holding_id, value AS jsonb FROM diku_mod_inventory_storage.holdings_record, jsonb_array_elements((jsonb->>'electronicAccess')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.holding_notes AS
SELECT
id AS id,
holding_id AS holding_id,
CAST(jsonb->>'holdingsNoteTypeId' AS UUID) AS holding_note_type_id,
CAST(jsonb->>'note' AS VARCHAR(1024)) AS note,
CAST(jsonb->>'staffOnly' AS BOOLEAN) AS staff_only
FROM (SELECT _id::text || ordinality::text AS id, _id AS holding_id, value AS jsonb FROM diku_mod_inventory_storage.holdings_record, jsonb_array_elements((jsonb->>'notes')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.extents AS
SELECT
id AS id,
holding_id AS holding_id,
CAST(jsonb->>'statement' AS VARCHAR(1024)) AS statement,
CAST(jsonb->>'note' AS VARCHAR(1024)) AS note
FROM (SELECT _id::text || ordinality::text AS id, _id AS holding_id, value AS jsonb FROM diku_mod_inventory_storage.holdings_record, jsonb_array_elements((jsonb->>'holdingsStatements')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.index_statements AS
SELECT
id AS id,
holding_id AS holding_id,
CAST(jsonb->>'statement' AS VARCHAR(1024)) AS statement,
CAST(jsonb->>'note' AS VARCHAR(1024)) AS note
FROM (SELECT _id::text || ordinality::text AS id, _id AS holding_id, value AS jsonb FROM diku_mod_inventory_storage.holdings_record, jsonb_array_elements((jsonb->>'holdingsStatementsForIndexes')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.supplement_statements AS
SELECT
id AS id,
holding_id AS holding_id,
CAST(jsonb->>'statement' AS VARCHAR(1024)) AS statement,
CAST(jsonb->>'note' AS VARCHAR(1024)) AS note
FROM (SELECT _id::text || ordinality::text AS id, _id AS holding_id, value AS jsonb FROM diku_mod_inventory_storage.holdings_record, jsonb_array_elements((jsonb->>'holdingsStatementsForSupplements')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.holding_entries AS
SELECT
id AS id,
holding_id AS holding_id,
CAST(jsonb->>'publicDisplay' AS BOOLEAN) AS public_display,
CAST(jsonb->>'enumeration' AS VARCHAR(1024)) AS enumeration,
CAST(jsonb->>'chronology' AS VARCHAR(1024)) AS chronology
FROM (SELECT _id::text || ordinality::text AS id, _id AS holding_id, value AS jsonb FROM diku_mod_inventory_storage.holdings_record, jsonb_array_elements((jsonb#>>'{receivingHistory,entries}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.holding_statistical_code_ids AS
SELECT
id AS id,
holding_id AS holding_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS holding_id, value AS jsonb FROM diku_mod_inventory_storage.holdings_record, jsonb_array_elements_text((jsonb->>'statisticalCodeIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.holdings AS
SELECT
_id AS id,
CAST(jsonb->>'hrid' AS VARCHAR(1024)) AS hrid,
CAST(jsonb->>'holdingsTypeId' AS UUID) AS holding_type_id,
CAST(jsonb->>'instanceId' AS UUID) AS instance_id,
CAST(jsonb->>'permanentLocationId' AS UUID) AS permanent_location_id,
CAST(jsonb->>'temporaryLocationId' AS UUID) AS temporary_location_id,
CAST(jsonb->>'callNumberTypeId' AS UUID) AS call_number_type_id,
CAST(jsonb->>'callNumberPrefix' AS VARCHAR(1024)) AS call_number_prefix,
CAST(jsonb->>'callNumber' AS VARCHAR(1024)) AS call_number,
CAST(jsonb->>'callNumberSuffix' AS VARCHAR(1024)) AS call_number_suffix,
CAST(jsonb->>'shelvingTitle' AS VARCHAR(1024)) AS shelving_title,
CAST(jsonb->>'acquisitionFormat' AS VARCHAR(1024)) AS acquisition_format,
CAST(jsonb->>'acquisitionMethod' AS VARCHAR(1024)) AS acquisition_method,
CAST(jsonb->>'receiptStatus' AS VARCHAR(1024)) AS receipt_status,
CAST(jsonb->>'illPolicyId' AS UUID) AS ill_policy_id,
CAST(jsonb->>'retentionPolicy' AS VARCHAR(1024)) AS retention_policy,
CAST(jsonb->>'digitizationPolicy' AS VARCHAR(1024)) AS digitization_policy,
CAST(jsonb->>'copyNumber' AS VARCHAR(1024)) AS copy_number,
CAST(jsonb->>'numberOfItems' AS VARCHAR(1024)) AS number_of_items,
CAST(jsonb#>>'{receivingHistory,displayType}' AS VARCHAR(1024)) AS receiving_history_display_type,
CAST(jsonb->>'discoverySuppress' AS BOOLEAN) AS discovery_suppress,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content,
instanceid AS instanceid,
permanentlocationid AS permanentlocationid,
temporarylocationid AS temporarylocationid,
holdingstypeid AS holdingstypeid,
callnumbertypeid AS callnumbertypeid,
illpolicyid AS illpolicyid
FROM diku_mod_inventory_storage.holdings_record;
CREATE VIEW uc.holding_note_types AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'source' AS VARCHAR(1024)) AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.holdings_note_type;
CREATE VIEW uc.holding_types AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'source' AS VARCHAR(1024)) AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.holdings_type;
CREATE VIEW uc.id_types AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.identifier_type;
CREATE VIEW uc.ill_policies AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'source' AS VARCHAR(1024)) AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.ill_policy;
CREATE VIEW uc.alternative_titles AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb->>'alternativeTitleTypeId' AS UUID) AS alternative_title_type_id,
CAST(jsonb->>'alternativeTitle' AS VARCHAR(1024)) AS alternative_title
FROM (SELECT _id::text || ordinality::text AS id, _id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements((jsonb->>'alternativeTitles')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.editions AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements_text((jsonb->>'editions')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.series AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements_text((jsonb->>'series')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.identifiers AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb->>'value' AS VARCHAR(1024)) AS value,
CAST(jsonb->>'identifierTypeId' AS UUID) AS identifier_type_id
FROM (SELECT _id::text || ordinality::text AS id, _id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements((jsonb->>'identifiers')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.contributors AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'contributorTypeId' AS UUID) AS contributor_type_id,
CAST(jsonb->>'contributorTypeText' AS VARCHAR(1024)) AS contributor_type_text,
CAST(jsonb->>'contributorNameTypeId' AS UUID) AS contributor_name_type_id,
CAST(jsonb->>'primary' AS BOOLEAN) AS primary
FROM (SELECT _id::text || ordinality::text AS id, _id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements((jsonb->>'contributors')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.subjects AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements_text((jsonb->>'subjects')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.classifications AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb->>'classificationNumber' AS VARCHAR(1024)) AS classification_number,
CAST(jsonb->>'classificationTypeId' AS UUID) AS classification_type_id
FROM (SELECT _id::text || ordinality::text AS id, _id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements((jsonb->>'classifications')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.publications AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb->>'publisher' AS VARCHAR(1024)) AS publisher,
CAST(jsonb->>'place' AS VARCHAR(1024)) AS place,
CAST(jsonb->>'dateOfPublication' AS VARCHAR(1024)) AS date_of_publication,
CAST(jsonb->>'role' AS VARCHAR(1024)) AS role
FROM (SELECT _id::text || ordinality::text AS id, _id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements((jsonb->>'publication')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.publication_frequency AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements_text((jsonb->>'publicationFrequency')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.publication_range AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements_text((jsonb->>'publicationRange')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.electronic_accesses AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb->>'uri' AS VARCHAR(1024)) AS uri,
CAST(jsonb->>'linkText' AS VARCHAR(1024)) AS link_text,
CAST(jsonb->>'materialsSpecification' AS VARCHAR(1024)) AS materials_specification,
CAST(jsonb->>'publicNote' AS VARCHAR(1024)) AS public_note,
CAST(jsonb->>'relationshipId' AS UUID) AS relationship_id
FROM (SELECT _id::text || ordinality::text AS id, _id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements((jsonb->>'electronicAccess')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.instance_format_ids AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements_text((jsonb->>'instanceFormatIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.physical_descriptions AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements_text((jsonb->>'physicalDescriptions')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.languages AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements_text((jsonb->>'languages')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.notes AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements_text((jsonb->>'notes')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.statistical_code_ids AS
SELECT
id AS id,
instance_id AS instance_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS instance_id, value AS jsonb FROM diku_mod_inventory_storage.instance, jsonb_array_elements_text((jsonb->>'statisticalCodeIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.instances AS
SELECT
_id AS id,
CAST(jsonb->>'hrid' AS VARCHAR(1024)) AS hrid,
CAST(jsonb->>'source' AS VARCHAR(1024)) AS source,
CAST(jsonb->>'title' AS VARCHAR(1024)) AS title,
CAST(jsonb->>'indexTitle' AS VARCHAR(1024)) AS index_title,
CAST(jsonb->>'instanceTypeId' AS UUID) AS instance_type_id,
CAST(jsonb->>'modeOfIssuanceId' AS UUID) AS mode_of_issuance_id,
CAST(jsonb->>'catalogedDate' AS VARCHAR(1024)) AS cataloged_date,
CAST(jsonb->>'previouslyHeld' AS BOOLEAN) AS previously_held,
CAST(jsonb->>'staffSuppress' AS BOOLEAN) AS staff_suppress,
CAST(jsonb->>'discoverySuppress' AS BOOLEAN) AS discovery_suppress,
CAST(jsonb->>'sourceRecordFormat' AS VARCHAR(1024)) AS source_record_format,
CAST(jsonb->>'statusId' AS UUID) AS status_id,
uc.TIMESTAMP_CAST(jsonb->>'statusUpdatedDate') AS status_updated_date,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content,
instancestatusid AS instancestatusid,
modeofissuanceid AS modeofissuanceid
FROM diku_mod_inventory_storage.instance;
CREATE VIEW uc.formats AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'code' AS VARCHAR(1024)) AS code,
CAST(jsonb->>'source' AS VARCHAR(1024)) AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.instance_format;
CREATE VIEW uc.relationships AS
SELECT
_id AS id,
CAST(jsonb->>'superInstanceId' AS UUID) AS super_instance_id,
CAST(jsonb->>'subInstanceId' AS UUID) AS sub_instance_id,
CAST(jsonb->>'instanceRelationshipTypeId' AS UUID) AS instance_relationship_type_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content,
superinstanceid AS superinstanceid,
subinstanceid AS subinstanceid,
instancerelationshiptypeid AS instancerelationshiptypeid
FROM diku_mod_inventory_storage.instance_relationship;
CREATE VIEW uc.relationship_types AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.instance_relationship_type;
CREATE VIEW uc.source_marc_fields AS
SELECT
id AS id,
source_marc_id AS source_marc_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS source_marc_id, value AS jsonb FROM diku_mod_inventory_storage.instance_source_marc, jsonb_array_elements_text((jsonb->>'fields')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.source_marcs AS
SELECT
_id AS id,
CAST(jsonb->>'leader' AS VARCHAR(24)) AS leader,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.instance_source_marc;
CREATE VIEW uc.statuses AS
SELECT
_id AS id,
CAST(jsonb->>'code' AS VARCHAR(1024)) AS code,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'source' AS VARCHAR(1024)) AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.instance_status;
CREATE VIEW uc.instance_types AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'code' AS VARCHAR(1024)) AS code,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.instance_type;
CREATE VIEW uc.institutions AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'code' AS VARCHAR(1024)) AS code,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.locinstitution;
CREATE VIEW uc.interfaces AS
SELECT
id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'uri' AS VARCHAR(1024)) AS uri,
CAST(jsonb->>'username' AS VARCHAR(1024)) AS username,
CAST(jsonb->>'password' AS VARCHAR(1024)) AS password,
CAST(jsonb->>'notes' AS VARCHAR(1024)) AS notes,
CAST(jsonb->>'available' AS BOOLEAN) AS available,
CAST(jsonb->>'delivery_method' AS VARCHAR(1024)) AS delivery_method,
CAST(jsonb->>'statistics_format' AS VARCHAR(1024)) AS statistics_format,
CAST(jsonb->>'locally_stored' AS VARCHAR(1024)) AS locally_stored,
CAST(jsonb->>'online_location' AS VARCHAR(1024)) AS online_location,
CAST(jsonb->>'statistics_notes' AS VARCHAR(1024)) AS statistics_notes,
jsonb_pretty(jsonb) AS content
FROM diku_mod_vendors.interface;
CREATE VIEW uc.item_former_ids AS
SELECT
id AS id,
item_id AS item_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS item_id, value AS jsonb FROM diku_mod_inventory_storage.item, jsonb_array_elements_text((jsonb->>'formerIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.item_year_caption AS
SELECT
id AS id,
item_id AS item_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS item_id, value AS jsonb FROM diku_mod_inventory_storage.item, jsonb_array_elements_text((jsonb->>'yearCaption')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.copy_numbers AS
SELECT
id AS id,
item_id AS item_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS item_id, value AS jsonb FROM diku_mod_inventory_storage.item, jsonb_array_elements_text((jsonb->>'copyNumbers')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.item_notes AS
SELECT
id AS id,
item_id AS item_id,
CAST(jsonb->>'itemNoteTypeId' AS UUID) AS item_note_type_id,
CAST(jsonb->>'note' AS VARCHAR(1024)) AS note,
CAST(jsonb->>'staffOnly' AS BOOLEAN) AS staff_only
FROM (SELECT _id::text || ordinality::text AS id, _id AS item_id, value AS jsonb FROM diku_mod_inventory_storage.item, jsonb_array_elements((jsonb->>'notes')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.circulation_notes AS
SELECT
id AS id,
item_id AS item_id,
CAST(jsonb->>'noteType' AS VARCHAR(1024)) AS note_type,
CAST(jsonb->>'note' AS VARCHAR(1024)) AS note,
CAST(jsonb->>'staffOnly' AS BOOLEAN) AS staff_only
FROM (SELECT _id::text || ordinality::text AS id, _id AS item_id, value AS jsonb FROM diku_mod_inventory_storage.item, jsonb_array_elements((jsonb->>'circulationNotes')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.item_electronic_accesses AS
SELECT
id AS id,
item_id AS item_id,
CAST(jsonb->>'uri' AS VARCHAR(1024)) AS uri,
CAST(jsonb->>'linkText' AS VARCHAR(1024)) AS link_text,
CAST(jsonb->>'materialsSpecification' AS VARCHAR(1024)) AS materials_specification,
CAST(jsonb->>'publicNote' AS VARCHAR(1024)) AS public_note,
CAST(jsonb->>'relationshipId' AS UUID) AS relationship_id
FROM (SELECT _id::text || ordinality::text AS id, _id AS item_id, value AS jsonb FROM diku_mod_inventory_storage.item, jsonb_array_elements((jsonb->>'electronicAccess')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.item_statistical_code_ids AS
SELECT
id AS id,
item_id AS item_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS item_id, value AS jsonb FROM diku_mod_inventory_storage.item, jsonb_array_elements_text((jsonb->>'statisticalCodeIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.items AS
SELECT
_id AS id,
CAST(jsonb->>'hrid' AS VARCHAR(1024)) AS hrid,
CAST(jsonb->>'holdingsRecordId' AS UUID) AS holding_id,
CAST(jsonb->>'discoverySuppress' AS BOOLEAN) AS discovery_suppress,
CAST(jsonb->>'accessionNumber' AS VARCHAR(1024)) AS accession_number,
CAST(jsonb->>'barcode' AS VARCHAR(1024)) AS barcode,
CAST(jsonb->>'itemLevelCallNumber' AS VARCHAR(1024)) AS call_number,
CAST(jsonb->>'itemLevelCallNumberPrefix' AS VARCHAR(1024)) AS call_number_prefix,
CAST(jsonb->>'itemLevelCallNumberSuffix' AS VARCHAR(1024)) AS call_number_suffix,
CAST(jsonb->>'itemLevelCallNumberTypeId' AS UUID) AS call_number_type_id,
CAST(jsonb->>'volume' AS VARCHAR(1024)) AS volume,
CAST(jsonb->>'enumeration' AS VARCHAR(1024)) AS enumeration,
CAST(jsonb->>'chronology' AS VARCHAR(1024)) AS chronology,
CAST(jsonb->>'itemIdentifier' AS VARCHAR(1024)) AS item_identifier,
CAST(jsonb->>'numberOfPieces' AS VARCHAR(1024)) AS number_of_pieces,
CAST(jsonb->>'descriptionOfPieces' AS VARCHAR(1024)) AS description_of_pieces,
CAST(jsonb->>'numberOfMissingPieces' AS VARCHAR(1024)) AS number_of_missing_pieces,
CAST(jsonb->>'missingPieces' AS VARCHAR(1024)) AS missing_pieces,
CAST(jsonb->>'missingPiecesDate' AS VARCHAR(1024)) AS missing_pieces_date,
CAST(jsonb->>'itemDamagedStatusId' AS UUID) AS item_damaged_status_id,
CAST(jsonb->>'itemDamagedStatusDate' AS VARCHAR(1024)) AS item_damaged_status_date,
CAST(jsonb#>>'{status,name}' AS VARCHAR(1024)) AS status_name,
uc.TIMESTAMP_CAST(jsonb#>>'{status,date}') AS status_date,
CAST(jsonb->>'materialTypeId' AS UUID) AS material_type_id,
CAST(jsonb->>'permanentLoanTypeId' AS UUID) AS permanent_loan_type_id,
CAST(jsonb->>'temporaryLoanTypeId' AS UUID) AS temporary_loan_type_id,
CAST(jsonb->>'permanentLocationId' AS UUID) AS permanent_location_id,
CAST(jsonb->>'temporaryLocationId' AS UUID) AS temporary_location_id,
CAST(jsonb->>'inTransitDestinationServicePointId' AS UUID) AS in_transit_destination_service_point_id,
CAST(jsonb->>'purchaseOrderLineIdentifier' AS UUID) AS order_item_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content,
holdingsrecordid AS holdingsrecordid,
permanentloantypeid AS permanentloantypeid,
temporaryloantypeid AS temporaryloantypeid,
materialtypeid AS materialtypeid,
permanentlocationid AS permanentlocationid,
temporarylocationid AS temporarylocationid
FROM diku_mod_inventory_storage.item;
CREATE VIEW uc.item_note_types AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'source' AS VARCHAR(1024)) AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.item_note_type;
CREATE VIEW uc.ledger_fiscal_years AS
SELECT
id AS id,
ledger_id AS ledger_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS ledger_id, value AS jsonb FROM diku_mod_finance_storage.ledger, jsonb_array_elements_text((jsonb->>'fiscal_years')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.ledgers AS
SELECT
id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'code' AS VARCHAR(1024)) AS code,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
CAST(jsonb->>'ledger_status' AS VARCHAR(1024)) AS ledger_status,
uc.TIMESTAMP_CAST(jsonb->>'period_start') AS period_start,
uc.TIMESTAMP_CAST(jsonb->>'period_end') AS period_end,
jsonb_pretty(jsonb) AS content
FROM diku_mod_finance_storage.ledger;
CREATE VIEW uc.libraries AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'code' AS VARCHAR(1024)) AS code,
CAST(jsonb->>'campusId' AS UUID) AS campus_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content,
campusid AS campusid
FROM diku_mod_inventory_storage.loclibrary;
CREATE VIEW uc.loans AS
SELECT
_id AS id,
CAST(jsonb->>'userId' AS UUID) AS user_id,
CAST(jsonb->>'proxyUserId' AS UUID) AS proxy_user_id,
CAST(jsonb->>'itemId' AS UUID) AS item_id,
CAST(jsonb#>>'{status,name}' AS VARCHAR(1024)) AS status_name,
uc.TIMESTAMP_CAST(jsonb->>'loanDate') AS loan_date,
uc.TIMESTAMP_CAST(jsonb->>'dueDate') AS due_date,
CAST(jsonb->>'returnDate' AS VARCHAR(1024)) AS return_date,
uc.TIMESTAMP_CAST(jsonb->>'systemReturnDate') AS system_return_date,
CAST(jsonb->>'action' AS VARCHAR(1024)) AS action,
CAST(jsonb->>'actionComment' AS VARCHAR(1024)) AS action_comment,
CAST(jsonb->>'itemStatus' AS VARCHAR(1024)) AS item_status,
CAST(jsonb->>'renewalCount' AS INTEGER) AS renewal_count,
CAST(jsonb->>'loanPolicyId' AS UUID) AS loan_policy_id,
CAST(jsonb->>'checkoutServicePointId' AS UUID) AS checkout_service_point_id,
CAST(jsonb->>'checkinServicePointId' AS UUID) AS checkin_service_point_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_circulation_storage.loan;
CREATE VIEW uc.loan_policies AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
CAST(jsonb->>'loanable' AS BOOLEAN) AS loanable,
CAST(jsonb#>>'{loansPolicy,profileId}' AS VARCHAR(1024)) AS loans_policy_profile_id,
CAST(jsonb#>>'{loansPolicy,period,duration}' AS INTEGER) AS loans_policy_period_duration,
CAST(jsonb#>>'{loansPolicy,period,intervalId}' AS VARCHAR(1024)) AS loans_policy_period_interval_id,
CAST(jsonb#>>'{loansPolicy,closedLibraryDueDateManagementId}' AS VARCHAR(1024)) AS loans_policy_closed_library_due_date_management_id,
CAST(jsonb#>>'{loansPolicy,gracePeriod,duration}' AS INTEGER) AS loans_policy_grace_period_duration,
CAST(jsonb#>>'{loansPolicy,gracePeriod,intervalId}' AS VARCHAR(1024)) AS loans_policy_grace_period_interval_id,
CAST(jsonb#>>'{loansPolicy,openingTimeOffset,duration}' AS INTEGER) AS loans_policy_opening_time_offset_duration,
CAST(jsonb#>>'{loansPolicy,openingTimeOffset,intervalId}' AS UUID) AS loans_policy_opening_time_offset_interval_id,
CAST(jsonb#>>'{loansPolicy,fixedDueDateScheduleId}' AS UUID) AS loans_policy_fixed_due_date_schedule_id,
CAST(jsonb->>'renewable' AS BOOLEAN) AS renewable,
CAST(jsonb#>>'{renewalsPolicy,unlimited}' AS BOOLEAN) AS renewals_policy_unlimited,
CAST(jsonb#>>'{renewalsPolicy,numberAllowed}' AS DECIMAL(19,2)) AS renewals_policy_number_allowed,
CAST(jsonb#>>'{renewalsPolicy,renewFromId}' AS VARCHAR(1024)) AS renewals_policy_renew_from_id,
CAST(jsonb#>>'{renewalsPolicy,differentPeriod}' AS BOOLEAN) AS renewals_policy_different_period,
CAST(jsonb#>>'{renewalsPolicy,period,duration}' AS INTEGER) AS renewals_policy_period_duration,
CAST(jsonb#>>'{renewalsPolicy,period,intervalId}' AS VARCHAR(1024)) AS renewals_policy_period_interval_id,
CAST(jsonb#>>'{renewalsPolicy,alternateFixedDueDateScheduleId}' AS UUID) AS renewals_policy_alternate_fixed_due_date_schedule_id,
CAST(jsonb#>>'{requestManagement,recalls,alternateGracePeriod,duration}' AS INTEGER) AS recalls_alternate_grace_period_duration,
CAST(jsonb#>>'{requestManagement,recalls,alternateGracePeriod,intervalId}' AS UUID) AS recalls_alternate_grace_period_interval_id,
CAST(jsonb#>>'{requestManagement,recalls,minimumGuaranteedLoanPeriod,duration}' AS INTEGER) AS recalls_minimum_guaranteed_loan_period_duration,
CAST(jsonb#>>'{requestManagement,recalls,minimumGuaranteedLoanPeriod,intervalId}' AS UUID) AS recalls_minimum_guaranteed_loan_period_interval_id,
CAST(jsonb#>>'{requestManagement,recalls,recallReturnInterval,duration}' AS INTEGER) AS recalls_recall_return_interval_duration,
CAST(jsonb#>>'{requestManagement,recalls,recallReturnInterval,intervalId}' AS UUID) AS recalls_recall_return_interval_interval_id,
CAST(jsonb#>>'{requestManagement,holds,alternateCheckoutLoanPeriod,duration}' AS INTEGER) AS holds_alternate_checkout_loan_period_duration,
CAST(jsonb#>>'{requestManagement,holds,alternateCheckoutLoanPeriod,intervalId}' AS UUID) AS holds_alternate_checkout_loan_period_interval_id,
CAST(jsonb#>>'{requestManagement,holds,renewItemsWithRequest}' AS BOOLEAN) AS holds_renew_items_with_request,
CAST(jsonb#>>'{requestManagement,holds,alternateRenewalLoanPeriod,duration}' AS INTEGER) AS holds_alternate_renewal_loan_period_duration,
CAST(jsonb#>>'{requestManagement,holds,alternateRenewalLoanPeriod,intervalId}' AS UUID) AS holds_alternate_renewal_loan_period_interval_id,
CAST(jsonb#>>'{requestManagement,pages,alternateCheckoutLoanPeriod,duration}' AS INTEGER) AS pages_alternate_checkout_loan_period_duration,
CAST(jsonb#>>'{requestManagement,pages,alternateCheckoutLoanPeriod,intervalId}' AS UUID) AS pages_alternate_checkout_loan_period_interval_id,
CAST(jsonb#>>'{requestManagement,pages,renewItemsWithRequest}' AS BOOLEAN) AS pages_renew_items_with_request,
CAST(jsonb#>>'{requestManagement,pages,alternateRenewalLoanPeriod,duration}' AS INTEGER) AS pages_alternate_renewal_loan_period_duration,
CAST(jsonb#>>'{requestManagement,pages,alternateRenewalLoanPeriod,intervalId}' AS UUID) AS pages_alternate_renewal_loan_period_interval_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content,
loanspolicy_fixedduedatescheduleid AS loanspolicy_fixedduedatescheduleid,
renewalspolicy_alternatefixedduedatescheduleid AS renewalspolicy_alternatefixedduedatescheduleid
FROM diku_mod_circulation_storage.loan_policy;
CREATE VIEW uc.loan_types AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.loan_type;
CREATE VIEW uc.location_service_point_ids AS
SELECT
id AS id,
location_id AS location_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS location_id, value AS jsonb FROM diku_mod_inventory_storage.location, jsonb_array_elements_text((jsonb->>'servicePointIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.locations AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'code' AS VARCHAR(1024)) AS code,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
CAST(jsonb->>'discoveryDisplayName' AS VARCHAR(1024)) AS discovery_display_name,
CAST(jsonb->>'isActive' AS BOOLEAN) AS is_active,
CAST(jsonb->>'institutionId' AS UUID) AS institution_id,
CAST(jsonb->>'campusId' AS UUID) AS campus_id,
CAST(jsonb->>'libraryId' AS UUID) AS library_id,
CAST(jsonb->>'primaryServicePoint' AS UUID) AS primary_service_point_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content,
institutionid AS institutionid,
campusid AS campusid,
libraryid AS libraryid
FROM diku_mod_inventory_storage.location;
CREATE VIEW uc.logins AS
SELECT
_id AS id,
CAST(jsonb->>'userId' AS UUID) AS user_id,
CAST(jsonb->>'hash' AS VARCHAR(1024)) AS hash,
CAST(jsonb->>'salt' AS VARCHAR(1024)) AS salt,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_login.auth_credentials;
CREATE VIEW uc.material_types AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'source' AS VARCHAR(1024)) AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.material_type;
CREATE VIEW uc.mode_of_issuances AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.mode_of_issuance;
CREATE VIEW uc.note_links AS
SELECT
id AS id,
note_id AS note_id,
CAST(jsonb->>'id' AS VARCHAR(1024)) AS id2,
CAST(jsonb->>'type' AS VARCHAR(1024)) AS type,
CAST(jsonb->>'domain' AS VARCHAR(1024)) AS domain
FROM (SELECT id::text || ordinality::text AS id, id AS note_id, value AS jsonb FROM diku_mod_notes.note_data, jsonb_array_elements((jsonb->>'links')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.notes2 AS
SELECT
id AS id,
CAST(jsonb->>'type' AS VARCHAR(1024)) AS type,
CAST(jsonb->>'title' AS VARCHAR(1024)) AS title,
CAST(jsonb->>'content' AS VARCHAR(1024)) AS content2,
CAST(jsonb->>'status' AS VARCHAR(1024)) AS status,
CAST(jsonb#>>'{creator,lastName}' AS VARCHAR(1024)) AS creator_last_name,
CAST(jsonb#>>'{creator,firstName}' AS VARCHAR(1024)) AS creator_first_name,
CAST(jsonb#>>'{creator,middleName}' AS VARCHAR(1024)) AS creator_middle_name,
CAST(jsonb#>>'{updater,lastName}' AS VARCHAR(1024)) AS updater_last_name,
CAST(jsonb#>>'{updater,firstName}' AS VARCHAR(1024)) AS updater_first_name,
CAST(jsonb#>>'{updater,middleName}' AS VARCHAR(1024)) AS updater_middle_name,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_notes.note_data;
CREATE VIEW uc.order_notes AS
SELECT
id AS id,
order_id AS order_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS order_id, value AS jsonb FROM diku_mod_orders_storage.purchase_order, jsonb_array_elements_text((jsonb->>'notes')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.orders AS
SELECT
id AS id,
CAST(jsonb->>'approved' AS BOOLEAN) AS approved,
CAST(jsonb->>'assignedTo' AS UUID) AS assigned_to_id,
CAST(jsonb#>>'{closeReason,reason}' AS VARCHAR(1024)) AS close_reason_reason,
CAST(jsonb#>>'{closeReason,note}' AS VARCHAR(1024)) AS close_reason_note,
uc.TIMESTAMP_CAST(jsonb->>'dateOrdered') AS date_ordered,
CAST(jsonb->>'manualPo' AS BOOLEAN) AS manual_po,
CAST(jsonb->>'poNumber' AS VARCHAR(1024)) AS po_number,
CAST(jsonb->>'orderType' AS VARCHAR(1024)) AS order_type,
CAST(jsonb->>'reEncumber' AS BOOLEAN) AS re_encumber,
CAST(jsonb#>>'{renewal,cycle}' AS VARCHAR(1024)) AS renewal_cycle,
CAST(jsonb#>>'{renewal,interval}' AS INTEGER) AS renewal_interval,
CAST(jsonb#>>'{renewal,manualRenewal}' AS BOOLEAN) AS renewal_manual_renewal,
CAST(jsonb#>>'{renewal,reviewPeriod}' AS INTEGER) AS renewal_review_period,
uc.TIMESTAMP_CAST(jsonb#>>'{renewal,renewalDate}') AS renewal_renewal_date,
CAST(jsonb->>'vendor' AS UUID) AS vendor_id,
CAST(jsonb->>'workflowStatus' AS VARCHAR(1024)) AS workflow_status,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.purchase_order;
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
CAST(jsonb->>'contributor' AS VARCHAR(1024)) AS contributor,
CAST(jsonb->>'contributorType' AS VARCHAR(1024)) AS contributor_type
FROM (SELECT id::text || ordinality::text AS id, id AS order_item_id, value AS jsonb FROM diku_mod_orders_storage.po_line, jsonb_array_elements((jsonb->>'contributors')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.order_item_product_ids AS
SELECT
id AS id,
order_item_id AS order_item_id,
CAST(jsonb->>'productId' AS UUID) AS product_id,
CAST(jsonb->>'productIdType' AS VARCHAR(1024)) AS product_id_type
FROM (SELECT id::text || ordinality::text AS id, id AS order_item_id, value AS jsonb FROM diku_mod_orders_storage.po_line, jsonb_array_elements((jsonb#>>'{details,productIds}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.order_item_fund_distributions AS
SELECT
id AS id,
order_item_id AS order_item_id,
CAST(jsonb->>'code' AS VARCHAR(1024)) AS code,
CAST(jsonb->>'encumbrance' AS UUID) AS encumbrance_id,
CAST(jsonb->>'percentage' AS DECIMAL(19,2)) AS percentage
FROM (SELECT id::text || ordinality::text AS id, id AS order_item_id, value AS jsonb FROM diku_mod_orders_storage.po_line, jsonb_array_elements((jsonb->>'fundDistribution')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.order_item_locations AS
SELECT
id AS id,
order_item_id AS order_item_id,
CAST(jsonb->>'locationId' AS UUID) AS location_id,
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
FROM (SELECT id::text || ordinality::text AS id, id AS order_item_id, value AS jsonb FROM diku_mod_orders_storage.po_line, jsonb_array_elements_text((jsonb->>'tags')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.order_items AS
SELECT
id AS id,
CAST(jsonb->>'edition' AS VARCHAR(1024)) AS edition,
CAST(jsonb->>'checkinItems' AS BOOLEAN) AS checkin_items,
CAST(jsonb->>'instanceId' AS UUID) AS instance_id,
CAST(jsonb->>'agreementId' AS UUID) AS agreement_id,
CAST(jsonb->>'acquisitionMethod' AS VARCHAR(1024)) AS acquisition_method,
CAST(jsonb->>'cancellationRestriction' AS BOOLEAN) AS cancellation_restriction,
CAST(jsonb->>'cancellationRestrictionNote' AS VARCHAR(1024)) AS cancellation_restriction_note,
CAST(jsonb->>'collection' AS BOOLEAN) AS collection,
CAST(jsonb#>>'{cost,listUnitPrice}' AS DECIMAL(19,2)) AS cost_list_unit_price,
CAST(jsonb#>>'{cost,listUnitPriceElectronic}' AS DECIMAL(19,2)) AS cost_list_unit_price_electronic,
CAST(jsonb#>>'{cost,currency}' AS VARCHAR(1024)) AS cost_currency,
CAST(jsonb#>>'{cost,additionalCost}' AS DECIMAL(19,2)) AS cost_additional_cost,
CAST(jsonb#>>'{cost,discount}' AS DECIMAL(19,2)) AS cost_discount,
CAST(jsonb#>>'{cost,discountType}' AS VARCHAR(1024)) AS cost_discount_type,
CAST(jsonb#>>'{cost,quantityPhysical}' AS INTEGER) AS cost_quantity_physical,
CAST(jsonb#>>'{cost,quantityElectronic}' AS INTEGER) AS cost_quantity_electronic,
CAST(jsonb#>>'{cost,poLineEstimatedPrice}' AS DECIMAL(19,2)) AS cost_po_line_estimated_price,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
CAST(jsonb#>>'{details,receivingNote}' AS VARCHAR(1024)) AS details_receiving_note,
uc.TIMESTAMP_CAST(jsonb#>>'{details,subscriptionFrom}') AS details_subscription_from,
CAST(jsonb#>>'{details,subscriptionInterval}' AS INTEGER) AS details_subscription_interval,
uc.TIMESTAMP_CAST(jsonb#>>'{details,subscriptionTo}') AS details_subscription_to,
CAST(jsonb->>'donor' AS VARCHAR(1024)) AS donor,
CAST(jsonb#>>'{eresource,activated}' AS BOOLEAN) AS eresource_activated,
CAST(jsonb#>>'{eresource,activationDue}' AS INTEGER) AS eresource_activation_due,
CAST(jsonb#>>'{eresource,createInventory}' AS VARCHAR(1024)) AS eresource_create_inventory,
CAST(jsonb#>>'{eresource,trial}' AS BOOLEAN) AS eresource_trial,
uc.TIMESTAMP_CAST(jsonb#>>'{eresource,expectedActivation}') AS eresource_expected_activation,
CAST(jsonb#>>'{eresource,userLimit}' AS INTEGER) AS eresource_user_limit,
CAST(jsonb#>>'{eresource,accessProvider}' AS UUID) AS eresource_access_provider_id,
CAST(jsonb#>>'{eresource,license,code}' AS VARCHAR(1024)) AS eresource_license_code,
CAST(jsonb#>>'{eresource,license,description}' AS VARCHAR(1024)) AS eresource_license_description,
CAST(jsonb#>>'{eresource,license,reference}' AS VARCHAR(1024)) AS eresource_license_reference,
CAST(jsonb#>>'{eresource,materialType}' AS UUID) AS eresource_material_type_id,
CAST(jsonb->>'orderFormat' AS VARCHAR(1024)) AS order_format,
CAST(jsonb->>'owner' AS VARCHAR(1024)) AS owner,
CAST(jsonb->>'paymentStatus' AS VARCHAR(1024)) AS payment_status,
CAST(jsonb#>>'{physical,createInventory}' AS VARCHAR(1024)) AS physical_create_inventory,
CAST(jsonb#>>'{physical,materialType}' AS UUID) AS physical_material_type_id,
CAST(jsonb#>>'{physical,materialSupplier}' AS UUID) AS physical_material_supplier_id,
uc.TIMESTAMP_CAST(jsonb#>>'{physical,expectedReceiptDate}') AS physical_expected_receipt_date,
uc.TIMESTAMP_CAST(jsonb#>>'{physical,receiptDue}') AS physical_receipt_due,
CAST(jsonb->>'poLineDescription' AS VARCHAR(1024)) AS po_line_description,
CAST(jsonb->>'poLineNumber' AS VARCHAR(1024)) AS po_line_number,
CAST(jsonb->>'publicationDate' AS VARCHAR(1024)) AS publication_year,
CAST(jsonb->>'publisher' AS VARCHAR(1024)) AS publisher,
CAST(jsonb->>'purchaseOrderId' AS UUID) AS purchase_order_id,
uc.TIMESTAMP_CAST(jsonb->>'receiptDate') AS receipt_date,
CAST(jsonb->>'receiptStatus' AS VARCHAR(1024)) AS receipt_status,
CAST(jsonb->>'requester' AS VARCHAR(1024)) AS requester,
CAST(jsonb->>'rush' AS BOOLEAN) AS rush,
CAST(jsonb->>'selector' AS VARCHAR(1024)) AS selector,
CAST(jsonb#>>'{source,code}' AS VARCHAR(1024)) AS source_code,
CAST(jsonb#>>'{source,description}' AS VARCHAR(1024)) AS source_description,
CAST(jsonb->>'title' AS VARCHAR(1024)) AS title,
CAST(jsonb#>>'{vendorDetail,instructions}' AS VARCHAR(1024)) AS vendor_detail_instructions,
CAST(jsonb#>>'{vendorDetail,noteFromVendor}' AS VARCHAR(1024)) AS vendor_detail_note_from_vendor,
CAST(jsonb#>>'{vendorDetail,refNumber}' AS VARCHAR(1024)) AS vendor_detail_ref_number,
CAST(jsonb#>>'{vendorDetail,refNumberType}' AS VARCHAR(1024)) AS vendor_detail_ref_number_type,
CAST(jsonb#>>'{vendorDetail,vendorAccount}' AS VARCHAR(1024)) AS vendor_detail_vendor_account,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.po_line;
CREATE VIEW uc.order_item_locations2 AS
SELECT
id AS id,
CAST(jsonb->>'locationId' AS UUID) AS location_id,
CAST(jsonb->>'quantity' AS INTEGER) AS quantity,
CAST(jsonb->>'quantityElectronic' AS INTEGER) AS quantity_electronic,
CAST(jsonb->>'quantityPhysical' AS INTEGER) AS quantity_physical,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.location;
CREATE VIEW uc.service_point_owners AS
SELECT
id AS id,
owner_id AS owner_id,
CAST(jsonb->>'value' AS VARCHAR(1024)) AS value,
CAST(jsonb->>'label' AS VARCHAR(1024)) AS label
FROM (SELECT id::text || ordinality::text AS id, id AS owner_id, value AS jsonb FROM diku_mod_feesfines.owners, jsonb_array_elements((jsonb->>'servicePointOwner')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.owners AS
SELECT
id AS id,
CAST(jsonb->>'owner' AS VARCHAR(1024)) AS owner,
CAST(jsonb->>'desc' AS VARCHAR(1024)) AS desc,
CAST(jsonb->>'defaultChargeNoticeId' AS UUID) AS default_charge_notice_id,
CAST(jsonb->>'defaultActionNoticeId' AS UUID) AS default_action_notice_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_feesfines.owners;
CREATE VIEW uc.patron_notice_policy_loan_notices AS
SELECT
id AS id,
patron_notice_policy_id AS patron_notice_policy_id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'templateId' AS UUID) AS template_id,
CAST(jsonb->>'templateName' AS VARCHAR(1024)) AS template_name,
CAST(jsonb->>'format' AS VARCHAR(1024)) AS format,
CAST(jsonb->>'frequency' AS VARCHAR(1024)) AS frequency,
CAST(jsonb->>'realTime' AS BOOLEAN) AS real_time,
CAST(jsonb#>>'{sendOptions,sendHow}' AS VARCHAR(1024)) AS send_options_send_how,
CAST(jsonb#>>'{sendOptions,sendWhen}' AS VARCHAR(1024)) AS send_options_send_when,
CAST(jsonb#>>'{sendOptions,sendBy,duration}' AS INTEGER) AS send_options_send_by_duration,
CAST(jsonb#>>'{sendOptions,sendBy,intervalId}' AS UUID) AS send_options_send_by_interval_id,
CAST(jsonb#>>'{sendOptions,sendEvery,duration}' AS INTEGER) AS send_options_send_every_duration,
CAST(jsonb#>>'{sendOptions,sendEvery,intervalId}' AS UUID) AS send_options_send_every_interval_id
FROM (SELECT _id::text || ordinality::text AS id, _id AS patron_notice_policy_id, value AS jsonb FROM diku_mod_circulation_storage.patron_notice_policy, jsonb_array_elements((jsonb->>'loanNotices')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.patron_notice_policy_fee_fine_notices AS
SELECT
id AS id,
patron_notice_policy_id AS patron_notice_policy_id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'templateId' AS UUID) AS template_id,
CAST(jsonb->>'templateName' AS VARCHAR(1024)) AS template_name,
CAST(jsonb->>'format' AS VARCHAR(1024)) AS format,
CAST(jsonb->>'frequency' AS VARCHAR(1024)) AS frequency,
CAST(jsonb->>'realTime' AS BOOLEAN) AS real_time,
CAST(jsonb#>>'{sendOptions,sendHow}' AS VARCHAR(1024)) AS send_options_send_how,
CAST(jsonb#>>'{sendOptions,sendBy,duration}' AS INTEGER) AS send_options_send_by_duration,
CAST(jsonb#>>'{sendOptions,sendBy,intervalId}' AS UUID) AS send_options_send_by_interval_id,
CAST(jsonb#>>'{sendOptions,sendEvery,duration}' AS INTEGER) AS send_options_send_every_duration,
CAST(jsonb#>>'{sendOptions,sendEvery,intervalId}' AS UUID) AS send_options_send_every_interval_id
FROM (SELECT _id::text || ordinality::text AS id, _id AS patron_notice_policy_id, value AS jsonb FROM diku_mod_circulation_storage.patron_notice_policy, jsonb_array_elements((jsonb->>'feeFineNotices')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.patron_notice_policy_request_notices AS
SELECT
id AS id,
patron_notice_policy_id AS patron_notice_policy_id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'templateId' AS UUID) AS template_id,
CAST(jsonb->>'templateName' AS VARCHAR(1024)) AS template_name,
CAST(jsonb->>'format' AS VARCHAR(1024)) AS format,
CAST(jsonb->>'frequency' AS VARCHAR(1024)) AS frequency,
CAST(jsonb->>'realTime' AS BOOLEAN) AS real_time,
CAST(jsonb#>>'{sendOptions,sendHow}' AS VARCHAR(1024)) AS send_options_send_how,
CAST(jsonb#>>'{sendOptions,sendWhen}' AS VARCHAR(1024)) AS send_options_send_when,
CAST(jsonb#>>'{sendOptions,sendBy,duration}' AS INTEGER) AS send_options_send_by_duration,
CAST(jsonb#>>'{sendOptions,sendBy,intervalId}' AS UUID) AS send_options_send_by_interval_id,
CAST(jsonb#>>'{sendOptions,sendEvery,duration}' AS INTEGER) AS send_options_send_every_duration,
CAST(jsonb#>>'{sendOptions,sendEvery,intervalId}' AS UUID) AS send_options_send_every_interval_id
FROM (SELECT _id::text || ordinality::text AS id, _id AS patron_notice_policy_id, value AS jsonb FROM diku_mod_circulation_storage.patron_notice_policy, jsonb_array_elements((jsonb->>'requestNotices')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.patron_notice_policies AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
CAST(jsonb->>'active' AS BOOLEAN) AS active,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_circulation_storage.patron_notice_policy;
CREATE VIEW uc.payments AS
SELECT
id AS id,
CAST(jsonb->>'nameMethod' AS VARCHAR(1024)) AS name_method,
CAST(jsonb->>'allowedRefundMethod' AS BOOLEAN) AS allowed_refund_method,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
CAST(jsonb->>'ownerId' AS UUID) AS owner_id,
jsonb_pretty(jsonb) AS content
FROM diku_mod_feesfines.payments;
CREATE VIEW uc.permission_tags AS
SELECT
id AS id,
permission_id AS permission_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS permission_id, value AS jsonb FROM diku_mod_permissions.permissions, jsonb_array_elements_text((jsonb->>'tags')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.permission_sub_permissions AS
SELECT
id AS id,
permission_id AS permission_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS permission_id, value AS jsonb FROM diku_mod_permissions.permissions, jsonb_array_elements_text((jsonb->>'subPermissions')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.permission_child_of AS
SELECT
id AS id,
permission_id AS permission_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS permission_id, value AS jsonb FROM diku_mod_permissions.permissions, jsonb_array_elements_text((jsonb->>'childOf')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.permission_granted_to AS
SELECT
id AS id,
permission_id AS permission_id,
CAST(jsonb AS UUID) AS permissions_user_id
FROM (SELECT _id::text || ordinality::text AS id, _id AS permission_id, value AS jsonb FROM diku_mod_permissions.permissions, jsonb_array_elements_text((jsonb->>'grantedTo')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.permissions AS
SELECT
_id AS id,
CAST(jsonb->>'permissionName' AS VARCHAR(1024)) AS permission_name,
CAST(jsonb->>'displayName' AS VARCHAR(1024)) AS display_name,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
CAST(jsonb->>'mutable' AS BOOLEAN) AS mutable,
CAST(jsonb->>'visible' AS BOOLEAN) AS visible,
CAST(jsonb->>'dummy' AS BOOLEAN) AS dummy,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_permissions.permissions;
CREATE VIEW uc.permissions_user_permissions AS
SELECT
id AS id,
permissions_user_id AS permissions_user_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS permissions_user_id, value AS jsonb FROM diku_mod_permissions.permissions_users, jsonb_array_elements_text((jsonb->>'permissions')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.permissions_users AS
SELECT
_id AS id,
CAST(jsonb->>'userId' AS UUID) AS user_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_permissions.permissions_users;
CREATE VIEW uc.phone_number_categories AS
SELECT
id AS id,
phone_number_id AS phone_number_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS phone_number_id, value AS jsonb FROM diku_mod_vendors.phone_number, jsonb_array_elements_text((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.phone_number_types AS
SELECT
id AS id,
phone_number_id AS phone_number_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS phone_number_id, value AS jsonb FROM diku_mod_vendors.phone_number, jsonb_array_elements_text((jsonb->>'types')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.phone_numbers AS
SELECT
id AS id,
CAST(jsonb->>'phone_number' AS VARCHAR(1024)) AS phone_number,
CAST(jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
CAST(jsonb->>'language' AS VARCHAR(1024)) AS language,
jsonb_pretty(jsonb) AS content
FROM diku_mod_vendors.phone_number;
CREATE VIEW uc.physical_volumes AS
SELECT
id AS id,
physical_id AS physical_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS physical_id, value AS jsonb FROM diku_mod_orders_storage.physical, jsonb_array_elements_text((jsonb->>'volumes')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.physicals AS
SELECT
id AS id,
CAST(jsonb->>'createInventory' AS VARCHAR(1024)) AS create_inventory,
CAST(jsonb->>'materialType' AS UUID) AS material_type_id,
CAST(jsonb->>'materialSupplier' AS UUID) AS material_supplier_id,
uc.TIMESTAMP_CAST(jsonb->>'expectedReceiptDate') AS expected_receipt_date,
uc.TIMESTAMP_CAST(jsonb->>'receiptDue') AS receipt_due,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.physical;
CREATE VIEW uc.pieces AS
SELECT
id AS id,
CAST(jsonb->>'caption' AS VARCHAR(1024)) AS caption,
CAST(jsonb->>'comment' AS VARCHAR(1024)) AS comment,
CAST(jsonb->>'format' AS VARCHAR(1024)) AS format,
CAST(jsonb->>'itemId' AS UUID) AS item_id,
CAST(jsonb->>'locationId' AS UUID) AS location_id,
CAST(jsonb->>'poLineId' AS UUID) AS po_line_id,
CAST(jsonb->>'receivingStatus' AS VARCHAR(1024)) AS receiving_status,
CAST(jsonb->>'supplement' AS BOOLEAN) AS supplement,
uc.TIMESTAMP_CAST(jsonb->>'receivedDate') AS received_date,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.pieces;
CREATE VIEW uc.proxies AS
SELECT
id AS id,
CAST(jsonb->>'userId' AS UUID) AS user_id,
CAST(jsonb->>'proxyUserId' AS UUID) AS proxy_user_id,
CAST(jsonb->>'requestForSponsor' AS VARCHAR(1024)) AS request_for_sponsor,
CAST(jsonb->>'notificationsTo' AS VARCHAR(1024)) AS notifications_to,
CAST(jsonb->>'accrueTo' AS VARCHAR(1024)) AS accrue_to,
CAST(jsonb->>'status' AS VARCHAR(1024)) AS status,
uc.TIMESTAMP_CAST(jsonb->>'expirationDate') AS expiration_date,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_users.proxyfor;
CREATE VIEW uc.refunds AS
SELECT
id AS id,
CAST(jsonb->>'nameReason' AS VARCHAR(1024)) AS name_reason,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
CAST(jsonb->>'accountId' AS UUID) AS account_id,
jsonb_pretty(jsonb) AS content
FROM diku_mod_feesfines.refunds;
CREATE VIEW uc.reporting_codes AS
SELECT
id AS id,
CAST(jsonb->>'code' AS VARCHAR(1024)) AS code,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.reporting_code;
CREATE VIEW uc.request_tags AS
SELECT
id AS id,
request_id AS request_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS request_id, value AS jsonb FROM diku_mod_circulation_storage.request, jsonb_array_elements_text((jsonb#>>'{tags,tagList}')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.requests AS
SELECT
_id AS id,
CAST(jsonb->>'requestType' AS VARCHAR(1024)) AS request_type,
uc.TIMESTAMP_CAST(jsonb->>'requestDate') AS request_date,
CAST(jsonb->>'requesterId' AS UUID) AS requester_id,
CAST(jsonb->>'proxyUserId' AS UUID) AS proxy_user_id,
CAST(jsonb->>'itemId' AS UUID) AS item_id,
CAST(jsonb->>'status' AS VARCHAR(1024)) AS status,
CAST(jsonb->>'cancellationReasonId' AS UUID) AS cancellation_reason_id,
CAST(jsonb->>'cancelledByUserId' AS UUID) AS cancelled_by_user_id,
CAST(jsonb->>'cancellationAdditionalInformation' AS VARCHAR(1024)) AS cancellation_additional_information,
uc.TIMESTAMP_CAST(jsonb->>'cancelledDate') AS cancelled_date,
CAST(jsonb->>'position' AS INTEGER) AS position,
CAST(jsonb#>>'{item,title}' AS VARCHAR(1024)) AS item_title,
CAST(jsonb#>>'{item,barcode}' AS VARCHAR(1024)) AS item_barcode,
CAST(jsonb#>>'{requester,firstName}' AS VARCHAR(1024)) AS requester_first_name,
CAST(jsonb#>>'{requester,lastName}' AS VARCHAR(1024)) AS requester_last_name,
CAST(jsonb#>>'{requester,middleName}' AS VARCHAR(1024)) AS requester_middle_name,
CAST(jsonb#>>'{requester,barcode}' AS VARCHAR(1024)) AS requester_barcode,
CAST(jsonb#>>'{requester,patronGroup}' AS VARCHAR(1024)) AS requester_patron_group,
CAST(jsonb#>>'{proxy,firstName}' AS VARCHAR(1024)) AS proxy_first_name,
CAST(jsonb#>>'{proxy,lastName}' AS VARCHAR(1024)) AS proxy_last_name,
CAST(jsonb#>>'{proxy,middleName}' AS VARCHAR(1024)) AS proxy_middle_name,
CAST(jsonb#>>'{proxy,barcode}' AS VARCHAR(1024)) AS proxy_barcode,
CAST(jsonb#>>'{proxy,patronGroup}' AS VARCHAR(1024)) AS proxy_patron_group,
CAST(jsonb->>'fulfilmentPreference' AS VARCHAR(1024)) AS fulfilment_preference,
CAST(jsonb->>'deliveryAddressTypeId' AS UUID) AS delivery_address_type_id,
uc.TIMESTAMP_CAST(jsonb->>'requestExpirationDate') AS request_expiration_date,
uc.TIMESTAMP_CAST(jsonb->>'holdShelfExpirationDate') AS hold_shelf_expiration_date,
CAST(jsonb->>'pickupServicePointId' AS UUID) AS pickup_service_point_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content,
cancellationreasonid AS cancellationreasonid
FROM diku_mod_circulation_storage.request;
CREATE VIEW uc.request_policy_request_types AS
SELECT
id AS id,
request_policy_id AS request_policy_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT _id::text || ordinality::text AS id, _id AS request_policy_id, value AS jsonb FROM diku_mod_circulation_storage.request_policy, jsonb_array_elements_text((jsonb->>'requestTypes')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.request_policies AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_circulation_storage.request_policy;
CREATE VIEW uc.service_point_staff_slips AS
SELECT
id AS id,
service_point_id AS service_point_id,
CAST(jsonb->>'id' AS UUID) AS staff_slip_id,
CAST(jsonb->>'printByDefault' AS BOOLEAN) AS print_by_default
FROM (SELECT _id::text || ordinality::text AS id, _id AS service_point_id, value AS jsonb FROM diku_mod_inventory_storage.service_point, jsonb_array_elements((jsonb->>'staffSlips')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.service_points AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'code' AS VARCHAR(1024)) AS code,
CAST(jsonb->>'discoveryDisplayName' AS VARCHAR(1024)) AS discovery_display_name,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
CAST(jsonb->>'shelvingLagTime' AS INTEGER) AS shelving_lag_time,
CAST(jsonb->>'pickupLocation' AS BOOLEAN) AS pickup_location,
CAST(jsonb#>>'{holdShelfExpiryPeriod,duration}' AS INTEGER) AS hold_shelf_expiry_period_duration,
CAST(jsonb#>>'{holdShelfExpiryPeriod,intervalId}' AS UUID) AS hold_shelf_expiry_period_interval_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.service_point;
CREATE VIEW uc.service_point_user_service_points AS
SELECT
id AS id,
service_point_user_id AS service_point_user_id,
CAST(jsonb AS UUID) AS service_point_id
FROM (SELECT _id::text || ordinality::text AS id, _id AS service_point_user_id, value AS jsonb FROM diku_mod_inventory_storage.service_point_user, jsonb_array_elements_text((jsonb->>'servicePointsIds')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.service_point_users AS
SELECT
_id AS id,
CAST(jsonb->>'userId' AS UUID) AS user_id,
CAST(jsonb->>'defaultServicePointId' AS UUID) AS default_service_point_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content,
defaultservicepointid AS defaultservicepointid
FROM diku_mod_inventory_storage.service_point_user;
CREATE VIEW uc.sources AS
SELECT
id AS id,
CAST(jsonb->>'code' AS VARCHAR(1024)) AS code,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.source;
CREATE VIEW uc.staff_slip_staff_slips AS
SELECT
id AS id,
staff_slip_id AS staff_slip_id,
CAST(jsonb->>'id' AS VARCHAR(1024)) AS id2,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
CAST(jsonb->>'active' AS BOOLEAN) AS active,
CAST(jsonb->>'template' AS VARCHAR(1024)) AS template,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username
FROM (SELECT _id::text || ordinality::text AS id, _id AS staff_slip_id, value AS jsonb FROM diku_mod_circulation_storage.staff_slips, jsonb_array_elements((jsonb->>'staffSlips')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.staff_slips AS
SELECT
_id AS id,
CAST(jsonb->>'totalRecords' AS INTEGER) AS total_records,
jsonb_pretty(jsonb) AS content
FROM diku_mod_circulation_storage.staff_slips;
CREATE VIEW uc.statistical_codes AS
SELECT
_id AS id,
CAST(jsonb->>'code' AS VARCHAR(1024)) AS code,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'statisticalCodeTypeId' AS UUID) AS statistical_code_type_id,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content,
statisticalcodetypeid AS statisticalcodetypeid
FROM diku_mod_inventory_storage.statistical_code;
CREATE VIEW uc.statistical_code_types AS
SELECT
_id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'source' AS VARCHAR(1024)) AS source,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_inventory_storage.statistical_code_type;
CREATE VIEW uc.tags AS
SELECT
_id AS id,
CAST(jsonb->>'label' AS VARCHAR(1024)) AS label,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_tags.tags;
CREATE VIEW uc.transactions AS
SELECT
id AS id,
CAST(jsonb->>'allocated' AS DECIMAL(19,2)) AS allocated,
CAST(jsonb->>'amount' AS DECIMAL(19,2)) AS amount,
CAST(jsonb->>'available' AS DECIMAL(19,2)) AS available,
CAST(jsonb->>'awaiting_payment' AS DECIMAL(19,2)) AS awaiting_payment,
CAST(jsonb->>'encumbered' AS DECIMAL(19,2)) AS encumbered,
CAST(jsonb->>'expenditures' AS DECIMAL(19,2)) AS expenditures,
CAST(jsonb->>'note' AS VARCHAR(1024)) AS note,
CAST(jsonb->>'overcharge' AS DECIMAL(19,2)) AS overcharge,
uc.TIMESTAMP_CAST(jsonb->>'timestamp') AS timestamp,
CAST(jsonb->>'source_id' AS UUID) AS source_id,
CAST(jsonb->>'transaction_type' AS VARCHAR(1024)) AS transaction_type,
CAST(jsonb->>'budget_id' AS UUID) AS budget_id2,
jsonb_pretty(jsonb) AS content,
budget_id AS budget_id
FROM diku_mod_finance_storage.transaction;
CREATE VIEW uc.transfers AS
SELECT
id AS id,
CAST(jsonb->>'accountName' AS VARCHAR(1024)) AS account_name,
CAST(jsonb->>'desc' AS VARCHAR(1024)) AS desc,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
CAST(jsonb->>'ownerId' AS UUID) AS owner_id,
jsonb_pretty(jsonb) AS content
FROM diku_mod_feesfines.transfers;
CREATE VIEW uc.transfer_criterias AS
SELECT
id AS id,
CAST(jsonb->>'criteria' AS VARCHAR(1024)) AS criteria,
CAST(jsonb->>'type' AS VARCHAR(1024)) AS type,
CAST(jsonb->>'value' AS DECIMAL(19,2)) AS value,
CAST(jsonb->>'interval' AS VARCHAR(1024)) AS interval,
jsonb_pretty(jsonb) AS content
FROM diku_mod_feesfines.transfer_criteria;
CREATE VIEW uc.url_categories AS
SELECT
id AS id,
url_id AS url_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS url_id, value AS jsonb FROM diku_mod_vendors.url, jsonb_array_elements_text((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.urls AS
SELECT
id AS id,
CAST(jsonb->>'value' AS VARCHAR(1024)) AS value,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
CAST(jsonb->>'language' AS VARCHAR(1024)) AS language,
CAST(jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
CAST(jsonb->>'notes' AS VARCHAR(1024)) AS notes,
jsonb_pretty(jsonb) AS content
FROM diku_mod_vendors.url;
CREATE VIEW uc.user_addresses AS
SELECT
id AS id,
user_id AS user_id,
CAST(jsonb->>'id' AS UUID) AS id2,
CAST(jsonb->>'countryId' AS VARCHAR(1024)) AS country_id,
CAST(jsonb->>'addressLine1' AS VARCHAR(1024)) AS address_line1,
CAST(jsonb->>'addressLine2' AS VARCHAR(1024)) AS address_line2,
CAST(jsonb->>'city' AS VARCHAR(1024)) AS city,
CAST(jsonb->>'region' AS VARCHAR(1024)) AS region,
CAST(jsonb->>'postalCode' AS VARCHAR(1024)) AS postal_code,
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
CAST(jsonb->>'username' AS VARCHAR(1024)) AS username,
CAST(jsonb->>'externalSystemId' AS VARCHAR(1024)) AS external_system_id,
CAST(jsonb->>'barcode' AS VARCHAR(1024)) AS barcode,
CAST(jsonb->>'active' AS BOOLEAN) AS active,
CAST(jsonb->>'type' AS VARCHAR(1024)) AS type,
CAST(jsonb->>'patronGroup' AS UUID) AS patron_group_id,
CAST(CONCAT_WS(' ', jsonb#>>'{personal,firstName}', jsonb#>>'{personal,middleName}', jsonb#>>'{personal,lastName}') AS VARCHAR(1024)) AS name,
CAST(jsonb#>>'{personal,lastName}' AS VARCHAR(1024)) AS last_name,
CAST(jsonb#>>'{personal,firstName}' AS VARCHAR(1024)) AS first_name,
CAST(jsonb#>>'{personal,middleName}' AS VARCHAR(1024)) AS middle_name,
CAST(jsonb#>>'{personal,email}' AS VARCHAR(1024)) AS email,
CAST(jsonb#>>'{personal,phone}' AS VARCHAR(1024)) AS phone,
CAST(jsonb#>>'{personal,mobilePhone}' AS VARCHAR(1024)) AS mobile_phone,
uc.TIMESTAMP_CAST(jsonb#>>'{personal,dateOfBirth}') AS date_of_birth,
CAST(jsonb#>>'{personal,preferredContactTypeId}' AS VARCHAR(1024)) AS preferred_contact_type_id,
uc.TIMESTAMP_CAST(jsonb->>'enrollmentDate') AS enrollment_date,
uc.TIMESTAMP_CAST(jsonb->>'expirationDate') AS expiration_date,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
jsonb_pretty(jsonb) AS content
FROM diku_mod_users.users;
CREATE VIEW uc.vendor_aliases AS
SELECT
id AS id,
vendor_id AS vendor_id,
CAST(jsonb->>'id' AS UUID) AS id2,
CAST(jsonb->>'value' AS VARCHAR(1024)) AS value,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description
FROM (SELECT id::text || ordinality::text AS id, id AS vendor_id, value AS jsonb FROM diku_mod_vendors.vendor, jsonb_array_elements((jsonb->>'aliases')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.vendor_address_categories AS
SELECT
id AS id,
vendor_address_id AS vendor_address_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS vendor_address_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS vendor_id, value AS jsonb FROM diku_mod_vendors.vendor, jsonb_array_elements((jsonb->>'addresses')::jsonb) WITH ORDINALITY) a, jsonb_array_elements_text((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.vendor_addresses AS
SELECT
id AS id,
vendor_id AS vendor_id,
CAST(jsonb->>'id' AS UUID) AS id2,
CAST(jsonb->>'addressLine1' AS VARCHAR(1024)) AS line1,
CAST(jsonb->>'addressLine2' AS VARCHAR(1024)) AS line2,
CAST(jsonb->>'city' AS VARCHAR(1024)) AS city,
CAST(jsonb->>'stateRegion' AS VARCHAR(1024)) AS state_region,
CAST(jsonb->>'zipCode' AS VARCHAR(1024)) AS zip_code,
CAST(jsonb->>'country' AS VARCHAR(1024)) AS country,
CAST(jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
CAST(jsonb->>'language' AS VARCHAR(1024)) AS language
FROM (SELECT id::text || ordinality::text AS id, id AS vendor_id, value AS jsonb FROM diku_mod_vendors.vendor, jsonb_array_elements((jsonb->>'addresses')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.vendor_phone_number_categories AS
SELECT
id AS id,
vendor_phone_number_id AS vendor_phone_number_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS vendor_phone_number_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS vendor_id, value AS jsonb FROM diku_mod_vendors.vendor, jsonb_array_elements((jsonb->>'phone_numbers')::jsonb) WITH ORDINALITY) a, jsonb_array_elements_text((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.vendor_phone_number_types AS
SELECT
id AS id,
vendor_phone_number_id AS vendor_phone_number_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS vendor_phone_number_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS vendor_id, value AS jsonb FROM diku_mod_vendors.vendor, jsonb_array_elements((jsonb->>'phone_numbers')::jsonb) WITH ORDINALITY) a, jsonb_array_elements_text((jsonb->>'types')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.vendor_phone_numbers AS
SELECT
id AS id,
vendor_id AS vendor_id,
CAST(jsonb->>'id' AS UUID) AS id2,
CAST(jsonb->>'phone_number' AS VARCHAR(1024)) AS phone_number,
CAST(jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
CAST(jsonb->>'language' AS VARCHAR(1024)) AS language
FROM (SELECT id::text || ordinality::text AS id, id AS vendor_id, value AS jsonb FROM diku_mod_vendors.vendor, jsonb_array_elements((jsonb->>'phone_numbers')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.email_address_categories AS
SELECT
id AS id,
vendor_email_id AS vendor_email_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS vendor_email_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS vendor_id, value AS jsonb FROM diku_mod_vendors.vendor, jsonb_array_elements((jsonb->>'emails')::jsonb) WITH ORDINALITY) a, jsonb_array_elements_text((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.vendor_emails AS
SELECT
id AS id,
vendor_id AS vendor_id,
CAST(jsonb->>'id' AS UUID) AS id2,
CAST(jsonb->>'value' AS VARCHAR(1024)) AS value,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
CAST(jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
CAST(jsonb->>'language' AS VARCHAR(1024)) AS language
FROM (SELECT id::text || ordinality::text AS id, id AS vendor_id, value AS jsonb FROM diku_mod_vendors.vendor, jsonb_array_elements((jsonb->>'emails')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.vendor_url_categories AS
SELECT
id AS id,
vendor_url_id AS vendor_url_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS vendor_url_id, value AS jsonb FROM (SELECT id::text || ordinality::text AS id, id AS vendor_id, value AS jsonb FROM diku_mod_vendors.vendor, jsonb_array_elements((jsonb->>'urls')::jsonb) WITH ORDINALITY) a, jsonb_array_elements_text((jsonb->>'categories')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.vendor_urls AS
SELECT
id AS id,
vendor_id AS vendor_id,
CAST(jsonb->>'id' AS UUID) AS id2,
CAST(jsonb->>'value' AS VARCHAR(1024)) AS value,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
CAST(jsonb->>'language' AS VARCHAR(1024)) AS language,
CAST(jsonb->>'isPrimary' AS BOOLEAN) AS is_primary,
CAST(jsonb->>'notes' AS VARCHAR(1024)) AS notes
FROM (SELECT id::text || ordinality::text AS id, id AS vendor_id, value AS jsonb FROM diku_mod_vendors.vendor, jsonb_array_elements((jsonb->>'urls')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.vendor_contacts AS
SELECT
id AS id,
vendor_id AS vendor_id,
CAST(jsonb AS UUID) AS contact_id
FROM (SELECT id::text || ordinality::text AS id, id AS vendor_id, value AS jsonb FROM diku_mod_vendors.vendor, jsonb_array_elements_text((jsonb->>'contacts')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.vendor_agreements AS
SELECT
id AS id,
vendor_id AS vendor_id,
CAST(jsonb->>'id' AS UUID) AS id2,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'discount' AS DECIMAL(19,2)) AS discount,
CAST(jsonb->>'reference_url' AS VARCHAR(1024)) AS reference_url,
CAST(jsonb->>'notes' AS VARCHAR(1024)) AS notes
FROM (SELECT id::text || ordinality::text AS id, id AS vendor_id, value AS jsonb FROM diku_mod_vendors.vendor, jsonb_array_elements((jsonb->>'agreements')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.currencies AS
SELECT
id AS id,
vendor_id AS vendor_id,
CAST(jsonb AS VARCHAR(1024)) AS content
FROM (SELECT id::text || ordinality::text AS id, id AS vendor_id, value AS jsonb FROM diku_mod_vendors.vendor, jsonb_array_elements_text((jsonb->>'vendor_currencies')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.vendor_interfaces AS
SELECT
id AS id,
vendor_id AS vendor_id,
CAST(jsonb->>'id' AS UUID) AS id2,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'uri' AS VARCHAR(1024)) AS uri,
CAST(jsonb->>'username' AS VARCHAR(1024)) AS username,
CAST(jsonb->>'password' AS VARCHAR(1024)) AS password,
CAST(jsonb->>'notes' AS VARCHAR(1024)) AS notes,
CAST(jsonb->>'available' AS BOOLEAN) AS available,
CAST(jsonb->>'delivery_method' AS VARCHAR(1024)) AS delivery_method,
CAST(jsonb->>'statistics_format' AS VARCHAR(1024)) AS statistics_format,
CAST(jsonb->>'locally_stored' AS VARCHAR(1024)) AS locally_stored,
CAST(jsonb->>'online_location' AS VARCHAR(1024)) AS online_location,
CAST(jsonb->>'statistics_notes' AS VARCHAR(1024)) AS statistics_notes
FROM (SELECT id::text || ordinality::text AS id, id AS vendor_id, value AS jsonb FROM diku_mod_vendors.vendor, jsonb_array_elements((jsonb->>'interfaces')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.vendor_accounts AS
SELECT
id AS id,
vendor_id AS vendor_id,
CAST(jsonb->>'id' AS UUID) AS id2,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'account_no' AS VARCHAR(1024)) AS account_no,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
CAST(jsonb->>'app_system_no' AS VARCHAR(1024)) AS app_system_no,
CAST(jsonb->>'payment_method' AS VARCHAR(1024)) AS payment_method,
CAST(jsonb->>'account_status' AS VARCHAR(1024)) AS account_status,
CAST(jsonb->>'contact_info' AS VARCHAR(1024)) AS contact_info,
CAST(jsonb->>'library_code' AS VARCHAR(1024)) AS library_code,
CAST(jsonb->>'library_edi_code' AS VARCHAR(1024)) AS library_edi_code,
CAST(jsonb->>'notes' AS VARCHAR(1024)) AS notes
FROM (SELECT id::text || ordinality::text AS id, id AS vendor_id, value AS jsonb FROM diku_mod_vendors.vendor, jsonb_array_elements((jsonb->>'accounts')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.vendor_vendor_types AS
SELECT
id AS id,
vendor_id AS vendor_id,
CAST(jsonb->>'id' AS UUID) AS id2,
CAST(jsonb->>'value' AS VARCHAR(1024)) AS value,
CAST(jsonb->>'locked' AS BOOLEAN) AS locked
FROM (SELECT id::text || ordinality::text AS id, id AS vendor_id, value AS jsonb FROM diku_mod_vendors.vendor, jsonb_array_elements((jsonb->>'vendor_types')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.vendor_changelogs AS
SELECT
id AS id,
vendor_id AS vendor_id,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
uc.TIMESTAMP_CAST(jsonb->>'timestamp') AS timestamp
FROM (SELECT id::text || ordinality::text AS id, id AS vendor_id, value AS jsonb FROM diku_mod_vendors.vendor, jsonb_array_elements((jsonb->>'changelogs')::jsonb) WITH ORDINALITY) a;
CREATE VIEW uc.vendors AS
SELECT
id AS id,
CAST(jsonb->>'name' AS VARCHAR(1024)) AS name,
CAST(jsonb->>'code' AS VARCHAR(1024)) AS code,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
CAST(jsonb->>'vendor_status' AS VARCHAR(1024)) AS vendor_status,
CAST(jsonb->>'language' AS VARCHAR(1024)) AS language,
CAST(jsonb->>'erp_code' AS VARCHAR(1024)) AS erp_code,
CAST(jsonb->>'payment_method' AS VARCHAR(1024)) AS payment_method,
CAST(jsonb->>'access_provider' AS BOOLEAN) AS access_provider,
CAST(jsonb->>'governmental' AS BOOLEAN) AS governmental,
CAST(jsonb->>'licensor' AS BOOLEAN) AS licensor,
CAST(jsonb->>'material_supplier' AS BOOLEAN) AS material_supplier,
CAST(jsonb->>'claiming_interval' AS INTEGER) AS claiming_interval,
CAST(jsonb->>'discount_percent' AS DECIMAL(19,2)) AS discount_percent,
CAST(jsonb->>'expected_activation_interval' AS INTEGER) AS expected_activation_interval,
CAST(jsonb->>'expected_invoice_interval' AS INTEGER) AS expected_invoice_interval,
CAST(jsonb->>'renewal_activation_interval' AS INTEGER) AS renewal_activation_interval,
CAST(jsonb->>'subscription_interval' AS INTEGER) AS subscription_interval,
CAST(jsonb->>'expected_receipt_interval' AS INTEGER) AS expected_receipt_interval,
CAST(jsonb->>'tax_id' AS VARCHAR(1024)) AS tax_id,
CAST(jsonb->>'liable_for_vat' AS BOOLEAN) AS liable_for_vat,
CAST(jsonb->>'tax_percentage' AS DECIMAL(19,2)) AS tax_percentage,
CAST(jsonb#>>'{edi,vendor_edi_code}' AS VARCHAR(1024)) AS edi_vendor_edi_code,
CAST(jsonb#>>'{edi,vendor_edi_type}' AS VARCHAR(1024)) AS edi_vendor_edi_type,
CAST(jsonb#>>'{edi,lib_edi_code}' AS VARCHAR(1024)) AS edi_lib_edi_code,
CAST(jsonb#>>'{edi,lib_edi_type}' AS VARCHAR(1024)) AS edi_lib_edi_type,
CAST(jsonb#>>'{edi,prorate_tax}' AS BOOLEAN) AS edi_prorate_tax,
CAST(jsonb#>>'{edi,prorate_fees}' AS BOOLEAN) AS edi_prorate_fees,
CAST(jsonb#>>'{edi,edi_naming_convention}' AS VARCHAR(1024)) AS edi_naming_convention,
CAST(jsonb#>>'{edi,send_acct_num}' AS BOOLEAN) AS edi_send_acct_num,
CAST(jsonb#>>'{edi,support_order}' AS BOOLEAN) AS edi_support_order,
CAST(jsonb#>>'{edi,support_invoice}' AS BOOLEAN) AS edi_support_invoice,
CAST(jsonb#>>'{edi,notes}' AS VARCHAR(1024)) AS edi_notes,
CAST(jsonb#>>'{edi,edi_ftp,ftp_format}' AS VARCHAR(1024)) AS edi_ftp_ftp_format,
CAST(jsonb#>>'{edi,edi_ftp,server_address}' AS VARCHAR(1024)) AS edi_ftp_server_address,
CAST(jsonb#>>'{edi,edi_ftp,username}' AS VARCHAR(1024)) AS edi_ftp_username,
CAST(jsonb#>>'{edi,edi_ftp,password}' AS VARCHAR(1024)) AS edi_ftp_password,
CAST(jsonb#>>'{edi,edi_ftp,ftp_mode}' AS VARCHAR(1024)) AS edi_ftp_ftp_mode,
CAST(jsonb#>>'{edi,edi_ftp,ftp_conn_mode}' AS VARCHAR(1024)) AS edi_ftp_ftp_conn_mode,
CAST(jsonb#>>'{edi,edi_ftp,ftp_port}' AS INTEGER) AS edi_ftp_ftp_port,
CAST(jsonb#>>'{edi,edi_ftp,order_directory}' AS VARCHAR(1024)) AS edi_ftp_order_directory,
CAST(jsonb#>>'{edi,edi_ftp,invoice_directory}' AS VARCHAR(1024)) AS edi_ftp_invoice_directory,
CAST(jsonb#>>'{edi,edi_ftp,notes}' AS VARCHAR(1024)) AS edi_ftp_notes,
CAST(jsonb#>>'{edi,edi_job,schedule_edi}' AS BOOLEAN) AS edi_job_schedule_edi,
uc.TIMESTAMP_CAST(jsonb#>>'{edi,edi_job,scheduling_date}') AS edi_job_scheduling_date,
CAST(jsonb#>>'{edi,edi_job,time}' AS VARCHAR(1024)) AS edi_job_time,
CAST(jsonb#>>'{edi,edi_job,is_monday}' AS BOOLEAN) AS edi_job_is_monday,
CAST(jsonb#>>'{edi,edi_job,is_tuesday}' AS BOOLEAN) AS edi_job_is_tuesday,
CAST(jsonb#>>'{edi,edi_job,is_wednesday}' AS BOOLEAN) AS edi_job_is_wednesday,
CAST(jsonb#>>'{edi,edi_job,is_thursday}' AS BOOLEAN) AS edi_job_is_thursday,
CAST(jsonb#>>'{edi,edi_job,is_friday}' AS BOOLEAN) AS edi_job_is_friday,
CAST(jsonb#>>'{edi,edi_job,is_saturday}' AS BOOLEAN) AS edi_job_is_saturday,
CAST(jsonb#>>'{edi,edi_job,is_sunday}' AS BOOLEAN) AS edi_job_is_sunday,
CAST(jsonb#>>'{edi,edi_job,send_to_emails}' AS VARCHAR(1024)) AS edi_job_send_to_emails,
CAST(jsonb#>>'{edi,edi_job,notify_all_edi}' AS BOOLEAN) AS edi_job_notify_all_edi,
CAST(jsonb#>>'{edi,edi_job,notify_invoice_only}' AS BOOLEAN) AS edi_job_notify_invoice_only,
CAST(jsonb#>>'{edi,edi_job,notify_error_only}' AS BOOLEAN) AS edi_job_notify_error_only,
CAST(jsonb#>>'{edi,edi_job,scheduling_notes}' AS VARCHAR(1024)) AS edi_job_scheduling_notes,
CAST(jsonb->>'san_code' AS VARCHAR(1024)) AS san_code,
jsonb_pretty(jsonb) AS content
FROM diku_mod_vendors.vendor;
CREATE VIEW uc.vendor_details AS
SELECT
id AS id,
CAST(jsonb->>'instructions' AS VARCHAR(1024)) AS instructions,
CAST(jsonb->>'noteFromVendor' AS VARCHAR(1024)) AS note_from_vendor,
CAST(jsonb->>'refNumber' AS VARCHAR(1024)) AS ref_number,
CAST(jsonb->>'refNumberType' AS VARCHAR(1024)) AS ref_number_type,
CAST(jsonb->>'vendorAccount' AS VARCHAR(1024)) AS vendor_account,
jsonb_pretty(jsonb) AS content
FROM diku_mod_orders_storage.vendor_detail;
CREATE VIEW uc.vendor_types AS
SELECT
id AS id,
CAST(jsonb->>'value' AS VARCHAR(1024)) AS value,
CAST(jsonb->>'locked' AS BOOLEAN) AS locked,
jsonb_pretty(jsonb) AS content
FROM diku_mod_vendors.vendor_type;
CREATE VIEW uc.waives AS
SELECT
id AS id,
CAST(jsonb->>'nameReason' AS VARCHAR(1024)) AS name_reason,
CAST(jsonb->>'description' AS VARCHAR(1024)) AS description,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,createdDate}') AS created_date,
CAST(jsonb#>>'{metadata,createdByUserId}' AS UUID) AS created_by_user_id,
CAST(jsonb#>>'{metadata,createdByUsername}' AS VARCHAR(1024)) AS created_by_username,
uc.TIMESTAMP_CAST(jsonb#>>'{metadata,updatedDate}') AS updated_date,
CAST(jsonb#>>'{metadata,updatedByUserId}' AS UUID) AS updated_by_user_id,
CAST(jsonb#>>'{metadata,updatedByUsername}' AS VARCHAR(1024)) AS updated_by_username,
CAST(jsonb->>'accountId' AS UUID) AS account_id,
jsonb_pretty(jsonb) AS content
FROM diku_mod_feesfines.waives;
