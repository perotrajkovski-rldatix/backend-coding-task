using Claims.DTOs.Claims;
using Claims.DTOs.Covers;

namespace Claims.DTOs;

public static class DtoMappings
{
    public static Claim ToDomain(this CreateClaimRequestDto request)
    {
        return new Claim
        {
            CoverId = request.CoverId,
            Created = request.Created,
            Name = request.Name,
            Type = request.Type,
            DamageCost = request.DamageCost
        };
    }

    public static ClaimResponseDto ToResponse(this Claim claim)
    {
        return new ClaimResponseDto
        {
            Id = claim.Id,
            CoverId = claim.CoverId,
            Created = claim.Created,
            Name = claim.Name,
            Type = claim.Type,
            DamageCost = claim.DamageCost
        };
    }

    public static Cover ToDomain(this CreateCoverRequestDto request)
    {
        return new Cover
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Type = request.Type
        };
    }

    public static CoverResponseDto ToResponse(this Cover cover)
    {
        return new CoverResponseDto
        {
            Id = cover.Id,
            StartDate = cover.StartDate,
            EndDate = cover.EndDate,
            Type = cover.Type,
            Premium = cover.Premium
        };
    }
}
