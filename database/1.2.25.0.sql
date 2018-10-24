INSERT INTO `product` (`product_id`, `product_code`, `product_name`, `product_category_id`, `qty_available`, `qty_uom`, `qty_uom_id`, `acb_cost`, `retail_discount_room_percentage`, `min_ret_gross_profit_margin_percentage`, `min_ws_gross_profit_margin_percentage`, `retail_sale_price`, `wholesale_sale_price`, `low_stock_alert_qty`, `is_reorder`, `comments`, `updated_on`, `updated_by`) VALUES ( 500, 'NO_SALE', 'Hamna Mauzo', 29, 10000, 'pc', 1, 0, 10, 40, 10, NULL, NULL, 1, b'1', NULL, NOW(), 'Firoz');
INSERT INTO `purchase` (`purchased_on`, `supplier_id`, `product_id`, `amt_purchased`, `qty_purchased`, `qty_uom_id`, `purchase_amt_paid_cash`, `comments`, `is_amendment`, `is_reversal`, `updated_on`, `updated_by`) VALUES ('2017-01-01 00:00:00', 20, 500, 0, 10000, 1, 0, 'Bogus product to track no sales', b'0', b'0', NOW(), 'Firoz');
commit;
