using MySql.Data.MySqlClient;
using System.Data.Common;

namespace MultiPlayerGame.Dao
{
    internal class DatabaseConnectionFactory
    {
        private static MySqlConnectionStringBuilder _connectionStringBuilder;

        public static string MySqlConnectionString => _connectionStringBuilder.ConnectionString;

        static DatabaseConnectionFactory() {
            _connectionStringBuilder = new MySqlConnectionStringBuilder() {
                Server = "localhost",
                Port = 3306,
                UserID = "root",
                Password = "123456",
                Database = "userdata",
                CharacterSet = "utf8",
                Pooling = true
            };
        }

        public static MySqlConnection GetDbConnection() {
            var connection = CreateMySqlConnection();
            connection.Open();
            return connection;
        }

        public static void UseDbConnection(Action<MySqlConnection> action) {
            using (var connection = CreateMySqlConnection()) {
                connection.Open();
                action(connection);
            }
        }

        public static TResult UseDbConnection<TResult>(Func<MySqlConnection, TResult> action) {
            using (var connection = CreateMySqlConnection()) {
                connection.Open();
                return action(connection);
            }
        }

        public static async Task UseDbConnectionAsync(Func<MySqlConnection, Task> action) {
            using (var connection = CreateMySqlConnection()) {
                await connection.OpenAsync();
                await action(connection);
            }
        }

        public static async Task<TResult> UseDbConnectionAsync<TResult>(Func<MySqlConnection, Task<TResult>> action) {
            using (var connection = CreateMySqlConnection()) {
                await connection.OpenAsync();
                return  await action(connection);
            }
        }

        private static MySqlConnection CreateMySqlConnection() {
            return new MySqlConnection(MySqlConnectionString);
        }

    }
}
