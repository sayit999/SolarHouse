insert into sale
(sale.sold_on, sale.product_id, sale.customer_id, sale.sale_amt, sale.qty_sold, sale.qty_uom_id, sale.comments, sale.is_amendment, sale.is_reversal, sale.updated_on, sale.updated_by)
values
("2016-01-31", 43, 2, 0, 1, 1, "Bogus Sale. One battery used in shop was bad", 1,1, "2018-06-28", "firoz");
commit;