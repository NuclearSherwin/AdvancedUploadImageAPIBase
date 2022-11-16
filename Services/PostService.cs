using FileUploadBase.DataContext;
using FileUploadBase.Interfaces;
using FileUploadBase.Models;

namespace FileUploadBase.Services;

public class PostService : IPostService
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _environment;

    public PostService(ApplicationDbContext db, IWebHostEnvironment environment)
    {
        _db = db;
        _environment = environment;
    }


    public async Task SavePostImageAsync(PostRequest postRequest)
    {
        var uniqueFileName = FileHelper.GetUniqueFileName(postRequest.Image.FileName);
        var uploads = Path.Combine(_environment.WebRootPath, "users", "posts", postRequest.UserId.ToString());
        
        var filePath = Path.Combine(uploads, uniqueFileName);


        Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? string.Empty);
            
            await postRequest.Image.CopyToAsync(new FileStream(filePath, FileMode.Create));
        postRequest.ImagePath = filePath;
        Console.WriteLine("Save post successfully");
        return;
    }

    public async Task<PostResponse> CreatePostAsync(PostRequest postRequest)
    {

        var post = new Post

        {
            
            UserId = postRequest.UserId,


            Description = postRequest.Description,


            Imagepath = postRequest.ImagePath,


            Ts = DateTime.Now,


            Published = true
            
        };

        var postEntry = await _db.Posts.AddAsync(post);
        var saveReponse = await _db.SaveChangesAsync();
        if (saveReponse < 0)
        {
            return new PostResponse { Success = false, Error = "Issue while saving the post", ErrorCode = "CP01" };
        }

        var postEntity = postEntry.Entity;
        var postModel = new PostModel
        {
            Id = postEntity.Id,
            Description = postEntity.Description,
            Ts = post.Ts,
            Imagepath = Path.Combine(postEntity.Imagepath),
            UserId = postEntity.UserId
        };

        return new PostResponse {Success = true, Post = postModel};
    }
}