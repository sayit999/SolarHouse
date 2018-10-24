ALTER TABLE `product`
	ADD COLUMN `low_stock_alert_qty` INT(11) NULL DEFAULT '1' COMMENT 'Min qty before triggers reorder' AFTER `wholesale_sale_price`;