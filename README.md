DBReport: A DataBase structure Reporting tool for database administrators
---

Version 1.13 - 11/05/2024

Database Administrators needs to compare database structures. Using [WinMerge](http://winmerge.org) on sql database structure files, it is difficult to compare because a lot of differences appear, whereas only a few of them are meaningful. DBReport shows only (and all) significant information that makes sense for daily administrator work.

- [Features](#features)
- [Database engines](#database-engines)
- [Example with the classical Northwind database](#example-with-the-classical-northwind-database)
- [Explanation](#explanation)
    - [Not nullable foreign key](#not-nullable-foreign-key)
    - [Not nullable without default value](#not-nullable-without-default-value)
    - [Nullable field for a unique index](#nullable-field-for-a-unique-index)
    - [MySql parameters](#mysql-parameters)
        - [sql_mode](#sql_mode)
        - [innodb_strict_mode](#innodb_strict_mode)
        - [collation](#collation)
        - [table engine](#table-engine)
        - [timeout](#timeout)
        - [Queries](#queries)
        - [How to change the server collation?](#how-to-change-the-server-collation)
- [Projects](#projects)
- [Versions](#versions)
    - [Version 1.13 - 11/05/2024](#version-113---11052024)
    - [Version 1.12 - 04/05/2024](#version-112---04052024)
    - [Version 1.11 - 27/04/2024](#version-111---27042024)
    - [Version 1.10 - 10/04/2024](#version-110---10042024)
    - [Version 1.09 - 15/04/2023](#version-109---15042023)
    - [Version 1.08 - 01/10/2022](#version-108---01102022)
    - [Version 1.07 - 04/03/2022](#version-107---04032022)
    - [Version 1.06 - 28/02/2022](#version-106---28022022)
    - [Version 1.05 - 05/03/2017](#version-105---05032017)
    - [Version 1.04 - 23/10/2016](#version-104---23102016)
    - [Version 1.03 - 18/09/2016](#version-103---18092016)
    - [Version 1.02 - 24/01/2016](#version-102---24012016)
    - [Version 1.01 - 03/01/2016: First version](#version-101---03012016-first-version)
- [Links](#links)
    - [See also](#see-also)

# Features
- Tables, fields and table relations (links) are displayed;
- Field types and (if available) default values are displayed;
- Table and field description are displayed, if available;
- Index and links (relationships between tables) are sorted and displayed (nobody needs to care about index order);
- Update and delete rules for relationships are displayed, if they are different to the default RESTRICT mode;
- Duplicate constraints are displayed: the same constraint can be added several times, and it can be hard to detect using an old version of phpMyAdmin (e.g. the version 4.1.4).

# Database engines
- MySql : MySql.Data.MySqlClient
- Oracle : System.Data.OracleClient
- SQLite : System.Data.SQLite

For SQLite:
- Free GUI tool: https://sqlitestudio.pl
- System.Data.SQLite.Core must be installed via Nuget (if you only copy/paste the packages, the project will not include checking the required DLLs at runtime), in order to get:
```
<package id="Stub.System.Data.SQLite.Core.NetFramework" version="1.0.118.0" targetFramework="net48" />
<package id="System.Data.SQLite.Core" version="1.0.118.0" targetFramework="net48" />
```
- This Data Provider must be added in the App.Config:
```
  <system.data>
    <DbProviderFactories>
      <remove invariant="System.Data.SQLite" />
      <add name="SQLite Data Provider" invariant="System.Data.SQLite" description=".Net Framework Data Provider for SQLite" type="System.Data.SQLite.SQLiteFactory, System.Data.SQLite" />
    </DbProviderFactories>
  </system.data>
```

# Example with the classical Northwind database

[www.geeksengine.com/article/export-access-to-mysql.html](http://www.geeksengine.com/article/export-access-to-mysql.html)  
[https://en.wikiversity.org/wiki/Database_Examples/Northwind](https://en.wikiversity.org/wiki/Database_Examples/Northwind)  
![Northwind database relationship in MS-Access](http://patrice.dargenton.free.fr/CodesSources/DBReport_fichiers/NorthwindDatabaseStructure)

 
    Database report 1.09
    --------------------
    
    Login    : root
    Server   : localhost
    Database : northwind
    Indexes  : Sorted
    
    MySql parameters :
    version_comment : MySQL Community Server (GPL)
    version : 5.6.15-log
    protocol_version : 10
    sql_mode : NO_ENGINE_SUBSTITUTION (Default : STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION)
    innodb_strict_mode : OFF (Default : ON)
    net_read_timeout : 30 (30 sec.)
    net_write_timeout : 60 (1 mn)
    Database default collation (DEFAULT_COLLATION_NAME) : utf8_unicode_ci (Default : utf8_general_ci)
    
    categories
      CategoryID (tinyint(5) unsigned) (autonumber)
      CategoryName (varchar(15)) ('')
      Description (mediumtext)
      Picture (varchar(50)) ('')
        Index   : CategoryID, Primary, Unique
        Index   : CategoryName, Unique
    
    customers
      CustomerID (varchar(5)) ('')
      CompanyName (varchar(40)) ('')
      ContactName (varchar(30)) (Unknown)
      ContactTitle (varchar(30)) ('')
      Address (varchar(60)) ('')
      City (varchar(15)) ('')
      Region (varchar(15)) ('')
      PostalCode (varchar(10)) ('')
      Country (varchar(15)) ('')
      Phone (varchar(24)) ('')
      Fax (varchar(24)) ('')
        Index   : City
        Index   : CompanyName
        Index   : PostalCode
        Index   : Region
        Index   : CustomerID, Primary, Unique
    
    employees
      EmployeeID (int(10) unsigned) (autonumber)
      LastName (varchar(20)) ('')
      FirstName (varchar(10)) ('')
      Title (varchar(30)) ('')
      TitleOfCourtesy (varchar(25)) ('')
      BirthDate (datetime)
      HireDate (datetime)
      Address (varchar(60)) ('')
      City (varchar(15)) ('')
      Region (varchar(15)) ('')
      PostalCode (varchar(10)) ('')
      Country (varchar(15)) ('')
      HomePhone (varchar(24)) ('')
      Extension (varchar(4)) ('')
      Photo (varchar(50)) ('')
      Notes (mediumtext)
      ReportsTo (int(10) unsigned)
        Index   : LastName
        Index   : PostalCode
        Index   : ReportsTo
        Index   : EmployeeID, Primary, Unique
    
    orders
      OrderID (int(10) unsigned) (autonumber)
      CustomerID (varchar(5)) ('')
      EmployeeID (int(10) unsigned)
      OrderDate (datetime)
      RequiredDate (datetime)
      ShippedDate (datetime)
      ShipVia (int(10) unsigned)
      Freight (double) (0)
      ShipName (varchar(40)) ('')
      ShipAddress (varchar(60)) ('')
      ShipCity (varchar(15)) ('')
      ShipRegion (varchar(15)) ('')
      ShipPostalCode (varchar(10)) ('')
      ShipCountry (varchar(15)) ('')
        Index   : CustomerID
        Index   : EmployeeID
        Index   : ShipVia
        Index   : ShipPostalCode
        Index   : ShippedDate
        Index   : OrderID, Primary, Unique
    
    order_details
      ID (int(10) unsigned) (autonumber)
      OrderID (int(10) unsigned)
      ProductID (int(10) unsigned)
      UnitPrice (double unsigned) (0)
      Quantity (smallint(5) unsigned) (1)
      Discount (float unsigned) (0)
        Index   : ProductID
        Index   : ID, Primary, Unique
        Index   : Uidx_OrderID_ProductID, Unique, 2 fields :
          field : OrderID
          field : ProductID
    
    products
      ProductID (int(10) unsigned) (autonumber)
      ProductName (varchar(40)) ('')
      SupplierID (int(10) unsigned)
      CategoryID (tinyint(5) unsigned)
      QuantityPerUnit (varchar(20)) ('')
      UnitPrice (double) (0)
      UnitsInStock (smallint(5) unsigned) (0)
      UnitsOnOrder (smallint(5) unsigned) (0)
      ReorderLevel (smallint(5) unsigned) (0)
      Discontinued (enum('y','n')) (n)
        Index   : CategoryID
        Index   : SupplierID
        Index   : ProductName
        Index   : ProductID, Primary, Unique
    
    shippers
      ShipperID (int(10) unsigned) (autonumber)
      CompanyName (varchar(40)) ('')
      Phone (varchar(24)) ('')
        Index   : ShipperID, Primary, Unique
    
    suppliers
      SupplierID (int(10) unsigned) (autonumber)
      CompanyName (varchar(40)) ('')
      ContactName (varchar(30)) ('')
      ContactTitle (varchar(30)) ('')
      Address (varchar(60)) ('')
      City (varchar(15)) ('')
      Region (varchar(15)) ('')
      PostalCode (varchar(10)) ('')
      Country (varchar(15)) ('')
      Phone (varchar(24)) ('')
      Fax (varchar(24)) ('')
      HomePage (varchar(255)) ('') (collation : utf8_general_ci)
        Index   : PostalCode
        Index   : CompanyName
        Index   : SupplierID, Primary, Unique
    
    
    Links
    
    categories
    
    customers
    
    employees
      employees : ReportsTo
    
    orders
      customers : CustomerID
      employees : EmployeeID
      shippers : ShipVia
    
    order_details
      orders : OrderID
      products : ProductID
    
    products
      categories : CategoryID
      suppliers : SupplierID
    
    shippers
    
    suppliers
    
    Report created : 19/02/2017 12:14:11 -> 19/02/2017 12:14:12 : 1.5 sec.
 
 
# Explanation
 
## Not nullable foreign key
This information reminds that this field needs to be filled, otherwise an error will be thrown.
 
## Not nullable without default value
This information reminds that this field needs to be filled, otherwise an error may or may not be thrown, according to the database server setting. For example, see the [MySql strict mode](http://dev.mysql.com/doc/refman/5.7/en/sql-mode.html#sql-mode-strict). If you provide a default value for all fields in this case, you will avoid this trap, and you won't have this warning anymore. Otherwise, it is safe to set the same strict mode setting for your development environment as your production environment.
 
## Nullable field for a unique index
MySQL (5.6) can't guarantee uniqueness (unicity) if one field of a unique key is nullable, you can have duplicates records in the table. Consider using a conventional mnemonic code instead, for example '_ALL' for 'all the items' of this field, and set it to not nullable.

## MySql parameters

If the database provider corresponds to MySql ("MySql.Data.MySqlClient"), then the following main MySql parameters are displayed in the report, but only if they are different form their default value: sql_mode, innodb_strict_mode, collation, table engine, timeout.

List of all system variables and options:  
https://dev.mysql.com/doc/refman/5.7/en/server-system-variables.html  
https://dev.mysql.com/doc/refman/5.7/en/server-options.html  

### sql_mode
sql_mode controls what SQL syntax MySQL accepts, and determines whether it silently ignores errors, or validates input syntax and data values. For example, if sql_mode is empty, implicit conversions can be performed without error (but only with warnings), see full documentation:  
https://dev.mysql.com/doc/refman/5.7/en/sql-mode.html  
https://dev.mysql.com/doc/refman/5.7/en/server-options.html#option_mysqld_sql-mode  
https://dev.mysql.com/doc/refman/5.7/en/server-system-variables.html#sysvar_sql_mode  
The main default values are: "STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION".

To understand the strict versus not strict sql_mode with a sample, see the section titled "The Effect of Strict SQL Mode on Statement Execution" there:  
https://dev.mysql.com/doc/refman/5.7/en/sql-mode.html  

### innodb_strict_mode
When innodb_strict_mode is ON, InnoDB returns errors rather than warnings for certain conditions. This is analogous to sql_mode in MySQL, it enables additional error checks for InnoDB tables:  
https://dev.mysql.com/doc/refman/5.7/en/innodb-parameters.html  
https://dev.mysql.com/doc/refman/5.7/en/innodb-parameters.html#sysvar_innodb_strict_mode  
The default value is ON since MySQL 5.7.7.

"Oracle recommends enabling innodb_strict_mode when using ROW_FORMAT and KEY_BLOCK_SIZE clauses in CREATE TABLE, ALTER TABLE, and CREATE INDEX statements. When innodb_strict_mode is disabled, InnoDB ignores conflicting clauses and creates the table or index with only a warning in the message log. The resulting table might have different characteristics than intended, such as lack of compression support when attempting to create a compressed table. When innodb_strict_mode is enabled, such problems generate an immediate error and the table or index is not created."

### collation
Once the default collation is set for DBReport, the database default collation is displayed if it is different from the default collation. The table collation is displayed in the same way, and finally each column collation.

Column collation is important because adding a foreign key may fail if the collation of columns is not the same.

### table engine
Once the default engine is set for DBReport, the table engine is displayed if it is different from the default table engine.

### timeout
The actual session level value for net_read_timeout and net_write_timeout are displayed. They have the same value as the global variable, if no session level instruction changes their value.

### Queries

Here are the queries for these MySql parameters for the Northwind database (schema_name and table_schema are the database name):

    SHOW VARIABLES WHERE Variable_Name IN ('version', 'version_comment', 'protocol_version', 'sql_mode', 'innodb_strict_mode', 'net_read_timeout', 'net_write_timeout');
    
    SHOW VARIABLES LIKE 'collation_server';
    
    SELECT 'DEFAULT_COLLATION_NAME', DEFAULT_COLLATION_NAME FROM information_schema.SCHEMATA WHERE schema_name = 'northwind';
    
    SELECT TABLE_NAME, ENGINE, COLLATION_NAME FROM information_schema.`TABLES` T, information_schema.`COLLATION_CHARACTER_SET_APPLICABILITY` CCSA WHERE CCSA.collation_name = T.table_collation AND T.table_schema = 'northwind';
    
    SELECT table_name, C.column_name, COLLATION_NAME FROM information_schema.`COLUMNS` C WHERE table_schema = 'northwind';

### How to change the server collation?

See there: http://kosalads.blogspot.fr/2013/03/mysql-55-how-to-change-mysql-default.html

If you run:  
show variables like '%collation%'  

you get:  
collation_connection = utf8mb4_general_ci  
collation_database = latin1_swedish_ci  
collation_server = latin1_swedish_ci  

If you create a new database, it's collation will be: latin1_swedish_ci  

Add this in my.ini at the [mysqld] section:  
init_connect='SET collation_connection = utf8_general_ci'  
init_connect='SET NAMES utf8'  
character-set-server=utf8  
collation-server=utf8_general_ci  
skip-character-set-client-handshake  

Shut down MySql and restart it, and do again the test:  

If you run:  
show variables like '%collation%'  

you get:  
collation_connection = utf8mb4_general_ci  
collation_database = utf8_general_ci  
collation_server = utf8_general_ci  

If you create a new database, it's collation will be: utf8_general_ci

Be careful because if you miss some parameters for MySql (for exemple init_connect), you will crash MySql, PhpMyAdmin, and your databases, make backup for all of them before changing this config!

# Projects
 
- ListBox or ComboBox to recall a list of databases recently used, instead of only the last one.
 
 
# Versions

## Version 1.13 - 11/05/2024
- List of installed system database providers, from  DBSchemaReader: DatabaseSchemaViewer.

## Version 1.12 - 04/05/2024
- DatabaseSchemaReader 2.10.1 -> 2.11.0;
- MySqlConnector 2.3.6 -> 2.3.7;
- MySql dll version displayed;
- Parameters added: SortTables, DisplayMySqlParameters;
- SQLite parameters added: DisplayAutonumberAsPrimaryKey, DisplayMultipleIndexName, RenameSQLiteMultipleIndex, DisplaySQLiteSimpleIndexName;
- SQLite fixes.

## Version 1.11 - 27/04/2024
- SQLite DBProvider : System.Data.SQLite.

## Version 1.10 - 10/04/2024
- Port and InstanceName added in parameter settings, for Oracle;
- Oracle DBProvider : System.Data.OracleClient.

## Version 1.09 - 15/04/2023
- mysql-connector-net-6.9.x.msi -> NuGet MySqlConnector 2.2.5;
- Packages update.

## Version 1.08 - 01/10/2022
- DatabaseSchemaReader 2.7.11 -> 2.7.14;
- .Net45 -> .Net48.

## Version 1.07 - 04/03/2022
- MySql parameter added: foreign_key_checks;
- DatabaseSchemaReader NuGet package added, version=2.7.11.

## Version 1.06 - 28/02/2022
- Exe, dll and html doc out from source code, and inside release.

## Version 1.05 - 05/03/2017
- Main MySql parameters added in the report: sql_mode, innodb_strict_mode, collation, table engine, timeout;
- ForeignKeyDeleteRule, ForeignKeyUpdateRule: default value are now configurable.

## Version 1.04 - 23/10/2016
- DBReport version added in the report;
- Default value for String: the empty string is distinguished from the null string.

## Version 1.03 - 18/09/2016
- Report generation time added to the end of the report;
- Duplicate constraints bug fixed (the same constraint can be added several times, and it can be hard to detect using an old version of phpMyAdmin);
- DbReader.ReaderProgress handled;
- DbReader version 1.3.7.0 -> 2.1.1.2 (faster!);
- DotNet 4.0 -> DotNet 4.5.

## Version 1.02 - 24/01/2016
- DB report change: 'an unique index' -> 'a unique index';
- MouseOver control messages: disabled;
- Reset settings button;
- Cancel button;
- Check boxes for options;
- Option to display or hide the description of tables and fields.
 
## Version 1.01 - 03/01/2016: First version
 
 
# Links
 
- The classical Northwind database  
  [www.geeksengine.com/article/export-access-to-mysql.html](http://www.geeksengine.com/article/export-access-to-mysql.html)  
  [https://en.wikiversity.org/wiki/Database_Examples/Northwind](https://en.wikiversity.org/wiki/Database_Examples/Northwind)
 
- Library used: [https://github.com/martinjw/dbschemareader](https://github.com/martinjw/dbschemareader)

## See also
 
- (french) [DBComp2](http://patrice.dargenton.free.fr/CodesSources/DBComp.html): le comparateur de structure de base de données Access  
  Source code: [DBComp.vbp.html](http://patrice.dargenton.free.fr/CodesSources/DBComp.vbp.html)