USE dev_Elnur

DROP TABLE ProductSubCategory;
DROP TABLE ProductCategory;


CREATE TABLE ProductCategory (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(25) NOT NULL
);

CREATE TABLE ProductSubCategory (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(25) NOT NULL,
    Description NVARCHAR(200) NOT NULL,
    ProductCategoryId INT NOT NULL,
    CONSTRAINT FK_ProductCategory_ProductCategory_Id
        FOREIGN KEY (ProductCategoryId) 
        REFERENCES ProductCategory(Id) 
        ON DELETE CASCADE 
        ON UPDATE CASCADE
);

CREATE TABLE Product (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(25) NOT NULL,
    Description NVARCHAR(250) NOT NULL,
	RichTextArea NVARCHAR(MAX) NULL,
	ProductSubCategoryId INT NOT NULL,
    CONSTRAINT FK_ProductSubCategory_ProductCategory_Id
        FOREIGN KEY (ProductSubCategoryId) 
        REFERENCES ProductSubCategory(Id) 
        ON DELETE CASCADE 
        ON UPDATE CASCADE
);