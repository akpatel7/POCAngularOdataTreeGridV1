CREATE view [dbo].[TradeHistory] as 

with 
Instructions as 
(
	select  'Instructions' update_type,
			trade_id,
			last_updated,
			instruction_label,
			instruction_type_label, 
			case 
				when convert(varchar,instruction_exit_date,107) is null then 'Open'
				else 'Closed'
			end [status]
	from Trade_Instruction i
		join Instruction_Type it
		on i.instruction_type_id = it.instruction_type_id
),

AbsPerformance as 
(
	select  'AbsPerformance' update_type,
			trade_id,
			last_updated,
			cast(return_value as varchar) + case p.measure_type_id
												when 1 then ' bps'
												when 3 then ' %'
												when 2 then ' ' + c.currency_code
											 end abs_perf_label						
	from Trade_Performance p
		join Measure_Type t
		on p.measure_type_id = t.measure_type_id
		left join Currency c
		on return_currency_id = currency_id	
	where return_benchmark_id is null	
),

RelPerformance as 
(
	select  'RelPerformance' update_type,
			trade_id,
			last_updated,
			cast(return_value as varchar) + case p.measure_type_id
												when 1 then ' bps'
												when 3 then ' %'
												when 2 then ' ' + c.currency_code
											 end rel_perf_label,
			benchmark_label 
	from Trade_Performance p
		join Measure_Type t
		on p.measure_type_id = t.measure_type_id
		left join Currency c
		on return_currency_id = currency_id
		left join Benchmark	b
		on p.return_benchmark_id = b.benchmark_id
	where return_benchmark_id is not null
),

MarkToMarket as 
(
	select  'Mark to Market Rate' update_type,
			trade_id,
			last_updated,
			cast(track_record_value as varchar) mark_to_market
	from Track_Record 
	where track_record_type_id = 1
),

Interest as 
(
	select  'Interest Rate' update_type,
			trade_id,
			last_updated,
			cast(track_record_value as varchar) interest_rate
	from Track_Record 
	where track_record_type_id = 2
),


Audit as 
(
	select *
	from
	(
		select  update_type,
				trade_id,
				last_updated,
				[status],
				instruction_label,
				instruction_type_label,
				'' mark_to_market,
				'' interest_rate,
				'' spot_carry_date,				
				'' abs_perf_label,
				'' rel_perf_label,
				'' benchmark_label
		from Instructions
			
		union all 

		select  update_type,
				trade_id,		
				last_updated,
				'' [status],
				'' instruction_label,
				'' instruction_type_label,
				'' mark_to_market,
				'' interest_rate,	
				'' spot_carry_date,			
				abs_perf_label,
				'' rel_perf_label,
				'' benchmark_label
		from AbsPerformance
		
		union all 

		select  update_type,
				trade_id,
				last_updated,
				'' [status],
				'' instruction_label,
				'' instruction_type_label,
				'' mark_to_market,
				'' interest_rate,
				'' spot_carry_date,
				'' abs_perf_label,
				rel_perf_label,
				benchmark_label
		from RelPerformance	
		
		union all 

		select  update_type,
				trade_id,
				last_updated,
				'' [status],
				'' instruction_label,
				'' instruction_type_label,								
				mark_to_market,				
				'' interest_rate,
				last_updated spot_carry_date,
				'' abs_perf_label,
				'' rel_perf_label,
				'' benchmark_label
		from MarkToMarket
		
		union all 

		select  update_type,
				trade_id,
				last_updated,
				'' [status],
				'' instruction_label,
				'' instruction_type_label,				
				'' mark_to_market,
				interest_rate,
				last_updated spot_carry_date,
				'' abs_perf_label,
				'' rel_perf_label,
				'' benchmark_label
		from Interest		
	) a
)



select isnull(row_number() over (order by [trade_id]),-1) as id, *
from (
		select  
			
				a.trade_id,
				last_updated						[date],
				max([status])						[status],
				max(instruction_type_label)			instructions_type,
				max(instruction_label)				instructions_comments,		
				max(mark_to_market)					mark_to_market_rate,
				max(interest_rate)					interest_rate,		
				max(isnull(sc.spot,''))				spot,
				max(isnull(sc.carry,''))			carry,		
				max(abs_perf_label)					absolute_performance,	
				max(rel_perf_label)					relative_performance,
				max(benchmark_label)				return_performance_benchmark		
		from Audit  a
			outer apply spot_carry(trade_id, spot_carry_date) sc	
		group by a.trade_id, a.last_updated
)q
