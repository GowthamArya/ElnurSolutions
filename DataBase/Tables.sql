USE dev_ElnurSolutionsDB

SELECT * FROM Products

CREATE TABLE ProductCategories (
    Id INT IDENTITY(1,1) NOT NULL,
    Name NVARCHAR(25) NOT NULL,
    Description NVARCHAR(200) NOT NULL,
    CreationDate DATETIME NOT NULL DEFAULT (GETDATE()),
    LastUpdate DATETIME NULL DEFAULT (GETDATE()),
    CONSTRAINT PK_ProductCategories PRIMARY KEY CLUSTERED (Id ASC)
);

CREATE TABLE ProductSubCategories (
    Id INT IDENTITY(1,1) NOT NULL,
    Name NVARCHAR(25) NOT NULL,
    Description NVARCHAR(200) NOT NULL,
    ProductCategoryId INT NOT NULL,
    CreationDate DATETIME NOT NULL DEFAULT (GETDATE()),
    LastUpdate DATETIME NULL DEFAULT (GETDATE()),
    CONSTRAINT PK_ProductSubCategories PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_ProductSubCategories_ProductCategories FOREIGN KEY (ProductCategoryId)
        REFERENCES ProductCategories(Id)
        ON DELETE CASCADE 
        ON UPDATE CASCADE
);


CREATE TABLE Products (
    Id INT IDENTITY(1,1) NOT NULL,
    Name NVARCHAR(25) NOT NULL,
    Description NVARCHAR(250) NOT NULL,
    RichTextArea NVARCHAR(MAX) NULL,
    ProductCategoryId INT NOT NULL,
    CreationDate DATETIME NOT NULL DEFAULT (GETDATE()),
    LastUpdate DATETIME NULL DEFAULT (GETDATE()),
    CONSTRAINT PK_Products PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_Products_ProductCategories FOREIGN KEY (ProductCategoryId) REFERENCES ProductCategories(Id)	ON DELETE CASCADE	ON UPDATE CASCADE
);