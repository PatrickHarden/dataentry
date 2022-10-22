-- FUNCTION: public.getlistingsbystate(text)

DROP FUNCTION IF EXISTS public.getlistingsbystate(text);

CREATE FUNCTION public.getlistingsbystate(
	state text)
    RETURNS TABLE("ID" integer, "ExternalID" text, "State" text, "DateUpdated" text, "HomeSiteID" text) 
    LANGUAGE 'sql'

    COST 100
    VOLATILE 
    ROWS 1000
AS $BODY$
	SELECT
		"li"."ID",
		"li"."ExternalID",
		"ld"."Data"->>'Value',
		"ld"."Data"->>'DateUpdated',
		"r"."HomeSiteID"
	FROM public."Listings" AS "li"
	INNER JOIN public."ListingData" AS "ld"
		ON "li"."ID" = "ld"."ListingID"
	LEFT JOIN public."Regions" r 
		ON "li"."RegionID" = r."ID"
	WHERE "ld"."DataType" = 'PublishingState'
	AND "ld"."Data"->>'Value' = state
$BODY$;
