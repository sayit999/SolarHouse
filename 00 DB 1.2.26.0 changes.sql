ALTER USER root@localhost IDENTIFIED BY 'arusha239';

ALTER TABLE `report_load_submit_status`
	CHANGE COLUMN `comments` `comments` VARCHAR(1000) NULL DEFAULT NULL AFTER `report_file_loc`;

ALTER TABLE `report_load_submit_status`
	ADD COLUMN `expected_cash` INT NULL AFTER `comments`;