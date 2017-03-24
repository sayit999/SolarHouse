delete
from sale 
where sold_on = "2017-02-14" 
and product_id in (select product_id from product where product_code in ("SMS018", "SMS013"))
and is_amendment = 1