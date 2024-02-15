using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using workshop.webapi.DataModels;
using workshop.webapi.DataTransfer.Requests;
using workshop.webapi.Repository;

namespace workshop.webapi.Endpoints
{
    public static class ClientEndpoint
    {
        public static void ConfigureClientEndpoint(this WebApplication app)
        {

            var blogGroup = app.MapGroup("blogs");
            blogGroup.MapGet("/", GetAll);
            blogGroup.MapPost("/", AddBlog).AddEndpointFilter(async (invocationContext, next) =>
            {
                Blog blog = invocationContext.GetArgument<Blog>(1);

                if (string.IsNullOrEmpty(blog.BlogTitle))
                    return Results.BadRequest("You must enter a valid blog title");
                
                return await next(invocationContext);
            }); ;
            blogGroup.MapGet("/{id}", GetById);
            blogGroup.MapPut("/{id}", Update);
            blogGroup.MapDelete("/{id}", Delete);

        }
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> Delete(IRepository<Blog> repository, ClaimsPrincipal user, int id)
        {
            Blog? entity = await repository.GetById(id);
            if (entity == null)
                return TypedResults.NotFound($"Could not find Car with Id:{id}");
            
            Blog? result = await repository.Delete(entity);
            return (result != null) ? TypedResults.Ok(new { DateTime=DateTime.Now, User=user.Email(), Blog = new { BlogID = result.Id, BlogTitle = result.BlogTitle }}) : TypedResults.BadRequest($"Car wasn't deleted");
        }
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Update(IRepository<Blog> repository, int id, BlogRequest request)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
                return TypedResults.NotFound($"Could not find Car with Id:{id}");
            
            entity.BlogTitle = request.BlogTitle ?? entity.BlogTitle;

            var result = await repository.Update(entity);

            return (result != null) ? TypedResults.Ok(result) : TypedResults.BadRequest("Couldn't save to the database?!");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAll(IRepository<Blog> repository)
        {
            return TypedResults.Ok(await repository.Get());
        }
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetById(IRepository<Blog> repository, int id)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
                return TypedResults.NotFound($"Could not find Car with Id:{id}");
            
            return TypedResults.Ok(entity);
        }
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> AddBlog(IRepository<Blog> repository, BlogRequest request)
        {
            var results = await repository.Get();

            if (results.Any(x => x.BlogTitle.Equals(request.BlogTitle, StringComparison.OrdinalIgnoreCase)))
            {
                return Results.BadRequest("Blog with provided title already exists");
            }

            var entity = new Blog() { BlogTitle = request.BlogTitle };
            await repository.Insert(entity);
            return TypedResults.Created($"/{entity.Id}", new { entity.BlogTitle });

        }
    }
}
