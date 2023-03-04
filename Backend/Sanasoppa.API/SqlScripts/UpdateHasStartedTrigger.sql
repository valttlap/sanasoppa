CREATE OR REPLACE FUNCTION update_has_started()
RETURNS TRIGGER
LANGUAGE plpgsql
AS $$
BEGIN
	IF NEW.GameState = 1 THEN
		NEW.HasStarted = FALSE;
	ELSE
		NEW.HsStarted = TRUE;
	END IF;

	RETURN NEW;
END;
$$;

CREATE TRIGGER update_has_started_trigger
BEFORE INSERT OR UPDATE OF GameState ON games
FOR EACH ROW
EXECUTE FUNCTION update_has_started();