using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechTest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE PROCEDURE sp_user_create(
                    IN  p_full_name       VARCHAR,
                    IN  p_phone           VARCHAR,
                    IN  p_address         VARCHAR,
                    IN  p_country_id      INT,
                    IN  p_department_id   INT,
                    IN  p_municipality_id INT,
                    OUT p_user_id         BIGINT
                )
                LANGUAGE plpgsql
                AS $$
                DECLARE
                    v_exists INT;
                BEGIN
                    IF p_full_name IS NULL OR length(trim(p_full_name)) = 0 THEN
                        RAISE EXCEPTION 'full_name requerido' USING ERRCODE = '22023';
                    END IF;

                    IF p_phone IS NULL OR length(trim(p_phone)) = 0 THEN
                        RAISE EXCEPTION 'phone requerido' USING ERRCODE = '22023';
                    END IF;

                    IF p_address IS NULL OR length(trim(p_address)) = 0 THEN
                        RAISE EXCEPTION 'address requerido' USING ERRCODE = '22023';
                    END IF;

                    SELECT 1 INTO v_exists
                    FROM municipality m
                    JOIN department d ON d.id = m.department_id
                    JOIN country c ON c.id = d.country_id
                    WHERE c.id = p_country_id
                      AND d.id = p_department_id
                      AND m.id = p_municipality_id
                    LIMIT 1;

                    IF v_exists IS NULL THEN
                        RAISE EXCEPTION 'País/Departamento/Municipio no son coherentes o no existen'
                            USING ERRCODE = '22023';
                    END IF;

                    INSERT INTO app_user (full_name, phone, address, municipality_id)
                    VALUES (trim(p_full_name), trim(p_phone), trim(p_address), p_municipality_id)
                    RETURNING id INTO p_user_id;
                END;
                $$;
                ");

            migrationBuilder.Sql(@"
                CREATE OR REPLACE PROCEDURE sp_user_get_by_id(
                    IN  p_user_id BIGINT,
                    OUT o_user_id BIGINT,
                    OUT o_full_name VARCHAR,
                    OUT o_phone VARCHAR,
                    OUT o_address VARCHAR,
                    OUT o_country_id INT,
                    OUT o_country_name VARCHAR,
                    OUT o_department_id INT,
                    OUT o_department_name VARCHAR,
                    OUT o_municipality_id INT,
                    OUT o_municipality_name VARCHAR,
                    OUT o_created_at TIMESTAMPTZ
                )
                LANGUAGE plpgsql
                AS $$
                BEGIN
                    SELECT
                        u.id,
                        u.full_name,
                        u.phone,
                        u.address,
                        c.id,
                        c.name,
                        d.id,
                        d.name,
                        m.id,
                        m.name,
                        u.created_at
                    INTO
                        o_user_id,
                        o_full_name,
                        o_phone,
                        o_address,
                        o_country_id,
                        o_country_name,
                        o_department_id,
                        o_department_name,
                        o_municipality_id,
                        o_municipality_name,
                        o_created_at
                    FROM app_user u
                    JOIN municipality m ON m.id = u.municipality_id
                    JOIN department d ON d.id = m.department_id
                    JOIN country c ON c.id = d.country_id
                    WHERE u.id = p_user_id;
                END;
                $$;
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_user_get_by_id(BIGINT);");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_user_create(VARCHAR, VARCHAR, VARCHAR, INT, INT, INT);");
        }
    }
}
