# Changelog

All notable changes to the DBReport project will be documented in this file.

## [Unreleased]

## [1.22] - 2025-06-05 Pre-release .Net Core in the DotNetCore branch
### Updated
- Ngplsql: the open source .NET data provider for PostgreSQL
- Dynamic configuration using shortcut (e.g. DBReport_Config1.exe.config read instead of DBReport.exe.config) does not work for the moment;
- The database connection must be explicit at compile time (trying to connect at run time is no longer possible, and DbProviderFactories is no longer supported in App.Config);
- The package for Oracle.ManagedDataAccess is not available in .Net Core, but in .Net 4 (so the connection constructor is ensured via .Net 4 backward compatibility and not directly in .Net Core);
- Net 4.8 update to .Net Core (.Net 8)

## [1.21] - 2024-06-05
### Added
- Ngplsql: the open source .NET data provider for PostgreSQL

### Updated
- Packages updated

## [1.20] - 2024-06-21
### Added
- [Enum](https://github.com/TylerBrinkley/Enums.NET) for DBProvider.

## [1.19] - 2024-06-15
### Added
- Option sort table in the user interface.

## [1.18] - 2024-06-14
### Added
- MariaDb DBProvider: MySqlConnector.

## [1.17] - 2024-05-31
### Added
- Option added: Display links below each table.

## [1.16] - 2024-05-31
### Added
- Oracle TNS connection mode (Transparent Network Substrate).

## [1.15] - 2024-05-24
### Added
- Table sorting also for links.

### Fixed
- Fix SQLite index.

## [1.14] - 2024-05-17
### Added
- Dynamic configuration: a specific configuration can be loaded at runtime from a command line argument, for example: if for example Config1 is given, then the DBReport_Config1.exe.config configuration is read instead of DBReport.exe.config and also C:\Users\[MyAccount]\AppData\Local\DBReport\DBReport.exe_Url_[xxx]\1.1.4.[xxxxx]\user.config for user-specific configuration.

## [1.13] - 2024-05-11
### Added
- List of installed system database providers, from  DBSchemaReader: DatabaseSchemaViewer.

## [1.12] - 2024-05-04
### Updated
- DatabaseSchemaReader 2.10.1 -> 2.11.0;
- MySqlConnector 2.3.6 -> 2.3.7.

### Added
- MySql dll version displayed;
- Parameters added: SortTables, DisplayMySqlParameters;
- SQLite parameters added: DisplayAutonumberAsPrimaryKey, DisplayMultipleIndexName, RenameSQLiteMultipleIndex, DisplaySQLiteSimpleIndexName.

### Fixed
- SQLite fixes.

## [1.11] - 2024-04-27
### Added
- SQLite DBProvider: System.Data.SQLite.

## [1.10] - 2024-04-15
### Added
- Port and InstanceName added in parameter settings, for Oracle;
- Oracle DBProvider : System.Data.OracleClient.

## [1.09] - 2024-04-10
### Updated
- mysql-connector-net-6.9.x.msi -> NuGet MySqlConnector 2.2.5;
- Packages update.

## [1.09] - 2022-10-01
### Updated
- DatabaseSchemaReader 2.7.11 -> 2.7.14;
- .Net45 -> .Net48.

## [1.08] - 2022-10-01
### Updated
- DatabaseSchemaReader 2.7.11 -> 2.7.14;
- .Net45 -> .Net48.

## [1.07] - 2022-03-04
### Added
- MySql parameter added: foreign_key_checks;
- DatabaseSchemaReader NuGet package added, version=2.7.11.

## [1.06] - 2022-02-28
### Fixed
- Exe, dll and html doc out from source code, and inside release.

## [1.05] - 2017-03-05
### Added
- Main MySql parameters added in the report: sql_mode, innodb_strict_mode, collation, table engine, timeout;
- ForeignKeyDeleteRule, ForeignKeyUpdateRule: default value are now configurable.

## [1.04] - 2016-10-23
### Added
- DBReport version added in the report.

### Fixed
- Default value for String: the empty string is distinguished from the null string.

## [1.03] - 2016-09-18
### Added
- Report generation time added to the end of the report.

### Updated
- DbReader.ReaderProgress handled;
- DbReader version 1.3.7.0 -> 2.1.1.2 (faster!);
- DotNet 4.0 -> DotNet 4.5.

### Fixed
- Duplicate constraints bug fixed (the same constraint can be added several times, and it can be hard to detect using an old version of phpMyAdmin).

## [1.02] - 2016-01-04
### Updated
- DB report change: 'an unique index' -> 'a unique index'.

### Added
- Reset settings button;
- Cancel button;
- Check boxes for options;
- Option to display or hide the description of tables and fields.

### Fixed
- MouseOver control messages: disabled.

## [1.01] - 2016-01-03 First version