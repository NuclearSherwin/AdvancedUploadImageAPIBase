using System.ComponentModel.DataAnnotations;
using FileUploadBase.Interfaces;
using FileUploadBase.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadBase.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly ILogger<PostsController> _logger;
    private readonly IPostService _postService;


    public PostsController(ILogger<PostsController> logger, IPostService postService)
    {
        this._logger = logger;
        this._postService = postService;
    }


    [HttpPost]
    [Route("")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<IActionResult> SubmitPost([FromForm] PostRequest postRequest)
    {
        if (postRequest == null)
        {
            return BadRequest(new PostResponse { Success = false, ErrorCode = "501", Error = "Invalid post request" });
        }

        if (string.IsNullOrEmpty(Request.GetMultipartBoundary()))
        {
            return BadRequest(new PostResponse { Success = false, ErrorCode = "502", Error = "Invalid post header" });
        }

        if (postRequest.Image != null)
        {
            await _postService.SavePostImageAsync(postRequest);
        }

        var postResponse = await _postService.CreatePostAsync(postRequest);
        if (!postResponse.Success)
        {
            return NotFound(postResponse);
        }

        return Ok(postResponse.Post);
    }
}