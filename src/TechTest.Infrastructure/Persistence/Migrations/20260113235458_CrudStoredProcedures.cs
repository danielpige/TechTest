using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechTest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CrudStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // sp_user_update
            migrationBuilder.Sql(@"
            CREATE OR REPLACE PROCEDURE sp_user_update(
                IN  p_user_id         BIGINT,
                IN  p_full_name       VARCHAR,
                IN  p_phone           VARCHAR,
                IN  p_address         VARCHAR,
                IN  p_country_id      INT,
                IN  p_department_id   INT,
                IN  p_municipality_id INT,
                OUT p_updated         BOOLEAN
            )
            LANGUAGE plpgsql
            AS $$
            DECLARE
                v_exists INT;
                v_rows   INT;
            BEGIN
                p_updated := false;

                IF p_user_id IS NULL OR p_user_id <= 0 THEN
                    RAISE EXCEPTION 'user_id inválido' USING ERRCODE = '22023';
                END IF;

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

                UPDATE app_user
                SET full_name       = trim(p_full_name),
                    phone           = trim(p_phone),
                    address         = trim(p_address),
                    municipality_id = p_municipality_id
                WHERE id = p_user_id;

                GET DIAGNOSTICS v_rows = ROW_COUNT;

                IF v_rows > 0 THEN
                    p_updated := true;
                END IF;
            END;
            $$;
            ");

            // sp_user_delete
            migrationBuilder.Sql(@"
            CREATE OR REPLACE PROCEDURE sp_user_delete(
                IN  p_user_id BIGINT,
                OUT p_deleted BOOLEAN
            )
            LANGUAGE plpgsql
            AS $$
            DECLARE
                v_rows INT;
            BEGIN
                p_deleted := false;

                IF p_user_id IS NULL OR p_user_id <= 0 THEN
                    RAISE EXCEPTION 'user_id inválido' USING ERRCODE = '22023';
                END IF;

                DELETE FROM app_user WHERE id = p_user_id;
                GET DIAGNOSTICS v_rows = ROW_COUNT;

                IF v_rows > 0 THEN
                    p_deleted := true;
                END IF;
            END;
            $$;
            ");

            // sp_user_get_all (refcursor)
            migrationBuilder.Sql(@"
            CREATE OR REPLACE PROCEDURE sp_user_get_all(
                IN  p_limit  INT,
                IN  p_offset INT,
                INOUT p_cursor REFCURSOR
            )
            LANGUAGE plpgsql
            AS $$
            BEGIN
                IF p_limit IS NULL OR p_limit <= 0 THEN
                    p_limit := 100;
                END IF;

                IF p_offset IS NULL OR p_offset < 0 THEN
                    p_offset := 0;
                END IF;

                OPEN p_cursor FOR
                SELECT
                    u.id              AS ""UserId"",
                    u.full_name       AS ""FullName"",
                    u.phone           AS ""Phone"",
                    u.address         AS ""Address"",
                    c.id              AS ""CountryId"",
                    c.name            AS ""CountryName"",
                    d.id              AS ""DepartmentId"",
                    d.name            AS ""DepartmentName"",
                    m.id              AS ""MunicipalityId"",
                    m.name            AS ""MunicipalityName"",
                    u.created_at      AS ""CreatedAt""
                FROM app_user u
                JOIN municipality m ON m.id = u.municipality_id
                JOIN department d ON d.id = m.department_id
                JOIN country c ON c.id = d.country_id
                ORDER BY u.id DESC
                LIMIT p_limit OFFSET p_offset;
            END;
            $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_user_get_all(INT, INT, REFCURSOR);");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_user_delete(BIGINT);");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_user_update(BIGINT, VARCHAR, VARCHAR, VARCHAR, INT, INT, INT);");
        }
    }
}
