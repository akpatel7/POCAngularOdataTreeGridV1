CREATE view [dbo].[TradeLookupData] as 

with Data as
(

SELECT
	*
FROM (
	SELECT DISTINCT TOP (100) PERCENT
		'structure_type_label' AS field,
		dbo.Structure_Type.structure_type_label AS value,
		dbo.Trade.trade_id as trade_id,
		dbo.Structure_Type.structure_type_label AS label
	FROM dbo.Trade
	INNER JOIN dbo.Structure_Type
	ON dbo.Trade.structure_type_id = dbo.Structure_Type.structure_type_id 
	
	UNION 
	
	SELECT DISTINCT TOP (100) PERCENT
		'position_label' AS field,
		dbo.Position.position_label AS value,
		dbo.Trade_Line.trade_id as trade_id,
		dbo.Position.position_label AS label
	FROM dbo.Trade_Line
	INNER JOIN dbo.Position
	ON dbo.Trade_Line.position_id = dbo.Position.position_id 
	
	UNION
	
	SELECT DISTINCT TOP (100) PERCENT
		'tradable_thing_label' AS field,
		dbo.Tradable_Thing.tradable_thing_label AS value,
		dbo.Trade_Line.trade_id as trade_id,
		dbo.Tradable_Thing.tradable_thing_label AS label
	FROM dbo.Tradable_Thing
	INNER JOIN dbo.Trade_Line
	ON dbo.Tradable_Thing.tradable_thing_id = dbo.Trade_Line.tradable_thing_id 
	
	UNION
	
	SELECT DISTINCT TOP (100) PERCENT 
		'instruction_type_label' AS field, 
		dbo.Instruction_Type.instruction_type_label AS value, 
		dbo.Trade_Instruction.trade_id as trade_id,
		dbo.Instruction_Type.instruction_type_label AS label 
	FROM dbo.Trade_Instruction 
	INNER JOIN dbo.Instruction_Type ON dbo.Trade_Instruction.instruction_type_id = dbo.Instruction_Type.instruction_type_id

	UNION 

	SELECT DISTINCT TOP (100) PERCENT
		'tradable_thing_label' AS field,
		dbo.Tradable_Thing.tradable_thing_label AS value,
		dbo.Trade_Line.trade_id AS trade_id,
		dbo.Tradable_Thing.tradable_thing_label AS label
	FROM dbo.Tradable_Thing
	INNER JOIN dbo.Trade_Line
	ON dbo.Tradable_Thing.tradable_thing_id = dbo.Trade_Line.tradable_thing_id 
	
	UNION 
	

	SELECT DISTINCT TOP (100) PERCENT
		'benchmark_label' AS field,
		dbo.Benchmark.benchmark_label AS value, 
		dbo.Trade_Performance.trade_id, 
		dbo.Benchmark.benchmark_label AS label
	FROM dbo.Benchmark
	INNER JOIN dbo.Trade_Performance 
	ON dbo.Benchmark.benchmark_id = dbo.Trade_Performance.return_benchmark_id
	
	UNION
	
	SELECT DISTINCT TOP (100) PERCENT 
		'service_code' AS field, 
		dbo.Service.service_code AS value, 
		dbo.Trade.trade_id as trade_id, 
        dbo.Service.service_code + ' - ' + dbo.Service.service_label AS label
	FROM dbo.Trade 
	INNER JOIN dbo.Service 
	ON dbo.Trade.service_id = dbo.Service.service_id
	
	UNION
	
	SELECT DISTINCT TOP (100) PERCENT 
			'tradable_thing_class_editorial_label' AS field, 
			dbo.Tradable_Thing_Class.tradable_thing_class_editorial_label AS value, 
			dbo.Trade_Line.trade_id as trade_id, 
			dbo.Tradable_Thing_Class.tradable_thing_class_editorial_label AS label
		FROM dbo.Tradable_Thing 
		INNER JOIN dbo.Trade_Line 
		ON dbo.Tradable_Thing.tradable_thing_id = dbo.Trade_Line.tradable_thing_id 
		INNER JOIN dbo.Tradable_Thing_Class 
		ON dbo.Tradable_Thing.tradable_thing_class_id = dbo.Tradable_Thing_Class.tradable_thing_class_id
		
	UNION
	
	SELECT DISTINCT TOP (100) PERCENT 
		'length_type_label' AS field, 
		dbo.Length_Type.length_type_label AS value, 
		dbo.Trade.trade_id as trade_id, 
        dbo.Length_Type.length_type_label AS label
	FROM dbo.Length_Type 
	INNER JOIN dbo.Trade 
	ON dbo.Length_Type.length_type_id = dbo.Trade.length_type_id
	
	UNION
	
	SELECT DISTINCT TOP (100) PERCENT 
			'isOpen' AS field, 
			CASE WHEN instruction_exit_date IS NULL THEN 'true' ELSE 'false' END AS value, 
			dbo.Trade.trade_id as trade_id, 
			MAX(CASE WHEN instruction_exit_date IS NULL THEN 'Open' ELSE 'Closed' END) AS label
		FROM dbo.Instruction_Type 
		INNER JOIN dbo.Trade_Instruction 
		ON dbo.Instruction_Type.instruction_type_id = dbo.Trade_Instruction.instruction_type_id 
		RIGHT OUTER JOIN dbo.Trade 
		ON dbo.Trade_Instruction.trade_id = dbo.Trade.trade_id
		GROUP BY dbo.Trade.trade_id, CASE WHEN instruction_exit_date IS NULL  THEN 'true' ELSE 'false' END
	
) d)

SELECT
	ISNULL(ROW_NUMBER() OVER (ORDER BY [field], [value], [trade_id]), -1) AS id,
	*
FROM (SELECT
	d.field,
	d.value,
	d.trade_id,
	d.label
FROM Data d) q


