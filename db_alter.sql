ALTER TABLE `sale`
	ADD COLUMN `is_reversal` BIT NOT NULL DEFAULT b'0' COMMENT 'is this a reversal amendment transaction' AFTER `is_amendment`;
ALTER TABLE `purchase`
	ADD COLUMN `is_reversal` BIT NOT NULL DEFAULT b'0' COMMENT 'is this a reversal amendment transaction' AFTER `is_amendment`;
ALTER TABLE `expense`
	ADD COLUMN `is_reversal` BIT NOT NULL DEFAULT b'0' COMMENT 'is this a reversal amendment transaction' AFTER `is_amendment`;
ALTER TABLE `payment`
	ADD COLUMN `is_reversal` BIT NOT NULL DEFAULT b'0' COMMENT 'is this a reversal amendment transaction' AFTER `is_amendment`;

ALTER TABLE `sale`
	ADD COLUMN `is_amendment` BIT NOT NULL DEFAULT b'0' COMMENT 'is this an amendment transaction' AFTER `comments`;
ALTER TABLE `purchase`
	ADD COLUMN `is_amendment` BIT NOT NULL DEFAULT b'0' COMMENT 'is this an amendment transaction' AFTER `comments`;
ALTER TABLE `expense`
	ADD COLUMN `is_amendment` BIT NOT NULL DEFAULT b'0' COMMENT 'is this an amendment transaction' AFTER `comments`;
ALTER TABLE `payment`
	ADD COLUMN `is_amendment` BIT NOT NULL DEFAULT b'0' COMMENT 'is this an amendment transaction' AFTER `comments`;

ALTER TABLE `purchase`
	ALTER `qty_purchased` DROP DEFAULT;
ALTER TABLE `purchase`
	CHANGE COLUMN `qty_purchased` `qty_purchased` INT(10) NOT NULL COMMENT 'Quatity we purchased.' AFTER `amt_purchased`;
