CREATE TABLE `OrderItem` (
	`Id` bigint NOT NULL AUTO_INCREMENT,
	`Name` varchar(50) NOT NULL,
	`Description` varchar(255) NULL,
	`Price` real NOT NULL,
	PRIMARY KEY (`Id`)
);

CREATE TABLE `SubCategory` (
	`Id` bigint NOT NULL AUTO_INCREMENT,
	`Name` varchar(50) NOT NULL,
	`CategoryId` bigint NOT NULL,
	PRIMARY KEY (`Id`)
);

CREATE TABLE `SubCategory_OrderItem` (
	`Id` bigint NOT NULL AUTO_INCREMENT,
	`SubCategoryId` bigint NOT NULL,
	`OrderItemId` bigint NOT NULL,
	PRIMARY KEY (`Id`)
);

CREATE TABLE `Tables` (
	`Id` bigint NOT NULL AUTO_INCREMENT,
	`Name` varchar(50) NOT NULL,
	PRIMARY KEY (`Id`)
);

CREATE TABLE `BookingStatus` (
	`Id` bigint NOT NULL AUTO_INCREMENT,
	`Name` varchar(50) NOT NULL,
	PRIMARY KEY (`Id`)
);

CREATE TABLE `Category` (
	`Id` bigint NOT NULL AUTO_INCREMENT,
	`Name` varchar(50) NOT NULL,
	PRIMARY KEY (`Id`)
);

CREATE TABLE `Tables_OrderItem` (
	`Id` bigint NOT NULL AUTO_INCREMENT,
	`TableId` bigint NOT NULL,
	`OrderItemId` bigint NOT NULL,
	`Quantity` real NOT NULL,
	PRIMARY KEY (`Id`)
);

ALTER TABLE `SubCategory` ADD CONSTRAINT `SubCategory_fk0` FOREIGN KEY (`CategoryId`) REFERENCES `Category`(`Id`);

ALTER TABLE `SubCategory_OrderItem` ADD CONSTRAINT `SubCategory_OrderItem_fk0` FOREIGN KEY (`SubCategoryId`) REFERENCES `SubCategory`(`Id`);

ALTER TABLE `SubCategory_OrderItem` ADD CONSTRAINT `SubCategory_OrderItem_fk1` FOREIGN KEY (`OrderItemId`) REFERENCES `OrderItem`(`Id`);

ALTER TABLE `Tables_OrderItem` ADD CONSTRAINT `Tables_OrderItem_fk0` FOREIGN KEY (`TableId`) REFERENCES `Tables`(`Id`);

ALTER TABLE `Tables_OrderItem` ADD CONSTRAINT `Tables_OrderItem_fk1` FOREIGN KEY (`OrderItemId`) REFERENCES `OrderItem`(`Id`);

