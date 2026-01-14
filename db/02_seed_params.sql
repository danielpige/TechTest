-- db/03_seed_params.sql
-- Datos de prueba para: country, department, municipality
-- Recomendación: ejecutar en la BD tech_test

BEGIN;

-- =========================
-- 1) Countries
-- =========================
INSERT INTO country (id, name, iso2)
VALUES
  (1, 'Colombia', 'CO'),
  (2, 'México', 'MX'),
  (3, 'Estados Unidos', 'US')
ON CONFLICT (id) DO UPDATE
SET name = EXCLUDED.name,
    iso2 = EXCLUDED.iso2;

-- =========================
-- 2) Departments (por país)
-- =========================
-- Colombia (1xx)
INSERT INTO department (id, country_id, name)
VALUES
  (101, 1, 'Atlántico'),
  (102, 1, 'Antioquia'),
  (103, 1, 'Cundinamarca'),
  (104, 1, 'Valle del Cauca'),
  (105, 1, 'Santander')
ON CONFLICT (id) DO UPDATE
SET country_id = EXCLUDED.country_id,
    name = EXCLUDED.name;

-- México (2xx)
INSERT INTO department (id, country_id, name)
VALUES
  (201, 2, 'Jalisco'),
  (202, 2, 'Ciudad de México'),
  (203, 2, 'Nuevo León')
ON CONFLICT (id) DO UPDATE
SET country_id = EXCLUDED.country_id,
    name = EXCLUDED.name;

-- Estados Unidos (3xx)
INSERT INTO department (id, country_id, name)
VALUES
  (301, 3, 'California'),
  (302, 3, 'Texas'),
  (303, 3, 'Florida')
ON CONFLICT (id) DO UPDATE
SET country_id = EXCLUDED.country_id,
    name = EXCLUDED.name;

-- =========================
-- 3) Municipalities (por departamento)
-- =========================
-- Atlántico (100x)
INSERT INTO municipality (id, department_id, name)
VALUES
  (1001, 101, 'Barranquilla'),
  (1002, 101, 'Soledad'),
  (1003, 101, 'Malambo')
ON CONFLICT (id) DO UPDATE
SET department_id = EXCLUDED.department_id,
    name = EXCLUDED.name;

-- Antioquia (110x)
INSERT INTO municipality (id, department_id, name)
VALUES
  (1101, 102, 'Medellín'),
  (1102, 102, 'Envigado'),
  (1103, 102, 'Itagüí')
ON CONFLICT (id) DO UPDATE
SET department_id = EXCLUDED.department_id,
    name = EXCLUDED.name;

-- Cundinamarca (120x)
INSERT INTO municipality (id, department_id, name)
VALUES
  (1201, 103, 'Soacha'),
  (1202, 103, 'Chía'),
  (1203, 103, 'Zipaquirá')
ON CONFLICT (id) DO UPDATE
SET department_id = EXCLUDED.department_id,
    name = EXCLUDED.name;

-- Valle del Cauca (130x)
INSERT INTO municipality (id, department_id, name)
VALUES
  (1301, 104, 'Cali'),
  (1302, 104, 'Palmira'),
  (1303, 104, 'Buenaventura')
ON CONFLICT (id) DO UPDATE
SET department_id = EXCLUDED.department_id,
    name = EXCLUDED.name;

-- Santander (140x)
INSERT INTO municipality (id, department_id, name)
VALUES
  (1401, 105, 'Bucaramanga'),
  (1402, 105, 'Floridablanca'),
  (1403, 105, 'Girón')
ON CONFLICT (id) DO UPDATE
SET department_id = EXCLUDED.department_id,
    name = EXCLUDED.name;

-- Jalisco (200x)
INSERT INTO municipality (id, department_id, name)
VALUES
  (2001, 201, 'Guadalajara'),
  (2002, 201, 'Zapopan')
ON CONFLICT (id) DO UPDATE
SET department_id = EXCLUDED.department_id,
    name = EXCLUDED.name;

-- Ciudad de México (210x)
INSERT INTO municipality (id, department_id, name)
VALUES
  (2101, 202, 'Cuauhtémoc'),
  (2102, 202, 'Coyoacán')
ON CONFLICT (id) DO UPDATE
SET department_id = EXCLUDED.department_id,
    name = EXCLUDED.name;

-- Nuevo León (220x)
INSERT INTO municipality (id, department_id, name)
VALUES
  (2201, 203, 'Monterrey'),
  (2202, 203, 'San Pedro Garza García')
ON CONFLICT (id) DO UPDATE
SET department_id = EXCLUDED.department_id,
    name = EXCLUDED.name;

-- California (300x)
INSERT INTO municipality (id, department_id, name)
VALUES
  (3001, 301, 'Los Angeles'),
  (3002, 301, 'San Francisco')
ON CONFLICT (id) DO UPDATE
SET department_id = EXCLUDED.department_id,
    name = EXCLUDED.name;

-- Texas (310x)
INSERT INTO municipality (id, department_id, name)
VALUES
  (3101, 302, 'Houston'),
  (3102, 302, 'Austin')
ON CONFLICT (id) DO UPDATE
SET department_id = EXCLUDED.department_id,
    name = EXCLUDED.name;

-- Florida (320x)
INSERT INTO municipality (id, department_id, name)
VALUES
  (3201, 303, 'Miami'),
  (3202, 303, 'Orlando')
ON CONFLICT (id) DO UPDATE
SET department_id = EXCLUDED.department_id,
    name = EXCLUDED.name;

-- =========================
-- 4) Ajustar secuencias (importante si insertas IDs manuales)
-- =========================
SELECT setval(pg_get_serial_sequence('country','id'),      (SELECT COALESCE(MAX(id), 1) FROM country));
SELECT setval(pg_get_serial_sequence('department','id'),   (SELECT COALESCE(MAX(id), 1) FROM department));
SELECT setval(pg_get_serial_sequence('municipality','id'), (SELECT COALESCE(MAX(id), 1) FROM municipality));

COMMIT;
