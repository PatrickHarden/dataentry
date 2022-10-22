SELECT l.* FROM "Listings" l
WHERE l."IsParent" = TRUE
AND EXISTS (
	SELECT 1 
	FROM "user"."AspNetUserClaims" uc
	WHERE uc."UserId" = @userId
		AND uc."ClaimType" = 'ListingClaim'
		AND uc."ClaimValue" = l."ID"::text
	
	UNION
	
	SELECT 1 
	FROM "user"."AspNetUserRoles" ur
	INNER JOIN "user"."AspNetRoleClaims" rc
		ON ur."RoleId" = rc."RoleId"
	WHERE ur."UserId" = @userId
		AND rc."ClaimType" = 'ListingClaim'
		AND rc."ClaimValue" = l."ID"::text)