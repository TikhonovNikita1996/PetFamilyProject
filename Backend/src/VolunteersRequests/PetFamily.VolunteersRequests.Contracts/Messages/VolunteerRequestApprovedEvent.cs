﻿using Pet.Family.SharedKernel;

namespace PetFamily.VolunteersRequests.Contracts.Messages;

public record VolunteerRequestApprovedEvent(Guid UserId) : IDomainEvent;