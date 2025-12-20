-- ============================================================================
-- Pre-Clear Database Rebuild Script
-- Purpose: Drop old database completely and recreate with clean schema
-- Date: December 16, 2025
-- ============================================================================

-- Drop existing database completely
DROP DATABASE IF EXISTS preclear;

-- Create fresh database
CREATE DATABASE preclear CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE preclear;

-- ============================================================================
-- TABLE 1: users (Master user table for all roles)
-- ============================================================================
CREATE TABLE users (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  
  -- Authentication
  email VARCHAR(255) NOT NULL UNIQUE,
  password_hash VARCHAR(255) NOT NULL,
  
  -- Basic Info
  first_name VARCHAR(150) NOT NULL,
  last_name VARCHAR(150) NOT NULL,
  role ENUM('shipper', 'broker', 'admin') NOT NULL DEFAULT 'shipper',
  
  -- Contact (basic)
  phone VARCHAR(50),
  company VARCHAR(255),
  
  -- Account Status
  tos_accepted BOOLEAN NOT NULL DEFAULT FALSE,
  tos_accepted_at TIMESTAMP NULL,
  email_verified BOOLEAN NOT NULL DEFAULT FALSE,
  verification_token VARCHAR(255),
  is_active BOOLEAN NOT NULL DEFAULT TRUE,
  
  -- Metadata
  metadata JSON,
  
  -- Timestamps
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  
  INDEX idx_email (email),
  INDEX idx_role (role),
  INDEX idx_active (is_active)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ============================================================================
-- TABLE 2: shipper_profiles (Extended profile for shippers)
-- ============================================================================
CREATE TABLE shipper_profiles (
  user_id BIGINT PRIMARY KEY,
  
  -- Address
  address_line_1 VARCHAR(255),
  address_line_2 VARCHAR(255),
  city VARCHAR(100),
  state VARCHAR(100),
  postal_code VARCHAR(20),
  country_code VARCHAR(3) NOT NULL DEFAULT 'US',
  
  -- Preferences
  timezone VARCHAR(50) DEFAULT 'America/New_York',
  language VARCHAR(10) DEFAULT 'en',
  
  -- Company Details
  company_role VARCHAR(100),
  
  -- Timestamps
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  
  FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ============================================================================
-- TABLE 3: broker_profiles (Extended profile for brokers)
-- ============================================================================
CREATE TABLE broker_profiles (
  user_id BIGINT PRIMARY KEY,
  
  -- License & Experience
  license_number VARCHAR(100),
  years_of_experience INT,
  
  -- Assignment Rules (JSON arrays for flexibility)
  origin_countries JSON,
  destination_countries JSON,
  hs_categories JSON,
  
  -- Preferences
  timezone VARCHAR(50) DEFAULT 'America/New_York',
  language VARCHAR(10) DEFAULT 'en',
  
  -- Status
  is_available BOOLEAN NOT NULL DEFAULT TRUE,
  max_concurrent_shipments INT DEFAULT 10,
  
  -- Timestamps
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  
  FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ============================================================================
-- TABLE 4: shipments (Core shipment entity with mandatory ownership)
-- ============================================================================
CREATE TABLE shipments (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  reference_id VARCHAR(120) NOT NULL UNIQUE,
  
  -- Ownership (MANDATORY - NO NULLS)
  created_by BIGINT NOT NULL,
  assigned_broker_id BIGINT NULL,
  
  -- Basic Info
  shipment_name VARCHAR(255),
  mode ENUM('Air', 'Sea', 'Ground') NOT NULL DEFAULT 'Ground',
  shipment_type ENUM('Domestic', 'International') NOT NULL DEFAULT 'International',
  
  -- Service Details
  carrier VARCHAR(100),
  service_level VARCHAR(50) DEFAULT 'Standard',
  incoterm VARCHAR(20),
  bill_to VARCHAR(20) DEFAULT 'Shipper',
  
  -- Financial
  currency VARCHAR(3) DEFAULT 'USD',
  customs_value DECIMAL(18,2),
  insurance_required BOOLEAN DEFAULT FALSE,
  
  -- Status Tracking
  status VARCHAR(50) NOT NULL DEFAULT 'draft',
  ai_approval_status ENUM('not-started', 'pending', 'approved', 'rejected') DEFAULT 'not-started',
  ai_compliance_score DECIMAL(5,2),
  broker_approval_status ENUM('not-started', 'pending', 'approved', 'rejected', 'documents-requested') DEFAULT 'not-started',
  
  -- Token (generated after full approval)
  preclear_token VARCHAR(50),
  token_generated_at TIMESTAMP NULL,
  
  -- Timestamps
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  
  FOREIGN KEY (created_by) REFERENCES users(id) ON DELETE RESTRICT,
  FOREIGN KEY (assigned_broker_id) REFERENCES users(id) ON DELETE SET NULL,
  
  INDEX idx_created_by (created_by),
  INDEX idx_broker (assigned_broker_id),
  INDEX idx_status (status),
  INDEX idx_reference (reference_id),
  INDEX idx_ai_status (ai_approval_status),
  INDEX idx_broker_status (broker_approval_status)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ============================================================================
-- TABLE 5: shipment_parties (Shipper & Consignee per shipment)
-- ============================================================================
CREATE TABLE shipment_parties (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  shipment_id BIGINT NOT NULL,
  
  party_type ENUM('shipper', 'consignee', 'third_party') NOT NULL,
  
  -- Contact
  company_name VARCHAR(255) NOT NULL,
  contact_name VARCHAR(150),
  phone VARCHAR(50),
  email VARCHAR(255),
  
  -- Address
  address_1 VARCHAR(255),
  address_2 VARCHAR(255),
  city VARCHAR(100),
  state VARCHAR(100),
  postal_code VARCHAR(50),
  country VARCHAR(100),
  tax_id VARCHAR(100),
  
  FOREIGN KEY (shipment_id) REFERENCES shipments(id) ON DELETE CASCADE,
  INDEX idx_shipment (shipment_id),
  INDEX idx_party_type (party_type)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ============================================================================
-- TABLE 6: shipment_packages (Package details)
-- ============================================================================
CREATE TABLE shipment_packages (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  shipment_id BIGINT NOT NULL,
  
  package_type ENUM('Box', 'Envelope', 'Pallet', 'Other') DEFAULT 'Box',
  
  -- Dimensions
  length DECIMAL(10,3),
  width DECIMAL(10,3),
  height DECIMAL(10,3),
  dimension_unit ENUM('cm', 'in') DEFAULT 'cm',
  
  -- Weight
  weight DECIMAL(10,3),
  weight_unit ENUM('kg', 'lb') DEFAULT 'kg',
  
  stackable BOOLEAN DEFAULT FALSE,
  
  FOREIGN KEY (shipment_id) REFERENCES shipments(id) ON DELETE CASCADE,
  INDEX idx_shipment (shipment_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ============================================================================
-- TABLE 7: shipment_products (Products within packages)
-- ============================================================================
CREATE TABLE shipment_products (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  package_id BIGINT NOT NULL,
  shipment_id BIGINT NOT NULL,
  
  -- Product Info
  name VARCHAR(255) NOT NULL,
  description TEXT,
  category VARCHAR(100),
  
  -- HS Classification
  hs_code VARCHAR(20),
  
  -- Quantity & Value
  quantity DECIMAL(18,3) DEFAULT 1,
  unit VARCHAR(20) DEFAULT 'pcs',
  unit_price DECIMAL(18,4),
  total_value DECIMAL(18,4),
  
  -- Origin
  origin_country VARCHAR(3),
  export_reason VARCHAR(50) DEFAULT 'Sale',
  
  FOREIGN KEY (package_id) REFERENCES shipment_packages(id) ON DELETE CASCADE,
  FOREIGN KEY (shipment_id) REFERENCES shipments(id) ON DELETE CASCADE,
  INDEX idx_package (package_id),
  INDEX idx_shipment (shipment_id),
  INDEX idx_hs_code (hs_code)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ============================================================================
-- TABLE 8: shipment_documents (Document tracking)
-- ============================================================================
CREATE TABLE shipment_documents (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  shipment_id BIGINT NOT NULL,
  
  document_type VARCHAR(100) NOT NULL,
  file_name VARCHAR(255) NOT NULL,
  file_path VARCHAR(500),
  file_size BIGINT,
  mime_type VARCHAR(100),
  
  -- Validation (AI validator results)
  validation_status ENUM('not-validated', 'pass', 'warning', 'fail'),
  validation_confidence DECIMAL(5,2),
  validation_notes JSON,
  
  uploaded_by BIGINT,
  uploaded_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  
  FOREIGN KEY (shipment_id) REFERENCES shipments(id) ON DELETE CASCADE,
  FOREIGN KEY (uploaded_by) REFERENCES users(id) ON DELETE SET NULL,
  INDEX idx_shipment (shipment_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ============================================================================
-- TABLE 9: shipment_compliance (AI evaluation results)
-- ============================================================================
CREATE TABLE shipment_compliance (
  shipment_id BIGINT PRIMARY KEY,
  
  -- AI Scores
  overall_score DECIMAL(5,2),
  risk_level ENUM('low', 'medium', 'high'),
  
  -- Flags
  dangerous_goods BOOLEAN DEFAULT FALSE,
  lithium_battery BOOLEAN DEFAULT FALSE,
  food_pharma_flag BOOLEAN DEFAULT FALSE,
  export_license_required BOOLEAN DEFAULT FALSE,
  
  -- AI Suggestions
  suggested_hs_code VARCHAR(20),
  estimated_duty DECIMAL(18,2),
  estimated_tax DECIMAL(18,2),
  
  -- Required Documents (JSON arrays)
  required_documents JSON,
  missing_documents JSON,
  
  -- Timestamps
  evaluated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  
  FOREIGN KEY (shipment_id) REFERENCES shipments(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ============================================================================
-- TABLE 10: broker_reviews (Broker actions on shipments)
-- ============================================================================
CREATE TABLE broker_reviews (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  shipment_id BIGINT NOT NULL,
  broker_id BIGINT NOT NULL,
  
  action ENUM('approved', 'rejected', 'documents-requested') NOT NULL,
  comments TEXT,
  requested_documents JSON,
  
  reviewed_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  
  FOREIGN KEY (shipment_id) REFERENCES shipments(id) ON DELETE CASCADE,
  FOREIGN KEY (broker_id) REFERENCES users(id) ON DELETE CASCADE,
  INDEX idx_shipment (shipment_id),
  INDEX idx_broker (broker_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ============================================================================
-- TABLE 11: notifications (User notifications)
-- ============================================================================
CREATE TABLE notifications (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  user_id BIGINT NOT NULL,
  
  type VARCHAR(100) NOT NULL,
  title VARCHAR(255) NOT NULL,
  message TEXT NOT NULL,
  
  shipment_id BIGINT NULL,
  
  is_read BOOLEAN DEFAULT FALSE,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  
  FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
  FOREIGN KEY (shipment_id) REFERENCES shipments(id) ON DELETE SET NULL,
  INDEX idx_user (user_id),
  INDEX idx_read (is_read)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ============================================================================
-- TABLE 12: shipment_messages (Chat/communication)
-- ============================================================================
CREATE TABLE shipment_messages (
  id BIGINT PRIMARY KEY AUTO_INCREMENT,
  shipment_id BIGINT NOT NULL,
  sender_id BIGINT NOT NULL,
  
  message TEXT NOT NULL,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  
  FOREIGN KEY (shipment_id) REFERENCES shipments(id) ON DELETE CASCADE,
  FOREIGN KEY (sender_id) REFERENCES users(id) ON DELETE CASCADE,
  INDEX idx_shipment (shipment_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ============================================================================
-- SEED DATA: Create demo users for testing
-- ============================================================================

-- Shipper user
INSERT INTO users (first_name, last_name, email, password_hash, role, phone, company, tos_accepted, tos_accepted_at, email_verified, is_active)
VALUES ('Demo', 'Shipper', 'shipper@demo.com', 'DWGze75ZnVYT73dNpDHu0w==:tPTvEK3WEF4Uu37TwOU6hR4xA848lI7ixexDm7sCbyg=', 'shipper', '+1-555-0100', 'Global Trade Inc', TRUE, NOW(), TRUE, TRUE);

SET @shipper_id = LAST_INSERT_ID();

INSERT INTO shipper_profiles (user_id, address_line_1, address_line_2, city, state, postal_code, country_code, timezone, language, company_role)
VALUES (@shipper_id, '123 Export Street', 'Suite 400', 'New York', 'NY', '10001', 'US', 'America/New_York', 'en', 'Export Manager');

-- Broker user
INSERT INTO users (first_name, last_name, email, password_hash, role, phone, company, tos_accepted, tos_accepted_at, email_verified, is_active)
VALUES ('John', 'Broker', 'broker@demo.com', 'DWGze75ZnVYT73dNpDHu0w==:tPTvEK3WEF4Uu37TwOU6hR4xA848lI7ixexDm7sCbyg=', 'broker', '+1-415-555-0199', 'Global Customs Brokers LLC', TRUE, NOW(), TRUE, TRUE);

SET @broker_id = LAST_INSERT_ID();

INSERT INTO broker_profiles (user_id, license_number, years_of_experience, origin_countries, destination_countries, hs_categories, timezone, language, is_available, max_concurrent_shipments)
VALUES (@broker_id, 'CB-12345-US', 10, 
  JSON_ARRAY('US', 'CA', 'MX'), 
  JSON_ARRAY('CN', 'IN', 'JP', 'KR', 'TW'), 
  JSON_ARRAY('84', '85', '90', '94', '95'), 
  'America/New_York', 'en', TRUE, 15);

-- Admin user
INSERT INTO users (first_name, last_name, email, password_hash, role, phone, company, tos_accepted, tos_accepted_at, email_verified, is_active)
VALUES ('System', 'Admin', 'admin@demo.com', 'DWGze75ZnVYT73dNpDHu0w==:tPTvEK3WEF4Uu37TwOU6hR4xA848lI7ixexDm7sCbyg=', 'admin', '+1-555-0001', 'Pre-Clear Admin', TRUE, NOW(), TRUE, TRUE);

-- ============================================================================
-- VERIFICATION QUERIES
-- ============================================================================
SELECT 'Database rebuild complete!' AS Status;
SELECT CONCAT('Total tables created: ', COUNT(*)) AS TableCount FROM information_schema.tables WHERE table_schema = 'preclear';
SELECT CONCAT('Total users seeded: ', COUNT(*)) AS UserCount FROM users;
SELECT id, email, role, company FROM users;
