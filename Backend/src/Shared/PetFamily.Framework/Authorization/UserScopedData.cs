﻿namespace PetFamily.Framework.Authorization;

public class UserScopedData
{
    public Guid UserId { get; set; }

    public List<string> Permissions { get; set; } = [];

    public List<string> Roles { get; set; } = [];
}