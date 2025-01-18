using PetFamily.VolunteersRequests.Domain;
using PetFamily.VolunteersRequests.Domain.Enums;
using PetFamily.VolunteersRequests.Domain.ValueObjects;

namespace PetFamily.VolunteersRequestsTests;

public class VolunteersRequestsTests
{
    private VolunteerRequest CreateTestRequest()
    {
        var volunteerInfo = VolunteerInfo.Create("John Doe").Value;
        var userId = Guid.NewGuid();
        var request = VolunteerRequest.Create(userId, volunteerInfo);
        
        return request.Value;
    }
    
    [Fact]
    public void CreateRequest_Should_Be_Not_Null()
    {
        // Arrange
        var volunteerInfo = VolunteerInfo.Create("John Doe").Value;
        var userId = Guid.NewGuid();
        
        // Act
        var request = VolunteerRequest.Create(userId, volunteerInfo);
        
        //Assert
        Assert.NotNull(request);
        Assert.Equal(userId, request.Value.UserId);
        Assert.Equal(RequestStatus.Submitted, request.Value.Status);
    }
    
    [Fact]
    public void ChangeStatus_To_Approved()
    {
        // Arrange
        var request = CreateTestRequest();
        
        // Act
        request.SetApprovedStatus(Guid.NewGuid());
        
        //Assert
        Assert.Equal(RequestStatus.Approved, request.Status);
    }
    
    [Fact]
    public void ChangeStatus_To_RevisionRequired()
    {
        // Arrange
        var request = CreateTestRequest();
        
        // Act
        request.SetRevisionRequiredStatus(Guid.NewGuid(), RejectionComment.Create("John Doe").Value);
        
        //Assert
        Assert.Equal(RequestStatus.RevisionRequired, request.Status);
    }
    
    [Fact]
    public void ChangeStatus_To_OnReview()
    {
        // Arrange
        var request = CreateTestRequest();
        
        // Act
        request.TakeInReview(Guid.NewGuid());
        
        //Assert
        Assert.Equal(RequestStatus.OnReview, request.Status);
    }
    
    [Fact]
    public void ChangeStatus_To_Rejected()
    {
        // Arrange
        var request = CreateTestRequest();
        
        // Act
        request.SetRejectStatus(Guid.NewGuid());
        
        //Assert
        Assert.Equal(RequestStatus.Rejected, request.Status);
    }
}