# DBReport : A DataBase structure Reporting tool for database administrators

[DBReport.html](http://patrice.dargenton.free.fr/CodesSources/DBReport.html)  
[DBReport.vbproj.html](http://patrice.dargenton.free.fr/CodesSources/DBReport.vbproj.html)  
By Patrice Dargenton (patrice.dargenton@free.fr)  
[My website](http://patrice.dargenton.free.fr/index.html)  
[My source codes](http://patrice.dargenton.free.fr/CodesSources/index.html)  

Database Administrators needs to compare database structures. Using [WinMerge](http://winmerge.org) on sql database structure files, it is difficult to compare because a lot of differences appear, whereas only a few of them are meaningful. DBReport shows only (and all) significant information that makes sense for daily administrator work.

#Features
- Tables, fields and table relations (links) are displayed ;
- Field types and (if available) default values are displayed ;
- Table and field description are displayed, if available ;
- Index and links are sorted and displayed (nobody needs to care about index order) ;
- Update and delete rules for relationships are displayed, if they are different to the default RESTRICT mode ;
- Duplicate constraint are displayed : the same constraint can be added several times, and can be hard to detect using an old version of phpMyAdmin (e.g. the version 4.1.4).

#Example with the classical Northwind database

[www.geeksengine.com/article/export-access-to-mysql.html](http://www.geeksengine.com/article/export-access-to-mysql.html)  
[www.geeksengine.com/lg.php?i=northwind-sql](http://www.geeksengine.com/lg.php?i=northwind-sql)  
![Northwind database relationship in MS-Access](http://wemadeitcharlotte.com/writings/accessapps_html_m5a21d901.png)

 
    Database report
    ---------------
     
    Login    : root
    Server   : localhost
    Database : northwind
    Indexes  : Sorted
     
    categories
      CategoryID (tinyint(5) unsigned) (autonumber)
      CategoryName (varchar(15))
      Description (mediumtext)
      Picture (varchar(50))
        Index   : CategoryID, Primary, Unique
        Index   : CategoryName, Unique
     
    customers
      CustomerID (varchar(5))
      CompanyName (varchar(40))
      ContactName (varchar(30)) (Unknown)
      ContactTitle (varchar(30))
      Address (varchar(60))
      City (varchar(15))
      Region (varchar(15))
      PostalCode (varchar(10))
      Country (varchar(15))
      Phone (varchar(24))
      Fax (varchar(24))
        Index   : City
        Index   : CompanyName
        Index   : PostalCode
        Index   : Region
        Index   : CustomerID, Primary, Unique
     
    employees
      EmployeeID (int(10) unsigned) (autonumber)
      LastName (varchar(20))
      FirstName (varchar(10))
      Title (varchar(30))
      TitleOfCourtesy (varchar(25))
      BirthDate (datetime)
      HireDate (datetime)
      Address (varchar(60))
      City (varchar(15))
      Region (varchar(15))
      PostalCode (varchar(10))
      Country (varchar(15))
      HomePhone (varchar(24))
      Extension (varchar(4))
      Photo (varchar(50))
      Notes (mediumtext)
      ReportsTo (int(10) unsigned)
        Index   : LastName
        Index   : PostalCode
        Index   : ReportsTo
        Index   : EmployeeID, Primary, Unique
     
    orders
      OrderID (int(10) unsigned) (autonumber)
      CustomerID (varchar(5))
      EmployeeID (int(10) unsigned)
      OrderDate (datetime)
      RequiredDate (datetime)
      ShippedDate (datetime)
      ShipVia (int(10) unsigned)
      Freight (double) (0)
      ShipName (varchar(40))
      ShipAddress (varchar(60))
      ShipCity (varchar(15))
      ShipRegion (varchar(15))
      ShipPostalCode (varchar(10))
      ShipCountry (varchar(15))
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
      ProductName (varchar(40))
      SupplierID (int(10) unsigned)
      CategoryID (tinyint(5) unsigned)
      QuantityPerUnit (varchar(20))
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
      CompanyName (varchar(40))
      Phone (varchar(24))
        Index   : ShipperID, Primary, Unique
     
    suppliers
      SupplierID (int(10) unsigned) (autonumber)
      CompanyName (varchar(40))
      ContactName (varchar(30))
      ContactTitle (varchar(30))
      Address (varchar(60))
      City (varchar(15))
      Region (varchar(15))
      PostalCode (varchar(10))
      Country (varchar(15))
      Phone (varchar(24))
      Fax (varchar(24))
      HomePage (varchar(255))
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

 
 
#Explanation
 
##Not nullable foreign key
This information reminds that this field needs to be filled, otherwise an error will be thrown.
 
##Not nullable without default value
This information reminds that this field needs to be filled, otherwise an error may or may not be thrown, according to the database server setting. For example, see the [MySql strict mode](http://dev.mysql.com/doc/refman/5.7/en/sql-mode.html#sql-mode-strict). If you provide a default value for all fields in this case, you will avoid this trap, and you won't have this warning anymore. Otherwise, it is safe to set the same strict mode setting for your development environment as your production environment.
 
##Nullable field for a unique index
MySQL (5.6) can't guarantee uniqueness (unicity) if one field of a unique key is nullable, you can have duplicates records in the table. Consider using a conventional mnemonic code instead, for example '_ALL' for 'all the items' of this field, and set it to not nullable.
 
 
#Projects
 
- ListBox or ComboBox to recall a list of databases recently used, instead of only the last one.
 
 
#Versions

##Version 1.03 - 18/09/2016
- Report generation time added to the end of the report ;
- Duplicate constraint bug fixed (the same constraint can be added several times, and can be hard to detect using an old version of phpMyAdmin) ;
- DbReader.ReaderProgress handled ;
- DbReader version 1.3.7.0 -> 2.1.1.2 (faster !) ;
- DotNet 4.0 -> DotNet 4.5.

##Version 1.02 - 24/01/2016
- DB report change : 'an unique index' -> 'a unique index' ;
- MouseOver control messages : disabled ;
- Reset settings button ;
- Cancel button ;
- Check boxes for options ;
- Option to display or hide the description of tables and fields.
 
##Version 1.01 - 03/01/2016 : First version
 
 
#Links
 
- The classical Northwind database  
  [www.geeksengine.com/article/export-access-to-mysql.html](http://www.geeksengine.com/article/export-access-to-mysql.html)  
  [www.geeksengine.com/lg.php?i=northwind-sql](http://www.geeksengine.com/lg.php?i=northwind-sql)
 
- Library used : [https://dbschemareader.codeplex.com](https://dbschemareader.codeplex.com)
 
##See also
 
- (french) [DBComp2](http://patrice.dargenton.free.fr/CodesSources/DBComp.html) : le comparateur de structure de base de données Access  
  Source code : [DBComp.vbp.html](http://patrice.dargenton.free.fr/CodesSources/DBComp.vbp.html)  