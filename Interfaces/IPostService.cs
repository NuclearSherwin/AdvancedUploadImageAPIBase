using FileUploadBase.Models;

namespace FileUploadBase.Interfaces;

public interface IPostService
{
    Task SavePostImageAsync(PostRequest postRequest);
    Task<PostResponse> CreatePostAsync(PostRequest postRequest);
}