using Dapper;
using Project.Core.Contract;
using Project.Core.Contract.Repository;
using Project.Model.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Implementation
{

    public class UserRepository: IUserRepository
    {

        private readonly  ISqlConnectionFactory _connectionFactory;

        public UserRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User> GetUser(string userName,string Password,CancellationToken cancellationToken)
        {
            await using SqlConnection connection = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@userName", userName);
            parameters.Add("@password", Password);

            var result = await connection.QueryFirstOrDefaultAsync<User>("dbo.uspGetUser", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }
        public async Task<IEnumerable<User>> GetAllUser(CancellationToken cancellationToken)
        {
            await using SqlConnection connection = _connectionFactory.CreateConnection();

            var result = await connection.QueryAsync<User>("dbo.uspGetUser", commandType: CommandType.StoredProcedure);
            return result;
        }

    }
}
