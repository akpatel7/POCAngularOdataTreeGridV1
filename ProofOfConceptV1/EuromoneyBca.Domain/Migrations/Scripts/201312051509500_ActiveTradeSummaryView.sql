CREATE view [dbo].[ActiveTradeSummary] as
select t.trade_id,
       t.trade_uri
        ,s.service_code                                    
        ,lt.length_type_label                              
        ,t.trade_editorial_label                           
        ,st.structure_type_label                           
		,t.last_updated
        ,ti.instruction_entry                              
        ,ti.instruction_entry_date                         
        ,ti.instruction_exit                               
        ,ti.instruction_exit_date                          
        ,ti.instruction_label                              
        ,it.instruction_type_label						
        ,tp_abs.return_value [absolute_performance]		
        ,tp_abs.measure_type_label [absolute_measure_type]
        ,tp_abs.currency_code [absolute_currency_code]
        ,tp_rel.return_value [relative_performance]		
        ,tp_rel.measure_type_label [relative_measure_type]
        ,tp_rel.benchmark_label
        ,tp_rel.currency_code [relative_currency_code]
		,cast(case when ti.instruction_exit_date is null then 1 else 0 end as bit) [isOpen]
        ,cast(case when ti.instruction_exit_date is not null and datediff(dd,getdate(), ti.instruction_exit_date) <= -7 then 1 else 0 end as bit) [isClosedFor7DaysOrMore]
    from Trade t
    join Structure_Type st
            on st.structure_type_id = t.structure_type_id
    join [Service] s
            on t.service_id = s.service_id
    join Length_Type lt
            on t.length_type_id = lt.length_type_id
    join (select trade_id
                            ,instruction_entry
                            ,instruction_entry_date
                            ,instruction_exit
                            ,instruction_exit_date
                            ,instruction_type_id
                            ,instruction_label
                            ,row_number() over (partition by trade_id order by last_updated desc) most_recent        
              from Trade_Instruction) ti
            on t.trade_id = ti.trade_id
                    and ti.most_recent = 1
    join Instruction_Type it
            on it.instruction_type_id  = ti.instruction_type_id

    left join (select return_value
                                     ,measure_type_label
                                     ,trade_id
                                     ,return_benchmark_id
                                     ,b.benchmark_label
                                     ,c.currency_code
                                     ,row_number() over (partition by trade_id order by last_updated desc) most_recent        
            from Trade_Performance tp
                    join Measure_Type mt on mt.measure_type_id = tp.measure_type_id
                    join Benchmark b on b.benchmark_id = tp.return_benchmark_id
                    left join Currency c on c.currency_id = tp.return_currency_id
            where return_benchmark_id is not null) tp_rel        
            on tp_rel.trade_id = t.trade_id
            and tp_rel.most_recent = 1

    left join (select return_value
                                     ,measure_type_label
                                     ,trade_id
                                     ,return_benchmark_id
                                     ,c.currency_code
                                     ,row_number() over (partition by trade_id order by last_updated desc) most_recent        
            from Trade_Performance tp
                    join Measure_Type mt on mt.measure_type_id = tp.measure_type_id
                    left join Currency c on c.currency_id = tp.return_currency_id
            where return_benchmark_id is null) tp_abs
            on tp_abs.trade_id = t.trade_id
            and tp_abs.most_recent = 1
	where  [status] = 1
