CREATE view [dbo].[TradeLineSummary] as 
select		tl.trade_id,
			tl.trade_line_id,
			tl.trade_line_label,
			tl.trade_line_editorial_label,
			tg.trade_line_group_label,
			tg.trade_line_group_id,
			tg.trade_line_group_editorial_label,
			tgt.trade_line_group_type_id,
			tgt.trade_line_group_type_label,
			tt.tradable_thing_id,
			tt.tradable_thing_uri,
			tt.tradable_thing_label,
			ttc.tradable_thing_class_id,
			ttc.tradable_thing_class_uri,
			ttc.tradable_thing_class_label,
			ttc.tradable_thing_class_editorial_label,
			p.position_id,
			p.position_uri,
			p.position_label
from		Trade_Line tl
join		Tradable_Thing tt
on			tt.tradable_thing_id = tl.tradable_thing_id
join		Tradable_Thing_Class ttc
on			ttc.tradable_thing_class_id = tt.tradable_thing_class_id
join		Trade_Line_Group tg
on			tg.trade_line_group_id = tl.trade_line_group_id
join		Trade_Line_Group_Type tgt
on			tgt.trade_line_group_type_id = tg.trade_line_group_type_id
join		Position p
on			p.position_id = tl.position_id