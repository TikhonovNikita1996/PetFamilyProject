﻿using PetFamily.Domain.ValueObjects;

namespace PetFamily.Domain.Entities.Others;

public record SocialMediaDetails
{
    public List<SocialMedia> SocialMedias { get; }
}