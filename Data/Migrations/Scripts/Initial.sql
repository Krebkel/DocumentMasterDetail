DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'DocumentMasterDetail') THEN
            CREATE SCHEMA "DocumentMasterDetail";
        END IF;
    END
$EF$;
CREATE TABLE IF NOT EXISTS "DocumentMasterDetail"."__EFMigrationsHistory"
(
    "MigrationId"    character varying(150) NOT NULL,
    "ProductVersion" character varying(32)  NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'DocumentMasterDetail') THEN
            CREATE SCHEMA "DocumentMasterDetail";
        END IF;
    END
$EF$;

CREATE TABLE "DocumentMasterDetail"."ErrorLogs"
(
    "Id"   integer GENERATED BY DEFAULT AS IDENTITY,
    "Date" timestamp with time zone NOT NULL,
    "Note" text                     NOT NULL,
    CONSTRAINT "PK_ErrorLogs" PRIMARY KEY ("Id")
);

CREATE TABLE "DocumentMasterDetail"."Invoices"
(
    "Id"          integer GENERATED BY DEFAULT AS IDENTITY,
    "Number"      text                     NOT NULL,
    "Date"        timestamp with time zone NOT NULL,
    "TotalAmount" numeric                  NOT NULL,
    "Note"        character varying(255)   NOT NULL,
    CONSTRAINT "PK_Invoices" PRIMARY KEY ("Id")
);

CREATE TABLE "DocumentMasterDetail"."Positions"
(
    "Id"        integer GENERATED BY DEFAULT AS IDENTITY,
    "Name"      text    NOT NULL,
    "Quantity"  numeric NOT NULL,
    "Value"     numeric NOT NULL,
    "InvoiceId" integer NOT NULL,
    CONSTRAINT "PK_Positions" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Positions_Invoices_InvoiceId" FOREIGN KEY ("InvoiceId") REFERENCES "DocumentMasterDetail"."Invoices" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_Invoices_Number" ON "DocumentMasterDetail"."Invoices" ("Number");

CREATE INDEX "IX_Positions_InvoiceId" ON "DocumentMasterDetail"."Positions" ("InvoiceId");

INSERT INTO "DocumentMasterDetail"."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240114030908_Initial', '8.0.1');

COMMIT;