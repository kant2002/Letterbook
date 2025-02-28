﻿using Letterbook.Core.Extensions;

namespace Letterbook.Core.Models;

/// <summary>
/// A Profile is the externally visible representation of an account on the network. In ActivityPub terms, it should map
/// 1:1 with Actors.
/// Local profiles are managed by one or more Accounts, which are the representation of a user internally to the system.
/// Remote profiles have no associated Accounts, and can only be created or modified by federated changes to the remote
/// Actor.
/// </summary>
public class Profile : IObjectRef
{
    private Profile()
    {
        Id = default!;
        Type = default;
        Handle = default!;
        DisplayName = default!;
    }

    public Profile(Uri id)
    {
        Id = id;
    }

    public Uri Id { get; set; }
    public string? LocalId { get; set; }
    public string Authority => Id.Authority;
    public string Handle { get; set; }
    public string DisplayName { get; set; }

    // Local profiles should all have an owner, but remote ones do not.
    // Could remote profiles be claimed through an account transfer?
    public Account? OwnedBy { get; set; }
    public ActivityActorType Type { get; set; }
    public ICollection<Audience> Audiences { get; set; } = new HashSet<Audience>();
    public ICollection<LinkedProfile> RelatedAccounts { get; set; } = new HashSet<LinkedProfile>();

    public static Profile CreatePerson(Uri baseUri, string handle)
    {
        var localId = ShortId.NewShortId();
        var profile = new Profile
        {
            Id = new Uri(baseUri, $"/actor/{localId}"),
            LocalId = localId,
            Type = ActivityActorType.Person,
            Handle = $@"{handle}@{baseUri.Authority}",
            DisplayName = handle,
        };
        return profile;
    }
}