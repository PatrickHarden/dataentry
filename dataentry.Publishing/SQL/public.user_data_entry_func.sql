DO $$
DECLARE
  	rolCount bigint := 0;
BEGIN 
	SELECT INTO rolCount COUNT(*)
		FROM pg_roles
		WHERE rolname='data_entry_func';
		
	IF rolCount = 0 THEN
		CREATE USER data_entry_func 
			WITH LOGIN NOSUPERUSER NOCREATEDB NOCREATEROLE 
			INHERIT NOREPLICATION 
			CONNECTION LIMIT -1 
			PASSWORD ':"postgresFuncPassword"';
	END IF;
	
	GRANT SELECT
	ON ALL TABLES IN SCHEMA public
	TO data_entry_func;

	GRANT EXECUTE 
	ON FUNCTION public.getlistingsbystate
	TO data_entry_func;

	GRANT EXECUTE 
	ON PROCEDURE public.updatelistingtostate
	TO data_entry_func;

	GRANT UPDATE
	ON TABLE public."ListingData"
	TO data_entry_func;
END $$;