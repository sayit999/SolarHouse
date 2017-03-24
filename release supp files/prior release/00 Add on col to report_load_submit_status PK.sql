ALTER TABLE `report_load_submit_status`
	DROP PRIMARY KEY,
	ADD PRIMARY KEY (`report_from`, `type`, `report_type`, `on`);
