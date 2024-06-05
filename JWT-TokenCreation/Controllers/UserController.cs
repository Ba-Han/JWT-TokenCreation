using JWT_TokenCreation.Models.Entities;
using JWT_TokenCreation.Models.LoginRequestModel;
using JWT_TokenCreation.Models.LoginResponseModel;
using JWT_TokenCreation.Query;
using JWT_TokenCreation.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_TokenCreation.Controllers;
public class UserController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly AdoDotNetService _adoDotNetService;

    public UserController(IConfiguration configuration, AdoDotNetService adoDotNetService)
    {
        _configuration = configuration;
        _adoDotNetService = adoDotNetService;
    }

    [HttpGet]
    [Route("api/user")]
    public IActionResult GetLoginUser()
    {
        try
        {
            string getQuery = UserQuery.GetUserRecordQuery();
            List<SqlParameter> getPara = new List<SqlParameter>()
            {
                new SqlParameter("@IsActive", true)
            };

            List<UserModel> lst = _adoDotNetService.Query<UserModel>(getQuery, getPara.ToArray());

            return Ok(lst);

        }catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpGet]
    [Route("api/user/{id}")]
    public IActionResult GetEachLoginUser(int id)
    {
        try
        {
            string getQuery = UserQuery.GetEachUserQuery();
            List<SqlParameter> getPara = new List<SqlParameter>()
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@IsActive", true)
            };

            List<LoginResponseModel> lst = _adoDotNetService.Query<LoginResponseModel>(getQuery, getPara.ToArray());
            if (lst.Count == 0)
                return BadRequest("This user not found.");

            return Ok(lst);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPost]
    [Route("api/login")]
    public IActionResult Login([FromBody] LoginRequestModel loginRequestModel)
    {
        try
        {
            if (loginRequestModel is null || string.IsNullOrEmpty(loginRequestModel.Email) || string.IsNullOrEmpty(loginRequestModel.Password))
                return BadRequest("Email or Password can not be empty.");

            string query = UserQuery.GetUserQuery();
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Email", loginRequestModel.Email),
                new SqlParameter("@Password", loginRequestModel.Password),
                new SqlParameter("IsActive", true)
            };

            List<UserModel> lst = _adoDotNetService.Query<UserModel>(query, parameters.ToArray());
            if (lst is null)
                return BadRequest("User Not Found. Login Fail");

            UserModel userDataModel = lst[0];

            return StatusCode(202, new
            {
                access_token = GenerateTokenDetail(userDataModel)
            });

        }catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpDelete]
    [Route("api/user/{id}")]
    public IActionResult GetLoginUser(int id)
    {
        try
        {
            string getQuery = UserQuery.GetDeleteUserQuery();
            List<SqlParameter> getPara = new List<SqlParameter>()
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@IsActive", false)
            };

            int removeUser = _adoDotNetService.Execute(getQuery, getPara.ToArray());
            if (removeUser == 0)
                return BadRequest("Deleting Fail.");

            return Ok("Deleting Successfully.");

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private string GenerateTokenDetail(UserModel user)
    {
        string issuerString = _configuration["Jwt:Issuer"]!;
        string audienceString = _configuration["Jwt:Audience"]!;
        string keyString = _configuration["Jwt:Key"]!;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier,user.Email)
        };

        var token = new JwtSecurityToken(
            issuerString,
            audienceString,
            claims,
            expires: DateTime.Now.AddMonths(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
