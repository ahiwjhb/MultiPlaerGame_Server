using MySql.Data.MySqlClient;
using Network.Protocol;

namespace MultiPlayerGame.Dao
{
    public class UserTableDao
    {
        private const string TableName = "user";

        public string Username { get; }

        public string Password { get; }

        public UserTableDao(string username, string password) {
            Username = username;
            Password = password;
        }

        public static int? QueryIDByUsername(string username) {
            string sql = $"select id from {TableName} where username = @username";

            using var connection = DatabaseConnectionFactory.GetDbConnection();
            MySqlCommand queryCommand = new MySqlCommand(sql, connection);
            queryCommand.Parameters.AddWithValue("@username", username);

            var reader = queryCommand.ExecuteReader();
            if (reader.HasRows) {
                reader.Read();
                return reader.GetInt32(0);
            }
            else {
                return null;
            }
        }

        public static bool QueryUserIsExist(int id) {
            string sql = $"select username from {TableName} where id = @id";

            using var connection = DatabaseConnectionFactory.GetDbConnection();
            var queryCommand = new MySqlCommand(sql, connection);
            queryCommand.Parameters.AddWithValue("@id", id);

            return queryCommand.ExecuteReader().HasRows;
        }

        public static string QueryUserPassword(int id) {
            string sql = $"select password from {TableName} where id = @id";

            using var connection = DatabaseConnectionFactory.GetDbConnection();
            var queryCommand = new MySqlCommand(sql, connection);
            queryCommand.Parameters.AddWithValue("@id", id);

            var reader = queryCommand.ExecuteReader();
            reader.Read();

            return reader.GetString(0);
        }

        public static void InsertToTable(string username, string password) {
            string insertSql = $"insert into {TableName} (username, password) values (@username, @password)";
            var insertCommand = new MySqlCommand(insertSql);
            insertCommand.Parameters.AddWithValue("@username", username);
            insertCommand.Parameters.AddWithValue("@password", password);

            DatabaseConnectionFactory.UseDbConnection(connection => {
                insertCommand.Connection = connection;
                insertCommand.ExecuteNonQuery();
            });
        }

        public static (string name, string profilePhotoBase64) QueryUserInfo(int id) {
            string sql = $"select username from {TableName} where id = @id";

            using var connection = DatabaseConnectionFactory.GetDbConnection();
            var queryCommand = new MySqlCommand(sql, connection);
            queryCommand.Parameters.AddWithValue("@id", id);

            var reader = queryCommand.ExecuteReader();
            reader.Read();
            return (reader.GetString(0), string.Empty);
        }
    }
}
