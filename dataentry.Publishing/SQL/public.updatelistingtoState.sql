-- PROCEDURE: public.updatelistingtostate(integer, text)

-- DROP PROCEDURE public.updatelistingtostate(integer, text);

CREATE OR REPLACE PROCEDURE public.updatelistingtostate(
	id integer,
	state text)
LANGUAGE 'sql'

AS $BODY$
UPDATE public."ListingData" AS ld
SET "Data" = coalesce("ld"."Data", '{}') || jsonb_build_object('Value', state) 
													|| jsonb_build_object('DateUpdated', NOW()) 
													|| jsonb_build_object('DateListed', 
														CASE WHEN state = 'Published' AND 
														(LEFT("ld"."Data" ->> 'DateListed', 1) = '0' OR "ld"."Data" ->> 'DateListed' = '' IS NOT FALSE)
														THEN to_json(NOW())::jsonb
														ELSE to_json("ld"."Data" ->> 'DateListed')::jsonb END)
WHERE "ld"."ListingID" = ID
	AND "ld"."DataType" = 'PublishingState'
$BODY$;