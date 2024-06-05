using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace JWT_TokenCreation.Services;
public class AdoDotNetService
{
    private readonly IConfiguration _configuration;

    public AdoDotNetService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #region Query
    public List<T> Query<T> (string query, SqlParameter[]? parameters = null)
    {
        SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        conn.Open();
        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddRange(parameters);
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        adapter.Fill(dt);
        conn.Close();

        string jsonStr = JsonConvert.SerializeObject(dt);
        List<T> resStr = JsonConvert.DeserializeObject<List<T>>(jsonStr)!;

        return resStr;
    }
    #endregion

    #region QueryFirstOrDefault
    public DataTable QueryFirstOrDefault(string query, SqlParameter[]? parameters = null)
    {
        SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        conn.Open();
        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddRange(parameters);
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        adapter.Fill(dt);
        conn.Close();

        return dt;
    }
    #endregion

    #region Execute
    public int Execute(string query, SqlParameter[]? parameters = null)
    {
        SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        conn.Open();
        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddRange(parameters);
        int resExecute = cmd.ExecuteNonQuery();
        conn.Close();

        return resExecute;
    }
    #endregion

}
