ALTER TABLE `purchase`
	ALTER `qty_purchased` DROP DEFAULT;
ALTER TABLE `purchase`
	CHANGE COLUMN `qty_purchased` `qty_purchased` INT(10) NOT NULL COMMENT 'Quatity we purchased.' AFTER `amt_purchased`;
