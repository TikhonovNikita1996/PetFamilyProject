using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Discussions.Application.Commands.Discussions.CloseDiscussion;
using PetFamily.Discussions.Application.Commands.Discussions.DeleteMessage;
using PetFamily.Discussions.Application.Commands.Discussions.EditMessage;
using PetFamily.Discussions.Application.Commands.Discussions.WriteMessage;
using PetFamily.Discussions.Presentation.Requests;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;

namespace PetFamily.Discussions.Presentation.Controllers;

[Authorize]
public class DiscussionController : BaseApiController
{
    [Permission(Permissions.Discussions.Delete)]
    [HttpPut("{discussionId:guid}/close")]
    public async Task<ActionResult<Guid>> Close(
        [FromServices] CloseDiscussionHandler handler,
        [FromRoute] Guid discussionId,
        CancellationToken cancellationToken)
    {
        var command = new CloseDiscussionCommand(discussionId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [Permission(Permissions.Discussions.Delete)]
    [HttpPut("{discussionId:guid}/message")]
    public async Task<ActionResult<Guid>> WriteMessage(
        [FromServices] WriteMessageHandler handler,
        [FromRoute] Guid discussionId,
        [FromBody] WriteMessageRequest request,
        CancellationToken cancellationToken)
    {
        var command = new WriteMessageCommand(discussionId, request.SenderId, request.MessageText);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [Permission(Permissions.Discussions.Delete)]
    [HttpPut("{discussionId:guid}/edit-message")]
    public async Task<ActionResult<Guid>> EditMessage(
        [FromServices] EditMessageHandler handler,
        [FromRoute] Guid discussionId,
        [FromBody] EditMessageRequest request,
        CancellationToken cancellationToken)
    {
        var command = new EditMessageCommand(discussionId, request.MessageId,
            request.SenderId, request.MessageText);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [Permission(Permissions.Discussions.Delete)]
    [HttpDelete("{discussionId:guid}/message")]
    public async Task<ActionResult<Guid>> DeleteMessage(
        [FromServices] DeleteMessageHandler handler,
        [FromRoute] Guid discussionId,
        [FromBody] DeleteMessageRequest request,
        CancellationToken cancellationToken)
    {
        var command = new DeleteMessageCommand(discussionId, request.MessageId,
            request.SenderId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
}