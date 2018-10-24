update purchase set purchase.purchased_on = date(purchase.purchased_on);
update sale set sale.sold_on = date(sale.sold_on);
update expense set expense.expensed_on = date(expense.expensed_on);
update payment set payment.paid_on = date(payment.paid_on);
commit;