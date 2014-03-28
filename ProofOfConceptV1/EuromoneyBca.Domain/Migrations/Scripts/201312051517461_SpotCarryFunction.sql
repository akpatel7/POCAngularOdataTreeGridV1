CREATE FUNCTION spot_carry 
(
	@trade_id			int,
	@spot_carry_date	datetime
)
RETURNS @spot_carry TABLE 
(
	trade_id	int,
	spot		varchar(50),
	carry		varchar(50)
)
AS
BEGIN

		with Metrics as
		(
			select tr.trade_id
				 ,ti.instruction_entry
				 ,tr.track_record_value
				 ,tr.track_record_type_id
				 ,case 
					when datediff(ww,ti.instruction_entry_date, isnull(ti.instruction_exit_date, getdate())) > 13 
						then 13
					else datediff(ww,ti.instruction_entry_date, isnull(ti.instruction_exit_date, getdate()))
				  end weeks_held
				 ,row_number() over (partition by tr.trade_id, track_record_type_id order by tr.track_record_date desc) spot_rank
				 ,row_number() over (partition by tr.trade_id, track_record_type_id, datediff(ww,ti.instruction_entry_date,track_record_date)/13 order by tr.track_record_date) carry_rank
			from Track_Record tr
			left join Trade_Instruction ti
				on tr.trade_id = ti.trade_id
				and ti.instruction_entry_date is not null	
			where tr.last_updated <= @spot_carry_date
				and tr.trade_id = @trade_id
		)

		insert into @spot_carry
		select	 tl.trade_id	
				,isnull(cast(avg(cast(case 
									when m.track_record_type_id = 1 and m.spot_rank = 1 
										then case 
												when position_id in (2,4,6,7) -- Sell
													then ((m.instruction_entry / m.track_record_value) - 1) * 100
												else ((m.track_record_value / m.instruction_entry) - 1) * 100
											end	
									else null
								end as decimal(18,2))) as varchar),'') spot
				,isnull(cast(sum(cast(case 
									when m.track_record_type_id = 2 and m.carry_rank = 1 
										then (power(power((1 + (m.track_record_value/cast(100 as decimal))),(1/cast(52 as decimal))),m.weeks_held) - 1) * 100
									else null
								end as decimal(18,2))) as varchar),'') carry
		from Trade_Line tl
		join Metrics m
			on tl.trade_id = m.trade_id
				and (m.spot_rank = 1 or m.carry_rank = 1)
		group by tl.trade_id
				,tl.trade_line_id
				,tl.trade_line_label
				,m.instruction_entry


	
	RETURN 
END