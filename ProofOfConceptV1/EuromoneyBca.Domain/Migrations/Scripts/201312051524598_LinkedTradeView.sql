CREATE view [dbo].[LinkedTrade] as
select r.trade_id,
       r.related_trade_id,
       l.trade_editorial_label,
	   count(r.related_trade_id) as linked_trade_count	   
from  [dbo].[Related_Trade] r 
       inner join [dbo].[Trade] l on r.related_trade_id = l.trade_id
       where l.status = 1
group by r.trade_id, r.related_trade_id, l.trade_editorial_label
      