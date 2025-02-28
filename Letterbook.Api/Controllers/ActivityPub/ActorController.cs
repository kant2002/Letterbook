﻿using System.Reflection;
using Letterbook.Core;
using Letterbook.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Letterbook.Api.Controllers.ActivityPub;

/// <summary>
/// Provides the endpoints specified for Actors in the ActivityPub spec
/// https://www.w3.org/TR/activitypub/#actors
/// </summary>
[ApiController]
[Route("[controller]")]
public class ActorController
{
    private readonly SnakeCaseRouteTransformer _transformer = new();
    private readonly Uri _baseUri;
    private readonly ILogger<ActorController> _logger;
    private readonly IActivityService _activityService;

    public ActorController(IOptions<CoreOptions> config, ILogger<ActorController> logger,
        IActivityService activityService)
    {
        _baseUri = new Uri($"{config.Value.Scheme}://{config.Value.DomainName}");
        _logger = logger;
        _activityService = activityService;
    }


    [HttpGet]
    [Route("{id}")]
    public ActionResult<DTO.Actor> GetActor(int id)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    [ActionName("Followers")]
    [Route("{id}/collections/[action]")]
    public IActionResult GetFollowers(int id)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    [ActionName("Following")]
    [Route("{id}/collections/[action]")]
    public IActionResult GetFollowing(int id)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    [ActionName("Liked")]
    [Route("{id}/collections/[action]")]
    public IActionResult GetLiked(int id)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    [ActionName("Inbox")]
    [Route("{id}/[action]")]
    public IActionResult GetInbox(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    [ActionName("Inbox")]
    [Route("{id}/[action]")]
    public IActionResult PostInbox(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    [Route("[action]")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<ActionResult> SharedInbox(DTO.Activity activity)
    {
        await _activityService.ReceiveNotes(new Note[]{}, Enum.Parse<ActivityType>(activity.Type), null);
        return new AcceptedResult();
    }

    [HttpGet]
    [ActionName("Outbox")]
    [Route("{id}/[action]")]
    public IActionResult GetOutbox(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    [ActionName("Outbox")]
    [Route("{id}/[action]")]
    public IActionResult PostOutbox(int id)
    {
        throw new NotImplementedException();
    }

    private Uri CollectionUri(string actionName, string id)
    {
        var (action, routeTemplate) = ActionAttributes(actionName);
        var route = "/actor/" + routeTemplate
            .Replace("[action]", action)
            .Replace("{id}", id);
        var transformed = string.Join("/", route
            .Split("/")
            .Select(part => _transformer.TransformOutbound(part)));
        var result = new Uri(_baseUri, transformed);

        return result;
    }

    private static (string action, string route) ActionAttributes(string action)
    {
        var method = typeof(ActorController).GetMethod(action);

        var actionName = (method ?? throw new InvalidOperationException($"no method with name {action}"))
            .GetCustomAttribute<ActionNameAttribute>();
        var route = method.GetCustomAttribute<RouteAttribute>();
        if (route == null) throw new InvalidOperationException($"no route for action {action}");
        return (actionName?.Name ?? method.Name, route.Template);
    }
}