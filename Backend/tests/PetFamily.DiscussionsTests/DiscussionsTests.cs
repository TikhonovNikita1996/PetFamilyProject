using PetFamily.Discussions.Domain;

namespace PetFamily.DiscussionsTests;

public class DiscussionsTests
{
    private Discussion CreateDiscussion()
    {
        var users = DiscussionUsers.Create(Guid.NewGuid(),Guid.NewGuid());
        var discussion = Discussion.Create(users).Value;
        
        return discussion;
    }
    
    [Fact]
    public void CreateDiscussion_Should_Be_Not_Null()
    {
        // Arrange
        var users = DiscussionUsers.Create(Guid.NewGuid(),Guid.NewGuid());
        
        // Act
        var discussion = Discussion.Create(users).Value;
        
        //Assert
        Assert.NotNull(discussion);
    }
    
    [Fact]
    public void CloseDiscussion_Status_Should_Be_Closed()
    {
        // Arrange
        var users = DiscussionUsers.Create(Guid.NewGuid(),Guid.NewGuid());
        var discussion = Discussion.Create(users).Value;
        
        // Act
        discussion.Close();
        
        //Assert
        Assert.Equal(DiscussionStatus.Closed, discussion.Status);
    }
    
    [Fact]
    public void Add_Message_To_Discussion_Should_Be_Not_Null()
    {
        // Arrange
        var discussion = CreateDiscussion();
        var userId = discussion.DiscussionUsers.ApplicantUserId;
        var messageText = MessageText.Create("Hello World").Value;
        var message = Message.Create(discussion.DiscussionId, userId, messageText);
        
        // Act
        discussion.AddMessage(message);
        
        //Assert
        Assert.Single(discussion.Messages);
    }
    
    [Fact]
    public void Add_Message_To_Discussion_When_User_Is_Not_In_Discussion_Should_Be_Failure()
    {
        // Arrange
        var discussion = CreateDiscussion();
        var messageText = MessageText.Create("Hello World").Value;
        var message = Message.Create(discussion.DiscussionId, Guid.NewGuid(), messageText);
        
        // Act
        var result = discussion.AddMessage(message);
        
        //Assert
        Assert.True(result.IsFailure);
    }
    
    [Fact]
    public void Remove_Message_From_Discussion_When_User_Is_Not_In_Discussion_Should_Be_Failure()
    {
        // Arrange
        var discussion = CreateDiscussion();
        var userId = discussion.DiscussionUsers.ApplicantUserId;
        var messageText = MessageText.Create("Hello World").Value;
        var message = Message.Create(discussion.DiscussionId, userId, messageText);
        discussion.AddMessage(message);
        
        // Act
        var result = discussion.RemoveMessage(message.MessageId, Guid.NewGuid());
        
        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(result.Error.Message, "You can delete only your messages.");
    }
    
    [Fact]
    public void Edit_Message_In_Discussion_When_User_Is_Not_In_Discussion_Should_Be_Failure()
    {
        // Arrange
        var discussion = CreateDiscussion();
        var userId = discussion.DiscussionUsers.ApplicantUserId;
        var messageText = MessageText.Create("Hello World").Value;
        var message = Message.Create(discussion.DiscussionId, userId, messageText);
        discussion.AddMessage(message);
        var newMessageText = MessageText.Create("New Message").Value;
        
        // Act
        var result = discussion.EditMessage(message.MessageId, Guid.NewGuid() ,newMessageText);
        
        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(result.Error.Message, "You can edit only your messages.");
    }
    
}